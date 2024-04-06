''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Interface	 : CMS.Plan.IContract
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This interface represents a contract for a contract
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	2/10/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Interface IContract
    ReadOnly Property BeginDate() As Date
    ReadOnly Property EndDate() As Date
End Interface