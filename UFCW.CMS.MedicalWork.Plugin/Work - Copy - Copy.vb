﻿Option Infer On
Option Strict On

Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Configuration
Imports System.Data.Common
Imports System.Data.DataTableExtensions
Imports System.Reflection
Imports System.Text
Imports System.Threading
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports UFCW.WCF
Imports System.Text.RegularExpressions
Imports SharedInterfaces
Imports System.IO

<PlugIn("Medical", "Queue")> Public Class Work
    Implements SharedInterfaces.IMessage

#Region "Private form variables and structs"

    Public Const METHODS_TO_SKIP As Integer = 2

    Private Shared _TraceBinding As New TraceSwitch("TraceBinding", "Trace Switch in App.Config", "0")
    Private Shared _TraceCloning As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config", "0")
    Private Shared _TraceMessaging As New BooleanSwitch("TraceMessaging", "Trace Switch in App.Config", "0")
    Private Shared _TraceParallel As New TraceSwitch("TraceParallel", "Parallel Trace Switch in App.Config", "0")

    Private _APPKEY As String = "UFCW\Claims\"

    ReadOnly _SHOWLOADTIME As Boolean = CBool(CType(ConfigurationManager.GetSection("ShowLoadTime"), IDictionary)("PayScreenOn"))
    ReadOnly _DomainUser As String = SystemInformation.UserName

    Private Structure PatientKey
        Dim FamilyID As Integer
        Dim ParticipantSSN As Integer
        Dim RelationID As Short?
        Dim PatientSSN As Integer
        Dim PatientFName As Object
        Dim PatientLName As Object
        Dim PatientDOB As Object
        Dim SecuritySW As Boolean

        Sub Empty()
            FamilyID = Nothing
            ParticipantSSN = Nothing
            RelationID = Nothing
            PatientSSN = Nothing
            PatientFName = Nothing
            PatientLName = Nothing
            PatientDOB = Nothing
            SecuritySW = Nothing
        End Sub
    End Structure

    Private _ProcedureValuesDT As DataTable
    Private _DiagnosisValuesDT As DataTable

    Private _DuplicatesClaimDS As DataSet
    Private _AssociatedClaimDS As DataSet

    Private WithEvents _ClaimDS As New ClaimDataset

    Private WithEvents _ClaimMasterBS As BindingSource
    Private WithEvents _MedHdrBS As BindingSource
    Private WithEvents _MedDtlBS As BindingSource
    Private _ClaimDr As DataRow
    Private _MedHdrDr As DataRow
    Private _MedDtlDR As DataRow

    Private WithEvents _AssOrigDtlLineBS As BindingSource
    Private WithEvents _AssDetailLineBS As BindingSource
    Private WithEvents _AssResultsBS As BindingSource

    Private WithEvents _DupDtlLineBS As BindingSource
    Private WithEvents _RouteDgBS As BindingSource

    Private _PatientAddressesDS As DataSet
    Private _ProviderAndNPIDS As DataSet

    'Private _AlertManagerDataTable As DataTable
    Private _AccumulatorsDT As DataTable
    Private _DetailAccumulatorsDT As DataTable
    Private _OriginalAccumulatorsDT As DataTable
    Private _ClaimAccumulatorsDT As DataTable
    Private _Transaction As DbTransaction
    Private _PlanDT As DataTable
    Private _PlanBS As BindingSource
    Private _FamilyFreeTextDV As DataView
    Private _PatientFreeTextDV As DataView

    Private _CustomerServiceFI As FileInfo

    Private _ClaimMemberAccumulatorManager As MemberAccumulatorManager
    Private WithEvents _ClaimAlertManager As AlertManagerCollection

    Private _ClaimBinder As MedicalBinder

    Private _RuleSetType As Integer? = PlanController.GetRulesetTypeID("General")

    Private _WorkSharedInterfacesMessage As SharedInterfaces.IMessage
    Private _ClaimID As Integer
    Private _OrigFont As Font
    Private _NormalHeight As Integer = 0
    Private _Mode As String
    Private _LoadingClaim As Boolean
    Private _PlansIncludingPreventativeRules As String() = ConfigurationManager.AppSettings("PlansIncludingPreventativeRules").Split(CChar(","))
    Private _AutoMergeSummaryLines As Boolean = CBool(ConfigurationManager.AppSettings("AutoMergeSummaryLines"))

    Private _PerformingCorrelation As Boolean = False
    Private _AlertFloat As Boolean = False
    Private _PatientKey As New PatientKey
    Private _DetailRowChanging As Boolean = False
    Private _LastAlertIndex As Integer = -1
    Private _Activating As Boolean = False
    Private _OrigGridContextMenu As ContextMenu
    Private _RightClickCol As Integer?
    Private _ChangingRows As Boolean = False
    Private _HighlightPlan As Boolean = False
    Private _LastPPOCProv As Integer?
    Private _DupRightClick As Boolean = False
    Private _DocTypeChanging As Boolean = False
    Private _ShowCancelOnClose As Boolean = True
    Private _ForceClose As Boolean = False
    Private _BizUserPrincipal As System.Security.Principal.WindowsPrincipal
    Private _OrigPartSSN As Integer?
    Private _OrigPatSSN As Integer?
    Private _DupTabPage As Boolean = False
    Private _AssTabPage As Boolean = False
    Private _UTLArchive As Boolean = False
    Private _DetailGroupBoxSelected As Boolean = False
    Private _AuditForm As Audit
    Private _RollbackAccumulators As Boolean = False
    Private _ViewedEligibility As Boolean = False
    Private _ViewedFreeText As Boolean = False
    Private _ViewedCOB As Boolean = False
    Private _ViewedAssociated As Boolean = False
    Private _ViewedDuplicates As Boolean = False
    Private _ViewedClaimHistory As Boolean = False
    Private _IsShown As Boolean = False

    Private WithEvents _FileNetDisplay As Display

    Private WithEvents _ExDup As ExecuteDuplicates
    Private WithEvents _ExAssociated As BackThread

    Private _DuplicateThread As Thread
    Private _AssociatedThread As Thread
    Private _ProcedureCodesWorkerThread As Thread
    Private _DiagnosisCodesWorkerThread As Thread

    Private _WorkerThread As Thread
    Private _WorkerThread2 As Thread

    Private _Cancel As Boolean = False
    Private _HighestEntryID As Integer
    Private _HasBeenAudited As Boolean 'this means that the claim actually has audit information associated with it.

    Private _TextCol As DataGridHighlightTextBoxColumn
    Private _ButtCol As DataGridHighlightButtonColumn
    Private _BoolCol As DataGridHighlightBoolColumn
    Private _ColTextBox As DataGridTextBox
    Private _IconCol As DataGridHighlightIconColumn
    Private _HoveringOverCell As New DataGridCell
    Private _EditableTextBox As Boolean

    Private _Delegate As [Delegate]
    Private _CurrencyManager As CurrencyManager

    Private _RefreshCount As Integer
    Private _ManualAccumulatorsForm As New ManualAccumulators

    Private _RePriceToolBarButtonEnabledState As Boolean
    Private _RePriceMenuItemEnabledState As Boolean
    Private _RePriceReturnMenuItemEnabledState As Boolean
    Private _RePriceReturnToolBarButtonEnabledState As Boolean
    Private _DropdownRepriceReturnToolBarButtonEnabledState As Boolean


#End Region

    Public Sub New(ByVal sharedInterfaceMessage As SharedInterfaces.IMessage, ByVal claimID As Integer, ByVal mode As String, Optional ByVal transaction As DbTransaction = Nothing)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)

        'Add any initialization after the InitializeComponent() call
        _WorkSharedInterfacesMessage = sharedInterfaceMessage
        _ClaimID = claimID
        _Mode = mode
        _Transaction = transaction
    End Sub

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            '

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

#Region "Public Properties"

    ' -----------------------------------------------------------------------------
    Public Property UniqueID As String

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' this property gets or sets the ability to cancel the close of the form
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	12/19/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property ShowCancelOnClose() As Boolean
        Get
            Return _ShowCancelOnClose
        End Get
        Set(ByVal value As Boolean)
            _ShowCancelOnClose = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' this property gets or sets the ability to cancel the close of the form
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	12/19/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property ForceClose() As Boolean
        Get
            Return _ForceClose
        End Get
        Set(ByVal value As Boolean)
            _ForceClose = value
        End Set
    End Property

    <System.ComponentModel.Description("Represents the application key used when accessing the registry.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property
#End Region

#Region "Plug In Code"

    Private Sub SharedInterfacesMessage(statusMessage As String)

        If _TraceMessaging.Enabled Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & " : " & statusMessage & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceMessaging" & vbTab)

        _WorkSharedInterfacesMessage.StatusMessage(statusMessage)

        'Task.Factory.StartNew(Sub() _WorkSharedInterfacesMessage.StatusMessage(statusMessage))

    End Sub
    Private Sub WorkStatusMessage(ByVal statusMessage As String) Implements SharedInterfaces.IMessage.StatusMessage
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' plugins inteface this sub to update status messages to display on this form
        ' </summary>
        ' <param name="msg"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        'required by interface

        SharedInterfacesMessage(statusMessage)

    End Sub
#End Region

#Region "Form Events"
    Private Sub Work_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed

        If _TraceMessaging.Enabled Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceMessaging" & vbTab)

    End Sub

    Private Sub Work_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' performs main initialization of form
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' Lalitha Moparthi Updated 12/09/2022
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim AuditDT As DataTable

        Try

            _LoadingClaim = True
            'there is a bug where the designer unsets this property after load or build
            'it is being reset here as a temporary fix
            DocumentHistoryViewer.ShowClose = False

            DropdownHoldRefreshToolBarButton.Tag = HoldContextMenuItem
            DropdownRepriceReturnToolBarButton.Tag = RepriceContextMenuItem

            _OrigFont = Me.Font

            SetSettings()

            _NormalHeight = AlertsImageListBox.Height

            'Init Active Directory
            AppDomain.CurrentDomain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.WindowsPrincipal)
            _BizUserPrincipal = CType(System.Threading.Thread.CurrentPrincipal, System.Security.Principal.WindowsPrincipal)

            ResultViewByClaimIDMenuItem.Visible = False
            If ConfigurationManager.GetSection("CustomerService") IsNot Nothing Then
                _CustomerServiceFI = New FileInfo(CStr(CType(ConfigurationManager.GetSection("CustomerService"), IDictionary)("EXEName")))

                If Not _CustomerServiceFI.Exists Then
                    _CustomerServiceFI = New FileInfo(Environment.CurrentDirectory & "\" & "UFCW.CMS.CustomerService.exe")
                End If

                If _CustomerServiceFI.Exists Then
                    ResultViewByClaimIDMenuItem.Visible = True
                End If
            End If

            'undo any accumulators
            If _Mode.ToUpper <> "AUDIT" Then
                Me.Text = "Process Claim - " & Format(_ClaimID, "00000000")
                _RollbackAccumulators = True
            Else
                Me.Text = "Audit Claim - " & Format(_ClaimID, "00000000")
            End If

            LoadDiagnosisValues()
            LoadCOBCombo()
            LoadPPOCombo()
            LoadPayeeCombo()
            LoadPlanCombo()
            LoadDocTypes()
            LoadProceduresValues()

            'inserted by pw to accomodate 475
            AuditDT = CMSDALFDBMD.RetrieveAuditInformation(_ClaimID) 'todo place on background thread
            _HasBeenAudited = AuditDT.Rows.Count > 0

            'avoid processing caused by Member Plan being established
            ' RemoveHandler cmbPlan.SelectedIndexChanged, AddressOf cmbPlan_SelectedIndexChanged

            _WorkSharedInterfacesMessage.StatusMessage("Loading Claim. Please Wait...")

            LoadClaim()

            '   AddHandler cmbPlan.SelectedIndexChanged, AddressOf cmbPlan_SelectedIndexChanged

            AddHandler AutoCheckBox.CheckedChanged, AddressOf CheckBoxs_CheckedChanged
            AddHandler WorkersCompCheckBox.CheckedChanged, AddressOf CheckBoxs_CheckedChanged
            AddHandler OtherCheckBox.CheckedChanged, AddressOf CheckBoxs_CheckedChanged
            AddHandler AuthCheckBox.CheckedChanged, AddressOf CheckBoxs_CheckedChanged
            AddHandler ChiroCheckBox.CheckedChanged, AddressOf CheckBoxs_CheckedChanged
            AddHandler OICheckBox.CheckedChanged, AddressOf CheckBoxs_CheckedChanged

            'merge is NOT available for JAA Claims
            If _MedDtlDR IsNot Nothing AndAlso CStr(_ClaimDr("DOC_TYPE")).ToUpper.Contains("HOSPITAL") AndAlso _Mode.ToUpper <> "AUDIT" AndAlso Not _MedHdrDr("PRICED_BY").ToString.Contains("JAA") Then
                ConsolidateButton.Visible = True
                If DetailLinesDataGrid.VisibleRowCount > 0 Then
                    If _MedDtlDR("STATUS").ToString = "MERGED" Then
                        _MedDtlBS.MoveLast()
                    Else
                        _MedDtlBS.MoveFirst()
                    End If
                End If
            End If
            If _MedDtlBS IsNot Nothing Then
                DirectCast(_MedDtlBS.DataSource, DataTable).AcceptChanges()
                _MedDtlBS.EndEdit()
                _MedDtlBS.ResetBindings(False)
            End If

        Catch ex As Exception
            Throw
        Finally
            _LoadingClaim = False
            AuditDT = Nothing
        End Try
    End Sub
    Public Sub RefreshLettersHistoryHandler()
        LettersHistoryControl.RefreshLettersHistory()
    End Sub

    Public Sub CancelLetterRequestHandler()
        LettersControl.Status.Text = "Letter print request cancelled id/maxid( " & CStr(LettersHistoryControl.LetterID) & " / " & LettersHistoryControl.MaxID & ")"
    End Sub

    Public Sub ReprintRequestHandler()
        LettersControl.Status.Text = "Letter scheduled for re-print id/maxid( " & CStr(LettersHistoryControl.LetterID) & " / " & LettersHistoryControl.MaxID & ")"
    End Sub

    Private Sub HideControlsForUTL()

        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Hides all appropriate controls for UTL doc type claims
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	6/13/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            Me.LettersTabPage.Enabled = False
            Me.DuplicatesTabPage.Enabled = False
            Me.FreeTextTabPage.Enabled = False
            Me.AccumulatorsTabPage.Enabled = False
            Me.AccidentGroupBox.Enabled = False
            Me.DetailLineGroupBox.Enabled = False
            Me.DetailLinesDataGrid.Enabled = False
            Me.cmbCOB.Enabled = False
            Me.cmbPricingNetwork.Enabled = False
            Me.CompleteToolBarButton.Enabled = False
            Me.RePriceReturnToolBarButton.Enabled = False
            Me.RePriceToolBarButton.Enabled = False
            Me.DropdownRepriceReturnToolBarButton.Enabled = False
            Me.ReCalcToolBarButton.Enabled = False
            Me.AnnotateButton.Enabled = False
            Me.CompleteMenuItem.Enabled = False
            Me.RePriceMenuItem.Enabled = False
            Me.RePriceReturnMenuItem.Enabled = False
            Me.RefreshMenuItem.Enabled = False
            Me.HistoryTabPage.Enabled = False
            Me.AddButton.Enabled = False
            Me.DeleteButton.Enabled = False
            Me.ConsolidateButton.Enabled = False

            Me.Tabs.TabPages.Remove(LettersTabPage)
            Me.Tabs.TabPages.Remove(DuplicatesTabPage)
            Me.Tabs.TabPages.Remove(FreeTextTabPage)
            Me.Tabs.TabPages.Remove(AccumulatorsTabPage)
            Me.Tabs.TabPages.Remove(HistoryTabPage)

            Me.AuthCheckBox.Enabled = False
            Me.ChiroCheckBox.Enabled = False
            Me.PatientHistoryButton.Enabled = False
            Me.cmbHosp.Enabled = False
            Me.cmbPayee.Enabled = False
            Me.NonPARCheckBox.Enabled = False
            Me.OOACheckBox.Enabled = False
            Me.txtTotalOIAmt.Enabled = False
            Me.txtTotalPaidAmt.Enabled = False
            Me.txtTotalChargedAmt.Enabled = False
            Me.txtTotalAllowedAmt.Enabled = False

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub SelectAccidentUI(ByVal claimAccumulatorManager As MemberAccumulatorManager, ByVal claimDS As DataSet, ByVal meddtlCurrentRowsDT As DataTable, ByRef accumIDForAccident As Integer?, ByRef accidentAccumName As String, ByRef accidentClaimID As Integer?, ByRef accidentDate As Date?)

        Dim AccidentAccumulatorSelectorForm As AccidentAccumulatorSelector

        Try

            'check for 90 days
            AccidentAccumulatorSelectorForm = New AccidentAccumulatorSelector(CShort(_ClaimDr("RELATION_ID")), CInt(_ClaimDr("FAMILY_ID")), CInt(_MedHdrDr("CLAIM_ID")), CDate(_MedHdrDr("INCIDENT_DATE")), claimAccumulatorManager)

            If Not AccidentAccumulatorSelectorForm.SuppressDisplay Then
                AccidentAccumulatorSelectorForm.ShowDialog()
            End If

            If AccidentAccumulatorSelectorForm.SelectedAccumulatorID Is Nothing Then
                Throw New Exception("ACCUMULATOR NOT SELECTED")
            Else
                accumIDForAccident = AccidentAccumulatorSelectorForm.SelectedAccumulatorID
                accidentAccumName = AccidentAccumulatorSelectorForm.SelectedName
                accidentClaimID = AccidentAccumulatorSelectorForm.SelectedClaimID
                accidentDate = AccidentAccumulatorSelectorForm.SelectedDate
            End If

        Catch ex As Exception
            Throw
        Finally
            If AccidentAccumulatorSelectorForm IsNot Nothing Then AccidentAccumulatorSelectorForm.Dispose()
        End Try

    End Sub

    Private Sub AccumulatorCheckIfOverrideNeededUI(ByVal claimAccumulatorManager As MemberAccumulatorManager, ByVal claimDS As DataSet, ByRef claimBinder As MedicalBinder)

        If claimBinder Is Nothing Then Exit Sub
        If claimBinder.BinderAccumulatorManager Is Nothing Then Return

        Dim Val As Integer
        Dim AccumulatorOverrideForm As AccumulatorOverride

        Try

            Val = CInt(claimAccumulatorManager.GetOriginalLifetimeValue(CInt(AccumulatorController.GetAccumulatorID("FIXAC"))))

            If Val <= 0 Then Return

            If _ClaimDr Is Nothing Then Exit Sub

            AccumulatorOverrideForm = New AccumulatorOverride(claimBinder.BinderAccumulatorManager)

            If Val = 1 Then
                AccumulatorOverrideForm.UseFamilyRelationIDs = True
                AccumulatorOverrideForm.FamilyID = If(IsDBNull(_ClaimDr("FAMILY_ID")), 0, CInt(_ClaimDr("FAMILY_ID"))) 'CInt(_ClaimDr("FAMILY_ID"))
                AccumulatorOverrideForm.RelationID = UFCWGeneral.IsNullShortHandler(_ClaimDr("RELATION_ID"))   ' CShort(_ClaimDr("RELATION_ID"))
                AccumulatorOverrideForm.ParticipantSSN = UFCWGeneral.IsNullStringHandler(_ClaimDr("PART_SSN"))
                AccumulatorOverrideForm.DisableInputs()
                AccumulatorOverrideForm.PatientNameTextBox.Text = UFCWGeneral.IsNullStringHandler(_ClaimDr("PAT_LNAME"), "") & ", " & UFCWGeneral.IsNullStringHandler(_ClaimDr("PAT_FNAME"), "")

                AccumulatorOverrideForm.DateOfBirthTextBox.DataBindings.Clear()
                Dim DobBind As Binding = New Binding("Text", _MedHdrBS, "PAT_DOB", True, DataSourceUpdateMode.OnPropertyChanged)
                DobBind.FormatString = "MM-dd-yyyy"
                AccumulatorOverrideForm.DateOfBirthTextBox.DataBindings.Add(DobBind)


                AccumulatorOverrideForm.MemberAccumManager = claimBinder.BinderAccumulatorManager
                AccumulatorOverrideForm.Search()

                MessageBox.Show("Please make sure the patient's accumulators are correct", "Accumulator Verification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                AccumulatorOverrideForm.ShowDialog()

                claimBinder.BinderAccumulatorManager = AccumulatorOverrideForm.MemberAccumManager

            End If

            DisableEnableButtons()

        Catch ex As Exception
            Throw
        Finally

            If AccumulatorOverrideForm IsNot Nothing Then AccumulatorOverrideForm.Dispose()
            AccumulatorOverrideForm = Nothing
        End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub Work_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        If _Activating Then Return
        If Me.IDMCorrelationTimer Is Nothing Then Return

        Dim CurDoc As Long?

        Try
            _Activating = True

            Me.IDMCorrelationTimer.Enabled = False

            Me.WindowState = FormWindowState.Normal

            If Me.Tabs IsNot Nothing AndAlso Me.Tabs.SelectedTab IsNot DuplicatesTabPage Then


                CurDoc = Display.CurrentDocumentID

                If CurDoc Is Nothing OrElse IsDBNull(_ClaimDr("DOCID")) Then
                    'ImageWarning.Visible = False
                Else
                    If CurDoc IsNot Nothing AndAlso CurDoc.ToString <> _ClaimDr("DOCID").ToString Then
                        ImageWarning.Visible = True
                    Else
                        ImageWarning.Visible = False
                    End If
                End If

                Me.Refresh()
            End If

            _Activating = False

        Catch ex As Exception
            Throw
        End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub IDMCorrelationTimer_Tick(sender As Object, e As EventArgs) Handles IDMCorrelationTimer.Tick

        ' -----------------------------------------------------------------------------
        ' <summary>
        ' determines if the user is navigating the Image Viewer and displays a warning
        ' label if the image doesn't match this work screen
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	4/28/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        If _PerformingCorrelation Then Return

        Dim CurDoc As Long?

        Try
            _PerformingCorrelation = True

            If _ClaimDr IsNot Nothing Then
                If IsDBNull(_ClaimDr("DOCID")) = False Then

                    CurDoc = Display.CurrentDocumentID

                    If CurDoc IsNot Nothing AndAlso _ClaimDr("DOCID") IsNot Nothing AndAlso CurDoc.ToString <> _ClaimDr("DOCID").ToString Then
                        If ImageWarning IsNot Nothing Then ImageWarning.Visible = True
                    Else
                        If ImageWarning IsNot Nothing Then ImageWarning.Visible = False
                    End If
                End If
            Else
                If IDMCorrelationTimer IsNot Nothing Then IDMCorrelationTimer.Enabled = False
            End If

        Catch IgnoreException As ApplicationException When IgnoreException.Message.Contains("Terminate")
            'ignore error
        Catch ex As Exception
            Throw
        Finally
            _PerformingCorrelation = False
        End Try

    End Sub

    'Private Sub IDMCorrelation_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles IDMCorrelationTimer.Elapsed
    '    ' -----------------------------------------------------------------------------
    '    ' <summary>
    '    ' determines if the user is navigating the Image Viewer and displays a warning
    '    ' label if the image doesn't match this work screen
    '    ' </summary>
    '    ' <param name="sender"></param>
    '    ' <param name="e"></param>
    '    ' <remarks>
    '    ' </remarks>
    '    ' <history>
    '    ' 	[Nick Snyder]	4/28/2006	Created
    '    ' </history>
    '    ' -----------------------------------------------------------------------------

    '    If _PerformingCorrelation = True Then Return

    '    Try
    '        _PerformingCorrelation = True

    '        If Not _ClaimMasterDataRow Is Nothing Then
    '            If IsDBNull(_ClaimDr("DOCID")) = False Then
    '                Dim CurDoc As Long?

    '                Using FNDisplay As New Display
    '                    CurDoc = FNDisplay.CurrentDocumentID
    '                End Using

    '                If Not CurDoc Is Nothing AndAlso Not _ClaimDr("DOCID") Is Nothing AndAlso CurDoc.ToString <> _ClaimDr("DOCID").ToString Then
    '                    If Not ImageWarning Is Nothing Then ImageWarning.Visible = True
    '                Else
    '                    If Not ImageWarning Is Nothing Then ImageWarning.Visible = False
    '                End If
    '            End If
    '        Else
    '            If Not IDMCorrelationTimer Is Nothing Then IDMCorrelationTimer.Enabled = False
    '        End If

    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (rethrow) Then
    '            Throw
    '        Else
    '            MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End If
    '    Finally
    '        _PerformingCorrelation = False
    '    End Try

    'End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub Work_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Deactivate
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Starts the IDM timer to monitor what the user is doing on the Image screen
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	4/28/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        If IDMCorrelationTimer IsNot Nothing Then IDMCorrelationTimer.Enabled = True
    End Sub

    Private Sub Work_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim Transaction As DbTransaction

        Try
            If Not _ForceClose Then

                'FinalizeEdits()

                If HasChanges() Then
                    Select Case MessageBox.Show("Changes have been made." & vbCrLf & "Would you like to save the changes?", "Save Changes", If(_ShowCancelOnClose, MessageBoxButtons.YesNoCancel, MessageBoxButtons.YesNo), MessageBoxIcon.Question)
                        Case DialogResult.Yes

                            'Cancel if info not valid
                            If Not HeaderInfoOk() Then
                                e.Cancel = True
                                Exit Select
                            End If

                            Transaction = CMSDALCommon.BeginTransaction

                            If SaveChanges(Transaction) Then
                                CMSDALCommon.CommitTransaction(Transaction)
                            Else
                                CMSDALCommon.RollbackTransaction(Transaction)
                            End If

                        Case DialogResult.Cancel
                            e.Cancel = True
                    End Select
                End If
            End If

        Catch ex As Exception
            If Transaction IsNot Nothing Then
                CMSDALCommon.RollbackTransaction(Transaction)
            End If
        Finally
            SaveSettings()
            If Not e.Cancel Then
                Cleanup()
            End If
        End Try

    End Sub

    Private Sub Cleanup()

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        If _Transaction IsNot Nothing Then _Transaction.Dispose()

        Try

            If _ClaimID > 0 Then CMSDALFDBMD.UnBusyItem(_ClaimID, _DomainUser.ToUpper, Nothing)
            If _ClaimDr IsNot Nothing AndAlso Not IsDBNull(_ClaimDr("FAMILY_ID")) Then CMSDALFDBMD.ReleaseFamilyLock(CInt(_ClaimDr("FAMILY_ID")))

        Catch IgnoreException As Exception
        End Try

        _MedDtlBS = Nothing
        _MedHdrBS = Nothing
        _ClaimMasterBS = Nothing

        Me.AlertsImageListBox.Items.Clear()
        Me.AlertsImageListBox.DataBindings.Clear()
        Me.AlertsImageListBox.DataSource = Nothing
        Me.AlertsImageListBox.Dispose()
        Me.AlertsImageListBox = Nothing

        Me.IDMCorrelationTimer.Enabled = False
        Me.IDMCorrelationTimer.Stop()
        Me.IDMCorrelationTimer.Dispose()
        Me.IDMCorrelationTimer = Nothing

        If _IconCol IsNot Nothing Then RemoveHandler _IconCol.PaintCellPicture, AddressOf DetermineCellIcon
        If _TextCol IsNot Nothing Then RemoveHandler _TextCol.OnPaint, AddressOf PlanHighlight
        If _ButtCol IsNot Nothing AndAlso _ButtCol.ColumnButton IsNot Nothing Then
            RemoveHandler _ButtCol.ColumnButton.Click, CType(_Delegate, Global.System.EventHandler)
            RemoveHandler _ButtCol.Formatting, AddressOf DetailLinesDataGrid_FormattingReason
            RemoveHandler _ButtCol.UnFormatting, AddressOf UnFormattingReason
            RemoveHandler _ButtCol.Formatting, AddressOf DetailLinesDataGrid_FormattingAccumulators
            RemoveHandler _ButtCol.Formatting, AddressOf UnFormattingAccumulators
        End If

        If _AuditForm IsNot Nothing Then _AuditForm.Dispose()

        Me.WorkFreeTextEditor.ClearFreeText()

        Me.txtFamilyID.DataBindings.Clear()
        Me.txtPartSSN.DataBindings.Clear()
        Me.txtPartNameFirst.DataBindings.Clear()
        Me.txtPartNameMiddle.DataBindings.Clear()
        Me.txtPartNameLast.DataBindings.Clear()
        Me.txtPatRelationID.DataBindings.Clear()
        Me.txtPatSSN.DataBindings.Clear()
        Me.txtPatNameFirst.DataBindings.Clear()
        Me.txtPatNameMiddle.DataBindings.Clear()
        Me.txtPatNameLast.DataBindings.Clear()
        Me.txtReceivedDate.DataBindings.Clear()
        Me.DocTypeComboBox.DataBindings.Clear()
        Me.DuplicateCheckBox.DataBindings.Clear()
        Me.txtReceivedDate.DataBindings.Clear()
        Me.txtOpenDate.DataBindings.Clear()
        Me.txtClaimID.DataBindings.Clear()
        Me.txtDocID.DataBindings.Clear()
        Me.txtMaxID.DataBindings.Clear()
        Me.txtPageCount.DataBindings.Clear()
        Me.txtBatchNum.DataBindings.Clear()
        Me.txtPriority.DataBindings.Clear()
        Me.txtReferenceClaim.DataBindings.Clear()
        Me.txtPatAcctNo.DataBindings.Clear()
        Me.txtIncidentDate.DataBindings.Clear()
        Me.AutoCheckBox.DataBindings.Clear()
        Me.WorkersCompCheckBox.DataBindings.Clear()
        Me.OtherCheckBox.DataBindings.Clear()
        Me.NonPARCheckBox.DataBindings.Clear()
        Me.OOACheckBox.DataBindings.Clear()
        Me.AuthCheckBox.DataBindings.Clear()
        Me.ChiroCheckBox.DataBindings.Clear()
        Me.OICheckBox.DataBindings.Clear()
        Me.cmbCOB.DataBindings.Clear()
        Me.cmbPricingNetwork.DataBindings.Clear()
        Me.cmbPayee.DataBindings.Clear()
        Me.txtPatBirthDate.DataBindings.Clear()
        Me.txtPatGender.DataBindings.Clear()
        Me.txtProviderID.DataBindings.Clear()
        Me.txtProviderLicenseNo.DataBindings.Clear()
        Me.txtBCCZIP.DataBindings.Clear()
        Me.txtProcedure.DataBindings.Clear()
        Me.cmbPlan.DataBindings.Clear()
        Me.txtDiagnoses.DataBindings.Clear()
        Me.txtModifiers.DataBindings.Clear()
        Me.txtPlaceOfService.DataBindings.Clear()
        Me.txtBillType.DataBindings.Clear()
        Me.txtFromDate.DataBindings.Clear()
        Me.txtToDate.DataBindings.Clear()
        Me.txtChargeAmt.DataBindings.Clear()
        Me.txtPricedAmt.DataBindings.Clear()
        Me.txtPaidAmt.DataBindings.Clear()
        Me.txtOIAmt.DataBindings.Clear()
        Me.txtUnits.DataBindings.Clear()
        Me.txtNDC.DataBindings.Clear()
        Me.cmbStatus.DataBindings.Clear()

        Me.DuplicateDetailLinesDataGrid.TableStyles.Clear()
        Me.DuplicateDetailLinesDataGrid.DataSource = Nothing
        Me.cmbPlan.DataSource = Nothing
        Me.cmbCOB.DataSource = Nothing
        Me.cmbPricingNetwork.DataSource = Nothing
        Me.cmbPayee.DataSource = Nothing
        Me.DocTypeComboBox.DataSource = Nothing
        Me.RouteDataGrid.DataSource = Nothing

        If _DuplicatesClaimDS IsNot Nothing Then _DuplicatesClaimDS.Dispose()
        _DuplicatesClaimDS = Nothing

        _ClaimBinder = Nothing

        If _ClaimMemberAccumulatorManager IsNot Nothing Then _ClaimMemberAccumulatorManager.AccumulatorSummaries.Clear()
        _ClaimMemberAccumulatorManager = Nothing

        If _AccumulatorsDT IsNot Nothing Then _AccumulatorsDT.Dispose()
        _AccumulatorsDT = Nothing

        If _ClaimAccumulatorsDT IsNot Nothing Then _ClaimAccumulatorsDT.Dispose()
        _ClaimAccumulatorsDT = Nothing

        If _OriginalAccumulatorsDT IsNot Nothing Then _OriginalAccumulatorsDT.Dispose()
        _OriginalAccumulatorsDT = Nothing

        If _DetailAccumulatorsDT IsNot Nothing Then _DetailAccumulatorsDT.Dispose()
        _DetailAccumulatorsDT = Nothing

        If _PlanDT IsNot Nothing Then _PlanDT.Dispose()
        _PlanDT = Nothing

        If _FamilyFreeTextDV IsNot Nothing Then _FamilyFreeTextDV.Dispose()
        _FamilyFreeTextDV = Nothing

        If _PatientFreeTextDV IsNot Nothing Then _PatientFreeTextDV.Dispose()
        _PatientFreeTextDV = Nothing

        If _ClaimAlertManager IsNot Nothing Then _ClaimAlertManager.Clear()
        _ClaimAlertManager = Nothing

        If _OrigGridContextMenu IsNot Nothing Then _OrigGridContextMenu.Dispose()
        _OrigGridContextMenu = Nothing

        _ExAssociated = Nothing
        _ExDup = Nothing
        _FileNetDisplay = Nothing
        _WorkerThread = Nothing
        _WorkerThread2 = Nothing
        _DuplicateThread = Nothing
        _AssociatedThread = Nothing

        Me.DetailLinesDataGrid.TableStyles.Clear()
        Me.DetailLinesDataGrid.DataSource = Nothing
        Me.DetailLinesDataGrid.Dispose()
        Me.DetailLinesDataGrid = Nothing

        Me.AccumulatorsDataGrid.TableStyles.Clear()
        Me.AccumulatorsDataGrid.DataSource = Nothing
        Me.AccumulatorsDataGrid.Dispose()
        Me.AccumulatorsDataGrid = Nothing

        Me.RouteDataGrid.TableStyles.Clear()
        Me.RouteDataGrid.DataSource = Nothing
        Me.RouteDataGrid.Dispose()
        Me.RouteDataGrid = Nothing

        Me.DuplicateDetailLinesDataGrid.TableStyles.Clear()
        Me.DuplicateDetailLinesDataGrid.DataSource = Nothing
        Me.DuplicateDetailLinesDataGrid.Dispose()
        Me.DuplicateDetailLinesDataGrid = Nothing

        Me.EligControl.Dispose()
        Me.DocumentHistoryViewer.Dispose()
        Me.WorkFreeTextEditor.Dispose()
        Me.LettersControl.Dispose()

        Me.CobControl.Dispose()
        Me.CobControl = Nothing

        Me.EligControl = Nothing
        Me.DocumentHistoryViewer = Nothing
        Me.WorkFreeTextEditor = Nothing
        Me.LettersControl = Nothing

        Me.AnnotationControl.Dispose()
        Me.ProvProviderControl.Dispose()
        Me.PartParticipantControl.Dispose()
        Me.LettersHistoryControl.Dispose()

        If _AuditForm IsNot Nothing Then _AuditForm.Dispose()
        _AuditForm = Nothing

        'me.provide
        If _TextCol IsNot Nothing Then _TextCol.Dispose()
        _TextCol = Nothing

        If _ButtCol IsNot Nothing Then _ButtCol.Dispose()
        _ButtCol = Nothing

        If _BoolCol IsNot Nothing Then _BoolCol.Dispose()
        _BoolCol = Nothing

        If _CurrencyManager IsNot Nothing Then
            If _CurrencyManager.List IsNot Nothing Then
                _CurrencyManager.List.Clear()
            End If
        End If

        If _ClaimDS IsNot Nothing Then RemoveHandler _ClaimDS.MEDHDR.ColumnChanging, AddressOf MEDHDR_ColumnChanging
        If _ClaimDS IsNot Nothing Then RemoveHandler _ClaimDS.MEDHDR.RowChanging, AddressOf MEDHDR_RowChanging
        If _ClaimDS IsNot Nothing Then RemoveHandler _ClaimDS.MEDHDR.RowChanged, AddressOf MEDHDR_RowChanged
        If _ClaimDS IsNot Nothing Then RemoveHandler _ClaimDS.MEDDTL.ColumnChanging, AddressOf MEDDTL_ColumnChanging
        If _ClaimDS IsNot Nothing Then RemoveHandler _ClaimDS.MEDMOD.RowChanging, AddressOf MEDMOD_RowChanging
        If _ClaimDS IsNot Nothing Then RemoveHandler _ClaimDS.MEDMOD.RowDeleting, AddressOf MEDMOD_RowChanging

        If _ClaimDS.ANNOTATIONS IsNot Nothing Then _ClaimDS.ANNOTATIONS.Dispose()
        If _ClaimDS.CLAIM_MASTER IsNot Nothing Then _ClaimDS.CLAIM_MASTER.Dispose()
        If _ClaimDS.FREE_TEXT IsNot Nothing Then _ClaimDS.FREE_TEXT.Dispose()
        If _ClaimDS.MEDDIAG IsNot Nothing Then _ClaimDS.MEDDIAG.Dispose()
        If _ClaimDS.MEDDTL IsNot Nothing Then _ClaimDS.MEDDTL.Dispose()
        If _ClaimDS.MEDHDR IsNot Nothing Then _ClaimDS.MEDHDR.Dispose()
        If _ClaimDS.MEDMOD IsNot Nothing Then _ClaimDS.MEDMOD.Dispose()
        If _ClaimDS.REASON IsNot Nothing Then _ClaimDS.REASON.Dispose()
        If _ClaimDS.ROUTING_HISTORY IsNot Nothing Then _ClaimDS.ROUTING_HISTORY.Dispose()

        If _ClaimDS IsNot Nothing Then _ClaimDS.Dispose()
        _ClaimDS = Nothing

        Me.MainMenu.Dispose()

        If components IsNot Nothing Then components.Dispose()

        Me.Dispose()

    End Sub

    Private Sub SetSettings()
        Dim FSize As Single
        Dim FStyle As FontStyle
        Dim FUnit As GraphicsUnit
        Dim FCharset As Byte

        Try

            FStyle = New FontStyle
            FUnit = New GraphicsUnit

            Me.Visible = False

            Me.Top = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)))
            Me.Height = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString))
            Me.Left = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)))
            Me.Width = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString))
            Me.WindowState = CType(GetSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)

            ' -----------------------------------------------------------------------------
            ' <summary>
            ' Sets the basic form settings.  Windowstate, height, width, top, and left.
            ' </summary>
            ' <remarks>
            ' </remarks>
            ' <history>
            ' 	[Nick Snyder]	11/16/2005	Created
            ' </history>
            ' -----------------------------------------------------------------------------

            Dim FName As String = GetSetting(_APPKEY, "MedicalWork\Settings", "FontName", Me.Font.Name)
            FSize = CSng(GetSetting(_APPKEY, "MedicalWork\Settings", "FontSize", CStr(Me.Font.Size)))
            FStyle = CType(GetSetting(_APPKEY, "MedicalWork\Settings", "FontStyle", CStr(Me.Font.Style)), FontStyle)
            FUnit = CType(GetSetting(_APPKEY, "MedicalWork\Settings", "FontUnit", CStr(Me.Font.Unit)), GraphicsUnit)
            FCharset = CByte(GetSetting(_APPKEY, "MedicalWork\Settings", "FontCharset", CStr(Me.Font.GdiCharSet)))

            Me.Font = New Font(FName, FSize, FStyle, FUnit, FCharset)

            DupesHSplitter.SplitPosition = CInt(GetSetting(_APPKEY, "MedicalWork\Settings", "DupesHSplitterPos", CStr(DupesHSplitter.SplitPosition)))

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub EstablishDOSRange(ByRef claimMasterDR As DataRow, ByRef medHDRDR As DataRow)

        Dim FirstDateOfService As Date?
        Dim LastDateOfService As Date?
        Dim DateOfService As Date?

        Dim LowDV As DataView
        Dim HighDV As DataView
        Dim HighToDV As DataView

        Try

            LowDV = New DataView(_ClaimDS.MEDDTL, "OCC_FROM_DATE = MIN(OCC_FROM_DATE) And STATUS <> 'MERGED'", "", DataViewRowState.CurrentRows)
            HighDV = New DataView(_ClaimDS.MEDDTL, "OCC_FROM_DATE = MAX(OCC_FROM_DATE) And STATUS <> 'MERGED'", "", DataViewRowState.CurrentRows)
            HighToDV = New DataView(_ClaimDS.MEDDTL, "OCC_TO_DATE = MAX(OCC_TO_DATE) And STATUS <> 'MERGED'", "", DataViewRowState.CurrentRows)

            If LowDV.Count > 0 Then
                FirstDateOfService = UFCWGeneral.IsNullDateHandler(LowDV(0)("OCC_FROM_DATE")) ' CType(If(IsDBNull(LowDV(0)("OCC_FROM_DATE")), Nothing, LowDV(0)("OCC_FROM_DATE")), Date?)
            End If

            If HighDV.Count > 0 Then
                LastDateOfService = CType(If(IsDBNull(HighDV(0)("OCC_FROM_DATE")), FirstDateOfService, HighDV(0)("OCC_FROM_DATE")), Date?)
                If HighToDV.Count > 0 AndAlso CType(If(IsDBNull(HighToDV(0)("OCC_TO_DATE")), Nothing, CDate(HighToDV(0)("OCC_TO_DATE"))), Date?) > LastDateOfService Then
                    LastDateOfService = UFCWGeneral.IsNullDateHandler(HighToDV(0)("OCC_TO_DATE")) 'CType(If(IsDBNull(HighToDV(0)("OCC_TO_DATE")), Nothing, HighToDV(0)("OCC_TO_DATE")), Date?)
                End If

            Else
                If HighToDV.Count > 0 Then
                    LastDateOfService = UFCWGeneral.IsNullDateHandler(HighToDV(0)("OCC_TO_DATE")) 'CType(If(IsDBNull(HighToDV(0)("OCC_TO_DATE")), Nothing, HighToDV(0)("OCC_TO_DATE")), Date?)
                Else
                    LastDateOfService = FirstDateOfService
                End If
            End If

            DateOfService = FirstDateOfService
            medHDRDR("OCC_FROM_DATE") = UFCWGeneral.ToNullDateHandler(FirstDateOfService)
            medHDRDR("OCC_TO_DATE") = UFCWGeneral.ToNullDateHandler(LastDateOfService)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub SaveSettings()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Saves the basic form settings.  Windowstate, height, width, top, and left.
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	11/16/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim TheWindowState As FormWindowState

        Try
            TheWindowState = Me.WindowState

            SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(TheWindowState).ToString)

            Me.WindowState = FormWindowState.Normal
            SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
            SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
            SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
            SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
            Me.WindowState = TheWindowState

            SaveSetting(_APPKEY, Me.Name & "\Settings", "FontName", Me.Font.Name)
            SaveSetting(_APPKEY, Me.Name & "\Settings", "FontSize", CStr(Me.Font.Size))
            SaveSetting(_APPKEY, Me.Name & "\Settings", "FontStyle", CStr(Me.Font.Style))
            SaveSetting(_APPKEY, Me.Name & "\Settings", "FontUnit", CStr(Me.Font.Unit))
            SaveSetting(_APPKEY, Me.Name & "\Settings", "FontCharset", CStr(Me.Font.GdiCharSet))

            SaveSetting(_APPKEY, Me.Name & "\Settings", "DupesHSplitterPos", CStr(DupesHSplitter.SplitPosition))

        Catch ex As Exception
            Throw
        End Try

    End Sub

#End Region

#Region "Tooltip Mouse Events"

    Private Sub Grids_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DuplicateDetailLinesDataGrid.MouseMove, DuplicatesOriginalDetailLinesDataGrid.MouseMove, DetailLinesDataGrid.MouseMove, AssociatedResultsDataGrid.MouseMove, AssociatedOriginalDetailLinesDataGrid.MouseMove, AssociatedDetailLinesDataGrid.MouseMove

        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try

            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & _HoveringOverCell.ToString & " " & Me.WorkEnhancedToolTip.GetToolTip(CType(sender, DataGridCustom)).ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            HTI = CType(sender, DataGridCustom).HitTest(e.X, e.Y)

            If HTI.Type <> DataGrid.HitTestType.Cell Then
                _HoveringOverCell.RowNumber = -1
                _HoveringOverCell.ColumnNumber = -1

                WorkEnhancedToolTip.SetToolTip(CType(sender, DataGridCustom), "")

            ElseIf HTI.Type = DataGrid.HitTestType.Cell AndAlso (HTI.Row <> _HoveringOverCell.RowNumber OrElse HTI.Column <> _HoveringOverCell.ColumnNumber) Then
                ' Store the new hit Cell

                _HoveringOverCell.RowNumber = HTI.Row
                _HoveringOverCell.ColumnNumber = HTI.Column
            End If

            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & _HoveringOverCell.ToString & " " & Me.WorkEnhancedToolTip.GetToolTip(CType(sender, DataGridCustom)).ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub
    Private Sub Grids_MouseHover(sender As Object, e As EventArgs) Handles DuplicateDetailLinesDataGrid.MouseHover, DuplicatesOriginalDetailLinesDataGrid.MouseHover, DetailLinesDataGrid.MouseHover, AssociatedResultsDataGrid.MouseHover, AssociatedOriginalDetailLinesDataGrid.MouseHover, AssociatedDetailLinesDataGrid.MouseHover

        Dim ToolTipText As New StringBuilder
        Dim ToolTipDR As DataRow
        Dim CellValues As String()
        Try
            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & _HoveringOverCell.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If Not IsNothing(_HoveringOverCell) AndAlso _HoveringOverCell.ColumnNumber > -1 AndAlso _HoveringOverCell.RowNumber > -1 AndAlso _HoveringOverCell.RowNumber <= (CType(sender, DataGridCustom).GetGridRowCount) Then

                Select Case CType(sender, DataGridCustom).GetColumnMapping(_HoveringOverCell.ColumnNumber)
                    Case "BILL_TYPE"
                        CellValues = CType(sender, DataGridCustom).Item(_HoveringOverCell).ToString.Split(CChar(","))

                        For Each BillType As String In CellValues
                            If BillType.Length < 1 Then Continue For
                            ToolTipDR = CMSDALFDBMD.RetrieveBillTypeValuesInformation(BillType.Trim)
                            If ToolTipDR IsNot Nothing Then
                                If ToolTipText.ToString.Trim.Length > 0 Then ToolTipText.AppendLine()
                                ToolTipText.Append(ToolTipDR.Item("FULL_DESC").ToString)
                            End If
                        Next

                    Case "REASONS", "REASON_SW"
                        CellValues = CType(sender, DataGridCustom).Item(_HoveringOverCell).ToString.Split(New Char() {CChar(","), CChar(" ")})

                        For Each Reason As String In CellValues
                            If Reason.Length < 1 Then Continue For

                            ToolTipDR = CMSDALFDBMD.RetrieveReasonValuesInformation(Reason.Trim, UFCWGeneral.IsNullDateHandler(CType(sender, DataGridCustom).GetTableRowFromGridPosition(_HoveringOverCell.RowNumber)("OCC_FROM_DATE")))
                            If ToolTipDR IsNot Nothing Then
                                If ToolTipText.ToString.Trim.Length > 0 Then ToolTipText.AppendLine()
                                ToolTipText.Append(ToolTipDR.Item("DESCRIPTION").ToString)
                            End If
                        Next

                    Case "DIAGNOSES", "DIAGNOSIS"
                        CellValues = CType(sender, DataGridCustom).Item(_HoveringOverCell).ToString.Split(New Char() {CChar(","), CChar(" ")})

                        For Each Diagnosis As String In CellValues
                            If Diagnosis.Length < 1 Then Continue For

                            ToolTipDR = CMSDALFDBMD.RetrieveDiagnosisValuesInformation(Diagnosis.Trim)
                            If ToolTipDR IsNot Nothing Then

                                If ToolTipText.ToString.Trim.Length > 0 Then ToolTipText.AppendLine()
                                ToolTipText.Append(ToolTipDR.Item("FULL_DESC").ToString)

                            End If
                        Next

                    Case "MODIFIER", "MODIFIERS"
                        CellValues = CType(sender, DataGridCustom).Item(_HoveringOverCell).ToString.Split(New Char() {CChar(","), CChar(" ")})

                        For Each Modifier As String In CellValues

                            If Modifier.Length < 1 Then Continue For

                            ToolTipDR = CMSDALFDBMD.RetrieveModifierValuesInformation(Modifier.Trim)
                            If ToolTipDR IsNot Nothing Then
                                If ToolTipText.ToString.Trim.Length > 0 Then ToolTipText.AppendLine()
                                ToolTipText.Append(ToolTipDR.Item("FULL_DESC").ToString)

                            End If
                        Next
                    Case "PROC_CODE"
                        CellValues = CType(sender, DataGridCustom).Item(_HoveringOverCell).ToString.Split(CChar(","))

                        For Each Procedure As String In CellValues
                            If Procedure.Length < 1 Then Continue For

                            ToolTipDR = CMSDALFDBMD.RetrieveProcedureValueInformation(Procedure.Trim)
                            If ToolTipDR IsNot Nothing Then
                                If ToolTipText.ToString.Trim.Length > 0 Then ToolTipText.AppendLine()
                                ToolTipText.Append(ToolTipDR.Item("SHORT_DESC").ToString)
                            End If
                        Next
                    Case "PLACE_OF_SERV"
                        CellValues = CType(sender, DataGridCustom).Item(_HoveringOverCell).ToString.Split(CChar(","))

                        For Each POS As String In CellValues

                            If POS.Length < 1 Then Continue For

                            ToolTipDR = CMSDALFDBMD.RetrievePlaceOfServiceValueInformation(POS.Trim)
                            If ToolTipDR IsNot Nothing Then
                                If ToolTipText.ToString.Trim.Length > 0 Then ToolTipText.AppendLine()
                                ToolTipText.Append(ToolTipDR.Item("FULL_DESC").ToString)
                            End If

                        Next

                    Case "NDC"
                        CellValues = CType(sender, DataGridCustom).Item(_HoveringOverCell).ToString.Split(CChar(","))
                        For Each NDC As String In CellValues
                            If NDC.Length < 1 Then Continue For
                            Dim NDCDesc As String = CMSDALFDBMD.RetrieveNDCDesc(NDC.Trim)
                            If NDCDesc IsNot Nothing Then
                                If ToolTipText.ToString.Trim.Length > 0 Then ToolTipText.AppendLine()
                                ToolTipText.Append(NDCDesc)
                            End If
                        Next
                End Select
            End If

            If ToolTipText IsNot Nothing AndAlso ToolTipText.ToString.Trim.Length > 0 Then
                If Me.WorkEnhancedToolTip Is Nothing OrElse String.Compare(Me.WorkEnhancedToolTip.GetToolTip(CType(sender, DataGridCustom)), ToolTipText.ToString) <> 0 OrElse Not Me.WorkEnhancedToolTip.Active Then
                    Me.WorkEnhancedToolTip.Active = True
                    Me.WorkEnhancedToolTip.SetToolTip(CType(sender, DataGridCustom), ToolTipText.ToString)
                End If
            End If

            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & _HoveringOverCell.ToString & " " & Me.WorkEnhancedToolTip.GetToolTip(CType(sender, DataGridCustom)).ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub cmbHosp_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbHosp.MouseHover
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' displays the description of the Code in a tooltip.
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim ToolTipText As New StringBuilder
        Dim CBox As ComboBox = CType(sender, ComboBox)

        Try

            If _Disposed OrElse _MedHdrBS Is Nothing OrElse _MedHdrBS.Position < 0 Then
                WorkEnhancedToolTip.SetToolTip(CBox, "")
                Return
            End If

            If CBox.SelectedIndex > -1 Then
                ToolTipText.Append(CBox.Text)
            End If

            If ToolTipText IsNot Nothing AndAlso ToolTipText.ToString.Trim.Length > 0 Then
                If Me.WorkEnhancedToolTip Is Nothing OrElse String.Compare(Me.WorkEnhancedToolTip.GetToolTip(CBox), ToolTipText.ToString) <> 0 OrElse Not WorkEnhancedToolTip.Active Then
                    WorkEnhancedToolTip.Active = True
                    WorkEnhancedToolTip.SetToolTip(CBox, ToolTipText.ToString)
                End If
            Else
                WorkEnhancedToolTip.SetToolTip(CBox, "")
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub


    Private Sub PlanComboBox_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPlan.MouseHover, cmbCOB.MouseHover, cmbPricingNetwork.MouseHover, cmbPayee.MouseHover
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' displays the description of the Code in a tooltip.
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim ToolTipText As New StringBuilder
        Dim DRs() As DataRow
        Dim Pos As Integer
        Dim CBox As ComboBox = CType(sender, ComboBox)

        Try

            Pos = CType(Me.BindingContext(CType(sender, ComboBox).DataSource), CurrencyManager).Position


            If _Disposed OrElse _MedHdrBS Is Nothing OrElse _MedHdrBS.Position < -1 OrElse Pos < 0 Then
                WorkEnhancedToolTip.SetToolTip(CBox, "")
                Return
            End If

            If CBox.SelectedIndex > -1 Then
                Select Case CBox.Name
                    Case "cmbCOB", "cmbPricingNetwork", "cmbPayee"
                        ToolTipText.Append(CType(CType(sender, ComboBox).DataSource, DataView)(Pos)("DESCRIPTION").ToString)
                        'ToolTipText.Append(CBox.SelectedValue.ToString)
                    Case "cmbPlan"
                        DRs = _PlanDT.Select("PLAN_TYPE = '" & CBox.Text.Trim & "'")
                        If DRs.Length > 0 Then
                            ToolTipText.Append(DRs(0)("PLAN_DESCRIPTION").ToString.Trim)
                        End If
                End Select
            End If

            If ToolTipText IsNot Nothing AndAlso ToolTipText.ToString.Trim.Length > 0 Then
                If Me.WorkEnhancedToolTip Is Nothing OrElse String.Compare(Me.WorkEnhancedToolTip.GetToolTip(CBox), ToolTipText.ToString) <> 0 OrElse WorkEnhancedToolTip.Active = False Then
                    WorkEnhancedToolTip.Active = True
                    WorkEnhancedToolTip.SetToolTip(CBox, ToolTipText.ToString)
                End If
            Else
                WorkEnhancedToolTip.SetToolTip(CBox, "")
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub
    Private Sub txtPOC_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtProcedure.MouseHover, txtPlaceOfService.MouseHover, txtNDC.MouseHover

        Dim ToolTipText As New StringBuilder
        Dim DGRow As DataRow
        Dim TBox As TextBox = CType(sender, TextBox)

        Try

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Position < 0 Then
                WorkEnhancedToolTip.SetToolTip(TBox, "")
                Return
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

            Select Case TBox.Name
                Case "txtProcedure"
                    ToolTipText.Append(DGRow("PROC_CODE_DESC").ToString.Trim)
                Case "txtPlaceOfService"
                    ToolTipText.Append(DGRow("PLACE_OF_SERV_DESC").ToString.Trim)
                Case "txtNDC"
                    ToolTipText.Append(DGRow("NDC_DESC").ToString.Trim)
                Case "txtProviderID"
                    ToolTipText.Append(_ClaimDr("PROV_NAME").ToString.Trim)
            End Select

            If ToolTipText IsNot Nothing AndAlso ToolTipText.ToString.Trim.Length > 0 Then
                If Me.WorkEnhancedToolTip Is Nothing OrElse String.Compare(Me.WorkEnhancedToolTip.GetToolTip(TBox), ToolTipText.ToString) <> 0 OrElse WorkEnhancedToolTip.Active = False Then
                    WorkEnhancedToolTip.Active = True
                    WorkEnhancedToolTip.SetToolTip(TBox, ToolTipText.ToString)
                End If
            Else
                WorkEnhancedToolTip.SetToolTip(TBox, "")
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub txt_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtProviderID.MouseHover

        Dim ToolTipText As New StringBuilder
        Dim DGRow As DataRow
        Dim TBox As TextBox = CType(sender, TextBox)

        Try

            If _Disposed OrElse _MedHdrBS Is Nothing OrElse _MedHdrBS.Position < 0 Then
                WorkEnhancedToolTip.SetToolTip(TBox, "")
                Return
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DGRow = DirectCast(_MedHdrBS.Current, DataRowView).Row

            Select Case TBox.Name
                Case "txtProviderID"
                    ToolTipText.Append(_ClaimDr("PROV_NAME").ToString.Trim)
            End Select

            If ToolTipText IsNot Nothing AndAlso ToolTipText.ToString.Trim.Length > 0 Then
                If Me.WorkEnhancedToolTip Is Nothing OrElse String.Compare(Me.WorkEnhancedToolTip.GetToolTip(TBox), ToolTipText.ToString) <> 0 OrElse WorkEnhancedToolTip.Active = False Then
                    WorkEnhancedToolTip.Active = True
                    WorkEnhancedToolTip.SetToolTip(TBox, ToolTipText.ToString)
                End If
            Else
                WorkEnhancedToolTip.SetToolTip(TBox, "")
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub txtModifiers_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtModifiers.MouseHover

        Dim TBox As TextBox = CType(sender, TextBox)
        Dim DetailLine As Integer
        Dim DR As DataRow

        Try

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Position < 0 Then
                WorkEnhancedToolTip.SetToolTip(TBox, "")
                Return
            End If

            DR = CType(_MedDtlBS.Current, DataRowView).Row
            DetailLine = CInt(DR("LINE_NBR"))

            Dim ToolTipTextQuery As String = _ClaimDS.Tables("MEDMOD").AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = DetailLine).
                Aggregate(ToolTipTextQuery, Function(SB As String, ToolTipTextDR As DataRow)
                                                If SB Is Nothing Then SB = ""
                                                Return CStr(SB & If(SB.Trim.Length > 0, Environment.NewLine & ToolTipTextDR("MODIFIER").ToString & " - " & ToolTipTextDR("FULL_DESC").ToString, ToolTipTextDR("MODIFIER").ToString & " - " & ToolTipTextDR("FULL_DESC").ToString))
                                            End Function)

            If ToolTipTextQuery IsNot Nothing AndAlso ToolTipTextQuery.ToString.Trim.Length > 0 Then
                If Me.WorkEnhancedToolTip Is Nothing OrElse String.Compare(Me.WorkEnhancedToolTip.GetToolTip(TBox), ToolTipTextQuery.ToString) <> 0 OrElse WorkEnhancedToolTip.Active = False Then
                    WorkEnhancedToolTip.Active = True
                    WorkEnhancedToolTip.SetToolTip(TBox, ToolTipTextQuery.ToString)
                End If
            Else
                WorkEnhancedToolTip.SetToolTip(TBox, "")
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub txtDiagnoses_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiagnoses.MouseHover

        Dim TBox As TextBox = CType(sender, TextBox)
        Dim DetailLine As Integer
        Dim DR As DataRow
        Dim ToolTipText As New StringBuilder
        Dim DV As DataView
        Try
            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Position < 0 Then
                WorkEnhancedToolTip.SetToolTip(TBox, "")
                Return
            End If

            DR = CType(_MedDtlBS.Current, DataRowView).Row
            DetailLine = CInt(DR("LINE_NBR"))

            DV = New DataView(_ClaimDS.Tables("MEDDIAG"), "LINE_NBR = " & DetailLine.ToString, "LINE_NBR, PRIORITY", DataViewRowState.CurrentRows)

            If DV.Count > 0 Then
                For Cnt As Integer = 0 To DV.Count - 1
                    If DV(Cnt).Row.RowState <> DataRowState.Deleted Then
                        If ToolTipText.ToString.Trim.Length > 0 Then ToolTipText.AppendLine()
                        ToolTipText.Append(DV(Cnt)("SHORT_DESC"))
                    End If
                Next
            End If
            If ToolTipText IsNot Nothing AndAlso ToolTipText.ToString.Trim.Length > 0 Then
                If Me.WorkEnhancedToolTip Is Nothing OrElse String.Compare(Me.WorkEnhancedToolTip.GetToolTip(CType(sender, TextBox)), ToolTipText.ToString) <> 0 OrElse WorkEnhancedToolTip.Active = False Then
                    WorkEnhancedToolTip.Active = True
                    WorkEnhancedToolTip.SetToolTip(CType(sender, TextBox), ToolTipText.ToString)
                End If
            Else
                WorkEnhancedToolTip.SetToolTip(CType(sender, TextBox), "")
            End If



        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub txtBillType_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBillType.MouseHover

        Dim ToolTipText As New StringBuilder
        Dim ToolTipDR As DataRow
        Dim TBox As TextBox = CType(sender, TextBox)

        Try

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Position < 0 Then
                WorkEnhancedToolTip.SetToolTip(TBox, "")
                Return
            End If

            ToolTipDR = CMSDALFDBMD.RetrieveBillTypeValuesInformation(TBox.Text)
            If ToolTipDR IsNot Nothing Then
                If ToolTipText.ToString.Trim.Length > 0 Then ToolTipText.AppendLine()
                ToolTipText.Append(ToolTipDR.Item("FULL_DESC").ToString)
            End If

            If ToolTipText IsNot Nothing AndAlso ToolTipText.ToString.Trim.Length > 0 Then
                If Me.WorkEnhancedToolTip Is Nothing OrElse String.Compare(Me.WorkEnhancedToolTip.GetToolTip(TBox), ToolTipText.ToString) <> 0 OrElse WorkEnhancedToolTip.Active = False Then
                    WorkEnhancedToolTip.Active = True
                    WorkEnhancedToolTip.SetToolTip(TBox, ToolTipText.ToString)
                End If
            Else
                WorkEnhancedToolTip.SetToolTip(TBox, "")
            End If
        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

#End Region

#Region "Menu\Button Events"

    Private Sub FontMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FontMenuItem.Click
        Dim FDia As FontDialog
        Try
            FDia = New FontDialog

            FDia.Font = Me.Font

            If FDia.ShowDialog(Me) <> DialogResult.Cancel Then
                Me.Font = New Font(FDia.Font.Name, FDia.Font.Size, FDia.Font.Style, FDia.Font.Unit, FDia.Font.GdiCharSet)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ResetFontMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetFontMenuItem.Click
        Me.Font = _OrigFont
    End Sub

    Private Sub ShortCutMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShortCutMenuItem.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Displays a summary of shortcuts
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/6/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim SC As Shortcuts

        Try
            SC = New Shortcuts

            SC.ShowDialog(Me)

        Catch ex As Exception
            Throw
        Finally
            If SC IsNot Nothing Then SC.Dispose()

        End Try
    End Sub

    Private Sub ClaimToolBar_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ClaimToolBar.ButtonClick

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:   " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Dim CloseForm As Boolean = False
        Dim ValidationOK As Boolean = True
        Dim AutoValidateHold As AutoValidate = Me.AutoValidate

        Try
            Select Case e.Button.Name
                Case "DropdownHoldRefreshToolBarButton"
                    CType(e.Button.Tag, MenuItem).PerformClick() 'loop around to use specific button
                Case "DropdownRepriceReturnToolBarButton"
                    CType(e.Button.Tag, MenuItem).PerformClick()
                Case Else

                    EnableMenus(False)

                    If _Mode.ToUpper = "AUDIT" Then
                        Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
                    Else
                        Select Case e.Button.Name
                            Case "RefreshToolBarButton"
                                Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
                            Case "HoldToolBarButton", "CompleteToolBarButton", "ReCalcToolBarButton"
                                Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
                                'Me.ValidateChildren() 'Ensure any values changed are bound into db if focus was transferred to a button
                        End Select
                    End If

                    Select Case e.Button.Name
                        Case "RePriceToolBarButton"
                            CloseForm = RePriceClaim(False)
                        Case "RePriceReturnToolBarButton"
                            CloseForm = RePriceClaim(True)
                        Case "CompleteToolBarButton"
                            CloseForm = CompleteClaim()
                        Case "HoldToolBarButton"
                            CloseForm = HoldClaim()
                        Case "RefreshToolBarButton"
                            RefreshClaim()
                        Case "ReCalcToolBarButton"
                            ReCalcClaim()
                    End Select

            End Select

            'Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable 'prevent focus movement from triggering Validation

            'EnableMenus(False)

            'If _Mode.ToUpper = "AUDIT" Then
            'Else
            '    Select Case True
            '        Case e.Button.Text.ToUpper.Contains("REFRESH")

            '            RefreshClaim()

            '        Case e.Button.Text.ToUpper.Contains("HOLD") '"HoldToolBarButton" 'values are saved to db if possible else discarded

            '            CloseForm = HoldClaim()

            '        Case e.Button.Text.ToUpper.Contains("COMPLETE")  '"CompleteToolBarButton"

            '            Me.AutoValidate = AutoValidateHold
            '            CloseForm = CompleteClaim()

            '        Case e.Button.Text.ToUpper.Contains("CALC")  '"ReCalcToolBarButton"

            '            Me.AutoValidate = AutoValidateHold
            '            ReCalcClaim()

            '        Case e.Button.Text.ToUpper.Contains("RETURN")  '"RePriceReturnToolBarButton"

            '            Me.AutoValidate = AutoValidateHold
            '            CloseForm = RePriceClaim(True)

            '        Case e.Button.Text.ToUpper.Contains("PRICE")

            '            Me.AutoValidate = AutoValidateHold
            '            CloseForm = RePriceClaim(False)  '"RePriceToolBarButton"

            '        Case Else
            '            Stop
            '    End Select
            'End If

        Catch IgnoreException As NullReferenceException
            'ignore this error
        Catch ex As Exception
            Throw
        Finally

            If CloseForm Then
                Me.Close()
            Else
                EnableMenus(True)
            End If
            Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange 'prevent focus movement from triggering Validation

        End Try

    End Sub

    Private Sub CompleteMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompleteMenuItem.Click

        Me.ClaimToolBar_ButtonClick(Me.CompleteToolBarButton, New ToolBarButtonClickEventArgs(Me.CompleteToolBarButton))

    End Sub

    Private Sub HoldMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HoldMenuItem.Click

        Me.ClaimToolBar_ButtonClick(Me.HoldToolBarButton, New ToolBarButtonClickEventArgs(Me.HoldToolBarButton))

    End Sub

    Private Sub RefreshMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshMenuItem.Click

        Me.ClaimToolBar_ButtonClick(Me.RefreshToolBarButton, New ToolBarButtonClickEventArgs(Me.RefreshToolBarButton))

    End Sub

    Private Sub HoldContextMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HoldContextMenuItem.Click

        Me.ClaimToolBar_ButtonClick(Me.HoldToolBarButton, New ToolBarButtonClickEventArgs(Me.HoldToolBarButton))

    End Sub

    Private Sub RefreshContextMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshContextMenuItem.Click

        Me.ClaimToolBar_ButtonClick(Me.RefreshToolBarButton, New ToolBarButtonClickEventArgs(Me.RefreshToolBarButton))

    End Sub

    Private Sub RepriceContextMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RepriceContextMenuItem.Click

        Me.ClaimToolBar_ButtonClick(Me.RePriceToolBarButton, New ToolBarButtonClickEventArgs(Me.RePriceToolBarButton))

    End Sub

    Private Sub RepriceReturnContextMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RepriceReturnContextMenuItem.Click

        Me.ClaimToolBar_ButtonClick(Me.RePriceReturnToolBarButton, New ToolBarButtonClickEventArgs(Me.RePriceReturnToolBarButton))

    End Sub

    Private Sub PendContextMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PendContextMenuItem.Click
        Try
            DropdownHoldRefreshToolBarButton.Tag = CType(sender, MenuItem)
            DropdownHoldRefreshToolBarButton.Text = CType(sender, MenuItem).Text

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub AssociatedResultsDataGrid_Click(sender As Object, e As EventArgs) Handles AssociatedResultsDataGrid.Click
        Try

            Select Case CType(sender, DataGridCustom).LastHitSpot.Type
                Case Is = System.Windows.Forms.DataGrid.HitTestType.None

                Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
                Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

                Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                    AssociatedDisplayLineDetails()
                Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

                Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

                Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption

                Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

            End Select
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SendToCSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClaimIDToolStripMenuItem.Click, FamilyIDToolStripMenuItem.Click, PatientToolStripMenuItem.Click

        'Dim BM As BindingManagerBase
        Dim DR As DataRow
        Dim ProcessProperties As ProcessStartInfo
        Dim BS As BindingSource

        Try

            'BM = Me.AssociatedResultsDataGrid.BindingContext(Me.AssociatedResultsDataGrid.DataSource, Me.AssociatedResultsDataGrid.DataMember)

            'If BM.Count < 1 Then Exit Sub
            BS = DirectCast(Me.AssociatedResultsDataGrid.DataSource, BindingSource)

            If BS Is Nothing OrElse BS.Current Is Nothing Then Exit Sub

            DR = DirectCast(BS.Current, DataRowView).Row

            If DR Is Nothing Then Exit Sub

            ProcessProperties = New ProcessStartInfo
            ProcessProperties.FileName = Environment.CurrentDirectory & "\" & "CustomerService.exe"

            Select Case True
                Case CType(sender, ToolStripMenuItem).Name.Contains("Claim")
                    ProcessProperties.Arguments = "CLAIM_ID=" & DR("CLAIM_ID").ToString
                Case CType(sender, ToolStripMenuItem).Name.Contains("Patient")
                    ProcessProperties.Arguments = "PAT_SSN=" & DR("PAT_SSN").ToString
                Case CType(sender, ToolStripMenuItem).Name.Contains("Family")
                    ProcessProperties.Arguments = "FAMILY_ID=" & DR("FAMILY_ID").ToString
            End Select
            ProcessProperties.WindowStyle = ProcessWindowStyle.Normal

            ProcessProperties.UseShellExecute = False
            Process.Start(ProcessProperties)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ResultsHistoryMenuItem_Click(sender As Object, e As EventArgs) Handles ResultsHistoryMenuItem.Click

        Dim AnnotationsDS As AnnotationsDataSet
        Dim DR As DataRow
        Dim Frm As ClaimsHistoryViewerForm
        Dim BS As BindingSource

        Try

            AnnotationsDS = New AnnotationsDataSet
            BS = DirectCast(Me.AssociatedResultsDataGrid.DataSource, BindingSource)
            If BS Is Nothing OrElse BS.Current Is Nothing Then Exit Sub

            DR = DirectCast(BS.Current, DataRowView).Row

            Frm = New ClaimsHistoryViewerForm

            If AnnotationsDS IsNot Nothing AndAlso AnnotationsDS.ANNOTATIONS IsNot Nothing Then
                AnnotationsDS.ANNOTATIONS.Rows.Clear()
            End If
            AnnotationsDS = CType(CMSDALFDBMD.RetrieveClaimAnnotations(CInt(DR("CLAIM_ID")), AnnotationsDS), AnnotationsDataSet)
            Frm.RefreshForm(CInt(DR("CLAIM_ID")), CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), DR("PART_FNAME").ToString, DR("PART_LNAME").ToString, DR("PAT_FNAME").ToString, DR("PAT_LNAME").ToString, AnnotationsDS.ANNOTATIONS)

            Frm.ShowDialog()

        Catch ex As Exception
            Throw
        Finally
            If Frm IsNot Nothing Then Frm.Dispose()
        End Try

    End Sub

    Private Sub RePriceMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RePriceMenuItem.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' submitts a claim to be repriced by batch pricing
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	9/18/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            RePriceClaim(False)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RePriceReturnMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RePriceReturnMenuItem.Click
        Try
            RePriceClaim(True)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RecalcMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecalcMenuItem.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Recalculates the claim based on the current data
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	9/18/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            ReCalcClaim()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RouteMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RouteMenuItem.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' opens a route dialog to route the claim to another user
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	12/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            RouteButton.PerformClick()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DisplayImageMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayImageMenuItem.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' displays the image in the filenet viewer
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/25/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            If IsDBNull(_ClaimDr("DOCID")) = False Then

                Using FNDisplay As New Display
                    FNDisplay.Display(New List(Of Long?) From {CLng(_ClaimDr("DOCID"))})
                End Using

            End If

        Catch ex As ApplicationException
            MessageBox.Show(ex.Message, "FileNet unavailable, Restarting application may resolve connectivity issues.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Throw
        End Try
    End Sub

    Private Sub ProcCodeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProcCodeButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' opens a procedure code selection dialog
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/25/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim Frm As ProcedureCodeLookupForm
        Dim DGRow As DataRow
        Dim ProcDR As DataRow


        Try
            DGRow = CType(_MedDtlBS.Current, DataRowView).Row

            If Not IsDBNull(DGRow("OCC_FROM_DATE")) Then
                Frm = New ProcedureCodeLookupForm(CType(If(IsDBNull(DGRow("OCC_FROM_DATE")), Nothing, DGRow("OCC_FROM_DATE")), Date?))
            Else
                Frm = New ProcedureCodeLookupForm
            End If

            If Frm.ShowDialog(Me) = DialogResult.OK Then

                ProcDR = Frm.SelectedProcedureCodeDataRow

                If Frm.Status = "UPDATELINE" Then

                    ErrorProvider1.ClearError(txtProcedure)

                    If DGRow("STATUS").ToString <> "MERGED" Then

                        DGRow("PROC_CODE") = ProcDR("PROC_VALUE")
                        DGRow("PROC_CODE_DESC") = ProcDR("SHORT_DESC")
                        _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & DGRow("LINE_NBR").ToString & ": Invalid Procedure Code'", CInt(DGRow("LINE_NBR")))

                        DGRow.EndEdit()

                    End If

                ElseIf Frm.Status = "UPDATEALL" Then

                    For Each DR As DataRow In CType(_MedDtlBS.DataSource, DataTable).Rows

                        DR.BeginEdit()

                        DR("PROC_CODE") = ProcDR("PROC_VALUE")

                        DR("PROC_CODE_DESC") = ProcDR("SHORT_DESC")

                        _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & DR("LINE_NBR").ToString & ": Invalid Procedure Code'", CInt(DR("LINE_NBR")))

                        DR.EndEdit()

                    Next

                ElseIf Frm.Status.ToUpper = "CLEARALL" Then

                    MassClearField("PROC_CODE")
                End If
            End If

            _MedDtlBS.EndEdit()

        Catch ex As Exception
            Throw
        Finally
            If Frm IsNot Nothing Then
                Frm.Close()
                Frm.Dispose()
            End If
        End Try
    End Sub

    Private Sub ModifiersButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ModifiersButton.Click
        Try

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ShowDetailLineModifiers()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub DiagnosisButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiagnosisButton.Click

        Try
            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ShowDetailLineDiagnosis()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ReasonButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReasonButton.Click

        Try
            If _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing Then Return

            ShowDetailLineReasons()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub AccumulatorsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AccumulatorsButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' opens an accumulator viewer for a given line of the item
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim DGRow As DataRow


        Try
            If _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse _MedDtlBS.Count = 0 Then Return

            DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

            ShowDetailLineAccumulators(CShort(DGRow("LINE_NBR")))

        Catch ex As Exception
            Throw
        End Try

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
            If _MedDtlBS IsNot Nothing AndAlso _MedDtlBS.Current IsNot Nothing Then
                DetailLinesDataGrid.ResetSelection()
                _MedDtlBS.MovePrevious()
                DetailLinesDataGrid.Select(_MedDtlBS.Position)
            End If

        Catch ex As Exception
            Throw
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

            If _MedDtlBS IsNot Nothing AndAlso _MedDtlBS.Position > -1 AndAlso _MedDtlBS.Count > (_MedDtlBS.Position + 1) Then
                DetailLinesDataGrid.ResetSelection()
                _MedDtlBS.MoveNext()
                DetailLinesDataGrid.Select(_MedDtlBS.Position)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ConsolidateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConsolidateButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Adds a new Detail line to the claim
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	9/19/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            ConsolidateDetailLines()

        Catch exArgument As ArgumentException
            MsgBox(exArgument.Message, MsgBoxStyle.Exclamation, "Invalid Merge request")
            Throw
        End Try
    End Sub

    Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Adds a new Detail line to the claim
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	9/19/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable

            AddDetailLine()

        Catch ex As Exception
            Throw
        Finally
            Me.AutoValidate = System.Windows.Forms.AutoValidate.Inherit
        End Try
    End Sub

    Private Sub AnnotateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AnnotateButton.Click
        Try

            ShowAnnotations()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PatientHistoryButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PatientHistoryButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Loads customer service as claim history
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	12/14/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim CustServ As CustomerServicePlugIn

        Try

            CustServ = New CustomerServicePlugIn(_WorkSharedInterfacesMessage, -1) With {
                .AppKey = _APPKEY & "PatientHistory\",
                .FamilyID = UFCWGeneral.IsNullIntegerHandler(_ClaimDr("FAMILY_ID")),
                .FamilyIDEnabled = False,
                .RelationID = UFCWGeneral.IsNullShortHandler(_ClaimDr("RELATION_ID"))
            }

            CustServ.Show()
            CustServ.Search()

            CustServ.Visible = False 'switch to dialog mode
            CustServ.ShowDialog(Me)

            _ViewedClaimHistory = True

        Catch ex As Exception
            Throw
        Finally
            CustServ.Dispose()
        End Try
    End Sub

    Private Sub PartLookupButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PartLookupButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' opens a participant lookup dialog
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        '     ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/22/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim PartLookup As ParticipantLookUpForm
        Dim Transaction As DbTransaction
        Dim SaveAndQuit As Boolean = False
        Dim CancelChanges As Boolean = False
        Dim DR As DataRow

        Try
            If Not IsDBNull(_ClaimDr("PART_FNAME")) AndAlso Not IsDBNull(_ClaimDr("PART_LNAME")) Then
                PartLookup = New ParticipantLookUpForm(_ClaimDr("PART_LNAME").ToString, _ClaimDr("PART_FNAME").ToString)
            ElseIf Not IsDBNull(_ClaimDr("PART_LNAME")) Then
                PartLookup = New ParticipantLookUpForm(_ClaimDr("PART_LNAME").ToString)
            Else
                PartLookup = New ParticipantLookUpForm
            End If

            If PartLookup.ShowDialog(Me) = DialogResult.OK Then

                DR = PartLookup.PatientRow

                If CBool(DR("TRUST_SW")) Then
                    If Not UFCWGeneralAD.CMSCanAdjudicateEmployee() Then
                        If MessageBox.Show("You are not authorized to work on Employee Claims." &
                                            vbCrLf & "You must save changes back to the queue." & vbCrLf &
                                            "Are you sure you want to make this change?", "Confirm Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                            SaveAndQuit = True
                        Else
                            CancelChanges = True
                        End If
                    End If
                End If

                If Not SaveAndQuit AndAlso Not CancelChanges Then
                    If IsFamilyLocked(CInt(DR("FAMILY_ID"))) Then
                        If MessageBox.Show("This Family is Now Locked." &
                                            vbCrLf & "You must save changes back to the queue." & vbCrLf &
                                            "Are you sure you want to make this change?", "Confirm Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                            SaveAndQuit = True
                        Else
                            CancelChanges = True
                            txtPartSSN.Text = UFCWGeneral.FormatSSN(CStr(_ClaimDr("PART_SSN")))
                        End If
                    Else
                        CMSDALFDBMD.InsertFamilyLock(CInt(DR("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")), _ClaimID, _DomainUser.ToUpper, SystemInformation.ComputerName)
                        CMSDALFDBMD.ReleaseFamilyLock(CInt(_ClaimDr("FAMILY_ID")))
                    End If
                End If

                If Not CancelChanges Then
                    _ClaimDr("FAMILY_ID") = DR("FAMILY_ID")
                    _ClaimDr("PART_SSN") = DR("PART_SSNO")
                    _ClaimDr("PART_FNAME") = DR("FIRST_NAME")
                    _ClaimDr("PART_INT") = DR("MIDDLE_INITIAL")
                    _ClaimDr("PART_LNAME") = DR("LAST_NAME")
                    _ClaimDr("SECURITY_SW") = DR("TRUST_SW")

                    _ClaimAlertManager.DeleteAlertRowsByMessage("Invalid Participant")

                    If CShort(_ClaimDr("RELATION_ID")) <> 0 Then
                        _ClaimDr("RELATION_ID") = -1
                        _ClaimDr("PAT_SSN") = 0
                        _ClaimDr("PAT_FNAME") = DBNull.Value
                        _ClaimDr("PAT_INT") = DBNull.Value
                        _ClaimDr("PAT_LNAME") = DBNull.Value
                        If _MedHdrDr IsNot Nothing Then
                            _MedHdrDr("PAT_DOB") = DBNull.Value
                            _MedHdrDr("PAT_SEX") = DBNull.Value
                        End If
                        _ClaimAlertManager.AddAlertRow(New Object() {"Invalid Patient", 0, "Header", 30})
                    Else
                        _ClaimDr("PAT_SSN") = DR("PART_SSNO")
                        _ClaimDr("PAT_FNAME") = DR("FIRST_NAME")
                        _ClaimDr("PAT_INT") = DR("MIDDLE_INITIAL")
                        _ClaimDr("PAT_LNAME") = DR("LAST_NAME")
                        If _MedHdrDr IsNot Nothing Then
                            _MedHdrDr("PAT_DOB") = DR("BIRTH_DATE")
                            _MedHdrDr("PAT_SEX") = DR("GENDER")
                        End If

                        _ClaimAlertManager.DeleteAlertRowsByMessage("Invalid Patient")

                        If _Mode.ToUpper = "AUDIT" Then
                            LoadEligibility(True)
                        Else
                            LoadEligibility(False)
                        End If
                    End If

                    _ClaimAlertManager.AddAlertRow(New Object() {"Re-Calc Is Required", 0, "Header", 30, Nothing})

                    _ClaimMasterBS.EndEdit()
                    _MedHdrBS.EndEdit()
                End If

                If SaveAndQuit Then
                    'not auth to work employee
                    FinalizeEdits()

                    Transaction = CMSDALCommon.BeginTransaction

                    If SaveChanges(Transaction) Then
                        CMSDALCommon.CommitTransaction(Transaction)
                        Me.Close()
                    Else
                        CMSDALCommon.RollbackTransaction(Transaction)
                    End If
                End If
            End If

        Catch ex As Exception
            If Transaction IsNot Nothing Then
                CMSDALCommon.RollbackTransaction(Transaction)
            End If
            Throw
        End Try
    End Sub

    Private Sub PatLookupButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PatLookupButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' opens a patient lookup dialog
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/22/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim PatLookup As PatientLookup
        Dim DV As DataView
        Dim Indx As Integer
        Dim PatientDR As ClaimDataset.PATIENTRow

        Try
            PatLookup = New PatientLookup(CInt(_ClaimDr("FAMILY_ID")))

            If PatLookup.ShowDialog(Me) = DialogResult.OK Then
                DV = PatLookup.PatientLookupDataGrid.GetDefaultDataView
                Indx = PatLookup.PatientLookupDataGrid.CurrentRowIndex

                _ClaimAlertManager.AddAlertRow(New Object() {"Re-Calc Is Required", 0, "Header", 30, Nothing})

                _ClaimDr("RELATION_ID") = DV(Indx)("RELATION_ID")
                _ClaimDr("PAT_SSN") = DV(Indx)("SSNO")
                _ClaimDr("PAT_FNAME") = DV(Indx)("FIRST_NAME")
                _ClaimDr("PAT_INT") = DV(Indx)("MIDDLE_INITIAL")
                _ClaimDr("PAT_LNAME") = DV(Indx)("LAST_NAME")

                If _MedHdrDr IsNot Nothing Then
                    _MedHdrDr("PAT_DOB") = DV(Indx)("BIRTH_DATE")
                    _MedHdrDr("PAT_SEX") = DV(Indx)("GENDER")
                End If

                _ClaimAlertManager.DeleteAlertRowsByMessage("Invalid Patient")
                _ClaimAlertManager.DeleteAlertRowsByMessage("Incomplete Member Info, reselect Patient")

                _ClaimDS.PATIENT.Clear()
                PatientDR = CType(_ClaimDS.PATIENT.NewRow, ClaimDataset.PATIENTRow)

                PatientDR.FAMILY_ID = CInt(DV(Indx)("FAMILY_ID"))
                PatientDR.RELATION_ID = CShort(DV(Indx)("RELATION_ID"))
                PatientDR.SSNO = CInt(DV(Indx)("SSNO"))
                PatientDR.PART_SSNO = CInt(DV(Indx)("PART_SSNO"))
                PatientDR.FIRST_NAME = UFCWGeneral.IsNullStringHandler(DV(Indx)("FIRST_NAME"))
                PatientDR.MIDDLE_INITIAL = UFCWGeneral.IsNullStringHandler(DV(Indx)("MIDDLE_INITIAL"))
                PatientDR.LAST_NAME = UFCWGeneral.IsNullStringHandler(DV(Indx)("LAST_NAME"))
                PatientDR.GENDER = UFCWGeneral.IsNullStringHandler(DV(Indx)("GENDER"))
                PatientDR.BIRTH_DATE = CDate(UFCWGeneral.IsNullDateHandler(DV(Indx)("BIRTH_DATE")))
                PatientDR.TRUST_SW = CBool(DV(Indx)("TRUST_SW"))
                PatientDR.SURVIVING_SPOUSE_SW = CBool(DV(Indx)("SURVIVING_SPOUSE_SW"))
                PatientDR.STEP_SW = CBool(DV(Indx)("STEP_SW"))
                PatientDR.FOSTER_SW = CBool(DV(Indx)("FOSTER_SW"))
                PatientDR.DISABLE_SW = CBool(DV(Indx)("DISABLE_SW"))
                PatientDR.STUDENT_SW = CBool(DV(Indx)("STUDENT_SW"))
                PatientDR.RELATION = CStr(DV(Indx)("RELATION"))

                _ClaimDS.PATIENT.AddPATIENTRow(PatientDR)

                If _Mode.ToUpper = "AUDIT" Then
                    LoadEligibility(True)
                Else
                    LoadEligibility(False)
                End If

                If _ClaimMemberAccumulatorManager.RelationID <> CShort(_ClaimDr("RELATION_ID")) Then
                    _ClaimMemberAccumulatorManager = New MemberAccumulatorManager(CShort(_ClaimDr("RELATION_ID")), CInt(_ClaimDr("FAMILY_ID")))
                End If
                _ClaimMasterBS.ResetBindings(False)
                _MedHdrBS.ResetBindings(False)
                '_ClaimMasterBS.EndEdit()
                '_MedHdrBS.EndEdit()

            End If

        Catch ex As Exception
            Throw
        Finally
            PatLookup.Dispose()

        End Try
    End Sub

    Private Sub AuditButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AuditButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' opens a audit dialog to add audits
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/26/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            If _AuditForm.ShowDialog(Me) <> DialogResult.Cancel Then
                LoadAuditAlerts(True)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ProvLookupButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProvLookupButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' opens a provider lookup dialog
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/24/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Frm As ProviderLookUpForm
        Dim AlertDV As DataView

        Try

            If Not IsDBNull(_ClaimDr("PROV_TIN")) Then
                Frm = New ProviderLookUpForm(CInt(_ClaimDr("PROV_TIN")))
            Else
                Frm = New ProviderLookUpForm()
            End If

            If Frm.ShowDialog(Me) = DialogResult.OK Then

                _ClaimDr("PROV_ID") = Frm.SelectedProvider("PROVIDER_ID")
                _MedHdrDr("PROV_ID") = Frm.SelectedProvider("PROVIDER_ID")
                _ClaimDr("PROV_NAME") = Frm.SelectedProvider("NAME1")
                _ClaimDr("PROV_TIN") = Frm.SelectedProvider("TAXID")
                _MedHdrDr("PROV_TIN") = Frm.SelectedProvider("TAXID")
                _MedHdrDr("PROV_ZIP") = Frm.SelectedProvider("ZIP")
                _MedHdrDr("PROV_ZIP2") = Frm.SelectedProvider("ZIP_4")


                _ClaimDr("PROV_ADDRESS_SUSPEND_SW") = If(IsDBNull(Frm.SelectedProvider("ADDRESSSUSPENDED")), False, Frm.SelectedProvider("ADDRESSSUSPENDED"))

                'remove old alerts

                _ClaimAlertManager.DeleteAlertRowsByMessage("Invalid Provider")
                _ClaimAlertManager.DeleteAlertRowsByMessage("Provider has Suspended Address")

                'Provider suspended
                If CBool(If(IsDBNull(Frm.SelectedProvider("SUSPEND_SW")), False, Frm.SelectedProvider("SUSPEND_SW"))) Then
                    _ClaimAlertManager.AddAlertRow(New Object() {"Invalid Provider", 0, "Header", 30})
                End If

                'Provider suspended Address
                If CBool(_ClaimDr("PROV_ADDRESS_SUSPEND_SW")) = True Then
                    _ClaimAlertManager.AddAlertRow(New Object() {"Provider has Suspended Address", 0, "Header", 30})
                End If

                _ClaimAlertManager.DeleteAlertRowsByMessage("Provider''s Name Matches Patient''s Name'")
                If Not IsDBNull(Frm.SelectedProvider("NAME1")) AndAlso Not IsDBNull(_ClaimDr("PAT_LNAME")) AndAlso CStr(Frm.SelectedProvider("NAME1")).StartsWith(CStr(_ClaimDr("PAT_LNAME"))) Then
                    _ClaimAlertManager.AddAlertRow(New Object() {"Provider's Name Matches Patient's Name", 0, "Header", 20})
                End If

                _ClaimAlertManager.DeleteAlertRowsByCategory("ProvAlert")
                If IsDBNull(Frm.SelectedProvider("ALERT")) = False AndAlso CStr(Frm.SelectedProvider("ALERT")).Trim <> "" Then
                    If AlertDV.Count = 0 Then
                        _ClaimAlertManager.AddAlertRow(New Object() {Frm.SelectedProvider("DESCRIPTION").ToString & " (Prov Alert)", 0, "ProvAlert", 20})
                    End If
                End If
                _ClaimMasterBS.EndEdit()
                ' Me.BindingContext(_ClaimDS.CLAIM_MASTER).EndCurrentEdit()

            End If

        Catch ex As Exception
            Throw
        Finally
            Frm.Dispose()
        End Try

    End Sub

    Private Sub DeleteMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteMenuItem.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Deletes a detail line
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	10/4/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            DeleteDetailLine()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ReverseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReverseToolStripMenuItem.Click
        Try
            ReversePayment(sender, e)
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub AssignDupMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AssignDupMenuItem.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' marks the claim as a duplicate of another claim
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	12/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            If Not CBool(_ClaimDr("DUPLICATE_SW")) Then
                _ClaimDr("DUPLICATE_SW") = True
            End If
            _ClaimMasterBS.EndEdit()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RouteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RouteButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' shows a route dialog to route the claim to another user
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	12/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            If RouteClaim() Then
                'If HasChanges() Then
                AcceptChanges()
                'End If
                Me.Close()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ArchiveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ArchiveButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' completes the claim without further processing
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	12/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Transaction As DbTransaction

        Try
            _ClaimDr("ARCHIVE_SW") = 1

            FinalizeEdits()

            Transaction = CMSDALCommon.BeginTransaction

            If HasChanges() Then
                If Not SaveChanges(Transaction) Then
                    CMSDALCommon.RollbackTransaction(Transaction)
                    Exit Try
                End If
            End If

            CMSDALFDBMD.ArchiveClaimMaster(_ClaimID, "COMPLETED", _DomainUser.ToUpper, Transaction)

            Dim HistSum As String = "CLAIM ID " & Format(_ClaimID, "00000000") & " HAS BEEN ARCHIVED OUT OF THE QUEUE"
            Dim HistDetail As String = "ADJUSTER " & _DomainUser.ToUpper & " ARCHIVED THIS ITEM." & Microsoft.VisualBasic.vbCrLf &
                                        "NO FURTHER PROCESSING IS NECESSARY."
            CMSDALFDBMD.CreateDocHistory(_ClaimID, UFCWGeneral.IsNullLongHandler(_ClaimDr("DOCID"), "DOCID"), "COMPLETE", CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")), CInt(_ClaimDr("PART_SSN")), CInt(_ClaimDr("PAT_SSN")), CStr(_ClaimDr("DOC_CLASS")), CStr(_ClaimDr("DOC_TYPE")), HistSum, HistDetail, _DomainUser.ToUpper, Transaction)
            CMSDALFDBMD.WriteToClaimsHistoryXML(CInt(_ClaimDr("CLAIM_ID")), UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PART_SSN")), UFCWGeneral.IsNullLongHandler(_ClaimDr("DOCID")), UFCWGeneral.IsNullIntegerHandler(_ClaimDr("FAMILY_ID")), Now(), "Archived")

            CMSDALCommon.CommitTransaction(Transaction)

            AcceptChanges()
            Me.Close()

        Catch ex As Exception
            If Transaction IsNot Nothing Then
                CMSDALCommon.RollbackTransaction(Transaction)
            End If
            Throw

        End Try
    End Sub

    Private Sub OverrideHistoryButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OverrideHistoryButton.Click

        Dim Frm As AccumulatorHistory

        Try

            Frm = New AccumulatorHistory

            Frm.MdiParent = Me.MdiParent

            Frm.AccumulatorHistoryCtrl.SetFormInfo(CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID"))) 'famId, perId)
            Frm.ShowDialog()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub AlertsImageListBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles AlertsImageListBox.MouseDown, AlertsBorder.MouseDown

        Dim DTDialog As DTDialog

        If e.Button = MouseButtons.Right Then
            DTDialog = New DTDialog(_ClaimAlertManager.AlertManagerDataTable, "Claim Alerts")
            DTDialog.Show(Me)
        End If

    End Sub

    Private Sub AlertsBorder_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles AlertsBorder.MouseLeave
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' decreases the size of the alerts box to save space on the form
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        _AlertFloat = False

        HideAlerts()

    End Sub

    Private Sub AlertsBorder_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles AlertsBorder.Leave
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' decreases the size of the alerts box to save space on the form
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        HideAlerts()
    End Sub

    Private Sub AlertsImageListBox_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles AlertsImageListBox.MouseHover
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' increases the size of the alerts box to show more alerts and allow a more friendly
        ' navaigation of alerts
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        If IDMCorrelationTimer.Enabled OrElse _AlertFloat Then Return

        _AlertFloat = True

        FloatAlerts()

    End Sub

    Private Sub AlertsImageListBox_Collapse() Handles AlertsImageListBox.Collapse
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' decreases the size of the alerts box to save space on the form
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            HideAlerts()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ProcessControlsInContainer(ByRef controlContainer As Object, ByVal readOnlyMode As Boolean, Optional ByVal excludeButtons As Boolean = False)

        Dim Ctrl As Control

        Try
            '            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If controlContainer Is Nothing Then Return

            Ctrl = CType(controlContainer, Control)

            If Ctrl Is Nothing Then Return

            Dim CtrlName As String = Ctrl.Name.ToUpper

            UFCWLastKeyData.TEXT = "Control: " & CtrlName

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
                ElseIf TypeOf (ctrl) Is CheckBox Then
                    If CType(ctrl, CheckBox).Enabled = readOnlyMode Then
                        CType(ctrl, CheckBox).Enabled = Not readOnlyMode
                    End If
                End If
            Else

                'continue down container chain
                For Each ChildCtrl As Object In ParentCtrl.Controls

                    'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Mid: " & Me.Name & " : (" & readOnlyMode.ToString & ") " & If(ParentCtrl.Parent.Name IsNot Nothing, ParentCtrl.Parent.Name & " : ", "") & ParentCtrl.Name & " : " & ChildCtrl.GetType.ToString & " : " & If(TypeOf (ChildCtrl) Is TextBox, CType(ChildCtrl, TextBox).ReadOnly, CType(ParentCtrl, Control).Enabled).ToString & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

                    Dim CtrlProperty As PropertyInfo
                    Dim result As Object

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
    Private Sub DetailLinesDataGridCustomContextMenu_Opening(sender As Object, e As CancelEventArgs) Handles DetailLinesDataGridCustomContextMenu.Opening

        Dim DR As DataRow
        Dim DG As DataGridCustom

        Dim DataGridCustomContextMenu As ContextMenuStrip

        Try

            DataGridCustomContextMenu = CType(sender, ContextMenuStrip)
            DataGridCustomContextMenu.Items("DeleteMenuItem").Enabled = False
            DataGridCustomContextMenu.Items("DeleteMenuItem").Available = False

            DG = CType(DirectCast(sender, System.Windows.Forms.ContextMenuStrip).SourceControl, DataGridCustom)

            If DG IsNot Nothing Then

                DR = DG.SelectedRowPreview

                If DR IsNot Nothing Then
                    If DR.RowState = DataRowState.Added Then
                        DataGridCustomContextMenu.Items("DeleteMenuItem").Enabled = True
                        DataGridCustomContextMenu.Items("DeleteMenuItem").Available = True
                    End If
                End If
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub SetUIElements()
        Dim DR As DataRow
        Dim UTLMode As Boolean = False
        Dim FixedNumberDetailLines As Boolean = False

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from occurring when buttons are disabled

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            'ProcessControlsInContainer(CType(grpEditPanel, Object), True, True)

            'grpEditPanel.SuspendLayout()


            Me.PrevButton.Visible = False
            Me.NextButton.Visible = False
            Me.AddButton.Visible = False
            Me.DeleteButton.Visible = False
            Me.ConsolidateButton.Visible = False


            Me.PrevButton.Enabled = False
            Me.NextButton.Enabled = False
            Me.AddButton.Enabled = False
            Me.DeleteButton.Enabled = False
            Me.ConsolidateButton.Enabled = False

            If DetailLinesDataGrid.DataSource IsNot Nothing Then
                If _MedDtlBS IsNot Nothing AndAlso _MedDtlBS.Count > 0 Then
                    Me.PrevButton.Visible = True
                    Me.NextButton.Visible = True
                    If _MedDtlBS.Position < _MedDtlBS.Count - 1 AndAlso _MedDtlBS.Count > 1 Then
                        Me.NextButton.Enabled = True
                    End If

                    If _MedDtlBS.Position > 0 AndAlso _MedDtlBS.Count > 1 Then
                        Me.PrevButton.Enabled = True
                    End If
                    If _MedDtlBS.Position > -1 Then
                        DR = DirectCast(_MedDtlBS.Current, DataRowView).Row
                    End If
                    DetailLineGroupBox.Text = "Edit Item - " & (_MedDtlBS.Position + 1).ToString
                Else
                    DetailLineGroupBox.Text = ""
                End If
            End If

            If _ClaimDr IsNot Nothing AndAlso Not IsDBNull(_ClaimDr("DOC_TYPE")) AndAlso _ClaimDr("DOC_TYPE").ToString.ToUpper.StartsWith("UTL_") Then
                UTLMode = True 'Only routing and SSN / TaxID updates are allowed.
                HideControlsForUTL()
            End If

            If _MedHdrDr Is Nothing OrElse _MedDtlDR Is Nothing Then
                Me.CompleteToolBarButton.Enabled = False
            Else
                Me.CompleteToolBarButton.Enabled = True
            End If

            If _MedHdrDr IsNot Nothing AndAlso Not IsDBNull(_MedHdrDr("PRICED_BY")) AndAlso _MedHdrDr("PRICED_BY").ToString.ToUpper.Contains("JAA") AndAlso _Mode.ToUpper <> "AUDIT" Then
                ArchiveButton.Enabled = False
            End If
            If _MedHdrDr IsNot Nothing AndAlso (_MedHdrDr("PRICED_BY").ToString.Contains("JAA") OrElse _MedHdrDr("PRICED_BY").ToString.Contains("835")) AndAlso _ClaimDr("EDI_SOURCE").ToString.Trim <> "INHOUSE" Then
                FixedNumberDetailLines = True
            End If

            If Not UTLMode Then
                '   ProcessSubControls(CType(grpEditPanel, Object), False, True)

                If _MedDtlBS.Count > 0 Then
                    Me.PrevButton.Visible = True
                    Me.NextButton.Visible = True
                End If

                If DR IsNot Nothing Then 'based upon row status / content decide how to present controls
                    If Not FixedNumberDetailLines AndAlso DR("STATUS").ToString <> "MERGED" Then
                        Me.AddButton.Visible = True
                        Me.AddButton.Enabled = True
                        If DR.RowState = DataRowState.Added Then
                            Me.DeleteButton.Visible = True
                            Me.DeleteButton.Enabled = True
                        End If
                    End If
                Else
                    Me.AddButton.Visible = True
                    Me.AddButton.Enabled = True
                End If

                For Each Ctrl In DetailLineGroupBox.Controls

                    Dim ControlName As String = ""

                    Select Case True
                        Case TypeOf Ctrl Is TextBox
                            ControlName = CType(Ctrl, TextBox).Name
                        Case TypeOf Ctrl Is Button
                            ControlName = CType(Ctrl, Button).Name
                        Case TypeOf Ctrl Is Label
                            ControlName = CType(Ctrl, Label).Name
                    End Select

                    Select Case ControlName
                        Case "NextButton", "PrevButton", "ConsolidateButton"
                        Case Else
                            Select Case True
                                Case TypeOf Ctrl Is TextBox
                                    If DR("STATUS").ToString = "MERGED" Then
                                        CType(Ctrl, TextBox).Enabled = False
                                    Else
                                        CType(Ctrl, TextBox).Enabled = True
                                    End If
                                Case TypeOf Ctrl Is Button
                                    If DR("STATUS").ToString = "MERGED" Then
                                        CType(Ctrl, Button).Enabled = False
                                    Else
                                        CType(Ctrl, Button).Enabled = True
                                    End If
                                Case TypeOf Ctrl Is Label
                                    If DR("STATUS").ToString = "MERGED" Then
                                        CType(Ctrl, Label).Enabled = False
                                    Else
                                        CType(Ctrl, Label).Enabled = True
                                    End If
                            End Select

                    End Select
                Next

            End If

            'If cmbCOB.SelectedValue.ToString.Trim.Length < 1 OrElse cmbCOB.SelectedValue.ToString = "0" Then
            '    txtOIAmt.Clear()
            '    txtOIAmt.Enabled = False
            'End If

            '  grpEditPanel.ResumeLayout() 'needed to ensure transparent controls child controls draw correctly 
            grpEditPanel.Refresh()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Me.AutoValidate = HoldAutoValidate
        End Try

    End Sub

    'Private Sub DetailLinesDataGrid_OnGoTo(ByRef Index As Integer) Handles DetailLinesDataGrid.OnGoTo
    '    ' -----------------------------------------------------------------------------
    '    ' <summary>
    '    ' sets the current row on a goto
    '    ' </summary>
    '    ' <param name="Index"></param>
    '    ' <remarks>
    '    ' </remarks>
    '    ' <history>
    '    ' 	[Nick Snyder]	9/22/2006	Created
    '    ' </history>
    '    ' -----------------------------------------------------------------------------
    '    Try
    '        DetailLinesDataGrid.RemoveHighlights()
    '        DetailLinesDataGrid.UnSelect(DetailLinesDataGrid.CurrentRowIndex)
    '        DetailLinesDataGrid.CurrentRowIndex = Index
    '        DetailLinesDataGrid.Select(Index)
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    Private Sub DetailLinesDataGrid_KeyPushed(ByVal msg As System.Windows.Forms.Message, ByVal key As System.Windows.Forms.Keys, ByRef Cancel As Boolean) Handles DetailLinesDataGrid.KeyPushed
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Handles special Key Combinations such as Delete or Insert
        ' </summary>
        ' <param name="msg"></param>
        ' <param name="key"></param>
        ' <param name="Cancel"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	10/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try
            Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable

            If key = Keys.Delete Then
                DeleteDetailLine()
            ElseIf key = Keys.Shift + Keys.Insert Then
                AddDetailLine()
            End If

        Catch ex As Exception
            Throw
        Finally
            Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        End Try
    End Sub

    'Private Sub PayeeComboBox_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles PayeeComboBox.Validated
    '    ' -----------------------------------------------------------------------------
    '    ' <summary>
    '    ' adds an alert if paying member
    '    ' </summary>
    '    ' <param name="sender"></param>
    '    ' <param name="e"></param>
    '    ' <remarks>
    '    ' </remarks>
    '    ' <history>
    '    ' 	[Nick Snyder]	2/28/2007	Created
    '    ' </history>
    '    ' -----------------------------------------------------------------------------

    '    Dim Binding As Binding

    '    Try


    '        If UpdateTextBinding(sender) Then
    '            Binding = CType(sender, ComboBox).DataBindings("SelectedValue")
    '            Binding.WriteValue()
    '        End If

    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (Rethrow) Then
    '            Throw
    '        Else
    '            MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End If
    '    End Try
    'End Sub

    'Private Sub txtProviderID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtProviderID.TextChanged
    '    ' -----------------------------------------------------------------------------
    '    ' <summary>
    '    ' only allows numeric data to be typed
    '    ' </summary>
    '    ' <param name="sender"></param>
    '    ' <param name="e"></param>
    '    ' <remarks>
    '    ' </remarks>
    '    ' <history>
    '    ' 	[Nick Snyder]	1/24/2007	Created
    '    ' </history>
    '    ' -----------------------------------------------------------------------------
    '    Dim IntCnt As Integer
    '    Dim StrTmp As String

    '    Try

    '        If Not IsNumeric(CType(sender, TextBox).Text) AndAlso Len(CType(sender, TextBox).Text) > 0 Then
    '            StrTmp = CType(sender, TextBox).Text
    '            For IntCnt = 1 To Len(StrTmp)
    '                If Not IsNumeric(Mid(StrTmp, IntCnt, 1)) AndAlso Len(StrTmp) > 0 _
    '                                            AndAlso Mid(StrTmp, IntCnt, 1) <> "-" Then
    '                    StrTmp = Replace(StrTmp, Mid(StrTmp, IntCnt, 1), "")
    '                End If
    '            Next
    '            CType(sender, TextBox).Text = StrTmp
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    Private Sub txtProviderID_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtProviderID.KeyUp
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Looks up provider id and name when enter is pressed
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/24/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            If e.KeyCode = Keys.Enter Then
                ValidateProvTIN()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub txtProviderID_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtProviderID.Validated
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Looks up provider id and name
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/24/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Try
            ValidateProvTIN()
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub ReasonButton_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles ReasonButton.MouseHover, ReasonLabel.MouseHover
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' displays all reason codes in a tooltip
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	10/20/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DetailLine As Integer
        Dim DR As DataRow
        Dim TargetControl As Control

        Try

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing Then Return

            DR = CType(_MedDtlBS.Current, DataRowView).Row
            DetailLine = CInt(DR("LINE_NBR"))
            If sender.GetType = GetType(Button) Then
                TargetControl = CType(sender, Button)
            Else
                TargetControl = CType(sender, Label)
            End If

            Dim ToolTipTextQuery As String = _ClaimDS.Tables("REASON").AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = DetailLine).
                Aggregate(ToolTipTextQuery, Function(SB As String, ToolTipTextDR As DataRow)
                                                If SB Is Nothing Then SB = ""
                                                Return CStr(SB & If(SB.Trim.Length > 0, Environment.NewLine & ToolTipTextDR("REASON").ToString & " - " & ToolTipTextDR("DESCRIPTION").ToString, ToolTipTextDR("REASON").ToString & " - " & ToolTipTextDR("DESCRIPTION").ToString))
                                            End Function)

            If ToolTipTextQuery IsNot Nothing AndAlso ToolTipTextQuery.ToString.Trim.Length > 0 Then
                If Me.WorkEnhancedToolTip Is Nothing OrElse String.Compare(Me.WorkEnhancedToolTip.GetToolTip(TargetControl), ToolTipTextQuery.ToString) <> 0 OrElse WorkEnhancedToolTip.Active = False Then
                    WorkEnhancedToolTip.Active = True
                    WorkEnhancedToolTip.SetToolTip(TargetControl, ToolTipTextQuery.ToString)
                End If
            Else
                WorkEnhancedToolTip.SetToolTip(TargetControl, "")
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ComboBox_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' handles a delete on the combo boxes
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
            If e.KeyCode = Keys.Delete Then
                CType(sender, ComboBox).SelectedValue = DBNull.Value
                CType(sender, ComboBox).Text = Nothing
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub AlertsImageListBox_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles AlertsImageListBox.DoubleClick
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' sets focus to the offending control of the alert
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim RaiseListChangedEventsSaved As Boolean = _MedDtlBS.RaiseListChangedEvents

        Try
            _MedDtlBS.RaiseListChangedEvents = False

            If AlertsImageListBox.Items.Count > 0 AndAlso CType(AlertsImageListBox.SelectedItem, ImageListBox.ImageListBoxItem).Tag IsNot Nothing Then
                AlertFocus(CType(AlertsImageListBox.SelectedItem, ImageListBox.ImageListBoxItem).Tag)
            End If

            _MedDtlBS.RaiseListChangedEvents = RaiseListChangedEventsSaved

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub AlertsImageListBox_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles AlertsImageListBox.MouseMove
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' sets the tooltip if an alert is longer than can be displayed
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim Indx As Integer
        Dim ToolTipText As New StringBuilder
        Dim G As Graphics
        Dim StrSize As SizeF

        Try

            G = Me.CreateGraphics
            StrSize = New SizeF

            Indx = AlertsImageListBox.IndexFromPoint(e.X, e.Y)
            If Indx = -1 Then Exit Try

            If Indx < CType(sender, ListBox).Items.Count Then
                ToolTipText.Append(CType(AlertsImageListBox.Items(Indx), ImageListBox.ImageListBoxItem).Text)
            End If

            StrSize = G.MeasureString(ToolTipText.ToString, CType(sender, ListBox).Font)

            If Indx < CType(sender, ListBox).Items.Count AndAlso IDMCorrelationTimer.Enabled = False Then
                If Indx <> _LastAlertIndex Then
                    If CType(sender, ListBox).Height <= _NormalHeight Then
                        _LastAlertIndex = Indx

                        CType(sender, ListBox).SelectedIndex = Indx
                    End If
                End If
            End If

            If ToolTipText IsNot Nothing AndAlso ToolTipText.ToString.Trim.Length > 0 Then
                If Me.WorkEnhancedToolTip Is Nothing OrElse String.Compare(Me.WorkEnhancedToolTip.GetToolTip(CType(sender, ListBox)), ToolTipText.ToString) <> 0 OrElse WorkEnhancedToolTip.Active = False Then
                    WorkEnhancedToolTip.Active = True
                    WorkEnhancedToolTip.SetToolTip(CType(sender, ListBox), ToolTipText.ToString)
                End If
            Else
                WorkEnhancedToolTip.SetToolTip(CType(sender, ListBox), "")
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub AlertsImageListBox_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles AlertsImageListBox.KeyUp
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' sets focus to the offending control of the alert
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
            If e.KeyCode = Keys.Enter OrElse e.KeyCode = Keys.Space Then
                If AlertsImageListBox.SelectedItem IsNot Nothing AndAlso CType(AlertsImageListBox.SelectedItem, ImageListBox.ImageListBoxItem).Tag IsNot Nothing Then
                    AlertFocus(CType(AlertsImageListBox.SelectedItem, ImageListBox.ImageListBoxItem).Tag)
                End If
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub txtFamilyID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFamilyID.TextChanged, txtPatRelationID.TextChanged
        Dim TBox As TextBox
        Try
            TBox = CType(sender, TextBox)
            Dim digitsOnly As Regex = New Regex("[^\d]")
            TBox.Text = digitsOnly.Replace(TBox.Text, "")

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PartSSNTextBox_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPartSSN.KeyUp
        Try
            If e.KeyCode = Keys.Enter Then
                ValidatePartSSN()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub txtSSN_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtPartSSN.Validating, txtPatSSN.Validating

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Dim TBox As TextBox = CType(sender, TextBox)

        Try

            ErrorProvider1.ClearError(TBox)

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse TBox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If TBox.Text.Trim.Length < 1 OrElse TBox.Text.Trim.Equals("000-00-0000") Then
                ErrorProvider1.SetErrorWithTracking(TBox, "Enter " & If(TBox.Name.ToUpper.Contains("PART"), "Participant", "Patient") & " SSN.")

            ElseIf (TBox.Text.Trim.Length > 0) Then

                If TBox.Text.Contains("-") Then TBox.Text = TBox.Text.Replace("-", "")

                If TBox.Text.Trim.Length <> 9 Then
                    ErrorProvider1.SetErrorWithTracking(TBox, "Enter Valid " & If(TBox.Name.ToUpper.Contains("PART"), "Participant", "Patient") & " SSN in format 000-00-0000.")
                End If
            End If

            If ErrorProvider1.GetError(TBox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw

        Finally

        End Try

    End Sub

    Private Sub txtPartSSN_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPartSSN.Validated
#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Try

            _OrigPartSSN = CType(_ClaimDr("PART_SSN"), Integer?)

            ValidatePartSSN(_OrigPartSSN)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    'Private Sub txtPatRelationID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPatRelationID.TextChanged

    '    Dim StrTmp As String

    '    If Not IsNumeric(CType(sender, TextBox).Text) AndAlso Len(CType(sender, TextBox).Text) > 0 Then
    '        StrTmp = CType(sender, TextBox).Text
    '        For Cnt As Integer = 1 To Len(StrTmp)
    '            If Not IsNumeric(Mid(StrTmp, Cnt, 1)) AndAlso Len(StrTmp) > 0 Then
    '                StrTmp = Replace(StrTmp, Mid(StrTmp, Cnt, 1), "")
    '            End If
    '        Next
    '        CType(sender, TextBox).Text = StrTmp
    '    End If
    'End Sub

    Private Sub txtSSN_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPatSSN.TextChanged, txtPartSSN.TextChanged
        Dim StrTmp As String

        If Not IsNumeric(CType(sender, TextBox).Text) AndAlso Len(CType(sender, TextBox).Text) > 0 Then
            StrTmp = CType(sender, TextBox).Text
            For Cnt As Integer = 1 To Len(StrTmp)
                If Not IsNumeric(Mid(StrTmp, Cnt, 1)) AndAlso Len(StrTmp) > 0 _
                                            AndAlso Mid(StrTmp, Cnt, 1) <> "-" Then
                    StrTmp = Replace(StrTmp, Mid(StrTmp, Cnt, 1), "")
                End If
            Next
            CType(sender, TextBox).Text = StrTmp
        End If
    End Sub

    Private Sub txtPatSSN_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPatSSN.KeyUp
        Try
            If e.KeyCode = Keys.Enter Then
                ValidatePatientSSN()
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub txtPatSSN_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPatSSN.Validated
#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        Try
            ValidatePatientSSN(_OrigPatSSN)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Tabs_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Tabs.SelectedIndexChanged
        Dim CurDoc As Long?
        Dim docs() As Object
        ReDim docs(1)

        Try

            ''Users do not want the claims to auto-sync with image anymore 1-24-06
            Select Case Me.Tabs.SelectedTab.Name.ToUpper
                Case "CLAIMTABPAGE"
                    If _DupTabPage OrElse _AssTabPage Then
                        _DupTabPage = False
                        _AssTabPage = False

                        ' -----------------------------------------------------------------------------
                        ' <summary>
                        ' resets the image in case it was changed looking at duplicates
                        ' </summary>
                        ' <param name="sender"></param>
                        ' <param name="e"></param>
                        ' <remarks>
                        ' </remarks>
                        ' <history>
                        ' 	[Nick Snyder]	12/5/2006	Created
                        ' </history>
                        ' -----------------------------------------------------------------------------

                        Dim FNDisplayVisible As Boolean = Display.Visible
                        CurDoc = Display.CurrentDocumentID

                        If FNDisplayVisible AndAlso IsDBNull(_ClaimDr("DOCID")) = False Then
                            If CurDoc IsNot Nothing AndAlso CurDoc <> CLng(_ClaimDr("DOCID")) Then
                                docs(0) = CLng(_ClaimDr("DOCID"))
                                Using FNDisplay As New Display
                                    FNDisplay.Display(docs)
                                End Using
                                ImageWarning.Visible = False
                                Me.Focus()
                            End If
                        End If
                        _DupTabPage = False
                        Me.Refresh()
                    End If
                Case "DUPLICATESTABPAGE"
                    _DupTabPage = True
                    _ViewedDuplicates = True
                Case "ASSOCIATEDTABPAGE"
                    _AssTabPage = True
                    SplitContainerAssociatedDetailvsCurrent.SplitterDistance = 45
                    SplitContainerAssociated.SplitterDistance = 35
                    AssociatedResultsDataGrid.ContextMenuPrepare(AssociatedClaimsContextMenuStrip)
                    _ViewedAssociated = True
                Case "HISTORYTABPAGE"
                    LoadDocHistory()
                Case "ADDRESSESTABPAGE"
                    LoadParticipantPatientAddress()
                Case "PROVIDERTABPAGE"
                    LoadProviderInfo()
                Case "COBTABPAGE"
                    LoadCOBInfo()
                    _ViewedCOB = True
                Case "LETTERSTABPAGE"
                    LoadLetters()
                Case "ELIGIBILITYTABPAGE"
                    _ViewedEligibility = True
                Case "FREETEXTTABPAGE"
                    _ViewedFreeText = True
            End Select


        Catch ex As Exception
            Throw
        Finally
            Me.SplitContainerDuplicates.Panel2.ResumeLayout(False)
        End Try
    End Sub

    Private Sub DupsTreeView_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles DupsTreeView.AfterSelect
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Loads the selected duplicate into the duplicate viewer
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	12/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim HeaderDV As DataView
        Dim ClaimID As Integer
        Dim docs() As Object
        ReDim docs(1)
        Try
            DuplicateDetailLinesDataGrid.DataSource = Nothing
            DuplicateDetailLinesDataGrid.CaptionText = "0 - Detail Lines"

            txtDupsFamilyID.Text = Nothing
            txtDupsRelID.Text = Nothing
            txtDupsPartSSN.Text = Nothing
            txtDupsPatSSN.Text = Nothing
            txtDupsProvTIN.Text = Nothing
            txtDupsStatus.Text = Nothing
            txtDupsCompletedDate.Text = Nothing

            If DupsTreeView.SelectedNode.Parent IsNot Nothing Then
                ClaimID = CInt(DupsTreeView.SelectedNode.Text.Split(CChar(";"))(0))

                If Not _DuplicatesClaimDS.Tables("DETAIL").Columns.Contains("REASONS") Then
                    _DuplicatesClaimDS.Tables("DETAIL").Columns.Add("REASONS")
                End If

                If Not _DuplicatesClaimDS.Tables("DETAIL").Columns.Contains("DIAGNOSES") Then
                    _DuplicatesClaimDS.Tables("DETAIL").Columns.Add("DIAGNOSES")
                End If

                If Not _DuplicatesClaimDS.Tables("DETAIL").Columns.Contains("MODIFIERS") Then
                    _DuplicatesClaimDS.Tables("DETAIL").Columns.Add("MODIFIERS")
                End If
                For Each DuplicatesDT As DataTable In _DuplicatesClaimDS.Tables
                    If DuplicatesDT.TableName <> "DETAIL" Then

                        HeaderDV = New DataView(DuplicatesDT, "CLAIM_ID = " & ClaimID, "CLAIM_ID", DataViewRowState.CurrentRows)

                        If HeaderDV.Count > 0 Then Exit For
                    End If
                Next

                If HeaderDV.Count > 0 Then

                    txtDupsFamilyID.Text = CStr(HeaderDV(0)("FAMILY_ID"))
                    txtDupsRelID.Text = CStr(HeaderDV(0)("RELATION_ID"))
                    txtDupsPartSSN.Text = UFCWGeneral.FormatSSN(CStr(HeaderDV(0)("PART_SSN")))
                    txtDupsPatSSN.Text = UFCWGeneral.FormatSSN(CStr(HeaderDV(0)("PAT_SSN")))

                    If Not IsDBNull(HeaderDV(0)("PROV_TIN")) Then
                        txtDupsProvTIN.Text = UFCWGeneral.FormatTIN(CStr(HeaderDV(0)("PROV_TIN")))
                    End If

                    txtDupsStatus.Text = CStr(If(CBool(HeaderDV(0)("ARCHIVE_SW")), "ARCHIVED", HeaderDV(0)("STATUS")))
                    txtDupsCompletedDate.Text = Format(HeaderDV(0)("STATUS_DATE"), "MM/dd/yyyy hh:mm:ss tt")

                    Dim ReasonsDV As New DataView(_DuplicatesClaimDS.Tables("REASON"), "CLAIM_ID = " & ClaimID, "CLAIM_ID", DataViewRowState.CurrentRows)
                    Dim DiagnosisDV As New DataView(_DuplicatesClaimDS.Tables("DIAGNOSIS"), "CLAIM_ID = " & ClaimID, "CLAIM_ID", DataViewRowState.CurrentRows)

                    Dim DetailDV As New DataView(_DuplicatesClaimDS.Tables("DETAIL"), "CLAIM_ID = " & ClaimID, "CLAIM_ID", DataViewRowState.CurrentRows)

                    For Each DetailDRV As DataRowView In DetailDV

                        Dim RowReasons As StringBuilder = New StringBuilder
                        ReasonsDV.RowFilter = "CLAIM_ID = " & ClaimID & " AND LINE_NBR = " & DetailDRV("LINE_NBR").ToString
                        ReasonsDV.Sort = "Priority"

                        For Each ReasonDRV As DataRowView In ReasonsDV
                            RowReasons.Append(If(RowReasons.ToString.Trim.Length > 0, ", ", "") & If(ReasonDRV("REASON").ToString.Trim = "LTR" AndAlso Not IsDBNull(ReasonDRV("REASON")), ReasonDRV("REASON").ToString.Trim & ": " & ReasonDRV("LETTER_NAMES").ToString, ReasonDRV("REASON").ToString.Trim))
                        Next

                        DetailDRV("REASONS") = RowReasons

                        Dim RowDiagnoses As StringBuilder = New StringBuilder
                        DiagnosisDV.RowFilter = "CLAIM_ID = " & ClaimID & " AND LINE_NBR = " & DetailDRV("LINE_NBR").ToString
                        DiagnosisDV.Sort = "Priority"

                        For Each DiagnosisDRV As DataRowView In DiagnosisDV
                            RowDiagnoses.Append(If(RowDiagnoses.ToString.Trim.Length > 0, ", ", "") & DiagnosisDRV("DIAGNOSIS").ToString)
                        Next

                        DetailDRV("DIAGNOSES") = RowDiagnoses

                    Next

                    DetailDV.Sort = "LINE_NBR"

                    _DupDtlLineBS = New BindingSource
                    _DupDtlLineBS.DataSource = DetailDV

                    DuplicateDetailLinesDataGrid.SuspendLayout()
                    DuplicateDetailLinesDataGrid.DataSource = _DupDtlLineBS
                    DuplicateDetailLinesDataGrid.SetTableStyle()
                    ' SetTableStyle(DuplicateDetailLinesDataGrid, False)
                    DuplicateDetailLinesDataGrid.ResumeLayout()
                    DuplicateDetailLinesDataGrid.Select(_DupDtlLineBS.Position)

                    DuplicateDetailLinesDataGrid.CaptionText = DetailDV.Count & " - Detail Line" & If(DetailDV.Count <> 1, "s", "") & " for Claim - " & DupsTreeView.SelectedNode.Text

                    If Not IsDBNull(HeaderDV(0)("DOCID")) Then

                        Dim CurDoc As Long?

                        CurDoc = Display.CurrentDocumentID


                        If CurDoc Is Nothing OrElse CurDoc IsNot Nothing AndAlso CurDoc.ToString <> HeaderDV(0)("DOCID").ToString Then

                            docs(0) = CLng(HeaderDV(0)("DOCID"))

                            Using FNDisplay As New Display
                                FNDisplay.Display(docs)
                            End Using

                            Me.Focus()

                            IDMCorrelationTimer.Enabled = True

                        End If

                        Me.Refresh()

                    End If
                End If
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DupsTreeView_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DupsTreeView.MouseDown
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' pops up a menu item to assign main claim as a duplicate
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	12/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DupNode As TreeNode

        Try
            If e.Button = MouseButtons.Right Then
                DupNode = DupsTreeView.GetNodeAt(e.X, e.Y)

                If DupNode IsNot Nothing Then
                    DupsTreeView.SelectedNode = DupNode

                    If DupsTreeView.SelectedNode.Parent IsNot Nothing Then
                        DupsTreeViewContextMenu.Show(DupsTreeView, New Drawing.Point(e.X, e.Y))
                    End If
                End If
            ElseIf e.Button = MouseButtons.Left Then
                DupNode = DupsTreeView.GetNodeAt(e.X, e.Y)

                If DupNode IsNot Nothing AndAlso DupsTreeView.SelectedNode Is DupNode Then
                    If DupNode.Parent IsNot Nothing Then
                        DupsTreeView.SelectedNode = DupNode.Parent
                        DupsTreeView.SelectedNode = DupNode
                    End If
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub EligControl_BeforeRefresh(ByVal sender As Object, ByRef Cancel As Boolean) Handles EligControl.BeforeRefresh
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Loads the current patient Information before refreshing the grid
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="Cancel"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/12/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try
            EligControl.FamilyID = UFCWGeneral.IsNullIntegerHandler(_ClaimDr("FAMILY_ID"))
            EligControl.RelationID = UFCWGeneral.IsNullShortHandler(_ClaimDr("RELATION_ID"))
            EligControl.DocType = UFCWGeneral.IsNullStringHandler(_ClaimDr("DOC_TYPE"))
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub EligControl_AfterRefresh(ByVal sender As Object) Handles EligControl.AfterRefresh
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Reloads eligibility for each detail line after refresh
        ' </summary>
        ' <param name="sender"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/17/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            LoadDetailLineEligibility()

            SumPaid()
            SyncAllowed()
            SumAllowed()
            SumOI()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CheckBoxs_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChiroCheckBox.CheckedChanged, OICheckBox.CheckedChanged
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Sets the value of the checkbox in the dataset. Binding doesn't work all the time
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	12/11/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim CheckBox As CheckBox
        Try
            CheckBox = CType(sender, CheckBox)
            If CheckBox.Name = "ChiroCheckBox" AndAlso CheckBox.Checked AndAlso _ClaimDr("DOC_TYPE").ToString.ToUpper.Contains("HOSPITAL") Then
                CheckBox.Checked = False
                MessageBox.Show("Chiro is not valid for Hospital Document Types.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf CheckBox.DataBindings.Count > 0 AndAlso _MedHdrBS IsNot Nothing AndAlso _MedHdrBS.Current IsNot Nothing AndAlso CBool(CType(_MedHdrBS.Current, DataRowView).Row(CheckBox.DataBindings("Checked").BindingMemberInfo.BindingMember)) <> CheckBox.Checked Then
                CType(_MedHdrBS.Current, DataRowView).Row(CheckBox.DataBindings("Checked").BindingMemberInfo.BindingMember) = CheckBox.Checked
                _ClaimAlertManager.AddAlertRow(New Object() {"Re-Calc Is Required", 0, "Header", 30})
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DocTypeComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DocTypeComboBox.SelectedIndexChanged
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' forces the user to save or cancel changes if the user doesn't have security
        ' for the doc Type they selected
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	12/15/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Transaction As DbTransaction

        If _DocTypeChanging Then Exit Sub

        Try
            _DocTypeChanging = True

            If _ClaimDr IsNot Nothing Then
                Dim LastDocType As String = CStr(_ClaimDr("DOC_TYPE"))

                If Not CheckForUserDocTypeSecurity(_DomainUser.ToUpper, CStr(_ClaimDr("DOC_CLASS")), CStr(_ClaimDr("DOC_TYPE"))) Then
                    If MessageBox.Show("You are not authorized to work on Doc Type " & _ClaimDr("DOC_TYPE").ToString & "." &
                                       vbCrLf & "You must save changes back to the queue." & vbCrLf &
                                       "Are you sure you want to make this change?", "Confirm Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        FinalizeEdits()

                        Transaction = CMSDALCommon.BeginTransaction

                        If SaveChanges(Transaction) Then
                            CMSDALCommon.CommitTransaction(Transaction)

                            AcceptChanges()

                            Me.Close()
                        Else
                            CMSDALCommon.RollbackTransaction(Transaction)
                            _ClaimDr("DOC_TYPE") = LastDocType
                        End If
                    Else
                        _ClaimDr("DOC_TYPE") = LastDocType
                        DisableEnableButtons()
                    End If
                Else
                    DisableEnableButtons()
                End If
                FinalizeEdits()
            End If
        Catch ex As Exception
            If Transaction IsNot Nothing Then
                CMSDALCommon.RollbackTransaction(Transaction)
            End If
        Finally
            _DocTypeChanging = False
        End Try
    End Sub

    Private Sub cmbCOB_Validating(sender As Object, e As CancelEventArgs) Handles cmbCOB.Validating

        Dim DR As DataRow
        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim BS As BindingSource

        Try

            If _MedHdrBS Is Nothing OrElse _MedHdrBS.Current Is Nothing OrElse CBox.ReadOnly Then Return

            DR = DirectCast(_MedHdrBS.Current, DataRowView).Row
            If DR Is Nothing Then Exit Sub

            ErrorProvider1.ClearError(CBox)

            If CBox.SelectedIndex < 0 Then
                ErrorProvider1.SetErrorWithTracking(CBox, " COB Type is required.")
            End If

            If ErrorProvider1.GetError(CBox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & DR("COB").ToString & "/" & CBox.Text & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & DR("COB").ToString & "/" & CBox.Text & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub

    Private Sub cmbCOB_Validated(sender As Object, e As EventArgs) Handles cmbCOB.SelectedIndexChanged, cmbCOB.Validated

        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim DR As DataRow
        Dim BS As BindingSource

        Try

            If _MedHdrBS Is Nothing OrElse _MedHdrBS.Current Is Nothing OrElse CBox.Enabled = False OrElse CBox.ReadOnly Then Return

            BS = DirectCast(_MedDtlBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & CType(_MedHdrBS.Current, DataRowView).Row("COB").ToString & "/" & CBox.Text & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = DirectCast(_MedHdrBS.Current, DataRowView).Row 'need to check if binding is working

            If cmbCOB.SelectedValue IsNot Nothing Then
                Select Case cmbCOB.SelectedValue.ToString
                    Case "1", "2", "3", "4", "5", "6"
                        txtOIAmt.Enabled = True
                    Case Else
                        txtOIAmt.Enabled = False
                End Select
            End If

            If DR.HasVersion(DataRowVersion.Proposed) Then
                Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Mid: " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & CType(_MedHdrBS.Current, DataRowView).Row("COB").ToString & "/" & CBox.Text & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

                _MedHdrBS.EndEdit()
                ' in edit mode ?
                _MedHdrBS.ResetBindings(False)
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & ":" & CBox.Name & " BS(" & BS.Position.ToString & ") Val(" & CType(_MedHdrBS.Current, DataRowView).Row("COB").ToString & "/" & CBox.Text & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub Tabs_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles Tabs.DrawItem
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Highlights tabs if the user is supposed to look at them
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/9/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim txtStr As String

        Try
            txtStr = Me.Tabs.TabPages(e.Index).Text

            If Me.Tabs.TabPages(e.Index) Is DuplicatesTabPage AndAlso _DuplicateThread IsNot Nothing AndAlso _DuplicateThread.IsAlive Then
#If TRACE Then
                If CInt(_TraceParallel.Level) > 3 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If
                e.Graphics.DrawString(txtStr, Me.Tabs.Font, Brushes.Red, e.Bounds.X + 3, e.Bounds.Y + 3)
            ElseIf Me.Tabs.TabPages(e.Index) Is AssociatedTabPage AndAlso _AssociatedThread IsNot Nothing AndAlso _AssociatedThread.IsAlive Then
#If TRACE Then
                If CInt(_TraceParallel.Level) > 3 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If
                e.Graphics.DrawString(txtStr, Me.Tabs.Font, Brushes.Red, e.Bounds.X + 3, e.Bounds.Y + 3)
            ElseIf Me.Tabs.TabPages(e.Index) Is DuplicatesTabPage AndAlso DupsTreeView.GetNodeCount(True) > 1 Then
#If TRACE Then
                If CInt(_TraceParallel.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If
                e.Graphics.DrawString(txtStr, Me.Tabs.Font, Brushes.Blue, e.Bounds.X + 3, e.Bounds.Y + 3)
                DupsTreeView.ExpandAll()
            ElseIf Me.Tabs.TabPages(e.Index) Is FreeTextTabPage AndAlso WorkFreeTextEditor.HasData Then
                e.Graphics.DrawString(txtStr, Me.Tabs.Font, Brushes.Blue, e.Bounds.X + 3, e.Bounds.Y + 3)
            ElseIf Me.Tabs.TabPages(e.Index) Is COBTabPage AndAlso _ClaimDS.MEDOTHER_INS.Rows.Count > 0 Then
                e.Graphics.DrawString(txtStr, Me.Tabs.Font, Brushes.Blue, e.Bounds.X + 3, e.Bounds.Y + 3)
            ElseIf Me.Tabs.TabPages(e.Index) Is AssociatedTabPage AndAlso _AssociatedClaimDS IsNot Nothing AndAlso _AssociatedClaimDS.Tables("Results") IsNot Nothing AndAlso _AssociatedClaimDS.Tables("Results").Rows.Count >= 1 Then
#If TRACE Then
                If CInt(_TraceParallel.Level) > 3 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If
                e.Graphics.DrawString(txtStr, Me.Tabs.Font, Brushes.Blue, e.Bounds.X + 3, e.Bounds.Y + 3)
            Else
#If TRACE Then
                If CInt(_TraceParallel.Level) > 3 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If
                e.Graphics.DrawString(txtStr, Me.Tabs.Font, Brushes.Black, e.Bounds.X + 3, e.Bounds.Y + 3)
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ImageWarning_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ImageWarning.VisibleChanged
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Shows or hides the 2 ImageWarning Labels when the first is changed
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/8/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        ImageWarning2.Visible = ImageWarning.Visible
    End Sub

#End Region

#Region "Events Assigned Via Code"

    Private Sub CLAIM_MASTER_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)
#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & e.Column.ColumnName & " " & e.Row(e.Column.ColumnName).ToString & " -> " & e.ProposedValue.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
    End Sub

    Private Sub MEDHDR_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Is called when a column is changed in MEDHDR
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/5/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & e.Column.ColumnName & " " & e.Row(e.Column.ColumnName).ToString & " -> " & e.ProposedValue.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Dim Changed As Boolean = False

        Try

            If IsDBNull(e.Row(e.Column.ColumnName)) AndAlso Not IsDBNull(e.ProposedValue) Then
                Changed = True
            ElseIf Not IsDBNull(e.Row(e.Column.ColumnName)) AndAlso IsDBNull(e.ProposedValue) Then
                Changed = True
            ElseIf Not IsDBNull(e.Row(e.Column.ColumnName)) AndAlso Not IsDBNull(e.ProposedValue) AndAlso Not (e.Row(e.Column.ColumnName).Equals(e.ProposedValue)) Then
                Changed = True
            End If

            If Changed Then
                Select Case e.Column.ColumnName
                    Case "INCIDENT_DATE", "FROM_DATE", "PAT_DOB"
                        _ClaimAlertManager.AddAlertRow(New Object() {"Re-Calc Is Required", 0, "Header", 30})
                    Case "PROV_ZIP"
                        If AllowReprice() Then _ClaimAlertManager.AddAlertRow(New Object() {"Re-Price Is Required", 0, "Header", 30})
                End Select
                _MedHdrBS.EndEdit()
            End If

        Catch ex As Exception
            Throw
        Finally
            DisableEnableButtons()
        End Try

    End Sub

    Private Sub MEDHDR_RowChanging(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

    End Sub
    Private Sub MEDHDR_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

    End Sub
    '    Try

    '        If _MedHdrBS IsNot Nothing andalso _MedHdrBS.Position > -1  Then
    '            If IsDBNull(CType(_MedHdrBS.Current, DataRowView).Row("INCIDENT_DATE")) AndAlso IncidentDateTextBox.Text.Trim.Length > 0 Then
    '                _ClaimAlertManager.AddAlertRow(New Object() {"Re-Calc Is Required", 0, "Header", 30})
    '            ElseIf Not IsDBNull(CType(_MedHdrBS.Current, DataRowView).Row("INCIDENT_DATE")) AndAlso CDate(CType(_MedHdrBS.Current, DataRowView).Row("INCIDENT_DATE")) <> CDate(IncidentDateTextBox.Text.Trim) Then
    '                _ClaimAlertManager.AddAlertRow(New Object() {"Re-Calc Is Required", 0, "Header", 30})
    '            End If
    '        End If

    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (rethrow) Then
    '            Throw
    '        Else
    '            MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End If
    '    End Try
    'End Sub

    Private Sub MEDMOD_RowChanging(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Is called when a column is changed in MEDMOD
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/5/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            Select Case e.Action
                Case DataRowAction.Add, DataRowAction.Delete
                    If AllowReprice() Then
                        _ClaimAlertManager.AddAlertRow(New Object() {"Re-Price Is Required", 0, "Header", 30})
                    End If
            End Select

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ManualAccumulatorButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManualAccumulatorButton.Click

        Try
            ManualAccumulatorButton.Enabled = False
            _ManualAccumulatorsForm = New ManualAccumulators
            _ManualAccumulatorsForm.MdiParent = Me.MdiParent
            _ManualAccumulatorsForm.ManualAccumulatorValues.IsInEditMode = True
            _ManualAccumulatorsForm.ManualAccumulatorValues.DisplayManualAccumulators(CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")))
            AddHandler _ManualAccumulatorsForm.ManualAccumulatorValues.CloseRequested, AddressOf ManualAccumulatorButton_CloseRequested

            _ManualAccumulatorsForm.ShowDialog()

        Catch ex As Exception
            Throw
        Finally

            ManualAccumulatorButton.Enabled = True

        End Try

    End Sub

    'Private Sub ProviderRenderingNPITextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtProviderRenderingNPI.TextChanged

    '    Dim IntCnt As Integer
    '    Dim StrTmp As String

    '    Try

    '        If Not IsNumeric(CType(sender, TextBox).Text) AndAlso Len(CType(sender, TextBox).Text) > 0 Then
    '            StrTmp = CType(sender, TextBox).Text
    '            For IntCnt = 1 To Len(StrTmp)
    '                If IsNumeric(Mid(StrTmp, IntCnt, 1)) = False AndAlso Len(StrTmp) > 0 Then
    '                    StrTmp = Replace(StrTmp, Mid(StrTmp, IntCnt, 1), "")
    '                End If
    '            Next
    '            CType(sender, TextBox).Text = StrTmp
    '        End If

    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Sub

    'Private Sub BCCZIPTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBCCZIP.TextChanged

    '    Dim StrTmp As String

    '    Try

    '        If Not IsNumeric(CType(sender, TextBox).Text) AndAlso Len(CType(sender, TextBox).Text) > 0 Then
    '            StrTmp = CType(sender, TextBox).Text
    '            For Cnt As Integer = 1 To Len(StrTmp)
    '                If Not IsNumeric(Mid(StrTmp, Cnt, 1)) AndAlso Len(StrTmp) > 0 Then
    '                    StrTmp = Replace(StrTmp, Mid(StrTmp, Cnt, 1), "")
    '                End If
    '            Next
    '            CType(sender, TextBox).Text = String.Format("{0:00000}", StrTmp)
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Sub

    Private Sub grpEditPanel_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles grpEditPanel.Enter
        _DetailGroupBoxSelected = True
    End Sub

    Private Sub DetailLineGroupBox_GotFocus(sender As Object, e As EventArgs) Handles grpEditPanel.GotFocus

        If DetailLineGroupBox.CausesValidation Then ValidateChildren(ValidationConstraints.ImmediateChildren Or ValidationConstraints.Enabled Or ValidationConstraints.TabStop)

    End Sub

    Private Sub DetailLineGroupBox_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles grpEditPanel.Leave
        _DetailGroupBoxSelected = False
    End Sub

    Private Sub AddButton_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddButton.Enter
        _DetailGroupBoxSelected = True
    End Sub

    Private Sub AddButton_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddButton.Leave
        _DetailGroupBoxSelected = False
    End Sub

    Private Sub NextButton_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles NextButton.Enter
        _DetailGroupBoxSelected = True
    End Sub

    Private Sub NextButton_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles NextButton.Leave
        _DetailGroupBoxSelected = False
    End Sub

    Private Sub PrevButton_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles PrevButton.Enter
        _DetailGroupBoxSelected = True
    End Sub

    Private Sub PrevButton_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles PrevButton.Leave
        _DetailGroupBoxSelected = False
    End Sub

    Private Sub Work_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp

        If Not (e.Modifiers = Keys.Control AndAlso (e.KeyCode = Keys.PageDown OrElse e.KeyCode = Keys.PageUp OrElse e.KeyCode = Keys.Add)) Then Return

        Try

            Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable

            If e.KeyCode = Keys.PageDown AndAlso e.Modifiers = Keys.Control AndAlso DetailLinesDataGrid.CurrentRowIndex < DetailLinesDataGrid.GetGridRowCount - 1 Then
                DetailLinesDataGrid.CurrentRowIndex += 1
            ElseIf e.KeyCode = Keys.PageUp AndAlso e.Modifiers = Keys.Control AndAlso DetailLinesDataGrid.CurrentRowIndex > 0 Then
                DetailLinesDataGrid.CurrentRowIndex -= 1
            End If

            If e.KeyCode = Keys.Add AndAlso e.Modifiers = Keys.Control Then
                AddDetailLine()
            End If

        Catch ex As Exception
            Throw
        Finally
            Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        End Try
    End Sub

#End Region

#Region "Mass Detail Line Update"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' pops up a mass update menu if the user right clicks a column header
    ' </summary>
    ' <param name="sender"></param>
    ' <param name="e"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[nick snyder]	8/16/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub DetailLinesDataGrid_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DetailLinesDataGrid.MouseDown
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo
        Try

            HTI = CType(sender, DataGridCustom).HitTest(e.X, e.Y)

            If HTI.Type = DataGrid.HitTestType.ColumnHeader AndAlso e.Button = MouseButtons.Right Then
                CType(sender, DataGridCustom).ContextMenu = MassChangeContextMenu
                _RightClickCol = HTI.Column

            Else
                CType(sender, DataGridCustom).ContextMenu = _OrigGridContextMenu
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' pops up a mass update menu if the user right clicks the Label
    ' </summary>
    ' <param name="sender"></param>
    ' <param name="e"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	8/24/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub ProcLabel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ProcLabel.MouseUp
        Try
            _RightClickCol = DetailLinesDataGrid.GetColumnPosition("PROC_CODE")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' pops up a mass update menu if the user right clicks the Label
    ' </summary>
    ' <param name="sender"></param>
    ' <param name="e"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	8/24/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub PlanLabel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PlanLabel.MouseUp
        Try
            _RightClickCol = DetailLinesDataGrid.GetColumnPosition("MED_PLAN")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' pops up a mass update menu if the user right clicks the Label
    ' </summary>
    ' <param name="sender"></param>
    ' <param name="e"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	8/24/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub ModLabel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ModLabel.MouseUp
        Try
            _RightClickCol = DetailLinesDataGrid.GetColumnPosition("MODIFIERS")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' pops up a mass update menu if the user right clicks the Label
    ' </summary>
    ' <param name="sender"></param>
    ' <param name="e"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	8/24/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub DiagnosisLabel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagnosisLabel.MouseUp
        Try
            _RightClickCol = DetailLinesDataGrid.GetColumnPosition("DIAGNOSES")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PlaceOfServLabel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PlaceOfServLabel.MouseUp
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' pops up a mass update menu if the user right clicks the Label
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	8/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            _RightClickCol = DetailLinesDataGrid.GetColumnPosition("PLACE_OF_SERV")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub BillTypeLabel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BillTypeLabel.MouseUp
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' pops up a mass update menu if the user right clicks the Label
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	8/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            _RightClickCol = DetailLinesDataGrid.GetColumnPosition("BILL_TYPE")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub FromLabel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles FromLabel.MouseUp
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' pops up a mass update menu if the user right clicks the Label
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	8/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            _RightClickCol = DetailLinesDataGrid.GetColumnPosition("OCC_FROM_DATE")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ToLabel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ToLabel.MouseUp
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' pops up a mass update menu if the user right clicks the Label
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	8/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            _RightClickCol = DetailLinesDataGrid.GetColumnPosition("OCC_TO_DATE")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ChargeLabel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ChargeLabel.MouseUp
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' pops up a mass update menu if the user right clicks the Label
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	8/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            _RightClickCol = DetailLinesDataGrid.GetColumnPosition("CHRG_AMT")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PricedLabel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PricedLabel.MouseUp
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' pops up a mass update menu if the user right clicks the Label
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	8/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            _RightClickCol = DetailLinesDataGrid.GetColumnPosition("PRICED_AMT")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub OILabel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles OILabel.MouseUp
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' pops up a mass update menu if the user right clicks the Label
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	8/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            _RightClickCol = DetailLinesDataGrid.GetColumnPosition("OTH_INS_AMT")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PaidLabel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PaidLabel.MouseUp
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' pops up a mass update menu if the user right clicks the Label
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	8/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            _RightClickCol = DetailLinesDataGrid.GetColumnPosition("PAID_AMT")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub UnitsLabel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles UnitsLabel.MouseUp
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' pops up a mass update menu if the user right clicks the Label
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	8/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            _RightClickCol = DetailLinesDataGrid.GetColumnPosition("DAYS_UNITS")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' pops up a mass update menu if the user right clicks the Label
    ' </summary>
    ' <param name="sender"></param>
    ' <param name="e"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	8/24/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub ActionLabel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ActionLabel.MouseUp
        Try
            _RightClickCol = DetailLinesDataGrid.GetColumnPosition("STATUS")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' pops up a mass update menu if the user right clicks the Label
    ' </summary>
    ' <param name="sender"></param>
    ' <param name="e"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	8/24/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub ReasonLabel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ReasonLabel.MouseUp
        Try
            _RightClickCol = DetailLinesDataGrid.GetColumnPosition("REASON_SW")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' pops up a mass update menu if the user right clicks the Label
    ' </summary>
    ' <param name="sender"></param>
    ' <param name="e"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	8/24/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub AccumulatorLabel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles AccumulatorLabel.MouseUp
        Try
            _RightClickCol = DetailLinesDataGrid.GetColumnPosition("Accumulators")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub NDCLabel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NDCLabel.MouseUp
        Try
            _RightClickCol = DetailLinesDataGrid.GetColumnPosition("NDC")
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub UpdateMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateMenuItem.Click

        Try
            If _RightClickCol IsNot Nothing Then MassUpdate(CInt(_RightClickCol))

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub MassUpdate(ByVal columnIndex As Integer)

        Dim Frm As New MassUpdateForm
        Dim Mapping As String
        Dim HeaderText As String
        Dim Value As Object
        Dim PlanDV As DataView
        Dim ProcCodeLookup As New ProcedureCodeLookupForm

        Dim MedDtlRow As DataRow

        Try

            Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable

            If Value Is Nothing Then
                Value = DBNull.Value
            End If
            Frm.FieldValue.Visible = False
            Frm.ActionComboBox.Visible = False
            Frm.PlanComboBox.Visible = False

            Mapping = DetailLinesDataGrid.GetCurrentTableStyle.GridColumnStyles(columnIndex).MappingName
            HeaderText = DetailLinesDataGrid.GetCurrentTableStyle.GridColumnStyles(columnIndex).HeaderText

            Select Case Mapping
                Case Is = "PROC_CODE"
                    Frm.FieldValue.MaxLength = 10
                Case Is = "MED_PLAN"
                Case Is = "MODIFIER_SW", "MODIFIER", "MODIFIERS"
                    Frm.FieldValue.MaxLength = 10
                Case Is = "DIAGNOSIS", "DIAGNOSES"
                    Frm.FieldValue.MaxLength = 10
                Case Is = "PLACE_OF_SERV"
                    Frm.FieldValue.MaxLength = 3
                Case Is = "BILL_TYPE"
                    Frm.FieldValue.MaxLength = 3
                Case Is = "OCC_FROM_DATE"
                    Frm.FieldValue.MaxLength = 10
                Case Is = "OCC_TO_DATE"
                    Frm.FieldValue.MaxLength = 10
                Case Is = "CHRG_AMT"
                Case Is = "PRICED_AMT"
                Case Is = "OTH_INS_AMT"
                Case Is = "PAID_AMT"
                Case Is = "DAYS_UNITS"
                    Frm.FieldValue.MaxLength = 9
                Case Is = "STATUS"
                    Frm.FieldValue.MaxLength = 4
                Case Is = "NDC"
                    Frm.FieldValue.MaxLength = 11
                Case Is = "REASON_SW", "REASONS"
                Case Is = "Accumulators"
                Case Else
                    MessageBox.Show("Field " & HeaderText & " is Not Updateable", "Cannot Update", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Try
            End Select

            If _MedDtlBS IsNot Nothing AndAlso _MedDtlBS.Current IsNot Nothing AndAlso Mapping <> "REASON_SW" Then

                MedDtlRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

                If Not IsDBNull(MedDtlRow(Mapping)) Then

                    Select Case True
                        Case TypeOf MedDtlRow(Mapping) Is Date
                            Value = UFCWGeneral.ValidateDate(MedDtlRow(Mapping))
                            If Value IsNot Nothing Then Value = CDate(Value).ToShortDateString
                        Case TypeOf MedDtlRow(Mapping) Is Decimal
                            Value = CDec(MedDtlRow(Mapping)).ToString
                        Case Else
                            Value = MedDtlRow(Mapping).ToString
                    End Select

                End If

                Select Case Mapping
                    Case Is = "MODIFIER_SW", "MODIFIER", "MODIFIERS"
                        ShowDetailLineModifiers()
                    Case Is = "DIAGNOSIS", "DIAGNOSES"
                        ShowDetailLineDiagnosis()
                    Case Is = "REASON_SW", "REASONS"
                        ShowDetailLineReasons()
                    Case Is = "Accumulators"
                        ShowDetailLineAccumulators(CShort(MedDtlRow("LINE_NBR")), True)
                    Case Is = "PROC_CODE"
                        If ProcCodeLookup.ShowDialog(Me) = DialogResult.OK Then
                            If ProcCodeLookup.Status = "UPDATELINE" Then
                                MedDtlRow("PROC_CODE") = If(ProcCodeLookup.ProcCode.ToString.Trim.Length > 0, ProcCodeLookup.ProcCode, "")
                            ElseIf ProcCodeLookup.Status = "UPDATEALL" Then
                                Frm.FieldValue.Text = CStr(If(ProcCodeLookup.ProcCode.ToString.Trim.Length > 0, ProcCodeLookup.ProcCode, ""))
                                MassUpdateField(Mapping, Frm.FieldValue.Text.ToUpper)
                                txtProcedure.Text = Frm.FieldValue.Text
                            ElseIf ProcCodeLookup.Status.ToUpper = "CLEARALL" Then
                                MassClearField("PROC_CODE")
                            End If
                        End If
                    Case Else

                        If Mapping = "STATUS" Then
                            Frm.ActionComboBox.Visible = True
                            Frm.ActionComboBox.SelectedValue = Value
                        ElseIf Mapping = "MED_PLAN" Then
                            PlanDV = New DataView(_PlanDT, "", "PLAN_TYPE", DataViewRowState.CurrentRows)
                            Frm.PlanComboBox.DataSource = PlanDV
                            Frm.PlanComboBox.DisplayMember = "PLAN_TYPE"
                            Frm.PlanComboBox.ValueMember = "PLAN_TYPE"

                            Frm.PlanComboBox.Visible = True
                            Frm.PlanComboBox.SelectedValue = Value
                        Else
                            Frm.FieldValue.Text = UFCWGeneral.IsNullStringHandler(Value)
                            Frm.FieldValue.Visible = True
                        End If

                        Frm.Top = Cursor.Position.Y
                        Frm.Left = Cursor.Position.X

                        Frm.FieldName = HeaderText
                        Frm.MappingName = Mapping

ShowUpdate:
                        If Frm.ShowDialog(Me) = DialogResult.OK Then
                            If Mapping = "STATUS" Then Frm.FieldValue.Text = Frm.ActionComboBox.Text
                            If Mapping = "MED_PLAN" Then Frm.FieldValue.Text = Frm.PlanComboBox.SelectedValue.ToString

                            If Not ValidateField(Mapping, Frm.FieldValue.Text.ToUpper, False) Then
                                Frm.FieldValue.SelectionStart = 0
                                Frm.FieldValue.SelectionLength = Frm.FieldValue.Text.Length

                                If Mapping = "STATUS" Then
                                    Frm.ActionComboBox.Focus()
                                ElseIf Mapping = "MED_PLAN" Then
                                    Frm.PlanComboBox.Focus()
                                Else
                                    Frm.FieldValue.Focus()
                                End If

                                GoTo ShowUpdate
                            Else
                                MassUpdateField(Mapping, Frm.FieldValue.Text.ToUpper)
                            End If
                        End If
                End Select
            End If

        Catch ex As Exception
            Throw
        Finally
            If Frm IsNot Nothing Then Frm.Dispose()
            Frm = Nothing

            If ProcCodeLookup IsNot Nothing Then ProcCodeLookup.Dispose()

            Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange

        End Try
    End Sub

    Private Sub ClearMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearMenuItem.Click

        Dim Mapping As String

        Try
            If _RightClickCol Is Nothing Then Return

            Mapping = DetailLinesDataGrid.GetCurrentTableStyle.GridColumnStyles(CInt(_RightClickCol)).MappingName

            If Mapping = "MODIFIERS" Then Mapping = "MODIFIER_SW"

            MassClearField(Mapping)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function ValidateField(ByVal fieldMapping As String, ByRef value As Object, ByVal silent As Boolean) As Boolean

        Dim ValueDR As DataRow
        Dim DR As DataRow

        Dim DV As DataView
        Dim AsOfDate As Date?

        Try

            DV = DetailLinesDataGrid.GetDefaultDataView

            Select Case fieldMapping

                Case Is = "OCC_FROM_DATE"

                    value = UFCWGeneral.ValidateDate(value)

                    If value Is Nothing Then
                        If Not silent Then
                            MessageBox.Show("Invalid First Date Of Service", "Invalid Date Value", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                        Return False
                    End If

                    For Each DR In DV.ToTable.Rows
                        If Not IsDBNull(DR("OCC_TO_DATE")) Then
                            If CDate(DR("OCC_TO_DATE")) < CDate(value) Then
                                If Not silent Then
                                    MessageBox.Show("That Date Value Is Greater Than To Date For Line " & DR("LINE_NBR").ToString, "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                End If
                                Return False
                            End If
                        End If
                    Next

                Case Is = "OCC_TO_DATE"

                    value = UFCWGeneral.ValidateDate(value)

                    If value Is Nothing Then
                        If Not silent Then
                            MessageBox.Show("Invalid Last Date Of Service", "Invalid Date Value", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                        Return False
                    End If

                    For Each DR In DV.ToTable.Rows

                        If CDate(DR("OCC_FROM_DATE")) > CDate(value) Then
                            If Not silent Then
                                MessageBox.Show("Last Date Of Service cannot be before First Date Of Service - Line " & DR("LINE_NBR").ToString, "Invalid Date Value", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            End If
                            Return False
                        End If

                    Next

                Case Is = "PROC_CODE"
                    If value.ToString.Trim.Length > 0 Then
                        For Each DR In DV.ToTable.Rows
                            AsOfDate = GetAsOfDate(DR, silent)
                            If AsOfDate Is Nothing Then
                                Return False
                            Else
                                ValueDR = CMSDALFDBMD.RetrieveProcedureValueInformation(CStr(value), AsOfDate)
                                If ValueDR Is Nothing Then
                                    If Not silent Then
                                        MessageBox.Show("Invalid Procedure Code - Line " & DR("LINE_NBR").ToString, "Invalid Procedure Code Value", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                    End If
                                    Return False
                                End If
                            End If
                        Next
                    End If

                Case Is = "MED_PLAN"
                    Dim strValue As String = CStr(value)
                    Dim bindingSource = CType(cmbPlan.DataSource, BindingSource)
                    Dim dt = CType(bindingSource.DataSource, DataTable)
                    Dim dt1 As DataTable = dt.Rows.Cast(Of DataRow)().Where(Function(row) Not row.ItemArray.All(Function(f) TypeOf f Is DBNull OrElse String.IsNullOrEmpty(If(TryCast(f, String), f.ToString())))).CopyToDataTable()
                    Dim PlansQuery = dt1.AsEnumerable().Where(Function(r) r.Field(Of String)("PLAN_TYPE").Contains(strValue))

                    If Not PlansQuery.any Then
                        If Not silent Then
                            MessageBox.Show("Invalid Medical Plan", "Invalid Plan Value", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                        Return False
                    End If

                Case Is = "MODIFIER"
                    'lookup modifier

                    For Each DR In DV.ToTable.Rows

                        AsOfDate = GetAsOfDate(DR, silent)

                        If AsOfDate Is Nothing Then
                            Return False
                        Else
                            ValueDR = CMSDALFDBMD.RetrieveModifierValuesInformation(CStr(value), AsOfDate)

                            If ValueDR Is Nothing Then
                                If Not silent Then
                                    MessageBox.Show("Invalid Modifier - Line " & DR("LINE_NBR").ToString, "Invalid Modifier Value", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                End If

                                Return False

                            End If
                        End If

                    Next

                Case Is = "DIAGNOSIS"

                    value = CStr(value).Replace(".", "").ToString.ToUpper

                    For Each DR In DV.ToTable.Rows

                        AsOfDate = GetAsOfDate(DR, silent)

                        If AsOfDate Is Nothing Then
                            Return False
                        Else
                            ValueDR = CMSDALFDBMD.RetrieveDiagnosisValuesInformation(CStr(value), AsOfDate)

                            If ValueDR Is Nothing Then
                                If Not silent Then
                                    MessageBox.Show("Invalid Diagnosis - Line " & DR("LINE_NBR").ToString, "Invalid Diagnosis Value", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                End If

                                Return False

                            End If
                        End If

                    Next

                Case Is = "PLACE_OF_SERV"

                    For Each DR In DV.ToTable.Rows

                        AsOfDate = GetAsOfDate(DR, silent)

                        If AsOfDate Is Nothing Then
                            Return False
                        Else
                            ValueDR = CMSDALFDBMD.RetrievePlaceOfServiceValueInformation(CStr(value), AsOfDate)

                            If ValueDR Is Nothing Then
                                If Not silent Then
                                    MessageBox.Show("Invalid Place Of Service - Line " & DR("LINE_NBR").ToString, "Invalid Place Of Service Value", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                End If
                                Return False
                            End If

                        End If

                    Next

                Case Is = "BILL_TYPE"

                    For Each DR In DV.ToTable.Rows

                        AsOfDate = GetAsOfDate(DR, silent)

                        If AsOfDate Is Nothing Then
                            Return False
                        Else
                            ValueDR = CMSDALFDBMD.RetrieveBillTypeValuesInformation(CStr(value), AsOfDate)

                            If ValueDR Is Nothing Then
                                If Not silent Then
                                    MessageBox.Show("Invalid Bill Type - Line " & DR("LINE_NBR").ToString, "Invalid Bill Type Value", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                End If
                                Return False
                            End If

                        End If

                    Next

                Case Is = "CHRG_AMT", "PRICED_AMT", "OTH_INS_AMT", "PAID_AMT", "PROCESSED_AMT"

                    If Not IsNumeric(value) Then
                        If Not silent Then
                            MessageBox.Show("Invalid Dollar Amount", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                        Return False
                    End If

                Case Is = "DAYS_UNITS"

                    If Not IsNumeric(value) Then
                        If Not silent Then
                            MessageBox.Show("Invalid Unit Value", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                        Return False
                    End If

                Case Is = "STATUS"
                    If Not cmbStatus.Items.Contains(value) Then
                        If Not silent Then
                            MessageBox.Show("Invalid Status", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                        Return False
                    End If

                Case Is = "REASON_SW"
                    'lookup Reason

                    If value.ToString.Length < 3 Then
                        If IsNumeric(value) Then
                            value = value.ToString.PadLeft(3, CChar("0"))
                        End If
                    End If

                    For Each DR In DV.ToTable.Rows

                        AsOfDate = GetAsOfDate(DR, silent)

                        If AsOfDate Is Nothing Then
                            Return False
                        Else
                            ValueDR = CMSDALFDBMD.RetrieveReasonValuesInformation(CStr(value), AsOfDate)

                            If ValueDR Is Nothing Then
                                If Not silent Then
                                    MessageBox.Show("Invalid Reason - Line " & DR("LINE_NBR").ToString, "Invalid Reason Value", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                End If

                                Return False

                            End If
                        End If

                    Next

            End Select

            Return True

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

    Private Function GetAsOfDate(DR As DataRow, silent As Boolean) As Date?
        Dim DOSDV As DataView

        If Not IsDBNull(DR("OCC_FROM_DATE")) Then
            Return CDate(DR("OCC_FROM_DATE"))
        Else
            DOSDV = New DataView(_ClaimDS.MEDDTL, "OCC_FROM_DATE = MIN(OCC_FROM_DATE)", "OCC_FROM_DATE", DataViewRowState.CurrentRows)

            If DOSDV.Count > 0 AndAlso Not IsDBNull(DOSDV(0)("OCC_FROM_DATE")) Then
                Return CDate(DOSDV(0)("OCC_FROM_DATE"))
            Else
                If Not IsDBNull(_ClaimDr("DATE_OF_SERVICE")) Then
                    Return CDate(_ClaimDr("DATE_OF_SERVICE"))
                Else
                    Return CDate(UFCWGeneral.NowDate)
                    'If Not silent Then
                    '    MessageBox.Show("There Is no Date Of Service To validate against.", "Unable To Validate", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    'End If
                End If
            End If
        End If

        Return Nothing
    End Function

    Private Sub MassUpdateField(ByVal fieldMapping As String, ByRef value As Object, Optional ByVal includeMergedItems As Boolean = False)

        Dim DV As DataView
        Dim DateOfService As DateTime = UFCWGeneral.NowDate
        Dim DR As DataRow

        Try

            Dim f1 As FieldInfo = GetType(ComboBox).GetField("EVENT_SELECTEDINDEXCHANGED", BindingFlags.[Static] Or BindingFlags.NonPublic)

            Dim obj As Object = f1.GetValue(cmbPlan)
            Dim pi As PropertyInfo = cmbPlan.[GetType]().GetProperty("Events", BindingFlags.NonPublic Or BindingFlags.Instance)
            Dim list As EventHandlerList = DirectCast(pi.GetValue(cmbPlan, Nothing), EventHandlerList)
            list.[RemoveHandler](obj, list(obj))


            'needed for bug if alert listbox is focused
            DetailLineGroupBox.Focus()

            Using WC As New GlobalCursor

                DV = DetailLinesDataGrid.GetDefaultDataView

                Select Case fieldMapping
                    Case Is = "PROC_CODE"
                        _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Invalid Procedure Code")

                        MassUpdateProcCode(fieldMapping, value, includeMergedItems)

                    Case Is = "MED_PLAN"

                        cmbPlan.SelectedValue = value
                        MassUpdatePlanType(CStr(value))
                    Case Is = "MODIFIER"

                        _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Invalid Modifier")

                        MassUpdateModifier(fieldMapping, value, includeMergedItems)

                    Case Is = "DIAGNOSIS"

                        _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Invalid Diagnosis")

                        value = CStr(value).Replace(".", "").ToString.ToUpper

                        MassUpdateDiagnosis(fieldMapping, value, includeMergedItems)

                    Case Is = "PLACE_OF_SERV"

                        _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Invalid Place of Service")

                        MassUpdatePOS(fieldMapping, value, includeMergedItems)

                    Case Is = "BILL_TYPE"

                        _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Invalid Bill Type")

                        MassUpdateBillType(fieldMapping, value, includeMergedItems)

                    Case Is = "OCC_FROM_DATE"

                        _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Missing From Date")
                        _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Was Received 1 Year After DOS")

                        MassUpdateFDOS(fieldMapping, value, includeMergedItems)

                        ErrorProvider1.ClearError(txtFromDate)
                    Case Is = "OCC_TO_DATE"

                        MassUpdateLDOS(fieldMapping, value, includeMergedItems)
                        ErrorProvider1.ClearError(txtToDate)
                    Case Is = "PRICED_AMT"

                        _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Paid Is More Than Priced")

                        For Cnt As Integer = 0 To DV.Count - 1
                            If value.ToString.IsDecimal() AndAlso Not IsDBNull(DV(Cnt)("PAID_AMT")) AndAlso CDec(value) < CDec(Format(DV(Cnt)("PAID_AMT"), "0.00")) Then
                                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DV(Cnt)("LINE_NBR").ToString & ": Paid Is More Than Priced", DV(Cnt)("LINE_NBR").ToString, "Detail", 20})
                            End If
                        Next

                    Case Is = "PAID_AMT"

                        _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Paid Is More Than Priced")
                        _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Paid Is 0 and a Reason is Required")

                        For Cnt As Integer = 0 To DV.Count - 1
                            If value.ToString.IsDecimal() AndAlso Not IsDBNull(DV(Cnt)("PRICED_AMT")) AndAlso CDec(value) > CDec(Format(DV(Cnt)("PRICED_AMT"), "0.00")) Then
                                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DV(Cnt)("LINE_NBR").ToString & ": Paid Is More Than Priced", DV(Cnt)("LINE_NBR").ToString, "Detail", 20})
                            End If

                            If Not CBool(DV(Cnt)("REASON_SW")) AndAlso ((IsNumeric(value) = False) OrElse ((IsNumeric(value) = True AndAlso CDec(value) = 0))) Then
                                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DV(Cnt)("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required", DV(Cnt)("LINE_NBR").ToString, "Detail", 30})
                            End If
                        Next

                    Case Is = "CHRG_AMT", "OTH_INS_AMT", "PROCESSED_AMT"
                    Case Is = "DAYS_UNITS"
                    Case Is = "STATUS"

                        _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Paid Is More Than Priced")
                        _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Paid Is 0 and a Reason is Required")

                        MassUpdateStatus(fieldMapping, value, includeMergedItems)

                    Case Is = "REASON_SW"

                        MassUpdateReason(fieldMapping, value, includeMergedItems)

                End Select

                For Each DR In _ClaimDS.MEDDTL
                    If includeMergedItems Then
                        If fieldMapping = "OCC_FROM_DATE" Then
                            DR(fieldMapping) = value
                            LoadDetailLineEligibility(DR, False)
                        End If
                    Else
                        If DR("STATUS").ToString <> "MERGED" Then
                            If fieldMapping = "OCC_FROM_DATE" Then
                                DR(fieldMapping) = value
                                LoadDetailLineEligibility(DR, False)
                            ElseIf fieldMapping = "CHRG_AMT" Then
                                DR("CHRG_AMT") = CDec(value)
                            ElseIf fieldMapping = "PRICED_AMT" Then
                                DR("PRICED_AMT") = CDec(value)
                            ElseIf fieldMapping = "OTH_INS_AMT" Then
                                DR("OTH_INS_AMT") = CDec(value)
                            ElseIf fieldMapping = "PAID_AMT" Then
                                DR("PAID_AMT") = CDec(value)
                            ElseIf fieldMapping = "PROCESSED_AMT" Then
                                DR("PROCESSED_AMT") = CDec(value)
                            ElseIf fieldMapping = "NDC" Then
                                DR("NDC") = value
                            Else
                                DR(fieldMapping) = value
                            End If
                        End If
                    End If
                Next

                If fieldMapping = "CHRG_AMT" Then
                    SumCharges()
                End If

                If fieldMapping = "PRICED_AMT" Then
                    SumPriced()
                End If
                If fieldMapping = "OTH_INS_AMT" Then
                    SumOI()
                End If


                If fieldMapping = "CHRG_AMT" OrElse fieldMapping = "PRICED_AMT" Then
                    SyncAllowed()
                    SumAllowed()
                End If

                SumPaid()

                _MedDtlBS.EndEdit()

            End Using

        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub
    Private Sub MassClearField(ByVal fieldMapping As String)
        Dim Value As Object = DBNull.Value
        Dim HeaderText As String
        Dim ReasonDV As DataView

        Try

            If DetailLinesDataGrid.GetGridRowCount = 0 Then Return

            If _RightClickCol IsNot Nothing Then HeaderText = DetailLinesDataGrid.GetCurrentTableStyle.GridColumnStyles(CInt(_RightClickCol)).HeaderText

            Select Case fieldMapping
                Case Is = "PROC_CODE"
                    'Proc_Code can't be null
                    Value = ""
                    _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Invalid Procedure Code'")
                Case Is = "MED_PLAN"
                    cmbPlan.SelectedValue = ""
                    Value = DBNull.Value
                Case Is = "MODIFIER_SW"
                    _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Invalid Modifier")

                    Value = 0
                    Dim QueryModDelete = _ClaimDS.MEDMOD.AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted)

                    For Each DR As DataRow In QueryModDelete.ToList()
                        DR.Delete()
                    Next

                Case Is = "DIAGNOSIS", "DIAGNOSES"

                Case Is = "PLACE_OF_SERV", "PLACE_OF_SERV_DESC"

                    _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Invalid Place of Service")
                    _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Possible Case Management")
                    Value = DBNull.Value

                Case Is = "BILL_TYPE", "BILL_TYPE_DESC"
                    _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Invalid Bill Type")
                    Value = DBNull.Value

                Case Is = "OCC_FROM_DATE"
                    _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Was Received 1 Year After DOS")

                Case Is = "OCC_TO_DATE"

                Case Is = "PRICED_AMT", "PAID_AMT"

                    _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Paid Is More Than Priced")

                    If fieldMapping = "PAID_AMT" Then
                        _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Paid Is 0 and a Reason is Required")
                    End If

                Case Is = "CHRG_AMT", "OTH_INS_AMT"
                Case Is = "DAYS_UNITS"
                Case Is = "REASON_SW"
                    'Reason_SW can't be null
                    Value = 0

                    _ClaimAlertManager.DeleteAlertRowsByCategory("Reasons")
                    _ClaimAlertManager.DeleteAllAlertRowsLikeMessage(": Paid Is 0 and a Reason is Required")

                    ReasonDV = New DataView(_ClaimDS.REASON, "", "Line_NBR", DataViewRowState.CurrentRows)
                    Do Until ReasonDV.Count = 0
                        ReasonDV(0).Row.Delete()
                    Loop

                Case Is = "Accumulators"

                    If MsgBox("Are you sure you want to Delete ALL Accumulators for ALL Lines", CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo, MsgBoxStyle), "Confirm Delete") = DialogResult.Yes Then
                        ClearAllDetailLinesAccumulators(_DetailAccumulatorsDT)
                    End If

                Case Is = "NDC"
                    Value = DBNull.Value
                Case Else
                    MessageBox.Show("Field " & HeaderText & " is Not Clearable", "Cannot Clear", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Try
            End Select

            For Each DR As DataRow In _ClaimDS.MEDDTL.Rows
                If DR("STATUS").ToString <> "MERGED" Then
                    DR(fieldMapping) = Value
                    Select Case fieldMapping
                        Case Is = "PROC_CODE"
                            DR("PROC_CODE_DESC") = "***INVALID PROCEDURE CODE***"
                            _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Invalid Procedure Code", DR("LINE_NBR").ToString, "Detail", 30})
                        Case Is = "MED_PLAN"
                        Case Is = "MODIFIER_SW"
                            DR("MODIFIER") = DBNull.Value
                            DR("MODIFIERS") = DBNull.Value
                        Case Is = "DIAGNOSIS", "DIAGNOSES"
                            ClearDiagnosisLine(CShort(DR("LINE_NBR")))
                        Case Is = "PLACE_OF_SERV", "PLACE_OF_SERV_DESC"
                            DR("PLACE_OF_SERV_DESC") = DBNull.Value
                        Case Is = "BILL_TYPE", "BILL_TYPE_DESC"
                            DR("BILL_TYPE_DESC") = DBNull.Value
                        Case Is = "OCC_FROM_DATE"
                            _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Missing From Date", DR("LINE_NBR").ToString, "Detail", 20})

                        Case Is = "OCC_TO_DATE"
                        Case Is = "PRICED_AMT", "PAID_AMT"
                            If fieldMapping = "PAID_AMT" Then
                                If Not CBool(DR("REASON_SW")) Then
                                    _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required", DR("LINE_NBR").ToString, "Detail", 30})
                                End If
                            End If
                        Case Is = "CHRG_AMT", "OTH_INS_AMT"
                        Case Is = "DAYS_UNITS"
                        Case Is = "REASON_SW"
                            If IsDBNull(DR("PAID_AMT")) OrElse CDec(DR("PAID_AMT")) = 0 Then
                                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required", DR("LINE_NBR").ToString, "Detail", 30})
                            End If
                        Case Is = "Accumulators"
                        Case Is = "NDC"
                    End Select
                End If
            Next

            If fieldMapping = "CHRG_AMT" Then
                SumCharges()
            End If

            If fieldMapping = "PRICED_AMT" Then
                SumPriced()
            End If

            If fieldMapping = "PAID_AMT" Then
                SumPaid()
            End If

            If fieldMapping = "OTH_INS_AMT" Then
                SumOI()
            End If

            If fieldMapping = "CHRG_AMT" OrElse fieldMapping = "PRICED_AMT" Then
                SyncAllowed()
                SumAllowed()
            End If

            _MedDtlBS.EndEdit()

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub
#End Region

#Region "Load Combo Boxes"
    Private Sub LoadPlanCombo()

        Try
            _PlanDT = CMSDALFDBMD.RetrievePlans
            _PlanBS = New BindingSource
            _PlanBS.DataSource = _PlanDT

            cmbPlan.DataSource = _PlanBS
            cmbPlan.DisplayMember = "PLAN_TYPE"
            cmbPlan.ValueMember = "PLAN_TYPE"

            'If cmbPlan.Items.Count > 0 Then Me.BindingContext(cmbPlan.DataSource).Position = 1 : Me.BindingContext(cmbPlan.DataSource).Position = 0
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadCOBCombo()
        Dim CobDT As DataTable
        Dim CobDV As DataView

        Try

            cmbCOB.SuspendLayout()

            CobDT = CMSDALFDBMD.RetrieveCOBs

            Me.cmbCOB.DataSource = Nothing
            Me.cmbCOB.Items.Clear()

            CobDV = New DataView(CobDT, "", "COB_VALUE", DataViewRowState.CurrentRows)

            Me.cmbCOB.DataSource = CobDV
            Me.cmbCOB.DisplayMember = "COB_DROPDOWN_TEXT"
            Me.cmbCOB.ValueMember = "COB_VALUE"

            cmbCOB.ResumeLayout()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadPPOCombo()


        Dim PPODT As DataTable
        Dim PPODV As DataView

        Try
            'RemoveHandler PPOComboBox.SelectedIndexChanged, AddressOf ComboBox_SelectedIndexChanged

            PPODT = CMSDALFDBMD.RetrievePPOs

            Me.cmbPricingNetwork.DataSource = Nothing
            Me.cmbPricingNetwork.Items.Clear()

            PPODV = New DataView(PPODT, "", "PPO_VALUE", DataViewRowState.CurrentRows)

            Me.cmbPricingNetwork.DataSource = PPODV
            Me.cmbPricingNetwork.DisplayMember = "PPO_VALUE"
            Me.cmbPricingNetwork.ValueMember = "PPO_VALUE"

        Catch ex As Exception
            Throw
        Finally
            'AddHandler PPOComboBox.SelectedIndexChanged, AddressOf ComboBox_SelectedIndexChanged
        End Try
    End Sub

    Private Sub LoadPayeeCombo()
        Dim PayeesDT As DataTable
        Dim PayeesDV As DataView

        Try
            'RemoveHandler PayeeComboBox.SelectedIndexChanged, AddressOf ComboBox_SelectedIndexChanged

            PayeesDT = CMSDALFDBMD.RetrievePayees

            Me.cmbPayee.DataSource = Nothing
            Me.cmbPayee.Items.Clear()

            PayeesDV = New DataView(PayeesDT, "", "PAYEE_VALUE", DataViewRowState.CurrentRows)

            Me.cmbPayee.DataSource = payeesDV
            Me.cmbPayee.DisplayMember = "PAYEE_VALUE"
            Me.cmbPayee.ValueMember = "PAYEE_VALUE"

        Catch ex As Exception
            Throw
        Finally
            'AddHandler PPOComboBox.SelectedIndexChanged, AddressOf ComboBox_SelectedIndexChanged
        End Try
    End Sub

    Private Sub LoadDocTypes()

        Dim DocTypesDT As DataTable

        Try
            DocTypesDT = CMSDALFDBMD.RetrieveActiveDocTypes

            Me.DocTypeComboBox.DataSource = Nothing
            Me.DocTypeComboBox.Items.Clear()

            Me.DocTypeComboBox.DataSource = DocTypesDT
            Me.DocTypeComboBox.DisplayMember = "DOC_TYPE"
            Me.DocTypeComboBox.ValueMember = "DOC_TYPE"

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadProceduresValues()
        Try
            _ProcedureCodesWorkerThread = New Thread(AddressOf GetProcedureCodes) With {
                .IsBackground = True
            }
            _ProcedureCodesWorkerThread.Start()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadDiagnosisValues()
        Try
            _DiagnosisCodesWorkerThread = New Thread(AddressOf GetDiagnosisCodes) With {
                .IsBackground = True
            }
            _DiagnosisCodesWorkerThread.Start()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub GetProcedureCodes()
        Try

            _ProcedureValuesDT = CMSDALFDBMD.RetrieveProcedureValuesAsOfEffectiveDate(UFCWGeneral.NowDate).Tables(0)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub GetDiagnosisCodes()
        Try

            _DiagnosisValuesDT = CMSDALFDBMD.RetrieveDiagnosisValuesEffectiveAsOf(UFCWGeneral.NowDate).Tables(0)

        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

#Region "Load Item Data"
    Private Sub RollbackClaimAccumulators()
        Try

            If _ClaimMemberAccumulatorManager Is Nothing Then
                _ClaimMemberAccumulatorManager = New MemberAccumulatorManager(CShort(_ClaimDr("RELATION_ID")), CInt(_ClaimDr("FAMILY_ID")))
            End If

            _ClaimMemberAccumulatorManager.RollbackAll(_ClaimID, _DomainUser.ToUpper)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadClaim()

        Dim ValidPatient As Boolean = False
        Dim docs() As Object
        ReDim docs(1)
        Try

            ClearDataBindings(ClaimTabPage)

            _ClaimAlertManager = New AlertManagerCollection With {
                .SuspendLayout = True
                     }

            AddHandler _ClaimAlertManager.AlertsChanged, AddressOf LoadAlerts
            AlertsImageListBox.Items.Clear()

            GetCompleteClaim()

            AddHandler _ClaimDS.CLAIM_MASTER.ColumnChanging, AddressOf CLAIM_MASTER_ColumnChanging

            AddHandler _ClaimDS.MEDHDR.ColumnChanging, AddressOf MEDHDR_ColumnChanging
            AddHandler _ClaimDS.MEDHDR.RowChanging, AddressOf MEDHDR_RowChanging
            AddHandler _ClaimDS.MEDHDR.RowChanged, AddressOf MEDHDR_RowChanged

            AddHandler _ClaimDS.MEDDTL.ColumnChanging, AddressOf MEDDTL_ColumnChanging
            AddHandler _ClaimDS.MEDDTL.RowChanging, AddressOf MEDDTL_RowChanging

            AddHandler _ClaimDS.MEDMOD.RowChanging, AddressOf MEDMOD_RowChanging
            AddHandler _ClaimDS.MEDMOD.RowDeleting, AddressOf MEDMOD_RowChanging

            LoadClaimMaster()

            If Not IsDBNull(_ClaimDr("DOCID")) Then
                docs(0) = _ClaimDr("DOCID")
                Using FNDisplay As New Display
                    FNDisplay.Display(docs)
                End Using

                If _Mode.ToUpper <> "AUDIT" Then
                    Me.Text = "Process Claim - " & Format(_ClaimID, "00000000") & " [IDM - " & _ClaimDr("DOCID").ToString & "]"
                Else
                    Me.Text = "Audit Claim - " & Format(_ClaimID, "00000000") & " [IDM - " & _ClaimDr("DOCID").ToString & "]"
                End If

            End If

            LoadClaimHeader()
            LoadClaimDetail()

            LoadHeaderDataBindings()
            LoadDetailLineDataBindings()

            ' AddHandler _MedDtlBS.CurrentChanged, AddressOf ClaimDataSetMeddtlBS_CurrentChanged

            If Not CStr(_ClaimDr("DOC_TYPE")).ToUpper.StartsWith("UTL_") Then
                LoadDuplicates(False) 'start thread to run synchronously
                LoadAssociated(False) 'start thread to run synchronously
            Else
                If CBool(_ClaimDr("ARCHIVE_SW")) Then
                    _UTLArchive = True
                End If
                CompleteToolBarButton.Enabled = False
            End If

            If _RollbackAccumulators Then
                RollbackClaimAccumulators()
                _RollbackAccumulators = False
            End If

            If _Mode.ToUpper = "AUDIT" Then
                AuditButton.Visible = True
                LoadEligibility(True)
                ProcessDenyRules(True)
                LoadCommittedAccumulators()
            Else
                LoadEligibility(False)
                ProcessDenyRules(False)
                ValidPatient = ValidatePatient()
                If ValidPatient Then
                    Using WC As New GlobalCursor

                        AccumulatorsDataGrid.DataSource = Nothing
                        _MedDtlBS.RaiseListChangedEvents = False
                        ClaimProcessor.LoadDetailLineAccumulators(AddressOf WorkStatusMessage, AddressOf SelectAccidentUI, AddressOf AccumulatorCheckIfOverrideNeededUI, _HighestEntryID, _ClaimBinder, _ClaimMemberAccumulatorManager, CType(_ClaimDS, DataSet), _DetailAccumulatorsDT, _AccumulatorsDT, _ClaimAlertManager)
                        _MedDtlBS.RaiseListChangedEvents = True
                    End Using
                End If
                If Not _HasBeenAudited Then
                    If CStr(_ClaimDr("DOC_TYPE")).ToUpper.Contains("HOSPITAL") Then
                        If _AutoMergeSummaryLines AndAlso _MedHdrDr IsNot Nothing _
                                    AndAlso _MedHdrDr("PRICED_BY").ToString.Contains("JAA") AndAlso _MedHdrDr("PRICED_BY").ToString.Contains(" (S)") _
                                    AndAlso _MedDtlDR IsNot Nothing AndAlso _MedDtlDR("STATUS").ToString.Trim <> "MERGED" Then
                            ConsolidateDetailLines()
                        End If
                        'PreProcessHospital()
                        'LineProcessor.PreProcessHospital(_ClaimMemberAccumulatorManager, CType(claimDS, DataSet))
                    End If
                End If
            End If

            LoadAccumulators()

            ClaimProcessor.LoadReasonsAlerts(CType(_ClaimDS, DataSet), _ClaimAlertManager)
            If _ClaimDS.ANNOTATIONS.Rows.Count > 0 Then Me.AnnotateButton.ForeColor = Color.Blue

            LoadFreeText()

            ClaimProcessor.LoadDetailLineAlerts(CType(_ClaimDS, DataSet), _ClaimAlertManager)
            ClaimProcessor.LoadDiagnosisAlerts(CType(_ClaimDS, DataSet), _ClaimAlertManager, _PlansIncludingPreventativeRules)
            ClaimProcessor.LoadPatientAlerts(CType(_ClaimDS, DataSet), _ClaimAlertManager)

            LoadAudit()
            LoadDocHistory()

            '  If DetailLinesDataGrid.GetGridRowCount > 0 Then LoadDetailLineRow(1)

            SetUIElements()

            LoadRoutingHistory()

            DisableEnableButtons()

            SumCharges()
            SumPriced()
            SumPaid()
            SyncAllowed()
            SumAllowed()
            SumOI()

            'AddHandler _ClaimDS.CLAIM_MASTER.ColumnChanging, AddressOf CLAIM_MASTER_ColumnChanging

            'AddHandler _ClaimDS.MEDHDR.ColumnChanging, AddressOf MEDHDR_ColumnChanging
            'AddHandler _ClaimDS.MEDHDR.RowChanging, AddressOf MEDHDR_RowChanging
            'AddHandler _ClaimDS.MEDHDR.RowChanged, AddressOf MEDHDR_RowChanged

            'AddHandler _ClaimDS.MEDDTL.ColumnChanging, AddressOf MEDDTL_ColumnChanging
            'AddHandler _ClaimDS.MEDDTL.ColumnChanged, AddressOf MEDDTL_ColumnChanged

            'AddHandler _ClaimDS.MEDDTL.RowChanging, AddressOf MEDDTL_RowChanging
            'AddHandler _ClaimDS.MEDDTL.RowChanged, AddressOf MEDDTL_RowChanged

            'AddHandler _ClaimDS.MEDMOD.RowChanging, AddressOf MEDMOD_RowChanging
            'AddHandler _ClaimDS.MEDMOD.RowDeleting, AddressOf MEDMOD_RowChanging

        Catch ex As Exception
            Throw
        Finally
            _ClaimAlertManager.SuspendLayout = False
        End Try
    End Sub

    Private Sub GetCompleteClaim()

        Dim DBTableNames() As String = {"CLAIM_MASTER", "MEDHDR", "MEDDTL", "MEDDIAG", "MEDMOD", "REASON", "PROVIDER", "FUNDDUALCOVERAGE", "PARTICIPANT", "PATIENT", "ELIGIBILITY", "LASTCOB", "ANNOTATIONS", "FREE_TEXT", "REG_ALERTS", "CLAIMDOCHISTORY", "ROUTING_HISTORY", "MEDOTHER_INS", "MEDOTHER_INS_COUNT", "LIFE_EVENT_GAPS"}
        Dim PatientClaimCount As Integer = 0
        Try
            _ClaimDS = New ClaimDataset
            _ClaimDS = CType(CMSDALFDBMD.RetrieveCompleteClaim(_ClaimID, PatientClaimCount, DBTableNames, _ClaimDS, _DomainUser.ToUpper), ClaimDataset)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadClaimMaster()
        Try
            _ClaimMasterBS = New BindingSource With {
                .DataSource = _ClaimDS.Tables("CLAIM_MASTER")
            }
            If _ClaimMasterBS.Count > 0 Then
                _ClaimDr = DirectCast(_ClaimMasterBS.Current, DataRowView).Row
                If _ClaimDr IsNot Nothing Then
                    _PatientKey.FamilyID = If(IsDBNull(_ClaimDr("FAMILY_ID")), 0, CInt(_ClaimDr("FAMILY_ID")))
                    _PatientKey.ParticipantSSN = If(IsDBNull(_ClaimDr("PART_SSN")), 0, CInt(_ClaimDr("PART_SSN")))
                    _PatientKey.RelationID = UFCWGeneral.IsNullShortHandler(_ClaimDr("RELATION_ID"))
                    _PatientKey.PatientSSN = If(IsDBNull(_ClaimDr("PAT_SSN")), 0, CInt(_ClaimDr("PAT_SSN")))
                    _PatientKey.PatientFName = _ClaimDr("PAT_FNAME")
                    _PatientKey.PatientLName = _ClaimDr("PAT_LNAME")

                    If _ClaimDS.PATIENT IsNot Nothing AndAlso _ClaimDS.PATIENT.Rows.Count > 0 Then _PatientKey.PatientDOB = _ClaimDS.PATIENT.Rows(0)("BIRTH_DATE")

                    _PatientKey.SecuritySW = CBool(_ClaimDr("SECURITY_SW"))

                    Me.EmployeeItemLabel.Visible = CBool(_ClaimDr("SECURITY_SW"))

                    ClaimProcessor.LoadClaimAlerts(CType(_ClaimDS, DataSet), _ClaimAlertManager)
                End If
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub LoadClaimHeader()
        Try
            _MedHdrBS = New BindingSource
            _MedHdrBS.DataSource = _ClaimDS.Tables("MEDHDR")

            If _MedHdrBS IsNot Nothing AndAlso _MedHdrBS.Current IsNot Nothing Then
                _MedHdrDr = DirectCast(_MedHdrBS.Current, DataRowView).Row
                If _Mode.ToUpper <> "AUDIT" Then
                    If CBool(_MedHdrDr("OUT_OF_AREA_SW")) AndAlso _MedHdrDr("PRICED_BY").ToString.Contains("JAA") Then
                        _MedHdrDr("OUT_OF_AREA_SW") = 0
                        'JAA O/A handling
                        _ClaimAlertManager.AddAlertRow(New Object() {"Original Claim Identified as Out Of Area, OI has been reset for JAA review.", 0, "Header", 20})
                    End If

                    If CBool(_MedHdrDr("CHIRO_SW")) AndAlso CBool(_ClaimDr("DOC_TYPE").ToString.ToUpper.Contains("HOSPITAL")) Then
                        _MedHdrDr("CHIRO_SW") = 0
                        'Chiro
                        _ClaimAlertManager.AddAlertRow(New Object() {"Chiro flag reset due to incompatability with HOSPITAL document type.", 0, "Header", 20})
                    End If
                End If
            Else
                CreateMEDHDR()
            End If

            LoadHeaderDefaults()

            ClaimProcessor.LoadHeaderAlerts(CType(_ClaimDS, DataSet), _ClaimAlertManager)

            HighlightHeaderAlerts()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadHeaderDefaults()
        Try
            If _MedHdrBS IsNot Nothing AndAlso _MedHdrBS.Current IsNot Nothing Then
                _MedHdrDr = DirectCast(_MedHdrBS.Current, DataRowView).Row
            End If

            If _MedHdrDr IsNot Nothing Then
                _MedHdrDr.BeginEdit()
                'Pricing Network
                If IsDBNull(_MedHdrDr("PPO")) Then
                    If Not IsDBNull(_MedHdrDr("PRICED_BY")) Then
                        Select Case _MedHdrDr("PRICED_BY").ToString.ToUpper
                            Case "BLUE CROSS"
                                _MedHdrDr("PPO") = "BC"
                            Case "BLUE CROSS FFE", "BLUE CROSS FFE (S)"
                                If CInt(_MedHdrDr("NON_PAR_SW")) = 1 Then
                                    _MedHdrDr("PPO") = "NPFFE"
                                Else
                                    _MedHdrDr("PPO") = "BCFFE"
                                End If
                            Case "BLUE CROSS JAA", "BLUE CROSS JAA (S)", "BLUE CROSS 835", "BLUE CROSS 835 (S)", "BLUE CROSS JAA (I)"
                                If CInt(_MedHdrDr("NON_PAR_SW")) = 1 Then
                                    _MedHdrDr("PPO") = "NPJAA"
                                Else
                                    _MedHdrDr("PPO") = "BCJAA"
                                End If
                            Case "HMC/APS"
                                _MedHdrDr("PPO") = "HM"
                        End Select
                    Else
                        _MedHdrDr("PPO") = "BC"
                    End If
                End If

                'Payee
                If IsDBNull(_MedHdrDr("PAYEE")) Then
                    _MedHdrDr("PAYEE") = "1"
                End If

                'COB - coordination of benefits ?new system
                If IsDBNull(_MedHdrDr("COB")) Then
                    _MedHdrDr("COB") = 0
                End If
                _MedHdrBS.EndEdit()
            Else 'set defaults if no MEDHDR Row
                ''Set COB value as last know family value
                Dim CobVal As Integer = 0

                If _ClaimDS.LASTCOB.Rows.Count > 0 Then
                    CobVal = CInt(_ClaimDS.LASTCOB.Rows(0)("COB"))
                End If
                cmbPricingNetwork.Text = "BC"
                cmbPayee.Text = "1"
                cmbCOB.SelectedValue = CobVal
            End If


        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadClaimDetail()
        Try
            _MedDtlBS = New BindingSource With {
                .DataSource = _ClaimDS.Tables("MEDDTL"),
                .Sort = "LINE_NBR"
                     }

            DetailLinesDataGrid.DataSource = _MedDtlBS

            SetTableStyle(DetailLinesDataGrid, DetailLinesDataGridCustomContextMenu)

            If _MedDtlBS IsNot Nothing AndAlso _MedDtlBS.Count > 0 Then
                _MedDtlDR = DirectCast(_MedDtlBS.Current, DataRowView).Row
            End If


        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub HighlightHeaderAlerts()

        Try
            If _MedHdrDr IsNot Nothing Then
                'Paying Member
                If IsDBNull(_MedHdrDr("PAYEE")) = False AndAlso _MedHdrDr("PAYEE").ToString = "3" Then
                    PayeeLabel.ForeColor = Color.Blue
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadAuditAlerts(ByVal refreshAlertBox As Boolean)

        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Loads Alerts from the Audit Form
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/12/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim CheckBoxes As CheckBox()
        Dim Alert As String
        Try
            _ClaimAlertManager.DeleteAlertRowsByCategory("Audit")

            CheckBoxes = _AuditForm.Audits
            For Cnt As Integer = 0 To UBound(CheckBoxes, 1) - 1
                If CheckBoxes(Cnt) IsNot Nothing AndAlso CheckBoxes(Cnt).Checked Then
                    If CheckBoxes(Cnt).Text.ToUpper = "OTHER" Then
                        Alert = _AuditForm.OtherText
                    Else
                        Alert = CheckBoxes(Cnt).Text
                    End If
                    'If _Mode.ToUpper = "AUDIT" Then
                    _ClaimAlertManager.AddAlertRow(New Object() {Alert & " (Audit)", 0, "Audit", 30})
                    'Else
                    '    _ClaimAlertManager.AddAlertRow(New Object() {Alert & " (Audit)", 0, "Audit", 30})
                    'End If
                End If
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadAlerts()
        Dim Image As Image

        'Message
        'LineNumber
        'Category
        'Severity
        'Tag

        Try

            RemoveHandler _ClaimAlertManager.AlertsChanged, AddressOf LoadAlerts

            If AlertsImageListBox Is Nothing Then Return

            If CInt(_TraceParallel.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ") < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & " (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ") < " & New System.Diagnostics.StackTrace(True).GetFrame(2).GetMethod.ToString & " (" & New System.Diagnostics.StackTrace(True).GetFrame(2).GetFileLineNumber.ToString & ") < " & New System.Diagnostics.StackTrace(True).GetFrame(3).GetMethod.ToString & " (" & New System.Diagnostics.StackTrace(True).GetFrame(3).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)

            AlertsImageListBox.Items.Clear()

            AlertsImageListBox.SuspendLayout()

            Dim QueryAlert =
                From Alert In _ClaimAlertManager.AlertManagerDataTable.AsEnumerable()
                Where Alert.RowState <> DataRowState.Deleted
                Order By Alert.Field(Of Integer?)("SEVERITY") Descending, Alert.Field(Of Short?)("LineNumber"), Alert.Field(Of String)("Category")
                Select Alert

            If QueryAlert.Any Then
                For Each DR In QueryAlert
                    Image = Nothing
                    If IsDBNull(DR("Severity")) OrElse DR("Severity").ToString.Trim.Length = 0 Then
                        DR("Severity") = "0"
                    End If
                    Select Case CType(DR("Severity"), Integer)
                        Case Is > 20 'critical
                            Image = DIList.Images(0)
                        Case Is > 10 'warning
                            Image = DIList.Images(1)
                        Case Is >= 0 'information
                            Image = DIList.Images(3)
                    End Select

                    AlertsImageListBox.Items.Add(New ImageListBox.ImageListBoxItem(CStr(DR("Message")), Image, AssociatedControlToHighlight(CStr(DR("Message")))))

                Next

            End If

            AlertsImageListBox.ResumeLayout()

        Catch ex As Exception
            Throw
        Finally
            AddHandler _ClaimAlertManager.AlertsChanged, AddressOf LoadAlerts
        End Try
    End Sub
    Private Sub MEDDTL_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        Dim Changed As Boolean = False
        'Dim BS As BindingSource

        Try

            'BS = DirectCast(_MedDtlBS, BindingSource)

            'If BS Is Nothing OrElse BS.Current Is Nothing OrElse BS.Position < 0 Then Return

            '  Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If IsDBNull(e.Row(e.Column.ColumnName)) AndAlso Not IsDBNull(e.ProposedValue) Then
                Changed = True
            ElseIf Not IsDBNull(e.Row(e.Column.ColumnName)) AndAlso IsDBNull(e.ProposedValue) Then
                Changed = True
            ElseIf Not IsDBNull(e.Row(e.Column.ColumnName)) AndAlso Not IsDBNull(e.ProposedValue) AndAlso Not (e.Row(e.Column.ColumnName).Equals(e.ProposedValue)) Then
                Changed = True
            End If

#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": Changed -> " & Changed.ToString & " : " & e.Column.ColumnName & " " & If(IsDBNull(e.Row(e.Column.ColumnName)), "Null", e.Row(e.Column.ColumnName).ToString) & " -> " & e.ProposedValue.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

            If Changed Then
                Select Case e.Column.ColumnName.ToUpper
                    Case "OCC_FROM_DATE"
                        '  _ClaimAlertManager.AddAlertRow(New Object() {"Re-Calc Is Required", 0, "Header", 30})
                        If AllowReprice() Then
                            _ClaimAlertManager.AddAlertRow(New Object() {"Re-Price Is Required", 0, "Header", 30})
                        End If

                    Case "PROC_CODE", "DAYS_UNITS", "CHRG_AMT"
                        If AllowReprice() Then
                            _ClaimAlertManager.AddAlertRow(New Object() {"Re-Price Is Required", 0, "Header", 30})
                        End If
                End Select
            End If

            '  Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub MEDDTL_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs)
        'Dim DGRow As DataRow

        Try

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Position < 0 Then Return

            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " IO:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            'DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub MEDDTL_RowChanging(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": Line -> " & e.Row("LINE_NBR").ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        'Dim DGRow As DataRow

        'Try

        '    If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Position < 0 Then Return

        '    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " IO:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        '    DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

        '    If (_MedDtlBS.Position + 1 < _MedDtlBS.Count) Then
        '        _MedDtlBS.MoveNext()
        '    End If
        'Catch ex As Exception
        '    Throw
        'End Try

    End Sub

    Private Sub MEDDTL_RowChanged(sender As Object, e As DataRowChangeEventArgs)

        Dim DGRow As DataRow

        Try

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Position < 0 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

            If DGRow.RowState <> DataRowState.Unchanged Then
                ' ReEvaluateClaimAfterLineChanged(DGRow)
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub LoadAccumulators()
        Dim DV As DataView

        Try

            If _AccumulatorsDT IsNot Nothing Then
                DV = New DataView(_AccumulatorsDT, "", "PRIORITY", DataViewRowState.CurrentRows)
                AccumulatorsDataGrid.DataSource = DV
                AccumulatorsDataGrid.SetTableStyle("WorkAccumulatorsDataGrid.xml")
                '   SetAccumulatorsTableStyle()
            End If

            If ManualAccumulatorValues IsNot Nothing Then
                ManualAccumulatorValues.IsInEditMode = False
                ManualAccumulatorValues.DisplayManualAccumulators(CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")))
            End If

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceParallel.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If
        End Try
    End Sub
    'Private Sub SetAccumulatorsTableStyle()
    '    ' -----------------------------------------------------------------------------
    '    ' <summary>
    '    ' builds the tablestyle for accumulators
    '    ' </summary>
    '    ' <remarks>
    '    ' </remarks>
    '    ' <history>
    '    ' 	[nick snyder]	8/16/2006	Created
    '    ' </history>
    '    ' -----------------------------------------------------------------------------
    '    Dim DGTS As DataGridTableStyle
    '    Dim TextCol As DataGridHighlightTextBoxColumn

    '    Try

    '        _CurrencyManager = CType(Me.BindingContext(_AccumulatorsDT), CurrencyManager)

    '        DGTS = New DataGridTableStyle(_CurrencyManager)
    '        DGTS.MappingName = _AccumulatorsDT.TableName
    '        DGTS.GridColumnStyles.Clear()
    '        DGTS.GridLineStyle = DataGridLineStyle.Solid

    '        TextCol = New DataGridHighlightTextBoxColumn
    '        TextCol.MappingName = "ACCUM_NAME"
    '        TextCol.HeaderText = "Accumulator Name"
    '        TextCol.Width = CInt(GetSetting(_APPKEY, "MedicalWork\Accumulators\ColumnSettings", "Col " & TextCol.MappingName, "100"))
    '        TextCol.NullText = ""
    '        DGTS.GridColumnStyles.Add(TextCol)

    '        TextCol = New DataGridHighlightTextBoxColumn
    '        TextCol.MappingName = "ORIGINAL_VALUE"
    '        TextCol.HeaderText = "Orig. Value"
    '        TextCol.Format = "0.00"
    '        TextCol.Width = CInt(GetSetting(_APPKEY, "MedicalWork\Accumulators\ColumnSettings", "Col " & TextCol.MappingName, "100"))
    '        TextCol.NullText = ""
    '        DGTS.GridColumnStyles.Add(TextCol)

    '        TextCol = New DataGridHighlightTextBoxColumn
    '        TextCol.MappingName = "PROPOSED_VALUE"
    '        TextCol.HeaderText = "After Claim Value"
    '        TextCol.Format = "0.00"
    '        TextCol.Width = CInt(GetSetting(_APPKEY, "MedicalWork\Accumulators\ColumnSettings", "Col " & TextCol.MappingName, "100"))
    '        TextCol.NullText = ""
    '        DGTS.GridColumnStyles.Add(TextCol)

    '        TextCol = New DataGridHighlightTextBoxColumn
    '        TextCol.MappingName = "YEAR"
    '        TextCol.HeaderText = "Year"
    '        TextCol.Format = "yyyy"
    '        TextCol.Width = CInt(GetSetting(_APPKEY, "MedicalWork\Accumulators\ColumnSettings", "Col " & TextCol.MappingName, "100"))
    '        TextCol.NullText = ""

    '        DGTS.GridColumnStyles.Add(TextCol)

    '        WorkAccumulatorsDataGrid.TableStyles.Clear()
    '        WorkAccumulatorsDataGrid.TableStyles.Add(DGTS)

    '    Catch ex As Exception
    '        Throw
    '    Finally
    '        _CurrencyManager = Nothing
    '        DGTS = Nothing
    '    End Try

    'End Sub
    Private Sub ManualAccumulatorValues_ManualAccumulatorsModified()

        Try
            ManualAccumulatorValues.DisplayManualAccumulators(CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")), UFCWGeneral.NowDate)
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub LoadRoutingHistory()
        Try
            _RouteDgBS = New BindingSource With {
                .DataSource = _ClaimDS.ROUTING_HISTORY
            }
            RouteDataGrid.SuspendLayout()
            RouteDataGrid.DataSource = _RouteDgBS
            RouteDataGrid.ResumeLayout()
            RouteDataGrid.SetTableStyle()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ProcessDenyRules(ByVal alertsOnly As Boolean)
        Dim ProvDR As DataRow
        Dim DGRow As DataRow
        Dim canDeny As Boolean = False

        Try

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Position < -1 Then Return


#If TRACE Then
                If CInt(_TraceParallel.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If

            If _MedDtlBS IsNot Nothing AndAlso _MedDtlBS.Current IsNot Nothing Then
                DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row
            End If

            If DGRow Is Nothing OrElse _MedHdrDr Is Nothing Then Return

            Using WC As New GlobalCursor

                Select Case _ClaimDr("DOC_TYPE").ToString.ToUpper
                          'Podiatry
                    Case "PODIATRY"
                        _MedHdrDr("PPO") = "IO"
                        If Not IsDBNull(_MedHdrDr("PROV_TIN")) AndAlso Not IsDBNull(_MedHdrDr("PROV_ID")) Then
                            ProvDR = _ClaimDS.PROVIDER.Rows(0)
                            _LastPPOCProv = CType(_MedHdrDr("PROV_ID"), Integer?)
                            If ProvDR IsNot Nothing AndAlso Not CBool(ProvDR("PPOC_ELIGIBLE_SW")) Then
                                'add an alert
                                _ClaimAlertManager.AddAlertRow(New Object() {"Non-PPOC Provider - Claim Denied", 0, "Header", 20})
                                If Not alertsOnly Then
                                    'Deny all lines
                                    canDeny = True
                                    'set as non-par
                                    _MedHdrDr("NON_PAR_SW") = True
                                End If
                            End If
                        End If
                        _MedHdrDr.EndEdit()
                        _MedHdrBS.EndEdit()
                                'Mental Health
                    Case "MENTAL HEALTH"
                        If Not CBool(_MedHdrDr("AUTHORIZED_SW")) Then
                            If Not alertsOnly Then
                                'Deny All Lines
                                canDeny = True
                            End If
                        End If
                End Select

                If canDeny Then
                    'Deny all lines
                    ' cmbStatus.SelectedText = "DENY"
                    _MedDtlBS.SuspendBinding()
                    Dim DV As DataView = New DataView(_ClaimDS.MEDDTL, "STATUS <> 'DENY'", "STATUS", DataViewRowState.CurrentRows)
                    For Each DVR As DataRowView In DV
                        DVR.Row("STATUS") = "DENY"
                    Next
                    _MedDtlBS.ResumeBinding()
                    _MedDtlBS.ResetBindings(False)
                End If

            End Using


        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
                If CInt(_TraceParallel.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If
        End Try

    End Sub

    Private Sub LoadCommittedAccumulators()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Loads Accumulators already committed for this claim
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/12/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim DocClass As String
        Dim RuleSetTypeName As String = "General"
        Dim RelevantDate As Date = UFCWGeneral.NowDate

        Try

            DocClass = CStr(_ClaimDr("DOC_CLASS"))

            _RuleSetType = PlanController.GetRulesetTypeID(RuleSetTypeName)
            RuleSetTypeName = GetRuleSetTypeName(CStr(_ClaimDr("DOC_TYPE")).ToUpper.Replace("+", "Plus").Replace("EMPLOYEE CLAIMS - ", "").Replace(" ", "_"))

            If RuleSetTypeName <> "" Then
                _RuleSetType = PlanController.GetRulesetTypeID(RuleSetTypeName)
            End If

            If _MedHdrDr IsNot Nothing Then
                If CBool(_MedHdrDr("CHIRO_SW")) Then
                    _RuleSetType = PlanController.GetRulesetTypeID("Chiropractic")
                End If
                If Not IsDBNull(_MedHdrDr("OCC_FROM_DATE")) AndAlso IsDate(_MedHdrDr("OCC_FROM_DATE")) Then
                    RelevantDate = CDate(_MedHdrDr("OCC_FROM_DATE"))
                    If Not IsDBNull(_MedHdrDr("OCC_TO_DATE")) AndAlso IsDate(_MedHdrDr("OCC_TO_DATE")) Then 'TO DATE could theoretically be a later year
                        RelevantDate = CDate(_MedHdrDr("OCC_TO_DATE"))
                    End If
                End If
            End If

            If _ClaimDr("DOC_TYPE").ToString.ToUpper = "VISION" Then
                _RuleSetType = PlanController.GetRulesetTypeID("Vision")
            End If

            If _ClaimMemberAccumulatorManager Is Nothing Then
                _ClaimMemberAccumulatorManager = New MemberAccumulatorManager(CShort(_ClaimDr("RELATION_ID")), CInt(_ClaimDr("FAMILY_ID")))
            End If

            _ClaimBinder = CType(BinderFactory.CreateBinder(CInt(_ClaimDr("CLAIM_ID")), DocClass, _RuleSetType), MedicalBinder)
            _ClaimBinder.BinderAccumulatorManager = _ClaimMemberAccumulatorManager

            _ClaimMemberAccumulatorManager.RefreshAccumulatorSummariesForMember()

            'no binder items
            'load accum tables
            _DetailAccumulatorsDT = _ClaimBinder.BinderAccumulatorManager.GetAccumulatorEntryValues(False, _ClaimID)

            _AccumulatorsDT = _ClaimBinder.GetAccumulatorSummaryCommitted(RelevantDate)

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub LoadUserAccumulators()
        Dim DateOfService As DateTime

        Try

            If Not IsDBNull(_ClaimDr("DATE_OF_SERVICE")) Then
                DateOfService = CDate(_ClaimDr("DATE_OF_SERVICE"))
            Else
                DateOfService = UFCWGeneral.NowDate
            End If

            If _ClaimMemberAccumulatorManager Is Nothing Then
                _ClaimMemberAccumulatorManager = New MemberAccumulatorManager(CShort(_ClaimDr("RELATION_ID")), CInt(_ClaimDr("FAMILY_ID")))
                _ClaimMemberAccumulatorManager.RefreshAccumulatorSummariesForMember()
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub BuildDetailLineAccumulators()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' builds the accumulator datatable to store accumulators
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try
            If _DetailAccumulatorsDT IsNot Nothing Then
                _DetailAccumulatorsDT.Clear()
                _DetailAccumulatorsDT.Dispose()
                _DetailAccumulatorsDT = Nothing
            End If

            _DetailAccumulatorsDT = CMSDALCommon.CreateAccumulatorValuesDT

        Catch ex As Exception
            Throw
        End Try
    End Sub


    'Private Sub LoadDiagnosis()
    '    ' -----------------------------------------------------------------------------
    '    ' <summary>
    '    ' loads claim diagnosis from database
    '    ' </summary>
    '    ' <remarks>
    '    ' </remarks>
    '    ' <history>
    '    ' 	[nick snyder]	8/16/2006	Created
    '    ' </history>
    '    ' -----------------------------------------------------------------------------

    '    Try

    '        If _ClaimDS.MEDDIAG.Count < 1 Then
    '            _ClaimDS.MEDDIAG.Rows.Clear()
    '            _ClaimDS = CType(CMSDALFDBMD.RetrieveClaimDiagnosis(_ClaimID, _ClaimDS, _Transaction), ClaimDataset)
    '        End If

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    'Private Sub LoadModifiers()
    '    ' -----------------------------------------------------------------------------
    '    ' <summary>
    '    ' loads claim modifiers from database
    '    ' </summary>
    '    ' <remarks>
    '    ' </remarks>
    '    ' <history>
    '    ' 	[nick snyder]	8/16/2006	Created
    '    ' </history>
    '    ' -----------------------------------------------------------------------------

    '    Try
    '        _ClaimDS.MEDMOD.Rows.Clear()

    '        _ClaimDS = CType(CMSDALFDBMD.RetrieveClaimModifier(_ClaimID, Nothing, _ClaimDS, _Transaction), ClaimDataset)

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    Private Function ValidatePatient() As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' checks if patient is valid
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/22/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim ParticipantDR As DataRow
        Dim PatientDR As DataRow

        Try

            If _ClaimDS.PARTICIPANT IsNot Nothing AndAlso _ClaimDS.PARTICIPANT.Rows.Count > 0 Then
                ParticipantDR = _ClaimDS.PARTICIPANT.Rows(0)
            End If

            If ParticipantDR Is Nothing Then Return False


            If _ClaimDS.PATIENT IsNot Nothing AndAlso _ClaimDS.PATIENT.Rows.Count > 0 Then PatientDR = _ClaimDS.PATIENT.Rows(0)

            ''To Set the values Gender and DOB values in the form

            If PatientDR Is Nothing OrElse Not IsDate(_MedHdrDr("PAT_DOB")) Then
                Return False
            Else
                If Not _MedHdrDr("PAT_DOB").Equals(PatientDR("BIRTH_DATE")) Then _MedHdrDr("PAT_DOB") = PatientDR("BIRTH_DATE")

                If Not _MedHdrDr("PAT_SEX").Equals(PatientDR("GENDER")) Then _MedHdrDr("PAT_SEX") = PatientDR("GENDER")

                _MedHdrBS.EndEdit()
                Return True
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Function

    'Private Function LoadPatientAlerts() As Boolean
    '    ' -----------------------------------------------------------------------------
    '    ' <summary>
    '    ' validates if patient is valid
    '    ' </summary>
    '    ' <remarks>
    '    ' </remarks>
    '    ' <history>
    '    ' 	[Nick Snyder]	1/22/2007	Created
    '    ' </history>
    '    ' -----------------------------------------------------------------------------

    '    Dim participantRow As DataRow
    '    Dim patientRow As DataRow
    '    Dim valid As Boolean = True

    '    Try

    '        If _ClaimDataSet.PARTICIPANT.Rows.Count > 0 Then
    '            participantRow = _ClaimDataSet.PARTICIPANT.Rows(0)
    '        End If

    '        If participantRow Is Nothing Then

    '            'add alert
    '            _ClaimAlertManager.AddAlertRow(New Object() {"Invalid Participant", 0, "Header", 30})

    '            valid = False
    '        End If

    '        If _ClaimDataSet.PATIENT.Rows.Count > 0 Then patientRow = _ClaimDataSet.PATIENT.Rows(0)

    '        ''To Set the values Gender and DOB values in the form
    '        If Not patientRow Is Nothing Then
    '            If IsDBNull(patientRow("BIRTH_DATE")) = False Then
    '                PatDOBTextBox.Text = CType(patientRow("BIRTH_DATE"), String).Replace("/", "-")
    '            End If
    '            If Not _ClaimDataSet.MEDHDR.Rows(0)("PAT_DOB").Equals(patientRow("BIRTH_DATE")) Then _ClaimDataSet.MEDHDR.Rows(0)("PAT_DOB") = patientRow("BIRTH_DATE")

    '            If IsDBNull(patientRow("GENDER")) = False Then
    '                PatSexTextBox.Text = CStr(patientRow("GENDER"))
    '            End If
    '            If Not _ClaimDataSet.MEDHDR.Rows(0)("PAT_SEX").Equals(patientRow("GENDER")) Then _ClaimDataSet.MEDHDR.Rows(0)("PAT_SEX") = patientRow("GENDER")
    '        End If

    '        If IsNothing(patientRow) OrElse Not IsDate(_ClaimDataSet.MEDHDR.Rows(0)("PAT_DOB")) Then

    '            _ClaimAlertManager.AddAlertRow(New Object() {"Invalid Patient", 0, "Header", 30})

    '            valid = False
    '        End If

    '        Return valid

    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (rethrow) Then
    '            Throw
    '        Else
    '            MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End If
    '    End Try
    'End Function

    Private Sub LoadEligibility(ByVal alertsOnly As Boolean)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Loads the Eligibility Control
        ' also adds an alert if not eligible
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/12/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            If _ClaimDS.ELIGIBILITY.Rows.Count > 0 Then

                If _ClaimDS.Tables.Contains("LIFE_EVENT_GAPS") AndAlso _ClaimDS.Tables("LIFE_EVENT_GAPS").Rows.Count > 0 Then

                    Dim GapDaysQuery = _ClaimDS.Tables("LIFE_EVENT_GAPS").AsEnumerable
                    Dim GapPeriodsQuery = From period In GapDaysQuery
                                          Group period By period!GAPMONTH, period!GAPYEAR
                                          Into Periods = Group

                    For Each period In GapPeriodsQuery
                        Dim DR = _ClaimDS.ELIGIBILITY.NewELIGIBILITYRow
                        DR.ItemArray = _ClaimDS.ELIGIBILITY.Rows(0).ItemArray
                        DR.ELIG_PERIOD = CDate(period.GAPMONTH.ToString & "/1/" & period.GAPYEAR.ToString)
                        DR.MED_ELIG_SW = False
                        DR.DEN_ELIG_SW = False
                        DR.STUDENT_SW = False
                        DR.DISABLE_SW = False
                        DR.MEDICAL_PLAN = 0
                        DR.PLAN_DESCRIPTION = ""
                        DR.PLAN_TYPE = ""
                        DR.STATUS = "No Coverage"
                        _ClaimDS.ELIGIBILITY.AddELIGIBILITYRow(DR)

                    Next

                End If

                EligControl.LoadEligibility(_ClaimDS.ELIGIBILITY, _ClaimDr("DOC_TYPE").ToString)

                LoadDetailLineEligibility(alertsOnly)

                SyncAllowed()
                SumAllowed()
                SumOI()
                SumPaid()

            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadParticipantAddress()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' loads the participant address control
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/26/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            PartParticipantControl.FamilyID = CInt(_ClaimDr("FAMILY_ID"))
            PartParticipantControl.RelationID = 0

            PartParticipantControl.LoadPARTICIPANT()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadPatientAddress()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' loads the patient address control
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/26/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            PatParticipantControl.FamilyID = CInt(_ClaimDr("FAMILY_ID"))
            PatParticipantControl.RelationID = CShort(_ClaimDr("RELATION_ID"))

            PatParticipantControl.LoadPARTICIPANT()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadLetters()
        Try

            If _PatientAddressesDS Is Nothing OrElse PatParticipantControl.FamilyID <> CInt(_ClaimDr("FAMILY_ID")) OrElse PatParticipantControl.RelationID <> CShort(_ClaimDr("RELATION_ID")) Then
                _PatientAddressesDS = New DataSet
                _PatientAddressesDS = CMSDALFDBMD.RetrievePatientParticipantAddresses(CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")), _PatientAddressesDS)
            End If

            If _PatientAddressesDS.Tables("PARTICIPANT_ADDRESS").Rows.Count > 0 Then
                Me.lblParticipantAddress.Text = UFCWGeneral.IsNullStringHandler(_PatientAddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("ADDRESS"), "")
                Me.lblPatientAddress.Text = UFCWGeneral.IsNullStringHandler(_PatientAddressesDS.Tables("PARTICIPANT_ADDRESS").Rows(0)("ADDRESS"), "")
            End If

            If _PatientAddressesDS.Tables("PATIENT_ADDRESS").Rows.Count > 0 Then
                Me.lblPatientAddress.Text = UFCWGeneral.IsNullStringHandler(_PatientAddressesDS.Tables("PATIENT_ADDRESS").Rows(0)("ADDRESS"), "")
            End If

            Me.lblParticipantFullName.Text = UFCWGeneral.IsNullStringHandler(_ClaimDr("PART_FNAME"), "") & " " & UFCWGeneral.IsNullStringHandler(_ClaimDr("PART_INT"), "") & " " & UFCWGeneral.IsNullStringHandler(_ClaimDr("PART_LNAME"), "")
            Me.lblPatientFullName.Text = UFCWGeneral.IsNullStringHandler(_ClaimDr("PAT_FNAME"), "") & " " & UFCWGeneral.IsNullStringHandler(_ClaimDr("PAT_INT"), "") & " " & UFCWGeneral.IsNullStringHandler(_ClaimDr("PAT_LNAME"), "")

            LettersControl.FamilyID = If(IsDBNull(_ClaimDr("FAMILY_ID")), 0, CInt(_ClaimDr("FAMILY_ID")))
            LettersControl.RelationID = CShort(If(IsDBNull(_ClaimDr("RELATION_ID")), -1, _ClaimDr("RELATION_ID")))
            LettersControl.ClaimID = If(IsDBNull(_ClaimDr("CLAIM_ID")), 0, CInt(_ClaimDr("CLAIM_ID")))
            LettersControl.ProviderID = If(IsDBNull(_ClaimDr("PROV_ID")), 0, CInt(_ClaimDr("PROV_ID")))
            'LettersControl.PARTSSN = If(IsDBNull(_ClaimDr("PART_SSN")), 0, CInt(_ClaimDr("PART_SSN")))
            'LettersControl.PATSSN = If(IsDBNull(_ClaimDr("PAT_SSN")), 0, CInt(_ClaimDr("PAT_SSN")))
            'LettersControl.PARTFNAME = UFCWGeneral.IsNullStringHandler(_ClaimDr("PART_FNAME"))
            'LettersControl.PARTLNAME = UFCWGeneral.IsNullStringHandler(_ClaimDr("PART_LNAME"))
            'LettersControl.PATFNAME = UFCWGeneral.IsNullStringHandler(_ClaimDr("PAT_FNAME"))
            'LettersControl.PATLNAME = UFCWGeneral.IsNullStringHandler(_ClaimDr("PAT_LNAME"))


            If Not IsDBNull(_ClaimDr("DOC_CLASS")) Then
                LettersControl.LoadLetters(_ClaimDr("DOC_CLASS").ToString)
            End If
            If Not IsDBNull(_ClaimDr("CLAIM_ID")) Then
                LettersHistoryControl.LoadLettersHistory(CType(_ClaimDr("CLAIM_ID"), Integer))
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadCOBInfo()

        Try

            If Not CobControl.ChangesPending AndAlso CobControl.FamilyID <> CInt(_ClaimDr("FAMILY_ID")) AndAlso (CobControl.RelationID Is Nothing OrElse CobControl.RelationID <> CShort(_ClaimDr("RELATION_ID"))) Then
                CobControl.FamilyID = If(IsDBNull(_ClaimDr("FAMILY_ID")), 0, CInt(_ClaimDr("FAMILY_ID")))
                CobControl.RelationID = UFCWGeneral.IsNullShortHandler(_ClaimDr("RELATION_ID"))
                CobControl.ReadOnlyMode = False

                If Not CobControl.COBDataSet Is Nothing AndAlso CobControl.COBDataSet.Tables("MEDOTHER_INS").Rows.Count = 0 Then
                    CobControl.COBDataSet.Tables("MEDOTHER_INS").Load(_ClaimDS.MEDOTHER_INS.CreateDataReader)
                    CobControl.COBDataSet.Tables("MEDOTHER_INS_COUNT").Load(_ClaimDS.MEDOTHER_INS_COUNT.CreateDataReader)
                End If

                CobControl.LoadCOB()
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadProviderInfo()
        '' to reduce the number of calls to database
        Try

            If _ClaimDS.MEDHDR.Rows.Count > 0 AndAlso (_ProviderAndNPIDS Is Nothing OrElse _ProviderAndNPIDS.Tables.Count < 1 OrElse (_ProviderAndNPIDS.Tables("PROVIDER_ADDRESS").Rows.Count > 0 AndAlso _ProviderAndNPIDS.Tables("PROVIDER_ADDRESS").Rows(0)("PROVIDER_ID").ToString <> _ClaimDr("PROV_ID").ToString)) Then
                _ProviderAndNPIDS = New DataSet
                _ProviderAndNPIDS = CMSDALFDBMD.RetrieveProviderAndNPIInfo(UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PROV_ID"), "PROV_ID"), UFCWGeneral.IsNullDecimalHandler(_MedHdrDr("RENDERING_NPI"), ""), _ProviderAndNPIDS)
            End If

            If _ProviderAndNPIDS IsNot Nothing AndAlso Not IsDBNull(_ClaimDr("PROV_ID")) Then
                ProvProviderControl.ProviderID = CInt(_ClaimDr("PROV_ID"))
                ProvProviderControl.LoadProvider(_ProviderAndNPIDS.Tables("PROVIDER_ADDRESS"))
            ElseIf Not IsDBNull(_ClaimDr("PROV_ID")) Then
                ProvProviderControl.LoadProvider(_ClaimDr("PROV_ID").ToString)
            Else
                ProvProviderControl.ClearProvider()
            End If

            If _ProviderAndNPIDS IsNot Nothing AndAlso _MedHdrDr IsNot Nothing AndAlso Not IsDBNull(_MedHdrDr("RENDERING_NPI")) Then
                NpiRegistryControl.NPI = CDec(_MedHdrDr("RENDERING_NPI"))
                NpiRegistryControl.LoadNPI(_ProviderAndNPIDS.Tables("NPI_REGISTRY"))
            Else
                NpiRegistryControl.ClearNPI()
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadParticipantPatientAddress()
        '' to reduce the number of calls to database
        Try
            If _PatientAddressesDS Is Nothing OrElse PatParticipantControl.FamilyID <> CInt(_ClaimDr("FAMILY_ID")) OrElse PatParticipantControl.RelationID <> CShort(_ClaimDr("RELATION_ID")) Then
                _PatientAddressesDS = New DataSet
                _PatientAddressesDS = CMSDALFDBMD.RetrievePatientParticipantAddresses(CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")), _PatientAddressesDS)
            End If

            PartParticipantControl.FamilyID = CInt(_ClaimDr("FAMILY_ID"))
            PartParticipantControl.RelationID = 0
            PartParticipantControl.LoadPARTICIPANT(_PatientAddressesDS.Tables("PARTICIPANT_ADDRESS"))

            PatParticipantControl.FamilyID = CInt(_ClaimDr("FAMILY_ID"))
            PatParticipantControl.RelationID = CShort(_ClaimDr("RELATION_ID"))
            PatParticipantControl.LoadPARTICIPANT(_PatientAddressesDS.Tables("PATIENT_ADDRESS"))

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadDetailLineEligibility(Optional ByVal alertsOnly As Boolean = False)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Loads the Eligibility Control and checks eligibility for all detail lines
        ' also adds an alert if not eligible
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/12/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            For Each DR As DataRow In _ClaimDS.MEDDTL.Rows()
                If DR.RowState <> DataRowState.Deleted Then LoadDetailLineEligibility(DR, alertsOnly)
            Next
            _MedDtlBS.EndEdit()
        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub

    Private Sub LoadDetailLineEligibility(ByRef DR As DataRow, ByVal alertsOnly As Boolean)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Loads the Eligibility Control and checks eligibility for a specific detail line
        ' also adds an alert if not eligible
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/12/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim EligDV As DataView

        Dim Eligible As Boolean = False
        Dim Cobra As Boolean = False
        Dim CobraCore As Boolean = False
        Dim PremiumRequired As Boolean = False
        Dim PremiumPaid As Boolean = False

        Dim EligDR As DataRow

        Try

            ' If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Position < -1 Then Return

            _ClaimAlertManager.DeleteAlertRowsByCategoryAndLine("Eligibility", CInt(DR("LINE_NBR")))

            If EligControl.EligibilityDataTable IsNot Nothing Then
                EligDV = New DataView(EligControl.EligibilityDataTable, "ELIG_PERIOD = '" & Format(If(IsDBNull(DR("OCC_FROM_DATE")), UFCWGeneral.NowDate, DR("OCC_FROM_DATE")), "MM-01-yyyy") & "'", "ELIG_PERIOD DESC", DataViewRowState.CurrentRows)
                If EligDV.Count > 0 Then
                    EligDR = EligDV(0).Row
                End If
            End If

            'Determine Eligibility
            Dim EligMedPlan As String = ""
            Dim EligStatus As String = ""
            Dim NotEligReason As String
            If EligDR IsNot Nothing Then
                If Not IsDBNull(EligDR("MED_ELIG_SW")) AndAlso CBool(EligDR("MED_ELIG_SW")) Then
                    Eligible = True
                End If

                If Not IsDBNull(EligDR("STATUS")) AndAlso CStr(EligDR("STATUS")).ToUpper = "COBRA" Then
                    Cobra = True
                End If

                If Cobra AndAlso Not IsDBNull(EligDR("DEN_ELIG_SW")) AndAlso CBool(EligDR("DEN_ELIG_SW")) AndAlso (_ClaimDr("DOC_TYPE").ToString.ToUpper = "VISION" OrElse _ClaimDr("DOC_TYPE").ToString.ToUpper = "VISION GVA") Then
                    CobraCore = True
                End If

                Dim FamilyCov As Boolean
                If CShort(_ClaimDr("RELATION_ID")) <> 0 AndAlso Not CBool(EligDR("FAMILY_SW")) Then
                    FamilyCov = False
                Else
                    FamilyCov = True
                End If

                If Not IsDBNull(EligDR("PREMIUM_SW")) AndAlso CBool(EligDR("PREMIUM_SW")) Then
                    PremiumRequired = True
                End If

                If PremiumRequired AndAlso Not IsDBNull(EligDR("BAL_MED")) Then
                    If CStr(EligDR("BAL_MED")).ToUpper = "Y" OrElse CStr(EligDR("BAL_MED")).ToUpper = "O" Then
                        PremiumPaid = True
                    End If
                End If

                Dim PremFamilyCov As Boolean

                If PremiumRequired AndAlso PremiumPaid AndAlso Not IsDBNull(EligDR("PREM_TYPE")) Then
                    If CStr(EligDR("PREM_TYPE")).ToUpper = "EMPCHILD" Then   ' if condition added by sbandi
                        If CStr(EligDR("RELATION")).ToUpper = "H" OrElse CStr(EligDR("RELATION")).ToUpper = "W" OrElse CStr(EligDR("RELATION")).ToUpper = "P" Then
                            PremFamilyCov = False
                        Else
                            PremFamilyCov = True
                        End If
                    ElseIf CStr(EligDR("PREM_TYPE")).ToUpper = "FAMILY" Then
                        PremFamilyCov = True
                    ElseIf CStr(EligDR("PREM_TYPE")).ToUpper = "KSINGLE" AndAlso Not IsDBNull(EligDR("RELATION")) Then
                        If CStr(EligDR("RELATION")).ToUpper = "H" OrElse CStr(EligDR("RELATION")).ToUpper = "W" OrElse CStr(EligDR("RELATION")).ToUpper = "P" Then
                            PremFamilyCov = False
                        Else
                            PremFamilyCov = True
                        End If
                    ElseIf CStr(EligDR("PREM_TYPE")).ToUpper = "SINGLE" AndAlso Not IsDBNull(EligDR("RELATION")) Then
                        If CStr(EligDR("RELATION")).ToUpper = "M" Then
                            PremFamilyCov = True
                        Else
                            PremFamilyCov = False
                        End If
                    Else
                        PremFamilyCov = False
                    End If
                ElseIf Not PremiumRequired Then
                    PremFamilyCov = True
                Else
                    PremFamilyCov = False
                End If

                If Not Eligible Then
                    NotEligReason = "Not Eligible"
                    GoTo UpdateDetail
                End If

                If Cobra AndAlso CobraCore Then
                    Eligible = False
                    NotEligReason = "Not Eligible For Vision"

                    GoTo UpdateDetail
                End If

                If Not FamilyCov Then
                    Eligible = False
                    NotEligReason = "Not Eligible For Family Coverage"

                    GoTo UpdateDetail
                End If

                If PremiumRequired AndAlso Not PremiumPaid Then
                    Eligible = False
                    NotEligReason = "Not Eligible Premium Not Paid"

                    GoTo UpdateDetail
                End If

                If PremiumRequired AndAlso PremiumPaid AndAlso Not PremFamilyCov Then
                    Eligible = False
                    NotEligReason = "Not Eligible For Family Coverage"

                    GoTo UpdateDetail
                End If

                Eligible = True
                NotEligReason = ""

                If Not IsDBNull(EligDR("PLAN_TYPE")) Then
                    EligMedPlan = CStr(EligDR("PLAN_TYPE"))
                End If

                If Not IsDBNull(EligDR("STATUS")) Then
                    EligStatus = CStr(EligDR("STATUS"))
                End If
            Else
                Eligible = False
                NotEligReason = "Not Eligible"
            End If

UpdateDetail:
            DR.BeginEdit()
            'Test if the Elig started or ended in the middle of the month
            Dim LineNotEligReason As String = NotEligReason
            If IsDBNull(DR("OCC_FROM_DATE")) OrElse (EligDR IsNot Nothing AndAlso Not (CDate(DR("OCC_FROM_DATE")) >= CDate(EligDR("FROM_DATE")) AndAlso CDate(DR("OCC_FROM_DATE")) <= CDate(EligDR("THRU_DATE")))) Then
                LineNotEligReason = "Not Eligible"
            End If

            If Not IsDBNull(DR("OCC_FROM_DATE")) AndAlso Eligible AndAlso EligDR IsNot Nothing AndAlso CDate(DR("OCC_FROM_DATE")) >= CDate(EligDR("FROM_DATE")) AndAlso CDate(DR("OCC_FROM_DATE")) <= CDate(EligDR("THRU_DATE")) Then
                'Elig
                If Not alertsOnly Then

                    If IsDBNull(DR("MED_PLAN")) OrElse DR("MED_PLAN").ToString <> EligMedPlan Then
                        DR("MED_PLAN") = EligMedPlan
                    End If

                    If IsDBNull(DR("ELIG_STATUS")) OrElse DR("ELIG_STATUS").ToString <> EligStatus Then
                        DR("ELIG_STATUS") = EligStatus
                    End If
                End If
            Else 'Not Elig

                If Not alertsOnly Then
                    'set to null if not already
                    DR("MED_PLAN") = DBNull.Value

                    DR("ELIG_STATUS") = DBNull.Value

                    'Deny Line not Elig

                    DR("STATUS") = "DENY"

                    'reset Paid and Processed

                    DR("PAID_AMT") = DBNull.Value

                    DR("PROCESSED_AMT") = DBNull.Value

                    'make allowed_amt = chrg_amt
                    DR("ALLOWED_AMT") = DR("CHRG_AMT")
                End If

                'add an alert
                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": " & LineNotEligReason, DR("LINE_NBR").ToString, "Eligibility", 20})
            End If
            DR.EndEdit()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadFreeText()
        Try
            WorkFreeTextEditor.FamilyID = CInt(_ClaimDr("FAMILY_ID"))
            WorkFreeTextEditor.RelationID = UFCWGeneral.IsNullShortHandler(_ClaimDr("RELATION_ID"))
            WorkFreeTextEditor.ParticipantSSN = CInt(_ClaimDr("PART_SSN"))
            If Not IsDBNull(_ClaimDr("PAT_SSN")) Then
                WorkFreeTextEditor.PatientSSN = CInt(_ClaimDr("PAT_SSN"))
            End If

            WorkFreeTextEditor.ParticipantFirst = UFCWGeneral.IsNullStringHandler(_ClaimDr("PART_FNAME"), "")
            WorkFreeTextEditor.ParticipantLast = UFCWGeneral.IsNullStringHandler(_ClaimDr("PART_LNAME"), "")
            WorkFreeTextEditor.PatientFirst = UFCWGeneral.IsNullStringHandler(_ClaimDr("PAT_FNAME"), "")
            WorkFreeTextEditor.PatientLast = UFCWGeneral.IsNullStringHandler(_ClaimDr("PAT_LNAME"), "")

            WorkFreeTextEditor.LoadFreeText(_ClaimDS.FREE_TEXT, _ClaimDS.REG_ALERTS)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadDuplicates(Optional ByVal transmitClaimInfo As Boolean = True)
        Dim MatchCnt As Integer = 0
        Dim PNode As TreeNode

        Try

            Me.ReCalcToolBarButton.Enabled = False
            Me.RefreshToolBarButton.Enabled = False
            Me.RefreshMenuItem.Enabled = False
            Me.RecalcMenuItem.Enabled = False
            Me.RePriceReturnMenuItem.Enabled = False
            Me.RePriceMenuItem.Enabled = False
            Me.RePriceReturnToolBarButton.Enabled = False
            Me.RePriceToolBarButton.Enabled = False
            Me.DropdownRepriceReturnToolBarButton.Enabled = False
            Me.CompleteToolBarButton.Enabled = False

#If TRACE Then
            If CInt(_TraceParallel.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If

            Using WC As New GlobalCursor

                _DuplicatesClaimDS = New DataSet

                DupsTreeView.Nodes.Clear()

                If _MedHdrDr IsNot Nothing AndAlso Not IsDBNull(_MedHdrDr("PROV_TIN")) AndAlso _ClaimDS.MEDDTL.Rows.Count > 0 Then

                    PNode = New TreeNode("Retrieving Duplicates, Please wait...")

                    PNode.ForeColor = Color.Red
                    PNode.Expand()

                    DupsTreeView.Nodes.Add(PNode)

                    _ExDup = New ExecuteDuplicates(Me, New ExecuteDuplicates.NotifyProgress(AddressOf DuplicateProcessingCompleted), _DuplicatesClaimDS, _ClaimDS, transmitClaimInfo)
                    _DuplicateThread = New Thread(AddressOf _ExDup.Execute)
                    _DuplicateThread.IsBackground = True

                    _DuplicateThread.Start()

                ElseIf _MedHdrDr IsNot Nothing AndAlso _ClaimDS.MEDDTL.Rows.Count = 0 Then
                    PNode = New TreeNode("Duplicate check unavailable due to missing detail.")

                    PNode.ForeColor = Color.Red
                    PNode.Expand()

                    DupsTreeView.Nodes.Add(PNode)

                ElseIf _MedHdrDr IsNot Nothing AndAlso IsDBNull(_MedHdrDr("PROV_TIN")) Then
                    PNode = New TreeNode("Duplicate check unavailable due to missing Provider info.")

                    PNode.ForeColor = Color.Red
                    PNode.Expand()

                    DupsTreeView.Nodes.Add(PNode)
                End If

                DuplicatesOriginalDetailLinesDataGrid.DataSource = _ClaimDS.MEDDTL

                SetTableStyle(DuplicatesOriginalDetailLinesDataGrid, False)

                DuplicatesOriginalDetailLinesDataGrid.CaptionText = "Current Claim"

            End Using

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceParallel.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If
        End Try
    End Sub

    Private Sub LoadAssociated(Optional ByVal transmitClaimInfo As Boolean = True)

        Try
            Me.ReCalcToolBarButton.Enabled = False
            Me.RefreshToolBarButton.Enabled = False
            Me.RefreshMenuItem.Enabled = False
            Me.RecalcMenuItem.Enabled = False
            Me.RePriceReturnMenuItem.Enabled = False
            Me.RePriceMenuItem.Enabled = False
            Me.RePriceReturnToolBarButton.Enabled = False
            Me.RePriceToolBarButton.Enabled = False
            Me.DropdownRepriceReturnToolBarButton.Enabled = False
            Me.CompleteToolBarButton.Enabled = False

            Using WC As New GlobalCursor

                _AssociatedClaimDS = New DataSet

                AssociatedResultsDataGrid.CaptionText = "Collecting Associated Claim(s) info"

                If _MedHdrDr IsNot Nothing AndAlso Not IsDBNull(_MedHdrDr("PROV_TIN")) AndAlso _MedDtlDR IsNot Nothing Then

                    _ExAssociated = New BackThread(Me, New BackThread.NotifyProgress(AddressOf AssociatedProcessingCompleted), _AssociatedClaimDS, _ClaimDS, _Mode.ToUpper = "AUDIT")
                    _AssociatedThread = New Thread(AddressOf _ExAssociated.Execute)
                    _AssociatedThread.IsBackground = True

                    _AssociatedThread.Start()
                End If

                _AssOrigDtlLineBS = New BindingSource With {
                    .DataSource = _ClaimDS.MEDDTL
                }

                AssociatedOriginalDetailLinesDataGrid.SuspendLayout()

                AssociatedOriginalDetailLinesDataGrid.DataSource = _AssOrigDtlLineBS

                AssociatedOriginalDetailLinesDataGrid.ResumeLayout()

                SetTableStyle(AssociatedOriginalDetailLinesDataGrid, False)

                AssociatedOriginalDetailLinesDataGrid.Select(_AssOrigDtlLineBS.Position)
                AssociatedOriginalDetailLinesDataGrid.CaptionText = "Current Claim"

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    'Private Sub LoadHeaderInfo()
    '    ' -----------------------------------------------------------------------------
    '    ' <summary>
    '    ' loads header info that can't be handled by a databinding
    '    ' </summary>
    '    ' <remarks>
    '    ' </remarks>
    '    ' <history>
    '    ' 	[nick snyder]	8/16/2006	Created
    '    ' Lalitha Moparthi  12/05/2022 Updated BindingSource
    '    ' </history>
    '    ' -----------------------------------------------------------------------------

    '    Try

    '        'If _MedHdrBS IsNot Nothing AndAlso _MedHdrBS.Position > -1 Then

    '        '    _MedHdrDr = DirectCast(_MedHdrBS.Current, DataRowView).Row

    '        '    If CInt(_MedHdrDr("NON_PAR_SW")) = 1 Then
    '        '        NonPARCheckBox.Checked = True
    '        '    End If

    '        '    If CInt(_MedHdrDr("OUT_OF_AREA_SW")) = 1 Then
    '        '        OOACheckBox.Checked = True
    '        '    End If

    '        'Else
    '        '    txtPatAcctNo.Text = ""
    '        '    cmbCOB.SelectedIndex = -1
    '        '    cmbPricingNetwork.SelectedIndex = -1
    '        '    txtIncidentDate.Text = ""
    '        '    OOACheckBox.Checked = False
    '        '    NonPARCheckBox.Checked = False
    '        '    cmbPayee.SelectedIndex = -1
    '        '    txtProviderID.Text = ""
    '        '    txtProviderLicenseNo.Text = ""
    '        '    txtProviderRenderingNPI.Text = ""
    '        '    txtBCCZIP.Text = ""
    '        '    cmbHosp.SelectedIndex = -1

    '        '    txtTotalChargedAmt.Text = ""
    '        '    txtTotalAllowedAmt.Text = ""
    '        '    txtTotalPaidAmt.Text = ""
    '        '    txtTotalOIAmt.Text = ""
    '        'End If

    '        LoadHeaderDefaults()

    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    Private Sub LoadDocHistory()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Loads the Document History
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	10/18/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            DocumentHistoryViewer.Annotations = _ClaimDS.ANNOTATIONS
            DocumentHistoryViewer.ParticipantSSN = If(IsDBNull(_ClaimDr("PART_SSN")), 0, CInt(_ClaimDr("PART_SSN")))
            DocumentHistoryViewer.PatientSSN = If(IsDBNull(_ClaimDr("PAT_SSN")), 0, CInt(_ClaimDr("PAT_SSN")))
            DocumentHistoryViewer.ParticipantFirst = UFCWGeneral.IsNullStringHandler("PART_FNAME")
            DocumentHistoryViewer.ParticipantLast = UFCWGeneral.IsNullStringHandler("PART_LNAME")
            DocumentHistoryViewer.PatientFirst = UFCWGeneral.IsNullStringHandler("PAT_FNAME")
            DocumentHistoryViewer.PatientLast = UFCWGeneral.IsNullStringHandler("PAT_LNAME")

            DocumentHistoryViewer.RefreshHistory(_ClaimID, CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")), _ClaimDS.CLAIMDOCHISTORY)

            DocumentHistoryViewer.AnnotateButton.Visible = False

            AnnotationControl.ClaimID = _ClaimID
            AnnotationControl.FamilyID = CInt(_ClaimDr("FAMILY_ID"))
            AnnotationControl.RelationID = CInt(_ClaimDr("RELATION_ID"))
            AnnotationControl.ParticipantSSN = If(IsDBNull(_ClaimDr("PART_SSN")), 0, CInt(_ClaimDr("PART_SSN")))
            AnnotationControl.PatientSSN = If(IsDBNull(_ClaimDr("PAT_SSN")), 0, CInt(_ClaimDr("PAT_SSN")))
            AnnotationControl.ParticipantFirst = UFCWGeneral.IsNullStringHandler("PART_FNAME")
            AnnotationControl.ParticipantLast = UFCWGeneral.IsNullStringHandler("PART_LNAME")
            AnnotationControl.PatientFirst = UFCWGeneral.IsNullStringHandler("PAT_FNAME")
            AnnotationControl.PatientLast = UFCWGeneral.IsNullStringHandler("PAT_LNAME")
            AnnotationControl.Annotations = _ClaimDS.ANNOTATIONS
            AnnotationControl.Annotations.AcceptChanges()
            AnnotationControl.Refresh()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadAudit()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Loads the Audit Form And Alerts
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/12/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Adjuster As String

        Try
            Adjuster = _DomainUser.ToUpper

            If Not IsDBNull(_ClaimDr("PENDED_TO")) Then
                Adjuster = CStr(_ClaimDr("PENDED_TO"))
            End If

            _AuditForm = New Audit(_ClaimID, Adjuster)

            _AuditForm.LoadAuditControl()

            LoadAuditAlerts(False)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LoadDetailLineRow(ByVal detailLine As Integer)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Loads Detail Line data into the Edit Panel
        ' </summary>
        ' <param name="DetailLine"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	4/10/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        '    Dim DV As DataView

        Try
            _ChangingRows = True

            '  DV = DetailLinesDataGrid.GetDefaultDataView

            DetailLineGroupBox.Text = "Edit Item - " & detailLine

            '   DetailLinesDataGrid.ResetSelection()

        Catch ex As Exception
            Throw
        Finally
            _ChangingRows = False
        End Try
    End Sub

    Private Sub ShowDetailLineReasons()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Opens a Reason Code Selector/Viewer Dialog to manage Reason Codes
        ' </summary>
        ' <param name="DetailLine"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	6/15/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim ApplyStatus As String = ""
        Dim MedDtlDR As DataRow
        Dim DetailLine As Short
        Dim DateOfService As Date
        Dim ReasonsDT As DataTable

        Try
            If _MedDtlBS Is Nothing OrElse _MedDtlBS.Position < 0 Then Return

            _ClaimAlertManager.SuspendLayout = True

            MedDtlDR = DirectCast(_MedDtlBS.Current, DataRowView).Row
            DetailLine = CShort(MedDtlDR("LINE_NBR"))
            DateOfService = CDate(If(IsDBNull(MedDtlDR("OCC_FROM_DATE")), UFCWGeneral.NowDate, MedDtlDR("OCC_FROM_DATE")))

            ReasonsDT = _ClaimDS.Tables("REASON").Clone
            If _ClaimDS.Tables("REASON").Rows.Count > 0 Then
                Dim ReasonsQuery = _ClaimDS.Tables("REASON").AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = DetailLine)
                If ReasonsQuery.Any Then
                    ReasonsDT = ReasonsQuery.CopyToDataTable
                End If
            End If

            Using ReasonForm As DetailLineReasons = New DetailLineReasons(_ClaimID, MedDtlDR, ReasonsDT)

                ReasonForm.GridLines = DetailLinesDataGrid.GetGridRowCount

                If ReasonForm.ShowDialog(Me) = DialogResult.OK Then
                    If ReasonForm.Status.ToUpper = "UPDATELINE" Then

                        '_ClaimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & DetailLine.ToString & ": Paid Is 0 and a Reason is Required'", DetailLine)
                        '_ClaimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & DetailLine.ToString & ": Paid Is More Than Priced'", DetailLine)

                        ApplyLineReasons(DetailLine, ReasonForm.SelectedCodesArray)
                        MedDtlDR("REASONS") = ReasonForm.SelectedCodesFlat

                    ElseIf ReasonForm.Status.ToUpper = "UPDATEALL" Then

                        '_ClaimAlertManager.DeleteAlertRowsByMessage(": Paid Is 0 and a Reason is Required'")
                        '_ClaimAlertManager.DeleteAlertRowsByMessage(": Paid Is More Than Priced'")

                        Dim UpdateAllQuery = From MD In _ClaimDS.Tables("MEDDTL").AsEnumerable
                                             Where MD.RowState <> DataRowState.Deleted _
                                             AndAlso MD.Field(Of String)("STATUS") <> "MERGED"

                        If UpdateAllQuery.Any Then
                            For Each MDR As DataRow In UpdateAllQuery.AsEnumerable
                                ApplyLineReasons(CShort(MDR("LINE_NBR")), ReasonForm.SelectedCodesArray)
                                MDR("REASONS") = ReasonForm.SelectedCodesFlat
                            Next
                        End If

                    ElseIf ReasonForm.Status.ToUpper = "CLEARLINE" Then
                        Dim QueryDelete = _ClaimDS.REASON.AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = DetailLine)
                        For Each DR As DataRow In QueryDelete.ToList()
                            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & DR("LINE_NBR").ToString & ": " & DR("DESCRIPTION").ToString & "'", CInt(DR("LINE_NBR")))
                            DR.Delete()
                        Next
                        MedDtlDR("REASON_SW") = 0
                        MedDtlDR("REASONS") = DBNull.Value
                        'if paid = 0 and there is not reasons add alert
                        If IsDBNull(MedDtlDR("PAID_AMT")) OrElse CDec(MedDtlDR("PAID_AMT")) = 0D Then
                            _ClaimAlertManager.AddAlertRow(New Object() {"Line " & MedDtlDR("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required", MedDtlDR("LINE_NBR").ToString, "Detail", 30})
                        End If
                    ElseIf ReasonForm.Status.ToUpper = "CLEARALL" Then
                        MassClearField("REASON_SW")
                    End If
                End If
            End Using
            _MedDtlBS.EndEdit()
        Catch ex As Exception
            Throw
        Finally
            _ClaimAlertManager.SuspendLayout = False
        End Try
    End Sub


    Private Sub ShowReadOnlyDetailLineReasons(ByVal detailLine As Short, ByVal dateOfService As Date)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Opens a Reason Code Viewer
        ' </summary>
        ' <param name="DetailLine"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	6/15/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim ClaimID As Integer
        Dim DupesClaimDS As ClaimDataset
        Dim ReasonForm As DetailLineReasons

        Try
            DupesClaimDS = New ClaimDataset
            ClaimID = CInt(DupsTreeView.SelectedNode.Text.Split(CChar(";"))(0))

            DupesClaimDS = CType(CMSDALFDBMD.RetrieveDetailLineReasons(ClaimID, detailLine, DupesClaimDS), ClaimDataset) ''mClaimID
            ReasonForm = New DetailLineReasons(ClaimID, detailLine, DupesClaimDS, True) ''mClaimID

            ReasonForm.ShowDialog(Me)

        Catch ex As Exception
            Throw
        Finally
            If DupesClaimDS IsNot Nothing Then
                DupesClaimDS.Clear()
                DupesClaimDS.Dispose()

            End If

            If ReasonForm IsNot Nothing Then
                ReasonForm.Dispose()
            End If
        End Try
    End Sub

    Private Sub ShowDetailLineModifiers()

        Dim Frm As DetailLineModifierForm
        Dim MedModDT As DataTable
        Dim LineNumber As Short
        Dim MedDtlDR As DataRow

        Try

            If _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing Then Return

            MedDtlDR = DirectCast(_MedDtlBS.Current, DataRowView).Row

            If MedDtlDR Is Nothing Then Exit Sub

            LineNumber = CShort(MedDtlDR("LINE_NBR"))

            MedModDT = _ClaimDS.Tables("MEDMOD").Clone
            If _ClaimDS.Tables("MEDMOD").Rows.Count > 0 Then
                Dim MedModAQuery = _ClaimDS.Tables("MEDMOD").AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = LineNumber)

                If MedModAQuery.Any Then
                    MedModDT = MedModAQuery.CopyToDataTable
                End If
            End If

            Frm = New DetailLineModifierForm(_ClaimID, MedDtlDR, MedModDT)

            If Frm.ShowDialog(Me) = DialogResult.OK Then
                Select Case Frm.Status.ToUpper
                    Case "UPDATELINE"
                        UpdateLineModifier(LineNumber, Frm)
                    Case "UPDATEALL"
                        For DetailLineNumber As Short = 1 To CShort(_MedDtlBS.Count)
                            UpdateLineModifier(DetailLineNumber, Frm)
                        Next
                    Case "CLEARLINE"
                        ClearModifierLine(LineNumber)
                    Case "CLEARALL"
                        MassClearField("MODIFIER_SW")
                End Select
            End If

        Catch ex As Exception
            Throw
        Finally
            If Frm IsNot Nothing Then
                Frm.Dispose()
            End If
            Frm = Nothing
        End Try
    End Sub

    Private Sub ShowDetailLinePOS()

        Dim Frm As POSLookupForm
        Dim DGRow As DataRow
        Dim POSDR As DataRow

        Try

            DGRow = CType(_MedDtlBS.Current, DataRowView).Row

            If Not IsDBNull(DGRow("OCC_FROM_DATE")) Then
                Frm = New POSLookupForm(CType(If(IsDBNull(DGRow("OCC_FROM_DATE")), Nothing, DGRow("OCC_FROM_DATE")), Date?))
            Else
                Frm = New POSLookupForm
            End If

            If Frm.ShowDialog(Me) = DialogResult.OK Then

                POSDR = Frm.SelectedPOSDataRow

                If Frm.Status = "UPDATELINE" Then

                    ErrorProvider1.ClearError(txtPlaceOfService)

                    If DGRow("STATUS").ToString <> "MERGED" Then

                        DGRow("PLACE_OF_SERV") = POSDR("PLACE_OF_SERV_VALUE")

                        DGRow("PLACE_OF_SERV_DESC") = POSDR("SHORT_DESC")
                        _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & DGRow("LINE_NBR").ToString & ": Invalid Place of Service'", CInt(DGRow("LINE_NBR")))

                        DGRow.EndEdit()

                    End If

                ElseIf Frm.Status = "UPDATEALL" Then

                    For Each DR As DataRow In CType(_MedDtlBS.DataSource, DataTable).Rows

                        DR.BeginEdit()

                        DR("PLACE_OF_SERV") = POSDR("PLACE_OF_SERV_VALUE")

                        DR("PLACE_OF_SERV_DESC") = POSDR("SHORT_DESC")

                        _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & DR("LINE_NBR").ToString & ": Invalid Place of Service'", CInt(DR("LINE_NBR")))

                        DR.EndEdit()

                    Next

                ElseIf Frm.Status.ToUpper = "CLEARALL" Then

                    MassClearField("PLACE_OF_SERV")
                End If
            End If

            _MedDtlBS.EndEdit()

        Catch ex As Exception
            Throw
        Finally
            If Frm IsNot Nothing Then
                Frm.Close()
                Frm.Dispose()
            End If
        End Try
    End Sub

    Private Sub ShowDetailLineBillType()

        Dim Frm As BillTypeLookupForm
        Dim DGRow As DataRow
        Dim BillTypeDR As DataRow

        Try
            DGRow = CType(_MedDtlBS.Current, DataRowView).Row

            If Not IsDBNull(DGRow("OCC_FROM_DATE")) Then
                Frm = New BillTypeLookupForm(CType(If(IsDBNull(DGRow("OCC_FROM_DATE")), UFCWGeneral.NowDate, DGRow("OCC_FROM_DATE")), Date?))
            Else
                Frm = New BillTypeLookupForm
            End If

            If Frm.ShowDialog(Me) = DialogResult.OK Then

                BillTypeDR = Frm.SelectedBillTypeDataRow

                If Frm.Status = "UPDATELINE" Then

                    ErrorProvider1.ClearError(txtProcedure)

                    If DGRow("STATUS").ToString <> "MERGED" Then

                        DGRow("BILL_TYPE") = BillTypeDR("BILL_TYPE_VALUE")

                        DGRow("BILL_TYPE_DESC") = BillTypeDR("DESC_3")

                        _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & DGRow("LINE_NBR").ToString & ": Invalid BillType'", CInt(DGRow("LINE_NBR")))

                    End If

                ElseIf Frm.Status = "UPDATEALL" Then

                    For Each DR As DataRow In CType(_MedDtlBS.DataSource, DataTable).Rows

                        DR("BILL_TYPE") = BillTypeDR("BILL_TYPE_VALUE")

                        DR("BILL_TYPE_DESC") = BillTypeDR("DESC_3")

                        _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & DR("LINE_NBR").ToString & ": Invalid BillType'", CInt(DR("LINE_NBR")))

                    Next

                ElseIf Frm.Status.ToUpper = "CLEARALL" Then
                    MassClearField("BILL_TYPE")
                    MassClearField("BILL_TYPE_DESC")
                End If
            End If

            _MedDtlBS.EndEdit()

        Catch ex As Exception
            Throw
        Finally
            If Frm IsNot Nothing Then
                Frm.Close()
                Frm.Dispose()
            End If
        End Try
    End Sub
    Private Sub ShowDetailLineDiagnosis()
        Dim Frm As DetailLineDiagnosisForm
        Dim MedDiagDT As DataTable
        Dim LineNumber As Short
        Dim MedDtlDR As DataRow

        Try
            ErrorProvider1.ClearError(txtDiagnoses)

            MedDtlDR = DirectCast(_MedDtlBS.Current, DataRowView).Row

            LineNumber = CShort(MedDtlDR("LINE_NBR"))

            MedDiagDT = _ClaimDS.Tables("MEDDIAG").Clone
            If _ClaimDS.Tables("MEDDIAG").Rows.Count > 0 Then
                Dim MedDiagQuery = _ClaimDS.Tables("MEDDIAG").AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = LineNumber)
                If MedDiagQuery.Any Then
                    MedDiagDT = MedDiagQuery.CopyToDataTable
                End If
            End If

            Frm = New DetailLineDiagnosisForm(_ClaimID, MedDtlDR, MedDiagDT)

            If Frm.ShowDialog(Me) = DialogResult.OK Then
                Select Case Frm.Status.ToUpper
                    Case "UPDATELINE"
                        UpdateLineDiagnoses(LineNumber, Frm)
                    Case "UPDATEALL"
                        For DetailLineNumber As Short = 1 To CShort(DetailLinesDataGrid.GetGridRowCount)
                            UpdateLineDiagnoses(DetailLineNumber, Frm)
                        Next
                    Case "CLEARLINE"
                        ClearDiagnosisLine(LineNumber)
                    Case "CLEARALL"
                        MassClearField("DIAGNOSIS")
                End Select
            End If
        Catch ex As Exception
            Throw
        Finally
            If Frm IsNot Nothing Then
                Frm.Dispose()
            End If
            Frm = Nothing
        End Try

    End Sub


    Private Sub ClearDiagnosisLine(detailLineNumber As Short)
        Try

            Dim QueryDelete = _ClaimDS.MEDDIAG.AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = detailLineNumber)

            For Each DR As DataRow In QueryDelete.ToList()
                DR.Delete()
            Next

            _ClaimAlertManager.DeleteAlertRowsLikeMessageAndLine("Line " & detailLineNumber & ": Invalid Diagnosis", detailLineNumber)

            Dim QueryMedDtl = _ClaimDS.MEDDTL.AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = detailLineNumber)

            Dim MedDtlDR As DataRow = QueryMedDtl.FirstOrDefault()

            If MedDtlDR IsNot Nothing Then
                MedDtlDR("DIAGNOSES") = DBNull.Value
                MedDtlDR("DIAG_SW") = 0
                MedDtlDR.EndEdit()
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub ClearModifierLine(detailLineNumber As Short)
        Try

            Dim QueryDelete = _ClaimDS.MEDMOD.AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = detailLineNumber)

            For Each DR As DataRow In QueryDelete.ToList()
                DR.Delete()
            Next

            _ClaimAlertManager.DeleteAlertRowsLikeMessageAndLine("Line " & detailLineNumber & ": Invalid Modifier", detailLineNumber)

            Dim QueryMedDtl = _ClaimDS.MEDDTL.AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = detailLineNumber)

            Dim MedDtlDR As DataRow = QueryMedDtl.FirstOrDefault()

            If MedDtlDR IsNot Nothing Then
                MedDtlDR("MODIFIER") = DBNull.Value
                MedDtlDR("MODIFIER_SW") = 0
                MedDtlDR.EndEdit()
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub UpdateAllDetailLinesAccumulators(accumDT As DataTable, accumOrderDV As DataView)

        Try

            For DetailLine As Integer = 1 To DetailLinesDataGrid.GetGridRowCount
                UpdateDetailLineAccumulators(DetailLine, accumDT, accumOrderDV)
            Next

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub UpdateDetailLineAccumulators(ByVal detailLine As Integer, accumDT As DataTable, accumOrderDV As DataView)
        Dim DetailDV As DataView
        Dim AccumDV As DataView
        Dim DV As DataView
        Dim UpdateDV As DataView
        Dim ApplyDate As DateTime
        Dim DR As DataRow

        Try
            DetailDV = New DataView(_ClaimDS.MEDDTL, "LINE_NBR = " & detailLine, "LINE_NBR", DataViewRowState.CurrentRows)
            AccumDV = New DataView(_DetailAccumulatorsDT, "LINE_NBR = " & detailLine, "LINE_NBR", DataViewRowState.CurrentRows)
            DV = New DataView(accumDT, "", "LINE_NBR", DataViewRowState.CurrentRows)
            UpdateDV = New DataView(_ClaimDS.MEDDTL, "STATUS <> 'MERGED' AND LINE_NBR = " & DetailLine, "Line_NBR", DataViewRowState.CurrentRows)
            DV.RowFilter = "CLAIM_ID=" & _ClaimID

            If UpdateDV.Count > 0 Then ' checking merge status

                If Not IsDBNull(DetailDV(0)("OCC_FROM_DATE")) Then
                    ApplyDate = CDate(DetailDV(0)("OCC_FROM_DATE"))
                ElseIf _ClaimDS.MEDHDR.Rows.Count > 0 AndAlso Not IsDBNull(_ClaimDS.MEDHDR.Rows(0)("OCC_FROM_DATE")) Then
                    ApplyDate = CDate(_ClaimDS.MEDHDR.Rows(0)("OCC_FROM_DATE"))
                Else
                    ApplyDate = UFCWGeneral.NowDate
                End If

                If DV.Count > 0 Then
                    For Cnt As Integer = 0 To DV.Count - 1
                        AccumDV.RowFilter = "LINE_NBR = " & detailLine & " AND ACCUM_NAME = '" & DV(Cnt)("ACCUM_NAME").ToString & "'"
                        If Not IsDBNull(DV(Cnt)("ACCUM_NAME")) AndAlso DV(Cnt)("ACCUM_NAME").ToString.Trim = "" Then

                        End If
                        Select Case DV(Cnt).Row.RowState
                            Case Is = DataRowState.Added
                                If AccumDV.Count = 0 Then

                                    _ClaimMemberAccumulatorManager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID(CStr(DV(Cnt)("ACCUM_NAME")))), _ClaimID, CShort(detailLine), ApplyDate, CDec(DV(Cnt)("ENTRY_VALUE")), True, _DomainUser.ToUpper)

                                    accumOrderDV.RowFilter = "ACCUM_NAME = '" & DV(Cnt)("ACCUM_NAME").ToString & "'"

                                    DR = AccumDV.Table.NewRow
                                    DR("CLAIM_ID") = _ClaimID
                                    DR("ACCUM_NAME") = DV(Cnt)("ACCUM_NAME")
                                    DR("ENTRY_VALUE") = DV(Cnt)("ENTRY_VALUE")
                                    DR("OVERRIDE_SW") = DV(Cnt)("OVERRIDE_SW")
                                    DR("LINE_NBR") = detailLine
                                    If accumOrderDV.Count < 1 Then
                                        DR("DISPLAY_ORDER") = 999999
                                    Else
                                        DR("DISPLAY_ORDER") = accumOrderDV(0)("DISPLAY_ORDER")
                                    End If
                                    DR("APPLY_DATE") = ApplyDate

                                    AccumDV.Table.Rows.Add(DR)

                                    DetailDV(0).Row("OVERRIDE_SW") = True
                                Else
                                    _ClaimMemberAccumulatorManager.OverrideEntry(CInt(AccumulatorController.GetAccumulatorID(CStr(DV(Cnt)("ACCUM_NAME")))), _ClaimID, CShort(detailLine), ApplyDate, CDec(DV(Cnt)("ENTRY_VALUE")), _DomainUser.ToUpper)

                                    AccumDV(0).Row("ENTRY_VALUE") = DV(Cnt)("ENTRY_VALUE")

                                    DetailDV(0).Row("OVERRIDE_SW") = True
                                End If
                            Case Is = DataRowState.Deleted
                                If AccumDV.Count > 0 Then
                                    _ClaimMemberAccumulatorManager.RemoveEntry(CInt(AccumulatorController.GetAccumulatorID(CStr(DV(Cnt)("ACCUM_NAME")))), CShort(detailLine))
                                    AccumDV(0).Row.Delete()

                                    DetailDV(0).Row("OVERRIDE_SW") = True
                                End If
                            Case Is = DataRowState.Modified, Is = DataRowState.Unchanged
                                If AccumDV.Count > 0 Then
                                    _ClaimMemberAccumulatorManager.OverrideEntry(CInt(AccumulatorController.GetAccumulatorID(CStr(DV(Cnt)("ACCUM_NAME")))), _ClaimID, CShort(detailLine), ApplyDate, CDec(DV(Cnt)("ENTRY_VALUE")), _DomainUser.ToUpper)

                                    If UFCWGeneral.IsNullDecimalHandler(AccumDV(0)("ENTRY_VALUE")) <> UFCWGeneral.IsNullDecimalHandler(DV(Cnt)("ENTRY_VALUE")) Then AccumDV(0).Row("ENTRY_VALUE") = DV(Cnt)("ENTRY_VALUE")
                                    DetailDV(0).Row("OVERRIDE_SW") = True
                                Else
                                    _ClaimMemberAccumulatorManager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID(CStr(DV(Cnt)("ACCUM_NAME")))), _ClaimID, CShort(detailLine), ApplyDate, CDec(DV(Cnt)("ENTRY_VALUE")), True, _DomainUser.ToUpper)

                                    accumOrderDV.RowFilter = "ACCUM_NAME = '" & DV(Cnt)("ACCUM_NAME").ToString & "'"

                                    DR = AccumDV.Table.NewRow

                                    DR("CLAIM_ID") = _ClaimID
                                    DR("ACCUM_NAME") = DV(Cnt)("ACCUM_NAME")
                                    DR("ENTRY_VALUE") = DV(Cnt)("ENTRY_VALUE")
                                    DR("OVERRIDE_SW") = DV(Cnt)("OVERRIDE_SW")
                                    DR("LINE_NBR") = detailLine
                                    If accumOrderDV.Count < 1 Then
                                        DR("DISPLAY_ORDER") = 999999
                                    Else
                                        DR("DISPLAY_ORDER") = accumOrderDV(0)("DISPLAY_ORDER")
                                    End If
                                    DR("APPLY_DATE") = ApplyDate

                                    AccumDV.Table.Rows.Add(DR)
                                    DetailDV(0).Row("OVERRIDE_SW") = True
                                End If
                        End Select
                    Next
                End If
                DV = New DataView(accumDT, "", "LINE_NBR", DataViewRowState.Deleted)
                If DV.Count > 0 Then
                    For Cnt As Integer = 0 To DV.Count - 1
                        AccumDV.RowFilter = "LINE_NBR = " & detailLine & " AND ACCUM_NAME = '" & DV(Cnt)("ACCUM_NAME").ToString & "'"

                        If AccumDV.Count > 0 Then
                            _ClaimMemberAccumulatorManager.RemoveEntry(CInt(AccumulatorController.GetAccumulatorID(CStr(DV(Cnt)("ACCUM_NAME")))), CShort(detailLine))

                            AccumDV(0).Row.Delete()

                            DetailDV(0).Row("OVERRIDE_SW") = True
                        End If
                    Next
                End If

                RefreshAccumulatorTables()

                _DetailAccumulatorsDT.AcceptChanges()
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub ClearDetailLineAccumulators(ByVal detailLine As Integer, ByVal accumulatorsDT As DataTable)
        Dim ApplyDate As DateTime
        Dim UpdatableDetailLineDV As DataView
        Dim LineAccumulatorsDV As DataView
        Dim AccumulatorsDV As DataView
        Try
            If _DetailAccumulatorsDT Is Nothing OrElse _ClaimDS.MEDDTL Is Nothing Then Return

            UpdatableDetailLineDV = New DataView(_ClaimDS.MEDDTL, "STATUS <> 'MERGED' AND LINE_NBR = " & detailLine, "LINE_NBR", DataViewRowState.CurrentRows)

            LineAccumulatorsDV = New DataView(_DetailAccumulatorsDT, "LINE_NBR = " & detailLine, "LINE_NBR", DataViewRowState.CurrentRows)
            AccumulatorsDV = New DataView(accumulatorsDT, "", "LINE_NBR", DataViewRowState.CurrentRows)
            If UpdatableDetailLineDV.Count > 0 Then ' checking merge status
                If Not IsDBNull(UpdatableDetailLineDV(0)("OCC_FROM_DATE")) Then
                    ApplyDate = CDate(UpdatableDetailLineDV(0)("OCC_FROM_DATE"))
                ElseIf _MedHdrDr IsNot Nothing AndAlso Not IsDBNull(_MedHdrDr("OCC_FROM_DATE")) Then
                    ApplyDate = CDate(_MedHdrDr("OCC_FROM_DATE"))
                Else
                    ApplyDate = UFCWGeneral.NowDate
                End If
                If AccumulatorsDV.Count > 0 Then
                    For Cnt As Integer = 0 To AccumulatorsDV.Count - 1
                        LineAccumulatorsDV.RowFilter = "LINE_NBR = " & detailLine & " AND ACCUM_NAME = '" & AccumulatorsDV(Cnt)("ACCUM_NAME").ToString & "'"

                        If LineAccumulatorsDV.Count > 0 Then
                            _ClaimMemberAccumulatorManager.OverrideEntry(CInt(AccumulatorController.GetAccumulatorID(CStr(AccumulatorsDV(Cnt)("ACCUM_NAME")))), _ClaimID, CShort(detailLine), ApplyDate, 0, _DomainUser.ToUpper)

                            If IsDBNull(LineAccumulatorsDV(0)("ENTRY_VALUE")) OrElse CInt(LineAccumulatorsDV(0)("ENTRY_VALUE")) <> 0 Then LineAccumulatorsDV(0).Row("ENTRY_VALUE") = 0
                            If IsDBNull(LineAccumulatorsDV(0).Row("OVERRIDE_SW")) OrElse Not CBool(LineAccumulatorsDV(0).Row("OVERRIDE_SW")) Then LineAccumulatorsDV(0).Row("OVERRIDE_SW") = 1
                        End If
                    Next
                End If

                RefreshAccumulatorTables()
                _DetailAccumulatorsDT.AcceptChanges()
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ClearAllDetailLinesAccumulators(accumulatorsDT As DataTable)

        Try

            For DetailLine As Integer = 1 To DetailLinesDataGrid.GetGridRowCount

                ClearDetailLineAccumulators(DetailLine, accumulatorsDT)

            Next

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ShowDetailLineAccumulators(ByVal detailLine As Short, Optional ByVal massUpdateMode As Boolean = False)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' opens an accumulator viewer for a given line of the item
        ' </summary>
        ' <param name="DetailLine"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	4/26/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim AccumOrderView As DataView
        Dim DetailLineAccumulatorsForm As DetailLineAccumulators

        Try
            If _DetailAccumulatorsDT Is Nothing Then BuildDetailLineAccumulators()

            AccumOrderView = New DataView(AccumulatorController.GetActiveAccumulators, "", "ACCUM_NAME", DataViewRowState.CurrentRows)

            DetailLineAccumulatorsForm = New DetailLineAccumulators(_ClaimID, detailLine, _DetailAccumulatorsDT, CBool(_Mode.ToUpper = "AUDIT"), massUpdateMode)
            DetailLineAccumulatorsForm.Left = Control.MousePosition.X
            DetailLineAccumulatorsForm.Top = Control.MousePosition.Y

            If DetailLineAccumulatorsForm.ShowDialog(Me) = DialogResult.OK Then
                Select Case DetailLineAccumulatorsForm.Status.ToUpper
                    Case Is = "UPDATELINE"
                        UpdateDetailLineAccumulators(detailLine, DetailLineAccumulatorsForm.LineAccumulatorsDT, AccumOrderView)
                    Case Is = "UPDATEALL"
                        UpdateAllDetailLinesAccumulators(DetailLineAccumulatorsForm.LineAccumulatorsDT, AccumOrderView)
                    Case Is = "CLEARLINE"
                        ClearDetailLineAccumulators(detailLine, _DetailAccumulatorsDT)
                    Case Is = "CLEARALL"
                        If MsgBox("Are you sure you want to Delete ALL Accumulators for ALL Lines", CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo, MsgBoxStyle), "Confirm Delete") = DialogResult.Yes Then
                            ClearAllDetailLinesAccumulators(_DetailAccumulatorsDT)
                        End If

                End Select
            End If
            _MedDtlBS.EndEdit()
        Catch ex As Exception
            Throw
        Finally
            If DetailLineAccumulatorsForm IsNot Nothing Then DetailLineAccumulatorsForm.Dispose()

        End Try
    End Sub

    Private Sub RefreshAccumulatorTables()
        Try
            If _AccumulatorsDT IsNot Nothing Then
                _AccumulatorsDT.Dispose()
                _AccumulatorsDT = Nothing
            End If

            'new claims do not have a claimbinder yet
            If _ClaimBinder IsNot Nothing Then _AccumulatorsDT = _ClaimBinder.GetAccumulatorSummary

            LoadAccumulators()

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub ShowAnnotations()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' shows form to display annotation for the claim
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim AnnotateForm As AnnotationDialog
        Dim AddDR As DataRow
        Dim DT As DataTable

        Try

            AnnotateForm = New AnnotationDialog(_ClaimID, CInt(_ClaimDr("FAMILY_ID")), CInt(_ClaimDr("RELATION_ID")), CInt(_ClaimDr("PART_SSN")), CInt(_ClaimDr("PAT_SSN")), _ClaimDr("PART_FNAME").ToString, _ClaimDr("PART_LNAME").ToString, _ClaimDr("PAT_FNAME").ToString, _ClaimDr("PAT_LNAME").ToString, _ClaimDS.ANNOTATIONS)

            If AnnotateForm.ShowDialog(Me) = DialogResult.OK Then
                DT = AnnotateForm.AnnotationsTable.GetChanges(DataRowState.Added)

                If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                    For Each DR As DataRow In DT.Rows
                        AddDR = _ClaimDS.ANNOTATIONS.NewRow

                        For Each DC As DataColumn In _ClaimDS.ANNOTATIONS.Columns
                            If DC.AutoIncrement = False Then
                                AddDR.Item(DC.ColumnName) = DR(DC.ColumnName)
                            End If
                        Next

                        _ClaimDS.ANNOTATIONS.Rows.Add(AddDR)
                    Next
                End If
            End If

        Catch ex As Exception
            Throw
        Finally
            AnnotateForm.Dispose()

        End Try
    End Sub

    Private Sub AcceptAnnotationChanges(sender As Object, e As AnnotationsEvent) Handles AnnotationControl.Save

        Dim AddDR As DataRow
        Dim DT As DataTable

        DT = AnnotationControl.Annotations.GetChanges(DataRowState.Added)

        If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
            For Each DR As DataRow In DT.Rows
                AddDR = _ClaimDS.ANNOTATIONS.NewRow

                For Each DC As DataColumn In _ClaimDS.ANNOTATIONS.Columns
                    If DC.AutoIncrement = False Then
                        AddDR.Item(DC.ColumnName) = DR(DC.ColumnName)
                    End If
                Next

                _ClaimDS.ANNOTATIONS.Rows.Add(AddDR)
            Next
        End If

    End Sub

    Private Sub FloatAlerts()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' increases the size of the alerts box to show more alerts and allow a more friendly
        ' navaigation of alerts
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            If AlertsImageListBox.Items.Count > 2 Then
                AlertsImageListBox.Height = _NormalHeight * 4
                AlertsImageListBox.BorderStyle = BorderStyle.FixedSingle

                AlertsBorder.BackColor = System.Drawing.SystemColors.ControlDark

                AlertsBorder.Left = AlertsImageListBox.Left - 5
                AlertsBorder.Top = AlertsImageListBox.Top - 5
                AlertsBorder.Width = Me.Width - AlertsImageListBox.Left - 3
                AlertsBorder.Height = AlertsImageListBox.Height + 10

                AlertsBorder.Visible = True
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub HideAlerts()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' decreases the size of the alerts box to save space on the form
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            AlertsBorder.Visible = False

            AlertsImageListBox.Height = _NormalHeight
            AlertsImageListBox.BorderStyle = BorderStyle.Fixed3D

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SyncAllowed(Optional ByVal detailLine As Short = -1)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Syncs up the lower of Charged & Priced
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	10/13/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DV As DataView
        Dim AlertDV As DataView

        Try

            If detailLine <> -1 Then
                DV = New DataView(_ClaimDS.MEDDTL, "LINE_NBR = " & detailLine, "LINE_NBR", DataViewRowState.CurrentRows)
            Else
                DV = New DataView(_ClaimDS.MEDDTL, "", "", DataViewRowState.CurrentRows)
            End If

            AlertDV = New DataView(_ClaimAlertManager.AlertManagerDataTable, "", "LineNumber, Category", DataViewRowState.CurrentRows)

            For Cnt As Integer = 0 To DV.Count - 1
                'If NotElig = False Then 'Elig
                If Not IsDBNull(DV(Cnt)("PRICED_AMT")) AndAlso Not IsDBNull(DV(Cnt)("CHRG_AMT")) Then
                    If CDec(DV(Cnt)("PRICED_AMT")) < CDec(DV(Cnt)("CHRG_AMT")) Then
                        DV(Cnt).Row("ALLOWED_AMT") = DV(Cnt)("PRICED_AMT")
                    Else
                        DV(Cnt).Row("ALLOWED_AMT") = DV(Cnt)("CHRG_AMT")
                    End If
                ElseIf Not IsDBNull(DV(Cnt)("PRICED_AMT")) AndAlso IsDBNull(DV(Cnt)("CHRG_AMT")) Then
                    DV(Cnt).Row("ALLOWED_AMT") = DV(Cnt)("PRICED_AMT")
                ElseIf IsDBNull(DV(Cnt)("PRICED_AMT")) AndAlso Not IsDBNull(DV(Cnt)("CHRG_AMT")) Then
                    DV(Cnt).Row("ALLOWED_AMT") = DV(Cnt)("CHRG_AMT")
                ElseIf IsDBNull(DV(Cnt)("PRICED_AMT")) AndAlso IsDBNull(DV(Cnt)("CHRG_AMT")) Then
                    DV(Cnt).Row("ALLOWED_AMT") = DBNull.Value
                End If
            Next
            _MedDtlBS.EndEdit()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function SumCharges() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' totals charges and loads it into total charges textbox
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	9/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Total As Decimal = 0
        Try

            Total = _ClaimDS.Tables("MEDDTL").AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso DirectCast(r("STATUS"), [String]) <> "MERGED" AndAlso r.Field(Of Decimal?)("CHRG_AMT") IsNot Nothing).Sum(Function(r) DirectCast(r("CHRG_AMT"), [Decimal]))

            txtTotalChargedAmt.Text = Format(Total, "0.00;-0.00")

            Return Total

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function SumPaid() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' totals Paid Amounts and loads it into total paid textbox
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	9/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Total As Decimal = 0

        Try

#If TRACE Then
            If CInt(_TraceParallel.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & " Rows -> " & _ClaimDS.MEDDTL.Rows.Count.ToString & " : " & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If

            Total = _ClaimDS.Tables("MEDDTL").AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso DirectCast(r("STATUS"), [String]) = "PAY" AndAlso r.Field(Of Decimal?)("PAID_AMT") IsNot Nothing).Sum(Function(r) DirectCast(r("PAID_AMT"), [Decimal]))

            txtTotalPaidAmt.Text = Format(Total, "0.00;-0.00")
            Return Total

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function SumPriced() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' totals Priced Amounts and loads it into total priced textbox
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	9/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim Total As Decimal = 0

        Try

            Total = _ClaimDS.Tables("MEDDTL").AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso DirectCast(r("STATUS"), [String]) <> "MERGED" AndAlso r.Field(Of Decimal?)("PRICED_AMT") IsNot Nothing).Sum(Function(r) DirectCast(r("PRICED_AMT"), [Decimal]))

            Return Total

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function SumAllowed() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' totals allowed amounts
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	10/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim SummaryPriced As Boolean = False

        Try
            SummaryPriced = _MedHdrDr("PRICED_BY").ToString.Contains(" (S)")

            Dim QuerySUMAllowedAmt As Decimal = _ClaimDS.Tables("MEDDTL").AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso If(SummaryPriced, DirectCast(r("STATUS"), [String]) = "PAY", DirectCast(r("STATUS"), [String]) <> "MERGED") AndAlso r.Field(Of Decimal?)("ALLOWED_AMT") IsNot Nothing).Sum(Function(r) DirectCast(r("ALLOWED_AMT"), [Decimal]))
            txtTotalAllowedAmt.Text = Format(QuerySUMAllowedAmt, "0.00;-0.00")

            Return QuerySUMAllowedAmt

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function SumOI() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' totals other insurance amounts
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	9/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim Total As Decimal = 0

        Try

            If _MedDtlBS IsNot Nothing AndAlso _MedDtlBS.Count > 0 Then
                Total = _ClaimDS.Tables("MEDDTL").AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso DirectCast(r("STATUS"), [String]) <> "MERGED" AndAlso r.Field(Of Decimal?)("OTH_INS_AMT") IsNot Nothing).Sum(Function(r) DirectCast(r("OTH_INS_AMT"), [Decimal]))
            Else
                If _MedHdrDr IsNot Nothing AndAlso Not IsDBNull(_MedHdrDr("TOT_OTH_INS_AMT")) Then
                    Total = CDec(_MedHdrDr("TOT_OTH_INS_AMT"))
                End If
            End If

            txtTotalOIAmt.Text = Format(Total, "0.00;-0.00")

            Return Total

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function SumProcessed() As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' totals processed amounts
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	9/25/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim Total As Decimal = 0

        Try
            Total = _ClaimDS.Tables("MEDDTL").AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso DirectCast(r("STATUS"), [String]) <> "MERGED" AndAlso r.Field(Of Decimal?)("PROCESSED_AMT") IsNot Nothing).Sum(Function(r) DirectCast(r("PROCESSED_AMT"), [Decimal]))
            Return Total
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub DeleteDetailLine()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Deletes a detail line and adjusts any detail lines above the deleted
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	10/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DV As DataView
        Dim SupportDV As DataView
        Dim LineNum As Short

        Dim DGRow As DataRow
        Dim BS As BindingSource
        Try
            DV = DetailLinesDataGrid.GetDefaultDataView

            If DetailLinesDataGrid.DataSource Is Nothing Then Return

            BS = DirectCast(DetailLinesDataGrid.DataSource, BindingSource)
            If BS IsNot Nothing AndAlso BS.Current IsNot Nothing Then
                DGRow = DirectCast(BS.Current, DataRowView).Row
            End If
            If DGRow Is Nothing OrElse (DGRow.RowState <> DataRowState.Added) Then Return

            LineNum = CShort(DGRow("LINE_NBR"))

            If MessageBox.Show(Me, "Are you sure you want to Delete Line " & LineNum & "?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                _ClaimAlertManager.ClearAlertRows()

                DGRow.Delete()
                _ClaimAlertManager.AddAlertRow(New Object() {"Re-Calc Is Required", 0, "Header", 30})

                BS.EndEdit()

                If BS.Count > 0 AndAlso BS.Position + 1 < BS.Count Then
                    BS.MoveNext()
                    _ClaimAlertManager.AddAlertRow(New Object() {"Re-Calc Is Required", 0, "Header", 30})
                End If

                If BS.Position > -1 Then
                    DetailLinesDataGrid.Select(BS.Position)
                    DetailLineGroupBox.Text = "Edit Item - " & (BS.Position + 1).ToString
                Else
                    DetailLineGroupBox.Text = ""
                End If

                'If (DGRow("STATUS").ToString.Trim = "MERGED" OrElse (_MedHdrDr("PRICED_BY").ToString.Contains("JAA") OrElse _MedHdrDr("PRICED_BY").ToString.Contains("835"))) Then
                '    DeleteButton.Visible = False
                '    DeleteButton.Enabled = False
                '    AddButton.Visible = False
                '    AddButton.Enabled = False
                'Else

                '    AddButton.Visible = True
                '    AddButton.Enabled = True

                '    If DGRow.RowState = DataRowState.Added Then
                '        DeleteButton.Visible = True
                '        DeleteButton.Enabled = True
                '    Else
                '        DeleteButton.Visible = False
                '        DeleteButton.Enabled = False
                '    End If
                'End If



                Dim QueryDeleteMEDDIAG = _ClaimDS.MEDDIAG.AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = LineNum)

                For Each Row As DataRow In QueryDeleteMEDDIAG.ToList()
                    Row.Delete()
                Next

                Dim QueryDeleteMEDMOD = _ClaimDS.MEDMOD.AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = LineNum)

                For Each Row As DataRow In QueryDeleteMEDMOD.ToList()
                    Row.Delete()
                Next

                Dim QueryDeleteREASON = _ClaimDS.REASON.AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = LineNum)

                For Each Row As DataRow In QueryDeleteREASON.ToList()
                    Row.Delete()
                Next

                DV = New DataView(_ClaimDS.MEDDTL, "LINE_NBR >= " & LineNum, "LINE_NBR", DataViewRowState.CurrentRows)

                If DV.Count > 0 Then
                    For Cnt As Integer = 0 To DV.Count - 1

                        SupportDV = New DataView(_ClaimDS.MEDDIAG, "LINE_NBR = " & DV(Cnt).Row.Item("LINE_NBR").ToString, "LINE_NBR", DataViewRowState.CurrentRows)
                        Do Until SupportDV.Count = 0
                            SupportDV(0).Row.Item("LINE_NBR") = CInt(DV(Cnt).Row.Item("LINE_NBR")) - 1
                        Loop

                        SupportDV = New DataView(_ClaimDS.MEDMOD, "LINE_NBR = " & DV(Cnt).Row.Item("LINE_NBR").ToString, "LINE_NBR", DataViewRowState.CurrentRows)
                        Do Until SupportDV.Count = 0
                            SupportDV(0).Row.Item("LINE_NBR") = CInt(DV(Cnt).Row.Item("LINE_NBR")) - 1
                        Loop

                        SupportDV = New DataView(_ClaimDS.REASON, "LINE_NBR = " & DV(Cnt).Row.Item("LINE_NBR").ToString, "LINE_NBR", DataViewRowState.CurrentRows)
                        Do Until SupportDV.Count = 0
                            SupportDV(0).Row.Item("LINE_NBR") = CInt(DV(Cnt).Row.Item("LINE_NBR")) - 1
                        Loop

                        DV(Cnt).Row.Item("LINE_NBR") = CInt(DV(Cnt).Row.Item("LINE_NBR")) - 1
                    Next

                End If
            End If
            BS.EndEdit()
            BS.ResetBindings(False)

            If _MedHdrBS.Count < 1 OrElse _MedDtlBS.Count < 1 Then
                Me.CompleteToolBarButton.Enabled = False
            Else
                Me.CompleteToolBarButton.Enabled = True
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub AddDetailLine()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' adds a detail Line
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	10/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim DR As DataRow
        'Dim CM As CurrencyManager
        Dim BS As BindingSource
        Try
            If Not AddButton.Enabled Then Return

            DetailLinesDataGrid.SuspendLayout()

            DR = _ClaimDS.MEDDTL.NewRow

            DR("CLAIM_ID") = _ClaimDr("CLAIM_ID")
            DR("LINE_NBR") = _ClaimDS.MEDDTL.Rows.Count + 1
            DR("SECURITY_SW") = _ClaimDr("SECURITY_SW")
            DR("FAMILY_ID") = _ClaimDr("FAMILY_ID")
            DR("RELATION_ID") = _ClaimDr("RELATION_ID")
            DR("PART_SSN") = _ClaimDr("PART_SSN")
            DR("PAT_SSN") = _ClaimDr("PAT_SSN")
            DR("MAXID") = _ClaimDr("MAXID")
            DR("MAXID_LINE_NBR") = _ClaimDS.MEDDTL.Rows.Count + 1

            DR("STATUS") = "NEW"
            DR("STATUS_DATE") = UFCWGeneral.NowDate
            DR("PRICING_ERROR") = DBNull.Value
            DR("PRICING_REASON") = DBNull.Value
            DR("OCC_FROM_DATE") = DBNull.Value
            DR("OCC_TO_DATE") = DBNull.Value

            DR("PLACE_OF_SERV") = DBNull.Value
            DR("PLACE_OF_SERV_DESC") = DBNull.Value
            DR("BILL_TYPE") = DBNull.Value
            DR("BILL_TYPE_DESC") = DBNull.Value
            DR("PROC_CODE") = ""
            DR("PROC_CODE_DESC") = "***INVALID PROCEDURE CODE***"
            DR("MODIFIER_SW") = False
            DR("REASON_SW") = False
            DR("DIAG_SW") = False
            DR("LOCAL_USE") = DBNull.Value
            DR("DAYS_UNITS") = DBNull.Value
            DR("MED_PLAN") = "" 'DBNull.Value
            DR("MEMTYPE") = DBNull.Value
            DR("DUPLICATE_SW") = False

            If _ClaimDS.MEDHDR.Rows.Count > 0 Then
                DR("SYSTEM_CODE") = _MedHdrDr("SYSTEM_CODE")
                DR("INCIDENT_DATE") = _MedHdrDr("INCIDENT_DATE")

                DR("NON_PAR_SW") = _MedHdrDr("NON_PAR_SW")
                DR("OUT_OF_AREA_SW") = _MedHdrDr("OUT_OF_AREA_SW")
                DR("AUTO_ACCIDENT_SW") = _MedHdrDr("AUTO_ACCIDENT_SW")
                DR("WORKERS_COMP_SW") = _MedHdrDr("WORKERS_COMP_SW")
                DR("OTH_ACCIDENT_SW") = _MedHdrDr("OTH_ACCIDENT_SW")
                DR("OTH_INS_SW") = _MedHdrDr("OTH_INS_SW")
            Else
                DR("SYSTEM_CODE") = 0
                DR("INCIDENT_DATE") = DBNull.Value

                DR("NON_PAR_SW") = 0
                DR("OUT_OF_AREA_SW") = 0
                DR("AUTO_ACCIDENT_SW") = 0
                DR("WORKERS_COMP_SW") = 0
                DR("OTH_ACCIDENT_SW") = 0
                DR("OTH_INS_SW") = 0
            End If

            DR("CHRG_AMT") = DBNull.Value
            DR("PRICED_AMT") = DBNull.Value
            DR("ALLOWED_AMT") = DBNull.Value
            DR("OTH_INS_AMT") = DBNull.Value
            DR("PAID_AMT") = DBNull.Value
            DR("PROCESSED_AMT") = DBNull.Value
            DR("OVERRIDE_SW") = 0
            DR("PROC_ID") = DBNull.Value
            DR("CHECK_SW") = 0
            DR("CHK_NBR") = DBNull.Value
            DR("CHK_DATE") = DBNull.Value
            DR("CLOSED_DATE") = DBNull.Value
            DR("ADJUSTER") = DBNull.Value
            DR("CREATE_USERID") = _DomainUser.ToUpper
            DR("CREATE_DATE") = UFCWGeneral.NowDate
            DR("USERID") = _DomainUser.ToUpper
            DR("LASTUPDT") = UFCWGeneral.NowDate

            DR("DIAGNOSIS") = DBNull.Value
            DR("MODIFIER") = DBNull.Value

            '  ClearDetailLineDatabindings()

            _ClaimDS.MEDDTL.Rows.Add(DR)

            'LoadDetailLineDataBindings()

            'add alerts
            _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Invalid Procedure Code", DR("LINE_NBR").ToString, "Detail", 30})
            _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required", DR("LINE_NBR").ToString, "Detail", 30})

            DetailLineGroupBox.Text = "Edit Item - " & DR("LINE_NBR").ToString

            'CM = CType(Me.BindingContext(DetailLinesDataGrid.DataSource), CurrencyManager)
            'CM.EndCurrentEdit()

            ' If CM.Position <> _ClaimDS.MEDDTL.Rows.Count - 1 Then CM.Position = _ClaimDS.MEDDTL.Rows.Count - 1
            BS = DirectCast(DetailLinesDataGrid.DataSource, BindingSource)
            BS.EndEdit()
            BS.ResetBindings(False)
            BS.MoveLast()
            DetailLinesDataGrid.Select(BS.Position)

        Catch ex As Exception
            Throw
        Finally

            DetailLinesDataGrid.ResumeLayout()

            If _MedHdrBS.Count < 1 OrElse _MedDtlBS.Count < 1 Then
                Me.CompleteToolBarButton.Enabled = False
            Else
                Me.CompleteToolBarButton.Enabled = True
            End If

        End Try
    End Sub

    Private Sub ConsolidateDetailLines()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' adds a detail Line
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	10/5/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DR As DataRow
        Dim DV As DataView
        Dim PlansDT As DataTable
        Dim BillTypesDT As DataTable
        Dim PlaceOfServiceDT As DataTable
        Dim AmountsDT As DataTable
        Dim DSHelper As DataSetHelper
        Dim HighDV As DataView
        Dim ConsolidatedDV As DataView

        Try

            If _MedDtlDR("STATUS").ToString.Trim = "MERGED" AndAlso CStr(_ClaimDr("DOC_TYPE")).ToUpper.Contains("HOSPITAL") AndAlso _Mode.ToUpper <> "AUDIT" AndAlso Not _MedHdrDr("PRICED_BY").ToString.Contains("JAA") Then

                If MessageBox.Show("Would you like to reverse the prior merge?", "Reverse Merge", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                    ConsolidatedDV = DetailLinesDataGrid.GetCurrentDataView
                    _ClaimAlertManager.ClearDetailAlertRows()

                    ConsolidatedDV.AllowEdit = True

                    MassUpdateField("STATUS", "NEW", True)

                    AddButton.Visible = True
                    AddButton.Enabled = True

                    ConsolidatedDV.AllowDelete = True

                    Try
                        ConsolidatedDV(ConsolidatedDV.Count - 1).Delete()

                    Catch ex As Exception
                        Try
                            ConsolidatedDV(ConsolidatedDV.Count - 1).Delete()

                        Catch ex2 As Exception
                            ' Stop
                        End Try
                    End Try
                    _MedDtlBS.MoveFirst()
                    _MedDtlBS.EndEdit()
                    'DetailLinesDataGrid.CurrentRowIndex = 0
                    'Me.BindingContext(_ClaimDS.MEDDTL).EndCurrentEdit()

                End If

            Else

                DSHelper = New DataSetHelper(_ClaimDS)

                'Establish mode PLAN
                PlansDT = DSHelper.CreateGroupByTable("MEDPLAN", _ClaimDS.Tables("MEDDTL"), "MED_PLAN ,COUNT(MED_PLAN) PLAN_TOTAL_LINES")
                DSHelper.InsertGroupByInto(_ClaimDS.Tables("MEDPLAN"), _ClaimDS.Tables("MEDDTL"), "MED_PLAN ,COUNT(MED_PLAN) PLAN_TOTAL_LINES", "", "MED_PLAN")
                PlansDT.DefaultView.Sort = "MED_PLAN DESC"

                If PlansDT.Rows.Count > 1 Then
                    Throw New ArgumentException("Merge cannot be performed on a claim using multiple plans")
                End If

                If (_MedHdrDr("PRICED_BY").ToString.Contains("JAA") OrElse _MedHdrDr("PRICED_BY").ToString.Contains("835")) AndAlso Not _MedHdrDr("PRICED_BY").ToString.Contains("(S)") Then
                    If Not (MessageBox.Show("Merging the lines of this claim will result in claim failing to close in Anthem (WGS)" & vbCrLf & vbCrLf & " Do you wish to continue merging the claim lines?", "Anthem Merge warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes) Then
                        Exit Sub
                    End If
                    _ClaimAlertManager.AddAlertRow(New Object() {"Merge process prevents Auto WGS Close from completing", 0, "Header", 30})
                End If

                _ClaimAlertManager.ClearDetailAlertRows()

                MassUpdateField("STATUS", "MERGED")

                DV = DetailLinesDataGrid.GetCurrentDataView

                DR = _ClaimDS.MEDDTL.NewRow

                'Establish mode PlaceOfService
                PlaceOfServiceDT = DSHelper.CreateGroupByTable("PLACE_OF_SERV", _ClaimDS.Tables("MEDDTL"), "PLACE_OF_SERV ,COUNT(PLACE_OF_SERV) PLACE_OF_SERV_TOTAL_LINES")
                DSHelper.InsertGroupByInto(_ClaimDS.Tables("PLACE_OF_SERV"), _ClaimDS.Tables("MEDDTL"), "PLACE_OF_SERV ,COUNT(PLACE_OF_SERV) PLACE_OF_SERV_TOTAL_LINES", "", "PLACE_OF_SERV")
                PlaceOfServiceDT.DefaultView.Sort = "PLACE_OF_SERV DESC"

                'Establish mode BillType
                BillTypesDT = DSHelper.CreateGroupByTable("BILLTYPE", _ClaimDS.Tables("MEDDTL"), "BILL_TYPE ,COUNT(BILL_TYPE) BILL_TYPE_TOTAL_LINES")
                DSHelper.InsertGroupByInto(_ClaimDS.Tables("BILLTYPE"), _ClaimDS.Tables("MEDDTL"), "BILL_TYPE ,COUNT(BILL_TYPE) BILL_TYPE_TOTAL_LINES", "", "BILL_TYPE")
                BillTypesDT.DefaultView.Sort = "BILL_TYPE DESC"

                'Sum Amounts from line items
                AmountsDT = DSHelper.CreateGroupByTable("Amounts", _ClaimDS.Tables("MEDDTL"), "CLAIM_ID, SUM(CHRG_AMT) TOT_CHRG_AMT_SUM, SUM(PRICED_AMT) TOT_PRICED_AMT_SUM,SUM(ALLOWED_AMT) TOT_ALLOWED_AMT_SUM,SUM(OTH_INS_AMT) TOT_OTH_INS_AMT_SUM,SUM(PAID_AMT) TOT_PAID_AMT_SUM,SUM(PROCESSED_AMT) TOT_PROCESSED_AMT_SUM")
                DSHelper.InsertGroupByInto(_ClaimDS.Tables("Amounts"), _ClaimDS.Tables("MEDDTL"), "CLAIM_ID, SUM(CHRG_AMT) TOT_CHRG_AMT_SUM, SUM(PRICED_AMT) TOT_PRICED_AMT_SUM, SUM(ALLOWED_AMT) TOT_ALLOWED_AMT_SUM, SUM(OTH_INS_AMT) TOT_OTH_INS_AMT_SUM, SUM(PAID_AMT) TOT_PAID_AMT_SUM,SUM(PROCESSED_AMT) TOT_PROCESSED_AMT_SUM", "", "CLAIM_ID")

                DR("CLAIM_ID") = _ClaimDr("CLAIM_ID")

                HighDV = New DataView(_ClaimDS.MEDDTL, "MAX(LINE_NBR) = LINE_NBR", "", DataViewRowState.CurrentRows)

                DR("LINE_NBR") = CInt(HighDV(0)("LINE_NBR")) + 1

                DR("SECURITY_SW") = _ClaimDr("SECURITY_SW")
                DR("FAMILY_ID") = _ClaimDr("FAMILY_ID")
                DR("RELATION_ID") = _ClaimDr("RELATION_ID")
                DR("PART_SSN") = _ClaimDr("PART_SSN")
                DR("PAT_SSN") = _ClaimDr("PAT_SSN")
                DR("MAXID") = _ClaimDr("MAXID")
                DR("MAXID_LINE_NBR") = DR("LINE_NBR")

                DR("STATUS") = "NEW"
                DR("STATUS_DATE") = UFCWGeneral.NowDate
                DR("PRICING_ERROR") = DBNull.Value
                DR("PRICING_REASON") = DBNull.Value

                EstablishDOSRange(_ClaimDS.CLAIM_MASTER.NewCLAIM_MASTERRow, DR)

                DR("PLACE_OF_SERV") = PlaceOfServiceDT.DefaultView(0)(0) 'Assign most common PlaceOfService
                DR("PLACE_OF_SERV_DESC") = DBNull.Value

                DR("BILL_TYPE") = BillTypesDT.DefaultView(0)(0)

                DR("BILL_TYPE_DESC") = DBNull.Value
                DR("PROC_CODE") = ""
                DR("PROC_CODE_DESC") = "***INVALID PROCEDURE CODE***"
                DR("MODIFIER_SW") = False
                DR("REASON_SW") = False
                DR("DIAG_SW") = False
                DR("LOCAL_USE") = DBNull.Value
                DR("DIAGNOSIS") = DBNull.Value 'Diagnoses.DefaultView(0)(0) 'Assign most common DIAGNOSIS
                DR("MODIFIER") = DBNull.Value 'Modifiers.DefaultView(0)(0) 'Assign most common MODIFIER

                DR("DAYS_UNITS") = "1.00"
                DR("MED_PLAN") = PlansDT.DefaultView(0)(0) 'Assign most common plan
                DR("MEMTYPE") = DBNull.Value
                DR("DUPLICATE_SW") = False
                DR("SYSTEM_CODE") = _MedHdrDr("SYSTEM_CODE")
                DR("INCIDENT_DATE") = _MedHdrDr("INCIDENT_DATE")
                DR("NON_PAR_SW") = _MedHdrDr("NON_PAR_SW")
                DR("OUT_OF_AREA_SW") = _MedHdrDr("OUT_OF_AREA_SW")
                DR("AUTO_ACCIDENT_SW") = _MedHdrDr("AUTO_ACCIDENT_SW")
                DR("WORKERS_COMP_SW") = _MedHdrDr("WORKERS_COMP_SW")
                DR("OTH_ACCIDENT_SW") = _MedHdrDr("OTH_ACCIDENT_SW")
                DR("OTH_INS_SW") = _MedHdrDr("OTH_INS_SW")
                DR("CHRG_AMT") = AmountsDT.Rows(0)("TOT_CHRG_AMT_SUM")
                DR("PRICED_AMT") = AmountsDT.Rows(0)("TOT_PRICED_AMT_SUM")
                DR("ALLOWED_AMT") = AmountsDT.Rows(0)("TOT_ALLOWED_AMT_SUM")
                DR("OTH_INS_AMT") = AmountsDT.Rows(0)("TOT_OTH_INS_AMT_SUM")
                DR("PAID_AMT") = AmountsDT.Rows(0)("TOT_PAID_AMT_SUM")
                DR("PROCESSED_AMT") = AmountsDT.Rows(0)("TOT_PROCESSED_AMT_SUM")
                DR("OVERRIDE_SW") = 0
                DR("PROC_ID") = DBNull.Value
                DR("CHECK_SW") = False
                DR("CHK_NBR") = DBNull.Value
                DR("CHK_DATE") = DBNull.Value
                DR("CLOSED_DATE") = DBNull.Value
                DR("ADJUSTER") = DBNull.Value
                DR("CREATE_USERID") = _DomainUser.ToUpper
                DR("CREATE_DATE") = UFCWGeneral.NowDate
                DR("USERID") = _DomainUser.ToUpper
                DR("LASTUPDT") = UFCWGeneral.NowDate

                ' ClearDetailLineDatabindings()
                _ClaimDS.MEDDTL.Rows.Add(DR)
                'LoadDetailLineDataBindings()

                'add alerts
                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Summary Line is marked as NEW and must be completed.", DR("LINE_NBR").ToString, "Detail", 30})
                _MedDtlBS.RaiseListChangedEvents = False
                _MedDtlBS.Filter = "LINE_NBR=" & CInt(DR("LINE_NBR"))

                'dv = DetailLinesDataGrid.GetDefaultDataView
                'Dim Cnt As Integer
                'For cnt = 0 To dv.Count - 1
                '    If CInt(dv(cnt)("LINE_NBR")) = CInt(DR("LINE_NBR")) Then
                '        DetailLineGroupBox.Text = "Edit Item - " & DR("LINE_NBR").ToString
                '        DetailLinesDataGrid.CurrentRowIndex = cnt
                '    End If
                'Next

                Me.CompleteToolBarButton.Enabled = False

                txtProcedure.Focus()

            End If

        Catch ex As ArgumentException
            Throw
        Catch ex As Exception
            MassUpdateField("STATUS", "NEW", True)
            Throw
        Finally

            If _ClaimDS.Tables("MEDPLAN") IsNot Nothing Then
                _ClaimDS.Tables.Remove("MEDPLAN")
            End If

            If _ClaimDS.Tables("PLACE_OF_SERV") IsNot Nothing Then
                _ClaimDS.Tables.Remove("PLACE_OF_SERV")
            End If

            If _ClaimDS.Tables("BILLTYPE") IsNot Nothing Then
                _ClaimDS.Tables.Remove("BILLTYPE")
            End If

            If _ClaimDS.Tables("Amounts") IsNot Nothing Then
                _ClaimDS.Tables.Remove("Amounts")
            End If
        End Try
    End Sub

    Private Function RouteClaim() As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' shows a route dialog to route teh claim to another user
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	12/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Transaction As DbTransaction
        Dim RouteDia As RouteDialog
        Dim HistDetail As String

        Try
            RouteDia = New RouteDialog(_ClaimID, CStr(_ClaimDr("DOC_TYPE")), _Mode.ToUpper)

            If RouteDia.ShowDialog(Me) = DialogResult.OK Then

                Transaction = CMSDALCommon.BeginTransaction

                If HasChanges() Then
                    If Not SaveChanges(Transaction) Then
                        CMSDALCommon.RollbackTransaction(Transaction)
                        Return False
                    End If
                End If

                If _Mode.ToUpper = "AUDIT" Then
                    CMSDALFDBMD.PendToAuditor(_ClaimID, RouteDia.Recipient.Trim.ToUpper, Transaction)
                Else
                    CMSDALFDBMD.PendToUser(_ClaimID, RouteDia.Recipient.Trim.ToUpper, Transaction)
                End If

                CMSDALFDBMD.UpdateClaimMasterRouting(_ClaimID, _DomainUser.ToUpper, Transaction)

                Dim HistSum As String = "CLAIM ID " & Format(_ClaimID, "00000000") & " REASSIGNED TO " & RouteDia.Recipient.ToUpper
                If _Mode.ToUpper = "AUDIT" Then
                    HistSum = "CLAIM ID " & Format(_ClaimID, "00000000") & " REASSIGNED TO AUDITOR " & RouteDia.Recipient.ToUpper
                Else
                    HistSum = "CLAIM ID " & Format(_ClaimID, "00000000") & " REASSIGNED TO " & RouteDia.Recipient.ToUpper
                End If

                HistDetail = RouteDia.Comment.ToUpper

                CMSDALFDBMD.CreateDocHistory(_ClaimID, UFCWGeneral.IsNullLongHandler(_ClaimDr("DOCID"), "DOCID"), "ROUTE",
                                                                UFCWGeneral.IsNullIntegerHandler(_ClaimDr("FAMILY_ID")), UFCWGeneral.IsNullShortHandler(_ClaimDr("RELATION_ID")),
                                                                UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PART_SSN")), UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PAT_SSN")),
                                                                UFCWGeneral.IsNullStringHandler(_ClaimDr("DOC_CLASS")),
                                                                UFCWGeneral.IsNullStringHandler(_ClaimDr("DOC_TYPE")),
                                                                HistSum, HistDetail, _DomainUser.ToUpper, Transaction)
                CMSDALFDBMD.CreateRoutingHistory(_ClaimID, _DomainUser.ToUpper, RouteDia.Recipient.Trim.ToUpper, RouteDia.Comment.ToUpper, _DomainUser.ToUpper, Transaction)
                CMSDALCommon.CommitTransaction(Transaction)
                Return True
            End If

            Return False

        Catch ex As Exception
            If Transaction IsNot Nothing Then
                CMSDALCommon.RollbackTransaction(Transaction)
            End If
            Throw
        Finally
            If RouteDia IsNot Nothing Then RouteDia.Dispose()

        End Try
    End Function

    Private Function CheckForUserDocTypeSecurity(ByVal User As String, ByVal DocClass As String, ByVal DocType As String) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' checks if the user has security for a given docclass and type
        ' </summary>
        ' <param name="User"></param>
        ' <param name="DocClass"></param>
        ' <param name="DocType"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	12/14/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DV As DataView

        Try
            DV = New DataView(CMSDALFDBMD.RetrieveUserDocTypes(User), "DOC_CLASS = '" & DocClass & "' And DOC_TYPE = '" & DocType & "'", "DOC_CLASS, DOC_TYPE", DataViewRowState.CurrentRows)
            If DV.Count > 0 Then
                Return True
            End If

            Return False

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Shared Function IsFamilyLocked(ByVal FamilyID As Integer, Optional ByVal Transaction As DbTransaction = Nothing) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="FamilyID"></param>
        ' <param name="Transaction"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/27/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DT As DataTable
        Try
            dt = CMSDALFDBMD.RetrieveFamilyLock(FamilyID, Transaction)
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Return True
            End If
            Return False
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function CompleteProcessing(ByRef transaction As DbTransaction) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' updates status and commits changes
        ' </summary>
        ' <param name="Transaction"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim HistSum As String
        Dim HistDetail As String

        Try
            Using WC As New GlobalCursor

                If _Mode.ToUpper = "AUDIT" Then
                    If _AuditForm.Status.ToUpper = "RELEASE" Then 'AndAlso HasChanges() = False Then
                        'RELEASE
                        CMSDALFDBMD.UpdateClaimMasterStatus(_ClaimID, "WAITPROCESSING", _DomainUser.ToUpper, transaction)

                        HistSum = "CLAIM ID " & Format(_ClaimID, "00000000") & " IS COMPLETE"
                        HistDetail = "AUDITOR " & _DomainUser.ToUpper & " RELEASED THIS DOCUMENT FROM AUDIT." & Microsoft.VisualBasic.vbCrLf &
                                                    "ENDING WORKFLOW PROCESSING."
                        CMSDALFDBMD.CreateDocHistory(_ClaimID, UFCWGeneral.IsNullLongHandler(_ClaimDr("DOCID"), "DOCID"), "AUDITRELEASE",
                                                     UFCWGeneral.IsNullIntegerHandler(_ClaimDr("FAMILY_ID")), UFCWGeneral.IsNullShortHandler(_ClaimDr("RELATION_ID")),
                                                     UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PART_SSN")), UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PAT_SSN")),
                                                     UFCWGeneral.IsNullStringHandler(_ClaimDr("DOC_CLASS")), UFCWGeneral.IsNullStringHandler(_ClaimDr("DOC_TYPE")),
                                                     HistSum, HistDetail, _DomainUser.ToUpper, transaction)
                    Else
                        'AUDITED
                        CMSDALFDBMD.UpdateClaimMasterStatus(_ClaimID, "INPROGRESS", _DomainUser.ToUpper, transaction)

                        Dim Checks As CheckBox()
                        Dim AuditText As String = ""

                        HistSum = "CLAIM ID " & Format(_ClaimID, "00000000") & " HAS BEEN AUDITED"
                        HistDetail = "AUDITOR " & _DomainUser.ToUpper & " COMPLETED AUDITING THIS ITEM." & Microsoft.VisualBasic.vbCrLf &
                                                    "THE FOLLOWING AUDITS WERE SELECTED:"

                        Checks = _AuditForm.Audits
                        For Cnt As Integer = 0 To UBound(Checks, 1) - 1
                            If Checks(Cnt).Checked = True Then
                                If Checks(Cnt).Text.ToUpper = "OTHER" Then
                                    AuditText = _AuditForm.OtherText
                                Else
                                    AuditText = Checks(Cnt).Text
                                End If
                                HistDetail &= Microsoft.VisualBasic.vbCrLf & AuditText
                            End If
                        Next

                        CMSDALFDBMD.CreateDocHistory(_ClaimID, UFCWGeneral.IsNullLongHandler(_ClaimDr("DOCID"), "DOCID"), "AUDITRETURN",
                                                            UFCWGeneral.IsNullIntegerHandler(_ClaimDr("FAMILY_ID")), UFCWGeneral.IsNullShortHandler(_ClaimDr("RELATION_ID")),
                                                            UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PART_SSN")), UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PAT_SSN")),
                                                            UFCWGeneral.IsNullStringHandler(_ClaimDr("DOC_CLASS")), UFCWGeneral.IsNullStringHandler(_ClaimDr("DOC_TYPE")),
                                                            HistSum, HistDetail, _DomainUser.ToUpper, transaction)
                    End If
                Else
                    If _ClaimMemberAccumulatorManager IsNot Nothing Then
                        _ClaimMemberAccumulatorManager.CommitAll(transaction)
                    Else
                        'Build?
                    End If

                    If ShouldClaimBeMovedToAuditQueue(_ClaimDS.CLAIM_MASTER, _ClaimDS.MEDHDR, _ClaimDS.MEDDTL) Then
                        'AUDIT
                        _ClaimDr("STATUS") = "AUDIT"

                        HistSum = "CLAIM ID " & Format(_ClaimID, "00000000") & " HAS BEEN SELECTED FOR AUTO-AUDIT"
                        HistDetail = "ADJUSTER " & _DomainUser.ToUpper & " COMPLETED THIS ITEM." & Microsoft.VisualBasic.vbCrLf &
                                                    "AUDIT HAS DEEMED THIS ITEM FOR AUTO-AUDIT."
                        CMSDALFDBMD.CreateDocHistory(_ClaimID, UFCWGeneral.IsNullLongHandler(_ClaimDr("DOCID"), "DOCID"), "AUDIT",
                                                            UFCWGeneral.IsNullIntegerHandler(_ClaimDr("FAMILY_ID")), UFCWGeneral.IsNullShortHandler(_ClaimDr("RELATION_ID")),
                                                     UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PART_SSN")), UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PAT_SSN")),
                                                     UFCWGeneral.IsNullStringHandler(_ClaimDr("DOC_CLASS")), UFCWGeneral.IsNullStringHandler(_ClaimDr("DOC_TYPE")),
                                                     HistSum, HistDetail, _DomainUser.ToUpper, transaction)
                    Else
                        'COMPLETE
                        _ClaimDr("STATUS") = "WAITPROCESSING"

                        HistSum = "CLAIM ID " & Format(_ClaimID, "00000000") & " IS COMPLETE"
                        HistDetail = "ADJUSTER " & _DomainUser.ToUpper & " MARKED THIS DOCUMENT COMPLETE." & Microsoft.VisualBasic.vbCrLf &
                                                    "ENDING WORKFLOW PROCESSING."
                        CMSDALFDBMD.CreateDocHistory(_ClaimID, UFCWGeneral.IsNullLongHandler(_ClaimDr("DOCID"), "DOCID"), "COMPLETE",
                                                            UFCWGeneral.IsNullIntegerHandler(_ClaimDr("FAMILY_ID")), UFCWGeneral.IsNullShortHandler(_ClaimDr("RELATION_ID")),
                                                            UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PART_SSN")), UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PAT_SSN")),
                                                            UFCWGeneral.IsNullStringHandler(_ClaimDr("DOC_CLASS")), UFCWGeneral.IsNullStringHandler(_ClaimDr("DOC_TYPE")),
                                                            HistSum, HistDetail, _DomainUser.ToUpper, transaction)

                    End If

                    If _ClaimDr("STATUS").ToString = "WAITPROCESSING" AndAlso _ClaimDr("RELATION_ID").ToString = "-1" Then
                        _ClaimDr("STATUS") = "COMPLETED"
                    End If

                    ''Consolidation of update processed and update status
                    CMSDALFDBMD.UpdateClaimMasterAndComplete(_ClaimID,
                                                                UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PART_SSN"), "PART_SSN"), UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PAT_SSN"), "PAT_SSN"),
                                                                CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")),
                                                                CShort(_ClaimDr("PRIORITY")), Math.Abs(CDec(_ClaimDr("SECURITY_SW"))),
                                                                Math.Abs(CDec(_ClaimDr("ATTACH_SW"))), Math.Abs(CDec(_ClaimDr("DUPLICATE_SW"))),
                                                                Math.Abs(CDec(_ClaimDr("BUSY_SW"))), CStr(_ClaimDr("STATUS")),
                                                                UFCWGeneral.NowDate,
                                                                TryCast(_ClaimDr("DOC_CLASS"), String),
                                                               TryCast(_ClaimDr("DOC_TYPE"), String),
                                                                UFCWGeneral.IsNullDateHandler(_ClaimDr("DATE_OF_SERVICE")),
                                                               TryCast(_ClaimDr("PAT_FNAME"), String), TryCast(_ClaimDr("PAT_INT"), String),
                                                                TryCast(_ClaimDr("PAT_LNAME"), String), TryCast(_ClaimDr("PART_FNAME"), String),
                                                                TryCast(_ClaimDr("PART_INT"), String), TryCast(_ClaimDr("PART_LNAME"), String),
                                                                UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PROV_TIN")),
                                                                UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PROV_ID")),
                                                                UFCWGeneral.IsNullIntegerHandler(_ClaimDr("REFRENCE_CLAIM")),
                                                                TryCast(_ClaimDr("PENDED_TO"), String), _DomainUser.ToUpper, transaction)
                End If


            End Using

            AcceptChanges()

            Return True

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function ValidateDetailLineRow(ByVal silent As Boolean) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' validates all changes made to the detaillines
        ' </summary>
        ' <param name="Silent"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            If txtProcedure.Text.Trim.Length = 0 Then
                If Not silent Then
                    MessageBox.Show("A valid Procedure Code is required", "Invalid Procedure Code", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtProcedure.Focus()
                End If

                Return False
            End If

            If cmbStatus.Text.Trim.Length = 0 Then
                If Not silent Then
                    MessageBox.Show("A valid Status is required", "Invalid Status", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    cmbStatus.Focus()
                End If

                Return False
            End If

            Return True

        Catch ex As Exception
            Throw
        End Try
    End Function


    Private Sub DetermineCellIcon(ByRef pic As Image, ByVal cell As System.Windows.Forms.DataGridCell)
        Dim DV As DataView
        Dim DRVer As DataRowVersion
        Dim AlertDV As DataView

        Try
            If DetailLinesDataGrid Is Nothing Then Exit Sub

            Try
                DV = DetailLinesDataGrid.GetDefaultDataView
                DRVer = DV(cell.RowNumber).RowVersion
            Catch ex As Exception
                Return
            End Try

            AlertDV = New DataView(_ClaimAlertManager.AlertManagerDataTable, "LineNumber = " & DV(cell.RowNumber)("LINE_NBR").ToString, "LineNumber, Severity DESC", DataViewRowState.CurrentRows)
            If AlertDV IsNot Nothing AndAlso AlertDV.Count > 0 Then
                Select Case CType(AlertDV(0)("Severity"), Integer)
                    Case Is > 20 'critical
                        pic = DIList.Images(0)
                    Case Is > 10 'warning
                        pic = DIList.Images(1)
                    Case Is >= 0 'information
                        pic = DIList.Images(3)
                End Select
            End If

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            Else
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Function IsAlert(ByVal line As Integer) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' tests if row has an alert or not
        ' </summary>
        ' <param name="Line"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	4/11/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            If line Mod 3 = 0 Then
                Return True
            End If

            Return False

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function IsAccident(ByVal medhdrDR As DataRow) As Boolean
        Try
            If Not IsDBNull(medhdrDR("INCIDENT_DATE")) Then
                If Not IsDBNull(medhdrDR("WORKERS_COMP_SW")) AndAlso (CBool(medhdrDR("WORKERS_COMP_SW")) OrElse CBool(medhdrDR("WORKERS_COMP_SW"))) Then
                    Return True
                ElseIf Not IsDBNull(medhdrDR("AUTO_ACCIDENT_SW")) AndAlso (CBool(medhdrDR("AUTO_ACCIDENT_SW")) OrElse CBool(medhdrDR("AUTO_ACCIDENT_SW"))) Then
                    Return True
                ElseIf Not IsDBNull(medhdrDR("OTH_ACCIDENT_SW")) AndAlso (CBool(medhdrDR("OTH_ACCIDENT_SW")) OrElse CBool(medhdrDR("OTH_ACCIDENT_SW"))) Then
                    Return True
                End If
            End If

            Return False

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub PlanHighlight(ByRef highlight As Boolean, ByVal cell As System.Windows.Forms.DataGridCell)

        '  Dim DV As DataView
        Dim OtherDV As DataView
        Dim Plan As String = ""
        Dim DR As DataRow

        Try

            If DetailLinesDataGrid.GetGridRowCount <= 1 Then Return
            If _MedDtlBS.Current IsNot Nothing Then
                DR = DirectCast(_MedDtlBS.Current, DataRowView).Row
            End If
            If DR IsNot Nothing AndAlso Not IsDBNull(DR("MED_PLAN")) Then
                Plan = CStr(DR("MED_PLAN")).ToUpper
            End If

            OtherDV = New DataView(_ClaimDS.MEDDTL, "ISNULL(MED_PLAN,'') <> '" & Plan & "'", "MED_PLAN", DataViewRowState.CurrentRows)
            If OtherDV.Count > 0 Then
                highlight = True
            Else
                highlight = False
            End If

            If _HighlightPlan <> highlight Then
                DetailLinesDataGrid.Invalidate()
            End If

            _HighlightPlan = highlight

        Catch ex As Exception
            Throw
        Finally

            If OtherDV IsNot Nothing Then OtherDV.Dispose()

        End Try
    End Sub

    Private Sub OriginalDetailLinesDataGrid_FormattingReason(ByRef value As Object, ByVal rowNum As Integer)
        Dim DV As DataView
        Dim ReasonDV As DataView
        Dim Output As String = ""

        Try

            DV = DuplicatesOriginalDetailLinesDataGrid.GetDefaultDataView
            If DV IsNot Nothing Then
                ReasonDV = New DataView(_ClaimDS.REASON, "LINE_NBR = " & DV(rowNum)("LINE_NBR").ToString, "LINE_NBR, PRIORITY", DataViewRowState.CurrentRows)
                If ReasonDV.Count > 0 Then
                    For Cnt As Integer = 0 To ReasonDV.Count - 1
                        If ReasonDV(Cnt).Row.RowState <> DataRowState.Deleted Then
                            Output &= If(Output.Trim.Length = 0, "", ", ") & ReasonDV(Cnt)("REASON").ToString
                        End If
                    Next

                    value = Output
                Else
                    value = "None"
                End If
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DetailLinesDataGrid_FormattingReason(ByRef value As Object, ByVal rowNum As Integer)
        Dim DV As DataView
        Dim ReasonDV As DataView
        Dim Output As String = ""

        Try

            DV = DetailLinesDataGrid.GetDefaultDataView

            If DV IsNot Nothing Then
                ReasonDV = New DataView(_ClaimDS.REASON, "LINE_NBR = " & DV(rowNum)("LINE_NBR").ToString, "LINE_NBR, PRIORITY", DataViewRowState.CurrentRows)
                If ReasonDV.Count > 0 Then
                    For Cnt As Integer = 0 To ReasonDV.Count - 1
                        If ReasonDV(Cnt).Row.RowState <> DataRowState.Deleted Then
                            Output &= If(Output.Trim.Length = 0, "", ", ") & ReasonDV(Cnt)("REASON").ToString
                        End If
                    Next

                    value = Output
                Else
                    value = "None"
                End If
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub UnFormattingReason(ByRef value As Object, ByVal rowNum As Integer)

        Dim DV As DataView

        Try
            DV = DetailLinesDataGrid.GetDefaultDataView

            If DV IsNot Nothing Then
                value = DV(rowNum)("REASON_SW")
            Else
                value = 0
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub UnFormattingAccumulators(ByRef value As Object, ByVal rowNum As Integer)

        Dim DV As DataView

        Try
            DV = DetailLinesDataGrid.GetDefaultDataView

            If DV IsNot Nothing Then
                value = DV(rowNum)("Accumulators")
            Else
                value = 0
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub DetailLinesDataGrid_FormattingAccumulators(ByRef value As Object, ByVal rowNum As Integer)
        Dim Output As String = ""
        Dim DV As DataView
        Try
            DV = DetailLinesDataGrid.GetDefaultDataView

            If DV IsNot Nothing AndAlso _DetailAccumulatorsDT IsNot Nothing Then
                Dim LineNbr As Short = CShort(DV(rowNum)("LINE_NBR"))
                Dim QueryAccums =
                            From LineAccums In _DetailAccumulatorsDT.AsEnumerable()
                            Where LineAccums.RowState <> DataRowState.Deleted _
                            AndAlso LineAccums.Field(Of Short)("LINE_NBR") = LineNbr _
                            AndAlso (LineAccums.Field(Of Decimal?)("ENTRY_VALUE") IsNot Nothing AndAlso LineAccums.Field(Of Decimal?)("ENTRY_VALUE") <> 0D)
                            Select LineAccums

                If QueryAccums.Any Then
                    For Each DVACCUM As DataRowView In QueryAccums.AsDataView
                        Output &= If(Output.Trim.Length = 0, "", ", ") & DVACCUM("ACCUM_NAME").ToString
                    Next
                End If

                value = Output
            Else
                value = "None"
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub RefreshClaim()
        Try

            LoadClaim()

            'If CStr(_ClaimDr("DOC_TYPE")).ToUpper.StartsWith("UTL_") Then
            '    HideControlsForUTL()
            'End If

        Catch ex As Exception
            Throw

        End Try
    End Sub

    Private Function HoldClaim() As Boolean

        Dim Transaction As DbTransaction

        Try

            '  If Not Me.ValidateChildren(ValidationConstraints.Enabled) Then Return False 'data must be valid to at least save.

            FinalizeEdits()

            If HasChanges() Then
                Transaction = CMSDALCommon.BeginTransaction

                If SaveChanges(Transaction) Then
                    Using WC As New GlobalCursor

                        Dim HistSum As String
                        Dim HistDetail As String

                        If _UTLArchive AndAlso Not CStr(_ClaimDr("DOC_TYPE")).ToUpper.StartsWith("UTL_") Then
                            CMSDALFDBMD.ArchiveClaimMaster(_ClaimID, "COMPLETED", _DomainUser.ToUpper, Transaction)

                            HistSum = "CLAIM ID " & Format(_ClaimID, "00000000") & " HAS BEEN ARCHIVED OUT OF THE QUEUE"
                            HistDetail = "ADJUSTER " & _DomainUser.ToUpper & " ARCHIVED THIS ITEM." & Microsoft.VisualBasic.vbCrLf &
                                                        "NO FURTHER PROCESSING IS NECESSARY."
                            CMSDALFDBMD.CreateDocHistory(_ClaimID, UFCWGeneral.IsNullLongHandler(_ClaimDr("DOCID"), "DOCID"), "COMPLETE", CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")), CInt(_ClaimDr("PART_SSN")), CInt(_ClaimDr("PAT_SSN")), CStr(_ClaimDr("DOC_CLASS")), CStr(_ClaimDr("DOC_TYPE")), HistSum, HistDetail, _DomainUser.ToUpper, Transaction)
                        Else
                            HistSum = "CLAIM ID " & Format(_ClaimID, "00000000") & " PLACED ON HOLD"
                            HistDetail = "ADJUSTER " & _DomainUser.ToUpper & " PLACED THIS ITEM ON HOLD."
                            CMSDALFDBMD.CreateDocHistory(_ClaimID, UFCWGeneral.IsNullLongHandler(_ClaimDr("DOCID"), "DOCID"), "ONHOLD", CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")), CInt(_ClaimDr("PART_SSN")), CInt(_ClaimDr("PAT_SSN")), CStr(_ClaimDr("DOC_CLASS")), CStr(_ClaimDr("DOC_TYPE")), HistSum, HistDetail, _DomainUser.ToUpper, Transaction)
                        End If

                        CMSDALCommon.CommitTransaction(Transaction)

                    End Using
                Else
                    CMSDALCommon.RollbackTransaction(Transaction)

                    Return False
                End If

            End If

            Return True

        Catch ex As Exception
            CMSDALCommon.RollbackTransaction(Transaction)
            Throw
        End Try

    End Function

    Private Function CompleteClaim() As Boolean


        Dim Transaction As DbTransaction

        Try

            _ClaimAlertManager.SuspendLayout = True

            FinalizeEdits()

            If Not HeaderInfoOk() Then Return False

            If _Mode.ToUpper <> "AUDIT" Then

                If Not AllowComplete() Then Return False

                ZeroPaidAmount()

            End If

            Transaction = CMSDALCommon.BeginTransaction

            If HasChanges() Then
                If Not SaveChanges(Transaction) Then
                    CMSDALCommon.RollbackTransaction(Transaction)
                    Return False
                End If
            End If

            If Not CompleteProcessing(Transaction) Then
                CMSDALCommon.RollbackTransaction(Transaction)
                Return False
            End If

            CMSDALCommon.CommitTransaction(Transaction)

            CMSDALFDBMD.WriteToClaimsHistoryXML(CInt(_ClaimDr("CLAIM_ID")), UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PART_SSN")), UFCWGeneral.IsNullLongHandler(_ClaimDr("DOCID")), UFCWGeneral.IsNullIntegerHandler(_ClaimDr("FAMILY_ID")), UFCWGeneral.NowDate, "Completed")

            Return True

        Catch ex As Exception

            If Transaction IsNot Nothing Then
                CMSDALCommon.RollbackTransaction(Transaction)
            End If
            Throw
        Finally
            _ClaimAlertManager.SuspendLayout = False
        End Try

    End Function

    Private Sub LettersControl_RefreshLettersHistory()
        Try
            LettersHistoryControl.RefreshLettersHistory()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LettersHistoryControl_CancelLetterRequest()
        Try
            If LettersControl Is Nothing Then Return
            LettersControl.Status.Text = "Letter print request cancelled id/maxid( " & CStr(LettersHistoryControl.LetterID) & " / " & LettersHistoryControl.MaxID & ")"
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LettersHistoryControl_ReprintLetterRequest()
        Try
            LettersControl.Status.Text = "Letter scheduled for re-print id/maxid( " & CStr(LettersHistoryControl.LetterID) & " / " & LettersHistoryControl.MaxID & ")"

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function ValidateHeader(Optional ByVal silent As Boolean = False) As Boolean

        Dim DV As DataView

        If _MedHdrDr IsNot Nothing Then

            ''Valid FamilyID/RelationID
            If CStr(_MedHdrDr("FAMILY_ID")) = "-1" Then
                MessageBox.Show("Family is not Valid.", "Invalid Family.", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            'valid COB
            If UFCWGeneral.IsNullStringHandler(_MedHdrDr("COB")) = "" Then
                MessageBox.Show("A Valid COB is Required" & vbCrLf & "Please Enter or Select a Valid COB.", "Invalid COB", MessageBoxButtons.OK, MessageBoxIcon.Information)
                cmbCOB.Focus()
                Return False
            End If

            'Valid PPO
            If UFCWGeneral.IsNullStringHandler(_MedHdrDr("PPO")) = "" Then
                MessageBox.Show("A Valid PPO is Required" & vbCrLf & "Please Enter or Select a Valid PPO.", "Invalid PPO", MessageBoxButtons.OK, MessageBoxIcon.Information)
                cmbPricingNetwork.Focus()
                Return False
            End If

            'valid Payee
            If UFCWGeneral.IsNullStringHandler(_MedHdrDr("PAYEE")) = "" Then
                MessageBox.Show("A Valid Payee is Required" & vbCrLf & "Please Enter or Select a Valid Payee.", "Invalid Payee", MessageBoxButtons.OK, MessageBoxIcon.Information)
                cmbPayee.Focus()
                Return False
            End If

            If Not _ClaimDr("DOC_TYPE").ToString.ToUpper.Contains("MENTAL HEALTH") Then
                '' BC & BCFFE Pricing Only applicable to PAR(P) and not applicable to PAR(N,O)
                If NonPARCheckBox.Checked AndAlso UFCWGeneral.IsNullStringHandler(_MedHdrDr("PPO")).Contains("BC") Then
                    MessageBox.Show("A valid PPO is Required" & vbCrLf & "BC/BCFFE/BCJAA cannot be used in conjunction with Non PAR Claims.", "Invalid PPO Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    cmbPricingNetwork.Focus()
                    Return False
                End If

                'Note the Only BC Pricing network valid for OOA is BCJAA
                If OOACheckBox.Checked AndAlso (UFCWGeneral.IsNullStringHandler(_MedHdrDr("PPO")).Replace(" ", "") = "BC" OrElse UFCWGeneral.IsNullStringHandler(_MedHdrDr("PPO")).Replace(" ", "") = "BCFFE") Then
                    MessageBox.Show("Only BCJAA can be used to designate an Out of Area claim as PAR.", "Invalid PPO Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    cmbPricingNetwork.Focus()
                    Return False
                End If

                ''TOT_PAID_AMT > 0 and with Line(s) identified as PAY, that a Payee has been assigned
                If Not IsDBNull(_MedHdrDr("TOT_PAID_AMT")) AndAlso CDec(_MedHdrDr("TOT_PAID_AMT")) > 0 AndAlso IsDBNull(_MedHdrDr("PAYEE")) Then
                    DV = New DataView(_ClaimDS.MEDDTL, "STATUS = 'PAY'", "LINE_NBR", DataViewRowState.CurrentRows)
                    If DV.Count > 0 Then
                        MessageBox.Show("A Valid Payee is Required" & vbCrLf & "Please Enter or Select a Valid Payee.", "Invalid Payee", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return False
                    End If
                End If
            End If

            Return True

        End If

    End Function

    Private Function ValidateDetail(Optional ByVal silent As Boolean = False) As Boolean

        Dim CompareDate As String = "'" & UFCWGeneral.NowDate.ToString("MM/dd/yyyy") & "'"
        Dim Lines As String = ""
        Dim ErrorCount As Integer = 0

        Try

            'Must have at least 1 line item to complete a claim
            If _ClaimDS.MEDDTL.Rows.Count < 1 Then
                MessageBox.Show("Claim has no line items." & vbCrLf & "Either Add(+) Line item(s) or Archive Claim.", "Invalid Claim", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            ''Valid FamilyID/RelationID
            If CStr(_MedHdrDr("RELATION_ID")).Trim = "-1" Then
                If _MedHdrDr("PRICED_BY").ToString.Contains("JAA") Then
                    'ALL Lines must be marked ineligible
                    Dim MEDDTLDV As DataView = New DataView(_ClaimDS.MEDDTL, "STATUS <> 'MERGED'", "LINE_NBR, PAID_AMT", DataViewRowState.CurrentRows)
                    If MEDDTLDV.Count > 0 Then
                        For Each DVR As DataRowView In MEDDTLDV
                            Dim ReasonsDV As DataView = New DataView(_ClaimDS.REASON, "REASON = '042' AND LINE_NBR = " & DVR("LINE_NBR").ToString, "", DataViewRowState.CurrentRows)
                            If ReasonsDV.Count = 0 Then
                                MessageBox.Show("All Lines must be identified as In-Eligible (042) when Patient is not identified", "Line " & DVR("LINE_NBR").ToString & " NOT marked In-eligible", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Return False
                            End If
                        Next
                    End If
                Else
                    MessageBox.Show("Relation is not Valid.", "Invalid Family / Relation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            End If

            ' Start Date Of Service is required
            Dim QueryFDOS =
                From MEDDTL In _ClaimDS.Tables("MEDDTL").AsEnumerable()
                Where MEDDTL.RowState <> DataRowState.Deleted _
                AndAlso MEDDTL.Field(Of String)("STATUS") <> "MERGED" _
                AndAlso MEDDTL.Field(Of Date?)("OCC_FROM_DATE") Is Nothing
                Order By MEDDTL.Field(Of Short)("LINE_NBR")
                Select MEDDTL


            For Each DVR As DataRowView In QueryFDOS.AsDataView
                Lines &= If(Lines.Length > 0, ", ", "") & If(Lines.Trim.Length > 0 AndAlso Lines.Length > 0, "& ", "") & DVR("LINE_NBR").ToString
                ErrorCount += 1
            Next

            If Lines.Length > 0 Then
                MessageBox.Show("Line" & If(ErrorCount > 1, "s ", " ") & Lines & If(ErrorCount > 1, " are ", " is ") & "missing First Date of Service ", "Invalid DOS", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            ' Last Date before Start Date
            Dim QueryDOSRange =
                From MEDDTL In _ClaimDS.Tables("MEDDTL").AsEnumerable()
                Where MEDDTL.RowState <> DataRowState.Deleted _
                AndAlso MEDDTL.Field(Of String)("STATUS") <> "MERGED" _
                AndAlso (MEDDTL.Field(Of Date?)("OCC_TO_DATE") Is Nothing OrElse MEDDTL.Field(Of Date?)("OCC_TO_DATE") < MEDDTL.Field(Of Date?)("OCC_FROM_DATE"))
                Order By MEDDTL.Field(Of Short)("LINE_NBR")
                Select MEDDTL


            For Each DVR As DataRowView In QueryDOSRange.AsDataView
                Lines &= If(Lines.Length > 0, ", ", "") & If(Lines.Trim.Length > 0 AndAlso Lines.Length > 0, "& ", "") & DVR("LINE_NBR").ToString
                ErrorCount += 1
            Next

            If Lines.Length > 0 Then
                MessageBox.Show("Line" & If(ErrorCount > 1, "s ", " ") & Lines & If(ErrorCount > 1, " are ", " is ") & "Last Date of Service is before First", "Invalid DOS Range", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            ' Start Date and End Date must be in 3 years
            Dim QueryDOSYear =
                From MEDDTL In _ClaimDS.Tables("MEDDTL").AsEnumerable()
                Where MEDDTL.RowState <> DataRowState.Deleted _
                AndAlso MEDDTL.Field(Of String)("STATUS") <> "MERGED" _
                AndAlso (MEDDTL.Field(Of Date)("OCC_TO_DATE").Subtract(MEDDTL.Field(Of Date)("OCC_FROM_DATE")).TotalDays > 1095)
                Order By MEDDTL.Field(Of Short)("LINE_NBR")
                Select MEDDTL


            For Each DVR As DataRowView In QueryDOSYear.AsDataView
                Lines &= If(Lines.Length > 0, ", ", "") & If(Lines.Trim.Length > 0 AndAlso Lines.Length > 0, "& ", "") & DVR("LINE_NBR").ToString
                ErrorCount += 1
            Next

            If Lines.Length > 0 Then
                If Not UFCWGeneralAD.CMSCanReopenFull AndAlso Not UFCWGeneralAD.CMSCanReprocess Then
                    MessageBox.Show("Line" & If(ErrorCount > 1, "s ", " ") & Lines & If(ErrorCount > 1, " are ", " is ") & "Start Date and End Date must be within last 3 years." & vbCrLf & "Please contact your manager to reopen the claim.", "Invalid DOS Range", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                Else
                    If MessageBox.Show("Start Date must be within last 3 years" &
                                                vbCrLf & "Are you sure you want to Continue?", "Invalid DOS Range", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    Else
                        Return False
                    End If
                End If
            End If
            ' Start Date and End Date must be in the same year
            Dim QueryDOS3Year =
                From MEDDTL In _ClaimDS.Tables("MEDDTL").AsEnumerable()
                Where MEDDTL.RowState <> DataRowState.Deleted _
                AndAlso MEDDTL.Field(Of String)("STATUS") <> "MERGED" _
                AndAlso DateDiff(DateInterval.Day, Date.Now, MEDDTL.Field(Of Date)("OCC_FROM_DATE")) > 1095
                Order By MEDDTL.Field(Of Short)("LINE_NBR")
                Select MEDDTL


            For Each DVR As DataRowView In QueryDOS3Year.AsDataView
                Lines &= If(Lines.Length > 0, ", ", "") & If(Lines.Trim.Length > 0 AndAlso Lines.Length > 0, "& ", "") & DVR("LINE_NBR").ToString
                ErrorCount += 1
            Next

            If Lines.Length > 0 Then
                If Not UFCWGeneralAD.CMSCanReopenFull AndAlso Not UFCWGeneralAD.CMSCanReprocess Then
                    MessageBox.Show("Line" & If(ErrorCount > 1, "s ", " ") & Lines & If(ErrorCount > 1, " are ", " is ") & "Start Date must be within last 3 years." & vbCrLf & "Please contact your manager to reopen the claim.", "Invalid DOS Range", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                Else
                    If MessageBox.Show("Start Date must be within last 3 years" &
                                                vbCrLf & "Are you sure you want to Continue?", "Invalid DOS Range", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    Else
                        Return False
                    End If
                End If
            End If


            ' Date Of Service in future
            Dim QueryDOSInFuture =
                From MEDDTL In _ClaimDS.Tables("MEDDTL").AsEnumerable()
                Where MEDDTL.RowState <> DataRowState.Deleted _
                AndAlso MEDDTL.Field(Of String)("STATUS") = "PAY" _
                AndAlso (MEDDTL.Field(Of Date?)("OCC_TO_DATE") > UFCWGeneral.NowDate.Date OrElse MEDDTL.Field(Of Date?)("OCC_FROM_DATE") > UFCWGeneral.NowDate.Date)
                Order By MEDDTL.Field(Of Short)("LINE_NBR")
                Select MEDDTL


            For Each DVR As DataRowView In QueryDOSInFuture.AsDataView
                Lines &= If(Lines.Length > 0, ", ", "") & If(Lines.Trim.Length > 0 AndAlso Lines.Length > 0, "& ", "") & DVR("LINE_NBR").ToString
                ErrorCount += 1
            Next

            If Lines.Length > 0 Then
                MessageBox.Show("Line" & If(ErrorCount > 1, "s ", " ") & Lines & If(ErrorCount > 1, " are ", " is ") & "'PAY' Lines cannot include future Date(s).", "Invalid Future Date(s)", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            ' Plan Required
            Dim QueryPlan =
                From MEDDTL In _ClaimDS.Tables("MEDDTL").AsEnumerable()
                Where MEDDTL.RowState <> DataRowState.Deleted _
                AndAlso MEDDTL.Field(Of String)("STATUS") = "PAY" _
                AndAlso (MEDDTL.Field(Of String)("MED_PLAN") Is Nothing OrElse MEDDTL.Field(Of String)("MED_PLAN").Trim.Length = 0)
                Order By MEDDTL.Field(Of Short)("LINE_NBR")
                Select MEDDTL

            For Each DVR As DataRowView In QueryPlan.AsDataView
                Lines &= If(Lines.Length > 0, ", ", "") & If(Lines.Trim.Length > 0 AndAlso Lines.Length > 0, "& ", "") & DVR("LINE_NBR").ToString
                ErrorCount += 1
            Next

            If Lines.Length > 0 Then
                MessageBox.Show("Line" & If(ErrorCount > 1, "s ", " ") & Lines & If(ErrorCount > 1, " are ", " is ") & "missing Plan for 'PAY' line.", "Plan Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            ' Procedure is required
            Dim QueryProcedure =
                From MEDDTL In _ClaimDS.Tables("MEDDTL").AsEnumerable()
                Where MEDDTL.RowState <> DataRowState.Deleted _
                AndAlso MEDDTL.Field(Of String)("STATUS") = "PAY" _
                AndAlso (MEDDTL.Field(Of String)("PROC_CODE") Is Nothing OrElse MEDDTL.Field(Of String)("PROC_CODE").Trim.Length = 0)
                Order By MEDDTL.Field(Of Short)("LINE_NBR")
                Select MEDDTL

            For Each DVR As DataRowView In QueryProcedure.AsDataView
                Lines &= If(Lines.Length > 0, ", ", "") & If(Lines.Trim.Length > 0 AndAlso Lines.Length > 0, "& ", "") & DVR("LINE_NBR").ToString
                ErrorCount += 1
            Next

            If Lines.Length > 0 Then
                MessageBox.Show("Line" & If(ErrorCount > 1, "s ", " ") & Lines & If(ErrorCount > 1, " are ", " is ") & " missing Procedure for 'PAY' line.", "Procedure Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            ' Status is required
            Dim QueryStatus =
                From MEDDTL In _ClaimDS.Tables("MEDDTL").AsEnumerable()
                Where MEDDTL.RowState <> DataRowState.Deleted _
                AndAlso (MEDDTL.Field(Of String)("STATUS") Is Nothing OrElse MEDDTL.Field(Of String)("STATUS").Trim.Length = 0)
                Order By MEDDTL.Field(Of Short)("LINE_NBR")
                Select MEDDTL

            For Each DVR As DataRowView In QueryStatus.AsDataView
                Lines &= If(Lines.Length > 0, ", ", "") & If(Lines.Trim.Length > 0 AndAlso Lines.Length > 0, "& ", "") & DVR("LINE_NBR").ToString
                ErrorCount += 1
            Next

            If Lines.Length > 0 Then
                MessageBox.Show("Line" & If(ErrorCount > 1, "s ", " ") & Lines & If(ErrorCount > 1, " are ", " is ") & "missing Pay/Deny.", "Action/Status Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            'Charge is missing/zero
            Dim QueryNoCharge =
                From MEDDTL In _ClaimDS.Tables("MEDDTL").AsEnumerable()
                Where MEDDTL.RowState <> DataRowState.Deleted _
                AndAlso MEDDTL.Field(Of String)("STATUS") = "PAY" _    ' Added LM 3/27/2023 only paid  lines 
                AndAlso MEDDTL.Field(Of Decimal?)("CHRG_AMT") Is Nothing
                Order By MEDDTL.Field(Of Short)("LINE_NBR")
                Select MEDDTL

            For Each DVR As DataRowView In QueryNoCharge.AsDataView
                Lines &= If(Lines.Length > 0, ", ", "") & If(Lines.Trim.Length > 0 AndAlso Lines.Length > 0, "& ", "") & DVR("LINE_NBR").ToString
                ErrorCount += 1
            Next

            If Lines.Length > 0 Then
                MessageBox.Show("Line" & If(ErrorCount > 1, "s ", " ") & Lines & If(ErrorCount > 1, " are ", " is ") & "missing Charge Amount.", "Charge Amount Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            'Charge is missing/zero
            Dim QueryZeroCharge =
                From MEDDTL In _ClaimDS.Tables("MEDDTL").AsEnumerable()
                Where MEDDTL.RowState <> DataRowState.Deleted _
                AndAlso MEDDTL.Field(Of String)("STATUS") = "PAY" _
                AndAlso MEDDTL.Field(Of Decimal?)("CHRG_AMT") = 0D
                Order By MEDDTL.Field(Of Short)("LINE_NBR")
                Select MEDDTL

            For Each DVR As DataRowView In QueryZeroCharge.AsDataView

                Dim LineNbr As Short = CShort(DVR("LINE_NBR"))
                Dim NONPar As Boolean = CBool(DVR("NON_PAR_SW"))

                Dim QueryReason =
                    From REASON In _ClaimDS.Tables("REASON").AsEnumerable()
                    Where REASON.RowState <> DataRowState.Deleted _
                    AndAlso REASON.Field(Of Short)("LINE_NBR") = LineNbr _
                    AndAlso ((NONPar = False AndAlso REASON.Field(Of String)("REASON") <> "FFW" AndAlso REASON.Field(Of String)("REASON") <> "LTR") _
                             OrElse
                            (NONPar = True AndAlso REASON.Field(Of String)("REASON") <> "031" AndAlso REASON.Field(Of String)("REASON") <> "LTR"))
                    Select REASON

                For Each DVRReason As DataRowView In QueryReason.AsDataView
                    Lines &= If(Lines.Length > 0, ", ", "") & If(Lines.Trim.Length > 0 AndAlso Lines.Length > 0, "& ", "") & DVRReason("LINE_NBR").ToString
                    ErrorCount += 1
                Next

            Next

            If Lines.Length > 0 Then
                MessageBox.Show("Line" & If(ErrorCount > 1, "s ", " ") & Lines & If(ErrorCount > 1, " are ", " is ") & "invalid. Charge Amount requires valid Reason Code (FFW/031).", "Charge/Reason mismatch", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            'Paid is missing
            Dim QueryNoPaid =
                From MEDDTL In _ClaimDS.Tables("MEDDTL").AsEnumerable()
                Where MEDDTL.RowState <> DataRowState.Deleted _
                AndAlso MEDDTL.Field(Of String)("STATUS") = "PAY" _
                AndAlso MEDDTL.Field(Of Decimal?)("PAID_AMT") Is Nothing
                Order By MEDDTL.Field(Of Short)("LINE_NBR")
                Select MEDDTL


            For Each DVR As DataRowView In QueryNoPaid.AsDataView
                Lines &= If(Lines.Length > 0, ", ", "") & If(Lines.Trim.Length > 0 AndAlso Lines.Length > 0, "& ", "") & DVR("LINE_NBR").ToString
                ErrorCount += 1
            Next

            If Lines.Trim.Length > 0 Then
                MessageBox.Show("Line" & If(ErrorCount > 1, "s ", " ") & Lines & If(ErrorCount > 1, " are ", " is ") & "missing Paid Amount.", "Paid Amount Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If



            'Priced is missing
            Dim QueryNoPriced =
                From MEDDTL In _ClaimDS.Tables("MEDDTL").AsEnumerable()
                Where MEDDTL.RowState <> DataRowState.Deleted _
                AndAlso MEDDTL.Field(Of String)("STATUS") = "PAY" _
                AndAlso MEDDTL.Field(Of Decimal?)("PRICED_AMT") Is Nothing
                Order By MEDDTL.Field(Of Short)("LINE_NBR")
                Select MEDDTL

            For Each DVR As DataRowView In QueryNoPriced.AsDataView
                Lines &= If(Lines.Length > 0, ", ", "") & If(Lines.Trim.Length > 0 AndAlso Lines.Length > 0, "& ", "") & DVR("LINE_NBR").ToString
                ErrorCount += 1
            Next

            If Lines.Length > 0 Then
                MessageBox.Show("Line" & If(ErrorCount > 1, "s ", " ") & Lines & If(ErrorCount > 1, " are ", " is ") & "missing Priced Amount.", "Priced Amount Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            'Paid = 0 with No Reason
            Dim QueryNoReason =
                From MEDDTL In _ClaimDS.Tables("MEDDTL").AsEnumerable()
                Where MEDDTL.RowState <> DataRowState.Deleted _
                AndAlso MEDDTL.Field(Of String)("STATUS") = "PAY" _
                AndAlso (MEDDTL.Field(Of Decimal?)("PAID_AMT") Is Nothing OrElse MEDDTL.Field(Of Decimal?)("PAID_AMT") = 0D)
                Order By MEDDTL.Field(Of Short)("LINE_NBR")
                Select MEDDTL

            For Each DVR As DataRowView In QueryNoReason.AsDataView
                'Paid = 0 with No Reason

                Dim LineNbr As Short = CShort(DVR("LINE_NBR"))

                Dim QueryReason =
                    From REASON In _ClaimDS.Tables("REASON").AsEnumerable()
                    Where REASON.RowState <> DataRowState.Deleted _
                    AndAlso REASON.Field(Of Short)("LINE_NBR") = LineNbr
                    Select REASON

                If Not QueryReason.Any Then
                    Lines &= If(Lines.Length > 0, ", ", "") & If(Lines.Trim.Length > 0 AndAlso Lines.Length > 0, "& ", "") & DVR("LINE_NBR").ToString
                    ErrorCount += 1
                End If
            Next

            If Lines.Length > 0 Then
                MessageBox.Show("Line" & If(ErrorCount > 1, "s ", " ") & Lines & If(ErrorCount > 1, " are ", " is ") & "paying $0 and require a Reason Code.", "Reason Code Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            Dim QueryMEDDTL =
                From MEDDTL In _ClaimDS.Tables("MEDDTL").AsEnumerable()
                Where MEDDTL.RowState <> DataRowState.Deleted _
                AndAlso MEDDTL.Field(Of String)("STATUS") = "PAY" _
                AndAlso MEDDTL.Field(Of Decimal?)("PAID_AMT") Is Nothing
                Select MEDDTL

            If QueryMEDDTL.Any Then
                If Not silent Then

                    For Each DVR As DataRowView In QueryMEDDTL.AsDataView

                        Dim LineNbr As Short = CShort(DVR("LINE_NBR"))

                        Dim QueryAccums =
                            From LineAccums In _DetailAccumulatorsDT.AsEnumerable()
                            Where LineAccums.RowState <> DataRowState.Deleted _
                            AndAlso LineAccums.Field(Of Short)("LINE_NBR") = LineNbr _
                            AndAlso (LineAccums.Field(Of Decimal?)("ENTRY_VALUE") IsNot Nothing AndAlso LineAccums.Field(Of Decimal?)("ENTRY_VALUE") <> 0D)
                            Select LineAccums

                        If Not QueryAccums.Any Then
                            Lines &= If(Lines.Length > 0, ", ", "") & If(Lines.Trim.Length > 0 AndAlso Lines.Length > 0, "& ", "") & DVR("LINE_NBR").ToString
                            ErrorCount += 1
                        End If

                    Next
                End If
            End If

            If Lines.Length > 0 Then
                MessageBox.Show("Line" & If(ErrorCount > 1, "s ", " ") & Lines & If(ErrorCount > 1, " are ", " is ") & "pay lines should either show $0.00 or have monies allocated to the members accumulators", "Invalid Pay Amt", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            Dim QueryDIAGNOSIS1 =
                From MEDDIAG In _ClaimDS.Tables("MEDDIAG").AsEnumerable()
                Where MEDDIAG.RowState <> DataRowState.Deleted
                Group By LINE_NBR = MEDDIAG.Field(Of Short)("LINE_NBR"),
                            PRIORITY = MEDDIAG.Field(Of Integer)("PRIORITY")
                Into Group
                Where Group.Count > 1
                Select Group

            If QueryDIAGNOSIS1.Any Then
                Dim DRCol As DataRowCollection = _ClaimDS.MEDDIAG.Rows
                DRCol.Clear()
                MassClearField("DIAGNOSES")
                MessageBox.Show("Diagnosis Values failed Validation. Please ReEnter ALL Line Item Diagnosis.", "Invalid Diagnosis", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            Dim QueryDIAGNOSIS2 =
                From MEDDIAG In _ClaimDS.Tables("MEDDIAG").AsEnumerable()
                Where MEDDIAG.RowState <> DataRowState.Deleted
                Group By LINE_NBR = MEDDIAG.Field(Of Short)("LINE_NBR"),
                            DIAGNOSIS = MEDDIAG.Field(Of String)("DIAGNOSIS")
                Into Group
                Where Group.Count > 1
                Select LINE_NBR

            For Each DR In QueryDIAGNOSIS2.AsEnumerable

                Lines &= If(Lines.Length > 0, ", ", "") & If(Lines.Trim.Length > 0 AndAlso Lines.Length > 0, "& ", "") & DR.ToString
                ErrorCount += 1

            Next

            If Lines.Length > 0 Then
                MessageBox.Show("Line" & If(ErrorCount > 1, "s ", " ") & Lines & If(ErrorCount > 1, " have ", " has ") & "duplicate Diagnosis and " & If(ErrorCount > 1, " are ", " is ") & "invalid.", "Invalid Diagnosis", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            Return True

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function HeaderInfoOk(Optional ByVal silent As Boolean = False) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Validates all header fields
        ' </summary>
        ' <param name="Silent"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            If txtFamilyID.Text = "" Then
                If Not silent Then
                    MessageBox.Show("A valid Family ID is required", "Invalid Family ID", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtFamilyID.Focus()
                End If

                Return False
            End If

            If txtPartSSN.Text = "" Then
                If Not silent Then
                    MessageBox.Show("A valid Participant SSN is required", "Invalid Participant SSN", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtPartSSN.Focus()
                End If

                Return False
            End If

            If txtPatRelationID.Text = "" Then
                If Not silent Then
                    MessageBox.Show("A valid Relation ID is required", "Invalid Relation ID", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtPatRelationID.Focus()
                End If

                Return False
            End If

            If txtPatSSN.Text = "" Then
                If Not silent Then
                    MessageBox.Show("A valid Patient SSN is required", "Invalid Patient SSN", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtPatSSN.Focus()
                End If

                Return False
            End If

            If txtProviderRenderingNPI.Text.Trim.Length > 0 AndAlso txtProviderRenderingNPI.Text.Trim.Length < 10 Then
                If Not silent Then
                    MessageBox.Show("A valid Provider NPI is required", "Invalid Provider NPI (Must be 10 digits)", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtProviderRenderingNPI.Focus()
                End If

                Return False
            End If

            If txtBCCZIP.Text.Trim.Length > 0 AndAlso txtBCCZIP.Text.Trim.Length < 5 Then
                If Not silent Then
                    MessageBox.Show("A valid Provider ZIP is required", "Invalid Provider ZIP (Must be 5 digits)", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtBCCZIP.Focus()
                End If

                Return False
            End If

            Return True

        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Sub EnableMenus(enableControls As Boolean)

        Me.RefreshToolBarButton.Enabled = enableControls
        If Me.RefreshMenuItem IsNot Nothing AndAlso Me.RefreshMenuItem.MenuItems.Count > 0 Then Me.RefreshMenuItem.Enabled = enableControls

        Me.ReCalcToolBarButton.Enabled = enableControls
        If Me.RecalcMenuItem IsNot Nothing AndAlso Me.RecalcMenuItem.MenuItems.Count > 0 Then Me.RecalcMenuItem.Enabled = enableControls

        Me.CompleteToolBarButton.Enabled = enableControls
        If Me.CompleteMenuItem IsNot Nothing AndAlso Me.CompleteMenuItem.MenuItems.Count > 0 Then Me.CompleteMenuItem.Enabled = enableControls

        Me.DropdownHoldRefreshToolBarButton.Enabled = enableControls
        Me.HoldToolBarButton.Enabled = enableControls
        If Me.HoldMenuItem IsNot Nothing AndAlso Me.HoldMenuItem.MenuItems.Count > 0 Then Me.HoldMenuItem.Enabled = enableControls

        If enableControls Then
            If AllowReprice() Then
                Me.DropdownRepriceReturnToolBarButton.Enabled = enableControls

                Me.DropdownRepriceReturnToolBarButton.Enabled = enableControls
                Me.RePriceToolBarButton.Enabled = enableControls
                Me.RePriceReturnToolBarButton.Enabled = enableControls
                If Me.RePriceMenuItem IsNot Nothing AndAlso Me.RePriceMenuItem.MenuItems.Count > 0 Then Me.RePriceMenuItem.Enabled = enableControls
                If Me.RePriceReturnMenuItem IsNot Nothing AndAlso Me.RePriceReturnMenuItem.MenuItems.Count > 0 Then Me.RePriceReturnMenuItem.Enabled = enableControls
            End If
        Else
            Me.DropdownRepriceReturnToolBarButton.Enabled = enableControls
            Me.RePriceToolBarButton.Enabled = enableControls
            Me.RePriceReturnToolBarButton.Enabled = enableControls
            If Me.RePriceMenuItem IsNot Nothing AndAlso Me.RePriceMenuItem.MenuItems.Count > 0 Then Me.RePriceMenuItem.Enabled = enableControls
            If Me.RePriceReturnMenuItem IsNot Nothing AndAlso Me.RePriceReturnMenuItem.MenuItems.Count > 0 Then Me.RePriceReturnMenuItem.Enabled = enableControls
        End If


    End Sub
    Private Sub DetailLinesDataGrid_RowCountChanged(ByVal previousRowCount As Integer?, ByVal currentRowCount As Integer) Handles DetailLinesDataGrid.RowCountChanged
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' prevents the users from editing if the are not any lines to edit
        ' </summary>
        ' <param name="PreviousRowCount"></param>
        ' <param name="CurrentRowCount"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            currentRowCount = DetailLinesDataGrid.GetGridRowCount()
            If Not CStr(_ClaimDr("DOC_TYPE")).ToUpper.StartsWith("UTL_") Then

                If currentRowCount = 0 Then
                    DetailLineGroupBox.Enabled = False
                Else
                    DetailLineGroupBox.Enabled = True
                End If

                DisableEnableButtons()
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub DisableEnableButtons()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' checks if buttons should be enabled or disabled
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/5/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            Try
                DetailLinesDataGrid.GetGridRowCount()

            Catch ex As Exception

                ReCalcToolBarButton.Enabled = False
                RePriceToolBarButton.Enabled = False
                RePriceReturnToolBarButton.Enabled = False
                DropdownRepriceReturnToolBarButton.Enabled = False
                Return
            End Try

            If _WorkerThread IsNot Nothing AndAlso _WorkerThread.IsAlive Then
                ' Stop
            Else
                If DetailLinesDataGrid.GetGridRowCount = 0 Then
                    ReCalcToolBarButton.Enabled = False
                    RePriceReturnToolBarButton.Enabled = False
                    RePriceToolBarButton.Enabled = False
                    DropdownRepriceReturnToolBarButton.Enabled = False
                Else
                    ReCalcToolBarButton.Enabled = True

                    If AllowReprice() Then
                        RePriceReturnToolBarButton.Enabled = True
                        RePriceToolBarButton.Enabled = True
                        DropdownRepriceReturnToolBarButton.Enabled = True
                    Else
                        RePriceReturnToolBarButton.Enabled = False
                        RePriceToolBarButton.Enabled = False
                        DropdownRepriceReturnToolBarButton.Enabled = False
                    End If

                    CompleteToolBarButton.Enabled = True
                    CompleteMenuItem.Enabled = True
                End If
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function AllowReprice() As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Checks if the claim is allowed to be repriced
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/5/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim DocTypeRow As DataRow
        Try

            If _ClaimDr Is Nothing Then Return False

            If (_ClaimDr("EDI_SOURCE").ToString.Trim = "INHOUSE" AndAlso _ClaimDr("EDI_STATUS").ToString.Trim = "RECEIVED") Then Return False

            If _MedHdrDr IsNot Nothing AndAlso (_MedHdrDr("PRICED_BY").ToString.Contains("JAA") OrElse _MedHdrDr("PRICED_BY").ToString.Contains("835")) Then Return False

            DocTypeRow = CMSDALFDBMD.RetrieveDocType(CStr(_ClaimDr("DOC_CLASS")), CStr(_ClaimDr("DOC_TYPE")))

            If DocTypeRow IsNot Nothing Then
                If Not IsDBNull(DocTypeRow("BC_REPRICE_SW")) AndAlso CBool(DocTypeRow("BC_REPRICE_SW")) Then
                    Return True
                End If
            End If

            Return False
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function AllowComplete() As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Process any security or rules to allow the user to complete the item
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	12/20/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DV As DataView
        Dim AlertDV As DataView
        Dim Cnt As Integer

        Try

            If Not ValidateHeader() Then Return False
            If Not ValidateDetail() Then Return False

            'Valid Participant
            AlertDV = New DataView(_ClaimAlertManager.AlertManagerDataTable, "Category = 'Header' And Message = 'Invalid Participant'", "Category, Message", DataViewRowState.CurrentRows)
            If AlertDV.Count > 0 Then
                MessageBox.Show("A Valid Participant is Required." & vbCrLf & "Please Enter or Select a Valid Participant.", "Invalid Participant", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtPartSSN.Focus()
                Return False
            End If

            'Valid Patient
            AlertDV = New DataView(_ClaimAlertManager.AlertManagerDataTable, "Category = 'Header' And Message = 'Invalid Patient'", "", DataViewRowState.CurrentRows)
            DV = New DataView(_ClaimDS.MEDDTL, "STATUS <> 'DENY'", "LINE_NBR, STATUS", DataViewRowState.CurrentRows)
            If AlertDV.Count > 0 AndAlso DV.Count > 0 AndAlso _ClaimDS.MEDDTL.Rows.Count > 0 Then
                MessageBox.Show("A Valid Patient is Required" & vbCrLf & "Please Enter or Select a Valid Patient.", "Invalid Patient", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtPatSSN.Focus()
                Return False
            End If

            DV = New DataView(_ClaimDS.MEDDTL, " STATUS <> 'DENY' And STATUS <> 'MERGED'", "", DataViewRowState.CurrentRows)
            If DV.Count > 0 Then 'only fully validate if any lines are being payed
                'Valid Provider
                AlertDV = New DataView(_ClaimAlertManager.AlertManagerDataTable, "Category = 'Header' And (Message = 'Invalid Provider' or Message = 'Provider is Suspended') ", "", DataViewRowState.CurrentRows)
                If AlertDV.Count > 0 Then
                    MessageBox.Show("A Valid Provider is Required." & vbCrLf & "Please specify a Valid (Non-Suspended) Provider.", "Invalid Provider", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtProviderID.Focus()
                    Return False
                End If

                'Valid Provider
                AlertDV = New DataView(_ClaimAlertManager.AlertManagerDataTable, "Category = 'Header' And Message = 'Provider has Suspended Address'", "", DataViewRowState.CurrentRows)
                If AlertDV.Count > 0 Then
                    MessageBox.Show("A Valid Provider Address is Required" & vbCrLf & "Please Enter or Select a Valid Provider.", "Invalid Provider Address", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    txtProviderID.Focus()
                    Return False
                End If
            End If

            're-calc
            AlertDV = New DataView(_ClaimAlertManager.AlertManagerDataTable, "Category = 'Header' And Message = 'Re-Calc Is Required'", "Category, Message", DataViewRowState.CurrentRows)
            If AlertDV.Count > 0 Then
                MessageBox.Show("You Must Click Re-Calc Due To Changes Made.", "Re-Calc Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            're_price
            '            If RePriceToolBarButton.Enabled = True Then
            AlertDV = New DataView(_ClaimAlertManager.AlertManagerDataTable, "Category = 'Header' And Message = 'Re-Price Is Required'", "Category, Message", DataViewRowState.CurrentRows)
            If AlertDV.Count > 0 Then
                MessageBox.Show("You Must Click Re-Price Due To Changes Made.", "Re-Price Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
            '           End If

            If _HighestEntryID <> _ClaimMemberAccumulatorManager.GetHighestEntryIdForFamily AndAlso CStr(_MedHdrDr("RELATION_ID")).Trim <> "-1" Then 'if patient is unidentified accumulators would not have been loaded
                MessageBox.Show("Changes have occurred to Accumulators since you opened the Claim." & vbCrLf &
                "Please Re-Calc Claim and verify accumulator values.", "Changes occurred", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If


            ' From Date is required
            DV = New DataView(_ClaimDS.MEDDTL, "STATUS = 'PAY' OR STATUS = 'DENY'", "LINE_NBR", DataViewRowState.CurrentRows)
            If DV.Count > 0 Then
                Dim Lines As String = ""
                Dim ErrorCount As Integer = 0
                For Cnt = 0 To DV.Count - 1
                    If IsDBNull(DV(Cnt)("OCC_FROM_DATE")) OrElse (CStr(DV(Cnt)("OCC_FROM_DATE")).TrimEnd = "") Then
                        Lines &= If(ErrorCount <> 0, ", ", "") & If(Cnt = DV.Count - 1 AndAlso ErrorCount <> 0, "& ", "") & DV(Cnt)("LINE_NBR").ToString
                        ErrorCount += 1
                    End If
                Next
                If Lines.Length > 0 Then
                    MessageBox.Show("Line" & If(ErrorCount > 1, "s ", " ") & Lines & If(ErrorCount > 1, " are ", " is ") & "missing Required Start Date ", "Invalid Date of Service", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            End If

            ''TO date is earlier than  than the FROM date
            DV = New DataView(_ClaimDS.MEDDTL, "", "LINE_NBR", DataViewRowState.CurrentRows)
            If DV.Count > 0 Then
                Dim Lines As String = ""

                For Cnt = 0 To DV.Count - 1
                    If Not IsDBNull(DV(Cnt)("OCC_FROM_DATE")) AndAlso Not IsDBNull(DV(Cnt)("OCC_TO_DATE")) Then
                        If CDate(DV(Cnt)("OCC_TO_DATE")) < CDate(DV(Cnt)("OCC_FROM_DATE")) Then
                            Lines &= If(Cnt <> 0, ", ", "") & If(Cnt = DV.Count - 1 AndAlso Cnt <> 0, "& ", "") & DV(Cnt)("LINE_NBR").ToString
                        End If
                    End If
                Next
                If Lines.Length > 0 Then
                    MessageBox.Show("Line" & If(DV.Count > 1, "s ", " ") & Lines & " Ending Date is before Start Date ", "Invalid Date Of Service Range", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            End If

            ''Status should be Either PAY or DENY
            DV = New DataView(_ClaimDS.MEDDTL, "STATUS <> 'PAY' AND STATUS <> 'DENY'", "LINE_NBR", DataViewRowState.CurrentRows)
            If DV.Count > 0 Then
                Dim Lines As String = ""
                Dim ErrorCount As Integer = 0
                For Cnt = 0 To DV.Count - 1
                    Lines &= If(ErrorCount <> 0, ", ", "") & If(Cnt = DV.Count - 1 AndAlso ErrorCount <> 0, "& ", "") & DV(Cnt)("LINE_NBR").ToString
                Next
                MessageBox.Show("Line" & If(ErrorCount > 1, "s ", " ") & Lines & " Action Should be Either PAY or DENY", "Invalid Action", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            ' For DENY status Paid Amount should be Zero
            DV = New DataView(_ClaimDS.MEDDTL, "STATUS = 'DENY' AND ISNULL(PAID_AMT, 0) > 0", "LINE_NBR", DataViewRowState.CurrentRows)
            If DV.Count > 0 Then
                Dim Lines As String = ""

                For Cnt = 0 To DV.Count - 1
                    Lines &= If(Cnt <> 0, ", ", "") & If(Cnt = DV.Count - 1 AndAlso Cnt <> 0, "& ", "") & DV(Cnt)("LINE_NBR").ToString
                Next
                MessageBox.Show("Line" & If(DV.Count > 1, "s ", " ") & Lines & " Action is DENY, Paid shoud be ZERO ", "Invalid Paid", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            ''Cannot complete with negative paid amount.
            DV = New DataView(_ClaimDS.MEDDTL, "STATUS = 'PAY' AND ISNULL(PAID_AMT, 0) < 0", "LINE_NBR", DataViewRowState.CurrentRows)
            If DV.Count > 0 Then
                Dim Lines As String = ""

                For Cnt = 0 To DV.Count - 1
                    Lines &= If(Cnt <> 0, ", ", "") & If(Cnt = DV.Count - 1 AndAlso Cnt <> 0, "& ", "") & DV(Cnt)("LINE_NBR").ToString
                Next
                MessageBox.Show("Line" & If(DV.Count > 1, "s ", " ") & Lines & " Paid should not be Negative ", "Invalid Paid", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            'below not needed
            'ViewedEligibility
            'ViewedClaimHistory
            'freetext
            If Not _ViewedFreeText AndAlso WorkFreeTextEditor.HasData Then
                MessageBox.Show("You Must Look At The Free Text Tab.", "Free Text Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Tabs.SelectedTab = FreeTextTabPage
                Return False
            End If

            If Not _ViewedCOB Then 'AndAlso CobControl.COBDataSet.Tables("MEDOTHER_INS").Rows.Count > 0 Then
                MessageBox.Show("You Must Look At The COB Tab.", "COB Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Tabs.SelectedTab = COBTabPage
                Return False
            End If

            'duplicates
            If Not _ViewedDuplicates AndAlso DupsTreeView.GetNodeCount(True) > 1 Then
                MessageBox.Show("You Must Look At The Duplicates Tab.", "Duplicates Exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Tabs.SelectedTab = DuplicatesTabPage
                Return False
            End If

            DisableEnableButtons()

            Return True

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub CreateMEDHDR()

        Dim MedHdrDR As DataRow

        Try

            MedHdrDR = _ClaimDS.MEDHDR.NewMEDHDRRow
            MedHdrDR.BeginEdit()

            MedHdrDR("CLAIM_ID") = _ClaimDr("CLAIM_ID")
            MedHdrDR("FAMILY_ID") = _ClaimDr("FAMILY_ID")
            MedHdrDR("RELATION_ID") = _ClaimDr("RELATION_ID")
            MedHdrDR("PART_SSN") = _ClaimDr("PART_SSN")
            MedHdrDR("PAT_SSN") = _ClaimDr("PAT_SSN")
            MedHdrDR("DOCID") = _ClaimDr("DOCID")
            MedHdrDR("MAXID") = _ClaimDr("MAXID")
            MedHdrDR("SYSTEM_CODE") = 0
            MedHdrDR("STATUS") = _ClaimDr("STATUS")
            MedHdrDR("STATUS_DATE") = _ClaimDr("STATUS_DATE")

            If cmbHosp.Text = "" Then
                MedHdrDR("ADMITTANCE") = DBNull.Value
            ElseIf cmbHosp.Text = "INPATIENT" Then
                MedHdrDR("ADMITTANCE") = "I"
            ElseIf cmbHosp.Text = "OUTPATIENT" Then
                MedHdrDR("ADMITTANCE") = "O"
            End If

            MedHdrDR("REC_DATE") = _ClaimDr("REC_DATE")
            MedHdrDR("NETWRK_REC_DATE") = DBNull.Value 'ClaimMasterDR("NETWRK_REC_DATE")

            MedHdrDR("OCC_FROM_DATE") = DBNull.Value
            MedHdrDR("OCC_TO_DATE") = DBNull.Value

            'If txtIncidentDate.Text.ToString.Trim.Length = 0 Then
            '    MedHdrDR("INCIDENT_DATE") = DBNull.Value
            'Else
            '    MedHdrDR("INCIDENT_DATE") = CDate(txtIncidentDate.Text.ToString.Trim)
            'End If

            '' For PPO
            'If cmbPricingNetwork.Text.ToString.Trim.Length = 0 Then
            '    MedHdrDR("PPO") = DBNull.Value
            'Else
            '    MedHdrDR("PPO") = cmbPricingNetwork.Text.ToString.Trim
            'End If

            '' For COB
            'If cmbCOB.Text.ToString.Trim.Length = 0 Then
            '    MedHdrDR("COB") = DBNull.Value
            'Else
            '    MedHdrDR("COB") = cmbCOB.SelectedValue
            'End If

            '' For PAYEE
            'If cmbPayee.Text.ToString.Trim.Length = 0 Then
            '    MedHdrDR("PAYEE") = DBNull.Value
            'Else
            '    MedHdrDR("PAYEE") = cmbPayee.Text.ToString.Trim
            'End If

            MedHdrDR("SECURITY_SW") = _ClaimDr("SECURITY_SW")
            MedHdrDR("DUPLICATE_SW") = _ClaimDr("DUPLICATE_SW")

            'If NonPARCheckBox.Checked Then
            '    MedHdrDR("NON_PAR_SW") = 1
            'Else
            '    MedHdrDR("NON_PAR_SW") = 0
            'End If

            'If OOACheckBox.Checked Then
            '    MedHdrDR("OUT_OF_AREA_SW") = 1
            'Else
            '    MedHdrDR("OUT_OF_AREA_SW") = 0
            'End If

            'If AutoCheckBox.Checked Then
            '    MedHdrDR("AUTO_ACCIDENT_SW") = 1
            'Else
            '    MedHdrDR("AUTO_ACCIDENT_SW") = 0
            'End If

            'If WorkersCompCheckBox.Checked Then
            '    MedHdrDR("WORKERS_COMP_SW") = 1
            'Else
            '    MedHdrDR("WORKERS_COMP_SW") = 0
            'End If

            'If OtherCheckBox.Checked Then
            '    MedHdrDR("OTH_ACCIDENT_SW") = 1
            'Else
            '    MedHdrDR("OTH_ACCIDENT_SW") = 0
            'End If

            ''' OTH_INS_SW is depends on COB(1 or 2) Value
            'If Not IsNothing(cmbCOB.SelectedValue) AndAlso Not IsDBNull(cmbCOB.SelectedValue) AndAlso (cmbCOB.SelectedValue.ToString = "1" OrElse cmbCOB.SelectedValue.ToString = "2") Then
            '    MedHdrDR("OTH_INS_SW") = 1
            'Else
            '    MedHdrDR("OTH_INS_SW") = 0
            'End If

            MedHdrDR("OTH_INS_REFUSAL_SW") = 0

            MedHdrDR("TOT_CHRG_AMT") = 0D

            MedHdrDR("ATTACH_SW") = _ClaimDr("ATTACH_SW")

            'If ChiroCheckBox.Checked Then
            '    MedHdrDR("CHIRO_SW") = 1
            'Else
            '    MedHdrDR("CHIRO_SW") = 0
            'End If

            'If ChiroCheckBox.Checked Then
            '    MedHdrDR("OTH_INS_SW") = 1
            'Else
            '    MedHdrDR("OTH_INS_SW") = 0
            'End If

            MedHdrDR("PARITY_SW") = 0
            MedHdrDR("SED_SW") = 0

            'If AuthCheckBox.Checked Then
            '    MedHdrDR("AUTHORIZED_SW") = 1
            'Else
            '    MedHdrDR("AUTHORIZED_SW") = 0
            'End If

            MedHdrDR("ASSIGN_OF_BEN_SW") = 0
            MedHdrDR("ADJUSTMENT_SW") = 0

            MedHdrDR("PAT_FNAME") = _ClaimDr("PAT_FNAME")
            MedHdrDR("PAT_LNAME") = _ClaimDr("PAT_LNAME")

            'If txtPatGender.Text.ToString.Trim.Length = 0 Then
            '    MedHdrDR("PAT_SEX") = DBNull.Value
            'Else
            '    MedHdrDR("PAT_SEX") = txtPatGender.Text.ToString.Trim
            'End If

            If txtPatBirthDate.Text.ToString.Trim.Length = 0 Then
                If Not IsDBNull(_PatientKey.PatientDOB) Then
                    MedHdrDR("PAT_DOB") = CDate(_PatientKey.PatientDOB)
                Else
                    MedHdrDR("PAT_DOB") = DBNull.Value
                End If
            Else
                MedHdrDR("PAT_DOB") = CDate(txtPatBirthDate.Text.ToString.Trim)
            End If

            'If txtPatAcctNo.Text.ToString.Trim.Length = 0 Then
            '    MedHdrDR("PAT_ACCT_NBR") = DBNull.Value
            'Else
            '    MedHdrDR("PAT_ACCT_NBR") = txtPatAcctNo.Text.ToString.Trim
            'End If

            MedHdrDR("PROV_TIN") = _ClaimDr("PROV_TIN")
            MedHdrDR("PROV_ID") = _ClaimDr("PROV_ID")

            'If txtBCCZIP.Text.ToString.Trim.Length = 0 Then
            '    MedHdrDR("PROV_ZIP") = DBNull.Value
            'Else
            '    MedHdrDR("PROV_ZIP") = txtBCCZIP.Text.ToString.Trim
            'End If

            'If txtProviderLicenseNo.Text.ToString.Trim.Length = 0 Then
            '    MedHdrDR("PROV_LICENSE") = DBNull.Value
            'Else
            '    MedHdrDR("PROV_LICENSE") = txtProviderLicenseNo.Text.ToString.Trim
            'End If

            'If txtProviderRenderingNPI.Text.ToString.Trim.Length = 0 Then
            '    MedHdrDR("RENDERING_NPI") = DBNull.Value
            'Else
            '    MedHdrDR("RENDERING_NPI") = txtProviderRenderingNPI.Text.ToString.Trim
            'End If

            If _ClaimDr("EDI_SOURCE").ToString = "INHOUSE" Then MedHdrDR("PRICED_BY") = "BLUE CROSS JAA (I)"

            MedHdrDR.EndEdit()

            _ClaimDS.MEDHDR.Rows.Add(MedHdrDR)

            '  _MedHdrBS.DataSource = _ClaimDS.Tables("MEDHDR")

            _MedHdrBS.EndEdit()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub FinalizeEdits()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' unloads header info that can't be handled by a databinding
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            For Each DT As DataTable In _ClaimDS.Tables
                Dim BMB As BindingManagerBase = Me.BindingContext(DT)
                BMB.EndCurrentEdit()
            Next
            If _MedDtlBS IsNot Nothing Then
                _MedDtlBS.ResetBindings(True)
                _MedDtlBS.EndEdit()
            End If

            If _MedHdrBS IsNot Nothing Then
                _MedHdrBS.ResetBindings(True)
                _MedHdrBS.EndEdit()
            End If

            If _ClaimMasterBS IsNot Nothing Then
                _ClaimMasterBS.ResetBindings(True)
                _ClaimMasterBS.EndEdit()
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Function IdentifyChanges(ByVal TargetDT As DataTable, ByVal TargetDR As DataRow) As Boolean
        Try
            Dim ChangeTable As DataTable = TargetDT.GetChanges(DataRowState.Added Or DataRowState.Modified Or DataRowState.Deleted)
            Dim r As DataRow
            Dim ChangeCnt As Integer = 0
            If ChangeTable IsNot Nothing Then
                r = ChangeTable.Rows(0)
                For cnt As Integer = 0 To ChangeTable.Columns.Count - 1
                    If IsDBNull(r(cnt)) = False AndAlso TargetDR.HasVersion(DataRowVersion.Original) AndAlso IsDBNull(TargetDR(cnt, DataRowVersion.Original)) = False Then
                        If UFCWGeneral.IsNullStringHandler(r(cnt)) <> UFCWGeneral.IsNullStringHandler(TargetDR(cnt, DataRowVersion.Original)) Then
                            ChangeCnt += 1
                        End If
                    ElseIf IsDBNull(r(cnt)) = False AndAlso TargetDR.HasVersion(DataRowVersion.Original) AndAlso IsDBNull(TargetDR(cnt, DataRowVersion.Original)) = True Then
                        ChangeCnt += 1
                    ElseIf IsDBNull(r(cnt)) = True AndAlso TargetDR.HasVersion(DataRowVersion.Original) AndAlso IsDBNull(TargetDR(cnt, DataRowVersion.Original)) = False Then
                        If TargetDR(cnt, DataRowVersion.Original).GetType <> GetType(Decimal) AndAlso Not ChangeTable.Columns(cnt).ToString.StartsWith("TOT") Then
                            ChangeCnt += 1
                        End If
                    End If
                Next
            End If
            If ChangeCnt > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function HasChanges() As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' determines if any changes have been made to the claim
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            If _ClaimDS.CLAIM_MASTER.GetChanges(DataRowState.Added Or DataRowState.Modified Or DataRowState.Deleted) IsNot Nothing AndAlso _ClaimDS.CLAIM_MASTER.GetChanges(DataRowState.Added Or DataRowState.Modified Or DataRowState.Deleted).Rows.Count > 0 Then
#If TRACE Then
                If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": CLAIM_MASTER Changed" & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
                If IdentifyChanges(DirectCast(_ClaimMasterBS.DataSource, DataTable), _ClaimDr) Then Return True
            End If

            If _ClaimDS.MEDHDR.GetChanges IsNot Nothing AndAlso _ClaimDS.MEDHDR.GetChanges.Rows.Count > 0 Then
#If TRACE Then
                If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": MEDHDR Changed" & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
                If IdentifyChanges(_ClaimDS.MEDHDR, _MedHdrDr) Then Return True
            End If

            If _ClaimDS.MEDDTL.GetChanges IsNot Nothing AndAlso _ClaimDS.MEDDTL.GetChanges.Rows.Count > 0 Then
#If TRACE Then
                If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": MEDDTL Changed" & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
                Dim Modifications As String = ""

                For Each DR As DataRow In _ClaimDS.MEDDTL.GetChanges.Rows
                    If DR.RowState <> DataRowState.Added Then
                        Modifications = UFCWGeneral.IdentifyChanges(DR, DetailLinesDataGrid.GetCurrentTableStyle())
                        If Modifications IsNot Nothing AndAlso Modifications.Length > 0 Then
                            Return True
                        End If
                    Else
                        Return True
                    End If
                Next
            End If

            If _ClaimDS.REASON.GetChanges IsNot Nothing AndAlso _ClaimDS.REASON.GetChanges.Rows.Count > 0 Then
#If TRACE Then
                If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": REASON Changed" & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
                Return True
            End If

            If _ClaimDS.MEDDIAG.GetChanges IsNot Nothing AndAlso _ClaimDS.MEDDIAG.GetChanges.Rows.Count > 0 Then
#If TRACE Then
                If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": MEDDIAG Changed" & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
                Return True
            End If

            If _ClaimDS.MEDMOD.GetChanges IsNot Nothing AndAlso _ClaimDS.MEDMOD.GetChanges.Rows.Count > 0 Then
#If TRACE Then
                If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": MEDMOD Changed" & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
                Return True
            End If

            If _ClaimDS.ANNOTATIONS.GetChanges IsNot Nothing AndAlso _ClaimDS.ANNOTATIONS.GetChanges.Rows.Count > 0 Then
#If TRACE Then
                If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": ANNOTATIONS Changed" & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
                Return True
            End If

            Return False

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function SaveChanges(ByRef transaction As DbTransaction) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Saves all changes made to the claim back to the database
        ' </summary>
        ' <param name="Transaction"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim PatientKeyChange As Boolean = False
        Dim PatientNameChange As Boolean = False
        Dim PartSSNChange As Boolean = False
        Dim AmountChanged As Boolean = False
        Dim TotChrg As Decimal = 0
        Dim TotPriced As Decimal = 0
        Dim TotAllowed As Decimal = 0
        Dim TotOtherIns As Decimal = 0
        Dim TotPaid As Decimal = 0
        Dim TotProcessed As Decimal = 0
        Dim DocSec As String
        Dim HistSum As String = "CLAIM ID " & Format(_ClaimID, "00000000") & " INDEX UPDATE"
        Dim HistDetail As String = "INDEXES UPDATED:" '= "ADJUSTER " & DomainUser.ToUpper & " MARKED THIS DOCUMENT COMPLETE." & Microsoft.VisualBasic.vbCrLf & "ENDING WORKFLOW PROCESSING."
        Dim OrigDV As DataView
        Dim CurDV As DataView
        Dim HistEntryDT As DataTable
        Dim HistDR As DataRow
        Dim Unpended As Boolean = False
        Dim DocTypeChange As Boolean = False
        Dim SecurityChange As Boolean = False

        Try

            HistEntryDT = New DataTable("HistoryEntries")

            HistEntryDT.Columns.Add("RowNum", System.Type.GetType("System.Int32"))
            HistEntryDT.Columns.Add("EntryPosition", System.Type.GetType("System.Int32"))
            HistEntryDT.Columns.Add("Detail", System.Type.GetType("System.String"))

            Using WC As New GlobalCursor

                If _MedDtlBS.Count > 0 Then

                    TotChrg = SumCharges()
                    TotPriced = SumPriced()
                    SyncAllowed()
                    TotAllowed = SumAllowed()
                    TotOtherIns = SumOI()
                    TotPaid = SumPaid()
                    TotProcessed = SumProcessed()


                    If Not AmountChanged AndAlso Not IsDBNull(_MedHdrDr("TOT_CHRG_AMT")) AndAlso CDec(_MedHdrDr("TOT_CHRG_AMT")) <> TotChrg Then
                        AmountChanged = True
                    ElseIf Not AmountChanged AndAlso IsDBNull(_MedHdrDr("TOT_CHRG_AMT")) Then
                        AmountChanged = True
                    End If

                    If Not AmountChanged AndAlso Not IsDBNull(_MedHdrDr("TOT_PRICED_AMT")) AndAlso CDec(_MedHdrDr("TOT_PRICED_AMT")) <> TotPriced Then
                        AmountChanged = True
                    ElseIf Not AmountChanged AndAlso IsDBNull(_MedHdrDr("TOT_PRICED_AMT")) Then
                        AmountChanged = True
                    End If

                    If Not AmountChanged AndAlso Not IsDBNull(_MedHdrDr("TOT_ALLOWED_AMT")) AndAlso CDec(_MedHdrDr("TOT_ALLOWED_AMT")) <> TotAllowed Then
                        AmountChanged = True
                    ElseIf Not AmountChanged AndAlso IsDBNull(_MedHdrDr("TOT_ALLOWED_AMT")) Then
                        AmountChanged = True
                    End If

                    If Not AmountChanged AndAlso Not IsDBNull(_MedHdrDr("TOT_OTH_INS_AMT")) AndAlso CDec(_MedHdrDr("TOT_OTH_INS_AMT")) <> TotOtherIns Then
                        AmountChanged = True
                    ElseIf Not AmountChanged AndAlso IsDBNull(_MedHdrDr("TOT_OTH_INS_AMT")) Then
                        AmountChanged = True
                    End If

                    If Not AmountChanged AndAlso Not IsDBNull(_MedHdrDr("TOT_PAID_AMT")) AndAlso CDec(_MedHdrDr("TOT_PAID_AMT")) <> TotPaid Then
                        AmountChanged = True
                    ElseIf Not AmountChanged AndAlso IsDBNull(_MedHdrDr("TOT_PAID_AMT")) Then
                        AmountChanged = True
                    End If

                    If Not AmountChanged AndAlso Not IsDBNull(_MedHdrDr("TOT_PROCESSED_AMT")) AndAlso CDec(_MedHdrDr("TOT_PROCESSED_AMT")) <> TotProcessed Then
                        AmountChanged = True
                    ElseIf Not AmountChanged AndAlso IsDBNull(_MedHdrDr("TOT_PROCESSED_AMT")) Then
                        AmountChanged = True
                    End If
                    EstablishDOSRange(_ClaimDr, _MedHdrDr)
                End If

                If _ClaimDS.CLAIM_MASTER.GetChanges IsNot Nothing AndAlso _ClaimDS.CLAIM_MASTER.GetChanges.Rows.Count > 0 Then
                    If (_PatientKey.FamilyID <> CInt(_ClaimDr("FAMILY_ID"))) _
                                OrElse (_PatientKey.ParticipantSSN <> CInt(_ClaimDr("PART_SSN"))) _
                                OrElse (_PatientKey.RelationID <> CInt(_ClaimDr("RELATION_ID"))) _
                                OrElse (_PatientKey.PatientSSN <> CInt(_ClaimDr("PAT_SSN"))) _
                                OrElse (_PatientKey.SecuritySW <> CBool(_ClaimDr("SECURITY_SW"))) Then
                        PatientKeyChange = True

                    End If

                    If _PatientKey.ParticipantSSN <> CInt(_ClaimDr("PART_SSN")) Then
                        PartSSNChange = True
                    End If

                    HistEntryDT = IdentifyChanges(_ClaimDS.CLAIM_MASTER, HistEntryDT, DataRowState.Modified)

                    OrigDV = New DataView(_ClaimDS.CLAIM_MASTER, "", "", DataViewRowState.OriginalRows)
                    CurDV = New DataView(_ClaimDS.CLAIM_MASTER, "", "", DataViewRowState.CurrentRows)

                    If OrigDV(0)("DOC_TYPE").ToString <> CurDV(0)("DOC_TYPE").ToString Then
                        DocTypeChange = True
                        CMSDALFDBMD.CreateDocHistory(_ClaimID, UFCWGeneral.IsNullLongHandler(_ClaimDr("DOCID"), "DOCID"), "UPDATE", CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")), CInt(_ClaimDr("PART_SSN")), CInt(_ClaimDr("PAT_SSN")), CStr(_ClaimDr("DOC_CLASS")), CStr(_ClaimDr("DOC_TYPE")), "Claim " & _ClaimID & " Reassigned to type " & _ClaimDr("DOC_TYPE").ToString, "Re-routed by system after change", _DomainUser.ToUpper, transaction)
                    End If

                    If CBool(OrigDV(0)("SECURITY_SW")) <> CBool(CurDV(0)("SECURITY_SW")) Then
                        SecurityChange = True
                    End If

                    CMSDALFDBMD.UpdateClaimMaster(_ClaimID,
                                                    UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PART_SSN"), "PART_SSN"),
                                                    UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PAT_SSN"), "PAT_SSN"),
                                                    CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")),
                                                    CShort(_ClaimDr("PRIORITY")), Math.Abs(CDec(_ClaimDr("SECURITY_SW"))),
                                                    Math.Abs(CDec(_ClaimDr("ATTACH_SW"))), Math.Abs(CDec(_ClaimDr("DUPLICATE_SW"))),
                                                    Math.Abs(CDec(_ClaimDr("BUSY_SW"))), TryCast(_ClaimDr("STATUS"), String),
                                                    UFCWGeneral.IsNullDateHandler(_ClaimDr("STATUS_DATE"), "STATUS_DATE"), TryCast(_ClaimDr("DOC_CLASS"), String),
                                                    TryCast(_ClaimDr("DOC_TYPE"), String),
                                                    UFCWGeneral.IsNullDateHandler(_ClaimDr("DATE_OF_SERVICE"), "DATE_OF_SERVICE"),
                                                    TryCast(_ClaimDr("PAT_FNAME"), String), TryCast(_ClaimDr("PAT_INT"), String),
                                                    TryCast(_ClaimDr("PAT_LNAME"), String), TryCast(_ClaimDr("PART_FNAME"), String),
                                                    TryCast(_ClaimDr("PART_INT"), String), TryCast(_ClaimDr("PART_LNAME"), String),
                                                    UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PROV_TIN"), "PROV_TIN"),
                                                    UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PROV_ID"), "PROV_ID", True),
                                                    UFCWGeneral.IsNullIntegerHandler(_ClaimDr("REFRENCE_CLAIM"), "REFRENCE_CLAIM"),
                                                    _DomainUser.ToUpper, transaction)
                End If

                If PatientKeyChange OrElse PatientNameChange OrElse AmountChanged OrElse (_ClaimDS.MEDHDR.GetChanges IsNot Nothing AndAlso _ClaimDS.MEDHDR.GetChanges.Rows.Count > 0) Then

                    If _MedHdrDr.RowState = DataRowState.Added Then

                        HistDR = HistEntryDT.NewRow

                        HistDR("RowNum") = 0
                        HistDR("EntryPosition") = HistEntryDT.Rows.Count + 1
                        HistDR("Detail") = "HEADER INFORMATION WAS CREATED"

                        HistEntryDT.Rows.Add(HistDR)

                        HistEntryDT = IdentifyChanges(_ClaimDS.MEDHDR, HistEntryDT, DataRowState.Added)

                        CMSDALFDBMD.CreateMEDHDR(_ClaimID, Math.Abs(CDec(_ClaimDr("SECURITY_SW"))),
                                                    CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")),
                                                    UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PART_SSN"), "PART_SSN"),
                                                    UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PAT_SSN"), "PAT_SSN"),
                                                    UFCWGeneral.IsNullLongHandler(_ClaimDr("DOCID"), "DOCID"),
                                                    TryCast(_ClaimDr("MAXID"), String),
                                                    TryCast(_MedHdrDr("SYSTEM_CODE"), String), TryCast(_ClaimDr("STATUS"), String),
                                                    UFCWGeneral.IsNullDateHandler(_ClaimDr("STATUS_DATE"), "STATUS_DATE"),
                                                    TryCast(_MedHdrDr("CLAIM_TYPE"), String),
                                                    TryCast(_MedHdrDr("ADMITTANCE"), String),
                                                    UFCWGeneral.IsNullDateHandler(_MedHdrDr("REC_DATE"), "REC_DATE"),
                                                    UFCWGeneral.IsNullDateHandler(_MedHdrDr("NETWRK_REC_DATE"), "NETWRK_REC_DATE"),
                                                    UFCWGeneral.IsNullDateHandler(_MedHdrDr("OCC_FROM_DATE"), "OCC_FROM_DATE"),
                                                    UFCWGeneral.IsNullDateHandler(_MedHdrDr("OCC_TO_DATE"), "OCC_TO_DATE"),
                                                    UFCWGeneral.IsNullDateHandler(_MedHdrDr("INCIDENT_DATE"), "INCIDENT_DATE"),
                                                    TryCast(_MedHdrDr("PPO"), String),
                                                    TryCast(_MedHdrDr("COB"), String),
                                                    TryCast(_MedHdrDr("PAYEE"), String),
                                                    TryCast(_MedHdrDr("PRICED_BY"), String),
                                                    TryCast(_MedHdrDr("PRICING_ERROR"), String),
                                                    Math.Abs(CDec(_MedHdrDr("ATTACH_SW"))), Math.Abs(CDec(_ClaimDr("DUPLICATE_SW"))), Math.Abs(CDec(_MedHdrDr("NON_PAR_SW"))),
                                                    Math.Abs(CDec(_MedHdrDr("OUT_OF_AREA_SW"))), Math.Abs(CDec(_MedHdrDr("AUTO_ACCIDENT_SW"))), Math.Abs(CDec(_MedHdrDr("WORKERS_COMP_SW"))),
                                                    Math.Abs(CDec(PreventativeDiagnosisPresent())),
                                                    Math.Abs(CDec(_MedHdrDr("OTH_ACCIDENT_SW"))), Math.Abs(CDec(_MedHdrDr("CHIRO_SW"))), Math.Abs(CDec(_MedHdrDr("OTH_INS_SW"))), Math.Abs(CDec(_MedHdrDr("PARITY_SW"))), Math.Abs(CDec(_MedHdrDr("SED_SW"))),
                                                    Math.Abs(CDec(_MedHdrDr("AUTHORIZED_SW"))), Math.Abs(CDec(_MedHdrDr("ASSIGN_OF_BEN_SW"))), Math.Abs(CDec(_MedHdrDr("ADJUSTMENT_SW"))), Math.Abs(CDec(_MedHdrDr("OTH_INS_REFUSAL_SW"))),
                                                    UFCWGeneral.IsNullIntegerHandler(_MedHdrDr("OTH_INS_ID"), "OTH_INS_ID"),
                                                    TryCast(_MedHdrDr("OTH_INS_POLICY_NBR"), String),
                                                    TryCast(_ClaimDr("PAT_FNAME"), String),
                                                    TryCast(_ClaimDr("PAT_LNAME"), String),
                                                    TryCast(_MedHdrDr("PAT_SEX"), String),
                                                    UFCWGeneral.IsNullDateHandler(_MedHdrDr("PAT_DOB"), "PAT_DOB"),
                                                    TryCast(_MedHdrDr("PAT_ACCT_NBR"), String),
                                                    UFCWGeneral.IsNullIntegerHandler(_MedHdrDr("PAT_ZIP"), "PAT_ZIP"),
                                                    UFCWGeneral.IsNullShortHandler(_MedHdrDr("PAT_ZIP2"), "PAT_ZIP2"),
                                                    UFCWGeneral.IsNullIntegerHandler(_MedHdrDr("PROV_TIN"), "PROV_TIN"),
                                                    UFCWGeneral.IsNullIntegerHandler(_MedHdrDr("PROV_ID"), "PROV_ID"),
                                                    UFCWGeneral.IsNullIntegerHandler(_MedHdrDr("PROV_ZIP"), "PROV_ZIP"),
                                                    UFCWGeneral.IsNullShortHandler(_MedHdrDr("PROV_ZIP2"), "PROV_ZIP2"),
                                                    TryCast(_MedHdrDr("PROV_LICENSE"), String),
                                                    UFCWGeneral.IsNullLongHandler(_MedHdrDr("RENDERING_NPI"), "RENDERING_NPI"),
                                                    UFCWGeneral.IsNullIntegerHandler(_MedHdrDr("BILL_TAXID"), "BILL_TAXID"),
                                                    TryCast(_MedHdrDr("BILL_NAME"), String), TryCast(_MedHdrDr("BILL_ADDR1"), String),
                                                    TryCast(_MedHdrDr("BILL_ADDR2"), String), TryCast(_MedHdrDr("BILL_CITY"), String),
                                                    TryCast(_MedHdrDr("BILL_STATE"), String),
                                                    UFCWGeneral.IsNullIntegerHandler(_MedHdrDr("BILL_ZIP"), "BILL_ZIP"),
                                                    UFCWGeneral.IsNullShortHandler(_MedHdrDr("BILL_ZIP2"), "BILL_ZIP2"),
                                                    UFCWGeneral.IsNullDecimalHandler(TotChrg, "TotChrg"),
                                                    UFCWGeneral.IsNullDecimalHandler(TotPriced, "TotPriced"),
                                                    UFCWGeneral.IsNullDecimalHandler(TotAllowed, "TotAllowed"),
                                                    UFCWGeneral.IsNullDecimalHandler(TotOtherIns, "TotOtherIns"),
                                                    UFCWGeneral.IsNullDecimalHandler(TotPaid, "TotPaid"),
                                                    UFCWGeneral.IsNullDecimalHandler(TotProcessed, "TotProcessed"),
                                                    UFCWGeneral.IsNullIntegerHandler(_MedHdrDr("HOLD_DAYS"), "HOLD_DAYS"),
                                                    UFCWGeneral.IsNullDateHandler(_MedHdrDr("HOLD_DATE"), "HOLD_DATE"),
                                                    TryCast(_MedHdrDr("HOLD_TIME"), String), _DomainUser.ToUpper, transaction)

                    Else
                        HistEntryDT = IdentifyChanges(_ClaimDS.MEDHDR, HistEntryDT, DataRowState.Modified)

                        CMSDALFDBMD.UpdateMEDHDR(_ClaimID, Math.Abs(CDec(_ClaimDr("SECURITY_SW"))),
                                                    CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")),
                                                    UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PART_SSN"), "PART_SSN"),
                                                    UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PAT_SSN"), "PAT_SSN"),
                                                    TryCast(_MedHdrDr("SYSTEM_CODE"), String),
                                                    TryCast(_ClaimDr("STATUS"), String),
                                                    UFCWGeneral.IsNullDateHandler(_ClaimDr("STATUS_DATE"), "STATUS_DATE"),
                                                    TryCast(_MedHdrDr("CLAIM_TYPE"), String),
                                                    TryCast(_MedHdrDr("ADMITTANCE"), String),
                                                    UFCWGeneral.IsNullDateHandler(_MedHdrDr("REC_DATE"), "REC_DATE"),
                                                    UFCWGeneral.IsNullDateHandler(_MedHdrDr("NETWRK_REC_DATE"), "NETWRK_REC_DATE"),
                                                    UFCWGeneral.IsNullDateHandler(_MedHdrDr("OCC_FROM_DATE"), "OCC_FROM_DATE"),
                                                    UFCWGeneral.IsNullDateHandler(_MedHdrDr("OCC_TO_DATE"), "OCC_TO_DATE"),
                                                    UFCWGeneral.IsNullDateHandler(_MedHdrDr("INCIDENT_DATE"), "INCIDENT_DATE"),
                                                    TryCast(_MedHdrDr("PPO"), String), TryCast(_MedHdrDr("COB"), String),
                                                    TryCast(_MedHdrDr("PAYEE"), String), TryCast(_MedHdrDr("PRICED_BY"), String),
                                                    TryCast(_MedHdrDr("PRICING_ERROR"), String),
                                                    Math.Abs(CDec(_MedHdrDr("ATTACH_SW"))), Math.Abs(CDec(_ClaimDr("DUPLICATE_SW"))), Math.Abs(CDec(_MedHdrDr("NON_PAR_SW"))),
                                                    Math.Abs(CDec(_MedHdrDr("OUT_OF_AREA_SW"))), Math.Abs(CDec(_MedHdrDr("AUTO_ACCIDENT_SW"))),
                                                    Math.Abs(CDec(_MedHdrDr("WORKERS_COMP_SW"))), Math.Abs(CDec(PreventativeDiagnosisPresent())), Math.Abs(CDec(_MedHdrDr("OTH_ACCIDENT_SW"))), Math.Abs(CDec(_MedHdrDr("CHIRO_SW"))),
                                                    Math.Abs(CDec(_MedHdrDr("PARITY_SW"))), Math.Abs(CDec(_MedHdrDr("SED_SW"))), Math.Abs(CDec(_MedHdrDr("AUTHORIZED_SW"))), Math.Abs(CDec(_MedHdrDr("ASSIGN_OF_BEN_SW"))),
                                                    Math.Abs(CDec(_MedHdrDr("ADJUSTMENT_SW"))), Math.Abs(CDec(_MedHdrDr("OTH_INS_SW"))), Math.Abs(CDec(_MedHdrDr("OTH_INS_REFUSAL_SW"))),
                                                    UFCWGeneral.IsNullIntegerHandler(_MedHdrDr("OTH_INS_ID"), "OTH_INS_ID"),
                                                    TryCast(_MedHdrDr("OTH_INS_POLICY_NBR"), String),
                                                    TryCast(_ClaimDr("PAT_FNAME"), String), TryCast(_ClaimDr("PAT_LNAME"), String), TryCast(_MedHdrDr("PAT_SEX"), String),
                                                    UFCWGeneral.IsNullDateHandler(_MedHdrDr("PAT_DOB"), "PAT_DOB"),
                                                    TryCast(_MedHdrDr("PAT_ACCT_NBR"), String),
                                                    UFCWGeneral.IsNullIntegerHandler(_MedHdrDr("PAT_ZIP"), "PAT_ZIP"),
                                                    UFCWGeneral.IsNullShortHandler(_MedHdrDr("PAT_ZIP2"), "PAT_ZIP2"),
                                                    UFCWGeneral.IsNullIntegerHandler(_MedHdrDr("PROV_TIN"), "PROV_TIN"),
                                                    UFCWGeneral.IsNullIntegerHandler(_MedHdrDr("PROV_ID"), "PROV_ID"),
                                                    UFCWGeneral.IsNullIntegerHandler(_MedHdrDr("PROV_ZIP"), "PROV_ZIP"),
                                                    UFCWGeneral.IsNullShortHandler(_MedHdrDr("PROV_ZIP2"), "PROV_ZIP2"),
                                                    UFCWGeneral.IsNullStringHandler(_MedHdrDr("PROV_LICENSE"), "PROV_LICENSE"),
                                                    UFCWGeneral.IsNullLongHandler(_MedHdrDr("RENDERING_NPI"), "RENDERING_NPI"),
                                                    UFCWGeneral.IsNullIntegerHandler(_MedHdrDr("BILL_TAXID"), "BILL_TAXID"),
                                                    UFCWGeneral.IsNullStringHandler(_MedHdrDr("BILL_NAME")), TryCast(_MedHdrDr("BILL_ADDR1"), String),
                                                    UFCWGeneral.IsNullStringHandler(_MedHdrDr("BILL_ADDR2")), TryCast(_MedHdrDr("BILL_CITY"), String),
                                                    UFCWGeneral.IsNullStringHandler(_MedHdrDr("BILL_STATE")),
                                                    UFCWGeneral.IsNullIntegerHandler(_MedHdrDr("BILL_ZIP"), "BILL_ZIP"),
                                                    UFCWGeneral.IsNullShortHandler(_MedHdrDr("BILL_ZIP2"), "BILL_ZIP2"),
                                                    UFCWGeneral.IsNullDecimalHandler(TotChrg, "TotChrg"),
                                                    UFCWGeneral.IsNullDecimalHandler(TotPriced, "TotPriced"),
                                                    UFCWGeneral.IsNullDecimalHandler(TotAllowed, "TotAllowed"),
                                                    UFCWGeneral.IsNullDecimalHandler(TotOtherIns, "TotOtherIns"),
                                                    UFCWGeneral.IsNullDecimalHandler(TotPaid, "TotPaid"),
                                                    UFCWGeneral.IsNullDecimalHandler(TotProcessed, "TotProcessed"),
                                                    UFCWGeneral.IsNullIntegerHandler(_MedHdrDr("HOLD_DAYS"), "HOLD_DAYS"),
                                                    UFCWGeneral.IsNullDateHandler(_MedHdrDr("HOLD_DATE"), "HOLD_DATE"),
                                                    TryCast(_MedHdrDr("HOLD_TIME"), String), _DomainUser.ToUpper, transaction)

                    End If
                End If

                If PatientKeyChange OrElse (_ClaimDS.MEDDTL.GetChanges IsNot Nothing AndAlso _ClaimDS.MEDDTL.GetChanges.Rows.Count > 0) Then

                    SaveMEDDTLChanges(PatientKeyChange, HistEntryDT, transaction)

                End If

                If PartSSNChange Then
                    If CBool(_ClaimDr("SECURITY_SW")) Then
                        DocSec = CType(ConfigurationManager.GetSection("FNDocSecurity"), IDictionary)("EMP").ToString
                    Else
                        DocSec = CType(ConfigurationManager.GetSection("FNDocSecurity"), IDictionary)("REG").ToString
                    End If

                    If _ClaimDr("DOCID") IsNot DBNull.Value Then
                        Dim FNDocument As New Document(CLng(_ClaimDr("DOCID")))
                        If FNDocument.UpdateSSN(CInt(_ClaimDr("DOCID")), CInt(_ClaimDr("PART_SSN")), DocSec, DocSec, DocSec) = False Then
                            Return False
                        End If
                    End If
                End If
            End Using

            If _ClaimDS.REASON.GetChanges IsNot Nothing AndAlso _ClaimDS.REASON.GetChanges.Rows.Count > 0 Then
                SaveREASONChanges(HistEntryDT, transaction)
            End If

            If _ClaimDS.MEDDIAG.GetChanges IsNot Nothing AndAlso _ClaimDS.MEDDIAG.GetChanges.Rows.Count > 0 Then
                SaveMEDDIAGChanges(HistEntryDT, transaction)
            End If

            If _ClaimDS.MEDMOD.GetChanges IsNot Nothing AndAlso _ClaimDS.MEDMOD.GetChanges.Rows.Count > 0 Then
                SaveMEDMODChanges(HistEntryDT, transaction)
            End If

            If _ClaimDS.ANNOTATIONS.GetChanges IsNot Nothing AndAlso _ClaimDS.ANNOTATIONS.GetChanges.Rows.Count > 0 Then
                For Each DR As DataRow In _ClaimDS.ANNOTATIONS.GetChanges(DataRowState.Added).Rows
                    CMSDALFDBMD.CreateAnnotation(_ClaimID, CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")),
                                              UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PART_SSN")), UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PAT_SSN")),
                                              UFCWGeneral.IsNullStringHandler(_ClaimDr("PART_FNAME")), UFCWGeneral.IsNullStringHandler(_ClaimDr("PART_LNAME")),
                                              UFCWGeneral.IsNullStringHandler(_ClaimDr("PAT_FNAME")), UFCWGeneral.IsNullStringHandler(_ClaimDr("PAT_LNAME")),
                                              UFCWGeneral.IsNullStringHandler(DR("ANNOTATION")), DR("FLAG"), _DomainUser.ToUpper, transaction)
                Next
            End If

            If DocTypeChange Then
                CMSDALFDBMD.CreateDocHistory(_ClaimID,
                                                                            UFCWGeneral.IsNullDecimalHandler(_ClaimDr("DOCID"), "DOCID"),
                                                                            "UPDATE",
                                                                            UFCWGeneral.IsNullIntegerHandler(_ClaimDr("FAMILY_ID")),
                                                                            UFCWGeneral.IsNullShortHandler(_ClaimDr("RELATION_ID")),
                                                                            UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PART_SSN")),
                                                                            UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PAT_SSN")),
                                                                            TryCast(_ClaimDr("DOC_CLASS"), String),
                                                                            TryCast(_ClaimDr("DOC_TYPE"), String),
                                                                            "Claim " & _ClaimID & " Reassigned to type " & UFCWGeneral.IsNullStringHandler(_ClaimDr("DOC_TYPE"), ""), "Re-routed by system after change", _DomainUser.ToUpper, transaction)

                If Not CheckForUserDocTypeSecurity(_DomainUser.ToUpper, CStr(_ClaimDr("DOC_CLASS")), UFCWGeneral.IsNullStringHandler(_ClaimDr("DOC_TYPE"), "")) Then
                    Unpended = True
                    CMSDALFDBMD.UnPendUser(_ClaimID, _DomainUser.ToUpper, transaction)
                    CMSDALFDBMD.UpdateClaimMasterStatus(_ClaimID, "OPEN", _DomainUser.ToUpper, transaction)
                End If
            End If

            If SecurityChange Then
                If CBool(_ClaimDr("SECURITY_SW")) AndAlso Not UFCWGeneralAD.CMSCanAdjudicateEmployee AndAlso Not Unpended Then
                    Unpended = True
                    CMSDALFDBMD.UnPendUser(_ClaimID, _DomainUser.ToUpper, transaction)
                    CMSDALFDBMD.UpdateClaimMasterStatus(_ClaimID, "OPEN", _DomainUser.ToUpper, transaction)
                End If
            End If

            CreateHistory(HistEntryDT, HistDetail, transaction)

            AcceptChanges()

            Return True

        Catch ex As Exception
            Throw
        Finally

            If HistEntryDT IsNot Nothing Then HistEntryDT.Dispose()

        End Try
    End Function
    Private Sub SaveMEDMODChanges(ByRef histEntryDT As DataTable, ByRef transaction As DbTransaction)

        Dim OrigDV As DataView
        Dim CurDV As DataView
        Dim NewVal As String
        Dim OrigVal As String
        Dim HistDR As DataRow
        Dim SubDV As DataView
        Dim OverDV As DataView

        Try

            Using WC As New GlobalCursor

                CMSDALFDBMD.DeleteModifier(_ClaimID, transaction)
                SubDV = New DataView(_ClaimDS.MEDDTL, "", "LINE_NBR", DataViewRowState.CurrentRows)
                For Each MEDDTLDVR As DataRowView In SubDV

                    OverDV = New DataView(_ClaimDS.MEDMOD, "LINE_NBR = " & MEDDTLDVR("LINE_NBR").ToString, "LINE_NBR, PRIORITY", DataViewRowState.Added Or DataViewRowState.CurrentRows Or DataViewRowState.Deleted)
                    For Each MEDMODDVR As DataRowView In OverDV

                        OrigDV = New DataView(_ClaimDS.MEDMOD, "LINE_NBR = " & MEDDTLDVR("LINE_NBR").ToString & " AND PRIORITY = " & MEDMODDVR("PRIORITY").ToString, "LINE_NBR, PRIORITY", DataViewRowState.OriginalRows)
                        CurDV = New DataView(_ClaimDS.MEDMOD, "LINE_NBR = " & MEDDTLDVR("LINE_NBR").ToString & " AND PRIORITY = " & MEDMODDVR("PRIORITY").ToString, "LINE_NBR, PRIORITY", DataViewRowState.CurrentRows)

                        If OrigDV.Count > CurDV.Count Then
                            HistDR = histEntryDT.NewRow

                            HistDR("RowNum") = OrigDV(0)("LINE_NBR")
                            HistDR("EntryPosition") = histEntryDT.Rows.Count + 1
                            HistDR("Detail") = "LINE " & OrigDV(0)("LINE_NBR").ToString & " REMOVED PRIORITY " & MEDMODDVR("PRIORITY").ToString & " MODIFIER " & OrigDV(0)("MODIFIER").ToString

                            histEntryDT.Rows.Add(HistDR)

                        ElseIf CurDV.Count > OrigDV.Count Then
                            HistDR = histEntryDT.NewRow

                            HistDR("RowNum") = CurDV(0)("LINE_NBR")
                            HistDR("EntryPosition") = histEntryDT.Rows.Count + 1
                            HistDR("Detail") = "LINE " & CurDV(0)("LINE_NBR").ToString & " ADDED PRIORITY " & MEDMODDVR("PRIORITY").ToString & " MODIFIER " & CurDV(0)("MODIFIER").ToString

                            histEntryDT.Rows.Add(HistDR)

                        ElseIf CurDV.Count = 0 AndAlso OrigDV.Count > 0 Then
                            HistDR = histEntryDT.NewRow

                            HistDR("RowNum") = OrigDV(0)("LINE_NBR")
                            HistDR("EntryPosition") = histEntryDT.Rows.Count + 1
                            HistDR("Detail") = "LINE " & OrigDV(0)("LINE_NBR").ToString & " REMOVED PRIORITY " & MEDMODDVR("PRIORITY").ToString & " MODIFIER " & OrigDV(0)("MODIFIER").ToString

                            histEntryDT.Rows.Add(HistDR)

                        ElseIf CurDV.Count = 0 AndAlso OrigDV.Count = 0 Then
                            'do nothing

                        Else
                            If IsDBNull(OrigDV(0)("MODIFIER")) Then
                                OrigVal = "NULL"
                            Else
                                OrigVal = CStr(OrigDV(0)("MODIFIER")).ToUpper
                            End If
                            If IsDBNull(CurDV(0)("MODIFIER")) Then
                                NewVal = "NULL"
                            Else
                                NewVal = CStr(CurDV(0)("MODIFIER")).ToUpper
                            End If

                            If NewVal <> OrigVal Then
                                HistDR = histEntryDT.NewRow

                                HistDR("RowNum") = OrigDV(0)("LINE_NBR")
                                HistDR("EntryPosition") = histEntryDT.Rows.Count + 1
                                HistDR("Detail") = "LINE " & OrigDV(0)("LINE_NBR").ToString & " PRIORITY " & MEDMODDVR("PRIORITY").ToString & " MODIFIER = " & NewVal & " (WAS '" & OrigVal & "')"

                                histEntryDT.Rows.Add(HistDR)

                            End If
                        End If

                    Next
                Next

                For Each DR As DataRow In _ClaimDS.MEDMOD.Rows
                    If DR.RowState <> DataRowState.Deleted Then
                        CMSDALFDBMD.CreateModifier(_ClaimID, CShort(CInt(DR("LINE_NBR").ToString)), CStr(DR("MODIFIER")), CShort(DR("PRIORITY")), _DomainUser.ToUpper, transaction)
                    End If
                Next

            End Using

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SaveMEDDIAGChanges(ByRef histEntryDT As DataTable, ByRef transaction As DbTransaction)

        Dim OrigDV As DataView
        Dim CurDV As DataView
        Dim NewVal As String
        Dim OrigVal As String
        Dim SubDV As DataView
        Dim OverDV As DataView
        Dim HistDR As DataRow

        Try

            Using WC As New GlobalCursor

                CMSDALFDBMD.DeleteDiagnosis(_ClaimID, transaction)

                SubDV = New DataView(_ClaimDS.MEDDTL, "", "LINE_NBR", DataViewRowState.CurrentRows)
                For Each MEDDTLDVR As DataRowView In SubDV

                    OverDV = New DataView(_ClaimDS.MEDDIAG, "LINE_NBR = " & MEDDTLDVR("LINE_NBR").ToString, "LINE_NBR, PRIORITY", DataViewRowState.Added Or DataViewRowState.CurrentRows Or DataViewRowState.Deleted)
                    For Each MEDDIAGDVR As DataRowView In OverDV

                        OrigDV = New DataView(_ClaimDS.MEDDIAG, "LINE_NBR = " & MEDDTLDVR("LINE_NBR").ToString & " AND PRIORITY = " & MEDDIAGDVR("PRIORITY").ToString, "LINE_NBR, PRIORITY", DataViewRowState.OriginalRows)
                        CurDV = New DataView(_ClaimDS.MEDDIAG, "LINE_NBR = " & MEDDTLDVR("LINE_NBR").ToString & " AND PRIORITY = " & MEDDIAGDVR("PRIORITY").ToString, "LINE_NBR, PRIORITY", DataViewRowState.CurrentRows)

                        If OrigDV.Count > CurDV.Count Then
                            HistDR = histEntryDT.NewRow

                            HistDR("RowNum") = OrigDV(0)("LINE_NBR")
                            HistDR("EntryPosition") = histEntryDT.Rows.Count + 1
                            HistDR("Detail") = "LINE " & OrigDV(0)("LINE_NBR").ToString & " REMOVED PRIORITY " & MEDDIAGDVR("PRIORITY").ToString & " DIAGNOSIS " & OrigDV(0)("DIAGNOSIS").ToString

                            histEntryDT.Rows.Add(HistDR)

                        ElseIf CurDV.Count > OrigDV.Count Then
                            HistDR = histEntryDT.NewRow

                            HistDR("RowNum") = CurDV(0)("LINE_NBR")
                            HistDR("EntryPosition") = histEntryDT.Rows.Count + 1
                            HistDR("Detail") = "LINE " & CurDV(0)("LINE_NBR").ToString & " ADDED PRIORITY " & MEDDIAGDVR("PRIORITY").ToString & " DIAGNOSIS " & CurDV(0)("DIAGNOSIS").ToString

                            histEntryDT.Rows.Add(HistDR)

                        ElseIf CurDV.Count = 0 AndAlso OrigDV.Count > 0 Then
                            HistDR = histEntryDT.NewRow

                            HistDR("RowNum") = OrigDV(0)("LINE_NBR")
                            HistDR("EntryPosition") = histEntryDT.Rows.Count + 1
                            HistDR("Detail") = "LINE " & OrigDV(0)("LINE_NBR").ToString & " REMOVED PRIORITY " & MEDDIAGDVR("PRIORITY").ToString & " DIAGNOSIS " & OrigDV(0)("DIAGNOSIS").ToString

                            histEntryDT.Rows.Add(HistDR)

                        ElseIf CurDV.Count = 0 AndAlso OrigDV.Count = 0 Then
                            'do nothing

                        Else
                            If IsDBNull(OrigDV(0)("DIAGNOSIS")) Then
                                OrigVal = "NULL"
                            Else
                                OrigVal = CStr(OrigDV(0)("DIAGNOSIS")).ToUpper
                            End If
                            If IsDBNull(CurDV(0)("DIAGNOSIS")) Then
                                NewVal = "NULL"
                            Else
                                NewVal = CStr(CurDV(0)("DIAGNOSIS")).ToUpper
                            End If

                            If NewVal <> OrigVal Then
                                HistDR = histEntryDT.NewRow

                                HistDR("RowNum") = OrigDV(0)("LINE_NBR")
                                HistDR("EntryPosition") = histEntryDT.Rows.Count + 1
                                HistDR("Detail") = "LINE " & OrigDV(0)("LINE_NBR").ToString & " PRIORITY " & MEDDIAGDVR("PRIORITY").ToString & " DIAGNOSIS = " & NewVal & " (WAS '" & OrigVal & "')"

                                histEntryDT.Rows.Add(HistDR)

                            End If
                        End If

                    Next

                Next

                For Each DR As DataRow In _ClaimDS.MEDDIAG.Rows
                    If DR.RowState <> DataRowState.Deleted Then
                        CMSDALFDBMD.CreateDiagnosis(_ClaimID, CShort(CInt(DR("LINE_NBR").ToString)), CStr(DR("DIAGNOSIS")), CShort(DR("PRIORITY")), _DomainUser.ToUpper, transaction)
                    End If
                Next

            End Using

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub SaveREASONChanges(ByRef histEntryDT As DataTable, ByRef transaction As DbTransaction)

        Dim OrigDV As DataView
        Dim CurDV As DataView
        Dim NewVal As String
        Dim OrigVal As String
        Dim SubDV As DataView
        Dim OverDV As DataView
        Dim HistDR As DataRow

        Try

            CMSDALFDBMD.DeleteReason(_ClaimID, transaction)

            SubDV = New DataView(_ClaimDS.MEDDTL, "", "LINE_NBR", DataViewRowState.CurrentRows)
            For Each MEDDTLDVR As DataRowView In SubDV

                OverDV = New DataView(_ClaimDS.REASON, "LINE_NBR = " & MEDDTLDVR("LINE_NBR").ToString, "LINE_NBR, PRIORITY", DataViewRowState.Added Or DataViewRowState.CurrentRows Or DataViewRowState.Deleted)
                For Each REASONDVR As DataRowView In OverDV

                    OrigDV = New DataView(_ClaimDS.REASON, "LINE_NBR = " & MEDDTLDVR("LINE_NBR").ToString & " AND PRIORITY = " & REASONDVR("PRIORITY").ToString, "", DataViewRowState.OriginalRows)
                    CurDV = New DataView(_ClaimDS.REASON, "LINE_NBR = " & MEDDTLDVR("LINE_NBR").ToString & " AND PRIORITY = " & REASONDVR("PRIORITY").ToString, "", DataViewRowState.CurrentRows)

                    If OrigDV.Count > CurDV.Count Then
                        HistDR = histEntryDT.NewRow

                        HistDR("RowNum") = OrigDV(0)("LINE_NBR")
                        HistDR("EntryPosition") = histEntryDT.Rows.Count + 1
                        HistDR("Detail") = "LINE " & OrigDV(0)("LINE_NBR").ToString & " REMOVED PRIORITY " & REASONDVR("PRIORITY").ToString & " REASON " & OrigDV(0)("REASON").ToString

                        histEntryDT.Rows.Add(HistDR)

                    ElseIf CurDV.Count > OrigDV.Count Then

                        HistDR = histEntryDT.NewRow

                        HistDR("RowNum") = CurDV(0)("LINE_NBR")
                        HistDR("EntryPosition") = histEntryDT.Rows.Count + 1
                        HistDR("Detail") = "LINE " & CurDV(0)("LINE_NBR").ToString & " ADDED PRIORITY " & REASONDVR("PRIORITY").ToString & " REASON " & CurDV(0)("REASON").ToString

                        histEntryDT.Rows.Add(HistDR)

                    ElseIf CurDV.Count = 0 AndAlso OrigDV.Count > 0 Then

                        HistDR = histEntryDT.NewRow

                        HistDR("RowNum") = OrigDV(0)("LINE_NBR")
                        HistDR("EntryPosition") = histEntryDT.Rows.Count + 1
                        HistDR("Detail") = "LINE " & OrigDV(0)("LINE_NBR").ToString & " REMOVED PRIORITY " & REASONDVR("PRIORITY").ToString & " REASON " & OrigDV(0)("REASON").ToString

                        histEntryDT.Rows.Add(HistDR)

                    ElseIf CurDV.Count = 0 AndAlso OrigDV.Count = 0 Then
                        'do nothing

                    Else
                        If IsDBNull(OrigDV(0)("REASON")) Then
                            OrigVal = "NULL"
                        Else
                            OrigVal = CStr(OrigDV(0)("REASON")).ToUpper
                        End If
                        If IsDBNull(CurDV(0)("REASON")) Then
                            NewVal = "NULL"
                        Else
                            NewVal = CStr(CurDV(0)("REASON")).ToUpper
                        End If

                        If NewVal <> OrigVal Then
                            HistDR = histEntryDT.NewRow

                            HistDR("RowNum") = OrigDV(0)("LINE_NBR")
                            HistDR("EntryPosition") = histEntryDT.Rows.Count + 1
                            HistDR("Detail") = "LINE " & OrigDV(0)("LINE_NBR").ToString & " PRIORITY " & REASONDVR("PRIORITY").ToString & " REASON = " & NewVal & " (WAS '" & OrigVal & "')"

                            histEntryDT.Rows.Add(HistDR)

                        End If
                    End If

                Next
            Next

            For Each DR As DataRow In _ClaimDS.REASON.Rows
                If DR.RowState <> DataRowState.Deleted Then
                    CMSDALFDBMD.CreateReason(_ClaimID, CShort(DR("LINE_NBR")), CStr(DR("REASON")), CShort(DR("PRIORITY")), CStr(DR("DESCRIPTION")), Math.Abs(CDec(DR("PRINT_SW"))), _DomainUser.ToUpper, transaction)
                End If
            Next

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub SaveMEDDTLChanges(ByRef patientKeyChange As Boolean, ByRef histEntryDT As DataTable, ByRef transaction As DbTransaction)

        Dim DT As DataTable
        Dim DispName As String = ""
        Dim Cnt As Integer = 0
        Dim HistDR As DataRow
        Dim RowCnt As Integer = 0
        Dim NewVal As String = ""

        Try

            If patientKeyChange OrElse _ClaimDS.MEDDTL.Rows(0)("STATUS").ToString.Trim = "MERGED" Then
                DT = _ClaimDS.MEDDTL 'apply changes to every row 
            Else
                DT = _ClaimDS.MEDDTL.GetChanges 'apply changes to only changed rows 
            End If

            For Each DR As DataRow In DT.Rows

                If DR.RowState <> DataRowState.Added AndAlso DR.RowState <> DataRowState.Deleted Then 'modified rows
                    Dim LineNbr As Short = CShort(DR("LINE_NBR"))

                    If Not IsDBNull(DR("MED_PLAN")) Then
                        If CStr(DR("MED_PLAN")).Trim.Length = 0 OrElse CStr(DR("MED_PLAN")).Trim = "-" Then
                            DR("MED_PLAN") = System.DBNull.Value
                        Else
                            DR("MED_PLAN") = DR("MED_PLAN").ToString.Trim
                        End If
                    End If

                    Dim QueryDIAGNOSIS =
                        From MEDDIAG In _ClaimDS.Tables("MEDDIAG").AsEnumerable()
                        Where MEDDIAG.RowState <> DataRowState.Deleted _
                        AndAlso MEDDIAG.Field(Of Short)("LINE_NBR") = LineNbr
                        Group By LINE_NBR = MEDDIAG.Field(Of Short)("LINE_NBR")
                        Into Group
                        Select Group

                    If QueryDIAGNOSIS.Any Then
                        DR("DIAG_SW") = 1
                    Else
                        DR("DIAG_SW") = 0
                    End If

                    Dim QueryREASON =
                        From REASON In _ClaimDS.Tables("REASON").AsEnumerable()
                        Where REASON.RowState <> DataRowState.Deleted _
                        AndAlso REASON.Field(Of Short)("LINE_NBR") = LineNbr
                        Group By LINE_NBR = REASON.Field(Of Short)("LINE_NBR")
                        Into Group
                        Select Group

                    If QueryREASON.Any Then
                        DR("REASON_SW") = 1
                    Else
                        DR("REASON_SW") = 0
                    End If

                    Dim QueryMEDMOD =
                        From MEDMOD In _ClaimDS.Tables("MEDMOD").AsEnumerable()
                        Where MEDMOD.RowState <> DataRowState.Deleted _
                        AndAlso MEDMOD.Field(Of Short)("LINE_NBR") = LineNbr
                        Group By LINE_NBR = MEDMOD.Field(Of Short)("LINE_NBR")
                        Into Group
                        Select Group

                    If QueryMEDMOD.Any Then
                        DR("MODIFIER_SW") = 1
                    Else
                        DR("MODIFIER_SW") = 0
                    End If

                    histEntryDT = IdentifyChanges(_ClaimDS.MEDDTL, histEntryDT, DR.RowState, "LINE_NBR = " & LineNbr.ToString, "LINE_NBR")

                    CMSDALFDBMD.UpdateMEDDTL(_ClaimID, CShort(DR("LINE_NBR")), Math.Abs(CDec(_ClaimDr("SECURITY_SW"))),
                                                CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")),
                                                UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PART_SSN"), "PART_SSN"),
                                                UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PAT_SSN"), "PAT_SSN"),
                                                CShort(DR("MAXID_LINE_NBR")), UFCWGeneral.IsNullStringHandler(DR("SYSTEM_CODE")),
                                                UFCWGeneral.IsNullStringHandler(DR("STATUS")),
                                                UFCWGeneral.IsNullDateHandler(DR("STATUS_DATE"), "STATUS_DATE"),
                                                UFCWGeneral.IsNullStringHandler(DR("PRICING_ERROR")), UFCWGeneral.IsNullStringHandler(DR("PRICING_SD_ERROR")),
                                                UFCWGeneral.IsNullStringHandler(DR("PRICING_REASON")),
                                                UFCWGeneral.IsNullDateHandler(DR("OCC_FROM_DATE"), "OCC_FROM_DATE"),
                                                UFCWGeneral.IsNullDateHandler(DR("OCC_TO_DATE"), "OCC_TO_DATE"),
                                                UFCWGeneral.IsNullDateHandler(_MedHdrDr("INCIDENT_DATE"), "INCIDENT_DATE"),
                                                UFCWGeneral.IsNullStringHandler(DR("PLACE_OF_SERV")),
                                                UFCWGeneral.IsNullStringHandler(DR("BILL_TYPE")), TryCast(DR("PROC_CODE"), String),
                                                Math.Abs(CDec(DR("MODIFIER_SW"))), Math.Abs(CDec(DR("REASON_SW"))), Math.Abs(CDec(DR("DIAG_SW"))), TryCast(DR("LOCAL_USE"), String),
                                                UFCWGeneral.IsNullDecimalHandler(DR("DAYS_UNITS"), "DAYS_UNITS"),
                                                UFCWGeneral.IsNullStringHandler(DR("MED_PLAN")),
                                                UFCWGeneral.IsNullStringHandler(DR("MEMTYPE")),
                                                UFCWGeneral.IsNullStringHandler(DR("ELIG_STATUS")),
                                                Math.Abs(CDec(_ClaimDr("DUPLICATE_SW"))), Math.Abs(CDec(_MedHdrDr("NON_PAR_SW"))), Math.Abs(CDec(_MedHdrDr("OUT_OF_AREA_SW"))),
                                                Math.Abs(CDec(_MedHdrDr("AUTO_ACCIDENT_SW"))), Math.Abs(CDec(_MedHdrDr("WORKERS_COMP_SW"))), Math.Abs(CDec(_MedHdrDr("OTH_ACCIDENT_SW"))),
                                                Math.Abs(CDec(_MedHdrDr("OTH_INS_SW"))),
                                                UFCWGeneral.IsNullDecimalHandler(DR("CHRG_AMT"), "CHRG_AMT"),
                                                UFCWGeneral.IsNullDecimalHandler(DR("PRICED_AMT"), "PRICED_AMT"),
                                                UFCWGeneral.IsNullDecimalHandler(DR("ALLOWED_AMT"), "ALLOWED_AMT"),
                                                UFCWGeneral.IsNullDecimalHandler(DR("OTH_INS_AMT"), "OTH_INS_AMT"),
                                                UFCWGeneral.IsNullDecimalHandler(DR("PAID_AMT"), "PAID_AMT"),
                                                UFCWGeneral.IsNullDecimalHandler(DR("PROCESSED_AMT"), "PROCESSED_AMT"),
                                                Math.Abs(CDec(DR("OVERRIDE_SW"))),
                                                UFCWGeneral.IsNullIntegerHandler(DR("PROC_ID"), "PROC_ID"),
                                                UFCWGeneral.IsNullIntegerHandler(DR("RULE_SET_ID"), "RULE_SET_ID"),
                                                Math.Abs(CDec(DR("CHECK_SW"))),
                                                UFCWGeneral.IsNullDateHandler(DR("CLOSED_DATE"), "CLOSED_DATE"),
                                                UFCWGeneral.IsNullStringHandler(DR("ADJUSTER")), _DomainUser.ToUpper,
                                                Math.Abs(CDec(DR("HRA_EXCLUDE"))),
                                                TryCast(DR("NDC"), String),
                                                TryCast(DR("RX_UNITS"), String),
                                                UFCWGeneral.IsNullDecimalHandler(DR("RX_QTY"), "RX_QTY"),
                                                TryCast(DR("RX_PRESCRIPTION_NUM"), String),
                                                transaction)

                ElseIf DR.RowState = DataRowState.Deleted Then 'DELETE - Note: This functionality only applies to the consoldation process which can only delete a single row

                    HistDR = histEntryDT.NewRow

                    HistDR("RowNum") = DR("LINE_NBR", DataRowVersion.Original)
                    HistDR("EntryPosition") = histEntryDT.Rows.Count + 1
                    HistDR("Detail") = "DELETED LINE " & DR("LINE_NBR", DataRowVersion.Original).ToString

                    histEntryDT.Rows.Add(HistDR)

                    histEntryDT = IdentifyChanges(_ClaimDS.MEDDTL, histEntryDT, DR.RowState, "LINE_NBR = " & DR("LINE_NBR", DataRowVersion.Original).ToString, "LINE_NBR")

                    CMSDALFDBMD.DeleteMEDDTLAndCascade(_ClaimID, CShort(DR("Line_NBR", DataRowVersion.Original)), transaction)

                ElseIf DR.RowState = DataRowState.Added Then 'ADD

                    If Not IsDBNull(DR("MED_PLAN")) Then
                        If CStr(DR("MED_PLAN")).Trim.Length = 0 OrElse CStr(DR("MED_PLAN")).Trim = "-" Then
                            DR("MED_PLAN") = System.DBNull.Value
                        Else
                            DR("MED_PLAN") = DR("MED_PLAN").ToString.Trim
                        End If
                    End If

                    HistDR = histEntryDT.NewRow

                    HistDR("RowNum") = DR("LINE_NBR")
                    HistDR("EntryPosition") = histEntryDT.Rows.Count + 1
                    HistDR("Detail") = "ADDED LINE " & DR("LINE_NBR").ToString

                    histEntryDT.Rows.Add(HistDR)

                    histEntryDT = IdentifyChanges(_ClaimDS.MEDDTL, histEntryDT, DR.RowState, "LINE_NBR = " & DR("LINE_NBR").ToString, "LINE_NBR")

                    CMSDALFDBMD.CreateMEDDTL(_ClaimID, CShort(DR("LINE_NBR")), Math.Abs(CDec(_ClaimDr("SECURITY_SW"))),
                                                CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")),
                                                UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PART_SSN"), "PART_SSN"),
                                                UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PAT_SSN"), "PAT_SSN"),
                                                UFCWGeneral.IsNullStringHandler(_ClaimDr("MAXID")),
                                                CShort(DR("MAXID_LINE_NBR")), UFCWGeneral.IsNullStringHandler(DR("SYSTEM_CODE")), UFCWGeneral.IsNullStringHandler(DR("STATUS")),
                                                UFCWGeneral.IsNullDateHandler(DR("STATUS_DATE"), "STATUS_DATE"),
                                                UFCWGeneral.IsNullStringHandler(DR("PRICING_ERROR")), UFCWGeneral.IsNullStringHandler(DR("PRICING_SD_ERROR")), UFCWGeneral.IsNullStringHandler(DR("PRICING_REASON")),
                                                UFCWGeneral.IsNullDateHandler(DR("OCC_FROM_DATE"), "OCC_FROM_DATE"),
                                                UFCWGeneral.IsNullDateHandler(DR("OCC_TO_DATE"), "OCC_TO_DATE"),
                                                UFCWGeneral.IsNullDateHandler(_MedHdrDr("INCIDENT_DATE"), "INCIDENT_DATE"),
                                                UFCWGeneral.IsNullStringHandler(DR("PLACE_OF_SERV")),
                                                UFCWGeneral.IsNullStringHandler(DR("BILL_TYPE")), UFCWGeneral.IsNullStringHandler(DR("PROC_CODE")),
                                                Math.Abs(CDec(DR("MODIFIER_SW"))), Math.Abs(CDec(DR("REASON_SW"))),
                                                Math.Abs(CDec(DR("DIAG_SW"))),
                                                UFCWGeneral.IsNullStringHandler(DR("LOCAL_USE")),
                                                UFCWGeneral.IsNullDecimalHandler(DR("DAYS_UNITS"), "DAYS_UNITS"),
                                                UFCWGeneral.IsNullStringHandler(DR("MED_PLAN")),
                                                UFCWGeneral.IsNullStringHandler(DR("MEMTYPE")), UFCWGeneral.IsNullStringHandler(DR("ELIG_STATUS")),
                                                Math.Abs(CDec(_ClaimDr("DUPLICATE_SW"))), Math.Abs(CDec(_MedHdrDr("NON_PAR_SW"))), Math.Abs(CDec(_MedHdrDr("OUT_OF_AREA_SW"))),
                                                Math.Abs(CDec(_MedHdrDr("AUTO_ACCIDENT_SW"))), Math.Abs(CDec(_MedHdrDr("WORKERS_COMP_SW"))), Math.Abs(CDec(_MedHdrDr("OTH_ACCIDENT_SW"))),
                                                Math.Abs(CDec(_MedHdrDr("OTH_INS_SW"))),
                                                UFCWGeneral.IsNullDecimalHandler(DR("CHRG_AMT"), "CHRG_AMT"),
                                                UFCWGeneral.IsNullDecimalHandler(DR("PRICED_AMT"), "PRICED_AMT"),
                                                UFCWGeneral.IsNullDecimalHandler(DR("ALLOWED_AMT"), "ALLOWED_AMT"),
                                                UFCWGeneral.IsNullDecimalHandler(DR("OTH_INS_AMT"), "OTH_INS_AMT"),
                                                UFCWGeneral.IsNullDecimalHandler(DR("PAID_AMT"), "PAID_AMT"),
                                                UFCWGeneral.IsNullDecimalHandler(DR("PROCESSED_AMT"), "PROCESSED_AMT"),
                                                Math.Abs(CDec(DR("OVERRIDE_SW"))),
                                                UFCWGeneral.IsNullIntegerHandler(DR("PROC_ID"), "PROC_ID"),
                                                UFCWGeneral.IsNullIntegerHandler(DR("RULE_SET_ID"), "RULE_SET_ID"),
                                                Math.Abs(CDec(DR("CHECK_SW"))),
                                                UFCWGeneral.IsNullDateHandler(DR("CLOSED_DATE"), "CLOSED_DATE"),
                                                TryCast(DR("ADJUSTER"), String),
                                                _DomainUser.ToUpper, Math.Abs(CDec(DR("HRA_EXCLUDE"))),
                                                TryCast(DR("NDC"), String),
                                                TryCast(DR("RX_UNITS"), String),
                                                UFCWGeneral.IsNullDecimalHandler(DR("RX_QTY"), "RX_QTY"),
                                                TryCast(DR("RX_PRESCRIPTION_NUM"), String),
                                                transaction)
                End If
            Next

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub CreateHistory(ByRef histEntryDT As DataTable, ByRef histDetail As String, ByRef transaction As DbTransaction)

        Dim HistSum As String = "CLAIM ID " & Format(_ClaimID, "00000000") & " INDEX UPDATE"
        Dim HistDV As DataView

        Try

            If histEntryDT.Rows.Count > 0 Then
                HistDV = New DataView(histEntryDT, "", "RowNum, EntryPosition", DataViewRowState.CurrentRows)

                For Cnt As Integer = 0 To HistDV.Count - 1
                    If histDetail.Length + Len(Microsoft.VisualBasic.vbCrLf & HistDV(Cnt)("Detail").ToString) > 4000 Then  '4000 is entlib limit
                        CMSDALFDBMD.CreateDocHistory(_ClaimID, UFCWGeneral.IsNullLongHandler(_ClaimDr("DOCID"), "DOCID"), "UPDATE", CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")), CInt(_ClaimDr("PART_SSN")), CInt(_ClaimDr("PAT_SSN")), CStr(_ClaimDr("DOC_CLASS")), CStr(_ClaimDr("DOC_TYPE")), HistSum, Microsoft.VisualBasic.Left(histDetail, 4000), _DomainUser.ToUpper, transaction)

                        HistSum = "CLAIM ID " & Format(_ClaimID, "00000000") & " INDEX UPDATE CONTINUED"
                        histDetail = "INDEXES UPDATED (CONTINUED):"
                    End If

                    histDetail &= Microsoft.VisualBasic.vbCrLf & HistDV(Cnt)("Detail").ToString

                    If Cnt = HistDV.Count - 1 OrElse histDetail.Length > 4000 Then '4000 is entlib limit
                        CMSDALFDBMD.CreateDocHistory(_ClaimID, UFCWGeneral.IsNullLongHandler(_ClaimDr("DOCID"), "DOCID"), "UPDATE", CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")), CInt(_ClaimDr("PART_SSN")), CInt(_ClaimDr("PAT_SSN")), CStr(_ClaimDr("DOC_CLASS")), CStr(_ClaimDr("DOC_TYPE")), HistSum, Microsoft.VisualBasic.Left(histDetail, 4000), _DomainUser.ToUpper, transaction)

                        HistSum = "CLAIM ID " & Format(_ClaimID, "00000000") & " INDEX UPDATE CONTINUED"
                        histDetail = "INDEXES UPDATED (CONTINUED):"
                    End If
                Next

            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function PreventativeDiagnosisPresent() As Boolean
        Try
            Return _ClaimDS.MEDDIAG.Rows.Cast(Of DataRow)().Any(Function(DR) DR.RowState <> DataRowState.Deleted AndAlso CBool(DR("PREVENTATIVE_USE_SW")))
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub AcceptChanges()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Accepts all changes made. to be used after a save
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	12/15/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            For Each DT As DataTable In _ClaimDS.Tables
                DT.AcceptChanges()
            Next

        Catch ex As Exception
            Throw
        Finally
            DisableEnableButtons()
        End Try
    End Sub

    Private Function GetRuleSetTypeName(ByVal columnName As String) As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets the friendly name of a column from the config file
        ' </summary>
        ' <param name="ColumnName"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	10/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try
            Return CStr(CType(ConfigurationManager.GetSection("RuleSetTypes"), IDictionary)(columnName))

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Sub ZeroPaidAmount()

        Try

            Dim QueryMEDDTL =
                From MEDDTL In _ClaimDS.Tables("MEDDTL").AsEnumerable()
                Where MEDDTL.RowState <> DataRowState.Deleted _
                AndAlso (MEDDTL.Field(Of String)("STATUS") = "PAY" AndAlso MEDDTL.Field(Of Decimal?)("PAID_AMT") Is Nothing) _
                OrElse (MEDDTL.Field(Of String)("STATUS") = "DENY" AndAlso (MEDDTL.Field(Of Decimal?)("PAID_AMT") IsNot Nothing AndAlso MEDDTL.Field(Of Decimal?)("PAID_AMT") <> 0D))
                Select MEDDTL

            For Each DVR As DataRowView In QueryMEDDTL.AsDataView
                DVR.Row("PAID_AMT") = 0D
            Next
            _MedDtlBS.EndEdit()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function ShouldClaimBeMovedToAuditQueue(ByVal claimMasterDT As DataTable, ByVal medhdrDT As DataTable, ByVal meddtlDT As DataTable) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Determines if a Claim should be audited
        ' </summary>
        ' <param name="claim_masterDataTable"></param>
        ' <param name="mdhdrDataTable"></param>
        ' <param name="mddtlDataTable"></param>
        ' <returns></returns>
        ' <remarks>  This is for implementing restricting Employee claims to move to Audit queue automatically  -- LM 02/21/2024
        ' </remarks>
        ' <history>
        '     [paulw]     11/16/2006  Created
        '   Lalitha Moparthi    03/01/2024 :: Added logic of removing Employee claims from Audit process
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim AuditRuleDT As DataTable
        Dim RuleDT As DataTable
        Dim DR As DataRow
        Dim DT As DataTable
        Dim CanMoveToAudit As Boolean = False

        Try
            AuditRuleDT = CMSDALFDBMD.RetrieveAllAuditRulesExtn
            Dim result As List(Of DataTable) = AuditRuleDT.AsEnumerable().GroupBy(Function(row) row.Field(Of Integer)("AUDIT_SET")).[Select](Function(g) g.CopyToDataTable()).ToList()

            For Each RuleDT In result
                For I As Integer = 0 To RuleDT.Rows.Count - 1
                    DR = RuleDT.Rows(I)
                    Select Case DR("AUDIT_TABLE_NAME").ToString
                        Case "CLAIM_MASTER"
                            DT = claimMasterDT
                        Case "MEDHDR"
                            DT = medhdrDT
                        Case "MEDDTL"
                            DT = meddtlDT
                    End Select

                    If DT.Rows.Count > 0 Then
                        For c As Integer = 0 To DT.Columns.Count - 1
                            If DT.Columns(c).ColumnName = DR("AUDIT_COLUMN_NAME").ToString Then
                                If DT.Rows(0)(c) IsNot System.DBNull.Value Then
                                    For r As Integer = 0 To DT.Rows.Count - 1
                                        If CStr(DR("AUDIT_SET")) = "0" Then
                                            If Not DoesColumnExceedThreshold(CStr(If(IsDBNull(DT.Rows(r)(c)), 0, CDec(DT.Rows(r)(c)))), CStr(DR("AUDIT_OPERATOR")), CStr(DR("AUDIT_OPERAND")), CStr(DR("AUDIT_OPERAND_DATA_TYPE"))) Then
                                                Return False
                                            End If
                                        Else
                                            If DoesColumnExceedThreshold(CStr(DT.Rows(r)(c)), CStr(DR("AUDIT_OPERATOR")), CStr(DR("AUDIT_OPERAND")), CStr(DR("AUDIT_OPERAND_DATA_TYPE"))) Then
                                                Return True
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If
                Next
            Next

            Return True

        Catch ex As Exception
            Throw
        End Try

    End Function
    'Private Function ShouldClaimBeMovedToAuditQueue(ByVal claimMasterDT As DataTable, ByVal medhdrDT As DataTable, ByVal meddtlDT As DataTable) As Boolean
    '    ' -----------------------------------------------------------------------------
    '    ' <summary>
    '    ' Determines if a Claim should be audited
    '    ' </summary>
    '    ' <param name="claim_masterDataTable"></param>
    '    ' <param name="mdhdrDataTable"></param>
    '    ' <param name="mddtlDataTable"></param>
    '    ' <returns></returns>
    '    ' <remarks>
    '    ' </remarks>
    '    ' <history>
    '    '     [paulw]     11/16/2006  Created
    '    ' </history>
    '    ' -----------------------------------------------------------------------------

    '    Dim RuleDT As DataTable
    '    Dim DR As DataRow
    '    Dim DT As DataTable

    '    Try

    '        RuleDT = CMSDALFDBMD.RetrieveAllAuditRules

    '        For I As Integer = 0 To RuleDT.Rows.Count - 1
    '            DR = RuleDT.Rows(I)
    '            Select Case DR("AUDIT_TABLE_NAME").ToString
    '                Case "CLAIM_MASTER"
    '                    DT = claimMasterDT
    '                Case "MEDHDR"
    '                    DT = medhdrDT
    '                Case "MEDDTL"
    '                    DT = meddtlDT
    '            End Select

    '            If DT.Rows.Count > 0 Then
    '                For c As Integer = 0 To DT.Columns.Count - 1
    '                    If DT.Columns(c).ColumnName = DR("AUDIT_COLUMN_NAME").ToString Then
    '                        If DT.Rows(0)(c) IsNot System.DBNull.Value Then
    '                            For r As Integer = 0 To DT.Rows.Count - 1
    '                                If DoesColumnExceedThreshold(CStr(DT.Rows(r)(c)), CStr(DR("AUDIT_OPERATOR")), CStr(DR("AUDIT_OPERAND")), CStr(DR("AUDIT_OPERAND_DATA_TYPE"))) Then
    '                                    Return True
    '                                End If
    '                            Next
    '                        End If
    '                    End If
    '                Next
    '            End If
    '        Next

    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Function
    Private Function DoesColumnExceedThreshold(ByVal columnValue As String, ByVal [operator] As String, ByVal operand As String, ByVal dataType As String) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Determines if a column exceeds the threshold, thus making the claim auditable
        ' </summary>
        ' <param name="columnValue"></param>
        ' <param name="operator"></param>
        ' <param name="operand"></param>
        ' <param name="dataType"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        '     [paulw]     11/16/2006  Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            Select Case dataType
                Case "System.DateTime"
                    Select Case [operator]
                        Case "="
                            If Convert.ToDateTime(columnValue) = Convert.ToDateTime(operand) Then Return True
                        Case ">"
                            If Convert.ToDateTime(columnValue) > Convert.ToDateTime(operand) Then Return True
                        Case "<"
                            If Convert.ToDateTime(columnValue) < Convert.ToDateTime(operand) Then Return True
                        Case ">="
                            If Convert.ToDateTime(columnValue) >= Convert.ToDateTime(operand) Then Return True
                        Case "<="
                            If Convert.ToDateTime(columnValue) <= Convert.ToDateTime(operand) Then Return True
                        Case "BETWEEN"
                            If Convert.ToDateTime(columnValue) >= Convert.ToDateTime(operand.Replace("AND", "$").Split(CChar("$"))(0)) AndAlso Convert.ToDateTime(columnValue) <= Convert.ToDateTime(operand.Replace("AND", "$").Split(CChar("$"))(1)) Then Return True
                    End Select
                Case "System.Double"
                    Select Case [operator]
                        Case "="
                            If Convert.ToDouble(columnValue) = Convert.ToDouble(operand) Then Return True
                        Case ">"
                            If Convert.ToDouble(columnValue) > Convert.ToDouble(operand) Then Return True
                        Case "<"
                            If Convert.ToDouble(columnValue) < Convert.ToDouble(operand) Then Return True
                        Case ">="
                            If Convert.ToDouble(columnValue) >= Convert.ToDouble(operand) Then Return True
                        Case "<="
                            If Convert.ToDouble(columnValue) <= Convert.ToDouble(operand) Then Return True
                        Case "<>"
                            If Convert.ToDouble(columnValue) <> Convert.ToDouble(operand) Then Return True
                        Case "BETWEEN"
                            If Convert.ToDouble(columnValue) >= Convert.ToDouble(operand.Replace("AND", "$").Split(CChar("$"))(0)) AndAlso Convert.ToDouble(columnValue) <= Convert.ToDouble(operand.Replace("AND", "$").Split(CChar("$"))(1)) Then Return True
                    End Select
                Case "System.Decimal"
                    Select Case [operator]
                        Case "="
                            If Convert.ToDecimal(columnValue) = Convert.ToDecimal(operand) Then Return True
                        Case ">"
                            If Convert.ToDecimal(columnValue) > Convert.ToDecimal(operand) Then Return True
                        Case "<"
                            If Convert.ToDecimal(columnValue) < Convert.ToDecimal(operand) Then Return True
                        Case ">="
                            If Convert.ToDecimal(columnValue) >= Convert.ToDecimal(operand) Then Return True
                        Case "<="
                            If Convert.ToDecimal(columnValue) <= Convert.ToDecimal(operand) Then Return True
                        Case "<>"
                            If Convert.ToDecimal(columnValue) <> Convert.ToDecimal(operand) Then Return True
                        Case "BETWEEN"
                            If Convert.ToDecimal(columnValue) >= Convert.ToDecimal(operand.Replace("AND", "$").Split(CChar("$"))(0)) AndAlso Convert.ToDecimal(columnValue) <= Convert.ToDecimal(operand.Replace("AND", "$").Split(CChar("$"))(1)) Then Return True
                    End Select
                Case "System.String"
                    If columnValue = operand Then Return True
            End Select

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub ManualAccumulatorValues_Refresh()
        Try
            If ManualAccumulatorValues IsNot Nothing Then
                ManualAccumulatorValues.RefreshManualAccumulators()
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub ManualAccumulatorButton_CloseRequested()

        Try

            _ManualAccumulatorsForm.Close()

            If CBool(_ManualAccumulatorsForm.ManualAccumulatorValues.RefreshPending) Then Call ManualAccumulatorValues_Refresh()

        Catch ex As Exception
            Throw
        Finally
            If _ManualAccumulatorsForm IsNot Nothing Then _ManualAccumulatorsForm = Nothing

        End Try

    End Sub

    Private Sub AssociatedProcessingCompleted(ByVal returnedDS As DataSet)

        Try

            _AssociatedClaimDS = returnedDS

            If (_AssociatedClaimDS.Tables("Results").Rows.Count >= 1) AndAlso _AssociatedClaimDS.Tables("Results").Rows(_AssociatedClaimDS.Tables("Results").Rows.Count - 1)("STATUS").ToString = "RESTRICTED" Then
                _AssociatedClaimDS.Tables("Results").Rows.RemoveAt(_AssociatedClaimDS.Tables("Results").Rows.Count - 1)
            End If

            _AssResultsBS = New BindingSource
            _AssResultsBS.DataSource = _AssociatedClaimDS.Tables("Results")

            AssociatedResultsDataGrid.SuspendLayout()

            AssociatedResultsDataGrid.DataSource = _AssResultsBS

            AssociatedResultsDataGrid.ResumeLayout()

            AssociatedResultsDataGrid.SetTableStyle()

            'SetTableStyle(AssociatedResultsDataGrid, False)

            AssociatedResultsDataGrid.CaptionText = "Associated Claim(s)"

#If TRACE Then
            If CInt(_TraceParallel.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If

        Catch ex As Exception
            Throw
        Finally

            _AssociatedThread = Nothing
            _ExAssociated = Nothing

            'trigger redraw of tab
            'Me.Tabs.DrawMode = TabDrawMode.Normal
            'Me.Tabs.DrawMode = TabDrawMode.OwnerDrawFixed

        End Try

    End Sub

    Private Sub DuplicateProcessingCompleted(ByVal returnedDS As DataSet)

        Dim Filter As String = ""
        Dim PNode As TreeNode
        Dim CNode As TreeNode
        Dim NodeText As String
        Dim MatchCnt As Integer = 0

        Try

            _DuplicatesClaimDS = returnedDS

            If _DuplicatesClaimDS.Tables.Count > 0 Then

                DupsTreeView.Nodes.Clear()

                _DuplicatesClaimDS.Tables("DETAIL").Columns.Add("REASONS")
                _DuplicatesClaimDS.Tables("DETAIL").Columns.Add("DIAGNOSES")

                For DTCnt As Integer = 0 To _DuplicatesClaimDS.Tables.Count - 1
                    If _DuplicatesClaimDS.Tables(DTCnt).Rows.Count > 0 Then

                        Select Case _DuplicatesClaimDS.Tables(DTCnt).TableName
                            Case "EXACT", "PARTIAL", "HEADER"
                                Filter = CStr(_DuplicatesClaimDS.Tables(DTCnt).Rows(0)("MATCH"))
                                PNode = New TreeNode(Filter)

                                For Cnt As Integer = 0 To _DuplicatesClaimDS.Tables(DTCnt).Rows.Count - 1
                                    If Not IsDBNull(_DuplicatesClaimDS.Tables(DTCnt).Rows(Cnt)("DOCID")) Then
                                        NodeText = _DuplicatesClaimDS.Tables(DTCnt).Rows(Cnt)("CLAIM_ID").ToString & "; (IDM Doc " & _DuplicatesClaimDS.Tables(DTCnt).Rows(Cnt)("DOCID").ToString & ")"
                                    Else
                                        NodeText = CStr(_DuplicatesClaimDS.Tables(DTCnt).Rows(Cnt)("CLAIM_ID"))
                                    End If

                                    CNode = New TreeNode(NodeText)
                                    PNode.Nodes.Add(CNode)
                                    MatchCnt += 1
                                Next

                                PNode.ForeColor = Color.Blue
                                PNode.Expand()

                                DupsTreeView.Nodes.Add(PNode)
#If TRACE Then
                                If CInt(_TraceParallel.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If
                        End Select

                    End If
                Next

                If MatchCnt = 0 Then
                    DupsTreeView.Nodes.Add("No Possible Duplicates Found")
                End If
            Else
                DupsTreeView.Nodes.Add("No Possible Duplicates Found")
            End If

        Catch ex As Exception
            Throw
        Finally
            _ExDup = Nothing
            _DuplicateThread = Nothing

            Me.Tabs.Refresh()

        End Try

    End Sub

    'Private Sub DetermineElectronicAlert(ClaimMasterDR As DataRow)

    '    Try

    '        If IsDBNull(ClaimMasterDR("MAXID")) = False Then
    '            Select Case True
    '                Case ClaimMasterDR("MAXID").ToString.ToUpper.StartsWith("E")
    '                    _ClaimAlertManager.AddAlertRow(New Object() {"Received as Electronic Submission via Cardiff", 0, "Header", 15})
    '                Case ClaimMasterDR("MAXID").ToString.ToUpper.StartsWith("U"), ClaimMasterDR("MAXID").ToString.ToUpper.StartsWith("H")
    '                    Select Case ClaimMasterDR("EDI_SOURCE").ToString
    '                        Case "PSBCCA", "PRBCCA"
    '                            Select Case ClaimMasterDR("REFERENCE_ID").ToString.Substring(7, 2)
    '                                Case "12", "13", "16", "24", "25", "36", "37", "39", "41", "42", "43", "44", "45", "46", "51", "56", "57", "67", "69", "73", "74", "75", "80", "89", "95", "97", "98"
    '                                    _ClaimAlertManager.AddAlertRow(New Object() {If(ClaimMasterDR("REFERENCE_ID").ToString.EndsWith("84"), "Adjustment - ", "") & "Received as " & If(ClaimMasterDR("EDI_SOURCE").ToString = "PSBCCA", "FFE", "JAA") & " Electronic Submission" & If(_ClaimDataSet.MEDHDR.Rows(0)("PRICED_BY").ToString.Contains(" (S)"), " (Summary Priced) ", "") & ", Original Paper Document via Anthem - DCN " & ClaimMasterDR("REFERENCE_ID").ToString, 0, "Header", 15})
    '                                Case "LB" To "LZ"
    '                                    _ClaimAlertManager.AddAlertRow(New Object() {If(ClaimMasterDR("REFERENCE_ID").ToString.EndsWith("84"), "Adjustment - ", "") & "Received as " & If(ClaimMasterDR("EDI_SOURCE").ToString = "PSBCCA", "FFE", "JAA") & " Electronic Submission" & If(_ClaimDataSet.MEDHDR.Rows(0)("PRICED_BY").ToString.Contains(" (S)"), " (Summary Priced) ", "") & ", Original Paper Document via Anthem - DCN " & ClaimMasterDR("REFERENCE_ID").ToString, 0, "Header", 15})
    '                                Case "MA" To "ZZ", "M0" To "M9", "47", "48", "49", "87"
    '                                    _ClaimAlertManager.AddAlertRow(New Object() {If(ClaimMasterDR("REFERENCE_ID").ToString.EndsWith("84"), "Adjustment - ", "") & "Received as " & If(ClaimMasterDR("EDI_SOURCE").ToString = "PSBCCA", "FFE", "JAA") & " Electronic Submission" & If(_ClaimDataSet.MEDHDR.Rows(0)("PRICED_BY").ToString.Contains(" (S)"), " (Summary Priced) ", "") & ", BlueCard via Anthem - DCN " & ClaimMasterDR("REFERENCE_ID").ToString, 0, "Header", 15})
    '                                Case Else
    '                                    _ClaimAlertManager.AddAlertRow(New Object() {If(ClaimMasterDR("REFERENCE_ID").ToString.EndsWith("84"), "Adjustment - ", "") & "Received as " & If(ClaimMasterDR("EDI_SOURCE").ToString = "PSBCCA", "FFE", "JAA") & " Electronic Submission" & If(_ClaimDataSet.MEDHDR.Rows(0)("PRICED_BY").ToString.Contains(" (S)"), " (Summary Priced) ", "") & "", 0, "Header", 15})
    '                            End Select

    '                        Case Else
    '                            _ClaimAlertManager.AddAlertRow(New Object() {"Received as Electronic Submission" & If(_ClaimDataSet.MEDHDR.Rows(0)("PRICED_BY").ToString.Contains(" (S)"), " (Summary Priced) ", "") & " from " & ClaimMasterDR("EDI_SOURCE").ToString, 0, "Header", 15})
    '                    End Select
    '            End Select
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Sub

    Private Sub AssociatedDisplayLineDetails()

        Dim DV As DataView
        ' Dim BM As BindingManagerBase
        Dim BS As BindingSource
        Dim DR As DataRow

        Dim ReasonsDV As DataView
        Dim DiagnosisDV As DataView
        Dim ModifierDV As DataView
        Dim DetailDV As DataView
        Try

            Using WC As New GlobalCursor

                DV = AssociatedResultsDataGrid.GetCurrentDataView
                'BM = Me.AssociatedResultsDataGrid.BindingContext(Me.AssociatedResultsDataGrid.DataSource, Me.AssociatedResultsDataGrid.DataMember)
                BS = DirectCast(Me.AssociatedResultsDataGrid.DataSource, BindingSource)

                If BS Is Nothing OrElse BS.Current Is Nothing Then Exit Sub

                DR = DirectCast(BS.Current, DataRowView).Row

                If Not _AssociatedClaimDS.Tables("MEDDTL").Columns.Contains("REASONS") Then
                    _AssociatedClaimDS.Tables("MEDDTL").Columns.Add("REASONS")
                End If

                If Not _AssociatedClaimDS.Tables("MEDDTL").Columns.Contains("DIAGNOSES") Then
                    _AssociatedClaimDS.Tables("MEDDTL").Columns.Add("DIAGNOSES")
                End If

                If Not _AssociatedClaimDS.Tables("MEDDTL").Columns.Contains("MODIFIERS") Then
                    _AssociatedClaimDS.Tables("MEDDTL").Columns.Add("MODIFIERS")
                End If

                ReasonsDV = New DataView(_AssociatedClaimDS.Tables("REASON"), "CLAIM_ID = " & CInt(DR("CLAIM_ID")), "CLAIM_ID", DataViewRowState.CurrentRows)
                DiagnosisDV = New DataView(_AssociatedClaimDS.Tables("MEDDIAG"), "CLAIM_ID = " & CInt(DR("CLAIM_ID")), "CLAIM_ID", DataViewRowState.CurrentRows)
                ModifierDV = New DataView(_AssociatedClaimDS.Tables("MEDMOD"), "CLAIM_ID = " & CInt(DR("CLAIM_ID")), "CLAIM_ID", DataViewRowState.CurrentRows)
                DetailDV = New DataView(_AssociatedClaimDS.Tables("MEDDTL"), "CLAIM_ID = " & CInt(DR("CLAIM_ID")), "CLAIM_ID", DataViewRowState.CurrentRows)

                For Each DetailDRV As DataRowView In DetailDV
                    Dim RowReasons As StringBuilder = New StringBuilder
                    ReasonsDV.RowFilter = "CLAIM_ID = " & CInt(DR("CLAIM_ID")) & " AND LINE_NBR = " & DetailDRV("LINE_NBR").ToString
                    ReasonsDV.Sort = "Priority"

                    For Each ReasonDRV As DataRowView In ReasonsDV
                        RowReasons.Append(If(RowReasons.ToString.Trim.Length > 0, ", ", "") & If(ReasonDRV("REASON").ToString.Trim = "LTR" AndAlso Not IsDBNull(ReasonDRV("REASON")), ReasonDRV("REASON").ToString.Trim & ": " & ReasonDRV("LETTER_NAMES").ToString, ReasonDRV("REASON").ToString.Trim))
                    Next

                    DetailDRV("REASONS") = RowReasons

                    Dim RowDiagnoses As StringBuilder = New StringBuilder
                    DiagnosisDV.RowFilter = "CLAIM_ID = " & CInt(DR("CLAIM_ID")) & " AND LINE_NBR = " & DetailDRV("LINE_NBR").ToString
                    DiagnosisDV.Sort = "Priority"
                    For Each DiagnosisDRV As DataRowView In DiagnosisDV
                        RowDiagnoses.Append(If(RowDiagnoses.ToString.Trim.Length > 0, ", ", "") & DiagnosisDRV("DIAGNOSIS").ToString)
                    Next

                    DetailDRV("DIAGNOSES") = RowDiagnoses

                    Dim RowModifiers As StringBuilder = New StringBuilder
                    ModifierDV.RowFilter = "CLAIM_ID = " & CInt(DR("CLAIM_ID")) & " AND LINE_NBR = " & DetailDRV("LINE_NBR").ToString
                    ModifierDV.Sort = "Priority"
                    For Each ModifierDRV As DataRowView In ModifierDV
                        RowModifiers.Append(If(RowModifiers.ToString.Trim.Length > 0, ", ", "") & ModifierDRV("MODIFIER").ToString)
                    Next

                    DetailDRV("MODIFIERS") = RowModifiers

                Next

                SumAmt(_AssociatedClaimDS.Tables("MEDDTL"), CInt(DR("CLAIM_ID")), "CHRG_AMT", txtAssociatedTotalCharges)
                SumAmt(_AssociatedClaimDS.Tables("MEDDTL"), CInt(DR("CLAIM_ID")), "PAID_AMT", txtAssociatedTotalPaid)
                SumAmt(_AssociatedClaimDS.Tables("MEDDTL"), CInt(DR("CLAIM_ID")), "ALLOWED_AMT", txtAssociatedTotalAllowed)
                SumAmt(_AssociatedClaimDS.Tables("MEDDTL"), CInt(DR("CLAIM_ID")), "OTH_INS_AMT", txtAssociatedTotalOtherInsurance)
                SumAmt(_AssociatedClaimDS.Tables("MEDDTL"), CInt(DR("CLAIM_ID")), "HRA_AMT", txtAssociatedTotalHRAApplied)

                txtAssociatedTaxID.Text = _AssociatedClaimDS.Tables("Results").Select("CLAIM_ID = " & CInt(DR("CLAIM_ID")))(0)("PROV_TIN").ToString

                DetailDV.Sort = "LINE_NBR"

                _AssDetailLineBS = New BindingSource
                _AssDetailLineBS.DataSource = DetailDV
                AssociatedDetailLinesDataGrid.SuspendLayout()
                AssociatedDetailLinesDataGrid.DataSource = _AssDetailLineBS
                AssociatedDetailLinesDataGrid.ResumeLayout()
                ' AssociatedDetailLinesDataGrid.SetTableStyle()
                SetTableStyle(AssociatedDetailLinesDataGrid, False)
                AssociatedDetailLinesDataGrid.Select(_AssDetailLineBS.Position)

                AssociatedDetailLinesDataGrid.CaptionText = DetailDV.Count & " - Detail Line" & If(DetailDV.Count <> 1, "s", "") & " for Claim - " & DR("CLAIM_ID").ToString

                IDMCorrelationTimer.Enabled = True

                If Not IsDBNull(DR("DOCID")) Then
                    Dim CurDoc As Long?

                    CurDoc = Display.CurrentDocumentID

                    If CurDoc Is Nothing OrElse CurDoc <> CLng(DR("DOCID")) Then

                        Using FNDisplay As New Display
                            FNDisplay.Display(New List(Of Long?) From {CLng(DR("DOCID"))})
                        End Using
                        Me.Focus()
                    End If

                End If

            End Using

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub

    Private Function SumAmt(dt As DataTable, claimID As Integer, fieldName As String, destinationControl As TextBox) As Decimal

        Dim DV As DataView
        Dim Total As Decimal = 0D

        Try

            DV = New DataView(dt, "CLAIM_ID = " & claimID.ToString & " AND ISNULL(CHRG_AMT,-1.23) <> -1.23  AND STATUS <> 'MERGED'", fieldName, DataViewRowState.CurrentRows)

            If DV.Count > 0 Then
                For Cnt As Integer = 0 To DV.Count - 1
                    Total += If(IsDBNull(DV(Cnt)(fieldName)), 0D, CDec(DV(Cnt)(fieldName)))
                Next

                destinationControl.Text = Format(Total, "0.00;-0.00")

                Return Total
            Else
                destinationControl.Text = ""
            End If

            Return 0D

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub ValidateProvTIN()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Looks up provider id and name
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/24/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim ProvDR As DataRow
        Dim TIN As String

        Try
            TIN = UFCWGeneral.UnFormatTIN(txtProviderID.Text)
            If IsNumeric(TIN) Then
                ProvDR = CMSDALFDBMD.RetrieveProviderInfo(CInt(TIN))
            End If

            If ProvDR IsNot Nothing Then
                _ClaimDr("PROV_ID") = ProvDR("PROVIDER_ID")
                _ClaimDr("PROV_NAME") = ProvDR("NAME1")
                _ClaimDr("PROV_TIN") = ProvDR("TAXID")

                _ClaimAlertManager.DeleteAlertRowsByMessage("Invalid Provider")
                _ClaimAlertManager.DeleteAlertRowsByMessage("Provider has Suspended Address")
                _ClaimAlertManager.DeleteAlertRowsByMessage("Provider''s Name Matches Patient''s Name")

                If IsDBNull(ProvDR("NAME1")) = False AndAlso IsDBNull(_ClaimDr("PAT_LNAME")) = False AndAlso CStr(ProvDR("NAME1")).StartsWith(CStr(_ClaimDr("PAT_LNAME"))) Then
                    _ClaimAlertManager.AddAlertRow(New Object() {"Provider's Name Matches Patient's Name", 0, "Header", 20})
                End If

                _ClaimAlertManager.DeleteAlertRowsByCategory("ProvAlert")
                If IsDBNull(ProvDR("ALERT")) = False AndAlso CStr(ProvDR("ALERT")).Trim <> "" Then
                    _ClaimAlertManager.AddAlertRow(New Object() {ProvDR("DESCRIPTION").ToString & " (Prov Alert)", 0, "ProvAlert", 30})
                End If

                'Provider suspended
                If CBool(If(IsDBNull(ProvDR("SUSPEND_SW")), False, ProvDR("SUSPEND_SW"))) Then
                    _ClaimAlertManager.AddAlertRow(New Object() {"Invalid Provider", 0, "Header", 30})
                End If

                'Provider suspended Address
                If CBool(ProvDR("PROV_ADDRESS_SUSPEND_SW")) = True Then
                    _ClaimAlertManager.AddAlertRow(New Object() {"Provider has Suspended Address", 0, "Header", 30})
                End If
            Else
                _ClaimDr("PROV_ID") = DBNull.Value
                _ClaimDr("PROV_NAME") = "***INVALID PROVIDER***"
                _ClaimDr("PROV_TIN") = DBNull.Value
                _ClaimAlertManager.AddAlertRow(New Object() {"Invalid Provider", 0, "Header", 30})
                _ClaimAlertManager.DeleteAlertRowsByMessage("Provider''s Name Matches Patient''s Name")
                _ClaimAlertManager.DeleteAlertRowsByCategory("ProvAlert")
            End If

            _ClaimMasterBS.EndEdit()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    'Private Sub DeleteAlertManagerDataTableRow(ByVal AlertManagerRowFields As Object())
    '    'checks if alert exists and adds if missing
    '    ' 0 = Message
    '    ' 1 = Line
    '    ' 2 = Category
    '    ' 3 = Priority
    '    ' 4 = Tags (Objects to Highlight)

    '    Dim AlertDataView As DataView

    '    Try

    '        AlertDataView = New DataView(_ClaimAlertManager.AlertManagerDataTable, "Category = '" & AlertManagerRowFields(2).ToString & "' And Message = '" & AlertManagerRowFields(0).ToString & "'", "Category, Message", DataViewRowState.CurrentRows)
    '        If AlertDataView.Count = 0 Then
    '            _ClaimAlertManager.AlertManagerDataTable.Rows.Add(AlertManagerRowFields)
    '        End If

    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Sub

    'Private Sub AddAlertManagerDataTableRow(ByVal AlertManagerRowFields As Object())
    '    'checks if alert exists and adds if missing
    '    ' 0 = Message
    '    ' 1 = Line
    '    ' 2 = Category
    '    ' 3 = Priority
    '    ' 4 = Tags (Objects to Highlight)

    '    Dim AlertDataView As DataView

    '    Try

    '        AlertDataView = New DataView(_ClaimAlertManager.AlertManagerDataTable, "Category = '" & AlertManagerRowFields(2).ToString & "' And Message = '" & AlertManagerRowFields(0).ToString & "'", "Category, Message", DataViewRowState.CurrentRows)
    '        If AlertDataView.Count = 0 Then
    '            _ClaimAlertManager.AlertManagerDataTable.Rows.Add(AlertManagerRowFields)
    '        End If

    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Sub

    Private Sub AlertFocus(ByVal obj As Object)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' handles setting focus to the offending control of the alert
        ' </summary>
        ' <param name="obj"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DG As Object
        Dim DV As DataView
        Dim G As Graphics
        Dim R As Rectangle

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If obj IsNot Nothing Then
                If TypeOf obj Is TabPage Then

                    If Me.Tabs.SelectedTab.Name = CType(obj, TabPage).Name Then
                        Me.Tabs.SelectedTab = CType(obj, TabPage)
                    End If

                ElseIf TypeOf obj Is Array Then

                    Dim ObjArray() As Object = CType(obj, Object())

                    For Cnt As Integer = 0 To UBound(ObjArray, 1)
                        If TypeOf ObjArray(Cnt) Is DataGrid Then DG = ObjArray(Cnt) 'DataGridPlus.DataGridCustom
                        If TypeOf ObjArray(Cnt) Is DataRow Then
                            DV = CType(DG, DataGridCustom).GetDefaultDataView()
                            For SubCnt As Integer = 0 To DV.Count - 1
                                If DV(SubCnt)("LINE_NBR").ToString = CType(ObjArray(Cnt), DataRow)("LINE_NBR").ToString Then
                                    CType(DG, DataGridCustom).CurrentRowIndex = SubCnt
                                    CType(DG, DataGridCustom).Select(SubCnt)
                                    Exit For
                                End If
                            Next
                        Else
                            AlertFocus(ObjArray(Cnt))
                        End If
                    Next

                ElseIf TypeOf obj Is ToolBarButton Then
                    G = ClaimToolBar.CreateGraphics
                    R = CType(obj, ToolBarButton).Rectangle

                    R.Inflate(-1, -1)

                    G.DrawRectangle(New Pen(Color.Red), R)

                    CType(obj, ToolBarButton).Parent.Focus()
                ElseIf TypeOf obj Is Control Then
                    CType(obj, Control).Focus()
                End If
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ValidatePartSSN(Optional ByVal origSSN As Integer? = Nothing)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' looks up participant info
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/22/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim PartDR As DataRow
        Dim SSN As Integer?
        Dim Transaction As DbTransaction
        Dim SaveAndQuit As Boolean = False
        Dim CancelChanges As Boolean = False

        Try
            origSSN = UFCWGeneral.IsNullIntegerHandler(_ClaimDr("PART_SSN"))

            SSN = UFCWGeneral.IsNullIntegerHandler(UFCWGeneral.UnFormatSSN(txtPartSSN.Text))

            If origSSN <> SSN Then
                If IsNumeric(SSN) Then
                    PartDR = CMSDALFDBMD.RetrieveParticipantInfo(CInt(SSN))

                    If PartDR IsNot Nothing Then

                        If CBool(PartDR("TRUST_SW")) Then
                            If UFCWGeneralAD.CMSCanAdjudicateEmployee() = False Then
                                If MessageBox.Show("You are not authorized to work on Employee Claims." &
                                                vbCrLf & "You must save changes back to the queue." & vbCrLf &
                                                "Are you sure you want to make this change?", "Confirm Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                                    SaveAndQuit = True
                                Else
                                    CancelChanges = True
                                    txtPartSSN.Text = UFCWGeneral.FormatSSN(CStr(_ClaimDr("PART_SSN")))
                                End If
                            End If
                        End If

                        If Not SaveAndQuit AndAlso Not CancelChanges Then
                            If IsFamilyLocked(CInt(PartDR("FAMILY_ID"))) Then
                                If MessageBox.Show("This Family is Currently Locked." &
                                                    vbCrLf & "You must save changes back to the queue." & vbCrLf &
                                                    "Are you sure you want to make this change?", "Confirm Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                                    SaveAndQuit = True
                                Else
                                    CancelChanges = True
                                    txtPartSSN.Text = UFCWGeneral.FormatSSN(CStr(_ClaimDr("PART_SSN")))
                                End If
                            Else
                                CMSDALFDBMD.InsertFamilyLock(CInt(PartDR("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")), _ClaimID, _DomainUser.ToUpper, SystemInformation.ComputerName)
                                CMSDALFDBMD.ReleaseFamilyLock(CInt(_ClaimDr("FAMILY_ID")))
                            End If
                        End If

                        If Not CancelChanges Then
                            _ClaimDr("FAMILY_ID") = PartDR("FAMILY_ID")
                            _ClaimDr("PART_SSN") = PartDR("PART_SSNO")
                            _ClaimDr("PART_FNAME") = PartDR("FIRST_NAME")
                            _ClaimDr("PART_INT") = PartDR("MIDDLE_INITIAL")
                            _ClaimDr("PART_LNAME") = PartDR("LAST_NAME")
                            _ClaimDr("SECURITY_SW") = PartDR("TRUST_SW")
                            _ClaimAlertManager.DeleteAlertRowsByMessage("Invalid Participant")

                            If CShort(_ClaimDr("RELATION_ID")) <> 0 Then
                                _ClaimDr("RELATION_ID") = -1
                                _ClaimDr("PAT_SSN") = 0
                                _ClaimDr("PAT_FNAME") = DBNull.Value
                                _ClaimDr("PAT_INT") = DBNull.Value
                                _ClaimDr("PAT_LNAME") = DBNull.Value

                                If _ClaimDS.MEDHDR.Rows.Count > 0 Then
                                    _MedHdrDr("PAT_DOB") = DBNull.Value
                                    _MedHdrDr("PAT_SEX") = DBNull.Value
                                End If

                                _ClaimAlertManager.AddAlertRow(New Object() {"Invalid Patient", 0, "Header", 30})
                            Else
                                _ClaimDr("PAT_SSN") = SSN
                                _ClaimDr("PAT_FNAME") = PartDR("FIRST_NAME")
                                _ClaimDr("PAT_INT") = PartDR("MIDDLE_INITIAL")
                                _ClaimDr("PAT_LNAME") = PartDR("LAST_NAME")

                                If _ClaimDS.MEDHDR.Rows.Count > 0 Then
                                    _MedHdrDr("PAT_DOB") = PartDR("BIRTH_DATE")
                                    _MedHdrDr("PAT_SEX") = PartDR("GENDER")
                                End If

                                _ClaimAlertManager.DeleteAllAlertRowsLikeMessage("Invalid Patient")

                                If _Mode.ToUpper = "AUDIT" Then
                                    LoadEligibility(True)
                                Else
                                    LoadEligibility(False)
                                End If
                            End If

                            _ClaimAlertManager.AddAlertRow(New Object() {"Re-Calc Is Required", 0, "Header", 30})

                        End If

                        If SaveAndQuit Then
                            'not auth to work employee
                            FinalizeEdits()

                            Transaction = CMSDALCommon.BeginTransaction

                            If SaveChanges(Transaction) Then
                                CMSDALCommon.CommitTransaction(Transaction)
                                Me.Close()
                            End If
                        End If
                    End If

                Else
                    _ClaimDr("PART_FNAME") = DBNull.Value
                    _ClaimDr("PART_INT") = DBNull.Value
                    _ClaimDr("PART_LNAME") = DBNull.Value

                    _ClaimAlertManager.AddAlertRow(New Object() {"Invalid Participant", 0, "Header", 30})
                End If

                _ClaimMasterBS.EndEdit()
                _MedHdrBS.EndEdit()
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ValidatePatientSSN(Optional ByVal origSSN As Integer? = Nothing)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' looks up patient info
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/22/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DR As DataRow
        Dim SSN As Integer?


        Try

            If origSSN Is Nothing Then origSSN = CType(_ClaimDr("PAT_SSN"), Integer?)

            SSN = CType(UFCWGeneral.UnFormatSSN(txtPatSSN.Text), Integer?)
            If CType(_ClaimDr("PAT_SSN"), Integer?) <> SSN OrElse origSSN <> SSN Then

                If IsNumeric(SSN) Then
                    DR = CMSDALFDBMD.RetrievePatientInfo(CInt(_ClaimDr("FAMILY_ID")), CInt(SSN))

                    If DR IsNot Nothing Then
                        If Not IsDBNull(_ClaimDr("RELATION_ID")) AndAlso CShort(_ClaimDr("RELATION_ID")) <> CInt(DR("RELATION_ID")) Then
                            _ClaimDr("RELATION_ID") = DR("RELATION_ID")

                            _ClaimAlertManager.AddAlertRow(New Object() {"Re-Calc Is Required", 0, "Header", 30})
                        End If

                        _ClaimDr("PAT_SSN") = DR("SSNO")
                        _ClaimDr("PAT_FNAME") = DR("FIRST_NAME")
                        _ClaimDr("PAT_INT") = DR("MIDDLE_INITIAL")
                        _ClaimDr("PAT_LNAME") = DR("LAST_NAME")

                        If _MedHdrDr IsNot Nothing Then
                            _MedHdrDr("PAT_DOB") = DR("BIRTH_DATE")
                            _MedHdrDr("PAT_SEX") = DR("GENDER")
                        End If

                        _ClaimAlertManager.DeleteAllAlertRowsLikeMessage("'Invalid Patient'")

                        If _Mode.ToUpper = "AUDIT" Then
                            LoadEligibility(True)
                        Else
                            LoadEligibility(False)
                        End If

                    Else

                        _ClaimDr("PAT_FNAME") = DBNull.Value
                        _ClaimDr("PAT_INT") = DBNull.Value
                        _ClaimDr("PAT_LNAME") = DBNull.Value

                        If _MedHdrDr IsNot Nothing Then
                            _MedHdrDr("PAT_DOB") = DBNull.Value
                            _MedHdrDr("PAT_SEX") = DBNull.Value
                        End If

                        _ClaimAlertManager.AddAlertRow(New Object() {"Invalid Patient", 0, "Header", 30})
                        _ClaimAlertManager.AddAlertRow(New Object() {"Re-Calc Is Required", 0, "Header", 30})

                    End If

                End If
                _ClaimMasterBS.EndEdit()
                _MedHdrBS.EndEdit()
                'Me.BindingContext(_ClaimDS.CLAIM_MASTER).EndCurrentEdit()
                'Me.BindingContext(_ClaimDS.MEDHDR).EndCurrentEdit()

            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function ValidateBillType(meddtlDR As DataRow, ByVal billType As String, ByVal dateOfService As Date?) As DataRow

        Dim DR As DataRow

        Try
            DR = CMSDALFDBMD.RetrieveBillTypeValuesInformation(billType, dateOfService)

            If DR Is Nothing Then
                txtBillType.SelectionStart = 0
                txtBillType.SelectionLength = txtBillType.Text.Length

                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & meddtlDR("LINE_NBR").ToString & ": Invalid Bill Type", meddtlDR("LINE_NBR").ToString, "Detail", 30})

                Return Nothing

            End If

            Return DR

        Catch ex As Exception
            Throw
        End Try

    End Function
#End Region

#Region "Style"

    Private Sub SetTableStyle(ByVal dg As DataGridCustom) 'called when no context menu is in use
        SetTableStyle(dg, Nothing, False)
    End Sub

    Private Sub SetTableStyle(ByVal dg As DataGridCustom, ByVal editable As Boolean) 'called when no context menu is in use, but editflag is supplied
        SetTableStyle(dg, Nothing, editable)
    End Sub

    Private Sub TableStyleReset(ByVal sender As Object, e As EventArgs)
        Dim dg As DataGridCustom = CType(sender, DataGridCustom)
        SetTableStyleColumns(dg, _EditableTextBox)
    End Sub

    Private Sub SetTableStyle(ByVal dg As DataGridCustom, ByVal dataGridCustomContextMenu As System.Windows.Forms.ContextMenuStrip, Optional ByVal editable As Boolean = True)

        Try
            _EditableTextBox = editable

            SetTableStyleColumns(dg, editable)
            dg.StyleName = dg.Name
            dg.AppKey = _APPKEY
            dg.ContextMenuPrepare(dataGridCustomContextMenu)

            RemoveHandler dg.ResetTableStyle, AddressOf TableStyleReset
            AddHandler dg.ResetTableStyle, AddressOf TableStyleReset

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub SetTableStyleColumns(ByVal dg As DataGridCustom, ByVal editable As Boolean)

        Dim DGTS As DataGridTableStyle
        Dim DGTSDefault As DataGridTableStyle

        Dim TextCol As DataGridFormattableTextBoxColumn
        Dim BoolCol As DataGridColorBoolColumn
        Dim IconCol As DataGridHighlightIconColumn
        ' Dim ButtCol As DataGridHighlightButtonColumn

        Dim ColsDV As DataView
        Dim DefaultStyleDS As DataSet
        Dim XMLStyleName As String
        Dim ResultDT As DataTable
        Dim ColumnSequenceDV As DataView

        Try

            XMLStyleName = dg.Name

            DefaultStyleDS = DataGridCustom.GetTableStyle(XMLStyleName)

            DGTS = New DataGridTableStyle()

            If dg.GetCurrentDataTable Is Nothing Then
                DGTS.MappingName = dg.Name
            Else
                DGTS.MappingName = dg.GetCurrentDataTable.TableName
            End If

            DGTS.GridColumnStyles.Clear()

            If DefaultStyleDS.Tables.Contains(XMLStyleName & "Style") Then
                If DefaultStyleDS.Tables(XMLStyleName & "Style").Columns.Contains("GridLineStyle") Then
                    DGTS.GridLineStyle = If(CBool(DefaultStyleDS.Tables(XMLStyleName & "Style").Rows(0)("GridLineStyle")), DataGridLineStyle.Solid, DataGridLineStyle.None)
                End If
                If DefaultStyleDS.Tables(XMLStyleName & "Style").Columns.Contains("RowHeadersVisible") Then
                    DGTS.RowHeadersVisible = CBool(DefaultStyleDS.Tables(XMLStyleName & "Style").Rows(0)("RowHeadersVisible"))
                End If
            End If

            ResultDT = dg.LoadColumnsSizeAndPosition(dg.Name & "\" & DGTS.MappingName & "\ColumnSettings", DefaultStyleDS.Tables("Column"))

            ColumnSequenceDV = New DataView(ResultDT)
            ColsDV = ColumnSequenceDV

            If dg.GetCurrentDataTable IsNot Nothing Then
                If dg.GetCurrentDataTable.Columns.Contains("Icon") = True Then
                    IconCol = New DataGridHighlightIconColumn(dg, DIList)
                    IconCol.HeaderText = ""
                    IconCol.MappingName = "Icon"
                    IconCol.NullText = ""
                    IconCol.Width = 16
                    IconCol.MaximumCharWidth = IconCol.Width
                    IconCol.MinimumCharWidth = IconCol.Width
                    IconCol.TextBox.CausesValidation = False

                    AddHandler IconCol.PaintCellPicture, AddressOf DetermineCellIcon

                    DGTS.GridColumnStyles.Add(IconCol)
                End If
            End If

            DGTSDefault = New DataGridTableStyle() 'This can be used to establish the columns displayed by default
            DGTSDefault.MappingName = "Default"

            For IntCol As Integer = 0 To ColsDV.Count - 1

                If (IsDBNull(ColsDV(IntCol).Item("Visible")) OrElse ColsDV(IntCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(IntCol).Item("Visible"))) Then
                    TextCol = New DataGridFormattableTextBoxColumn
                    TextCol.MappingName = CStr(ColsDV(IntCol).Item("Mapping"))
                    TextCol.HeaderText = CStr(ColsDV(IntCol).Item("HeaderText"))
                    If IsDBNull(ColsDV(IntCol).Item("Format")) = False AndAlso ColsDV(IntCol).Item("Format").ToString.Trim.Length > 0 Then
                        TextCol.Format = CStr(ColsDV(IntCol).Item("Format"))
                    End If
                    TextCol.TextBox.CausesValidation = False
                    DGTSDefault.GridColumnStyles.Add(TextCol)
                End If

                If ((IsDBNull(ColsDV(IntCol).Item("Visible")) OrElse ColsDV(IntCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(IntCol).Item("Visible"))) AndAlso (GetAllSettings(_APPKEY, dg.Name & "\" & DGTS.MappingName & "\Customize\ColumnSelector") Is Nothing OrElse CDbl(GetSetting(_APPKEY, dg.Name & "\" & DGTS.MappingName & "\Customize\ColumnSelector", "Col " & ColsDV(IntCol).Item("Mapping").ToString & " Customize", "0")) = 1)) Then

                    If ColsDV(IntCol).Item("Type").ToString.Trim = "Text" OrElse Not editable Then
                        TextCol = New DataGridFormattableTextBoxColumn
                        TextCol.MappingName = CStr(ColsDV(IntCol).Item("Mapping"))
                        TextCol.HeaderText = CStr(ColsDV(IntCol).Item("HeaderText"))
                        TextCol.Width = CInt(ColsDV(IntCol).Item("SizeInPixels"))
                        TextCol.NullText = CStr(ColsDV(IntCol).Item("NullText"))
                        TextCol.TextBox.WordWrap = True
                        TextCol.TextBox.Name = TextCol.MappingName

                        Try

                            If Not IsDBNull(ColsDV(IntCol).Item("ReadOnly")) AndAlso ColsDV(IntCol).Item("ReadOnly").ToString.Trim.Length > 0 AndAlso CBool(ColsDV(IntCol).Item("ReadOnly")) Then
                                TextCol.ReadOnly = True
                            End If

                        Catch IgnoreException As Exception
                        End Try

                        If IsDBNull(ColsDV(IntCol).Item("Format")) = False Then
                            If ColsDV(IntCol).Item("Format").ToString.Trim = "YesNo" Then
                                AddHandler TextCol.Formatting, AddressOf DataGridCustom.FormattingYesNo
                            ElseIf ColsDV(IntCol).Item("Format").ToString.Trim.Length > 0 Then
                                TextCol.Format = CStr(ColsDV(IntCol).Item("Format"))
                            End If
                        End If

                        If CStr(ColsDV(IntCol).Item("Mapping")).ToUpper = "MED_PLAN" Then
                            AddHandler TextCol.OnPaint, AddressOf PlanHighlight
                        End If
                        If CStr(ColsDV(IntCol).Item("Mapping")).ToUpper = "REASON_SW" Then
                            Select Case dg.Name
                                Case "DetailLinesDataGrid"
                                    AddHandler TextCol.Formatting, AddressOf DetailLinesDataGrid_FormattingReason
                                Case "OriginalDetailLinesDataGrid"
                                    AddHandler TextCol.Formatting, AddressOf OriginalDetailLinesDataGrid_FormattingReason
                            End Select
                        End If

                        TextCol.TextBox.CausesValidation = False
                        DGTS.GridColumnStyles.Add(TextCol)

                    ElseIf ColsDV(IntCol).Item("Type").ToString.Contains("Button") Then

                        _ButtCol = New DataGridHighlightButtonColumn(dg)
                        _ButtCol.Alignment = HorizontalAlignment.Left
                        _ButtCol.MappingName = CStr(ColsDV(IntCol).Item("Mapping"))
                        _ButtCol.HeaderText = CStr(ColsDV(IntCol).Item("HeaderText"))
                        _ButtCol.Width = CInt(ColsDV(IntCol).Item("SizeInPixels"))
                        _ButtCol.NullText = CStr(ColsDV(IntCol).Item("NullText"))

                        _Delegate = [Delegate].CreateDelegate(GetType(System.EventHandler), Me, ColsDV(IntCol).Item("Method").ToString)
                        AddHandler _ButtCol.ColumnButton.Click, CType(_Delegate, Global.System.EventHandler)

                        If CStr(ColsDV(IntCol).Item("Mapping")).ToUpper = "REASON_SW" Then
                            AddHandler _ButtCol.Formatting, AddressOf DetailLinesDataGrid_FormattingReason
                            AddHandler _ButtCol.UnFormatting, AddressOf UnFormattingReason
                        ElseIf CStr(ColsDV(IntCol).Item("Mapping")).ToUpper = "ACCUMULATORS" Then
                            AddHandler _ButtCol.Formatting, AddressOf DetailLinesDataGrid_FormattingAccumulators
                            AddHandler _ButtCol.UnFormatting, AddressOf UnFormattingAccumulators
                        End If

                        DGTS.PreferredRowHeight = _ButtCol.ColumnButton.Height + 2
                        DGTS.GridColumnStyles.Add(_ButtCol)

                    ElseIf ColsDV(IntCol).Item("Type").ToString.Contains("Bool") Then
                        BoolCol = New DataGridColorBoolColumn(IntCol)
                        BoolCol.MappingName = CStr(ColsDV(IntCol).Item("Mapping"))
                        BoolCol.HeaderText = CStr(ColsDV(IntCol).Item("HeaderText"))
                        BoolCol.Width = CInt(GetSetting(_APPKEY, dg.Name & "\" & DGTS.MappingName.ToString & "\ColumnSettings", "Col " & ColsDV(IntCol).Item("Mapping").ToString, CStr(UFCWGeneral.MeasureWidthinPixels(CInt(ColsDV(IntCol).Item("DefaultCharWidth")), dg.Font.Name, dg.Font.Size))))
                        BoolCol.NullText = CStr(ColsDV(IntCol).Item("NullText"))
                        BoolCol.TrueValue = CType("1", Decimal)
                        BoolCol.FalseValue = CType("0", Decimal)
                        BoolCol.AllowNull = False


                        Try

                            If Not IsDBNull(ColsDV(IntCol).Item("ReadOnly")) AndAlso CBool(ColsDV(IntCol).Item("ReadOnly")) Then
                                BoolCol.ReadOnly = True
                            End If

                        Catch ex As Exception
                        End Try

                        DGTS.GridColumnStyles.Add(BoolCol)

                    End If
                End If

            Next

            dg.TableStyles.Clear()
            dg.TableStyles.Add(DGTS)
            dg.TableStyles.Add(DGTSDefault)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            Else
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try

    End Sub

#End Region

#Region "Style Delegates"
    Private Sub ModifierColumnButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'These are called dynamically via late binding
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Shows the modifier dialog
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	4/11/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DGRow As DataRow

        Try
            If _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing Then Return

            DGRow = CType(_MedDtlBS.Current, DataRowView).Row

            If DGRow("STATUS").ToString <> "MERGED" Then
                ShowDetailLineModifiers()
            End If

            'set the button tag
            If (IsDBNull(CType(sender, Button).Tag) AndAlso IsDBNull(DGRow("MODIFIER"))) Then
                'do nothing
            ElseIf Not (IsDBNull(CType(sender, Button).Tag) AndAlso IsDBNull(DGRow("MODIFIER"))) OrElse
                        (IsDBNull(CType(sender, Button).Tag) AndAlso Not IsDBNull(DGRow("MODIFIER"))) OrElse
                        (CType(sender, Button).Tag.ToString <> DGRow("MODIFIER").ToString) Then
                CType(sender, Button).Tag = DGRow("MODIFIER")
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub DiagnosisColumnButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' shows the diagnosis dialog
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	4/11/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        'Dim DV As DataView
        Dim DGRow As DataRow
        Try
            If _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing Then Exit Sub
            DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row
            If DGRow Is Nothing Then Exit Sub

            If DGRow("STATUS").ToString <> "MERGED" Then
                ShowDetailLineDiagnosis()
            End If
            'set the button tag
            If (IsDBNull(CType(sender, Button).Tag) AndAlso IsDBNull(DGRow("DIAGNOSIS"))) Then
                'do nothing
            ElseIf Not (IsDBNull(CType(sender, Button).Tag) AndAlso IsDBNull(DGRow("DIAGNOSIS"))) OrElse
                            (IsDBNull(CType(sender, Button).Tag) AndAlso Not IsDBNull(DGRow("DIAGNOSIS"))) OrElse
                            (CType(sender, Button).Tag.ToString <> DGRow("DIAGNOSIS").ToString) Then
                CType(sender, Button).Tag = DGRow("DIAGNOSIS")
            End If

            DetailLinesDataGrid.EndCurrentEdit()

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub ReasonColumnButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' shows the reason dialog
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	4/11/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        'Dim DGRow As DataRow

        Try
            'If _MedDtlBS Is Nothing OrElse _MedDtlBS.Position < 0 Then Return

            ' DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

            '   If DGRow IsNot Nothing AndAlso DGRow("STATUS").ToString <> "MERGED" Then
            ShowDetailLineReasons()
            ' End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub AccumulatorsColumnButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' shows the Accumulators dialog
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	4/11/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DGRow As DataRow

        Try
            If _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing Then Return

            DGRow = CType(_MedDtlBS.Current, DataRowView).Row

            If DGRow IsNot Nothing AndAlso Not IsDBNull(DGRow("STATUS")) AndAlso DGRow("STATUS").ToString <> "MERGED" Then
                ShowDetailLineAccumulators(CShort(DGRow("LINE_NBR")))
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ReasonReadOnlyColumnButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' shows the reason dialog in readonly mode
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	4/11/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DGRow As DataRow

        Try
            If _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing Then Return

            DGRow = CType(_MedDtlBS.Current, DataRowView).Row
            If DGRow IsNot Nothing Then
                ShowReadOnlyDetailLineReasons(CShort(DGRow("LINE_NBR")), CDate(DGRow("OCC_FROM_DATE")))
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub



#End Region
    Private Function AssociatedControlToHighlight(alertMessage As String) As Object

        Dim DetailLineNumber As Short?

        Try
            If alertMessage.Contains("Line ") AndAlso alertMessage.Contains(":") Then
                DetailLineNumber = CType((alertMessage.Replace("Line ", "").Split(CChar(":")))(0).Trim, Short?)
                'DetailLinesDV = CloneHelper.DeepCopy(DetailLinesDataGrid.GetDefaultDataView.ToTable).DefaultView 'avoids currency/databinding issues
                'DetailLinesDV.RowFilter = "LINE_NBR = " & DetailLineNumber.ToString
                'DetailLinesDRV = DetailLinesDV(0)
            End If

            Select Case True
                Case alertMessage.Contains("(Audit)")
                    If _Mode.ToUpper = "AUDIT" Then
                        Return New Object() {ClaimTabPage, AuditButton}
                    Else
                        Return New Object() {ClaimTabPage}
                    End If
                Case alertMessage.Contains("Bill Type not validated because DOS is unavailable")
                    Return New Object() {ClaimTabPage, DetailLinesDataGrid, DetailLineNumber, txtBillType}
                Case alertMessage.Contains("Possible Preventative Diagnosis")
                    Return New Object() {ClaimTabPage, DetailLinesDataGrid, DetailLineNumber}
                Case alertMessage.Contains("Anthem Reported MRU Code")
                    Return New Object() {ClaimTabPage, RePriceToolBarButton}
                Case alertMessage.Contains("Paying Member")
                    Return New Object() {ClaimTabPage, cmbPayee}
                Case alertMessage.Contains("Incomplete Member Info, reselect Patient")
                    Return New Object() {ClaimTabPage, PatLookupButton}
                Case alertMessage.Contains("Original Claim Identified as Out Of Area, OI has been reset for JAA review")
                    Return New Object() {ClaimTabPage, OOACheckBox}
                Case alertMessage.Contains("Invalid Place of Service")
                    Return New Object() {ClaimTabPage, DetailLinesDataGrid, DetailLineNumber, txtPlaceOfService}
                Case alertMessage.Contains("Possible Case Management")
                    Return New Object() {ClaimTabPage, DetailLinesDataGrid, DetailLineNumber, txtPlaceOfService}
                Case alertMessage.Contains("(Pricing)")
                    Return New Object() {ClaimTabPage, RePriceToolBarButton}
                Case alertMessage.Contains("Check Units against Image")
                    Return New Object() {ClaimTabPage, DetailLinesDataGrid, DetailLineNumber, txtFromDate}
                Case alertMessage.Contains("Was Received 1 Year After DOS")
                    Return New Object() {ClaimTabPage, DetailLinesDataGrid, DetailLineNumber, txtFromDate}
                Case alertMessage.Contains("Non-PPOC Provider - Claim Denied")
                    Return New Object() {ClaimTabPage, txtProviderID}
                Case alertMessage.Contains("Missing From Date"), alertMessage.Contains("Invalid From Date")
                    Return New Object() {ClaimTabPage, DetailLinesDataGrid, DetailLineNumber, txtFromDate}
                Case alertMessage.Contains("Re-Calc Is Required")
                    Return New Object() {ClaimTabPage, ReCalcToolBarButton}
                Case alertMessage.Contains("Invalid Patient")
                    Return New Object() {ClaimTabPage, txtPatSSN}
                Case alertMessage.Contains("Paid Is More Than Priced")
                    Return New Object() {ClaimTabPage, DetailLinesDataGrid, DetailLineNumber, txtPaidAmt}
                Case alertMessage.Contains("Paid Is 0 and a Reason is Required")
                    Return New Object() {ClaimTabPage, DetailLinesDataGrid, DetailLineNumber, ReasonButton}
                Case alertMessage.Contains("Invalid Procedure Code")
                    Return New Object() {ClaimTabPage, DetailLinesDataGrid, DetailLineNumber, txtProcedure}
                Case alertMessage.Contains("Invalid Participant")
                    Return New Object() {ClaimTabPage, txtPartSSN}
                Case alertMessage.Contains("Invalid Bill Type")
                    Return New Object() {ClaimTabPage, DetailLinesDataGrid, DetailLineNumber, txtBillType}
                Case alertMessage.Contains("Invalid Provider"), alertMessage.Contains("Provider has Suspended Address"), alertMessage.Contains("(Prov Alert)"), alertMessage.Contains("Provider is Suspended"), alertMessage.Contains("Provider's Name Matches Patient's Name")
                    Return New Object() {ClaimTabPage, txtProviderID, ProvLookupButton}
                Case alertMessage.Contains("Summary Line is marked as NEW and must be completed")
                    Return New Object() {ClaimTabPage, DetailLinesDataGrid, DetailLineNumber, ReasonButton}
                Case alertMessage.Contains("Merge process prevents Auto WGS Close from completing")
                    Return New Object() {ClaimTabPage, ReCalcToolBarButton}
                Case alertMessage.Contains("Invalid Diagnosis")
                    Return New Object() {ClaimTabPage, DetailLinesDataGrid, DetailLineNumber, DiagnosisButton}
                Case alertMessage.Contains("Invalid Modifier")
                    Return New Object() {ClaimTabPage, DetailLinesDataGrid, DetailLineNumber, ModifiersButton}
                Case alertMessage.Contains("Provider's Name Matches Patient's Name")
                    Return New Object() {ClaimTabPage, ProvLookupButton}
                Case alertMessage.Contains("Provider has Suspended Address")
                    Return New Object() {ClaimTabPage, ProvLookupButton}

                Case DetailLineNumber IsNot Nothing
                    Return New Object() {ClaimTabPage, DetailLinesDataGrid}
                Case Else
                    Return New Object() {ClaimTabPage}
            End Select

        Catch
            Throw
        End Try

    End Function

#Region "Data Binding"

    Private Sub SSNBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' adjusts SSN values entered for a databinding
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim eOriginal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso Not IsNumeric(e.Value) Then
                e.Value = UFCWGeneral.UnFormatSSN(CStr(e.Value))
            End If

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, TextBox).Name & " Proposed: " & CStr(TryCast(e.Value, String)) & " Original: " & eOriginal & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        End Try
    End Sub

    Private Sub SSNBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' formats SSN values entered for a databinding
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim eOriginal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso e.Value IsNot Nothing AndAlso e.Value.ToString.Trim.Length > 0 AndAlso IsNumeric(e.Value) Then
                e.Value = UFCWGeneral.FormatSSN(CStr(e.Value))
            End If

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, TextBox).Name & " Proposed: " & CStr(TryCast(e.Value, String)) & " Original: " & eOriginal & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        End Try
    End Sub

    Private Sub TINBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' adjusts TIN values entered for a databinding
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim eOriginal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso e.Value.ToString.Trim.Length = 0 Then
                e.Value = DBNull.Value
            ElseIf Not IsDBNull(e.Value) AndAlso Not IsNumeric(e.Value) Then
                e.Value = UFCWGeneral.UnFormatTIN(CStr(e.Value))
            End If

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, TextBox).Name & " Proposed: " & CStr(TryCast(e.Value, String)) & " Original: " & eOriginal & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        End Try
    End Sub

    Private Sub TINBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' formats TIN values entered for a databinding
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim eOriginal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso e.Value IsNot Nothing AndAlso e.Value.ToString.Trim.Length > 0 AndAlso IsNumeric(e.Value) Then
                e.Value = UFCWGeneral.FormatTIN(CStr(e.Value))
            End If

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, TextBox).Name & " Proposed: " & CStr(TryCast(e.Value, String)) & " Original: " & eOriginal & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        End Try
    End Sub

    Private Sub ComboBoxSelectedItem_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Dim eOriginal As String = e.Value.ToString

        Try

            If TypeOf (e.Value) Is DataRowView Then

                If IsDBNull(CType(e.Value, System.Data.DataRowView).Row(CType(sender, ComboBox).DisplayMember)) AndAlso CType(e.Value, System.Data.DataRowView).Row(CType(sender, ComboBox).DisplayMember).ToString.Trim.Length = 0 Then
                    e.Value = DBNull.Value
                End If

            Else
                If Not IsDBNull(e.Value) AndAlso e.Value.ToString.Trim.Length = 0 Then
                    e.Value = DBNull.Value
                End If
            End If

        Catch ex As InvalidCastException
            'ignore this error for combo boxes parse
        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, ComboBox).Name & " Proposed: " & CStr(TryCast(e.Value, String)) & " Original: " & eOriginal & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        End Try
    End Sub

    Private Sub ComboBoxText_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Dim eOriginal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso e.Value.ToString.Trim.Length = 0 Then
                e.Value = DBNull.Value
            End If
        Catch ex As InvalidCastException
            'ignore this error for combo boxes parse
        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, ComboBox).Name & " Proposed: " & CStr(TryCast(e.Value, String)) & " Original: " & eOriginal & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        End Try
    End Sub

    Private Sub AgeAtDOSLabelBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Try
            If Not IsDBNull(e.Value) AndAlso CStr(e.Value).Trim.Length > 0 AndAlso _MedHdrDr IsNot Nothing AndAlso Not IsDBNull(_MedHdrDr("PAT_DOB")) Then
                e.Value = DateDiff(DateInterval.Month, CDate(_MedHdrDr("PAT_DOB")), CDate(e.Value)) \ 12 & " Y " & DateDiff(DateInterval.Month, CDate(_MedHdrDr("PAT_DOB")), CDate(e.Value)) Mod 12 & " M"
            Else
                e.Value = ""
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub UCaseBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Converts the value to upper case
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	9/8/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim eOriginal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso e.Value IsNot Nothing AndAlso e.Value.ToString.Trim.Length > 0 AndAlso CStr(e.Value) <> CStr(e.Value).ToUpper Then
                e.Value = CStr(e.Value).ToUpper
#If TRACE Then
                If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, TextBox).Name & " Proposed: " & CStr(TryCast(e.Value, String)) & " Original: " & eOriginal & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub UCaseBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	9/8/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim eOriginal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso CStr(e.Value).Trim.Length > 0 AndAlso e.Value.ToString.ToUpper <> CStr(e.Value).ToUpper Then
                e.Value = CStr(e.Value).ToUpper
            Else
                e.Value = Nothing
            End If

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, TextBox).Name & " Proposed: " & CStr(TryCast(e.Value, String)) & " Original: " & eOriginal & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        End Try
    End Sub

    Private Sub ZipBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Dim eOriginal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso e.Value IsNot Nothing AndAlso e.Value.ToString.Trim.Length > 0 AndAlso CStr(e.Value) <> String.Format("{0:00000}", e.Value) Then
                e.Value = String.Format("{0:00000}", e.Value)
#If TRACE Then
                If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, TextBox).Name & " Proposed: " & CStr(TryCast(e.Value, String)) & " Original: " & eOriginal & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub ZipBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)

        Dim eOriginal As String = e.Value.ToString

        Try
            If e.Value.ToString.Trim.Length < 1 Then
                e.Value = System.DBNull.Value
            Else
                e.Value = String.Format("{0:00000}", e.Value)
            End If

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, TextBox).Name & " Proposed: " & CStr(TryCast(e.Value, String)) & " Original: " & eOriginal & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        End Try
    End Sub

    Private Sub ProviderRenderingNPIBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Dim eOriginal As String = e.Value.ToString

        Try
            If Not IsDBNull(e.Value) AndAlso e.Value IsNot Nothing AndAlso e.Value.ToString.Trim.Length > 0 Then
                e.Value = String.Format("{0:0000000000}", e.Value)
            End If

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, TextBox).Name & " Proposed: " & CStr(TryCast(e.Value, String)) & " Original: " & eOriginal & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        End Try
    End Sub

    Private Sub ProviderRenderingNPIBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)
        Dim eOriginal As String = e.Value.ToString

        Try
            If e.Value.ToString.Trim.Length < 1 Then
                e.Value = System.DBNull.Value
            Else
                e.Value = String.Format("{0:0000000000}", e.Value)
            End If

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, TextBox).Name & " Proposed: " & CStr(TryCast(e.Value, String)) & " Original: " & eOriginal & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        End Try
    End Sub

    Private Sub ComboBoxBinding_Format(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)

        Dim eOriginal As Object = e.Value

        Try
            If eOriginal Is Nothing Then Exit Sub

            Select Case CType(CType(sender, Binding).Control, ComboBox).Name
                Case "cmbHosp"
                    Select Case e.Value.ToString.Trim.ToUpper
                        Case "I"
                            e.Value = "I - INPATIENT"
                        Case "O"
                            e.Value = "O - OUTPATIENT"
                        Case Else
                            e.Value = Nothing
                    End Select

            End Select

            If Not IsDBNull(e.Value) AndAlso e.Value IsNot Nothing AndAlso e.Value.ToString.Trim.Length > 0 Then
                e.Value = CStr(e.Value).ToUpper
            End If

        Catch ex As Exception
            Throw

        Finally
#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, ComboBox).Name & " Proposed: " & CStr(e.Value) & " Original: " & eOriginal.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        End Try
    End Sub

    Private Sub ComboBoxBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)

        Dim eOriginal As Object = e.Value

        Try
            If eOriginal Is Nothing Then Exit Sub

            If e.Value.ToString.Trim.Length > 0 Then
                Select Case CType(CType(sender, Binding).Control, ComboBox).Name
                    Case "cmbHosp"
                        e.Value = e.Value.ToString.Substring(0, 1)
                End Select
            End If

            If Not IsDBNull(e.Value) AndAlso e.Value.ToString.Trim.Length = 0 Then
                e.Value = DBNull.Value
            End If

        Catch ex As InvalidCastException
            'ignore this error for combo boxes parse
        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, ComboBox).Name & " Proposed: " & CStr(TryCast(e.Value, String)) & " Original: " & eOriginal.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        End Try
    End Sub

    Private Sub NullBinding_Parse(ByVal sender As Object, ByVal e As System.Windows.Forms.ConvertEventArgs)

        Dim eOriginal As String = e.Value.ToString

        Try
            If eOriginal Is Nothing Then Exit Sub

            If e.Value.ToString.Trim.Length < 1 Then
                e.Value = System.DBNull.Value
            End If

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(CType(sender, Binding).BindableComponent, TextBox).Name & " Proposed: " & CStr(TryCast(e.Value, String)) & " Original: " & eOriginal & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        End Try
    End Sub

    Private Sub BindingComboCompleteEventHandler(ByVal sender As Object, ByVal e As System.Windows.Forms.BindingCompleteEventArgs)

        Try

            If Not e.BindingCompleteState = BindingCompleteState.Success Then
                MessageBox.Show("Control " & e.Binding.Control.Name & " " & e.ErrorText, "Problem converting data to database format", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Private Function BindingEqual(binding As Binding, ByVal newValue As String, ByVal fieldName As String) As Boolean

        Dim Equal As Boolean = False
        Try

            If CType(DirectCast(binding.BindingManagerBase, System.Windows.Forms.CurrencyManager).Current, DataRowView)(binding.BindingMemberInfo.BindingMember).ToString = newValue Then
                Return True
            End If

            'Dim BMB As BindingManagerBase = Me.BindingContext(binding.DataSource, binding.BindingMemberInfo.BindingField)

            'If BMB.Current Is Nothing AndAlso newValue Is Nothing Then
            '    Equal = True
            'ElseIf (IsDBNull(BMB.Current) OrElse BMB.Current Is Nothing) AndAlso newValue.Length > 0 Then
            '    Equal = False
            'ElseIf Not IsDBNull(BMB.Current) AndAlso BMB.Current.ToString.Length > 0 AndAlso newValue.Length = 0 Then
            '    Equal = False
            'ElseIf (IsDBNull(BMB.Current) OrElse BMB.Current Is Nothing) AndAlso (newValue Is Nothing OrElse IsDBNull(newValue)) Then
            '    Equal = True
            'ElseIf (IsDBNull(BMB.Current) OrElse BMB.Current Is Nothing) AndAlso newValue.Length = 0 Then
            '    Equal = True
            'ElseIf BMB.Current.Equals(CTypeDynamic(newValue, BMB.Current.GetType)) Then
            '    Equal = True
            'End If

#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & "Validated as Equal: " & Equal.ToString & " " & fieldName & " Proposed: " & newValue & " Original: " & Me.BindingContext(binding.DataSource, binding.BindingMemberInfo.BindingField).Current.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

            'Return Equal

        Catch ex As Exception

#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & "Failed Validated but returned as Equal: " & Equal.ToString & " " & fieldName & " Proposed: " & newValue & " Original: " & Me.BindingContext(binding.DataSource, binding.BindingMemberInfo.BindingField).Current.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

            Return True 'the assumption is that the value failed the attempt to be type casted, therefore could not be written safely back to the datasource
        End Try

    End Function

    Private Sub LoadHeaderDataBindings()

        Dim Bind As Binding

        Try
            _ClaimMasterBS.SuspendBinding()
            _MedHdrBS.SuspendBinding()

            txtFamilyID.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "FAMILY_ID")
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtFamilyID.DataBindings.Add(Bind)

            txtPartSSN.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PART_SSN")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never
            AddHandler Bind.Format, AddressOf SSNBinding_Format
            AddHandler Bind.Parse, AddressOf SSNBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtPartSSN.DataBindings.Add(Bind)

            txtPartNameFirst.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PART_FNAME")
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtPartNameFirst.DataBindings.Add(Bind)

            txtPartNameMiddle.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PART_INT")
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtPartNameMiddle.DataBindings.Add(Bind)

            txtPartNameLast.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PART_LNAME")
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtPartNameLast.DataBindings.Add(Bind)

            txtPatRelationID.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "RELATION_ID", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtPatRelationID.DataBindings.Add(Bind)

            txtPatSSN.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PAT_SSN", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.Format, AddressOf SSNBinding_Format
            AddHandler Bind.Parse, AddressOf SSNBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtPatSSN.DataBindings.Add(Bind)

            txtPatNameFirst.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PAT_FNAME", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtPatNameFirst.DataBindings.Add(Bind)

            txtPatNameMiddle.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PAT_INT", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtPatNameMiddle.DataBindings.Add(Bind)

            txtPatNameLast.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PAT_LNAME", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtPatNameLast.DataBindings.Add(Bind)

            txtReceivedDate.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "REC_DATE")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never 'ReadOnly
            AddHandler Bind.Format, AddressOf UFCWGeneral.DateOnlyBinding_Format
            txtReceivedDate.DataBindings.Add(Bind)

            DocTypeComboBox.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _ClaimMasterBS, "DOC_TYPE")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            DocTypeComboBox.DataBindings.Add(Bind)

            DuplicateCheckBox.DataBindings.Clear()
            Bind = New Binding("Checked", _ClaimMasterBS, "DUPLICATE_SW")
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            DuplicateCheckBox.DataBindings.Add(Bind)

            txtOpenDate.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "OPEN_DATE")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never 'Readonly
            AddHandler Bind.Format, AddressOf UFCWGeneral.DateOnlyBinding_Format
            txtOpenDate.DataBindings.Add(Bind)

            txtClaimID.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "CLAIM_ID")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never
            txtClaimID.DataBindings.Add(Bind)

            txtDocID.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "DOCID")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never
            txtDocID.DataBindings.Add(Bind)

            txtMaxID.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "MAXID")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtMaxID.DataBindings.Add(Bind)

            txtPageCount.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PAGE_COUNT")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtPageCount.DataBindings.Add(Bind)

            txtBatchNum.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "BATCH")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtBatchNum.DataBindings.Add(Bind)

            txtPriority.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PRIORITY")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtPriority.DataBindings.Add(Bind)

            txtReferenceClaim.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "REFRENCE_CLAIM")
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtReferenceClaim.DataBindings.Add(Bind)

            txtReferenceID.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "REFERENCE_ID")
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtReferenceID.DataBindings.Add(Bind)

            txtPatAcctNo.DataBindings.Clear()
            Bind = New Binding("Text", _MedHdrBS, "PAT_ACCT_NBR")
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtPatAcctNo.DataBindings.Add(Bind)

            txtIncidentDate.DataBindings.Clear()
            Bind = New Binding("Text", _MedHdrBS, "INCIDENT_DATE", True, DataSourceUpdateMode.OnPropertyChanged)
            Bind.FormatString = "MM-dd-yyyy"
            AddHandler Bind.Parse, AddressOf UFCWGeneral.DateOnlyBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtIncidentDate.DataBindings.Add(Bind)

            AutoCheckBox.DataBindings.Clear()
            Bind = New Binding("Checked", _MedHdrBS, "AUTO_ACCIDENT_SW", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            AutoCheckBox.DataBindings.Add(Bind)

            WorkersCompCheckBox.DataBindings.Clear()
            Bind = New Binding("Checked", _MedHdrBS, "WORKERS_COMP_SW", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            WorkersCompCheckBox.DataBindings.Add(Bind)

            OtherCheckBox.DataBindings.Clear()
            Bind = New Binding("Checked", _MedHdrBS, "OTH_ACCIDENT_SW", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            OtherCheckBox.DataBindings.Add(Bind)

            AuthCheckBox.DataBindings.Clear()
            Bind = New Binding("Checked", _MedHdrBS, "AUTHORIZED_SW", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            AuthCheckBox.DataBindings.Add(Bind)

            ChiroCheckBox.DataBindings.Clear()
            Bind = New Binding("Checked", _MedHdrBS, "CHIRO_SW", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            ChiroCheckBox.DataBindings.Add(Bind)

            OICheckBox.DataBindings.Clear()
            Bind = New Binding("Checked", _MedHdrBS, "OTH_INS_SW", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            OICheckBox.DataBindings.Add(Bind)

            NonPARCheckBox.DataBindings.Clear()
            Bind = New Binding("Checked", _MedHdrBS, "NON_PAR_SW", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            NonPARCheckBox.DataBindings.Add(Bind)

            OOACheckBox.DataBindings.Clear()
            Bind = New Binding("Checked", _MedHdrBS, "OUT_OF_AREA_SW", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            OOACheckBox.DataBindings.Add(Bind)

            cmbHosp.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _MedHdrBS, "ADMITTANCE", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.Format, AddressOf ComboBoxBinding_Format
            AddHandler Bind.Parse, AddressOf ComboBoxBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            cmbHosp.DataBindings.Add(Bind)

            cmbCOB.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _MedHdrBS, "COB")
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            cmbCOB.DataBindings.Add(Bind)

            cmbPricingNetwork.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _MedHdrBS, "PPO")
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            cmbPricingNetwork.DataBindings.Add(Bind)

            cmbPayee.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _MedHdrBS, "PAYEE")
            cmbPayee.DataBindings.Add(Bind)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler

            txtPatBirthDate.DataBindings.Clear()
            Bind = New Binding("Text", _MedHdrBS, "PAT_DOB")
            AddHandler Bind.Format, AddressOf UFCWGeneral.DateOnlyBinding_Format
            AddHandler Bind.Parse, AddressOf UFCWGeneral.DateOnlyBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtPatBirthDate.DataBindings.Add(Bind)

            txtPatGender.DataBindings.Clear()
            Bind = New Binding("Text", _MedHdrBS, "PAT_SEX")
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtPatGender.DataBindings.Add(Bind)

            txtProviderID.DataBindings.Clear()
            Bind = New Binding("Text", _MedHdrBS, "PROV_TIN")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged
            AddHandler Bind.Format, AddressOf TINBinding_Format
            AddHandler Bind.Parse, AddressOf TINBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtProviderID.DataBindings.Add(Bind)

            txtProviderLicenseNo.DataBindings.Clear()
            Bind = New Binding("Text", _MedHdrBS, "PROV_LICENSE")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged
            AddHandler Bind.Format, AddressOf UCaseBinding_Format
            AddHandler Bind.Parse, AddressOf UCaseBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtProviderLicenseNo.DataBindings.Add(Bind)

            txtProviderRenderingNPI.DataBindings.Clear()
            Bind = New Binding("Text", _MedHdrBS, "RENDERING_NPI")
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            AddHandler Bind.Parse, AddressOf ProviderRenderingNPIBinding_Parse
            AddHandler Bind.Format, AddressOf ProviderRenderingNPIBinding_Format
            txtProviderRenderingNPI.DataBindings.Add(Bind)

            txtBCCZIP.DataBindings.Clear()
            Bind = New Binding("Text", _MedHdrBS, "PROV_ZIP")
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            AddHandler Bind.Parse, AddressOf ZipBinding_Parse
            AddHandler Bind.Format, AddressOf ZipBinding_Format
            txtBCCZIP.DataBindings.Add(Bind)
            ''---Totals---
            txtTotalChargedAmt.DataBindings.Clear()
            Bind = New Binding("Text", _MedHdrBS, "TOT_CHRG_AMT", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            AddHandler Bind.Format, AddressOf UFCWGeneral.MoneyBinding_Format
            AddHandler Bind.Parse, AddressOf UFCWGeneral.MoneyBinding_Parse
            txtTotalChargedAmt.DataBindings.Add(Bind)

            txtTotalAllowedAmt.DataBindings.Clear()
            Bind = New Binding("Text", _MedHdrBS, "TOT_ALLOWED_AMT", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            AddHandler Bind.Format, AddressOf UFCWGeneral.MoneyBinding_Format
            AddHandler Bind.Parse, AddressOf UFCWGeneral.MoneyBinding_Parse
            txtTotalAllowedAmt.DataBindings.Add(Bind)

            txtTotalOIAmt.DataBindings.Clear()
            Bind = New Binding("Text", _MedHdrBS, "TOT_OTH_INS_AMT", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            AddHandler Bind.Format, AddressOf UFCWGeneral.MoneyBinding_Format
            AddHandler Bind.Parse, AddressOf UFCWGeneral.MoneyBinding_Parse
            txtTotalOIAmt.DataBindings.Add(Bind)

            txtTotalPaidAmt.DataBindings.Clear()
            Bind = New Binding("Text", _MedHdrBS, "TOT_PAID_AMT", True, DataSourceUpdateMode.OnPropertyChanged)
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            AddHandler Bind.Format, AddressOf UFCWGeneral.MoneyBinding_Format
            AddHandler Bind.Parse, AddressOf UFCWGeneral.MoneyBinding_Parse
            txtTotalPaidAmt.DataBindings.Add(Bind)

            'free text
            WorkFreeTextEditor.NotesFamilyIDTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "FAMILY_ID")
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            WorkFreeTextEditor.NotesFamilyIDTextBox.DataBindings.Add(Bind)

            WorkFreeTextEditor.NotesPartSSNTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PART_SSN")
            AddHandler Bind.Format, AddressOf SSNBinding_Format
            AddHandler Bind.Parse, AddressOf SSNBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            WorkFreeTextEditor.NotesPartSSNTextBox.DataBindings.Add(Bind)

            WorkFreeTextEditor.NotesRelationIDTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "RELATION_ID")
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            WorkFreeTextEditor.NotesRelationIDTextBox.DataBindings.Add(Bind)

            WorkFreeTextEditor.NotesPatSSNTextBox.DataBindings.Clear()
            Bind = New Binding("Text", _ClaimMasterBS, "PAT_SSN")
            AddHandler Bind.Format, AddressOf SSNBinding_Format
            AddHandler Bind.Parse, AddressOf SSNBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            WorkFreeTextEditor.NotesPatSSNTextBox.DataBindings.Add(Bind)

            _ClaimMasterBS.ResumeBinding()
            _MedHdrBS.ResumeBinding()

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

    Private Sub LoadDetailLineDataBindings()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Adds data bindings to handle display of each detail line as the line is selected
        ' in the grid
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	5/4/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Dim Bind As Binding

        Try

            _MedDtlBS.SuspendBinding()

            txtProcedure.DataBindings.Clear()
            Bind = New Binding("Text", _MedDtlBS, "PROC_CODE")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            txtProcedure.DataBindings.Add(Bind)

            cmbPlan.DataBindings.Clear()
            Bind = New Binding("SelectedValue", _MedDtlBS, "MED_PLAN")
            Bind = New Binding("Text", _MedDtlBS, "MED_PLAN")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.BindingComplete, AddressOf BindingComboCompleteEventHandler
            cmbPlan.DataBindings.Add(Bind)

            txtDiagnoses.DataBindings.Clear()
            Bind = New Binding("Text", _MedDtlBS, "DIAGNOSES", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            txtDiagnoses.DataBindings.Add(Bind)

            txtModifiers.DataBindings.Clear()
            Bind = New Binding("Text", _MedDtlBS, "MODIFIERS")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            txtModifiers.DataBindings.Add(Bind)

            ' txtReasons.DataBindings.Clear()
            'Bind = New Binding("Text", _MedDtlBS, "REASONS")
            'Bind.DataSourceUpdateMode = DataSourceUpdateMode.Never
            'AddHandler Bind.Parse, AddressOf ReasonsBinding_Parse
            'AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            'txtReasons.DataBindings.Add(Bind)

            txtPlaceOfService.DataBindings.Clear()
            Bind = New Binding("Text", _MedDtlBS, "PLACE_OF_SERV")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.Parse, AddressOf GenericBinding_Parse
            AddHandler Bind.BindingComplete, AddressOf BindingCompleteEventHandler
            txtPlaceOfService.DataBindings.Add(Bind)

            txtBillType.DataBindings.Clear()
            Bind = New Binding("Text", _MedDtlBS, "BILL_TYPE", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            txtBillType.DataBindings.Add(Bind)

            txtFromDate.DataBindings.Clear()
            Bind = New Binding("Text", _MedDtlBS, "OCC_FROM_DATE")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.Format, AddressOf UFCWGeneral.DateOnlyBinding_Format
            AddHandler Bind.Parse, AddressOf UFCWGeneral.DateOnlyBinding_Parse
            txtFromDate.DataBindings.Add(Bind)

            txtToDate.DataBindings.Clear()
            Bind = New Binding("Text", _MedDtlBS, "OCC_TO_DATE")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.Format, AddressOf UFCWGeneral.DateOnlyBinding_Format
            AddHandler Bind.Parse, AddressOf UFCWGeneral.DateOnlyBinding_Parse
            txtToDate.DataBindings.Add(Bind)

            txtChargeAmt.DataBindings.Clear()
            Bind = New Binding("Text", _MedDtlBS, "CHRG_AMT")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.Parse, AddressOf UFCWGeneral.MoneyBinding_Parse
            AddHandler Bind.Format, AddressOf UFCWGeneral.MoneyBinding_Format
            txtChargeAmt.DataBindings.Add(Bind)

            txtPricedAmt.DataBindings.Clear()
            Bind = New Binding("Text", _MedDtlBS, "PRICED_AMT")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.Parse, AddressOf UFCWGeneral.MoneyBinding_Parse
            AddHandler Bind.Format, AddressOf UFCWGeneral.MoneyBinding_Format
            txtPricedAmt.DataBindings.Add(Bind)

            txtPaidAmt.DataBindings.Clear()
            Bind = New Binding("Text", _MedDtlBS, "PAID_AMT")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.Parse, AddressOf UFCWGeneral.MoneyBinding_Parse
            AddHandler Bind.Format, AddressOf UFCWGeneral.MoneyBinding_Format
            txtPaidAmt.DataBindings.Add(Bind)

            txtOIAmt.DataBindings.Clear()
            Bind = New Binding("Text", _MedDtlBS, "OTH_INS_AMT")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
            AddHandler Bind.Parse, AddressOf UFCWGeneral.MoneyBinding_Parse
            AddHandler Bind.Format, AddressOf UFCWGeneral.MoneyBinding_Format
            txtOIAmt.DataBindings.Add(Bind)

            txtUnits.DataBindings.Clear()
            Bind = New Binding("Text", _MedDtlBS, "DAYS_UNITS")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged
            txtUnits.DataBindings.Add(Bind)

            txtNDC.DataBindings.Clear()
            Bind = New Binding("Text", _MedDtlBS, "NDC")
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged
            txtNDC.DataBindings.Add(Bind)


            cmbStatus.DataBindings.Clear()
            Bind = New Binding("Text", _MedDtlBS, "STATUS") 'No associated datasource, embedded collection in use
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged
            cmbStatus.DataBindings.Add(Bind)


            AgeAtDOSLabel.DataBindings.Clear()
            Bind = New Binding("Text", _MedDtlBS, "OCC_FROM_DATE", True)
            Bind.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged
            AddHandler Bind.Format, AddressOf AgeAtDOSLabelBinding_Format
            AgeAtDOSLabel.DataBindings.Add(Bind)

            _MedDtlBS.ResumeBinding()

        Catch ex As Exception
            Throw
        Finally
            _ChangingRows = False
        End Try
    End Sub

    Private Sub GenericBinding_Parse(sender As Object, e As ConvertEventArgs)
        Dim DR As DataRow

        Try

            If _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = DirectCast(_MedDtlBS.Current, DataRowView).Row

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub ReasonsBinding_Parse(sender As Object, e As ConvertEventArgs)
        ' if binding uses propertychanged associated updates should happen here, else in the validating event
        ' Maybe it should always happen here??
        'why does this fire when ecancel is called
        'Dim BS As BindingSource
        Dim DR As DataRow
        Dim RaiseListChangedEventsSaved As Boolean

        Try

            If _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            RaiseListChangedEventsSaved = _MedDtlBS.RaiseListChangedEvents
            DR = DirectCast(_MedDtlBS.Current, DataRowView).Row

            Dim NoCommaText As String = e.Value.ToString.ToUpper '.Replace(",", " ")
            Dim Reasons2Add As String() = NoCommaText.Split(New Char() {CChar(",")}, StringSplitOptions.RemoveEmptyEntries)

            _MedDtlBS.RaiseListChangedEvents = False

            ApplyLineReasons(CShort(DR("LINE_NBR")), Reasons2Add)

            _MedDtlBS.RaiseListChangedEvents = RaiseListChangedEventsSaved

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    '    Private Sub ClearDetailLineDatabindings()
    '        ' -----------------------------------------------------------------------------
    '        ' <summary>
    '        ' clears databindings for the detail
    '        ' </summary>
    '        ' <remarks>
    '        ' </remarks>
    '        ' <history>
    '        ' 	[nick snyder]	8/16/2006	Created
    '        ' </history>
    '        ' -----------------------------------------------------------------------------
    '        Try

    '#If TRACE Then
    '            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
    '#End If

    '            txtProcedure.DataBindings.Clear()
    '            cmbPlan.DataBindings.Clear()
    '            txtDiagnoses.DataBindings.Clear()
    '            txtModifiers.DataBindings.Clear()
    '            txtPlaceOfService.DataBindings.Clear()
    '            txtBillType.DataBindings.Clear()
    '            txtFromDate.DataBindings.Clear()
    '            txtToDate.DataBindings.Clear()
    '            txtChargeAmt.DataBindings.Clear()
    '            txtPricedAmt.DataBindings.Clear()
    '            txtPaidAmt.DataBindings.Clear()
    '            txtOIAmt.DataBindings.Clear()
    '            txtUnits.DataBindings.Clear()
    '            cmbStatus.DataBindings.Clear()

    '        Catch ex As Exception
    '            Throw
    '        End Try
    '    End Sub

#End Region

#Region "Header Form Events"

    Private Sub cmbPayee_Validating(sender As Object, e As CancelEventArgs) Handles cmbPayee.Validating

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim DR As DataRow

        Try

            ErrorProvider1.ClearError(CBox)

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse CBox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _MedDtlBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = CType(_MedDtlBS.Current, DataRowView).Row

            If CBox.SelectedIndex < 0 Then
                ErrorProvider1.SetErrorWithTracking(CBox, " Payee selection required.")
            End If

            If ErrorProvider1.GetError(CBox).Trim.Length > 0 Then 'are there any errors  
                e.Cancel = True
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _MedDtlBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        Finally

        End Try

    End Sub


    Private Sub cmbPayee_Validated(sender As Object, e As EventArgs) Handles cmbPayee.Validated
        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Try
            _ClaimAlertManager.DeleteAllAlertRowsLikeMessage("Paying Member")
            If IsNumeric(CBox.Text) AndAlso CBox.Text = "3" Then
                PayeeLabel.ForeColor = Color.Blue
                _ClaimAlertManager.AddAlertRow(New Object() {"Paying Member", 0, "Header", 20})
            Else
                PayeeLabel.ForeColor = Color.Black
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub txtIncidentDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtIncidentDate.Validating

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Dim Tbox As TextBox = CType(sender, TextBox)
        Dim DR As DataRow

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse Tbox.ReadOnly OrElse Tbox.Text.Trim.Length < 1 Then Return

            ErrorProvider1.ClearError(Tbox)

            Dim HoldDate As Date? = UFCWGeneral.ValidateDate(Tbox.Text)
            If HoldDate Is Nothing Then
                ErrorProvider1.SetErrorWithTracking(Tbox, " Date format must be mm/dd/yyyy or mm-dd-yyyy.")
            Else
                DR = DirectCast(_MedDtlBS.Current, DataRowView).Row

                If IsDate(txtToDate.Text) AndAlso HoldDate > CDate(txtToDate.Text) Then
                    ErrorProvider1.SetErrorWithTracking(Me.txtToDate, " Incident Date must be before To Date")
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
#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(sender, TextBox).Name & " Proposed: " & CType(sender, TextBox).Text & " Cancel: " & e.Cancel.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

    End Sub

    Private Sub BoundTextBoxHeader_Validated(sender As Object, e As System.EventArgs)

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Dim Binding As Binding

        Try

            Binding = CType(sender, TextBox).DataBindings("TEXT")
            If Binding IsNot Nothing AndAlso UpdateTextBinding(sender) Then
                Binding.WriteValue()
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub txtPatBirthDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtPatBirthDate.Validating

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Dim TBox As TextBox = CType(sender, TextBox)
        Dim DR As DataRow

        Dim HoldDate As Date?

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ErrorProvider1.ClearError(TBox)

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse TBox.ReadOnly Then Return

            If TBox.Text.Trim.Length < 1 Then

                SetErrorWithTracking(ErrorProvider1, TBox, "Birth date is required.")
            Else
                HoldDate = UFCWGeneral.ValidateDate(TBox.Text)
                If HoldDate Is Nothing Then

                    SetErrorWithTracking(ErrorProvider1, TBox, "Date format must be mm/dd/yyyy or mm-dd-yyyy.")

                Else

                    If HoldDate > Now.Date Then
                        SetErrorWithTracking(ErrorProvider1, TBox, "Birth date cannot be future date.")
                    End If

                    If txtFromDate.TextLength > 0 AndAlso IsDate(txtFromDate.Text) AndAlso HoldDate > CDate(txtFromDate.Text) Then
                        SetErrorWithTracking(ErrorProvider1, TBox, " Birth Date must be on or before From Date")
                    End If

                    DR = _ClaimDS.Tables("REG_MASTER").Select("RELATION_ID =" & 1)(0)
                    If DR IsNot Nothing AndAlso IsDate(DR("BIRTH_DATE")) Then
                        If HoldDate <> CDate(DR("BIRTH_DATE")) Then
                            SetErrorWithTracking(ErrorProvider1, TBox, " Birth Date does not match RegM")
                        End If
                    End If

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
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(sender, TextBox).Name & " Proposed: " & CType(sender, TextBox).Text & " Cancel: " & e.Cancel.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

    End Sub

#End Region

#Region "Detail Form Events"

    Private Sub txtFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtFromDate.Validating

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Dim Tbox As TextBox = CType(sender, TextBox)
        Dim DGRow As DataRow

        Try

            ErrorProvider1.ClearError(Tbox)

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse Tbox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & DGRow("LINE_NBR").ToString & ": Invalid From Date", CInt(DGRow("LINE_NBR")))
            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & DGRow("LINE_NBR").ToString & ": Missing From Date", CInt(DGRow("LINE_NBR")))

            If Tbox.Text.Trim.Length < 1 Then
                ErrorProvider1.SetErrorWithTracking(Tbox, " From Date is required.")
            Else
                Dim HoldDate As Date? = UFCWGeneral.ValidateDate(Tbox.Text)
                If HoldDate Is Nothing Then
                    ErrorProvider1.SetErrorWithTracking(Tbox, " Date format must be mm/dd/yyyy or mm-dd-yyyy.")
                Else

                    If IsDate(txtToDate.Text) AndAlso CDate(txtToDate.Text) < HoldDate Then
                        ErrorProvider1.SetErrorWithTracking(Me.txtToDate, " From Date must be before To Date")
                    End If

                    If Tbox.Text <> CDate(HoldDate).ToString("MM-dd-yyyy") Then
                        Tbox.Text = CDate(HoldDate).ToString("MM-dd-yyyy")
                    End If

                End If
            End If

            If ErrorProvider1.GetError(Tbox).Trim.Length > 0 Then 'are there any errors 
                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DGRow("LINE_NBR").ToString & ": " & If(Tbox.Text.Trim.Length > 0, "Invalid", "Missing") & " From Date", DGRow("LINE_NBR").ToString, "Detail", 20, New Object() {ClaimTabPage, DetailLinesDataGrid, Tbox.DataBindings("TEXT").DataSource, Tbox}})
                e.Cancel = True
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(sender, TextBox).Name & " Proposed: " & CType(sender, TextBox).Text & " Cancel: " & e.Cancel.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

    End Sub

    Private Sub txtFromDate_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.Validated

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Dim Binding As Binding

        Dim DGRow As DataRow
        Dim Tbox As TextBox = CType(sender, TextBox)

        Try
            _MedDtlBS = DirectCast(DetailLinesDataGrid.DataSource, BindingSource)

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse Tbox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

            'Binding = Tbox.DataBindings("TEXT")
            'If Tbox.Text.Trim.Length > 1 OrElse UFCWGeneral.ValidateDate(Tbox.Text) IsNot Nothing Then
            '    ErrorProvider1.ClearError(Tbox)
            'End If
            If DGRow Is Nothing Then Exit Sub

            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & DGRow("LINE_NBR").ToString & ": Missing From Date", CInt(DGRow("LINE_NBR")))
            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & DGRow("LINE_NBR").ToString & ": Was Received 1 Year After DOS", CInt(DGRow("LINE_NBR")))

            If Not IsDate(CType(sender, TextBox).Text) Then
                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DGRow("LINE_NBR").ToString & ": Missing From Date", DGRow("LINE_NBR").ToString, "Detail", 20, New Object() {ClaimTabPage, DetailLinesDataGrid, Binding.DataSource, CType(sender, TextBox)}})
            ElseIf Not IsDBNull(_ClaimDr("REC_DATE")) AndAlso CDate(CType(sender, TextBox).Text) < DateAdd(DateInterval.Year, -1, CDate(_ClaimDr("REC_DATE"))) Then
                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DGRow("LINE_NBR").ToString & ": Was Received 1 Year After DOS", DGRow("LINE_NBR").ToString, "Detail", 15})
            End If
            '  If Binding IsNot Nothing AndAlso UpdateTextBinding(sender) Then

            'DGRow(Binding.BindingMemberInfo.BindingField) = UFCWGeneral.ToNullDateHandler(CType(sender, TextBox).Text)

            ''Validate From date based on the Status

            'reevaluate diagnosis preventative status based upon new from date

            'If IsDate(CType(sender, TextBox).Text) Then
            If IsDate(txtFromDate.Text) Then
                If IsDate(txtToDate.Text) Then
                    If CDate(txtFromDate.Text) > CDate(txtToDate.Text) Then
                        txtToDate.Text = txtFromDate.Text
                    End If
                Else
                    txtToDate.Text = txtFromDate.Text
                End If

                Dim DiagDV As New DataView(_ClaimDS.MEDDIAG, "LINE_NBR = " & DGRow("LINE_NBR").ToString, "", DataViewRowState.CurrentRows)

                For Each DRV As DataRowView In DiagDV
                    Dim DR As DataRow = CMSDALFDBMD.RetrieveDiagnosisPreventativeStatusEffectiveAsOf(CStr(DRV("DIAGNOSIS")), CDate(CType(sender, TextBox).Text))
                    If DR IsNot Nothing AndAlso CBool(DRV("PREVENTATIVE_USE_SW")) <> CBool(DR("PREVENTATIVE_USE_SW")) Then
                        DRV.Row("PREVENTATIVE_USE_SW") = DR("PREVENTATIVE_USE_SW")
                    End If
                Next

                LoadDetailLineEligibility(DGRow, False)

                SumPaid()
                SyncAllowed()
                SumAllowed()
                SumOI()
            End If
            '  End If
            'If Not IsDBNull(DGRow("DIAGNOSIS")) Then
            '    Dim UpdatePreventativeQuery = _ClaimDS.MEDDIAG.AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = CInt(DGRow("LINE_NBR")))

            '    For Each MedDiagDR As DataRow In UpdatePreventativeQuery.ToList()
            '        Dim DR As DataRow = CMSDALFDBMD.RetrieveDiagnosisPreventativeStatusEffectiveAsOf(CStr(MedDiagDR("DIAGNOSIS")), CDate(Tbox.Text))
            '        If DR IsNot Nothing AndAlso CBool(MedDiagDR("PREVENTATIVE_USE_SW")) <> CBool(DR("PREVENTATIVE_USE_SW")) Then
            '            MedDiagDR("PREVENTATIVE_USE_SW") = DR("PREVENTATIVE_USE_SW")
            '            MedDiagDR.EndEdit()
            '        End If
            '    Next

            'End If


            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub txtToDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtToDate.Validating

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Dim Tbox As TextBox = CType(sender, TextBox)
        Dim DGRow As DataRow

        Try

            ErrorProvider1.ClearError(Tbox)

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse Tbox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & DGRow("LINE_NBR").ToString & ": Invalid To Date", CInt(DGRow("LINE_NBR")))
            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & DGRow("LINE_NBR").ToString & ": Missing To Date", CInt(DGRow("LINE_NBR")))

            If Tbox.Text.Trim.Length < 1 Then
                If IsDate(txtFromDate.Text) Then
                    txtToDate.Text = txtFromDate.Text
                Else
                    ErrorProvider1.SetErrorWithTracking(Tbox, " To Date is required.")
                End If
            Else
                Dim HoldDate As Date? = UFCWGeneral.ValidateDate(Tbox.Text)
                If HoldDate Is Nothing Then
                    ErrorProvider1.SetErrorWithTracking(Tbox, " Date format must be mm/dd/yyyy or mm-dd-yyyy.")
                Else
                    DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

                    If IsDate(txtFromDate.Text) AndAlso CDate(txtFromDate.Text) > HoldDate Then
                        ErrorProvider1.SetErrorWithTracking(Me.txtToDate, " To Date must be on or after From Date")
                    End If

                    If Tbox.Text <> CDate(HoldDate).ToString("MM-dd-yyyy") Then
                        Tbox.Text = CDate(HoldDate).ToString("MM-dd-yyyy")
                    End If

                End If
            End If

            If ErrorProvider1.GetError(Tbox).Trim.Length > 0 Then 'are there any errors 
                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DGRow("LINE_NBR").ToString & ": " & If(Tbox.Text.Trim.Length > 0, "Invalid", "Missing") & " To Date", DGRow("LINE_NBR").ToString, "Detail", 20, New Object() {ClaimTabPage, DetailLinesDataGrid, Tbox.DataBindings("TEXT").DataSource, Tbox}})
                e.Cancel = True
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(sender, TextBox).Name & " Proposed: " & CType(sender, TextBox).Text & " Cancel: " & e.Cancel.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

    End Sub
    Private Sub BoundTextBoxDetail_Validated(sender As Object, e As System.EventArgs) Handles txtOIAmt.Validated, txtToDate.Validated, txtUnits.Validated

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame().GetMethod.ToString & " Control: " & CType(sender, TextBox).Name & " ValidateChildren: " & _ValidateChildren.ToString & " ValidateCancelled: " & _ValidateCancelled.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        Dim Binding As Binding
        Dim DGRow As DataRow
        Try

            _MedDtlBS = DirectCast(DetailLinesDataGrid.DataSource, BindingSource)

            If _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse _MedDtlBS.Count = 0 Then Return

            DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

            If DGRow Is Nothing Then Return

            Binding = CType(sender, TextBox).DataBindings("TEXT")

            If Not IsDate(txtToDate.Text) AndAlso Binding.DataSource.ToString.ToUpper.Contains("MEDDTL") Then
                If Not IsDate(txtFromDate.Text) Then
                    _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & DGRow("LINE_NBR").ToString & ": Missing From Date", CInt(DGRow("LINE_NBR")))
                    _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DGRow("LINE_NBR").ToString & ": Missing From Date", DGRow("LINE_NBR").ToString, "Detail", 20, New Object() {ClaimTabPage, DetailLinesDataGrid, Binding.DataSource, CType(sender, TextBox)}})
                Else
                    txtToDate.Text = txtFromDate.Text
                End If
            End If

            If Binding IsNot Nothing AndAlso UpdateTextBinding(sender) Then

                Select Case DGRow(Binding.BindingMemberInfo.BindingField).GetType
                    Case System.Type.GetType("System.Integer")
                        DGRow(Binding.BindingMemberInfo.BindingField) = UFCWGeneral.ToNullIntegerHandler(CType(sender, TextBox).Text)
                    Case System.Type.GetType("System.Decimal")
                        DGRow(Binding.BindingMemberInfo.BindingField) = UFCWGeneral.ToNullDecimalHandler(CType(sender, TextBox).Text)
                    Case System.Type.GetType("System.Date")
                        DGRow(Binding.BindingMemberInfo.BindingField) = UFCWGeneral.ToNullDateHandler(CType(sender, TextBox).Text)
                    Case Else

                        DGRow(Binding.BindingMemberInfo.BindingField) = CType(sender, TextBox).Text
                End Select

                SumOI()

            End If

            _MedDtlBS.EndEdit()


        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub PaidTextBox_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPaidAmt.Validated
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' forces changes to be updated to the dataset
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[nick snyder]	8/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame().GetMethod.ToString & " Control: " & CType(sender, TextBox).Name & " ValidateChildren: " & _ValidateChildren.ToString & " ValidateCancelled: " & _ValidateCancelled.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        'Dim Binding As Binding
        Dim BS As BindingSource

        Dim DGRow As DataRow

        If DetailLinesDataGrid.GetGridRowCount = 0 Then Return

        Try

            BS = DirectCast(DetailLinesDataGrid.DataSource, BindingSource)
            DGRow = DirectCast(BS.Current, DataRowView).Row
            If DGRow Is Nothing Then Return
            '   Binding = CType(sender, TextBox).DataBindings("TEXT")

            '     If Binding IsNot Nothing AndAlso UpdateTextBinding(sender) Then

            '   DGRow(Binding.BindingMemberInfo.BindingField) = UFCWGeneral.ToNullDecimalHandler(CType(sender, TextBox).Text)
            '   DGRow("PAID_AMT") = UFCWGeneral.ToNullDecimalHandler(txtPaidAmt.Text)

            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & DGRow("LINE_NBR").ToString & ": Paid Is More Than Priced'", CInt(DGRow("LINE_NBR")))
            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & DGRow("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required'", CInt(DGRow("LINE_NBR")))

            If CType(sender, TextBox).Text.IsDecimal() AndAlso Not IsDBNull(DGRow("PRICED_AMT")) AndAlso CDec(CType(sender, TextBox).Text) > CDec(Format(DGRow("PRICED_AMT"), "0.00")) Then
                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DGRow("LINE_NBR").ToString & ": Paid Is More Than Priced", DGRow("LINE_NBR").ToString, "Detail", 20})
            End If

            If (Not CBool(DGRow("REASON_SW")) AndAlso Not CType(sender, TextBox).Text.IsDecimal()) OrElse (Not CBool(DGRow("REASON_SW")) AndAlso ((IsNumeric(CType(sender, TextBox).Text) AndAlso CDec(CType(sender, TextBox).Text) = 0))) Then
                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DGRow("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required", DGRow("LINE_NBR").ToString, "Detail", 30})
            End If

            SumPaid()
            BS.EndEdit()
            ' End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub cmbPlan_Validating(sender As Object, e As CancelEventArgs) Handles cmbPlan.Validating

        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim DR As DataRow

        Try

            ErrorProvider1.ClearError(CBox)

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Position < 0 OrElse CBox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _MedDtlBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = CType(_MedDtlBS.Current, DataRowView).Row

            If CBox.SelectedIndex < 0 Then
                ErrorProvider1.SetErrorWithTracking(CBox, " Plan selection required.")
            End If

            If ErrorProvider1.GetError(CBox).Trim.Length > 0 Then 'are there any errors  
                e.Cancel = True
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _MedDtlBS.Position.ToString & ") SI(" & CBox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub
    Private Sub cmbPlan_Validated(sender As Object, e As EventArgs) Handles cmbPlan.Validated

        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim DR As DataRow
        Dim Equal As Boolean = False

        Try

            If _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse _MedDtlBS.Count = 0 OrElse CBox.Enabled = False OrElse CBox.ReadOnly Then Return

            DR = DirectCast(_MedDtlBS.Current, DataRowView).Row 'need to check if binding is working

            If DR Is Nothing Then Exit Sub

            If DR.RowState = DataRowState.Modified Then
                If IsDBNull(DR("MED_PLAN", DataRowVersion.Original)) AndAlso IsDBNull(DR("MED_PLAN", DataRowVersion.Current)) Then
                    Equal = True
                ElseIf IsDBNull(DR("MED_PLAN", DataRowVersion.Original)) AndAlso Not IsDBNull(DR("MED_PLAN", DataRowVersion.Current)) Then
                    Equal = False
                ElseIf Not IsDBNull(DR("MED_PLAN", DataRowVersion.Original)) AndAlso IsDBNull(DR("MED_PLAN", DataRowVersion.Current)) Then
                    Equal = False
                ElseIf Not IsDBNull(DR("MED_PLAN", DataRowVersion.Original)) AndAlso Not IsDBNull(DR("MED_PLAN", DataRowVersion.Current)) Then
                    If Not DR("MED_PLAN", DataRowVersion.Current).Equals(DR("MED_PLAN", DataRowVersion.Original)) Then
                        Equal = False
                    Else
                        Equal = True
                    End If
                End If
            End If

            If Not Equal Then
                _ClaimAlertManager.AddAlertRow(New Object() {"Re-Calc Is Required", 0, "Header", 30})
            End If


            'Binding = CType(sender, ComboBox).DataBindings("SelectedValue")
            'If Binding IsNot Nothing AndAlso UpdateTextBinding(sender) Then
            '    _ClaimAlertManager.AddAlertRow(New Object() {"Re-Calc Is Required", 0, "Header", 30})
            '    DR(Binding.BindingMemberInfo.BindingField) = CType(sender, ComboBox).SelectedValue
            'End If

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    '    Private Sub cmbPlan_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPlan.Validated

    '#If TRACE Then
    '        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
    '#End If

    '        Dim Binding As Binding
    '        Dim DGRow As DataRow
    '        Dim Cbox As ExComboBox = CType(sender, ExComboBox)

    '        Try

    '            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Position < 0 OrElse Cbox.ReadOnly Then Return

    '            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & _MedDtlBS.Position.ToString & ") SI(" & Cbox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    '            DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

    '            Binding = Cbox.DataBindings("SelectedValue")

    '            If Binding IsNot Nothing AndAlso UpdateTextBinding(sender) Then
    '                _ClaimAlertManager.AddAlertRow(New Object() {"Re-Calc Is Required", 0, "Header", 30}) 'only removed in recalc

    '                Cbox.DataBindings("SelectedValue").WriteValue()

    '                DGRow.EndEdit()
    '                _MedDtlBS.EndEdit()
    '            End If

    '            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & _MedDtlBS.Position.ToString & ") SI(" & Cbox.SelectedIndex & ") " & Me.Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    '        Catch ex As Exception
    '            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '            If (Rethrow) Then
    '                Throw
    '            End If
    '        End Try
    '    End Sub

    Private Sub cmbPlan_DropDown(sender As Object, e As EventArgs) Handles cmbPlan.DropDown

        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        Try

            ErrorProvider1.ClearError(CBox)

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))


            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub cmbPlan_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPlan.SelectedIndexChanged
        Dim BS As BindingSource
        Dim DGRow As DataRow
        Try
            If _ChangingRows OrElse DetailLinesDataGrid.CurrentRowIndex < 0 Then Return

            BS = DirectCast(DetailLinesDataGrid.DataSource, BindingSource)
            If BS IsNot Nothing AndAlso BS.Current IsNot Nothing Then
                DGRow = DirectCast(BS.Current, DataRowView).Row
            End If

            If DGRow Is Nothing Then Return

            If DGRow("MED_PLAN").ToString.Trim <> cmbPlan.Text Then
                _ClaimAlertManager.AddAlertRow(New Object() {"Re-Calc Is Required", 0, "Header", 30})
                DGRow("MED_PLAN") = cmbPlan.Text
            End If
            BS.EndEdit()

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub


    Private Sub txtProcedure_Validating(sender As Object, e As EventArgs) Handles txtProcedure.Validated
#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Dim Tbox As TextBox = CType(sender, TextBox)
        Dim DGRow As DataRow
        Dim ProcDR As DataRow

        Try

            ErrorProvider1.ClearError(Tbox)

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse Tbox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & DGRow("LINE_NBR").ToString & ": Invalid Procedure Code'", CInt(DGRow("LINE_NBR")))

            If Tbox.Text.Trim.Length > 0 Then
                If Not IsDate(txtFromDate.Text) Then
                    _ClaimAlertManager.DeleteAlertRowsLikeMessageAndLine("Procedure Code", CInt(DGRow("LINE_NBR")), "Denied")
                    '   _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DGRow("LINE_NBR").ToString & ": Procedure Code not validated because DOS is unavailable", DGRow("LINE_NBR").ToString, "Detail", 20})
                    ProcDR = CMSDALFDBMD.RetrieveProcedureValueInformation(Tbox.Text, UFCWGeneral.NowDate)
                Else

                    ProcDR = CMSDALFDBMD.RetrieveProcedureValueInformation(Tbox.Text, CDate(txtFromDate.Text))
                End If

                If ProcDR Is Nothing Then
                    DGRow("PROC_CODE_DESC") = "***INVALID PROCEDURE CODE***"
                    ErrorProvider1.SetErrorWithTracking(Tbox, " Procedure Code is Invalid.")
                    _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DGRow("LINE_NBR").ToString & ": Invalid Procedure Code", DGRow("LINE_NBR").ToString, "Detail", 30})
                Else
                    DGRow("PROC_CODE_DESC") = ProcDR("SHORT_DESC")
                End If
            Else
                DGRow("PROC_CODE_DESC") = "***MISSING PROCEDURE CODE***" 'DBNull.Value
                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DGRow("LINE_NBR").ToString & ": Invalid Procedure Code", DGRow("LINE_NBR").ToString, "Detail", 30})
            End If

            _MedDtlBS.EndEdit()
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(sender, TextBox).Name & " Proposed: " & CType(sender, TextBox).Text & " Cancel: " & e.Cancel.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
    End Sub

    Private Sub txtDiagnoses_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDiagnoses.KeyPress

        If Char.IsLetterOrDigit(e.KeyChar) OrElse Char.IsWhiteSpace(e.KeyChar) OrElse e.KeyChar = CChar(",") OrElse Char.IsControl(e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub txtDiagnoses_KeyDown(sender As Object, e As KeyEventArgs) Handles txtDiagnoses.KeyDown
        PatternMatcher.ClipBoardCleaner(sender, e, New String() {"[^\s,a-zA-Z0-9]+"}) 'Allow Space, Comma and AlphaNumeric only
    End Sub

    Private Sub txtDiagnoses_Validating(sender As Object, e As CancelEventArgs) Handles txtDiagnoses.Validating

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Dim Tbox As TextBox = CType(sender, TextBox)
        Dim MeddtlDR As DataRow
        Dim DiagnosisDR As DataRow

        Try

            ErrorProvider1.ClearError(Tbox)

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse Tbox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            MeddtlDR = DirectCast(_MedDtlBS.Current, DataRowView).Row

            _ClaimAlertManager.DeleteAlertRowsLikeMessageAndLine("Diagnosis", CInt(MeddtlDR("LINE_NBR")))

            If Not IsDate(txtFromDate.Text) Then
                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDR("LINE_NBR").ToString & ": Diagnosis not validated because DOS is unavailable", MeddtlDR("LINE_NBR").ToString, "Detail", 20})
            Else

                Dim NoCommaText As String = Tbox.Text.ToUpper.Replace(",", " ")
                Dim Diagnoses2Add As String() = NoCommaText.Split(New Char() {CChar(" ")}, StringSplitOptions.RemoveEmptyEntries)

                If NoCommaText.Trim.Length < 1 Then
                    _ClaimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDR("LINE_NBR").ToString & ": Diagnosis not specified", MeddtlDR("LINE_NBR").ToString, "Detail", 15})
                Else

                    For Each Diagnosis As String In Diagnoses2Add

                        DiagnosisDR = CMSDALFDBMD.RetrieveDiagnosisValuesInformation(Diagnosis.Trim, CDate(txtFromDate.Text))

                        If DiagnosisDR Is Nothing Then

                            ErrorProvider1.SetErrorWithTracking(Tbox, " Diagnosis (" & Diagnosis & ") is Invalid.")

                            _ClaimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDR("LINE_NBR").ToString & ": Invalid Diagnosis", MeddtlDR("LINE_NBR").ToString, "Detail", 30})

                            Exit For

                        End If
                    Next

                    Dim AnyDuplicateQuery = Diagnoses2Add.GroupBy(Function(x) x).Any(Function(g) g.Count() > 1)
                    If AnyDuplicateQuery Then
                        ErrorProvider1.SetErrorWithTracking(Tbox, " Same Diagnosis specified multiple times.")
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

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(sender, TextBox).Name & " Proposed: " & CType(sender, TextBox).Text & " Cancel: " & e.Cancel.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

    End Sub

    Private Sub txtDiagnoses_Validated(sender As Object, e As EventArgs) Handles txtDiagnoses.Validated
#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Dim DGRow As DataRow
        Dim Tbox As TextBox = CType(sender, TextBox)
        Dim LineNumber As Short
        Dim ValidatedMedDiagDT As DataTable
        Dim DIAGNOSES As New StringBuilder

        Try

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse Tbox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If Not IsDate(txtFromDate.Text) Then Return

            DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

            Dim Diagnoses2Add As String() = Tbox.Text.ToUpper.Replace(",", " ").Split(New Char() {CChar(" ")}, StringSplitOptions.RemoveEmptyEntries)
            Dim NextPriorityQuery As Integer = 0

            LineNumber = CShort(DGRow("LINE_NBR"))

            ValidatedMedDiagDT = _ClaimDS.Tables("MEDDIAG").Clone
            If _ClaimDS.Tables("MEDDIAG").Rows.Count > 0 Then
                Dim MedDiagExistingQuery = From MDA In Diagnoses2Add
                                           Join MDL In _ClaimDS.Tables("MEDDIAG").AsEnumerable().Where(Function(x) x.RowState <> DataRowState.Deleted) On MDL.Field(Of String)("DIAGNOSIS") Equals MDA
                                           Where MDL.Field(Of Short)("LINE_NBR") = LineNumber

                If MedDiagExistingQuery.Any Then

                    For Each LineMod In MedDiagExistingQuery

                        Dim DR As DataRow = ValidatedMedDiagDT.NewRow

                        Dim DiagnosisDR As DataRow = CMSDALFDBMD.RetrieveDiagnosisPreventativeStatusEffectiveAsOf(CStr(LineMod.MDA), CDate(txtFromDate.Text))

                        DR("CLAIM_ID") = _ClaimID
                        DR("LINE_NBR") = LineNumber
                        DR("DIAGNOSIS") = CStr(LineMod.MDA)
                        DR("SHORT_DESC") = CStr(DiagnosisDR("SHORT_DESC"))
                        DR("FULL_DESC") = CStr(DiagnosisDR("FULL_DESC"))
                        DR("PREVENTATIVE_USE_SW") = CDec(DiagnosisDR("PREVENTATIVE_USE_SW"))

                        DR("PRIORITY") = NextPriorityQuery '1st entry should be zero.
                        NextPriorityQuery += 1

                        ValidatedMedDiagDT.Rows.Add(DR)

                        If DIAGNOSES.Length > 0 Then
                            DIAGNOSES.Append("," & CStr(DR("DIAGNOSIS")))
                        Else
                            DIAGNOSES.Append(CStr(DR("DIAGNOSIS")))
                        End If

                    Next

                End If
            End If

            If Diagnoses2Add.Length > 0 Then

                Dim MedDiagAddedQuery = From MDA In Diagnoses2Add
                                        Group Join MDL In _ClaimDS.Tables("MEDDIAG").AsEnumerable().Where(Function(x) x.RowState <> DataRowState.Deleted AndAlso x.Field(Of Short)("LINE_NBR") = LineNumber) On MDL.Field(Of String)("DIAGNOSIS") Equals MDA Into gj = Group
                                        From NewMDA In gj.DefaultIfEmpty()
                                        Select New With {Key MDA, Key .MDL = If(NewMDA, Nothing)}

                If MedDiagAddedQuery.Any Then

                    For Each LineMod In MedDiagAddedQuery

                        If LineMod.MDL IsNot Nothing Then Continue For

                        Dim DR As DataRow = ValidatedMedDiagDT.NewRow

                        Dim DiagnosisDR As DataRow = CMSDALFDBMD.RetrieveDiagnosisPreventativeStatusEffectiveAsOf(CStr(LineMod.MDA), CDate(txtFromDate.Text))

                        DR("CLAIM_ID") = _ClaimID
                        DR("LINE_NBR") = LineNumber
                        DR("DIAGNOSIS") = CStr(LineMod.MDA)
                        DR("SHORT_DESC") = CStr(DiagnosisDR("SHORT_DESC"))
                        DR("FULL_DESC") = CStr(DiagnosisDR("FULL_DESC"))
                        DR("PREVENTATIVE_USE_SW") = CDec(DiagnosisDR("PREVENTATIVE_USE_SW"))

                        DR("PRIORITY") = NextPriorityQuery '1st entry should be zero.
                        NextPriorityQuery += 1

                        ValidatedMedDiagDT.Rows.Add(DR)

                        If DIAGNOSES.Length > 0 Then
                            DIAGNOSES.Append("," & CStr(DR("DIAGNOSIS")))
                        Else
                            DIAGNOSES.Append(CStr(DR("DIAGNOSIS")))
                        End If
                    Next

                End If

            End If

            Dim DeletedMedDiagRows = _ClaimDS.Tables("MEDDIAG").AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = LineNumber).ToList()

            For Each DeleteDR As DataRow In DeletedMedDiagRows
                DeleteDR.Delete()
            Next

            If ValidatedMedDiagDT.Rows.Count > 0 Then
                _ClaimDS.Tables("MEDDIAG").Merge(ValidatedMedDiagDT)
            End If

            If DGRow("DIAGNOSES").ToString <> DIAGNOSES.ToString Then
                DGRow("DIAGNOSES") = DIAGNOSES.ToString
            End If

            _MedDtlBS.EndEdit()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub txtModifiers_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtModifiers.KeyPress

        If Char.IsLetterOrDigit(e.KeyChar) OrElse Char.IsWhiteSpace(e.KeyChar) OrElse e.KeyChar = CChar(",") OrElse Char.IsControl(e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub txtModifiers_KeyDown(sender As Object, e As KeyEventArgs) Handles txtModifiers.KeyDown
        PatternMatcher.ClipBoardCleaner(sender, e, New String() {"[^\s,a-zA-Z0-9]+"}) 'Allow Space, Comma and AlphaNumeric only
    End Sub

    '    Private Sub txtModifiers_Validating(sender As Object, e As CancelEventArgs) Handles txtModifiers.Validating

    '#If TRACE Then
    '        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
    '#End If

    '        Dim Tbox As TextBox = CType(sender, TextBox)
    '        Dim MeddtlDR As DataRow
    '        Dim ModifierDR As DataRow

    '        Try

    '            ErrorProvider1.ClearError(Tbox)

    '            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Position < 0 OrElse Tbox.ReadOnly Then Return

    '            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    '            MeddtlDR = DirectCast(_MedDtlBS.Current, DataRowView).Row

    '            _ClaimAlertManager.DeleteAlertRowsLikeMessageAndLine("Modifier", CInt(MeddtlDR("LINE_NBR")))

    '            If Tbox.Text.Trim.Length < 1 Then Return

    '            If Not IsDate(txtFromDate.Text) Then
    '                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDR("LINE_NBR").ToString & ": Modifier(s) not validated because DOS is unavailable", MeddtlDR("LINE_NBR").ToString, "Detail", 20})
    '            Else

    '                Dim Modifiers2Add As String() = Tbox.Text.ToUpper.Replace(",", " ").Split(New Char() {CChar(" ")}, StringSplitOptions.RemoveEmptyEntries)
    '                For Each Modifier As String In Modifiers2Add

    '                    ModifierDR = CMSDALFDBMD.RetrieveModifierValuesInformation(Modifier.Trim, CDate(txtFromDate.Text))

    '                    If ModifierDR Is Nothing Then

    '                        ErrorProvider1.SetErrorWithTracking(Tbox, " Modifier is Invalid. Seperate Modifiers with a comma(,)")

    '                        _ClaimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDR("LINE_NBR").ToString & ": Invalid Modifier", MeddtlDR("LINE_NBR").ToString, "Detail", 30})

    '                        Exit For

    '                    End If
    '                Next
    '                Dim AnyDuplicateQuery = Modifiers2Add.GroupBy(Function(x) x).Any(Function(g) g.Count() > 1)
    '                If AnyDuplicateQuery Then
    '                    ErrorProvider1.SetErrorWithTracking(Tbox, " Same Modifier specified multiple times.")
    '                End If
    '            End If

    '            If ErrorProvider1.GetError(Tbox).Trim.Length > 0 Then 'are there any errors 
    '                e.Cancel = True
    '            End If

    '            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    '        Catch ex As Exception
    '            Throw
    '        Finally
    '        End Try

    '#If TRACE Then
    '        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(sender, TextBox).Name & " Proposed: " & CType(sender, TextBox).Text & " Cancel: " & e.Cancel.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
    '#End If

    '    End Sub

    '    Private Sub txtModifiers_Validated(sender As Object, e As EventArgs) Handles txtModifiers.Validated

    '#If TRACE Then
    '            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
    '#End If

    '        Dim Binding As Binding

    '        Dim DGRow As DataRow
    '        Dim Tbox As TextBox = CType(sender, TextBox)
    '        Dim LineNumber As Short
    '        Dim ValidatedMedModDT As DataTable
    '        Dim MODIFIERS As New StringBuilder

    '        Try

    '            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Position < 0 OrElse Tbox.ReadOnly Then Return

    '            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    '            DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

    '            Dim Modifiers2Add As String() = Tbox.Text.ToUpper.Replace(",", " ").Split(New Char() {CChar(" ")}, StringSplitOptions.RemoveEmptyEntries)
    '            Dim NextPriorityQuery As Integer = 0

    '            LineNumber = CShort(DGRow("LINE_NBR"))

    '            ValidatedMedModDT = _ClaimDS.Tables("MEDMOD").Clone
    '            If _ClaimDS.Tables("MEDMOD").Rows.Count > 0 Then
    '                Dim MedModExistingQuery = From MMA In Modifiers2Add
    '                                          Join MML In _ClaimDS.Tables("MEDMOD").AsEnumerable().Where(Function(x) x.RowState <> DataRowState.Deleted) On MML.Field(Of String)("MODIFIER") Equals MMA
    '                                          Where MML.Field(Of Short)("LINE_NBR") = LineNumber

    '                If MedModExistingQuery.Any Then

    '                    For Each LineMod In MedModExistingQuery

    '                        Dim DR As DataRow = ValidatedMedModDT.NewRow

    '                        Dim ModifierDR As DataRow = CMSDALFDBMD.RetrieveModifierValuesInformation(CStr(LineMod.MMA))

    '                        DR("CLAIM_ID") = _ClaimID
    '                        DR("LINE_NBR") = LineNumber
    '                        DR("MODIFIER") = CStr(LineMod.MMA)
    '                        DR("FULL_DESC") = ModifierDR("FULL_DESC")

    '                        DR("PRIORITY") = NextPriorityQuery '1st entry should be zero.
    '                        NextPriorityQuery += 1

    '                        ValidatedMedModDT.Rows.Add(DR)

    '                        If MODIFIERS.Length > 0 Then
    '                            MODIFIERS.Append("," & CStr(DR("MODIFIER")))
    '                        Else
    '                            MODIFIERS.Append(CStr(DR("MODIFIER")))
    '                        End If

    '                    Next

    '                End If
    '            End If

    '            If Modifiers2Add.Length > 0 Then

    '                Dim MedModAddedQuery = From MMA In Modifiers2Add
    '                                       Group Join MML In _ClaimDS.Tables("MEDMOD").AsEnumerable().Where(Function(x) x.Field(Of Short)("LINE_NBR") = LineNumber) On MML.Field(Of String)("Modifier") Equals MMA Into gj = Group
    '                                       From NewMMA In gj.DefaultIfEmpty()
    '                                       Select New With {Key MMA, Key .MML = If(NewMMA, Nothing)}

    '                If MedModAddedQuery.Any Then

    '                    For Each LineMod In MedModAddedQuery

    '                        If LineMod.MML IsNot Nothing Then Continue For

    '                        Dim DR As DataRow = ValidatedMedModDT.NewRow

    '                        Dim ModifierDR As DataRow = CMSDALFDBMD.RetrieveModifierValuesInformation(CStr(LineMod.MMA))

    '                        DR("CLAIM_ID") = _ClaimID
    '                        DR("LINE_NBR") = LineNumber
    '                        DR("MODIFIER") = CStr(LineMod.MMA)
    '                        DR("FULL_DESC") = ModifierDR("FULL_DESC")

    '                        DR("PRIORITY") = NextPriorityQuery '1st entry should be zero.
    '                        NextPriorityQuery += 1

    '                        ValidatedMedModDT.Rows.Add(DR)

    '                        If MODIFIERS.Length > 0 Then
    '                            MODIFIERS.Append("," & CStr(DR("MODIFIER")))
    '                        Else
    '                            MODIFIERS.Append(CStr(DR("MODIFIER")))
    '                        End If

    '                    Next

    '                End If

    '            End If

    '            Dim DeletedMedModRows = _ClaimDS.Tables("MEDMOD").AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = LineNumber).ToList()

    '            For Each DeleteDR As DataRow In DeletedMedModRows
    '                DeleteDR.Delete()
    '            Next

    '            If ValidatedMedModDT.Rows.Count > 0 Then
    '                _ClaimDS.Tables("MEDMOD").Merge(ValidatedMedModDT)
    '            End If

    '            If DGRow("MODIFIERS").ToString <> MODIFIERS.ToString Then
    '                DGRow("MODIFIERS") = MODIFIERS.ToString
    '            End If

    '            _MedDtlBS.EndEdit()

    '            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    '        Catch ex As Exception
    '            Throw
    '        End Try

    '    End Sub

    Private Sub txtPlaceOfService_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPlaceOfService.KeyPress

        If Char.IsDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub txtPlaceOfService_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPlaceOfService.KeyDown

        PatternMatcher.ClipBoardCleaner(New String() {"[^0-9]+"})

    End Sub

    Private Sub txtPlaceOfService_Validating(sender As Object, e As CancelEventArgs) Handles txtPlaceOfService.Validating

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Dim MedDtlDR As DataRow
        Dim DR As DataRow
        Dim Tbox As TextBox = CType(sender, TextBox)

        Try

            ErrorProvider1.ClearError(Tbox)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse Tbox.ReadOnly Then Return

            MedDtlDR = DirectCast(_MedDtlBS.Current, DataRowView).Row

            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & MedDtlDR("LINE_NBR").ToString & ": Possible Case Management", CInt(MedDtlDR("LINE_NBR")))
            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & MedDtlDR("LINE_NBR").ToString & ": Place of Service not validated because DOS is unavailable", CInt(MedDtlDR("LINE_NBR")))
            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & MedDtlDR("LINE_NBR").ToString & ": Invalid Place of Service", CInt(MedDtlDR("LINE_NBR")))

            'lookup Place Of Service
            If Tbox.Text.Trim.Length > 0 Then
                If IsDBNull(MedDtlDR("OCC_FROM_DATE")) Then
                    _ClaimAlertManager.AddAlertRow(New Object() {"Line " & MedDtlDR("LINE_NBR").ToString & ": Place of Service not validated because DOS is unavailable", MedDtlDR("LINE_NBR").ToString, "Detail", 20})
                Else

                    DR = CMSDALFDBMD.RetrievePlaceOfServiceValueInformation(Tbox.Text, CType(If(IsDBNull(MedDtlDR("OCC_FROM_DATE")), UFCWGeneral.NowDate, MedDtlDR("OCC_FROM_DATE")), Date?))

                    If DR Is Nothing Then

                        If IsDBNull(MedDtlDR("PLACE_OF_SERV_DESC")) OrElse MedDtlDR("PLACE_OF_SERV_DESC").ToString.Trim <> "***INVALID PLACE OF SERVICE***" Then
                            MedDtlDR("PLACE_OF_SERV_DESC") = "***INVALID PLACE OF SERVICE***" 'DBNull.Value
                        End If

                        _ClaimAlertManager.AddAlertRow(New Object() {"Line " & MedDtlDR("LINE_NBR").ToString & ": Invalid Place of Service", MedDtlDR("LINE_NBR").ToString, "Detail", 30})
                        ErrorProvider1.SetErrorWithTracking(Tbox, " Invalid Place of Service")
                    Else
                        If CInt(Tbox.Text) = 12 Then
                            _ClaimAlertManager.AddAlertRow(New Object() {"Line " & MedDtlDR("LINE_NBR").ToString & ": Possible Case Management", MedDtlDR("LINE_NBR").ToString, "Detail", 20, New Object() {ClaimTabPage, DetailLinesDataGrid, MedDtlDR, txtPlaceOfService}})
                        End If
                    End If
                End If
            End If

            If ErrorProvider1.GetError(Tbox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If

        Catch ex As Exception
            Throw
        Finally

#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(sender, TextBox).Name & " Proposed: " & CType(sender, TextBox).Text & " Cancel: " & e.Cancel.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        End Try
    End Sub

    Private Sub txtBillType_Validating(sender As Object, e As CancelEventArgs) Handles txtBillType.Validating
        'Optional
#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Dim DGRow As DataRow
        Dim DR As DataRow
        Dim Tbox As TextBox = CType(sender, TextBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ErrorProvider1.ClearError(Tbox)

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse Tbox.ReadOnly Then Return

            DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

            _ClaimAlertManager.DeleteAlertRowsLikeMessageAndLine("Bill Type", CInt(DGRow("LINE_NBR")))

            If Tbox.Text.Trim.Length < 1 Then Return 'not required

            If Not IsDate(txtFromDate.Text) Then
                DR = ValidateBillType(DGRow, Tbox.Text, UFCWGeneral.NowDate)
                '  _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DGRow("LINE_NBR").ToString & ": Bill Type not validated because DOS is unavailable", DGRow("LINE_NBR").ToString, "Detail", 20})
            Else
                DR = ValidateBillType(DGRow, Tbox.Text, CDate(txtFromDate.Text))
            End If
            If DR IsNot Nothing Then
                DGRow("BILL_TYPE_DESC") = DR("DESC_3")
            End If
            'If ErrorProvider1.GetError(Tbox).Trim.Length > 0 Then 'are there any errors 
            '    e.Cancel = True
            'Else
            '    If IsDBNull(DGRow("BILL_TYPE_DESC")) OrElse DGRow("BILL_TYPE_DESC").ToString.Trim <> DR("FULL_DESC").ToString Then
            '        DGRow("BILL_TYPE_DESC") = DR("FULL_DESC")
            '    End If
            'End If
            DGRow.EndEdit()
            _MedDtlBS.EndEdit()
        Catch ex As Exception
            Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

#If TRACE Then
            If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & ": " & CType(sender, TextBox).Name & " Proposed: " & CType(sender, TextBox).Text & " Cancel: " & e.Cancel.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
        End Try

    End Sub

    Private Sub RequiredMEDDTLMoney_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs)

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Dim Tbox As TextBox = CType(sender, TextBox)
        Dim DGRow As DataRow

        Try

            ErrorProvider1.ClearError(Tbox)

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse Tbox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & DGRow("LINE_NBR").ToString & ": Invalid " & Tbox.Tag.ToString & " Amount Specified", CInt(DGRow("LINE_NBR")))
            _ClaimAlertManager.DeleteAlertRowsLikeMessageAndLine("Line " & DGRow("LINE_NBR").ToString & ": " & Tbox.Tag.ToString & " is required.", CInt(DGRow("LINE_NBR")))

            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & DGRow("LINE_NBR").ToString & ": Paid Is More Than Priced'", CInt(DGRow("LINE_NBR")))
            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & DGRow("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required'", CInt(DGRow("LINE_NBR")))

            If Tbox.Text.Trim.Length < 1 Then
                ErrorProvider1.SetErrorWithTracking(Tbox, Tbox.Tag.ToString & " is required.")
                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DGRow("LINE_NBR").ToString & ": " & Tbox.Tag.ToString & " is required.", DGRow("LINE_NBR").ToString, "Detail", 20})
            Else
                If Not IsDecimal(Tbox.Text.Trim) Then
                    ErrorProvider1.SetErrorWithTracking(Tbox, "Invalid " & Tbox.Tag.ToString & " Amount Specified")
                    _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DGRow("LINE_NBR").ToString & ": Invalid " & Tbox.Tag.ToString & " Amount Specified.", DGRow("LINE_NBR").ToString, "Detail", 20})

                Else

                    If Tbox.Text.Trim <> CDec(Tbox.Text.Trim).ToString("0.00") Then
                        Tbox.Text = CDec(Tbox.Text.Trim).ToString("0.00")
                    End If
                End If
            End If

            If txtPaidAmt.Text.IsDecimal() Then
                If Not IsDBNull(DGRow("PAID_AMT")) AndAlso CDec(txtPaidAmt.Text) < CDec(DGRow("PAID_AMT")) Then
                    ErrorProvider1.SetErrorWithTracking(Tbox, " Paid Is More Than Priced")
                    _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DGRow("LINE_NBR").ToString & ": Paid Is More Than Priced", DGRow("LINE_NBR").ToString, "Detail", 20})
                End If
                If CDec(txtPaidAmt.Text) = 0 AndAlso Not CBool(DGRow("REASON_SW")) Then
                    _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DGRow("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required", DGRow("LINE_NBR").ToString, "Detail", 15})
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
#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & " : " & CType(sender, TextBox).Name & " Proposed: " & CType(sender, TextBox).Text & " Cancel: " & e.Cancel.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If
    End Sub

    Private Sub OptionalMEDDTLMoney_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs)

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Dim Tbox As TextBox = CType(sender, TextBox)
        Dim DGRow As DataRow

        Try

            ErrorProvider1.ClearError(Tbox)

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse Tbox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & DGRow("LINE_NBR").ToString & ": Invalid " & Tbox.Tag.ToString & " Amount Specified", CInt(DGRow("LINE_NBR")))

            If Tbox.Text.Trim.Length < 1 Then Return

            If Not IsDecimal(Tbox.Text.Trim) Then
                ErrorProvider1.SetErrorWithTracking(Tbox, "Invalid " & Tbox.Tag.ToString & " Amount Specified")
                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DGRow("LINE_NBR").ToString & ": Invalid " & Tbox.Tag.ToString & " Amount Specified.", DGRow("LINE_NBR").ToString, "Detail", 20})

            Else

                If Tbox.Text.Trim <> CDec(Tbox.Text.Trim).ToString("0.00") Then
                    Tbox.Text = CDec(Tbox.Text.Trim).ToString("0.00")
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
#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & " : " & CType(sender, TextBox).Name & " Proposed: " & CType(sender, TextBox).Text & " Cancel: " & e.Cancel.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

    End Sub

    Private Sub OptionalMEDDTLQty_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs)

#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

        Dim Tbox As TextBox = CType(sender, TextBox)
        Dim DGRow As DataRow

        Try

            ErrorProvider1.ClearError(Tbox)

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse Tbox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DGRow = DirectCast(_MedDtlBS.Current, DataRowView).Row

            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & DGRow("LINE_NBR").ToString & ": Invalid Quantity/Units Specified", CInt(DGRow("LINE_NBR")))

            If Tbox.Text.Trim.Length < 1 Then Return

            If Not IsDecimal(Tbox.Text.Trim) Then
                ErrorProvider1.SetErrorWithTracking(Tbox, ": Invalid Quantity/Units Specified")
                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DGRow("LINE_NBR").ToString & ": Invalid Quantity/Units Specified", DGRow("LINE_NBR").ToString, "Detail", 20})

            Else

                If Tbox.Text.Trim.Contains(".") AndAlso Tbox.Text.Trim <> CDec(Tbox.Text.Trim).ToString("0.00") Then
                    Tbox.Text = CDec(Tbox.Text.Trim).ToString("0.00")
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
#If TRACE Then
        If CInt(_TraceBinding.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : " & Me.Name & " : " & CType(sender, TextBox).Name & " Proposed: " & CType(sender, TextBox).Text & " Cancel: " & e.Cancel.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""), "TraceBinding" & vbTab)
#End If

    End Sub

    Private Sub cmbStatus_DropDown(sender As Object, e As EventArgs) Handles cmbStatus.DropDown

        Dim CBox As ExComboBox = CType(sender, ExComboBox)

        ErrorProvider1.ClearError(CBox)

    End Sub

    Private Sub cmbStatus_Validating(sender As Object, e As CancelEventArgs) Handles cmbStatus.Validating

        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim DR As DataRow

        Try

            ErrorProvider1.ClearError(CBox)

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing OrElse CBox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & _MedDtlBS.Position.ToString & ") Val(" & CType(_MedDtlBS.Current, DataRowView).Row("STATUS").ToString & "/" & CBox.Text & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = CType(_MedDtlBS.Current, DataRowView).Row

            If CBox.SelectedIndex < 0 Then
                ErrorProvider1.SetErrorWithTracking(CBox, " Status selection required.")
            End If

            If ErrorProvider1.GetError(CBox).Trim.Length > 0 Then 'are there any errors  
                e.Cancel = True
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & ":" & CBox.Name & " BS(" & _MedDtlBS.Position.ToString & ") Val(" & CType(_MedDtlBS.Current, DataRowView).Row("STATUS").ToString & "/" & CBox.Text & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub

    Private Sub cmbStatus_Validated(sender As Object, e As EventArgs) Handles cmbStatus.Validated

        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim DR As DataRow
        Dim BS As BindingSource

        Try

            If _MedHdrBS Is Nothing OrElse _MedHdrBS.Current Is Nothing OrElse CBox.Enabled = False OrElse CBox.ReadOnly Then Return

            BS = DirectCast(_MedDtlBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & _MedDtlBS.Position.ToString & ") Val(" & CType(_MedDtlBS.Current, DataRowView).Row("STATUS").ToString & "/" & CBox.Text & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            DR = DirectCast(_MedHdrBS.Current, DataRowView).Row 'need to check if binding is working

            If DR.HasVersion(DataRowVersion.Proposed) Then
                Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Mid: " & Me.Name & ":" & CBox.Name & " BS(" & _MedDtlBS.Position.ToString & ") Val(" & CType(_MedDtlBS.Current, DataRowView).Row("STATUS").ToString & "/" & CBox.Text & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

                _MedDtlBS.EndEdit()
                ' in edit mode ?
                _MedDtlBS.ResetBindings(False)
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & ":" & CBox.Name & " BS(" & _MedDtlBS.Position.ToString & ") Val(" & CType(_MedDtlBS.Current, DataRowView).Row("STATUS").ToString & "/" & CBox.Text & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub cmbStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStatus.SelectedIndexChanged

        Dim CBox As ExComboBox = CType(sender, ExComboBox)
        Dim DR As DataRow
        Try
            If _ChangingRows OrElse DetailLinesDataGrid.CurrentRowIndex < 0 Then Return

            _MedDtlBS = DirectCast(DetailLinesDataGrid.DataSource, BindingSource)

            If _MedDtlBS Is Nothing OrElse _MedDtlBS.Current Is Nothing Then Return

            RemoveHandler cmbStatus.SelectedIndexChanged, AddressOf cmbStatus_SelectedIndexChanged

            DR = DirectCast(_MedDtlBS.Current, DataRowView).Row
            If DR Is Nothing Then Return

            If cmbStatus.Text = "DENY" Then
                _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & DR("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required'", CInt(DR("LINE_NBR")))
                _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & DR("LINE_NBR").ToString & ": Paid Is More Than Priced'", CInt(DR("LINE_NBR")))

                DR("STATUS") = "DENY"
                DR("PAID_AMT") = 0D
                DR("PROCESSED_AMT") = 0D
            Else
                DR("STATUS") = "PAY"
            End If

            If Not IsDBNull(DR("PRICED_AMT")) AndAlso Not IsDBNull(DR("PAID_AMT")) AndAlso CDec(DR("PAID_AMT")) > CDec(Format(DR("PRICED_AMT"), "0.00")) Then
                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Paid Is More Than Priced", DR("LINE_NBR").ToString, "Detail", 20})
            End If

            If Not CBool(DR("REASON_SW")) AndAlso Not IsDBNull(DR("PAID_AMT")) AndAlso CDec(DR("PAID_AMT")) = 0 Then
                _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required", DR("LINE_NBR").ToString, "Detail", 30})
            End If

            SumPaid()

            _MedDtlBS.EndEdit()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & CBox.Name & " BS(" & _MedDtlBS.Position.ToString & ") Val(" & CType(_MedDtlBS.Current, DataRowView).Row("STATUS").ToString & "/" & CBox.Text & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & ":" & CBox.Name & " BS(" & _MedDtlBS.Position.ToString & ") Val(" & CType(_MedDtlBS.Current, DataRowView).Row("STATUS").ToString & "/" & CBox.Text & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            AddHandler cmbStatus.SelectedIndexChanged, AddressOf cmbStatus_SelectedIndexChanged
        End Try

    End Sub

#End Region

    Private Function IdentifyChanges(ByRef changedDT As DataTable, ByVal histEntryDT As DataTable, ByVal rowState As DataRowState, Optional ByVal rowFilter As String = Nothing, Optional ByVal rowSort As String = Nothing) As DataTable

        Dim OrigDV As DataView
        Dim CurDV As DataView
        Dim NewVal As String
        Dim OrigVal As String
        Dim DispName As String
        Dim HistDR As DataRow

        Try

            'FinalizeEdits()

            OrigDV = New DataView(changedDT, rowFilter, rowSort, DataViewRowState.OriginalRows)
            CurDV = New DataView(changedDT, rowFilter, rowSort, DataViewRowState.CurrentRows)

            For ColumnNum As Integer = 0 To CurDV.Table.Columns.Count - 1

                If rowState = DataRowState.Added OrElse IsDBNull(OrigDV(0)(ColumnNum)) Then
                    OrigVal = "NULL"
                Else
                    OrigVal = UFCWGeneral.IsNullStringHandler(OrigDV(0)(ColumnNum), "").ToUpper.Trim
                End If

                If rowState <> DataRowState.Added AndAlso (rowState = DataRowState.Deleted OrElse IsDBNull(CurDV(0)(ColumnNum))) Then
                    NewVal = "NULL"
                Else
                    NewVal = UFCWGeneral.IsNullStringHandler(CurDV(0)(ColumnNum), "").ToUpper.Trim
                End If

                DispName = GetColumnDisplayName(CurDV.Table.Columns(ColumnNum).ColumnName.ToUpper)

                If NewVal <> OrigVal AndAlso Not ((IsNumeric(NewVal) AndAlso IsNumeric(OrigVal)) AndAlso (CDbl(NewVal) = CDbl(OrigVal))) Then
                    HistDR = histEntryDT.NewRow

                    If changedDT.TableName = "MEDHDR" OrElse changedDT.TableName = "CLAIM_MASTER" Then
                        HistDR("RowNum") = 0
                        HistDR("Detail") = DispName & " = " & NewVal & " (was '" & OrigVal & "')"
                    Else
                        HistDR("RowNum") = If(NewVal = "NULL", OrigDV(0)("LINE_NBR"), CurDV(0)("LINE_NBR"))
                        HistDR("Detail") = "LINE " & If(rowState = DataRowState.Added, "N/A", OrigDV(0)("LINE_NBR").ToString) & " - " & DispName & " = " & NewVal & " (was '" & OrigVal & "')"
                    End If

                    HistDR("EntryPosition") = histEntryDT.Rows.Count + 1
                    histEntryDT.Rows.Add(HistDR)

                End If
            Next

            Return histEntryDT

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function GetColumnDisplayName(ByVal columnName As String) As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' gets the friendly name of a column from the config file
        ' </summary>
        ' <param name="ColumnName"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	10/16/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DispName As String
        Try

            DispName = CStr(CType(ConfigurationManager.GetSection("HistoryDisplayColumnNames"), IDictionary)(columnName)) & ""
            If DispName.Trim.Length = 0 Then DispName = "*" & columnName.ToUpper

            Return DispName

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub ReCalcClaim()

        Dim AddRepriceAlert As Boolean = False
        Dim AlertDV As DataView
        Dim ValidPatient As Boolean

        Try
            If MsgBox("If you Re-Calc, you will lose all the changes you have made. Are you sure you wish to Re-Calc?", CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo, MsgBoxStyle), "Confirm Delete") = DialogResult.Yes Then

                _ClaimAlertManager.SuspendLayout = True

                FinalizeEdits()

                _Transaction = Nothing

                AlertDV = New DataView(_ClaimAlertManager.AlertManagerDataTable, "Category = 'Header' And Message = 'Re-Price Is Required'", "Category, Message", DataViewRowState.CurrentRows)

                If AlertDV.Count > 0 Then
                    AddRepriceAlert = True
                End If

                _ClaimAlertManager.Clear()
                AlertsImageListBox.Clear()

                Me.Refresh()

                Using WC As New GlobalCursor

                    If _Mode.ToUpper = "AUDIT" Then
                        ProcessDenyRules(True)
                    Else
                        ProcessDenyRules(False)
                    End If

                    ValidPatient = ValidatePatient()
                    LoadDetailLineEligibility(True) ' LM 03/14/2023
                    SumPaid()
                    SyncAllowed()
                    SumAllowed()
                    SumOI()
                End Using

                If _Mode.ToUpper = "AUDIT" Then
                    LoadCommittedAccumulators()
                Else
                    If ValidPatient Then
                        AccumulatorsDataGrid.DataSource = Nothing
                        _DetailAccumulatorsDT = Nothing
                        _AccumulatorsDT = Nothing
                        Using WC As New GlobalCursor
                            ' _MedDtlBS.RaiseListChangedEvents = False
                            ClaimProcessor.LoadDetailLineAccumulators(AddressOf WorkStatusMessage, AddressOf SelectAccidentUI, AddressOf AccumulatorCheckIfOverrideNeededUI, _HighestEntryID, _ClaimBinder, _ClaimMemberAccumulatorManager, CType(_ClaimDS, DataSet), _DetailAccumulatorsDT, _AccumulatorsDT, _ClaimAlertManager)
                            ' _MedDtlBS.RaiseListChangedEvents = True
                            '  _MedDtlBS.EndEdit()
                            'Me.BindingContext(_ClaimDataSet.MEDDTL).EndCurrentEdit()

                        End Using

                    End If
                End If

                LoadDuplicates()
                LoadAccumulators()


                Using WC As New GlobalCursor

                    ClaimProcessor.LoadReasonsAlerts(CType(_ClaimDS, DataSet), _ClaimAlertManager)
                    ClaimProcessor.LoadClaimAlerts(CType(_ClaimDS, DataSet), _ClaimAlertManager)
                    ClaimProcessor.LoadHeaderAlerts(CType(_ClaimDS, DataSet), _ClaimAlertManager)

                    HighlightHeaderAlerts()

                    ClaimProcessor.LoadDetailLineAlerts(CType(_ClaimDS, DataSet), _ClaimAlertManager)

                    ClaimProcessor.LoadDiagnosisAlerts(CType(_ClaimDS, DataSet), _ClaimAlertManager, _PlansIncludingPreventativeRules)

                    If AddRepriceAlert Then
                        _ClaimAlertManager.AddAlertRow(New Object() {"Re-Price Is Required", 0, "Header", 30})
                    End If

                    ClaimProcessor.LoadPatientAlerts(CType(_ClaimDS, DataSet), _ClaimAlertManager)

                End Using

                '#If TRACE Then
                '            If CInt(_TraceParallel.Level) > 1 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
                '#End If

                SumCharges()
                SumPriced()
                SumPaid()
                SyncAllowed()
                SumAllowed()
                SumOI()

                'If DetailLinesDataGrid.CurrentRowIndex <> -1 Then
                '    LoadDetailLineRow(CInt(DetailLinesDataGrid.GetDefaultDataView(DetailLinesDataGrid.CurrentRowIndex)("LINE_NBR")))
                'End If
                If _MedDtlBS IsNot Nothing AndAlso _MedDtlBS.Current IsNot Nothing Then
                    _MedDtlDR = DirectCast(_MedDtlBS.Current, DataRowView).Row
                    If _MedDtlDR IsNot Nothing Then
                        LoadDetailLineRow(CInt(_MedDtlDR("LINE_NBR")))
                    End If
                End If
                If _MedHdrDr IsNot Nothing AndAlso _MedDtlDR IsNot Nothing Then
                    Me.CompleteToolBarButton.Enabled = True
                Else
                    Me.CompleteToolBarButton.Enabled = False
                End If

                DisableEnableButtons()
            End If
        Catch ex As Exception
            Throw
        Finally
            _ClaimAlertManager.SuspendLayout = False
        End Try
    End Sub

    Private Function RePriceClaim(ByVal returnToExaminer As Boolean) As Boolean

        Dim Transaction As DbTransaction
        Dim MedDTLDV As DataView
        Dim Lines As String = ""

        Try

            FinalizeEdits()

            ''FromDate is required for  PAY Status

            MedDTLDV = New DataView(_ClaimDS.MEDDTL, "STATUS = 'PAY'", "LINE_NBR", DataViewRowState.CurrentRows)
            If MedDTLDV.Count > 0 Then
                For Cnt As Integer = 0 To MedDTLDV.Count - 1
                    If IsDBNull(MedDTLDV(Cnt)("OCC_FROM_DATE")) OrElse (CStr(MedDTLDV(Cnt)("OCC_FROM_DATE")).TrimEnd = "") Then
                        Lines &= If(Cnt <> 0, ", ", "") & If(Cnt = MedDTLDV.Count - 1 AndAlso Cnt <> 0, "& ", "") & MedDTLDV(Cnt)("LINE_NBR").ToString
                    End If
                Next
                If Lines.Length > 0 Then
                    MessageBox.Show("Line" & If(MedDTLDV.Count > 1, "s ", " ") & Lines & " having Action PAY, Required FromDate ", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            End If

            ''Gender is required to price claim
            If UFCWGeneral.IsNullStringHandler(_MedHdrDr("PAT_SEX")) <> "M" AndAlso UFCWGeneral.IsNullStringHandler(_MedHdrDr("PAT_SEX")) <> "F" Then
                MessageBox.Show("Gender is required to Price a claim.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            ''DOB is required to price claim
            If IsDBNull(_MedHdrDr("PAT_DOB")) Then
                MessageBox.Show("DOB is required to Price a claim.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            ''detail line is required to price claim
            If _ClaimDS.MEDDTL.Rows.Count < 1 Then
                MessageBox.Show("Claim has no detail lines.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            Transaction = CMSDALCommon.BeginTransaction

            If HasChanges() Then
                If Not SaveChanges(Transaction) Then
                    CMSDALCommon.RollbackTransaction(Transaction)
                    Return False
                End If
            End If

            CMSDALFDBMD.CreateBCPricingWithReturn(_ClaimID, returnToExaminer, _DomainUser.ToUpper, Transaction)

            Dim HistSum As String = "CLAIM ID " & Format(_ClaimID, "00000000") & " WAS SUBMITTED FOR BLUE CROSS BATCH PRICING" & If(returnToExaminer, " - Return Requested", "")

            Dim HistDetail As String = "FUNCTION: BLUE CROSS BATCH PRICE" & Microsoft.VisualBasic.vbCrLf &
                            "CLAIM ID: " & Format(_ClaimID, "00000000") & Microsoft.VisualBasic.vbCrLf &
                            "TOTAL DETAIL LINES: " & DetailLinesDataGrid.GetDefaultDataView.Count & If(returnToExaminer, " (Return Requested)", "")

            CMSDALFDBMD.CreateDocHistory(_ClaimID, UFCWGeneral.IsNullLongHandler(_ClaimDr("DOCID"), "DOCID"), "REPRICE", CInt(_ClaimDr("FAMILY_ID")), CShort(_ClaimDr("RELATION_ID")), CInt(_ClaimDr("PART_SSN")), CInt(_ClaimDr("PAT_SSN")), CStr(_ClaimDr("DOC_CLASS")), CStr(_ClaimDr("DOC_TYPE")), HistSum, HistDetail, _DomainUser.ToUpper, Transaction)

            CMSDALCommon.CommitTransaction(Transaction)

            Return True

        Catch ex As Exception
            CMSDALCommon.RollbackTransaction(Transaction)
            Throw

        End Try

    End Function

    Private Sub UpdateLineDiagnoses(detailLine As Short, DiagnosisForm As DetailLineDiagnosisForm)
        Try

            Dim QueryMedDtl = _ClaimDS.MEDDTL.AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of String)("STATUS") <> "MERGED" AndAlso r.Field(Of Short)("LINE_NBR") = detailLine)

            Dim MedDtlDR As DataRow = QueryMedDtl.FirstOrDefault()

            Dim QueryDelete = _ClaimDS.MEDDIAG.AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = detailLine)

            For Each DR As DataRow In QueryDelete.ToList()
                DR.Delete()
            Next

            If DiagnosisForm.MEDDIAG.Rows.Count > 0 Then
                MedDtlDR("DIAG_SW") = 1

                _ClaimDS.MEDDIAG.BeginLoadData()

                Dim SortedPriorityQuery = DiagnosisForm.MEDDIAG.AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted).OrderBy(Of Integer)(Function(r) r.Field(Of Integer)("PRIORITY"))

                MedDtlDR("DIAGNOSES") = DBNull.Value

                For Each AddDR As DataRow In SortedPriorityQuery
                    AddDR("LINE_NBR") = MedDtlDR("LINE_NBR")
                    If Not IsDBNull(AddDR("PRIORITY")) AndAlso CInt(AddDR("PRIORITY")) = 0 Then
                        MedDtlDR("DIAGNOSIS") = AddDR("DIAGNOSIS")
                    End If
                    If Not IsDBNull(MedDtlDR("DIAGNOSES")) Then
                        MedDtlDR("DIAGNOSES") = CStr(MedDtlDR("DIAGNOSES")) & "," & CStr(AddDR("DIAGNOSIS"))
                    Else
                        MedDtlDR("DIAGNOSES") = AddDR("DIAGNOSIS")
                    End If

                    _ClaimDS.Tables("MEDDIAG").ImportRow(AddDR)
                Next

                _ClaimDS.Tables("MEDDIAG").EndLoadData()

            Else
                MedDtlDR("DIAG_SW") = 0
                MedDtlDR("DIAGNOSIS") = DBNull.Value
                MedDtlDR("DIAGNOSES") = DBNull.Value
                _ClaimAlertManager.DeleteAlertRowsLikeMessageAndLine("Line " & detailLine & ": Invalid Diagnosis", detailLine)
            End If
            MedDtlDR.EndEdit()
            _MedDtlBS.EndEdit()
        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub
    Private Sub UpdateLineModifier(detailLine As Short, ModifierForm As DetailLineModifierForm)
        Try

            Dim QueryMedDtl = _ClaimDS.MEDDTL.AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of String)("STATUS") <> "MERGED" AndAlso r.Field(Of Short)("LINE_NBR") = detailLine)

            Dim MedDtlDR As DataRow = QueryMedDtl.FirstOrDefault()

            Dim QueryDelete = _ClaimDS.MEDMOD.AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = detailLine)

            For Each DR As DataRow In QueryDelete.ToList()
                DR.Delete()
            Next

            If ModifierForm.MedMod.Rows.Count > 0 Then
                MedDtlDR("MODIFIER_SW") = 1

                _ClaimDS.MEDMOD.BeginLoadData()
                _ClaimAlertManager.DeleteAlertRowsLikeMessageAndLine("Line " & detailLine & ": Invalid Modifier", detailLine)

                Dim SortedPriorityQuery = ModifierForm.MedMod.AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted).OrderBy(Of Integer)(Function(r) r.Field(Of Integer)("PRIORITY"))

                MedDtlDR("MODIFIERS") = DBNull.Value

                For Each AddDR As DataRow In SortedPriorityQuery
                    AddDR("LINE_NBR") = MedDtlDR("LINE_NBR")
                    If Not IsDBNull(AddDR("PRIORITY")) AndAlso CInt(AddDR("PRIORITY")) = 0 Then
                        MedDtlDR("MODIFIER") = AddDR("MODIFIER")
                    End If
                    If Not IsDBNull(MedDtlDR("MODIFIERS")) Then
                        MedDtlDR("MODIFIERS") = CStr(MedDtlDR("MODIFIERS")) & "," & CStr(AddDR("MODIFIER"))
                    Else
                        MedDtlDR("MODIFIERS") = AddDR("MODIFIER")
                    End If
                    _ClaimDS.Tables("MEDMOD").ImportRow(AddDR)
                Next
                _ClaimDS.Tables("MEDMOD").EndLoadData()
            Else
                MedDtlDR("MODIFIER_SW") = 0
                MedDtlDR("MODIFIER") = DBNull.Value
                _ClaimAlertManager.DeleteAlertRowsLikeMessageAndLine("Line " & detailLine & ": Invalid Modifier", detailLine)
            End If

            MedDtlDR.EndEdit()
            _MedDtlBS.EndEdit()

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub
    Private Function UpdateTextBinding(sender As Object) As Boolean

        Dim Binding As Binding
        Dim NewValue As String
        Dim ControlName As String

        Try

            Select Case True
                Case TypeOf (sender) Is ComboBox

                    Binding = CType(sender, ComboBox).DataBindings("SelectedValue")
                    NewValue = CType(sender, ComboBox).SelectedValue.ToString
                    ControlName = CType(sender, ComboBox).Name

                Case TypeOf (sender) Is TextBox

                    Binding = CType(sender, TextBox).DataBindings("TEXT")
                    NewValue = CType(sender, TextBox).Text
                    ControlName = CType(sender, TextBox).Name

            End Select

            Try

                If Not BindingEqual(Binding, NewValue, ControlName) Then
                    Return True
                End If

            Catch ex As Exception

                If Not BindingEqual(Binding, Nothing, ControlName) Then 'if data cannot be written back to datasource write nothing
                    Return True
                End If

            End Try

            Return False

        Catch ex As Exception
            Throw
        Finally
            'If Binding IsNot Nothing Then CType(Binding.DataSource, DataTable).EndLoadData()
        End Try

    End Function
    Private Sub MassUpdatePlanType(ByVal planValue As String)
        Try
            For Each DR As DataRow In CType(_MedDtlBS.DataSource, DataTable).Rows
                DR.BeginEdit()
                DR("MED_PLAN") = planValue
                DR.EndEdit()
            Next
            _MedDtlBS.EndEdit()
        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub MassUpdateModifier(fieldMapping As String, value As Object, includeMergedItems As Boolean)

        Dim CodeValueDR As DataRow
        Dim DateOfService As DateTime = UFCWGeneral.NowDate
        Dim NewDR As DataRow
        Dim DateOfServiceDV As DataView
        Dim ModifierDV As DataView

        Try
            For Each DR As DataRow In _ClaimDS.MEDDTL.Rows
                If Not IsDBNull(DR("OCC_FROM_DATE")) Then
                    DateOfService = CDate(DR("OCC_FROM_DATE"))
                Else
                    DateOfServiceDV = New DataView(_ClaimDS.MEDDTL, "OCC_FROM_DATE = MIN(OCC_FROM_DATE)", "OCC_FROM_DATE", DataViewRowState.CurrentRows)

                    If DateOfServiceDV.Count > 0 AndAlso Not IsDBNull(DateOfServiceDV(0)("OCC_FROM_DATE")) Then
                        DateOfService = CDate(DateOfServiceDV(0)("OCC_FROM_DATE"))
                    ElseIf Not IsDBNull(_ClaimDr("DATE_OF_SERVICE")) Then
                        DateOfService = CDate(_ClaimDr("DATE_OF_SERVICE"))
                    End If
                End If

                CodeValueDR = CMSDALFDBMD.RetrieveModifierValuesInformation(CStr(value), DateOfService)
                If CodeValueDR Is Nothing Then
                    txtModifiers.SelectionStart = 0
                    txtModifiers.SelectionLength = txtModifiers.Text.Length

                    _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Invalid Modifier Code", DR("LINE_NBR").ToString, "Detail", 30})
                End If

                NewDR = _ClaimDS.MEDMOD.NewRow

                ModifierDV = New DataView(_ClaimDS.MEDMOD, "LINE_NBR = " & DR("LINE_NBR").ToString & " And Priority = 0", "LINE_NBR, Priority", DataViewRowState.CurrentRows)
                If ModifierDV.Count > 0 Then

                    NewDR.ItemArray = ModifierDV(0).Row.ItemArray

                    NewDR("MODIFIER") = value
                    NewDR("FULL_DESC") = CodeValueDR("FULL_DESC")

                    ModifierDV(0).Row.Delete()

                    ModifierDV.RowFilter = "LINE_NBR = " & DR("LINE_NBR").ToString & " And MODIFIER = '" & CStr(value) & "'"
                    If ModifierDV.Count > 0 Then
                        ModifierDV(0).Row.Delete()

                        ModifierDV.RowFilter = "LINE_NBR = " & DR("LINE_NBR").ToString
                        If ModifierDV.Count > 0 Then
                            For SubCnt As Integer = 0 To ModifierDV.Count - 1
                                ModifierDV(SubCnt).Row("PRIORITY") = SubCnt + 1
                            Next
                        End If
                    End If
                Else
                    NewDR("CLAIM_ID") = DR("CLAIM_ID")
                    NewDR("LINE_NBR") = DR("LINE_NBR")
                    NewDR("MODIFIER") = value
                    NewDR("FULL_DESC") = CodeValueDR("FULL_DESC")
                    NewDR("PRIORITY") = 0
                End If

                _ClaimDS.MEDMOD.Rows.Add(NewDR)

                If IsDBNull(DR("MODIFIER_SW")) OrElse Not CBool(DR("MODIFIER_SW")) Then
                    DR("MODIFIER_SW") = 1
                End If
            Next

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub MassUpdateDiagnosis(fieldMapping As String, value As Object, includeMergedItems As Boolean)

        Dim CodeValueDR As DataRow
        Dim DateOfService As DateTime = UFCWGeneral.NowDate
        Dim NewDR As DataRow
        Dim DateOfServiceDV As DataView
        Dim DiagnosisDV As DataView

        Try
            For Each DR As DataRow In _ClaimDS.MEDDTL

                If Not IsDBNull(DR("OCC_FROM_DATE")) Then
                    DateOfService = CDate(DR("OCC_FROM_DATE"))
                Else
                    DateOfServiceDV = New DataView(_ClaimDS.MEDDTL, "OCC_FROM_DATE = MIN(OCC_FROM_DATE)", "OCC_FROM_DATE", DataViewRowState.CurrentRows)

                    If DateOfServiceDV.Count > 0 AndAlso Not IsDBNull(DateOfServiceDV(0)("OCC_FROM_DATE")) Then
                        DateOfService = CDate(DateOfServiceDV(0)("OCC_FROM_DATE"))
                    ElseIf Not IsDBNull(_ClaimDr("DATE_OF_SERVICE")) Then
                        DateOfService = CDate(_ClaimDr("DATE_OF_SERVICE"))
                    End If
                End If

                CodeValueDR = CMSDALFDBMD.RetrieveDiagnosisValuesInformation(CStr(value), DateOfService)
                If CodeValueDR Is Nothing Then
                    txtDiagnoses.SelectionStart = 0
                    txtDiagnoses.SelectionLength = txtDiagnoses.Text.Length

                    _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Invalid Diagnosis", DR("LINE_NBR").ToString, "Detail", 30})
                End If

                NewDR = _ClaimDS.MEDDIAG.NewRow

                DiagnosisDV = New DataView(_ClaimDS.MEDDIAG, "LINE_NBR = " & DR("LINE_NBR").ToString & " And Priority = 0", "Line_NBR, Priority", DataViewRowState.CurrentRows)
                If DiagnosisDV.Count > 0 Then
                    NewDR.ItemArray = DiagnosisDV(0).Row.ItemArray

                    NewDR("DIAGNOSIS") = value
                    NewDR("SHORT_DESC") = CodeValueDR("SHORT_DESC")

                    DiagnosisDV(0).Row.Delete()

                    DiagnosisDV.RowFilter = "LINE_NBR = " & DR("LINE_NBR").ToString & " And DIAGNOSIS = '" & CStr(value) & "'"
                    If DiagnosisDV.Count > 0 Then
                        DiagnosisDV(0).Row.Delete()

                        DiagnosisDV.RowFilter = "LINE_NBR = " & DR("LINE_NBR").ToString
                        If DiagnosisDV.Count > 0 Then
                            For SubCnt As Integer = 0 To DiagnosisDV.Count - 1
                                DiagnosisDV(SubCnt).Row("PRIORITY") = SubCnt + 1
                            Next
                        End If
                    End If
                Else
                    NewDR("CLAIM_ID") = DR("CLAIM_ID")
                    NewDR("LINE_NBR") = DR("LINE_NBR")
                    NewDR("DIAGNOSIS") = value
                    NewDR("SHORT_DESC") = CodeValueDR("SHORT_DESC")
                    NewDR("PRIORITY") = 0
                End If

                _ClaimDS.MEDDIAG.Rows.Add(NewDR)

                If IsDBNull(DR("DIAG_SW")) OrElse Not CBool(DR("DIAG_SW")) Then
                    DR("DIAG_SW") = 1
                End If

            Next
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub MassUpdatePOS(fieldMapping As String, value As Object, includeMergedItems As Boolean)
        Dim CodeValueDR As DataRow
        Dim DateOfService As DateTime = UFCWGeneral.NowDate
        Dim DateOfServiceDV As DataView

        Try
            For Each DR As DataRow In _ClaimDS.MEDDTL
                If Not IsDBNull(DR("OCC_FROM_DATE")) Then
                    DateOfService = CDate(DR("OCC_FROM_DATE"))
                Else
                    DateOfServiceDV = New DataView(_ClaimDS.MEDDTL, "OCC_FROM_DATE = MIN(OCC_FROM_DATE)", "OCC_FROM_DATE", DataViewRowState.CurrentRows)

                    If DateOfServiceDV.Count > 0 AndAlso Not IsDBNull(DateOfServiceDV(0)("OCC_FROM_DATE")) Then
                        DateOfService = CDate(DateOfServiceDV(0)("OCC_FROM_DATE"))
                    ElseIf Not IsDBNull(_ClaimDr("DATE_OF_SERVICE")) Then
                        DateOfService = CDate(_ClaimDr("DATE_OF_SERVICE"))
                    End If
                End If

                CodeValueDR = CMSDALFDBMD.RetrievePlaceOfServiceValueInformation(CStr(If(IsNumeric(value), CInt(value), value)), DateOfService)
                If CodeValueDR Is Nothing Then
                    txtPlaceOfService.SelectionStart = 0
                    txtPlaceOfService.SelectionLength = txtPlaceOfService.Text.Length

                    If DR("PLACE_OF_SERV_DESC").ToString <> "***INVALID PLACE OF SERVICE***" Then DR("PLACE_OF_SERV_DESC") = "***INVALID PLACE OF SERVICE***"

                    _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Invalid Place of Service", DR("LINE_NBR").ToString, "Detail", 30})
                Else
                    If DR("PLACE_OF_SERV_DESC").ToString <> CodeValueDR("SHORT_DESC").ToString Then DR("PLACE_OF_SERV_DESC") = CodeValueDR("SHORT_DESC")
                End If

                If CInt(value) = 12 Then
                    _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Possible Case Management", DR("LINE_NBR").ToString, "Detail", 20})
                Else
                    _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("Line " & DR("LINE_NBR").ToString & ": Possible Case Management", CInt(DR("LINE_NBR")))
                End If
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub MassUpdateBillType(fieldMapping As String, value As Object, includeMergedItems As Boolean)

        Dim CodeValueDR As DataRow
        Dim DateOfService As DateTime = UFCWGeneral.NowDate
        Dim DateOfServiceDV As DataView

        Try
            For Each DR As DataRow In _ClaimDS.MEDDTL
                If Not IsDBNull(DR("OCC_FROM_DATE")) Then
                    DateOfService = CDate(DR("OCC_FROM_DATE"))
                Else
                    DateOfServiceDV = New DataView(_ClaimDS.MEDDTL, "OCC_FROM_DATE = MIN(OCC_FROM_DATE)", "OCC_FROM_DATE", DataViewRowState.CurrentRows)

                    If DateOfServiceDV.Count > 0 AndAlso Not IsDBNull(DateOfServiceDV(0)("OCC_FROM_DATE")) Then
                        DateOfService = CDate(DateOfServiceDV(0)("OCC_FROM_DATE"))
                    ElseIf Not IsDBNull(_ClaimDr("DATE_OF_SERVICE")) Then
                        DateOfService = CDate(_ClaimDr("DATE_OF_SERVICE"))
                    End If
                End If

                CodeValueDR = CMSDALFDBMD.RetrieveBillTypeValuesInformation(CStr(value), DateOfService)
                If CodeValueDR Is Nothing Then
                    txtPlaceOfService.SelectionStart = 0
                    txtPlaceOfService.SelectionLength = txtPlaceOfService.Text.Length

                    If DR("BILL_TYPE_DESC").ToString <> "***INVALID BILL TYPE***" Then DR("BILL_TYPE_DESC") = "***INVALID BILL TYPE***"

                    _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Invalid Place of Service", DR("LINE_NBR").ToString, "Detail", 30})
                Else
                    DR("BILL_TYPE") = CodeValueDR("BILL_TYPE_VALUE")
                    DR("BILL_TYPE_DESC") = CodeValueDR("FULL_DESC")
                End If
            Next

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub MassUpdateFDOS(fieldMapping As String, value As Object, includeMergedItems As Boolean)
        Dim DateOfService As DateTime = UFCWGeneral.NowDate
        Dim ApplyStatus As String = ""
        Try
            If Not IsDate(value) Then
                If IsNumeric(value) Then
                    Select Case CStr(value).Length
                        Case Is = 8
                            value = Microsoft.VisualBasic.Left(CStr(value), 2) & "-" & Microsoft.VisualBasic.Mid(CStr(value), 3, 2) & "-" & Microsoft.VisualBasic.Right(CStr(value), 4)
                        Case Is = 6
                            value = Microsoft.VisualBasic.Left(CStr(value), 2) & "-" & Microsoft.VisualBasic.Mid(CStr(value), 3, 2) & "-" & Microsoft.VisualBasic.Right(CStr(value), 2)
                        Case Is = 4
                            value = Microsoft.VisualBasic.Left(CStr(value), 1) & "-" & Microsoft.VisualBasic.Mid(CStr(value), 2, 1) & "-" & Microsoft.VisualBasic.Right(CStr(value), 2)
                    End Select
                End If
            End If

            'For Each DR As DataRow In _ClaimDS.MEDDTL
            '    If value.ToString.Trim.Length > 0 AndAlso IsDate(value) Then
            '        DR.BeginEdit()
            '        DR("OCC_FROM_DATE") = CDate(value)
            '        DR.EndEdit()
            '    Else
            '        _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Missing From Date", DR("LINE_NBR").ToString, "Detail", 20})
            '    End If

            '    If value.ToString.Trim.Length = 0 OrElse (value.ToString.Trim.Length > 0 AndAlso IsDate(value) AndAlso Not IsDBNull(_ClaimDr("REC_DATE")) AndAlso CDate(value) > DateAdd(DateInterval.Year, -1, CDate(_ClaimDr("REC_DATE")))) Then
            '    Else
            '        _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Was Received 1 Year After DOS", DR("LINE_NBR").ToString, "Detail", 20})
            '    End If
            'Next
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub MassUpdateLDOS(fieldMapping As String, value As Object, includeMergedItems As Boolean)
        Dim DateOfService As DateTime = UFCWGeneral.NowDate
        Dim ApplyStatus As String = ""

        Try
            If Not IsDate(value) Then
                If IsNumeric(value) Then
                    Select Case CStr(value).Length
                        Case Is = 8
                            value = Microsoft.VisualBasic.Left(CStr(value), 2) & "-" & Microsoft.VisualBasic.Mid(CStr(value), 3, 2) & "-" & Microsoft.VisualBasic.Right(CStr(value), 4)
                        Case Is = 6
                            value = Microsoft.VisualBasic.Left(CStr(value), 2) & "-" & Microsoft.VisualBasic.Mid(CStr(value), 3, 2) & "-" & Microsoft.VisualBasic.Right(CStr(value), 2)
                        Case Is = 4
                            value = Microsoft.VisualBasic.Left(CStr(value), 1) & "-" & Microsoft.VisualBasic.Mid(CStr(value), 2, 1) & "-" & Microsoft.VisualBasic.Right(CStr(value), 2)
                    End Select
                End If
            End If

            For Each DR As DataRow In _ClaimDS.MEDDTL
                If value.ToString.Trim.Length > 0 AndAlso IsDate(value) Then
                    DR("OCC_TO_DATE") = CDate(value)
                Else
                    _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Missing To Date", DR("LINE_NBR").ToString, "Detail", 20})
                End If
            Next
            _MedDtlBS.EndEdit()

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub MassUpdateStatus(fieldMapping As String, value As Object, includeMergedItems As Boolean)
        Try
            _MedDtlBS.SuspendBinding()
            For Each DR As DataRow In _ClaimDS.MEDDTL
                DR("STATUS") = value
            Next
            _MedDtlBS.EndEdit()
            _MedDtlBS.ResumeBinding()
            _MedDtlBS.ResetBindings(False)
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub MassUpdateReason(fieldMapping As String, value As Object, includeMergedItems As Boolean)

        Dim CodeValueDR As DataRow
        Dim DateOfService As DateTime = UFCWGeneral.NowDate
        Dim NewDR As DataRow
        Dim DateOfServiceDV As DataView
        Dim ApplyStatus As String = ""

        Try
            _MedDtlBS.SuspendBinding()

            If value.ToString.Length < 3 Then
                If IsNumeric(value) Then
                    value = value.ToString.PadLeft(3, CChar("0"))
                End If
            End If

            For Each DR As DataRow In _ClaimDS.MEDDTL
                NewDR = _ClaimDS.REASON.NewRow

                If Not IsDBNull(DR("OCC_FROM_DATE")) Then
                    DateOfService = CDate(DR("OCC_FROM_DATE"))
                Else
                    DateOfServiceDV = New DataView(_ClaimDS.MEDDTL, "OCC_FROM_DATE = MIN(OCC_FROM_DATE)", "OCC_FROM_DATE", DataViewRowState.CurrentRows)

                    If DateOfServiceDV.Count > 0 AndAlso IsDBNull(DateOfServiceDV(0)("OCC_FROM_DATE")) = False Then
                        DateOfService = CDate(DateOfServiceDV(0)("OCC_FROM_DATE"))
                    ElseIf IsDBNull(_ClaimDr("DATE_OF_SERVICE")) = False Then
                        DateOfService = CDate(_ClaimDr("DATE_OF_SERVICE"))
                    End If
                End If

                CodeValueDR = CMSDALFDBMD.RetrieveReasonValuesInformation(CStr(value), DateOfService)
                If CodeValueDR Is Nothing Then
                    Throw New ApplicationException("Reason Not valid for specified Date")
                End If

                Using ReasonDV As DataView = New DataView(_ClaimDS.REASON, "LINE_NBR = " & DR("LINE_NBR").ToString & " And Priority = 0", "Line_NBR, Priority", DataViewRowState.CurrentRows)

                    If ReasonDV.Count > 0 Then
                        If Not CBool(ReasonDV(0)("PRINT_SW")) Then
                            ReasonDV(0)("DESCRIPTION") = CStr(ReasonDV(0)("DESCRIPTION")).Replace("'", "''")
                            _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & ReasonDV(0)("LINE_NBR").ToString & ": " & ReasonDV(0)("DESCRIPTION").ToString & "'", CInt(DR("LINE_NBR")))
                        End If

                        NewDR.ItemArray = ReasonDV(0).Row.ItemArray

                        NewDR("REASON") = value
                        NewDR("DESCRIPTION") = CodeValueDR("DESCRIPTION")
                        NewDR("PRINT_SW") = CodeValueDR("PRINT_SW")
                        NewDR("APPLY_STATUS") = CodeValueDR("APPLY_STATUS")

                        ReasonDV(0).Row.Delete()

                        ReasonDV.RowFilter = "LINE_NBR = " & DR("LINE_NBR").ToString & " And REASON = '" & CStr(value) & "'"
                        If ReasonDV.Count > 0 Then
                            If Not CBool(ReasonDV(0)("PRINT_SW")) Then
                                ReasonDV(0)("DESCRIPTION") = CStr(ReasonDV(0)("DESCRIPTION")).Replace("'", "''")
                                _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & ReasonDV(0)("LINE_NBR").ToString & ": " & ReasonDV(0)("DESCRIPTION").ToString & "'", CInt(DR("LINE_NBR")))
                            End If

                            ReasonDV(0).Row.Delete()

                            ReasonDV.RowFilter = "LINE_NBR = " & DR("LINE_NBR").ToString
                            If ReasonDV.Count > 0 Then
                                For SubCnt As Integer = 0 To ReasonDV.Count - 1
                                    ReasonDV(SubCnt).Row("PRIORITY") = SubCnt + 1
                                Next
                            End If
                        End If
                    Else
                        NewDR("CLAIM_ID") = DR("CLAIM_ID")
                        NewDR("LINE_NBR") = DR("LINE_NBR")
                        NewDR("REASON") = value
                        NewDR("DESCRIPTION") = CodeValueDR("DESCRIPTION")
                        NewDR("PRINT_SW") = CodeValueDR("PRINT_SW")
                        NewDR("APPLY_STATUS") = CodeValueDR("APPLY_STATUS")
                        NewDR("PRIORITY") = 0
                    End If
                End Using

                _ClaimDS.REASON.Rows.Add(NewDR)

                If Not CBool(CodeValueDR("PRINT_SW")) Then
                    _ClaimAlertManager.AddAlertRow(New Object() {"Line " & NewDR("LINE_NBR").ToString & ": " & NewDR("DESCRIPTION").ToString, NewDR("LINE_NBR").ToString, "Reasons", 10})
                End If

                _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & DR("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required'", CInt(DR("LINE_NBR")))

                If Not IsDBNull(CodeValueDR("APPLY_STATUS")) AndAlso CStr(CodeValueDR("APPLY_STATUS")).Trim <> "" Then
                    ApplyStatus = CStr(CodeValueDR("APPLY_STATUS")).ToUpper
                End If

                If ApplyStatus <> "DENY" Then
                    Using ReasonDV As DataView = New DataView(_ClaimDS.REASON, "LINE_NBR = " & DR("LINE_NBR").ToString & " AND APPLY_STATUS = 'DENY'", "Line_NBR", DataViewRowState.CurrentRows)
                        If ReasonDV.Count > 0 Then
                            ApplyStatus = "DENY"
                        End If
                    End Using
                End If

                If ApplyStatus <> "" Then
                    DR("STATUS") = ApplyStatus
                End If

                If ApplyStatus = "DENY" AndAlso Not IsDBNull(DR("PAID_AMT")) Then
                    _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & DR("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required'", CInt(DR("LINE_NBR")))
                    _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & DR("LINE_NBR").ToString & ": Paid Is More Than Priced'", CInt(DR("LINE_NBR")))

                    If Not IsDBNull(DR("PRICED_AMT")) AndAlso CDec(DR("PAID_AMT")) > CDec(Format(DR("PRICED_AMT"), "0.00")) Then
                        _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Paid Is More Than Priced", DR("LINE_NBR").ToString, "Detail", 20})
                    End If

                End If
            Next
            _MedDtlBS.ResumeBinding()
            _MedDtlBS.EndEdit()
            _MedDtlBS.ResetBindings(False)
            value = 1 'True
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub MassUpdateProcCode(fieldMapping As String, value As Object, includeMergedItems As Boolean)

        Dim CodeValueDR As DataRow
        Dim DateOfService As DateTime = UFCWGeneral.NowDate
        Dim DateOfServiceDV As DataView

        Try
            For Each DR As DataRow In _ClaimDS.MEDDTL
                If Not IsDBNull(DR("OCC_FROM_DATE")) Then
                    DateOfService = CDate(DR("OCC_FROM_DATE"))
                Else
                    DateOfServiceDV = New DataView(_ClaimDS.MEDDTL, "OCC_FROM_DATE = MIN(OCC_FROM_DATE)", "OCC_FROM_DATE", DataViewRowState.CurrentRows)

                    If DateOfServiceDV.Count > 0 AndAlso Not IsDBNull(DateOfServiceDV(0)("OCC_FROM_DATE")) Then
                        DateOfService = CDate(DateOfServiceDV(0)("OCC_FROM_DATE"))
                    ElseIf Not IsDBNull(_ClaimDr("DATE_OF_SERVICE")) Then
                        DateOfService = CDate(_ClaimDr("DATE_OF_SERVICE"))
                    End If
                End If

                CodeValueDR = CMSDALFDBMD.RetrieveProcedureValueInformation(CStr(value), DateOfService)

                If CodeValueDR Is Nothing Then
                    txtProcedure.SelectionStart = 0
                    txtProcedure.SelectionLength = txtProcedure.Text.Length
                    DR("PROC_CODE_DESC") = "***INVALID PROCEDURE CODE***" 'DBNull.Value
                    _ClaimAlertManager.AddAlertRow(New Object() {"Line " & DR("LINE_NBR").ToString & ": Invalid Procedure Code", DR("LINE_NBR").ToString, "Detail", 30, Nothing})
                Else
                    DR("PROC_CODE_DESC") = CodeValueDR("SHORT_DESC")
                End If
            Next
            _MedDtlBS.EndEdit()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DupsTreeViewContextMenu_Opening(sender As Object, e As CancelEventArgs) Handles DupsTreeViewContextMenu.Opening

        If Not UFCWGeneralAD.CMSCanReopenFull() Then
            ReverseToolStripMenuItem.Visible = False
            ReverseToolStripMenuItem.Enabled = False
        End If

    End Sub

    Private Sub ReversePayment(sender As Object, e As EventArgs)
        Dim Frm As ReversalForm
        Dim ClaimID As Integer

        Try
            If DupsTreeView.SelectedNode.Parent IsNot Nothing Then
                ClaimID = CInt(DupsTreeView.SelectedNode.Text.Split(CChar(";"))(0))
            End If

            Frm = New ReversalForm
            Frm.AccumulatorValues.ShowLineDetails = True

            Frm.SetClaimID(ClaimID, _DuplicatesClaimDS)
            Frm.DisplayLineItemsByClaim(ClaimID, _DuplicatesClaimDS)

            Frm.Show()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ReEvaluateClaimAfterLineChanged(DGRow As DataRow)
        'Validate From date based on the Status
        'Reevaluate diagnosis preventative status based upon new from date
        'Recalculate Line and Claim $$$ Amts

        Try

            LoadDetailLineEligibility(DGRow, False)

            SyncAllowed(CShort(DGRow("LINE_NBR")))

            SumPriced()
            SumAllowed()

            SumPaid()
            SumAllowed()
            SumOI()

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

#Region "BindingSource"

    Private Sub ClaimDataSetMeddtlBS_PositionChanged(sender As Object, e As EventArgs) Handles _MedDtlBS.PositionChanged

        Dim BS As BindingSource

        Try
            '    _ChangingRows = True
            BS = CType(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If BS Is Nothing OrElse BS.Position < 0 OrElse BS.Count < 1 Then Return 'no current row, most likely an item filter value was changed

            _MedDtlDR = DirectCast(BS.Current, DataRowView).Row

            If _MedDtlDR IsNot Nothing Then
                LoadDetailLineRow(CInt(_MedDtlDR("LINE_NBR")))
                SetUIElements()
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            '   _ChangingRows = False
        End Try

    End Sub

    'Private Sub ClaimDataSetMeddtlBS_CurrentChanged(sender As Object, e As EventArgs) Handles _MedDtlBS.CurrentChanged

    '    Dim BS As BindingSource
    '    Dim DR As DataRow

    '    Try

    '        BS = DirectCast(sender, BindingSource)

    '        If BS Is Nothing OrElse BS.Position < 0 OrElse BS.Current Is Nothing OrElse BS.Count < 1 OrElse DetailLinesDataGrid.GetGridRowCount < 1 OrElse DetailLinesDataGrid.DataSource Is Nothing Then Return

    '        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    '        DR = DirectCast(BS.Current, DataRowView).Row

    '        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    '    Catch ex As Exception
    '        Throw
    '    Finally
    '    End Try

    'End Sub

    'Private Sub ClaimDataSetMeddtlBS_CurrentItemChanged(sender As Object, e As EventArgs) Handles _MedDtlBS.PositionChanged
    '    'Called after CurrentChanged and one of the properties of Current is changed
    '    Dim BS As BindingSource

    '    Try

    '        BS = CType(sender, BindingSource)

    '        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    '        SetUIElements()

    '        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    '    Catch ex As Exception
    '        Throw
    '    Finally

    '    End Try
    'End Sub

#End Region

#Region "Form Overrides"

    '<System.Diagnostics.DebuggerStepThrough()>
    'Protected Overrides Sub WndProc(ByRef m As Message)

    '    Select Case ((m.WParam.ToInt64() And &HFFFF) And &HFFF0)

    '        Case &HF060 ' The user choose to close the form.
    '            Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    '    End Select

    '    Select Case m.Msg

    '        Case NativeMethods.WM_NOTIFY, NativeMethods.WM_REFLECT + NativeMethods.WM_NOTIFY
    '            Dim NM As NMHDR = DirectCast(m.GetLParam(GetType(NMHDR)), NMHDR)
    '            Select Case NM.code
    '                ' new switch added to prevent the TabControl from changing to next TabPage ...
    '                'in case of validation cancelled...
    '                'Turn  tabControlState[TABCONTROLSTATE_UISelection] = false and Return So that no WmSelChange() gets fired.
    '                'If validation not cancelled then tabControlState[TABCONTROLSTATE_UISelection] is turned ON to set the focus on to the ...
    '                'next TabPage..

    '                Case NativeMethods.TCN_SELCHANGING
    '                    Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
    '                Case NativeMethods.TCN_SELCHANGE
    '                    Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
    '                Case NativeMethods.TTN_GETDISPINFOA, NativeMethods.TTN_GETDISPINFOW
    '                    ' MSDN:
    '                    ' Setting the max width has the added benefit of enabling Multiline
    '                    ' tool tips!
    '                    '
    '                    Stop
    '            End Select
    '            Exit Select
    '    End Select
    '    'If m.Msg = tabBaseReLayoutMessage Then
    '    '    WmTabBaseReLayout(m)
    '    '    Return
    '    'End If

    '    MyBase.WndProc(m)

    'End Sub

    'Private Sub cmbHosp_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cmbHosp.SelectionChangeCommitted

    '    Dim CBox As ExComboBox = CType(sender, ExComboBox)

    '    Try
    '        If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Position < 0 OrElse CBox.ReadOnly OrElse CBox.SelectedIndex < 0 Then Return

    '        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    '        CType(CBox.Parent, TransparentContainer).ValidateChildren() 'this will trigger validation of the cmbbox triggering write of value to DS

    '        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: SI(" & CBox.SelectedIndex & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (Rethrow) Then
    '            Throw
    '        End If
    '    End Try
    'End Sub

    Private Sub BillTypeButton_Click(sender As Object, e As EventArgs) Handles BillTypeButton.Click
        Try

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Position < 0 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ShowDetailLineBillType()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try

    End Sub

    Private Sub POSButton_Click(sender As Object, e As EventArgs) Handles POSButton.Click
        '  Dim DGRow As DataRow
        Try

            If _Disposed OrElse _MedDtlBS Is Nothing OrElse _MedDtlBS.Position < 0 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ShowDetailLinePOS()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                Throw
            End If
        End Try

    End Sub


    Private Sub ApplyLineReasons(LineNumber As Short, Reasons2Add() As String)

        Try
            Dim ValidatedReasonDT As DataTable = _ClaimDS.Tables("REASON").Clone
            Dim NextPriorityQuery As Integer = 0

            If _ClaimDS.Tables("REASON").Rows.Count > 0 AndAlso Reasons2Add IsNot Nothing AndAlso Reasons2Add.Length > 0 Then
                Dim ReasonExistingQuery = From MRA In Reasons2Add
                                          Join MRL In _ClaimDS.Tables("REASON").AsEnumerable().Where(Function(x) x.RowState <> DataRowState.Deleted) On MRL.Field(Of String)("REASON") Equals MRA
                                          Where MRL.Field(Of Short)("LINE_NBR") = LineNumber

                If ReasonExistingQuery.Any Then

                    For Each LineReason In ReasonExistingQuery

                        Dim DR As DataRow = ValidatedReasonDT.NewRow

                        Dim ReasonDR As DataRow = CMSDALFDBMD.RetrieveReasonValuesInformation(CStr(LineReason.MRA))

                        DR("CLAIM_ID") = _ClaimID
                        DR("LINE_NBR") = LineNumber
                        DR("REASON") = ReasonDR("REASON")
                        DR("DESCRIPTION") = ReasonDR("DESCRIPTION")
                        DR("APPLY_STATUS") = ReasonDR("APPLY_STATUS")
                        DR("PRINT_SW") = ReasonDR("PRINT_SW")

                        DR("PRIORITY") = NextPriorityQuery '1st entry should be zero.
                        NextPriorityQuery += 1

                        ValidatedReasonDT.Rows.Add(DR)

                    Next

                End If
            End If

            If Reasons2Add IsNot Nothing AndAlso Reasons2Add.Length > 0 Then

                Dim ReasonAddedQuery = From RA In Reasons2Add
                                       Group Join RL In _ClaimDS.Tables("REASON").AsEnumerable().Where(Function(x) x.RowState <> DataRowState.Deleted AndAlso x.Field(Of Short)("LINE_NBR") = LineNumber) On RL.Field(Of String)("REASON") Equals RA Into gj = Group
                                       From NewRA In gj.DefaultIfEmpty()
                                       Select New With {Key RA, Key .RL = If(NewRA, Nothing)}

                If ReasonAddedQuery.Any Then

                    For Each LineReason In ReasonAddedQuery

                        If LineReason.RL IsNot Nothing Then Continue For

                        Dim DR As DataRow = ValidatedReasonDT.NewRow

                        Dim ReasonDR As DataRow = CMSDALFDBMD.RetrieveReasonValuesInformation(CStr(LineReason.RA))

                        DR("CLAIM_ID") = _ClaimID
                        DR("LINE_NBR") = LineNumber
                        DR("REASON") = ReasonDR("REASON")
                        DR("DESCRIPTION") = ReasonDR("DESCRIPTION")
                        DR("APPLY_STATUS") = ReasonDR("APPLY_STATUS")
                        DR("PRINT_SW") = ReasonDR("PRINT_SW")

                        DR("PRIORITY") = NextPriorityQuery '1st entry should be zero.
                        NextPriorityQuery += 1

                        ValidatedReasonDT.Rows.Add(DR)

                    Next

                End If

            End If

            Dim DeletedReasonRows = _ClaimDS.Tables("REASON").AsEnumerable().Where(Function(r) r.RowState <> DataRowState.Deleted AndAlso r.Field(Of Short)("LINE_NBR") = LineNumber).ToList()
            For Each DeleteDR As DataRow In DeletedReasonRows
                _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & DeleteDR("LINE_NBR").ToString & ": " & DeleteDR("DESCRIPTION").ToString & "'", CInt(DeleteDR("LINE_NBR")))
                DeleteDR.Delete()
            Next

            If ValidatedReasonDT.Rows.Count > 0 Then
                _ClaimDS.Tables("REASON").Merge(ValidatedReasonDT)
            End If

            Dim MeddtlLineItemQuery = _ClaimDS.Tables("MEDDTL").AsEnumerable().Where(Function(x) x.RowState <> DataRowState.Deleted AndAlso x.Field(Of Short)("LINE_NBR") = LineNumber)
            Dim MeddtlDR As DataRow = MeddtlLineItemQuery.First

            Dim ReasonMergedQuery = _ClaimDS.Tables("REASON").AsEnumerable().Where(Function(x) x.RowState <> DataRowState.Deleted AndAlso x.Field(Of Short)("LINE_NBR") = LineNumber).ToList

            Dim ApplyStatus As String
            For Each ReasonDR As DataRow In ReasonMergedQuery

                'DENY overrides all; if already deny leave deny
                If Not IsDBNull(ReasonDR("APPLY_STATUS")) AndAlso (ApplyStatus Is Nothing OrElse ApplyStatus <> "DENY") Then
                    ApplyStatus = CStr(ReasonDR("APPLY_STATUS")).ToUpper
                End If

                If Not CBool(ReasonDR("PRINT_SW")) Then
                    _ClaimAlertManager.AddAlertRow(New Object() {"Line " & ReasonDR("LINE_NBR").ToString & ": " & ReasonDR("DESCRIPTION").ToString, ReasonDR("LINE_NBR").ToString, "Reasons", 10})
                End If
            Next

            If ReasonMergedQuery.Any Then
                MeddtlDR("REASON_SW") = 1
                If IsDBNull(MeddtlDR("STATUS")) OrElse (ApplyStatus IsNot Nothing AndAlso ApplyStatus <> "" AndAlso MeddtlDR("STATUS").ToString.Trim <> ApplyStatus) Then
                    MeddtlDR("STATUS") = ApplyStatus
                Else
                    ApplyStatus = MeddtlDR("STATUS").ToString
                End If
                If ApplyStatus = "DENY" AndAlso Not IsDBNull(MeddtlDR("PAID_AMT")) Then
                    If CDec(MeddtlDR("PAID_AMT")) <> 0D Then
                        MeddtlDR("PAID_AMT") = 0D
                    End If
                End If

                'If paid = 0 And there Is reasons Then delete alert
                If IsDBNull(MeddtlDR("PAID_AMT")) OrElse CDec(MeddtlDR("PAID_AMT")) = 0D Then
                    _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & MeddtlDR("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required'", CInt(MeddtlDR("LINE_NBR")))
                End If

                If ApplyStatus = "DENY" AndAlso Not IsDBNull(MeddtlDR("PAID_AMT")) Then
                    _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & MeddtlDR("LINE_NBR").ToString & ": Paid Is More Than Priced'", CInt(MeddtlDR("LINE_NBR")))
                    _ClaimAlertManager.DeleteAlertRowsByMessageAndLine("'Line " & MeddtlDR("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required'", CInt(MeddtlDR("LINE_NBR")))

                    If Not IsDBNull(MeddtlDR("PRICED_AMT")) AndAlso CDec(MeddtlDR("PAID_AMT")) > CDec(Format(MeddtlDR("PRICED_AMT"), "0.00")) Then
                        _ClaimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDR("LINE_NBR").ToString & ": Paid Is More Than Priced", MeddtlDR("LINE_NBR").ToString, "Detail", 20})
                    End If

                    If Not CBool(MeddtlDR("REASON_SW")) AndAlso CDec(MeddtlDR("PAID_AMT")) = 0D Then
                        _ClaimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDR("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required", MeddtlDR("LINE_NBR").ToString, "Detail", 30})
                    End If

                    SumPaid()
                End If
            Else
                MeddtlDR("REASON_SW") = 0
                If IsDBNull(MeddtlDR("PAID_AMT")) OrElse CDec(MeddtlDR("PAID_AMT")) = 0D Then
                    _ClaimAlertManager.AddAlertRow(New Object() {"Line " & MeddtlDR("LINE_NBR").ToString & ": Paid Is 0 and a Reason is Required", MeddtlDR("LINE_NBR").ToString, "Detail", 30})
                End If
            End If


        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub txtPlaceOfService_GotFocus(sender As Object, e As EventArgs) Handles txtPlaceOfService.GotFocus

        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " IO:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    End Sub

    Private Sub txtPlaceOfService_Enter(sender As Object, e As EventArgs) Handles txtPlaceOfService.Enter

        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " IO:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    End Sub

#End Region

End Class