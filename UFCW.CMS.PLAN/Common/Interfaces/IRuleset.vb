Option Explicit On
Option Strict On
''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Interface	 : CMS.Plan.IRuleset
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' The 'Contract' For the Ruleset
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	    1/25/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Interface IRuleset
    Inherits ICollection
#Region "Properties"
    Property RulesetID() As Integer
    Property RuleSetName() As String
    Property Hidden() As Boolean
    Sub Add(ByVal item As Rule)
    Sub ClearRules()
#End Region
End Interface