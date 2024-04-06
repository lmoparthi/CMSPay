Imports System.Windows.Forms

Public Class TestForm
    Private Sub ButtonRefresh_Click(sender As Object, e As EventArgs) Handles ButtonRefresh.Click


    End Sub

    Private Sub ButtonShowCoverage_Click(sender As Object, e As EventArgs) Handles ButtonShowCoverage.Click
        Dim CoverageElection As CoverageElectionForm
        Dim Result As DialogResult

        Try

            CoverageElection = New CoverageElectionForm(CType(Me.FamilyID.Text.ToString, Integer), CBool(ReadOnlyCheckBox.CheckState = CheckState.Checked), "92708", "ACTIVE")

            CoverageElection.MemType = 1

            Result = CoverageElection.ShowDialog()

        Catch ex As Exception
            Throw
        Finally
            If CoverageElection IsNot Nothing Then
                CoverageElection.Dispose()

            End If
            CoverageElection = Nothing
        End Try
    End Sub
End Class