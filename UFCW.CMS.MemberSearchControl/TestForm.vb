Public Class TestForm

    Public Shared Sub Main()

        AddHandler Application.ThreadException, AddressOf UFCWThreadExceptionHandler.Application_ThreadException
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf UFCWThreadExceptionHandler.CurrentDomain_UnhandledException

        CMSDALCommon.InitializeEL()

        Try

            Application.Run(New TestForm)

        Catch ex As Exception

            MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

End Class