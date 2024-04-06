Imports System.Configuration
Imports System.Reflection
Imports System.Runtime.ExceptionServices
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms
Imports Microsoft.Practices.EnterpriseLibrary.Logging

Public Class UFCWThreadExceptionHandler

    Private Shared _ComputerName As String = UFCWGeneral.ComputerName
    Private Shared _DomainUser As String = UFCWGeneral.WindowsUserID.Name
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
                Logger.Write(UFCWGeneral.NowDate.ToString)

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

            Dim CombinedMessage As New StringBuilder("Application_ThreadException: ")
            Dim CombinedStackTrace As New StringBuilder("Stack Trace: ")

            CombinedMessage.Append(CType(e.Exception, Exception).Message & Environment.NewLine)
            CombinedStackTrace.Append(CType(e.Exception, Exception).StackTrace & Environment.NewLine)

            If CType(e.Exception, Exception).InnerException IsNot Nothing Then

                CombinedMessage.Append(" Inner Exception: " & Environment.NewLine)

                If TypeOf e.Exception Is AggregateException Then
                    For Each IE As Exception In CType(e.Exception, AggregateException).InnerExceptions
                        CombinedMessage.Append(IE.Message & Environment.NewLine)
                        CombinedStackTrace.Append(IE.StackTrace & Environment.NewLine)
                    Next
                Else
                    CombinedMessage.Append(CType(e.Exception, Exception).InnerException.Message & Environment.NewLine)
                    CombinedStackTrace.Append(CType(e.Exception, Exception).InnerException.StackTrace & Environment.NewLine)
                End If

                CombinedMessage.Append(CombinedStackTrace.ToString)

            Else

                CombinedMessage.Append(CombinedStackTrace.ToString)
                CombinedMessage.Append(Environment.NewLine & " No Inner Exception")

            End If

            ReportException(CombinedMessage.ToString)

        Catch ex As Exception

            MessageBox.Show(e.Exception.ToString & vbCrLf & ex.Message & vbCrLf & ex.StackTrace, "Unhandled Fatal Error in Application(ReportException)", MessageBoxButtons.OK, MessageBoxIcon.Stop)

        Finally

            Try

                Logger.Write("Exited due to Application Thread Exception.")
                Logger.Write(e.Exception.ToString, "Exception")
                Logger.Write(UFCWGeneral.NowDate.ToString)

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

            ReportException("CurrentDomain_UnhandledException: " & If(CType(e.ExceptionObject, Exception).InnerException IsNot Nothing, CType(e.ExceptionObject, Exception).InnerException.ToString, " No Inner Exception") & vbCrLf & vbCrLf & vbCrLf & "Stack Trace:" & vbCrLf & CType(e.ExceptionObject, Exception).StackTrace.ToString & vbCrLf & e.ExceptionObject.ToString)

        Catch ex As Exception
            ' Fatal error, terminate program

            Logger.Write(UFCWGeneral.NowDate.ToString & "A: " & vbTab & "Error in Exception Handler.")
            Logger.Write(UFCWGeneral.NowDate.ToString & "B: " & vbTab & ex.ToString)
            Logger.Write(UFCWGeneral.NowDate.ToString & "C: " & vbTab & e.ToString)
            Logger.Write(UFCWGeneral.NowDate.ToString & "D: " & vbTab & e.ExceptionObject.ToString)
            Logger.Write(UFCWGeneral.NowDate.ToString & "E: " & vbTab & If(CType(e.ExceptionObject, Exception).InnerException IsNot Nothing, CType(e.ExceptionObject, Exception).InnerException.ToString, " No Inner Exception"))

            If CType(e.ExceptionObject, Exception).InnerException IsNot Nothing Then
                Logger.Write(UFCWGeneral.NowDate.ToString & "F: " & vbTab & CType(e.ExceptionObject, Exception).InnerException.Message.ToString)
            End If

            MessageBox.Show(If(CType(e.ExceptionObject, Exception).InnerException IsNot Nothing, CType(e.ExceptionObject, Exception).InnerException.ToString, " No Inner Exception") & vbCrLf & ex.Message & vbCrLf & ex.StackTrace, "Unhandled Fatal Error in CurrentDomain", MessageBoxButtons.OK, MessageBoxIcon.Stop)

        Finally
            Try

                Logger.Write(UFCWGeneral.NowDate.ToString & vbTab & "Exited due to Application Unhandled Exception.")
                Logger.Write(UFCWGeneral.NowDate.ToString & vbTab & "e.GetType " & e.GetType.ToString)
                Logger.Write(UFCWGeneral.NowDate.ToString & vbTab & "e.ExceptionObject.GetType " & e.ExceptionObject.GetType.ToString)
                Logger.Write(UFCWGeneral.NowDate.ToString & vbTab & " " & e.ExceptionObject.ToString)

                Logger.Write(If(CType(e.ExceptionObject, Exception).InnerException IsNot Nothing, CType(e.ExceptionObject, Exception).InnerException.ToString, " No Inner Exception"), "Exception")

            Catch ex2 As Exception

                MessageBox.Show(If(CType(e.ExceptionObject, Exception).InnerException IsNot Nothing, CType(e.ExceptionObject, Exception).InnerException.ToString, " No Inner Exception") & vbCrLf & ex2.Message & vbCrLf & ex2.StackTrace, "Unhandled Fatal Error in CurrentDomain", MessageBoxButtons.OK, MessageBoxIcon.Stop)

            End Try

            Application.Exit()
        End Try

    End Sub
    <System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions>
    Private Shared Sub ReportException(ByVal errorMessage As String, Optional MailEnabled As Boolean = True)

        SyncLock (_ReportExceptionSyncLock)

            Dim SummaryTxt As String
            Dim BodySB As New StringBuilder

            Try

                SummaryTxt = "Application Exception - " & Assembly.GetEntryAssembly().FullName & " - Computer: " & _ComputerName.ToString & " - User: " & _DomainUser

                BodySB.Append(Now.ToLongTimeString.ToString & vbCrLf & SummaryTxt & vbCrLf & errorMessage)

                BodySB.Append(vbCrLf & vbCrLf & "Failure in: " & Application.ExecutablePath.ToString & vbCrLf & "Log: " & "<" & UFCWLastKeyData.Log & ">" & vbCrLf & "CMD: " & UFCWLastKeyData.CMD & vbCrLf & "TEXT: " & UFCWLastKeyData.TEXT & vbCrLf & "ClaimID: " & UFCWLastKeyData.ClaimID & vbCrLf & "FamilyID: " & UFCWLastKeyData.FamilyID & vbCrLf & "RelationID: " & UFCWLastKeyData.RelationID & vbCrLf & "SSN: " & UFCWLastKeyData.SSN & vbCrLf & "SQL: " & UFCWLastKeyData.SQL & vbCrLf & "DocID: " & UFCWLastKeyData.DocumentID)

                If UFCWEMail.MailEnabled Then errorMessage &= UFCWEMail.SendMail(SummaryTxt, BodySB.ToString)

                System.Diagnostics.Trace.WriteLine("")

                Console.WriteLine(BodySB.ToString)

                'build before sending email to avoid additional errors obscuring problem
                Logger.Write("In ShowThreadExceptionDialog ")
                Logger.Write("Failure in: " & Application.ExecutablePath.ToString)
                Logger.Write(SummaryTxt & vbCrLf & errorMessage)

                Logger.Write(vbCrLf & "Log: " & UFCWLastKeyData.Log)
                Logger.Write(vbCrLf & "Command Line: " & UFCWLastKeyData.CMD)
                Logger.Write(vbCrLf & "Text: " & UFCWLastKeyData.TEXT)
                Logger.Write(vbCrLf & "ClaimID: " & UFCWLastKeyData.ClaimID)
                Logger.Write(vbCrLf & "FamilyID: " & UFCWLastKeyData.FamilyID)
                Logger.Write(vbCrLf & "RelationID: " & UFCWLastKeyData.RelationID)
                Logger.Write(vbCrLf & "SSN: " & UFCWLastKeyData.SSN)
                Logger.Write(vbCrLf & "SQL: " & UFCWLastKeyData.SQL)
                Logger.Write(vbCrLf & "DocID: " & UFCWLastKeyData.DocumentID)


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
