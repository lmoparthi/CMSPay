Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ComponentModel

Public Class Audit
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _ClaimID As Integer
    Private _Adjuster As String

    Private _APPKEY As String = "UFCW\Claims\"

#Region "Public Properties"

    <DefaultValue("UFCW\Claims\"), Browsable(True), System.ComponentModel.Description("Represents the application key used when accessing the registry.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property
#End Region

#Region " Windows Form Designer generated code "

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
    Public Overloads Sub Dispose()
        AuditControl.Dispose()
        MyBase.Dispose()
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents AuditControl As AuditDetailsControl
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.AuditControl = New AuditDetailsControl()
        Me.SuspendLayout()
        '
        'AuditControl
        '
        Me.AuditControl.Adjuster = Nothing
        Me.AuditControl.ClaimID = Nothing
        Me.AuditControl.Location = New System.Drawing.Point(4, 4)
        Me.AuditControl.MaximumSize = New System.Drawing.Size(812, 552)
        Me.AuditControl.MinimumSize = New System.Drawing.Size(812, 552)
        Me.AuditControl.Name = "AuditControl"
        Me.AuditControl.Size = New System.Drawing.Size(812, 552)
        Me.AuditControl.TabIndex = 0
        '
        'Audit
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(817, 543)
        Me.Controls.Add(Me.AuditControl)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximumSize = New System.Drawing.Size(833, 582)
        Me.MinimumSize = New System.Drawing.Size(833, 582)
        Me.Name = "Audit"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Audit Items"
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Constructors"
    Sub New(ByVal claimID As Integer, ByVal adjuster As String)
        Me.New()

        _ClaimID = claimID
        _Adjuster = adjuster
    End Sub
#End Region

#Region "Properties"
    <System.ComponentModel.Browsable(True), System.ComponentModel.Description("Gets or Sets the ClaimID of the Document.")>
    Public Property ClaimID() As Integer
        Get
            Return _ClaimID
        End Get
        Set(ByVal value As Integer)
            _ClaimID = value
        End Set
    End Property

    <Browsable(True), System.ComponentModel.Description("Gets or Sets the Adjuster of the Claim.")>
    Public Property Adjuster() As String
        Get
            Return _Adjuster
        End Get
        Set(ByVal value As String)
            _Adjuster = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets the Status of Audit Items.")>
    Public ReadOnly Property Status() As String
        Get
            If AuditControl.Status = AuditDetailsControl.AuditStatus.Release Then
                Return "RELEASE"
            Else
                Return "AUDITED"
            End If
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets Audit CheckBoxes.")>
    Public ReadOnly Property Audits() As CheckBox()
        Get
            Return AuditControl.GetAllCheckBoxControls
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets Other Text From Audit Control.")>
    Public ReadOnly Property OtherText() As String
        Get
            Return AuditControl.OtherTextBox.Text
        End Get
    End Property
#End Region

#Region "Form Events"
    Private Sub Audit_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            SetSettings()
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub Audit_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Try
            SaveSettings()
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub SetSettings()
        Me.Top = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Left = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.WindowState = CType(GetSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)
    End Sub

    Private Sub SaveSettings()
        Dim lWindowState As FormWindowState = Me.WindowState
        SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(lWindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = lWindowState
    End Sub

    Private Sub AuditControl_Close(ByVal result As AuditDetailsControl.CloseStatus) Handles AuditControl.Close
        Try
            If result = AuditDetailsControl.CloseStatus.Cancel Then
                Me.DialogResult = DialogResult.Cancel
            Else
                Me.DialogResult = DialogResult.OK
            End If
            Me.Close()
        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

#Region "Custom Subs/Functions"
    Public Sub LoadAuditControl(ByVal theClaimID As Integer, ByVal theAdjuster As String)
        Try
            _ClaimID = theClaimID
            _Adjuster = theAdjuster

            LoadAuditControl()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadAuditControl()
        Try
            AuditControl.ClaimID = CStr(_ClaimID)
            AuditControl.Adjuster = _Adjuster
        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

End Class