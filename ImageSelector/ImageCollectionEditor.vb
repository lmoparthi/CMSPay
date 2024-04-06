Option Strict On

Public Class ImageCollectionEditor
    Inherits System.ComponentModel.Design.CollectionEditor

    Private Types() As System.Type
    Sub New(ByVal type As System.Type)
        MyBase.New(type)
        Types = New System.Type() {GetType(Image)}
    End Sub
    Protected Overrides Function CreateNewItemTypes() As System.Type()
        Return Types
    End Function

    'Sub New()
    '    MyBase.New(GetType(Image))
    'End Sub

    'Protected Overrides Function SetItems(ByVal editValue As Object, ByVal value() As Object) As Object
    '    Return MyBase.SetItems(editValue, value)
    'End Function

    'Protected Overrides Function GetItems(ByVal editValue As Object) As Object()
    '    Return MyBase.GetItems(editValue)
    'End Function

    Protected Overrides Function CreateCollectionForm() As System.ComponentModel.Design.CollectionEditor.CollectionForm
        Return MyBase.CreateCollectionForm()
    End Function

    Public Overloads Overrides Function EditValue(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal provider As System.IServiceProvider, ByVal value As Object) As Object
        Return MyBase.EditValue(context, provider, value)
    End Function
End Class
