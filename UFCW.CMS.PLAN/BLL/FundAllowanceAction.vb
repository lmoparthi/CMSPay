Option Strict On

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.FundReductionAction
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Fund Reduction Actions
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	5/3/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public Class FundAllowanceAction
    Inherits Action

#Region "Private Variables"
    Private _ReturnString As String
#End Region

#Region "Constructors"
    Public Sub New()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Constructor
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/3/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        _Description = "REDUCED TO ALLOWANCE OF:"
    End Sub
#End Region

#Region "Methods"
    Protected Overloads Overrides Function Execute(ByVal actionValue As Decimal) As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Basically builds up the arg to return from ToString()
        ' </summary>
        ' <param name="actionValue">The value that the action will use</param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/3/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            _ReturnString = "Action: FUND AMOUNT OF: " & Math.Round(actionValue, 2).ToString & " " & _Description & " " & Me.Name

            Return actionValue

        Catch ex As Exception
            Throw
        End Try
    End Function

    Protected Overloads Overrides Function Execute(ByVal actionValue As Decimal, ByVal condition As Condition) As Decimal
        Try

            _ReturnString = "Action: FUND AMOUNT OF: " & Math.Round(actionValue, 2).ToString & " " & _Description & " " & Me.Name
            Return actionValue

        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Overrides Function ToString() As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Common ToString function
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/3/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
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
    ' 	[paulw]	5/3/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Overrides ReadOnly Property Description() As String
        Get
            Return _Description
        End Get
    End Property
#End Region
End Class