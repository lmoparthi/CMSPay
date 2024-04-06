Imports System.Threading
Imports System.Windows.Forms
Imports System.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports System.Reflection
Imports System.Threading.Tasks
Imports System.Runtime.ExceptionServices
Imports System.Text


Public Class CMSDALLastKeyData
    Private Shared _ClaimID As New Stack
    Private Shared _DocumentID As New Stack
    Private Shared _FamilyID As New Stack
    Private Shared _RelationID As New Stack
    Private Shared _Log As New Stack
    Private Shared _CMD As New Stack
    Private Shared _SQL As New Stack
    Private Shared _SSN As New Stack
    Private Shared _TEXT As New Stack
    Private Shared _StackSizeLimit As Integer = If(ConfigurationManager.AppSettings("StackSizeLimit") Is Nothing, 3, CInt(ConfigurationManager.AppSettings("StackSizeLimit")))

    Public Shared Property ClaimID As String

        Get
            If _ClaimID Is Nothing Then Return ""
            Return String.Join(";", _ClaimID.ToArray)
        End Get
        Set(value As String)
            If Not _ClaimID.Contains(value) Then _ClaimID.Push(Now.ToLongTimeString.ToString & "- " & value)
            If _ClaimID.Count > _StackSizeLimit Then _ClaimID.Pop()
        End Set
    End Property

    Public Shared Property DocumentID As String

        Get
            If _DocumentID Is Nothing Then Return ""
            Return String.Join(";", _DocumentID.ToArray)
        End Get
        Set(value As String)
            If Not _DocumentID.Contains(value) Then _DocumentID.Push(Now.ToLongTimeString.ToString & "- " & value)
            If _DocumentID.Count > _StackSizeLimit Then _DocumentID.Pop()
        End Set
    End Property
    Public Shared Property FamilyID As String

        Get
            If _FamilyID Is Nothing Then Return ""
            Return String.Join(";", _FamilyID.ToArray)
        End Get
        Set(value As String)
            If Not _FamilyID.Contains(value) Then _FamilyID.Push(Now.ToLongTimeString.ToString & "- " & value)
            If _FamilyID.Count > _StackSizeLimit Then _FamilyID.Pop()
        End Set
    End Property

    Public Shared Property RelationID As String

        Get
            If _RelationID Is Nothing Then Return ""
            Return String.Join(";", _RelationID.ToArray)
        End Get
        Set(value As String)
            If Not _RelationID.Contains(value) Then _RelationID.Push(Now.ToLongTimeString.ToString & "- " & value)
            If _RelationID.Count > _StackSizeLimit Then _RelationID.Pop()
        End Set
    End Property

    Public Shared Property SQL As String

        Get
            If _SQL Is Nothing Then Return ""
            Return String.Join(";", _SQL.ToArray)
        End Get
        Set(value As String)
            If Not _SQL.Contains(value) Then _SQL.Push(Now.ToLongTimeString.ToString & "- " & value)
            If _SQL.Count > _StackSizeLimit Then _SQL.Pop()
        End Set
    End Property

    Public Shared Property CMD As String

        Get
            If _CMD Is Nothing Then Return ""
            Return String.Join(";", _CMD.ToArray)
        End Get
        Set(value As String)
            If Not _CMD.Contains(value) Then _CMD.Push(Now.ToLongTimeString.ToString & "- " & value)
            If _CMD.Count > _StackSizeLimit Then _CMD.Pop()
        End Set
    End Property

    Public Shared Property Log As String

        Get
            If _Log Is Nothing Then Return ""
            Return String.Join(";", _Log.ToArray)
        End Get
        Set(value As String)
            If Not _Log.Contains(value) Then _Log.Push(value)
            If _Log.Count > _StackSizeLimit Then _Log.Pop()
        End Set
    End Property

    Public Shared Property TEXT As String

        Get
            If _TEXT Is Nothing Then Return ""
            Return String.Join(";", _TEXT.ToArray)
        End Get
        Set(value As String)
            If Not _TEXT.Contains(value) Then _TEXT.Push(Now.ToLongTimeString.ToString & "- " & value)
            If _TEXT.Count > _StackSizeLimit Then _TEXT.Pop()
        End Set
    End Property

    Public Shared Property SSN As String

        Get
            If _SSN Is Nothing Then Return ""
            Return String.Join(";", _SSN.ToArray)
        End Get
        Set(value As String)
            If Not _SSN.Contains(value) Then _SSN.Push(Now.ToLongTimeString.ToString & "- " & value)
            If _SSN.Count > _StackSizeLimit Then _SSN.Pop()
        End Set
    End Property
End Class

Public Class CMSDALThreadExceptionHandler
    Private Shared _ComputerName As String = SystemInformation.ComputerName
    Private Shared _DomainUser As String = SystemInformation.UserName
    Private Shared _ReportExceptionSyncLock As New Object
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    <System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions>
    Public Shared Sub Application_ThreadException(ByVal sender As System.Object, ByVal e As UnobservedTaskExceptionEventArgs)

        Try
            ' Exit the program if the user clicks Abort.

            ReportException("Unhandled Unobserved Task Thread Exception:" & vbCrLf & vbCrLf & e.Exception.Flatten.Message & vbCrLf & vbCrLf & e.Exception.Flatten.GetType().ToString() & vbCrLf & vbCrLf & "Stack Trace:" & vbCrLf & e.Exception.Flatten.StackTrace)

        Catch ex As Exception

            MessageBox.Show(CType(e.Exception, Exception).ToString & vbCrLf & ex.Message & vbCrLf & ex.StackTrace, "Unhandled Fatal Error in Application", MessageBoxButtons.OK, MessageBoxIcon.Stop)

        Finally

            Try


                Logger.Write("Exited due to Application UnobservedTask Thread Exception.")
                Logger.Write(CType(e.Exception, Exception).ToString, "Exception")
                Logger.Write(General.NowDate.ToString)

            Catch ex As Exception

            End Try

            Try
                Application.Exit()

            Catch ex As Exception
            End Try

        End Try

    End Sub

    <System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions>
    Public Shared Sub Application_ThreadException(ByVal sender As System.Object, ByVal e As ThreadExceptionEventArgs)

        Try
            ' Exit the program if the user clicks Abort.
            '            ReportException("Unhandled Thread Exception:" & vbCrLf & vbCrLf & e.Exception.Message & vbCrLf & vbCrLf & e.Exception.GetType().ToString() & vbCrLf & vbCrLf & "Stack Trace:" & vbCrLf & e.Exception.StackTrace)

            ReportException(e.Exception.Message & vbCrLf & vbCrLf & "Stack Trace:" & vbCrLf & e.Exception.StackTrace)

        Catch ex As Exception

            MessageBox.Show(e.Exception.ToString & vbCrLf & ex.Message & vbCrLf & ex.StackTrace, "Unhandled Fatal Error in Application(ReportException)", MessageBoxButtons.OK, MessageBoxIcon.Stop)

        Finally

            Try

                Logger.Write("Exited due to Application Thread Exception.")
                Logger.Write(e.Exception.ToString, "Exception")
                Logger.Write(General.NowDate.ToString)

            Catch ex As Exception
            End Try

            Environment.Exit(0)

        End Try
    End Sub

    <System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions>
    Public Shared Sub CurrentDomain_FirstChanceException(ByVal sender As Object, ByVal e As FirstChanceExceptionEventArgs)

        Try
            ' Exit the program if the user clicks Abort.

            ReportException("First Chance Exception:" & vbCrLf & AppDomain.CurrentDomain.FriendlyName & vbCrLf & e.Exception.Message & vbCrLf & vbCrLf & e.Exception.GetType().ToString() & vbCrLf & vbCrLf & "Stack Trace:" & vbCrLf & e.Exception.StackTrace)

        Catch ex As Exception

            MessageBox.Show(CType(e.Exception, Exception).ToString & vbCrLf & ex.Message & vbCrLf & ex.StackTrace, "Unhandled Fatal Error in Application", MessageBoxButtons.OK, MessageBoxIcon.Stop)

        Finally
        End Try

    End Sub

    <System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions>
    Public Shared Sub CurrentDomain_UnhandledException(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)

        Try
            ' Exit the program if an email was successfully sent

            ReportException("Unhandled Exception: " & If(CType(e.ExceptionObject, Exception).InnerException IsNot Nothing, CType(e.ExceptionObject, Exception).InnerException.ToString, " No Inner Exception") & vbCrLf & vbCrLf & vbCrLf & "Stack Trace:" & vbCrLf & CType(e.ExceptionObject, Exception).StackTrace.ToString & vbCrLf & e.ExceptionObject.ToString)

        Catch ex As Exception
            ' Fatal error, terminate program

            Logger.Write(General.NowDate.ToString & "A: " & vbTab & "Error in Exception Handler.")
            Logger.Write(General.NowDate.ToString & "B: " & vbTab & ex.ToString)
            Logger.Write(General.NowDate.ToString & "C: " & vbTab & e.ToString)
            Logger.Write(General.NowDate.ToString & "D: " & vbTab & e.ExceptionObject.ToString)
            Logger.Write(General.NowDate.ToString & "E: " & vbTab & If(CType(e.ExceptionObject, Exception).InnerException IsNot Nothing, CType(e.ExceptionObject, Exception).InnerException.ToString, " No Inner Exception"))

            If CType(e.ExceptionObject, Exception).InnerException IsNot Nothing Then
                Logger.Write(General.NowDate.ToString & "F: " & vbTab & CType(e.ExceptionObject, Exception).InnerException.Message.ToString)
            End If

            MessageBox.Show(If(CType(e.ExceptionObject, Exception).InnerException IsNot Nothing, CType(e.ExceptionObject, Exception).InnerException.ToString, " No Inner Exception") & vbCrLf & ex.Message & vbCrLf & ex.StackTrace, "Unhandled Fatal Error in CurrentDomain", MessageBoxButtons.OK, MessageBoxIcon.Stop)

        Finally
            Try

                Logger.Write(General.NowDate.ToString & vbTab & "Exited due to Application Unhandled Exception.")
                Logger.Write(General.NowDate.ToString & vbTab & "e.GetType " & e.GetType.ToString)
                Logger.Write(General.NowDate.ToString & vbTab & "e.ExceptionObject.GetType " & e.ExceptionObject.GetType.ToString)
                Logger.Write(General.NowDate.ToString & vbTab & " " & e.ExceptionObject.ToString)

                Logger.Write(If(CType(e.ExceptionObject, Exception).InnerException IsNot Nothing, CType(e.ExceptionObject, Exception).InnerException.ToString, " No Inner Exception"), "Exception")

            Catch ex2 As Exception

                MessageBox.Show(If(CType(e.ExceptionObject, Exception).InnerException IsNot Nothing, CType(e.ExceptionObject, Exception).InnerException.ToString, " No Inner Exception") & vbCrLf & ex2.Message & vbCrLf & ex2.StackTrace, "Unhandled Fatal Error in CurrentDomain", MessageBoxButtons.OK, MessageBoxIcon.Stop)

            End Try

            Application.Exit()
        End Try

    End Sub
    <System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions>
    Private Shared Sub ReportException(ByVal errorMessage As String)

        SyncLock (_ReportExceptionSyncLock)

            Dim SummaryTxt As String
            Dim BodySB As New StringBuilder

            Try

                SummaryTxt = "Application Exception - " & Assembly.GetEntryAssembly().FullName & " - Computer: " & _ComputerName.ToString & " - User: " & _DomainUser
                BodySB.Append(Now.ToLongTimeString.ToString & vbCrLf & SummaryTxt & vbCrLf & errorMessage)

                BodySB.Append(vbCrLf & vbCrLf & "Failure in: " & Application.ExecutablePath.ToString & vbCrLf & "Log: " & "<" & CMSDALLastKeyData.Log & ">" & vbCrLf & "CMD: " & CMSDALLastKeyData.CMD & vbCrLf & "TEXT: " & CMSDALLastKeyData.TEXT & vbCrLf & "ClaimID: " & CMSDALLastKeyData.ClaimID & vbCrLf & "FamilyID: " & CMSDALLastKeyData.FamilyID & vbCrLf & "RelationID: " & CMSDALLastKeyData.RelationID & vbCrLf & "SSN: " & CMSDALLastKeyData.SSN & vbCrLf & "SQL: " & CMSDALLastKeyData.SQL & vbCrLf & "DocID: " & CMSDALLastKeyData.DocumentID)

                If CMSDALCommon.MailEnabled Then errorMessage &= CMSDALCommon.SendMail(SummaryTxt, BodySB.ToString)

                System.Diagnostics.Trace.WriteLine("")

                Console.WriteLine(BodySB.ToString)

                'build before sending email to avoid additional errors obscuring problem
                Logger.Write("In ShowThreadExceptionDialog ")
                Logger.Write("Failure in: " & Application.ExecutablePath.ToString)
                Logger.Write(SummaryTxt & vbCrLf & errorMessage)

                Logger.Write(vbCrLf & "Log: " & CMSDALLastKeyData.Log)
                Logger.Write(vbCrLf & "Command Line: " & CMSDALLastKeyData.CMD)
                Logger.Write(vbCrLf & "Text: " & CMSDALLastKeyData.TEXT)
                Logger.Write(vbCrLf & "ClaimID: " & CMSDALLastKeyData.ClaimID)
                Logger.Write(vbCrLf & "FamilyID: " & CMSDALLastKeyData.FamilyID)
                Logger.Write(vbCrLf & "RelationID: " & CMSDALLastKeyData.RelationID)
                Logger.Write(vbCrLf & "SSN: " & CMSDALLastKeyData.SSN)
                Logger.Write(vbCrLf & "SQL: " & CMSDALLastKeyData.SQL)
                Logger.Write(vbCrLf & "DocID: " & CMSDALLastKeyData.DocumentID)


            Catch ex As Exception
                MessageBox.Show("Attempt to log Error failed. " & vbCrLf & "Capture screen and report to Help desk" & vbCrLf & errorMessage & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex.GetType().ToString() & vbCrLf & vbCrLf & "Stack Trace:" & vbCrLf & ex.StackTrace)
            Finally

                If ConfigurationManager.AppSettings("ShowExceptionDialog") IsNot Nothing AndAlso CBool(ConfigurationManager.AppSettings("ShowExceptionDialog")) Then
                    MessageBox.Show(BodySB.ToString, "Application Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End If

            End Try
        End SyncLock


    End Sub

End Class
