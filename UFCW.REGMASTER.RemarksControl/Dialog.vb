Option Strict On
Imports System.ComponentModel

Public Class RemarksDialog

    'Private Shared _TraceSwitch As New BooleanSwitch("CloneTraceSwitch", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\RegMaster\RemarksControl"

#Region "Constructor"
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub
    Public Sub New(ByVal familyID As Integer, ByVal relationID As Integer, ByVal partSSN As Integer, ByVal remarkDS As DataSet, ByVal readOnlyMode As Boolean)
        Me.New()

        RemarksControl.FamilyID = familyID
        RemarksControl.RelationID = relationID
        RemarksControl.PartSSN = partSSN
        RemarksControl.REMARKDataSet = remarkDS
        RemarksControl.ReadOnlyMode = readOnlyMode

        RemarksControl.LoadREMARKS()

        Me.Text = "Remarks for the RelationID " & relationID

    End Sub
#End Region

    Private Sub SaveSettings()
        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, "Main\Settings", "Top", CStr(Me.Top))
        SaveSetting(_APPKEY, "Main\Settings", "Height", CStr(Me.Height))
        SaveSetting(_APPKEY, "Main\Settings", "Left", CStr(Me.Left))
        SaveSetting(_APPKEY, "Main\Settings", "Width", CStr(Me.Width))
    End Sub

    Private Sub SetSettings()

        Me.Visible = False

        Me.Top = CInt(GetSetting(_APPKEY, "Main\Settings", "Top", CStr(Me.Top)))
        Me.Height = CInt(GetSetting(_APPKEY, "Main\Settings", "Height", CStr(Me.Height)))
        Me.Left = CInt(GetSetting(_APPKEY, "Main\Settings", "Left", CStr(Me.Left)))
        Me.Width = CInt(GetSetting(_APPKEY, "Main\Settings", "Width", CStr(Me.Width)))

    End Sub

    Private Sub RemarksControl_Save() Handles RemarksControl.Save
        Me.DialogResult = DialogResult.OK
        Me.Show() '' Me.Close()
    End Sub

    Private Sub RemarksControl_cancel() Handles RemarksControl.Cancel
        Me.DialogResult = DialogResult.Cancel
    End Sub
    Private Sub RemarksControl_yes() Handles RemarksControl.Yes
        Me.DialogResult = DialogResult.Yes
    End Sub
    Private Sub RemarksControl_No() Handles RemarksControl.No
        Me.DialogResult = DialogResult.No
        RemarksControl.txtRemarks.Focus()
        RemarksControl.SaveActionButton.Enabled = True
    End Sub

    Private Sub RemarksDialog_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetSettings()
    End Sub

    Private Sub RemarksDialog_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        SaveSettings()
    End Sub
End Class