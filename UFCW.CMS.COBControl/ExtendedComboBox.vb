Imports System.Runtime.InteropServices

Public Class ExtendedComboBox
    Inherits System.Windows.Forms.ComboBox

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

#Region "-- Declarations --"
    Private _ReadOnly As Boolean
    Private _DroppedDown As Boolean
    Private _SelectedIndex As Integer = -1
    Private _DropDownStyle As ComboBoxStyle = ComboBoxStyle.DropDown

    Private Const EM_SETREADONLY As Int32 = &HCF
    Private Const EM_EMPTYUNDOBUFFER As Int32 = &HCD
    Private Const CB_SHOWDROPDOWN As Int32 = &H14F
    Private Const GW_CHILD As Int32 = 5
#End Region     ' -- Declarations --

#Region "-- Properties --"
    <System.ComponentModel.Browsable(True), System.ComponentModel.DefaultValue(False)> _
    Public Property [ReadOnly]() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            ' In design mode we don't want setting the read only property
            ' to alter the dropdown style.
            If Not Me.DesignMode Then
                ' If the DropDownStyle is anything other than DropDown then setting
                ' ReadOnly to true will have no affect. Therefore we'll force the style
                ' to DropDown as it goes to ReadOnly and restore it when it's turned off.
                If Value Then
                    ' If the value is changing then we want to save the dropdown style.
                    ' In case the value gets set to true more than once we don't want
                    ' to loose the saved drop down style.
                    If _ReadOnly <> Value Then
                        _DropDownStyle = MyBase.DropDownStyle
                        MyBase.DropDownStyle = ComboBoxStyle.DropDown
                    End If
                Else    ' restore the saved drop down style
                    MyBase.DropDownStyle = _DropDownStyle
                End If
            End If
            _ReadOnly = Value
            ' If readonly then don't let the user tab to the field
            MyBase.TabStop = Not Value
            ' Setting TabStop to false causes the text in the box to be selected if it matches
            ' an entry in the list. Setting selection length to zero removes the selection.
            MyBase.SelectionLength = 0
            ' Send the textbox portion of the combo the readonly message.
            ' It will change the color and behavior.
            NativeMethods.SendMessage(NativeMethods.GetWindow(Me.Handle, GW_CHILD), EM_SETREADONLY, CType(Value, IntPtr), CType(0, IntPtr))
            ' If text was typed or pasted into the textbox, the context menu will
            ' have the undo activated. When the text box is in the readonly state
            ' the undo will still be active from the right click context menu
            ' allowing the user to restore the previous value. This sendmessage
            ' will clear the undo buffer which will clear the undo.
            NativeMethods.SendMessage(NativeMethods.GetWindow(Me.Handle, GW_CHILD), EM_EMPTYUNDOBUFFER, CType(Value, IntPtr), CType(0, IntPtr))
            ' the dropdown may have been dropped before the readonly is set
            _DroppedDown = False
            Me.Refresh()
        End Set
    End Property
    '
    ' Saving and returning a local copy of the selected index keeps a
    ' changed value from being returned when the control is in the
    ' readonly state. The OnSelectedIndexChanged event captures the
    ' index value that is returned here. Must be shadows or else the
    ' value passed won't cause the OnSelectedIndexChanged method to fire
    ' and the text value to be displayed, won't.
    '
    Public Shadows Property SelectedIndex() As Integer
        Get
            Return _SelectedIndex
        End Get
        Set(ByVal value As Integer)
            _SelectedIndex = Value
            MyBase.SelectedIndex = Value
            If Value = -1 Then ' Set it twice to work around databound bug KB327244
                _SelectedIndex = Value
                MyBase.SelectedIndex = Value
            End If
        End Set
    End Property
#End Region     ' -- Properties --

#Region "-- Overrides --"
    '
    ' Intercepting message 273 when readonly and the listbox is dropped
    ' keeps the user from selecting an item in the list and having it update
    ' the text value of the combo as well as firing the associated changed events.
    ' Since we intercept a windows message we will have to manually bring up
    ' the listbox.
    '
    ' msg 305 (0x131)   =  an item was clicked from the dropdown list
    ' msg 273 (0x111)   = (WM_COMMAND) follows dropdown list click and all other actions?
    ' msg 8465 (0x2111) = (WM_REFLECT + WM_COMMAND) subsequent command after the 273
    '
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        ' Cannot use me.DroppedDown, it causes a System.StackOverflowException.
        ' Asking for it's value must produce windows messages for the combobox
        ' and thus create a recursive loop.
        If _ReadOnly AndAlso _DroppedDown Then
            If m.Msg = 273 Then
                _DroppedDown = False
                ' bring up the dropdown
                NativeMethods.SendMessage(Me.Handle, CB_SHOWDROPDOWN, CType(False, IntPtr), CType(0, IntPtr))
                Exit Sub
            End If
        End If

        Try

            MyBase.WndProc(m)

        Catch ex As Exception

        End Try
    End Sub
    '
    ' This event will not fire when msg 273 is intercepted in WndProc. When in the
    ' readonly state clicking on an item in the listbox appears to have no effect. It
    ' does in fact change the value of MyBase.SelectedIndex. Saving the last good index
    ' value locally allows the overriden Index property to supply the proper index value.
    '
    Protected Overrides Sub OnSelectedIndexChanged(ByVal e As System.EventArgs)
        _SelectedIndex = MyBase.SelectedIndex
        MyBase.OnSelectedIndexChanged(e)
    End Sub
    '
    ' We must manually track dropped state. Asking the control if it's
    ' dropped from within WndProc will cause a System.StackOverflowException.
    '
    Protected Overrides Sub OnDropDown(ByVal e As System.EventArgs)
        _DroppedDown = True
        MyBase.OnDropDown(e)
    End Sub
    '
    ' The up and down arrow keys cause the combobox to change selection to the next
    ' or previous in the list. The page up and page down keys change the selection
    ' by one page at a time as defined by the size of the dropdown list. Setting
    ' e.Handled to true if any of these keys is pressed stops the selection change
    ' when readonly. The alt down arrow combination is allowed since it drops the listbox.
    '
    Protected Overrides Sub OnKeyDown(ByVal e As System.Windows.Forms.KeyEventArgs)
        If _ReadOnly Then
            If e.KeyCode = Keys.Up OrElse e.KeyCode = Keys.PageUp OrElse _
              e.KeyCode = Keys.PageDown OrElse _
             (e.KeyCode = Keys.Down And ((Control.ModifierKeys And Keys.Alt) <> Keys.Alt)) Then
                e.Handled = True
            End If
        End If
        MyBase.OnKeyDown(e)
    End Sub
    '
    ' The combobox default behavior when pressing F4 is to drop the listbox.
    ' If F4 is immediately pressed a second time the OnSelectionChangeCommitted
    ' event fires regardless of whether a change has been made or not. When
    ' readonly we don't want a change event to fire. This code will stop it.
    '
    Protected Overrides Sub OnSelectionChangeCommitted(ByVal e As System.EventArgs)
        If Not _ReadOnly Then
            MyBase.OnSelectionChangeCommitted(e)
        End If
    End Sub
#End Region     ' -- Overrides --

End Class

Friend NotInheritable Class NativeMethods
    Private Sub New()
    End Sub

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Friend Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function


    <DllImport("user32", CharSet:=CharSet.Auto, SetLastError:=True, ExactSpelling:=True)> _
    Friend Shared Function GetWindow(ByVal hwnd As IntPtr, ByVal uCmd As Integer) As IntPtr
    End Function

End Class
