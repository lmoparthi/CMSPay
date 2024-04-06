Imports System.ComponentModel
Imports System.Reflection

Friend Class CustomPropertyDescriptor
    Inherits PropertyDescriptor

    Private propertyInfo As PropertyInfo
    Public Sub New(ByVal propertyInfo As PropertyInfo)
        MyBase.New(propertyInfo.Name, Array.ConvertAll(propertyInfo.GetCustomAttributes(True), Function(o) CType(o, Attribute)))
        Me.propertyInfo = propertyInfo
    End Sub
    Public Overrides Function CanResetValue(ByVal component As Object) As Boolean
        Return False
    End Function

    Public Overrides ReadOnly Property ComponentType() As Type
        Get
            Return Me.propertyInfo.DeclaringType
        End Get
    End Property

    Public Overrides Function GetValue(ByVal component As Object) As Object
        Return Me.propertyInfo.GetValue(component, Nothing)
    End Function

    Public Overrides ReadOnly Property IsReadOnly() As Boolean
        Get
            Return Not Me.propertyInfo.CanWrite
        End Get
    End Property

    Public Overrides ReadOnly Property PropertyType() As Type
        Get
            Return Me.propertyInfo.PropertyType
        End Get
    End Property

    Public Overrides Sub ResetValue(ByVal component As Object)
    End Sub

    Public Overrides Sub SetValue(ByVal component As Object, ByVal value As Object)
        Me.propertyInfo.SetValue(component, value, Nothing)
    End Sub

    Public Overrides Function ShouldSerializeValue(ByVal component As Object) As Boolean
        Return False
    End Function
End Class