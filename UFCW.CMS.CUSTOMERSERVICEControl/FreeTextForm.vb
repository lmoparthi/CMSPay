Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ComponentModel

Public Class FreeTextForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"

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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents FreeTextEditor1 As FreeTextEditor

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.FreeTextEditor1 = New FreeTextEditor()
        Me.SuspendLayout()
        '
        'FreeTextEditor1
        '
        Me.FreeTextEditor1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FreeTextEditor1.AppKey = "UFCW\Claims\"
        Me.FreeTextEditor1.AppName = ""
        Me.FreeTextEditor1.AutoScroll = True
        Me.FreeTextEditor1.BackColor = System.Drawing.SystemColors.Control
        Me.FreeTextEditor1.Location = New System.Drawing.Point(0, 0)
        Me.FreeTextEditor1.Name = "FreeTextEditor1"
        Me.FreeTextEditor1.Size = New System.Drawing.Size(520, 568)
        Me.FreeTextEditor1.TabIndex = 0
        '
        'FreeTextForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(520, 566)
        Me.Controls.Add(Me.FreeTextEditor1)
        Me.Name = "FreeTextForm"
        Me.Text = "FreeText"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub FreeTextForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()
    End Sub

    Private Sub FreeTextForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Try
            UFCWGeneral.SaveFormPosition(Me, _APPKEY)

        Catch ex As Exception
            Throw
        Finally
            FreeTextEditor1.Dispose()
            FreeTextEditor1 = Nothing
        End Try
    End Sub

    Public Sub LoadControl(ByVal familyId As Integer, ByVal relationId As Short?, ByVal partSSN As Integer, ByVal patSSN As Integer, ByVal partFname As String, ByVal partLname As String, ByVal patFname As String, ByVal patLname As String)
        FreeTextEditor1.FamilyID = familyId
        FreeTextEditor1.RelationID = relationId
        FreeTextEditor1.ParticipantSSN = partSSN
        FreeTextEditor1.PatientSSN = patSSN
        FreeTextEditor1.ParticipantFirst = partFname
        FreeTextEditor1.ParticipantLast = partLname
        FreeTextEditor1.PatientFirst = patFname
        FreeTextEditor1.PatientLast = patLname
        FreeTextEditor1.LoadFreeText()
    End Sub
End Class