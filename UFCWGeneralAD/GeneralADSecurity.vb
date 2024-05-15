Option Infer On
Option Strict On

Imports System.Collections.Specialized
Imports System.DirectoryServices
Imports System.DirectoryServices.AccountManagement
Imports System.Security.Principal
'<System.Diagnostics.DebuggerStepThrough()>
Partial Public Class UFCWGeneralAD
    'Note: When compiling for Debug these will appear to be unused due to compiler directives.

    Private Shared ReadOnly _WindowsUserID As WindowsIdentity = WindowsIdentity.GetCurrent()
    Private Shared ReadOnly _WindowsPrincipalForID As New WindowsPrincipal(_WindowsUserID)

#Region "Shared Methods Security"

    Public Shared Function GetUsersInGroup(ByVal group As String) As DataTable
        ' Get all users from an Active Directory distribution group

        Dim ADGroup As AD_Group
        Try

            ADGroup = New AD_Group("UFCW", "UFCWDC1", group)

            Return ADGroup.ReturnUsers()

        Catch ex As Exception

            Throw

        End Try

    End Function
    Public Shared Sub AddNewUsersToGroup(ByVal group As String, ByVal userName As String)
        Dim ADGroup As AD_Group
        Try

            ADGroup = New AD_Group("UFCW", "UFCWDC1", group)

            ADGroup.AddUsers(userName)

        Catch ex As Exception

            Throw

        End Try
    End Sub
    Public Shared Sub DeleteUsersFromGroup(ByVal group As String, ByVal userName As String)
        Dim ADGroup As AD_Group
        Try

            ADGroup = New AD_Group("UFCW", "UFCWDC1", group)

            ADGroup.RemoveUsers(userName)

        Catch ex As Exception

            Throw

        End Try
    End Sub


    Public Shared Function GetAllGroups() As DataTable
        Dim ADGroup As AD_Group
        Try

            ADGroup = New AD_Group("UFCW", "UFCWDC1")
            Return ADGroup.FindAllGroups()

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Shared Function GetAllUserGroups(ByVal userName As String) As DataTable
        Dim ADGroup As AD_Group
        Try

            ADGroup = New AD_Group("UFCW", "UFCWDC1")

            Return ADGroup.FindAllUserGroups(userName)

        Catch ex As Exception

            Throw
        End Try
    End Function
    Public Shared Function GetDomainUserGroupPermissions() As DataTable
        Dim ADGroup As AD_Group
        Try

            ADGroup = New AD_Group("UFCW", "UFCWDC1")

            Return ADGroup.FindDomainUserGroupPermissions(_WindowsUserID.User)
        Catch ex As Exception

            Throw
        End Try
    End Function

    Public Shared Function GetUserGroupMembership() As StringCollection

        Dim a As New StringCollection
        Dim currentIdentity As WindowsIdentity = WindowsIdentity.GetCurrent()

#If DEBUG AndAlso CMSCanRunReports = True Then 'This conditional check is Configuration Properties \ Build
        a.Add("CMSCanRunReports")
#End If
#If DEBUG AndAlso CMSPay = True Then
        a.Add("CMSPay")
#End If
#If DEBUG AndAlso CMSUsers = True Then
        a.Add("CMSUsers")
#End If
#If DEBUG AndAlso CMSDental = True Then
        a.Add("CMSDental")
#End If
#If DEBUG AndAlso CMSLocals = True Then
        a.Add("CMSLocals")
#End If
#If DEBUG AndAlso CMSUTL = True Then
        a.Add("CMSUTL")
#End If
#If DEBUG AndAlso CMSCanAudit = True Then
        a.Add("CMSCanAudit")
#End If
#If DEBUG AndAlso CMSCanAdjudicateEmployee = True Then
        a.Add("CMSCanAdjudicateEmployee")
#End If
#If DEBUG AndAlso CMSCanAccessLocalsEmployee = True Then
        a.Add("CMSCanAccessLocalsEmployee")
#End If
#If DEBUG AndAlso CMSCanReprocess = True Then
        a.Add("CMSCanReprocess")
#End If
#If DEBUG AndAlso CMSCanRemovePricing = True Then
        a.Add("CMSCanRemovePricing")
#End If
#If DEBUG AndAlso CMSCanOverrideAccumulators = True Then
        a.Add("CMSCanOverrideAccumulators")
#End If
#If DEBUG AndAlso CMSCanPickWork = True Then
        a.Add("CMSCanPickWork")
#End If
#If DEBUG AndAlso CMSEligibility = True Then
        a.Add("CMSEligibility")
#End If
#If DEBUG AndAlso CMSEligibilityEmployee = True Then
        a.Add("CMSEligibilityEmployee")
#End If
#If DEBUG AndAlso CMSHRA = True Then
        a.Add("CMSHRA")
#End If
#If DEBUG AndAlso CMSCanReopenPartial = True Then
        a.Add("CMSCanReopenPartial")
#End If
#If DEBUG AndAlso CMSCanReopenFull = True Then
        a.Add("CMSCanReopenFull")
#End If
#If DEBUG AndAlso CMSCanModifyCOB = True Then
        a.Add("CMSCanModifyCOB")
#End If
#If DEBUG AndAlso CMSCanRunReports = True Then
        a.Add("CMSCanRunReports")
#End If
#If DEBUG AndAlso CMSCanViewEligibilityHours = True Then
        a.Add("CMSCanViewEligibilityHours")
#End If
#If DEBUG AndAlso CMSCanViewRxDetail = True Then
        a.Add("CMSCanViewRxDetail")
#End If
#If DEBUG AndAlso CMSCanAddProvider = True Then
        a.Add("CMSCanAddProvider")
#End If
#If DEBUG AndAlso CMSCanRePrintEOB = True Then
        a.Add("CMSCanRePrintEOB")
#End If

#If DEBUG Then
#Else
        For Each group As IdentityReference In currentIdentity.Groups
            Try

                Dim b As String() = group.Translate(GetType(NTAccount)).ToString.Split(CChar("\"))
                a.Add(b(b.Length - 1))
            Catch ex As Exception
                'ignore failed translations the only negative is certain functions will not be avaiable
            End Try
        Next
#End If

        Return a
    End Function
    Public Shared Function WorkflowAdministrators() As Boolean

#If DEBUG And WorkflowAdministrators = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And WorkflowAdministrators = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\WorkflowAdministrators")
#End If
    End Function
    Public Shared Function WorkflowEmployeeAccess() As Boolean

#If DEBUG And WorkflowEmployeeAccess = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And WorkflowEmployeeAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\WorkflowEmployeeAccess")
#End If
    End Function
    Public Shared Function WorkFlowUsers() As Boolean

#If DEBUG And WorkFlowUsers = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And WorkFlowUsers = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\WorkFlowUsers")
#End If
    End Function

    Public Shared Function WorkFlowCanExport() As Boolean

#If DEBUG And WorkFlowCanExport = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And WorkFlowCanExport = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\WorkFlowCanExport")
#End If
    End Function
    Public Shared Function WorkFlowCanPrint() As Boolean

#If DEBUG And WorkFlowCanPrint = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And WorkFlowCanPrint = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\WorkFlowCanPrint")
#End If
    End Function
    Public Shared Function WorkFlowCanRunReports() As Boolean

#If DEBUG And WorkFlowCanRunReports = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And WorkFlowCanRunReports = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\WorkFlowCanRunReports")
#End If
    End Function
    Public Shared Function REGMCheckAddressAccess() As Boolean                                             ''Determine who can modify Check Address

#If DEBUG And REGMCheckAddressAccess = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And REGMCheckAddressAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\PensionUser") Or _WindowsPrincipalForID.IsInRole("UFCW\REGMReports")
#End If
        'Return True
    End Function
    Public Shared Function REGMTermAccess() As Boolean
        ''For Terms Maintenance Access

#If DEBUG And REGMTermAccess = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And REGMTermAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMTermAccess")
#End If
        'Return True
    End Function
    Public Shared Function REGMLifeEventDeleteAccess() As Boolean
        ''Life Event Delete Access

#If DEBUG And REGMLifeEventDeleteAccess = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And REGMLifeEventDeleteAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMLifeEventDeleteAccess")
#End If
        'Return True
    End Function
    Public Shared Function REGMChangeSSNAccess() As Boolean
        ''Change SSN Access

#If DEBUG And REGMChangeSSNAccess = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And REGMChangeSSNAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMChangeSSNAccess")
#End If
        'Return True
    End Function
    Public Shared Function REGMCobraAccess() As Boolean
        ''Change SSN Access

#If DEBUG And REGMCobraAccess = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And REGMCobraAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMCobraAccess")
#End If
        'Return True
    End Function
    Public Shared Function REGMReports() As Boolean
        ''REGM Supervisor Access, Allows Delete Access in Coverage

#If DEBUG And REGMReports = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And REGMReports = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMReports")
#End If
        'Return True
    End Function
    Public Shared Function REGMEmployeeAccess() As Boolean
        ''REGM Employee Access

#If DEBUG And REGMEmployeeAccess Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And REGMEmployeeAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMEmployeeAccess")
#End If
        'Return True
    End Function
    Public Shared Function REGMVendorAccess() As Boolean
        ''REGM Readonly Access, restricts context menus in Dental, Eligibility, Supresses History content

#If DEBUG And REGMVendorAccess = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And REGMVendorAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMVendorAccess")
#End If
        'Return True
    End Function
    Public Shared Function REGMUser() As Boolean
        ''REGM Minimum Access?

#If DEBUG And REGMUser Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And REGMUser = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMUser")
#End If
        'Return True
    End Function
    Public Shared Function REGMEligMaintenanceAccess() As Boolean
        ''REGM Enables Expanded Eligibility Context menu(s)
#If DEBUG And REGMEligMaintenanceAccess = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And REGMEligMaintenanceAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMEligMaintenanceAccess")
#End If
        'Return True
    End Function
    Public Shared Function REGMAdministrators() As Boolean 'Does not exist?


#If DEBUG And REGMAdministrators = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And REGMAdministrators = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMAdministrators")
#End If
        'Return True

    End Function

    Public Shared Function REGMSupervisor() As Boolean 'Does not exist?
        ''Allows Elig recalculation to be used
#If DEBUG And REGMLifeEventDeleteAccess = True Then '' This is supervisor level security in REGM
        Return True
#ElseIf DEBUG And REGMLifeEventDeleteAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMLifeEventDeleteAccess")
#End If
        'Return True
    End Function
    Public Shared Function REGMRegMasterDeleteAccess() As Boolean
        ''Enables Delete in COB, Coverage, Alerts, Elig Retirement
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
        ''ReadOnly access
#If DEBUG And REGMReadOnlyAccess = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And REGMReadOnlyAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\REGMReadOnlyAccess")
#End If
        'Return True
    End Function
    Public Shared Function CMSCanCreateClaim() As Boolean

#If DEBUG And CMSCanCreateClaim = True Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSCanCreateClaim = False Then
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanCreateClaim")
#End If
        Return True
    End Function
    Public Shared Function CMSCanAddProvider() As Boolean

#If DEBUG And CMSCanAddProvider = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And CMSCanAddProvider = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanAddProvider")
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
        Return True
    End Function

    Public Shared Function CMSCanViewRxDetail() As Boolean

#If DEBUG And CMSCanViewRxDetail = True Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSCanViewRxDetail = False Then
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanViewRxDetail")
#End If
        Return True
    End Function

    Public Shared Function CMSCanModifyProvider() As Boolean

#If DEBUG And CMSCanModifyProvider = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And CMSCanModifyProvider = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanModifyProvider")
#End If
        'Return True
    End Function
    Public Shared Function CMSAdministrators() As Boolean


#If DEBUG And CMSAdministrators = True Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSAdministrators = False Then 'This conditional check is Configuration Properties \ Build
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSAdministrators")
#End If
        Return True

    End Function
    Public Shared Function CMSCanDeleteProvider() As Boolean

#If DEBUG And CMSCanDeleteProvider = True Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And CMSCanDeleteProvider = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanDeleteProvider")
#End If
        'Return True
    End Function
    Public Shared Function CMSUsers() As Boolean

#If DEBUG And CMSUsers = True Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSUsers = False Then
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSUsers")
#End If
        Return True
    End Function

    Public Shared Function CMSPay() As Boolean

#If DEBUG And CMSPay = True Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSPay = False Then
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSPay")
#End If
        Return True
    End Function

    Public Shared Function CMSLocals() As Boolean

#If DEBUG And CMSLocals = True Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSLocals = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
                Return _WindowsPrincipalForID.IsInRole("UFCW\CMSLocals")
#End If
        Return True

    End Function
    Public Shared Function CMSDental() As Boolean

#If DEBUG And CMSDental = True Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSDental = False Then 'This conditional check is Configuration Properties \ Build
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSDental")
#End If
        Return True

    End Function
    Public Shared Function CMSCanAdjudicateEmployee() As Boolean

#If DEBUG And CMSCanAdjudicateEmployee = True Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSCanAdjudicateEmployee = False Then 'This conditional check is Configuration Properties \ Build
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanAdjudicateEmployee")
#End If
        Return True

    End Function
    Public Shared Function CMSLocalsEmployee() As Boolean

#If DEBUG And CMSCanAccessLocalsEmployee = True Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSCanAccessLocalsEmployee = False Then 'This conditional check is Configuration Properties \ Build
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanAccessLocalsEmployee")
#End If
        Return True

    End Function
    Public Shared Function CMSCanPickWork() As Boolean

#If DEBUG And CMSCanPickWork = True Then
                Return True
#ElseIf DEBUG And CMSCanPickWork = False Then
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanPickWork")
#End If
        Return True
    End Function
    Public Shared Function CMSCanRemovePricing() As Boolean

#If DEBUG And CMSCanRemovePricing Then
                Return True
#ElseIf DEBUG And CMSCanRemovePricing = False Then
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanRemovePricing")
#End If
        Return True
    End Function
    Public Shared Function CMSCanOverrideAccumulators() As Boolean

#If DEBUG And CMSCanOverrideAccumulators Then
                Return True
#ElseIf DEBUG And CMSCanOverrideAccumulators = False Then
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanOverrideAccumulators")
#End If
        Return True
    End Function
    Public Shared Function CMSEligibilityEmployee() As Boolean

#If DEBUG And CMSEligibilityEmployee Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSEligibilityEmployee = False Then 'This conditional check is Configuration Properties \ Build
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSEligibilityEmployee")
#End If
        Return True

    End Function
    Public Shared Function CMSEligibility() As Boolean

#If DEBUG And CMSEligibility Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSEligibility = False Then 'This conditional check is Configuration Properties \ Build
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSEligibility")
#End If
        Return True

    End Function
    Public Shared Function PENSIONUser() As Boolean

#If DEBUG And PENSIONUSer Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And PENSIONUSer = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\PENSIONUser")
#End If
        'Return True

    End Function
    Public Shared Function CMSHRA() As Boolean

#If DEBUG And CMSHRA Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSHRA = False Then 'This conditional check is Configuration Properties \ Build
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSHRA")
#End If
        Return True

    End Function
    Public Shared Function CMSCanAudit() As Boolean

#If DEBUG And CMSCanAudit Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSCanAudit = False Then 'This conditional check is Configuration Properties \ Build
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanAudit")
#End If
        Return True

    End Function
    Public Shared Function CMSHRAEmployee() As Boolean

#If DEBUG And CMSHRAEmployee Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSHRAEmployee = False Then 'This conditional check is Configuration Properties \ Build
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSHRAEmployee")
#End If
        Return True
    End Function
    Public Shared Function CMSCanReprocess() As Boolean

#If DEBUG And CMSCanReprocess Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSCanReprocess = False Then 'This conditional check is Configuration Properties \ Build
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanReprocess")
#End If
        Return True

    End Function
    Public Shared Function CMSUTL() As Boolean

#If DEBUG And CMSUTL Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And CMSUTL = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSUTL")
#End If
        'Return True

    End Function
    Public Shared Function CMSCanReopenFull() As Boolean

#If DEBUG And CMSCanReopenFull Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And CMSCanReopenFull = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanReopenFull")
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
    Public Shared Function CMSCanReopenPartial() As Boolean

#If DEBUG And CMSCanReopenPartial Then 'This conditional check is Configuration Properties \ Build
        Return True
#ElseIf DEBUG And CMSCanReopenPartial = False Then 'This conditional check is Configuration Properties \ Build
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanReopenPartial")
#End If
        'Return True

    End Function
    Public Shared Function CMSCanModifyCOB() As Boolean

#If DEBUG And CMSCanModifyCOB Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSCanModifyCOB = False Then 'This conditional check is Configuration Properties \ Build
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanModifyCOB")
#End If
        Return True

    End Function
    Public Shared Function CMSCanViewHours() As Boolean

#If DEBUG And CMSCanViewHours Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSCanViewHours = False Then 'This conditional check is Configuration Properties \ Build
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanViewHours")
#End If
        Return True

    End Function
    Public Shared Function CMSCanViewEligibilityHours() As Boolean

#If DEBUG And CMSCanViewEligibilityHours Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSCanViewEligibilityHours = False Then 'This conditional check is Configuration Properties \ Build
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanViewEligibilityHours")
#End If
        Return True

    End Function
    Public Shared Function CMSCanRunReports() As Boolean

#If DEBUG And CMSCanRunReports Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And CMSCanRunReports = False Then 'This conditional check is Configuration Properties \ Build
                Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\CMSCanRunReports")
#End If
        Return True

    End Function

    Public Shared Function UFCWMaintAdminAccess() As Boolean

#If DEBUG And UFCWMaintAdminAccess = True Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And UFCWMaintAdminAccess = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\UFCWMaintAdminAccess")
#End If
    End Function
#End Region
    Public Shared Function UFCWMaintWorkFlow() As Boolean

#If DEBUG And UFCWMaintWorkFlow = True Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And UFCWMaintWorkFlow = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\UFCWMaintWorkFlow")
#End If
    End Function

    Public Shared Function UFCWMaintKofax() As Boolean
#If DEBUG And UFCWMaintKofax = True Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And UFCWMaintKofax = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\UFCWMaintKofax")
#End If
    End Function
    Public Shared Function UFCWMaintPasswords() As Boolean
#If DEBUG And UFCWMaintPasswords = True Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And UFCWMaintPasswords = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\UFCWMaintPasswords")
#End If
    End Function
    Public Shared Function UFCWADGroups() As Boolean
#If DEBUG And UFCWADGroups = True Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And UFCWADGroups = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\UFCWADGroups")
#End If
    End Function
    Public Shared Function UFCWMaintWebRegistration() As Boolean
#If DEBUG And UFCWMaintWebRegistration = True Then 'This conditional check is Configuration Properties \ Build
                Return True
#ElseIf DEBUG And UFCWMaintWebRegistration = False Then
        Return False
#Else
        Return _WindowsPrincipalForID.IsInRole("UFCW\UFCWMaintWebRegistration")
#End If
    End Function
End Class
Public Class AD_Group
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _DomainNameValue As String
    Private _ServerNameValue As String
    Private _GroupNameValue As String
    Public Sub New(ByVal domainName As String, ByVal ServerName As String, ByVal GroupName As String)
        _DomainNameValue = domainName
        _ServerNameValue = ServerName
        _GroupNameValue = GroupName
    End Sub
    Public Sub New(ByVal domainName As String, ByVal ServerName As String)
        _DomainNameValue = domainName
        _ServerNameValue = ServerName
    End Sub
    Public Function ReturnUsers() As DataTable
        Dim StrDirEntryPath As String = "WinNT://" + _DomainNameValue + "/" + _ServerNameValue + "/" + _GroupNameValue + ",group"
        Dim Users As Object
        Dim Group As DirectoryEntry
        Dim ActiveDirTable As DataTable
        Dim User1 As Object
        Dim UserIDCol As New DataColumn("UserID")
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

    Public Function FindAllGroups() As DataTable
        Dim StrDirEntryPath As String = "WinNT://" + _DomainNameValue + "/" + _ServerNameValue + ",computer"
        Dim ActiveDirTable As DataTable
        Dim GroupIDCol As New DataColumn("GroupID")
        Dim group As GroupPrincipal
        Dim myNewRow As DataRow

        Try
            ActiveDirTable = New DataTable("GroupList")
            GroupIDCol.DataType = System.Type.GetType("System.String")
            ActiveDirTable.Columns.Add(GroupIDCol)

            Dim ctx As New PrincipalContext(ContextType.Domain, _DomainNameValue, "OU=UFCWApplicationSecurity, DC=ufcw,DC=ad")
            group = New GroupPrincipal(ctx)
            Dim search As New PrincipalSearcher(group)
            For Each g In search.FindAll()
                myNewRow = ActiveDirTable.NewRow()
                myNewRow("GroupID") = g.Name
                ActiveDirTable.Rows.Add(myNewRow)
            Next
            ctx.Dispose()
            ActiveDirTable.DefaultView.Sort = "GroupID asc"
            ActiveDirTable = ActiveDirTable.DefaultView.ToTable()
            Return ActiveDirTable

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Function FindDomainUserGroupPermissions(ByVal loginUserSid As SecurityIdentifier) As DataTable
        Dim StrDirEntryPath As String = "WinNT://" + _DomainNameValue + "/" + _ServerNameValue + ",computer"  '"LDAP://UFCWDC1/OU=UFCWApplicationSecurity,DC=ufcw,DC=ad"
        Dim ActiveDirTable As DataTable
        Dim GroupIDCol As New DataColumn("GroupID")
        Dim EnabledCol As New DataColumn("Enabled")
        Dim myNewRow As DataRow
        Dim group As GroupPrincipal
        Try
            ActiveDirTable = New DataTable("GroupList")
            GroupIDCol.DataType = System.Type.GetType("System.String")
            ActiveDirTable.Columns.Add(GroupIDCol)

            EnabledCol.DataType = System.Type.GetType("System.Boolean")
            ActiveDirTable.Columns.Add(EnabledCol)

            Dim ctx As New PrincipalContext(ContextType.Domain, _DomainNameValue, "OU=UFCWApplicationSecurity, DC=ufcw,DC=ad")
            group = New GroupPrincipal(ctx)
            Dim search As New PrincipalSearcher(group)
            ' For Each g In search.FindAll().Where(Function(s) s.SamAccountName.StartsWith("Work", StringComparison.OrdinalIgnoreCase))
            For Each g In search.FindAll()
                If CheckWritePermission("LDAP://" & g.DistinguishedName) Then
                    myNewRow = ActiveDirTable.NewRow()
                    myNewRow("GroupID") = g.Name
                    myNewRow("Enabled") = True
                    ActiveDirTable.Rows.Add(myNewRow)
                End If
            Next
            ctx.Dispose()
            ctx = Nothing
            ActiveDirTable.DefaultView.Sort = "GroupID asc"
            ActiveDirTable = ActiveDirTable.DefaultView.ToTable()
            Return ActiveDirTable

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function FindAllUserGroups(ByVal userName As String) As DataTable
        Dim StrDirEntryPath As String = "WinNT://" + _DomainNameValue + "/" + _ServerNameValue + ",computer"  '"LDAP://UFCWDC1/CN=Users,DC=ufcw,DC=ad"
        Dim ActiveDirTable As DataTable
        Dim GroupIDCol As New DataColumn("GroupID")
        Dim myNewRow As DataRow
        Dim user As UserPrincipal

        Try
            ActiveDirTable = New DataTable("GroupList")
            GroupIDCol.DataType = System.Type.GetType("System.String")
            ActiveDirTable.Columns.Add(GroupIDCol)


            Dim ctx As New PrincipalContext(ContextType.Domain, _DomainNameValue)
            user = UserPrincipal.FindByIdentity(ctx, identityType:=IdentityType.SamAccountName, userName)
            If user IsNot Nothing Then
                Dim groupMemberships As ArrayList = New ArrayList()
                groupMemberships = Groups(DirectCast(user.GetUnderlyingObject, System.DirectoryServices.DirectoryEntry).Path, True)

                For Each s As String In groupMemberships
                    Dim valueString() As String = s.Split(CChar(","))
                    myNewRow = ActiveDirTable.NewRow()
                    myNewRow("GroupID") = valueString(0).Replace("CN=", "")
                    ActiveDirTable.Rows.Add(myNewRow)
                Next
                ctx.Dispose()
                ctx = Nothing
            End If

            ActiveDirTable.DefaultView.Sort = "GroupID asc"
            ActiveDirTable = ActiveDirTable.DefaultView.ToTable()
            Return ActiveDirTable

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function Groups(ByVal userDn As String, ByVal recursive As Boolean) As ArrayList
        Dim groupMemberships As ArrayList = New ArrayList()
        Return AttributeValuesMultiString("memberOf", userDn, groupMemberships, recursive)
    End Function
    Public Function AttributeValuesMultiString(ByVal attributeName As String, ByVal objectDn As String, ByVal valuesCollection As ArrayList, ByVal recursive As Boolean) As ArrayList
        Try
            Dim ent As DirectoryEntry = New DirectoryEntry(objectDn)
            Dim ValueCollection As PropertyValueCollection = ent.Properties(attributeName)
            Dim en As IEnumerator = ValueCollection.GetEnumerator()

            While en.MoveNext()

                If en.Current IsNot Nothing Then

                    If Not valuesCollection.Contains(en.Current.ToString()) Then

                        valuesCollection.Add(en.Current.ToString())

                        If recursive Then
                            AttributeValuesMultiString(attributeName, "LDAP://" & Replace(en.Current.ToString(), "/", "\/"), valuesCollection, True)
                        End If
                    End If
                End If
            End While

            ent.Close()
            ent.Dispose()

            Return valuesCollection
        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Shared Function CheckWritePermission(ByVal path As String) As Boolean
        Try
            Using de As DirectoryEntry = New DirectoryEntry(path)
                de.RefreshCache(New String() {"allowedAttributesEffective"})
                Return de.Properties("allowedAttributesEffective").Value IsNot Nothing
            End Using
        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Sub AddUsers(ByVal userName As String)
        Dim StrDirEntryPath As String = "WinNT://" + _DomainNameValue + "/" + _ServerNameValue + ",computer"
        Dim Group As DirectoryEntry
        Dim UserEntry As DirectoryEntry
        Dim AD As DirectoryEntry

        Try
            AD = New DirectoryEntry(StrDirEntryPath)
            UserEntry = AD.Children.Find(userName, "user")
            Group = New DirectoryEntry("WinNT://" + _DomainNameValue + "/" + _ServerNameValue + "/" + _GroupNameValue + ",group")
            If Group IsNot Nothing AndAlso UserEntry IsNot Nothing Then
                Group.Invoke("Add", New Object() {UserEntry.Path.ToString()})
                Group.CommitChanges()
                Group.RefreshCache()
                Group.Close()
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub RemoveUsers(ByVal userName As String)
        Dim StrDirEntryPath As String = "WinNT://" + _DomainNameValue + "/" + _ServerNameValue + ",computer"
        Dim Group As DirectoryEntry
        Dim UserEntry As DirectoryEntry
        Dim AD As DirectoryEntry

        Try
            AD = New DirectoryEntry(StrDirEntryPath)
            UserEntry = AD.Children.Find(userName, "user")
            Group = New DirectoryEntry("WinNT://" + _DomainNameValue + "/" + _ServerNameValue + "/" + _GroupNameValue + ",group")
            If Group IsNot Nothing AndAlso UserEntry IsNot Nothing Then
                Group.Invoke("Remove", New Object() {UserEntry.Path.ToString()})
                Group.CommitChanges()
                Group.RefreshCache()
                Group.Close()
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

End Class
