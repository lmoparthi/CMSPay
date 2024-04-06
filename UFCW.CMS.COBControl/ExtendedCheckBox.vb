Imports System.ComponentModel

Public Class ExtendedCheckBox
    Inherits ExCheckBox

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Public Property OneZero() As String

        Get

            If Me.Checked Then

                Return "1"
            Else

                Return "0"

            End If
        End Get

        Set(ByVal value As String)

            Dim Changed As Boolean = False

            If value = "1" Then

                If Not Me.Checked Then
                    Changed = True
                End If

                Me.Checked = True

                If Changed Then
                    OnPropertyChanged("OneZero")
                End If
            Else

                If Me.Checked Then
                    Changed = True
                End If

                Me.Checked = False

                If Changed Then
                    OnPropertyChanged("OneZero")
                End If

            End If
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler

    Private Sub OnPropertyChanged(ByVal name As String)
        Try

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))

        Catch ex As Exception
        End Try

    End Sub

    Protected Overloads Overrides Sub OnCheckedChanged(ByVal e As EventArgs)
        Try

            OnPropertyChanged("OneZero")

            MyBase.OnCheckedChanged(e)

        Catch ex As Exception
        End Try
    End Sub

End Class