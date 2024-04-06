Option Explicit On
Option Strict On
''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Interface	 : CMS.Plan.IPlan
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' The 'Contract' for a Plan
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/25/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Interface IPlan

#Region "Properties"
    ReadOnly Property PlanType() As String
    ReadOnly Property Description() As String
    'ReadOnly Property CreateDate() As System.DateTime
    'ReadOnly Property CreateUserID() As String
    'ReadOnly Property ModifiedDate() As System.DateTime
    'ReadOnly Property ModifiedUserID() As String

    '    ReadOnly Property stagingMasterID() As Int32
    ReadOnly Property Procedures() As Procedures
    'ReadOnly Property Rulesets as Ruleset
#End Region

#Region "Functions"
    'Function SearchProcedures() As ProcedureCollection
#End Region

End Interface