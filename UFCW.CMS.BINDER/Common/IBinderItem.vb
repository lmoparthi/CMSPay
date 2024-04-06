''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Binder
''' Interface	 : CMS.Binder.IBinderItem
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Interface for a BinderItem
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/4/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Interface IBinderItem

#Region "Properties"
    Property LineNumber() As Short
    Property Procedure() As ProcedureActive
    Property RuleSetIDUsed() As Integer
    Property RuleSetNameUsed() As String
    Property ValuedAmount() As Decimal
    Property PaymentAmount() As Decimal
    Property MemberAmount() As Decimal
    Property UnitAmount() As Decimal
    Property Status() As BinderItemStatus
    Property StatusDescription() As String
    Property IncidentDate() As Date
    Property DateOfService() As Date
    Property DateOfBirth() As Date
    Property Gender() As String
    Property PrimaryDiagnosis() As String

    ReadOnly Property IsPreventative(ByVal rlSetType As Integer) As Boolean
    ReadOnly Property IsHRAInEligible(ByVal rlSetType As Integer) As Boolean
    ReadOnly Property HasAccidentRule(ByVal rlSetType As Integer) As Boolean
    ReadOnly Property AccidentDurationType(ByVal rlSetType As Integer) As DateType
    ReadOnly Property AccidentDuration(ByVal rlSetType As Integer) As Integer

#End Region

#Region "Methods"

    Sub ReplaceAccidentAccumulator(ByVal accumName As String)
    Sub RemoveAccidentAccumulators()

#End Region

End Interface

Public Enum BinderItemStatus
    Processed
    NeedsReprice
    Failed
    Denied
    ProviderWriteOff
    InvalidAccumulator
End Enum

