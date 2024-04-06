Option Infer On

Imports System.Collections.Generic
Imports System.Windows.Forms
Imports Overby.Extensions.Attachments ' PM> Install-Package Overby.Extensions.Attachments
Imports System.Text

<System.Diagnostics.DebuggerStepThrough()>
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

    <System.Runtime.CompilerServices.Extension>
    Public Function Validate(ByVal ep As ErrorProvider,
                  ByVal Control As Control,
                  Optional ByVal ErrorMessage As String = “Field cannot be empty!”) As Boolean

        If String.IsNullOrEmpty(Control.Text.Trim) Then
            ep.SetError(Control, ErrorMessage)
            Return False
            Exit Function
        End If

        ep.SetError(Control, Nothing)
        Return True
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function HasErrors(ByVal ep As ErrorProvider) As Boolean
        Dim err As Nullable(Of Integer) = (From e In ep.ContainerControl.Controls
                                           Let msg = ep.GetError(CType(e, Control))
                                           Where msg.Length > 0
                                           Select e).Count
        If err.GetValueOrDefault(0) > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function GetErrorMessages(ByVal ep As ErrorProvider) As String
        Dim sb As New StringBuilder

        Dim controls = ep.GetOrSetAttached(Function() New HashSet(Of Control)()).Value

        Dim errControls = From c In controls
                          Let err = ep.GetError(c)
                          Let hasErr = Not String.IsNullOrEmpty(err)
                          Where hasErr
                          Select err

        For Each errmsg In errControls
            sb.Append(errmsg)
        Next

        Return sb.ToString

    End Function

End Module
