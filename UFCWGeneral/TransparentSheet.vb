Option Infer On

Imports System.Security.Permissions
Imports System.Windows.Forms

<System.Diagnostics.DebuggerStepThrough()>
Public Class TransparentSheet
    Inherits ContainerControl

    Public Sub New()
        'Disable painting the background.
        '        SetStyle(ControlStyles.ContainerControl, True)
        SetStyle(ControlStyles.Opaque, True)
        UpdateStyles()

        'Make sure to set the AutoScaleMode property to None so that the location and size
        'property don't automatically change when placed in a form that has different font than this.
        AutoScaleMode = Windows.Forms.AutoScaleMode.None
        '       ResizeRedraw = True

        'Tab stop on a transparent sheet makes no sense.
        TabStop = False
    End Sub

    Private Const WS_EX_TRANSPARENT As Short = &H20
    Protected Overrides ReadOnly Property CreateParams() As System.Windows.Forms.CreateParams
        <SecurityPermission(SecurityAction.Demand, UnmanagedCode:=True)>
        Get
            Dim cp = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or WS_EX_TRANSPARENT
            Return cp
        End Get
    End Property

End Class

<System.Diagnostics.DebuggerStepThrough()>
Public Class TransparentContainer
    Inherits ContainerControl
    Implements IContainerControl

    Private ActiveCtrl As Control

    Public Property ActiveControl As Control Implements IContainerControl.ActiveControl
        Get
            Return ActiveCtrl
        End Get

        Set(ByVal value As Control)
            ' Make sure the control is a member of the ControlCollection.
            If Me.Controls.Contains(value) Then
                ActiveCtrl = value
            End If
        End Set
    End Property

    Public Function ActivateControl(ByVal active As Control) As Boolean Implements IContainerControl.ActivateControl

        If Me.Controls.Contains(active) Then
            ' Select the control and scroll the control into view if needed.
            active.Select()
            Me.ActiveCtrl = active
            Return True
        End If
        Return False
    End Function

    Public Sub New()
        'Disable painting the background.
        'SetStyle(ControlStyles.Opaque, True)

        'SetStyle(ControlStyles.ContainerControl _
        '        Or ControlStyles.SupportsTransparentBackColor _
        '        Or ControlStyles.Opaque _
        '        Or ControlStyles.DoubleBuffer _
        '        Or ControlStyles.UserPaint _
        '        Or ControlStyles.AllPaintingInWmPaint, True)

        'SetStyle(ControlStyles.ContainerControl _
        '        Or ControlStyles.SupportsTransparentBackColor _
        '        Or ControlStyles.Opaque, True)

        'Me.SetStyle(ControlStyles.UserPaint, True)

        SetStyle(ControlStyles.ContainerControl _
                Or ControlStyles.SupportsTransparentBackColor _
                Or ControlStyles.OptimizedDoubleBuffer _
                Or ControlStyles.UserPaint _
                Or ControlStyles.Selectable _
                Or ControlStyles.AllPaintingInWmPaint, True)


        'Me.SetStyle(ControlStyles.ContainerControl _
        '            Or ControlStyles.ResizeRedraw, True)
        Me.UpdateStyles()

        'Make sure to set the AutoScaleMode property to None so that the location and size
        'property don't automatically change when placed in a form that has different font than this.
        AutoScaleMode = Windows.Forms.AutoScaleMode.None

    End Sub

    Dim _InFocus As Boolean = False
    Public Overloads Function Focus() As Boolean
        Dim Focused As Boolean
        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " (" & CType(Controls(0), Control).Name & "-" & CType(Controls(0), Control).Focused & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If Not _InFocus Then
                _InFocus = True
                Focused = MyBase.Focus
                Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Mid: " & Me.Name & " (" & CType(Controls(0), Control).Name & "-" & CType(Controls(0), Control).Focused & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
            End If

            Return Focused

            'Return CType(Controls(0), Control).Focus()

        Catch ex As Exception
            Throw
        Finally
            _InFocus = False
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " (" & CType(Controls(0), Control).Name & "-" & CType(Controls(0), Control).Focused & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Function

    Protected Overrides Sub OnGotFocus(e As EventArgs)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " (" & CType(Controls(0), Control).Name & "-" & CType(Controls(0), Control).Focused & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            CType(Controls(0), Control).Focus()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " (" & CType(Controls(0), Control).Name & "-" & CType(Controls(0), Control).Focused & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            MyBase.OnResize(e)
            '            Me.Refresh()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Overrides ReadOnly Property Focused As Boolean
        Get
            Return MyBase.Focused
        End Get
    End Property

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Parent.Parent.Name & " " & Me.Parent.Name & " " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            MyBase.OnPaint(e)

            If Me.Visible Then
                Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Mid: " & Me.Parent.Parent.Name & " " & Me.Parent.Name & " " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Parent.Parent.Name & " " & Me.Parent.Name & " " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        End Try
    End Sub

    'Protected Overrides ReadOnly Property CreateParams() As System.Windows.Forms.CreateParams
    '    <SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode:=True)>
    '    Get
    '        Dim cp = MyBase.CreateParams
    '        cp.ExStyle = cp.ExStyle Or WS_EX_TRANSPARENT
    '        Return cp
    '    End Get
    'End Property
End Class

<System.Diagnostics.DebuggerStepThrough()>
Public Class OpaqueContainer
    Inherits ContainerControl

    Public Sub New()

        SetStyle(ControlStyles.ContainerControl _
                Or ControlStyles.DoubleBuffer _
                Or ControlStyles.UserPaint _
                Or ControlStyles.AllPaintingInWmPaint, True)

        Me.UpdateStyles()

    End Sub

    Public Overrides ReadOnly Property Focused As Boolean
        Get
            Me.Invalidate()
            Return MyBase.Focused
        End Get
    End Property
    Public Overrides Function ValidateChildren() As Boolean
        Me.Invalidate()
        Return MyBase.ValidateChildren()
    End Function

    Public Overrides Function ValidateChildren(validationConstraints As ValidationConstraints) As Boolean
        Me.Invalidate()
        Return MyBase.ValidateChildren(validationConstraints)
    End Function

End Class
Public Class ComboBoxContainer
    Inherits ContainerControl
    Implements IContainerControl

    Private ActiveCtrl As Control

    Public Sub New()
    End Sub

    Public Property ActiveControl As Control Implements IContainerControl.ActiveControl
        Get
            Return ActiveCtrl
        End Get

        Set(ByVal value As Control)
            ' Make sure the control is a member of the ControlCollection.
            If Me.Controls.Contains(value) Then
                ActiveCtrl = value
            End If
        End Set
    End Property

    Public Function ActivateControl(ByVal active As Control) As Boolean Implements IContainerControl.ActivateControl

        If Me.Controls.Contains(active) Then
            ' Select the control and scroll the control into view if needed.
            active.Select()
            Me.ActiveCtrl = active
            Return True
        End If
        Return False
    End Function

End Class
