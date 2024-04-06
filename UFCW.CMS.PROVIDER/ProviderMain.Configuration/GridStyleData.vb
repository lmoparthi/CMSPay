Imports System
Imports System.Drawing
Imports System.Xml.Serialization

Public Class GridStyleData

    Private m_ColumnCount As Integer
    Private m_Column As ColumnData

    Public Sub New()

    End Sub

    Public Sub New(ByVal ColumnCount As Integer, ByVal Column As ColumnData)
        Me.ColumnCount = ColumnCount
        Me.Column = Column
    End Sub

    <XmlElement("ColumnCount")> _
    Public Property ColumnCount() As Integer
        Get
            Return Me.m_ColumnCount
        End Get
        Set(ByVal Value As Integer)
            Me.m_ColumnCount = Value
        End Set
    End Property

    <XmlElement()> _
    Public Property Column() As ColumnData
        Get
            Return Me.m_Column
        End Get

        Set(ByVal Value As ColumnData)
            m_Column = New ColumnData
            m_Column.DefaultOrder = Value.DefaultOrder
            m_Column.Format = Value.Format
            m_Column.HeaderText = Value.HeaderText
            m_Column.Mapping = Value.Mapping
            m_Column.NullText = Value.NullText
        End Set
    End Property

End Class

Public Class ColumnData

    Private m_Mapping As String
    Private m_HeaderText As String
    Private m_Format As String
    Private m_DefaultOrder As Single
    Private m_NullText As String

    Public Sub New()
    End Sub

    Public Property HeaderText() As String
        Get
            Return Me.m_HeaderText
        End Get
        Set(ByVal Value As String)
            Me.m_HeaderText = Value
        End Set
    End Property

    Public Property Format() As String
        Get
            Return Me.m_Format
        End Get
        Set(ByVal Value As String)
            Me.m_Format = Value
        End Set
    End Property
    Public Property NullText() As String
        Get
            Return Me.m_NullText
        End Get
        Set(ByVal Value As String)
            Me.m_NullText = Value
        End Set
    End Property
    Public Property Mapping() As String
        Get
            Return Me.m_Mapping
        End Get
        Set(ByVal Value As String)
            Me.m_Mapping = Value
        End Set
    End Property
    Public Property DefaultOrder() As Single
        Get
            Return Me.m_DefaultOrder
        End Get
        Set(ByVal Value As Single)
            Me.m_DefaultOrder = Value
        End Set
    End Property

End Class

