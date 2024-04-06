Option Strict On
Imports System.Windows.Forms

Public Class A2countMaintenance
    Private _APPKEY As String = "UFCW\RegMaster\"
    Private _FamilyID As Integer
    Private _RelationID As Integer
    Private _ReadOnlyMode As Boolean = False
    Private _RegMEmployeeAccess As Boolean = UFCWGeneralAD.REGMEmployeeAccess

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

        A2CountControl.LoadA2Count(FamilyID, 0, SSN)

        A2CountControl.ReadOnlyMode = False
        A2CountControl.Enabled = True
        A2CountControl.ProcessControls(CType(A2CountControl, Object), False)

        btnshow.Enabled = False
        FamilyIdTextBox.Enabled = False
        FamilyIdTextBox.Text = FamilyID.ToString
        FamilyIdTextBox.ReadOnly = True
    End Sub


    Private Sub btnClear_Click(sender As System.Object, e As System.EventArgs) Handles btnClear.Click
        Try
            If A2CountControl.PendingChanges = True Then
                MessageBox.Show(Me, "Changes have been made." & vbCrLf &
                                             "Please Complete the changes before continuing", "Save changes", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

                Return
            End If
            btnshow.Enabled = True
            Me.FamilyIdTextBox.Clear()
            Me.A2CountControl.ClearAll()
            FamilyIdTextBox.Enabled = True
            FamilyIdTextBox.ReadOnly = False
            Me.FamilyIdTextBox.Focus()

        Catch ex As Exception

            Throw
        Finally
        End Try
    End Sub

    Private Sub btnshow_Click(sender As Object, e As EventArgs) Handles btnshow.Click
        Dim partssn As Integer
        Try

            If Me.FamilyIdTextBox.Text.Length > 0 AndAlso IsNumeric(Me.FamilyIdTextBox.Text) Then

                Dim ParticipentDS As DataSet = RegMasterDAL.RetrieveSecureRegMasterByFamilyid(CInt(Me.FamilyIdTextBox.Text))

                If ParticipentDS IsNot Nothing Then
                    '' Restricted Access to REGMEmployeeAccess
                    If (_RegMEmployeeAccess = False) AndAlso ((ParticipentDS.Tables("REG_MASTER").Rows.Count > 0) AndAlso (CInt(ParticipentDS.Tables("REG_MASTER").Rows(0)("PART_SSNO")) = 0)) Then
                        MessageBox.Show("You are not authorized to view Trust Employee Information.", "Access Restricted", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                        FamilyIdTextBox.Clear()
                        Exit Try
                    End If
                End If

                If (ParticipentDS.Tables("REG_MASTER").Rows.Count > 0) Then
                    partssn = CInt(ParticipentDS.Tables("REG_MASTER").Rows(0)("PART_SSNO"))
                End If

                A2CountControl.LoadA2Count(CInt(Me.FamilyIdTextBox.Text), 0, partssn)
                A2CountControl.ReadOnlyMode = False
                A2CountControl.Enabled = True
                A2CountControl.ProcessControls(CType(A2CountControl, Object), False)
                btnshow.Enabled = False
                FamilyIdTextBox.Enabled = False
                FamilyIdTextBox.ReadOnly = True
            End If
        Catch ex As Exception

            Throw
        Finally
        End Try
    End Sub

    Private Sub A2countMaintenance_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        If Not IsNothing(A2CountControl) Then
            A2CountControl.Dispose()
            A2CountControl = Nothing
        End If
    End Sub

    Private Sub A2countMaintenance_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Try
            If A2CountControl.PendingChanges = True Then
                MessageBox.Show(Me, "Changes have been made to A2count Maintenance Screen." & vbCrLf & _
                                             "Please Complete the changes before continuing", "Save changes", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

                e.Cancel = True
            Else
                Me.Dispose()
            End If

            ''  Me.Dispose()
        Catch ex As Exception

            Throw
        Finally
        End Try

    End Sub

    Private Sub A2countMaintenance_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Me.FamilyIdTextBox.Text.Length = 0 Then
            _ReadOnlyMode = True
            A2CountControl.ReadOnlyMode = True
            A2CountControl.ProcessControls(CType(A2CountControl, Object), True)
            A2CountControl.Enabled = False
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

End Class