Option Explicit On
Option Strict On

Imports System.Collections.Specialized
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common

Friend NotInheritable Class GeneralDataDAL
    ''' -----------------------------------------------------------------------------
    ''' Project	 : UFCW.CMS.Plan
    ''' Class	 : CMS.Plan.GeneralDataDAL
    '''
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' This class handles all CMS related data that does not belong to Staging or Active
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[paulw]	10/5/2006	Created (Moved over general DAL functions from other
    '''                         DAL classes too).
    ''' </history>
    ''' -----------------------------------------------------------------------------

    Private Shared ProcedureTotalDS As DataSet
    Private Shared _DiagnosisTotalDS As DataSet
    Private Shared _DiagnosisTotalSC As New StringCollection
    Private Shared _BillTypeTotalDS As DataSet
    Private Shared _BillTypeTotalSC As New StringCollection
    Private Shared _ModifierTotalDS As DataSet
    Private Shared _ModifierTotalSC As New StringCollection
    Private Shared _POSTotalDS As DataSet
    Private Shared _POSTotalSC As New StringCollection
    Private Shared ProviderTotalDS As DataSet
    Private Shared _ProviderTotalDT As DataTable
    Private Shared _ProviderTotalSC As New StringCollection
    Private Shared _RuleSetTypesTotalDS As DataSet
    Private Shared _RuleSetTypesTotalSC As New Collection

#Region "Constructor"
#End Region

#Region "Gets"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets all the ruleset types
    ' </summary>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/5/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetRulesetTypes() As Collection

        Try
            If _RuleSetTypesTotalSC?.Count = 0 Then
                If _RuleSetTypesTotalDS Is Nothing Then
                    _RuleSetTypesTotalDS = CMSDALFDBMD.RetrieveRuleSetTypesValues(_RuleSetTypesTotalDS)
                End If
                For Each DR As DataRow In _RuleSetTypesTotalDS.Tables(0).Rows
                    _RuleSetTypesTotalSC.Add(New RuleSetType(DR("RULE_SET_TYPE").ToString(), Convert.ToInt32(DR("RULE_SET_TYPE_ID").ToString())))
                Next
            End If

            Return _RuleSetTypesTotalSC

        Catch ex As Exception

            Throw New DataLayerException("Could not retrieve valid procedures from the database - " & ex.ToString, ex)

        Finally

        End Try

    End Function

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets the Valid Modifiers
    ' </summary>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	6/13/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetValidModifiers() As StringCollection

        Try
            If _ModifierTotalSC.Count = 0 Then
                If _ModifierTotalDS Is Nothing Then
                    _ModifierTotalDS = CMSDALFDBMD.RetrieveModifierValues(Nothing, _ModifierTotalDS)
                End If
                For Each DR As DataRow In _ModifierTotalDS.Tables(0).Rows
                    _ModifierTotalSC.Add(DR("MODIFIER_VALUE").ToString())
                Next
            End If

            Return _ModifierTotalSC
        Catch ex As Exception

            Throw New DataLayerException("Could not retrieve valid modifiers from the database - " & ex.ToString, ex)

        Finally
        End Try
    End Function
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets the Valid Place of Services
    ' </summary>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	6/13/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetValidPlaceOfServices() As StringCollection
        Try
            If _POSTotalSC.Count = 0 Then
                If _POSTotalDS Is Nothing Then
                    _POSTotalDS = CMSDALFDBMD.RetrievePlaceOfServiceValues(Nothing, _POSTotalDS)
                End If
                For Each DR As DataRow In _POSTotalDS.Tables(0).Rows
                    _POSTotalSC.Add(DR("PLACE_OF_SERV_VALUE").ToString())
                Next
            End If

            Return _POSTotalSC

        Catch ex As Exception

            Throw New DataLayerException("Could not retrieve valid Place of Services from the database - " & ex.ToString, ex)

        Finally
        End Try
    End Function
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets all the Valid Diagnosis Codes
    ' </summary>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	6/13/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetValidDiagnosis() As StringCollection

        Try

            If _DiagnosisTotalSC?.Count = 0 Then
                If _DiagnosisTotalDS Is Nothing Then
                    _DiagnosisTotalDS = CMSDALFDBMD.RetrieveDiagnosisValues(_DiagnosisTotalDS)
                End If
                For Each DR As DataRow In _DiagnosisTotalDS.Tables(0).Rows
                    _DiagnosisTotalSC.Add(DR("DIAG_VALUE").ToString())
                Next
            End If

            Return _DiagnosisTotalSC

        Catch ex As Exception

            Throw New DataLayerException("Could not retrieve valid diagnosis from the database - " & ex.ToString, ex)

        Finally
        End Try
    End Function
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets a list of Valid Providers
    ' </summary>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	6/13/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetValidProviders() As StringCollection
        Try

            If _ProviderTotalSC.Count = 0 Then

                If _ProviderTotalDT Is Nothing Then
                    _ProviderTotalDT = CMSDALFDBMD.RetrievePARs()
                End If

                For Each DR As DataRow In _ProviderTotalDT.Rows
                    _ProviderTotalSC.Add(DR("PAR_VALUE").ToString())
                Next
            End If

            Return _ProviderTotalSC

        Catch ex As Exception

            Throw New DataLayerException("Could not retrieve valid providers from the database - " & ex.ToString, ex)

        Finally
        End Try

    End Function
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets a list of Valid Bill Types
    ' </summary>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	6/13/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetValidBillTypes() As StringCollection

        Try
            If _BillTypeTotalSC.Count = 0 Then
                If _BillTypeTotalDS Is Nothing Then
                    _BillTypeTotalDS = CMSDALFDBMD.RetrieveBillTypeValues(Nothing, _BillTypeTotalDS)
                End If
                For Each DR As DataRow In _BillTypeTotalDS.Tables(0).Rows
                    _BillTypeTotalSC.Add(DR("BILL_TYPE_VALUE").ToString())
                Next
            End If

            Return _BillTypeTotalSC
        Catch ex As Exception

            Throw New DataLayerException("Could not retrieve valid bill types from the database - " & ex.ToString, ex)

        Finally
        End Try
    End Function

#End Region

#Region "Adds"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Add a ruleset type to the database
    ' </summary>
    ' <param name="typeName"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/5/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Sub AddRulesetType(ByVal typeName As String)
        Dim DB As Database = CMSDALCommon.CreateDatabase()

        Try
            Dim SQLCommand As String = "FDBMD.CREATE_RULE_SET_TYPE"
            Dim DBCommandWrapper As DbCommand = DB.GetStoredProcCommand(SQLCommand)
            DB.AddInParameter(DBCommandWrapper, "@RULE_SET_TYPE", DbType.String, typeName)
            DB.CreateConnection.Open()
            DB.ExecuteNonQuery(DBCommandWrapper)
        Catch ex As Exception

            Throw New PublishDataException("Cannot Add Ruleset Type - " & ex.ToString, ex)

        Finally
            If DB.CreateConnection IsNot Nothing Then
                DB.CreateConnection.Close()
            End If
        End Try
    End Sub
#End Region
End Class