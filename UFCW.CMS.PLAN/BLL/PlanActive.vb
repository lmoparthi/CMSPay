Option Explicit On
Option Strict On
''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.PlanActive
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class represents and handles Active Plans
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	1/25/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public Class PlanActive
    Inherits Plan

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

#Region "Constructors"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Constructor
    ' </summary>
    ' <param name="planType"></param>
    ' <param name="planDescription"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	1/25/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New(ByVal planType As String, ByVal planDescription As String, ByVal constructAsLightweight As Boolean)
        MyBase.New()
        Me._Description = planDescription
        Me._PlanType = planType
        Dim procCollection As New Procedures
        If Not constructAsLightweight Then
            procCollection = PlanActiveDAL.GetActiveProcedures(planType, constructAsLightweight)
        End If
        Me._Procedures = procCollection
        'MyBase.New(planDescription, planType, CType(IF(constructAsLightweight, New ProcedureCollection, PlanActiveDAL.GetActiveProcedures(planType)), ProcedureCollection))
    End Sub
#End Region
End Class