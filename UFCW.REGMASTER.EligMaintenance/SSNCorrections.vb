Option Strict On
Imports System.ComponentModel
Imports System.Windows.Forms
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class SSNCorrections

    Private _APPKEY As String = "UFCW\RegMaster\"

    <Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")> _
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

    Private Sub SsnChangeControl_Load(sender As Object, e As EventArgs) Handles Me.Load
        '' Me.Cursor = Cursors.Default
    End Sub

    Private Sub SetSettings()

        Me.Top = If(CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(Me.AppKey, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)
    End Sub

    Private Sub SSNCorrections_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Try
            If SsnChangeControl.MadeHRSChanges = True Then
                MessageBox.Show(Me, "Changes have been made to Social Security Changes Screen." & vbCrLf & _
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
End Class