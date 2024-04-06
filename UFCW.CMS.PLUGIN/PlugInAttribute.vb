Option Explicit On 
Option Strict On

Namespace PlugInSharedInterfaces
    ' -----------------------------------------------------------------------------
    ' Project	 : PlugIn
    ' Class	 : PlugInAttribute
    ' 
    ' -----------------------------------------------------------------------------
    ' <summary>
    ' These are the possible attributes a plugin can have.
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[Nick Snyder]	3/1/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Class PlugInAttribute
        Inherits System.Attribute

        Private _mnuText As String
        Private _PlugInDestination As String
        Private _imgIndex As Integer
        Private _Position As Integer = -1

        Public ReadOnly Property MenuText() As String
            Get
                Return _mnuText
            End Get
        End Property

        Public ReadOnly Property Destination() As String
            Get
                Return _PlugInDestination
            End Get
        End Property

        Public ReadOnly Property ImageIndex() As Integer
            Get
                Return _imgIndex
            End Get
        End Property

        Public ReadOnly Property Position() As Integer
            Get
                Return _Position
            End Get
        End Property

        Public Sub New(ByVal mnuText As String, ByVal PlugInDestination As String)
            _mnuText = mnuText
            _PlugInDestination = PlugInDestination
        End Sub

        Public Sub New(ByVal mnuText As String, ByVal PlugInDestination As String, ByVal ImageIndex As Integer)
            _mnuText = mnuText
            _PlugInDestination = PlugInDestination
            _imgIndex = ImageIndex
        End Sub

        Public Sub New(ByVal mnuText As String, ByVal Position As Integer, ByVal PlugInDestination As String)
            _mnuText = mnuText
            _PlugInDestination = PlugInDestination
            _imgIndex = ImageIndex
            _Position = Position
        End Sub

        Public Sub New(ByVal mnuText As String, ByVal Position As Integer, ByVal PlugInDestination As String, ByVal ImageIndex As Integer)
            _mnuText = mnuText
            _PlugInDestination = PlugInDestination
            _imgIndex = ImageIndex
            _Position = Position
        End Sub
    End Class

End Namespace