Public Class ListCategory

    Private _ListDescription As String
    Private _ListID As Object

    ' Default empty constructor. 
    Public Sub New()
        _ListDescription = ""
        _ListID = Nothing
    End Sub

    Public Sub New(ByVal ListDescription As String, ByVal ListID As Object)
        _ListDescription = ListDescription
        _ListID = ListID
    End Sub

    Public Property ListDescription() As String
        Get
            Return _ListDescription
        End Get
        Set(ByVal sValue As String)
            _ListDescription = sValue
        End Set
    End Property

    ' This is the property that holds the extra data. 
    Public Property ItemData() As Object
        Get
            Return _ListID
        End Get
        Set(ByVal iValue As Object)
            _ListID = iValue
        End Set
    End Property

    ' This is neccessary because the ListBox and ComboBox rely 
    ' on this method when determining the text to display. 
    Public Overrides Function ToString() As String
        Return ListDescription
    End Function

End Class

