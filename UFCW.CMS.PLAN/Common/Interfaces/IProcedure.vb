Option Explicit On
Option Strict On
''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Interface	 : CMS.Plan.IProcedure
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' The 'contract' for a Procedure
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/25/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Interface IProcedure

#Region "Properties"

    ReadOnly Property ProcedureID() As Integer?
    Property ProcedureCode() As String
    Property PlanType() As String
    Property Provider() As String
    Property Modifier() As String
    Property Gender() As String
    Property MonthsMin() As Integer?
    Property MonthsMax() As Integer?
    Property PlaceOfService() As String
    Property BillType() As String
    Property Diagnosis() As String
    Property Rulesets() As Rulesets
    Function GetHashCode() As Integer
    Property Weight() As Integer?
    Function IsDenyRule(ByVal ruleSetType As Integer?) As Boolean
    Function IsProviderWriteOffRule(ByVal ruleSetType As Integer?) As Boolean
    Function IsDenyOrProviderWriteOffRule(ByVal ruleSetType As Integer?) As Boolean
    Function IsOriginalRule(ByVal ruleSetType As Integer?) As Boolean
    Function IsReviewRule(ByVal ruleSetType As Integer?) As Boolean

#End Region

End Interface