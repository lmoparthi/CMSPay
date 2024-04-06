<Serializable()> _
Public Class Rulesets
    Inherits CollectionBase
    Implements ICloneable

    ' -----------------------------------------------------------------------------
    ' Project	 : UFCW.CMS.Plan
    ' Class	 : CMS.Plan.RulesetCollection
    '
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' This class represents a ruleset collection
    ' </summary>
    ' <remarks>
    ' Constraints mandate that only one of each type can be in the collection
    ' </remarks>
    ' <history>
    ' 	[paulw]	7/21/2006   Created
    ' </history>
    ' -----------------------------------------------------------------------------


#Region "Properties"
    Default Public Property Item(ByVal index As Integer) As RuleSet
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Used for indexing of the collection
        ' </summary>
        ' <param name="index"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	7/21/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return DirectCast(Me.List(index), RuleSet)
        End Get
        Set(ByVal value As RuleSet)
            Me.List(index) = value
        End Set
    End Property
#End Region

#Region "Methods"

    Public Sub Add(ByVal ruleSetToAdd As RuleSet, Optional replaceIfExists As Boolean = True)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Adds a ruleset to the collection.  only allows one per type
        ' </summary>
        ' <param name="value"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	7/21/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            If replaceIfExists Then
                For Each RuleSet As RuleSet In Me.List
                    If RuleSet.RulesetType = ruleSetToAdd.RulesetType Then
                        Me.List.Remove(RuleSet)
                        Exit For
                    End If
                Next
            End If

            Me.List.Add(ruleSetToAdd)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Protected Overrides Sub OnValidate(ByVal value As Object)

        ' -----------------------------------------------------------------------------
        ' <summary>
        ' This is called when a ruleset is added
        ' </summary>
        ' <param name="value"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	7/21/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        MyBase.OnValidate(value)
        If Not (TypeOf (value) Is IRuleset) Then
            Throw New ArgumentException("Collection only supports objects implementing IRuleset.")
        End If
    End Sub

    Public Function Contains(ByVal ruleSetToCheck As IRuleset) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Determines if the ruleset already exists in the collection
        ' </summary>
        ' <param name="item"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	7/21/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        If ruleSetToCheck Is Nothing Then Throw New ArgumentNullException("item")

        Dim RuleSet As RuleSet

        Try

            For I As Integer = 0 To Me.List.Count - 1
                RuleSet = DirectCast(Me.List(I), RuleSet)
                If RuleSet.RuleSetName = ruleSetToCheck.RuleSetName Then
                    Return True
                End If
            Next
        Catch ex As Exception
            Throw
        Finally
            RuleSet = Nothing
        End Try
    End Function

    Public Function Contains(ByVal ruleSetTypeToCheck As Integer) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Determines if the type of ruleset already exists in the collection
        ' </summary>
        ' <param name="value"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	7/21/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim RuleSet As RuleSet
        Try

            For I As Integer = 0 To Me.List.Count - 1
                RuleSet = DirectCast(Me.List(I), RuleSet)
                If RuleSet.RulesetType = ruleSetTypeToCheck Then
                    Return True
                End If
            Next

        Catch ex As Exception
            Throw
        Finally
            RuleSet = Nothing
        End Try
    End Function
#End Region

#Region "Clone"

    Public Function Clone() As Object Implements ICloneable.Clone

        Try

            Return CloneHelper.Clone(Me)

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

#End Region

End Class