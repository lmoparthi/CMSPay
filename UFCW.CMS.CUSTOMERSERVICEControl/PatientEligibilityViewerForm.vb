Imports System.ComponentModel


Public Class PatientEligibilityViewerForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"
    Private _FamilyID As Integer
    Private _RelationID As Short?
    Private _EligibilityDT As DataTable

    Friend WithEvents ExitButton As System.Windows.Forms.Button
    Friend WithEvents PatientEligibilityControl As EligibilityControl


#Region "Public Properties"
    <DefaultValue("UFCW\Claims\"), Browsable(True), System.ComponentModel.Description("Represents the application key used when accessing the registry.")> _
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = Value
        End Set
    End Property

    <Browsable(False), System.ComponentModel.Description("Used to load the Eligibility from an external source.")> _
    Public Property EligibilityDT() As DataTable
        Get
            Return _EligibilityDT
        End Get
        Set(ByVal value As DataTable)
            _EligibilityDT = Value
        End Set
    End Property
#End Region

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal familyID As Integer, ByVal relationID As Short?)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _FamilyID = familyID
        _RelationID = relationID

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
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.ExitButton = New System.Windows.Forms.Button()
        Me.PatientEligibilityControl = New EligibilityControl()
        Me.SuspendLayout()
        '
        'ExitButton
        '
        Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ExitButton.Location = New System.Drawing.Point(385, 273)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(75, 23)
        Me.ExitButton.TabIndex = 1
        Me.ExitButton.Text = "Exit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'PatientEligibilityControl
        '
        Me.PatientEligibilityControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PatientEligibilityControl.AppKey = "UFCW\Claims\"
        Me.PatientEligibilityControl.Location = New System.Drawing.Point(0, 0)
        Me.PatientEligibilityControl.Name = "PatientEligibilityControl"
        Me.PatientEligibilityControl.Size = New System.Drawing.Size(472, 266)
        Me.PatientEligibilityControl.TabIndex = 92
        '
        'PatientEligibilityViewerForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.ExitButton
        Me.ClientSize = New System.Drawing.Size(472, 302)
        Me.Controls.Add(Me.PatientEligibilityControl)
        Me.Controls.Add(Me.ExitButton)
        Me.Name = "PatientEligibilityViewerForm"
        Me.Text = "Patients Eligibility History"
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Form Events"
    Private Sub PatientsEligibilityViewerForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

        LoadPatientEligibility()

    End Sub


    Private Sub PatientsEligibilityViewerForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        Try
            UFCWGeneral.SaveFormPosition(Me, _APPKEY)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ExitButton_Click(sender As System.Object, e As System.EventArgs) Handles ExitButton.Click
        Me.Close()
    End Sub

#End Region

#Region "custom Code"
    Private Sub LoadPatientEligibility()
        Try
            PatientEligibilityControl.LoadEligibility(_FamilyID, _RelationID)

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

#End Region
End Class