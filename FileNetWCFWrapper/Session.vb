Imports UFCW.WCF.FileNet

Public Class Session
    Implements IDisposable

    Private Shared _TraceWCF As New BooleanSwitch("TraceWCF", "Trace Switch in App.Config")

    Private _WCFServiceFileNetProperties As FileNetSessionProperties

    Public ReadOnly Property WCFServiceFileNetProperties As FileNetSessionProperties
        Get
            Return _WCFServiceFileNetProperties
        End Get
    End Property

    Public ReadOnly Property LoggedOn() As Boolean
        Get
            'If (String.IsNullOrEmpty(WCFWrapperCommon.FileNetUserID) OrElse WCFWrapperCommon.FileNetUserID.Trim.Length = 0) OrElse WCFWrapperCommon.FileNetPassword.Length = 0 Then Throw New ApplicationException("Logon with UserID and Password to continue. ")

            If _WCFServiceFileNetProperties Is Nothing Then RefreshFileNetSessionProperties()

            Return _WCFServiceFileNetProperties.LoggedOn
        End Get
    End Property

    Public ReadOnly Property LibraryName() As String
        Get

            If _WCFServiceFileNetProperties Is Nothing Then RefreshFileNetSessionProperties()
            Return _WCFServiceFileNetProperties.LibraryName
        End Get
    End Property

    Public ReadOnly Property UserName() As String
        Get
            'If (String.IsNullOrEmpty(WCFWrapperCommon.FileNetUserID) OrElse WCFWrapperCommon.FileNetUserID.Trim.Length = 0) OrElse WCFWrapperCommon.FileNetPassword.Length = 0 Then Throw New ApplicationException("Logon with UserID and Password to continue. ")

            If _WCFServiceFileNetProperties Is Nothing Then RefreshFileNetSessionProperties()

            Return _WCFServiceFileNetProperties.UserName
        End Get
    End Property

    Sub New()

        WCFWrapperCommon.WCFProcessInfo = New WCFProcessInfo With {.ByProcessName = True, .ProcessID = Process.GetCurrentProcess.Id, .ProcessName = Process.GetCurrentProcess.ProcessName}

    End Sub

    Sub New(WCFProcessInfo As WCFProcessInfo)

        WCFWrapperCommon.WCFProcessInfo = WCFProcessInfo

    End Sub

    Public Function Logon(ByVal fileNetUserID As String, ByVal fileNetPassword As String) As Boolean
        Try

            WCFWrapperCommon.FileNetUserID = fileNetUserID.Trim
            WCFWrapperCommon.FileNetPassword = fileNetPassword.Trim

            If WCFWrapperCommon.FileNetUserID.Length = 0 OrElse WCFWrapperCommon.FileNetPassword.Length = 0 Then Throw New ApplicationException("UserID and Password are required. ")

            _WCFServiceFileNetProperties = WCFWrapperCommon.WCFServiceHostClient.Logon(WCFWrapperCommon.WCFServiceFaultException, WCFWrapperCommon.FileNetUserID, WCFWrapperCommon.FileNetPassword)

            Return _WCFServiceFileNetProperties.LoggedOn


        Catch ex As System.ServiceModel.CommunicationObjectAbortedException

            Return False

        Catch ex As System.ServiceModel.CommunicationObjectFaultedException

            Return False

        Catch ex As System.ServiceModel.CommunicationException

            Return False

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Sub Logoff()
        Try

            If _WCFServiceFileNetProperties Is Nothing Then RefreshFileNetSessionProperties()

            _WCFServiceFileNetProperties.LoggedOn = WCFWrapperCommon.WCFServiceHostClient.Logoff(WCFWrapperCommon.WCFServiceFaultException)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    'Public Sub Terminate()
    '    Try

    '        If WCFWrapperCommon.WCFServiceHostClient IsNot Nothing Then
    '            WCFWrapperCommon.DetachFromService(New WCFProcessInfo With {.ProcessID = Process.GetCurrentProcess.Id, .ProcessName = Process.GetCurrentProcess.ProcessName})
    '        End If

    '    Catch fe As FaultException When fe.Message.Contains("Terminate")
    '        'normal termination
    '    Catch ex As Exception
    '        Throw
    '    Finally
    '    End Try
    'End Sub

    Private Sub RefreshFileNetSessionProperties()
        Try

            'If (String.IsNullOrEmpty(WCFWrapperCommon.FileNetUserID) OrElse WCFWrapperCommon.FileNetUserID.Trim.Length = 0) OrElse WCFWrapperCommon.FileNetPassword.Length = 0 Then Throw New ApplicationException("Logon with UserID and Password to continue. ")

            _WCFServiceFileNetProperties = WCFWrapperCommon.WCFServiceHostClient.SessionProperties(WCFWrapperCommon.WCFServiceFaultException)

        Catch ex As Exception
            Throw
        End Try
    End Sub

#Region "Dispose"

    Private _Disposed As Boolean = False

    ' Implement IDisposable.
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overridable Overloads Sub Dispose(disposing As Boolean)
        If _Disposed = False Then
            If disposing Then
                ' Free other state (managed objects).
                _Disposed = True
            End If
            ' Free your own state (unmanaged objects).
            ' Set large fields to null.

        End If
    End Sub

    Protected Overrides Sub Finalize()
        ' Simply call Dispose(False).
        Dispose(False)
    End Sub

#End Region

End Class
