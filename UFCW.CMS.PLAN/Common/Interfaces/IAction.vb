''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessorEngine
''' Interface	 : CMS.ProcessorEngine.IAction
'''
''' -----------------------------------------------------------------------------
''' <summary>
'''
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/12/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Interface IAction
    Delegate Sub ExecuteAction(ByVal actionArgs As ActionArgs)
    ReadOnly Property Description() As String
    Property ActionValueType() As ActionValueTypes
    Function Execute(ByVal amt As Decimal) As Decimal
    Function Execute(ByVal amt As Decimal, ByVal condition As Condition) As Decimal
    Property ActionValue() As Decimal
    Property ClaimID() As Integer
    Property LineNumber() As Short
    Property Name() As String
    'Sub Finalize()
    'Sub Dispose()
    'Sub Dispose(ByVal disposing As Boolean)
    Function ToString() As String
End Interface
Public Enum ActionValueTypes
    ActualValue
    FundPaymentValue
    MemberPaymentValue
    UnitsValue
    PreSubTotalMemberPaymentValue
    PreSubTotalNonParMemberPaymentValue
    PreSubTotalParMemberPaymentValue
    ParMemberPaymentValue
    NonParMemberPaymentValue
    Other
End Enum
