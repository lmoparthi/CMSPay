Option Explicit On
Option Strict On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.Contract
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents a Contract
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/26/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public NotInheritable Class Contract
    Implements IContract

#Region "Constructors"

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Private constructor so you must instantiate with begin and end dates
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/26/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub New()

    End Sub
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Contract that forces instantiation with begin and end date of contract
    ' </summary>
    ' <param name="beginDate">The begin date of the contract</param>
    ' <param name="endDate">The end date of the contract</param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/26/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New(ByVal beginDate As Date, ByVal endDate As Date)
        If beginDate = #12:00:00 AM# Then
            Throw New InvalidContractException("Must Set Beginning Contract Date")
            Return
        End If
        If endDate = #12:00:00 AM# Then
            Throw New InvalidContractException("Must Set End Contract Date")
            Return
        End If
        _BeginDate = beginDate
        _EndDate = endDate
    End Sub
#End Region

#Region "Properties and Variables"

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _BeginDate As Date
    Private _EndDate As Date
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets the beginning date for this contract
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/26/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public ReadOnly Property BeginDate() As Date Implements IContract.BeginDate
        Get
            Return _BeginDate
        End Get
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' gets the end date for this contract
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/26/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public ReadOnly Property EndDate() As Date Implements IContract.EndDate
        Get
            Return _EndDate
        End Get
    End Property
#End Region

#Region "Validation"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Determines if this Contract is Valid according to business rules.
    ' Only one future contract is allowed at anytime
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/26/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public ReadOnly Property IsValidForPublication() As Boolean
        Get
            Try
                'both contracts are publised
                If Not ContractController.NextContract Is Nothing Then
                    'return because there can be no more contracts published
                    Return False
                End If
                'if there are not gaps between contract
                If Not Date.op_Equality(ContractController.CurrentContract.EndDate.AddDays(1), CDate(_BeginDate.ToShortDateString)) Then
                    Return False
                End If
                'if the beginning date is not less than the end date
                If Not Date.op_LessThan(CDate(_BeginDate.ToShortDateString), CDate(_EndDate.ToShortDateString)) Then
                    Return False
                End If

                Return True

            Catch ex As Exception

                Throw New InvalidContractException("Contract determine if contract is valid", ex)

            End Try
        End Get
    End Property
#End Region

End Class