''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.Binder
''' Class	 : CMS.Binder.BinderItemEventArgs
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class is used for event handling
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/7/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class BinderItemEventArgs
    Inherits EventArgs

#Region "Private variables"

    Private _OldValue As Object
    Private _NewValue As Object
    Private _PropertyName As String
#End Region

#Region "Constructors"

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Default Constructor
    ' </summary>
    ' <param name="oldVal"></param>
    ' <param name="newVal"></param>
    ' <param name="propertyName"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw] 4/7/2006    Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New(ByVal oldVal As Object, ByVal newVal As Object, ByVal propertyName As String)
        _OldValue = oldVal
        _NewValue = newVal
    End Sub
#End Region
#Region "Properties"
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Old value of item
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw] 4/7/2006    Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public ReadOnly Property OldValue() As Object
        Get
            Return _OldValue
        End Get
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' New value of item
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw] 4/7/2006    Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public ReadOnly Property NewValue() As Object
        Get
            Return _NewValue
        End Get
    End Property

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Name of object to be changed from old value to new value
    ' </summary>
    ' <value></value>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw] 4/7/2006    Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public ReadOnly Property PropertyName() As String
        Get
            Return _PropertyName
        End Get
    End Property
#End Region
End Class