Option Infer On
Option Strict On

Imports System.Xml
Imports System.Configuration
Imports System.ComponentModel
Imports System.Threading
Imports System.Data.Common
Imports System.Collections.Generic
Imports System.Data.DataTableExtensions
Imports UFCW.WCF
Imports DDTek.DB2
Imports System.Threading.Tasks
Imports System.Text.RegularExpressions
Imports System.IO

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.CustomerServiceControl
''' Class	 : CMS.CustomerServiceControl.CustomerServiceControl
'''
''' -----------------------------------------------------------------------------
''' <summary>
'''
''' </summary>
''' <remarks>
''' Some of the code in here was taken from Nick Snyder's customer service
'''   application.  I have tried to give him credit in the appropriate
'''   functions, but not all functions are commented.
''' </remarks>
''' <history>
''' 	[paulw]	10/18/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class CustomerServiceControl
    Inherits System.Windows.Forms.UserControl

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Dim designMode As Boolean = (LicenseManager.UsageMode = LicenseUsageMode.Designtime)

        If Not designMode Then

            ExtendedSearchMorningStart = CDate(CType(ConfigurationManager.GetSection("ExtendedSearchMorningExclusion"), IDictionary)("Start"))
            ExtendedSearchMorningEnd = CDate(CType(ConfigurationManager.GetSection("ExtendedSearchMorningExclusion"), IDictionary)("End"))
            ExtendedSearchAfternoonStart = CDate(CType(ConfigurationManager.GetSection("ExtendedSearchAfternoonExclusion"), IDictionary)("Start"))
            ExtendedSearchAfternoonEnd = CDate(CType(ConfigurationManager.GetSection("ExtendedSearchAfternoonExclusion"), IDictionary)("End"))
            ExtendedSearchEveningStart = CDate(CType(ConfigurationManager.GetSection("ExtendedSearchEveningExclusion"), IDictionary)("Start"))
            ExtendedSearchEveningEnd = CDate(CType(ConfigurationManager.GetSection("ExtendedSearchEveningExclusion"), IDictionary)("End"))

            If _DisableSearchUI Then
                SearchCriteriaGroupBox.Visible = False
                SearchResultsGroupBox.Dock = DockStyle.Fill
            End If

            _ENV = CMSDALCommon.DefaultEnvironment

        End If

    End Sub

    Friend WithEvents DiseaseManagementMenuItem As ToolStripMenuItem
    Friend WithEvents EligibilityAlertLabel As ToolStripLabel
    Friend WithEvents ReasonsTextBox As TextBox
    Friend WithEvents ReasonsButton As Button
    Friend WithEvents ReasonsLabel As Label

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.

            If components IsNot Nothing Then
                components.Dispose()
            End If

            If _COBDS IsNot Nothing Then _COBDS.Dispose()
            If _CoverageHistoryDS IsNot Nothing Then _CoverageHistoryDS.Dispose()
            If _DentalDS IsNot Nothing Then _DentalDS.Dispose()
            If _DentalPendDS IsNot Nothing Then _DentalPendDS.Dispose()
            If _DentalPREAuthDS IsNot Nothing Then _DentalPREAuthDS.Dispose()
            If _PremLetterDS IsNot Nothing Then _PremLetterDS.Dispose()
            If _PremiumsDS IsNot Nothing Then _PremiumsDS.Dispose()
            If _PrescriptionsDS IsNot Nothing Then _PrescriptionsDS.Dispose()
            If _ParticipantFamilyBS IsNot Nothing Then _ParticipantFamilyBS.Dispose()

            If _HRADS IsNot Nothing Then _HRADS.Dispose()
            If _HRABalanceDS IsNot Nothing Then _HRABalanceDS.Dispose()
            If _HRQDS IsNot Nothing Then _HRQDS.Dispose()

            If _DocsinBatchDS IsNot Nothing Then _DocsinBatchDS.Dispose()

            If _OriginalDT IsNot Nothing Then _OriginalDT.Dispose()
            If _DiagnosisDT IsNot Nothing Then _DiagnosisDT.Dispose()
            If _ProcedureDT IsNot Nothing Then _ProcedureDT.Dispose()
            If _ModifierDT IsNot Nothing Then _ModifierDT.Dispose()
            If _PlaceOfServiceDT IsNot Nothing Then _PlaceOfServiceDT.Dispose()
            If _BillTypeDT IsNot Nothing Then _BillTypeDT.Dispose()
            If QueueImageList IsNot Nothing Then QueueImageList.Dispose()
            CustomerServiceResultsDataGrid.TableStyles.Clear()

            If CustomerServiceResultsDataGrid IsNot Nothing Then CustomerServiceResultsDataGrid.Dispose()
            CustomerServiceResultsDataGrid = Nothing

            If MainGrid IsNot Nothing Then MainGrid.Dispose()
            MainGrid = Nothing
            If _AnnotationsDS IsNot Nothing Then
                _AnnotationsDS.ANNOTATIONS.Clear()
                _AnnotationsDS.ANNOTATIONS.Dispose()
                _AnnotationsDS.Dispose()
            End If
            _AnnotationsDS = Nothing

            If _SearchResultsDT IsNot Nothing Then _SearchResultsDT.Dispose()
            _SearchResultsDT = Nothing
            If _StatusDV IsNot Nothing Then _StatusDV.Dispose()
            _StatusDV = Nothing
            If MCnt IsNot Nothing Then MCnt.Dispose()
            If _ShowLabel IsNot Nothing Then _ShowLabel.Dispose()
            If _ShowLabel2 IsNot Nothing Then _ShowLabel2.Dispose()
            If TypeDropDown IsNot Nothing Then TypeDropDown.Dispose()
            If TypeDropDown2 IsNot Nothing Then TypeDropDown2.Dispose()

            If _Eligcolor IsNot Nothing Then _Eligcolor.Dispose()
            _Eligcolor = Nothing

            If _InEligcolor IsNot Nothing Then _InEligcolor.Dispose()
            _InEligcolor = Nothing

            If _PartialInEligcolor IsNot Nothing Then _PartialInEligcolor.Dispose()
            _InEligcolor = Nothing

            If _PartialEligcolor IsNot Nothing Then _PartialEligcolor.Dispose()
            _InEligcolor = Nothing

        End If

        ' Free any unmanaged objects here.
        '
        _Disposed = True

        ' Call base class implementation.
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents SearchResultsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents TabControl As System.Windows.Forms.TabControl
    Friend WithEvents GenTab As System.Windows.Forms.TabPage
    Friend WithEvents MainPanel As System.Windows.Forms.Panel
    Friend WithEvents PatientsSSNlabel As System.Windows.Forms.Label
    Friend WithEvents PatientSSNTextBox As ExTextBox
    Friend WithEvents ProviderIDLabel As System.Windows.Forms.Label
    Friend WithEvents IncidentDateDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents DocTypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents ClaimIdLabel As System.Windows.Forms.Label
    Friend WithEvents ClaimIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents SearchCriteriaGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents AccidentCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents IncidentDateCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents AccidentGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents AccidentTypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents DocumentDetailsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents ProcedureCodeLabel As System.Windows.Forms.Label
    Friend WithEvents ProcedureCodeTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ModifierLabel As System.Windows.Forms.Label
    Friend WithEvents ModifierTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PlaceOfServiceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PlaceOfServiceLabel As System.Windows.Forms.Label
    Friend WithEvents ProviderCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents ProviderComboxBox As System.Windows.Forms.ComboBox
    Friend WithEvents BillTypeLabel As System.Windows.Forms.Label
    Friend WithEvents BillTypeTextBox As System.Windows.Forms.TextBox
    Friend WithEvents DiagnosisCodesTextBox As System.Windows.Forms.TextBox
    Friend WithEvents DiagnosisLabel As System.Windows.Forms.Label
    Friend WithEvents ParticipantSSNLabel As System.Windows.Forms.Label
    Friend WithEvents ParticipantSSNTextBox As ExTextBox
    Friend WithEvents FamilyIdLabel As System.Windows.Forms.Label
    Friend WithEvents FamilyIDTextBox As ExTextBox
    Friend WithEvents FamilyInformationGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents DateOfServiceCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents SearchButton As System.Windows.Forms.Button
    Friend WithEvents DocTypeCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents ResultsDataGridCustomContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResultsHistoryMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ResultsAnnotateMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProviderNameLabel As System.Windows.Forms.Label
    Friend WithEvents ProviderNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ResultsDisplayDocumentMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MatchesLabel As System.Windows.Forms.Label
    Friend WithEvents ShowStatusLabel As System.Windows.Forms.Label
    Friend WithEvents StatusTypesComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents CustomerServiceResultsDataGrid As DataGridCustom
    Friend WithEvents SearchingLabel As System.Windows.Forms.Label
    Friend WithEvents ResultsDisplayLineDetailsMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BetweenLabel As System.Windows.Forms.Label
    Friend WithEvents AndLabel As System.Windows.Forms.Label
    Friend WithEvents DateOfServiceFromDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateOfServiceToDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents ResultsAuditMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeniedCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents ResultsDisplayEligibiltyMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FreeTextMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClearAllButton As System.Windows.Forms.Button
    Friend WithEvents ToolTipTriggerLabel As System.Windows.Forms.Label
    Friend WithEvents ReprintEOBMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReprintEOBMMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReprintEOBPMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReprintEOBBothMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProcedureGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents ShowDocTypeLabel As System.Windows.Forms.Label
    Friend WithEvents DocTypesComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents AccumulatorsContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AccumulatorsMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProcessedDateToDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents ProcessedDateFromDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents ProcessedDateCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents AndLabel2 As System.Windows.Forms.Label
    Friend WithEvents BetweenLabel2 As System.Windows.Forms.Label
    Friend WithEvents DocIdLabel As System.Windows.Forms.Label
    Friend WithEvents DocIDTextBox As ExTextBox
    Friend WithEvents PatientLastNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PatientLastNameLabel As System.Windows.Forms.Label
    Friend WithEvents ChiroCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents ParticipantsFirstNameLabel As System.Windows.Forms.Label
    Friend WithEvents CancelButton As System.Windows.Forms.Button
    Friend WithEvents PatientFirstNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ReprocessMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemovePricingMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReOpenMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FamilyRelationMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents QueueImageList As System.Windows.Forms.ImageList
    Friend WithEvents MenuItemPaste As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EligibilityTab As System.Windows.Forms.TabPage
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ProcCodeButton As System.Windows.Forms.Button
    Friend WithEvents ModifiersButton As System.Windows.Forms.Button
    Friend WithEvents DiagnosisButton As System.Windows.Forms.Button
    Friend WithEvents ProviderNameLookupButton As System.Windows.Forms.Button
    Friend WithEvents ProviderIDLookupButton As System.Windows.Forms.Button
    Friend WithEvents ProviderIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents DemographicsTab As System.Windows.Forms.TabPage
    Friend WithEvents EligibilityDataSet As EligibilityDataSet
    Friend WithEvents HRAActivityTab As System.Windows.Forms.TabPage
    Friend WithEvents PremiumsHistoryTab As System.Windows.Forms.TabPage
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents PremiumsControl As PremiumsHistoryControl
    Friend WithEvents PremiumsEnrollmentControl As PremiumsEnrollmentControl
    Friend WithEvents AccumulatorsTab As System.Windows.Forms.TabPage
    Friend WithEvents RelationIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents BatchNumberLabel As System.Windows.Forms.Label
    Friend WithEvents BatchNumberTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ProviderTab As System.Windows.Forms.TabPage
    Friend WithEvents ProviderControl As ProviderControl
    Friend WithEvents RelationIdLabel As System.Windows.Forms.Label
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents PrescriptionsTab As System.Windows.Forms.TabPage
    Friend WithEvents PrescriptionsControl As PrescriptionsControl
    Friend WithEvents DentalTab As System.Windows.Forms.TabPage
    Friend WithEvents DentalControl As DentalControl
    Friend WithEvents EligibilitySplitContainer As System.Windows.Forms.SplitContainer
    Friend WithEvents EligibilityControl As EligibilityControl
    Friend WithEvents EligibilityDualCoverageControl As EligibilityControl
    Friend WithEvents AccumulatorsSplitContainer As System.Windows.Forms.SplitContainer
    Friend WithEvents AccumulatorValues As AccumulatorValues
    Friend WithEvents AccumulatorDualCoverageValues As AccumulatorValues
    Friend WithEvents FamilyGroupBoxViewButton As System.Windows.Forms.Button
    Friend WithEvents ClaimGroupBoxViewButton As System.Windows.Forms.Button
    Friend WithEvents PlaceofServiceButton As System.Windows.Forms.Button
    Friend WithEvents BillTypeButton As System.Windows.Forms.Button
    Friend WithEvents AdjusterFilterLabel As System.Windows.Forms.Label
    Friend WithEvents AdjustersComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents ReprocessPTCMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PatientSearchButton As System.Windows.Forms.Button
    Friend WithEvents HoursTab As System.Windows.Forms.TabPage
    Friend WithEvents HoursSplitContainer As System.Windows.Forms.SplitContainer
    Friend WithEvents HoursControl As HoursControl
    Friend WithEvents HoursDualCoverageControl As HoursControl
    Friend WithEvents NpiRegistryControl As NPIRegistryControl
    Friend WithEvents COBTab As System.Windows.Forms.TabPage
    Friend WithEvents CobControl As COBControl
    Friend WithEvents StatusLabel As System.Windows.Forms.Label
    Friend WithEvents StatusComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents AdjusterComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents AdjusterLabel As System.Windows.Forms.Label
    Friend WithEvents ImagingTab As System.Windows.Forms.TabPage
    Friend WithEvents UFCWDocsControl As UFCWDocsControl
    Friend WithEvents FreeTextTab As System.Windows.Forms.TabPage
    Friend WithEvents HRASplitContainer As System.Windows.Forms.SplitContainer
    Friend WithEvents HraBalanceControl As HRABalanceControl
    Friend WithEvents HraActivityControl As HRAActivityControl
    Friend WithEvents HrqControl As HRQControl
    Friend WithEvents HRQSplitContainer As System.Windows.Forms.SplitContainer
    Friend WithEvents PatientLookupButton As System.Windows.Forms.Button
    Friend WithEvents EligibilityHoursTab As System.Windows.Forms.TabPage
    Friend WithEvents EligibilityHoursSplitContainer As System.Windows.Forms.SplitContainer
    Friend WithEvents EligibilityHoursControl As EligibilityHoursControl
    Friend WithEvents EligibilityHoursDualCoverageControl As EligibilityHoursControl
    Friend WithEvents ResultViewByClaimIDMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PendToOriginalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PendToCurrentToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FreeTextEditor As FreeTextEditor
    Friend WithEvents FamilyInfoDataGridContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents LifeEventsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReprocessPTOMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CoverageTab As System.Windows.Forms.TabPage
    Friend WithEvents CoverageHistory As CoverageHistory
    Friend WithEvents ContactInfoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FamilySummaryRemarksToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AndLabel3 As System.Windows.Forms.Label
    Friend WithEvents BetweenLabel3 As System.Windows.Forms.Label
    Friend WithEvents ReceivedFromDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents ReceivedToDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents ReceivedDateCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents CheckGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents CheckNumberLabel As System.Windows.Forms.Label
    Friend WithEvents CheckAmountLabel As System.Windows.Forms.Label
    Friend WithEvents CheckAmountTextBox As System.Windows.Forms.TextBox
    Friend WithEvents CheckNumberTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PatientEligibilityToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitContainer3 As System.Windows.Forms.SplitContainer
    Friend WithEvents ParticipantDemographicsGrid As DataGridCustom
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents SplitContainer4 As System.Windows.Forms.SplitContainer
    Friend WithEvents DualParticipantDemographicsGrid As DataGridCustom
    Friend WithEvents BlinkTimer As System.Windows.Forms.Timer
    Friend WithEvents TaskTimer As System.Windows.Forms.Timer
    Friend WithEvents ParticipantToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents AddressLabel As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ParticipantHighAlertLabel As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ParticipantAlertLabel As System.Windows.Forms.ToolStripLabel
    Friend WithEvents DualParticipantToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents DualParticipantAddressLabel As System.Windows.Forms.ToolStripLabel
    Friend WithEvents DualParticipantHighAlertLabel As System.Windows.Forms.ToolStripLabel
    Friend WithEvents DualParticipantAlertLabel As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ClaimIDToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FamilyIDToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PatientToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AlertHistoryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CustomerServiceControl))
        Me.ReprocessPTOMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SearchCriteriaGroupBox = New System.Windows.Forms.GroupBox()
        Me.PatientSearchButton = New System.Windows.Forms.Button()
        Me.RelationIdLabel = New System.Windows.Forms.Label()
        Me.RelationIDTextBox = New System.Windows.Forms.TextBox()
        Me.CancelButton = New System.Windows.Forms.Button()
        Me.ClearAllButton = New System.Windows.Forms.Button()
        Me.SearchButton = New System.Windows.Forms.Button()
        Me.FamilyInformationGroupBox = New System.Windows.Forms.GroupBox()
        Me.PatientLookupButton = New System.Windows.Forms.Button()
        Me.FamilyGroupBoxViewButton = New System.Windows.Forms.Button()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ParticipantsFirstNameLabel = New System.Windows.Forms.Label()
        Me.PatientFirstNameTextBox = New System.Windows.Forms.TextBox()
        Me.PatientLastNameLabel = New System.Windows.Forms.Label()
        Me.PatientLastNameTextBox = New System.Windows.Forms.TextBox()
        Me.PatientsSSNlabel = New System.Windows.Forms.Label()
        Me.PatientSSNTextBox = New ExTextBox()
        Me.FamilyIDTextBox = New ExTextBox()
        Me.FamilyIdLabel = New System.Windows.Forms.Label()
        Me.DocumentDetailsGroupBox = New System.Windows.Forms.GroupBox()
        Me.ReceivedToDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.AndLabel3 = New System.Windows.Forms.Label()
        Me.BetweenLabel3 = New System.Windows.Forms.Label()
        Me.ReceivedFromDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.ReceivedDateCheckBox = New System.Windows.Forms.CheckBox()
        Me.CheckGroupBox = New System.Windows.Forms.GroupBox()
        Me.CheckNumberLabel = New System.Windows.Forms.Label()
        Me.CheckAmountLabel = New System.Windows.Forms.Label()
        Me.CheckAmountTextBox = New System.Windows.Forms.TextBox()
        Me.CheckNumberTextBox = New System.Windows.Forms.TextBox()
        Me.AdjusterComboBox = New System.Windows.Forms.ComboBox()
        Me.AdjusterLabel = New System.Windows.Forms.Label()
        Me.StatusComboBox = New System.Windows.Forms.ComboBox()
        Me.StatusLabel = New System.Windows.Forms.Label()
        Me.ClaimGroupBoxViewButton = New System.Windows.Forms.Button()
        Me.BatchNumberLabel = New System.Windows.Forms.Label()
        Me.BatchNumberTextBox = New System.Windows.Forms.TextBox()
        Me.DocIDTextBox = New ExTextBox()
        Me.DocIdLabel = New System.Windows.Forms.Label()
        Me.ProcessedDateToDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.AndLabel2 = New System.Windows.Forms.Label()
        Me.BetweenLabel2 = New System.Windows.Forms.Label()
        Me.ProcessedDateFromDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.ProcessedDateCheckBox = New System.Windows.Forms.CheckBox()
        Me.DeniedCheckBox = New System.Windows.Forms.CheckBox()
        Me.DateOfServiceToDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.AndLabel = New System.Windows.Forms.Label()
        Me.BetweenLabel = New System.Windows.Forms.Label()
        Me.DocTypeCheckBox = New System.Windows.Forms.CheckBox()
        Me.DateOfServiceFromDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.DateOfServiceCheckBox = New System.Windows.Forms.CheckBox()
        Me.ProcedureGroupBox = New System.Windows.Forms.GroupBox()
        Me.ReasonsTextBox = New System.Windows.Forms.TextBox()
        Me.ReasonsButton = New System.Windows.Forms.Button()
        Me.ReasonsLabel = New System.Windows.Forms.Label()
        Me.PlaceofServiceButton = New System.Windows.Forms.Button()
        Me.BillTypeButton = New System.Windows.Forms.Button()
        Me.DiagnosisButton = New System.Windows.Forms.Button()
        Me.DiagnosisCodesTextBox = New System.Windows.Forms.TextBox()
        Me.DiagnosisLabel = New System.Windows.Forms.Label()
        Me.BillTypeLabel = New System.Windows.Forms.Label()
        Me.BillTypeTextBox = New System.Windows.Forms.TextBox()
        Me.ProviderComboxBox = New System.Windows.Forms.ComboBox()
        Me.ProviderCheckBox = New System.Windows.Forms.CheckBox()
        Me.PlaceOfServiceTextBox = New System.Windows.Forms.TextBox()
        Me.PlaceOfServiceLabel = New System.Windows.Forms.Label()
        Me.ModifierLabel = New System.Windows.Forms.Label()
        Me.ModifierTextBox = New System.Windows.Forms.TextBox()
        Me.ProcedureCodeTextBox = New System.Windows.Forms.TextBox()
        Me.ProcedureCodeLabel = New System.Windows.Forms.Label()
        Me.ProcCodeButton = New System.Windows.Forms.Button()
        Me.ModifiersButton = New System.Windows.Forms.Button()
        Me.ChiroCheckBox = New System.Windows.Forms.CheckBox()
        Me.ClaimIdLabel = New System.Windows.Forms.Label()
        Me.ClaimIDTextBox = New System.Windows.Forms.TextBox()
        Me.DocTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.AccidentGroupBox = New System.Windows.Forms.GroupBox()
        Me.IncidentDateDateTimePicker = New System.Windows.Forms.DateTimePicker()
        Me.AccidentCheckBox = New System.Windows.Forms.CheckBox()
        Me.AccidentTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.IncidentDateCheckBox = New System.Windows.Forms.CheckBox()
        Me.ProviderNameLabel = New System.Windows.Forms.Label()
        Me.ProviderNameTextBox = New System.Windows.Forms.TextBox()
        Me.ProviderNameLookupButton = New System.Windows.Forms.Button()
        Me.ProviderIDLabel = New System.Windows.Forms.Label()
        Me.ProviderIDTextBox = New System.Windows.Forms.TextBox()
        Me.ParticipantSSNTextBox = New ExTextBox()
        Me.ParticipantSSNLabel = New System.Windows.Forms.Label()
        Me.ProviderIDLookupButton = New System.Windows.Forms.Button()
        Me.AccumulatorsContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AccumulatorsMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FamilyRelationMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemPaste = New System.Windows.Forms.ToolStripMenuItem()
        Me.SearchResultsGroupBox = New System.Windows.Forms.GroupBox()
        Me.TabControl = New System.Windows.Forms.TabControl()
        Me.GenTab = New System.Windows.Forms.TabPage()
        Me.MainPanel = New System.Windows.Forms.Panel()
        Me.AdjusterFilterLabel = New System.Windows.Forms.Label()
        Me.AdjustersComboBox = New System.Windows.Forms.ComboBox()
        Me.ShowDocTypeLabel = New System.Windows.Forms.Label()
        Me.DocTypesComboBox = New System.Windows.Forms.ComboBox()
        Me.MatchesLabel = New System.Windows.Forms.Label()
        Me.ShowStatusLabel = New System.Windows.Forms.Label()
        Me.StatusTypesComboBox = New System.Windows.Forms.ComboBox()
        Me.CustomerServiceResultsDataGrid = New DataGridCustom()
        Me.DemographicsTab = New System.Windows.Forms.TabPage()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer()
        Me.ParticipantToolStrip = New System.Windows.Forms.ToolStrip()
        Me.EligibilityAlertLabel = New System.Windows.Forms.ToolStripLabel()
        Me.AddressLabel = New System.Windows.Forms.ToolStripLabel()
        Me.ParticipantHighAlertLabel = New System.Windows.Forms.ToolStripLabel()
        Me.ParticipantAlertLabel = New System.Windows.Forms.ToolStripLabel()
        Me.ParticipantDemographicsGrid = New DataGridCustom()
        Me.SplitContainer4 = New System.Windows.Forms.SplitContainer()
        Me.DualParticipantToolStrip = New System.Windows.Forms.ToolStrip()
        Me.DualParticipantAddressLabel = New System.Windows.Forms.ToolStripLabel()
        Me.DualParticipantHighAlertLabel = New System.Windows.Forms.ToolStripLabel()
        Me.DualParticipantAlertLabel = New System.Windows.Forms.ToolStripLabel()
        Me.DualParticipantDemographicsGrid = New DataGridCustom()
        Me.EligibilityTab = New System.Windows.Forms.TabPage()
        Me.EligibilitySplitContainer = New System.Windows.Forms.SplitContainer()
        Me.EligibilityControl = New EligibilityControl()
        Me.EligibilityDualCoverageControl = New EligibilityControl()
        Me.EligibilityHoursTab = New System.Windows.Forms.TabPage()
        Me.EligibilityHoursSplitContainer = New System.Windows.Forms.SplitContainer()
        Me.EligibilityHoursControl = New EligibilityHoursControl()
        Me.EligibilityHoursDualCoverageControl = New EligibilityHoursControl()
        Me.HoursTab = New System.Windows.Forms.TabPage()
        Me.HoursSplitContainer = New System.Windows.Forms.SplitContainer()
        Me.HoursControl = New HoursControl()
        Me.HoursDualCoverageControl = New HoursControl()
        Me.PremiumsHistoryTab = New System.Windows.Forms.TabPage()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.PremiumsControl = New PremiumsHistoryControl()
        Me.PremiumsEnrollmentControl = New PremiumsEnrollmentControl()
        Me.CoverageTab = New System.Windows.Forms.TabPage()
        Me.CoverageHistory = New CoverageHistory()
        Me.HRAActivityTab = New System.Windows.Forms.TabPage()
        Me.HRASplitContainer = New System.Windows.Forms.SplitContainer()
        Me.HRQSplitContainer = New System.Windows.Forms.SplitContainer()
        Me.HraBalanceControl = New HRABalanceControl()
        Me.HrqControl = New HRQControl()
        Me.HraActivityControl = New HRAActivityControl()
        Me.AccumulatorsTab = New System.Windows.Forms.TabPage()
        Me.AccumulatorsSplitContainer = New System.Windows.Forms.SplitContainer()
        Me.AccumulatorValues = New AccumulatorValues()
        Me.AccumulatorDualCoverageValues = New AccumulatorValues()
        Me.ProviderTab = New System.Windows.Forms.TabPage()
        Me.NpiRegistryControl = New NPIRegistryControl()
        Me.ProviderControl = New ProviderControl()
        Me.PrescriptionsTab = New System.Windows.Forms.TabPage()
        Me.PrescriptionsControl = New PrescriptionsControl()
        Me.DentalTab = New System.Windows.Forms.TabPage()
        Me.DentalControl = New DentalControl()
        Me.COBTab = New System.Windows.Forms.TabPage()
        Me.CobControl = New COBControl()
        Me.FreeTextTab = New System.Windows.Forms.TabPage()
        Me.FreeTextEditor = New FreeTextEditor()
        Me.ImagingTab = New System.Windows.Forms.TabPage()
        Me.UFCWDocsControl = New UFCWDocsControl()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.SearchingLabel = New System.Windows.Forms.Label()
        Me.FamilyInfoDataGridContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.LifeEventsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContactInfoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FamilySummaryRemarksToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PatientEligibilityToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AlertHistoryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DiseaseManagementMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResultsDataGridCustomContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResultsDisplayDocumentMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResultsHistoryMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResultsDisplayLineDetailsMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResultViewByClaimIDMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClaimIDToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FamilyIDToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PatientToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FreeTextMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResultsAnnotateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResultsAuditMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResultsDisplayEligibiltyMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReprintEOBMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReprintEOBMMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReprintEOBPMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReprintEOBBothMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReprocessMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReprocessPTCMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemovePricingMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReOpenMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ToolTipTriggerLabel = New System.Windows.Forms.Label()
        Me.QueueImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.PendToOriginalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PendToCurrentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BlinkTimer = New System.Windows.Forms.Timer(Me.components)
        Me.TaskTimer = New System.Windows.Forms.Timer(Me.components)
        Me.EligibilityDataSet = New EligibilityDataSet()
        Me.SearchCriteriaGroupBox.SuspendLayout()
        Me.FamilyInformationGroupBox.SuspendLayout()
        Me.DocumentDetailsGroupBox.SuspendLayout()
        Me.CheckGroupBox.SuspendLayout()
        Me.ProcedureGroupBox.SuspendLayout()
        Me.AccidentGroupBox.SuspendLayout()
        Me.AccumulatorsContextMenu.SuspendLayout()
        Me.SearchResultsGroupBox.SuspendLayout()
        Me.TabControl.SuspendLayout()
        Me.GenTab.SuspendLayout()
        Me.MainPanel.SuspendLayout()
        CType(Me.CustomerServiceResultsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DemographicsTab.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer3.Panel1.SuspendLayout()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        Me.ParticipantToolStrip.SuspendLayout()
        CType(Me.ParticipantDemographicsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer4.Panel1.SuspendLayout()
        Me.SplitContainer4.Panel2.SuspendLayout()
        Me.SplitContainer4.SuspendLayout()
        Me.DualParticipantToolStrip.SuspendLayout()
        CType(Me.DualParticipantDemographicsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.EligibilityTab.SuspendLayout()
        CType(Me.EligibilitySplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.EligibilitySplitContainer.Panel1.SuspendLayout()
        Me.EligibilitySplitContainer.Panel2.SuspendLayout()
        Me.EligibilitySplitContainer.SuspendLayout()
        Me.EligibilityHoursTab.SuspendLayout()
        CType(Me.EligibilityHoursSplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.EligibilityHoursSplitContainer.Panel1.SuspendLayout()
        Me.EligibilityHoursSplitContainer.Panel2.SuspendLayout()
        Me.EligibilityHoursSplitContainer.SuspendLayout()
        Me.HoursTab.SuspendLayout()
        CType(Me.HoursSplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.HoursSplitContainer.Panel1.SuspendLayout()
        Me.HoursSplitContainer.Panel2.SuspendLayout()
        Me.HoursSplitContainer.SuspendLayout()
        Me.PremiumsHistoryTab.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.CoverageTab.SuspendLayout()
        Me.HRAActivityTab.SuspendLayout()
        CType(Me.HRASplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.HRASplitContainer.Panel1.SuspendLayout()
        Me.HRASplitContainer.Panel2.SuspendLayout()
        Me.HRASplitContainer.SuspendLayout()
        CType(Me.HRQSplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.HRQSplitContainer.Panel1.SuspendLayout()
        Me.HRQSplitContainer.Panel2.SuspendLayout()
        Me.HRQSplitContainer.SuspendLayout()
        Me.AccumulatorsTab.SuspendLayout()
        CType(Me.AccumulatorsSplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.AccumulatorsSplitContainer.Panel1.SuspendLayout()
        Me.AccumulatorsSplitContainer.Panel2.SuspendLayout()
        Me.AccumulatorsSplitContainer.SuspendLayout()
        Me.ProviderTab.SuspendLayout()
        Me.PrescriptionsTab.SuspendLayout()
        Me.DentalTab.SuspendLayout()
        Me.COBTab.SuspendLayout()
        Me.FreeTextTab.SuspendLayout()
        Me.ImagingTab.SuspendLayout()
        Me.FamilyInfoDataGridContextMenuStrip.SuspendLayout()
        Me.ResultsDataGridCustomContextMenu.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EligibilityDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ReprocessPTOMenuItem
        '
        Me.ReprocessPTOMenuItem.Name = "ReprocessPTOMenuItem"
        Me.ReprocessPTOMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.ReprocessPTOMenuItem.Text = "Pend To Original"
        '
        'SearchCriteriaGroupBox
        '
        Me.SearchCriteriaGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SearchCriteriaGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.SearchCriteriaGroupBox.Controls.Add(Me.PatientSearchButton)
        Me.SearchCriteriaGroupBox.Controls.Add(Me.RelationIdLabel)
        Me.SearchCriteriaGroupBox.Controls.Add(Me.RelationIDTextBox)
        Me.SearchCriteriaGroupBox.Controls.Add(Me.CancelButton)
        Me.SearchCriteriaGroupBox.Controls.Add(Me.ClearAllButton)
        Me.SearchCriteriaGroupBox.Controls.Add(Me.SearchButton)
        Me.SearchCriteriaGroupBox.Controls.Add(Me.FamilyInformationGroupBox)
        Me.SearchCriteriaGroupBox.Controls.Add(Me.FamilyIDTextBox)
        Me.SearchCriteriaGroupBox.Controls.Add(Me.FamilyIdLabel)
        Me.SearchCriteriaGroupBox.Controls.Add(Me.DocumentDetailsGroupBox)
        Me.SearchCriteriaGroupBox.Controls.Add(Me.ProviderIDLabel)
        Me.SearchCriteriaGroupBox.Controls.Add(Me.ProviderIDTextBox)
        Me.SearchCriteriaGroupBox.Controls.Add(Me.ParticipantSSNTextBox)
        Me.SearchCriteriaGroupBox.Controls.Add(Me.ParticipantSSNLabel)
        Me.SearchCriteriaGroupBox.Controls.Add(Me.ProviderIDLookupButton)
        Me.SearchCriteriaGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.SearchCriteriaGroupBox.Location = New System.Drawing.Point(8, 16)
        Me.SearchCriteriaGroupBox.Name = "SearchCriteriaGroupBox"
        Me.SearchCriteriaGroupBox.Size = New System.Drawing.Size(740, 422)
        Me.SearchCriteriaGroupBox.TabIndex = 1
        Me.SearchCriteriaGroupBox.TabStop = False
        Me.SearchCriteriaGroupBox.Text = "Search Criteria"
        '
        'PatientSearchButton
        '
        Me.PatientSearchButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PatientSearchButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.PatientSearchButton.Location = New System.Drawing.Point(111, 392)
        Me.PatientSearchButton.Name = "PatientSearchButton"
        Me.PatientSearchButton.Size = New System.Drawing.Size(75, 23)
        Me.PatientSearchButton.TabIndex = 14
        Me.PatientSearchButton.Text = "Get Patient"
        Me.ToolTip1.SetToolTip(Me.PatientSearchButton, "Performs a search using the claim's patient information")
        Me.PatientSearchButton.Visible = False
        '
        'RelationIdLabel
        '
        Me.RelationIdLabel.AutoSize = True
        Me.RelationIdLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RelationIdLabel.Location = New System.Drawing.Point(287, 23)
        Me.RelationIdLabel.Name = "RelationIdLabel"
        Me.RelationIdLabel.Size = New System.Drawing.Size(40, 13)
        Me.RelationIdLabel.TabIndex = 4
        Me.RelationIdLabel.Text = "Rel ID:"
        Me.RelationIdLabel.UseMnemonic = False
        '
        'RelationIDTextBox
        '
        Me.RelationIDTextBox.Location = New System.Drawing.Point(327, 20)
        Me.RelationIDTextBox.MaxLength = 3
        Me.RelationIDTextBox.Name = "RelationIDTextBox"
        Me.RelationIDTextBox.Size = New System.Drawing.Size(24, 20)
        Me.RelationIDTextBox.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.RelationIDTextBox, "The Relation ID, (Family ID Required)")
        '
        'CancelButton
        '
        Me.CancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelButton.Enabled = False
        Me.CancelButton.Location = New System.Drawing.Point(307, 392)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelButton.TabIndex = 13
        Me.CancelButton.Text = "Cancel"
        Me.ToolTip1.SetToolTip(Me.CancelButton, "Cancel the Search")
        Me.CancelButton.Visible = False
        '
        'ClearAllButton
        '
        Me.ClearAllButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ClearAllButton.CausesValidation = False
        Me.ClearAllButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ClearAllButton.Location = New System.Drawing.Point(209, 392)
        Me.ClearAllButton.Name = "ClearAllButton"
        Me.ClearAllButton.Size = New System.Drawing.Size(75, 23)
        Me.ClearAllButton.TabIndex = 12
        Me.ClearAllButton.Text = "Clear All"
        '
        'SearchButton
        '
        Me.SearchButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SearchButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.SearchButton.Location = New System.Drawing.Point(13, 392)
        Me.SearchButton.Name = "SearchButton"
        Me.SearchButton.Size = New System.Drawing.Size(75, 23)
        Me.SearchButton.TabIndex = 11
        Me.SearchButton.Text = "Search"
        '
        'FamilyInformationGroupBox
        '
        Me.FamilyInformationGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyInformationGroupBox.Controls.Add(Me.PatientLookupButton)
        Me.FamilyInformationGroupBox.Controls.Add(Me.FamilyGroupBoxViewButton)
        Me.FamilyInformationGroupBox.Controls.Add(Me.ParticipantsFirstNameLabel)
        Me.FamilyInformationGroupBox.Controls.Add(Me.PatientFirstNameTextBox)
        Me.FamilyInformationGroupBox.Controls.Add(Me.PatientLastNameLabel)
        Me.FamilyInformationGroupBox.Controls.Add(Me.PatientLastNameTextBox)
        Me.FamilyInformationGroupBox.Controls.Add(Me.PatientsSSNlabel)
        Me.FamilyInformationGroupBox.Controls.Add(Me.PatientSSNTextBox)
        Me.FamilyInformationGroupBox.Location = New System.Drawing.Point(8, 45)
        Me.FamilyInformationGroupBox.Name = "FamilyInformationGroupBox"
        Me.FamilyInformationGroupBox.Size = New System.Drawing.Size(726, 71)
        Me.FamilyInformationGroupBox.TabIndex = 9
        Me.FamilyInformationGroupBox.TabStop = False
        Me.FamilyInformationGroupBox.Text = "Patient Information"
        '
        'PatientLookupButton
        '
        Me.PatientLookupButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.PatientLookupButton.Location = New System.Drawing.Point(423, 47)
        Me.PatientLookupButton.Name = "PatientLookupButton"
        Me.PatientLookupButton.Size = New System.Drawing.Size(24, 21)
        Me.PatientLookupButton.TabIndex = 7
        Me.PatientLookupButton.TabStop = False
        Me.PatientLookupButton.Text = "?"
        Me.ToolTip1.SetToolTip(Me.PatientLookupButton, "Display a dialog to search by Name or SSN")
        '
        'FamilyGroupBoxViewButton
        '
        Me.FamilyGroupBoxViewButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyGroupBoxViewButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.FamilyGroupBoxViewButton.FlatAppearance.BorderSize = 0
        Me.FamilyGroupBoxViewButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.FamilyGroupBoxViewButton.ImageIndex = 0
        Me.FamilyGroupBoxViewButton.ImageList = Me.ImageList1
        Me.FamilyGroupBoxViewButton.Location = New System.Drawing.Point(705, 9)
        Me.FamilyGroupBoxViewButton.Margin = New System.Windows.Forms.Padding(0)
        Me.FamilyGroupBoxViewButton.Name = "FamilyGroupBoxViewButton"
        Me.FamilyGroupBoxViewButton.Size = New System.Drawing.Size(16, 11)
        Me.FamilyGroupBoxViewButton.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.FamilyGroupBoxViewButton, "Hide/Show Family Search Panel")
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.SystemColors.Control
        Me.ImageList1.Images.SetKeyName(0, "ExpandRibbon.jpg")
        Me.ImageList1.Images.SetKeyName(1, "CollapseRibbon.JPG")
        '
        'ParticipantsFirstNameLabel
        '
        Me.ParticipantsFirstNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ParticipantsFirstNameLabel.Location = New System.Drawing.Point(219, 50)
        Me.ParticipantsFirstNameLabel.Name = "ParticipantsFirstNameLabel"
        Me.ParticipantsFirstNameLabel.Size = New System.Drawing.Size(101, 14)
        Me.ParticipantsFirstNameLabel.TabIndex = 5
        Me.ParticipantsFirstNameLabel.Text = "Patient's First Name:"
        '
        'PatientFirstNameTextBox
        '
        Me.PatientFirstNameTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.PatientFirstNameTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PatientFirstNameTextBox.Location = New System.Drawing.Point(324, 47)
        Me.PatientFirstNameTextBox.MaxLength = 11
        Me.PatientFirstNameTextBox.Name = "PatientFirstNameTextBox"
        Me.PatientFirstNameTextBox.Size = New System.Drawing.Size(96, 20)
        Me.PatientFirstNameTextBox.TabIndex = 6
        Me.ToolTip1.SetToolTip(Me.PatientFirstNameTextBox, "The Patient's first name")
        '
        'PatientLastNameLabel
        '
        Me.PatientLastNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.PatientLastNameLabel.Location = New System.Drawing.Point(8, 49)
        Me.PatientLastNameLabel.Name = "PatientLastNameLabel"
        Me.PatientLastNameLabel.Size = New System.Drawing.Size(102, 16)
        Me.PatientLastNameLabel.TabIndex = 3
        Me.PatientLastNameLabel.Text = "Patient's Last Name:"
        '
        'PatientLastNameTextBox
        '
        Me.PatientLastNameTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.PatientLastNameTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PatientLastNameTextBox.Location = New System.Drawing.Point(116, 47)
        Me.PatientLastNameTextBox.MaxLength = 11
        Me.PatientLastNameTextBox.Name = "PatientLastNameTextBox"
        Me.PatientLastNameTextBox.Size = New System.Drawing.Size(96, 20)
        Me.PatientLastNameTextBox.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.PatientLastNameTextBox, "The Patient's last name")
        '
        'PatientsSSNlabel
        '
        Me.PatientsSSNlabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.PatientsSSNlabel.Location = New System.Drawing.Point(8, 28)
        Me.PatientsSSNlabel.Name = "PatientsSSNlabel"
        Me.PatientsSSNlabel.Size = New System.Drawing.Size(72, 13)
        Me.PatientsSSNlabel.TabIndex = 1
        Me.PatientsSSNlabel.Text = "Patient's SSN:"
        '
        'PatientSSNTextBox
        '
        Me.PatientSSNTextBox.Location = New System.Drawing.Point(116, 24)
        Me.PatientSSNTextBox.MaxLength = 11
        Me.PatientSSNTextBox.Name = "PatientSSNTextBox"
        Me.PatientSSNTextBox.Size = New System.Drawing.Size(71, 20)
        Me.PatientSSNTextBox.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.PatientSSNTextBox, "The Patient's SSN")
        '
        'FamilyIDTextBox
        '
        Me.FamilyIDTextBox.Location = New System.Drawing.Point(222, 20)
        Me.FamilyIDTextBox.MaxLength = 10
        Me.FamilyIDTextBox.Name = "FamilyIDTextBox"
        Me.FamilyIDTextBox.Size = New System.Drawing.Size(63, 20)
        Me.FamilyIDTextBox.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.FamilyIDTextBox, "The family ID ")
        '
        'FamilyIdLabel
        '
        Me.FamilyIdLabel.AutoSize = True
        Me.FamilyIdLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.FamilyIdLabel.Location = New System.Drawing.Point(170, 22)
        Me.FamilyIdLabel.Name = "FamilyIdLabel"
        Me.FamilyIdLabel.Size = New System.Drawing.Size(53, 13)
        Me.FamilyIdLabel.TabIndex = 2
        Me.FamilyIdLabel.Text = "Family ID:"
        '
        'DocumentDetailsGroupBox
        '
        Me.DocumentDetailsGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DocumentDetailsGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.DocumentDetailsGroupBox.Controls.Add(Me.ReceivedToDateTimePicker)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.AndLabel3)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.BetweenLabel3)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.ReceivedFromDateTimePicker)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.ReceivedDateCheckBox)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.CheckGroupBox)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.AdjusterComboBox)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.AdjusterLabel)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.StatusComboBox)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.StatusLabel)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.ClaimGroupBoxViewButton)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.BatchNumberLabel)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.BatchNumberTextBox)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.DocIDTextBox)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.DocIdLabel)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.ProcessedDateToDateTimePicker)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.AndLabel2)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.BetweenLabel2)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.ProcessedDateFromDateTimePicker)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.ProcessedDateCheckBox)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.DeniedCheckBox)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.DateOfServiceToDateTimePicker)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.AndLabel)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.BetweenLabel)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.DocTypeCheckBox)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.DateOfServiceFromDateTimePicker)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.DateOfServiceCheckBox)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.ProcedureGroupBox)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.ChiroCheckBox)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.ClaimIdLabel)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.ClaimIDTextBox)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.DocTypeComboBox)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.AccidentGroupBox)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.ProviderNameLabel)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.ProviderNameTextBox)
        Me.DocumentDetailsGroupBox.Controls.Add(Me.ProviderNameLookupButton)
        Me.DocumentDetailsGroupBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.DocumentDetailsGroupBox.Location = New System.Drawing.Point(8, 118)
        Me.DocumentDetailsGroupBox.Name = "DocumentDetailsGroupBox"
        Me.DocumentDetailsGroupBox.Size = New System.Drawing.Size(726, 264)
        Me.DocumentDetailsGroupBox.TabIndex = 10
        Me.DocumentDetailsGroupBox.TabStop = False
        Me.DocumentDetailsGroupBox.Text = "Claim/Document Details"
        '
        'ReceivedToDateTimePicker
        '
        Me.ReceivedToDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.ReceivedToDateTimePicker.Location = New System.Drawing.Point(290, 76)
        Me.ReceivedToDateTimePicker.Name = "ReceivedToDateTimePicker"
        Me.ReceivedToDateTimePicker.Size = New System.Drawing.Size(95, 20)
        Me.ReceivedToDateTimePicker.TabIndex = 40
        Me.ToolTip1.SetToolTip(Me.ReceivedToDateTimePicker, "The Date of Service")
        '
        'AndLabel3
        '
        Me.AndLabel3.Location = New System.Drawing.Point(259, 76)
        Me.AndLabel3.Name = "AndLabel3"
        Me.AndLabel3.Size = New System.Drawing.Size(32, 16)
        Me.AndLabel3.TabIndex = 39
        Me.AndLabel3.Text = "And"
        '
        'BetweenLabel3
        '
        Me.BetweenLabel3.Location = New System.Drawing.Point(108, 76)
        Me.BetweenLabel3.Name = "BetweenLabel3"
        Me.BetweenLabel3.Size = New System.Drawing.Size(49, 16)
        Me.BetweenLabel3.TabIndex = 37
        Me.BetweenLabel3.Text = "Between"
        '
        'ReceivedFromDateTimePicker
        '
        Me.ReceivedFromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.ReceivedFromDateTimePicker.Location = New System.Drawing.Point(161, 76)
        Me.ReceivedFromDateTimePicker.Name = "ReceivedFromDateTimePicker"
        Me.ReceivedFromDateTimePicker.Size = New System.Drawing.Size(95, 20)
        Me.ReceivedFromDateTimePicker.TabIndex = 38
        Me.ToolTip1.SetToolTip(Me.ReceivedFromDateTimePicker, "The Received Date")
        '
        'ReceivedDateCheckBox
        '
        Me.ReceivedDateCheckBox.Location = New System.Drawing.Point(8, 76)
        Me.ReceivedDateCheckBox.Name = "ReceivedDateCheckBox"
        Me.ReceivedDateCheckBox.Size = New System.Drawing.Size(104, 16)
        Me.ReceivedDateCheckBox.TabIndex = 36
        Me.ReceivedDateCheckBox.Text = "Received Date"
        Me.ToolTip1.SetToolTip(Me.ReceivedDateCheckBox, "Check to restrict selection by Received Date")
        '
        'CheckGroupBox
        '
        Me.CheckGroupBox.Controls.Add(Me.CheckNumberLabel)
        Me.CheckGroupBox.Controls.Add(Me.CheckAmountLabel)
        Me.CheckGroupBox.Controls.Add(Me.CheckAmountTextBox)
        Me.CheckGroupBox.Controls.Add(Me.CheckNumberTextBox)
        Me.CheckGroupBox.Location = New System.Drawing.Point(389, 66)
        Me.CheckGroupBox.Name = "CheckGroupBox"
        Me.CheckGroupBox.Size = New System.Drawing.Size(264, 32)
        Me.CheckGroupBox.TabIndex = 35
        Me.CheckGroupBox.TabStop = False
        Me.CheckGroupBox.Text = "Check"
        '
        'CheckNumberLabel
        '
        Me.CheckNumberLabel.Location = New System.Drawing.Point(6, 13)
        Me.CheckNumberLabel.Name = "CheckNumberLabel"
        Me.CheckNumberLabel.Size = New System.Drawing.Size(27, 13)
        Me.CheckNumberLabel.TabIndex = 18
        Me.CheckNumberLabel.Text = "Nbr:"
        '
        'CheckAmountLabel
        '
        Me.CheckAmountLabel.Location = New System.Drawing.Point(122, 13)
        Me.CheckAmountLabel.Name = "CheckAmountLabel"
        Me.CheckAmountLabel.Size = New System.Drawing.Size(61, 13)
        Me.CheckAmountLabel.TabIndex = 17
        Me.CheckAmountLabel.Text = "Amount (>):"
        '
        'CheckAmountTextBox
        '
        Me.CheckAmountTextBox.Location = New System.Drawing.Point(186, 9)
        Me.CheckAmountTextBox.Name = "CheckAmountTextBox"
        Me.CheckAmountTextBox.Size = New System.Drawing.Size(72, 20)
        Me.CheckAmountTextBox.TabIndex = 16
        Me.ToolTip1.SetToolTip(Me.CheckAmountTextBox, "The amount of the amount of the claim")
        '
        'CheckNumberTextBox
        '
        Me.CheckNumberTextBox.Location = New System.Drawing.Point(41, 9)
        Me.CheckNumberTextBox.MaxLength = 12
        Me.CheckNumberTextBox.Name = "CheckNumberTextBox"
        Me.CheckNumberTextBox.Size = New System.Drawing.Size(78, 20)
        Me.CheckNumberTextBox.TabIndex = 8
        Me.CheckNumberTextBox.WordWrap = False
        '
        'AdjusterComboBox
        '
        Me.AdjusterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.AdjusterComboBox.DropDownWidth = 250
        Me.AdjusterComboBox.Enabled = False
        Me.AdjusterComboBox.Items.AddRange(New Object() {"(all)"})
        Me.AdjusterComboBox.Location = New System.Drawing.Point(450, 120)
        Me.AdjusterComboBox.Name = "AdjusterComboBox"
        Me.AdjusterComboBox.Size = New System.Drawing.Size(129, 21)
        Me.AdjusterComboBox.TabIndex = 34
        Me.ToolTip1.SetToolTip(Me.AdjusterComboBox, "Status")
        Me.AdjusterComboBox.Visible = False
        '
        'AdjusterLabel
        '
        Me.AdjusterLabel.Location = New System.Drawing.Point(393, 120)
        Me.AdjusterLabel.Name = "AdjusterLabel"
        Me.AdjusterLabel.Size = New System.Drawing.Size(45, 16)
        Me.AdjusterLabel.TabIndex = 33
        Me.AdjusterLabel.Text = "Adjuster:"
        Me.AdjusterLabel.Visible = False
        '
        'StatusComboBox
        '
        Me.StatusComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.StatusComboBox.DropDownWidth = 250
        Me.StatusComboBox.Enabled = False
        Me.StatusComboBox.Items.AddRange(New Object() {"(all)"})
        Me.StatusComboBox.Location = New System.Drawing.Point(450, 98)
        Me.StatusComboBox.Name = "StatusComboBox"
        Me.StatusComboBox.Size = New System.Drawing.Size(129, 21)
        Me.StatusComboBox.TabIndex = 32
        Me.ToolTip1.SetToolTip(Me.StatusComboBox, "Status")
        Me.StatusComboBox.Visible = False
        '
        'StatusLabel
        '
        Me.StatusLabel.Location = New System.Drawing.Point(393, 98)
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(40, 16)
        Me.StatusLabel.TabIndex = 31
        Me.StatusLabel.Text = "Status:"
        Me.StatusLabel.Visible = False
        '
        'ClaimGroupBoxViewButton
        '
        Me.ClaimGroupBoxViewButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClaimGroupBoxViewButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClaimGroupBoxViewButton.FlatAppearance.BorderSize = 0
        Me.ClaimGroupBoxViewButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ClaimGroupBoxViewButton.ImageIndex = 0
        Me.ClaimGroupBoxViewButton.ImageList = Me.ImageList1
        Me.ClaimGroupBoxViewButton.Location = New System.Drawing.Point(705, 9)
        Me.ClaimGroupBoxViewButton.Margin = New System.Windows.Forms.Padding(0)
        Me.ClaimGroupBoxViewButton.Name = "ClaimGroupBoxViewButton"
        Me.ClaimGroupBoxViewButton.Size = New System.Drawing.Size(16, 11)
        Me.ClaimGroupBoxViewButton.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.ClaimGroupBoxViewButton, "Hide/Show Claim Search Panel")
        '
        'BatchNumberLabel
        '
        Me.BatchNumberLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.BatchNumberLabel.Location = New System.Drawing.Point(393, 46)
        Me.BatchNumberLabel.Name = "BatchNumberLabel"
        Me.BatchNumberLabel.Size = New System.Drawing.Size(34, 16)
        Me.BatchNumberLabel.TabIndex = 12
        Me.BatchNumberLabel.Text = "Batch:"
        '
        'BatchNumberTextBox
        '
        Me.BatchNumberTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.BatchNumberTextBox.Location = New System.Drawing.Point(431, 46)
        Me.BatchNumberTextBox.Name = "BatchNumberTextBox"
        Me.BatchNumberTextBox.Size = New System.Drawing.Size(216, 20)
        Me.BatchNumberTextBox.TabIndex = 13
        Me.ToolTip1.SetToolTip(Me.BatchNumberTextBox, "The Batch Number")
        '
        'DocIDTextBox
        '
        Me.DocIDTextBox.Location = New System.Drawing.Point(299, 46)
        Me.DocIDTextBox.MaxLength = 12
        Me.DocIDTextBox.Name = "DocIDTextBox"
        Me.DocIDTextBox.Size = New System.Drawing.Size(78, 20)
        Me.DocIDTextBox.TabIndex = 11
        Me.ToolTip1.SetToolTip(Me.DocIDTextBox, "The Document ID")
        Me.DocIDTextBox.WordWrap = False
        '
        'DocIdLabel
        '
        Me.DocIdLabel.Location = New System.Drawing.Point(254, 46)
        Me.DocIdLabel.Name = "DocIdLabel"
        Me.DocIdLabel.Size = New System.Drawing.Size(44, 16)
        Me.DocIdLabel.TabIndex = 10
        Me.DocIdLabel.Text = "Doc ID:"
        '
        'ProcessedDateToDateTimePicker
        '
        Me.ProcessedDateToDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.ProcessedDateToDateTimePicker.Location = New System.Drawing.Point(290, 120)
        Me.ProcessedDateToDateTimePicker.Name = "ProcessedDateToDateTimePicker"
        Me.ProcessedDateToDateTimePicker.Size = New System.Drawing.Size(95, 20)
        Me.ProcessedDateToDateTimePicker.TabIndex = 27
        Me.ToolTip1.SetToolTip(Me.ProcessedDateToDateTimePicker, "The Date of Completion")
        '
        'AndLabel2
        '
        Me.AndLabel2.Location = New System.Drawing.Point(259, 120)
        Me.AndLabel2.Name = "AndLabel2"
        Me.AndLabel2.Size = New System.Drawing.Size(32, 16)
        Me.AndLabel2.TabIndex = 26
        Me.AndLabel2.Text = "And"
        '
        'BetweenLabel2
        '
        Me.BetweenLabel2.Location = New System.Drawing.Point(108, 120)
        Me.BetweenLabel2.Name = "BetweenLabel2"
        Me.BetweenLabel2.Size = New System.Drawing.Size(49, 16)
        Me.BetweenLabel2.TabIndex = 24
        Me.BetweenLabel2.Text = "Between"
        '
        'ProcessedDateFromDateTimePicker
        '
        Me.ProcessedDateFromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.ProcessedDateFromDateTimePicker.Location = New System.Drawing.Point(161, 120)
        Me.ProcessedDateFromDateTimePicker.Name = "ProcessedDateFromDateTimePicker"
        Me.ProcessedDateFromDateTimePicker.Size = New System.Drawing.Size(95, 20)
        Me.ProcessedDateFromDateTimePicker.TabIndex = 25
        Me.ToolTip1.SetToolTip(Me.ProcessedDateFromDateTimePicker, "The Date of Completion")
        '
        'ProcessedDateCheckBox
        '
        Me.ProcessedDateCheckBox.Location = New System.Drawing.Point(8, 120)
        Me.ProcessedDateCheckBox.Name = "ProcessedDateCheckBox"
        Me.ProcessedDateCheckBox.Size = New System.Drawing.Size(104, 16)
        Me.ProcessedDateCheckBox.TabIndex = 23
        Me.ProcessedDateCheckBox.Text = "Processed Date"
        Me.ToolTip1.SetToolTip(Me.ProcessedDateCheckBox, "Check to use Processed Date")
        '
        'DeniedCheckBox
        '
        Me.DeniedCheckBox.Location = New System.Drawing.Point(488, 24)
        Me.DeniedCheckBox.Name = "DeniedCheckBox"
        Me.DeniedCheckBox.Size = New System.Drawing.Size(64, 16)
        Me.DeniedCheckBox.TabIndex = 28
        Me.DeniedCheckBox.Text = "Denied"
        '
        'DateOfServiceToDateTimePicker
        '
        Me.DateOfServiceToDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateOfServiceToDateTimePicker.Location = New System.Drawing.Point(290, 98)
        Me.DateOfServiceToDateTimePicker.Name = "DateOfServiceToDateTimePicker"
        Me.DateOfServiceToDateTimePicker.Size = New System.Drawing.Size(95, 20)
        Me.DateOfServiceToDateTimePicker.TabIndex = 22
        Me.ToolTip1.SetToolTip(Me.DateOfServiceToDateTimePicker, "The Date of Service")
        '
        'AndLabel
        '
        Me.AndLabel.Location = New System.Drawing.Point(259, 98)
        Me.AndLabel.Name = "AndLabel"
        Me.AndLabel.Size = New System.Drawing.Size(32, 16)
        Me.AndLabel.TabIndex = 21
        Me.AndLabel.Text = "And"
        '
        'BetweenLabel
        '
        Me.BetweenLabel.Location = New System.Drawing.Point(108, 98)
        Me.BetweenLabel.Name = "BetweenLabel"
        Me.BetweenLabel.Size = New System.Drawing.Size(49, 16)
        Me.BetweenLabel.TabIndex = 19
        Me.BetweenLabel.Text = "Between"
        '
        'DocTypeCheckBox
        '
        Me.DocTypeCheckBox.Location = New System.Drawing.Point(8, 46)
        Me.DocTypeCheckBox.Name = "DocTypeCheckBox"
        Me.DocTypeCheckBox.Size = New System.Drawing.Size(82, 24)
        Me.DocTypeCheckBox.TabIndex = 8
        Me.DocTypeCheckBox.Text = "Doc Type:"
        '
        'DateOfServiceFromDateTimePicker
        '
        Me.DateOfServiceFromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateOfServiceFromDateTimePicker.Location = New System.Drawing.Point(161, 98)
        Me.DateOfServiceFromDateTimePicker.Name = "DateOfServiceFromDateTimePicker"
        Me.DateOfServiceFromDateTimePicker.Size = New System.Drawing.Size(95, 20)
        Me.DateOfServiceFromDateTimePicker.TabIndex = 20
        Me.ToolTip1.SetToolTip(Me.DateOfServiceFromDateTimePicker, "The Date of Service")
        '
        'DateOfServiceCheckBox
        '
        Me.DateOfServiceCheckBox.Location = New System.Drawing.Point(8, 98)
        Me.DateOfServiceCheckBox.Name = "DateOfServiceCheckBox"
        Me.DateOfServiceCheckBox.Size = New System.Drawing.Size(104, 16)
        Me.DateOfServiceCheckBox.TabIndex = 18
        Me.DateOfServiceCheckBox.Text = "Date of Service"
        Me.ToolTip1.SetToolTip(Me.DateOfServiceCheckBox, "Check to use Date of Service")
        '
        'ProcedureGroupBox
        '
        Me.ProcedureGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProcedureGroupBox.Controls.Add(Me.ReasonsTextBox)
        Me.ProcedureGroupBox.Controls.Add(Me.ReasonsButton)
        Me.ProcedureGroupBox.Controls.Add(Me.ReasonsLabel)
        Me.ProcedureGroupBox.Controls.Add(Me.PlaceofServiceButton)
        Me.ProcedureGroupBox.Controls.Add(Me.BillTypeButton)
        Me.ProcedureGroupBox.Controls.Add(Me.DiagnosisButton)
        Me.ProcedureGroupBox.Controls.Add(Me.DiagnosisCodesTextBox)
        Me.ProcedureGroupBox.Controls.Add(Me.DiagnosisLabel)
        Me.ProcedureGroupBox.Controls.Add(Me.BillTypeLabel)
        Me.ProcedureGroupBox.Controls.Add(Me.BillTypeTextBox)
        Me.ProcedureGroupBox.Controls.Add(Me.ProviderComboxBox)
        Me.ProcedureGroupBox.Controls.Add(Me.ProviderCheckBox)
        Me.ProcedureGroupBox.Controls.Add(Me.PlaceOfServiceTextBox)
        Me.ProcedureGroupBox.Controls.Add(Me.PlaceOfServiceLabel)
        Me.ProcedureGroupBox.Controls.Add(Me.ModifierLabel)
        Me.ProcedureGroupBox.Controls.Add(Me.ModifierTextBox)
        Me.ProcedureGroupBox.Controls.Add(Me.ProcedureCodeTextBox)
        Me.ProcedureGroupBox.Controls.Add(Me.ProcedureCodeLabel)
        Me.ProcedureGroupBox.Controls.Add(Me.ProcCodeButton)
        Me.ProcedureGroupBox.Controls.Add(Me.ModifiersButton)
        Me.ProcedureGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ProcedureGroupBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.ProcedureGroupBox.Location = New System.Drawing.Point(8, 143)
        Me.ProcedureGroupBox.Name = "ProcedureGroupBox"
        Me.ProcedureGroupBox.Size = New System.Drawing.Size(712, 72)
        Me.ProcedureGroupBox.TabIndex = 29
        Me.ProcedureGroupBox.TabStop = False
        '
        'ReasonsTextBox
        '
        Me.ReasonsTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ReasonsTextBox.Enabled = False
        Me.ReasonsTextBox.Location = New System.Drawing.Point(566, 16)
        Me.ReasonsTextBox.MaxLength = 7
        Me.ReasonsTextBox.Name = "ReasonsTextBox"
        Me.ReasonsTextBox.Size = New System.Drawing.Size(58, 20)
        Me.ReasonsTextBox.TabIndex = 46
        Me.ToolTip1.SetToolTip(Me.ReasonsTextBox, "Enter reason code(s) separated by comma")
        Me.ReasonsTextBox.Visible = False
        '
        'ReasonsButton
        '
        Me.ReasonsButton.Enabled = False
        Me.ReasonsButton.Font = New System.Drawing.Font("Wingdings", 4.0!)
        Me.ReasonsButton.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ReasonsButton.Location = New System.Drawing.Point(628, 14)
        Me.ReasonsButton.Name = "ReasonsButton"
        Me.ReasonsButton.Size = New System.Drawing.Size(24, 23)
        Me.ReasonsButton.TabIndex = 44
        Me.ReasonsButton.Text = "lll"
        Me.ReasonsButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ReasonsButton.Visible = False
        '
        'ReasonsLabel
        '
        Me.ReasonsLabel.AutoSize = True
        Me.ReasonsLabel.CausesValidation = False
        Me.ReasonsLabel.Enabled = False
        Me.ReasonsLabel.Location = New System.Drawing.Point(506, 19)
        Me.ReasonsLabel.Name = "ReasonsLabel"
        Me.ReasonsLabel.Size = New System.Drawing.Size(55, 13)
        Me.ReasonsLabel.TabIndex = 45
        Me.ReasonsLabel.Text = "Reason(s)"
        Me.ReasonsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ReasonsLabel.Visible = False
        '
        'PlaceofServiceButton
        '
        Me.PlaceofServiceButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.PlaceofServiceButton.Location = New System.Drawing.Point(477, 14)
        Me.PlaceofServiceButton.Name = "PlaceofServiceButton"
        Me.PlaceofServiceButton.Size = New System.Drawing.Size(24, 23)
        Me.PlaceofServiceButton.TabIndex = 11
        Me.PlaceofServiceButton.Text = "..."
        Me.ToolTip1.SetToolTip(Me.PlaceofServiceButton, "Display a list of valid Place of Service Codes")
        '
        'BillTypeButton
        '
        Me.BillTypeButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.BillTypeButton.Location = New System.Drawing.Point(328, 43)
        Me.BillTypeButton.Name = "BillTypeButton"
        Me.BillTypeButton.Size = New System.Drawing.Size(24, 23)
        Me.BillTypeButton.TabIndex = 18
        Me.BillTypeButton.Text = "..."
        Me.ToolTip1.SetToolTip(Me.BillTypeButton, "Display a list of Valid Bill Types")
        '
        'DiagnosisButton
        '
        Me.DiagnosisButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.DiagnosisButton.Location = New System.Drawing.Point(192, 43)
        Me.DiagnosisButton.Name = "DiagnosisButton"
        Me.DiagnosisButton.Size = New System.Drawing.Size(24, 23)
        Me.DiagnosisButton.TabIndex = 15
        Me.DiagnosisButton.Text = "..."
        Me.ToolTip1.SetToolTip(Me.DiagnosisButton, "Display a list of valid Diagnosis")
        '
        'DiagnosisCodesTextBox
        '
        Me.DiagnosisCodesTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.DiagnosisCodesTextBox.Location = New System.Drawing.Point(82, 43)
        Me.DiagnosisCodesTextBox.MaxLength = 120
        Me.DiagnosisCodesTextBox.Name = "DiagnosisCodesTextBox"
        Me.DiagnosisCodesTextBox.Size = New System.Drawing.Size(104, 20)
        Me.DiagnosisCodesTextBox.TabIndex = 13
        Me.ToolTip1.SetToolTip(Me.DiagnosisCodesTextBox, "Seperate with either a comma for multiple selections, or a dash(-) for a range")
        Me.DiagnosisCodesTextBox.WordWrap = False
        '
        'DiagnosisLabel
        '
        Me.DiagnosisLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.DiagnosisLabel.Location = New System.Drawing.Point(2, 46)
        Me.DiagnosisLabel.Name = "DiagnosisLabel"
        Me.DiagnosisLabel.Size = New System.Drawing.Size(71, 16)
        Me.DiagnosisLabel.TabIndex = 12
        Me.DiagnosisLabel.Text = "Diagnosis(es):"
        '
        'BillTypeLabel
        '
        Me.BillTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.BillTypeLabel.Location = New System.Drawing.Point(219, 46)
        Me.BillTypeLabel.Name = "BillTypeLabel"
        Me.BillTypeLabel.Size = New System.Drawing.Size(43, 16)
        Me.BillTypeLabel.TabIndex = 16
        Me.BillTypeLabel.Text = "Bill Type:"
        '
        'BillTypeTextBox
        '
        Me.BillTypeTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.BillTypeTextBox.Location = New System.Drawing.Point(264, 43)
        Me.BillTypeTextBox.MaxLength = 9
        Me.BillTypeTextBox.Name = "BillTypeTextBox"
        Me.BillTypeTextBox.Size = New System.Drawing.Size(60, 20)
        Me.BillTypeTextBox.TabIndex = 17
        Me.ToolTip1.SetToolTip(Me.BillTypeTextBox, "The Bill Type")
        Me.BillTypeTextBox.WordWrap = False
        '
        'ProviderComboxBox
        '
        Me.ProviderComboxBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ProviderComboxBox.Items.AddRange(New Object() {"P", "O", "N"})
        Me.ProviderComboxBox.Location = New System.Drawing.Point(410, 43)
        Me.ProviderComboxBox.Name = "ProviderComboxBox"
        Me.ProviderComboxBox.Size = New System.Drawing.Size(64, 21)
        Me.ProviderComboxBox.TabIndex = 20
        Me.ToolTip1.SetToolTip(Me.ProviderComboxBox, "What type of PAR")
        '
        'ProviderCheckBox
        '
        Me.ProviderCheckBox.Location = New System.Drawing.Point(358, 43)
        Me.ProviderCheckBox.Name = "ProviderCheckBox"
        Me.ProviderCheckBox.Size = New System.Drawing.Size(48, 16)
        Me.ProviderCheckBox.TabIndex = 19
        Me.ProviderCheckBox.Text = "PAR:"
        Me.ToolTip1.SetToolTip(Me.ProviderCheckBox, "Check to use PAR")
        '
        'PlaceOfServiceTextBox
        '
        Me.PlaceOfServiceTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.PlaceOfServiceTextBox.Location = New System.Drawing.Point(410, 16)
        Me.PlaceOfServiceTextBox.MaxLength = 9
        Me.PlaceOfServiceTextBox.Name = "PlaceOfServiceTextBox"
        Me.PlaceOfServiceTextBox.Size = New System.Drawing.Size(64, 20)
        Me.PlaceOfServiceTextBox.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.PlaceOfServiceTextBox, "The Place of Service")
        Me.PlaceOfServiceTextBox.WordWrap = False
        '
        'PlaceOfServiceLabel
        '
        Me.PlaceOfServiceLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.PlaceOfServiceLabel.Location = New System.Drawing.Point(358, 18)
        Me.PlaceOfServiceLabel.Name = "PlaceOfServiceLabel"
        Me.PlaceOfServiceLabel.Size = New System.Drawing.Size(40, 16)
        Me.PlaceOfServiceLabel.TabIndex = 9
        Me.PlaceOfServiceLabel.Text = "POS:"
        '
        'ModifierLabel
        '
        Me.ModifierLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ModifierLabel.Location = New System.Drawing.Point(219, 18)
        Me.ModifierLabel.Name = "ModifierLabel"
        Me.ModifierLabel.Size = New System.Drawing.Size(40, 16)
        Me.ModifierLabel.TabIndex = 6
        Me.ModifierLabel.Text = "Modifier:"
        '
        'ModifierTextBox
        '
        Me.ModifierTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ModifierTextBox.Location = New System.Drawing.Point(264, 16)
        Me.ModifierTextBox.MaxLength = 9
        Me.ModifierTextBox.Name = "ModifierTextBox"
        Me.ModifierTextBox.Size = New System.Drawing.Size(60, 20)
        Me.ModifierTextBox.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.ModifierTextBox, "The Modifier")
        Me.ModifierTextBox.WordWrap = False
        '
        'ProcedureCodeTextBox
        '
        Me.ProcedureCodeTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProcedureCodeTextBox.Location = New System.Drawing.Point(82, 16)
        Me.ProcedureCodeTextBox.MaxLength = 120
        Me.ProcedureCodeTextBox.Name = "ProcedureCodeTextBox"
        Me.ProcedureCodeTextBox.Size = New System.Drawing.Size(104, 20)
        Me.ProcedureCodeTextBox.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.ProcedureCodeTextBox, "Seperate with either a comma for multiple selections, or a dash(-) for a range")
        Me.ProcedureCodeTextBox.WordWrap = False
        '
        'ProcedureCodeLabel
        '
        Me.ProcedureCodeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ProcedureCodeLabel.Location = New System.Drawing.Point(2, 18)
        Me.ProcedureCodeLabel.Name = "ProcedureCodeLabel"
        Me.ProcedureCodeLabel.Size = New System.Drawing.Size(71, 16)
        Me.ProcedureCodeLabel.TabIndex = 2
        Me.ProcedureCodeLabel.Text = "Proc. Code(s):"
        '
        'ProcCodeButton
        '
        Me.ProcCodeButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ProcCodeButton.Location = New System.Drawing.Point(192, 15)
        Me.ProcCodeButton.Name = "ProcCodeButton"
        Me.ProcCodeButton.Size = New System.Drawing.Size(24, 23)
        Me.ProcCodeButton.TabIndex = 5
        Me.ProcCodeButton.TabStop = False
        Me.ProcCodeButton.Text = "..."
        Me.ToolTip1.SetToolTip(Me.ProcCodeButton, "Display a list of Valid Procedures")
        '
        'ModifiersButton
        '
        Me.ModifiersButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ModifiersButton.Location = New System.Drawing.Point(328, 15)
        Me.ModifiersButton.Name = "ModifiersButton"
        Me.ModifiersButton.Size = New System.Drawing.Size(24, 23)
        Me.ModifiersButton.TabIndex = 8
        Me.ModifiersButton.Text = "..."
        Me.ToolTip1.SetToolTip(Me.ModifiersButton, "Display a list of valid Modifiers")
        '
        'ChiroCheckBox
        '
        Me.ChiroCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ChiroCheckBox.Location = New System.Drawing.Point(556, 24)
        Me.ChiroCheckBox.Name = "ChiroCheckBox"
        Me.ChiroCheckBox.Size = New System.Drawing.Size(45, 16)
        Me.ChiroCheckBox.TabIndex = 17
        Me.ChiroCheckBox.Text = "Chiro"
        Me.ToolTip1.SetToolTip(Me.ChiroCheckBox, "If it is a Chiropropractic claim")
        '
        'ClaimIdLabel
        '
        Me.ClaimIdLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ClaimIdLabel.Location = New System.Drawing.Point(8, 24)
        Me.ClaimIdLabel.Name = "ClaimIdLabel"
        Me.ClaimIdLabel.Size = New System.Drawing.Size(47, 16)
        Me.ClaimIdLabel.TabIndex = 1
        Me.ClaimIdLabel.Text = "Claim ID:"
        '
        'ClaimIDTextBox
        '
        Me.ClaimIDTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ClaimIDTextBox.Location = New System.Drawing.Point(55, 22)
        Me.ClaimIDTextBox.MaxLength = 26
        Me.ClaimIDTextBox.Name = "ClaimIDTextBox"
        Me.ClaimIDTextBox.Size = New System.Drawing.Size(157, 20)
        Me.ClaimIDTextBox.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.ClaimIDTextBox, "Claim Identifier (DCN, MAXID, Claim ID, etc)")
        Me.ClaimIDTextBox.WordWrap = False
        '
        'DocTypeComboBox
        '
        Me.DocTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DocTypeComboBox.DropDownWidth = 250
        Me.DocTypeComboBox.Enabled = False
        Me.DocTypeComboBox.Location = New System.Drawing.Point(91, 46)
        Me.DocTypeComboBox.Name = "DocTypeComboBox"
        Me.DocTypeComboBox.Size = New System.Drawing.Size(149, 21)
        Me.DocTypeComboBox.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.DocTypeComboBox, "The Doc Type")
        '
        'AccidentGroupBox
        '
        Me.AccidentGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AccidentGroupBox.Controls.Add(Me.IncidentDateDateTimePicker)
        Me.AccidentGroupBox.Controls.Add(Me.AccidentCheckBox)
        Me.AccidentGroupBox.Controls.Add(Me.AccidentTypeComboBox)
        Me.AccidentGroupBox.Controls.Add(Me.IncidentDateCheckBox)
        Me.AccidentGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.AccidentGroupBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.AccidentGroupBox.Location = New System.Drawing.Point(8, 215)
        Me.AccidentGroupBox.Name = "AccidentGroupBox"
        Me.AccidentGroupBox.Size = New System.Drawing.Size(712, 42)
        Me.AccidentGroupBox.TabIndex = 30
        Me.AccidentGroupBox.TabStop = False
        Me.AccidentGroupBox.Text = "Accident Information"
        '
        'IncidentDateDateTimePicker
        '
        Me.IncidentDateDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.IncidentDateDateTimePicker.Location = New System.Drawing.Point(115, 16)
        Me.IncidentDateDateTimePicker.Name = "IncidentDateDateTimePicker"
        Me.IncidentDateDateTimePicker.Size = New System.Drawing.Size(96, 20)
        Me.IncidentDateDateTimePicker.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.IncidentDateDateTimePicker, "The Incident Date")
        '
        'AccidentCheckBox
        '
        Me.AccidentCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.AccidentCheckBox.Location = New System.Drawing.Point(229, 16)
        Me.AccidentCheckBox.Name = "AccidentCheckBox"
        Me.AccidentCheckBox.Size = New System.Drawing.Size(96, 24)
        Me.AccidentCheckBox.TabIndex = 2
        Me.AccidentCheckBox.Text = "Accident Type:"
        Me.ToolTip1.SetToolTip(Me.AccidentCheckBox, "Check to use Accident Type")
        '
        'AccidentTypeComboBox
        '
        Me.AccidentTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.AccidentTypeComboBox.Items.AddRange(New Object() {"", "Workers Comp", "Auto Accident", "Other"})
        Me.AccidentTypeComboBox.Location = New System.Drawing.Point(335, 16)
        Me.AccidentTypeComboBox.Name = "AccidentTypeComboBox"
        Me.AccidentTypeComboBox.Size = New System.Drawing.Size(121, 21)
        Me.AccidentTypeComboBox.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.AccidentTypeComboBox, "The Accident type")
        '
        'IncidentDateCheckBox
        '
        Me.IncidentDateCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.IncidentDateCheckBox.Location = New System.Drawing.Point(16, 16)
        Me.IncidentDateCheckBox.Name = "IncidentDateCheckBox"
        Me.IncidentDateCheckBox.Size = New System.Drawing.Size(96, 24)
        Me.IncidentDateCheckBox.TabIndex = 0
        Me.IncidentDateCheckBox.Text = "Incident Date:"
        Me.ToolTip1.SetToolTip(Me.IncidentDateCheckBox, "Check to use Incident Date")
        '
        'ProviderNameLabel
        '
        Me.ProviderNameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ProviderNameLabel.Location = New System.Drawing.Point(254, 24)
        Me.ProviderNameLabel.Name = "ProviderNameLabel"
        Me.ProviderNameLabel.Size = New System.Drawing.Size(79, 16)
        Me.ProviderNameLabel.TabIndex = 3
        Me.ProviderNameLabel.Text = "Provider Name:"
        '
        'ProviderNameTextBox
        '
        Me.ProviderNameTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ProviderNameTextBox.Location = New System.Drawing.Point(338, 22)
        Me.ProviderNameTextBox.MaxLength = 32000
        Me.ProviderNameTextBox.Name = "ProviderNameTextBox"
        Me.ProviderNameTextBox.Size = New System.Drawing.Size(112, 20)
        Me.ProviderNameTextBox.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.ProviderNameTextBox, "The Provider's Name (Partial Entry OK)")
        Me.ProviderNameTextBox.WordWrap = False
        '
        'ProviderNameLookupButton
        '
        Me.ProviderNameLookupButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ProviderNameLookupButton.Location = New System.Drawing.Point(453, 21)
        Me.ProviderNameLookupButton.Name = "ProviderNameLookupButton"
        Me.ProviderNameLookupButton.Size = New System.Drawing.Size(24, 21)
        Me.ProviderNameLookupButton.TabIndex = 5
        Me.ProviderNameLookupButton.TabStop = False
        Me.ProviderNameLookupButton.Text = "?"
        Me.ToolTip1.SetToolTip(Me.ProviderNameLookupButton, "Display a dialog to search by Name or TaxID or NPI")
        '
        'ProviderIDLabel
        '
        Me.ProviderIDLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ProviderIDLabel.Location = New System.Drawing.Point(357, 22)
        Me.ProviderIDLabel.Name = "ProviderIDLabel"
        Me.ProviderIDLabel.Size = New System.Drawing.Size(52, 16)
        Me.ProviderIDLabel.TabIndex = 6
        Me.ProviderIDLabel.Text = "Provider #:"
        '
        'ProviderIDTextBox
        '
        Me.ProviderIDTextBox.Location = New System.Drawing.Point(412, 20)
        Me.ProviderIDTextBox.MaxLength = 10
        Me.ProviderIDTextBox.Name = "ProviderIDTextBox"
        Me.ProviderIDTextBox.Size = New System.Drawing.Size(86, 20)
        Me.ProviderIDTextBox.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.ProviderIDTextBox, "The UFCW ProviderID, Provider's TIN ( 9 digits),  or Rendering Provider NPI (10 d" &
        "igits)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        '
        'ParticipantSSNTextBox
        '
        Me.ParticipantSSNTextBox.Location = New System.Drawing.Point(96, 20)
        Me.ParticipantSSNTextBox.MaxLength = 11
        Me.ParticipantSSNTextBox.Name = "ParticipantSSNTextBox"
        Me.ParticipantSSNTextBox.Size = New System.Drawing.Size(71, 20)
        Me.ParticipantSSNTextBox.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.ParticipantSSNTextBox, "The Participant's SSN")
        '
        'ParticipantSSNLabel
        '
        Me.ParticipantSSNLabel.AutoSize = True
        Me.ParticipantSSNLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ParticipantSSNLabel.Location = New System.Drawing.Point(8, 22)
        Me.ParticipantSSNLabel.Name = "ParticipantSSNLabel"
        Me.ParticipantSSNLabel.Size = New System.Drawing.Size(92, 13)
        Me.ParticipantSSNLabel.TabIndex = 0
        Me.ParticipantSSNLabel.Text = "Participant's SSN:"
        '
        'ProviderIDLookupButton
        '
        Me.ProviderIDLookupButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ProviderIDLookupButton.Location = New System.Drawing.Point(503, 20)
        Me.ProviderIDLookupButton.Name = "ProviderIDLookupButton"
        Me.ProviderIDLookupButton.Size = New System.Drawing.Size(24, 20)
        Me.ProviderIDLookupButton.TabIndex = 8
        Me.ProviderIDLookupButton.TabStop = False
        Me.ProviderIDLookupButton.Text = "?"
        Me.ToolTip1.SetToolTip(Me.ProviderIDLookupButton, "Display a dialog to search by Name or TaxID or NPI")
        '
        'AccumulatorsContextMenu
        '
        Me.AccumulatorsContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AccumulatorsMenuItem, Me.FamilyRelationMenuItem, Me.MenuItemPaste})
        Me.AccumulatorsContextMenu.Name = "AccumulatorsContextMenu"
        Me.AccumulatorsContextMenu.Size = New System.Drawing.Size(201, 70)
        '
        'AccumulatorsMenuItem
        '
        Me.AccumulatorsMenuItem.Name = "AccumulatorsMenuItem"
        Me.AccumulatorsMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.AccumulatorsMenuItem.Text = "Accumulators"
        '
        'FamilyRelationMenuItem
        '
        Me.FamilyRelationMenuItem.Name = "FamilyRelationMenuItem"
        Me.FamilyRelationMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.FamilyRelationMenuItem.Text = "Family/Relation Lookup"
        '
        'MenuItemPaste
        '
        Me.MenuItemPaste.Name = "MenuItemPaste"
        Me.MenuItemPaste.Size = New System.Drawing.Size(200, 22)
        Me.MenuItemPaste.Text = "Paste"
        '
        'SearchResultsGroupBox
        '
        Me.SearchResultsGroupBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SearchResultsGroupBox.Controls.Add(Me.TabControl)
        Me.SearchResultsGroupBox.Controls.Add(Me.SearchingLabel)
        Me.SearchResultsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.SearchResultsGroupBox.Location = New System.Drawing.Point(8, 444)
        Me.SearchResultsGroupBox.Name = "SearchResultsGroupBox"
        Me.SearchResultsGroupBox.Size = New System.Drawing.Size(740, 293)
        Me.SearchResultsGroupBox.TabIndex = 2
        Me.SearchResultsGroupBox.TabStop = False
        Me.SearchResultsGroupBox.Text = "Search Results"
        '
        'TabControl
        '
        Me.TabControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl.Controls.Add(Me.GenTab)
        Me.TabControl.Controls.Add(Me.DemographicsTab)
        Me.TabControl.Controls.Add(Me.EligibilityTab)
        Me.TabControl.Controls.Add(Me.EligibilityHoursTab)
        Me.TabControl.Controls.Add(Me.HoursTab)
        Me.TabControl.Controls.Add(Me.PremiumsHistoryTab)
        Me.TabControl.Controls.Add(Me.CoverageTab)
        Me.TabControl.Controls.Add(Me.HRAActivityTab)
        Me.TabControl.Controls.Add(Me.AccumulatorsTab)
        Me.TabControl.Controls.Add(Me.ProviderTab)
        Me.TabControl.Controls.Add(Me.PrescriptionsTab)
        Me.TabControl.Controls.Add(Me.DentalTab)
        Me.TabControl.Controls.Add(Me.COBTab)
        Me.TabControl.Controls.Add(Me.FreeTextTab)
        Me.TabControl.Controls.Add(Me.ImagingTab)
        Me.TabControl.Cursor = System.Windows.Forms.Cursors.Default
        Me.TabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed
        Me.TabControl.ItemSize = New System.Drawing.Size(62, 18)
        Me.TabControl.Location = New System.Drawing.Point(8, 16)
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        Me.TabControl.Size = New System.Drawing.Size(724, 271)
        Me.TabControl.TabIndex = 0
        Me.TabControl.Visible = False
        '
        'GenTab
        '
        Me.GenTab.AutoScroll = True
        Me.GenTab.BackColor = System.Drawing.Color.Transparent
        Me.GenTab.Controls.Add(Me.MainPanel)
        Me.GenTab.Location = New System.Drawing.Point(4, 22)
        Me.GenTab.Name = "GenTab"
        Me.GenTab.Size = New System.Drawing.Size(716, 245)
        Me.GenTab.TabIndex = 0
        Me.GenTab.Text = " Results "
        Me.GenTab.UseVisualStyleBackColor = True
        '
        'MainPanel
        '
        Me.MainPanel.AutoScroll = True
        Me.MainPanel.BackColor = System.Drawing.SystemColors.Control
        Me.MainPanel.Controls.Add(Me.AdjusterFilterLabel)
        Me.MainPanel.Controls.Add(Me.AdjustersComboBox)
        Me.MainPanel.Controls.Add(Me.ShowDocTypeLabel)
        Me.MainPanel.Controls.Add(Me.DocTypesComboBox)
        Me.MainPanel.Controls.Add(Me.MatchesLabel)
        Me.MainPanel.Controls.Add(Me.ShowStatusLabel)
        Me.MainPanel.Controls.Add(Me.StatusTypesComboBox)
        Me.MainPanel.Controls.Add(Me.CustomerServiceResultsDataGrid)
        Me.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainPanel.Location = New System.Drawing.Point(0, 0)
        Me.MainPanel.Name = "MainPanel"
        Me.MainPanel.Size = New System.Drawing.Size(716, 245)
        Me.MainPanel.TabIndex = 0
        '
        'AdjusterFilterLabel
        '
        Me.AdjusterFilterLabel.AutoSize = True
        Me.AdjusterFilterLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.AdjusterFilterLabel.Location = New System.Drawing.Point(363, 9)
        Me.AdjusterFilterLabel.Name = "AdjusterFilterLabel"
        Me.AdjusterFilterLabel.Size = New System.Drawing.Size(48, 13)
        Me.AdjusterFilterLabel.TabIndex = 4
        Me.AdjusterFilterLabel.Text = "Adjuster:"
        Me.ToolTip1.SetToolTip(Me.AdjusterFilterLabel, "Filter By Adjuster")
        '
        'AdjustersComboBox
        '
        Me.AdjustersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.AdjustersComboBox.DropDownWidth = 250
        Me.AdjustersComboBox.Items.AddRange(New Object() {"(all)"})
        Me.AdjustersComboBox.Location = New System.Drawing.Point(412, 5)
        Me.AdjustersComboBox.Name = "AdjustersComboBox"
        Me.AdjustersComboBox.Size = New System.Drawing.Size(125, 21)
        Me.AdjustersComboBox.TabIndex = 5
        '
        'ShowDocTypeLabel
        '
        Me.ShowDocTypeLabel.AutoSize = True
        Me.ShowDocTypeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ShowDocTypeLabel.Location = New System.Drawing.Point(175, 9)
        Me.ShowDocTypeLabel.Name = "ShowDocTypeLabel"
        Me.ShowDocTypeLabel.Size = New System.Drawing.Size(57, 13)
        Me.ShowDocTypeLabel.TabIndex = 2
        Me.ShowDocTypeLabel.Text = "Doc Type:"
        Me.ToolTip1.SetToolTip(Me.ShowDocTypeLabel, "Filter By Document Type")
        '
        'DocTypesComboBox
        '
        Me.DocTypesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DocTypesComboBox.DropDownWidth = 250
        Me.DocTypesComboBox.Items.AddRange(New Object() {"(all)"})
        Me.DocTypesComboBox.Location = New System.Drawing.Point(234, 6)
        Me.DocTypesComboBox.Name = "DocTypesComboBox"
        Me.DocTypesComboBox.Size = New System.Drawing.Size(125, 21)
        Me.DocTypesComboBox.TabIndex = 3
        '
        'MatchesLabel
        '
        Me.MatchesLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MatchesLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.MatchesLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MatchesLabel.Location = New System.Drawing.Point(588, 8)
        Me.MatchesLabel.Name = "MatchesLabel"
        Me.MatchesLabel.Size = New System.Drawing.Size(120, 13)
        Me.MatchesLabel.TabIndex = 6
        Me.MatchesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ShowStatusLabel
        '
        Me.ShowStatusLabel.AutoSize = True
        Me.ShowStatusLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ShowStatusLabel.Location = New System.Drawing.Point(5, 9)
        Me.ShowStatusLabel.Name = "ShowStatusLabel"
        Me.ShowStatusLabel.Size = New System.Drawing.Size(40, 13)
        Me.ShowStatusLabel.TabIndex = 0
        Me.ShowStatusLabel.Text = "Status:"
        Me.ToolTip1.SetToolTip(Me.ShowStatusLabel, "Filter By Status")
        '
        'StatusTypesComboBox
        '
        Me.StatusTypesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.StatusTypesComboBox.DropDownWidth = 250
        Me.StatusTypesComboBox.Items.AddRange(New Object() {"(all)"})
        Me.StatusTypesComboBox.Location = New System.Drawing.Point(47, 5)
        Me.StatusTypesComboBox.Name = "StatusTypesComboBox"
        Me.StatusTypesComboBox.Size = New System.Drawing.Size(125, 21)
        Me.StatusTypesComboBox.TabIndex = 1
        '
        'CustomerServiceResultsDataGrid
        '
        Me.CustomerServiceResultsDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.CustomerServiceResultsDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.CustomerServiceResultsDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.CustomerServiceResultsDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.CustomerServiceResultsDataGrid.ADGroupsThatCanFind = ""
        Me.CustomerServiceResultsDataGrid.ADGroupsThatCanMultiSort = ""
        Me.CustomerServiceResultsDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.CustomerServiceResultsDataGrid.AllowAutoSize = True
        Me.CustomerServiceResultsDataGrid.AllowColumnReorder = True
        Me.CustomerServiceResultsDataGrid.AllowCopy = False
        Me.CustomerServiceResultsDataGrid.AllowCustomize = True
        Me.CustomerServiceResultsDataGrid.AllowDelete = False
        Me.CustomerServiceResultsDataGrid.AllowDragDrop = True
        Me.CustomerServiceResultsDataGrid.AllowEdit = False
        Me.CustomerServiceResultsDataGrid.AllowExport = True
        Me.CustomerServiceResultsDataGrid.AllowFilter = True
        Me.CustomerServiceResultsDataGrid.AllowFind = True
        Me.CustomerServiceResultsDataGrid.AllowGoTo = True
        Me.CustomerServiceResultsDataGrid.AllowMultiSelect = True
        Me.CustomerServiceResultsDataGrid.AllowMultiSort = False
        Me.CustomerServiceResultsDataGrid.AllowNew = False
        Me.CustomerServiceResultsDataGrid.AllowPrint = True
        Me.CustomerServiceResultsDataGrid.AllowRefresh = False
        Me.CustomerServiceResultsDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CustomerServiceResultsDataGrid.AppKey = "UFCW\Claims\"
        Me.CustomerServiceResultsDataGrid.AutoSaveCols = True
        Me.CustomerServiceResultsDataGrid.BackgroundColor = System.Drawing.Color.White
        Me.CustomerServiceResultsDataGrid.CaptionVisible = False
        Me.CustomerServiceResultsDataGrid.ColumnHeaderLabel = Nothing
        Me.CustomerServiceResultsDataGrid.ColumnRePositioning = False
        Me.CustomerServiceResultsDataGrid.ColumnResizing = False
        Me.CustomerServiceResultsDataGrid.ConfirmDelete = True
        Me.CustomerServiceResultsDataGrid.CopySelectedOnly = True
        Me.CustomerServiceResultsDataGrid.CurrentBSPosition = -1
        Me.CustomerServiceResultsDataGrid.DataMember = ""
        Me.CustomerServiceResultsDataGrid.DragColumn = 0
        Me.CustomerServiceResultsDataGrid.ExportSelectedOnly = True
        Me.CustomerServiceResultsDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.CustomerServiceResultsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.CustomerServiceResultsDataGrid.HighlightedRow = Nothing
        Me.CustomerServiceResultsDataGrid.HighLightModifiedRows = False
        Me.CustomerServiceResultsDataGrid.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.CustomerServiceResultsDataGrid.IsMouseDown = False
        Me.CustomerServiceResultsDataGrid.LastGoToLine = ""
        Me.CustomerServiceResultsDataGrid.Location = New System.Drawing.Point(8, 32)
        Me.CustomerServiceResultsDataGrid.MultiSort = False
        Me.CustomerServiceResultsDataGrid.Name = "CustomerServiceResultsDataGrid"
        Me.CustomerServiceResultsDataGrid.OldSelectedRow = Nothing
        Me.CustomerServiceResultsDataGrid.PreviousBSPosition = -1
        Me.CustomerServiceResultsDataGrid.ReadOnly = True
        Me.CustomerServiceResultsDataGrid.RetainRowSelectionAfterSort = True
        Me.CustomerServiceResultsDataGrid.SetRowOnRightClick = True
        Me.CustomerServiceResultsDataGrid.ShiftPressed = False
        Me.CustomerServiceResultsDataGrid.SingleClickBooleanColumns = True
        Me.CustomerServiceResultsDataGrid.Size = New System.Drawing.Size(700, 210)
        Me.CustomerServiceResultsDataGrid.Sort = Nothing
        Me.CustomerServiceResultsDataGrid.StyleName = ""
        Me.CustomerServiceResultsDataGrid.SubKey = ""
        Me.CustomerServiceResultsDataGrid.SuppressMouseDown = False
        Me.CustomerServiceResultsDataGrid.SuppressTriangle = False
        Me.CustomerServiceResultsDataGrid.TabIndex = 7
        '
        'DemographicsTab
        '
        Me.DemographicsTab.AutoScroll = True
        Me.DemographicsTab.BackColor = System.Drawing.Color.Transparent
        Me.DemographicsTab.Controls.Add(Me.SplitContainer2)
        Me.DemographicsTab.Location = New System.Drawing.Point(4, 22)
        Me.DemographicsTab.Name = "DemographicsTab"
        Me.DemographicsTab.Size = New System.Drawing.Size(716, 245)
        Me.DemographicsTab.TabIndex = 3
        Me.DemographicsTab.Text = " FAMILY INFO "
        Me.DemographicsTab.UseVisualStyleBackColor = True
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.SplitContainer3)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.SplitContainer4)
        Me.SplitContainer2.Size = New System.Drawing.Size(716, 245)
        Me.SplitContainer2.SplitterDistance = 125
        Me.SplitContainer2.TabIndex = 0
        '
        'SplitContainer3
        '
        Me.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer3.IsSplitterFixed = True
        Me.SplitContainer3.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer3.Name = "SplitContainer3"
        Me.SplitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer3.Panel1.Controls.Add(Me.ParticipantToolStrip)
        Me.SplitContainer3.Panel1MinSize = 17
        '
        'SplitContainer3.Panel2
        '
        Me.SplitContainer3.Panel2.Controls.Add(Me.ParticipantDemographicsGrid)
        Me.SplitContainer3.Panel2MinSize = 17
        Me.SplitContainer3.Size = New System.Drawing.Size(716, 125)
        Me.SplitContainer3.SplitterDistance = 25
        Me.SplitContainer3.SplitterWidth = 1
        Me.SplitContainer3.TabIndex = 0
        '
        'ParticipantToolStrip
        '
        Me.ParticipantToolStrip.BackColor = System.Drawing.SystemColors.Control
        Me.ParticipantToolStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.ParticipantToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ParticipantToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EligibilityAlertLabel, Me.AddressLabel, Me.ParticipantHighAlertLabel, Me.ParticipantAlertLabel})
        Me.ParticipantToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow
        Me.ParticipantToolStrip.Location = New System.Drawing.Point(0, 0)
        Me.ParticipantToolStrip.Name = "ParticipantToolStrip"
        Me.ParticipantToolStrip.Size = New System.Drawing.Size(41, 3)
        Me.ParticipantToolStrip.TabIndex = 23
        '
        'EligibilityAlertLabel
        '
        Me.EligibilityAlertLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.EligibilityAlertLabel.Name = "EligibilityAlertLabel"
        Me.EligibilityAlertLabel.Padding = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.EligibilityAlertLabel.Size = New System.Drawing.Size(10, 0)
        Me.EligibilityAlertLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'AddressLabel
        '
        Me.AddressLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.AddressLabel.ImageTransparentColor = System.Drawing.Color.Transparent
        Me.AddressLabel.Name = "AddressLabel"
        Me.AddressLabel.Padding = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.AddressLabel.Size = New System.Drawing.Size(10, 0)
        Me.AddressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ParticipantHighAlertLabel
        '
        Me.ParticipantHighAlertLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ParticipantHighAlertLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.ParticipantHighAlertLabel.ForeColor = System.Drawing.Color.OrangeRed
        Me.ParticipantHighAlertLabel.ImageTransparentColor = System.Drawing.Color.Transparent
        Me.ParticipantHighAlertLabel.Name = "ParticipantHighAlertLabel"
        Me.ParticipantHighAlertLabel.Padding = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.ParticipantHighAlertLabel.Size = New System.Drawing.Size(10, 0)
        Me.ParticipantHighAlertLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ParticipantAlertLabel
        '
        Me.ParticipantAlertLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ParticipantAlertLabel.ImageTransparentColor = System.Drawing.Color.Transparent
        Me.ParticipantAlertLabel.Name = "ParticipantAlertLabel"
        Me.ParticipantAlertLabel.Padding = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.ParticipantAlertLabel.Size = New System.Drawing.Size(10, 0)
        Me.ParticipantAlertLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ParticipantDemographicsGrid
        '
        Me.ParticipantDemographicsGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.ParticipantDemographicsGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.ParticipantDemographicsGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ParticipantDemographicsGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ParticipantDemographicsGrid.ADGroupsThatCanFind = ""
        Me.ParticipantDemographicsGrid.ADGroupsThatCanMultiSort = ""
        Me.ParticipantDemographicsGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.ParticipantDemographicsGrid.AllowAutoSize = True
        Me.ParticipantDemographicsGrid.AllowColumnReorder = True
        Me.ParticipantDemographicsGrid.AllowCopy = True
        Me.ParticipantDemographicsGrid.AllowCustomize = True
        Me.ParticipantDemographicsGrid.AllowDelete = False
        Me.ParticipantDemographicsGrid.AllowDragDrop = False
        Me.ParticipantDemographicsGrid.AllowEdit = False
        Me.ParticipantDemographicsGrid.AllowExport = True
        Me.ParticipantDemographicsGrid.AllowFilter = False
        Me.ParticipantDemographicsGrid.AllowFind = True
        Me.ParticipantDemographicsGrid.AllowGoTo = False
        Me.ParticipantDemographicsGrid.AllowMultiSelect = True
        Me.ParticipantDemographicsGrid.AllowMultiSort = False
        Me.ParticipantDemographicsGrid.AllowNew = False
        Me.ParticipantDemographicsGrid.AllowPrint = True
        Me.ParticipantDemographicsGrid.AllowRefresh = False
        Me.ParticipantDemographicsGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ParticipantDemographicsGrid.AppKey = "UFCW\Claims\"
        Me.ParticipantDemographicsGrid.AutoSaveCols = True
        Me.ParticipantDemographicsGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ParticipantDemographicsGrid.CaptionVisible = False
        Me.ParticipantDemographicsGrid.ColumnHeaderLabel = Nothing
        Me.ParticipantDemographicsGrid.ColumnRePositioning = False
        Me.ParticipantDemographicsGrid.ColumnResizing = False
        Me.ParticipantDemographicsGrid.ConfirmDelete = True
        Me.ParticipantDemographicsGrid.CopySelectedOnly = True
        Me.ParticipantDemographicsGrid.CurrentBSPosition = -1
        Me.ParticipantDemographicsGrid.DataMember = ""
        Me.ParticipantDemographicsGrid.DragColumn = 0
        Me.ParticipantDemographicsGrid.ExportSelectedOnly = True
        Me.ParticipantDemographicsGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.ParticipantDemographicsGrid.HighlightedRow = Nothing
        Me.ParticipantDemographicsGrid.HighLightModifiedRows = False
        Me.ParticipantDemographicsGrid.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.ParticipantDemographicsGrid.IsMouseDown = False
        Me.ParticipantDemographicsGrid.LastGoToLine = ""
        Me.ParticipantDemographicsGrid.Location = New System.Drawing.Point(0, 2)
        Me.ParticipantDemographicsGrid.MultiSort = False
        Me.ParticipantDemographicsGrid.Name = "ParticipantDemographicsGrid"
        Me.ParticipantDemographicsGrid.OldSelectedRow = Nothing
        Me.ParticipantDemographicsGrid.PreviousBSPosition = -1
        Me.ParticipantDemographicsGrid.ReadOnly = True
        Me.ParticipantDemographicsGrid.RetainRowSelectionAfterSort = True
        Me.ParticipantDemographicsGrid.SetRowOnRightClick = True
        Me.ParticipantDemographicsGrid.ShiftPressed = False
        Me.ParticipantDemographicsGrid.SingleClickBooleanColumns = True
        Me.ParticipantDemographicsGrid.Size = New System.Drawing.Size(716, 93)
        Me.ParticipantDemographicsGrid.Sort = Nothing
        Me.ParticipantDemographicsGrid.StyleName = ""
        Me.ParticipantDemographicsGrid.SubKey = ""
        Me.ParticipantDemographicsGrid.SuppressMouseDown = False
        Me.ParticipantDemographicsGrid.SuppressTriangle = False
        Me.ParticipantDemographicsGrid.TabIndex = 20
        '
        'SplitContainer4
        '
        Me.SplitContainer4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer4.IsSplitterFixed = True
        Me.SplitContainer4.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer4.Name = "SplitContainer4"
        Me.SplitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer4.Panel1
        '
        Me.SplitContainer4.Panel1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.SplitContainer4.Panel1.Controls.Add(Me.DualParticipantToolStrip)
        Me.SplitContainer4.Panel1MinSize = 17
        '
        'SplitContainer4.Panel2
        '
        Me.SplitContainer4.Panel2.Controls.Add(Me.DualParticipantDemographicsGrid)
        Me.SplitContainer4.Size = New System.Drawing.Size(716, 116)
        Me.SplitContainer4.SplitterDistance = 25
        Me.SplitContainer4.SplitterWidth = 1
        Me.SplitContainer4.TabIndex = 0
        '
        'DualParticipantToolStrip
        '
        Me.DualParticipantToolStrip.BackColor = System.Drawing.SystemColors.Control
        Me.DualParticipantToolStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.DualParticipantToolStrip.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DualParticipantToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.DualParticipantToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DualParticipantAddressLabel, Me.DualParticipantHighAlertLabel, Me.DualParticipantAlertLabel})
        Me.DualParticipantToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow
        Me.DualParticipantToolStrip.Location = New System.Drawing.Point(0, 0)
        Me.DualParticipantToolStrip.Name = "DualParticipantToolStrip"
        Me.DualParticipantToolStrip.Size = New System.Drawing.Size(716, 25)
        Me.DualParticipantToolStrip.TabIndex = 25
        '
        'DualParticipantAddressLabel
        '
        Me.DualParticipantAddressLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.DualParticipantAddressLabel.Name = "DualParticipantAddressLabel"
        Me.DualParticipantAddressLabel.Size = New System.Drawing.Size(10, 15)
        Me.DualParticipantAddressLabel.Text = " "
        '
        'DualParticipantHighAlertLabel
        '
        Me.DualParticipantHighAlertLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.DualParticipantHighAlertLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.DualParticipantHighAlertLabel.ForeColor = System.Drawing.Color.OrangeRed
        Me.DualParticipantHighAlertLabel.Name = "DualParticipantHighAlertLabel"
        Me.DualParticipantHighAlertLabel.Size = New System.Drawing.Size(10, 15)
        Me.DualParticipantHighAlertLabel.Text = " "
        '
        'DualParticipantAlertLabel
        '
        Me.DualParticipantAlertLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.DualParticipantAlertLabel.Name = "DualParticipantAlertLabel"
        Me.DualParticipantAlertLabel.Size = New System.Drawing.Size(0, 0)
        '
        'DualParticipantDemographicsGrid
        '
        Me.DualParticipantDemographicsGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.DualParticipantDemographicsGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.DualParticipantDemographicsGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DualParticipantDemographicsGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DualParticipantDemographicsGrid.ADGroupsThatCanFind = ""
        Me.DualParticipantDemographicsGrid.ADGroupsThatCanMultiSort = ""
        Me.DualParticipantDemographicsGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DualParticipantDemographicsGrid.AllowAutoSize = True
        Me.DualParticipantDemographicsGrid.AllowColumnReorder = True
        Me.DualParticipantDemographicsGrid.AllowCopy = True
        Me.DualParticipantDemographicsGrid.AllowCustomize = True
        Me.DualParticipantDemographicsGrid.AllowDelete = False
        Me.DualParticipantDemographicsGrid.AllowDragDrop = False
        Me.DualParticipantDemographicsGrid.AllowEdit = False
        Me.DualParticipantDemographicsGrid.AllowExport = True
        Me.DualParticipantDemographicsGrid.AllowFilter = False
        Me.DualParticipantDemographicsGrid.AllowFind = True
        Me.DualParticipantDemographicsGrid.AllowGoTo = False
        Me.DualParticipantDemographicsGrid.AllowMultiSelect = True
        Me.DualParticipantDemographicsGrid.AllowMultiSort = False
        Me.DualParticipantDemographicsGrid.AllowNew = False
        Me.DualParticipantDemographicsGrid.AllowPrint = True
        Me.DualParticipantDemographicsGrid.AllowRefresh = False
        Me.DualParticipantDemographicsGrid.AppKey = "UFCW\Claims\Dual\"
        Me.DualParticipantDemographicsGrid.AutoSaveCols = True
        Me.DualParticipantDemographicsGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.DualParticipantDemographicsGrid.CaptionVisible = False
        Me.DualParticipantDemographicsGrid.ColumnHeaderLabel = Nothing
        Me.DualParticipantDemographicsGrid.ColumnRePositioning = False
        Me.DualParticipantDemographicsGrid.ColumnResizing = False
        Me.DualParticipantDemographicsGrid.ConfirmDelete = True
        Me.DualParticipantDemographicsGrid.CopySelectedOnly = True
        Me.DualParticipantDemographicsGrid.CurrentBSPosition = -1
        Me.DualParticipantDemographicsGrid.DataMember = ""
        Me.DualParticipantDemographicsGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DualParticipantDemographicsGrid.DragColumn = 0
        Me.DualParticipantDemographicsGrid.ExportSelectedOnly = True
        Me.DualParticipantDemographicsGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DualParticipantDemographicsGrid.HighlightedRow = Nothing
        Me.DualParticipantDemographicsGrid.HighLightModifiedRows = False
        Me.DualParticipantDemographicsGrid.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.DualParticipantDemographicsGrid.IsMouseDown = False
        Me.DualParticipantDemographicsGrid.LastGoToLine = ""
        Me.DualParticipantDemographicsGrid.Location = New System.Drawing.Point(0, 0)
        Me.DualParticipantDemographicsGrid.MultiSort = False
        Me.DualParticipantDemographicsGrid.Name = "DualParticipantDemographicsGrid"
        Me.DualParticipantDemographicsGrid.OldSelectedRow = Nothing
        Me.DualParticipantDemographicsGrid.PreviousBSPosition = -1
        Me.DualParticipantDemographicsGrid.ReadOnly = True
        Me.DualParticipantDemographicsGrid.RetainRowSelectionAfterSort = True
        Me.DualParticipantDemographicsGrid.SetRowOnRightClick = True
        Me.DualParticipantDemographicsGrid.ShiftPressed = False
        Me.DualParticipantDemographicsGrid.SingleClickBooleanColumns = True
        Me.DualParticipantDemographicsGrid.Size = New System.Drawing.Size(716, 90)
        Me.DualParticipantDemographicsGrid.Sort = Nothing
        Me.DualParticipantDemographicsGrid.StyleName = ""
        Me.DualParticipantDemographicsGrid.SubKey = ""
        Me.DualParticipantDemographicsGrid.SuppressMouseDown = False
        Me.DualParticipantDemographicsGrid.SuppressTriangle = False
        Me.DualParticipantDemographicsGrid.TabIndex = 42
        '
        'EligibilityTab
        '
        Me.EligibilityTab.AutoScroll = True
        Me.EligibilityTab.BackColor = System.Drawing.Color.Transparent
        Me.EligibilityTab.Controls.Add(Me.EligibilitySplitContainer)
        Me.EligibilityTab.Location = New System.Drawing.Point(4, 22)
        Me.EligibilityTab.Name = "EligibilityTab"
        Me.EligibilityTab.Size = New System.Drawing.Size(716, 245)
        Me.EligibilityTab.TabIndex = 2
        Me.EligibilityTab.Text = " FAMILY ELIGIBILITY "
        Me.EligibilityTab.UseVisualStyleBackColor = True
        '
        'EligibilitySplitContainer
        '
        Me.EligibilitySplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EligibilitySplitContainer.Location = New System.Drawing.Point(0, 0)
        Me.EligibilitySplitContainer.Name = "EligibilitySplitContainer"
        Me.EligibilitySplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'EligibilitySplitContainer.Panel1
        '
        Me.EligibilitySplitContainer.Panel1.Controls.Add(Me.EligibilityControl)
        '
        'EligibilitySplitContainer.Panel2
        '
        Me.EligibilitySplitContainer.Panel2.Controls.Add(Me.EligibilityDualCoverageControl)
        Me.EligibilitySplitContainer.Size = New System.Drawing.Size(716, 245)
        Me.EligibilitySplitContainer.SplitterDistance = 145
        Me.EligibilitySplitContainer.TabIndex = 0
        '
        'EligibilityControl
        '
        Me.EligibilityControl.AppKey = "UFCW\Claims\"
        Me.EligibilityControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EligibilityControl.Location = New System.Drawing.Point(0, 0)
        Me.EligibilityControl.Name = "EligibilityControl"
        Me.EligibilityControl.Size = New System.Drawing.Size(716, 145)
        Me.EligibilityControl.TabIndex = 91
        '
        'EligibilityDualCoverageControl
        '
        Me.EligibilityDualCoverageControl.AppKey = "UFCW\Claims\Dual\"
        Me.EligibilityDualCoverageControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EligibilityDualCoverageControl.Location = New System.Drawing.Point(0, 0)
        Me.EligibilityDualCoverageControl.Name = "EligibilityDualCoverageControl"
        Me.EligibilityDualCoverageControl.Size = New System.Drawing.Size(716, 96)
        Me.EligibilityDualCoverageControl.TabIndex = 92
        '
        'EligibilityHoursTab
        '
        Me.EligibilityHoursTab.Controls.Add(Me.EligibilityHoursSplitContainer)
        Me.EligibilityHoursTab.Location = New System.Drawing.Point(4, 22)
        Me.EligibilityHoursTab.Name = "EligibilityHoursTab"
        Me.EligibilityHoursTab.Padding = New System.Windows.Forms.Padding(3)
        Me.EligibilityHoursTab.Size = New System.Drawing.Size(716, 245)
        Me.EligibilityHoursTab.TabIndex = 14
        Me.EligibilityHoursTab.Text = " Elg. HOURS "
        Me.EligibilityHoursTab.UseVisualStyleBackColor = True
        '
        'EligibilityHoursSplitContainer
        '
        Me.EligibilityHoursSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EligibilityHoursSplitContainer.Location = New System.Drawing.Point(3, 3)
        Me.EligibilityHoursSplitContainer.Name = "EligibilityHoursSplitContainer"
        Me.EligibilityHoursSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'EligibilityHoursSplitContainer.Panel1
        '
        Me.EligibilityHoursSplitContainer.Panel1.Controls.Add(Me.EligibilityHoursControl)
        '
        'EligibilityHoursSplitContainer.Panel2
        '
        Me.EligibilityHoursSplitContainer.Panel2.Controls.Add(Me.EligibilityHoursDualCoverageControl)
        Me.EligibilityHoursSplitContainer.Size = New System.Drawing.Size(710, 239)
        Me.EligibilityHoursSplitContainer.SplitterDistance = 115
        Me.EligibilityHoursSplitContainer.TabIndex = 1
        '
        'EligibilityHoursControl
        '
        Me.EligibilityHoursControl.AppKey = "UFCW\Claims\"
        Me.EligibilityHoursControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EligibilityHoursControl.DualFamily = False
        Me.EligibilityHoursControl.Location = New System.Drawing.Point(0, 0)
        Me.EligibilityHoursControl.Memtype = ""
        Me.EligibilityHoursControl.Name = "EligibilityHoursControl"
        Me.EligibilityHoursControl.Size = New System.Drawing.Size(710, 115)
        Me.EligibilityHoursControl.TabIndex = 14
        '
        'EligibilityHoursDualCoverageControl
        '
        Me.EligibilityHoursDualCoverageControl.AppKey = "UFCW\Claims\Dual\"
        Me.EligibilityHoursDualCoverageControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EligibilityHoursDualCoverageControl.DualFamily = False
        Me.EligibilityHoursDualCoverageControl.Location = New System.Drawing.Point(0, 0)
        Me.EligibilityHoursDualCoverageControl.Memtype = ""
        Me.EligibilityHoursDualCoverageControl.Name = "EligibilityHoursDualCoverageControl"
        Me.EligibilityHoursDualCoverageControl.Size = New System.Drawing.Size(710, 120)
        Me.EligibilityHoursDualCoverageControl.TabIndex = 14
        '
        'HoursTab
        '
        Me.HoursTab.BackColor = System.Drawing.Color.Transparent
        Me.HoursTab.Controls.Add(Me.HoursSplitContainer)
        Me.HoursTab.Location = New System.Drawing.Point(4, 22)
        Me.HoursTab.Name = "HoursTab"
        Me.HoursTab.Size = New System.Drawing.Size(716, 245)
        Me.HoursTab.TabIndex = 10
        Me.HoursTab.Text = " HOURS "
        Me.HoursTab.UseVisualStyleBackColor = True
        '
        'HoursSplitContainer
        '
        Me.HoursSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HoursSplitContainer.Location = New System.Drawing.Point(0, 0)
        Me.HoursSplitContainer.Name = "HoursSplitContainer"
        Me.HoursSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'HoursSplitContainer.Panel1
        '
        Me.HoursSplitContainer.Panel1.Controls.Add(Me.HoursControl)
        '
        'HoursSplitContainer.Panel2
        '
        Me.HoursSplitContainer.Panel2.Controls.Add(Me.HoursDualCoverageControl)
        Me.HoursSplitContainer.Size = New System.Drawing.Size(716, 245)
        Me.HoursSplitContainer.SplitterDistance = 119
        Me.HoursSplitContainer.TabIndex = 0
        '
        'HoursControl
        '
        Me.HoursControl.AppKey = "UFCW\Claims\"
        Me.HoursControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HoursControl.Location = New System.Drawing.Point(0, 0)
        Me.HoursControl.Name = "HoursControl"
        Me.HoursControl.Size = New System.Drawing.Size(716, 119)
        Me.HoursControl.TabIndex = 14
        '
        'HoursDualCoverageControl
        '
        Me.HoursDualCoverageControl.AppKey = "UFCW\Claims\Dual\"
        Me.HoursDualCoverageControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HoursDualCoverageControl.Location = New System.Drawing.Point(0, 0)
        Me.HoursDualCoverageControl.Name = "HoursDualCoverageControl"
        Me.HoursDualCoverageControl.Size = New System.Drawing.Size(716, 122)
        Me.HoursDualCoverageControl.TabIndex = 14
        '
        'PremiumsHistoryTab
        '
        Me.PremiumsHistoryTab.AutoScroll = True
        Me.PremiumsHistoryTab.BackColor = System.Drawing.Color.Transparent
        Me.PremiumsHistoryTab.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PremiumsHistoryTab.Controls.Add(Me.SplitContainer1)
        Me.PremiumsHistoryTab.Location = New System.Drawing.Point(4, 22)
        Me.PremiumsHistoryTab.Name = "PremiumsHistoryTab"
        Me.PremiumsHistoryTab.Size = New System.Drawing.Size(716, 245)
        Me.PremiumsHistoryTab.TabIndex = 7
        Me.PremiumsHistoryTab.Text = " PREMIUMS "
        Me.PremiumsHistoryTab.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.PremiumsControl)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.PremiumsEnrollmentControl)
        Me.SplitContainer1.Size = New System.Drawing.Size(712, 241)
        Me.SplitContainer1.SplitterDistance = 117
        Me.SplitContainer1.TabIndex = 1
        '
        'PremiumsControl
        '
        Me.PremiumsControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PremiumsControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PremiumsControl.Location = New System.Drawing.Point(0, 0)
        Me.PremiumsControl.Name = "PremiumsControl"
        Me.PremiumsControl.Size = New System.Drawing.Size(712, 117)
        Me.PremiumsControl.TabIndex = 1
        '
        'PremiumsEnrollmentControl
        '
        Me.PremiumsEnrollmentControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PremiumsEnrollmentControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PremiumsEnrollmentControl.Location = New System.Drawing.Point(0, 0)
        Me.PremiumsEnrollmentControl.Name = "PremiumsEnrollmentControl"
        Me.PremiumsEnrollmentControl.Size = New System.Drawing.Size(712, 120)
        Me.PremiumsEnrollmentControl.TabIndex = 2
        '
        'CoverageTab
        '
        Me.CoverageTab.Controls.Add(Me.CoverageHistory)
        Me.CoverageTab.Location = New System.Drawing.Point(4, 22)
        Me.CoverageTab.Name = "CoverageTab"
        Me.CoverageTab.Size = New System.Drawing.Size(716, 245)
        Me.CoverageTab.TabIndex = 15
        Me.CoverageTab.Text = " COVERAGE "
        Me.CoverageTab.UseVisualStyleBackColor = True
        '
        'CoverageHistory
        '
        Me.CoverageHistory.AppKey = "UFCW\Claims\"
        Me.CoverageHistory.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CoverageHistory.DualFamily = False
        Me.CoverageHistory.Location = New System.Drawing.Point(0, 0)
        Me.CoverageHistory.MedPlan = ""
        Me.CoverageHistory.Memtype = ""
        Me.CoverageHistory.Name = "CoverageHistory"
        Me.CoverageHistory.Size = New System.Drawing.Size(716, 245)
        Me.CoverageHistory.Status = Nothing
        Me.CoverageHistory.TabIndex = 0
        '
        'HRAActivityTab
        '
        Me.HRAActivityTab.BackColor = System.Drawing.Color.Transparent
        Me.HRAActivityTab.Controls.Add(Me.HRASplitContainer)
        Me.HRAActivityTab.Location = New System.Drawing.Point(4, 22)
        Me.HRAActivityTab.Name = "HRAActivityTab"
        Me.HRAActivityTab.Size = New System.Drawing.Size(716, 245)
        Me.HRAActivityTab.TabIndex = 5
        Me.HRAActivityTab.Text = " HRA/HRQ "
        Me.HRAActivityTab.UseVisualStyleBackColor = True
        '
        'HRASplitContainer
        '
        Me.HRASplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HRASplitContainer.Location = New System.Drawing.Point(0, 0)
        Me.HRASplitContainer.Name = "HRASplitContainer"
        Me.HRASplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'HRASplitContainer.Panel1
        '
        Me.HRASplitContainer.Panel1.Controls.Add(Me.HRQSplitContainer)
        '
        'HRASplitContainer.Panel2
        '
        Me.HRASplitContainer.Panel2.Controls.Add(Me.HraActivityControl)
        Me.HRASplitContainer.Size = New System.Drawing.Size(716, 245)
        Me.HRASplitContainer.SplitterDistance = 82
        Me.HRASplitContainer.TabIndex = 1
        '
        'HRQSplitContainer
        '
        Me.HRQSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HRQSplitContainer.Location = New System.Drawing.Point(0, 0)
        Me.HRQSplitContainer.Name = "HRQSplitContainer"
        '
        'HRQSplitContainer.Panel1
        '
        Me.HRQSplitContainer.Panel1.Controls.Add(Me.HraBalanceControl)
        '
        'HRQSplitContainer.Panel2
        '
        Me.HRQSplitContainer.Panel2.Controls.Add(Me.HrqControl)
        Me.HRQSplitContainer.Size = New System.Drawing.Size(716, 82)
        Me.HRQSplitContainer.SplitterDistance = 324
        Me.HRQSplitContainer.TabIndex = 0
        '
        'HraBalanceControl
        '
        Me.HraBalanceControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HraBalanceControl.FamilyID = -1
        Me.HraBalanceControl.Location = New System.Drawing.Point(0, 0)
        Me.HraBalanceControl.Name = "HraBalanceControl"
        Me.HraBalanceControl.Size = New System.Drawing.Size(324, 82)
        Me.HraBalanceControl.TabIndex = 1
        '
        'HrqControl
        '
        Me.HrqControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HrqControl.Location = New System.Drawing.Point(0, 0)
        Me.HrqControl.Name = "HrqControl"
        Me.HrqControl.Size = New System.Drawing.Size(388, 82)
        Me.HrqControl.TabIndex = 0
        '
        'HraActivityControl
        '
        Me.HraActivityControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.HraActivityControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HraActivityControl.Location = New System.Drawing.Point(0, 0)
        Me.HraActivityControl.Name = "HraActivityControl"
        Me.HraActivityControl.Size = New System.Drawing.Size(716, 159)
        Me.HraActivityControl.TabIndex = 1
        '
        'AccumulatorsTab
        '
        Me.AccumulatorsTab.AutoScroll = True
        Me.AccumulatorsTab.BackColor = System.Drawing.Color.Transparent
        Me.AccumulatorsTab.Controls.Add(Me.AccumulatorsSplitContainer)
        Me.AccumulatorsTab.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AccumulatorsTab.Location = New System.Drawing.Point(4, 22)
        Me.AccumulatorsTab.Name = "AccumulatorsTab"
        Me.AccumulatorsTab.Size = New System.Drawing.Size(716, 245)
        Me.AccumulatorsTab.TabIndex = 4
        Me.AccumulatorsTab.Text = " ACCUMULATORS "
        Me.AccumulatorsTab.UseVisualStyleBackColor = True
        '
        'AccumulatorsSplitContainer
        '
        Me.AccumulatorsSplitContainer.BackColor = System.Drawing.Color.Transparent
        Me.AccumulatorsSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AccumulatorsSplitContainer.Location = New System.Drawing.Point(0, 0)
        Me.AccumulatorsSplitContainer.Name = "AccumulatorsSplitContainer"
        '
        'AccumulatorsSplitContainer.Panel1
        '
        Me.AccumulatorsSplitContainer.Panel1.Controls.Add(Me.AccumulatorValues)
        '
        'AccumulatorsSplitContainer.Panel2
        '
        Me.AccumulatorsSplitContainer.Panel2.Controls.Add(Me.AccumulatorDualCoverageValues)
        Me.AccumulatorsSplitContainer.Size = New System.Drawing.Size(716, 245)
        Me.AccumulatorsSplitContainer.SplitterDistance = 339
        Me.AccumulatorsSplitContainer.TabIndex = 6
        '
        'AccumulatorValues
        '
        Me.AccumulatorValues.BackColor = System.Drawing.Color.Transparent
        Me.AccumulatorValues.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AccumulatorValues.IsInEditMode = False
        Me.AccumulatorValues.Location = New System.Drawing.Point(0, 0)
        Me.AccumulatorValues.MinimumSize = New System.Drawing.Size(200, 150)
        Me.AccumulatorValues.Name = "AccumulatorValues"
        Me.AccumulatorValues.ShowClaimView = False
        Me.AccumulatorValues.ShowHistory = False
        Me.AccumulatorValues.ShowLineDetails = False
        Me.AccumulatorValues.Size = New System.Drawing.Size(339, 245)
        Me.AccumulatorValues.TabIndex = 5
        '
        'AccumulatorDualCoverageValues
        '
        Me.AccumulatorDualCoverageValues.BackColor = System.Drawing.Color.Transparent
        Me.AccumulatorDualCoverageValues.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AccumulatorDualCoverageValues.IsInEditMode = False
        Me.AccumulatorDualCoverageValues.Location = New System.Drawing.Point(0, 0)
        Me.AccumulatorDualCoverageValues.MinimumSize = New System.Drawing.Size(200, 150)
        Me.AccumulatorDualCoverageValues.Name = "AccumulatorDualCoverageValues"
        Me.AccumulatorDualCoverageValues.ShowClaimView = False
        Me.AccumulatorDualCoverageValues.ShowHistory = False
        Me.AccumulatorDualCoverageValues.ShowLineDetails = False
        Me.AccumulatorDualCoverageValues.Size = New System.Drawing.Size(373, 245)
        Me.AccumulatorDualCoverageValues.TabIndex = 6
        '
        'ProviderTab
        '
        Me.ProviderTab.AutoScroll = True
        Me.ProviderTab.BackColor = System.Drawing.Color.Transparent
        Me.ProviderTab.Controls.Add(Me.NpiRegistryControl)
        Me.ProviderTab.Controls.Add(Me.ProviderControl)
        Me.ProviderTab.Location = New System.Drawing.Point(4, 22)
        Me.ProviderTab.Name = "ProviderTab"
        Me.ProviderTab.Size = New System.Drawing.Size(192, 74)
        Me.ProviderTab.TabIndex = 8
        Me.ProviderTab.Text = " PROVIDER "
        Me.ProviderTab.UseVisualStyleBackColor = True
        '
        'NpiRegistryControl
        '
        Me.NpiRegistryControl.BackColor = System.Drawing.SystemColors.Control
        Me.NpiRegistryControl.Location = New System.Drawing.Point(433, 4)
        Me.NpiRegistryControl.Name = "NpiRegistryControl"
        Me.NpiRegistryControl.Size = New System.Drawing.Size(524, 751)
        Me.NpiRegistryControl.TabIndex = 1
        '
        'ProviderControl
        '
        Me.ProviderControl.BackColor = System.Drawing.SystemColors.Control
        Me.ProviderControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ProviderControl.Location = New System.Drawing.Point(0, 0)
        Me.ProviderControl.Name = "ProviderControl"
        Me.ProviderControl.NPI = New Decimal(New Integer() {0, 0, 0, 0})
        Me.ProviderControl.Size = New System.Drawing.Size(957, 755)
        Me.ProviderControl.TabIndex = 0
        Me.ProviderControl.TaxID = 0
        '
        'PrescriptionsTab
        '
        Me.PrescriptionsTab.BackColor = System.Drawing.Color.Transparent
        Me.PrescriptionsTab.Controls.Add(Me.PrescriptionsControl)
        Me.PrescriptionsTab.Location = New System.Drawing.Point(4, 22)
        Me.PrescriptionsTab.Name = "PrescriptionsTab"
        Me.PrescriptionsTab.Size = New System.Drawing.Size(192, 74)
        Me.PrescriptionsTab.TabIndex = 9
        Me.PrescriptionsTab.Text = " PRESCRIPTIONS "
        Me.PrescriptionsTab.UseVisualStyleBackColor = True
        '
        'PrescriptionsControl
        '
        Me.PrescriptionsControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PrescriptionsControl.Location = New System.Drawing.Point(0, 0)
        Me.PrescriptionsControl.Name = "PrescriptionsControl"
        Me.PrescriptionsControl.Size = New System.Drawing.Size(192, 74)
        Me.PrescriptionsControl.TabIndex = 0
        '
        'DentalTab
        '
        Me.DentalTab.BackColor = System.Drawing.Color.Transparent
        Me.DentalTab.Controls.Add(Me.DentalControl)
        Me.DentalTab.Location = New System.Drawing.Point(4, 22)
        Me.DentalTab.Name = "DentalTab"
        Me.DentalTab.Size = New System.Drawing.Size(192, 74)
        Me.DentalTab.TabIndex = 10
        Me.DentalTab.Text = " DENTAL "
        Me.DentalTab.UseVisualStyleBackColor = True
        '
        'DentalControl
        '
        Me.DentalControl.AutoScroll = True
        Me.DentalControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DentalControl.Location = New System.Drawing.Point(0, 0)
        Me.DentalControl.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.DentalControl.Name = "DentalControl"
        Me.DentalControl.Size = New System.Drawing.Size(192, 74)
        Me.DentalControl.TabIndex = 0
        '
        'COBTab
        '
        Me.COBTab.Controls.Add(Me.CobControl)
        Me.COBTab.Location = New System.Drawing.Point(4, 22)
        Me.COBTab.Name = "COBTab"
        Me.COBTab.Size = New System.Drawing.Size(192, 74)
        Me.COBTab.TabIndex = 11
        Me.COBTab.Text = " COB "
        Me.COBTab.UseVisualStyleBackColor = True
        '
        'CobControl
        '
        Me.CobControl.AppKey = "UFCW\Claims\"
        Me.CobControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.CobControl.BackColor = System.Drawing.SystemColors.Control
        Me.CobControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CobControl.Location = New System.Drawing.Point(0, 0)
        Me.CobControl.Name = "CobControl"
        Me.CobControl.Size = New System.Drawing.Size(192, 74)
        Me.CobControl.TabIndex = 0
        '
        'FreeTextTab
        '
        Me.FreeTextTab.BackColor = System.Drawing.Color.Transparent
        Me.FreeTextTab.Controls.Add(Me.FreeTextEditor)
        Me.FreeTextTab.Location = New System.Drawing.Point(4, 22)
        Me.FreeTextTab.Name = "FreeTextTab"
        Me.FreeTextTab.Size = New System.Drawing.Size(192, 74)
        Me.FreeTextTab.TabIndex = 13
        Me.FreeTextTab.Text = " FREETEXT "
        Me.FreeTextTab.UseVisualStyleBackColor = True
        '
        'FreeTextEditor
        '
        Me.FreeTextEditor.AppKey = "UFCW\Claims\"
        Me.FreeTextEditor.AppName = ""
        Me.FreeTextEditor.AutoScroll = True
        Me.FreeTextEditor.BackColor = System.Drawing.SystemColors.Control
        Me.FreeTextEditor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FreeTextEditor.Location = New System.Drawing.Point(0, 0)
        Me.FreeTextEditor.Name = "FreeTextEditor"
        Me.FreeTextEditor.Size = New System.Drawing.Size(192, 74)
        Me.FreeTextEditor.TabIndex = 0
        '
        'ImagingTab
        '
        Me.ImagingTab.AutoScroll = True
        Me.ImagingTab.Controls.Add(Me.UFCWDocsControl)
        Me.ImagingTab.Controls.Add(Me.Panel1)
        Me.ImagingTab.Location = New System.Drawing.Point(4, 22)
        Me.ImagingTab.Name = "ImagingTab"
        Me.ImagingTab.Padding = New System.Windows.Forms.Padding(3)
        Me.ImagingTab.Size = New System.Drawing.Size(716, 245)
        Me.ImagingTab.TabIndex = 12
        Me.ImagingTab.Text = " IMAGING "
        Me.ImagingTab.UseVisualStyleBackColor = True
        '
        'UFCWDocsControl
        '
        Me.UFCWDocsControl.AppKey = "UFCW\Claims\"
        Me.UFCWDocsControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UFCWDocsControl.Location = New System.Drawing.Point(3, 3)
        Me.UFCWDocsControl.Mode = SearchMode.SearchSQLOnly
        Me.UFCWDocsControl.Name = "UFCWDocsControl"
        Me.UFCWDocsControl.Size = New System.Drawing.Size(710, 239)
        Me.UFCWDocsControl.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(713, 242)
        Me.Panel1.TabIndex = 1
        '
        'SearchingLabel
        '
        Me.SearchingLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SearchingLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.SearchingLabel.Location = New System.Drawing.Point(3, 16)
        Me.SearchingLabel.Name = "SearchingLabel"
        Me.SearchingLabel.Size = New System.Drawing.Size(734, 274)
        Me.SearchingLabel.TabIndex = 1
        Me.SearchingLabel.Text = "Searching ..."
        Me.SearchingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.SearchingLabel.Visible = False
        '
        'FamilyInfoDataGridContextMenuStrip
        '
        Me.FamilyInfoDataGridContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LifeEventsToolStripMenuItem, Me.ContactInfoToolStripMenuItem, Me.FamilySummaryRemarksToolStripMenuItem, Me.PatientEligibilityToolStripMenuItem, Me.AlertHistoryToolStripMenuItem, Me.DiseaseManagementMenuItem})
        Me.FamilyInfoDataGridContextMenuStrip.Name = "ResultsMenu"
        Me.FamilyInfoDataGridContextMenuStrip.Size = New System.Drawing.Size(212, 136)
        '
        'LifeEventsToolStripMenuItem
        '
        Me.LifeEventsToolStripMenuItem.Name = "LifeEventsToolStripMenuItem"
        Me.LifeEventsToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.LifeEventsToolStripMenuItem.Text = "Life Events"
        '
        'ContactInfoToolStripMenuItem
        '
        Me.ContactInfoToolStripMenuItem.Name = "ContactInfoToolStripMenuItem"
        Me.ContactInfoToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.ContactInfoToolStripMenuItem.Text = "Contact Info"
        '
        'FamilySummaryRemarksToolStripMenuItem
        '
        Me.FamilySummaryRemarksToolStripMenuItem.Name = "FamilySummaryRemarksToolStripMenuItem"
        Me.FamilySummaryRemarksToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.FamilySummaryRemarksToolStripMenuItem.Text = "Family Summary Remarks"
        '
        'PatientEligibilityToolStripMenuItem
        '
        Me.PatientEligibilityToolStripMenuItem.Name = "PatientEligibilityToolStripMenuItem"
        Me.PatientEligibilityToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.PatientEligibilityToolStripMenuItem.Text = "Patient Eligibility"
        '
        'AlertHistoryToolStripMenuItem
        '
        Me.AlertHistoryToolStripMenuItem.Name = "AlertHistoryToolStripMenuItem"
        Me.AlertHistoryToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.AlertHistoryToolStripMenuItem.Text = "Alert History"
        '
        'DiseaseManagementMenuItem
        '
        Me.DiseaseManagementMenuItem.Name = "DiseaseManagementMenuItem"
        Me.DiseaseManagementMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.DiseaseManagementMenuItem.Text = "Disease Management"
        '
        'ResultsDataGridCustomContextMenu
        '
        Me.ResultsDataGridCustomContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResultsDisplayDocumentMenuItem, Me.ResultsHistoryMenuItem, Me.ResultsDisplayLineDetailsMenuItem, Me.ResultViewByClaimIDMenuItem, Me.FreeTextMenuItem, Me.ResultsAnnotateMenuItem, Me.ResultsAuditMenuItem, Me.ResultsDisplayEligibiltyMenuItem, Me.ReprintEOBMenuItem, Me.ReprocessMenuItem, Me.RemovePricingMenuItem, Me.ReOpenMenuItem})
        Me.ResultsDataGridCustomContextMenu.Name = "ResultsMenu"
        Me.ResultsDataGridCustomContextMenu.Size = New System.Drawing.Size(256, 268)
        '
        'ResultsDisplayDocumentMenuItem
        '
        Me.ResultsDisplayDocumentMenuItem.Name = "ResultsDisplayDocumentMenuItem"
        Me.ResultsDisplayDocumentMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.ResultsDisplayDocumentMenuItem.Text = "Display Document"
        '
        'ResultsHistoryMenuItem
        '
        Me.ResultsHistoryMenuItem.Name = "ResultsHistoryMenuItem"
        Me.ResultsHistoryMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.ResultsHistoryMenuItem.Text = "History"
        '
        'ResultsDisplayLineDetailsMenuItem
        '
        Me.ResultsDisplayLineDetailsMenuItem.Name = "ResultsDisplayLineDetailsMenuItem"
        Me.ResultsDisplayLineDetailsMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.ResultsDisplayLineDetailsMenuItem.Text = "Line Details"
        '
        'ResultViewByClaimIDMenuItem
        '
        Me.ResultViewByClaimIDMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ClaimIDToolStripMenuItem, Me.FamilyIDToolStripMenuItem, Me.PatientToolStripMenuItem})
        Me.ResultViewByClaimIDMenuItem.Name = "ResultViewByClaimIDMenuItem"
        Me.ResultViewByClaimIDMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.ResultViewByClaimIDMenuItem.Text = "Send To"
        Me.ResultViewByClaimIDMenuItem.ToolTipText = "Opens a new Customer Service instance using the search by criteria"
        '
        'ClaimIDToolStripMenuItem
        '
        Me.ClaimIDToolStripMenuItem.Name = "ClaimIDToolStripMenuItem"
        Me.ClaimIDToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.ClaimIDToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ClaimIDToolStripMenuItem.Text = "Claim ID"
        '
        'FamilyIDToolStripMenuItem
        '
        Me.FamilyIDToolStripMenuItem.Name = "FamilyIDToolStripMenuItem"
        Me.FamilyIDToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.FamilyIDToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.FamilyIDToolStripMenuItem.Text = "Family ID"
        '
        'PatientToolStripMenuItem
        '
        Me.PatientToolStripMenuItem.Name = "PatientToolStripMenuItem"
        Me.PatientToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.PatientToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.PatientToolStripMenuItem.Text = "Patient SSN"
        '
        'FreeTextMenuItem
        '
        Me.FreeTextMenuItem.Name = "FreeTextMenuItem"
        Me.FreeTextMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.FreeTextMenuItem.Text = "Free Text"
        '
        'ResultsAnnotateMenuItem
        '
        Me.ResultsAnnotateMenuItem.Name = "ResultsAnnotateMenuItem"
        Me.ResultsAnnotateMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.ResultsAnnotateMenuItem.Text = "Annotate"
        '
        'ResultsAuditMenuItem
        '
        Me.ResultsAuditMenuItem.Name = "ResultsAuditMenuItem"
        Me.ResultsAuditMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.ResultsAuditMenuItem.Text = "Flag for Audit Queue"
        '
        'ResultsDisplayEligibiltyMenuItem
        '
        Me.ResultsDisplayEligibiltyMenuItem.Name = "ResultsDisplayEligibiltyMenuItem"
        Me.ResultsDisplayEligibiltyMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.ResultsDisplayEligibiltyMenuItem.Text = "Eligibility"
        Me.ResultsDisplayEligibiltyMenuItem.Visible = False
        '
        'ReprintEOBMenuItem
        '
        Me.ReprintEOBMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ReprintEOBMMenuItem, Me.ReprintEOBPMenuItem, Me.ReprintEOBBothMenuItem})
        Me.ReprintEOBMenuItem.Name = "ReprintEOBMenuItem"
        Me.ReprintEOBMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.ReprintEOBMenuItem.Text = "Reprint EOB"
        '
        'ReprintEOBMMenuItem
        '
        Me.ReprintEOBMMenuItem.Name = "ReprintEOBMMenuItem"
        Me.ReprintEOBMMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.ReprintEOBMMenuItem.Text = "Member"
        '
        'ReprintEOBPMenuItem
        '
        Me.ReprintEOBPMenuItem.Name = "ReprintEOBPMenuItem"
        Me.ReprintEOBPMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.ReprintEOBPMenuItem.Text = "Provider"
        '
        'ReprintEOBBothMenuItem
        '
        Me.ReprintEOBBothMenuItem.Name = "ReprintEOBBothMenuItem"
        Me.ReprintEOBBothMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.ReprintEOBBothMenuItem.Text = "Both"
        '
        'ReprocessMenuItem
        '
        Me.ReprocessMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ReprocessPTOMenuItem, Me.ReprocessPTCMenuItem})
        Me.ReprocessMenuItem.Name = "ReprocessMenuItem"
        Me.ReprocessMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.ReprocessMenuItem.Text = "&Reprocess"
        '
        'ReprocessPTCMenuItem
        '
        Me.ReprocessPTCMenuItem.Name = "ReprocessPTCMenuItem"
        Me.ReprocessPTCMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.ReprocessPTCMenuItem.Text = "Pend To Current"
        '
        'RemovePricingMenuItem
        '
        Me.RemovePricingMenuItem.Name = "RemovePricingMenuItem"
        Me.RemovePricingMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.RemovePricingMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.RemovePricingMenuItem.Text = "R&emove From Pricing"
        Me.RemovePricingMenuItem.Visible = False
        '
        'ReOpenMenuItem
        '
        Me.ReOpenMenuItem.Name = "ReOpenMenuItem"
        Me.ReOpenMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.ReOpenMenuItem.Size = New System.Drawing.Size(255, 22)
        Me.ReOpenMenuItem.Text = "Re&Open Completed Claim"
        Me.ReOpenMenuItem.Visible = False
        '
        'ToolTip1
        '
        Me.ToolTip1.AutomaticDelay = 50
        Me.ToolTip1.AutoPopDelay = 5500
        Me.ToolTip1.InitialDelay = 50
        Me.ToolTip1.ReshowDelay = 10
        Me.ToolTip1.ShowAlways = True
        '
        'ToolTipTriggerLabel
        '
        Me.ToolTipTriggerLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ToolTipTriggerLabel.Location = New System.Drawing.Point(320, 0)
        Me.ToolTipTriggerLabel.Name = "ToolTipTriggerLabel"
        Me.ToolTipTriggerLabel.Size = New System.Drawing.Size(200, 16)
        Me.ToolTipTriggerLabel.TabIndex = 0
        '
        'QueueImageList
        '
        Me.QueueImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.QueueImageList.ImageSize = New System.Drawing.Size(16, 16)
        Me.QueueImageList.TransparentColor = System.Drawing.Color.Transparent
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'PendToOriginalToolStripMenuItem
        '
        Me.PendToOriginalToolStripMenuItem.Name = "PendToOriginalToolStripMenuItem"
        Me.PendToOriginalToolStripMenuItem.Size = New System.Drawing.Size(32, 19)
        '
        'PendToCurrentToolStripMenuItem
        '
        Me.PendToCurrentToolStripMenuItem.Name = "PendToCurrentToolStripMenuItem"
        Me.PendToCurrentToolStripMenuItem.Size = New System.Drawing.Size(32, 19)
        '
        'BlinkTimer
        '
        Me.BlinkTimer.Interval = 200
        '
        'TaskTimer
        '
        Me.TaskTimer.Interval = 1000
        '
        'EligibilityDataSet
        '
        Me.EligibilityDataSet.DataSetName = "EligibilityDataSet"
        Me.EligibilityDataSet.Locale = New System.Globalization.CultureInfo("en-US")
        Me.EligibilityDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'CustomerServiceControl
        '
        Me.Controls.Add(Me.SearchResultsGroupBox)
        Me.Controls.Add(Me.SearchCriteriaGroupBox)
        Me.Controls.Add(Me.ToolTipTriggerLabel)
        Me.Name = "CustomerServiceControl"
        Me.Size = New System.Drawing.Size(752, 740)
        Me.SearchCriteriaGroupBox.ResumeLayout(False)
        Me.SearchCriteriaGroupBox.PerformLayout()
        Me.FamilyInformationGroupBox.ResumeLayout(False)
        Me.FamilyInformationGroupBox.PerformLayout()
        Me.DocumentDetailsGroupBox.ResumeLayout(False)
        Me.DocumentDetailsGroupBox.PerformLayout()
        Me.CheckGroupBox.ResumeLayout(False)
        Me.CheckGroupBox.PerformLayout()
        Me.ProcedureGroupBox.ResumeLayout(False)
        Me.ProcedureGroupBox.PerformLayout()
        Me.AccidentGroupBox.ResumeLayout(False)
        Me.AccumulatorsContextMenu.ResumeLayout(False)
        Me.SearchResultsGroupBox.ResumeLayout(False)
        Me.TabControl.ResumeLayout(False)
        Me.GenTab.ResumeLayout(False)
        Me.MainPanel.ResumeLayout(False)
        Me.MainPanel.PerformLayout()
        CType(Me.CustomerServiceResultsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DemographicsTab.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.SplitContainer3.Panel1.ResumeLayout(False)
        Me.SplitContainer3.Panel1.PerformLayout()
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer3.ResumeLayout(False)
        Me.ParticipantToolStrip.ResumeLayout(False)
        Me.ParticipantToolStrip.PerformLayout()
        CType(Me.ParticipantDemographicsGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer4.Panel1.ResumeLayout(False)
        Me.SplitContainer4.Panel1.PerformLayout()
        Me.SplitContainer4.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer4.ResumeLayout(False)
        Me.DualParticipantToolStrip.ResumeLayout(False)
        Me.DualParticipantToolStrip.PerformLayout()
        CType(Me.DualParticipantDemographicsGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.EligibilityTab.ResumeLayout(False)
        Me.EligibilitySplitContainer.Panel1.ResumeLayout(False)
        Me.EligibilitySplitContainer.Panel2.ResumeLayout(False)
        CType(Me.EligibilitySplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.EligibilitySplitContainer.ResumeLayout(False)
        Me.EligibilityHoursTab.ResumeLayout(False)
        Me.EligibilityHoursSplitContainer.Panel1.ResumeLayout(False)
        Me.EligibilityHoursSplitContainer.Panel2.ResumeLayout(False)
        CType(Me.EligibilityHoursSplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.EligibilityHoursSplitContainer.ResumeLayout(False)
        Me.HoursTab.ResumeLayout(False)
        Me.HoursSplitContainer.Panel1.ResumeLayout(False)
        Me.HoursSplitContainer.Panel2.ResumeLayout(False)
        CType(Me.HoursSplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.HoursSplitContainer.ResumeLayout(False)
        Me.PremiumsHistoryTab.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.CoverageTab.ResumeLayout(False)
        Me.HRAActivityTab.ResumeLayout(False)
        Me.HRASplitContainer.Panel1.ResumeLayout(False)
        Me.HRASplitContainer.Panel2.ResumeLayout(False)
        CType(Me.HRASplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.HRASplitContainer.ResumeLayout(False)
        Me.HRQSplitContainer.Panel1.ResumeLayout(False)
        Me.HRQSplitContainer.Panel2.ResumeLayout(False)
        CType(Me.HRQSplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.HRQSplitContainer.ResumeLayout(False)
        Me.AccumulatorsTab.ResumeLayout(False)
        Me.AccumulatorsSplitContainer.Panel1.ResumeLayout(False)
        Me.AccumulatorsSplitContainer.Panel2.ResumeLayout(False)
        CType(Me.AccumulatorsSplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.AccumulatorsSplitContainer.ResumeLayout(False)
        Me.ProviderTab.ResumeLayout(False)
        Me.PrescriptionsTab.ResumeLayout(False)
        Me.DentalTab.ResumeLayout(False)
        Me.COBTab.ResumeLayout(False)
        Me.FreeTextTab.ResumeLayout(False)
        Me.ImagingTab.ResumeLayout(False)
        Me.FamilyInfoDataGridContextMenuStrip.ResumeLayout(False)
        Me.ResultsDataGridCustomContextMenu.ResumeLayout(False)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EligibilityDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Private Variables"

    Private Shared _TraceCloning As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private Const _CRYPTORKEY As String = "UFCW CMS ACCESS"

    Private _APPKEY As String = "UFCW\Claims\"
    Private _DisableSearchUI As Boolean
    Private _SearchUIMode As Integer
    Private _CollapseUIMode As Boolean = False

    Private _HTI As System.Windows.Forms.DataGrid.HitTestInfo

    Private _StatusDV As DataView
    Private MCnt As Label
    Private _ShowLabel As Label
    Private _ShowLabel2 As Label

    Private WithEvents TypeDropDown As ComboBox
    Private WithEvents TypeDropDown2 As ComboBox
    Private WithEvents MainGrid As New DataGrid

    Private _Loading As Boolean = True
    Private _Loaded As Boolean = False
    Private _Cancel As Boolean = False

    Private Shared _DiagnosisDT As DataTable
    Private Shared _BillTypeDT As DataTable
    Private Shared _ProcedureDT As DataTable
    Private Shared _ModifierDT As DataTable
    Private Shared _ReasonDT As DataTable
    Private Shared _PlaceOfServiceDT As DataTable
    Private Shared _RegAlertReasonDT As DataTable
    Private Shared _StatusDT As DataTable
    Private Shared _DocTypesDT As New DataTable
    Private Shared _LetterDocTypesDT As New DataTable

    Private Shared _IsBatchSearch As Boolean
    Private Shared _IsMemberSearch As Boolean
    Private Shared _IsProviderSearch As Boolean
    Private Shared _IsClaimSearch As Boolean
    Private Shared _IsDocumentSearch As Boolean
    Private Shared _IsAccumClaimSearch As Boolean = True
    Private Shared _IsAccumFamilySearch As Boolean = True

    Private Shared _PatientsDS As DataSet
    Private Shared _PartDR As DataRow
    Private Shared _PatDR As DataRow

    Private _FoundPatient As Boolean = False
    Private _FoundParticipant As Boolean = False

    Private _OriginalDT As DataTable

    Private _SearchResultsDT As DataTable
    Private _AnnotationsDS As New AnnotationsDataSet

    Private _SSNForMenu As String
    Private _DOSDuration As Integer = CInt(ConfigurationManager.AppSettings("DOS_DURATION"))
    Private ExtendedSearchMorningStart As Date
    Private ExtendedSearchMorningEnd As Date
    Private ExtendedSearchAfternoonStart As Date
    Private ExtendedSearchAfternoonEnd As Date
    Private ExtendedSearchEveningStart As Date
    Private ExtendedSearchEveningEnd As Date

    Private _HoverLastRecordedCell As New DataGridCell

    Private _IncludeAccumulators As Boolean = True
    Private _IncludeDemographics As Boolean = True
    Private _IncludeHRAActivity As Boolean = True
    Private _IncludeHRABalance As Boolean = True
    Private _IncludeImaging As Boolean = True
    Private _IncludeProvider As Boolean = True
    Private _IncludePrescriptions As Boolean = True
    Private _IncludeDental As Boolean = True

    Private _EmployeeAccess As Boolean = False
    Private _LocalEmployeeAccess As Boolean = False

    Private _IsHRATabs As String = ConfigurationManager.AppSettings("HRATABS")

    Private CallClaimAccum As New MethodInvoker(AddressOf PopulateClaimAccumulatorForm)
    Private CallAccum As New MethodInvoker(AddressOf PopulateAccumulatorForm)


    Private _Eligcolor As New SolidBrush(Color.FromArgb(255, 124, 227, 0)) ''Green
    Private _PartialEligcolor As New SolidBrush(Color.FromArgb(255, 209, 255, 154)) ''Light Green
    Private _InEligcolor As New SolidBrush(Color.FromArgb(255, 253, 126, 106)) '' Red
    Private _PartialInEligcolor As New SolidBrush(Color.FromArgb(255, 254, 221, 215)) ''Pink
    Private _BlinkStarttime As Nullable(Of DateTime)

    Private _PrescriptionsDS As PrescriptionsDataSet
    Private _DentalDS As DataSet
    Private _DentalPREAuthDS As DataSet
    Private _DentalPendDS As DataSet
    Private _HRADS As DataSet
    Private _HRABalanceDS As DataSet
    Private _HRQDS As DataSet
    Private _HoursDS As DataSet
    Private _HoursDualCoverageDS As DataSet
    Private _EligibilityHoursDS As DataSet
    Private _EligibilityHoursDualCoverageDS As DataSet
    Private _COBDS As DataSet
    Private _DocsinBatchDS As DataSet
    Private _CoverageHistoryDS As DataSet
    Private _PremiumsDS As DataSet
    Private _PremLetterDS As DataSet

    Private _PopulateHRATask As Task
    Private _PopulateHRQTask As Task
    Private _PopulateHRABalanceTask As Task
    Private _PopulateBatchTask As Task
    Private _PopulateCoverageHistoryTask As Task(Of DataSet)
    Private _PopulateHoursTask As Task(Of DataSet)
    Private _PopulateHoursDualCoverageTask As Task(Of DataSet)

    Private _PopulateEligibilityHoursTask As Task(Of DataSet)
    Private _PopulateEligibilityHoursDualCoverageTask As Task(Of DataSet)

    Private _PopulatePrescriptionsTask As Task(Of PrescriptionsDataSet)
    Private _PopulateDentalTask As Task(Of DataSet)
    Private _PopulateDentalPREAuthTask As Task(Of DataSet)
    Private _PopulateDentalPendTask As Task(Of DataSet)
    Private _PopulateCOBTask As Task(Of DataSet)
    Private _PopulatePremiumsEnrollmentTask As Task(Of DataSet)
    Private _PopulatePremiumsTask As Task(Of DataSet)

    Private rownumber As Integer = 1000

    Private _ParticipantFamilyBS As New BindingSource
    Private _ENV As String
#End Region

#Region "Public Properties"
    <System.ComponentModel.Browsable(True), System.ComponentModel.DefaultValue(False), System.ComponentModel.Description("Gets or Sets if Search Criteria section of control is displayed")>
    Public Property DisableSearchUI() As Boolean
        Get
            Return _DisableSearchUI
        End Get
        Set(ByVal value As Boolean)
            _DisableSearchUI = value

            If _DisableSearchUI Then
                SearchCriteriaGroupBox.Visible = False
                SearchResultsGroupBox.Dock = DockStyle.Fill
            Else
            End If

        End Set
    End Property

    <DefaultValue("UFCW\Claims\"), Browsable(True), System.ComponentModel.Description("Represents the application key used when accessing the registry.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the CheckAmount.")>
    Public Property CheckAmount() As String
        Get
            Return Me.CheckAmountTextBox.Text
        End Get
        Set(ByVal value As String)
            If Me.CheckAmountTextBox.Text IsNot Nothing Then
                Me.CheckAmountTextBox.Text = value
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property CheckAmountEnabled() As Boolean
        Get
            Return Me.CheckAmountTextBox.Enabled
        End Get
        Set(ByVal value As Boolean)
            Me.CheckAmountTextBox.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Denied() As Boolean
        Get
            Return Me.DeniedCheckBox.Checked
        End Get
        Set(ByVal value As Boolean)
            Me.DeniedCheckBox.Checked = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Patient SSN.")>
    Public Property PatientSSN() As String
        Get
            Return Me.PatientSSNTextBox.Text
        End Get
        Set(ByVal value As String)
            If Me.PatientSSNTextBox.Text IsNot Nothing Then
                Me.PatientSSNTextBox.Text = value
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property PatientSSNEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.PatientSSNTextBox.Enabled = value
            Me.PatientsSSNlabel.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the ProviderID.")>
    Public Property ProviderID() As String
        Get
            Return Me.ProviderIDTextBox.Text
        End Get
        Set(ByVal value As String)
            If Me.ProviderIDTextBox.Text IsNot Nothing Then
                ProviderIDTextBox.Text = value
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property ProviderIdEnabled() As Boolean
        Set(ByVal value As Boolean)
            ProviderIDTextBox.Enabled = value
            Me.ProviderIDLabel.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the BatchNumber.")>
    Public Property BatchNumber() As String
        Get
            Return Me.BatchNumberTextBox.Text
        End Get
        Set(ByVal value As String)
            If Me.BatchNumberTextBox.Text IsNot Nothing Then
                Me.BatchNumberTextBox.Text = value
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property BatchNumberEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.BatchNumberTextBox.Enabled = value
            Me.BatchNumberLabel.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participant SSN.")>
    Public Property ParticipantSSN() As Integer?
        Get
            Return If(Me.ParticipantSSNTextBox.Text.Trim.Length > 0, CType(Me.ParticipantSSNTextBox.Text.Trim, Integer?), Nothing)
        End Get
        Set(ByVal value As Integer?)
            If value IsNot Nothing Then
                Me.ParticipantSSNTextBox.Text = CStr(value)
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property ParticipantSSNEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.ParticipantSSNTextBox.Enabled = value
            Me.ParticipantSSNLabel.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID.")>
    Public Property FamilyID() As Integer?
        Get
            Return If(Me.FamilyIDTextBox.Text.Trim.Length > 0, CType(Me.FamilyIDTextBox.Text.Trim, Integer?), Nothing)
        End Get
        Set(ByVal value As Integer?)
            If value IsNot Nothing Then
                Me.FamilyIDTextBox.Text = CStr(value)
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property FamilyIDEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.FamilyIDTextBox.Enabled = value
            Me.FamilyIdLabel.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FileNet DocumentID.")>
    Public Property DocID() As Integer
        Get
            Return CInt(Me.DocIDTextBox.Text)
        End Get
        Set(ByVal value As Integer)
            Me.DocIDTextBox.Text = CStr(value)
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property DocIDEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.DocIDTextBox.Enabled = value
            Me.DocIdLabel.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participants LastName.")>
    Public Property PatientLastName() As String
        Get
            Return Me.PatientLastNameTextBox.Text
        End Get
        Set(ByVal value As String)
            If Me.PatientLastNameTextBox.Text IsNot Nothing Then
                Me.PatientLastNameTextBox.Text = value
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property PatientLastNameEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.PatientLastNameTextBox.Enabled = value
            Me.PatientLastNameLabel.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participants RelationID.")>
    Public Property RelationID() As Short?
        Get
            Return If(Me.RelationIDTextBox.Text.Trim.Length > 0, CType(Me.RelationIDTextBox.Text.Trim, Short?), Nothing)
        End Get
        Set(ByVal value As Short?)
            If value IsNot Nothing Then
                Me.RelationIDTextBox.Text = CStr(value)
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property RelationIdEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.RelationIDTextBox.Enabled = value
            Me.RelationIdLabel.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the ClaimID.")>
    Public Property ClaimID() As String
        Get
            Return Me.ClaimIDTextBox.Text
        End Get
        Set(ByVal value As String)
            If Me.ClaimIDTextBox.Text IsNot Nothing Then
                Me.ClaimIDTextBox.Text = value
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property ClaimIDEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.ClaimIDTextBox.Enabled = value
            Me.ClaimIdLabel.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the First Date of Service.")>
    Public Property DateOfServiceFrom() As Date
        Get
            Return Me.DateOfServiceFromDateTimePicker.Value
        End Get
        Set(ByVal value As Date)
            DateOfServiceFromDateTimePicker.Value = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the First Received Date.")>
    Public Property ReceivedDateFrom() As Date
        Get
            Return Me.ReceivedFromDateTimePicker.Value
        End Get
        Set(ByVal value As Date)
            Me.ReceivedFromDateTimePicker.Value = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Last Received Date.")>
    Public Property ReceivedDateTo() As Date
        Get
            Return Me.ReceivedToDateTimePicker.Value
        End Get
        Set(ByVal value As Date)
            Me.ReceivedToDateTimePicker.Value = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the First Processed Date.")>
    Public Property ProcessedDateFrom() As Date
        Get
            Return Me.ProcessedDateFromDateTimePicker.Value
        End Get
        Set(ByVal value As Date)
            Me.ProcessedDateFromDateTimePicker.Value = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Last Processed Date.")>
    Public Property ProcessedDateTo() As Date
        Get
            Return Me.ProcessedDateToDateTimePicker.Value
        End Get
        Set(ByVal value As Date)
            Me.ProcessedDateToDateTimePicker.Value = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Last Date of Service.")>
    Public Property DateOfServiceTo() As Date
        Get
            Return Me.DateOfServiceToDateTimePicker.Value
        End Get
        Set(ByVal value As Date)
            DateOfServiceToDateTimePicker.Value = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property ReceivedDateEnabled() As Boolean
        Set(ByVal value As Boolean)
            ReceivedToDateTimePicker.Enabled = value
            ReceivedFromDateTimePicker.Enabled = value
            Me.BetweenLabel3.Enabled = value
            Me.AndLabel3.Enabled = value
            Me.ReceivedDateCheckBox.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property DateOfServiceEnabled() As Boolean
        Set(ByVal value As Boolean)
            DateOfServiceToDateTimePicker.Enabled = value
            DateOfServiceFromDateTimePicker.Enabled = value
            Me.BetweenLabel.Enabled = value
            Me.AndLabel.Enabled = value
            Me.DateOfServiceCheckBox.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property ProcessDateEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.ProcessedDateFromDateTimePicker.Enabled = value
            Me.ProcessedDateToDateTimePicker.Enabled = value
            Me.BetweenLabel2.Enabled = value
            Me.AndLabel2.Enabled = value
            Me.ProcessedDateCheckBox.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the DocumentType.")>
    Public Property DocType() As String
        Get
            Return Me.DocTypeComboBox.Text
        End Get
        Set(ByVal value As String)
            Me.DocTypeComboBox.SelectedItem = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property DocTypeEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.DocTypeComboBox.Enabled = value
            Me.DocTypeCheckBox.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Provider Name.")>
    Public Property ProviderName() As String
        Get
            Return Me.ProviderNameTextBox.Text
        End Get
        Set(ByVal value As String)
            If Me.ProviderNameTextBox.Text IsNot Nothing Then
                Me.ProviderNameTextBox.Text = value
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property DoctorsLastNameEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.ProviderNameTextBox.Enabled = value
            Me.ProviderNameLabel.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Check Number.")>
    Public Property CheckNumber() As String
        Get
            Return Me.CheckNumberTextBox.Text
        End Get
        Set(ByVal value As String)
            If Me.CheckNumberTextBox.Text IsNot Nothing Then
                Me.CheckNumberTextBox.Text = value
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property CheckInfoEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.CheckGroupBox.Enabled = value
            Me.CheckNumberTextBox.Enabled = value
            Me.CheckNumberLabel.Enabled = value
            Me.CheckAmountTextBox.Enabled = value
            Me.CheckAmountLabel.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property Chiropractic() As Boolean
        Get
            Return Me.ChiroCheckBox.Checked
        End Get
        Set(ByVal value As Boolean)
            Me.ChiroCheckBox.Checked = value
            If value = True Then
                ChiroCheckBox.Visible = True
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Procedure Code.")>
    Public Property ProcedureCode() As String
        Get
            Return Me.ProcedureCodeTextBox.Text
        End Get
        Set(ByVal value As String)
            If Me.ProcedureCodeTextBox.Text IsNot Nothing Then
                Me.ProcedureCodeTextBox.Text = value
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property ProcedureCodeEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.ProcedureCodeTextBox.Enabled = value
            Me.ProcedureCodeLabel.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Modifier.")>
    Public Property Modifier() As String
        Get
            Return Me.ModifierTextBox.Text
        End Get
        Set(ByVal value As String)
            If Me.ModifierTextBox.Text IsNot Nothing Then
                Me.ModifierTextBox.Text = value
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Modifier.")>
    Public Property CollapseUI() As Integer
        Get
            Return CInt(_CollapseUIMode)
        End Get
        Set(ByVal value As Integer)
            _CollapseUIMode = CBool(value)
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property ModifierEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.ModifierTextBox.Enabled = value
            Me.ModifierLabel.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the BillType.")>
    Public Property BillType() As String
        Get
            Return Me.BillTypeTextBox.Text
        End Get
        Set(ByVal value As String)
            If Me.BillTypeTextBox.Text IsNot Nothing Then
                Me.BillTypeTextBox.Text = value
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property BillTypeEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.BillTypeTextBox.Enabled = value
            Me.BillTypeLabel.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Diagnosis.")>
    Public Property Diagnosis() As String
        Get
            Return Me.DiagnosisCodesTextBox.Text
        End Get
        Set(ByVal value As String)
            If Me.DiagnosisCodesTextBox.Text IsNot Nothing Then
                Me.DiagnosisCodesTextBox.Text = value
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property DiagnosisEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.DiagnosisCodesTextBox.Enabled = value
            Me.DiagnosisLabel.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Place of Service.")>
    Public Property PlaceOfService() As String
        Get
            Return Me.PlaceOfServiceTextBox.Text
        End Get
        Set(ByVal value As String)
            If Me.PlaceOfServiceTextBox.Text IsNot Nothing Then
                Me.PlaceOfServiceTextBox.Text = value
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property PlaceOfServiceEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.PlaceOfServiceTextBox.Enabled = value
            Me.PlaceOfServiceLabel.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Par / Non Par Flag.")>
    Public Property Par() As String
        Get
            Return Me.ProviderComboxBox.Text
        End Get
        Set(ByVal value As String)
            If Me.ProviderComboxBox.Text IsNot Nothing Then
                Me.ProviderComboxBox.Text = value
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property ParEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.ProviderCheckBox.Enabled = value
            Me.ProviderComboxBox.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Indicident Date.")>
    Public Property IncidentDate() As Date
        Get
            Return Me.IncidentDateDateTimePicker.Value
        End Get
        Set(ByVal value As Date)
            Me.IncidentDateDateTimePicker.Value = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property IncidentDateEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.IncidentDateDateTimePicker.Enabled = value
            Me.IncidentDateCheckBox.Enabled = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Accident Type.")>
    Public Property AccidentType() As String
        Get
            Return Me.AccidentTypeComboBox.Text
        End Get
        Set(ByVal value As String)
            Me.AccidentTypeComboBox.Text = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public WriteOnly Property AccidentTypeEnabled() As Boolean
        Set(ByVal value As Boolean)
            Me.AccidentTypeComboBox.Enabled = value
            Me.AccidentCheckBox.Enabled = value
        End Set
    End Property

#End Region

#Region "Other Properties"
    Shared Property ProcedureListOfValues() As DataTable
        Get
            Return _ProcedureDT
        End Get
        Set(ByVal value As DataTable)
            _ProcedureDT = value
        End Set
    End Property
    Shared Property BillTypeListOfValues() As DataTable
        Get
            Return _BillTypeDT
        End Get
        Set(ByVal value As DataTable)
            _BillTypeDT = value
        End Set
    End Property
    Shared Property ModifierListOfValues() As DataTable
        Get
            Return _ModifierDT
        End Get
        Set(ByVal value As DataTable)
            _ModifierDT = value
        End Set
    End Property
    Shared ReadOnly Property DiagnosisListOfValues() As DataTable
        Get
            Return _DiagnosisDT
        End Get
    End Property
    Shared ReadOnly Property POSListOfValues() As DataTable
        Get
            Return _PlaceOfServiceDT
        End Get
    End Property
    Shared ReadOnly Property ReasonListOfValues() As DataTable
        Get
            Return _ReasonDT
        End Get
    End Property
#End Region

#Region "Form/Control Events"

    Private Sub ClearAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearAllButton.Click

        Try
            SearchButton.Enabled = False
            ClearAllButton.Enabled = False
            PatientSearchButton.Enabled = False

            ClearAll()
            ErrorProvider1.Clear()

            ClearAllButton.Visible = True

        Catch ex As Exception
            Throw
        Finally
            SearchButton.Enabled = True
            ClearAllButton.Enabled = True
            PatientSearchButton.Enabled = True

        End Try

    End Sub

    Private Sub FreeTextMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FreeTextMenuItem.Click
        If _SearchResultsDT Is Nothing Then Return

        Dim Frm As FreeTextForm
        Dim DV As DataView

        Try

            Frm = New FreeTextForm
            DV = CType(CustomerServiceResultsDataGrid.DataSource, DataView)

            Frm.Show()
            Frm.LoadControl(CInt(DV(CustomerServiceResultsDataGrid.CurrentRowIndex)("FAMILY_ID")), UFCWGeneral.IsNullShortHandler(DV(CustomerServiceResultsDataGrid.CurrentRowIndex)("RELATION_ID")), CInt(DV(CustomerServiceResultsDataGrid.CurrentRowIndex)("PART_SSN")), CInt(DV(CustomerServiceResultsDataGrid.CurrentRowIndex)("PAT_SSN")), UFCWGeneral.IsNullStringHandler(DV(CustomerServiceResultsDataGrid.CurrentRowIndex)("PART_FNAME")), UFCWGeneral.IsNullStringHandler(DV(CustomerServiceResultsDataGrid.CurrentRowIndex)("PART_LNAME")), UFCWGeneral.IsNullStringHandler(DV(CustomerServiceResultsDataGrid.CurrentRowIndex)("PAT_FNAME")), UFCWGeneral.IsNullStringHandler(DV(CustomerServiceResultsDataGrid.CurrentRowIndex)("PAT_LNAME")))

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub IsDigit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles FamilyIDTextBox.KeyPress, RelationIDTextBox.KeyPress,
                                                                                                                    ProviderIDTextBox.KeyPress, CheckNumberTextBox.KeyPress, CheckAmountTextBox.KeyPress
        If Char.IsDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub
    Private Sub ProcedureCodeTextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ProcedureCodeTextBox.KeyPress

        If Char.IsLetterOrDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) OrElse e.KeyChar = "-" OrElse e.KeyChar = "," Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub DiagnosisTextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles DiagnosisCodesTextBox.KeyPress

        If Char.IsLetterOrDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) OrElse e.KeyChar = "-" OrElse e.KeyChar = "," OrElse e.KeyChar = "." Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub IsLetterOrDigitOrPeriod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles BatchNumberTextBox.KeyPress
        If Char.IsLetterOrDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) OrElse e.KeyChar = "." Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub IsLetterOrDigit_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles ClaimIDTextBox.KeyPress, ModifierTextBox.KeyPress, PlaceOfServiceTextBox.KeyPress,
                                                                                                                              BillTypeTextBox.KeyPress
        If Char.IsLetterOrDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub HandleStatusChange_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DocTypesComboBox.SelectedIndexChanged, StatusTypesComboBox.SelectedIndexChanged,
                                                                                                                            AdjustersComboBox.SelectedIndexChanged
        If _Loading = False Then HandleStatusChange()

    End Sub

    Private Sub AccumulatorsSplitContainer_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles AccumulatorsSplitContainer.Resize

        Try

            AccumulatorValues.Width = AccumulatorsSplitContainer.Panel1.Width
            AccumulatorValues.Height = AccumulatorsSplitContainer.Panel1.Height

            AccumulatorDualCoverageValues.Width = AccumulatorsSplitContainer.Panel2.Width
            AccumulatorDualCoverageValues.Height = AccumulatorsSplitContainer.Panel2.Height

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub AccumulatorsSplitContainer_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles AccumulatorsSplitContainer.SplitterMoved

        Try

            AccumulatorValues.Width = AccumulatorsSplitContainer.Panel1.Width
            AccumulatorValues.Height = AccumulatorsSplitContainer.Panel1.Height

            AccumulatorDualCoverageValues.Width = AccumulatorsSplitContainer.Panel2.Width
            AccumulatorDualCoverageValues.Height = AccumulatorsSplitContainer.Panel2.Height

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Sub TabControl_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl.SelectedIndexChanged
        'If Clearing = True Or Loading = True Then Exit Sub

        If _SearchResultsDT Is Nothing Then Return

        Dim TC As TabControl
        Dim DT As DataTable
        Dim DT2 As DataTable
        Dim ClassDV As DataView
        Dim DT3 As DataTable

        'If Loading = True Then Exit Sub

        'Loading = True
        Try
            TC = CType(sender, TabControl)

            If TC.SelectedTab Is Nothing Then Return
            If TC.SelectedTab.Text = "Class" Then Return

            If (TC.SelectedTab Is EligibilityTab) OrElse
                (TC.SelectedTab Is HoursTab) OrElse
                (TC.SelectedTab Is EligibilityHoursTab) OrElse
                (TC.SelectedTab Is COBTab) OrElse
                (TC.SelectedTab Is DemographicsTab) OrElse
                (TC.SelectedTab Is AccumulatorsTab) OrElse
                (TC.SelectedTab Is PremiumsHistoryTab) OrElse
                (TC.SelectedTab Is ProviderTab) OrElse
                (TC.SelectedTab Is PrescriptionsTab) OrElse
                (TC.SelectedTab Is DentalTab) OrElse
                (TC.SelectedTab Is ImagingTab) OrElse
                (TC.SelectedTab Is FreeTextTab) OrElse
                (TC.SelectedTab Is CoverageTab) OrElse
                (TC.SelectedTab Is HRAActivityTab) Then

                StatusTypesComboBox.Visible = False
                ShowStatusLabel.Visible = False
                DocTypesComboBox.Visible = False
                ShowDocTypeLabel.Visible = False
                MatchesLabel.Visible = False
            Else
                StatusTypesComboBox.Visible = True
                ShowStatusLabel.Visible = True
                DocTypesComboBox.Visible = True
                ShowDocTypeLabel.Visible = True
                MatchesLabel.Visible = True
            End If

            Me.SearchingLabel.Visible = False

            Select Case True
                Case TC.SelectedTab.Name = "MEDICAL"

                    _StatusDV = New DataView(_SearchResultsDT, "DOC_CLASS = '" & TC.SelectedTab.Text & "'", "STATUS", DataViewRowState.CurrentRows)

                    DT = CMSDALCommon.SelectDistinctAndSorted("", _StatusDV.Table, "STATUS", True)
                    StatusTypesComboBox.DataSource = DT
                    StatusTypesComboBox.DisplayMember = "STATUS"

                    DT2 = CMSDALCommon.SelectDistinctAndSorted("", _StatusDV.Table, "DOC_TYPE", True)
                    DocTypesComboBox.DataSource = DT2
                    DocTypesComboBox.DisplayMember = "DOC_TYPE"

                    If _StatusDV.Table.Columns.Contains("PROCESSED_BY") Then
                        DT3 = CMSDALCommon.SelectDistinctAndSorted("", _StatusDV.Table, "PROCESSED_BY", True)
                        AdjustersComboBox.DataSource = DT3
                        AdjustersComboBox.DisplayMember = "PROCESSED_BY"
                    End If

                    ClassDV = New DataView(_SearchResultsDT)
                    ClassDV.RowFilter = "DOC_CLASS = '" & TC.SelectedTab.Text & "'"

                    CustomerServiceResultsDataGrid.DataSource = ClassDV
                    CustomerServiceResultsDataGrid.Sort = If(CustomerServiceResultsDataGrid.LastSortedBy, CustomerServiceResultsDataGrid.DefaultSort)

                    If StatusTypesComboBox.Text <> "(all)" Then ClassDV.RowFilter += " And STATUS = '" & StatusTypesComboBox.Text & "'"
                    If DocTypesComboBox.Text <> "(all)" Then ClassDV.RowFilter += " And DOC_TYPE = '" & DocTypesComboBox.Text & "'"

                    If _StatusDV.Table.Columns.Contains("PROCESSED_BY") Then
                        If AdjustersComboBox.Text <> "(all)" Then ClassDV.RowFilter += " And PROCESSED_BY = '" & AdjustersComboBox.Text & "'"
                    End If

                    MatchesLabel.Text = ClassDV.Count & " of " & _SearchResultsDT.Rows.Count & " matches"

                Case TC.SelectedTab Is AccumulatorsTab
                    TC.SelectedTab.Controls.Clear()

                    If _PatientsDS IsNot Nothing Then

                        Me.SearchingLabel.Visible = True

                        Me.SearchingLabel.BringToFront()
                        Me.SearchingLabel.Text = "Loading Accumulators... Please Wait"

                        If _SearchResultsDT.Rows.Count = 1 AndAlso _SearchResultsDT.Rows(0)("STATUS").ToString = "RESTRICTED" Then

                            _IncludeAccumulators = False

                        ElseIf _IsAccumClaimSearch AndAlso Not _IsAccumFamilySearch Then
                            _IncludeAccumulators = True
                            Me.Invoke(CallClaimAccum)
                        ElseIf _IsAccumClaimSearch AndAlso _IsAccumFamilySearch Then
                            _IncludeAccumulators = True
                            Me.Invoke(CallClaimAccum)
                        ElseIf _IsAccumClaimSearch = False AndAlso _IsAccumFamilySearch Then
                            _IncludeAccumulators = True
                            Me.Invoke(CallAccum)
                        Else
                            _IncludeAccumulators = False
                        End If
                    ElseIf _IsAccumClaimSearch Then
                        _IncludeAccumulators = True
                        Me.Invoke(CallClaimAccum)
                    End If

                Case TC.SelectedTab Is PrescriptionsTab

                    PopulatePrescriptionsInfo(CInt(Me.FamilyID), Me.RelationID)

                Case TC.SelectedTab Is DentalTab

                    PopulateDentalInfo(CInt(Me.FamilyID), Me.RelationID)
                    PopulateDentalPREAuthInfo(CInt(Me.FamilyID), Me.RelationID)
                    PopulateDentalPENDInfo(CInt(Me.FamilyID), Me.RelationID)

                Case TC.SelectedTab Is HRAActivityTab

                    PopulateHRAInfo(Me.FamilyID, Me.RelationID)

                Case TC.SelectedTab Is COBTab

                    'PopulateCOBInfo(CInt(Me.FamilyID), Me.RelationID) 'always a patient level

                Case TC.SelectedTab Is CoverageTab

                    PopulateCoverageHistoryInfo(CInt(Me.FamilyID))

                Case TC.SelectedTab Is HoursTab

                    'PopulateHoursInfo(CInt(Me.FamilyID))

                Case TC.SelectedTab Is EligibilityHoursTab

                    'PopulateEligibilityHoursInfo(CInt(Me.FamilyID))

                Case TC.SelectedTab Is PremiumsHistoryTab

                    PopulatePremiumsInfo(CInt(Me.FamilyID))
                    PopulatePremiumsEnrollmentInfo(CInt(Me.FamilyID))

            End Select

            If TC.SelectedTab IsNot AccumulatorsTab Then 'only other tab construct containing the same number controls

                TC.SelectedTab.Controls.Add(MainPanel)

                If TC.SelectedTab.Controls.Count = 5 Then TC.SelectedTab.Controls("MainPanel").BringToFront()

                If _PatientsDS IsNot Nothing Then
                    _PatientsDS.Tables(0).DefaultView.RowFilter = ""

                    If _PatientsDS.Tables.Count > 1 AndAlso _PatientsDS.Tables(1).Rows.Count > 0 Then
                        _PatientsDS.Tables(1).DefaultView.RowFilter = ""
                    End If
                End If

                TC.SelectedTab.Refresh()

            End If

        Catch ex As Exception

            Throw

        Finally

            If (TC.SelectedTab Is DemographicsTab) Then
                BlinkTimer.Enabled = True
            End If

        End Try
    End Sub

    Private Sub FamilyGroupBoxViewButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FamilyGroupBoxViewButton.Click
        ToggleFamilyGroupBox()
        If Me.FamilyGroupBoxViewButton.ImageIndex = 0 Then
            Me.FamilyGroupBoxViewButton.ImageIndex = 1
        Else
            Me.FamilyGroupBoxViewButton.ImageIndex = 0
        End If
    End Sub

    Private Sub ClaimGroupBoxViewButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClaimGroupBoxViewButton.Click

        ToggleDocumentGroupBox()
        If Me.ClaimGroupBoxViewButton.ImageIndex = 0 Then
            Me.ClaimGroupBoxViewButton.ImageIndex = 1
        Else
            Me.ClaimGroupBoxViewButton.ImageIndex = 0
        End If
    End Sub

    'Private Sub DocIDTextBox_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles DocIDTextBox.Leave
    '    ValidateDocID()
    'End Sub

    Private Sub ResultsDisplayLineDetailsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResultsDisplayLineDetailsMenuItem.Click

        Dim DG As DataGridCustom
        Dim Docs As New List(Of Long?)
        Dim TSMenuItem As ToolStripMenuItem
        Dim DataGridCustomContextMenu As ContextMenuStrip

        Try

            TSMenuItem = CType(sender, ToolStripMenuItem)
            DataGridCustomContextMenu = CType(TSMenuItem.GetCurrentParent, ContextMenuStrip)
            DG = CType(DataGridCustomContextMenu.SourceControl, DataGridCustom)

            OpenLineDetails(DG)

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Sub ResultsAuditMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResultsAuditMenuItem.Click

        If _SearchResultsDT Is Nothing Then Return

        Dim Transaction As DbTransaction
        Dim BM As BindingManagerBase
        Dim DR As DataRow
        Dim SelectedRows As New ArrayList()
        Dim CM As CurrencyManager
        Dim DV As DataView

        Try

            CM = CType(Me.BindingContext(CustomerServiceResultsDataGrid.DataSource, CustomerServiceResultsDataGrid.DataMember), CurrencyManager)
            DV = CType(CM.List, DataView)

            For I As Integer = 0 To DV.Count - 1

                If CustomerServiceResultsDataGrid.IsSelected(I) Then
                    SelectedRows.Add(I)
                End If

            Next

            Transaction = CMSDALCommon.BeginTransaction

            For I As Integer = 0 To SelectedRows.Count - 1

                CustomerServiceResultsDataGrid.CurrentRowIndex = CInt(SelectedRows(I))

                BM = Me.CustomerServiceResultsDataGrid.BindingContext(Me.CustomerServiceResultsDataGrid.DataSource, Me.CustomerServiceResultsDataGrid.DataMember)
                DR = CType(BM.Current, DataRowView).Row

                If DR("CLAIM_ID") Is DBNull.Value Then Return

                CMSDALFDBMD.UpdateClaimMasterStatus(CInt(DR("CLAIM_ID")), "AUDIT", SystemInformation.UserName.ToUpper, Transaction)
                Dim HistSum As String = "CLAIM ID " & Format(DR("CLAIM_ID"), "00000000") & " HAS BEEN SELECTED FOR AUDIT"
                Dim HistDetail As String = "AUDITOR " & UFCWGeneral.WindowsUserID.Name & " MANUALLY SELECTED THIS ITEM FOR AUDIT."
                CMSDALFDBMD.CreateDocHistory(CInt(DR("CLAIM_ID")), UFCWGeneral.IsNullLongHandler(DR("DOCID")), "AUDIT", CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), CStr(DR("DOC_CLASS")), CStr(DR("DOC_TYPE")), HistSum, HistDetail, SystemInformation.UserName.ToUpper, Transaction)

                DR("STATUS") = "AUDIT"
                DR("STATUS_DATE") = UFCWGeneral.NowDate.ToString("MM/dd/yyyy")

            Next

            CMSDALCommon.CommitTransaction(Transaction)

            MessageBox.Show("Claim" & If(SelectedRows.Count > 0, "(s)", "") & " has been flagged for Audit", "Audit", MessageBoxButtons.OK)

        Catch ex As Exception
            Try

                If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing AndAlso Transaction.Connection.State <> ConnectionState.Closed Then
                    CMSDALCommon.RollbackTransaction(Transaction)
                End If

            Finally
            End Try

            Throw

        Finally
        End Try

    End Sub

    Private Sub CustomerServiceControl_ParentChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.ParentChanged
        If _Loading = False AndAlso Me.Parent IsNot Nothing Then LoadDatabaseOrFilenetSpecificInfo()
    End Sub

    Private Sub ResultsAnnotateMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResultsAnnotateMenuItem.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' Code "Stolen" from Nick Synder
        ' </remarks>
        ' <history>
        ' 	[paulw]	11/20/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        If _SearchResultsDT Is Nothing Then Return

        Dim Transaction As DbTransaction
        Dim AnnotateForm As AnnotationDialog

        Dim BM As BindingManagerBase
        Dim BMDR As DataRow

        Dim DR As DataRow
        Dim DT As DataTable

        Try

            BM = Me.CustomerServiceResultsDataGrid.BindingContext(Me.CustomerServiceResultsDataGrid.DataSource, Me.CustomerServiceResultsDataGrid.DataMember)
            BMDR = CType(BM.Current, DataRowView).Row

            If BMDR("CLAIM_ID") Is DBNull.Value Then Return

            _AnnotationsDS.ANNOTATIONS.Rows.Clear()
            _AnnotationsDS = CType(CMSDALFDBMD.RetrieveClaimAnnotations(CInt(BMDR("CLAIM_ID")), _AnnotationsDS), AnnotationsDataSet)

            AnnotateForm = New AnnotationDialog(CInt(BMDR("CLAIM_ID")), CInt(BMDR("FAMILY_ID")), CInt(BMDR("RELATION_ID")), CInt(BMDR("PART_SSN")), CInt(BMDR("PAT_SSN")), BMDR("PART_FNAME").ToString, BMDR("PART_LNAME").ToString, BMDR("PAT_FNAME").ToString, BMDR("PAT_LNAME").ToString, _AnnotationsDS.ANNOTATIONS)

            If AnnotateForm.ShowDialog(Me) = DialogResult.OK Then
                DT = AnnotateForm.AnnotationsTable.GetChanges(DataRowState.Added)

                If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                    Transaction = CMSDALCommon.BeginTransaction

                    For Each DR In DT.Rows
                        CMSDALFDBMD.CreateAnnotation(CInt(BMDR("CLAIM_ID")), CInt(BMDR("FAMILY_ID")), CShort(BMDR("RELATION_ID")),
                                              CInt(BMDR("PART_SSN")), CInt(BMDR("PAT_SSN")),
                                              BMDR("PART_FNAME").ToString, BMDR("PART_LNAME").ToString,
                                              BMDR("PAT_FNAME").ToString, BMDR("PAT_LNAME").ToString,
                                              CStr(DR("ANNOTATION")), DR("FLAG"), SystemInformation.UserName.ToUpper, Transaction)
                    Next
                    'AnnotateForm.
                    CMSDALCommon.CommitTransaction(Transaction)
                End If
            End If

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

            If AnnotateForm IsNot Nothing Then AnnotateForm.Dispose()
            AnnotateForm = Nothing

        End Try
    End Sub

    Private Sub ResultsHistoryMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResultsHistoryMenuItem.Click

        Dim BM As BindingManagerBase
        Dim DR As DataRow
        Dim Frm As ClaimsHistoryViewerForm

        Try
            If _SearchResultsDT Is Nothing Then Return

            BM = Me.CustomerServiceResultsDataGrid.BindingContext(Me.CustomerServiceResultsDataGrid.DataSource, Me.CustomerServiceResultsDataGrid.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            Frm = New ClaimsHistoryViewerForm
            Frm.Show()

            _AnnotationsDS.ANNOTATIONS.Rows.Clear()
            _AnnotationsDS = CType(CMSDALFDBMD.RetrieveClaimAnnotations(CInt(DR("CLAIM_ID")), _AnnotationsDS), AnnotationsDataSet)

            Frm.RefreshForm(CInt(DR("CLAIM_ID")), CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), DR("PART_FNAME").ToString, DR("PART_LNAME").ToString, DR("PAT_FNAME").ToString, DR("PAT_LNAME").ToString, CType(_AnnotationsDS.ANNOTATIONS, DataTable))

        Catch ex As Exception

            Throw

        Finally
        End Try
    End Sub

    Private Sub CheckForChiroCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If _Loading = False Then
            LoadDatabaseOrFilenetSpecificInfo()
            ChiroCheckBox.Visible = True
        End If
    End Sub

    Private Sub EditClearAllMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ClearAll()
    End Sub

    Private Sub SearchMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Search()
    End Sub

    Private Sub ReprintEOBMMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReprintEOBMMenuItem.Click
        Dim Transaction As DbTransaction
        Dim BM As BindingManagerBase
        Dim DR As DataRow

        If _SearchResultsDT Is Nothing Then Return

        Try
            BM = Me.CustomerServiceResultsDataGrid.BindingContext(Me.CustomerServiceResultsDataGrid.DataSource, Me.CustomerServiceResultsDataGrid.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            If DR("CLAIM_ID") Is DBNull.Value Then Return

            Transaction = CMSDALCommon.BeginTransaction

            CMSDALFDBMD.UpdateClaimMasterStatus(CInt(DR("CLAIM_ID")), "REOBM", SystemInformation.UserName.ToUpper, Transaction)
            Dim HistSum As String = "CLAIM ID " & Format(DR("CLAIM_ID"), "00000000") & " HAS BEEN SELECTED FOR A REPRINT OF MEMBER'S EOB"
            Dim HistDetail As String = "USER " & UFCWGeneral.WindowsUserID.Name.ToUpper & " SELECTED THIS ITEM FOR A REPRINT OF THE MEMBER'S EOB."
            CMSDALFDBMD.CreateDocHistory(CInt(DR("CLAIM_ID")), UFCWGeneral.IsNullLongHandler(DR("DOCID")), "REPRINTEOBM", CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), CStr(DR("DOC_CLASS")), CStr(DR("DOC_TYPE")), HistSum, HistDetail, SystemInformation.UserName.ToUpper, Transaction)

            CMSDALCommon.CommitTransaction(Transaction)

            DR("STATUS") = "REOBM"
            DR("STATUS_DATE") = UFCWGeneral.NowDate.ToString("MM/dd/yyyy")

            MessageBox.Show("Claim has been flagged to reprint member's EOB")

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
        End Try
    End Sub

    Private Sub ReprintEOBPMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReprintEOBPMenuItem.Click
        Dim Transaction As DbTransaction
        Dim BM As BindingManagerBase
        Dim DR As DataRow

        If _SearchResultsDT Is Nothing Then Return

        Try

            BM = Me.CustomerServiceResultsDataGrid.BindingContext(Me.CustomerServiceResultsDataGrid.DataSource, Me.CustomerServiceResultsDataGrid.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            If DR("CLAIM_ID") Is DBNull.Value Then Return

            Transaction = CMSDALCommon.BeginTransaction

            CMSDALFDBMD.UpdateClaimMasterStatus(CInt(DR("CLAIM_ID")), "REOBP", SystemInformation.UserName.ToUpper, Transaction)
            Dim HistSum As String = "CLAIM ID " & Format(DR("CLAIM_ID"), "00000000") & " HAS BEEN SELECTED FOR A REPRINT OF PROVIDER'S EOB"
            Dim HistDetail As String = "USER " & UFCWGeneral.WindowsUserID.Name.ToUpper & " SELECTED THIS ITEM FOR A REPRINT OF THE PROVIDER'S EOB."
            CMSDALFDBMD.CreateDocHistory(CInt(DR("CLAIM_ID")), UFCWGeneral.IsNullLongHandler(DR("DOCID")), "REPRINTEOBP", CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), CStr(DR("DOC_CLASS")), CStr(DR("DOC_TYPE")), HistSum, HistDetail, SystemInformation.UserName.ToUpper, Transaction)
            CMSDALCommon.CommitTransaction(Transaction)

            DR("STATUS") = "REOBP"
            DR("STATUS_DATE") = UFCWGeneral.NowDate.ToString("MM/dd/yyyy")
            MessageBox.Show("Claim has been flagged to reprint provider's EOB")

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
        End Try
    End Sub

    Private Sub ReprintEOBBothMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReprintEOBBothMenuItem.Click
        Dim Transaction As DbTransaction
        Dim BM As BindingManagerBase
        Dim DR As DataRow

        If _SearchResultsDT Is Nothing Then Return

        Try

            BM = Me.CustomerServiceResultsDataGrid.BindingContext(Me.CustomerServiceResultsDataGrid.DataSource, Me.CustomerServiceResultsDataGrid.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            If DR("CLAIM_ID") Is DBNull.Value Then Return

            Transaction = CMSDALCommon.BeginTransaction

            CMSDALFDBMD.UpdateClaimMasterStatus(CInt(DR("CLAIM_ID")), "REOBB", SystemInformation.UserName.ToUpper, Transaction)
            Dim HistSum As String = "CLAIM ID " & Format(DR("CLAIM_ID"), "00000000") & " HAS BEEN SELECTED FOR A REPRINT OF MEMBER'S AND PROVIDER'S EOB"
            Dim HistDetail As String = "USER " & UFCWGeneral.WindowsUserID.Name.ToUpper & " SELECTED THIS ITEM FOR A REPRINT OF THE MEMBER'S AND PROVIDER'S EOB."
            CMSDALFDBMD.CreateDocHistory(CInt(DR("CLAIM_ID")), UFCWGeneral.IsNullLongHandler(DR("DOCID")), "REPRINTEOB", CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), CStr(DR("DOC_CLASS")), CStr(DR("DOC_TYPE")), HistSum, HistDetail, SystemInformation.UserName.ToUpper, Transaction)
            CMSDALCommon.CommitTransaction(Transaction)

            DR("STATUS") = "REOBB"
            DR("STATUS_DATE") = UFCWGeneral.NowDate.ToString("MM/dd/yyyy")

            MessageBox.Show("Claim has been flagged to reprint member AND provider EOB")

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
        End Try
    End Sub

    Private Sub RemovePricingMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemovePricingMenuItem.Click
        Dim Transaction As DbTransaction
        Dim BM As BindingManagerBase
        Dim DR As DataRow

        Dim DV As DataView

        Dim CM As CurrencyManager

        Try

            CM = CType(Me.BindingContext(CustomerServiceResultsDataGrid.DataSource, CustomerServiceResultsDataGrid.DataMember), CurrencyManager)
            DV = CType(CM.List, DataView)

            BM = Me.CustomerServiceResultsDataGrid.BindingContext(Me.CustomerServiceResultsDataGrid.DataSource, Me.CustomerServiceResultsDataGrid.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            If DV.Count = 0 Then Exit Sub
            If DR("CLAIM_ID") Is DBNull.Value Then Exit Sub

            Me.SearchingLabel.Text = "Removing From Pricing ..."

            Transaction = CMSDALCommon.BeginTransaction

            If IsDBNull(DR("PENDED_TO")) = False Then
                CMSDALFDBMD.UpdateClaimMasterStatus(CInt(DR("CLAIM_ID")), "INPROGRESS", SystemInformation.UserName.ToUpper, Transaction)
            Else
                CMSDALFDBMD.UpdateClaimMasterStatus(CInt(DR("CLAIM_ID")), "NEW", SystemInformation.UserName.ToUpper, Transaction)
            End If

            Dim HistSum As String = "CLAIM ID " & Format(DR("CLAIM_ID"), "00000000") & " WAS REMOVED FROM PRICING"
            Dim HistDetail As String = "ADJUSTOR " & SystemInformation.UserName.ToUpper & " SENT THIS ITEM BACK TO THE QUEUE TO PROCESS."
            CMSDALFDBMD.CreateDocHistory(CInt(DR("CLAIM_ID")), UFCWGeneral.IsNullLongHandler(DR("DOCID")), "UPDATE", CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), CStr(DR("DOC_CLASS")), CStr(DR("DOC_TYPE")), HistSum, HistDetail, SystemInformation.UserName.ToUpper, Transaction)

            CMSDALCommon.CommitTransaction(Transaction)

            If IsDBNull(DR("PENDED_TO")) = False Then
                DR("STATUS") = "INPROGRESS"
            Else
                DR("STATUS") = "NEW"
            End If
            DR("STATUS_DATE") = UFCWGeneral.NowDate.ToString("MM/dd/yyyy")

            MessageBox.Show("Claim Was Removed From Pricing", "Remove Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

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
        End Try
    End Sub

    Private Sub ReOpenMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReOpenMenuItem.Click

        Dim Transaction As DbTransaction

        Dim BM As BindingManagerBase
        Dim DR As DataRow
        Dim DV As DataView
        Dim CM As CurrencyManager

        Try

            CM = CType(Me.BindingContext(CustomerServiceResultsDataGrid.DataSource, CustomerServiceResultsDataGrid.DataMember), CurrencyManager)
            DV = CType(CM.List, DataView)

            BM = Me.CustomerServiceResultsDataGrid.BindingContext(Me.CustomerServiceResultsDataGrid.DataSource, Me.CustomerServiceResultsDataGrid.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            If DV.Count = 0 Then Exit Sub
            If DR("CLAIM_ID") Is DBNull.Value Then Exit Sub

            If MessageBox.Show(If(IsDBNull(DR("PROCESSED_BY")) OrElse DR("PROCESSED_BY").ToString.Trim.ToUpper <> SystemInformation.UserName.ToUpper, "Are you sure you want to reopen and take ownership of this claim?" & Environment.NewLine() & "Item was processed by " & DR("PROCESSED_BY").ToString, "Are you sure you want to reopen this claim?"), "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Me.SearchingLabel.Text = "ReOpening Claim ..."

                Transaction = CMSDALCommon.BeginTransaction

                CMSDALFDBMD.ReopenClaim(CInt(DR("CLAIM_ID")), SystemInformation.UserName.ToUpper, Transaction)

                Dim HistSum As String = "CLAIM ID " & Format(DR("CLAIM_ID"), "00000000") & " WAS REOPENED"
                If Not IsDBNull(DR("PROCESSED_BY")) AndAlso DR("PROCESSED_BY").ToString.Trim.ToUpper = SystemInformation.UserName.ToUpper Then
                    Dim HistDetail As String = "ADJUSTOR " & SystemInformation.UserName.ToUpper & " REOPENED THIS ITEM FOR PROCESSING."
                    CMSDALFDBMD.CreateDocHistory(CInt(DR("CLAIM_ID")), UFCWGeneral.IsNullLongHandler(DR("DOCID")), "REOPEN", CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), CStr(DR("DOC_CLASS")), CStr(DR("DOC_TYPE")), HistSum, HistDetail, SystemInformation.UserName.ToUpper, Transaction)
                Else
                    Dim HistDetail As String = "ADJUSTOR " & SystemInformation.UserName.ToUpper & " REOPENED AND TOOK OWNERSHIP OF THIS ITEM FOR PROCESSING."
                    CMSDALFDBMD.CreateDocHistory(CInt(DR("CLAIM_ID")), UFCWGeneral.IsNullLongHandler(DR("DOCID")), "REOPEN", CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), CStr(DR("DOC_CLASS")), CStr(DR("DOC_TYPE")), HistSum, HistDetail, SystemInformation.UserName.ToUpper, Transaction)
                End If

                CMSDALCommon.CommitTransaction(Transaction)

                DR("STATUS") = "INPROGRESS"
                DR("STATUS_DATE") = UFCWGeneral.NowDate.ToString("MM/dd/yyyy")
                DR("PENDED_TO") = SystemInformation.UserName.ToUpper

                MessageBox.Show("Claim Was ReOpened and Assigned to You", "ReOpen Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

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
        End Try
    End Sub

    Private Sub IncidentDateCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IncidentDateCheckBox.CheckedChanged
        If _Loading = False Then
            Me.IncidentDateDateTimePicker.Enabled = IncidentDateCheckBox.Checked
        End If
    End Sub

    Private Sub AccidentCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AccidentCheckBox.CheckedChanged
        If _Loading = False Then
            LoadDatabaseOrFilenetSpecificInfo()
            Me.AccidentTypeComboBox.Enabled = Me.AccidentCheckBox.Checked
        End If
    End Sub

    Private Sub ProviderCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProviderCheckBox.CheckedChanged
        Me.ProviderComboxBox.Enabled = Me.ProviderCheckBox.Checked
    End Sub

    Private Sub CustomerServiceControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            _Loading = False
        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Sub HideUIElementsBasedUponSecurity()


        Me.AdjustersComboBox.Visible = False
        Me.AdjusterFilterLabel.Visible = False
        Me.CheckAmountLabel.Visible = False
        Me.CheckAmountTextBox.Visible = False
        Me.ProcedureGroupBox.Visible = False
        Me.AccidentGroupBox.Visible = False
        Me.ResultsHistoryMenuItem.Visible = False

        Me.ResultsAuditMenuItem.Available = False
        Me.DiseaseManagementMenuItem.Available = False

        If UFCWGeneralAD.CMSCanAudit Then
            Me.CheckAmountLabel.Visible = True
            Me.CheckAmountTextBox.Visible = True
            Me.ResultsAuditMenuItem.Available = True
        End If

        If UFCWGeneralAD.CMSCanReprocess Then
            AdjustersComboBox.Visible = True
            AdjusterFilterLabel.Visible = True
        End If

        If UFCWGeneralAD.CMSLocals OrElse ((UFCWGeneralAD.CMSEligibility OrElse UFCWGeneralAD.CMSDental) AndAlso Not UFCWGeneralAD.CMSUsers) Then

            SearchCriteriaGroupBox.Height = 296 + SearchButton.Height + 25
            DocumentDetailsGroupBox.Height = 151
            SearchButton.Top = 305

            PatientSearchButton.Top = SearchButton.Top
            ClearAllButton.Top = SearchButton.Top
            CancelButton.Top = ClearAllButton.Top

            If UFCWGeneralAD.CMSEligibility AndAlso Not UFCWGeneralAD.CMSUsers Then
                HideClaimSearchGroupBox()
                HideProviderSearchGroupBox()
            ElseIf UFCWGeneralAD.CMSDental AndAlso Not UFCWGeneralAD.CMSUsers Then
                HideClaimSearchGroupBox()
            Else
                SearchResultsGroupBox.Top = SearchCriteriaGroupBox.Height + 20
                SearchResultsGroupBox.Size = New Size(SearchResultsGroupBox.Width, Me.Height - Me.SearchCriteriaGroupBox.Height - 20)
            End If

        End If

        If Not UFCWGeneralAD.CMSLocals Then
            ProcedureGroupBox.Visible = True
            AccidentGroupBox.Visible = True
            ResultsHistoryMenuItem.Available = True
            DiseaseManagementMenuItem.Available = True

        End If

    End Sub

    Private Sub DateOfServiceCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateOfServiceCheckBox.CheckedChanged
        If _Loading = False Then
            LoadDatabaseOrFilenetSpecificInfo()
            DateOfServiceToDateTimePicker.Enabled = DateOfServiceCheckBox.Checked
            DateOfServiceFromDateTimePicker.Enabled = DateOfServiceCheckBox.Checked
            Me.BetweenLabel.Enabled = DateOfServiceCheckBox.Checked
            Me.AndLabel.Enabled = DateOfServiceCheckBox.Checked
        End If
    End Sub

    Private Sub ReceivedDateCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReceivedDateCheckBox.CheckedChanged
        If _Loading = False Then
            LoadDatabaseOrFilenetSpecificInfo()
            ReceivedToDateTimePicker.Enabled = ReceivedDateCheckBox.Checked
            ReceivedFromDateTimePicker.Enabled = ReceivedDateCheckBox.Checked
            Me.BetweenLabel3.Enabled = ReceivedDateCheckBox.Checked
            Me.AndLabel3.Enabled = ReceivedDateCheckBox.Checked
        End If
    End Sub

    Private Sub DocTypeCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DocTypeCheckBox.CheckedChanged
        DocTypeComboBox.Enabled = DocTypeCheckBox.Checked
        If DocTypeComboBox.DataSource Is Nothing Then
            DocTypeComboBox.DataSource = _LetterDocTypesDT
            DocTypeComboBox.DisplayMember = "DOC_TYPE"
            DocTypeComboBox.ValueMember = "DOC_TYPE"
        End If
    End Sub

    Private Sub SearchButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchButton.Click

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from occurring when buttons are disabled

            SearchButton.Enabled = False
            ClearAllButton.Enabled = False
            PatientSearchButton.Enabled = False

            ErrorProvider1.ClearError(ProcessedDateToDateTimePicker)
            ErrorProvider1.ClearError(DateOfServiceToDateTimePicker)
            ErrorProvider1.ClearError(ReceivedToDateTimePicker)
            ErrorProvider1.ClearError(ProcessedDateCheckBox)
            ErrorProvider1.ClearError(DateOfServiceCheckBox)
            ErrorProvider1.ClearError(ReceivedDateCheckBox)

            ErrorProvider1.ClearError(PatientSSNTextBox)
            ErrorProvider1.ClearError(ParticipantSSNTextBox)
            ErrorProvider1.ClearError(FamilyIDTextBox)

            If Not ValidateChildren(ValidationConstraints.Enabled) Then Return

            ResetTabs()

            If Me.ParticipantSSNTextBox.Text.Trim.Length < 1 AndAlso
                Me.ProviderIDTextBox.Text.Trim.Length < 1 AndAlso
                Me.DocIDTextBox.Text.Trim.Length < 1 AndAlso
                Me.BatchNumberTextBox.Text.Trim.Length < 1 AndAlso
                Me.PatientLastNameTextBox.Text.Trim.Length < 1 AndAlso
                Me.PatientSSNTextBox.Text.Trim.Length < 1 AndAlso
                Me.ClaimIDTextBox.Text.Trim.Length < 1 AndAlso
                Me.FamilyIDTextBox.Text.Trim.Length < 1 Then

                If Me.ProcessedDateCheckBox.Checked Then
                    If UFCWGeneralAD.CMSCanAudit Then
                        If (TimeOfDay >= ExtendedSearchMorningStart OrElse TimeOfDay < ExtendedSearchMorningEnd) OrElse (TimeOfDay >= ExtendedSearchAfternoonStart OrElse TimeOfDay < ExtendedSearchAfternoonEnd) OrElse (TimeOfDay >= ExtendedSearchEveningStart OrElse TimeOfDay < ExtendedSearchEveningEnd) Then
                        Else
                            If Me.ProcessedDateFrom > Me.ProcessedDateTo.AddMonths(-_DOSDuration) OrElse (Me.AdjusterComboBox.Text <> "(all)" OrElse Me.StatusComboBox.Text <> "(all)") Then
                            Else
                                ErrorProvider1.SetErrorWithTracking(Me.ProcessedDateToDateTimePicker, "The search requested is estimated to take to long. Please limit any Processed Date Only query to " & _DOSDuration & " months or less")
                                MessageBox.Show("The search requested is estimated to take to long. " & Environment.NewLine() & " Please limit any Processed Date Only query to " & _DOSDuration & " months or less.", "Revise Search", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                                Exit Sub
                            End If
                        End If
                    Else
                        If Me.ProcessedDateFrom > Me.ProcessedDateTo.AddDays(-31) Then
                        Else
                            ErrorProvider1.SetErrorWithTracking(Me.ProcessedDateToDateTimePicker, "The search you are attempting will take too long. Please limit any Processed Date Only query to 1 month or less")
                            MessageBox.Show("The search requested is estimated to take to long. " & Environment.NewLine() & " Please limit any Processed Date Only query to 1 month or less.", "Revise Search", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                            Exit Sub
                        End If
                    End If

                ElseIf Me.ReceivedDateCheckBox.Checked Then
                    If UFCWGeneralAD.CMSCanAudit Then
                        If (TimeOfDay >= ExtendedSearchMorningStart OrElse TimeOfDay < ExtendedSearchMorningEnd) OrElse (TimeOfDay >= ExtendedSearchAfternoonStart OrElse TimeOfDay < ExtendedSearchAfternoonEnd) OrElse (TimeOfDay >= ExtendedSearchEveningStart OrElse TimeOfDay < ExtendedSearchEveningEnd) Then
                        Else
                            If Me.ReceivedDateFrom > Me.ReceivedDateTo.AddMonths(-_DOSDuration) OrElse (Me.AdjusterComboBox.Text <> "(all)" OrElse Me.StatusComboBox.Text <> "(all)") Then
                            Else
                                ErrorProvider1.SetErrorWithTracking(Me.ReceivedToDateTimePicker, "The search you are attempting will take too long. Please limit any Received Date Only query to " & _DOSDuration & " months or less")
                                MessageBox.Show("The search requested is estimated to take to long. " & Environment.NewLine() & " Please limit any Received Date Only query to " & _DOSDuration & " months or less.", "Revise Search", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                                Exit Sub
                            End If
                        End If
                    Else
                        If Me.ReceivedDateFrom > Me.ReceivedDateTo.AddDays(-31) Then
                        Else
                            ErrorProvider1.SetErrorWithTracking(Me.ReceivedToDateTimePicker, "The search you are attempting will take too long. Please limit any Received Date Only query to 1 month or less")
                            MessageBox.Show("The search requested is estimated to take to long. " & Environment.NewLine() & " Please limit any Received Date Only query to 1 month or less.", "Revise Search", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                            Exit Sub
                        End If
                    End If

                ElseIf Me.DateOfServiceCheckBox.Checked Then
                    If UFCWGeneralAD.CMSCanAudit Then
                        If (TimeOfDay >= ExtendedSearchMorningStart OrElse TimeOfDay < ExtendedSearchMorningEnd) OrElse (TimeOfDay >= ExtendedSearchAfternoonStart OrElse TimeOfDay < ExtendedSearchAfternoonEnd) OrElse (TimeOfDay >= ExtendedSearchEveningStart OrElse TimeOfDay < ExtendedSearchEveningEnd) Then
                        Else
                            If Me.DateOfServiceFrom > Me.DateOfServiceTo.AddMonths(-_DOSDuration) OrElse (Me.AdjusterComboBox.Text <> "(all)" OrElse Me.StatusComboBox.Text <> "(all)") Then
                            Else
                                ErrorProvider1.SetErrorWithTracking(Me.ReceivedToDateTimePicker, "The search you are attempting will take too long. Please limit any Date Of Service Only query to " & _DOSDuration & " months or less")
                                MessageBox.Show("The search requested is estimated to take to long. " & Environment.NewLine() & " Please limit any Date Of Service Only query to " & _DOSDuration & " months or less.", "Revise Search", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                                Exit Sub
                            End If
                        End If
                    Else
                        If Me.DateOfServiceFrom > Me.DateOfServiceTo.AddDays(-31) Then
                        Else
                            ErrorProvider1.SetErrorWithTracking(Me.ReceivedToDateTimePicker, "The search you are attempting will take too long. Please limit any Date of Service Only query to 1 month or less")
                            MessageBox.Show("The search requested is estimated to take to long. " & Environment.NewLine() & " Please limit any Date Of Service Only query to 1 month or less.", "Revise Search", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                            Exit Sub
                        End If
                    End If

                ElseIf Me.DocTypeCheckBox.Checked And UFCWGeneralAD.CMSLocals Then
                    ErrorProvider1.SetErrorWithTracking(PatientSSNTextBox, "Search by either Participant SSN, Patient SSN, Claim ID or Family ID / Relation ID (Default 0)")
                    ErrorProvider1.SetErrorWithTracking(ParticipantSSNTextBox, "Search by either Participant SSN, Patient SSN, Claim ID or Family ID / Relation ID (Default 0)")
                    ErrorProvider1.SetErrorWithTracking(FamilyIDTextBox, "Search by either Participant SSN, Patient SSN, Claim ID or Family ID / Relation ID (Default 0)")
                    MessageBox.Show("The search you are attempting is not permitted. " & Environment.NewLine() & " Please include indentifying information such as SSN, ClaimID, FamilyID.", "Revise Search", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
                    Return
                Else
                    ErrorProvider1.SetErrorWithTracking(PatientSSNTextBox, "Specify to search by Patient SSN")
                    ErrorProvider1.SetErrorWithTracking(ParticipantSSNTextBox, "Specify to search by Participant SSN")
                    ErrorProvider1.SetErrorWithTracking(ProviderIDTextBox, "Specify to search by Provider TIN")
                    ErrorProvider1.SetErrorWithTracking(ClaimIDTextBox, "Specify to search by Claim ID")
                    ErrorProvider1.SetErrorWithTracking(BatchNumberTextBox, "Specify to search by Batch")
                    ErrorProvider1.SetErrorWithTracking(DocIDTextBox, "Specify to search by Document ID")
                    ErrorProvider1.SetErrorWithTracking(FamilyIDTextBox, "Specify to search by Family ID / Relation ID (Default 0)")
                    ErrorProvider1.SetErrorWithTracking(DateOfServiceCheckBox, "Specify to search by Date of Service.")
                    ErrorProvider1.SetErrorWithTracking(ProcessedDateCheckBox, "Specify to search by Processed Date.")

                    MessageBox.Show("The search requested is estimated to take to long. " & Environment.NewLine() & " Please additional selection criteria to limit your results.", "Revise Search", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

                    Return
                End If
            ElseIf Me.ProviderIDTextBox.Text.Trim.Length > 0 AndAlso (Not (Me.ProcessedDateCheckBox.Checked OrElse Me.ReceivedDateCheckBox.Checked OrElse Me.DateOfServiceCheckBox.Checked) AndAlso (
                                                                                                                                                                                                    Me.ParticipantSSNTextBox.Text.Trim.Length < 1 AndAlso
                                                                                                                                                                                                    Me.DocIDTextBox.Text.Trim.Length < 1 AndAlso
                                                                                                                                                                                                    Me.BatchNumberTextBox.Text.Trim.Length < 1 AndAlso
                                                                                                                                                                                                    Me.PatientLastNameTextBox.Text.Trim.Length < 1 AndAlso
                                                                                                                                                                                                    Me.PatientSSNTextBox.Text.Trim.Length < 1 AndAlso
                                                                                                                                                                                                    Me.ClaimIDTextBox.Text.Trim.Length < 1 AndAlso
                                                                                                                                                                                                    Me.FamilyIDTextBox.Text.Trim.Length < 1
                                                                                                                                                                                                    )) Then
                ErrorProvider1.SetErrorWithTracking(Me.ProviderIDTextBox, "The search requested is estimated to take to long. Please limit your results by specifying a date range.")

                MessageBox.Show("The search requested is unpredictable and estimated to take to long. " & Environment.NewLine() & " Limit your search by specifying additional criteria such as a date range.", "Revise Search", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

                Return

            ElseIf UFCWGeneral.UnFormatSSN(Me.ParticipantSSNTextBox.Text) >= "999999997" OrElse UFCWGeneral.UnFormatSSN(Me.PatientSSNTextBox.Text) >= "999999997" OrElse UFCWGeneral.UnFormatSSN(Me.ParticipantSSNTextBox.Text) = "000000000" OrElse UFCWGeneral.UnFormatSSN(Me.PatientSSNTextBox.Text) = "000000000" Then

                If Me.ReceivedDateCheckBox.Checked AndAlso Me.ReceivedDateFrom > Me.ReceivedDateTo.AddMonths(-1).AddDays(-1) Then
                Else
                    ErrorProvider1.SetErrorWithTracking(Me.ReceivedDateCheckBox, "The search requested is estimated to take to long. Received Date period of 1 month or less is required")
                    If ReceivedDateCheckBox.Checked Then
                        ErrorProvider1.SetErrorWithTracking(Me.ReceivedToDateTimePicker, "The search requested is estimated to take to long Received Date period of 1 month or less is required")
                    Else
                        ErrorProvider1.SetErrorWithTracking(Me.ReceivedDateCheckBox, "The search requested is estimated to take to long. Received Date range of less than 1 month is required")

                    End If

                    MessageBox.Show("The search requested Is unpredictable And estimated to take to long. " & Environment.NewLine() & " Limit any 999-99-9997/8 searches by including a Received Date of 1 month Or less.", "Revise Search", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

                    Return

                End If

            End If

            Me.SearchingLabel.Text = "Searching ..."

            Search()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH: mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

            Throw

        Finally

            SearchButton.Enabled = True
            ClearAllButton.Enabled = True
            PatientSearchButton.Enabled = True

            Me.AutoValidate = HoldAutoValidate
        End Try

    End Sub

    Private Sub ResultsDataGrid_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CustomerServiceResultsDataGrid.MouseDown
        Dim DG As DataGridCustom

        Try

            DG = CType(sender, DataGridCustom)

            _HTI = DG.HitTest(e.X, e.Y)

            'Select Case _HTI.Type
            '    Case System.Windows.Forms.DataGrid.HitTestType.RowHeader, DataGrid.HitTestType.Cell
            '        If ((Control.ModifierKeys And Keys.Shift) = Keys.Shift) OrElse
            '            ((Control.ModifierKeys And Keys.ShiftKey) = Keys.ShiftKey) OrElse
            '            ((Control.ModifierKeys And Keys.Control) = Keys.Control) OrElse
            '            ((Control.ModifierKeys And Keys.ControlKey) = Keys.ControlKey) Then
            '        ElseIf e.Button = MouseButtons.Right AndAlso Not DG.IsSelected(_HTI.Row) Then
            '            DG.ResetSelection()
            '        ElseIf e.Button = MouseButtons.Right Then
            '        ElseIf e.Button = MouseButtons.Left Then
            '        Else
            '            DG.ResetSelection()
            '        End If

            '        DG.Select(_HTI.Row)
            'End Select

        Catch ex As Exception

            Throw

        End Try

    End Sub

    Private Sub ResultsDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustomerServiceResultsDataGrid.DoubleClick

        Select Case CType(sender, DataGridCustom).LastHitSpot.Type
            Case Is = System.Windows.Forms.DataGrid.HitTestType.None

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader

                OpenLineDetails(CType(sender, DataGridCustom))

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

        End Select

    End Sub

#End Region

#Region "Misc Methods"
    Public Sub InitializeControl()

        Try

            LoadDatabaseOrFilenetSpecificInfo()

            'Me.ToolTip1.SetToolTip(Me.ToolTipTriggerLabel, db.GetConnection.ConnectionString)

            Me.DateOfServiceToDateTimePicker.Value = UFCWGeneral.NowDate
            Me.DateOfServiceFromDateTimePicker.Value = UFCWGeneral.NowDate
            Me.ProcessedDateFromDateTimePicker.Value = UFCWGeneral.NowDate
            Me.ProcessedDateToDateTimePicker.Value = UFCWGeneral.NowDate
            Me.ReceivedFromDateTimePicker.Value = UFCWGeneral.NowDate
            Me.ReceivedToDateTimePicker.Value = UFCWGeneral.NowDate

            HideUIElementsBasedUponSecurity()

            Me.TabControl.TabPages.Remove(EligibilityTab)
            Me.TabControl.TabPages.Remove(HoursTab)
            Me.TabControl.TabPages.Remove(EligibilityHoursTab)
            Me.TabControl.TabPages.Remove(COBTab)
            Me.TabControl.TabPages.Remove(PremiumsHistoryTab)
            Me.TabControl.TabPages.Remove(DemographicsTab)
            Me.TabControl.TabPages.Remove(AccumulatorsTab)
            Me.TabControl.TabPages.Remove(ImagingTab)
            Me.TabControl.TabPages.Remove(FreeTextTab)
            Me.TabControl.TabPages.Remove(CoverageTab)
            Me.TabControl.TabPages.Remove(HRAActivityTab)
            Me.TabControl.TabPages.Remove(ProviderTab)
            Me.TabControl.TabPages.Remove(PrescriptionsTab)
            Me.TabControl.TabPages.Remove(DentalTab)
            Me.MatchesLabel.Visible = False
            Me.IncidentDateDateTimePicker.Enabled = False
            Me.AccidentTypeComboBox.Enabled = False
            Me.ProviderComboxBox.Enabled = False
            Me.DateOfServiceToDateTimePicker.Enabled = False
            Me.DateOfServiceFromDateTimePicker.Enabled = False
            Me.ProcessedDateFromDateTimePicker.Enabled = False
            Me.ProcessedDateToDateTimePicker.Enabled = False
            Me.ReceivedFromDateTimePicker.Enabled = False
            Me.ReceivedToDateTimePicker.Enabled = False
            Me.BetweenLabel.Enabled = False
            Me.AndLabel.Enabled = False
            Me.BetweenLabel2.Enabled = False
            Me.AndLabel2.Enabled = False
            Me.BetweenLabel3.Enabled = False
            Me.AndLabel3.Enabled = False
            Me.DocTypeComboBox.Enabled = False

            Me.ParticipantSSNTextBox.Focus()
            Me.ParticipantSSNTextBox.AutoSize = True
            Me.ParticipantSSNTextBox.Text = ""

        Catch ex As Exception

            Throw

        End Try
    End Sub

    Public Sub SetFocus()
        Me.ParticipantSSNTextBox.Focus()
        Me.ParticipantSSNTextBox.AutoSize = True
        Me.ParticipantSSNTextBox.Text = ""
        Me.ParticipantSSNTextBox.TabIndex = 1
    End Sub

    Private Sub OpenLineDetails(dg As DataGridCustom)

        If _SearchResultsDT Is Nothing Then Return


        Dim BM As BindingManagerBase
        Dim DR As DataRow
        Dim DRs As ArrayList
        Dim ItemMaxCount As Integer

        Try

            Using WC As New GlobalCursor

                BM = Me.CustomerServiceResultsDataGrid.BindingContext(Me.CustomerServiceResultsDataGrid.DataSource, Me.CustomerServiceResultsDataGrid.DataMember)
                DR = CType(BM.Current, DataRowView).Row

                DRs = dg.GetSelectedDataRows()

                For Each DR In DRs

                    If ItemMaxCount > 2 Then Exit For

                    Dim Frm As LineDetailsForm

                    If Not IsDBNull(DR("CLAIM_ID")) Then
                        Frm = New LineDetailsForm
                        Frm.AccumulatorValues.ShowLineDetails = True
                        Frm.SetClaimID(CInt(DR("CLAIM_ID")))
                        Frm.Show()

                        If IsDBNull(DR("OCC_FROM_DATE")) Then
                            Frm.DisplayLineItemsByPatient(DR)
                        Else
                            Frm.DisplayLineItemsByClaim(DR)
                        End If
                    End If

                    ItemMaxCount += 1

                Next

            End Using

        Catch ex As Exception

            Throw
        Finally
        End Try

    End Sub

    Private Function SelectedRowsAreForSamePerson() As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Determines of the selected row are all from the same person
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	11/30/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DV As DataView
        Dim TheFamilyID As Integer
        Dim TheRelationID As Integer

        Try

            DV = CType(CustomerServiceResultsDataGrid.DataSource, DataView)
            TheFamilyID = CInt(DV(CustomerServiceResultsDataGrid.CurrentRowIndex)("FAMILY_ID"))
            TheRelationID = CInt(DV(CustomerServiceResultsDataGrid.CurrentRowIndex)("RELATION_ID"))

            For I As Integer = 0 To DV.Table.Rows.Count - 2
                If CustomerServiceResultsDataGrid.IsSelected(I) Then
                    If CInt(DV.Table.Rows(I)("FAMILY_ID")) <> TheFamilyID OrElse CInt(DV.Table.Rows(I)("RELATION_ID")) <> TheRelationID Then
                        Return False
                    End If
                End If
            Next
            Return True

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Sub LoadDatabaseOrFilenetSpecificInfo()

        Try

            If Not _Loaded Then

                Using WC As New GlobalCursor

                    Me.SearchResultsGroupBox.Top = Me.SearchCriteriaGroupBox.Top + Me.SearchCriteriaGroupBox.Height
                    SearchResultsGroupBox.Height = Me.Height - Me.SearchCriteriaGroupBox.Height - 20
                    SearchButton.Top = Me.SearchCriteriaGroupBox.Top + Me.SearchCriteriaGroupBox.Height - 45

                    Me.PatientSearchButton.Top = Me.SearchButton.Top
                    Me.ClearAllButton.Top = Me.SearchButton.Top
                    Me.CancelButton.Top = Me.ClearAllButton.Top

                    BindDocTypeControl()
                    BindAdjusterControl()

                    Me.ResultsDisplayDocumentMenuItem.ShortcutKeys = (System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D)
                    Me.ResultsDisplayDocumentMenuItem.ShowShortcutKeys = True
                    Me.ResultsDisplayEligibiltyMenuItem.ShortcutKeys = (System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.I)
                    Me.ResultsDisplayEligibiltyMenuItem.ShowShortcutKeys = True
                    Me.ResultsDisplayLineDetailsMenuItem.ShortcutKeys = (System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L)
                    Me.ResultsDisplayLineDetailsMenuItem.ShowShortcutKeys = True
                    Me.ResultsHistoryMenuItem.ShortcutKeys = (System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.H)
                    Me.ResultsHistoryMenuItem.ShowShortcutKeys = True
                    Me.FamilyRelationMenuItem.Enabled = False
                    Me.Hide()

                    Dim ClaimStatusWorkerThread As New Thread(New ThreadStart(AddressOf GetStatusValues)) With {
                        .IsBackground = True,
                        .Priority = ThreadPriority.Normal
                    }
                    ClaimStatusWorkerThread.Start()

                    Dim BillTypeCodesWorkerThread As New Thread(New ThreadStart(AddressOf GetBillTypeCodes))
                    BillTypeCodesWorkerThread.IsBackground = True
                    BillTypeCodesWorkerThread.Priority = ThreadPriority.Normal
                    BillTypeCodesWorkerThread.Start()

                    Dim LettersWorkerThread As New Thread(New ThreadStart(AddressOf GetLetters))
                    LettersWorkerThread.IsBackground = True
                    LettersWorkerThread.Priority = ThreadPriority.Normal
                    LettersWorkerThread.Start()

                    Dim ReasonCodesWorkerThread As New Thread(New ThreadStart(AddressOf GetReasonCodes))
                    ReasonCodesWorkerThread.IsBackground = True
                    ReasonCodesWorkerThread.Priority = ThreadPriority.Normal
                    ReasonCodesWorkerThread.Start()

                    Dim ModifierCodesWorkerThread As New Thread(New ThreadStart(AddressOf GetModifierCodes))
                    ModifierCodesWorkerThread.IsBackground = True
                    ModifierCodesWorkerThread.Priority = ThreadPriority.Normal
                    ModifierCodesWorkerThread.Start()

                    Dim PlaceOfServiceCodesWorkerThread As New Thread(New ThreadStart(AddressOf GetPlaceOfServiceCodes))
                    PlaceOfServiceCodesWorkerThread.IsBackground = True
                    PlaceOfServiceCodesWorkerThread.Priority = ThreadPriority.Normal
                    PlaceOfServiceCodesWorkerThread.Start()

                    Dim RegAlertReasonValuesWorkerThread As New Thread(New ThreadStart(AddressOf GetRegAlertReasonValues))
                    RegAlertReasonValuesWorkerThread.IsBackground = True
                    RegAlertReasonValuesWorkerThread.Priority = ThreadPriority.Normal
                    RegAlertReasonValuesWorkerThread.Start()

                    Dim ProcedureCodesWorkerThread As New Thread(New ThreadStart(AddressOf GetProcedureCodes))
                    ProcedureCodesWorkerThread.IsBackground = True
                    ProcedureCodesWorkerThread.Priority = ThreadPriority.Normal
                    ProcedureCodesWorkerThread.Start()

                    Dim DiagnosisCodesWorkerThread As New Thread(New ThreadStart(AddressOf GetDiagnosisCodes))
                    DiagnosisCodesWorkerThread.IsBackground = True
                    DiagnosisCodesWorkerThread.Priority = ThreadPriority.Normal
                    DiagnosisCodesWorkerThread.Start()

                    _Loaded = True

                    Me.Show()

                End Using
            End If

        Catch ex As Exception
        Finally

        End Try
    End Sub

    Private Sub GetProcedureCodes()
        _ProcedureDT = CMSDALFDBMD.RetrieveProcedureValuesAsOfEffectiveDate(UFCWGeneral.NowDate).Tables(0)
    End Sub

    Private Sub GetDiagnosisCodes()
        _DiagnosisDT = CMSDALFDBMD.RetrieveDiagnosisValuesEffectiveAsOf(UFCWGeneral.NowDate).Tables(0)
    End Sub

    Private Sub GetModifierCodes()
        _ModifierDT = CMSDALFDBMD.RetrieveModifierValues(UFCWGeneral.NowDate).Tables(0)
    End Sub

    Private Sub GetRegAlertReasonValues()
        _RegAlertReasonDT = CMSDALFDBEL.RetrieveRegAlertReasonValues()
    End Sub

    Private Sub GetPlaceOfServiceCodes()
        _PlaceOfServiceDT = CMSDALFDBMD.RetrievePlaceOfServiceValues(UFCWGeneral.NowDate).Tables(0)
    End Sub

    Private Sub GetBillTypeCodes()
        _BillTypeDT = CMSDALFDBMD.RetrieveBillTypeValues(UFCWGeneral.NowDate).Tables(0)
    End Sub

    Private Sub GetReasonCodes()
        _ReasonDT = CMSDALFDBMD.RetrieveReasonValues(UFCWGeneral.NowDate).Tables(0)
    End Sub

    Private Sub BindAdjusterControl()
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/18/2006	Created
        '     [malkoi] 7/17/2007 Modified, refactored
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DT As DataTable

        Try


            Using WC As New GlobalCursor
                DT = CMSDALFDBMD.RetrieveDistinctUsers()
                AdjusterComboBox.DataSource = DT
                AdjusterComboBox.DisplayMember = "USERID"
                AdjusterComboBox.ValueMember = "USERID"
            End Using

        Catch ex As Exception
            Throw
        End Try


    End Sub

    Private Shared Sub BindDocTypeControl()
        _DocTypesDT = CMSDALFDBMD.RetrieveActiveDocTypes()
    End Sub

    Private Sub BindAccidentControl()

        Dim DT As DataTable

        Try
            Using WC As New GlobalCursor
                DT = CMSDALFDBMD.RetrieveAccidents()
                AccidentTypeComboBox.DataSource = DT
                AccidentTypeComboBox.DisplayMember = "ACCIDENT_VALUE"
                AccidentTypeComboBox.ValueMember = "ACCIDENT_VALUE"
            End Using

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub BindStatusControl()
        Dim DV As DataView

        Try

            DV = New DataView(_StatusDT, "", "STATUS ASC", DataViewRowState.CurrentRows)
            StatusComboBox.DataSource = DV
            StatusComboBox.DisplayMember = "STATUS"
            StatusComboBox.ValueMember = "STATUS"

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ToggleGroupBoxes()
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/18/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        ToggleDocumentGroupBox()
        ToggleFamilyGroupBox()
    End Sub

    Private Sub ToggleDocumentGroupBox()
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/18/2006	Created
        '     [malkoi] 7/17/2207 Modified, includes functionality for ACR MED-580
        ' </history>
        ' -----------------------------------------------------------------------------

        If UFCWGeneralAD.CMSLocals Then
            If Me.DocumentDetailsGroupBox.Height = 22 Then
                Me.SearchCriteriaGroupBox.Height += 146
                Me.DocumentDetailsGroupBox.Height = 151
            Else
                Me.SearchCriteriaGroupBox.Height -= 146
                Me.DocumentDetailsGroupBox.Height = 22
            End If

        Else

            If Me.DocumentDetailsGroupBox.Height = 22 Then
                Me.SearchCriteriaGroupBox.Height += 250
                Me.DocumentDetailsGroupBox.Height = 265
            Else
                Me.SearchCriteriaGroupBox.Height -= 250
                Me.DocumentDetailsGroupBox.Height = 22
            End If

        End If

        Me.SearchResultsGroupBox.Top = Me.SearchCriteriaGroupBox.Top + Me.SearchCriteriaGroupBox.Height
        SearchResultsGroupBox.Height = Me.Height - Me.SearchCriteriaGroupBox.Height - 20

        Me.PatientSearchButton.Top = Me.SearchButton.Top
        Me.ClearAllButton.Top = Me.SearchButton.Top
        Me.CancelButton.Top = Me.ClearAllButton.Top

    End Sub

    Private Sub HideClaimSearchGroupBox()

        ClaimGroupBoxViewButton.Visible = False

        If Me.DocumentDetailsGroupBox.Height = 20 Then
            Me.DocumentDetailsGroupBox.Height = 151
            Me.SearchCriteriaGroupBox.Height += 151
        Else
            Me.DocumentDetailsGroupBox.Height = 20
            Me.SearchCriteriaGroupBox.Height -= 151
        End If

        Me.SearchResultsGroupBox.Top = Me.SearchCriteriaGroupBox.Top + Me.SearchCriteriaGroupBox.Height
        SearchResultsGroupBox.Height = Me.Height - Me.SearchCriteriaGroupBox.Height - 20
        SearchButton.Top = Me.SearchCriteriaGroupBox.Top + Me.SearchCriteriaGroupBox.Height - 45

        Me.PatientSearchButton.Top = Me.SearchButton.Top
        Me.ClearAllButton.Top = Me.SearchButton.Top
        Me.CancelButton.Top = Me.ClearAllButton.Top

        DocumentDetailsGroupBox.Visible = False
        BatchNumberLabel.Visible = False
        BatchNumberTextBox.Visible = False

    End Sub

    Private Sub HideProviderSearchGroupBox()

        ProviderIDLabel.Visible = False
        ProviderIDTextBox.Visible = False
        ProviderIDLookupButton.Visible = False

    End Sub

    Private Sub ToggleFamilyGroupBox()
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/18/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        If Me.FamilyInformationGroupBox.Height = 22 Then
            Me.FamilyInformationGroupBox.Height = 75
            Me.DocumentDetailsGroupBox.Top += 50
            Me.SearchCriteriaGroupBox.Height += 50
        Else
            Me.FamilyInformationGroupBox.Height = 22
            Me.DocumentDetailsGroupBox.Top -= 50
            Me.SearchCriteriaGroupBox.Height -= 50
        End If

        Me.SearchResultsGroupBox.Top = Me.SearchCriteriaGroupBox.Top + Me.SearchCriteriaGroupBox.Height + 10
        SearchResultsGroupBox.Height = Me.Height - Me.SearchCriteriaGroupBox.Height - 22
        SearchButton.Top = Me.SearchCriteriaGroupBox.Top + Me.SearchCriteriaGroupBox.Height - 45

        Me.PatientSearchButton.Top = Me.SearchButton.Top
        Me.ClearAllButton.Top = Me.SearchButton.Top
        Me.CancelButton.Top = Me.ClearAllButton.Top

    End Sub

    Private Function GetSearchCriteriaInXmlFormat() As XmlDocument
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        '
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/18/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim XMLDoc As New XmlDocument
        Dim RootElem As XmlElement
        Dim CriterionElem As XmlElement
        Dim SSN As String

        Try

            XMLDoc.AppendChild(XMLDoc.CreateXmlDeclaration("1.0", "UTF-8", "no"))

            RootElem = XMLDoc.CreateElement("Criteria")
            XMLDoc.AppendChild(RootElem)

            SSN = String.Empty
            If Me.PatientSSNTextBox.Text.Length > 0 Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "PatientSSN")
                SSN = CStr(UFCWGeneral.DecryptSSN(UFCWGeneral.UnFormatSSN(PatientSSNTextBox.Text.Trim)))
                CriterionElem.SetAttribute("Value", SSN)
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.ProviderIDTextBox.Text.Length > 9 Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "NPI")
                CriterionElem.SetAttribute("Value", Me.ProviderIDTextBox.Text.Trim)
                RootElem.AppendChild(CriterionElem)
                'criterionElem = xmlDoc.CreateElement("Criterion")
                'criterionElem.SetAttribute("Name", "BillingNPI")
                'criterionElem.SetAttribute("Value", Me.ProviderIDTextBox.Text.ToString.Trim)
                'rootElem.AppendChild(criterionElem)
                'criterionElem = xmlDoc.CreateElement("Criterion")
                'criterionElem.SetAttribute("Name", "RenderingNPI")
                'criterionElem.SetAttribute("Value", Me.ProviderIDTextBox.Text.ToString.Trim)
                'rootElem.AppendChild(criterionElem)
            ElseIf Me.ProviderIDTextBox.Text.Length > 7 Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "ProviderTIN")
                CriterionElem.SetAttribute("Value", Me.ProviderIDTextBox.Text.Trim)
                RootElem.AppendChild(CriterionElem)
            ElseIf Me.ProviderIDTextBox.Text.Length > 0 Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "ProviderID")
                CriterionElem.SetAttribute("Value", Me.ProviderIDTextBox.Text.Trim)
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.DocIDTextBox.Text.Length > 0 Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "DocID")
                CriterionElem.SetAttribute("Value", Me.DocIDTextBox.Text.Trim)
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.BatchNumberTextBox.Text.Length > 0 Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "BatchNumber")
                CriterionElem.SetAttribute("Value", "'" & Me.BatchNumberTextBox.Text.Trim & "'")
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.ParticipantSSNTextBox.Text.Length > 0 Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "ParticipantSSN")
                SSN = CStr(UFCWGeneral.DecryptSSN(ParticipantSSNTextBox.Text.Trim))
                CriterionElem.SetAttribute("Value", SSN)
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.FamilyIDTextBox.Text.Length > 0 Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "FamilyId")
                CriterionElem.SetAttribute("Value", Me.FamilyIDTextBox.Text.ToString.Trim)
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.RelationIDTextBox.Text.Length > 0 Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "RelationId")
                CriterionElem.SetAttribute("Value", Me.RelationIDTextBox.Text.Trim)
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.ClaimIDTextBox.Text.Length > 0 AndAlso IsInteger(Me.ClaimIDTextBox.Text.Trim) Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "ClaimId")
                CriterionElem.SetAttribute("Value", Me.ClaimIDTextBox.Text.Trim)
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.ClaimIDTextBox.Text.Length > 0 AndAlso Not IsInteger(Me.ClaimIDTextBox.Text.Trim) AndAlso IsInteger(Me.ClaimIDTextBox.Text.Substring(0, 1)) Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "ReferenceId")
                CriterionElem.SetAttribute("Value", "'" & Me.ClaimIDTextBox.Text.Trim & "'")
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.ClaimIDTextBox.Text.Length > 0 AndAlso Not IsInteger(Me.ClaimIDTextBox.Text.Trim) AndAlso Not IsInteger(Me.ClaimIDTextBox.Text.Substring(0, 1)) Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "MaxID")
                CriterionElem.SetAttribute("Value", "'" & Me.ClaimIDTextBox.Text.Trim & "'")
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.DateOfServiceCheckBox.Checked Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "DateOfService")
                CriterionElem.SetAttribute("Value", "'" & Format(Me.DateOfServiceFromDateTimePicker.Value, "yyyy-MM-dd") & "' AND '" & Format(DateOfServiceToDateTimePicker.Value, "yyyy-MM-dd") & "'")
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.ProcessedDateCheckBox.Checked Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "ProcessedDate")
                CriterionElem.SetAttribute("Value", "'" & Format(Me.ProcessedDateFromDateTimePicker.Value, "yyyy-MM-dd") & "-00.00.00' AND '" & Format(Me.ProcessedDateToDateTimePicker.Value, "yyyy-MM-dd") & "-23.59.59' ")
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.ReceivedDateCheckBox.Checked Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "ReceivedDate")
                CriterionElem.SetAttribute("Value", "'" & Format(Me.ReceivedFromDateTimePicker.Value, "yyyy-MM-dd") & "' AND '" & Format(Me.ReceivedToDateTimePicker.Value, "yyyy-MM-dd") & "' ")
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.DocTypeCheckBox.Checked Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "DocType")
                CriterionElem.SetAttribute("Value", "'" & Me.DocTypeComboBox.Text & "'")
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.DeniedCheckBox.Checked Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "STATUS")
                CriterionElem.SetAttribute("Value", "'DENY'")
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.ProviderNameTextBox.Text.Length > 0 Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "ProviderName")
                CriterionElem.SetAttribute("Value", "'" & Replace(Me.ProviderNameTextBox.Text.TrimStart, "'", "''") & "%'")
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.PatientLastNameTextBox.Text.Length > 0 Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "PatientLastName")
                CriterionElem.SetAttribute("Value", "'" & Me.PatientLastNameTextBox.Text.TrimStart & "'")
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.PatientFirstNameTextBox.Text.Length > 0 Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "PatientFirstName")
                CriterionElem.SetAttribute("Value", "'" & Me.PatientFirstNameTextBox.Text.TrimStart & "'")
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.CheckNumberTextBox.Text.Length > 0 Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "CheckNumber")
                CriterionElem.SetAttribute("Value", Me.CheckNumberTextBox.Text.Trim)
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.CheckAmountTextBox.Text.Length > 0 And IsNumeric(CheckAmountTextBox.Text) Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "CheckAmount")
                CriterionElem.SetAttribute("Value", Me.CheckAmountTextBox.Text.Trim)
                RootElem.AppendChild(CriterionElem)
            End If

            If ChiroCheckBox.Checked Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "Chiro")
                CriterionElem.SetAttribute("Value", "True")
                RootElem.AppendChild(CriterionElem)
            End If

            'If Me.ProcedureCodeTextBox.Text.Length > 0 AndAlso Me.ProcedureCodeToTextBox.Text.Length > 0 Then
            '    CriterionElem = XMLDoc.CreateElement("Criterion")
            '    CriterionElem.SetAttribute("Name", "ProcedureCode")
            '    CriterionElem.SetAttribute("Value", "'" & Me.ProcedureCodeTextBox.Text & "' AND '" & Trim(Me.ProcedureCodeToTextBox.Text) & "'")
            '    RootElem.AppendChild(CriterionElem)
            'ElseIf Me.ProcedureCodeTextBox.Text.Length > 0 Then
            '    CriterionElem = XMLDoc.CreateElement("Criterion")
            '    CriterionElem.SetAttribute("Name", "ProcedureCode")
            '    CriterionElem.SetAttribute("Value", "'" & Me.ProcedureCodeTextBox.Text & "' AND '" & Trim(Me.ProcedureCodeTextBox.Text) & "'")
            '    RootElem.AppendChild(CriterionElem)
            '    'ElseIf Me.ProcedureCodeToTextBox.Text.Length > 0 Then
            '    '    CriterionElem = XMLDoc.CreateElement("Criterion")
            '    '    CriterionElem.SetAttribute("Name", "ProcedureCode")
            '    '    CriterionElem.SetAttribute("Value", "'" & Me.ProcedureCodeToTextBox.Text & "' AND '" & Trim(Me.ProcedureCodeToTextBox.Text) & "'")
            '    '    RootElem.AppendChild(CriterionElem)
            'End If

            If Me.ModifierTextBox.Text.Length > 0 Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "Modifier")
                CriterionElem.SetAttribute("Value", "'" & Me.ModifierTextBox.Text.Trim & "'")
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.DiagnosisCodesTextBox.Text.Length > 0 Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "Diagnosis")
                CriterionElem.SetAttribute("Value", "'" & Me.DiagnosisCodesTextBox.Text.Trim & "'")
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.ProcedureCodeTextBox.Text.Length > 0 Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "ProcedureCode")
                CriterionElem.SetAttribute("Value", "'" & Me.ProcedureCodeTextBox.Text.Trim & "'")
                RootElem.AppendChild(CriterionElem)
            End If

            'If Me.DiagnosisTextBox.Text.Length > 0 AndAlso Me.DiagnosisToTextBox.Text.Length > 0 Then
            '    CriterionElem = XMLDoc.CreateElement("Criterion")
            '    CriterionElem.SetAttribute("Name", "Diagnosis")
            '    CriterionElem.SetAttribute("Value", "'" & Me.DiagnosisTextBox.Text & "' AND '" & Trim(Me.DiagnosisToTextBox.Text) & "'")
            '    RootElem.AppendChild(CriterionElem)
            'ElseIf Me.DiagnosisTextBox.Text.Length > 0 Then
            '    CriterionElem = XMLDoc.CreateElement("Criterion")
            '    CriterionElem.SetAttribute("Name", "Diagnosis")
            '    CriterionElem.SetAttribute("Value", "'" & Me.DiagnosisTextBox.Text & "' AND '" & Trim(Me.DiagnosisTextBox.Text) & "'")
            '    RootElem.AppendChild(CriterionElem)
            '    'ElseIf Me.DiagnosisToTextBox.Text.Length > 0 Then
            '    '    CriterionElem = XMLDoc.CreateElement("Criterion")
            '    '    CriterionElem.SetAttribute("Name", "Diagnosis")
            '    '    CriterionElem.SetAttribute("Value", "'" & Me.DiagnosisToTextBox.Text & "' AND '" & Trim(Me.DiagnosisToTextBox.Text) & "'")
            '    '    RootElem.AppendChild(CriterionElem)
            'End If

            If Me.BillTypeTextBox.Text.Length > 0 Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "BillType")
                CriterionElem.SetAttribute("Value", "'" & Me.BillTypeTextBox.Text.Trim & "'")
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.PlaceOfServiceTextBox.Text.Length > 0 Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "PlaceOfService")
                CriterionElem.SetAttribute("Value", "'" & Me.PlaceOfServiceTextBox.Text.Trim & "'")
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.ProviderCheckBox.Checked Then
                If ProviderComboxBox.Text.ToUpper.IndexOf("P") >= 0 Then
                    CriterionElem = XMLDoc.CreateElement("Criterion")
                    CriterionElem.SetAttribute("Name", "OutOfArea")
                    CriterionElem.SetAttribute("Value", "False")
                    RootElem.AppendChild(CriterionElem)
                    CriterionElem = XMLDoc.CreateElement("Criterion")
                    CriterionElem.SetAttribute("Name", "NonParticipating")
                    CriterionElem.SetAttribute("Value", "False")
                    RootElem.AppendChild(CriterionElem)
                ElseIf ProviderComboxBox.Text.ToUpper.IndexOf("N") >= 0 Then
                    CriterionElem = XMLDoc.CreateElement("Criterion")
                    CriterionElem.SetAttribute("Name", "NonParticipating")
                    CriterionElem.SetAttribute("Value", "True")
                    RootElem.AppendChild(CriterionElem)
                ElseIf ProviderComboxBox.Text.ToUpper.IndexOf("O") >= 0 Then
                    CriterionElem = XMLDoc.CreateElement("Criterion")
                    CriterionElem.SetAttribute("Name", "OutOfArea")
                    CriterionElem.SetAttribute("Value", "True")
                    RootElem.AppendChild(CriterionElem)
                End If
            End If

            If Me.IncidentDateCheckBox.Checked Then
                CriterionElem = XMLDoc.CreateElement("Criterion")
                CriterionElem.SetAttribute("Name", "IncidentDate")
                CriterionElem.SetAttribute("Value", "'" & Format(Me.IncidentDateDateTimePicker.Value, "yyyy-MM-dd") & "'")
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.AccidentCheckBox.Checked And AccidentTypeComboBox.Text.Trim.Length > 0 Then
                'these values are not dynamic because of the nature
                ' of the table design.  there is no related table
                ' and each type of accident has a switch in the table
                ' so we must set them explicitly since there is no
                ' way to set them dynamically.
                CriterionElem = XMLDoc.CreateElement("Criterion")
                If AccidentTypeComboBox.Text.ToUpper.IndexOf("AUTO") >= 0 Then
                    CriterionElem.SetAttribute("Name", "AutoAccident")
                ElseIf AccidentTypeComboBox.Text.ToUpper.IndexOf("WORKERS") >= 0 Then
                    CriterionElem.SetAttribute("Name", "WorkersComp")
                ElseIf AccidentTypeComboBox.Text.ToUpper.IndexOf("OTHER") >= 0 Then
                    CriterionElem.SetAttribute("Name", "OtherAccident")
                End If
                CriterionElem.SetAttribute("Value", "True")
                RootElem.AppendChild(CriterionElem)
            End If

            If Me.ProcessedDateCheckBox.Checked Then

                If Me.ReasonsTextBox.Text.Length > 0 Then
                    CriterionElem = XMLDoc.CreateElement("Criterion")
                    CriterionElem.SetAttribute("Name", "Reason")
                    CriterionElem.SetAttribute("Value", "'" & Me.ReasonsTextBox.Text.Trim & "'")
                    RootElem.AppendChild(CriterionElem)
                End If

                If Me.StatusComboBox.Text <> "(all)" Then
                    CriterionElem = XMLDoc.CreateElement("Criterion")
                    CriterionElem.SetAttribute("Name", "ClaimStat")
                    CriterionElem.SetAttribute("Value", "'" & Me.StatusComboBox.Text.Trim & "'")
                    RootElem.AppendChild(CriterionElem)
                End If
                If Me.AdjusterComboBox.Text <> "(all)" Then
                    CriterionElem = XMLDoc.CreateElement("Criterion")
                    CriterionElem.SetAttribute("Name", "Adjuster")
                    CriterionElem.SetAttribute("Value", "'" & Me.AdjusterComboBox.Text.Trim & "'")
                    RootElem.AppendChild(CriterionElem)
                End If
            End If

            Return XMLDoc

        Catch ex As Exception

            Throw

        End Try
    End Function

    Private Sub PopulateStatusTypes()
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/18/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DT1 As DataTable
        Dim DT2 As DataTable

        Try
            DT1 = _SearchResultsDT
            DT2 = CMSDALCommon.SelectDistinctAndSorted("", DT1, "STATUS")
            StatusTypesComboBox.DataSource = DT2
            StatusTypesComboBox.DisplayMember = "STATUS"

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PopulateDocTypes()
        Dim DT1 As DataTable
        Dim DT2 As DataTable

        Try

            DT1 = _SearchResultsDT
            DT2 = CMSDALCommon.SelectDistinctAndSorted("", DT1, "DOC_TYPE")
            DocTypesComboBox.DataSource = DT2
            DocTypesComboBox.DisplayMember = "DOC_TYPE"
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PopulateAdjusters()
        Dim DT1 As DataTable
        Dim DT2 As DataTable

        Try

            DT1 = _SearchResultsDT
            DT2 = CMSDALCommon.SelectDistinctAndSorted("", DT1, "PROCESSED_BY")
            AdjustersComboBox.DataSource = DT2
            AdjustersComboBox.DisplayMember = "PROCESSED_BY"

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ApplySecurityToTabs(ByVal resultsTab As TabPage)

        If (UFCWGeneralAD.CMSEligibility OrElse UFCWGeneralAD.CMSDental) AndAlso Not UFCWGeneralAD.CMSUsers Then
            If resultsTab IsNot Nothing Then TabControl.TabPages.Remove(resultsTab)
            If TabControl.TabPages.Contains(AccumulatorsTab) Then TabControl.TabPages.Remove(AccumulatorsTab)
        End If

        If Not (UFCWGeneralAD.CMSUsers OrElse UFCWGeneralAD.CMSLocals OrElse UFCWGeneralAD.CMSHRA) Then
            If TabControl.TabPages.Contains(HRAActivityTab) Then TabControl.TabPages.Remove(HRAActivityTab)
        End If

        If UFCWGeneralAD.CMSLocals Then
            If TabControl.TabPages.Contains(PrescriptionsTab) Then TabControl.TabPages.Remove(PrescriptionsTab)
        End If

        If Not (UFCWGeneralAD.CMSUsers OrElse UFCWGeneralAD.CMSDental OrElse UFCWGeneralAD.CMSLocals) Then
            If TabControl.TabPages.Contains(DentalTab) Then TabControl.TabPages.Remove(DentalTab)
        End If

    End Sub

    Private Sub PrePopulateAssociatedControls()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Populates the tabs
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/18/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim SSNO As Integer?
        Dim DR As DataRow

        Try

            If Not _SearchUIMode = -1 Then 'Note the provider control can also be populated indirectly as the result of a search

                Using WC As New GlobalCursor

                    If _IsBatchSearch Then
                        CollectDocsControlBatchInfo(BatchNumberTextBox.Text)
                    End If

                    ''For Imaging Tab
                    If _IsDocumentSearch Then

                        UFCWDocsControl.LoadUFCWDOCSFromDocID(CLng(DocIDTextBox.Text))
                        DR = UFCWDocsControl.GetParticipant()

                        If DR IsNot Nothing Then
                            If CBool(DR("TRUST_SW")) AndAlso UFCWGeneralAD.CMSEligibilityEmployee = False Then
                                _IncludeImaging = False 'excludes ImagingTab
                                MessageBox.Show("You are not authorized to view Trust Employee Documents.", "Access Restricted", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                            End If
                        End If
                    ElseIf Not _IsMemberSearch AndAlso _IsProviderSearch AndAlso Me.ProviderIDTextBox.Text.ToString.Trim.Length > 0 Then
                        If Me.ReceivedDateCheckBox.Checked Then
                            UFCWDocsControl.LoadUFCWDOCSFromProviderTIN(CInt(Me.ProviderIDTextBox.Text), CType(Me.ReceivedDateFrom.Date, Date?), CType(Me.ReceivedDateTo.Date, Date?), _EmployeeAccess)
                        Else
                            UFCWDocsControl.LoadUFCWDOCSFromProviderTIN(CInt(Me.ProviderIDTextBox.Text), _EmployeeAccess)
                        End If
                    Else

                        If _PartDR IsNot Nothing Then
                            SSNO = CInt(_PartDR("PART_SSNO"))
                        ElseIf PatientSSNTextBox.Text.Trim.Length > 0 Then
                            SSNO = CInt(UFCWGeneral.UnFormatSSN(PatientSSNTextBox.Text))
                        ElseIf ParticipantSSNTextBox.Text.Trim.Length > 0 Then
                            SSNO = CInt(UFCWGeneral.UnFormatSSN(ParticipantSSNTextBox.Text))
                        End If

                        If SSNO IsNot Nothing Then
                            If Me.ReceivedDateCheckBox.Checked Then
                                If _IsProviderSearch Then
                                    UFCWDocsControl.LoadUFCWDOCSBySSNAndTaxID(CInt(SSNO), CInt(Me.ProviderIDTextBox.Text), Me.ReceivedDateFrom.Date, _EmployeeAccess)
                                Else
                                    UFCWDocsControl.LoadUFCWDOCSFromSSN(CInt(SSNO), Me.ReceivedDateFrom.Date, _EmployeeAccess)
                                End If
                            Else
                                If _IsProviderSearch Then
                                    UFCWDocsControl.LoadUFCWDOCSBySSNAndTaxID(CInt(SSNO), CInt(Me.ProviderIDTextBox.Text), _EmployeeAccess)
                                Else
                                    UFCWDocsControl.LoadUFCWDOCSFromSSN(CInt(SSNO), _EmployeeAccess)
                                End If
                            End If
                        Else
                            If Me.ReceivedDateCheckBox.Checked Then
                                UFCWDocsControl.LoadUFCWDOCSByReceivedDate(Me.ReceivedDateFrom.Date, _EmployeeAccess)
                            End If
                        End If

                    End If

                    If (_IsClaimSearch OrElse _IsMemberSearch) AndAlso FamilyID IsNot Nothing AndAlso CDbl(FamilyID) > -1 Then

                        'Relation -1 instructs controls to return family level info

                        If _IncludeHRAActivity Then
                            CollectHRAInfo(CInt(Me.FamilyID), Me.RelationID) 'use 3 threads to parallelize db2 access
                        End If

                        CollectCOBInfo(CInt(Me.FamilyID), Me.RelationID)

                        PopulateFreeTextInfo(_PatientsDS, CInt(Me.FamilyID), Me.RelationID)

                        PopulateEligibilityInfo(True, CInt(Me.FamilyID), Me.RelationID) 'threads initiated in here

                        '' For Prescriptions
                        If _IncludePrescriptions Then
                            If Me.ReceivedDateCheckBox.Checked Then
                                CollectPrescriptionsInfo(CInt(Me.FamilyID), Me.RelationID, Me.ReceivedDateFrom.Date, Me.ReceivedDateTo.Date)
                            Else
                                CollectPrescriptionsInfo(CInt(Me.FamilyID), Me.RelationID)
                            End If
                        End If

                        '' For Dental
                        If _IncludeDental Then
                            If Me.DateOfServiceCheckBox.Checked Then
                                CollectDentalInfo(CInt(Me.FamilyID), Me.RelationID, Me.ReceivedDateFrom.Date, Me.ReceivedDateTo.Date)
                            Else
                                CollectDentalInfo(CInt(Me.FamilyID), Me.RelationID)
                            End If
                            CollectDentalPREAuthInfo(CInt(Me.FamilyID), Me.RelationID)
                            CollectDentalPENDInfo(CInt(Me.FamilyID), Me.RelationID)
                        End If

                    End If

                    ''To Docs tab search by eligibility BatchNumber
                    If _IsBatchSearch Then
                        PopulateDocsControlBatchInfo(BatchNumberTextBox.Text, _EmployeeAccess)
                    End If

                    If UFCWDocsControl.DocumentsFound < 1 Then _IncludeImaging = False 'excludes ImagingTab

                    'initiates wait for data.
                    TaskTimer.Enabled = True

                End Using

            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub PopulateDentalInfo(ByVal familyID As Integer, Optional ByVal relationID As Short? = Nothing)

        If _PopulateDentalTask IsNot Nothing Then
            _PopulateDentalTask.Wait()

            _DentalDS = New DataSet
            _DentalDS = _PopulateDentalTask.Result

            If Me.DateOfServiceCheckBox.Checked Then
                DentalControl.LoadDentalControl(Me.DateOfServiceFrom, Me.DateOfServiceTo, familyID, relationID, _DentalDS)
            Else
                DentalControl.LoadDentalControl(familyID, relationID, _DentalDS)
            End If

            _PopulateDentalTask = Nothing

        End If

    End Sub

    Private Sub CollectDentalInfo(familyID As Integer, relationID As Short?, Optional dosFromDate As Date? = Nothing, Optional dosThruDate As Date? = Nothing)

        _DentalDS = New DataSet
        _DentalDS.Tables.Clear()

        If dosFromDate IsNot Nothing Then
            _PopulateDentalTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBDN.GetDentalInformation(CType(dosFromDate, Date), CType(dosThruDate, Date), familyID, relationID, _DentalDS))
        Else
            _PopulateDentalTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBDN.GetDentalInformation(familyID, relationID, _DentalDS))
        End If

    End Sub

    Private Sub PopulateDentalPREAuthInfo(ByVal familyID As Integer, Optional ByVal relationID As Short? = Nothing)

        If _PopulateDentalPREAuthTask IsNot Nothing Then
            _PopulateDentalPREAuthTask.Wait()

            _DentalPREAuthDS = New DataSet
            _DentalPREAuthDS = _PopulateDentalPREAuthTask.Result

            DentalControl.LoadPREAuthDentalControl(familyID, relationID, _DentalPREAuthDS)

            _PopulateDentalPREAuthTask = Nothing

        End If

    End Sub
    Private Sub CollectDentalPREAuthInfo(familyID As Integer, Optional ByVal relationID As Short? = Nothing)

        _DentalPREAuthDS = New DataSet
        _DentalPREAuthDS.Tables.Clear()

        _PopulateDentalPREAuthTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBDN.GetDentalPendOrPreAuthInformation(familyID, relationID, "PREAUTH", _DentalPREAuthDS))

    End Sub

    Private Sub PopulateDentalPENDInfo(ByVal familyID As Integer, Optional ByVal relationID As Short? = Nothing)

        If _PopulateDentalPendTask IsNot Nothing Then
            _PopulateDentalPendTask.Wait()

            _DentalPendDS = New DataSet
            _DentalPendDS = _PopulateDentalPendTask.Result

            DentalControl.LoadPENDDentalControl(familyID, relationID, _DentalPendDS)

            _PopulateDentalPendTask = Nothing

        End If

    End Sub
    Private Sub CollectDentalPENDInfo(familyID As Integer, Optional ByVal relationID As Short? = Nothing)

        _DentalPendDS = New DataSet
        _DentalPendDS.Tables.Clear()

        _PopulateDentalPendTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBDN.GetDentalPendOrPreAuthInformation(familyID, relationID, "PEND", _DentalPendDS))

    End Sub
    Private Sub PopulatePrescriptionsInfo(ByVal familyID As Integer, Optional ByVal relationID As Short? = Nothing)

        If _IncludePrescriptions Then
            If _PopulatePrescriptionsTask IsNot Nothing Then
                _PopulatePrescriptionsTask.Wait()

                _PrescriptionsDS = _PopulatePrescriptionsTask.Result

                If Me.ReceivedDateCheckBox.Checked Then
                    PrescriptionsControl.LoadPrescriptionsControl(Me.ReceivedDateFrom.Date, Me.ReceivedDateTo.Date, familyID, relationID, _PrescriptionsDS)
                Else
                    PrescriptionsControl.LoadPrescriptionsControl(familyID, relationID, _PrescriptionsDS)
                End If

                _PopulatePrescriptionsTask = Nothing

            End If
        End If

    End Sub
    Private Sub CollectPrescriptionsInfo(familyID As Integer, relationID As Short?, Optional receivedDateFrom As Date? = Nothing, Optional receivedDateTo As Date? = Nothing)

        _PrescriptionsDS = New PrescriptionsDataSet
        _PrescriptionsDS.Tables.Clear()

        If receivedDateFrom IsNot Nothing Then
            _PopulatePrescriptionsTask = Task(Of PrescriptionsDataSet).Factory.StartNew(Function() CType(CMSDALFDBMD.GetPrescriptionsInformation(CType(receivedDateFrom, Date), CType(receivedDateTo, Date), familyID, relationID, _PrescriptionsDS), PrescriptionsDataSet))
        Else
            _PopulatePrescriptionsTask = Task(Of PrescriptionsDataSet).Factory.StartNew(Function() CType(CMSDALFDBMD.GetPrescriptionsInformation(familyID, relationID, _PrescriptionsDS), PrescriptionsDataSet))
        End If

    End Sub

    Private Sub PopulateHRAInfo(familyID As Integer?, relationID As Short?)

        Try
            If familyID Is Nothing Then Return

            If _IncludeHRAActivity AndAlso _PopulateHRATask IsNot Nothing Then

                _PopulateHRATask.Wait()

                HraActivityControl.LoadHRAActivityControl(CInt(familyID), relationID, _HRADS)

                _PopulateHRATask = Nothing

            End If

            If _IncludeHRABalance AndAlso _PopulateHRABalanceTask IsNot Nothing Then

                Task.WaitAll(_PopulateHRABalanceTask, _PopulateHRQTask)

                HraBalanceControl.HRABalance(CInt(familyID), 0, _HRABalanceDS)
                HrqControl.LoadHRQControl(CInt(familyID), relationID, _HRQDS)

                _PopulateHRABalanceTask = Nothing

            End If

        Catch ex As Exception

            Throw

        End Try

    End Sub

    Private Sub PopulateDocsControlBatchInfo(ByVal batchNumber As String, ByVal trustSW As Boolean)

        _PopulateBatchTask.Wait()

        UFCWDocsControl.LoadUFCWDOCSFromBATCH(batchNumber, trustSW, _DocsinBatchDS)

    End Sub

    Private Sub CollectHRAInfo(familyID As Integer, relationID As Short?)

        If _IncludeHRAActivity Then

            _HRADS = New DataSet
            _HRADS.Tables.Clear()

            _PopulateHRATask = Task(Of DataSet).Factory.StartNew(Function() CType(CMSDALFDBHRA.RetrieveHRAAllHistory(familyID, relationID, _HRADS), DataSet))

        End If

        If _IncludeHRABalance Then 'Andalso Not _IncludeHRAActivity Then

            _HRABalanceDS = New DataSet
            _HRQDS = New DataSet

            _PopulateHRABalanceTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBHRA.GetHRABalanceByFamilyIDRelationID(familyID, relationID, _HRABalanceDS))
            _PopulateHRQTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBHRA.GetHRQInformation(familyID, relationID, _HRQDS))

        End If

    End Sub

    Private Sub CollectDocsControlBatchInfo(ByVal batchNumber As String)

        _DocsinBatchDS = New DataSet

        _PopulateBatchTask = Task(Of DataSet).Factory.StartNew(Function() UFCWDocsDAL.GetUFCWDocsFromBATCH(batchNumber, _DocsinBatchDS))

    End Sub


    Private Sub DisplayAppropriateTabs()

        Dim TB As TabPage
        Dim AddIt As Boolean

        Try
            TabControl.TabPages.Remove(EligibilityTab)
            TabControl.TabPages.Remove(HoursTab)
            TabControl.TabPages.Remove(EligibilityHoursTab)
            TabControl.TabPages.Remove(COBTab)
            TabControl.TabPages.Remove(DemographicsTab)
            TabControl.TabPages.Remove(ImagingTab)
            TabControl.TabPages.Remove(FreeTextTab)
            TabControl.TabPages.Remove(CoverageTab)
            TabControl.TabPages.Remove(AccumulatorsTab)
            TabControl.TabPages.Remove(HRAActivityTab)
            TabControl.TabPages.Remove(ProviderTab)
            TabControl.TabPages.Remove(PremiumsHistoryTab)
            TabControl.TabPages.Remove(PrescriptionsTab)
            TabControl.TabPages.Remove(DentalTab)

            If Not TabControl.TabPages.Contains(GenTab) Then
                TB = GenTab
                TB.Controls.Add(MainPanel)
                TB.Text = ""

                For I As Integer = 0 To TabControl.TabPages.Count - 1
                    TabControl.TabPages(I).Controls.Clear()
                Next

                TabControl.TabPages.Clear()
                TabControl.TabPages.Add(TB)

            End If

            If _SearchResultsDT.Rows.Count > 0 Then

                Dim DT As DataTable = CMSDALCommon.SelectDistinctAndSorted("", _SearchResultsDT, "DOC_CLASS")

                If DT.Rows.Count > 0 Then
                    For I As Integer = 0 To DT.Rows.Count - 1
                        TB = New TabPage(CStr(DT.Rows(I).Item("DOC_CLASS")))
                        TB = LoadTempControls(TB)
                        AddIt = True
                        For Each t As TabPage In TabControl.TabPages
                            If t.Text = TB.Text Then
                                AddIt = False
                                TabControl.TabPages.Remove(t)
                            End If
                        Next

                        TB.Name = CStr(DT.Rows(I).Item("DOC_CLASS"))

                        If AddIt Then TabControl.TabPages.Add(TB)
                    Next
                End If
            End If

            If _SearchUIMode = 0 Then 'only display tabs when data is required to be viewed

                If _IsMemberSearch OrElse _IsClaimSearch Then
                    TabControl.TabPages.Add(EligibilityTab)
                    TabControl.TabPages.Add(CoverageTab)

                    If UFCWGeneralAD.CMSCanViewEligibilityHours Then
                        TabControl.TabPages.Add(EligibilityHoursTab)
                    End If

                    If UFCWGeneralAD.CMSCanViewHours Then
                        TabControl.TabPages.Add(HoursTab)
                    End If

                    TabControl.TabPages.Add(PremiumsHistoryTab)
                    TabControl.TabPages.Add(FreeTextTab)
                    TabControl.TabPages.Add(COBTab)
                Else
                    If TabControl.TabPages.Contains(EligibilityTab) Then TabControl.TabPages.Remove(EligibilityTab)
                    If TabControl.TabPages.Contains(HoursTab) Then TabControl.TabPages.Remove(HoursTab)
                    If TabControl.TabPages.Contains(EligibilityHoursTab) Then TabControl.TabPages.Remove(EligibilityHoursTab)
                    If TabControl.TabPages.Contains(CoverageTab) Then TabControl.TabPages.Remove(CoverageTab)
                    If TabControl.TabPages.Contains(COBTab) Then TabControl.TabPages.Remove(COBTab)
                    If TabControl.TabPages.Contains(PremiumsHistoryTab) Then TabControl.TabPages.Remove(PremiumsHistoryTab)
                End If

                If _IncludeDemographics Then
                    TabControl.TabPages.Add(DemographicsTab)
                Else
                    If TabControl.TabPages.Contains(DemographicsTab) Then TabControl.TabPages.Remove(DemographicsTab)
                End If

                If _IncludeHRAActivity AndAlso _IsHRATabs.ToUpper = "ON" Then
                    TabControl.TabPages.Add(HRAActivityTab)
                Else
                    If TabControl.TabPages.Contains(HRAActivityTab) Then TabControl.TabPages.Remove(HRAActivityTab)
                End If

                If _IncludeAccumulators Then
                    TabControl.TabPages.Add(AccumulatorsTab)
                Else
                    If TabControl.TabPages.Contains(AccumulatorsTab) Then TabControl.TabPages.Remove(AccumulatorsTab)
                End If

                ''For Prescriptions
                If _IncludePrescriptions AndAlso UFCWGeneralAD.CMSUsers Then
                    TabControl.TabPages.Add(PrescriptionsTab)
                End If

                ''For Dental
                If _IncludeDental Then
                    TabControl.TabPages.Add(DentalTab)
                End If

                ''For ImagingTab
                If _IncludeImaging Then
                    TabControl.TabPages.Add(ImagingTab)
                End If

                If _IncludeProvider Then
                    TabControl.TabPages.Add(ProviderTab)
                End If

            End If

            ApplySecurityToTabs(TabControl.SelectedTab)

            If TabControl.TabPages.Contains(GenTab) Then

                TabControl.TabPages.Remove(GenTab)

                TabControl.SelectedIndex = -1

                If TabControl.TabPages.Count > 0 Then

                    If (Not _DisableSearchUI AndAlso (FreeTextEditor.PartAlert OrElse FreeTextEditor.PatAlert)) AndAlso TabControl.TabPages.Contains(FreeTextTab) Then
                        TabControl.SelectedTab = FreeTextTab
                    Else

                        If TabControl.TabPages.Contains(DemographicsTab) Then
                            TabControl.SelectedTab = DemographicsTab
                        Else
                            TabControl.SelectedIndex = 0
                        End If

                    End If

                End If


            End If

        Catch ex As Exception

            Throw

        End Try

    End Sub

    Private Sub ClearAll()
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	11/27/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        TaskTimer.Enabled = False

        TabControl.Visible = False

        ClearAllDataSources()
        ClearAllUIElements()

    End Sub

    Private Sub ClearAllDataSources()

        _SearchResultsDT = Nothing

        _PrescriptionsDS = Nothing
        _DentalDS = Nothing
        _HRADS = Nothing
        _HRABalanceDS = Nothing
        _HRQDS = Nothing
        _HoursDS = Nothing
        _EligibilityHoursDS = Nothing
        _COBDS = Nothing
        _DocsinBatchDS = Nothing
        _CoverageHistoryDS = Nothing
        _PremiumsDS = Nothing
        _PremLetterDS = Nothing

        _PopulateHRATask = Nothing
        _PopulateHRQTask = Nothing
        _PopulateHRABalanceTask = Nothing
        _PopulateBatchTask = Nothing
        _PopulateCoverageHistoryTask = Nothing
        _PopulateHoursTask = Nothing
        _PopulateEligibilityHoursTask = Nothing
        _PopulatePrescriptionsTask = Nothing
        _PopulateDentalTask = Nothing
        _PopulateCOBTask = Nothing
        _PopulatePremiumsEnrollmentTask = Nothing
        _PopulatePremiumsTask = Nothing

        EligibilityControl.ClearAll()
        EligibilityDualCoverageControl.ClearAll()

        CustomerServiceResultsDataGrid.DataSource = Nothing

        AccumulatorValues.Dispose()
        AccumulatorValues = New AccumulatorValues

        AccumulatorValues.FamilyAccumulatorsDataGrid.Visible = False
        AccumulatorValues.FamilyAccumulatorsCurrentLabel.Visible = False
        AccumulatorValues.FamilyAccumulatorsPriorDataGrid.Visible = False
        AccumulatorValues.FamilyAccumulatorsPriorLabel.Visible = False
        AccumulatorValues.PersonalAccumulatorCurrentLabel.Visible = False
        AccumulatorValues.PersonalAccumulatorDataGrid.Visible = False
        AccumulatorValues.PersonalAccumulatorPriorDataGrid.Visible = False
        AccumulatorValues.PersonalAccumulatorPriorLabel.Visible = False

        AccumulatorDualCoverageValues.Dispose()
        AccumulatorDualCoverageValues = New AccumulatorValues

        AccumulatorDualCoverageValues.FamilyAccumulatorsDataGrid.Visible = False
        AccumulatorDualCoverageValues.FamilyAccumulatorsCurrentLabel.Visible = False
        AccumulatorDualCoverageValues.FamilyAccumulatorsPriorDataGrid.Visible = False
        AccumulatorDualCoverageValues.FamilyAccumulatorsPriorLabel.Visible = False
        AccumulatorDualCoverageValues.PersonalAccumulatorCurrentLabel.Visible = False
        AccumulatorDualCoverageValues.PersonalAccumulatorDataGrid.Visible = False
        AccumulatorDualCoverageValues.PersonalAccumulatorPriorDataGrid.Visible = False
        AccumulatorDualCoverageValues.PersonalAccumulatorPriorLabel.Visible = False

        ParticipantDemographicsGrid.DataSource = Nothing
        DualParticipantDemographicsGrid.DataSource = Nothing

        HraActivityControl.ClearAll()
        HraBalanceControl.ClearAll()
        HrqControl.ClearAll()

        PremiumsControl.ClearAll()
        PremiumsEnrollmentControl.ClearAll()

        HoursControl.ClearAll()
        HoursDualCoverageControl.ClearAll()

        ProviderControl.ClearProvider()
        UFCWDocsControl.Clearall()

        FreeTextEditor.ClearFreeText()

        CobControl.ClearAll()
        PrescriptionsControl.ClearAll()
        DentalControl.ClearAll()
    End Sub

    Private Sub ResetTabs()


        TabControl.Visible = False
        TabControl.SuspendLayout()

        PatientSearchButton.Visible = False

        AccumulatorsTab.Text = " ACCUMULATORS "
        HoursTab.Text = " HOURS "
        EligibilityHoursTab.Text = " Elg. HOURS "
        COBTab.Text = " COB "
        ImagingTab.Text = " IMAGING "
        EligibilityTab.Text = " FAMILY ELIGIBILITY "
        CoverageTab.Text = " COVERAGE "
        DemographicsTab.Text = " FAMILY INFO "

        RemoveHandler TabControl.SelectedIndexChanged, AddressOf TabControl_SelectedIndexChanged

        TabControl.SelectedIndex = -1

        If TabControl.TabPages.Contains(EligibilityTab) Then TabControl.TabPages.Remove(EligibilityTab)
        If TabControl.TabPages.Contains(CoverageTab) Then TabControl.TabPages.Remove(CoverageTab)
        If TabControl.TabPages.Contains(HoursTab) Then TabControl.TabPages.Remove(HoursTab)
        If TabControl.TabPages.Contains(EligibilityHoursTab) Then TabControl.TabPages.Remove(EligibilityHoursTab)
        If TabControl.TabPages.Contains(COBTab) Then TabControl.TabPages.Remove(COBTab)
        If TabControl.TabPages.Contains(PremiumsHistoryTab) Then TabControl.TabPages.Remove(PremiumsHistoryTab)
        If TabControl.TabPages.Contains(DemographicsTab) Then TabControl.TabPages.Remove(DemographicsTab)
        If TabControl.TabPages.Contains(ImagingTab) Then TabControl.TabPages.Remove(ImagingTab)
        If TabControl.TabPages.Contains(FreeTextTab) Then TabControl.TabPages.Remove(FreeTextTab)
        If TabControl.TabPages.Contains(AccumulatorsTab) Then TabControl.TabPages.Remove(AccumulatorsTab)
        If TabControl.TabPages.Contains(ProviderTab) Then TabControl.TabPages.Remove(ProviderTab)
        If TabControl.TabPages.Contains(HRAActivityTab) Then TabControl.TabPages.Remove(HRAActivityTab)
        If TabControl.TabPages.Contains(PrescriptionsTab) Then TabControl.TabPages.Remove(PrescriptionsTab)
        If TabControl.TabPages.Contains(DentalTab) Then TabControl.TabPages.Remove(DentalTab)

        AddHandler TabControl.SelectedIndexChanged, AddressOf TabControl_SelectedIndexChanged

        TabControl.ResumeLayout()

    End Sub

    Private Sub ClearAllUIElements()

        If Me.PatientSSNTextBox.Enabled Then Me.PatientSSNTextBox.Text = ""
        If Me.ProviderIDTextBox.Enabled Then Me.ProviderIDTextBox.Text = ""
        If Me.BatchNumberTextBox.Enabled Then Me.BatchNumberTextBox.Text = ""
        If Me.ParticipantSSNTextBox.Enabled Then Me.ParticipantSSNTextBox.Text = ""
        If Me.FamilyIDTextBox.Enabled Then Me.FamilyIDTextBox.Text = ""
        If Me.RelationIDTextBox.Enabled Then Me.RelationIDTextBox.Text = ""
        If Me.ClaimIDTextBox.Enabled Then Me.ClaimIDTextBox.Text = ""
        If Me.DocIDTextBox.Enabled Then Me.DocIDTextBox.Text = ""
        If Me.DateOfServiceCheckBox.Enabled Then Me.DateOfServiceCheckBox.Checked = False
        If Me.ProcessedDateCheckBox.Enabled Then Me.ProcessedDateCheckBox.Checked = False
        If Me.ReceivedDateCheckBox.Enabled Then Me.ReceivedDateCheckBox.Checked = False
        If Me.DocTypeCheckBox.Enabled Then Me.DocTypeCheckBox.Checked = False
        If Me.ProviderNameTextBox.Enabled Then Me.ProviderNameTextBox.Text = ""
        If Me.CheckNumberTextBox.Enabled Then Me.CheckNumberTextBox.Text = ""
        If Me.ProcedureCodeTextBox.Enabled Then Me.ProcedureCodeTextBox.Text = ""
        If Me.ModifierTextBox.Enabled Then Me.ModifierTextBox.Text = ""
        If Me.DiagnosisCodesTextBox.Enabled Then Me.DiagnosisCodesTextBox.Text = ""
        If Me.BillTypeTextBox.Enabled Then Me.BillTypeTextBox.Text = ""
        If Me.PlaceOfServiceTextBox.Enabled Then Me.PlaceOfServiceTextBox.Text = ""
        If Me.ProviderCheckBox.Enabled Then Me.ProviderCheckBox.Checked = False
        If Me.IncidentDateCheckBox.Enabled Then Me.IncidentDateCheckBox.Checked = False
        If Me.AccidentCheckBox.Enabled Then Me.AccidentCheckBox.Checked = False
        If Me.ChiroCheckBox.Enabled Then Me.ChiroCheckBox.Checked = False
        If Me.DeniedCheckBox.Enabled Then Me.DeniedCheckBox.Checked = False
        If Me.CheckAmountTextBox.Enabled Then Me.CheckAmountTextBox.Text = ""

        Me.CustomerServiceResultsDataGrid.ContextMenuStrip = Nothing

        ResetTabs()

        MatchesLabel.Text = ""
        ToolTip1.RemoveAll()

        Me.SearchingLabel.Text = ""
        If Me.PatientFirstNameTextBox.Enabled Then Me.PatientFirstNameTextBox.Text = ""
        If Me.PatientLastNameTextBox.Enabled Then Me.PatientLastNameTextBox.Text = ""

        Me.AddressLabel.Text = ""
        Me.PrescriptionsControl.ClearAll()
        Me.NpiRegistryControl.ClearNPI()
        Me.ProviderControl.ClearProvider()

    End Sub

    Private Shared Function GetUniqueClaimIdRows(ByVal searchDataTable As DataTable) As DataTable
        Dim DT As DataTable
        Dim Found As Boolean = False

        Try

            If searchDataTable Is Nothing Then Return Nothing

            DT = searchDataTable.Copy
            DT.Rows.Clear()

            DT.BeginLoadData()
            For I As Integer = 0 To searchDataTable.Rows.Count - 1
                Found = False
                If DT.Select("CLAIM_ID = " & searchDataTable.Rows(I)("CLAIM_ID").ToString.Trim).Length > 0 Then
                    Found = True
                End If
                If Not Found Then
                    DT.ImportRow(searchDataTable.Rows(I))
                End If
            Next
            DT.EndLoadData()

            Return DT

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Sub ShowNDCLookup()

        Dim NDCCodeLookUp As New NDCLookup

        Try

            If NDCCodeLookUp.ShowDialog(Me) = DialogResult.OK Then
                NDCCodeLookUp.Hide()
            End If

        Catch ex As Exception

            Throw

        Finally
            If NDCCodeLookUp IsNot Nothing Then NDCCodeLookUp.Dispose()
            NDCCodeLookUp = Nothing
        End Try

    End Sub

    Public Sub ShowDiagnosisLookup()

        Dim DiagnosisCodeLookUp As New DiagnosisCodeLookupForm

        Try

            If DiagnosisCodeLookUp.ShowDialog(Me) = DialogResult.OK Then

            End If

        Catch ex As Exception

            Throw

        Finally
            If DiagnosisCodeLookUp IsNot Nothing Then DiagnosisCodeLookUp.Dispose()
            DiagnosisCodeLookUp = Nothing
        End Try

    End Sub

    Public Sub ShowProcedureLookup()
        Dim ProcCodeLookup As New ProcedureCodeLookupForm

        Try

            If ProcCodeLookup.ShowDialog(Me) = DialogResult.OK Then
            End If

        Catch ex As Exception

            Throw

        Finally
            If ProcCodeLookup IsNot Nothing Then ProcCodeLookup.Dispose()
            ProcCodeLookup = Nothing

        End Try

    End Sub

    Public Sub ShowModifierLookup()
        Dim ModifierCodeLookUp As New ModifierLookup

        Try

            If ModifierCodeLookUp.ShowDialog(Me) = DialogResult.OK Then
            End If

        Catch ex As Exception

            Throw

        Finally

            If ModifierCodeLookUp IsNot Nothing Then ModifierCodeLookUp.Dispose()
            ModifierCodeLookUp = Nothing

        End Try
    End Sub

    Public Sub ShowBillTypeLookup()

        Dim BillTypeLookUp As New BillTypeLookup

        Try

            If BillTypeLookUp.ShowDialog(Me) = DialogResult.OK Then
            End If

        Catch ex As Exception

            Throw

        Finally

            If BillTypeLookUp IsNot Nothing Then BillTypeLookUp.Dispose()
            BillTypeLookUp = Nothing
        End Try

    End Sub

    Public Sub ShowPlaceOfServiceLookup()

        Dim POSLookUp As New PlaceOfServiceLookup

        Try

            If POSLookUp.ShowDialog(Me) = DialogResult.OK Then
            End If

        Catch ex As Exception

            Throw

        Finally

            If POSLookUp IsNot Nothing Then POSLookUp.Dispose()
            POSLookUp = Nothing
        End Try

    End Sub

    Public Function DeterminePatientAndParticipant(ByRef familyDS As DataSet, ByRef patDR As DataRow, ByRef partDR As DataRow) As Boolean

        Dim FamiliesDT As New DataTable
        Dim DocumentSSN As Integer
        Dim DR As DataRow

        Try
            patDR = Nothing
            partDR = Nothing

            _FoundPatient = False
            _FoundParticipant = False

            Using WC As New GlobalCursor

                If FamilyIDTextBox.Text.Trim.Length > 0 Then
                Else
                    If ParticipantSSNTextBox.Text.Trim.Length > 0 Then
                        DR = CMSDALFDBMD.RetrieveParticipantInfofromPartSSN(CInt(UFCWGeneral.UnFormatSSN(ParticipantSSNTextBox.Text)))
                        If DR IsNot Nothing Then FamiliesDT = DR.Table.Copy()
                    ElseIf PatientSSNTextBox.Text.Trim.Length > 0 Then
                        DR = CMSDALFDBMD.RetrievePatientInfo(CInt(UFCWGeneral.UnFormatSSN(PatientSSNTextBox.Text))) 'Possible Dual coverage
                        If DR IsNot Nothing Then FamiliesDT = DR.Table.Copy()
                    ElseIf ClaimIDTextBox.Text.Trim.Length > 0 Then
                        If ClaimIDTextBox.Text.Trim.IsInteger() Then
                            FamiliesDT = CMSDALFDBMD.RetrieveFamilyIDRelationIDByClaimID(CInt(ClaimIDTextBox.Text.Trim))
                        Else
                            FamiliesDT = CMSDALFDBMD.RetrieveFamilyIDRelationIDByMaxIDOrReferenceID(ClaimIDTextBox.Text.Trim)
                        End If
                        ' AndAlso (Not reg.IsMatch(ClaimIDTextBox.Text.Trim) AndAlso reg2.IsMatch(ClaimIDTextBox.Text.Trim)) AndAlso CDec(ClaimIDTextBox.Text.Trim) < Int32.MaxValue Then
                    ElseIf DocIDTextBox.Text.Trim.Length > 0 Then
                        DocumentSSN = CMSDALDBO.RetrieveSSNByDocumentID(If(_ENV = "P", "SQL Database Instance", "SQ1 Database Instance"), CInt(DocIDTextBox.Text.Trim))
                        DR = CMSDALFDBMD.RetrievePatientInfo(DocumentSSN)
                        If DR IsNot Nothing Then FamiliesDT = DR.Table.Copy()
                    End If

                End If

                If FamiliesDT IsNot Nothing AndAlso FamiliesDT.Rows.Count = 1 Then
                    Me.FamilyID = UFCWGeneral.IsNullIntegerHandler(FamiliesDT(0)("FAMILY_ID"))
                    If ClaimIDTextBox.Text.Trim.Length > 0 OrElse DocIDTextBox.Text.Trim.Length > 0 Then
                        Me.RelationID = UFCWGeneral.IsNullShortHandler(FamiliesDT(0)("RELATION_ID"))
                        _FoundPatient = True
                    End If
                End If

                If Me.FamilyID IsNot Nothing Then
                    familyDS = CMSDALFDBMD.RetrievePatients(CInt(Me.FamilyID))

                    If familyDS Is Nothing OrElse familyDS.Tables(0).Rows.Count > 0 Then
                        Dim QueryParticipantResults =
                            From Family In familyDS.Tables(0).AsEnumerable()
                            Where Family.Field(Of Integer)("RELATION_ID") = 0
                            Select Family

                        partDR = QueryParticipantResults.First
                        _FoundParticipant = True

                        If PatientSSNTextBox.Text.Trim.Length > 0 Then

                            Dim QueryPatientResults =
                                From Family In familyDS.Tables(0).AsEnumerable()
                                Where Family.Field(Of Integer)("SSNO") = CInt(UFCWGeneral.UnFormatSSN(PatientSSNTextBox.Text))
                                Select Family

                            If QueryPatientResults.Any() Then
                                patDR = QueryPatientResults.First
                                Me.RelationID = UFCWGeneral.IsNullShortHandler(patDR("RELATION_ID"))

                                _FoundPatient = True

                            End If

                        ElseIf RelationID.ToString.Trim.Length > 0 Then

                            Dim QueryPatientResults =
                                From Family In familyDS.Tables(0).AsEnumerable()
                                Where Family.Field(Of Integer)("RELATION_ID") = CShort(Me.RelationID)
                                Select Family

                            If QueryPatientResults.Any() Then
                                patDR = QueryPatientResults.First
                                Me.RelationID = UFCWGeneral.IsNullShortHandler(patDR("RELATION_ID"))

                                _FoundPatient = True

                            End If
                        End If
                    End If

                End If

            End Using

            Return _FoundParticipant

        Catch ex As Exception

            Throw

        End Try

    End Function

    Private Sub ResolveProviderTabUsingClaimsResults()

        Dim ProviderIDResults As DataTable

        Try

            If Not _SearchUIMode = -1 Then ' Claim Only Search requested from pay screen

                Using WC As New GlobalCursor

                    ProviderIDResults = CMSDALCommon.SelectDistinctAndSorted("CustomerServiceResults", _SearchResultsDT, "PROV_ID")

                    If ProviderIDResults IsNot Nothing AndAlso ProviderIDResults.Rows.Count = 1 Then
                        ProviderControl.LoadProvider(ProviderIDResults(0)("PROV_ID").ToString)
                    Else
                        ProviderIDResults = CMSDALCommon.SelectDistinctAndSorted("CustomerServiceResults", _SearchResultsDT, "PROV_TIN")
                        If ProviderIDResults IsNot Nothing AndAlso ProviderIDResults.Rows.Count = 1 Then
                            ProviderControl.LoadProvider(ProviderIDResults(0)("PROV_TIN").ToString)
                        End If
                    End If

                    NpiRegistryControl.Visible = False

                    If ProviderControl.NPI > 0 Then
                        NpiRegistryControl.LoadNPI(CDec(ProviderControl.NPI))
                        NpiRegistryControl.Visible = True
                    Else
                        ProviderIDResults = CMSDALCommon.SelectDistinctAndSorted("CustomerServiceResults", _SearchResultsDT, "RENDERING_NPI")
                        If ProviderIDResults IsNot Nothing AndAlso ProviderIDResults.Rows.Count = 1 Then
                            NpiRegistryControl.LoadNPI(CDec(ProviderIDResults(0)("RENDERING_NPI").ToString))
                            NpiRegistryControl.Visible = True
                        End If
                    End If

                    If ProviderControl.ProviderID > 0 OrElse NpiRegistryControl.NPI > 0 Then _IncludeProvider = True

                End Using
            End If

        Catch ex As Exception

            Throw

        End Try

    End Sub

    Private Sub ResolveProviderTabUsingSearchCriteria()

        Try

            NpiRegistryControl.Visible = False

            If Not _SearchUIMode = -1 Then ' Claim Only Search requested from pay screen

                Using WC As New GlobalCursor

                    If Me.ProviderIDTextBox.Text.Trim.Length > 9 Then

                    ElseIf Me.ProviderIDTextBox.Text.Trim.Length > 7 Then

                        ProviderControl.LoadProvider(Me.ProviderIDTextBox.Text.ToString.Trim)

                    ElseIf Me.ProviderIDTextBox.Text.Trim.Length > 0 Then
                        ProviderControl.LoadProvider(Me.ProviderIDTextBox.Text.ToString.Trim)
                    ElseIf Me.ProviderIDTextBox.Text.Trim.Length < 1 Then
                        Exit Sub
                    End If

                    If ProviderControl.NPI > 0 Then
                        NpiRegistryControl.LoadNPI(CDec(ProviderControl.NPI))
                    Else
                        NpiRegistryControl.LoadNPI(CDec(Me.ProviderIDTextBox.Text.ToString.Trim))
                    End If

                    If NpiRegistryControl.NPI > 0 Then NpiRegistryControl.Visible = True

                    If ProviderControl.ProviderID > 0 OrElse NpiRegistryControl.NPI > 0 Then _IncludeProvider = True

                End Using
            End If

        Catch ex As Exception

            Throw

        End Try

    End Sub

    Public Sub Search(Optional ByVal mode As Integer = 0)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Search using the criteria on the form
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/18/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim SearchbyCount As Integer = 0
        Dim DT As DataTable
        Dim SearchThread As ExecuteSearch
        Dim XMLDoc As XmlDocument

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            SearchButton.Enabled = False
            PatientSearchButton.Enabled = False
            ClearAllButton.Enabled = False
            CancelButton.Enabled = False

            If Not Me.ValidateChildren(ValidationConstraints.Enabled) Then
                Return
            End If

            TaskTimer.Enabled = False

            _SearchUIMode = mode

            If _CollapseUIMode Then
                CollapseControl()
            End If

            _IsBatchSearch = False
            _IsMemberSearch = False
            _IsProviderSearch = False
            _IsClaimSearch = False
            _IsDocumentSearch = False

            _IsAccumFamilySearch = True
            _IsAccumClaimSearch = True

            _IncludeDemographics = False
            _IncludeDental = False

            _IncludeHRAActivity = True
            _IncludeHRABalance = True
            _IncludePrescriptions = True
            _IncludeAccumulators = True
            _IncludeImaging = True

            _IncludeProvider = False

            _PatientsDS = Nothing
            _PartDR = Nothing
            _PatDR = Nothing

            If PatientSSNTextBox.Text.ToString.Trim.Length > 0 Then SearchbyCount += 1
            If PatientSSNTextBox.Text.ToString.Trim.Length > 0 AndAlso RelationIDTextBox.Text.Trim.Length > 0 Then SearchbyCount += 1
            If ParticipantSSNTextBox.Text.ToString.Trim.Length > 0 Then SearchbyCount += 1
            If FamilyIDTextBox.Text.ToString.Trim.Length > 0 AndAlso ParticipantSSNTextBox.Text.Trim.Length = 0 Then SearchbyCount += 1

            ErrorProvider1.Clear()

            If SearchbyCount > 1 Then
                ErrorProvider1.SetErrorWithTracking(PatientSSNTextBox, "Search by either Family ID / Relation ID (Default 0), Participant SSN, Patient SSN")
                ErrorProvider1.SetErrorWithTracking(ParticipantSSNTextBox, "Search by either Family ID / Relation ID (Default 0), Participant SSN, Patient SSN")
                ErrorProvider1.SetErrorWithTracking(FamilyIDTextBox, "Search by either Family ID / Relation ID (Default 0), Participant SSN, Patient SSN")
                MessageBox.Show("Search by only 1 (One) of Family ID / Relation ID (Default 0), Participant SSN, Patient SSN", "Improper Search Criteria", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If UFCWGeneralAD.CMSEligibilityEmployee OrElse UFCWGeneralAD.CMSCanAdjudicateEmployee Then
                _EmployeeAccess = True
            End If

            If UFCWGeneralAD.CMSLocalsEmployee OrElse UFCWGeneralAD.CMSDental OrElse UFCWGeneralAD.CMSEligibility OrElse UFCWGeneralAD.CMSEligibilityEmployee OrElse UFCWGeneralAD.CMSCanAdjudicateEmployee OrElse UFCWGeneralAD.CMSUsers Then
                _LocalEmployeeAccess = True
            End If

            If PatientSSNTextBox.Text.ToString.Trim.Length > 0 Then
                RelationIDTextBox.Text = ""
            End If

            If PatientSSNTextBox.Text.ToString.Trim.Length > 0 OrElse ParticipantSSNTextBox.Text.ToString.Trim.Length > 0 OrElse Me.FamilyIDTextBox.Text.ToString.Trim.Length > 0 Then
                _IsMemberSearch = True
                _IncludeDental = True
            End If

            If Me.ProviderIDTextBox.Text.Trim.Length > 0 OrElse Me.ProviderNameTextBox.Text.Trim.Length > 0 Then
                _IsProviderSearch = True
            End If

            If Me.ClaimIDTextBox.Text.ToString.Trim.Length > 0 Then
                _IsClaimSearch = True
                _IncludeDental = False
                _IncludeImaging = False
            End If

            If PatientSSNTextBox.Text.ToString.Trim.Length > 0 Then

                'check if Patient SSN crosses family's
                _IncludeDemographics = True

                Select Case EstablishFamily()
                    Case DialogResult.Ignore ' if a patient exists in multiple families but no family is selected then disable all family specific views
                        _IsClaimSearch = False
                        _IsMemberSearch = False
                        _IncludeDental = False
                        _IncludeAccumulators = False
                        _IncludeHRABalance = False
                        _IncludeHRAActivity = False
                        _IncludeDemographics = False

                End Select
            End If

            If DocIDTextBox.Text.ToString.Trim.Length > 0 Then
                _IsDocumentSearch = True
            End If

            If ParticipantSSNTextBox.Text.ToString.Trim.Length > 0 Then
                If Not ValidateParticipantSSN() Then Return
            End If

            ''For Family info tab
            If ParticipantSSNTextBox.Text.ToString.Trim.Length > 0 OrElse Me.FamilyIDTextBox.Text.ToString.Trim.Length > 0 OrElse Me.ClaimIDTextBox.Text.ToString.Trim.Length > 0 Then
                _IncludeDemographics = True
            End If

            ''For Accumulator tab(claim search)
            If Me.ClaimIDTextBox.Text.ToString.Trim.Length = 0 Then
                _IsAccumClaimSearch = False
            End If

            ''For Accumulator tab(not claim search)
            If PatientSSNTextBox.Text.ToString.Trim.Length = 0 AndAlso ParticipantSSNTextBox.Text.ToString.Trim.Length = 0 AndAlso Me.FamilyIDTextBox.Text.ToString.Trim.Length = 0 Then
                _IsAccumFamilySearch = False
            End If

            If Not _IsAccumClaimSearch AndAlso Not _IsAccumFamilySearch Then
                _IncludeAccumulators = False
            End If

            If Me.BatchNumberTextBox.Text.ToString.Trim.Length > 0 Then
                _IsBatchSearch = True
            End If

            '' For HRAActivity and HRABalance  ''for Prescriptions tab also
            If Me.FamilyIDTextBox.Text.ToString.Trim.Length = 0 AndAlso PatientSSNTextBox.Text.ToString.Trim.Length = 0 AndAlso ParticipantSSNTextBox.Text.ToString.Trim.Length = 0 Then
                _IncludeHRAActivity = False
                _IncludeHRABalance = False
                _IncludePrescriptions = False
            End If

            ''Validating Date of Service From and To values
            If Me.DateOfServiceCheckBox.CheckState = CheckState.Checked AndAlso DateOfServiceFromDateTimePicker.Value.ToString.Trim.Length > 0 AndAlso DateOfServiceToDateTimePicker.Value.ToString.Trim.Length > 0 Then
                If DateOfServiceFromDateTimePicker.Value.Date > DateOfServiceToDateTimePicker.Value.Date Then
                    ErrorProvider1.SetErrorWithTracking(DateOfServiceToDateTimePicker, "Date is before 'FROM'")

                    MessageBox.Show("'To' Date of Service should be greater than 'From' Date of Service", "Invalid Date Of Service", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If
            End If

            ''Validating Date of Service From and To values
            If Me.ProcessedDateCheckBox.CheckState = CheckState.Checked AndAlso ProcessedDateFromDateTimePicker.Value.ToString.Trim.Length > 0 AndAlso ProcessedDateToDateTimePicker.Value.ToString.Trim.Length > 0 Then
                If ProcessedDateFromDateTimePicker.Value.Date > ProcessedDateToDateTimePicker.Value.Date Then
                    ErrorProvider1.SetErrorWithTracking(ProcessedDateToDateTimePicker, "Date is before 'FROM'")

                    MessageBox.Show("'To' Processed Date should be greater than 'From' Processed Date", "Invalid Processed Date", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If
            End If

            ''Validating Received Date From and To values
            If Me.ReceivedDateCheckBox.CheckState = CheckState.Checked AndAlso ReceivedFromDateTimePicker.Value.ToString.Trim.Length > 0 AndAlso ReceivedToDateTimePicker.Value.ToString.Trim.Length > 0 Then
                If ReceivedFromDateTimePicker.Value.Date > ReceivedToDateTimePicker.Value.Date Then
                    ErrorProvider1.SetErrorWithTracking(ReceivedToDateTimePicker, "Date is before 'FROM'")

                    MessageBox.Show("'To' Received Date should be greater than 'From' Received Date", "Invalid Received Date", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If
            End If

            CustomerServiceResultsDataGrid.ReadOnly = True

            TabControl.SendToBack()
            SearchingLabel.BringToFront()
            SearchingLabel.Refresh()

            ClearAllDataSources()

            If _IsMemberSearch OrElse _IsClaimSearch Then

                If Not DeterminePatientAndParticipant(_PatientsDS, _PatDR, _PartDR) Then

                    If _IsClaimSearch Then
                        MessageBox.Show("Claim Not Found.", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    ElseIf PatientSSNTextBox.Text.Trim.Length > 0 OrElse RelationIDTextBox.Text.Trim.Length > 0 Then
                        MessageBox.Show("Patient Not Found.", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        MessageBox.Show("Participant Not Found.  (Try searching by Patient)", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If

                    Return
                End If

                'if search is specific to an individual validate security ahead of all else
                If _PartDR IsNot Nothing Then
                    If CInt(_PartDR("TRUST_SW")) = 1 AndAlso Not _EmployeeAccess Then
                        RestrictedAccess("")
                        Return
                    ElseIf CInt(_PartDR("LOCAL_SW")) = 1 AndAlso Not _EmployeeAccess AndAlso Not _LocalEmployeeAccess Then
                        RestrictedAccess("LOCAL")
                        Return
                    End If
                End If

            End If

            Using WC As New GlobalCursor

                PrePopulateAssociatedControls()

                XMLDoc = GetSearchCriteriaInXmlFormat()
                'Dim DT As DataTable = FDBMD.RetrieveClaims(xmlDoc.SelectNodes("/Criteria"))
                'Threaded the search for better user response
                '************************************************************************

                If UFCWGeneralAD.CMSCanAudit Then
                    CustomerServiceResultsDataGrid.AllowMultiSelect = True
                End If

                SearchThread = New ExecuteSearch(XMLDoc.SelectNodes("/Criteria"), _EmployeeAccess, _LocalEmployeeAccess)

                SearchThread.Execute()
                DT = SearchThread.ResultSet

                _SearchResultsDT = DT

            End Using

            If _SearchResultsDT?.Rows.Count < 1 Then Me.CustomerServiceResultsDataGrid.ContextMenuStrip = Nothing

            If _SearchResultsDT?.Rows.Count > 0 Then
                'Claims have been identified using search critiria
                Using WC As New GlobalCursor

                    ResolveProviderTabUsingClaimsResults()

                    '' Restricted Access to CMSCanAdjudicateEmployee and CMSCanAccessLocalsEmployee
                    If ((Not _EmployeeAccess AndAlso Not _LocalEmployeeAccess) AndAlso (_SearchResultsDT.Rows.Count = 1)) Then
                        If (_SearchResultsDT.Rows(0)("STATUS").ToString.Contains("RESTRICTED")) Then
                            RestrictedAccess(CStr(_SearchResultsDT.Rows(0)("STATUS")))
                            Exit Try
                        End If
                    ElseIf (Not _EmployeeAccess) AndAlso (_SearchResultsDT.Rows.Count = 1) Then
                        If (_SearchResultsDT.Rows(0)("STATUS").ToString.Contains("RESTRICTED")) Then
                            RestrictedAccess(CStr(_SearchResultsDT.Rows(0)("STATUS")))
                            Exit Try
                        End If

                    End If

                    If (_SearchResultsDT.Rows.Count >= 1) AndAlso _SearchResultsDT.Rows(_SearchResultsDT.Rows.Count - 1)("STATUS").ToString = "RESTRICTED" Then
                        _SearchResultsDT.Rows.RemoveAt(_SearchResultsDT.Rows.Count - 1)
                    End If

                    CustomerServiceResultsDataGrid.SuspendLayout()

                    CustomerServiceResultsDataGrid.DataSource = _SearchResultsDT
                    CustomerServiceResultsDataGrid.Sort = If(CustomerServiceResultsDataGrid.LastSortedBy, CustomerServiceResultsDataGrid.DefaultSort)

                    SetTableStyle(CustomerServiceResultsDataGrid, ResultsDataGridCustomContextMenu, CustomerServiceResultsDataGrid.Name)

                    _OriginalDT = Me.CustomerServiceResultsDataGrid.GetCurrentDataTable  '' Required for Exporting into Excel format

                    CustomerServiceResultsDataGrid.ResumeLayout()

                End Using

                If (_IsClaimSearch OrElse _IsDocumentSearch) AndAlso _SearchResultsDT.Rows.Count > 0 Then
                    PatientSearchButton.Visible = True
                End If

            Else 'No claims were found

                If _IsProviderSearch Then ResolveProviderTabUsingSearchCriteria()

                StatusTypesComboBox.DataSource = Nothing
                DocTypesComboBox.DataSource = Nothing
                AdjustersComboBox.DataSource = Nothing
                Me.MatchesLabel.Text = ""
                Me.CustomerServiceResultsDataGrid.DataSource = Nothing
                Me.CustomerServiceResultsDataGrid.ContextMenuStrip = Nothing
            End If

            If _IsMemberSearch Then
                If PatientSSNTextBox.Text.ToString.Trim.Length > 0 OrElse RelationID.ToString.Trim.Length > 0 Then
                    If _FoundPatient Then
                        If _SearchResultsDT Is Nothing OrElse _SearchResultsDT.Rows.Count < 1 Then
                            MessageBox.Show("No Matching Claims Found for Patient", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        End If
                    Else
                        MessageBox.Show("Patient not Found (Try searching by Family / Participant)", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                Else
                    If _FoundParticipant Then
                        If _SearchResultsDT Is Nothing OrElse _SearchResultsDT.Rows.Count < 1 Then
                            MessageBox.Show("No Matching Claims Found for Participant (Try searching by Patient)", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        End If
                    Else
                        MessageBox.Show("No matching records. Participant not Found (Try searching by Patient)", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                End If
            ElseIf _IsClaimSearch Then

                If _SearchResultsDT Is Nothing OrElse _SearchResultsDT.Rows.Count < 1 Then
                    MessageBox.Show("No Matching Claim(s) Found.", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                    _IncludeAccumulators = False

                End If

            ElseIf _IsProviderSearch Then
                If _SearchResultsDT Is Nothing OrElse _SearchResultsDT.Rows.Count < 1 Then
                    MessageBox.Show("No Matching Claims Found.", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            ElseIf _IsProviderSearch AndAlso UFCWDocsControl.DocumentsFound > 0 Then
            ElseIf _IsDocumentSearch Then
                If (_SearchResultsDT Is Nothing OrElse _SearchResultsDT.Rows.Count < 1) AndAlso UFCWDocsControl.DocumentsFound < 1 Then
                    MessageBox.Show("No Matching Document Found.", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            Else
                If (_SearchResultsDT Is Nothing OrElse _SearchResultsDT.Rows.Count < 1) AndAlso UFCWDocsControl.DocumentsFound < 1 Then
                    MessageBox.Show("No matching records. ", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
            End If

            If CustomerServiceResultsDataGrid.DataSource IsNot Nothing Then
                Dim DV As DataView
                Dim CM As CurrencyManager

                CM = CType(Me.BindingContext(CustomerServiceResultsDataGrid.DataSource, CustomerServiceResultsDataGrid.DataMember), CurrencyManager)
                DV = CType(CM.List, DataView)

                If DV IsNot Nothing Then

                    If DV.Count > 0 Then
                        If CStr(DV(0)("STATUS")) <> "COMPLETED" Then
                            Me.ReprintEOBMenuItem.Enabled = False
                        Else
                            Me.ReprintEOBMenuItem.Enabled = True
                        End If
                    End If
                End If
            End If

            TabControl.Visible = True

            DisplayAppropriateTabs()

            TabControl.BringToFront()
            TabControl.Refresh()

        Catch ex As System.IO.IOException

            Me.CustomerServiceResultsDataGrid.ContextMenuStrip = Nothing
            StatusTypesComboBox.DataSource = Nothing
            DocTypesComboBox.DataSource = Nothing
            AdjustersComboBox.DataSource = Nothing
            Me.MatchesLabel.Text = ""
            Me.CustomerServiceResultsDataGrid.DataSource = Nothing

            MessageBox.Show("Search Request is taking too long and was cancelled. " & Environment.NewLine() & " Try limiting your search by including other search criteria such as a date range.", "Search TimeOut occurred.", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As DB2Exception

            Throw

        Catch ex As NullReferenceException

            Me.CustomerServiceResultsDataGrid.ContextMenuStrip = Nothing
            StatusTypesComboBox.DataSource = Nothing
            DocTypesComboBox.DataSource = Nothing
            AdjustersComboBox.DataSource = Nothing
            Me.MatchesLabel.Text = ""
            Me.CustomerServiceResultsDataGrid.DataSource = Nothing

            MessageBox.Show(ex.ToString, "Invalid request", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As ArgumentException

            Me.CustomerServiceResultsDataGrid.ContextMenuStrip = Nothing
            StatusTypesComboBox.DataSource = Nothing
            DocTypesComboBox.DataSource = Nothing
            AdjustersComboBox.DataSource = Nothing
            Me.MatchesLabel.Text = ""
            Me.CustomerServiceResultsDataGrid.DataSource = Nothing

            MessageBox.Show(ex.ToString, "Access Restricted", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            Throw

        Finally

            Me.AutoValidate = HoldAutoValidate

            SearchingLabel.SendToBack()

            SearchThread = Nothing

            SearchButton.Enabled = True
            PatientSearchButton.Enabled = True
            ClearAllButton.Enabled = True
            CancelButton.Enabled = False
            ClearAllButton.Visible = True

        End Try
    End Sub

    Private Sub PopulateCOBInfo(ByVal arg As Object)

        Dim args() As Integer = CType(arg, Integer())

        PopulateCOBInfo(args(0), CType(args(1), Short?))
    End Sub
    Private Sub CollectPremiumsEnrollmentInfo(familyID As Integer)
        _PopulatePremiumsEnrollmentTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBMD.GetPremLetterHistory(familyID, _PremLetterDS))
    End Sub
    Private Sub PopulatePremiumsEnrollmentInfo(ByVal familyID As Integer)

        If _PopulatePremiumsEnrollmentTask IsNot Nothing Then
            _PopulatePremiumsEnrollmentTask.Wait()

            _PremLetterDS = New DataSet
            _PremLetterDS = _PopulatePremiumsEnrollmentTask.Result

            PremiumsEnrollmentControl.LoadPremiumsControl(familyID, _PremLetterDS)

            _PopulatePremiumsEnrollmentTask = Nothing

        End If

    End Sub

    Private Shared Sub CollectEligibilityHoursInfo(ByRef populateEligibilityHoursTask As Task(Of DataSet), familyID As Integer, eligibilityHoursDS As DataSet)
        populateEligibilityHoursTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBMD.GetEligibilityHours(familyID, eligibilityHoursDS))
    End Sub

    Private Shared Sub PopulateEligibilityHoursInfo(ByRef eligibilityHoursControl As EligibilityHoursControl, ByRef populateEligibilityHoursTask As Task(Of DataSet), ByVal familyID As Integer, ByRef eligibilityHoursDS As DataSet)

        If populateEligibilityHoursTask IsNot Nothing Then
            populateEligibilityHoursTask.Wait()

            eligibilityHoursDS = New DataSet
            eligibilityHoursDS = populateEligibilityHoursTask.Result

            eligibilityHoursControl.LoadHours(familyID, eligibilityHoursDS)

            populateEligibilityHoursTask = Nothing

        End If

    End Sub

    Private Sub CollectPremiumsInfo(familyID As Integer)
        _PopulatePremiumsTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBMD.GetPremiumInformation(familyID, _PremiumsDS))
    End Sub

    Private Sub PopulatePremiumsInfo(ByVal familyID As Integer)

        If _PopulatePremiumsTask IsNot Nothing Then
            _PopulatePremiumsTask.Wait()

            _PremiumsDS = New DataSet
            _PremiumsDS = _PopulatePremiumsTask.Result

            PremiumsControl.LoadPremiumsControl(familyID, _PremiumsDS)

            _PopulatePremiumsTask = Nothing

        End If

    End Sub

    Private Shared Sub CollectHoursInfo(ByRef populateHoursTask As Task(Of DataSet), familyID As Integer, hoursDS As DataSet)
        populateHoursTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBMD.GetHours(familyID, hoursDS))
    End Sub

    Private Shared Sub PopulateHoursInfo(ByRef hoursControl As HoursControl, ByRef populateHoursTask As Task(Of DataSet), ByVal familyID As Integer, ByRef hoursDS As DataSet)

        If populateHoursTask IsNot Nothing Then
            populateHoursTask.Wait()

            hoursDS = New DataSet
            hoursDS = populateHoursTask.Result

            hoursControl.LoadHours(familyID, hoursDS)

            populateHoursTask = Nothing

        End If

    End Sub

    Private Sub CollectCoverageHistoryInfo(familyID As Integer)
        _PopulateCoverageHistoryTask = Task(Of DataSet).Factory.StartNew(Function() CMSDALFDBEL.RetrieveCoverageHistoryAndNetwork(familyID, _CoverageHistoryDS))
    End Sub

    Private Sub PopulateCoverageHistoryInfo(ByVal familyID As Integer)

        If _PopulateCoverageHistoryTask IsNot Nothing Then
            _PopulateCoverageHistoryTask.Wait()

            _CoverageHistoryDS = New DataSet
            _CoverageHistoryDS = _PopulateCoverageHistoryTask.Result

            _PopulateCoverageHistoryTask = Nothing

            CoverageHistory.AppKey = _APPKEY
            CoverageHistory.LoadCoverageHistory(familyID, _CoverageHistoryDS)

        End If

    End Sub

    Private Sub CollectCOBInfo(familyID As Integer, relationID As Short?)

        _PopulateCOBTask = Task(Of DataSet).Factory.StartNew(Function() CType(CMSDALFDBMD.RetrieveCOBInfo(familyID, CShort(If(relationID Is Nothing, -1S, relationID))), DataSet))

    End Sub

    Private Sub PopulateCOBInfo(ByVal theFamilyID As Integer, Optional ByVal theRelationID As Short? = Nothing)

        If _PopulateCOBTask IsNot Nothing Then
            _PopulateCOBTask.Wait()

            _COBDS = New DataSet
            _COBDS.Tables.Clear()
            _COBDS = _PopulateCOBTask.Result

            _PopulateCOBTask = Nothing

            CobControl.LoadCOB(theFamilyID, theRelationID, _COBDS)
            CobControl.ReadOnlyMode = theRelationID IsNot Nothing AndAlso Not UFCWGeneralAD.CMSCanModifyCOB

        End If

    End Sub

    Private Sub PopulateFreeTextInfo(ByVal patientsDS As DataSet, ByVal theFamilyID As Integer, Optional ByVal theRelationID As Short? = Nothing)
        Dim PatDRV As DataRowView
        Dim PartDRV As DataRowView

        Try

            FreeTextEditor.FamilyID = theFamilyID
            FreeTextEditor.RelationID = theRelationID

            patientsDS.Tables(0).DefaultView.RowFilter = "RELATION_ID = 0 AND FAMILY_ID = " & theFamilyID.ToString

            If patientsDS.Tables(0).DefaultView.Count > 0 Then
                PartDRV = patientsDS.Tables(0).DefaultView(0)

                FreeTextEditor.ParticipantSSN = CInt(PartDRV("SSNO"))
                FreeTextEditor.ParticipantLast = UFCWGeneral.IsNullStringHandler(PartDRV("LAST_NAME"))
                FreeTextEditor.ParticipantFirst = UFCWGeneral.IsNullStringHandler(PartDRV("FIRST_NAME"))
            End If

            patientsDS.Tables(0).DefaultView.RowFilter = "FAMILY_ID = " & theFamilyID.ToString & " AND RELATION_ID = " & If(theRelationID Is Nothing, 0S, theRelationID).ToString

            If patientsDS.Tables(0).DefaultView.Count > 0 Then
                PatDRV = patientsDS.Tables(0).DefaultView(0)

                FreeTextEditor.PatientSSN = CInt(PatDRV("SSNO"))
                FreeTextEditor.PatientLast = UFCWGeneral.IsNullStringHandler(PatDRV("LAST_NAME"))
                FreeTextEditor.PatientFirst = UFCWGeneral.IsNullStringHandler(PatDRV("FIRST_NAME"))
            End If

            FreeTextEditor.LoadFreeText()

        Catch ex As Exception

            Throw

        Finally
            patientsDS.Tables(0).DefaultView.RowFilter = ""
        End Try

    End Sub

    Private Sub PopulateEligibilityInfo(ByVal claimsFound As Boolean, Optional ByVal theFamilyID As Integer? = Nothing, Optional ByVal theRelationID As Short? = Nothing)

        Dim SSN As String
        Dim PartDR As DataRow

        Try

            If theFamilyID Is Nothing Then

                If Me.ParticipantSSNTextBox.Text <> "" Then
                    SSN = ParticipantSSNTextBox.Text.Replace("-"c, "")
                    PartDR = CMSDALFDBMD.RetrieveParticipantInfo(CInt(SSN))
                ElseIf Me.PatientSSNTextBox.Text <> "" Then
                    SSN = PatientSSNTextBox.Text.Replace("-"c, "")
                    PartDR = CMSDALFDBMD.RetrievePatientInfo(CInt(SSN))

                End If

                If PartDR IsNot Nothing Then
                    If CBool(PartDR("TRUST_SW")) AndAlso Not UFCWGeneralAD.CMSCanAdjudicateEmployee AndAlso Not UFCWGeneralAD.CMSEligibilityEmployee Then
                        Throw New ArgumentException("You are not authorized to view Trust Employee Information.")
                    End If
                    theFamilyID = CInt(PartDR("FAMILY_ID"))
                    theRelationID = UFCWGeneral.IsNullShortHandler(PartDR("RELATION_ID"))
                Else
                    If claimsFound Then
                        MsgBox("No Participant Eligibility found.", MsgBoxStyle.Information, "Eligibility Warning")
                    Else
                        MsgBox("Search criteria did not result in any Matches in either Claims or Eligibility." & vbCrLf & "Retry search as Patient.", MsgBoxStyle.Information, "No Matches Found")
                    End If
                End If

            End If

            If theFamilyID IsNot Nothing Then

                If _IsMemberSearch OrElse _IsClaimSearch Then

                    CollectCoverageHistoryInfo(CInt(Me.FamilyID))

                    CollectHoursInfo(_PopulateHoursTask, CInt(Me.FamilyID), _HoursDS)
                    CollectEligibilityHoursInfo(_PopulateEligibilityHoursTask, CInt(Me.FamilyID), _EligibilityHoursDS)
                    CollectPremiumsInfo(CInt(Me.FamilyID))
                    CollectPremiumsEnrollmentInfo(CInt(Me.FamilyID))

                    If _PatientsDS.Tables.Count > 1 AndAlso _PatientsDS.Tables(1).Rows.Count > 0 Then 'Dual Coverage member
                        CollectHoursInfo(_PopulateHoursDualCoverageTask, CInt(_PatientsDS.Tables(1).Rows(0)("FAMILY_ID")), _HoursDualCoverageDS)
                        CollectEligibilityHoursInfo(_PopulateEligibilityHoursDualCoverageTask, CInt(_PatientsDS.Tables(1).Rows(0)("FAMILY_ID")), _EligibilityHoursDualCoverageDS)
                    End If

                    EligibilityControl.LoadEligibility(theFamilyID) 'eligibility is always family specific
                    If EligibilityControl.EligibilityDataTable.Rows.Count < 1 Then
                        If claimsFound Then
                            MsgBox("No Participant Eligibility found.", MsgBoxStyle.Information, "Eligibility Warning")
                        Else
                            MsgBox("Search criteria did not result in any Matches in either Claims or Eligibility.", MsgBoxStyle.Information, "No Matches Found")
                        End If
                    End If

                    EligibilityTab.Text = " FAMILY ELIGIBILITY "
                    Me.EligibilitySplitContainer.Panel2Collapsed = True

                    If _PatientsDS.Tables.Count > 1 AndAlso _PatientsDS.Tables(1).Rows.Count > 0 Then 'Dual Coverage member
                        EligibilityTab.Text = "   FAMILY ELIG. (DUAL Cov.)   "
                        Me.EligibilitySplitContainer.Panel2Collapsed = False

                        EligibilityDualCoverageControl.LoadEligibility(CInt(_PatientsDS.Tables(1).Rows(0)("FAMILY_ID")))

                        If EligibilityControl.EligibilityDataTable.Rows.Count < 1 Then
                            If claimsFound Then
                                MsgBox("No Participant Eligibility found.", MsgBoxStyle.Information, "Eligibility Warning")
                            Else
                                MsgBox("Search criteria did not result in any Matches in either Claims or Eligibility.", MsgBoxStyle.Information, "No Matches Found")
                            End If
                        End If
                    ElseIf theRelationID > 0 Then

                        Me.EligibilitySplitContainer.Panel2Collapsed = False

                        EligibilityDualCoverageControl.LoadEligibility(theFamilyID, theRelationID)
                        If EligibilityControl.EligibilityDataTable.Rows.Count > 0 Then
                            Me.EligibilitySplitContainer.Panel2Collapsed = False
                        End If

                    End If

                    Me.HoursSplitContainer.Panel2Collapsed = True
                    Me.EligibilityHoursSplitContainer.Panel2Collapsed = True

                    HoursTab.Text = " HOURS "
                    EligibilityHoursTab.Text = " Elg. HOURS "

                    If _PatientsDS.Tables.Count > 1 AndAlso _PatientsDS.Tables(1).Rows.Count > 0 Then 'Dual Coverage member
                        HoursTab.Text = "   HOURS (DUAL Cov.)   "
                        EligibilityHoursTab.Text = "   Elg. HOURS (DUAL Cov.)   "

                        Me.HoursSplitContainer.Panel2Collapsed = False
                        Me.EligibilityHoursSplitContainer.Panel2Collapsed = False

                    End If

                End If

                If _IncludeDemographics Then LoadDemographics(_PatientsDS)

            End If

        Catch ex As Exception

            Throw

        End Try

    End Sub

    Private Shared Function TransformResultsForBooleanDisplay(ByVal DT As DataTable) As DataTable

        Dim ReturnDT As New DataTable

        For I As Integer = 0 To DT.Columns.Count - 1
            ReturnDT.Columns.Add(New DataColumn)
            ReturnDT.Columns(I).ColumnMapping = DT.Columns(I).ColumnMapping
            ReturnDT.Columns(I).ColumnName = DT.Columns(I).ColumnName
            ReturnDT.Columns(I).DataType = DT.Columns(I).DataType
            ReturnDT.Columns(I).AllowDBNull = DT.Columns(I).AllowDBNull
        Next

        For I As Integer = 0 To DT.Columns.Count - 1
            If ReturnDT.Columns(I).ColumnName.EndsWith("_SW") Then
                ReturnDT.Columns(I).DataType = System.Type.GetType("System.Boolean")
                For x As Integer = 0 To ReturnDT.Rows.Count - 1
                    If ReturnDT.Rows(x)(I) Is System.DBNull.Value Then
                        ReturnDT.Rows(x)(I) = 0
                    End If
                Next
            End If
        Next

        ReturnDT.BeginLoadData()
        For I As Integer = 0 To DT.Rows.Count - 1
            ReturnDT.ImportRow(DT.Rows(I))
        Next
        ReturnDT.EndLoadData()

        For I As Integer = 0 To DT.Columns.Count - 1
            If ReturnDT.Columns(I).ColumnName.EndsWith("_SW") Then
                For X As Integer = 0 To ReturnDT.Rows.Count - 1
                    If ReturnDT.Rows(X)(I) Is System.DBNull.Value Then
                        ReturnDT.Rows(X)(I) = 0
                    End If
                Next
            End If
        Next

        ReturnDT.TableName = "CustomerServiceResults"
        Return ReturnDT

    End Function

    Private Sub DoSearchAbortCleanUp()
        Me.CustomerServiceResultsDataGrid.ContextMenuStrip = Nothing
        StatusTypesComboBox.DataSource = Nothing
        DocTypesComboBox.DataSource = Nothing
        AdjustersComboBox.DataSource = Nothing
        Me.MatchesLabel.Text = ""
        SearchingLabel.Text = ""
        SearchingLabel.SendToBack()
        TabControl.BringToFront()
        TabControl.Refresh()
    End Sub

    Protected Overrides Function ProcessDialogKey(ByVal keyData As System.Windows.Forms.Keys) As Boolean
        If keyData = Keys.Return Then

            SearchButton_Click(New Button, New System.EventArgs)

        ElseIf keyData = Shortcut.CtrlT Then
            ToggleGroupBoxes()
        Else
            Return MyBase.ProcessDialogKey(keyData)
        End If
    End Function

    Private Sub HandleStatusChange()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Handles the Change of a Status
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/18/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            If _SearchResultsDT Is Nothing Then Return

            _StatusDV = New DataView(_SearchResultsDT)
            _StatusDV.Sort = "DOC_CLASS"
            If TabControl.SelectedTab IsNot Nothing Then _StatusDV.RowFilter = "DOC_CLASS = '" & TabControl.SelectedTab.Text & "'"

            If Me.StatusTypesComboBox.Text <> "(all)" AndAlso Me.StatusTypesComboBox.Text.Length > 0 Then _StatusDV.RowFilter += " And STATUS = '" & StatusTypesComboBox.Text & "'"
            If Me.DocTypesComboBox.Text <> "(all)" AndAlso Me.DocTypesComboBox.Text.Length > 0 Then _StatusDV.RowFilter += " And DOC_TYPE = '" & DocTypesComboBox.Text & "'"

            ' if CMSLocals then no Adjuster column is visible
            If Me.AdjustersComboBox.Text <> "(all)" AndAlso Me.AdjustersComboBox.Text.Length > 0 Then _StatusDV.RowFilter += " And PROCESSED_BY = '" & AdjustersComboBox.Text & "'"

            CustomerServiceResultsDataGrid.DataSource = _StatusDV
            MatchesLabel.Text = _StatusDV.Count & " of " & _SearchResultsDT.Rows.Count

        Catch ex As Exception
            Dim s As String = ex.ToString
        End Try
    End Sub

    Private Function LoadTempControls(ByVal TPage As TabPage) As TabPage
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Load temporary controls for dynamic control creation
        ' </summary>
        ' <param name="TPage"></param>
        ' <returns></returns>
        ' <remarks>
        ' "Stolen" from Nick Snyder
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/18/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            MCnt = New Label
            _ShowLabel = New Label
            _ShowLabel2 = New Label
            TypeDropDown = New ComboBox
            TypeDropDown2 = New ComboBox
            MainGrid = New DataGrid

            MCnt.AutoSize = True
            MCnt.Location = New System.Drawing.Point(360, 160)
            MCnt.Anchor = System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right
            MCnt.Anchor = CType(MCnt.Anchor + AnchorStyles.Right, AnchorStyles)
            MCnt.Name = "MCnt"
            MCnt.Size = MatchesLabel.Size
            MCnt.TabIndex = 7
            MCnt.Text = "0 of 0 Matches"

            _ShowLabel.AutoSize = True
            _ShowLabel.Location = New System.Drawing.Point(2, 9)
            _ShowLabel.Name = "ShowStatusLabel"
            _ShowLabel.Size = ShowStatusLabel.Size
            _ShowLabel.TabIndex = 6
            _ShowLabel.Text = "Show Status:"

            _ShowLabel2.AutoSize = True
            _ShowLabel2.Location = New System.Drawing.Point(247, 9)
            _ShowLabel2.Name = "ShowDocTypeLabel"
            _ShowLabel2.Size = ShowDocTypeLabel.Size
            _ShowLabel2.TabIndex = 7
            _ShowLabel2.Text = "Show Doc Type:"

            TypeDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            TypeDropDown.Items.AddRange(New Object() {"(all)"})
            TypeDropDown.Location = New System.Drawing.Point(72, 5)
            TypeDropDown.Name = "StatusTypesComboBox"
            TypeDropDown.Size = StatusTypesComboBox.Size
            TypeDropDown.Sorted = True
            TypeDropDown.TabIndex = 5

            TypeDropDown2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            TypeDropDown2.Items.AddRange(New Object() {"(all)"})
            TypeDropDown2.Location = New System.Drawing.Point(336, 5)
            TypeDropDown2.Name = "DocTypesComboBox"
            TypeDropDown2.Size = DocTypesComboBox.Size
            TypeDropDown2.Sorted = True
            TypeDropDown2.TabIndex = 9

            MainGrid.Anchor = (((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right)
            MainGrid.BackgroundColor = System.Drawing.Color.White
            MainGrid.CaptionVisible = False
            MainGrid.ContextMenuStrip = Me.ResultsDataGridCustomContextMenu
            MainGrid.DataMember = ""
            MainGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
            MainGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
            MainGrid.Location = New System.Drawing.Point(8, 40)
            MainGrid.Name = "ResultsDataGrid"
            MainGrid.ReadOnly = True
            MainGrid.Size = CustomerServiceResultsDataGrid.Size
            MainGrid.TabIndex = 4

            TPage.Controls.Add(MCnt)
            TPage.Controls.Add(_ShowLabel)
            TPage.Controls.Add(TypeDropDown)
            TPage.Controls.Add(MainGrid)

            Return TPage

        Catch ex As Exception

            Throw
        End Try
    End Function

    Public Sub CollapseControl()
        Try

            ClaimGroupBoxViewButton.PerformClick()
            FamilyGroupBoxViewButton.PerformClick()

        Catch ex As Exception

            Throw

        End Try
    End Sub

    Private Sub GetStatusValues()
        _StatusDT = CMSDALFDBMD.RetrieveClaimStatusValues()
        Dim DV As DataView = New DataView(_StatusDT, "STATUS='(all)'", "", DataViewRowState.CurrentRows)
        If DV.Count = 0 Then
            _StatusDT.Rows.Add(New Object() {"(all)"})
        End If
        BindStatusControl()
    End Sub

    Private Sub GetLetters()
        Dim LettersDS As DataSet
        Dim LetterValuesDT As DataTable
        Dim DV As DataView
        Dim DR As DataRow

        Try
            LettersDS = CMSDALFDBMD.GetLetters("MEDICAL")
            DV = New DataView(LettersDS.Tables("LETTER_MASTER"), "", "", DataViewRowState.CurrentRows)
            LetterValuesDT = New DataTable("LETTERS")
            LetterValuesDT.Columns.Add("DOC_TYPE")

            If DV.Count > 0 Then
                For Cnt As Integer = 0 To DV.Count - 1
                    DR = LetterValuesDT.NewRow
                    DR("DOC_TYPE") = "LETTER - " & LettersDS.Tables(0).Rows(Cnt)("LETTER_NAME").ToString
                    LetterValuesDT.Rows.Add(DR)
                Next
            End If

            _LetterDocTypesDT = _DocTypesDT.Clone
            For Each DocTypesDR As DataRow In _DocTypesDT.Rows
                DocTypesDR.EndEdit()
                _LetterDocTypesDT.ImportRow(DocTypesDR)
            Next

            For Each LetterValuesDR As DataRow In LetterValuesDT.Rows
                LetterValuesDR.EndEdit()
                _LetterDocTypesDT.ImportRow(LetterValuesDR)
            Next

        Catch ex As Exception

            Throw

        End Try
    End Sub
#End Region

    Private Sub DiagnosisCodesTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiagnosisCodesTextBox.TextChanged

        If _DiagnosisDT Is Nothing Then Return

        Dim ToolTipText As String = "The Diagnosis"
        Dim DRs As DataRow()

        Try

            If CType(sender, TextBox).Text.Trim.Length > 0 Then
                DRs = _DiagnosisDT.Select("DIAG_VALUE = '" & CType(sender, TextBox).Text.Replace("."c, "") & "'")
                If DRs.Length > 0 Then
                    ToolTipText = DRs(0)("FULL_DESC").ToString
                Else
                    ToolTipText = "*** INVALID DIAGNOSIS ***"
                End If
            End If

            If ToolTipText IsNot Nothing AndAlso ToolTipText.ToString.Trim.Length > 0 Then
                If Me.ToolTip1 Is Nothing OrElse String.Compare(Me.ToolTip1.GetToolTip(CType(sender, TextBox)), ToolTipText.ToString) <> 0 OrElse ToolTip1.Active = False Then
                    ToolTip1.Active = True
                    ToolTip1.SetToolTip(CType(sender, TextBox), ToolTipText.ToString)
                End If
            Else
                ToolTip1.SetToolTip(CType(sender, TextBox), "")
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ProcedureCodeTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProcedureCodeTextBox.TextChanged

        If _ProcedureDT Is Nothing Then Return

        Dim Tooltiptext As String = "The Procedure Code"
        Dim DRs As DataRow()

        Try

            If CType(sender, TextBox).Text.Trim.Length > 0 Then
                DRs = _ProcedureDT.Select("PROC_VALUE = '" & CType(sender, TextBox).Text & "'")
                If DRs.Length > 0 Then
                    Tooltiptext = DRs(0)("FULL_DESC").ToString
                Else
                    Tooltiptext = "*** INVALID PROCEDURE CODE ***"
                End If
            End If

            If Tooltiptext IsNot Nothing AndAlso Tooltiptext.ToString.Trim.Length > 0 Then
                If Me.ToolTip1 Is Nothing OrElse String.Compare(Me.ToolTip1.GetToolTip(CType(sender, TextBox)), Tooltiptext.ToString) <> 0 OrElse ToolTip1.Active = False Then
                    ToolTip1.Active = True
                    ToolTip1.SetToolTip(CType(sender, TextBox), Tooltiptext.ToString)
                End If
            Else
                ToolTip1.SetToolTip(CType(sender, TextBox), "")
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ModifierTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ModifierTextBox.TextChanged

        If _ModifierDT Is Nothing Then Return

        Dim Tooltiptext As String = "The Modifier"
        Dim DRs As DataRow()

        Try

            If CType(sender, TextBox).Text.Trim.Length > 0 Then
                DRs = _ModifierDT.Select("MODIFIER_VALUE = '" & CType(sender, TextBox).Text & "'")
                If DRs.Length > 0 Then
                    Tooltiptext = DRs(0)("FULL_DESC").ToString
                Else
                    Tooltiptext = "*** INVALID MODIFIER ***"
                End If
            End If

            If Tooltiptext IsNot Nothing AndAlso Tooltiptext.ToString.Trim.Length > 0 Then
                If Me.ToolTip1 Is Nothing OrElse String.Compare(Me.ToolTip1.GetToolTip(CType(sender, TextBox)), Tooltiptext.ToString) <> 0 OrElse ToolTip1.Active = False Then
                    ToolTip1.Active = True
                    ToolTip1.SetToolTip(CType(sender, TextBox), Tooltiptext.ToString)
                End If
            Else
                ToolTip1.SetToolTip(CType(sender, TextBox), "")
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub PlaceOfServiceTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlaceOfServiceTextBox.TextChanged

        If _PlaceOfServiceDT Is Nothing Then Return

        Dim Tooltiptext As String = "The Place of Service"
        Dim DRs As DataRow()

        Try

            If CType(sender, TextBox).Text.Trim.Length > 0 Then
                DRs = _PlaceOfServiceDT.Select("PLACE_OF_SERV_VALUE = '" & CType(sender, TextBox).Text & "'")
                If DRs.Length > 0 Then
                    Tooltiptext = DRs(0)("FULL_DESC").ToString
                Else
                    Tooltiptext = "*** INVALID PLACE OF SERVICE ***"
                End If
            End If

            If Tooltiptext IsNot Nothing AndAlso Tooltiptext.ToString.Trim.Length > 0 Then
                If Me.ToolTip1 Is Nothing OrElse String.Compare(Me.ToolTip1.GetToolTip(CType(sender, TextBox)), Tooltiptext.ToString) <> 0 OrElse ToolTip1.Active = False Then
                    ToolTip1.Active = True
                    ToolTip1.SetToolTip(CType(sender, TextBox), Tooltiptext.ToString)
                End If
            Else
                ToolTip1.SetToolTip(CType(sender, TextBox), "")
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub BillTypeTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BillTypeTextBox.TextChanged

        If _BillTypeDT Is Nothing Then Return

        Dim Tooltiptext As String = "The Bill Type"
        Dim DRs As DataRow()

        Try

            If CType(sender, TextBox).Text.Trim.Length > 0 Then
                DRs = _BillTypeDT.Select("BILL_TYPE_VALUE = '" & CType(sender, TextBox).Text & "'")
                If DRs.Length > 0 Then
                    Tooltiptext = DRs(0)("FULL_DESC").ToString
                Else
                    Tooltiptext = "*** INVALID BILL TYPE ***"
                End If
            End If

            If Tooltiptext IsNot Nothing AndAlso Tooltiptext.ToString.Trim.Length > 0 Then
                If Me.ToolTip1 Is Nothing OrElse String.Compare(Me.ToolTip1.GetToolTip(CType(sender, TextBox)), Tooltiptext.ToString) <> 0 OrElse ToolTip1.Active = False Then
                    ToolTip1.Active = True
                    ToolTip1.SetToolTip(CType(sender, TextBox), Tooltiptext.ToString)
                End If
            Else
                ToolTip1.SetToolTip(CType(sender, TextBox), "")
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub AccumulatorsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AccumulatorsMenuItem.Click

        Dim Frm As AccumulatorView

        Try

            If _SSNForMenu IsNot Nothing Then
                If _SSNForMenu.Length > 0 Then
                    Frm = New AccumulatorView
                    Frm.AccumulatorValues.SetFormInfo(UFCWGeneral.DecryptSSN(_SSNForMenu).ToString, UFCWGeneral.NowDate)
                    Frm.AccumulatorValues.ShowLineDetails = False
                    Frm.Show()
                End If
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ProcessedDateCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProcessedDateCheckBox.CheckedChanged
        If _Loading = False Then
            LoadDatabaseOrFilenetSpecificInfo()
            Me.ProcessedDateToDateTimePicker.Enabled = ProcessedDateCheckBox.Checked
            Me.ProcessedDateFromDateTimePicker.Enabled = ProcessedDateCheckBox.Checked
            Me.BetweenLabel2.Enabled = ProcessedDateCheckBox.Checked
            Me.AndLabel2.Enabled = ProcessedDateCheckBox.Checked
            If UFCWGeneralAD.CMSCanAudit OrElse UFCWGeneralAD.CMSCanRunReports Then
                Me.StatusLabel.Visible = ProcessedDateCheckBox.Checked
                Me.StatusComboBox.Visible = ProcessedDateCheckBox.Checked
                Me.StatusComboBox.Enabled = ProcessedDateCheckBox.Checked
                Me.AdjusterLabel.Visible = ProcessedDateCheckBox.Checked
                Me.AdjusterComboBox.Visible = ProcessedDateCheckBox.Checked
                Me.AdjusterComboBox.Enabled = ProcessedDateCheckBox.Checked
                Me.ReasonsTextBox.Enabled = ProcessedDateCheckBox.Checked
                Me.ReasonsTextBox.Visible = ProcessedDateCheckBox.Checked
                Me.ReasonsLabel.Enabled = ProcessedDateCheckBox.Checked
                Me.ReasonsLabel.Visible = ProcessedDateCheckBox.Checked
                Me.ReasonsButton.Enabled = ProcessedDateCheckBox.Checked
                Me.ReasonsButton.Visible = ProcessedDateCheckBox.Checked
            End If
        End If
    End Sub

    Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        _Cancel = True
    End Sub

    Public Shared Function Execute(ByVal nodeList As XmlNodeList) As DataTable
        Try
            Return CMSDALFDBMD.RetrieveSelection(nodeList, UFCWGeneralAD.CMSCanAdjudicateEmployee, UFCWGeneralAD.CMSEligibilityEmployee)

        Catch ex As Exception
            If System.Threading.Thread.CurrentThread.ThreadState <> ThreadState.AbortRequested Then
                Throw
            End If
        End Try
    End Function

    Private Sub FamilyIdTextBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles FamilyIDTextBox.MouseDown
        If (Me.FamilyIDTextBox.Text.Length < 1 OrElse Not IsNumeric(Me.FamilyIDTextBox.Text)) OrElse (Me.RelationIDTextBox.Text.Length < 1 OrElse Not IsNumeric(Me.RelationIDTextBox.Text)) Then
            Me.AccumulatorsMenuItem.Enabled = False
        Else
            Me.AccumulatorsMenuItem.Enabled = True
        End If
        Me.FamilyRelationMenuItem.Visible = True
        If Me.FamilyIDTextBox.Text.Length < 1 Then
            Me.FamilyRelationMenuItem.Enabled = False
        Else
            If IsNumeric(Me.FamilyIDTextBox.Text) Then Me.FamilyRelationMenuItem.Enabled = True
        End If

    End Sub

    Private Sub RelationIdTextBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If (Me.FamilyIDTextBox.Text.Length < 1 OrElse Not IsNumeric(Me.FamilyIDTextBox.Text)) OrElse (Me.RelationIDTextBox.Text.Length < 1 OrElse Not IsNumeric(Me.RelationIDTextBox.Text)) Then
            Me.AccumulatorsMenuItem.Enabled = False
        Else
            Me.AccumulatorsMenuItem.Enabled = True
        End If
        Me.FamilyRelationMenuItem.Visible = False
        RelationIDTextBox.Focus()
    End Sub

    Private Sub PatientSSNTextBox_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PatientSSNTextBox.MouseDown

        If PatientSSNTextBox.Text.Length < 1 OrElse Not IsNumeric(PatientSSNTextBox.Text.Replace("-"c, "")) Then
            Me.AccumulatorsMenuItem.Enabled = False
        Else
            Me.AccumulatorsMenuItem.Enabled = True
        End If
        Me.FamilyRelationMenuItem.Visible = False

    End Sub

    Private Sub ParticipantSSNTextBox_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ParticipantSSNTextBox.MouseDown

        If ParticipantSSNTextBox.Text.Length < 1 OrElse Not IsNumeric(ParticipantSSNTextBox.Text.Replace("-"c, "")) Then
            Me.AccumulatorsMenuItem.Enabled = False
        Else
            Me.AccumulatorsMenuItem.Enabled = True
        End If
        Me.FamilyRelationMenuItem.Visible = False

    End Sub

    Private Sub ResultsMenu_Opening(ByVal sender As Object, ByVal e As System.EventArgs) Handles ResultsDataGridCustomContextMenu.Opening

        Dim InvalidStatusCount As Integer = 0
        Dim NotCurrentUsers As Integer = 0

        Dim ValidStatusCount As Integer = 0
        Dim DataGridCustomContextMenu As ContextMenuStrip

        Dim DRs As New ArrayList
        Dim DR As DataRow
        Dim DG As DataGridCustom

        Try

            DataGridCustomContextMenu = CType(sender, ContextMenuStrip)

            DataGridCustomContextMenu.Items("ReprocessMenuItem").Available = False
            DataGridCustomContextMenu.Items("RemovePricingMenuItem").Available = False
            DataGridCustomContextMenu.Items("ReOpenMenuItem").Available = False
            DataGridCustomContextMenu.Items("ResultsAuditMenuItem").Available = False
            DataGridCustomContextMenu.Items("ResultsAnnotateMenuItem").Available = False
            DataGridCustomContextMenu.Items("ReprintEOBMenuItem").Available = False
            DataGridCustomContextMenu.Items("ResultsHistoryMenuItem").Available = False
            DataGridCustomContextMenu.Items("ResultsDisplayEligibiltyMenuItem").Available = False
            DataGridCustomContextMenu.Items("FreeTextMenuItem").Available = False
            DataGridCustomContextMenu.Items("ResultViewByClaimIDMenuItem").Available = False
            DataGridCustomContextMenu.Items("FreeTextMenuItem").Available = False


            DG = CType(DataGridCustomContextMenu.SourceControl, DataGridCustom)

            DRs = DG.GetSelectedDataRows()

            If DRs?.Count = 1 Then

                DataGridCustomContextMenu.Items("ResultsDisplayDocumentMenuItem").Enabled = True
                DataGridCustomContextMenu.Items("ResultsDisplayDocumentMenuItem").Available = True
                DataGridCustomContextMenu.Items("ResultsDisplayDocumentMenuItem").Text = "Display Document"

                If Not UFCWGeneralAD.CMSLocals Then
                    DataGridCustomContextMenu.Items("ResultsHistoryMenuItem").Enabled = True
                    DataGridCustomContextMenu.Items("ResultsHistoryMenuItem").Available = True
                End If

                DataGridCustomContextMenu.Items("ResultsDisplayLineDetailsMenuItem").Enabled = True
                DataGridCustomContextMenu.Items("ResultsDisplayLineDetailsMenuItem").Available = True
                DataGridCustomContextMenu.Items("ResultsDisplayLineDetailsMenuItem").Text = "Line Details"

                DataGridCustomContextMenu.Items("FreeTextMenuItem").Enabled = True
                DataGridCustomContextMenu.Items("FreeTextMenuItem").Available = True
                DataGridCustomContextMenu.Items("ResultsAnnotateMenuItem").Enabled = True
                DataGridCustomContextMenu.Items("ResultsAnnotateMenuItem").Available = True

                DataGridCustomContextMenu.Items("ResultsDisplayEligibiltyMenuItem").Enabled = True

                If (UFCWGeneralAD.CMSLocals AndAlso UFCWGeneralAD.CMSCanViewEligibilityHours) OrElse Not UFCWGeneralAD.CMSLocals Then
                    DataGridCustomContextMenu.Items("ResultsDisplayEligibiltyMenuItem").Available = True
                End If

                DataGridCustomContextMenu.Items("ResultViewByClaimIDMenuItem").Enabled = True
                DataGridCustomContextMenu.Items("ResultViewByClaimIDMenuItem").Available = True
            End If

            If DRs?.Count > 1 Then
                DataGridCustomContextMenu.Items("ResultsDisplayDocumentMenuItem").Text = "Display Documents"
                DataGridCustomContextMenu.Items("ResultsDisplayLineDetailsMenuItem").Text = "Line Details (3 Claims Max)"
            End If

            InvalidStatusCount = 0
            ValidStatusCount = 0

            If DRs?.Count > 0 Then
                For Each DR In DRs
                    Select Case DR("STATUS").ToString.ToUpper
                        Case "COMPLETED"
                            ValidStatusCount += 1
                        Case Else
                            InvalidStatusCount += 1
                    End Select

                Next
            End If

            If InvalidStatusCount = 0 AndAlso ValidStatusCount = 1 Then
                DataGridCustomContextMenu.Items("ReprintEOBMenuItem").Available = True
                DataGridCustomContextMenu.Items("ReprintEOBMenuItem").Enabled = True

                InvalidStatusCount = 0
                ValidStatusCount = 0

                If DRs?.Count > 0 Then
                    For Each DR In DRs
                        Select Case DR("PAYEE").ToString.ToUpper
                            Case "1"
                                ValidStatusCount += 1
                            Case Else
                                InvalidStatusCount += 1
                        End Select
                    Next

                End If

                If ValidStatusCount = 1 Then
                    For Each TooStripDropDownMenu As ToolStripDropDownItem In CType(DataGridCustomContextMenu.Items("ResultsDisplayDocumentMenuItem"), ToolStripMenuItem).DropDownItems
                        TooStripDropDownMenu.Enabled = True
                    Next
                Else
                    For Each TooStripDropDownMenu As ToolStripDropDownItem In CType(DataGridCustomContextMenu.Items("ResultsDisplayDocumentMenuItem"), ToolStripMenuItem).DropDownItems
                        TooStripDropDownMenu.Enabled = False
                    Next
                End If
            ElseIf InvalidStatusCount = 0 And ValidStatusCount > 1 Then

                DataGridCustomContextMenu.Items("ReprintEOBMenuItem").Available = True
                DataGridCustomContextMenu.Items("ReprintEOBMenuItem").Enabled = False

            End If

            InvalidStatusCount = 0
            ValidStatusCount = 0

            If DRs?.Count > 0 Then
                For Each DR In DRs
                    Select Case DR("STATUS").ToString.ToUpper
                        Case "WAITPROCESSING", "AUDITING", "AUDIT"
                            ValidStatusCount += 1
                        Case Else
                            InvalidStatusCount += 1
                    End Select

                    Select Case DR("PROCESSED_BY").ToString.ToUpper
                        Case SystemInformation.UserName.ToUpper
                        Case Else
                            NotCurrentUsers += 1
                    End Select

                Next
            End If

            If InvalidStatusCount = 0 AndAlso ValidStatusCount = 1 Then
                DataGridCustomContextMenu.Items("ReprocessMenuItem").Available = True
                DataGridCustomContextMenu.Items("ReprocessMenuItem").Enabled = True
            ElseIf InvalidStatusCount = 0 AndAlso ValidStatusCount > 1 Then
                DataGridCustomContextMenu.Items("ReprocessMenuItem").Available = True
                DataGridCustomContextMenu.Items("ReprocessMenuItem").Enabled = False
            End If

            CType(DataGridCustomContextMenu.Items("ReprocessMenuItem"), ToolStripMenuItem).DropDownItems("ReprocessPTOMenuItem").Available = False
            CType(DataGridCustomContextMenu.Items("ReprocessMenuItem"), ToolStripMenuItem).DropDownItems("ReprocessPTOMenuItem").Enabled = False

            CType(DataGridCustomContextMenu.Items("ReprocessMenuItem"), ToolStripMenuItem).DropDownItems("ReprocessPTCMenuItem").Available = False
            CType(DataGridCustomContextMenu.Items("ReprocessMenuItem"), ToolStripMenuItem).DropDownItems("ReprocessPTCMenuItem").Enabled = False

            If DataGridCustomContextMenu.Items("ReprocessMenuItem").Available Then
                If UFCWGeneralAD.CMSCanReprocess Then
                    CType(DataGridCustomContextMenu.Items("ReprocessMenuItem"), ToolStripMenuItem).DropDownItems("ReprocessPTOMenuItem").Available = True
                    CType(DataGridCustomContextMenu.Items("ReprocessMenuItem"), ToolStripMenuItem).DropDownItems("ReprocessPTOMenuItem").Enabled = True

                    CType(DataGridCustomContextMenu.Items("ReprocessMenuItem"), ToolStripMenuItem).DropDownItems("ReprocessPTCMenuItem").Available = True
                    CType(DataGridCustomContextMenu.Items("ReprocessMenuItem"), ToolStripMenuItem).DropDownItems("ReprocessPTCMenuItem").Enabled = True
                End If

                If UFCWGeneralAD.CMSUsers AndAlso NotCurrentUsers < 1 Then
                    CType(DataGridCustomContextMenu.Items("ReprocessMenuItem"), ToolStripMenuItem).DropDownItems("ReprocessPTOMenuItem").Available = True
                    CType(DataGridCustomContextMenu.Items("ReprocessMenuItem"), ToolStripMenuItem).DropDownItems("ReprocessPTOMenuItem").Enabled = True
                End If

            End If

            If CType(DataGridCustomContextMenu.Items("ReprocessMenuItem"), ToolStripMenuItem).DropDownItems("ReprocessPTOMenuItem").Available = False AndAlso CType(DataGridCustomContextMenu.Items("ReprocessMenuItem"), ToolStripMenuItem).DropDownItems("ReprocessPTCMenuItem").Available = False Then
                DataGridCustomContextMenu.Items("ReprocessMenuItem").Available = False
                DataGridCustomContextMenu.Items("ReprocessMenuItem").Enabled = False
            End If

            InvalidStatusCount = 0
            ValidStatusCount = 0

            If UFCWGeneralAD.CMSCanRemovePricing Then

                If DRs?.Count > 0 Then
                    For Each DR In DRs
                        Select Case DR("STATUS").ToString.ToUpper
                            Case "PRICING"
                                ValidStatusCount += 1
                            Case Else
                                InvalidStatusCount += 1
                        End Select
                    Next
                End If

                If InvalidStatusCount = 0 AndAlso ValidStatusCount = 1 Then
                    DataGridCustomContextMenu.Items("RemovePricingMenuItem").Enabled = True
                    DataGridCustomContextMenu.Items("RemovePricingMenuItem").Available = True
                ElseIf InvalidStatusCount = 0 AndAlso ValidStatusCount > 1 Then
                    DataGridCustomContextMenu.Items("RemovePricingMenuItem").Enabled = False
                    DataGridCustomContextMenu.Items("RemovePricingMenuItem").Available = True
                End If
            End If

            InvalidStatusCount = 0
            ValidStatusCount = 0

            If CustomerServiceResultsDataGrid.DataSource IsNot Nothing AndAlso CType(CustomerServiceResultsDataGrid.DataSource, DataView).Table.Columns.Contains("REOPENABLE") Then

                If UFCWGeneralAD.CMSCanReopenFull Then

                    If DRs?.Count > 0 Then
                        For Each DR In DRs
                            Select Case DR("REOPENABLE").ToString.ToUpper = "YES" AndAlso DR("STATUS").ToString.ToUpper = "COMPLETED"

                                Case True
                                    ValidStatusCount += 1
                                Case Else
                                    InvalidStatusCount += 1
                            End Select
                        Next

                    End If

                    If InvalidStatusCount = 0 AndAlso ValidStatusCount = 1 Then
                        DataGridCustomContextMenu.Items("ReOpenMenuItem").Available = True
                        DataGridCustomContextMenu.Items("ReOpenMenuItem").Enabled = True
                    ElseIf InvalidStatusCount = 0 AndAlso ValidStatusCount > 1 Then
                        DataGridCustomContextMenu.Items("ReOpenMenuItem").Available = True
                        DataGridCustomContextMenu.Items("ReOpenMenuItem").Enabled = False
                    End If

                ElseIf CType(CustomerServiceResultsDataGrid.DataSource, DataView).Table.Columns.Contains("PENDED_TO") AndAlso UFCWGeneralAD.CMSCanReopenPartial Then

                    If DRs?.Count > 0 Then
                        For Each DR In DRs
                            Select Case DR("REOPENABLE").ToString.ToUpper = "YES" AndAlso DR("STATUS").ToString.ToUpper = "COMPLETED" AndAlso DR("PENDED_TO").ToString.Trim.ToUpper = SystemInformation.UserName.ToUpper
                                Case True
                                    ValidStatusCount += 1
                                Case Else
                                    InvalidStatusCount += 1
                            End Select
                        Next

                    End If

                    If InvalidStatusCount = 0 AndAlso ValidStatusCount = 1 Then
                        DataGridCustomContextMenu.Items("ReOpenMenuItem").Available = True
                        DataGridCustomContextMenu.Items("ReOpenMenuItem").Enabled = True
                    ElseIf InvalidStatusCount = 0 AndAlso ValidStatusCount > 1 Then
                        DataGridCustomContextMenu.Items("ReOpenMenuItem").Available = True
                        DataGridCustomContextMenu.Items("ReOpenMenuItem").Enabled = False
                    End If

                End If
            End If

            InvalidStatusCount = 0
            ValidStatusCount = 0

            If UFCWGeneralAD.CMSCanAudit Then

                If DRs?.Count > 0 Then
                    For Each DR In DRs
                        Select Case DR("STATUS").ToString.ToUpper
                            Case "WAITPROCESSING", "AUDITING", "AUDIT"
                                ValidStatusCount += 1
                            Case Else
                                InvalidStatusCount += 1
                        End Select
                    Next

                End If

                If InvalidStatusCount = 0 AndAlso ValidStatusCount > 0 Then
                    DataGridCustomContextMenu.Items("ResultsAuditMenuItem").Enabled = True
                    DataGridCustomContextMenu.Items("ResultsAuditMenuItem").Available = True
                    DataGridCustomContextMenu.Items("ResultsAuditMenuItem").Text = "Send To Audit"
                End If

            End If

        Catch ex As Exception

            Throw

        Finally

#If DEBUG Then
            For Each tsi As ToolStripItem In ResultsDataGridCustomContextMenu.Items
                Debug.Print("Name: " & tsi.Name & " Text: " & tsi.Text & " Visible: " & tsi.Visible.ToString & " Enabled: " & tsi.Enabled.ToString)
            Next
#End If

        End Try

    End Sub

    Private Shared Function ValidateReopenOK(ByVal Reason As String) As Boolean

        Dim ReasonsDS As DataSet
        Dim SelectedDRs As DataRow()

        Try

            ReasonsDS = CMSDALFDBMD.RetrieveReasonValues(UFCWGeneral.NowDate)
            SelectedDRs = ReasonsDS.Tables(0).Select("REASON = '" & Reason & "' AND PENDING_USE_SW = 1")
            If SelectedDRs.Length > 0 Then
                Return True
            End If

        Catch ex As Exception

            Throw

        End Try

    End Function

    Private Sub FamilyRelationMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FamilyRelationMenuItem.Click
        Dim Frm As FamilyList

        Try
            Frm = New FamilyList
            Frm.SetFormInfo(CInt(FamilyIDTextBox.Text))
            Frm.MdiParent = Me.ParentForm.MdiParent
            Frm.Show()

        Catch ex As Exception

            Throw

        Finally
            If Frm IsNot Nothing Then
                Frm.Dispose()
            End If
            Frm = Nothing
        End Try
    End Sub

    Private Sub txtSSN_Validating(sender As Object, e As CancelEventArgs) Handles ParticipantSSNTextBox.Validating, PatientSSNTextBox.Validating

        Dim TBox As TextBox = CType(sender, TextBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ErrorProvider1.ClearError(TBox)

            If TBox.Text.Trim.Length > 0 Then
                If Not UFCWGeneral.ValidateSSN(TBox.Text.Trim) Then
                    ErrorProvider1.SetErrorWithTracking(TBox, "Enter Valid SSN. 9 Digits formatted as 000000000 or 000-00-0000")
                End If
                'If (TBox.Text.Trim.Length <> 9 AndAlso TBox.Text.Trim.Length <> 11) OrElse (Not IsNumeric(TBox.Text)) OrElse (TBox.Text.Trim.Length = 11 AndAlso Not TBox.Text.Contains("-")) Then
                '        ErrorProvider1.SetErrorWithTracking(TBox, "Enter Valid SSN. 000000000 or 000-00-0000")
                '    End If

            End If

            If ErrorProvider1.GetError(TBox).Trim.Length > 0 Then 'are there any errors  
                e.Cancel = True
            ElseIf TBox.Text.Trim.Length > 0 AndAlso TBox.Text <> UFCWGeneral.FormatSSN(TBox.Text) Then
                TBox.Text = UFCWGeneral.FormatSSN(TBox.Text)
            End If

        Catch ex As Exception

            Throw

        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub PatientSSNTextBox_Validated(sender As Object, e As EventArgs) Handles ParticipantSSNTextBox.Validated, PatientSSNTextBox.Validated
        _SSNForMenu = Me.ParticipantSSNTextBox.Text
    End Sub

    Private Sub txtSSN_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ParticipantSSNTextBox.KeyPress, PatientSSNTextBox.KeyPress
        'if typing allow number or dash(-)
        Dim EntryOkRegex As Regex = New Regex("^[0-9\-]+$")

        If Not (System.Char.IsControl(e.KeyChar)) AndAlso Not EntryOkRegex.IsMatch(e.KeyChar) Then
            e.Handled = True
        End If

    End Sub


    Private Sub ProvId_KeyPress(sender As Object, e As KeyPressEventArgs) Handles ProviderIDTextBox.KeyPress
        'if typing allow number or dash(-)
        Dim EntryOkRegex As Regex = New Regex("^[0-9\-]+$")

        If Not (System.Char.IsControl(e.KeyChar)) AndAlso Not EntryOkRegex.IsMatch(e.KeyChar) Then
            e.Handled = True
        End If

    End Sub

    Private Sub ProvTINBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProviderIDTextBox.TextChanged

        Dim TBox As TextBox = CType(sender, TextBox)
        Dim IntCnt As Integer
        Dim StrTmp As String

        If Not IsNumeric(TBox.Text) AndAlso Len(TBox.Text) > 0 Then
            StrTmp = TBox.Text
            For IntCnt = 1 To Len(StrTmp)
                If Not IsNumeric(Mid(StrTmp, IntCnt, 1)) AndAlso Len(StrTmp) > 0 Then
                    StrTmp = Replace(StrTmp, Mid(StrTmp, IntCnt, 1), "")
                End If
            Next
            TBox.Text = StrTmp

        End If

    End Sub
    Private Sub ProvTINTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles ProviderIDTextBox.KeyDown

        Dim strBuffer As String
        Dim stuff As IDataObject = Clipboard.GetDataObject()

        If e.Control OrElse e.Shift Then
            If e.KeyCode = Keys.V OrElse e.KeyCode = Keys.Insert Then
                If (e.Control AndAlso e.KeyCode = Keys.V) OrElse (e.Shift AndAlso e.KeyCode = Keys.Insert) Then
                    e.Handled = True
                    strBuffer = CType(stuff.GetData(DataFormats.Text), String)
                    For IntCnt As Integer = 1 To Len(strBuffer)
                        If Not IsNumeric(Mid(strBuffer, IntCnt, 1)) AndAlso Len(strBuffer) > 0 Then
                            strBuffer = Replace(strBuffer, Mid(strBuffer, IntCnt, 1), "")
                        End If
                    Next

                    ProviderIDTextBox.Text = strBuffer
                End If
            End If
        End If
    End Sub
    'Private Sub Menu_Paste(ByVal sender As System.Object, ByVal e As System.EventArgs)

    '    Dim TBox As TextBox = CType(sender, TextBox)

    '    ' Determine if there is any text in the Clipboard to paste into the text box.
    '    If Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) = True Then
    '        ' Determine if any text is selected in the text box.
    '        If TBox.SelectionLength > 0 Then
    '            ' Ask user if they want to paste over currently selected text.
    '            If MessageBox.Show("Do you want to paste over current selection?", "Cut Example", MessageBoxButtons.YesNo) = DialogResult.No Then
    '                ' Move selection to the point after the current selection and paste.
    '                TBox.SelectionStart = TBox.SelectionStart + TBox.SelectionLength
    '            End If
    '        End If
    '        ' Paste current text in Clipboard into text box.
    '        TBox.Paste()
    '    End If

    'End Sub

    'Private Sub MenuItemPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemPaste.Click

    '    'Dim S As IDataObject
    '    'Dim O() As String
    '    'Dim F() As String
    '    Dim TBox As TextBox = CType(sender, TextBox)

    '    Try

    '        'S = Clipboard.GetDataObject()
    '        'F = S.GetFormats(True)
    '        'O = S.GetFormats(False)

    '        If Clipboard.ContainsText Then

    '            TBox = DirectCast(DirectCast(DirectCast(sender, System.Windows.Forms.ToolStripItem).Owner, ContextMenuStrip).SourceControl, TextBox)
    '            If TBox.Name.Contains("SSN") Then

    '                UFCWGeneral.ClipBoardSSNCleaner()

    '            Else
    '                UFCWGeneral.ClipboardNumberTrimmer()

    '            End If

    '            TBox.Text = Clipboard.GetText

    '        End If

    '    Catch ex As Exception
    '    End Try

    'End Sub

    'Private Sub SSNFormatted_KeyDown(sender As Object, e As KeyEventArgs) Handles ParticipantSSNTextBox.KeyDown, PatientSSNTextBox.KeyDown

    '    If (e.KeyCode = Keys.V AndAlso e.Modifiers = Keys.Control) OrElse (e.KeyCode = Keys.Insert AndAlso e.Modifiers = Keys.Shift) Then

    '        Dim ClipText As String = Clipboard.GetText
    '        If ClipText.Length < 20 Then
    '            UFCWGeneral.ClipBoardSSNCleaner()
    '        Else
    '            e.Handled = True 'this suppresses the actual paste
    '        End If

    '    End If

    'End Sub

    'Private Sub NumberTrimmed_KeyDown(sender As Object, e As KeyEventArgs) Handles FamilyIDTextBox.KeyDown

    '    If (e.KeyCode = Keys.V AndAlso e.Modifiers = Keys.Control) OrElse (e.KeyCode = Keys.Insert AndAlso e.Modifiers = Keys.Shift) Then

    '        Dim ClipText As String = Clipboard.GetText
    '        If ClipText.Length < 20 Then
    '            e.Handled = UFCWGeneral.ClipboardNumberTrimmer()
    '        Else
    '            e.Handled = True
    '        End If

    '    End If

    'End Sub

    Private Function EstablishFamily() As DialogResult
        Dim DistinctFamiliesDT As DataTable
        Dim FamilyListfrm As FamilyList
        Dim FamilyListfrmDR As DialogResult

        Try

            If FamilyIDTextBox.Text.Trim.Length < 1 Then

                Using WC As New GlobalCursor
                    DistinctFamiliesDT = CMSDALFDBMD.RetrievePatientsBySSN(CInt(UFCWGeneral.UnFormatSSN(PatientSSNTextBox.Text)))
                End Using

                If DistinctFamiliesDT IsNot Nothing AndAlso DistinctFamiliesDT.Rows.Count > 1 Then
                    FamilyListfrm = New FamilyList(DistinctFamiliesDT)

                    FamilyListfrmDR = FamilyListfrm.ShowDialog()

                    Select Case FamilyListfrmDR
                        Case DialogResult.OK
                            FamilyIDTextBox.Text = FamilyListfrm.SelectedFamilyID.ToString()
                    End Select

                    Return FamilyListfrmDR

                End If

                Return DialogResult.Cancel

            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    'Private Function ValidateDocID() As Boolean
    '    If IsNumeric(Me.DocIDTextBox.Text) Then
    '    Else
    '        If DocIDTextBox.Text.Length > 0 Then
    '            ErrorProvider1.SetErrorWithTracking(DocIDTextBox, "This is not a proper Document ID")
    '            MessageBox.Show("This is not a Document ID", "Improper DocID", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Return False
    '        End If
    '    End If

    '    Return True

    'End Function

    Private Function ValidateParticipantSSN() As Boolean

        If IsNumeric(ParticipantSSNTextBox.Text) Then
            ParticipantSSNTextBox.Text = UFCWGeneral.FormatSSN(ParticipantSSNTextBox.Text)
        Else
            If ParticipantSSNTextBox.Text.Length > 0 Then
                If Not IsNumeric(UFCWGeneral.UnFormatSSN(ParticipantSSNTextBox.Text)) Then
                    ErrorProvider1.SetErrorWithTracking(ParticipantSSNTextBox, "This is not a proper SSN or Encrypted SSN")
                    MessageBox.Show("This is not a proper SSN or Encrypted SSN", "Improper SSN", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            End If
        End If

        Return True

    End Function

    Private Sub FamilyIdTextBox_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles FamilyIDTextBox.MouseUp
        FamilyIDTextBox.Focus()
    End Sub

    Private Sub ProcCodeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProcCodeButton.Click
        '' -----------------------------------------------------------------------------
        '' <summary>
        '' opens a procedure code selection dialog
        '' </summary>
        '' <param name="sender"></param>
        '' <param name="e"></param>
        '' <remarks>
        '' </remarks>
        '' <history>
        '' 	[Nick Snyder]	1/25/2007	Created
        ''     [malkoi] 7/18/2007 Pulled this code from Work, credit goes to Nick!
        '' </history>
        '' -----------------------------------------------------------------------------

        Try
            SelectProcedures(ProcedureCodeTextBox.Text)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub SelectProcedures(txtProcedures As String)

        Dim ProcedureCodesDT As DataTable

        Dim DR As DataRow

        Try

            ProcedureCodesDT = New DataTable("Procedures")
            ProcedureCodesDT.Columns.Add("PROC_VALUE")
            ProcedureCodesDT.Columns.Add("FULL_DESC")
            ProcedureCodesDT.Columns.Add("Priority", System.Type.GetType("System.Int32"))

            Dim NoCommaText As String = txtProcedures.ToUpper.Replace(",", " ")
            Dim ProceduresAL As String() = NoCommaText.Split(New Char() {CChar(" ")}, StringSplitOptions.RemoveEmptyEntries)
            Dim Priority As Short = 0

            For Each Procedure In ProceduresAL
                DR = ProcedureCodesDT.NewRow
                DR("PROC_VALUE") = Procedure
                DR("FULL_DESC") = ""
                DR("Priority") = Priority

                ProcedureCodesDT.Rows.Add(DR)
                Priority += 1S
            Next

            ProcedureCodesDT.AcceptChanges()
            Using FRM As ProcedureCodesForm = New ProcedureCodesForm(ProcedureCodesDT, False)

                If FRM.ShowDialog(Me) = DialogResult.OK Then
                    ProcedureCodesDT = FRM.SelectedProcedures
                End If

            End Using

            If ProcedureCodesDT IsNot Nothing AndAlso ProcedureCodesDT.Rows.Count > 0 Then
                Dim FlattenProcedureCodesQuery = String.Join(",", ProcedureCodesDT.AsEnumerable.Select(Function(p) p("PROC_VALUE").ToString()))
                Dim FlattenProcedureDescQuery = String.Join(vbCrLf, ProcedureCodesDT.AsEnumerable.Select(Function(p) p("FULL_DESC").ToString()))

                If FlattenProcedureCodesQuery.Any Then
                    ProcedureCodeTextBox.Text = FlattenProcedureCodesQuery
                    ToolTip1.SetToolTip(ProcedureCodeTextBox, FlattenProcedureDescQuery)
                End If
            End If

        Catch ex As Exception

            Throw

        Finally

        End Try

        'Dim Codes As String = ""
        'Const Comma As Char = ","c
        'Dim ProcCodes() As String

        'Try
        '    Using FRM As ProcedureCodesForm = New ProcedureCodesForm(False)

        '        If FRM.ShowDialog(Me) = DialogResult.OK Then
        '            For cnt As Integer = 0 To FRM.ProcedureCodesDataGrid.GetGridRowCount - 1
        '                If FRM.ProcedureCodesDataGrid.IsSelected(cnt) = True Then
        '                    Codes &= If(Codes <> "", ",", "") & FRM.ProcedureCodesDataGrid.Item(cnt, CInt(FRM.ProcedureCodesDataGrid.GetColumnPosition("PROC_VALUE"))).ToString
        '                End If
        '            Next
        '            ProcCodes = Codes.Split(Comma)
        '            If ProcCodes.Length > 1 Then
        '                ProcedureCodeTextBox.Text = ProcCodes(0)
        '                ProcedureCodeToTextBox.Text = ProcCodes(ProcCodes.Length - 1)
        '            Else
        '                ProcedureCodeTextBox.Text = ProcCodes(0)
        '            End If
        '        End If
        '    End Using

        'Catch ex As Exception
        '    
        '    If (Rethrow) Then
        '        Throw
        '    Else
        '        MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    End If
        'Finally
        'End Try

    End Sub

    Private Sub SelectDiagnoses(txtDiagnoses As String)

        Dim DiagnosisCodesDT As DataTable

        Dim DR As DataRow

        Try

            DiagnosisCodesDT = New DataTable("Diagnoses")
            DiagnosisCodesDT.Columns.Add("DIAG_VALUE")
            DiagnosisCodesDT.Columns.Add("FULL_DESC")
            DiagnosisCodesDT.Columns.Add("Priority", System.Type.GetType("System.Int32"))

            Dim NoCommaText As String = txtDiagnoses.ToUpper.Replace(",", " ")
            Dim DiagnosisAL As String() = NoCommaText.Split(New Char() {CChar(" ")}, StringSplitOptions.RemoveEmptyEntries)
            Dim Priority As Short = 0

            For Each Diag In DiagnosisAL
                DR = DiagnosisCodesDT.NewRow
                DR("DIAG_VALUE") = Diag
                DR("FULL_DESC") = ""
                DR("Priority") = Priority

                DiagnosisCodesDT.Rows.Add(DR)
                Priority += 1S
            Next

            DiagnosisCodesDT.AcceptChanges()
            Using FRM As DiagnosisCodesForm = New DiagnosisCodesForm(DiagnosisCodesDT, False)

                If FRM.ShowDialog(Me) = DialogResult.OK Then
                    DiagnosisCodesDT = FRM.SelectedDiagnoses
                End If

            End Using

            If DiagnosisCodesDT IsNot Nothing AndAlso DiagnosisCodesDT.Rows.Count > 0 Then
                Dim FlattenDiagnosisCodesQuery = String.Join(",", DiagnosisCodesDT.AsEnumerable.Select(Function(p) p("DIAG_VALUE").ToString()))
                Dim FlattenDiagnosisDescQuery = String.Join(vbCrLf, DiagnosisCodesDT.AsEnumerable.Select(Function(p) p("FULL_DESC").ToString()))

                If FlattenDiagnosisCodesQuery.Any Then
                    DiagnosisCodesTextBox.Text = FlattenDiagnosisCodesQuery
                    ToolTip1.SetToolTip(DiagnosisCodesTextBox, FlattenDiagnosisDescQuery)
                End If
            End If

        Catch ex As Exception

            Throw

        Finally

        End Try

    End Sub

    Private Sub ModifiersButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ModifiersButton.Click
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' opens a modifier dialog to select modifiers
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	1/25/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim ModifierCodeLookUp As ModifierLookup

        Try
            ModifierCodeLookUp = New ModifierLookup

            If ModifierCodeLookUp.ShowDialog(Me) = DialogResult.OK Then
                For Cnt As Integer = 0 To ModifierCodeLookUp.ModifierDataGrid.GetGridRowCount - 1
                    If ModifierCodeLookUp.ModifierDataGrid.IsSelected(Cnt) = True Then
                        ModifierTextBox.Text = CStr(ModifierCodeLookUp.ModifierDataGrid.Item(Cnt, CInt(ModifierCodeLookUp.ModifierDataGrid.GetColumnPosition("MODIFIER_VALUE"))))
                    End If
                Next
            End If

        Catch ex As Exception

            Throw

        Finally
            ModifierCodeLookUp.Dispose()
            ModifierCodeLookUp = Nothing
        End Try

    End Sub

    Private Sub DiagnosisButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiagnosisButton.Click

        Try
            SelectDiagnoses(DiagnosisCodesTextBox.Text)

        Catch ex As Exception
            Throw
        End Try

        'Const Comma As Char = ","c
        'Dim Codes As String = ""
        'Dim Diagcodes() As String
        'Dim DiagnosisCodeLookUp As DiagnosisCodeLookup

        'Try
        '    DiagnosisCodeLookUp = New DiagnosisCodeLookup

        '    If DiagnosisCodeLookUp.ShowDialog(Me) = DialogResult.OK Then
        '        For cnt As Integer = 0 To DiagnosisCodeLookUp.DiagnosisCodesLookupDataGrid.GetGridRowCount - 1
        '            If DiagnosisCodeLookUp.DiagnosisCodesLookupDataGrid.IsSelected(cnt) = True Then
        '                Codes &= If(Codes <> "", ", ", "") & DiagnosisCodeLookUp.DiagnosisCodesLookupDataGrid.Item(cnt, CInt(DiagnosisCodeLookUp.DiagnosisCodesLookupDataGrid.GetColumnPosition("DIAG_VALUE"))).ToString
        '            End If
        '        Next

        '        Diagcodes = Codes.Split(Comma)

        '        If Diagcodes.Length > 1 Then
        '            DiagnosisCodesTextBox.Text = Diagcodes(0)
        '            'DiagnosisToTextBox.Text = Diagcodes(Diagcodes.Length - 1)
        '        Else
        '            DiagnosisCodesTextBox.Text = Diagcodes(0)
        '        End If
        '    End If

        'Catch ex As Exception
        '    
        '    If (Rethrow) Then
        '        Throw
        '    Else
        '        MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    End If
        'Finally
        '    DiagnosisCodeLookUp.Dispose()
        '    DiagnosisCodeLookUp = Nothing
        'End Try

    End Sub

    Private Sub SearchButton_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchButton.MouseHover

        ToolTip1.SetToolTip(SearchButton, "")

        If CBool(Control.ModifierKeys And Keys.Control) AndAlso CMSDALFDBMD.LastSQL IsNot Nothing Then
            Clipboard.SetDataObject(CMSDALFDBMD.LastSQL)
            ToolTip1.SetToolTip(SearchButton, CMSDALFDBMD.LastSQL)
        ElseIf CBool(Control.ModifierKeys And Keys.Shift) AndAlso CMSDALFDBMD.LastSQL IsNot Nothing Then
            ToolTip1.SetToolTip(SearchButton, CMSDALFDBMD.LastSQL)
        Else
            Me.ToolTip1.SetToolTip(Me.SearchButton, "Search for specified criteria.")
        End If

    End Sub

    Private Sub ProviderLookUpButtons_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProviderIDLookupButton.Click, ProviderNameLookupButton.Click

        Dim Frm As ProviderLookUpForm

        Try

            If ProviderNameTextBox.Text <> "" Then
                Frm = New ProviderLookUpForm(ProviderNameTextBox.Text)
            Else
                Frm = New ProviderLookUpForm()
            End If

            If Frm.ShowDialog(Me) = DialogResult.OK Then

                ClearAll()

                Me.ProviderIDTextBox.Text = If(IsNumeric(Frm.ProviderID) AndAlso CInt(Frm.ProviderID) > 0, Frm.ProviderID.ToString, Format(Frm.ProvTIN, "000000000").ToString)

            End If

            ProviderNameTextBox.Text = ""

        Catch ex As Exception

            Throw

        Finally
            Frm.Dispose()
            Frm = Nothing
        End Try
    End Sub

    Private Function LoadDemographics(ByVal patientsDS As DataSet) As Boolean

        Dim AddressDR As DataRow
        Dim ParticipantDemographicsDS As DataSet
        Dim DualParticipantDemographics As DataSet

        Try
            RemoveHandler _ParticipantFamilyBS.PositionChanged, AddressOf ParticipantFamilyBindingSource_CurrentChanged

            ParticipantDemographicsGrid.DataSource = Nothing
            DualParticipantDemographicsGrid.DataSource = Nothing

            Me.AddressLabel.Text = ""
            Me.ParticipantAlertLabel.Text = ""
            Me.ParticipantHighAlertLabel.Text = ""

            Me.DualParticipantAddressLabel.Text = ""
            Me.DualParticipantAlertLabel.Text = ""
            Me.DualParticipantHighAlertLabel.Text = ""

            patientsDS.Tables(0).TableName = "PARTICIPANTFAMILY"

            If patientsDS.Tables.Count > 1 Then
                patientsDS.Tables(1).TableName = "DUALCOVERAGEFAMILY"
            End If

            Using WC As New GlobalCursor

                ParticipantDemographicsDS = CMSDALFDBMD.RetrieveRegaddressAndAlerts(CType(patientsDS.Tables("PARTICIPANTFAMILY").Rows(0)("FAMILY_ID"), Integer?))
                ParticipantDemographicsDS.Tables(0).TableName = "ADDRESSES"
                ParticipantDemographicsDS.Tables(1).TableName = "PARTICIPANT_ALERTS"
                ParticipantDemographicsDS.Tables(2).TableName = "PATIENTS_ELIGIBILITY"

                patientsDS.Merge(ParticipantDemographicsDS.Tables(0))
                patientsDS.Merge(ParticipantDemographicsDS.Tables(2))

                _ParticipantFamilyBS.DataSource = patientsDS
                _ParticipantFamilyBS.DataMember = "PARTICIPANTFAMILY"

                AddHandler _ParticipantFamilyBS.CurrentChanged, AddressOf ParticipantFamilyBindingSource_CurrentChanged

                ParticipantDemographicsGrid.DataSource = _ParticipantFamilyBS

                SetTableStyle(ParticipantDemographicsGrid, FamilyInfoDataGridContextMenuStrip, "CustomerServiceDemographicsDataGrid")
                ParticipantDemographicsGrid.Sort = If(ParticipantDemographicsGrid.LastSortedBy, ParticipantDemographicsGrid.DefaultSort("CustomerServiceDemographicsDataGrid"))

                ParticipantFamilyBindingSource_CurrentChanged(_ParticipantFamilyBS, New EventArgs)

                Dim QueryParticipantAlerts =
                    From Alerts In ParticipantDemographicsDS.Tables("PARTICIPANT_ALERTS").AsEnumerable()
                    Where (CInt(Alerts("FAMILY_ALERT")) = 1 OrElse CInt(Alerts("RELATION_ID")) = CInt(patientsDS.Tables("PARTICIPANTFAMILY").Rows(0)("RELATION_ID")))

                For Each DR In QueryParticipantAlerts
                    Select Case DR("HIGHLIGHTED_ALERT").ToString
                        Case "0"
                            Me.ParticipantAlertLabel.Text &= If(Me.ParticipantAlertLabel.Text.Trim.Length > 0, " \ ", "") & DR("ALERT_REASON_DESC").ToString & If(DR("PASSPHRASE").ToString.Trim.Length > 0, " :  ( " & DR("PASSPHRASE").ToString.Trim & " )", "")
                        Case Else 'highlight requested
                            Me.ParticipantHighAlertLabel.Text &= If(Me.ParticipantHighAlertLabel.Text.Trim.Length > 0, " \ ", "") & DR("ALERT_REASON_DESC").ToString & If(DR("PASSPHRASE").ToString.Trim.Length > 0, " : ( " & DR("PASSPHRASE").ToString.Trim & " )", "")
                    End Select
                Next

                If Me.ParticipantHighAlertLabel.Text.Trim.Length > 0 AndAlso Me.ParticipantAlertLabel.Text.Trim.Length > 0 Then
                    Me.ParticipantAlertLabel.Text = " \ " & Me.ParticipantAlertLabel.Text
                End If
                DemographicsTab.Text = " FAMILY INFO "

                If patientsDS.Tables.Contains("DUALCOVERAGEFAMILY") Then

                    DualParticipantDemographicsGrid.DataSource = patientsDS
                    DualParticipantDemographicsGrid.DataMember = "DUALCOVERAGEFAMILY"

                    SetTableStyle(DualParticipantDemographicsGrid, FamilyInfoDataGridContextMenuStrip, "CustomerServiceDemographicsDataGrid")

                    DualParticipantDemographicsGrid.Sort = If(DualParticipantDemographicsGrid.LastSortedBy, DualParticipantDemographicsGrid.DefaultSort("CustomerServiceDemographicsDataGrid"))

                    DualParticipantDemographics = CMSDALFDBMD.RetrieveRegaddressAndAlerts(CType(patientsDS.Tables("PARTICIPANTFAMILY").Rows(0)("FAMILY_ID"), Integer?))

                    Dim QueryDualParticipant =
                        From Addresses In DualParticipantDemographics.Tables(0).AsEnumerable()
                        Where (CInt(Addresses("RELATION_ID")) = 0)

                    If QueryDualParticipant.Any() Then AddressDR = QueryDualParticipant.First

                    If AddressDR IsNot Nothing Then Me.DualParticipantAddressLabel.Text += "Participant Address: " & CType(AddressDR("ADDRESS"), String)

                    Dim QueryDualParticipantAlerts =
                        From Alerts In DualParticipantDemographics.Tables(1).AsEnumerable()
                        Where (CInt(Alerts("FAMILY_ALERT")) = 1 OrElse CInt(Alerts("RELATION_ID")) = CInt(patientsDS.Tables("PARTICIPANTFAMILY").Rows(0)("RELATION_ID")))

                    For Each DR In QueryDualParticipantAlerts
                        Select Case DR("HIGHLIGHTED_ALERT").ToString
                            Case "0"
                                Me.DualParticipantAlertLabel.Text &= If(Me.DualParticipantAlertLabel.Text.Trim.Length > 0, " \ ", "") & DR("ALERT_REASON_DESC").ToString & If(DR("PASSPHRASE").ToString.Trim.Length > 0, " : ( " & DR("PASSPHRASE").ToString.Trim & " )", "")
                            Case Else 'highlight requested
                                Me.DualParticipantHighAlertLabel.Text &= If(Me.DualParticipantHighAlertLabel.Text.Trim.Length > 0, " \ ", "") & DR("ALERT_REASON_DESC").ToString & If(DR("PASSPHRASE").ToString.Trim.Length > 0, " : ( " & DR("PASSPHRASE").ToString.Trim & " )", "")
                        End Select
                    Next

                    If Me.DualParticipantHighAlertLabel.Text.Trim.Length > 0 AndAlso Me.DualParticipantAlertLabel.Text.Trim.Length > 0 Then
                        Me.DualParticipantAlertLabel.Text = " \ " & Me.DualParticipantAlertLabel.Text
                    End If

                    Me.SplitContainer2.Panel2Collapsed = False
                    DemographicsTab.Text = "   FAMILY INFO (DUAL Cov.)   "

                    Return True
                Else
                    Me.SplitContainer2.Panel2Collapsed = True
                    Return False
                End If

            End Using

            ''To display address in tab

        Catch ex As Exception

            Throw

        End Try
    End Function

    Private Sub ParticipantFamilyBindingSource_CurrentChanged(sender As Object, e As System.EventArgs)

        Dim BS As BindingSource

        Dim AddressDR As DataRow
        Dim DR As DataRow
        Dim DS As DataSet

        Try

            BS = CType(sender, BindingSource)

            If BS.Current Is Nothing Then Return

            DR = CType(BS.Current, DataRowView).Row

            Dim QueryParticipantAddressquery =
                    From Addresses In CType(BS.DataSource, DataSet).Tables("ADDRESSES").AsEnumerable()
                    Where (CInt(Addresses("RELATION_ID")) = CInt(DR("RELATION_ID")) OrElse CInt(Addresses("RELATION_ID")) = 0) _
                    AndAlso CInt(Addresses("ADDRESS_TYPE")) < 2
                    Order By Addresses("RELATION_ID") Descending

            If QueryParticipantAddressquery.Any() Then AddressDR = QueryParticipantAddressquery.First

            If AddressDR IsNot Nothing Then Me.AddressLabel.Text = If(CInt(AddressDR("RELATION_ID")) = 0, "Participant ", "Dependent ") & AddressDR("NAME").ToString & " Address: " & AddressDR("ADDRESS").ToString

            DS = CType(BS.DataSource, DataSet)

            Select Case DS.Tables("PATIENTS_ELIGIBILITY").Select("RELATION_ID = " & DR("RELATION_ID").ToString).Length
                Case Is > 0
                    EligibilityAlertLabel.Text = "Eligible"
                    EligibilityAlertLabel.ForeColor = Color.DarkGreen
                Case Else
                    EligibilityAlertLabel.Text = "Not Eligible"
                    EligibilityAlertLabel.ForeColor = Color.DarkRed
            End Select

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub TableStyleReset(ByVal sender As Object, e As EventArgs)
        Dim dg As DataGridCustom = CType(sender, DataGridCustom)
        SetTableStyleColumns(dg, dg.StyleName)
    End Sub

    Private Shared Sub SetTableStyle(ByVal dg As DataGridCustom, ByVal assignedGridName As String) 'called when no context menu is in use, but editflag is supplied
        dg.SetTableStyle()
    End Sub

    Private Sub SetTableStyle(ByVal dg As DataGridCustom, ByVal dataGridCustomContextMenu As System.Windows.Forms.ContextMenuStrip, ByVal xmlStyleName As String)

        Try

            SetTableStyleColumns(dg, xmlStyleName)

            dg.StyleName = xmlStyleName
            dg.ContextMenuPrepare(dataGridCustomContextMenu)

            RemoveHandler dg.ResetTableStyle, AddressOf TableStyleReset
            AddHandler dg.ResetTableStyle, AddressOf TableStyleReset

        Catch ex As Exception

            Throw

        End Try

    End Sub

    Private Sub SetTableStyleColumns(dg As DataGridCustom, ByRef xmlName As String)

        Dim DGTS As DataGridTableStyle
        Dim DGTSDefault As DataGridTableStyle

        Dim TextCol As DataGridFormattableTextBoxColumn
        Dim BoolCol As DataGridColorBoolColumn

        Dim ColsDV As DataView
        Dim DefaultStyleDS As DataSet
        Dim ResultDT As DataTable
        Dim ColumnSequenceDV As DataView

        Dim IntCol As Integer

        Try

            DefaultStyleDS = DataGridCustom.GetTableStyle(xmlName)

            If DefaultStyleDS Is Nothing OrElse DefaultStyleDS.Tables.Count < 1 Then Return

            DGTS = New DataGridTableStyle()

            If dg.GetCurrentDataTable Is Nothing Then
                DGTS.MappingName = dg.Name
            Else
                DGTS.MappingName = dg.GetCurrentDataTable.TableName
            End If

            DGTS.GridColumnStyles.Clear()

            If DefaultStyleDS.Tables.Contains(xmlName & "Style") Then
                If DefaultStyleDS.Tables(xmlName & "Style").Columns.Contains("GridLineStyle") Then
                    DGTS.GridLineStyle = If(CBool(DefaultStyleDS.Tables(xmlName & "Style").Rows(0)("GridLineStyle")), DataGridLineStyle.Solid, DataGridLineStyle.None)
                End If
                If DefaultStyleDS.Tables(xmlName & "Style").Columns.Contains("RowHeadersVisible") Then
                    DGTS.RowHeadersVisible = CBool(DefaultStyleDS.Tables(xmlName & "Style").Rows(0)("RowHeadersVisible"))
                End If
            End If

            ResultDT = dg.LoadColumnsSizeAndPosition(dg.Name & "\" & DGTS.MappingName & "\ColumnSettings", DefaultStyleDS.Tables("Column"))

            ColumnSequenceDV = New DataView(ResultDT)
            ColsDV = ColumnSequenceDV

            DGTSDefault = New DataGridTableStyle() 'This can be used to establish the columns displayed by default
            DGTSDefault.MappingName = "Default"

            For IntCol = 0 To ColsDV.Count - 1

                If Not UFCWGeneralAD.CMSCanReprocess AndAlso Not UFCWGeneralAD.CMSEligibility Then
                    Select Case ColsDV(IntCol).Item("Mapping").ToString
                        Case "PENDED_TO", "PROCESSED_BY"
                            Continue For
                    End Select
                End If

                If Not UFCWGeneralAD.CMSCanRunReports Then
                    Select Case ColsDV(IntCol).Item("Mapping").ToString
                        Case "EMPLOYEE"
                            Continue For
                    End Select
                End If

                If (IsDBNull(ColsDV(IntCol).Item("Visible")) OrElse ColsDV(IntCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(IntCol).Item("Visible")) = True) Then
                    TextCol = New DataGridFormattableTextBoxColumn(IntCol)
                    TextCol.MappingName = CStr(ColsDV(IntCol).Item("Mapping"))
                    TextCol.HeaderText = CStr(ColsDV(IntCol).Item("HeaderText"))

                    If IsDBNull(ColsDV(IntCol).Item("Format")) = False AndAlso ColsDV(IntCol).Item("Format").ToString.Trim.Length > 0 Then
                        TextCol.Format = CStr(ColsDV(IntCol).Item("Format"))
                    End If

                    DGTSDefault.GridColumnStyles.Add(TextCol)
                End If

                If ((IsDBNull(ColsDV(IntCol).Item("Visible")) OrElse ColsDV(IntCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(IntCol).Item("Visible")) = True) AndAlso (GetAllSettings(_APPKEY, dg.Name & "\" & DGTS.MappingName.ToString & "\Customize\ColumnSelector") Is Nothing OrElse CDbl(GetSetting(_APPKEY, dg.Name & "\" & DGTS.MappingName.ToString & "\Customize\ColumnSelector", "Col " & ColsDV(IntCol).Item("Mapping").ToString & " Customize", "0")) = 1)) Then
                    If ColsDV(IntCol).Item("Type").ToString = "Text" Then
                        TextCol = New DataGridFormattableTextBoxColumn(IntCol)
                        TextCol.MappingName = CStr(ColsDV(IntCol).Item("Mapping"))
                        TextCol.HeaderText = CStr(ColsDV(IntCol).Item("HeaderText"))
                        TextCol.Width = CInt(ColsDV(IntCol).Item("SizeInPixels"))
                        TextCol.NullText = CStr(ColsDV(IntCol).Item("NullText"))
                        TextCol.TextBox.WordWrap = True

                        Try

                            If Not IsDBNull(ColsDV(IntCol).Item("ReadOnly")) AndAlso ColsDV(IntCol).Item("ReadOnly").ToString.Trim.Length > 0 AndAlso CBool(ColsDV(IntCol).Item("ReadOnly")) = True Then
                                TextCol.ReadOnly = True
                            End If

                        Catch IgnoreException As Exception
                        End Try

                        If IsDBNull(ColsDV(IntCol).Item("Format")) = False Then
                            If ColsDV(IntCol).Item("Format").ToString = "YesNo" Then
                                AddHandler TextCol.Formatting, AddressOf DataGridCustom.FormattingYesNo
                            ElseIf ColsDV(IntCol).Item("Format").ToString.Trim.Length > 0 Then
                                TextCol.Format = CStr(ColsDV(IntCol).Item("Format"))
                            End If
                        End If

                        If DGTS.MappingName.Contains("PARTICIPANT") Then AddHandler TextCol.SetCellFormat, AddressOf HighlightGridCell
                        If DGTS.MappingName.Contains("DUALCOVERAGE") Then AddHandler TextCol.SetCellFormat, AddressOf HighlightGridCell

                        DGTS.GridColumnStyles.Add(TextCol)

                    ElseIf ColsDV(IntCol).Item("Type").ToString.Contains("Bool") Then
                        BoolCol = New DataGridColorBoolColumn(IntCol)
                        BoolCol.MappingName = CStr(ColsDV(IntCol).Item("Mapping"))
                        BoolCol.HeaderText = CStr(ColsDV(IntCol).Item("HeaderText"))
                        BoolCol.Width = CInt(ColsDV(IntCol).Item("SizeInPixels"))
                        BoolCol.NullValue = If(IsNumeric(ColsDV(IntCol).Item("NullText").ToString.Trim), CDec(ColsDV(IntCol).Item("NullText")), 0)
                        BoolCol.TrueValue = CType("1", Decimal)
                        BoolCol.FalseValue = CType("0", Decimal)
                        BoolCol.AllowNull = True

                        Try

                            If Not IsDBNull(ColsDV(IntCol).Item("ReadOnly")) AndAlso ColsDV(IntCol).Item("ReadOnly").ToString.Trim.Length > 0 AndAlso CBool(ColsDV(IntCol).Item("ReadOnly")) = True Then
                                BoolCol.ReadOnly = True
                            End If

                        Catch IgnoreException As Exception
                        End Try

                        If DGTS.MappingName.Contains("PARTICIPANT") Then AddHandler BoolCol.SetCellFormat, AddressOf HighlightGridCell
                        If DGTS.MappingName.Contains("DUALCOVERAGE") Then AddHandler BoolCol.SetCellFormat, AddressOf HighlightGridCell

                        DGTS.GridColumnStyles.Add(BoolCol)

                    End If
                End If

            Next

            dg.TableStyles.Clear()
            dg.TableStyles.Add(DGTS)
            dg.TableStyles.Add(DGTSDefault)

        Catch ex As Exception

            Throw

        Finally
            DGTS = Nothing
        End Try


    End Sub

    Private Shared Sub FormattingByMask(ByRef value As Object, rowNum As Integer)
        Try

            Select Case True
                Case value.GetType Is GetType(System.String)
                    If IsDBNull(value) = False AndAlso CStr(value) = "A" Then
                        value = "Yes"
                    Else
                        value = "No"
                    End If
                Case Else
                    If IsDBNull(value) = False AndAlso (CInt(value) = 1 OrElse CInt(value) = -1) Then
                        value = "Yes"
                    Else
                        value = "No"
                    End If
            End Select

        Catch ex As Exception

            Throw

        End Try
    End Sub

    Private Sub PopulateClaimAccumulatorForm()

        If _SearchResultsDT Is Nothing Then Return

        Dim AccumFName As String = ""
        Dim AccumLName As String = ""

        Dim DV As DataView
        Dim TheFamilyID As Integer? = Nothing
        Dim TheRelationID As Short? = Nothing
        Dim TheClaimID As String
        Dim DOS As Date?

        Try

            If Me.ClaimIDTextBox.Text.Trim.Length > 0 AndAlso _SearchResultsDT.Rows.Count > 0 Then
                If Me.ClaimIDTextBox.Text.Trim.IsInteger() Then 'ClaimID search
                    DV = New DataView(_SearchResultsDT, "CLAIM_ID=" & Me.ClaimIDTextBox.Text.Trim, "", DataViewRowState.CurrentRows)
                ElseIf Not IsNumeric(Me.ClaimIDTextBox.Text.Trim.Substring(0, 1)) Then 'Maxid search
                    DV = New DataView(_SearchResultsDT, "MAXID='" & Me.ClaimIDTextBox.Text.Trim & "'", "", DataViewRowState.CurrentRows)
                Else 'DCN Search
                    DV = New DataView(_SearchResultsDT, "REFERENCE_ID='" & Me.ClaimIDTextBox.Text.Trim & "'", "", DataViewRowState.CurrentRows)
                End If

                TheFamilyID = UFCWGeneral.IsNullIntegerHandler(DV(0)("FAMILY_ID"))
                TheRelationID = UFCWGeneral.IsNullShortHandler(DV(0)("RELATION_ID"))

                TheClaimID = CStr(DV(0)("CLAIM_ID"))
                DOS = CType(If(DV(0)("OCC_FROM_DATE") Is System.DBNull.Value, Nothing, DV(0)("OCC_FROM_DATE")), Date?)
                AccumFName = CType(DV(0)("PAT_FNAME"), String)
                AccumLName = CType(DV(0)("PAT_LNAME"), String)
            End If

            If TheFamilyID.HasValue Then
                Using WC As New GlobalCursor

                    TabControl.SelectedTab.Controls.Add(AccumulatorValues)

                    If AccumulatorValues.FamilyID <> TheFamilyID.Value OrElse AccumulatorValues.LoadID = 0 Then
                        With AccumulatorValues
                            .ShowClaimView = True
                            .ShowHistory = True
                            .SetFormInfo(CInt(TheFamilyID), CShort(TheRelationID), DOS, CInt(TheClaimID))
                            .LoadID = 1
                            .Show()
                        End With
                    End If
                End Using

                AccumulatorValues.AccumulatorPatientName.Text = AccumLName & ", " & AccumFName & " (" & AccumulatorValues.FamilyID & ")"

                AccumulatorValues.AccumulatorPatientName.Visible = True
                AccumulatorValues.FamilyAccumulatorsDataGrid.Visible = True
                AccumulatorValues.FamilyAccumulatorsCurrentLabel.Visible = True
                AccumulatorValues.FamilyAccumulatorsPriorDataGrid.Visible = True
                AccumulatorValues.FamilyAccumulatorsPriorLabel.Visible = True
                AccumulatorValues.PersonalAccumulatorCurrentLabel.Visible = True
                AccumulatorValues.PersonalAccumulatorDataGrid.Visible = True
                AccumulatorValues.PersonalAccumulatorPriorDataGrid.Visible = True
                AccumulatorValues.PersonalAccumulatorPriorLabel.Visible = True

                AccumulatorValues.Width = AccumulatorValues.Parent.Width
                AccumulatorValues.Height = AccumulatorValues.Parent.Height

            End If

        Catch ex As Exception

            Throw

        Finally

            Me.SearchingLabel.Visible = False

        End Try
    End Sub

    Private Sub PopulateAccumulatorForm()

        Dim AccumFName As String = ""
        Dim AccumLName As String = ""

        Dim DV As DataView
        Dim DR As DataRow

        Dim NameText As String

        Try

            If _SearchResultsDT Is Nothing Then Return

            Using WC As New GlobalCursor

                AccumulatorsSplitContainer.Panel1.Controls.Add(AccumulatorValues)

                DV = _PatientsDS.Tables("PARTICIPANTFAMILY").DefaultView

                DV.RowFilter = "RELATION_ID = " & If(Me.RelationIDTextBox.Text <> "", CInt(Me.RelationIDTextBox.Text), 0)
                If DV.Count > 0 Then
                    DR = DV.Item(0).Row
                End If

                If DR IsNot Nothing Then

                    AccumFName = DR("FIRST_NAME").ToString
                    AccumLName = DR("LAST_NAME").ToString

                    If AccumulatorValues.FamilyID <> CInt(DR("FAMILY_ID")) OrElse AccumulatorValues.LoadID = 0 Then

                        With AccumulatorValues
                            .ShowClaimView = False
                            .ShowHistory = True
                            .SetFormInfo(CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")))
                            .ShowLineDetails = False
                            .LoadID = 1
                            .Show()
                        End With
                    End If

                    If AccumFName <> "" And AccumLName <> "" Then NameText = AccumLName & ", " & AccumFName & " (" & AccumulatorValues.FamilyID & ", " & AccumulatorValues.RelationID & ")"

                    AccumulatorValues.AccumulatorPatientName.Text = NameText
                    AccumulatorValues.FamilyAccumulatorsDataGrid.Visible = True
                    AccumulatorValues.FamilyAccumulatorsCurrentLabel.Visible = True
                    AccumulatorValues.FamilyAccumulatorsPriorDataGrid.Visible = True
                    AccumulatorValues.FamilyAccumulatorsPriorLabel.Visible = True
                    AccumulatorValues.PersonalAccumulatorCurrentLabel.Visible = True
                    AccumulatorValues.PersonalAccumulatorDataGrid.Visible = True
                    AccumulatorValues.PersonalAccumulatorPriorDataGrid.Visible = True
                    AccumulatorValues.PersonalAccumulatorPriorLabel.Visible = True

                    AccumulatorValues.Width = AccumulatorValues.Parent.Width
                    AccumulatorValues.Height = AccumulatorValues.Parent.Height

                Else
                    AccumulatorValues.Visible = False
                End If

                Me.AccumulatorsSplitContainer.Panel2Collapsed = True

                AccumulatorsTab.Text = " ACCUMULATORS "

                If _PatientsDS.Tables.Contains("DUALCOVERAGEFAMILY") AndAlso _PatientsDS.Tables("DUALCOVERAGEFAMILY").Rows.Count > 0 Then

                    AccumulatorsSplitContainer.Panel2.Controls.Add(AccumulatorDualCoverageValues)

                    Me.AccumulatorsSplitContainer.Panel2Collapsed = False
                    AccumulatorsTab.Text = "   ACCUMULATORS (DUAL Cov.)   "

                    DV = _PatientsDS.Tables("DUALCOVERAGEFAMILY").DefaultView

                    DV.RowFilter = "RELATION_ID = 0"
                    If DV.Count > 0 Then
                        DR = DV.Item(0).Row
                    End If

                    If DR IsNot Nothing Then

                        AccumFName = DR("FIRST_NAME").ToString
                        AccumLName = DR("LAST_NAME").ToString

                        If AccumulatorDualCoverageValues.FamilyID <> CInt(DR("FAMILY_ID")) OrElse AccumulatorDualCoverageValues.LoadID = 0 Then

                            With AccumulatorDualCoverageValues
                                .ShowClaimView = False
                                .ShowHistory = True
                                .IsInEditMode = False
                                .ReadOnly = True
                                .SetFormInfo(CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")), UFCWGeneral.NowDate, -5) '-5 is the indication to ignore the claim#
                                .ShowLineDetails = False
                                .LoadID = 1
                                .Show()
                            End With
                        End If

                        If AccumFName <> "" AndAlso AccumLName <> "" Then NameText = AccumLName & ", " & AccumFName & " (" & AccumulatorDualCoverageValues.FamilyID & ", " & AccumulatorDualCoverageValues.RelationID & ")"

                        AccumulatorDualCoverageValues.AccumulatorPatientName.Text = NameText
                        AccumulatorDualCoverageValues.FamilyAccumulatorsDataGrid.Visible = True
                        AccumulatorDualCoverageValues.FamilyAccumulatorsCurrentLabel.Visible = True
                        AccumulatorDualCoverageValues.FamilyAccumulatorsPriorDataGrid.Visible = True
                        AccumulatorDualCoverageValues.FamilyAccumulatorsPriorLabel.Visible = True
                        AccumulatorDualCoverageValues.PersonalAccumulatorCurrentLabel.Visible = True
                        AccumulatorDualCoverageValues.PersonalAccumulatorDataGrid.Visible = True
                        AccumulatorDualCoverageValues.PersonalAccumulatorPriorDataGrid.Visible = True
                        AccumulatorDualCoverageValues.PersonalAccumulatorPriorLabel.Visible = True

                        AccumulatorDualCoverageValues.Width = AccumulatorDualCoverageValues.Parent.Width
                        AccumulatorDualCoverageValues.Height = AccumulatorDualCoverageValues.Parent.Height

                    Else
                        AccumulatorDualCoverageValues.Visible = False
                    End If

                End If

                TabControl.SelectedTab.Controls.Add(AccumulatorsSplitContainer)

            End Using

        Catch ex As Exception

            Throw

        Finally

            Me.SearchingLabel.Visible = False

        End Try
    End Sub

    Private Sub RestrictedAccess(ByVal status As String)

        TaskTimer.Enabled = False 'cancel further preemptive loading of controls.

        StatusTypesComboBox.DataSource = Nothing
        DocTypesComboBox.DataSource = Nothing

        Me.MatchesLabel.Text = ""
        Me.CustomerServiceResultsDataGrid.DataSource = Nothing

        MessageBox.Show("You are not authorized to view" & If(status.ToUpper.Contains("LOCAL"), " Local", " Trust") & " Employee Information.", "Access Restricted", MessageBoxButtons.OK, MessageBoxIcon.Stop)

        TabControl.Visible = False

    End Sub

    Private Shared Function CompareValues(ByVal str1 As String, ByVal str2 As String) As Boolean
        Dim Value As Boolean = True
        If Not IsNumeric((str1.Substring(0, 1))) AndAlso IsNumeric(str2.Substring(0, 1)) Then
            If str1 < str2 Then
                Value = False
            End If
        ElseIf IsNumeric((str1.Substring(0, 1))) AndAlso Not IsNumeric(str2.Substring(0, 1)) Then
            If str1 < str2 Then
                Value = False
            End If
        Else
            If str1 > str2 Then
                Value = False
            End If
        End If
        Return Value
    End Function

    Private Function RowsMultiSelected() As ArrayList

        Dim AL As New ArrayList()
        Dim CM As CurrencyManager
        Dim DV As DataView

        Try

            If CustomerServiceResultsDataGrid.DataSource IsNot Nothing Then
                CM = CType(Me.BindingContext(CustomerServiceResultsDataGrid.DataSource, CustomerServiceResultsDataGrid.DataMember), CurrencyManager)
                DV = CType(CM.List, DataView)

                For I As Integer = 0 To DV.Count - 1
                    If CustomerServiceResultsDataGrid.IsSelected(I) Then
                        AL.Add(DV(I))
                    End If
                Next

                Return AL
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Function MultiRowsSelected() As Boolean

        Dim CM As CurrencyManager
        Dim DV As DataView
        Dim ICount As Integer = 0

        Try

            If CustomerServiceResultsDataGrid.DataSource IsNot Nothing Then
                CM = CType(Me.BindingContext(CustomerServiceResultsDataGrid.DataSource, CustomerServiceResultsDataGrid.DataMember), CurrencyManager)
                DV = CType(CM.List, DataView)

                For i As Integer = 0 To DV.Count - 1
                    If CustomerServiceResultsDataGrid.IsSelected(i) Then
                        If ICount > 1 Then Return True
                        ICount += 1
                    End If
                Next

            End If

            Return False

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Sub PlaceofServiceButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlaceofServiceButton.Click
        Dim POSLookUp As PlaceOfServiceLookup

        Try
            POSLookUp = New PlaceOfServiceLookup

            If POSLookUp.ShowDialog(Me) = DialogResult.OK Then
                For cnt As Integer = 0 To POSLookUp.PlaceOfServiceDataGrid.GetGridRowCount - 1
                    If POSLookUp.PlaceOfServiceDataGrid.IsSelected(cnt) = True Then
                        PlaceOfServiceTextBox.Text = CStr(POSLookUp.PlaceOfServiceDataGrid.Item(cnt, CInt(POSLookUp.PlaceOfServiceDataGrid.GetColumnPosition("PLACE_OF_SERV_VALUE"))))
                    End If
                Next
            End If

        Catch ex As Exception

            Throw

        End Try
    End Sub

    Private Sub BillTypeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BillTypeButton.Click
        Try
            Dim BillTypeLookUp As New BillTypeLookup

            If BillTypeLookUp.ShowDialog(Me) = DialogResult.OK Then
                For cnt As Integer = 0 To BillTypeLookUp.BillTypeDataGrid.GetGridRowCount - 1
                    If BillTypeLookUp.BillTypeDataGrid.IsSelected(cnt) = True Then
                        BillTypeTextBox.Text = CStr(BillTypeLookUp.BillTypeDataGrid.Item(cnt, CInt(BillTypeLookUp.BillTypeDataGrid.GetColumnPosition("BILL_TYPE_VALUE"))))
                    End If
                Next
            End If

        Catch ex As Exception

            Throw

        End Try
    End Sub

    Private Sub ReprocessPTOMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReprocessPTOMenuItem.Click
        Dim Transaction As DbTransaction
        Dim DR As DataRow

        Try

            DR = CustomerServiceResultsDataGrid.SelectedRowPreview

            If DR("CLAIM_ID") Is DBNull.Value Then Exit Sub

            Me.SearchingLabel.Text = "Reprocessing ..."

            Transaction = CMSDALCommon.BeginTransaction

            CMSDALFDBMD.UpdateClaimMasterStatus(CInt(DR("CLAIM_ID")), "INPROGRESS", SystemInformation.UserName.ToUpper, Transaction)

            Dim HistSum As String = "CLAIM ID " & Format(DR("CLAIM_ID"), "00000000") & " WAS RESET BACK TO INPROGRESS"
            Dim HistDetail As String = "ADJUSTOR " & SystemInformation.UserName.ToUpper & " SENT THIS ITEM BACK TO THE QUEUE TO REPROCESS."

            CMSDALFDBMD.CreateDocHistory(CInt(DR("CLAIM_ID")), UFCWGeneral.IsNullLongHandler(DR("DOCID")), "REPROCESSED", CInt(DR("FAMILY_ID")), UFCWGeneral.IsNullShortHandler(DR("RELATION_ID")), UFCWGeneral.IsNullIntegerHandler(DR("PART_SSN")), UFCWGeneral.IsNullIntegerHandler(DR("PAT_SSN")), CStr(DR("DOC_CLASS")), CStr(DR("DOC_TYPE")), HistSum, HistDetail, SystemInformation.UserName.ToUpper, Transaction)

            CMSDALCommon.CommitTransaction(Transaction)

            DR("STATUS") = "INPROGRESS"
            DR("STATUS_DATE") = UFCWGeneral.NowDate.ToString("MM/dd/yyyy")
            DR.EndEdit()

            MessageBox.Show("Claim Was Pended For Reprocess", "Reprocess Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

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
        End Try
    End Sub

    Private Sub ReprocessPTCMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReprocessPTCMenuItem.Click
        Dim Transaction As DbTransaction
        Dim DR As DataRow

        Try

            DR = CustomerServiceResultsDataGrid.SelectedRowPreview

            If DR("CLAIM_ID") Is DBNull.Value Then Exit Sub
            Me.SearchingLabel.Text = "Reprocessing ..."

            Transaction = CMSDALCommon.BeginTransaction

            CMSDALFDBMD.UpdateClaimMasterStatus(CInt(DR("CLAIM_ID")), "INPROGRESS", SystemInformation.UserName.ToUpper, Transaction)

            Dim HistSum As String = "CLAIM ID " & Format(DR("CLAIM_ID"), "00000000") & " WAS RESET BACK TO INPROGRESS"
            Dim HistDetail As String = "ADJUSTOR " & SystemInformation.UserName.ToUpper & " SENT THIS ITEM BACK TO THE QUEUE TO REPROCESS."
            CMSDALFDBMD.CreateDocHistory(CInt(DR("CLAIM_ID")), UFCWGeneral.IsNullLongHandler(DR("DOCID")), "REPROCESSED", CInt(DR("FAMILY_ID")), UFCWGeneral.IsNullShortHandler(DR("RELATION_ID")), UFCWGeneral.IsNullIntegerHandler(DR("PART_SSN")), UFCWGeneral.IsNullIntegerHandler(DR("PAT_SSN")), CStr(DR("DOC_CLASS")), CStr(DR("DOC_TYPE")), HistSum, HistDetail, SystemInformation.UserName.ToUpper, Transaction)
            CMSDALFDBMD.PendToUser(CInt(DR("CLAIM_ID")), SystemInformation.UserName.ToUpper, Transaction)

            CMSDALCommon.CommitTransaction(Transaction)

            DR("STATUS") = "INPROGRESS"
            DR("STATUS_DATE") = UFCWGeneral.NowDate.ToString("MM/dd/yyyy")

            MessageBox.Show("Claim Was Pended to You For Reprocess", "Reprocess Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

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
        End Try
    End Sub

    Private Sub CustomerServiceResultsDataGrid_MouseHover(sender As Object, e As EventArgs) Handles CustomerServiceResultsDataGrid.MouseHover

    End Sub

    Private Sub ResultsDataGrid_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles CustomerServiceResultsDataGrid.MouseMove
        Dim ToolTipText As String = ""
        Dim ReasonDRs As DataRow()

        Dim DG As DataGridCustom
        Dim HTI As DataGrid.HitTestInfo
        Dim HoverCell As DataGridCell

        Try

            DG = CType(sender, DataGridCustom)
            HTI = DG.HitTest(e.X, e.Y)

            If HTI.Type = DataGrid.HitTestType.Cell Then
                _HoverLastRecordedCell = New DataGridCell(HTI.Row, HTI.Column) ' Store the new hit row
            Else
                Return
            End If

            If HTI.Row > -1 AndAlso HTI.Row <= (DG.GetGridRowCount) Then
                HoverCell = New DataGridCell(HTI.Row, HTI.Column)

                If DG.GetColumnMapping(HTI.Column) = "REASON" Then
                    ReasonDRs = CustomerServiceControl.ReasonListOfValues.Select("REASON = '" & DG.Item(HoverCell).ToString & "'")
                    If ReasonDRs.Length > 0 Then
                        If DG.Item(HoverCell).ToString = ReasonDRs(0).Item("REASON").ToString Then
                            ToolTipText = CStr(ReasonDRs(0).Item("DESCRIPTION"))
                        End If
                    End If
                End If
            End If

            If ToolTipText IsNot Nothing AndAlso ToolTipText.ToString.Trim.Length > 0 Then
                If ToolTip1 IsNot Nothing Then
                    If String.Compare(ToolTip1.GetToolTip(DG), ToolTipText.ToString) <> 0 OrElse ToolTip1.Active = False Then
                        ToolTip1.Active = True
                        ToolTip1.SetToolTip(DG, ToolTipText.ToString)
                    End If
                End If
            Else
                ToolTip1.SetToolTip(DG, "")
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub PatientSearchButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PatientSearchButton.Click

        Dim TheFamilyID As String = CStr(_SearchResultsDT.Rows(0).Item("FAMILY_ID"))
        Dim TheRelationID As String = CStr(_SearchResultsDT.Rows(0).Item("RELATION_ID"))

        ClearAll()

        Me.FamilyIDTextBox.Text = TheFamilyID
        Me.RelationIDTextBox.Text = TheRelationID

        Search()

    End Sub

    Private Sub PatientSearchButton_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles PatientSearchButton.MouseHover
        Dim ToolTipText As String = "Performs a search using the claim's patient information."

        If ToolTipText IsNot Nothing AndAlso ToolTipText.ToString.Trim.Length > 0 Then
            If Me.ToolTip1 Is Nothing OrElse String.Compare(Me.ToolTip1.GetToolTip(CType(sender, Button)), ToolTipText.ToString) <> 0 OrElse Not ToolTip1.Active Then
                ToolTip1.Active = True
                ToolTip1.SetToolTip(CType(sender, Button), ToolTipText.ToString)
            End If
        Else
            ToolTip1.SetToolTip(CType(sender, Button), "")
        End If

    End Sub

    Private Sub ProviderIDTextBox_Leave(sender As Object, e As System.EventArgs) Handles ProviderIDTextBox.Leave
        If ProviderIDTextBox.Text.Trim.Length <> 9 Then ProviderIDTextBox.Text = ProviderIDTextBox.Text.TrimStart("0"c)
    End Sub

    'Private Sub NumericTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles FamilyIDTextBox.TextChanged, RelationIDTextBox.TextChanged, DocIDTextBox.TextChanged, ProviderIDTextBox.TextChanged
    '    Dim TBox As TextBox = CType(sender, TextBox)
    '    Dim strTmp As String

    '    If Not IsNumeric(TBox.Text) AndAlso Len(TBox.Text) > 0 Then
    '        strTmp = TBox.Text
    '        For IntCnt As Integer = 1 To Len(strTmp)
    '            If Not IsNumeric(Mid(strTmp, IntCnt, 1)) AndAlso Len(strTmp) > 0 Then
    '                strTmp = Replace(strTmp, Mid(strTmp, IntCnt, 1), "")
    '            End If
    '        Next
    '        TBox.Text = strTmp
    '    End If

    'End Sub

    Private Sub AlphaNumericTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClaimIDTextBox.TextChanged
        Dim TBox As TextBox = CType(sender, TextBox)
        Dim intCnt As Integer
        Dim strTmp As String

        strTmp = TBox.Text
        For intCnt = 1 To Len(strTmp)
            If Not IsCharacterAlphaNumeric(CChar(Mid(strTmp, intCnt, 1))) AndAlso Len(strTmp) > 0 Then
                strTmp = Replace(strTmp, Mid(strTmp, intCnt, 1), "")
            End If
        Next
        TBox.Text = strTmp

    End Sub

    Private Sub HighlightGridCell(ByVal sender As Object, ByVal e As DataGridFormatCellEventArgs)

        Dim DG As DataGridCustom

        Select Case sender.GetType
            Case GetType(DataGridFormattableTextBoxColumn), GetType(DataGridHighlightTextBoxColumn)

                DG = CType(CType(sender, DataGridHighlightTextBoxColumn).DataGridTableStyle.DataGrid, DataGridCustom)

            Case GetType(DataGridColorBoolColumn), GetType(DataGridHighlightBoolColumn)

                DG = CType(CType(sender, DataGridHighlightBoolColumn).DataGridTableStyle.DataGrid, DataGridCustom)

            Case Else
                Throw New ApplicationException("unexpected column type" & sender.GetType.ToString)

        End Select

        If DG.DataSource Is Nothing Then Exit Sub

        Dim DV As DataView

        Try
            DV = DG.GetCurrentDataView

            If DV IsNot Nothing Then
                If Not IsDBNull(DV(e.RowNumber)("FROM_DATE")) AndAlso Not IsDBNull(DV(e.RowNumber)("THRU_DATE")) Then

                    If CDate(DV(e.RowNumber)("FROM_DATE")) <= CDate(Format(UFCWGeneral.NowDate, "MM-dd-yyyy")) AndAlso CDate(Format(UFCWGeneral.NowDate, "MM-dd-yyyy")) <= CDate(DV(e.RowNumber)("THRU_DATE")) Then

                        Select Case True
                            Case CInt(DV(e.RowNumber)("ELIGIBILITY_GAP_COUNT")) > 0 AndAlso CInt(DV(e.RowNumber)("ELIGIBILITY_GAP_COUNT")) = CInt(DV(e.RowNumber)("ELIGIBILITY_MAX_GAP_COUNT"))
                                e.BackBrush = _InEligcolor
                            Case CInt(DV(e.RowNumber)("ELIGIBILITY_GAP_COUNT")) > 0
                                e.BackBrush = _PartialEligcolor
                            Case Else
                                e.BackBrush = _Eligcolor
                        End Select

                    Else
                        Select Case True
                            Case (CInt(DV(e.RowNumber)("ELIGIBILITY_GAP_COUNT")) > 0 AndAlso CInt(DV(e.RowNumber)("ELIGIBILITY_GAP_COUNT")) = CInt(DV(e.RowNumber)("ELIGIBILITY_MAX_GAP_COUNT"))) OrElse (CDate(Format(UFCWGeneral.NowDate, "MM-dd-yyyy")) < CDate(DV(e.RowNumber)("FROM_DATE")))
                                e.BackBrush = _InEligcolor
                            Case CInt(DV(e.RowNumber)("ELIGIBILITY_GAP_COUNT")) > 0 OrElse CDate(Format(UFCWGeneral.NowDate, "MM-dd-yyyy")) > CDate(DV(e.RowNumber)("THRU_DATE"))
                                e.BackBrush = _PartialInEligcolor
                            Case Else
                                e.BackBrush = _Eligcolor
                        End Select
                    End If

                    e.ForeBrush = Brushes.Black
                End If
            End If
        Catch ex As Exception

            Throw

        End Try

    End Sub

    'Private Sub HighlightDualGridCell(ByVal sender As Object, ByVal e As DataGridFormatCellEventArgs)

    '    Dim DG As DataGridCustom = CType(CType(sender, DataGridPlus.DataGridFormattableTextBoxColumn).DataGridTableStyle.DataGrid, DataGridCustom)

    '    If DG.DataSource Is Nothing Then Exit Sub

    '    Dim DV As DataView
    '    Dim CM As CurrencyManager

    '    Try

    '        CM = CType(Me.BindingContext(DG.DataSource, DG.DataMember), CurrencyManager)
    '        DV = CType(CM.List, DataView)

    '        If DV IsNot Nothing Then
    '            If Not IsDBNull(DV(e.RowNumber)("FROM_DATE")) AndAlso Not IsDBNull(DV(e.RowNumber)("THRU_DATE")) Then

    '                If CDate(DV(e.RowNumber)("FROM_DATE")) <= CDate(Format(UFCWGeneral.NowDate, "MM-dd-yyyy")) AndAlso CDate(Format(UFCWGeneral.NowDate, "MM-dd-yyyy")) <= CDate(DV(e.RowNumber)("THRU_DATE")) Then

    '                    Select Case True
    '                        Case CInt(DV(e.RowNumber)("ELIGIBILITY_GAP_COUNT")) > 0 AndAlso CInt(DV(e.RowNumber)("ELIGIBILITY_GAP_COUNT")) = CInt(DV(e.RowNumber)("ELIGIBILITY_MAX_GAP_COUNT"))
    '                            e.BackBrush = _InEligcolor
    '                        Case CInt(DV(e.RowNumber)("ELIGIBILITY_GAP_COUNT")) > 0
    '                            e.BackBrush = _PartialEligcolor
    '                        Case Else
    '                            e.BackBrush = _Eligcolor
    '                    End Select

    '                Else
    '                    Select Case True
    '                        Case (CInt(DV(e.RowNumber)("ELIGIBILITY_GAP_COUNT")) > 0 AndAlso CInt(DV(e.RowNumber)("ELIGIBILITY_GAP_COUNT")) = CInt(DV(e.RowNumber)("ELIGIBILITY_MAX_GAP_COUNT"))) OrElse (CDate(Format(UFCWGeneral.NowDate, "MM-dd-yyyy")) < CDate(DV(e.RowNumber)("FROM_DATE")))
    '                            e.BackBrush = _InEligcolor
    '                        Case CInt(DV(e.RowNumber)("ELIGIBILITY_GAP_COUNT")) > 0 OrElse CDate(Format(UFCWGeneral.NowDate, "MM-dd-yyyy")) > CDate(DV(e.RowNumber)("THRU_DATE"))
    '                            e.BackBrush = _PartialInEligcolor
    '                        Case Else
    '                            e.BackBrush = _Eligcolor
    '                    End Select
    '                End If

    '                e.ForeBrush = Brushes.Black
    '            End If
    '        End If
    '    Catch ex As Exception
    '        
    '        If (Rethrow) Then
    '            Throw
    '        Else
    '            MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End If
    '    End Try
    'End Sub

    Private Sub HighlightDualGridRows(ByVal sender As Object, ByVal e As DataGridFormatRowEventArgs)

        If DualParticipantDemographicsGrid.DataSource Is Nothing Then Exit Sub

        Dim DV As DataView
        Dim CM As CurrencyManager

        Try

            CM = CType(Me.BindingContext(DualParticipantDemographicsGrid.DataSource, DualParticipantDemographicsGrid.DataMember), CurrencyManager)
            DV = CType(CM.List, DataView)
            If DV IsNot Nothing Then
                If Not IsDBNull(DV(e.RowNumber)("FROM_DATE")) AndAlso Not IsDBNull(DV(e.RowNumber)("THRU_DATE")) Then

                    If CDate(DV(e.RowNumber)("FROM_DATE")) <= CDate(Format(UFCWGeneral.NowDate, "MM-dd-yyyy")) AndAlso CDate(Format(UFCWGeneral.NowDate, "MM-dd-yyyy")) <= CDate(DV(e.RowNumber)("THRU_DATE")) Then
                        If CInt(DV(e.RowNumber)("ELIGIBILITY_GAP_COUNT")) > 0 Then
                            e.BackBrush = _PartialEligcolor
                        Else
                            e.BackBrush = _Eligcolor
                        End If

                    Else
                        If CInt(DV(e.RowNumber)("ELIGIBILITY_GAP_COUNT")) > 0 Then
                            e.BackBrush = _PartialInEligcolor
                        Else
                            e.BackBrush = _InEligcolor
                        End If
                    End If

                    e.ForeBrush = Brushes.Black

                End If
            End If

        Catch ex As Exception

            Throw
        Finally
        End Try
    End Sub

    Private Sub ColumnSelectorMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim ContextMenu As ContextMenuStrip = CType(CType(sender, ToolStripMenuItem).Owner, System.Windows.Forms.ContextMenuStrip)
        Dim ExportDataGrid As DataGridCustom = CType(ContextMenu.SourceControl, DataGridCustom)

        ExportDataGrid.SelectExportCols()

    End Sub

    Private Shared Function GetSelectedColumns(ByVal dg As DataGridCustom) As String()

        Dim GridCol As DataGridColumnStyle
        Dim Cols() As String
        ReDim Cols(0)

        Dim DGTS As New DataGridTableStyle()

        DGTS = dg.GetTableStyleFromGrid()

        For Each GridCol In DGTS.GridColumnStyles
            If CDbl(GetSetting(dg.AppKey, "CustomerService\" & dg.Name & "\ColumnSelector", "Col " & GridCol.MappingName & " Export", "0")) = 1 Then
                Cols(UBound(Cols, 1)) = GridCol.MappingName
                ReDim Preserve Cols(UBound(Cols, 1) + 1)
            End If
        Next
        ReDim Preserve Cols(UBound(Cols, 1) - 1)

        Return Cols
    End Function

    Private Sub PatientFirstNameTextBox_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles PatientFirstNameTextBox.MouseClick, PatientLastNameTextBox.MouseClick
        PatientLookupButton.PerformClick()
    End Sub

    Private Sub PatientLookupButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PatientLookupButton.Click


        Try

            Using Frm As PatientLookUpForm = New PatientLookUpForm(Me.PatientLastNameTextBox.Text, Me.PatientFirstNameTextBox.Text)

                If Frm.ShowDialog(Me) = DialogResult.OK Then

                    ClearAll()

                    Me.FamilyID = Frm.FamilyID
                    Me.RelationID = Frm.RelationID
                Else
                    Me.FamilyID = Nothing
                    Me.RelationID = Nothing
                End If

            End Using

        Catch ex As Exception

            Throw

        Finally
        End Try

    End Sub

    Private Sub ResultViewByClaimIDMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If _SearchResultsDT Is Nothing Then Return

        Dim BM As BindingManagerBase
        Dim DR As DataRow

        Dim ClaimID As String

        Try

            BM = Me.CustomerServiceResultsDataGrid.BindingContext(Me.CustomerServiceResultsDataGrid.DataSource, Me.CustomerServiceResultsDataGrid.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            ClaimID = DR("CLAIM_ID").ToString.Trim

            ClearAllButton.PerformClick()
            ClaimIDTextBox.Text = ClaimID

            SearchButton.PerformClick()

        Catch ex As Exception

            Throw
        Finally

        End Try

    End Sub

    Private Sub ResultsDisplayDocumentMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResultsDisplayDocumentMenuItem.Click

        If _SearchResultsDT Is Nothing Then Return

        Dim BM As BindingManagerBase
        Dim DR As DataRow
        Dim DG As DataGridCustom
        Dim DRs As ArrayList
        Dim Docs As New List(Of Long?)
        Dim TSMenuItem As ToolStripMenuItem
        Dim DataGridCustomContextMenu As ContextMenuStrip

        Dim Cnt As Integer = 0
        Dim Tot As Integer = 0

        Dim DV As DataView

        Try
            TSMenuItem = CType(sender, ToolStripMenuItem)
            DataGridCustomContextMenu = CType(TSMenuItem.GetCurrentParent, ContextMenuStrip)
            DG = CType(DataGridCustomContextMenu.SourceControl, DataGridCustom)

            BM = DG.BindingContext(DG.DataSource, DG.DataMember)
            DR = CType(BM.Current, DataRowView).Row
            DV = CType(DG.DataSource, DataView)

            DRs = DG.GetSelectedDataRows()

            For Each DR In DRs
                If Not IsDBNull(DR("DOCID")) Then
                    Docs.Add(CLng(DR("DOCID")))
                End If
            Next

            If Not Docs.Any Then
                MessageBox.Show("There are no documents to display.", "No Documents", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else

                Using FNDisplay As New Display
                    FNDisplay.Display(Docs)

                    'FNDisplay.Display(New List(Of Long?) From {CLng(DR("DOCID"))})
                End Using

            End If

        Catch ex As System.ServiceModel.FaultException(Of UFCW.WCF.FileNet.FileNetWCFFault)

            MessageBox.Show(ex.Message, "FileNet unavailable, Rebooting your PC may resolve connectivity issues.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch ex As ApplicationException

            MessageBox.Show(ex.Message, "FileNet unavailable, Restarting application may resolve connectivity issues.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch ex As TimeoutException

            MessageBox.Show(ex.Message, "FileNet unavailable, Restarting application may resolve connectivity issues.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Sub mnuFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CustomerServiceResultsDataGrid.ShowFindDialog()
    End Sub

    Private Sub mnuAutoSize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CustomerServiceResultsDataGrid.AutoSize()
    End Sub

    Private Sub LifeEventsMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LifeEventsToolStripMenuItem.Click

        Dim ContextMenu As ContextMenuStrip
        Dim Grid As DataGridCustom

        Dim BM As BindingManagerBase
        Dim DR As DataRow

        Try

            ContextMenu = CType(CType(sender, ToolStripMenuItem).Owner, System.Windows.Forms.ContextMenuStrip)
            Grid = CType(ContextMenu.SourceControl, DataGridCustom)

            BM = Grid.BindingContext(Grid.DataSource, Grid.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            Using Frm As LifeEventsViewerForm = New LifeEventsViewerForm(CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")), CDate(DR("FROM_DATE")), CDate(DR("THRU_DATE")))

                Frm.ShowDialog()

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub ContactInfoMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ContactInfoToolStripMenuItem.Click

        Dim ContextMenu As ContextMenuStrip
        Dim Grid As DataGridCustom

        Dim BM As BindingManagerBase
        Dim DR As DataRow

        Try

            ContextMenu = CType(CType(sender, ToolStripMenuItem).Owner, System.Windows.Forms.ContextMenuStrip)
            Grid = CType(ContextMenu.SourceControl, DataGridCustom)

            BM = Grid.BindingContext(Grid.DataSource, Grid.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            Using Frm As ContactInfoViewerForm = New ContactInfoViewerForm(CInt(DR("FAMILY_ID").ToString), CInt(DR("RELATION_ID").ToString))
                Frm.ShowDialog()
            End Using

        Catch ex As Exception

            Throw

        Finally
        End Try

    End Sub

    Private Sub FamilySummaryRemarksToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FamilySummaryRemarksToolStripMenuItem.Click

        Dim ContextMenu As ContextMenuStrip
        Dim Grid As DataGridCustom

        Dim BM As BindingManagerBase
        Dim DR As DataRow
        Dim Frm As FamilySummaryRemarksViewerForm

        Try

            ContextMenu = CType(CType(sender, ToolStripMenuItem).Owner, System.Windows.Forms.ContextMenuStrip)
            Grid = CType(ContextMenu.SourceControl, DataGridCustom)

            BM = Grid.BindingContext(Grid.DataSource, Grid.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            Frm = New FamilySummaryRemarksViewerForm(CInt(DR("FAMILY_ID").ToString), CInt(DR("RELATION_ID").ToString))
            Frm.Show()

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    'Private Sub ResultsDisplayEligibiltyMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResultsDisplayEligibiltyMenuItem.Click
    '    If _SearchResultsDT Is Nothing Then Return
    'End Sub

    Private Sub PatientEligibilityToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PatientEligibilityToolStripMenuItem.Click, ResultsDisplayEligibiltyMenuItem.Click
        Dim ContextMenu As ContextMenuStrip
        Dim Grid As DataGridCustom

        Dim BM As BindingManagerBase
        Dim DR As DataRow

        Try

            ContextMenu = CType(CType(sender, ToolStripMenuItem).Owner, System.Windows.Forms.ContextMenuStrip)
            Grid = CType(ContextMenu.SourceControl, DataGridCustom)

            BM = Grid.BindingContext(Grid.DataSource, Grid.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            Using Frm As PatientEligibilityViewerForm = New PatientEligibilityViewerForm(CInt(DR("FAMILY_ID")), UFCWGeneral.IsNullShortHandler(DR("RELATION_ID")))
                Frm.ShowDialog()
            End Using

        Catch ex As Exception

            Throw
        Finally
        End Try
    End Sub

    Private Sub BlinkTimer_Tick(sender As System.Object, e As System.EventArgs) Handles BlinkTimer.Tick

        If _BlinkStarttime Is Nothing Then _BlinkStarttime = UFCWGeneral.NowDate

        If (UFCWGeneral.NowDate.AddSeconds(-3) > _BlinkStarttime) Then
            BlinkTimer.Enabled = False
            _BlinkStarttime = Nothing
            ParticipantHighAlertLabel.Enabled = True
            DualParticipantHighAlertLabel.Enabled = True
        Else
            ParticipantHighAlertLabel.Enabled = Not ParticipantHighAlertLabel.Enabled
            DualParticipantHighAlertLabel.Enabled = Not DualParticipantHighAlertLabel.Enabled
        End If

    End Sub

    Private Sub TaskTimer_Tick(sender As System.Object, e As System.EventArgs) Handles TaskTimer.Tick
        Try
            TaskTimer.Enabled = False

            If _PopulatePrescriptionsTask IsNot Nothing AndAlso _PopulatePrescriptionsTask.IsCompleted Then
                PopulatePrescriptionsInfo(CInt(Me.FamilyID), Me.RelationID)
            End If

            If _PopulateHRATask IsNot Nothing AndAlso _PopulateHRATask.IsCompleted Then
                PopulateHRAInfo(CInt(Me.FamilyID), Me.RelationID)
            End If

            If _PopulateCOBTask IsNot Nothing AndAlso _PopulateCOBTask.IsCompleted Then
                PopulateCOBInfo(CInt(Me.FamilyID), Me.RelationID) 'always a patient level
            End If

            If _PopulateCoverageHistoryTask IsNot Nothing AndAlso _PopulateCoverageHistoryTask.IsCompleted Then
                PopulateCoverageHistoryInfo(CInt(Me.FamilyID))
            End If

            If _PopulateHoursTask IsNot Nothing AndAlso _PopulateHoursTask.IsCompleted Then
                PopulateHoursInfo(HoursControl, _PopulateHoursTask, CInt(Me.FamilyID), _HoursDS)
            End If

            If _PopulateHoursDualCoverageTask IsNot Nothing AndAlso _PopulateHoursDualCoverageTask.IsCompleted Then
                PopulateHoursInfo(HoursDualCoverageControl, _PopulateHoursDualCoverageTask, CInt(_PatientsDS.Tables(1).Rows(0)("FAMILY_ID")), _HoursDualCoverageDS)
            End If

            If _PopulateEligibilityHoursTask IsNot Nothing AndAlso _PopulateEligibilityHoursTask.IsCompleted Then
                PopulateEligibilityHoursInfo(EligibilityHoursControl, _PopulateEligibilityHoursTask, CInt(Me.FamilyID), _EligibilityHoursDS)
            End If

            If _PopulateEligibilityHoursDualCoverageTask IsNot Nothing AndAlso _PopulateEligibilityHoursDualCoverageTask.IsCompleted Then
                PopulateEligibilityHoursInfo(EligibilityHoursDualCoverageControl, _PopulateEligibilityHoursDualCoverageTask, CInt(_PatientsDS.Tables(1).Rows(0)("FAMILY_ID")), _EligibilityHoursDualCoverageDS)
            End If

            If _PopulatePremiumsTask IsNot Nothing AndAlso _PopulatePremiumsTask.IsCompleted Then
                PopulatePremiumsInfo(CInt(Me.FamilyID))
            End If

            If _PopulatePremiumsEnrollmentTask IsNot Nothing AndAlso _PopulatePremiumsEnrollmentTask.IsCompleted Then
                PopulatePremiumsEnrollmentInfo(CInt(Me.FamilyID))
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SendToCSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PatientToolStripMenuItem.Click, FamilyIDToolStripMenuItem.Click, ClaimIDToolStripMenuItem.Click
        Dim BM As BindingManagerBase
        Dim DR As DataRow
        Dim ProcessProperties As ProcessStartInfo
        Dim CustomerServiceFI As FileInfo

        Try

            BM = Me.CustomerServiceResultsDataGrid.BindingContext(Me.CustomerServiceResultsDataGrid.DataSource, Me.CustomerServiceResultsDataGrid.DataMember)
            DR = CType(BM.Current, DataRowView).Row
            ProcessProperties = New ProcessStartInfo

            ProcessProperties.FileName = Environment.CurrentDirectory & "\" & "UFCW.CMS.CustomerService.exe"
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
            'Shell(Environment.CurrentDirectory & "\" & "CustomerService.exe  CLAIM_ID=" & dr("CLAIM_ID").ToString, AppWinStyle.NormalFocus)

            'Process.Start(Environment.CurrentDirectory & "\" & "CustomerService.exe", " CLAIM_ID=" & dr("CLAIM_ID").ToString)

            'ProcessProperties = New ProcessStartInfo
            'CustomerServiceFI = UFCWGeneral.FindCustomerService()

            'ProcessProperties.FileName = CustomerServiceFI.FullName 'Environment.CurrentDirectory & "\" & "UFCW.CMS.CustomerService.exe"

            'Select Case True
            '    Case CType(sender, ToolStripMenuItem).Name.Contains("Claim")
            '        ProcessProperties.Arguments = "CLAIM_ID=" & DR("CLAIM_ID").ToString
            '    Case CType(sender, ToolStripMenuItem).Name.Contains("Patient")
            '        ProcessProperties.Arguments = "PAT_SSN=" & DR("PAT_SSN").ToString
            '    Case CType(sender, ToolStripMenuItem).Name.Contains("Family")
            '        ProcessProperties.Arguments = "FAMILY_ID=" & DR("FAMILY_ID").ToString
            'End Select
            'ProcessProperties.WindowStyle = ProcessWindowStyle.Normal

            'ProcessProperties.UseShellExecute = False
            'Process.Start(ProcessProperties)

        Catch ex As Exception

            Throw

        End Try

    End Sub

    Private Sub AlertHistoryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AlertHistoryToolStripMenuItem.Click
        Dim ContextMenu As ContextMenuStrip
        Dim Grid As DataGridCustom

        Dim BM As BindingManagerBase
        Dim DR As DataRow

        Try

            ContextMenu = CType(CType(sender, ToolStripMenuItem).Owner, System.Windows.Forms.ContextMenuStrip)
            Grid = CType(ContextMenu.SourceControl, DataGridCustom)

            BM = Grid.BindingContext(Grid.DataSource, Grid.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            Using Frm As AlertsHistoryViewerForm = New AlertsHistoryViewerForm(CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")))
                Frm.ShowDialog()
            End Using

        Catch ex As Exception

            Throw

        Finally
        End Try
    End Sub

    Private Sub FamilyIdTextBox_Validating(sender As Object, e As CancelEventArgs) Handles FamilyIDTextBox.Validating

        Dim TBox As TextBox = CType(sender, TextBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ErrorProvider1.ClearError(TBox)

            If TBox.Enabled = False OrElse TBox.ReadOnly = True Then Exit Sub

            If (TBox.Text.Trim.Length > 0) Then

                If Not IsNumeric(TBox.Text.Trim) OrElse Not IsInteger(TBox.Text.Trim) OrElse CInt(TBox.Text.Trim) < 1 Then
                    ErrorProvider1.SetErrorWithTracking(TBox, "Invalid FamilyID")
                    MessageBox.Show("This is not a valid FamilyID", "Invalid FamilyID", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    End Sub

    Private Sub ClaimIDTextBox_Validating(sender As Object, e As CancelEventArgs) Handles ClaimIDTextBox.Validating

        Dim TBox As TextBox = CType(sender, TextBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ErrorProvider1.ClearError(TBox)

            If TBox.Enabled = False OrElse TBox.ReadOnly = True Then Exit Sub

            If (TBox.Text.Trim.Length > 0) Then

                If IsInteger(TBox.Text.Trim) AndAlso CInt(TBox.Text.Trim) > 0 Then 'ClaimID search
                ElseIf Not IsNumeric(TBox.Text.Trim.Substring(0, 1)) AndAlso IsNumeric(TBox.Text.Trim.Substring(1, TBox.Text.Trim.Length - 1)) Then 'Maxid search
                ElseIf TBox.Text.Trim.Length = 15 AndAlso IsNumeric(TBox.Text.Trim.Substring(0, 4)) Then
                Else
                    ErrorProvider1.SetErrorWithTracking(TBox, "Invalid Claim#, DCN or MaxID")
                    MessageBox.Show("This is not a valid Claim#, DCN or MaxID", "Improper Claim Identifier", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    End Sub

    Private Sub DocIdTextBox_Validating(sender As Object, e As CancelEventArgs) Handles DocIDTextBox.Validating

        Dim TBox As TextBox = CType(sender, TextBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ErrorProvider1.ClearError(TBox)

            If TBox.Enabled = False OrElse TBox.ReadOnly = True Then Exit Sub

            If (TBox.Text.Trim.Length > 0) Then

                If Not IsNumeric(TBox.Text.Trim) OrElse Not IsInteger(TBox.Text.Trim) OrElse (IsInteger(TBox.Text.Trim) AndAlso CInt(TBox.Text.Trim) < 1) Then
                    ErrorProvider1.SetErrorWithTracking(TBox, "DocID format or value is not valid. Must be 6-7 digits long")
                    MessageBox.Show("DocID is not valid.", "Invalid DocumentID.", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    End Sub

    Private Sub ProviderIDTextBox_Validating(sender As Object, e As CancelEventArgs) Handles ProviderIDTextBox.Validating

        If ProviderIDTextBox.Text.Trim.Length > 0 Then
            If IsNumeric(Me.ProviderIDTextBox.Text.Trim) AndAlso Not IsInteger(Me.ProviderIDTextBox.Text.Trim) Then 'ProviderID search
                ErrorProvider1.SetErrorWithTracking(ProviderIDTextBox, "This is not a ProviderID/TaxID")
                MessageBox.Show("This is not a valid ProviderID or TaxID", "Improper ProviderID/TaxID", MessageBoxButtons.OK, MessageBoxIcon.Error)
                e.Cancel = True
            End If
        End If

    End Sub

    Private Sub DualParticipantDemographicsGrid_PositionChanged(sender As Object, e As EventArgs) Handles DualParticipantDemographicsGrid.PositionChanged

        Dim BS As BindingSource

        Try

            BS = DirectCast(sender, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  CM(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If BS.Position > -1 AndAlso BS.Current IsNot Nothing Then
                Dim DR As DataRow = CType(BS.Current, DataRowView).Row
                If DR IsNot Nothing Then
                    Me.DualParticipantAddressLabel.Text = If(CInt(DR("RELATION_ID")) = 0, "Participant ", "Dependent ") & "Address: " & DR("ADDRESS").ToString
                End If
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  CM(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub

    'Private Sub DualParticipantDemographicsGrid_CurrentRowChanged(CurrentRowIndex As Integer?, LastRowIndex As Integer?) Handles DualParticipantDemographicsGrid.CurrentRowChanged
    '    Dim AddressDR As DataRow

    '    AddressDR = DemographicsGrid_CurrentRowChanged(DualParticipantDemographicsGrid)

    '    If AddressDR IsNot Nothing Then Me.DualParticipantAddressLabel.Text = If(CInt(AddressDR("RELATION_ID")) = 0, "Participant ", "Dependent ") & "Address: " & AddressDR("ADDRESS").ToString

    'End Sub

    'Private Function DemographicsGrid_CurrentRowChanged(ByRef demographicsGrid As DataGridCustom) As DataRow

    '    If demographicsGrid.GetGridRowCount = 0 Then Return Nothing

    '    Try

    '    Catch ex As Exception

    '        
    '        If (Rethrow) Then
    '            Throw
    '        Else
    '            MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End If

    '    End Try

    'End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub TabControl_DrawItem(sender As Object, e As DrawItemEventArgs) Handles TabControl.DrawItem


        Dim tabContas As TabControl = DirectCast(sender, TabControl)
        Dim sTexto As String = tabContas.TabPages(e.Index).Text
        Dim g As Graphics = e.Graphics
        Dim fonte As Font = tabContas.Font
        Dim format = New System.Drawing.StringFormat

        'CHANGES HERE...
        format.Alignment = StringAlignment.Center
        format.LineAlignment = StringAlignment.Center
        Dim pincel As New SolidBrush(Color.Black)
        'RENEMED VARIEBLE HERE...
        Dim retangulo As RectangleF = RectangleF.op_Implicit(tabContas.GetTabRect(e.Index))
        If tabContas.SelectedIndex = e.Index Then
            fonte = New Font(fonte, FontStyle.Bold)
            pincel = New SolidBrush(Color.Black)
            'CHANGED BACKGROUN COLOR HERE...
            '            g.FillRectangle(Brushes.White, retangulo)
        End If
        g.DrawString(sTexto, fonte, pincel, retangulo, format)
    End Sub

    Private Sub DiseaseManagementToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DiseaseManagementMenuItem.Click

        Dim ContextMenu As ContextMenuStrip
        Dim Grid As DataGridCustom

        Dim BM As BindingManagerBase
        Dim DR As DataRow

        Try

            ContextMenu = CType(CType(sender, ToolStripMenuItem).Owner, System.Windows.Forms.ContextMenuStrip)
            Grid = CType(ContextMenu.SourceControl, DataGridCustom)

            BM = Grid.BindingContext(Grid.DataSource, Grid.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            Using Frm As PatientDiseaseManagementForm = New PatientDiseaseManagementForm(CInt(DR("FAMILY_ID")), UFCWGeneral.IsNullShortHandler(DR("RELATION_ID")))
                Frm.ShowDialog()
            End Using

        Catch ex As Exception

            Throw

        Finally
        End Try

    End Sub

    Private Sub FamilyInfoDataGridContextMenuStrip_Opening(sender As Object, e As CancelEventArgs) Handles FamilyInfoDataGridContextMenuStrip.Opening
        Dim DataGridCustomContextMenu As ContextMenuStrip

        DataGridCustomContextMenu = CType(sender, ContextMenuStrip)
        DataGridCustomContextMenu.Items("DiseaseManagementMenuItem").Available = False

        If Not UFCWGeneralAD.CMSLocals Then
            DataGridCustomContextMenu.Items("DiseaseManagementMenuItem").Available = True
        End If

    End Sub

    Private Sub ReasonButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReasonsButton.Click

        Try

            ReasonsTextBox.Text = SelectReasons(ReasonsTextBox.Text)

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Function SelectReasons(ByVal txtReasons As String) As String

        Dim ReasonDT As ClaimDataset.REASONDataTable

        Dim DR As DataRow

        Try

            ReasonDT = New ClaimDataset.REASONDataTable
            Dim NoCommaText As String = txtReasons.ToUpper.Replace(",", " ")
            Dim ReasonsAL As String() = NoCommaText.Split(New Char() {CChar(" ")}, StringSplitOptions.RemoveEmptyEntries)
            Dim Priority As Short = 0

            For Each Reason In ReasonsAL
                DR = ReasonDT.NewRow
                DR("CLAIM_ID") = -1
                DR("LINE_NBR") = -1
                DR("PRIORITY") = Priority
                DR("REASON") = Reason
                DR("DESCRIPTION") = ""

                ReasonDT.Rows.Add(DR)
                Priority = +1
            Next

            ReasonDT.AcceptChanges()

            Using FRM As LineReasonsForm = New LineReasonsForm(-1, -1, ReasonDT, False)

                If FRM.ShowDialog(Me) = DialogResult.OK Then
                    ReasonDT = FRM.SelectedReasons
                End If

            End Using

            If ReasonDT IsNot Nothing AndAlso ReasonDT.Rows.Count > 0 Then
                Dim FlattenQuery = String.Join(",", ReasonDT.Select(Function(p) p.REASON.ToString()))
                If FlattenQuery.Any Then Return FlattenQuery
            End If

        Catch ex As Exception

            Throw

        Finally

        End Try

    End Function

    Private Sub ProcedureCodeTextBox_Validating(sender As Object, e As CancelEventArgs) Handles ProcedureCodeTextBox.Validating

        Dim TBox As TextBox = CType(sender, TextBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ErrorProvider1.ClearError(TBox)

            If TBox.Text.Trim.Length > 0 Then

                If TBox.Text.Any(Function(p) p = "-") AndAlso TBox.Text.Any(Function(p) p = ",") Then
                    ErrorProvider1.SetErrorWithTracking(TBox, "Range (-) and List(,) operators cannot be used in the same search.")
                ElseIf TBox.Text.Count(Function(p) p = "-") > 1 Then
                    ErrorProvider1.SetErrorWithTracking(TBox, "Only a Single Range can be searched at a time.")
                ElseIf TBox.Text.Any(Function(p) p = "-") Then

                    Dim NoSpaceText As String = TBox.Text.ToUpper.Replace(" ", "")
                    Dim ProcedureAL As String() = NoSpaceText.Split(New Char() {CChar("-")}, StringSplitOptions.RemoveEmptyEntries)
                    If ProcedureAL.Length <> 2 Then
                        ErrorProvider1.SetErrorWithTracking(TBox, "A Range is two Procedures seperated by hyphen e.g A0000-A9999.")
                    ElseIf Not (ProcedureAL(0) = ProcedureAL.Min) Then
                        ErrorProvider1.SetErrorWithTracking(TBox, "Range must be in lowest to highest sequence order ")
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

    End Sub

    Private Sub DiagnosisCodesTextBox_Validating(sender As Object, e As CancelEventArgs) Handles DiagnosisCodesTextBox.Validating

        Dim TBox As TextBox = CType(sender, TextBox)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ErrorProvider1.ClearError(TBox)

            If TBox.Text.Trim.Length > 0 Then

                If TBox.Text.Any(Function(p) p = "-") AndAlso TBox.Text.Any(Function(p) p = ",") Then
                    ErrorProvider1.SetErrorWithTracking(TBox, "Range (-) and List(,) operators cannot be used in the same search.")
                ElseIf TBox.Text.Count(Function(p) p = "-") > 1 Then
                    ErrorProvider1.SetErrorWithTracking(TBox, "Only a Single Range can be searched at a time.")
                ElseIf TBox.Text.Any(Function(p) p = "-") Then

                    Dim NoSpaceText As String = TBox.Text.ToUpper.Replace(" ", "")
                    Dim DiagnosisAL As String() = NoSpaceText.Split(New Char() {CChar("-")}, StringSplitOptions.RemoveEmptyEntries)
                    If DiagnosisAL.Length <> 2 Then
                        ErrorProvider1.SetErrorWithTracking(TBox, "A Range is two Diagnosis seperated by hyphen e.g A0000-A9999.")
                    ElseIf Not (DiagnosisAL(0) = DiagnosisAL.Min) Then
                        ErrorProvider1.SetErrorWithTracking(TBox, "Range must be in lowest to highest sequence order ")
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

    End Sub

    Private Sub DiagnosisCodesTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles DiagnosisCodesTextBox.KeyDown

        If (e.KeyCode = Keys.V AndAlso e.Modifiers = Keys.Control) OrElse (e.KeyCode = Keys.Insert AndAlso e.Modifiers = Keys.Shift) Then
            Dim ClipText As String = Clipboard.GetText
            If ClipText.Length < 200 Then
                For X As Integer = 1 To ClipText.Length
                    ClipText = Regex.Replace(ClipText, "[^A-Z0-9|\-\,\.]", String.Empty, RegexOptions.IgnoreCase)
                Next

                If Clipboard.GetText <> ClipText Then
                    If ClipText.Length > 0 Then
                        Clipboard.SetText(ClipText)
                    Else
                        Clipboard.Clear()
                    End If
                End If
            Else
                e.Handled = True
            End If
        End If

    End Sub

    Private Sub ProcedureCodeTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles ProcedureCodeTextBox.KeyDown
        If (e.KeyCode = Keys.V AndAlso e.Modifiers = Keys.Control) OrElse (e.KeyCode = Keys.Insert AndAlso e.Modifiers = Keys.Shift) Then
            Dim ClipText As String = Clipboard.GetText
            If ClipText.Length < 200 Then
                For X As Integer = 1 To ClipText.Length
                    ClipText = Regex.Replace(ClipText, "[^A-Z0-9|\-\,]", String.Empty, RegexOptions.IgnoreCase)
                Next

                If Clipboard.GetText <> ClipText Then
                    If ClipText.Length > 0 Then
                        Clipboard.SetText(ClipText)
                    Else
                        Clipboard.Clear()
                    End If
                End If
            Else
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub FamilyIDTextBox_HandleCreated(sender As Object, e As EventArgs) Handles FamilyIDTextBox.HandleCreated

    End Sub

    Private Sub DocIDTextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DocIDTextBox.KeyPress
        'if typing allow number or dash(-)
        Dim EntryOkRegex As Regex = New Regex("^[0-9]+$")

        If Not (System.Char.IsControl(e.KeyChar)) AndAlso Not EntryOkRegex.IsMatch(e.KeyChar) Then
            e.Handled = True
        End If

    End Sub
End Class

#Region "BackThread Classes"
Public Class ExecuteSearch

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _NodeList As XmlNodeList
    Private _EmployeeAccess As Boolean
    Private _LocalEmployeeAccess As Boolean
    Private _ResultSet As DataTable

    Public ReadOnly Property ResultSet As DataTable
        Get
            Return _ResultSet
        End Get
    End Property
    Sub New(ByVal NodeList As XmlNodeList, ByVal EmployeeAccess As Boolean)
        _NodeList = NodeList
        _EmployeeAccess = EmployeeAccess
        _LocalEmployeeAccess = False
    End Sub

    Sub New(ByVal NodeList As XmlNodeList, ByVal EmployeeAccess As Boolean, ByVal LocalEmployeeAccess As Boolean)
        _NodeList = NodeList
        _EmployeeAccess = EmployeeAccess
        _LocalEmployeeAccess = LocalEmployeeAccess
    End Sub

    Public Sub Execute()
        Try
            Using WC As New GlobalCursor
                _ResultSet = CMSDALFDBMD.RetrieveSelection(_NodeList, _EmployeeAccess, _LocalEmployeeAccess)
            End Using

        Catch ex As DB2Exception

            If ex.SQLState.Contains("40001") Then
                MessageBox.Show("Search did not complete in allocated time, add additional criteria if possible and retry search.", "Please refine Search Criteria", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Else
                Throw
            End If

        Catch ex As Exception

            Throw

        Finally
        End Try
    End Sub
End Class

Public Class PopulateControls

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer
    Private _RelationID As Integer

    Sub New(ByVal FamilyID As Integer, ByVal RelationID As Integer)
        _FamilyID = FamilyID
        _RelationID = RelationID
    End Sub

    Public Shared Sub Execute()
        Try

        Catch ex As DB2Exception

            If ex.SQLState.Contains("40001") Then
                MessageBox.Show("Search did not complete in allocated time, add additional criteria if possible and retry search.", "Please refine Search Criteria", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Else

                Throw
            End If

        Catch ex As Exception

            Throw

        End Try
    End Sub
End Class

#End Region