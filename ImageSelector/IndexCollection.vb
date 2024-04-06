Option Strict On

Public Class IndexCollection
    Inherits System.Collections.CollectionBase

    Public ReadOnly Property Item(ByVal Index As Integer) As Integer
        Get
            Return CInt(List.Item(Index))
        End Get
    End Property

    Public ReadOnly Property Contains(ByVal Index As Integer) As Boolean
        Get
            Return List.Contains(Index)
        End Get
    End Property

    Public Function Add(ByVal Index As Integer) As Integer
        Return List.Add(Index)
    End Function

    Public Sub Remove(ByVal Index As Integer)
        List.RemoveAt(List.IndexOf(Index))
    End Sub
End Class
