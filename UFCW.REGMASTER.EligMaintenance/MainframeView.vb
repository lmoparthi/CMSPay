Option Strict On
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ComponentModel
Imports System.Configuration
Imports System.Windows.Forms

Public Class frmMainframeView
    Inherits System.Windows.Forms.Form

#Region "Variables"

    Private _APPKEY As String = "UFCW\RegMaster\"
    Private _FamilyID As Integer
    Dim _HoursDS As New DataSet

#End Region

#Region "Public Properties"
    <DefaultValue("UFCW\RegMaster\"), Browsable(True), System.ComponentModel.Description("Represents the application key used when accessing the registry.")> _
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal Value As String)
            _APPKEY = Value
        End Set
    End Property
#End Region

    Private _Disposed As Boolean = False
    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            '
            If _HoursDS IsNot Nothing Then
                _HoursDS.Dispose()
            End If
            _HoursDS = Nothing

            If Not (components Is Nothing) Then
                components.Dispose()
            End If

        End If

        ' Free any unmanaged objects here.
        '
        _Disposed = True

        ' Call base class implementation.
        MyBase.Dispose(disposing)
    End Sub

#Region "Constructors"

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub
    Public Sub New(ByVal FamilyID As Integer)
        Me.New()
        _FamilyID = FamilyID
    End Sub

#End Region

#Region "Custom Procedure"

    Private Sub LoadMainframeview()
        Try

            _HoursDS = RegMasterDAL.RetrieveMainframeviewByFamilyID(_FamilyID)

            MainframeviewDataGrid.DataSource = _HoursDS.Tables(0)
            MainframeviewDataGrid.SetTableStyle()
            MainframeviewDataGrid.Sort = If(MainframeviewDataGrid.LastSortedBy, MainframeviewDataGrid.DefaultSort)

            MainframeviewDataGrid.CaptionText = "Total Eligibility Hours for Family (" & _FamilyID.ToString & ") "

        Catch ex As Exception

                Throw
        Finally
            Cursor.Current = Cursors.Default
        End Try
    End Sub

#End Region

#Region "Form Events"
    Private Sub frmMainframeView_load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetSettings()
        LoadMainframeview()
        Me.Focus()
    End Sub

    Private Sub SetSettings()
        Me.Top = If(CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(Me.AppKey, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)
    End Sub

    Private Sub SaveSettings()
        Dim lWindowState As FormWindowState = Me.WindowState
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "WindowState", CInt(lWindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = lWindowState

    End Sub

    Private Sub frmMainframeView_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        Try
            SaveSettings()
            MainframeviewDataGrid.TableStyles.Clear()
            MainframeviewDataGrid.DataSource = Nothing

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Private Sub ExitButton_Click(sender As System.Object, e As System.EventArgs) Handles ExitButton.Click
        Me.Close()
    End Sub

    Private Sub EligAcctHrsMaint_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        SaveSettings()
        Me.Dispose()
    End Sub

#End Region

End Class