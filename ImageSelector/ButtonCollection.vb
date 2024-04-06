Option Strict On

Public Class ButtonCollection
    Inherits System.Collections.CollectionBase

    Public ReadOnly Property Item(ByVal Index As Integer) As Button
        Get
            Return CType(List.Item(Index), Button)
        End Get
    End Property

    Public ReadOnly Property Contains(ByVal Item As Button) As Boolean
        Get
            Return List.Contains(Item)
        End Get
    End Property

    Public Function Add(ByVal Item As Button) As Integer
        Return List.Add(Item)
    End Function

    Public Sub Remove(ByVal Item As Button)
        List.RemoveAt(List.IndexOf(Item))
    End Sub
End Class
