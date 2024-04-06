Option Explicit On 
Option Strict On

Imports System.Reflection

''' -----------------------------------------------------------------------------
''' Project	 : PlugIn
''' Class	 : PlugInInfo
''' 
''' -----------------------------------------------------------------------------
''' <summary>
''' This is the possible plugin information when a plugin is created
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[Nick Snyder]	3/1/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class PlugInInfo
    Private _assemblyName As AssemblyName
    Private _typeName As String

    Public ReadOnly Property AssemblyName() As AssemblyName
        Get
            Return _assemblyName
        End Get
    End Property

    Public ReadOnly Property TypeName() As String
        Get
            Return _typeName
        End Get
    End Property

    Public Sub New(ByVal asmName As AssemblyName, ByVal typName As String)
        _assemblyName = asmName
        _typeName = typName
    End Sub
End Class