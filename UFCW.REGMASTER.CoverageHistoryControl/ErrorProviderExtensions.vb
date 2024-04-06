Option Infer On

Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Linq
Imports Overby.Extensions.Attachments ' PM> Install-Package Overby.Extensions.Attachments

Public Module ErrorProviderExtensions
    <System.Runtime.CompilerServices.Extension>
    Public Sub TrackControl(ByVal ep As ErrorProvider, ByVal c As Control)
        Dim controls = ep.GetOrSetAttached(Function() New HashSet(Of Control)()).Value
        controls.Add(c)
    End Sub

    <System.Runtime.CompilerServices.Extension>
    Public Sub SetErrorWithTracking(ByVal ep As ErrorProvider, ByVal c As Control, ByVal [error] As String)
        ep.TrackControl(c)
        ep.SetError(c, [error])
    End Sub

    <System.Runtime.CompilerServices.Extension>
    Public Function GetErrorCount(ByVal ep As ErrorProvider) As Integer
        Dim controls = ep.GetOrSetAttached(Function() New HashSet(Of Control)()).Value

        Dim errControls = From c In controls
                          Let err = ep.GetError(c)
                          Let hasErr = Not String.IsNullOrEmpty(err)
                          Where hasErr
                          Select c

        Dim errCount = errControls.Count()
        Return errCount
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Sub ClearError(ByVal ep As ErrorProvider, ByVal c As Control)
        ep.SetError(c, Nothing)
    End Sub
End Module
