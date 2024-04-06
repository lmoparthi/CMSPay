Option Strict On

Public Class SelectedImageCollection
    Inherits System.Collections.CollectionBase

    Public ReadOnly Property Item(ByVal Index As Integer) As Image
        Get
            Return CType(List.Item(Index), Image)
        End Get
    End Property

    Public ReadOnly Property Contains(ByVal Item As Image) As Boolean
        Get
            Return List.Contains(Item)
        End Get
    End Property

    Public Function IndexOf(ByVal Item As Image) As Integer
        List.IndexOf(Item)
    End Function

    Public Function Add(ByVal Item As Image) As Integer
        Return List.Add(Item)
    End Function

    Public Sub Remove(ByVal Item As Image)
        List.RemoveAt(List.IndexOf(Item))
    End Sub
End Class
