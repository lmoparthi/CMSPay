Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Windows.Forms
Imports System.ComponentModel

<SharedInterfaces.PlugIn("Customer Service", 1, "Main", 0)> Public Class CustomerServicePlugIn
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"

    Private _Message As SharedInterfaces.IMessage
    Private _OpenIndex As Integer
    Private _UniqueID As String

#Region " Windows Form Designer generated code "
    Public Sub New(ByVal objMsg As SharedInterfaces.IMessage, ByVal openIndex As Integer)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _Message = objMsg
        _OpenIndex = openIndex

    End Sub

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents CustServiceControl As CustomerServiceControl
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim Resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CustomerServicePlugIn))
        Me.CustServiceControl = New CustomerServiceControl()
        Me.SuspendLayout()
        '
        'CustServiceControl
        '
        Me.CustServiceControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CustServiceControl.Location = New System.Drawing.Point(0, 0)
        Me.CustServiceControl.Name = "CustServiceControl"
        Me.CustServiceControl.Size = New System.Drawing.Size(724, 698)
        Me.CustServiceControl.TabIndex = 0
        '
        'CustomerService
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(720, 710)
        Me.Controls.Add(Me.CustServiceControl)
        Me.Icon = CType(Resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(528, 616)
        Me.Name = "CustomerService"
        Me.Text = "Customer Service"
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Properties"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' this property gets or sets a unique id suppilied by the calling form
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	3/24/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property UniqueID() As String
        Get
            Return _UniqueID
        End Get
        Set(ByVal value As String)
            _UniqueID = value
        End Set
    End Property

    <DefaultValue(CBool(False)), Browsable(True), System.ComponentModel.Description("Shows or Hides the Close Button.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property

    Public Property CheckAmount() As String
        Get
            Return CustServiceControl.CheckAmount
        End Get
        Set(ByVal value As String)
            CustServiceControl.CheckAmount = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    '
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	12/14/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property CheckAmountEnabled() As Boolean
        Get
            Return CustServiceControl.CheckAmountEnabled
        End Get
        Set(ByVal value As Boolean)
            CustServiceControl.CheckAmountEnabled = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    '
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	11/9/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property Denied() As Boolean
        Get
            Return CustServiceControl.Denied
        End Get
        Set(ByVal value As Boolean)
            CustServiceControl.Denied = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the PatientSSNTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property PatientSSN() As String
        Get
            Return CustServiceControl.PatientSSN
        End Get
        Set(ByVal value As String)
            CustServiceControl.PatientSSN = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the PatientSSNTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property PatientSSNEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.PatientSSNEnabled = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the ProviderIdTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property ProviderId() As String
        Get
            Return CustServiceControl.ProviderID
        End Get
        Set(ByVal value As String)
            CustServiceControl.ProviderID = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the ProviderIdTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property ProviderIdEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.ProviderIdEnabled = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the BatchNumberTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property BatchNumber() As String
        Get
            Return CustServiceControl.BatchNumber
        End Get
        Set(ByVal value As String)
            CustServiceControl.BatchNumber = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the BatchNumberTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property BatchNumberEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.BatchNumberEnabled = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the ParticipantSSNTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property ParticipantSSN() As Integer?
        Get
            Return CustServiceControl.ParticipantSSN
        End Get
        Set(ByVal value As Integer?)
            CustServiceControl.ParticipantSSN = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the ParticipantSSNTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property ParticipantSSNEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.ParticipantSSNEnabled = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the FamilyIdTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property FamilyID() As Integer?
        Get
            Return CustServiceControl.FamilyID
        End Get
        Set(ByVal value As Integer?)
            CustServiceControl.FamilyID = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the FamilyIdTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property FamilyIDEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.FamilyIDEnabled = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the RelationIdTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property RelationID() As Short?
        Get
            Return CustServiceControl.RelationID
        End Get
        Set(ByVal value As Short?)
            CustServiceControl.RelationID = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the RelationIdTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property RelationIdEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.RelationIdEnabled = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the DocumentIDTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property DocumentID() As String
        Get
            Return CStr(CustServiceControl.DocID)
        End Get
        Set(ByVal value As String)
            CustServiceControl.DocID = CInt(value)
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the DocumentIDTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property DocumentIDEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.DocIDEnabled = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the ClaimIdTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property ClaimId() As String
        Get
            Return CustServiceControl.ClaimID
        End Get
        Set(ByVal value As String)
            CustServiceControl.ClaimID = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the ClaimIdTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property ClaimIdEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.ClaimIDEnabled = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the DateOfServiceFromDateTimePicker control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property ReceivedDateFrom() As Date
        Get
            Return CustServiceControl.ReceivedDateFrom
        End Get
        Set(ByVal value As Date)
            CustServiceControl.ReceivedDateFrom = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the DateOfServiceToDateTimePicker control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	11/8/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property ReceivedDateTo() As Date
        Get
            Return CustServiceControl.ReceivedDateTo
        End Get
        Set(ByVal value As Date)
            CustServiceControl.ReceivedDateTo = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the ReceivedDateTimePicker control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property ReceivedDateEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.ReceivedDateEnabled = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the DateOfServiceFromDateTimePicker control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property DateOfServiceFrom() As Date
        Get
            Return CustServiceControl.DateOfServiceFrom
        End Get
        Set(ByVal value As Date)
            CustServiceControl.DateOfServiceFrom = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the DateOfServiceToDateTimePicker control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	11/8/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property DateOfServiceTo() As Date
        Get
            Return CustServiceControl.DateOfServiceTo
        End Get
        Set(ByVal value As Date)
            CustServiceControl.DateOfServiceTo = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the DateOfServiceDateTimePicker control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property DateOfServiceEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.DateOfServiceEnabled = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the DocTypeComboBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property DocType() As String
        Get
            Return CustServiceControl.DocType
        End Get
        Set(ByVal value As String)
            CustServiceControl.DocType = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the DocTypeComboBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property DocTypeEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.DocTypeEnabled = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the ProviderNameTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property ProviderName() As String
        Get
            Return CustServiceControl.ProviderName
        End Get
        Set(ByVal value As String)
            CustServiceControl.ProviderName = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the DoctorsLastNameTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property DoctorsLastNameEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.DoctorsLastNameEnabled = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the CheckNumberTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property CheckNumber() As String
        Get
            Return CustServiceControl.CheckNumber
        End Get
        Set(ByVal value As String)
            CustServiceControl.CheckNumber = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the CheckNumberTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property CheckNumberEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.CheckInfoEnabled = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the ChiroCheckBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property Chiropractic() As Boolean
        Get
            Return CustServiceControl.Chiropractic
        End Get
        Set(ByVal value As Boolean)
            CustServiceControl.Chiropractic = value
        End Set
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the ProcedureCodeTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property ProcedureCode() As String
        Get
            Return CustServiceControl.ProcedureCode
        End Get
        Set(ByVal value As String)
            CustServiceControl.ProcedureCode = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the ProcedureCodeTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property ProcedureCodeEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.ProcedureCodeEnabled = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the  ModifierTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property CollapseUI() As Integer
        Get
            Return CustServiceControl.CollapseUI
        End Get
        Set(ByVal value As Integer)
            CustServiceControl.CollapseUI = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the  ModifierTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property Modifier() As String
        Get
            Return CustServiceControl.Modifier
        End Get
        Set(ByVal value As String)
            CustServiceControl.Modifier = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the ModifierTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property ModifierEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.ModifierEnabled = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the BillTypeTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property BillType() As String
        Get
            Return CustServiceControl.BillType
        End Get
        Set(ByVal value As String)
            CustServiceControl.BillType = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the BillTypeTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property BillTypeEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.BillTypeEnabled = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the DiagnosisTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property Diagnosis() As String
        Get
            Return CustServiceControl.Diagnosis
        End Get
        Set(ByVal value As String)
            CustServiceControl.Diagnosis = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the DiagnosisTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property DiagnosisEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.DiagnosisEnabled = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the PlaceOfServiceTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property PlaceOfService() As String
        Get
            Return CustServiceControl.PlaceOfService
        End Get
        Set(ByVal value As String)
            CustServiceControl.PlaceOfService = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the PlaceOfServiceTextBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property PlaceOfServiceEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.PlaceOfServiceEnabled = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the ProviderComboxBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property Par() As String
        Get
            Return CustServiceControl.Par
        End Get
        Set(ByVal value As String)
            CustServiceControl.Par = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the ProviderComboxBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property ParEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.ParEnabled = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the IncidentDateDateTimePicker control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property IncidentDate() As Date
        Get
            Return CustServiceControl.IncidentDate
        End Get
        Set(ByVal value As Date)
            CustServiceControl.IncidentDate = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the IncidentDateDateTimePicker control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property IncidentDateEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.IncidentDateEnabled = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets/Sets the AccidentTypeComboBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Property AccidentType() As String
        Get
            Return CustServiceControl.AccidentType
        End Get
        Set(ByVal value As String)
            CustServiceControl.AccidentType = value
        End Set
    End Property
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Enables/Disables the AccidentTypeComboBox control
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/23/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public WriteOnly Property AccidentTypeEnabled() As Boolean
        Set(ByVal value As Boolean)
            CustServiceControl.AccidentTypeEnabled = value
        End Set
    End Property
#End Region

#Region "Form Events"
    Private Sub CustomerService_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            If _OpenIndex = -1 Then
                Me.Text = "Patient History - Family (" & FamilyID & ") RelationID (" & RelationID & ")"
                CustServiceControl.CollapseControl()

            ElseIf _OpenIndex = 0 Then
                Me.Text = "Customer Service"
            Else
                Me.Text = "Customer Service - " & _OpenIndex
            End If

            If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

            CustServiceControl.InitializeControl()

        Catch ex As Exception

	Throw
        End Try
    End Sub

    'Private Sub CustomerService_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
    '    Try
    '        Me.Visible = False
    '        Me.MdiParent = Nothing

    '        CustServiceControl.CloseControl()

    '        SaveSettings()
    '    Catch ex As Exception
    '        Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
    '        If (rethrow) Then
    '            MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '        End If
    '    End Try
    'End Sub

#End Region

    Public Sub Search()

        Try
            CustServiceControl.Search(_OpenIndex)

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Private Sub CustomerService_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Try
            Me.Visible = False
            Me.MdiParent = Nothing

            UFCWGeneral.SaveFormPosition(Me, _APPKEY)

        Catch ex As Exception

	Throw
        Finally

            If CustServiceControl IsNot Nothing Then CustServiceControl.Dispose()
            CustServiceControl = Nothing

        End Try
    End Sub

End Class