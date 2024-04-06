Option Strict On
Option Infer On

Imports Microsoft.Office.Interop

Public Class DCNListLookup
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Friend WithEvents pnlExcel As Panel
#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents OKButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.pnlExcel = New System.Windows.Forms.Panel()
        Me.SuspendLayout()
        '
        'OKButton
        '
        Me.OKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OKButton.Location = New System.Drawing.Point(467, 332)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 44
        Me.OKButton.Text = "OK"
        '
        'pnlExcel
        '
        Me.pnlExcel.Location = New System.Drawing.Point(13, 13)
        Me.pnlExcel.Name = "pnlExcel"
        Me.pnlExcel.Size = New System.Drawing.Size(527, 299)
        Me.pnlExcel.TabIndex = 45
        '
        'DCNListLookup
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.OKButton
        Me.ClientSize = New System.Drawing.Size(552, 358)
        Me.Controls.Add(Me.pnlExcel)
        Me.Controls.Add(Me.OKButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(392, 176)
        Me.Name = "DCNListLookup"
        Me.ShowInTaskbar = False
        Me.Text = "Select Diagnosis ..."
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Form Events"
    Private Sub DiagnosisLookup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            If Not UFCWGeneral.SetFormPosition(Me) Then Me.CenterToScreen()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DetailLineDiagnosis_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        UFCWGeneral.SaveFormPosition(Me)

    End Sub

#End Region

    Public Sub ReleaseComObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)

        Catch ex As Exception
        Finally
            obj = Nothing
        End Try
    End Sub

    Private Sub OKButton_Click(sender As Object, e As EventArgs) Handles OKButton.Click
        Dim sExcelFileName As String = "C:Temp\dcnBook1.xlsx"
        Dim oExcel As New Excel.Application

        oExcel.DisplayAlerts = False
        oExcel.Workbooks.Open(sExcelFileName)
        oExcel.Application.WindowState = Excel.XlWindowState.xlMaximized
        oExcel.Visible = True

        NativeMethods.SetParent(CType(oExcel.Hwnd, IntPtr), pnlExcel.Handle)
        NativeMethods.SendMessage(CType(oExcel.Hwnd, IntPtr), NativeMethods.WM_SYSCOMMAND, CType(SC.SC_MAXIMIZE, IntPtr), IntPtr.Zero)

    End Sub
End Class