Option Strict On

Imports System.ComponentModel
Public Class AnnotationDialog
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    'Friend WithEvents EligibilityControl1 As UFCW.CMS.Eligibility.EligibilityControl
    Friend WithEvents AnnotationsControl As AnnotationsControl
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.AnnotationsControl = New AnnotationsControl()
        Me.SuspendLayout()
        '
        'AnnotationsControl
        '
        Me.AnnotationsControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AnnotationsControl.AppKey = "UFCW\Claims\"
        Me.AnnotationsControl.Location = New System.Drawing.Point(0, 0)
        Me.AnnotationsControl.Name = "AnnotationsControl"
        Me.AnnotationsControl.Size = New System.Drawing.Size(419, 488)
        Me.AnnotationsControl.TabIndex = 0
        '
        'AnnotationDialog
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(418, 486)
        Me.Controls.Add(Me.AnnotationsControl)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(336, 328)
        Me.Name = "AnnotationDialog"
        Me.Text = "Annotation Editor / History"
        Me.ResumeLayout(False)

    End Sub

#End Region

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"

    Public Event AnnotationsModified(sender As Object, e As AnnotationsEvent)

    Public Sub New(ByVal claimID As Integer, ByVal familyID As Integer, ByVal relationID As Integer, ByVal partSSN As Integer, ByVal patSSN As Integer, ByVal partFName As String, ByVal partLName As String, ByVal patFName As String, ByVal patLName As String, ByVal annotationsDT As DataTable)
        Me.New()
        'AnnotationsControl1 = New AnnotationsControl(ClaimID, FamilyID, RelationID, PartSSN, PatSSN, PartFName, PartLName, PatFName, PatLName, annotationTable)
        AnnotationsControl.ClaimID = claimID
        AnnotationsControl.FamilyID = familyID
        AnnotationsControl.RelationID = relationID
        AnnotationsControl.ParticipantSSN = partSSN
        AnnotationsControl.PatientSSN = patSSN
        AnnotationsControl.ParticipantFirst = partFName
        AnnotationsControl.ParticipantLast = partLName
        AnnotationsControl.PatientFirst = patFName
        AnnotationsControl.PatientLast = patLName
        AnnotationsControl.Annotations = annotationsDT
        AnnotationsControl.Location = New System.Drawing.Point(0, 0)
    End Sub

    '<Browsable(True), System.ComponentModel.Description("Gets or Sets the Patient Last Name of the Document.")> _
    Public Property AnnotationsTable() As DataTable
        Get
            Return AnnotationsControl.Annotations
        End Get
        Set(ByVal value As DataTable)
            Dim DV As New DataView(value, "", "", DataViewRowState.CurrentRows)
            If DV.Count > 0 Then
                For Cnt As Integer = 0 To DV.Count - 1
                    AnnotationsControl.Annotations.Rows.Add(DV(Cnt).Row.ItemArray)
                Next
                AnnotationsControl.Annotations.AcceptChanges()
            End If
        End Set
    End Property

    Private Sub AnnotationDialog_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = DialogResult.Cancel
            Me.Hide()
        End If
    End Sub

    Private Sub AnnotationDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.AnnotationsControl.Location = New System.Drawing.Point(0, 0)
        Me.Top = CInt(GetSetting(_APPKEY, "AuditControlWindow\Settings", "Top", CStr(Me.Top)))
        Me.Height = CInt(GetSetting(_APPKEY, "AuditControlWindow\Settings", "Height", CStr(Me.Height)))
        Me.Left = CInt(GetSetting(_APPKEY, "AuditControlWindow\Settings", "Left", CStr(Me.Left)))
        Me.Width = CInt(GetSetting(_APPKEY, "AuditControlWindow\Settings", "Width", CStr(Me.Width)))
    End Sub

    Private Sub AnnotationControl1_Closing()
        Me.Hide()
        SaveSetting(_APPKEY, "AuditControlWindow\Settings", "Top", CStr(Me.Top))
        SaveSetting(_APPKEY, "AuditControlWindow\Settings", "Height", CStr(Me.Height))
        SaveSetting(_APPKEY, "AuditControlWindow\Settings", "Left", CStr(Me.Left))
        SaveSetting(_APPKEY, "AuditControlWindow\Settings", "Width", CStr(Me.Width))
    End Sub

    Private Sub AnnotationDialog_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        SaveSetting(_APPKEY, "AuditControlWindow\Settings", "Top", CStr(Me.Top))
        SaveSetting(_APPKEY, "AuditControlWindow\Settings", "Height", CStr(Me.Height))
        SaveSetting(_APPKEY, "AuditControlWindow\Settings", "Left", CStr(Me.Left))
        SaveSetting(_APPKEY, "AuditControlWindow\Settings", "Width", CStr(Me.Width))
    End Sub

    Private Sub AnnotationsControl1_Closing() Handles AnnotationsControl.Closing
        Me.DialogResult = DialogResult.Cancel
        Me.Hide()
    End Sub

    Private Sub AnnotationsControl1_Save(sender As Object, e As AnnotationsEvent) Handles AnnotationsControl.Save

        Me.DialogResult = DialogResult.OK
        Me.Hide()

        RaiseEvent AnnotationsModified(Me, New AnnotationsEvent(e.AnnotationsTable))

    End Sub

End Class