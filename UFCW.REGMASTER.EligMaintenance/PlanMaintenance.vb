Option Strict On
Imports System.Windows.Forms
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class PlanMaintenance

    Private _APPKEY As String = "UFCW\RegMaster\"
    Private _FamilyID As Integer
    Private _RelationID As Integer
    Private _ReadOnlyMode As Boolean = False
    Private _REGMEmployeeAccess As Boolean = UFCWGeneralAD.REGMEmployeeAccess

    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal Value As String)

            _APPKEY = Value
        End Set
    End Property

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        'dont want to display the default table style
    End Sub

    Public Sub New(ByVal FamilyID As Integer, ByVal RelationID As Integer, ByVal SSN As Integer, Optional ByVal readonlymode As Boolean = False)
        Me.New()
        WaitingPeriodControl.LoadWaitingPeriod(FamilyID, 0, SSN)
        WaitingPeriodControl.ReadOnlyMode = False
        WaitingPeriodControl.Enabled = True
        WaitingPeriodControl.ProcessControls(False)
        btnshow.Enabled = False
        FamilyIdTextBox.Enabled = False
        FamilyIdTextBox.Text = FamilyID.ToString
        FamilyIdTextBox.ReadOnly = True
    End Sub


    Private Sub btnClear_Click(sender As System.Object, e As System.EventArgs) Handles btnClear.Click
        Try
            If WaitingPeriodControl.PendingChanges = True Then
                MessageBox.Show(Me, "Changes have been made." & vbCrLf &
                                             "Please Complete the changes before continuing", "Save changes", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

                Return
            End If

            Me.FamilyIdTextBox.Clear()
            Me.WaitingPeriodControl.ClearAll()
            Me.FamilyIdTextBox.Focus()
        Catch ex As Exception

            Throw
        Finally
        End Try
    End Sub

    Private Sub btnshow_Click(sender As Object, e As EventArgs) Handles btnshow.Click
        Dim PartSSN As Integer
        Try

            If Me.FamilyIdTextBox.Text.Length > 0 AndAlso IsNumeric(Me.FamilyIdTextBox.Text) Then

                Dim ParticipentDS As DataSet = RegMasterDAL.RetrieveSecureRegMasterByFamilyid(CInt(Me.FamilyIdTextBox.Text))

                If ParticipentDS IsNot Nothing Then
                    '' Restricted Access to REGMEmployeeAccess
                    If (_REGMEmployeeAccess = False) AndAlso ((ParticipentDS.Tables("REG_MASTER").Rows.Count > 0) AndAlso (CInt(ParticipentDS.Tables("REG_MASTER").Rows(0)("PART_SSNO")) = 0)) Then
                        MessageBox.Show("You are not authorized to view Trust Employee Information.", "Access Restricted", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                        FamilyIdTextBox.Clear()
                        Exit Try
                    End If
                End If

                If (ParticipentDS.Tables("REG_MASTER").Rows.Count > 0) Then
                    PartSSN = CInt(ParticipentDS.Tables("REG_MASTER").Rows(0)("PART_SSNO"))
                End If
                WaitingPeriodControl.LoadWaitingPeriod(CInt(Me.FamilyIdTextBox.Text), 0, PartSSN)
                WaitingPeriodControl.ReadOnlyMode = False
                WaitingPeriodControl.Enabled = True
                WaitingPeriodControl.ProcessControls(False)

            End If
        Catch ex As Exception

                Throw
        Finally
        End Try
    End Sub

    Private Sub PlanMaintenance_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Try
            If WaitingPeriodControl.PendingChanges = True Then
                MessageBox.Show(Me, "Changes have been made to Plan Maintenance Screen." & vbCrLf & _
                                             "Please Complete the changes before continuing", "Save changes", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

                e.Cancel = True
            Else
                Me.Dispose()
            End If
            '' Me.Dispose()
        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub frmPlanMaint_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Me.FamilyIdTextBox.Text.Length = 0 Then
            _ReadOnlyMode = True
            WaitingPeriodControl.ReadOnlyMode = True
            WaitingPeriodControl.ProcessControls(True)
            WaitingPeriodControl.Enabled = False
            FamilyIdTextBox.Focus()
        End If
    End Sub

    Private Sub FamilyIdTextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles FamilyIdTextBox.KeyPress
        If Char.IsDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub PlanMaintenance_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        If Not IsNothing(WaitingPeriodControl) Then
            WaitingPeriodControl.Dispose()
            WaitingPeriodControl = Nothing
        End If
    End Sub

End Class
