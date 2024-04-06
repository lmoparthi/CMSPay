Imports System.Threading.Tasks

'Public Class CMSDALFileNet
'    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

'    Shared Sub New()
'    End Sub

'    Sub New()
'    End Sub

'    Public Shared Event UnobservedTaskException As EventHandler(Of UnobservedTaskExceptionEventArgs)
'    Public Shared Sub DisplayFNImage(ByVal docIDs() As Long)

'        Dim Docs As New List(Of Long?)

'        Try
'            For Each Doc As Long In docIDs.ToList
'                Docs.Add(Doc)
'            Next

'            DisplayFNImage(Docs)

'        Catch ex As Exception
'            Throw
'        End Try
'    End Sub

'    Public Shared Sub DisplayFNImage(ByVal docID As Long)

'        Try

'            DisplayFNImage(New List(Of Long?) From {docID})

'        Catch ex As Exception
'            Throw
'        End Try
'    End Sub

'    Public Shared Sub DisplayFNImage(ByVal docs As List(Of Long?))

'        Try

'            UFCWLastKeyData.DocumentID = String.Join(";", docs)

'            Dim AsyncTask As Task = Task.Factory.StartNew(Sub() FileNetImageDisplay.DisplayDocument(docs)).ContinueWith(Sub(t)
'                                                                                                                            Throw t.Exception
'                                                                                                                        End Sub,
'                                                        TaskContinuationOptions.OnlyOnFaulted)

'        Catch ae As AggregateException
'            ' Assume we know what's going on with this particular exception.
'            ' Rethrow anything else. AggregateException.Handle provides
'            ' another way to express this. See later example.
'            For Each ex As Exception In ae.InnerExceptions
'                Throw
'            Next

'        Catch ex As Exception
'            Throw
'        End Try

'    End Sub

'    Private Sub HandleError(sender As Object, e As UnobservedTaskExceptionEventArgs) Handles Me.UnobservedTaskException
'        UFCWThreadExceptionHandler.Application_ThreadException(sender, e)
'    End Sub

'    Public Shared Function InitializeFileNetUserInfo() As Tuple(Of String, String)
'        ' -----------------------------------------------------------------------------
'        ' <summary>
'        ' Gets User Name and Password based on AD Security Roles.
'        ' </summary>
'        ' <remarks>
'        ' </remarks>
'        ' <history>
'        ' 	[Nick Snyder]	4/7/2006	Created
'        ' </history>
'        ' -----------------------------------------------------------------------------

'        Dim FileNetUser As Object
'        Dim FileNetPassword As Object
'        Dim FNUser As String
'        Dim FNPassword As String
'        Const CRYPTORKEY As String = "UFCW CMS ACCESS"

'        Try
'            If UFCWGeneralAD.CMSCanAdjudicateEmployee OrElse UFCWGeneralAD.REGMEmployeeAccess Then

'                If UFCWGeneralAD.CMSAdministrators OrElse UFCWGeneralAD.REGMAdministrators Then
'                    FileNetUser = CMSDALFDBMD.RetrieveRoleSettings("FN Admin User Name")
'                    If FileNetUser IsNot Nothing AndAlso Not IsDBNull(FileNetUser) Then
'                        FNUser = Cryptor.Cryptor.DecryptString(FileNetUser.ToString, CRYPTORKEY)
'                    Else
'                        FNUser = ""
'                    End If

'                    FileNetPassword = CMSDALFDBMD.RetrieveRoleSettings("FN Admin Password")
'                    If FileNetPassword IsNot Nothing AndAlso Not IsDBNull(FileNetPassword) Then
'                        FNPassword = Cryptor.Cryptor.DecryptString(FileNetPassword.ToString, CRYPTORKEY)
'                    Else
'                        FNPassword = ""
'                    End If
'                Else
'                    FileNetUser = CMSDALFDBMD.RetrieveRoleSettings("FN Emp User Name")
'                    If FileNetUser IsNot Nothing AndAlso Not IsDBNull(FileNetUser) Then
'                        FNUser = Cryptor.Cryptor.DecryptString(FileNetUser.ToString, CRYPTORKEY)
'                    Else
'                        FNUser = ""
'                    End If

'                    FileNetPassword = CMSDALFDBMD.RetrieveRoleSettings("FN Emp Password")
'                    If FileNetPassword IsNot Nothing AndAlso Not IsDBNull(FileNetPassword) Then
'                        FNPassword = Cryptor.Cryptor.DecryptString(FileNetPassword.ToString, CRYPTORKEY)
'                    Else
'                        FNPassword = ""
'                    End If
'                End If
'            Else
'                FileNetUser = CMSDALFDBMD.RetrieveRoleSettings("FN User Name")
'                If FileNetUser IsNot Nothing AndAlso Not IsDBNull(FileNetUser) Then
'                    FNUser = Cryptor.Cryptor.DecryptString(FileNetUser.ToString, CRYPTORKEY)
'                Else
'                    FNUser = ""
'                End If

'                FileNetPassword = CMSDALFDBMD.RetrieveRoleSettings("FN Password")
'                If FileNetPassword IsNot Nothing AndAlso Not IsDBNull(FileNetPassword) Then
'                    FNPassword = Cryptor.Cryptor.DecryptString(FileNetPassword.ToString, CRYPTORKEY)
'                Else
'                    FNPassword = ""
'                End If
'            End If

'            Return New Tuple(Of String, String)(FNUser, FNPassword)

'        Catch ex As Exception
'            Throw
'        End Try
'    End Function

'End Class

'Public Class FileNetImageDisplay
'    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

'    Shared Sub New()

'    End Sub
'    Sub New()

'    End Sub

'    Public Shared Sub DisplayDocument(docIDs As List(Of Long?))

'        Try

'            Using FNDisplay As New UFCW.WCF.Display
'                FNDisplay.Display(docIDs)
'            End Using

'        Catch ex As Exception
'            Throw
'        End Try
'    End Sub

'End Class