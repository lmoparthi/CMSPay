

Public Class TestForm

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    Public Shared Sub Main()

        CMSDALCommon.InitializeEL()

        AddHandler Application.ThreadException, AddressOf UFCWThreadExceptionHandler.Application_ThreadException
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf UFCWThreadExceptionHandler.CurrentDomain_UnhandledException

        Try

            Application.Run(New TestForm)

        Catch ex As Exception

            MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub Refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRefresh.Click

        CobControl1.ClearAll()

        Me.CobControl1.ReadOnlyMode = CBool(ReadOnlyCheckBox.CheckState = CheckState.Checked)

        Me.CobControl1.LoadCOB(CType(Me.FamilyID.Text.ToString, Integer), If(Me.RelationID.Text.Trim.Length > 0, CType(Me.RelationID.Text.Trim, Short?), Nothing))

    End Sub

    Private Sub ButtonClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClear.Click

        Me.CobControl1.ClearAll()

    End Sub

#End Region
End Class