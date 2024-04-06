''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.FundDenialAction
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Fund Denial Action
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	3/8/2007	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public Class ProviderWriteOffAction
    Inherits Action

#Region "Private Variables"
    Private _ReturnString As String
#End Region

#Region "Constructors"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Constructor
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/3/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New()
        _Description = "PROVIDER WRITE OFF"
    End Sub
#End Region

#Region "Methods"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Basically builds up the arg to return from ToString()
    ' </summary>
    ' <param name="actionValue">The value that the action will use</param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/3/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Protected Overloads Overrides Function Execute(ByVal actionValue As Decimal) As Decimal
        _ReturnString = _Description
        Throw New ProviderWriteOffException
    End Function

    Protected Overloads Overrides Function Execute(ByVal actionValue As Decimal, ByVal cnd As Condition) As Decimal
        _ReturnString = "Action: FUND AMOUNT OF: " & Math.Round(actionValue, 2).ToString & " " & Me._Description & " " & Me.Name
        Throw New ProviderWriteOffException
    End Function
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Common ToString function
    ' </summary>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/3/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Overrides Function ToString() As String
        Return _ReturnString
    End Function
#End Region

#Region "Properties"

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' The description of what the action is
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/3/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Overrides ReadOnly Property Description() As String
        Get
            Return _Description
        End Get
    End Property
#End Region

End Class