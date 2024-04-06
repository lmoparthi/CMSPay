Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports UFCW.WCF.FileNet
Imports UFCW.WCF

Public Class FindDocuments
    Private Shared _SingleInstance As Boolean = False

    Const CRYPTORKEY As String = "UFCW CMS ACCESS"

    Public ReadOnly Property SSN() As Integer
        Get
            Return UfcwDocsControl1.SSN
        End Get
    End Property

    Public ReadOnly Property DocID() As Long
        Get
            Return UfcwDocsControl1.DocID
        End Get
    End Property

    Private Sub SearchButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchButton.Click
        Dim OptionsSelected As Double

        ErrorProvider1.SetError(SearchButton, "")

        If txtSearchMemSSN.Text.Length > 0 Then
            OptionsSelected += 0.5
        End If
        If txtSearchProviderTaxID.Text.Length > 0 Then
            OptionsSelected += 0.5
        End If
        If txtSearchDocID.Text.Length > 0 Then
            OptionsSelected += 1
        End If
        If txtSearchFamilyID.Text.Length > 0 Then
            OptionsSelected += 1
        End If
        If txtSearchMaximID.Text.Length > 0 Then
            OptionsSelected += 1
        End If
        If txtSearchBatch.Text.Length > 0 Then
            OptionsSelected += 1
        End If

        If OptionsSelected > 1 Then
            ErrorProvider1.SetError(SearchButton, "Only a single search criteria is allowed.")
            Return
        End If

        If txtSearchProviderTaxID.Text.Length > 0 AndAlso txtSearchMemSSN.Text.Length > 0 Then
            If txtSearchDate.Text.Length > 0 Then
                UfcwDocsControl1.LoadUFCWDOCSBySSNAndTaxID(CType(txtSearchMemSSN.Text.Replace("-", ""), Integer), CType(txtSearchProviderTaxID.Text.Replace("-", ""), Integer), CType(txtSearchDate.Text, Date), UFCWGeneralAD.REGMEmployeeAccess)
            Else
                UfcwDocsControl1.LoadUFCWDOCSBySSNAndTaxID(CType(txtSearchMemSSN.Text.Replace("-", ""), Integer), CType(txtSearchProviderTaxID.Text.Replace("-", ""), Integer), UFCWGeneralAD.REGMEmployeeAccess)
            End If

        ElseIf txtSearchMemSSN.Text.Length > 0 Then
            If txtSearchDate.Text.Length > 0 Then
                UfcwDocsControl1.LoadUFCWDOCSFromSSN(CType(txtSearchMemSSN.Text.Replace("-", ""), Integer), CType(txtSearchDate.Text, Date), UFCWGeneralAD.REGMEmployeeAccess)
            Else
                UfcwDocsControl1.LoadUFCWDOCSFromSSN(CType(txtSearchMemSSN.Text.Replace("-", ""), Integer), UFCWGeneralAD.REGMEmployeeAccess)
            End If

        ElseIf txtSearchProviderTaxID.Text.Length > 0 Then
            If txtSearchDate.Text.Length > 0 Then
                UfcwDocsControl1.LoadUFCWDOCSFromProviderTIN(CType(txtSearchProviderTaxID.Text.Replace("-", ""), Integer), CType(txtSearchDate.Text, Date), UFCWGeneralAD.REGMEmployeeAccess)
            Else
                UfcwDocsControl1.LoadUFCWDOCSFromProviderTIN(CType(txtSearchProviderTaxID.Text.Replace("-", ""), Integer), UFCWGeneralAD.REGMEmployeeAccess)
            End If

        ElseIf txtSearchDocID.Text.Length > 0 Then
            UfcwDocsControl1.LoadUFCWDOCSFromDocID(CLng(txtSearchDocID.Text.Trim), UFCWGeneralAD.REGMEmployeeAccess)

        ElseIf txtSearchMaximID.Text.Length > 0 Then
            UfcwDocsControl1.LoadUFCWDOCSFromMaxID(txtSearchMaximID.Text.Trim, UFCWGeneralAD.REGMEmployeeAccess)

        ElseIf txtSearchBatch.Text.Length > 0 Then
            UfcwDocsControl1.LoadUFCWDOCSFromBATCH(txtSearchBatch.Text.Trim, UFCWGeneralAD.REGMEmployeeAccess)

        ElseIf txtSearchFamilyID.Text.Length > 0 Then
            ErrorProvider1.SetError(txtSearchFamilyID, "")
            Dim RegMDT As DataTable = CMSDALFDBMD.RetrieveRegMasterByFamilyID(CInt(txtSearchFamilyID.Text))
            If RegMDT IsNot Nothing AndAlso RegMDT.Rows.Count > 0 Then
                If txtSearchDate.Text.Length > 0 Then
                    UfcwDocsControl1.LoadUFCWDOCSFromSSN(CInt(RegMDT(0)("SSNO")), CType(txtSearchDate.Text, Date), UFCWGeneralAD.REGMEmployeeAccess)
                Else
                    UfcwDocsControl1.LoadUFCWDOCSFromSSN(CInt(RegMDT(0)("SSNO")), UFCWGeneralAD.REGMEmployeeAccess)
                End If
            Else
                ErrorProvider1.SetError(txtSearchFamilyID, "FamilyID Not Found")
            End If

        ElseIf txtSearchDate.Text.Length > 0 Then
            UfcwDocsControl1.LoadUFCWDOCSByReceivedDateAllDocClasses(CType(txtSearchDate.Text, Date), UFCWGeneralAD.REGMEmployeeAccess)
        End If
    End Sub

    Private Sub ClearButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearButton.Click
        txtSearchMemSSN.Clear()
        txtSearchProviderTaxID.Clear()
        txtSearchDocID.Clear()
        txtSearchFamilyID.Clear()
        txtSearchDate.Clear()
        txtSearchMaximID.Clear()
        UfcwDocsControl1.Clearall()
    End Sub

    Private Sub txtSearchMemSSN_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearchMemSSN.KeyPress, txtSearchProviderTaxID.KeyPress
        Dim EntryOkRegex As New Regex("^[0-9\-]+$")

        If Not (System.Char.IsControl(e.KeyChar)) AndAlso Not (System.Char.IsWhiteSpace(e.KeyChar)) AndAlso Not EntryOkRegex.IsMatch(e.KeyChar) Then
            e.Handled = True
        End If

    End Sub

    Private Sub txtSearchMemSSN_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSearchProviderTaxID.KeyDown

        If (e.KeyCode = Keys.V AndAlso e.Modifiers = Keys.Control) OrElse (e.KeyCode = Keys.Insert AndAlso e.Modifiers = Keys.Shift) Then
            Dim ClipText As String = Clipboard.GetText
            If ClipText.Length < 20 Then
                For X As Integer = 1 To ClipText.Length
                    ClipText = Regex.Replace(ClipText, "[^0-9|\-]", String.Empty, RegexOptions.IgnoreCase)
                Next

                If Clipboard.GetText <> ClipText Then
                    If ClipText.Length > 0 Then
                        Clipboard.SetText(ClipText)
                    Else
                        Clipboard.Clear()
                    End If
                End If
            Else
                e.Handled = True
            End If
        End If

    End Sub

    Private Sub txtSearchDocID_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearchDocID.KeyPress, txtSearchFamilyID.KeyPress
        Dim EntryOkRegex As New Regex("^[0-9]+$")

        If Not (System.Char.IsControl(e.KeyChar)) AndAlso Not (System.Char.IsWhiteSpace(e.KeyChar)) AndAlso Not EntryOkRegex.IsMatch(e.KeyChar) Then
            e.Handled = True
        End If

    End Sub

    Private Sub txtSearchProvider_Validating(sender As Object, e As CancelEventArgs) Handles txtSearchProviderTaxID.Validating

        Dim TBox As TextBox = CType(sender, TextBox)
        Dim EntryOkRegex As New Regex("(?!(\d){3}(-| |)\1{2}\2\1{4})(?!666|000|9\d{2})(\b\d{3}(-| |)(?!00)\d{2}\4(?!0{4})\d{4}\b)|(\d{2}-\d{7})")

        ErrorProvider1.SetError(TBox, "")

        If TBox.Text.Trim.Length > 0 AndAlso Not EntryOkRegex.IsMatch(TBox.Text) Then
            ErrorProvider1.SetError(TBox, "TIN/SSN invalid. use 12-3456789 or 123-45-6789 formats only")
            e.Cancel = True
        End If
    End Sub

    Private Sub txtSearchMemSSN_Validating(sender As Object, e As CancelEventArgs) Handles txtSearchMemSSN.Validating

        Dim TBox As TextBox = CType(sender, TextBox)
        Dim EntryOkRegex As New Regex("(?!(\d){3}(-| |)\1{2}\2\1{4})(?!666|000|9\d{2})(\b\d{3}(-| |)(?!00)\d{2}\4(?!0{4})\d{4}\b)")

        ErrorProvider1.SetError(TBox, "")

        If TBox.Text.Trim.Length > 0 AndAlso Not EntryOkRegex.IsMatch(TBox.Text) Then
            ErrorProvider1.SetError(TBox, "SSN invalid. use 123-45-6789 or 123456789 formats only")
            e.Cancel = True
        End If
    End Sub

    Private Sub txtSearchDate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearchDate.KeyPress

        If Char.IsDigit(e.KeyChar) OrElse (Microsoft.VisualBasic.Asc(e.KeyChar) = 45) OrElse (Microsoft.VisualBasic.Asc(e.KeyChar) = 47) OrElse Char.IsControl(e.KeyChar) Then
            'do nothing
        Else
            e.Handled = True
        End If

    End Sub

    Private Sub txtSearchDate_Validating(sender As Object, e As CancelEventArgs) Handles txtSearchDate.Validating
        Dim TBox As TextBox = CType(sender, TextBox)

        If TBox.Enabled = False OrElse TBox.ReadOnly OrElse TBox.TextLength < 1 Then Return

        Try

            ErrorProvider1.ClearError(TBox)

            Dim HoldDate As Date? = UFCWGeneral.ValidateDate(TBox.Text)
            If HoldDate IsNot Nothing Then

                If TBox.Text <> CDate(HoldDate).ToString("MM-dd-yyyy") Then
                    TBox.Text = CDate(HoldDate).ToString("MM-dd-yyyy")
                End If

            Else
                ErrorProvider1.SetError(TBox, " Date is invalid (MM-dd-yyyy)")
            End If

            If ErrorProvider1.GetError(TBox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelActionButton.Click
        Me.Close()
    End Sub

    Private Sub FindDocuments_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim FNInfo As Tuple(Of String, String) = WCFWrapperCommon.InitializeFileNetUserInfo()

        Using FNSession As New Session(New WCFProcessInfo With {.ByProcessName = _SingleInstance, .ProcessID = Process.GetCurrentProcess.Id, .ProcessName = Process.GetCurrentProcess.ProcessName})
            If FNSession.LoggedOn = False Then FNSession.Logon(FNInfo.Item1, FNInfo.Item2)
        End Using

    End Sub


End Class