Imports System.Xml
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Imports System.Data.Common

Public Class AuditDetailsControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.

            If components IsNot Nothing Then
                components.Dispose()
            End If

            _DontAddList = Nothing

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
    Friend WithEvents MoreInfoTextBox As System.Windows.Forms.TextBox
    Friend WithEvents OtherInfoLabel As System.Windows.Forms.Label
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Friend WithEvents CancelButton As System.Windows.Forms.Button
    Public WithEvents OtherTextBox As System.Windows.Forms.TextBox
    Friend WithEvents HospitalInpatientCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents HospitalOutpatientCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents SurgeonCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents AssistantSurgeonCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents AnesthetistCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents DrVisitHospitalCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents DrVisitOfficeCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents ChiropracticCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents PhysicalTherapyCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents AccidentCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents VisionHearingAidCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents NoFollowUpCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents AdjusterNeededMoreInfoCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents DuplicateCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents WrongPayeeCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents OmittedToProcessCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents OmittedToPayCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents IndustryDualCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents COBOtherCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents DualCoverageDocCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents AnnualClaimCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents FileAdjustmentCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents FailureToMiscCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents RVPNumberCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents ProcessedUnderWrongClaimCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents WrongSSNCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents WrongDoSCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents FailureToComplyCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents OtherCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents XrayLabCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents RetireeMaximumsdonotapplyonthisclaimCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents AnnualmaximumappliesCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents FailuretorouteCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents FailuretoUpdateCOBScreenCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents WorkingSpousePenaltyAppliesCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents MentalHealthSwitchCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents IncorrectPricedAmountCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents IncorrectUnitsOfServiceCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents OmittedModifierCodeCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents IncorrectAccumulatorUpdateCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents OutofStateClaimCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents IncorrectChargeCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents NonParProviderCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents NonParSurgicenterCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents MedicareCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents ProceduralErrorCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents PaymentErrorCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents PPOIndicatorIncorrectCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents WrongClaimantCheckBox As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.HospitalInpatientCheckBox = New System.Windows.Forms.CheckBox()
        Me.HospitalOutpatientCheckBox = New System.Windows.Forms.CheckBox()
        Me.SurgeonCheckBox = New System.Windows.Forms.CheckBox()
        Me.AssistantSurgeonCheckBox = New System.Windows.Forms.CheckBox()
        Me.AnesthetistCheckBox = New System.Windows.Forms.CheckBox()
        Me.DrVisitHospitalCheckBox = New System.Windows.Forms.CheckBox()
        Me.DrVisitOfficeCheckBox = New System.Windows.Forms.CheckBox()
        Me.ChiropracticCheckBox = New System.Windows.Forms.CheckBox()
        Me.PhysicalTherapyCheckBox = New System.Windows.Forms.CheckBox()
        Me.AccidentCheckBox = New System.Windows.Forms.CheckBox()
        Me.VisionHearingAidCheckBox = New System.Windows.Forms.CheckBox()
        Me.NoFollowUpCheckBox = New System.Windows.Forms.CheckBox()
        Me.AdjusterNeededMoreInfoCheckBox = New System.Windows.Forms.CheckBox()
        Me.DuplicateCheckBox = New System.Windows.Forms.CheckBox()
        Me.WrongPayeeCheckBox = New System.Windows.Forms.CheckBox()
        Me.WrongClaimantCheckBox = New System.Windows.Forms.CheckBox()
        Me.OmittedToProcessCheckBox = New System.Windows.Forms.CheckBox()
        Me.OmittedToPayCheckBox = New System.Windows.Forms.CheckBox()
        Me.IndustryDualCheckBox = New System.Windows.Forms.CheckBox()
        Me.COBOtherCheckBox = New System.Windows.Forms.CheckBox()
        Me.DualCoverageDocCheckBox = New System.Windows.Forms.CheckBox()
        Me.AnnualClaimCheckBox = New System.Windows.Forms.CheckBox()
        Me.FileAdjustmentCheckBox = New System.Windows.Forms.CheckBox()
        Me.FailureToMiscCheckBox = New System.Windows.Forms.CheckBox()
        Me.RVPNumberCheckBox = New System.Windows.Forms.CheckBox()
        Me.ProcessedUnderWrongClaimCheckBox = New System.Windows.Forms.CheckBox()
        Me.WrongSSNCheckBox = New System.Windows.Forms.CheckBox()
        Me.WrongDoSCheckBox = New System.Windows.Forms.CheckBox()
        Me.FailureToComplyCheckBox = New System.Windows.Forms.CheckBox()
        Me.OtherCheckBox = New System.Windows.Forms.CheckBox()
        Me.OtherTextBox = New System.Windows.Forms.TextBox()
        Me.MoreInfoTextBox = New System.Windows.Forms.TextBox()
        Me.OtherInfoLabel = New System.Windows.Forms.Label()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.CancelButton = New System.Windows.Forms.Button()
        Me.XrayLabCheckBox = New System.Windows.Forms.CheckBox()
        Me.RetireeMaximumsdonotapplyonthisclaimCheckBox = New System.Windows.Forms.CheckBox()
        Me.AnnualmaximumappliesCheckBox = New System.Windows.Forms.CheckBox()
        Me.FailuretorouteCheckBox = New System.Windows.Forms.CheckBox()
        Me.FailuretoUpdateCOBScreenCheckBox = New System.Windows.Forms.CheckBox()
        Me.WorkingSpousePenaltyAppliesCheckBox = New System.Windows.Forms.CheckBox()
        Me.MentalHealthSwitchCheckBox = New System.Windows.Forms.CheckBox()
        Me.IncorrectPricedAmountCheckBox = New System.Windows.Forms.CheckBox()
        Me.IncorrectUnitsOfServiceCheckBox = New System.Windows.Forms.CheckBox()
        Me.OmittedModifierCodeCheckBox = New System.Windows.Forms.CheckBox()
        Me.IncorrectAccumulatorUpdateCheckBox = New System.Windows.Forms.CheckBox()
        Me.OutofStateClaimCheckBox = New System.Windows.Forms.CheckBox()
        Me.IncorrectChargeCheckBox = New System.Windows.Forms.CheckBox()
        Me.NonParProviderCheckBox = New System.Windows.Forms.CheckBox()
        Me.NonParSurgicenterCheckBox = New System.Windows.Forms.CheckBox()
        Me.MedicareCheckBox = New System.Windows.Forms.CheckBox()
        Me.ProceduralErrorCheckBox = New System.Windows.Forms.CheckBox()
        Me.PaymentErrorCheckBox = New System.Windows.Forms.CheckBox()
        Me.PPOIndicatorIncorrectCheckBox = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'HospitalInpatientCheckBox
        '
        Me.HospitalInpatientCheckBox.Location = New System.Drawing.Point(8, 16)
        Me.HospitalInpatientCheckBox.Name = "HospitalInpatientCheckBox"
        Me.HospitalInpatientCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.HospitalInpatientCheckBox.TabIndex = 0
        Me.HospitalInpatientCheckBox.Tag = "Hospital Inpatient"
        Me.HospitalInpatientCheckBox.Text = "Hospital Inpatient" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(No certificate on file)"
        '
        'HospitalOutpatientCheckBox
        '
        Me.HospitalOutpatientCheckBox.Location = New System.Drawing.Point(8, 53)
        Me.HospitalOutpatientCheckBox.Name = "HospitalOutpatientCheckBox"
        Me.HospitalOutpatientCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.HospitalOutpatientCheckBox.TabIndex = 1
        Me.HospitalOutpatientCheckBox.Tag = "Hospital Outpatient"
        Me.HospitalOutpatientCheckBox.Text = "Hospital Outpatient"
        '
        'SurgeonCheckBox
        '
        Me.SurgeonCheckBox.Location = New System.Drawing.Point(8, 84)
        Me.SurgeonCheckBox.Name = "SurgeonCheckBox"
        Me.SurgeonCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.SurgeonCheckBox.TabIndex = 2
        Me.SurgeonCheckBox.Tag = "Surgeon"
        Me.SurgeonCheckBox.Text = "Surgeon"
        '
        'AssistantSurgeonCheckBox
        '
        Me.AssistantSurgeonCheckBox.Location = New System.Drawing.Point(8, 115)
        Me.AssistantSurgeonCheckBox.Name = "AssistantSurgeonCheckBox"
        Me.AssistantSurgeonCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.AssistantSurgeonCheckBox.TabIndex = 3
        Me.AssistantSurgeonCheckBox.Tag = "Assistant Surgeon"
        Me.AssistantSurgeonCheckBox.Text = "Assistant Surgeon"
        '
        'AnesthetistCheckBox
        '
        Me.AnesthetistCheckBox.Location = New System.Drawing.Point(8, 146)
        Me.AnesthetistCheckBox.Name = "AnesthetistCheckBox"
        Me.AnesthetistCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.AnesthetistCheckBox.TabIndex = 4
        Me.AnesthetistCheckBox.Tag = "Anesthetist"
        Me.AnesthetistCheckBox.Text = "Anesthetist"
        '
        'DrVisitHospitalCheckBox
        '
        Me.DrVisitHospitalCheckBox.Location = New System.Drawing.Point(8, 177)
        Me.DrVisitHospitalCheckBox.Name = "DrVisitHospitalCheckBox"
        Me.DrVisitHospitalCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.DrVisitHospitalCheckBox.TabIndex = 5
        Me.DrVisitHospitalCheckBox.Tag = "Dr. Visit Hospital"
        Me.DrVisitHospitalCheckBox.Text = "Dr. Visit Hospital"
        '
        'DrVisitOfficeCheckBox
        '
        Me.DrVisitOfficeCheckBox.Location = New System.Drawing.Point(8, 208)
        Me.DrVisitOfficeCheckBox.Name = "DrVisitOfficeCheckBox"
        Me.DrVisitOfficeCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.DrVisitOfficeCheckBox.TabIndex = 6
        Me.DrVisitOfficeCheckBox.Tag = "Dr. Visit Office"
        Me.DrVisitOfficeCheckBox.Text = "Dr. Visit Office"
        '
        'ChiropracticCheckBox
        '
        Me.ChiropracticCheckBox.Location = New System.Drawing.Point(8, 239)
        Me.ChiropracticCheckBox.Name = "ChiropracticCheckBox"
        Me.ChiropracticCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.ChiropracticCheckBox.TabIndex = 7
        Me.ChiropracticCheckBox.Tag = "Chiropractic"
        Me.ChiropracticCheckBox.Text = "Chiropractic Switch"
        '
        'PhysicalTherapyCheckBox
        '
        Me.PhysicalTherapyCheckBox.Location = New System.Drawing.Point(8, 270)
        Me.PhysicalTherapyCheckBox.Name = "PhysicalTherapyCheckBox"
        Me.PhysicalTherapyCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.PhysicalTherapyCheckBox.TabIndex = 8
        Me.PhysicalTherapyCheckBox.Tag = "Physical Therapy"
        Me.PhysicalTherapyCheckBox.Text = "Physical Therapy freetext not updated"
        '
        'AccidentCheckBox
        '
        Me.AccidentCheckBox.Location = New System.Drawing.Point(8, 332)
        Me.AccidentCheckBox.Name = "AccidentCheckBox"
        Me.AccidentCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.AccidentCheckBox.TabIndex = 10
        Me.AccidentCheckBox.Tag = "Accident"
        Me.AccidentCheckBox.Text = "Accident (Incorrect accident accumulator chosen)"
        '
        'VisionHearingAidCheckBox
        '
        Me.VisionHearingAidCheckBox.Location = New System.Drawing.Point(8, 363)
        Me.VisionHearingAidCheckBox.Name = "VisionHearingAidCheckBox"
        Me.VisionHearingAidCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.VisionHearingAidCheckBox.TabIndex = 11
        Me.VisionHearingAidCheckBox.Tag = "Vision-Hearing Aid"
        Me.VisionHearingAidCheckBox.Text = "Vision and Hearing-Aid"
        '
        'NoFollowUpCheckBox
        '
        Me.NoFollowUpCheckBox.Location = New System.Drawing.Point(8, 394)
        Me.NoFollowUpCheckBox.Name = "NoFollowUpCheckBox"
        Me.NoFollowUpCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.NoFollowUpCheckBox.TabIndex = 12
        Me.NoFollowUpCheckBox.Tag = "No Follow Up"
        Me.NoFollowUpCheckBox.Text = "No Follow-up of Pended Claim"
        '
        'AdjusterNeededMoreInfoCheckBox
        '
        Me.AdjusterNeededMoreInfoCheckBox.Location = New System.Drawing.Point(8, 433)
        Me.AdjusterNeededMoreInfoCheckBox.Name = "AdjusterNeededMoreInfoCheckBox"
        Me.AdjusterNeededMoreInfoCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.AdjusterNeededMoreInfoCheckBox.TabIndex = 13
        Me.AdjusterNeededMoreInfoCheckBox.Tag = "Adjuster Needed More Info"
        Me.AdjusterNeededMoreInfoCheckBox.Text = "Adjuster Needed More Info"
        '
        'DuplicateCheckBox
        '
        Me.DuplicateCheckBox.Location = New System.Drawing.Point(8, 472)
        Me.DuplicateCheckBox.Name = "DuplicateCheckBox"
        Me.DuplicateCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.DuplicateCheckBox.TabIndex = 14
        Me.DuplicateCheckBox.Tag = "Duplicate"
        Me.DuplicateCheckBox.Text = "Duplicate Payment/Processing"
        '
        'WrongPayeeCheckBox
        '
        Me.WrongPayeeCheckBox.Location = New System.Drawing.Point(209, 16)
        Me.WrongPayeeCheckBox.Name = "WrongPayeeCheckBox"
        Me.WrongPayeeCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.WrongPayeeCheckBox.TabIndex = 15
        Me.WrongPayeeCheckBox.Tag = "Wrong Payee"
        Me.WrongPayeeCheckBox.Text = "Wrong Payee"
        '
        'WrongClaimantCheckBox
        '
        Me.WrongClaimantCheckBox.Location = New System.Drawing.Point(209, 53)
        Me.WrongClaimantCheckBox.Name = "WrongClaimantCheckBox"
        Me.WrongClaimantCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.WrongClaimantCheckBox.TabIndex = 16
        Me.WrongClaimantCheckBox.Tag = "Wrong Claimant"
        Me.WrongClaimantCheckBox.Text = "Wrong Claimant"
        '
        'OmittedToProcessCheckBox
        '
        Me.OmittedToProcessCheckBox.Location = New System.Drawing.Point(209, 84)
        Me.OmittedToProcessCheckBox.Name = "OmittedToProcessCheckBox"
        Me.OmittedToProcessCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.OmittedToProcessCheckBox.TabIndex = 17
        Me.OmittedToProcessCheckBox.Tag = "Omitted To Process"
        Me.OmittedToProcessCheckBox.Text = "Omitted To Process"
        '
        'OmittedToPayCheckBox
        '
        Me.OmittedToPayCheckBox.Location = New System.Drawing.Point(209, 115)
        Me.OmittedToPayCheckBox.Name = "OmittedToPayCheckBox"
        Me.OmittedToPayCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.OmittedToPayCheckBox.TabIndex = 18
        Me.OmittedToPayCheckBox.Tag = "Omitted To Pay"
        Me.OmittedToPayCheckBox.Text = "Omitted To Pay"
        '
        'IndustryDualCheckBox
        '
        Me.IndustryDualCheckBox.Location = New System.Drawing.Point(209, 146)
        Me.IndustryDualCheckBox.Name = "IndustryDualCheckBox"
        Me.IndustryDualCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.IndustryDualCheckBox.TabIndex = 19
        Me.IndustryDualCheckBox.Tag = "Industry Dual"
        Me.IndustryDualCheckBox.Text = "Industry Dual Coverage"
        '
        'COBOtherCheckBox
        '
        Me.COBOtherCheckBox.Location = New System.Drawing.Point(209, 177)
        Me.COBOtherCheckBox.Name = "COBOtherCheckBox"
        Me.COBOtherCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.COBOtherCheckBox.TabIndex = 20
        Me.COBOtherCheckBox.Tag = "COB Other"
        Me.COBOtherCheckBox.Text = "COB - Other Coverage"
        '
        'DualCoverageDocCheckBox
        '
        Me.DualCoverageDocCheckBox.Location = New System.Drawing.Point(209, 208)
        Me.DualCoverageDocCheckBox.Name = "DualCoverageDocCheckBox"
        Me.DualCoverageDocCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.DualCoverageDocCheckBox.TabIndex = 21
        Me.DualCoverageDocCheckBox.Tag = "Dual Coverage Doc"
        Me.DualCoverageDocCheckBox.Text = "Dual Coverage Documentation"
        '
        'AnnualClaimCheckBox
        '
        Me.AnnualClaimCheckBox.Location = New System.Drawing.Point(209, 239)
        Me.AnnualClaimCheckBox.Name = "AnnualClaimCheckBox"
        Me.AnnualClaimCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.AnnualClaimCheckBox.TabIndex = 22
        Me.AnnualClaimCheckBox.Tag = "Annual Claim"
        Me.AnnualClaimCheckBox.Text = "Annual Claim Form"
        '
        'FileAdjustmentCheckBox
        '
        Me.FileAdjustmentCheckBox.Location = New System.Drawing.Point(209, 270)
        Me.FileAdjustmentCheckBox.Name = "FileAdjustmentCheckBox"
        Me.FileAdjustmentCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.FileAdjustmentCheckBox.TabIndex = 23
        Me.FileAdjustmentCheckBox.Tag = "File Adjustment"
        Me.FileAdjustmentCheckBox.Text = "File Adjustment"
        '
        'FailureToMiscCheckBox
        '
        Me.FailureToMiscCheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FailureToMiscCheckBox.Location = New System.Drawing.Point(209, 301)
        Me.FailureToMiscCheckBox.Name = "FailureToMiscCheckBox"
        Me.FailureToMiscCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.FailureToMiscCheckBox.TabIndex = 24
        Me.FailureToMiscCheckBox.Tag = "Failure to Annotate, Route for Reindexing or Send for Rescanning"
        Me.FailureToMiscCheckBox.Text = "Failure to Annotate, Route for Reindexing or Send for Rescanning"
        '
        'RVPNumberCheckBox
        '
        Me.RVPNumberCheckBox.Location = New System.Drawing.Point(209, 332)
        Me.RVPNumberCheckBox.Name = "RVPNumberCheckBox"
        Me.RVPNumberCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.RVPNumberCheckBox.TabIndex = 25
        Me.RVPNumberCheckBox.Tag = "RVP Number"
        Me.RVPNumberCheckBox.Text = "RVP Number"
        '
        'ProcessedUnderWrongClaimCheckBox
        '
        Me.ProcessedUnderWrongClaimCheckBox.Location = New System.Drawing.Point(209, 363)
        Me.ProcessedUnderWrongClaimCheckBox.Name = "ProcessedUnderWrongClaimCheckBox"
        Me.ProcessedUnderWrongClaimCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.ProcessedUnderWrongClaimCheckBox.TabIndex = 26
        Me.ProcessedUnderWrongClaimCheckBox.Tag = "Processed Under Wrong Claim"
        Me.ProcessedUnderWrongClaimCheckBox.Text = "Processed Under Wrong Claim Number"
        '
        'WrongSSNCheckBox
        '
        Me.WrongSSNCheckBox.Location = New System.Drawing.Point(209, 394)
        Me.WrongSSNCheckBox.Name = "WrongSSNCheckBox"
        Me.WrongSSNCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.WrongSSNCheckBox.TabIndex = 27
        Me.WrongSSNCheckBox.Tag = "Wrong SSN"
        Me.WrongSSNCheckBox.Text = "Wrong SSN"
        '
        'WrongDoSCheckBox
        '
        Me.WrongDoSCheckBox.Location = New System.Drawing.Point(209, 433)
        Me.WrongDoSCheckBox.Name = "WrongDoSCheckBox"
        Me.WrongDoSCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.WrongDoSCheckBox.TabIndex = 28
        Me.WrongDoSCheckBox.Tag = "Wrong DoS"
        Me.WrongDoSCheckBox.Text = "Wrong Date Of Service/Fee"
        '
        'FailureToComplyCheckBox
        '
        Me.FailureToComplyCheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.FailureToComplyCheckBox.Location = New System.Drawing.Point(209, 470)
        Me.FailureToComplyCheckBox.Name = "FailureToComplyCheckBox"
        Me.FailureToComplyCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.FailureToComplyCheckBox.TabIndex = 29
        Me.FailureToComplyCheckBox.Tag = "Failure To Comply"
        Me.FailureToComplyCheckBox.Text = "Failure To Comply With Previous Audit Finding(s)"
        '
        'OtherCheckBox
        '
        Me.OtherCheckBox.Location = New System.Drawing.Point(603, 121)
        Me.OtherCheckBox.Name = "OtherCheckBox"
        Me.OtherCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.OtherCheckBox.TabIndex = 30
        Me.OtherCheckBox.Tag = "Other"
        Me.OtherCheckBox.Text = "Other"
        '
        'OtherTextBox
        '
        Me.OtherTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.OtherTextBox.Enabled = False
        Me.OtherTextBox.Location = New System.Drawing.Point(603, 156)
        Me.OtherTextBox.Multiline = True
        Me.OtherTextBox.Name = "OtherTextBox"
        Me.OtherTextBox.Size = New System.Drawing.Size(206, 145)
        Me.OtherTextBox.TabIndex = 31
        '
        'MoreInfoTextBox
        '
        Me.MoreInfoTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.MoreInfoTextBox.Location = New System.Drawing.Point(416, 529)
        Me.MoreInfoTextBox.Multiline = True
        Me.MoreInfoTextBox.Name = "MoreInfoTextBox"
        Me.MoreInfoTextBox.Size = New System.Drawing.Size(184, 16)
        Me.MoreInfoTextBox.TabIndex = 32
        Me.MoreInfoTextBox.Visible = False
        '
        'OtherInfoLabel
        '
        Me.OtherInfoLabel.Location = New System.Drawing.Point(416, 510)
        Me.OtherInfoLabel.Name = "OtherInfoLabel"
        Me.OtherInfoLabel.Size = New System.Drawing.Size(184, 16)
        Me.OtherInfoLabel.TabIndex = 33
        Me.OtherInfoLabel.Text = "Other Information"
        Me.OtherInfoLabel.Visible = False
        '
        'SaveButton
        '
        Me.SaveButton.Location = New System.Drawing.Point(615, 470)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(75, 23)
        Me.SaveButton.TabIndex = 34
        Me.SaveButton.Text = "Save"
        '
        'CancelButton
        '
        Me.CancelButton.Location = New System.Drawing.Point(696, 470)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelButton.TabIndex = 35
        Me.CancelButton.Text = "Cancel"
        '
        'XrayLabCheckBox
        '
        Me.XrayLabCheckBox.Location = New System.Drawing.Point(8, 301)
        Me.XrayLabCheckBox.Name = "XrayLabCheckBox"
        Me.XrayLabCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.XrayLabCheckBox.TabIndex = 9
        Me.XrayLabCheckBox.Tag = "X-Ray Lab"
        Me.XrayLabCheckBox.Text = "X-Ray and Lab"
        '
        'RetireeMaximumsdonotapplyonthisclaimCheckBox
        '
        Me.RetireeMaximumsdonotapplyonthisclaimCheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.RetireeMaximumsdonotapplyonthisclaimCheckBox.Location = New System.Drawing.Point(406, 301)
        Me.RetireeMaximumsdonotapplyonthisclaimCheckBox.Name = "RetireeMaximumsdonotapplyonthisclaimCheckBox"
        Me.RetireeMaximumsdonotapplyonthisclaimCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.RetireeMaximumsdonotapplyonthisclaimCheckBox.TabIndex = 45
        Me.RetireeMaximumsdonotapplyonthisclaimCheckBox.Tag = "Retiree - Maximums do not apply on this claim"
        Me.RetireeMaximumsdonotapplyonthisclaimCheckBox.Text = "Retiree - Maximums do not apply on this claim"
        '
        'AnnualmaximumappliesCheckBox
        '
        Me.AnnualmaximumappliesCheckBox.Location = New System.Drawing.Point(406, 270)
        Me.AnnualmaximumappliesCheckBox.Name = "AnnualmaximumappliesCheckBox"
        Me.AnnualmaximumappliesCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.AnnualmaximumappliesCheckBox.TabIndex = 44
        Me.AnnualmaximumappliesCheckBox.Tag = "Annual maximum applies"
        Me.AnnualmaximumappliesCheckBox.Text = "Annual maximum applies"
        '
        'FailuretorouteCheckBox
        '
        Me.FailuretorouteCheckBox.Location = New System.Drawing.Point(406, 239)
        Me.FailuretorouteCheckBox.Name = "FailuretorouteCheckBox"
        Me.FailuretorouteCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.FailuretorouteCheckBox.TabIndex = 43
        Me.FailuretorouteCheckBox.Tag = "Failure to route"
        Me.FailuretorouteCheckBox.Text = "Failure to route"
        '
        'FailuretoUpdateCOBScreenCheckBox
        '
        Me.FailuretoUpdateCOBScreenCheckBox.Location = New System.Drawing.Point(406, 208)
        Me.FailuretoUpdateCOBScreenCheckBox.Name = "FailuretoUpdateCOBScreenCheckBox"
        Me.FailuretoUpdateCOBScreenCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.FailuretoUpdateCOBScreenCheckBox.TabIndex = 42
        Me.FailuretoUpdateCOBScreenCheckBox.Tag = "Failure to Update COB Screen"
        Me.FailuretoUpdateCOBScreenCheckBox.Text = "Failure to Update COB Screen"
        '
        'WorkingSpousePenaltyAppliesCheckBox
        '
        Me.WorkingSpousePenaltyAppliesCheckBox.Location = New System.Drawing.Point(406, 177)
        Me.WorkingSpousePenaltyAppliesCheckBox.Name = "WorkingSpousePenaltyAppliesCheckBox"
        Me.WorkingSpousePenaltyAppliesCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.WorkingSpousePenaltyAppliesCheckBox.TabIndex = 41
        Me.WorkingSpousePenaltyAppliesCheckBox.Tag = "Working Spouse Penalty Applies"
        Me.WorkingSpousePenaltyAppliesCheckBox.Text = "Working Spouse Penalty Applies"
        '
        'MentalHealthSwitchCheckBox
        '
        Me.MentalHealthSwitchCheckBox.Location = New System.Drawing.Point(406, 146)
        Me.MentalHealthSwitchCheckBox.Name = "MentalHealthSwitchCheckBox"
        Me.MentalHealthSwitchCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.MentalHealthSwitchCheckBox.TabIndex = 40
        Me.MentalHealthSwitchCheckBox.Tag = "Mental Health Switch"
        Me.MentalHealthSwitchCheckBox.Text = "Mental Health Switch"
        '
        'IncorrectPricedAmountCheckBox
        '
        Me.IncorrectPricedAmountCheckBox.Location = New System.Drawing.Point(406, 115)
        Me.IncorrectPricedAmountCheckBox.Name = "IncorrectPricedAmountCheckBox"
        Me.IncorrectPricedAmountCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.IncorrectPricedAmountCheckBox.TabIndex = 39
        Me.IncorrectPricedAmountCheckBox.Tag = "Incorrect priced amount"
        Me.IncorrectPricedAmountCheckBox.Text = "Incorrect priced amount"
        '
        'IncorrectUnitsOfServiceCheckBox
        '
        Me.IncorrectUnitsOfServiceCheckBox.Location = New System.Drawing.Point(406, 84)
        Me.IncorrectUnitsOfServiceCheckBox.Name = "IncorrectUnitsOfServiceCheckBox"
        Me.IncorrectUnitsOfServiceCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.IncorrectUnitsOfServiceCheckBox.TabIndex = 38
        Me.IncorrectUnitsOfServiceCheckBox.Tag = "Incorrect units/number of services"
        Me.IncorrectUnitsOfServiceCheckBox.Text = "Incorrect units/number of services"
        '
        'OmittedModifierCodeCheckBox
        '
        Me.OmittedModifierCodeCheckBox.Location = New System.Drawing.Point(406, 53)
        Me.OmittedModifierCodeCheckBox.Name = "OmittedModifierCodeCheckBox"
        Me.OmittedModifierCodeCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.OmittedModifierCodeCheckBox.TabIndex = 37
        Me.OmittedModifierCodeCheckBox.Tag = "Omitted modifier code"
        Me.OmittedModifierCodeCheckBox.Text = "Omitted modifier code"
        '
        'IncorrectAccumulatorUpdateCheckBox
        '
        Me.IncorrectAccumulatorUpdateCheckBox.Location = New System.Drawing.Point(406, 16)
        Me.IncorrectAccumulatorUpdateCheckBox.Name = "IncorrectAccumulatorUpdateCheckBox"
        Me.IncorrectAccumulatorUpdateCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.IncorrectAccumulatorUpdateCheckBox.TabIndex = 36
        Me.IncorrectAccumulatorUpdateCheckBox.Tag = "Incorrect Accumulator Update"
        Me.IncorrectAccumulatorUpdateCheckBox.Text = "Incorrect Accumulator Update"
        '
        'OutofStateClaimCheckBox
        '
        Me.OutofStateClaimCheckBox.Location = New System.Drawing.Point(406, 363)
        Me.OutofStateClaimCheckBox.Name = "OutofStateClaimCheckBox"
        Me.OutofStateClaimCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.OutofStateClaimCheckBox.TabIndex = 47
        Me.OutofStateClaimCheckBox.Tag = "Out of State Claim"
        Me.OutofStateClaimCheckBox.Text = "Out of State Claim"
        '
        'IncorrectChargeCheckBox
        '
        Me.IncorrectChargeCheckBox.Location = New System.Drawing.Point(406, 332)
        Me.IncorrectChargeCheckBox.Name = "IncorrectChargeCheckBox"
        Me.IncorrectChargeCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.IncorrectChargeCheckBox.TabIndex = 46
        Me.IncorrectChargeCheckBox.Tag = "Incorrect Charge"
        Me.IncorrectChargeCheckBox.Text = "Incorrect Charge"
        '
        'NonParProviderCheckBox
        '
        Me.NonParProviderCheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.NonParProviderCheckBox.Location = New System.Drawing.Point(406, 470)
        Me.NonParProviderCheckBox.Name = "NonParProviderCheckBox"
        Me.NonParProviderCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.NonParProviderCheckBox.TabIndex = 50
        Me.NonParProviderCheckBox.Tag = "Non-Par Provider"
        Me.NonParProviderCheckBox.Text = "Non-Par Provider"
        '
        'NonParSurgicenterCheckBox
        '
        Me.NonParSurgicenterCheckBox.Location = New System.Drawing.Point(406, 433)
        Me.NonParSurgicenterCheckBox.Name = "NonParSurgicenterCheckBox"
        Me.NonParSurgicenterCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.NonParSurgicenterCheckBox.TabIndex = 49
        Me.NonParSurgicenterCheckBox.Tag = "Non-Par Surgicenter"
        Me.NonParSurgicenterCheckBox.Text = "Non-Par Surgicenter"
        '
        'MedicareCheckBox
        '
        Me.MedicareCheckBox.Location = New System.Drawing.Point(406, 394)
        Me.MedicareCheckBox.Name = "MedicareCheckBox"
        Me.MedicareCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.MedicareCheckBox.TabIndex = 48
        Me.MedicareCheckBox.Tag = "Medicare"
        Me.MedicareCheckBox.Text = "Medicare"
        '
        'ProceduralErrorCheckBox
        '
        Me.ProceduralErrorCheckBox.Location = New System.Drawing.Point(603, 84)
        Me.ProceduralErrorCheckBox.Name = "ProceduralErrorCheckBox"
        Me.ProceduralErrorCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.ProceduralErrorCheckBox.TabIndex = 53
        Me.ProceduralErrorCheckBox.Tag = "Procedural Error"
        Me.ProceduralErrorCheckBox.Text = "Procedural Error"
        '
        'PaymentErrorCheckBox
        '
        Me.PaymentErrorCheckBox.Location = New System.Drawing.Point(603, 53)
        Me.PaymentErrorCheckBox.Name = "PaymentErrorCheckBox"
        Me.PaymentErrorCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.PaymentErrorCheckBox.TabIndex = 52
        Me.PaymentErrorCheckBox.Tag = "Payment Error"
        Me.PaymentErrorCheckBox.Text = "Payment Error"
        '
        'PPOIndicatorIncorrectCheckBox
        '
        Me.PPOIndicatorIncorrectCheckBox.Location = New System.Drawing.Point(603, 16)
        Me.PPOIndicatorIncorrectCheckBox.Name = "PPOIndicatorIncorrectCheckBox"
        Me.PPOIndicatorIncorrectCheckBox.Size = New System.Drawing.Size(191, 31)
        Me.PPOIndicatorIncorrectCheckBox.TabIndex = 51
        Me.PPOIndicatorIncorrectCheckBox.Tag = "PPO Indicator Incorrect"
        Me.PPOIndicatorIncorrectCheckBox.Text = "PPO Indicator Incorrect"
        '
        'AuditDetailsControl
        '
        Me.Controls.Add(Me.ProceduralErrorCheckBox)
        Me.Controls.Add(Me.PaymentErrorCheckBox)
        Me.Controls.Add(Me.PPOIndicatorIncorrectCheckBox)
        Me.Controls.Add(Me.NonParProviderCheckBox)
        Me.Controls.Add(Me.NonParSurgicenterCheckBox)
        Me.Controls.Add(Me.MedicareCheckBox)
        Me.Controls.Add(Me.OutofStateClaimCheckBox)
        Me.Controls.Add(Me.IncorrectChargeCheckBox)
        Me.Controls.Add(Me.RetireeMaximumsdonotapplyonthisclaimCheckBox)
        Me.Controls.Add(Me.AnnualmaximumappliesCheckBox)
        Me.Controls.Add(Me.FailuretorouteCheckBox)
        Me.Controls.Add(Me.FailuretoUpdateCOBScreenCheckBox)
        Me.Controls.Add(Me.WorkingSpousePenaltyAppliesCheckBox)
        Me.Controls.Add(Me.MentalHealthSwitchCheckBox)
        Me.Controls.Add(Me.IncorrectPricedAmountCheckBox)
        Me.Controls.Add(Me.IncorrectUnitsOfServiceCheckBox)
        Me.Controls.Add(Me.OmittedModifierCodeCheckBox)
        Me.Controls.Add(Me.IncorrectAccumulatorUpdateCheckBox)
        Me.Controls.Add(Me.CancelButton)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.OtherInfoLabel)
        Me.Controls.Add(Me.MoreInfoTextBox)
        Me.Controls.Add(Me.OtherTextBox)
        Me.Controls.Add(Me.OtherCheckBox)
        Me.Controls.Add(Me.FailureToComplyCheckBox)
        Me.Controls.Add(Me.WrongDoSCheckBox)
        Me.Controls.Add(Me.WrongSSNCheckBox)
        Me.Controls.Add(Me.ProcessedUnderWrongClaimCheckBox)
        Me.Controls.Add(Me.RVPNumberCheckBox)
        Me.Controls.Add(Me.FailureToMiscCheckBox)
        Me.Controls.Add(Me.FileAdjustmentCheckBox)
        Me.Controls.Add(Me.AnnualClaimCheckBox)
        Me.Controls.Add(Me.DualCoverageDocCheckBox)
        Me.Controls.Add(Me.COBOtherCheckBox)
        Me.Controls.Add(Me.IndustryDualCheckBox)
        Me.Controls.Add(Me.OmittedToPayCheckBox)
        Me.Controls.Add(Me.OmittedToProcessCheckBox)
        Me.Controls.Add(Me.WrongClaimantCheckBox)
        Me.Controls.Add(Me.WrongPayeeCheckBox)
        Me.Controls.Add(Me.DuplicateCheckBox)
        Me.Controls.Add(Me.AdjusterNeededMoreInfoCheckBox)
        Me.Controls.Add(Me.NoFollowUpCheckBox)
        Me.Controls.Add(Me.VisionHearingAidCheckBox)
        Me.Controls.Add(Me.AccidentCheckBox)
        Me.Controls.Add(Me.XrayLabCheckBox)
        Me.Controls.Add(Me.PhysicalTherapyCheckBox)
        Me.Controls.Add(Me.ChiropracticCheckBox)
        Me.Controls.Add(Me.DrVisitOfficeCheckBox)
        Me.Controls.Add(Me.DrVisitHospitalCheckBox)
        Me.Controls.Add(Me.AnesthetistCheckBox)
        Me.Controls.Add(Me.AssistantSurgeonCheckBox)
        Me.Controls.Add(Me.SurgeonCheckBox)
        Me.Controls.Add(Me.HospitalOutpatientCheckBox)
        Me.Controls.Add(Me.HospitalInpatientCheckBox)
        Me.MaximumSize = New System.Drawing.Size(812, 552)
        Me.MinimumSize = New System.Drawing.Size(812, 552)
        Me.Name = "AuditDetailsControl"
        Me.Size = New System.Drawing.Size(812, 552)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

#Region "Private Variables and Properties"
    Private _ClaimID As String
    Private _Adjuster As String
    Private _DontAddList As ArrayList
    Private _bizUserPrincipal As System.Security.Principal.WindowsPrincipal
    Public Enum AuditStatus
        Release
        Route
    End Enum
    Public Enum CloseStatus
        Cancel
        OK
    End Enum
    Private _status As AuditStatus
    Public Event Close(ByVal Result As CloseStatus)
    Public Property ClaimID() As String
        Get
            Return _ClaimID
        End Get
        Set(ByVal value As String)
            _ClaimID = Value
            '#If Debug = False Then
            If Not Value Is Nothing Then
                RefreshForm()
            End If
            '#End If
        End Set
    End Property

    Public Property Adjuster() As String
        Get
            Return _Adjuster
        End Get
        Set(ByVal value As String)
            _Adjuster = Value
        End Set
    End Property

    Public ReadOnly Property Status() As AuditStatus
        Get
            Return _status
        End Get
    End Property
#End Region

#Region "Form and Control Events"
    Private Sub AuditDetailsControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        AppDomain.CurrentDomain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.WindowsPrincipal)
        _bizUserPrincipal = CType(System.Threading.Thread.CurrentPrincipal, System.Security.Principal.WindowsPrincipal)
    End Sub

    Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        SetStatus()
        RaiseEvent Close(CloseStatus.Cancel)
    End Sub

    Private Sub SaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveButton.Click

        Dim Transaction As DbTransaction
        Dim XMLDoc As XmlDocument

        Try
            Using WC As New GlobalCursor

                XMLDoc = GetAuditInfoInXmlFormat()

                Transaction = CMSDALCommon.BeginTransaction

                CMSDALFDBMD.SaveAuditInformation(XMLDoc.SelectNodes("//MetaData"), XMLDoc.SelectNodes("//AuditPieces"), Transaction)

                CMSDALCommon.CommitTransaction(Transaction)

                RefreshForm()
                SetStatus()

                RaiseEvent Close(CloseStatus.OK)

            End Using


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
    Private Sub CheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HospitalInpatientCheckBox.CheckStateChanged, _
                                                                                                            HospitalOutpatientCheckBox.CheckStateChanged, _
                                                                                                            SurgeonCheckBox.CheckStateChanged, _
                                                                                                            AssistantSurgeonCheckBox.CheckStateChanged, _
                                                                                                            AnesthetistCheckBox.CheckStateChanged, _
                                                                                                            DrVisitHospitalCheckBox.CheckStateChanged, _
                                                                                                            DrVisitOfficeCheckBox.CheckStateChanged, _
                                                                                                            ChiropracticCheckBox.CheckStateChanged, _
                                                                                                            PhysicalTherapyCheckBox.CheckStateChanged, _
                                                                                                            AccidentCheckBox.CheckStateChanged, _
                                                                                                            VisionHearingAidCheckBox.CheckStateChanged, _
                                                                                                            NoFollowUpCheckBox.CheckStateChanged, _
                                                                                                            AdjusterNeededMoreInfoCheckBox.CheckStateChanged, _
                                                                                                            DuplicateCheckBox.CheckStateChanged, _
                                                                                                            WrongPayeeCheckBox.CheckStateChanged, _
                                                                                                            OmittedToProcessCheckBox.CheckStateChanged, _
                                                                                                            OmittedToPayCheckBox.CheckStateChanged, _
                                                                                                            IndustryDualCheckBox.CheckStateChanged, _
                                                                                                            COBOtherCheckBox.CheckStateChanged, _
                                                                                                            DualCoverageDocCheckBox.CheckStateChanged, _
                                                                                                            AnnualClaimCheckBox.CheckStateChanged, _
                                                                                                            FileAdjustmentCheckBox.CheckStateChanged, _
                                                                                                            FailureToMiscCheckBox.CheckStateChanged, _
                                                                                                            RVPNumberCheckBox.CheckStateChanged, _
                                                                                                            ProcessedUnderWrongClaimCheckBox.CheckStateChanged, _
                                                                                                            WrongSSNCheckBox.CheckStateChanged, _
                                                                                                            WrongDoSCheckBox.CheckStateChanged, _
                                                                                                            FailureToComplyCheckBox.CheckStateChanged, _
                                                                                                            XrayLabCheckBox.CheckStateChanged, _
                                                                                                            RetireeMaximumsdonotapplyonthisclaimCheckBox.CheckStateChanged, _
                                                                                                            AnnualmaximumappliesCheckBox.CheckStateChanged, _
                                                                                                            FailuretorouteCheckBox.CheckStateChanged, _
                                                                                                            FailuretoUpdateCOBScreenCheckBox.CheckStateChanged, _
                                                                                                            WorkingSpousePenaltyAppliesCheckBox.CheckStateChanged, _
                                                                                                            MentalHealthSwitchCheckBox.CheckStateChanged, _
                                                                                                            IncorrectPricedAmountCheckBox.CheckStateChanged, _
                                                                                                            IncorrectUnitsOfServiceCheckBox.CheckStateChanged, _
                                                                                                            OmittedModifierCodeCheckBox.CheckStateChanged, _
                                                                                                            IncorrectAccumulatorUpdateCheckBox.CheckStateChanged, _
                                                                                                            OutofStateClaimCheckBox.CheckStateChanged, _
                                                                                                            IncorrectChargeCheckBox.CheckStateChanged, _
                                                                                                            NonParProviderCheckBox.CheckStateChanged, _
                                                                                                            NonParSurgicenterCheckBox.CheckStateChanged, _
                                                                                                            MedicareCheckBox.CheckStateChanged, _
                                                                                                            ProceduralErrorCheckBox.CheckStateChanged, _
                                                                                                            PaymentErrorCheckBox.CheckStateChanged, _
                                                                                                            PPOIndicatorIncorrectCheckBox.CheckStateChanged, _
                                                                                                            WrongClaimantCheckBox.CheckStateChanged
        If CType(sender, CheckBox).Checked Then
            _DontAddList.Remove(CType(sender, CheckBox).Name.ToString)
        Else
            _DontAddList.Add(CType(sender, CheckBox).Name.ToString)
        End If

    End Sub

    Private Sub OtherCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OtherCheckBox.CheckedChanged
        Me.OtherTextBox.Enabled = OtherCheckBox.Checked
        If OtherCheckBox.Checked Then
            _DontAddList.Remove("OtherCheckBox")
        Else
            _DontAddList.Add("OtherCheckBox")
        End If
    End Sub

#End Region

#Region "Misc"
    Private Sub SetStatus()
        Dim AuditCount As Integer = 0

        For Each o As Control In Me.Controls
            If TypeOf o Is CheckBox Then
                If CType(o, CheckBox).Checked = True Then
                    AuditCount += 1
                End If
            End If
        Next

        If AuditCount > 0 Then
            _status = AuditStatus.Route
        Else
            _status = AuditStatus.Release
        End If

    End Sub

    Public Function GetAllCheckBoxControls() As CheckBox()
        Dim AuditChoice As Integer = 0

        For Each o As Control In Me.Controls
            If TypeOf o Is CheckBox AndAlso CType(o, CheckBox).CheckState = CheckState.Checked Then
                AuditChoice += 1
            End If
        Next

        Dim chkBxs() As CheckBox
        ReDim chkBxs(AuditChoice)

        Dim i As Integer = 0

        For Each o As Control In Me.Controls
            If TypeOf o Is CheckBox AndAlso CType(o, CheckBox).CheckState = CheckState.Checked Then
                chkBxs(i) = CType(o, CheckBox)
                i += 1
            End If
        Next

        Return chkBxs

    End Function

    Private Sub RefreshForm()

        Dim DT As DataTable = CMSDALFDBMD.RetrieveAuditInformation(CInt(Me._ClaimID))

        Application.DoEvents()

        PopulateForm(dt)

        SetStatus()

    End Sub
    Private Sub ClearForm()
        _DontAddList = New ArrayList

        For Each o As Control In Me.Controls
            If TypeOf o Is CheckBox Then
                CType(o, CheckBox).Checked = False
            End If
        Next

        Me.OtherTextBox.Text = ""
        MoreInfoTextBox.Text = ""

    End Sub
    Private Sub PopulateForm(ByVal DT As DataTable)

        ClearForm()

        Dim DR As DataRow

        For i As Integer = 0 To dt.Rows.Count - 1
            dr = dt.Rows(i)

            Select Case dr("AUDIT_CATEGORY_NAME").ToString
                Case "Other"
                    For Each o As Control In Me.Controls
                        If TypeOf o Is CheckBox Then
                            If CType(o, CheckBox).Tag.ToString = dr("AUDIT_CATEGORY_NAME").ToString AndAlso dr("AUDIT_TEXT").ToString.Trim.Length > 0 Then
                                CType(o, CheckBox).Checked = True
                                Me.OtherTextBox.Text = dr("AUDIT_TEXT").ToString
                                Exit For
                            End If
                        End If
                    Next

                Case "More Information"
                    MoreInfoTextBox.Text = dr("AUDIT_TEXT").ToString.Trim
                Case Else
                    For Each o As Control In Me.Controls
                        If TypeOf o Is CheckBox Then
                            If CType(o, CheckBox).Tag.ToString = dr("AUDIT_CATEGORY_NAME").ToString Then
                                CType(o, CheckBox).Checked = CBool(dr("AUDIT_TEXT"))
                                Exit For
                            End If
                        End If
                    Next
            End Select

        Next

    End Sub
    Private Function GetAuditInfoInXmlFormat() As XmlDocument
        Dim xmlDoc As New XmlDocument
        Dim criterionRootElem As XmlElement
        Dim metaDataRootElem As XmlElement
        Dim criterionElem As XmlElement
        Dim metaDataElem As XmlElement

        xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "no"))

        Dim mainDataRootElem As XmlElement = xmlDoc.CreateElement("AuditData")
        xmlDoc.AppendChild(mainDataRootElem)

        metaDataRootElem = xmlDoc.CreateElement("MetaData")
        mainDataRootElem.AppendChild(metaDataRootElem)

        metaDataElem = xmlDoc.CreateElement("CLAIM_ID")
        metaDataElem.SetAttribute("Name", "CLAIM_ID")
        metaDataElem.SetAttribute("Value", Me._ClaimID)
        metaDataRootElem.AppendChild(metaDataElem)

        metaDataElem = xmlDoc.CreateElement("AUDITOR")
        metaDataElem.SetAttribute("Name", "AUDITOR")
        metaDataElem.SetAttribute("Value", _bizUserPrincipal.Identity.Name)
        metaDataRootElem.AppendChild(metaDataElem)

        metaDataElem = xmlDoc.CreateElement("AUDIT_DATE")
        metaDataElem.SetAttribute("Name", "AUDIT_DATE")
        metaDataElem.SetAttribute("Value", UFCWGeneral.NowDate.ToString)
        metaDataRootElem.AppendChild(metaDataElem)

        metaDataElem = xmlDoc.CreateElement("ADJUSTER")
        metaDataElem.SetAttribute("Name", "ADJUSTER")
        metaDataElem.SetAttribute("Value", Me._Adjuster)
        metaDataRootElem.AppendChild(metaDataElem)

        criterionRootElem = xmlDoc.CreateElement("AuditPieces")
        mainDataRootElem.AppendChild(criterionRootElem)

        AddAuditSelectionsInXmlFormat(xmlDoc, criterionRootElem)

        criterionElem = xmlDoc.CreateElement("AuditInfo")
        criterionElem.SetAttribute("Name", "More Information")
        criterionElem.SetAttribute("Value", Me.MoreInfoTextBox.Text.Replace("'"c, "`"c))
        criterionRootElem.AppendChild(criterionElem)

        Return xmlDoc
    End Function

    Private Sub AddAuditSelectionsInXmlFormat(ByRef xmlDoc As XmlDocument, ByRef criterionRootElem As XmlElement)

        Dim criterionElem As XmlElement

        For Each o As Control In Me.Controls
            If TypeOf o Is CheckBox Then
                Dim chk As CheckBox
                chk = DirectCast(o, CheckBox)

                If chk.Checked AndAlso Not _DontAddList.Contains(chk.Name) AndAlso chk.Tag IsNot Nothing AndAlso chk.Tag.ToString.Trim.Length > 0 Then
                    criterionElem = xmlDoc.CreateElement("AuditInfo")
                    criterionElem.SetAttribute("Name", chk.Tag.ToString)
                    If chk.Tag.ToString = "Other" Then
                        criterionElem.SetAttribute("Value", Me.OtherTextBox.Text.Trim)
                    Else
                        criterionElem.SetAttribute("Value", "True")
                    End If
                    criterionRootElem.AppendChild(criterionElem)
                ElseIf _DontAddList.Contains(chk.Name) Then
                    criterionElem = xmlDoc.CreateElement("AuditInfo")
                    criterionElem.SetAttribute("Name", chk.Tag.ToString)
                    If chk.Tag.ToString = "Other" Then
                        criterionElem.SetAttribute("Value", "")
                    Else
                        criterionElem.SetAttribute("Value", "False")
                    End If
                    criterionRootElem.AppendChild(criterionElem)
                End If

            End If
        Next

    End Sub

#End Region

End Class