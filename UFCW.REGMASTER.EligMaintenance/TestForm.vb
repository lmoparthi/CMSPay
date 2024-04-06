Imports System.Windows.Forms

Public Class TestForm
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim FRM As EligAcctHrsMaintForm
        Dim HoursDS As DataSet
        Try

            HoursDS = RegMasterDAL.GetEligibilityHours(txtFamilyID.Text)

            FRM = New EligAcctHrsMaintForm(txtFamilyID.text, False, Nothing)

            FRM.EligHoursDataSet = HoursDS  '' used to display when user selected different month
            FRM.Memtype = "1"

            FRM.LoadEligAccountHours()

            Dim Result As DialogResult = FRM.ShowDialog()

            If FRM.EligibiltyCalculated Then

            End If

            If Result = System.Windows.Forms.DialogResult.Yes Then
            End If

        Catch ex As Exception
        Finally
            If FRM IsNot Nothing Then
                FRM.Dispose()
            End If
            FRM = Nothing
        End Try
    End Sub

    Private Sub Test_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class