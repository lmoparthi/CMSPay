Option Strict On

Imports System.Security.Principal

' -----------------------------------------------------------------------------
' Project	 : UFCW.CMS.Plan
' Class	 : CMS.Plan.AlterAccumulatorAction
'
' -----------------------------------------------------------------------------
' <summary>
'
' </summary>
' <remarks>
' </remarks>
' <history>
' 	[paulw]	4/19/2006	Created
'     [paulw] 4/26/2006   Added Dispose methods.  For reference to the methodology
'                         used in this class see:
'                         https://www.informit.com/articles/article.asp?p=25751&seqNum=3&rl=1
'                         Make sure to note that if this class is ever changed
'                         and inherited FROM, the boolean values change.
'                         The original reason for implementing Dispose was at the recommendation
'                         of FxCop.
'     [paulw] 9/12/2006   Commented out Dispose methods to try and see impact on performance.
'     [paulw] 9/13/2006   No impact seen on performance by commenting out.  From my research,
'                         my current recommendation would be to leave the dispose code commented out.
' </history>
' -----------------------------------------------------------------------------
<Serializable()> _
Public NotInheritable Class AlterAccumulatorAction
    Inherits Action

#Region "Private Variables"

    Private _MemberAccumulatorManager As MemberAccumulatorManager
    Private _ReturnString As String
    Private _OriginalClaimIDForAccident As Integer = -1
    Private _CurrentUserID As String

#End Region

#Region "Constructors"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Constructor
    ' </summary>
    ' <param name="accum"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	4/20/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New(ByVal accum As MemberAccumulatorManager)

        Dim WindowsUserID As WindowsIdentity
        Dim WindowsPrincipalForID As WindowsPrincipal

        If accum Is Nothing Then
            Throw New ArgumentNullException("accum")
        End If

        Try

            WindowsUserID = WindowsIdentity.GetCurrent()
            WindowsPrincipalForID = New WindowsPrincipal(WindowsUserID)

            _CurrentUserID = WindowsPrincipalForID.Identity.Name.ToUpper

            _MemberAccumulatorManager = accum

        Catch ex As Exception
            Throw
        End Try

    End Sub
#End Region

#Region "Properties"
    Public Property OriginalClaimIDForAccident() As Integer
        Get
            Return _OriginalClaimIDForAccident
        End Get
        Set(ByVal value As Integer)
            _OriginalClaimIDForAccident = Value
        End Set
    End Property
    Public Overrides ReadOnly Property Description() As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/1/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return "POST TO ACCUMULATOR " & Me.Name.Trim()
        End Get
    End Property

    Public Property AccumulatorManager() As MemberAccumulatorManager
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets/Sets the MemberAccumulatorManager object
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/20/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _MemberAccumulatorManager
        End Get
        Set(ByVal value As MemberAccumulatorManager)
            _MemberAccumulatorManager = Value
        End Set
    End Property
#End Region

#Region "Methods"
    Protected Overloads Overrides Function Execute(ByVal actionValue As Decimal) As Decimal
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Executes this action
        ' </summary>
        ' <param name="actionValue"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/26/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            _ReturnString = "Action: " & Me.Description & " VALUE : " & Math.Round(actionValue, 2)
            _MemberAccumulatorManager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID(Me.Name)), Me.ClaimID, CShort(Me.LineNumber), Me.OriginalClaimIDForAccident, CDate(Me.ApplyDate), Math.Round(actionValue, 2), False, _CurrentUserID)

            Return actionValue

        Catch ex As Exception
            Throw
        End Try
    End Function

    Protected Overloads Overrides Function Execute(ByVal actionValue As Decimal, ByVal condition As Condition) As Decimal

        Dim CurrentVal As Decimal
        Dim HeadRoom As Decimal
        Dim InsertAmount As Decimal

        Try

            _ReturnString = "Action: " & Me.Description & " VALUE : " & Math.Round(actionValue, 2)

            CurrentVal = _MemberAccumulatorManager.GetProposedValue(CInt(AccumulatorController.GetAccumulatorID(condition.AccumulatorName)), condition.DurationType, condition.Duration, CDate(Me.ApplyDate), condition.Direction)
            HeadRoom = condition.Operand - CurrentVal
            InsertAmount = actionValue

            'need to know what other accumulators of this type (ex. in-net ded) posted, if it is a family ded, and post that amount
            If HeadRoom < actionValue And HeadRoom >= 0 Then
                InsertAmount = HeadRoom
            End If

            _MemberAccumulatorManager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID(Me.Name)), Me.ClaimID, CShort(Me.LineNumber), Me.OriginalClaimIDForAccident, CDate(Me.ApplyDate), Math.Round(InsertAmount, 2), False, _CurrentUserID)

            Return InsertAmount

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Overrides Function ToString() As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Overrides the ToString method
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/1/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Return _ReturnString
    End Function
#End Region

End Class