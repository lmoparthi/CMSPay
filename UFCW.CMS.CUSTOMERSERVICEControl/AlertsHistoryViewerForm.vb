Imports System.ComponentModel


Public Class AlertsHistoryViewerForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"

    Private _FamilyID As Integer
    Private _RelationID As Integer
    Private _FromDate As Date
    Private _ThruDate As Date
    Private _AlertsViewDS As New DataSet

#Region "Public Properties"
    <DefaultValue("UFCW\Claims\"), Browsable(True), System.ComponentModel.Description("Represents the application key used when accessing the registry.")> _
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = Value
        End Set
    End Property
#End Region

    Private _Disposed As Boolean

    Protected Overrides Sub Finalize()
        Me.Dispose(False)
    End Sub

    ' Protected implementation of Dispose pattern.
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If Not _Disposed Then
            If disposing Then

                If components IsNot Nothing Then
                    components.Dispose()
                End If

                If _AlertsViewDS IsNot Nothing Then
                    _AlertsViewDS.Dispose()
                End If
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            ' TODO: set large fields to null.
            _Disposed = True
        End If

        ' Call the base class implementation.
        MyBase.Dispose(disposing)
    End Sub

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal FamilyID As Integer, ByVal RelationID As Integer)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _FamilyID = FamilyID
        _RelationID = RelationID
    End Sub
#End Region
#Region "Custom Procedure"

    Private Sub LoadAlerts()
        Try

            Using WC As New GlobalCursor

                _AlertsViewDS = CMSDALFDBEL.RetrieveRegAlertsByFamilyID(_FamilyID)
                AlertDataGrid.SuspendLayout()
                If _AlertsViewDS IsNot Nothing Then
                    If _AlertsViewDS.Tables.Count > 0 Then
                        _AlertsViewDS.Tables(0).DefaultView.RowFilter = "RELATION_ID = " & _RelationID
                        AlertDataGrid.DataSource = _AlertsViewDS.Tables(0)
                    End If
                End If

                AlertDataGrid.SetTableStyle()
                AlertDataGrid.ResumeLayout()

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

#End Region

#Region "Form Events"

    Private Sub AlertsHistoryViewerForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

        LoadAlerts()

    End Sub

    Private Sub AlertsHistoryViewerForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        Try
            UFCWGeneral.SaveFormPosition(Me, _APPKEY)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ExitButton_Click(sender As System.Object, e As System.EventArgs) Handles ExitButton.Click
        Me.Close()
    End Sub

#End Region

End Class