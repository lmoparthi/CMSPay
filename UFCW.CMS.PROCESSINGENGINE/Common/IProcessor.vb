''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessingEngine
''' Class	 : IProcessor
'''
''' -----------------------------------------------------------------------------
''' <summary>
'''
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/21/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Interface IProcessor
    Sub Process(ByVal binder As Binder)
    Event OnProcessorComplete(ByVal actionDescriptions As Collection)
End Interface