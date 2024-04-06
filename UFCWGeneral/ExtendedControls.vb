Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Windows.Forms.VisualStyles

<System.Diagnostics.DebuggerStepThrough()>
Public Class ProgressStatusBar : Inherits StatusBar

    Private _ProgressBarPanel As Integer = -1

    Sub New()
        ProgressBar.Hide()
        Me.Controls.Add(ProgressBar)
    End Sub

    Public Property SetProgressBar() As Integer
        Get
            Return _ProgressBarPanel
        End Get
        Set(ByVal Value As Integer)
            _ProgressBarPanel = Value
            Me.Panels(_ProgressBarPanel).Style = StatusBarPanelStyle.OwnerDraw
        End Set
    End Property

    Public Property ProgressBar() As New ProgressBar

    Private Sub Reposition(ByVal sender As Object, ByVal sbdevent As System.Windows.Forms.StatusBarDrawItemEventArgs) Handles MyBase.DrawItem
        ProgressBar.Location = New Point(sbdevent.Bounds.X, sbdevent.Bounds.Y)
        ProgressBar.Size = New Size(sbdevent.Bounds.Width, sbdevent.Bounds.Height)
        ProgressBar.Show()
    End Sub
End Class

<System.Diagnostics.DebuggerStepThrough()>
Public Class ExComboBox
    Inherits ComboBox

    Private _UnSelectable As Boolean = True
    Private _Pnl As New DblPanel()

    Private Const WM_MOUSEWHEEL As Integer = 256
    Private Const WM_LBUTTONDOWN As Integer = &H201
    Private Const WM_LBUTTONDBLCLK As Integer = &H203
    Private Const VK_SHIFT As Integer = &H10

    Private textFlags As TextFormatFlags = TextFormatFlags.Default
    Private textBorder As New Rectangle()
    Private textRectangle As New Rectangle()

    Private _SelectionChangeCommittedActivated As Boolean
    Private _SelectedIndexChangedActivated As Boolean
    Private _OnMouseClickActivated As Boolean

    Public Sub New()

        '_Pnl.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))

        _Pnl.Width = 17
        _Pnl.Height = Me.Height - 5
        _Pnl.Left = Me.Width - 18
        _Pnl.Top = 4

        Me.Controls.Add(_Pnl)

        _Pnl.BringToFront()
        _Pnl.Visible = False

    End Sub

    Private Sub ClearFlags()

        _SelectionChangeCommittedActivated = False
        _SelectedIndexChangedActivated = False
        _OnMouseClickActivated = False

    End Sub
    Protected Overrides Sub RefreshItem(index As Integer)

        MyBase.RefreshItem(index)

        ClearFlags()
    End Sub

    Protected Overrides Sub RefreshItems()

        MyBase.RefreshItems()

        ClearFlags()
    End Sub

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        MyBase.OnMouseClick(e)
        _OnMouseClickActivated = True
    End Sub

    Protected Overrides Sub OnDropDownClosed(e As EventArgs)

        MyBase.OnDropDownClosed(e)

        If Me.Parent IsNot Nothing AndAlso TypeOf (Me.Parent) Is TransparentContainer Then

            If _SelectedIndexChangedActivated AndAlso Not _OnMouseClickActivated Then

                CType(Me.Parent, TransparentContainer).ValidateChildren() 'this will trigger validation of the cmbbox triggering write of value to DS

                ClearFlags()

            End If
        End If
    End Sub
    Protected Overrides Sub OnDropDown(e As EventArgs)

        MyBase.OnDropDown(e)

        ClearFlags()
    End Sub
    Protected Overrides Sub OnValidating(e As CancelEventArgs)

        MyBase.OnValidating(e)

        ClearFlags()
    End Sub

    Protected Overrides Sub OnSelectedIndexChanged(e As EventArgs)

        MyBase.OnSelectedIndexChanged(e)

        If Me.Parent IsNot Nothing AndAlso TypeOf (Me.Parent) Is TransparentContainer Then

            _OnMouseClickActivated = False

            If Focused AndAlso SelectedIndex > -1 Then _SelectedIndexChangedActivated = True

            If _SelectionChangeCommittedActivated AndAlso _SelectedIndexChangedActivated Then

                CType(Me.Parent, TransparentContainer).ValidateChildren() 'this will trigger validation of the cmbbox triggering write of value to DS

                ClearFlags()
            End If
        End If

    End Sub

    Protected Overrides Sub OnSelectionChangeCommitted(e As EventArgs)

        MyBase.OnSelectionChangeCommitted(e)

        If Me.Parent IsNot Nothing AndAlso TypeOf (Me.Parent) Is TransparentContainer Then
            If SelectedIndex > -1 Then _SelectionChangeCommittedActivated = True

            If _SelectionChangeCommittedActivated AndAlso _SelectedIndexChangedActivated Then

                CType(Me.Parent, TransparentContainer).ValidateChildren() 'this will trigger validation of the cmbbox triggering write of value to DS

                ClearFlags()
            End If
        End If

    End Sub


    Protected Overrides Sub OnKeyPress(ByVal e As KeyPressEventArgs)
        If _UnSelectable Then
            e.Handled = True
        Else
            MyBase.OnKeyPress(e)
        End If
    End Sub

    Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
        If _UnSelectable Then
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
        _Pnl.Left = Me.Width - 18
    End Sub

    Public Property [ReadOnly]() As Boolean
        Get
            Return _UnSelectable
        End Get
        Set(ByVal value As Boolean)

            _UnSelectable = value

            MakeUnselectable(_UnSelectable)

        End Set

    End Property

    Private Sub MakeUnselectable(ByVal unselectable As Boolean)

        Try
            If _Pnl.Parent IsNot Nothing Then

                If unselectable AndAlso Me.DropDownStyle <> ComboBoxStyle.Simple Then
                    _Pnl.Visible = True

                Else
                    _Pnl.Visible = False
                End If
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        If _UnSelectable Then
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

    <System.Diagnostics.DebuggerStepThrough()>
    Protected Overrides Sub WndProc(ByRef m As Message)

        If _UnSelectable Then
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
            _Pnl.Visible = False
        Else
            If _UnSelectable Then
                _Pnl.Visible = True
            Else
                _Pnl.Visible = False
            End If
        End If

        MyBase.OnDropDownStyleChanged(e)

    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Protected Class DblPanel
        Inherits Panel

        Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

            Try

                If Me.Visible Then
                    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Parent.Parent.Name & " " & Me.Parent.Name & " " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

                    ComboBoxRenderer.DrawDropDownButton(e.Graphics, e.ClipRectangle, System.Windows.Forms.VisualStyles.ComboBoxState.Disabled)

                    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Parent.Parent.Name & " " & Me.Parent.Name & " " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

                End If

            Catch ex As Exception
                Throw
            End Try
        End Sub
    End Class

End Class

<System.Diagnostics.DebuggerStepThrough()>
Public Class CustomCheckBox
    Inherits Control

    Private textRectangleValue As New Rectangle()
    Private clickedLocationValue As New Point()
    Private clicked As Boolean = False
    Private state As CheckBoxState = CheckBoxState.UncheckedNormal

    Public Sub New()
        With Me
            .Location = New Point(50, 50)
            .Size = New Size(100, 20)
            .Text = "Click here"
            .Font = SystemFonts.IconTitleFont
        End With
    End Sub

    ' Calculate the text bounds, exluding the check box.
    Public ReadOnly Property TextRectangle() As Rectangle
        Get
            Using g As Graphics = Me.CreateGraphics()
                With textRectangleValue
                    .X = Me.ClientRectangle.X + CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal).Width
                    .Y = Me.ClientRectangle.Y
                    .Width = Me.ClientRectangle.Width - CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal).Width
                    .Height = Me.ClientRectangle.Height
                End With
            End Using
            Return textRectangleValue
        End Get
    End Property

    ' Draw the check box in the current state.
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        CheckBoxRenderer.DrawCheckBox(e.Graphics, Me.ClientRectangle.Location, TextRectangle, Me.Text, Me.Font, TextFormatFlags.HorizontalCenter, clicked, state)
    End Sub

    ' Draw the check box in the checked or unchecked state, alternately.
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If Not clicked Then
            With Me
                .clicked = True
                .Text = "Clicked!"
                .state = CheckBoxState.CheckedPressed
            End With
            Invalidate()
        Else
            With Me
                .clicked = False
                .Text = "Click here"
                .state = CheckBoxState.UncheckedNormal
            End With
            Invalidate()
        End If
    End Sub

    ' Draw the check box in the hot state. 
    Protected Overrides Sub OnMouseHover(ByVal e As EventArgs)
        MyBase.OnMouseHover(e)
        If clicked Then
            state = CheckBoxState.CheckedHot
        Else
            state = CheckBoxState.UncheckedHot
        End If
        Invalidate()
    End Sub

    ' Draw the check box in the hot state. 
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        Me.OnMouseHover(e)
    End Sub

    ' Draw the check box in the unpressed state.
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        If clicked Then
            state = CheckBoxState.CheckedNormal
        Else
            state = CheckBoxState.UncheckedNormal
        End If
        Invalidate()
    End Sub

End Class

<System.Diagnostics.DebuggerStepThrough()>
Public Class ExTextBox
    Inherits TextBox

    Private _AlreadyFocused As Boolean

    Protected Overrides Sub OnLeave(ByVal e As EventArgs)
        MyBase.OnLeave(e)

        _AlreadyFocused = False

    End Sub

    Protected Overrides Sub OnGotFocus(ByVal e As EventArgs)
        MyBase.OnGotFocus(e)

        ' Select all text only if the mouse isn't down.
        ' This makes tabbing to the textbox give focus.
        If MouseButtons = MouseButtons.None Then

            Me.SelectAll()
            _AlreadyFocused = True

        End If

    End Sub

    Protected Overrides Sub OnMouseUp(ByVal mevent As MouseEventArgs)
        MyBase.OnMouseUp(mevent)

        ' Web browsers like Google Chrome select the text on mouse up.
        ' They only do it if the textbox isn't already focused,
        ' and if the user hasn't selected all text.
        If Not _AlreadyFocused AndAlso Me.SelectionLength = 0 Then

            _AlreadyFocused = True
            Me.SelectAll()

        End If

    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        ' Trap WM_PASTE:
        If m.Msg = &H302 AndAlso Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) Then

            If Me.Name.Contains("SSN") Then

                If Not UFCWGeneral.ClipBoardSSNCleaner() Then Beep()

            Else
                If Not UFCWGeneral.ClipboardNumberTrimmer() Then Beep()

            End If

            Me.SelectedText = Clipboard.GetText()

            Return
        End If
        MyBase.WndProc(m)
    End Sub

End Class

<System.Diagnostics.DebuggerStepThrough()>
Public Class ExCheckBox
    Inherits CheckBox

    Private _Unselectable As Boolean = False
    Private _Pnl As New DblPanel()
    Private Const WM_MOUSEWHEEL As Integer = 256
    Private Const WM_LBUTTONDOWN As Integer = &H201
    Private Const WM_LBUTTONDBLCLK As Integer = &H203
    Private Const VK_SHIFT As Integer = &H10

    Public Sub New()

        'Select Case Me.CheckAlign
        '    Case Drawing.ContentAlignment.BottomLeft
        '        _Pnl.BackColor = Color.Beige
        '    Case Drawing.ContentAlignment.BottomCenter
        '        _Pnl.BackColor = Color.Cyan
        '    Case Drawing.ContentAlignment.BottomRight
        '        _Pnl.BackColor = Color.DarkBlue
        '    Case Drawing.ContentAlignment.MiddleLeft
        '        _Pnl.BackColor = Color.DeepPink
        '    Case Drawing.ContentAlignment.MiddleCenter
        '        _Pnl.BackColor = Color.Firebrick
        '    Case Drawing.ContentAlignment.MiddleRight
        '        _Pnl.BackColor = Color.Gainsboro
        '    Case Drawing.ContentAlignment.TopLeft
        '        _Pnl.BackColor = Color.Honeydew
        '    Case Drawing.ContentAlignment.TopCenter
        '        _Pnl.BackColor = Color.Indigo
        '    Case Drawing.ContentAlignment.TopRight
        '        _Pnl.BackColor = Color.Khaki
        'End Select

        'Dim gs As Drawing.Size = CheckBoxRenderer.GetGlyphSize(Me.Graphics, CType(Me.Parent, ExCheckBox).BoxState)
        Dim ms As Size = Me.SizeFromClientSize(New Size(Me.Width, Me.Height))

        Me.Controls.Add(_Pnl)

        Dim CheckBoxRectangle As Rectangle = _Pnl.CheckBoxRectangle

        _Pnl.Height = CheckBoxRectangle.Height
        _Pnl.Width = CheckBoxRectangle.Width
        _Pnl.Left = CheckBoxRectangle.X
        _Pnl.Top = CheckBoxRectangle.Y

        _Pnl.BringToFront()
        _Pnl.Visible = False

    End Sub

    Protected Overrides Sub OnKeyPress(ByVal e As KeyPressEventArgs)
        If _Unselectable = True Then
            e.Handled = True
        Else
            MyBase.OnKeyPress(e)
        End If
    End Sub

    Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
        If _Unselectable = True Then
            e.Handled = True
        Else
            MyBase.OnKeyDown(e)
        End If
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)

        Dim CheckBoxRectangle As Rectangle = _Pnl.CheckBoxRectangle

        _Pnl.Height = CheckBoxRectangle.Height
        _Pnl.Width = CheckBoxRectangle.Width
        _Pnl.Left = CheckBoxRectangle.X
        _Pnl.Top = CheckBoxRectangle.Y

        MyBase.OnResize(e)
    End Sub

    Public Property [ReadOnly]() As Boolean
        Get
            Return _Unselectable
        End Get
        Set(ByVal value As Boolean)
            _Unselectable = value
            MakeUnselectable(_Unselectable)
        End Set
    End Property

    <Browsable(True), System.ComponentModel.Description("Specifies Visual State of Checkbox")>
    Public Property BoxState As CheckBoxState
    Public Property ClickState As Boolean

    Private Sub MakeUnselectable(ByVal Unselectable As Boolean)

        Try
            If _Pnl.Parent IsNot Nothing Then
                If Unselectable = True Then
                    _Pnl.Visible = True
                Else
                    _Pnl.Visible = False
                End If
                _Pnl.Parent.BackColor = SystemColors.Control
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        If _Unselectable = True Then
            If keyData = Keys.Up OrElse keyData = Keys.Down OrElse keyData = Keys.PageUp OrElse keyData = Keys.PageDown Then
                Return True
            End If
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If Not ClickState Then
            With Me
                .ClickState = True
                .BoxState = CheckBoxState.CheckedPressed
            End With
            Invalidate()
        Else
            With Me
                .ClickState = False
                .BoxState = CheckBoxState.UncheckedNormal
            End With
            Invalidate()
        End If
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Protected Overrides Sub WndProc(ByRef m As Message)

        If Me._Unselectable = True Then
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

    <System.Diagnostics.DebuggerStepThrough()>
    Protected Class DblPanel
        Inherits Panel

        Private _TextRectangleValue As New Rectangle()
        Private _CheckBoxRectangleValue As New Rectangle()

        Public Property State As CheckBoxState
        Public Property Clicked As Boolean

        Public Sub New()

            SetStyle(ControlStyles.ContainerControl _
                Or ControlStyles.SupportsTransparentBackColor _
                Or ControlStyles.OptimizedDoubleBuffer _
                Or ControlStyles.UserPaint _
                Or ControlStyles.Selectable _
                Or ControlStyles.AllPaintingInWmPaint, True)


            Me.UpdateStyles()

        End Sub

        Public ReadOnly Property TextRectangle() As Rectangle
            Get
                Using g As Graphics = Me.CreateGraphics()
                    With _TextRectangleValue
                        .X = CType(Me.Parent, ExCheckBox).ClientRectangle.X + CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal).Width
                        .Y = CType(Me.Parent, ExCheckBox).ClientRectangle.Y
                        .Width = CType(Me.Parent, ExCheckBox).ClientRectangle.Width - CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal).Width
                        .Height = CType(Me.Parent, ExCheckBox).ClientRectangle.Height
                    End With
                End Using
                Return _TextRectangleValue
            End Get
        End Property

        Public ReadOnly Property CheckBoxRectangle() As Rectangle
            Get
                Using g As Graphics = Me.CreateGraphics()
                    With _CheckBoxRectangleValue

                        .Width = CheckBoxRenderer.GetGlyphSize(g, CType(Me.Parent, ExCheckBox).BoxState).Width
                        .Height = CType(Me.Parent, ExCheckBox).ClientRectangle.Height

                        Dim gs As Drawing.Size = CheckBoxRenderer.GetGlyphSize(g, CType(Me.Parent, ExCheckBox).BoxState)

                        Select Case CType(Me.Parent, ExCheckBox).CheckAlign
                            Case Drawing.ContentAlignment.TopCenter, Drawing.ContentAlignment.MiddleCenter, Drawing.ContentAlignment.BottomCenter
                                .X = CType(Me.Parent, ExCheckBox).Width - (.Width \ 2)
                                .Y = CType(Me.Parent, ExCheckBox).ClientRectangle.Y
                            Case Drawing.ContentAlignment.TopRight, Drawing.ContentAlignment.MiddleRight, Drawing.ContentAlignment.BottomRight
                                .X = CType(Me.Parent, ExCheckBox).Width - (.Width + 1)
                                .Y = CType(Me.Parent, ExCheckBox).ClientRectangle.Y
                            Case Drawing.ContentAlignment.TopLeft, Drawing.ContentAlignment.MiddleLeft, Drawing.ContentAlignment.BottomLeft
                                .X = CType(Me.Parent, ExCheckBox).ClientRectangle.X
                                .Y = CType(Me.Parent, ExCheckBox).ClientRectangle.Y
                        End Select

                    End With
                End Using
                Return _CheckBoxRectangleValue
            End Get
        End Property

        Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
            Try

                MyBase.OnPaint(e)

                If Me.Visible = False Then
                Else
                    'Select Case CType(Me.Parent, ExCheckBox).CheckAlign
                    '    Case Drawing.ContentAlignment.BottomLeft
                    '        Me.BackColor = Color.Beige
                    '    Case Drawing.ContentAlignment.BottomCenter
                    '        Me.BackColor = Color.Cyan
                    '    Case Drawing.ContentAlignment.BottomRight
                    '        Me.BackColor = Color.DarkBlue
                    '    Case Drawing.ContentAlignment.MiddleLeft
                    '        Me.BackColor = Color.DeepPink
                    '    Case Drawing.ContentAlignment.MiddleCenter
                    '        Me.BackColor = Color.Firebrick
                    '    Case Drawing.ContentAlignment.MiddleRight
                    '        Me.BackColor = Color.Gainsboro
                    '    Case Drawing.ContentAlignment.TopLeft
                    '        Me.BackColor = Color.Honeydew
                    '    Case Drawing.ContentAlignment.TopCenter
                    '        Me.BackColor = Color.Indigo
                    '    Case Drawing.ContentAlignment.TopRight
                    '        Me.BackColor = Color.Khaki
                    'End Select

                    Me.BackColor = Color.Transparent

                    Dim gs As Drawing.Size = CheckBoxRenderer.GetGlyphSize(e.Graphics, CType(Me.Parent, ExCheckBox).BoxState)
                    'gs.Height.ToString & "/" & gs.Width.ToString & CType(Me.Parent, ExCheckBox).Text

                    Dim ms As Size = Me.SizeFromClientSize(New Size(CType(Me.Parent, ExCheckBox).Width, CType(Me.Parent, ExCheckBox).Height))
                    Dim pnlms As Size = Me.SizeFromClientSize(New Size(Me.Width, Me.Height))

                    Dim CheckBoxRectangle As Rectangle = CheckBoxRectangle

                    CheckBoxRenderer.DrawCheckBox(e.Graphics, CType(Me.Parent, ExCheckBox).Bounds.Location, TextRectangle, CType(Me.Parent, ExCheckBox).Text, CType(Me.Parent, ExCheckBox).Font, TextFormatFlags.VerticalCenter, CType(Me.Parent, ExCheckBox).ClickState, CType(Me.Parent, ExCheckBox).BoxState)
                    CheckBoxRenderer.DrawParentBackground(e.Graphics, TextRectangle, Me)
                End If

            Catch ex As Exception
                Throw
            End Try
        End Sub
    End Class

End Class
