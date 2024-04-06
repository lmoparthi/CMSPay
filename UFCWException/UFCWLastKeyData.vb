Imports System.Configuration

Public Class UFCWLastKeyData
    Private Shared _ClaimID As New Stack
    Private Shared _DocumentID As New Stack
    Private Shared _FamilyID As New Stack
    Private Shared _RelationID As New Stack
    Private Shared _Log As New Stack
    Private Shared _CMD As New Stack
    Private Shared _SQL As New Stack
    Private Shared _SSN As New Stack
    Private Shared _TEXT As New Stack
    Private Shared _StackSizeLimit As Integer = If(ConfigurationManager.AppSettings("StackSizeLimit") Is Nothing, 3, CInt(ConfigurationManager.AppSettings("StackSizeLimit")))

    Public Shared Property ClaimID As String

        Get
            If _ClaimID Is Nothing Then Return ""
            Return String.Join(";", _ClaimID.ToArray)
        End Get
        Set(value As String)
            If Not _ClaimID.Contains(value) Then _ClaimID.Push(Now.ToLongTimeString.ToString & "- " & value)
            If _ClaimID.Count > _StackSizeLimit Then _ClaimID.Pop()
        End Set
    End Property

    Public Shared Property DocumentID As String

        Get
            If _DocumentID Is Nothing Then Return ""
            Return String.Join(";", _DocumentID.ToArray)
        End Get
        Set(value As String)
            If Not _DocumentID.Contains(value) Then _DocumentID.Push(Now.ToLongTimeString.ToString & "- " & value)
            If _DocumentID.Count > _StackSizeLimit Then _DocumentID.Pop()
        End Set
    End Property
    Public Shared Property FamilyID As String

        Get
            If _FamilyID Is Nothing Then Return ""
            Return String.Join(";", _FamilyID.ToArray)
        End Get
        Set(value As String)
            If Not _FamilyID.Contains(value) Then _FamilyID.Push(Now.ToLongTimeString.ToString & "- " & value)
            If _FamilyID.Count > _StackSizeLimit Then _FamilyID.Pop()
        End Set
    End Property

    Public Shared Property RelationID As String

        Get
            If _RelationID Is Nothing Then Return ""
            Return String.Join(";", _RelationID.ToArray)
        End Get
        Set(value As String)
            If Not _RelationID.Contains(value) Then _RelationID.Push(Now.ToLongTimeString.ToString & "- " & value)
            If _RelationID.Count > _StackSizeLimit Then _RelationID.Pop()
        End Set
    End Property

    Public Shared Property SQL As String

        Get
            If _SQL Is Nothing Then Return ""
            Return String.Join(";", _SQL.ToArray)
        End Get
        Set(value As String)
            If Not _SQL.Contains(value) Then _SQL.Push(Now.ToLongTimeString.ToString & "- " & value)
            If _SQL.Count > _StackSizeLimit Then _SQL.Pop()
        End Set
    End Property

    Public Shared Property CMD As String

        Get
            If _CMD Is Nothing Then Return ""
            Return String.Join(";", _CMD.ToArray)
        End Get
        Set(value As String)
            If Not _CMD.Contains(value) Then _CMD.Push(Now.ToLongTimeString.ToString & "- " & value)
            If _CMD.Count > _StackSizeLimit Then _CMD.Pop()
        End Set
    End Property

    Public Shared Property Log As String

        Get
            If _Log Is Nothing Then Return ""
            Return String.Join(";", _Log.ToArray)
        End Get
        Set(value As String)
            If Not _Log.Contains(value) Then _Log.Push(value)
            If _Log.Count > _StackSizeLimit Then _Log.Pop()
        End Set
    End Property

    Public Shared Property TEXT As String

        Get
            If _TEXT Is Nothing Then Return ""
            Return String.Join(";", _TEXT.ToArray)
        End Get
        Set(value As String)
            If Not _TEXT.Contains(value) Then _TEXT.Push(Now.ToLongTimeString.ToString & "- " & value)
            If _TEXT.Count > _StackSizeLimit Then _TEXT.Pop()
        End Set
    End Property

    Public Shared Property SSN As String

        Get
            If _SSN Is Nothing Then Return ""
            Return String.Join(";", _SSN.ToArray)
        End Get
        Set(value As String)
            If Not _SSN.Contains(value) Then _SSN.Push(Now.ToLongTimeString.ToString & "- " & value)
            If _SSN.Count > _StackSizeLimit Then _SSN.Pop()
        End Set
    End Property
End Class

