Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms

Namespace DataGridIcons
    Public Class DataGridIconOnlyColumn
        Inherits DataGridTextBoxColumn

        'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

        Private WithEvents Pics As ImageList
        Private _KeyGrid As DataGrid

        Public Sub New(ByVal icons As ImageList, ByVal dg As DataGrid)
            MyBase.New()

            Pics = Icons
            _KeyGrid = dg
        End Sub

        Protected Overloads Overrides Sub Paint(ByVal g As Graphics, ByVal bounds As Rectangle, ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal backBrush As Brush, ByVal foreBrush As Brush, ByVal alignToRight As Boolean)
            Try
                Dim IconIndx As Integer
                Dim ColNum As Integer = _KeyGrid.TableStyles(0).GridColumnStyles.IndexOf(_KeyGrid.TableStyles(0).GridColumnStyles("Code"))

                'erase background
                g.FillRectangle(backBrush, bounds)

                Select Case Asc(_KeyGrid.Item(rowNum, ColNum)) - &H40
                    Case Is = 1 'Normal
                        IconIndx = 9
                    Case Is = 2 'Audit
                        IconIndx = 7
                    Case Is = 4 'Route
                        IconIndx = 1
                    Case Is = 8 'Error
                        IconIndx = 5
                    Case Is = 16 'System
                        IconIndx = 3
                    Case Is = 32 'User
                        IconIndx = 2
                    Case Is = 64 'Letter
                        IconIndx = 13
                    Case Is = 128 'Poke
                        IconIndx = 11
                End Select

                'draw pic
                g.DrawImage(Me.Pics.Images(IconIndx), bounds)
            Catch ex As System.Exception
                MsgBox(ex.Message & ex.StackTrace)
            End Try
        End Sub

        Protected Overloads Overrides Sub Edit(ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal readOnly1 As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)

            Try
                'do not allow the unbound cell to become active
                If (Me.MappingName Is "Icon") Then
                    Return
                End If
                MyBase.Edit(source, rowNum, bounds, readOnly1, instantText, cellIsVisible)
            Catch ex As System.Exception
                MsgBox(ex.Message & ex.StackTrace)
            End Try
        End Sub
    End Class
End Namespace