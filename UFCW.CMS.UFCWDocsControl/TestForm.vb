Imports UFCW.WCF
Imports UFCW.WCF.FileNet

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

        UFCWDocsControl1.ClearAll()

        Me.UFCWDocsControl1.ReadOnlyMode = CBool(ReadOnlyCheckBox.CheckState = CheckState.Checked)

        Me.UFCWDocsControl1.LoadUFCWDOCSFromSSN(CType(Me.PartSSN.Text.ToString, Integer))

    End Sub

    Private Sub ButtonClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClear.Click

        Me.UFCWDocsControl1.ClearAll()

    End Sub

    Private Sub TestForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim FNInfo As Tuple(Of String, String) = WCFWrapperCommon.InitializeFileNetUserInfo()

        Using FNSession As New Session(New WCFProcessInfo With {.ByProcessName = False, .ProcessID = Process.GetCurrentProcess.Id, .ProcessName = Process.GetCurrentProcess.ProcessName})

            If FNSession.LoggedOn = False Then FNSession.Logon(FNInfo.Item1, FNInfo.Item2)

        End Using

    End Sub

#End Region
End Class