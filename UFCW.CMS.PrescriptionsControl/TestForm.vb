Public Class TestForm

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        PrescriptionsControl1.FamilyID = CType(FamilyIDTextBox.Text, Integer)
        PrescriptionsControl1.LoadPrescriptionsControl(CDate("2011-01-01"), CDate("2017-01-01"), CType(FamilyIDTextBox.Text, Integer), CType(RelationIDTextBox.Text, Short))

    End Sub
End Class