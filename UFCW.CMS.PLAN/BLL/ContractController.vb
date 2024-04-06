Option Strict On
Option Explicit On
Imports System
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.ContractController
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class is used for basic control of data for Contracts.
''' In this class I am assuming that we are starting with a database
'''  that already has the current contract in the proper table. Some logix
'''  is not correct if that assumption is wrong.  For instance, to verify if
'''  a contract is valid for publication (IsValidForPublish), we are checking the
'''  current contract.  In other words, there is no way to publish a 'current'
'''  contract.  and there should not be anyway.
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/25/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public NotInheritable Class ContractController

#Region "Constructors"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Constructor is private to ensure no instantiation
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub New()
    End Sub
#End Region

#Region "Functions"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Retrieves the Contract dates that sorround the request date
    ' </summary>
    ' <param name="requestDate">The date that should fall within the range of a contract</param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetContract(ByVal requestDate As Date) As Contract
        Try
            Return ContractDAL.GetContract(requestDate)
        Catch ex As Exception

            Throw New InvalidContractException("Invalid Contract", ex)

        End Try
    End Function
#End Region

#Region "Properties and Variables"
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private Shared _CurrentContract As Contract
    Private Shared _NextContract As Contract
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets the Current Contract
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared ReadOnly Property CurrentContract() As Contract
        Get
            If _CurrentContract Is Nothing Then
                _CurrentContract = GetContract(UFCWGeneral.NowDate)
            End If
            Return _CurrentContract
        End Get
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Returns the next contract
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/26/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared ReadOnly Property NextContract() As Contract
        Get
            If _NextContract Is Nothing Then
                'get the contract for 'current contract end date + 1 day' (next contract)
                _NextContract = GetContract(CurrentContract.EndDate.AddDays(1))
            End If
            Return _NextContract
        End Get
    End Property
#End Region

#Region "Publish"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Publishes the valid contract that is passed in
    ' </summary>
    ' <param name="contractToPublish">The contract to publish</param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/26/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub Publish(ByVal contractToPublish As Contract)
        If contractToPublish Is Nothing Then Throw New System.ArgumentNullException("contractToPublish")
        If Not contractToPublish.IsValidForPublication Then
            Throw New PublishException("Contract Is Not Valid")
            Return
        End If
        Try
            ContractDAL.PublishContract(contractToPublish)
        Catch ex As Exception

            Throw New PublishException("There was a Problem Publishing the Contract")

        End Try
    End Sub
#End Region
End Class