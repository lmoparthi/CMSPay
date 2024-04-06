Option Strict On
Imports System.Windows.Forms
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class RetirementHistory

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

    Public Sub New(ByVal readonlymode As Boolean, Optional ByVal familyID As Integer? = Nothing)
        Me.New()

        _ReadOnlyMode = readonlymode

        If familyID IsNot Nothing Then

            SearchButton.Enabled = False

            EligRetirementElements.LoadEligRetireeElements(readonlymode, CInt(familyID))
            FamilyIdTextBox.Text = familyID.ToString

            FamilyIdTextBox.ReadOnly = True

        Else
            EligRetirementElements.LoadEligRetireeElements()
        End If

    End Sub

    Private Sub btnClear_Click(sender As System.Object, e As System.EventArgs) Handles btnClear.Click
        Try
            If EligRetirementElements.ChangesPending Then
                MessageBox.Show(Me, "Changes have been made." & vbCrLf &
                                             "Please Complete the changes before continuing", "Save changes", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

                Return
            End If

            Me.FamilyIdTextBox.Clear()
            Me.EligRetirementElements.ClearAll()
            Me.FamilyIdTextBox.Focus()

            SearchButton.Enabled = True
            FamilyIdTextBox.Enabled = True
            FamilyIdTextBox.ReadOnly = False
            Me.FamilyIdTextBox.Focus()

        Catch ex As Exception

            Throw
        Finally
        End Try
    End Sub

    Private Sub SearchButton_Click(sender As Object, e As EventArgs) Handles SearchButton.Click
        Try

            Me.EligRetirementElements.ClearAll()

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

                EligRetirementElements.LoadEligRetireeElements(_ReadOnlyMode, CInt(Me.FamilyIdTextBox.Text))

                SearchButton.Enabled = False
                FamilyIdTextBox.Enabled = False
                FamilyIdTextBox.ReadOnly = True

            End If

        Catch ex As Exception

                Throw
        Finally
        End Try
    End Sub

    Private Sub RetirementHistory_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Try
            If EligRetirementElements.ChangesPending Then
                MessageBox.Show(Me, "Changes have been made to Retirement History Screen." & vbCrLf &
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

    Private Sub RetirementHistory_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Me.FamilyIdTextBox.Text.Length = 0 Then
            '_ReadOnlyMode = True
            EligRetirementElements.ReadOnlyMode = _ReadOnlyMode
            EligRetirementElements.ProcessSubControls(CType(EligRetirementElements, Object), True)
            FamilyIdTextBox.Focus()
        End If

    End Sub

    Private Sub FamilyIdTextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles FamilyIdTextBox.KeyPress
        If Char.IsDigit(e.KeyChar) Or Char.IsControl(e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub RetirementHistory_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        If EligRetirementElements IsNot Nothing Then
            EligRetirementElements.Dispose()
            EligRetirementElements = Nothing
        End If
    End Sub

End Class
