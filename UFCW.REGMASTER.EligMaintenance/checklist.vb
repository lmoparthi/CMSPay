Option Strict On
Imports System.Windows.Forms
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class frmchecklist

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

    Public Sub New(ByVal familyID As Integer, ByVal relationID As Integer, Optional ByVal readonlymode As Boolean = False)
        Me.New()

        EligCalcElementsControl.LoadEligCalcElements(familyID, 0)
        EligCalcElementsControl.ReadOnlyMode = readonlymode
        EligCalcElementsControl.ProcessControls()

        SearchActionButton.Enabled = False
        FamilyIdTextBox.Enabled = False
        FamilyIdTextBox.Text = familyID.ToString
        FamilyIdTextBox.ReadOnly = True

    End Sub

    Private Sub btnClear_Click(sender As System.Object, e As System.EventArgs) Handles ClearActionButton.Click

        Try

            If EligCalcElementsControl.ChangesPending Then
                MessageBox.Show(Me, "Changes have been made." & vbCrLf &
                                             "Please Complete the changes before continuing", "Save changes", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

                Return
            End If

            EligCalcElementsControl.ClearAll()

            SearchActionButton.Enabled = True

            FamilyIdTextBox.Enabled = True
            FamilyIdTextBox.ReadOnly = False

            Me.FamilyIdTextBox.Clear()
            Me.FamilyIdTextBox.Focus()

        Catch ex As Exception

            Throw
        Finally
        End Try
    End Sub

    Private Sub btnshow_Click(sender As Object, e As EventArgs) Handles SearchActionButton.Click
        Try

            Me.EligCalcElementsControl.ClearAll()

            If Me.FamilyIdTextBox.Text.Length > 0 AndAlso IsNumeric(Me.FamilyIdTextBox.Text) Then

                Dim ParticipantDS As DataSet = RegMasterDAL.RetrieveSecureRegMasterByFamilyid(CInt(Me.FamilyIdTextBox.Text))

                If ParticipantDS IsNot Nothing Then
                    '' Restricted Access to REGMEmployeeAccess
                    If (_REGMEmployeeAccess = False) AndAlso ((ParticipantDS.Tables("REG_MASTER").Rows.Count > 0) AndAlso (CInt(ParticipantDS.Tables("REG_MASTER").Rows(0)("PART_SSNO")) = 0)) Then
                        MessageBox.Show("You are not authorized to view Trust Employee Information.", "Access Restricted", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                        FamilyIdTextBox.Clear()
                        Exit Try
                    End If
                End If

                EligCalcElementsControl.LoadEligCalcElements(CInt(Me.FamilyIdTextBox.Text), 0)
                EligCalcElementsControl.ReadOnlyMode = _ReadOnlyMode
                EligCalcElementsControl.ProcessControls()

                SearchActionButton.Enabled = False

                FamilyIdTextBox.Enabled = False
                FamilyIdTextBox.ReadOnly = True
            End If

        Catch ex As Exception

                Throw
        Finally
        End Try
    End Sub

    Private Sub frmchecklist_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Try
            If EligCalcElementsControl.ChangesPending Then
                MessageBox.Show(Me, "Changes have been made to Plan Maintenance Screen." & vbCrLf &
                                             "Please Complete the changes before continuing", "Save changes", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

                e.Cancel = True
            Else
                Me.Dispose()
            End If

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub frmchecklist_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Me.FamilyIdTextBox.Text.Length = 0 Then
            _ReadOnlyMode = True

            EligCalcElementsControl.ReadOnlyMode = _ReadOnlyMode
            EligCalcElementsControl.ProcessControls()

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

    Private Sub Checklist_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        If EligCalcElementsControl IsNot Nothing Then
            EligCalcElementsControl.Dispose()
            EligCalcElementsControl = Nothing
        End If
    End Sub

    Private Sub ExitButton_Click(sender As Object, e As EventArgs) Handles ExitActionButton.Click
        Me.Close()
    End Sub

    Private Sub EligCalcElementsControl_Load(sender As Object, e As EventArgs) Handles EligCalcElementsControl.Load

    End Sub
End Class
