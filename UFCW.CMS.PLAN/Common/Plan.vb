''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Plan
''' Class	 : CMS.Plan.Plan
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class reprents a base plan
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	2/10/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public MustInherit Class Plan
    Implements IPlan, IComparable

#Region "Private Variables"
    Protected _Description As String
    Protected _PlanType As String
    Protected _Procedures As Procedures
#End Region

#Region "Constructors"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Basic Constructor
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	2/10/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New()
    End Sub
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Force this as the base constructor
    ' </summary>
    ' <param name="planDescription"></param>
    ' <param name="planType"></param>
    ' <param name="planProcedures"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	2/10/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New(ByVal planDescription As String, ByVal planType As String, ByVal planProcedures As Procedures)
        _Description = planDescription
        _PlanType = planType
        _Procedures = planProcedures
    End Sub
#End Region

#Region "Methods"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Compare the Plans based on the description
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[behzadk]	6/20/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Function CompareTo(ByVal planObj As Object) As Integer Implements System.IComparable.CompareTo
        If Not TypeOf planObj Is Plan Then
            Throw New Exception("Object is not a Plan")
        End If
        Dim Compare As Plan = DirectCast(planObj, Plan)
        Dim Result As Integer = Me.Description.CompareTo(Compare.Description)

        If result = 0 Then
            result = Me.Description.CompareTo(Compare.Description)
        End If
        Return result
    End Function
#End Region

#Region "Properties"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' returns the description
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	2/10/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public ReadOnly Property Description() As String Implements IPlan.Description
        Get
            Return _Description
        End Get
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' returns the plantype
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	2/10/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public ReadOnly Property PlanType() As String Implements IPlan.PlanType
        Get
            Return _PlanType
        End Get
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' returns the procedures
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	2/10/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public ReadOnly Property Procedures() As Procedures Implements IPlan.Procedures
        Get
            Return _Procedures
        End Get
    End Property
#End Region

End Class