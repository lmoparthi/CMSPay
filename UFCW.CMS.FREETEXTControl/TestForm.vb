Public Class TestForm

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        FreeTextEditor1.FamilyID = CInt(NotesFamilyIDTextBox.Text.ToString)
        If IsNumeric(NotesRelationIDTextBox.Text.ToString) Then FreeTextEditor1.RelationID = CInt(NotesRelationIDTextBox.Text.ToString)
        If IsNumeric(NotesPartSSNTextBox.Text.ToString) Then FreeTextEditor1.ParticipantSSN = CInt(NotesPartSSNTextBox.Text.ToString)

        FreeTextEditor1.ReadOnlyMode = ReadOnlyCheckBox.Checked
        FreeTextEditor1.LoadFreeText()

    End Sub
End Class