Imports System.Collections.Specialized
Imports System.DirectoryServices
Imports System.Security.Principal
Imports System.Windows.Forms
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Security

Public Class AD_Group
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _DomainNameValue As String
    Private _ServerNameValue As String
    Private _GroupNameValue As String
    Public Sub New(ByVal domainName As String, ByVal ServerName As String, ByVal GroupName As String)
        _DomainNameValue = DomainName
        _ServerNameValue = ServerName
        _GroupNameValue = GroupName
    End Sub

    Public Function ReturnUsers() As DataTable
        Dim StrDirEntryPath As String = "WinNT://" + _DomainNameValue + "/" + _ServerNameValue + "/" + _GroupNameValue + ",group"
        Dim Users As Object
        Dim Group As DirectoryEntry
        Dim ActiveDirTable As DataTable
        Dim User1 As Object
        Dim UserIDCol As DataColumn = New DataColumn("UserID")
        Dim UserEntry As DirectoryEntry


        Try

            Group = New DirectoryEntry(StrDirEntryPath)

            Users = Group.Invoke("members")
            ActiveDirTable = New DataTable("UserList")

            UserIDCol.DataType = System.Type.GetType("System.String")
            ActiveDirTable.Columns.Add(UserIDCol)

            For Each User1 In CType(Users, IEnumerable)

                UserEntry = New DirectoryEntry(User1)
                Dim myNewRow As DataRow
                myNewRow = ActiveDirTable.NewRow()
                myNewRow("UserID") = UserEntry.Name
                ActiveDirTable.Rows.Add(myNewRow)
            Next

            Return ActiveDirTable

        Catch ex As Exception
            Throw
        Finally
            If ActiveDirTable IsNot Nothing Then ActiveDirTable.Dispose()
            ActiveDirTable = Nothing
            If Group IsNot Nothing Then Group.Dispose()
            Group = Nothing
            If UserEntry IsNot Nothing Then UserEntry.Dispose()
            UserEntry = Nothing
            If UserIDCol IsNot Nothing Then UserIDCol.Dispose()
            UserIDCol = Nothing
        End Try

    End Function

End Class

Public Class CMSDALSecurity
    'Private Shared _TraceCloning As New TraceSwitch("TraceCloning", "Trace Switch in App.Config", "0")

    Private Shared _UserName As String
    Private Shared _WindowsUserID As WindowsIdentity = WindowsIdentity.GetCurrent()
    Private Shared _WindowsPrincipalForID As WindowsPrincipal = New WindowsPrincipal(_WindowsUserID)
    Private Shared _CMSCanAdjudicateEmployeeMembershipDT As DataTable = GetUsersInGroup("CMSCanAdjudicateEmployee")
    Private Shared _ComputerName As String = SystemInformation.ComputerName
    Private Shared _GetUniqueKey As New Object

    Public Shared Function InitializeFileNetUserInfo() As Tuple(Of String, String)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets User Name and Password based on AD Security Roles.
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	4/7/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim FileNetUser As Object
        Dim FileNetPassword As Object
        Dim FNUser As String
        Dim FNPassword As String
        Const CRYPTORKEY As String = "UFCW CMS ACCESS"

        Try
            If CMSDALSecurity.CMSCanAdjudicateEmployeeAccess Then
                If CMSDALSecurity.CMSAdministratorsAccess Then
                    FileNetUser = CMSDALFDBMD.RetrieveRoleSettings("FN Admin User Name")
                    If FileNetUser IsNot Nothing AndAlso Not IsDBNull(FileNetUser) Then
                        FNUser = Cryptor.Cryptor.DecryptString(FileNetUser.ToString, CRYPTORKEY)
                    Else
                        FNUser = ""
                    End If

                    FileNetPassword = CMSDALFDBMD.RetrieveRoleSettings("FN Admin Password")
                    If FileNetPassword IsNot Nothing AndAlso Not IsDBNull(FileNetPassword) Then
                        FNPassword = Cryptor.Cryptor.DecryptString(FileNetPassword.ToString, CRYPTORKEY)
                    Else
                        FNPassword = ""
                    End If
                Else
                    FileNetUser = CMSDALFDBMD.RetrieveRoleSettings("FN Emp User Name")
                    If FileNetUser IsNot Nothing AndAlso Not IsDBNull(FileNetUser) Then
                        FNUser = Cryptor.Cryptor.DecryptString(FileNetUser.ToString, CRYPTORKEY)
                    Else
                        FNUser = ""
                    End If

                    FileNetPassword = CMSDALFDBMD.RetrieveRoleSettings("FN Emp Password")
                    If FileNetPassword IsNot Nothing AndAlso Not IsDBNull(FileNetPassword) Then
                        FNPassword = Cryptor.Cryptor.DecryptString(FileNetPassword.ToString, CRYPTORKEY)
                    Else
                        FNPassword = ""
                    End If
                End If
            Else
                FileNetUser = CMSDALFDBMD.RetrieveRoleSettings("FN User Name")
                If FileNetUser IsNot Nothing AndAlso Not IsDBNull(FileNetUser) Then
                    FNUser = Cryptor.Cryptor.DecryptString(FileNetUser.ToString, CRYPTORKEY)
                Else
                    FNUser = ""
                End If

                FileNetPassword = CMSDALFDBMD.RetrieveRoleSettings("FN Password")
                If FileNetPassword IsNot Nothing AndAlso Not IsDBNull(FileNetPassword) Then
                    FNPassword = Cryptor.Cryptor.DecryptString(FileNetPassword.ToString, CRYPTORKEY)
                Else
                    FNPassword = ""
                End If
            End If

            Return New Tuple(Of String, String)(FNUser, FNPassword)

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function InitializeUserInfo(ByVal userName As String) As String

        Dim EncryptedPW As String
        Try
            EncryptedPW = CType(CMSDALDBO.RetrieveRoleSettings(userName & " Password"), String)
            If EncryptedPW IsNot Nothing AndAlso Not IsDBNull(EncryptedPW) Then
                Return Cryptor.Cryptor.DecryptString(EncryptedPW, userName.Trim & " ACCESS")
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared ReadOnly Property WindowsUserID As WindowsIdentity
        Get
            Return _WindowsUserID
        End Get
    End Property

    Public Shared ReadOnly Property ComputerName As String
        Get
            Return _ComputerName
        End Get
    End Property

    Public Shared Function GetUniqueKey() As String

        SyncLock (_GetUniqueKey)

            Dim MaxSize As Integer = 8
            Dim MinSize As Integer = 5
            Dim Chars(62) As Char
            Dim Size As Integer = MaxSize
            Dim DataArray(1) As Byte
            Dim Crypto As System.Security.Cryptography.RNGCryptoServiceProvider
            Dim A As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"

            Try

                Chars = A.ToCharArray()
                Crypto = New System.Security.Cryptography.RNGCryptoServiceProvider()

                Crypto.GetNonZeroBytes(DataArray)
                Size = MaxSize

                DataArray = New Byte(Size - 1) {}

                Crypto.GetNonZeroBytes(DataArray)

                Dim SBResult As New Text.StringBuilder(Size)
                For Each B As Byte In DataArray
                    SBResult.Append(B)
                Next

                Return SBResult.ToString

            Catch ex As Exception
                Throw
            Finally
                If Crypto IsNot Nothing Then Crypto.Dispose()
                Crypto = Nothing
            End Try

        End SyncLock

    End Function

    Public Shared ReadOnly Property Password(ByVal databasename As String) As String
        Get
            Dim EncryptedPassword As String = System.Configuration.ConfigurationManager.AppSettings(databasename & "PW")

            Return EncryptionHelper.SimpleCrypt(encryptedPassword)

        End Get
    End Property
    Public Shared Property UserName(ByVal Schema As String) As String
        Get
            Dim DB As Database
            db = CMSDALCommon.CreateDatabase(Schema)
            _UserName = db.ConnectionString.Substring(db.ConnectionString.ToLower.IndexOf("user id") + 8, db.ConnectionString.IndexOf(";"c, db.ConnectionString.ToLower.IndexOf("user id")) - db.ConnectionString.ToLower.IndexOf("user id") - 8)

            Return _UserName

        End Get
        Set(ByVal value As String)
            _UserName = Value
        End Set
    End Property

    Public Shared Function REGMRegMasterDeleteAccess() As Boolean

#If DEBUG And REGMRegMasterDeleteAccess = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And REGMRegMasterDeleteAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMRegMasterDeleteAccess")
#End If
        'Return True
    End Function

    Public Shared Function REGMReadOnlyAccess() As Boolean

#If DEBUG And REGMReadOnlyAccess = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And REGMReadOnlyAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMReadOnlyAccess")
#End If
        'Return True
    End Function

    Public Shared Function CMSCanCreateClaimAccess() As Boolean

#If Debug And CMSCanCreateClaim = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSCanCreateClaim = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanCreateClaim")
#End If
        'Return True
    End Function

    Public Shared Function CMSCanAddProviderAccess() As Boolean

#If DEBUG And CMSCanAddProvider = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And CMSCanAddProvider = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanAddProvider")
#End If
        'Return True
    End Function
    Public Shared Function CMSCanModifyProviderAccess() As Boolean

#If Debug And CMSCanModifyProvider = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSCanModifyProvider = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanModifyProvider")
#End If
        'Return True
    End Function
    Public Shared Function CMSAdministratorsAccess() As Boolean


#If Debug And CMSAdministrators = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSAdministrators = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSAdministrators")
#End If
        'Return True

    End Function
    Public Shared Function CMSCanDeleteProviderAccess() As Boolean

#If Debug And CMSCanDeleteProvider = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSCanDeleteProvider = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanDeleteProvider")
#End If
        'Return True
    End Function
    Public Shared Function CMSUsersAccess() As Boolean

#If Debug And CMSUsers = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSUsers = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSUsers")
#End If
        'Return True
    End Function
    Public Shared Function CMSLocalsAccess() As Boolean

#If Debug And CMSLocals = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSLocals = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSLocals")
#End If
        'Return True

    End Function
    Public Shared Function CMSDentalAccess() As Boolean

#If Debug And CMSDental = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSDental = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSDental")
#End If
        'Return True

    End Function
    Public Shared Function CMSCanAdjudicateEmployeeAccess() As Boolean

#If Debug And CMSCanAdjudicateEmployee = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSCanAdjudicateEmployee = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanAdjudicateEmployee")
#End If
        'Return True

    End Function
    Public Shared Function CMSLocalsEmployeeAccess() As Boolean

#If Debug And CMSCanAccessLocalsEmployee = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSCanAccessLocalsEmployee = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanAccessLocalsEmployee")
#End If
        'Return True

    End Function
    Public Shared Function CMSCanPickWorkAccess() As Boolean

#If Debug And CMSCanPickWork = True Then
        Return True
#ElseIf Debug And CMSCanPickWork = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanPickWork")
#End If

    End Function
    Public Shared Function CMSCanRemovePricingAccess() As Boolean

#If Debug And CMSCanRemovePricing Then
        Return True
#ElseIf Debug And CMSCanRemovePricing = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanRemovePricing")
#End If

    End Function
    Public Shared Function CMSCanOverrideAccumulatorsAccess() As Boolean

#If Debug And CMSCanOverrideAccumulators Then
        Return True
#ElseIf Debug And CMSCanOverrideAccumulators = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanOverrideAccumulators")
#End If

    End Function
    Public Shared Function CMSEligibilityEmployeeAccess() As Boolean

#If Debug And CMSEligibilityEmployee Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSEligibilityEmployee = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSEligibilityEmployee")
#End If
        'Return True

    End Function
    Public Shared Function CMSEligibilityAccess() As Boolean

#If Debug And CMSEligibility Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSEligibility = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSEligibility")
#End If
        'Return True

    End Function
    Public Shared Function PENSIONUserAccess() As Boolean

#If DEBUG And PENSIONUSer Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And PENSIONUSer = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\PENSIONUser")
#End If
        'Return True

    End Function
    Public Shared Function CMSHRAAccess() As Boolean

#If DEBUG And CMSHRA Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And CMSHRA = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSHRA")
#End If
        'Return True

    End Function
    Public Shared Function CMSCanAuditAccess() As Boolean

#If Debug And CMSCanAudit Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSCanAudit = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanAudit")
#End If
        'Return True

    End Function
    Public Shared Function CMSCanRePrintEOB() As Boolean

#If DEBUG And CMSCanRePrintEOB = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And CMSCanRePrintEOB = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanRePrintEOB")
#End If

    End Function
    Public Shared Function CMSHRAEmployeeAccess() As Boolean

#If Debug And CMSHRAEmployee Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSHRAEmployee = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSHRAEmployee")
#End If
    End Function
    Public Shared Function CMSCanReprocessAccess() As Boolean

#If Debug And CMSCanReprocess Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSCanReprocess = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanReprocess")
#End If
        'Return True

    End Function
    Public Shared Function CMSUTLAccess() As Boolean

#If Debug And CMSUTL Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSUTL = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSUTL")
#End If
        'Return True

    End Function
    Public Shared Function CMSCanReopenFullAccess() As Boolean

#If Debug And CMSCanReopenFull Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSCanReopenFull = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanReopenFull")
#End If
        'Return True

    End Function
    Public Shared Function CMSCanReopenPartialAccess() As Boolean

#If Debug And CMSCanReopenPartial Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSCanReopenPartial = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanReopenPartial")
#End If
        'Return True

    End Function
    Public Shared Function CMSCanModifyCOB() As Boolean

#If Debug And CMSCanModifyCOB Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSCanModifyCOB = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanModifyCOB")
#End If
        'Return True

    End Function
    Public Shared Function CMSCanViewHours() As Boolean

#If Debug And CMSCanViewHours Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSCanViewHours = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanViewHours")
#End If
        'Return True

    End Function
    Public Shared Function CMSCanViewEligibilityHours() As Boolean

#If DEBUG And CMSCanViewEligibilityHours Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And CMSCanViewEligibilityHours = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanViewEligibilityHours")
#End If
        'Return True

    End Function
    Public Shared Function CMSCanRunReports() As Boolean

#If Debug And CMSCanRunReports Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And CMSCanRunReports = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanRunReports")
#End If
        'Return True

    End Function
    Public Shared Function GetUsersInGroup(ByVal group As String) As DataTable
        ' Get all users from an Active Directory distribution group

        Dim ADGroup As AD_Group
        Try

            ADGroup = New AD_Group("UFCW", "UFCWDC1", group)

            Return ADGroup.ReturnUsers()

        Catch ex As Exception

            ADGroup = New AD_Group("UFCW", "UFCWDC2", group)

            Return ADGroup.ReturnUsers()

        End Try

    End Function

    Private Shared Function GetUserGroupMembership(ByVal strUser As String) As StringCollection
        Dim Groups As New StringCollection()
        Dim ObjectGroups As Object
        Dim ObjectEntry As DirectoryEntry
        Dim DirSearcher As DirectorySearcher
        Dim SearchResult As SearchResult
        Dim DirUsers As DirectoryEntry
        Dim DirGrpEntry As DirectoryEntry

        Try
            ObjectEntry = New DirectoryEntry("LDAP://CN=users,DC=pardesifashions,DC=com")
            DirSearcher = New DirectorySearcher(ObjectEntry, "(sAMAccountName=" & strUser & ")")
            SearchResult = DirSearcher.FindOne()

            If SearchResult IsNot Nothing Then
                DirUsers = New DirectoryEntry(SearchResult.Path)
                ' Invoke Groups method.

                ObjectGroups = DirUsers.Invoke("Groups")
                For Each ob As Object In DirectCast(ObjectGroups, IEnumerable)
                    ' Create object for each group.

                    DirGrpEntry = New DirectoryEntry(ob)
                    Groups.Add(DirGrpEntry.Name)
                Next
            End If

            Return Groups

        Catch ex As Exception
            Trace.Write(ex.Message)
        Finally
            If ObjectEntry IsNot Nothing Then ObjectEntry.Dispose()
            ObjectEntry = Nothing
            If DirSearcher IsNot Nothing Then DirSearcher.Dispose()
            DirSearcher = Nothing
            If DirUsers IsNot Nothing Then DirUsers.Dispose()
            DirUsers = Nothing
            If DirGrpEntry IsNot Nothing Then DirGrpEntry.Dispose()
            DirGrpEntry = Nothing
        End Try
    End Function
    Public Shared Function REGMEmployeeAccess() As Boolean

#If Debug And REGMEmployeeAccess Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And REGMEmployeeAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMEmployeeAccess")
#End If
        'Return True
    End Function
    Public Shared Function REGMVendorAccess() As Boolean

#If DEBUG And REGMVendorAccess = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And REGMVendorAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMVendorAccess")
#End If
        'Return True
    End Function

    Public Shared Function REGMUserAccess() As Boolean

#If DEBUG And REGMUser Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And REGMUser = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMUser")
#End If
        'Return True
    End Function
    Public Shared Function CMSCanModifyAlerts() As Boolean

#If DEBUG And CMSCanModifyAlerts Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And CMSCanModifyAlerts = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanModifyAlerts")
#End If
        'Return True
    End Function
    Public Shared Function REGMEligMaintenanceAccess() As Boolean

#If Debug And REGMEligMaintenanceAccess = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf Debug And REGMEligMaintenanceAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMEligMaintenanceAccess")
#End If
        'Return True
    End Function
    Public Shared Function REGMSupervisorAccess() As Boolean

#If Debug And REGMLifeEventDeleteAccess = True Then '' This is supervisor level security in REGM
        Return True
#ElseIf Debug And REGMLifeEventDeleteAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMLifeEventDeleteAccess")
#End If
        'Return True
    End Function

End Class

Public Class EncryptionHelper

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Shared _Entropy As Byte() = System.Text.Encoding.Unicode.GetBytes("Salt Is Not A Password")

    Public Shared Function EncryptString(ByVal input As System.Security.SecureString) As String
        Dim EncryptedData As Byte() = System.Security.Cryptography.ProtectedData.Protect(System.Text.Encoding.Unicode.GetBytes(ToInsecureString(input)), _Entropy, System.Security.Cryptography.DataProtectionScope.CurrentUser)
        Return Convert.ToBase64String(EncryptedData)
    End Function

    Public Shared Function DecryptString(ByVal encryptedData As String) As SecureString
        Try
            Dim DecryptedData As Byte() = System.Security.Cryptography.ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), _Entropy, System.Security.Cryptography.DataProtectionScope.CurrentUser)
            Return ToSecureString(System.Text.Encoding.Unicode.GetString(DecryptedData))
        Catch
            Return New SecureString()
        End Try
    End Function

    Public Shared Function Encrypt256BitString(ByVal input As System.Security.SecureString, Optional ByRef tableName As String = Nothing) As String
        Return Cryptor.Cryptor.EncryptString(input.ToString, If(tableName, "") & " CMSDAL")
    End Function

    Public Shared Function Decrypt256BitString(ByVal encryptedData As String, Optional ByRef tableName As String = Nothing) As SecureString
        Try
            Return ToSecureString(Cryptor.Cryptor.DecryptString(encryptedData, If(tableName, "") & " CMSDAL"))
        Catch
            Return New SecureString()
        End Try
    End Function

    Public Shared Function ToSecureString(ByVal input As String) As SecureString
        Dim Secure As SecureString
        Try
            Secure = New SecureString()

            For Each C As Char In input
                Secure.AppendChar(C)
            Next
            Secure.MakeReadOnly()

            Return Secure

        Catch ex As Exception
            Throw
        Finally
            If Secure IsNot Nothing Then Secure.Dispose()
            Secure = Nothing
        End Try
    End Function

    Public Shared Function ToInsecureString(ByVal input As SecureString) As String
        Dim ReturnValue As String = String.Empty
        Dim Ptr As IntPtr
        Try
            Ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input)
            ReturnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(Ptr)
        Finally
            System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(Ptr)
        End Try
        Return ReturnValue
    End Function


    Public Shared Function SimpleCrypt(ByVal text As String) As String
        ' Encrypts/decrypts the passed string using 
        ' a simple ASCII value-swapping algorithm
        Dim StrTempChar As String, i As Integer
        For i = 1 To Len(text)
            If Asc(Mid$(text, i, 1)) < 128 Then
                StrTempChar = _
          CType(Asc(Mid$(text, i, 1)) + 128, String)
            ElseIf Asc(Mid$(text, i, 1)) > 128 Then
                StrTempChar = _
          CType(Asc(Mid$(text, i, 1)) - 128, String)
            End If
            Mid$(text, i, 1) = _
                Chr(CType(StrTempChar, Integer))
        Next i
        Return Text
    End Function

End Class
