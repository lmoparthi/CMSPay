Imports System.Net.Mail
Imports System.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports System.Net
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates

Public Class UFCWEMail
    Public Shared ReadOnly Property MailTo As String
        Get
            Dim Mail2 As String

            If ConfigurationManager.AppSettings("MailTo").Contains("#") Then
                If (CType(ConfigurationManager.GetSection("dataConfiguration"), Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings).DefaultDatabase.Contains(" P ")) Then
                    Mail2 = ConfigurationManager.AppSettings("MailToProd")
                Else
                    Mail2 = ConfigurationManager.AppSettings("MailToTest")
                End If
            End If
            Return If(Mail2 IsNot Nothing AndAlso Mail2.Trim.Length > 0, Mail2, ConfigurationManager.AppSettings("MailTo"))

        End Get
    End Property

    Public Shared Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal errors As SslPolicyErrors) As Boolean
        Return True
    End Function

    Public Shared Property MailCC As String = ConfigurationManager.AppSettings("MailCC")
    Public Shared Property MailBCC As String = ConfigurationManager.AppSettings("MailBCC")
    Public Shared Property MailSmtpClient As String = ConfigurationManager.AppSettings("SMTPServer")

    Public Shared ReadOnly Property MailFrom As String
        Get
            Try

                Return ConfigurationManager.AppSettings("MailFrom").ToString.Replace("%", UFCWGeneral.ComputerName.Replace("UFCW", ""))

            Catch ex As Exception
                Throw
            End Try
        End Get
    End Property

    Public Shared ReadOnly Property MailSubject As String
        Get
            Try

                Return ConfigurationManager.AppSettings("MailSubject").ToString.Replace("%", UFCWGeneral.ComputerName.Replace("UFCW", ""))

            Catch ex As Exception
                Throw
            End Try
        End Get
    End Property

    Public Shared ReadOnly Property MailEnabled As Boolean
        Get
            Try

                Return CBool(ConfigurationManager.AppSettings("EnableMail"))

            Catch ex As Exception
                Throw
            End Try

        End Get
    End Property

    Public Shared Function SendMail(ByVal mailSubject As String, ByVal mailBody As String) As String
        Try

            SendMail(MailTo, MailCC, MailBCC, MailFrom, mailSubject, mailBody) 'use app config defaults

            Return ""

        Catch ex As Exception

            Return "SendMail failed to report error due to (" & ex.Message & ")"
        End Try

    End Function

    Public Shared Sub SendMail(ByVal mailTo As String, ByVal mailCC As String, ByVal mailBCC As String, ByVal mailFrom As String, ByVal mailSubject As String, ByVal mailBody As String)
        SendMail(mailTo, mailCC, mailBCC, mailFrom, mailSubject, mailBody, Nothing)
    End Sub

    Public Shared Sub SendMail(ByVal mailTo As String, ByVal mailCC As String, ByVal mailBCC As String, ByVal mailFrom As String, ByVal mailSubject As String, ByVal mailBody As String, attachments As List(Of FileIOCommon))

        Dim EMailAttachmentsFiles As New List(Of FileIOCommon)

        Dim EMailAttachment As System.Net.Mail.Attachment
        Dim SMTPUserID As String
        Dim SMTPUserPW As String
        Dim SMTPServer As String
        Dim ToEmailIDs As String()
        Dim CCEmailIDs As String()
        Dim BCCEmailIDs As String()
        Dim MailBodyAV As AlternateView

        Try

            SMTPUserID = ConfigurationManager.AppSettings("SMTPUser")
            SMTPUserPW = ConfigurationManager.AppSettings("SMTPPW")
            SMTPServer = ConfigurationManager.AppSettings("SMTPServer")

            If SMTPUserID Is Nothing OrElse SMTPUserPW Is Nothing OrElse SMTPServer Is Nothing Then Throw New ApplicationException("Invalid SMTP Settings")

            EMailAttachmentsFiles = New List(Of FileIOCommon)

            mailTo = mailTo.Replace("#", "MSTONE")
            mailTo = mailTo.Replace(",", ";") ' this fixes a common mistake of using , as a seperator

            Try
                If mailFrom.IndexOf("[") > 0 Then
                    mailFrom = mailFrom.Substring(0, mailFrom.IndexOf("[")) & mailFrom.Substring(mailFrom.IndexOf("]") + 2)
                End If

            Catch ex As Exception
            End Try

            ToEmailIDs = mailTo.ToString().Split(CChar(";"))
            CCEmailIDs = mailCC?.ToString().Split(CChar(";"))
            BCCEmailIDs = mailBCC?.ToString().Split(CChar(";"))

            If BCCEmailIDs IsNot Nothing AndAlso BCCEmailIDs.Length > 0 Then
                mailFrom = "DoNotReply@SCUFCWFUNDS.COM"
            End If

            Using MailMessage = New System.Net.Mail.MailMessage

                If attachments IsNot Nothing Then
                    EMailAttachmentsFiles = attachments
                End If

                '' Multiple attachments
                If EMailAttachmentsFiles IsNot Nothing Then
                    For Each F As FileIOCommon In EMailAttachmentsFiles

                        Try

                            EMailAttachment = New System.Net.Mail.Attachment(F.FullName)
                            MailMessage.Attachments.Add(EMailAttachment)

                        Catch ex As Exception
                            mailBody = mailBody & Environment.NewLine & "Failed to attach: " & F.FullName
                        End Try

                    Next
                End If

                Try
                    MailMessage.From = New MailAddress(mailFrom)
                Catch ex As Exception
                    MailMessage.From = New MailAddress(System.Reflection.Assembly.GetEntryAssembly().GetName().Name)
                End Try

                For J As Integer = 0 To ToEmailIDs.Length - 1
                    If ToEmailIDs(J).Trim.Length > 0 Then MailMessage.To.Add(New MailAddress(ToEmailIDs(J)))
                Next

                If CCEmailIDs IsNot Nothing Then
                    For J As Integer = 0 To CCEmailIDs.Length - 1
                        If CCEmailIDs(J).Trim.Length > 0 Then MailMessage.CC.Add(New MailAddress(CCEmailIDs(J)))
                    Next
                End If

                If BCCEmailIDs IsNot Nothing Then
                    For J As Integer = 0 To BCCEmailIDs.Length - 1
                        If BCCEmailIDs(J).Trim.Length > 0 Then MailMessage.Bcc.Add(New MailAddress(BCCEmailIDs(J)))
                    Next
                End If

                If mailSubject.IndexOf("[") > 0 Then
                    mailSubject = UFCWGeneral.ApplyDateFormatting(mailSubject, False)
                End If

                MailMessage.Subject = mailSubject
                If mailBody Is Nothing Then
                    mailBody = ""
                End If

                If mailBody.IndexOf("[") > 0 Then
                    mailBody = UFCWGeneral.ApplyDateFormatting(mailBody, False)
                End If

                MailBodyAV = AlternateView.CreateAlternateViewFromString(mailBody, Nothing, "text/plain")

                Try
                    If mailBody.IndexOf(":SIG") > 0 Then
                        MailBodyAV = AlternateView.CreateAlternateViewFromString(mailBody.Replace(":SIG", Environment.NewLine & "<br /><br /> <img src=cid:UFCWOperatorSig>"), Nothing, "text/html")
                        Dim SIG As New LinkedResource(".\UFCWOperatorSig.gif") With {
                        .ContentId = "UFCWOperatorSig"
                    }
                        MailBodyAV.LinkedResources.Add(SIG)
                    End If

                Catch ex As Exception
                End Try

                MailMessage.AlternateViews.Add(MailBodyAV)

                ServicePointManager.ServerCertificateValidationCallback = DirectCast(System.Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)), System.Net.Security.RemoteCertificateValidationCallback)

                SMTPUserPW = UFCWCryptor.DecryptString(SMTPUserPW, "SMTP Logon ACCESS")

                Dim SmtpUser As New System.Net.NetworkCredential(SMTPUserID, SMTPUserPW, "SCUFCWFUNDS.COM")

                Using ServerMailClient = New System.Net.Mail.SmtpClient With {
                    .Host = SMTPServer,
                    .Port = 25,
                    .DeliveryMethod = SmtpDeliveryMethod.Network,
                    .UseDefaultCredentials = False,
                    .Credentials = SmtpUser,
                    .EnableSsl = False
                }

                    Try
                        ServerMailClient.Send(MailMessage)

                    Catch ex As System.Net.Mail.SmtpFailedRecipientException When ex.Message.Contains("Mailbox unavailable") Or ex.Message.Contains("Unable to send to a recipient")

                        Logger.Write("Failed to Send Email To - " & ex.FailedRecipient.ToString & " - " & ex.StatusCode & Environment.NewLine & ex.Message)

                    Catch ex As System.Net.Mail.SmtpException When ex.Message.Contains("Message size exceeds")

                        MailMessage.Attachments.Clear()

                        MailMessage.Body = mailBody & Environment.NewLine & "Attachment(s) too large to send. Check log for location."

                        ServerMailClient.Send(MailMessage)

                    Catch ex As Exception
                        Throw
                    End Try

                End Using

            End Using

        Catch ex As Exception

            Dim exHold As Exception = ex

            Try

                Logger.Write("Attempted to Send Email - From " & mailFrom.ToString & " - To - " & mailTo.ToString & " - Header - " & mailSubject.ToString & " - Message - " & mailBody.ToString & Environment.NewLine & If(ex.InnerException IsNot Nothing, ex.InnerException.Message, "") & Environment.NewLine & ex.Message & Environment.NewLine & ex.StackTrace)

            Catch ex2 As Exception

            End Try

            ex = exHold

            Throw

        Finally

            If EMailAttachment IsNot Nothing Then EMailAttachment.Dispose()
            If MailBodyAV IsNot Nothing Then MailBodyAV.Dispose()

        End Try

    End Sub

End Class
