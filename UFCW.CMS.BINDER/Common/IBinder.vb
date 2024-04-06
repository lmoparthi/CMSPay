Option Strict On

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Binder
''' Interface	 : CMS.Binder.IBinder
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Interface for a Binder
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/4/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Interface IBinder

#Region "Properties"

    Property BinderAccumulatorManager() As MemberAccumulatorManager
    Property BinderItems() As Hashtable
    Property BinderAlertManager() As AlertManagerCollection
    Property Facts() As Facts
    Property DateOfClaim() As Date?
    Property ClaimNumber() As Integer
    Property OriginalClaimIDForAccident() As Integer

    ReadOnly Property HasAccidentRule() As Boolean
    ReadOnly Property IsHRAInEligible() As Boolean
    ReadOnly Property IsPreventative() As Boolean
    ReadOnly Property TypeOfBinder() As Integer
    ReadOnly Property DocumentClass() As String

#End Region

#Region "Methods"

    Sub AddBinderItem(ByVal lineNumber As Short, ByVal procedure As ProcedureActive, ByVal valuedAmount As Decimal, ByVal incidentDate As Date)
    Sub AddBinderItem(ByVal lineNumber As Short)
    Sub AddBinderItem(ByVal binderItem As BinderItem)
    Sub BuildFacts()
    Sub BuildFacts(ByVal lineNumbers As Short())
    Sub RemoveBinderItem(ByVal lineNumber As Short)
    Sub RemoveAccidentAccumulators()
    Sub ReplaceAccidentAccumulator(ByVal accumulatorName As String)
    Function GetAccumulatorSummary() As DataTable
    Function GetAccumulatorSummaryCommitted(ByVal conditionsDate As Date) As DataTable
    Function GetBinderItem(ByVal lineNumber As Short) As BinderItem
    Function GetOriginalAccumulatorSummary() As DataTable
    Function NewBinderItem() As BinderItem

#End Region

End Interface