Option Strict On
Imports System.Windows.Forms
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Public Class frmTerminations

    Private _APPKEY As String = "UFCW\RegMaster\"
    Private _FamilyID As Integer
    Private _RelationID As Integer
    Private _ReadOnlyMode As Boolean = True

    ReadOnly _REGMEmployeeAccess As Boolean = UFCWGeneralAD.REGMEmployeeAccess
    ReadOnly _REGMReadOnlyAccess As Boolean = UFCWGeneralAD.REGMReadOnlyAccess
    ReadOnly _REGMVendorAccess As Boolean = UFCWGeneralAD.REGMVendorAccess

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

    Public Sub New(ByVal familyID As Integer, ByVal relationID As Integer, ByVal ssn As Integer, Optional ByVal memName As String = "", Optional ByVal readonlymode As Boolean? = Nothing)

        Me.New()

        _ReadOnlyMode = If(readonlymode Is Nothing, _ReadOnlyMode, CBool(readonlymode))

        TerminationsControl.ReadOnlyMode = _ReadOnlyMode
        TerminationsControl.VisibleHistory = Not _REGMVendorAccess
        TerminationsControl.LoadTerms(familyID, 0, ssn, memName)

        btnshow.Enabled = False
        FamilyIdTextBox.Enabled = False
        FamilyIdTextBox.Text = familyID.ToString
        FamilyIdTextBox.ReadOnly = True
        txtSSN.Enabled = False
        txtSSN.ReadOnly = True

    End Sub

    Private Sub btnClear_Click(sender As System.Object, e As System.EventArgs) Handles btnClear.Click
        Try
            If TerminationsControl.ChangesPending Then
                MessageBox.Show(Me, "Changes have been made." & vbCrLf &
                                             "Please Complete the changes before continuing", "Save changes", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)

                Return
            End If
            btnshow.Enabled = True
            Me.FamilyIdTextBox.Clear()
            txtSSN.Clear()

            Me.TerminationsControl.ClearAll()

            FamilyIdTextBox.Enabled = True
            FamilyIdTextBox.ReadOnly = False
            txtSSN.Enabled = True
            txtSSN.ReadOnly = False

            Me.FamilyIdTextBox.Focus()

        Catch ex As Exception

            Throw
        Finally
        End Try
    End Sub

    Private Sub btnshow_Click(sender As Object, e As EventArgs) Handles btnshow.Click
        Dim partssn As Integer : Dim fam As Integer : Dim StrMemName As String = ""
        Try


            If Me.FamilyIdTextBox.Text.Length = 0 And Me.txtSSN.Text.Length = 0 Then

                MessageBox.Show("Please Enter FamilyID OR SSN")
            ElseIf Me.FamilyIdTextBox.Text.Length > 0 AndAlso IsNumeric(Me.FamilyIdTextBox.Text) Then

                Dim ParticipantDS As DataSet = RegMasterDAL.RetrieveSecureRegMasterByFamilyid(CInt(Me.FamilyIdTextBox.Text))

                If Not IsNothing(ParticipantDS) Then
                    '' Restricted Access to REGMEmployeeAccess
                    If (_REGMEmployeeAccess = False) AndAlso ((ParticipantDS.Tables("REG_MASTER").Rows.Count > 0) AndAlso (CInt(ParticipantDS.Tables("REG_MASTER").Rows(0)("PART_SSNO")) = 0)) Then
                        MessageBox.Show("You are not authorized to view Trust Employee Information.", "Access Restricted", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                        FamilyIdTextBox.Clear()
                        Exit Try
                    End If
                End If

                If (ParticipantDS.Tables("REG_MASTER").Rows.Count > 0) Then
                    partssn = CInt(ParticipantDS.Tables("REG_MASTER").Rows(0)("PART_SSNO"))

                    Dim dr As DataRow = ParticipantDS.Tables("REG_MASTER").Rows(0)
                    StrMemName = CStr(If(IsDBNull(dr("LAST_NAME")), "", dr("LAST_NAME"))) & " " & CStr(If(IsDBNull(dr("FIRST_NAME")), "", dr("FIRST_NAME"))) & " " & CStr(If(IsDBNull(dr("MIDDLE_INITIAL")), "", dr("MIDDLE_INITIAL")))

                    TerminationsControl.ReadOnlyMode = _ReadOnlyMode
                    TerminationsControl.Enabled = True
                    TerminationsControl.LoadTerms(CInt(Me.FamilyIdTextBox.Text), 0, partssn, StrMemName)

                    btnshow.Enabled = False
                    FamilyIdTextBox.Enabled = False
                    FamilyIdTextBox.ReadOnly = True
                    txtSSN.Enabled = False
                    txtSSN.ReadOnly = True
                Else
                    MessageBox.Show("No Matching Results", "No Records", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            Else   '' SSN search

                Dim ParticipantDS As DataSet = RegMasterDAL.RetrieveSecureRegMasterByPartSSNO(CInt((Me.txtSSN.Text)))

                If ParticipantDS IsNot Nothing Then
                    '' Restricted Access to REGMEmployeeAccess
                    If (_REGMEmployeeAccess = False) AndAlso ((ParticipantDS.Tables("REG_MASTER").Rows.Count > 0) AndAlso (CInt(ParticipantDS.Tables("REG_MASTER").Rows(0)("PART_SSNO")) = 0)) Then
                        MessageBox.Show("You are not authorized to view Trust Employee Information.", "Access Restricted", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                        txtSSN.Clear()
                        Exit Try
                    End If
                End If


                If (ParticipantDS.Tables("REG_MASTER").Rows.Count > 0) Then
                    fam = CInt(ParticipantDS.Tables("REG_MASTER").Rows(0)("FAMILY_ID"))

                    Dim dr As DataRow = ParticipantDS.Tables("REG_MASTER").Rows(0)
                    StrMemName = CStr(If(IsDBNull(dr("LAST_NAME")), "", dr("LAST_NAME"))) & " " & CStr(If(IsDBNull(dr("FIRST_NAME")), "", dr("FIRST_NAME"))) & " " & CStr(If(IsDBNull(dr("MIDDLE_INITIAL")), "", dr("MIDDLE_INITIAL")))


                    TerminationsControl.ReadOnlyMode = _ReadOnlyMode
                    TerminationsControl.Enabled = True
                    TerminationsControl.LoadTerms(fam, 0, CInt((Me.txtSSN.Text)), StrMemName)

                    btnshow.Enabled = False
                    FamilyIdTextBox.Enabled = False
                    FamilyIdTextBox.ReadOnly = True
                    txtSSN.Enabled = False
                    txtSSN.ReadOnly = True
                Else
                    MessageBox.Show("No Matching Results", "No Records", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            End If
        Catch ex As Exception

                Throw
        Finally
        End Try
    End Sub

    Private Sub frmTerminations_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed

        If TerminationsControl IsNot Nothing Then
            TerminationsControl.Dispose()
        End If
        TerminationsControl = Nothing

    End Sub

    Private Sub frmTerminations_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Try
            If TerminationsControl.ChangesPending Then
                MessageBox.Show(Me, "Changes have been made to Terminations Maintenance Screen." & vbCrLf &
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

    Private Sub frmTerminations_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Me.FamilyIdTextBox.Text.Length = 0 Then
            TerminationsControl.ReadOnlyMode = _ReadOnlyMode
            TerminationsControl.Enabled = False
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

    Private Sub txtSSN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSSN.KeyPress
        If Char.IsDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

End Class