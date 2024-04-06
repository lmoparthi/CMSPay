' Copyright (c) 2005 Claudio Grazioli, http://www.grazioli.ch
'
' This code is free software; you can redistribute it and/or modify it.
' However, this header must remain intact and unchanged.  Additional
' information may be appended after this header.  Publications based on
' this code must also include an appropriate reference.
' 
' This code is distributed in the hope that it will be useful, but 
' WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
' or FITNESS FOR A PARTICULAR PURPOSE.
'
' Thanx for contributions to this ReadOnlyComboBox to Alexandre Cunha, Brasil.


Imports System.Drawing
Imports System.Windows.Forms

Public Class ExComboBox
    Inherits ComboBox

    Private m_Unselectable As Boolean = False
    Private pnl As New DblPanel()
    Private Const WM_MOUSEWHEEL As Integer = 256
    Private Const WM_LBUTTONDOWN As Integer = &H201
    Private Const WM_LBUTTONDBLCLK As Integer = &H203
    Private Const VK_SHIFT As Integer = &H10

    Private textFlags As TextFormatFlags = TextFormatFlags.Default
    Private textBorder As New Rectangle()
    Private textRectangle As New Rectangle()

    Public Sub New()

        pnl.Width = 17
        pnl.Height = Me.Height - 2
        pnl.Left = Me.Width - 18
        pnl.Top = 1
        Me.Controls.Add(pnl)
        pnl.BringToFront()
        pnl.Visible = False

    End Sub

    Protected Overrides Sub OnKeyPress(ByVal e As KeyPressEventArgs)
        If m_Unselectable = True Then
            e.Handled = True
        Else
            MyBase.OnKeyPress(e)
        End If
    End Sub

    Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
        If m_Unselectable = True Then
            If CInt(e.KeyData) = 131139 Then
                If Me.SelectedText IsNot Nothing Then
                    Clipboard.SetText(Me.SelectedText)
                End If
            End If
            e.Handled = True
        Else
            MyBase.OnKeyDown(e)
        End If
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        pnl.Left = Me.Width - 18
    End Sub

    Public Property [ReadOnly]() As Boolean
        Get
            Return m_Unselectable
        End Get
        Set(ByVal value As Boolean)
            m_Unselectable = value
            MakeUnselectable(m_Unselectable)
        End Set
    End Property

    Private Sub MakeUnselectable(ByVal Unselectable As Boolean)

        Try
            If pnl.Parent IsNot Nothing Then
                If m_Unselectable = True AndAlso Me.DropDownStyle <> ComboBoxStyle.Simple Then
                    pnl.Visible = True
                    pnl.Parent.BackColor = SystemColors.Control
                Else
                    pnl.Visible = False
                    pnl.Parent.BackColor = SystemColors.Window
                End If
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        If m_Unselectable = True Then
            If Me.DropDownStyle = ComboBoxStyle.DropDownList Then
                If keyData <> Keys.Tab Then
                    Return True
                End If
            Else
                If keyData = Keys.Up OrElse keyData = Keys.Down OrElse keyData = Keys.PageUp OrElse keyData = Keys.PageDown Then
                    Return True
                End If
            End If
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        If Me.m_Unselectable = True Then
            If m.Msg = WM_MOUSEWHEEL OrElse m.Msg = WM_LBUTTONDBLCLK Then
                Return
            End If
            If m.Msg = WM_LBUTTONDOWN Then
                Me.Focus()
                Return
            End If
        End If

        MyBase.WndProc(m)
    End Sub

    Protected Overrides Sub OnDropDownStyleChanged(ByVal e As EventArgs)
        If Me.DropDownStyle = ComboBoxStyle.Simple Then
            pnl.Visible = False
        Else
            If m_Unselectable = True Then
                pnl.Visible = True
            Else
                pnl.Visible = False
            End If
        End If

        MyBase.OnDropDownStyleChanged(e)
    End Sub

    Protected Class DblPanel
        Inherits Panel

        Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

            MyBase.OnPaint(e)

            If Me.Visible = False Then
            Else
                ComboBoxRenderer.DrawDropDownButton(e.Graphics, e.ClipRectangle, System.Windows.Forms.VisualStyles.ComboBoxState.Disabled)

                'Dim Pen As Pen = New Pen(Color.DarkGray)
                'Dim penBR As Pen = New Pen(Color.LightGray)
                'Dim penArrow As Pen = New Pen(Color.LightGray)
                'penArrow.Width = 2
                'penArrow.EndCap = LineCap.Square

                'Dim g As Graphics = e.Graphics
                'Dim lgb As LinearGradientBrush = New LinearGradientBrush(New Point(0, 0), New Point(0, Me.Height + 1), Color.LightGray, Color.Gray)

                'g.FillRectangle(lgb, New Rectangle(New Point(0, 0), Me.Size))
                'g.DrawLine(penBR, New Point(0, 0), New Point(Me.Width - 1, 0))
                'g.DrawLine(penBR, New Point(Me.Width - 1, 0), New Point(Me.Width - 1, Me.Height - 1))
                'g.DrawLine(penBR, New Point(0, Me.Height - 1), New Point(Me.Width - 1, Me.Height - 1))
                'g.DrawLine(penBR, New Point(0, 0), New Point(0, Me.Height - 1))
                'g.DrawLine(Pen, New Point(1, 0), New Point(Me.Width - 2, 0))
                'g.DrawLine(Pen, New Point(Me.Width - 1, 1), New Point(Me.Width - 1, Me.Height - 2))
                'g.DrawLine(Pen, New Point(1, Me.Height - 1), New Point(Me.Width - 2, Me.Height - 1))
                'g.DrawLine(Pen, New Point(0, 1), New Point(0, Me.Height - 2))

                'g.DrawLine(penArrow, New Point(4, 7), New Point(8, 11))
                'g.DrawLine(penArrow, New Point(8, 11), New Point(11, 8))

                'Pen.Dispose()
                'penBR.Dispose()
                'penArrow.Dispose()
                'g = Nothing
                'lgb.Dispose()
            End If
        End Sub
    End Class

End Class
