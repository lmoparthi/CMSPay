Option Infer On

Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.Drawing.Design
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Drawing

''' <summary>
''' EnhancedToolTip supports the ToolTipWhenDisabled and SizeOfToolTipWhenDisabled
''' extender properties that can be used to show tooltip messages when the associated
''' control is disabled.
''' </summary>
''' <remarks>
''' EnhancedToolTip does not work with the Form and its derived classes.
''' </remarks>
<ProvideProperty("ToolTipWhenDisabled", GetType(Control))> _
<ProvideProperty("SizeOfToolTipWhenDisabled", GetType(Control))> _
Public Class EnhancedToolTip
    Inherits ToolTip

#Region " Required constructor "
    'This constructor is required for the Windows Forms Designer to instantiate
    'an object of this class with New(Me.components).
    'To verify this, just remove this constructor. Build it and then put the
    'component on a form. Take a look at the Designer.vb file for InitializeComponents(),
    'and search for the line where it instantiates this class.
    Public Sub New(ByVal container As System.ComponentModel.IContainer)
        MyBase.New()
        '    Me.OwnerDraw = True
        'Required for Windows.Forms Class Composition Designer support
        If (container IsNot Nothing) Then
            container.Add(Me)
        End If

    End Sub
#End Region

#Region " ToolTipWhenDisabled extender property support "
    Private m_ToolTipWhenDisabled As New Dictionary(Of Control, String)
    Private m_TransparentSheet As New Dictionary(Of Control, TransparentSheet)

    Public Sub SetToolTipWhenDisabled(ByVal control As Control, ByVal caption As String)
        If control Is Nothing Then
            Throw New ArgumentNullException(NameOf(control))
        End If

        If Not String.IsNullOrEmpty(caption) Then
            m_ToolTipWhenDisabled(control) = caption
            If Not control.Enabled Then
                'When the control is disabled at design time, the EnabledChanged
                'event won't fire. So, on the first Paint event, we should call
                'ShowToolTipWhenDisabled().
                AddHandler control.Paint, AddressOf DisabledControl_Paint
            End If
            AddHandler control.EnabledChanged, AddressOf Control_EnabledChanged
        Else
            m_ToolTipWhenDisabled.Remove(control)
            RemoveHandler control.EnabledChanged, AddressOf Control_EnabledChanged
        End If
    End Sub

    Private Sub DisabledControl_Paint(ByVal sender As Object, ByVal e As EventArgs)
        Dim control As Control = CType(sender, Control)
        ShowToolTipWhenDisabled(control)
        'Immediately remove the handler because we don't need it any longer.
        RemoveHandler control.Paint, AddressOf DisabledControl_Paint
    End Sub

    <Category("Misc")> _
    <Description("Determines the ToolTip shown when the mouse hovers over the disabled control.")> _
    <Localizable(True)> _
    <Editor(GetType(MultilineStringEditor), GetType(UITypeEditor))> _
    <DefaultValue("")> _
    Public Function GetToolTipWhenDisabled(ByVal control As Control) As String
        If control Is Nothing Then
            Throw New ArgumentNullException(NameOf(control))
        End If

        If m_ToolTipWhenDisabled.ContainsKey(control) Then
            Return m_ToolTipWhenDisabled(control)
        Else
            Return ""
        End If
    End Function

    Private Sub Control_EnabledChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim control As Control = CType(sender, Control)
        If control.Enabled Then
            ShowToolTip(control)
        Else
            ShowToolTipWhenDisabled(control)
        End If
    End Sub

    Private Sub ShowToolTip(ByVal control As Control)
        If TypeOf control Is Form Then
            'We don't support ToolTipWhenDisabled for the Form class.
        Else
            TakeOffTransparentSheet(control)
        End If
    End Sub

    Private Sub ShowToolTipWhenDisabled(ByVal control As Control)
        If TypeOf control Is Form Then
            'We don't support ToolTipWhenDisabled for the Form class.
        Else
            If control.Parent.Enabled Then
                PutOnTransparentSheet(control)
            Else
                'If the parent control is disabled, we can't show the
                'ToolTipWhenDisabled. So, do not call PutOnTransparentSheet(),
                'otherwise, Control_EnabledChanged() event on this control
                'will be repeatedly fired because of ts.BringToFront() in
                'PutOnTransparentSheet().
            End If
        End If
    End Sub

    Private Sub PutOnTransparentSheet(ByVal control As Control)
        Dim ts As New TransparentSheet
        ts.Location = control.Location
        If m_SizeOfToolTipWhenDisabled.ContainsKey(control) Then
            ts.Size = m_SizeOfToolTipWhenDisabled(control)
        Else
            ts.Size = control.Size
        End If
        control.Parent.Controls.Add(ts)
        ts.BringToFront()
        m_TransparentSheet(control) = ts
        SetToolTip(ts, m_ToolTipWhenDisabled(control))
    End Sub

    Private Sub TakeOffTransparentSheet(ByVal control As Control)
        If m_TransparentSheet.ContainsKey(control) Then
            Dim ts = m_TransparentSheet(control)
            control.Parent.Controls.Remove(ts)
            SetToolTip(ts, "")
            ts.Dispose()
            m_TransparentSheet.Remove(control)
        End If
    End Sub
#End Region

#Region " Support for the oversized transparent sheet to cover multiple visual controls. "
    Private m_SizeOfToolTipWhenDisabled As New Dictionary(Of Control, Size)

    Public Sub SetSizeOfToolTipWhenDisabled(ByVal control As Control, ByVal value As Size)
        If control Is Nothing Then
            Throw New ArgumentNullException(NameOf(control))
        End If

        If Not value.IsEmpty Then
            m_SizeOfToolTipWhenDisabled(control) = value
        Else
            m_SizeOfToolTipWhenDisabled.Remove(control)
        End If
    End Sub

    <Category("Misc")> _
    <Description("Determines the size of the ToolTip when the control is disabled." & _
                 " Leave it to 0,0, unless you want the ToolTip to pop up over wider" & _
                 " rectangular area than this control.")> _
    <DefaultValue(GetType(Size), "0,0")> _
    Public Function GetSizeOfToolTipWhenDisabled(ByVal control As Control) As Size
        If control Is Nothing Then
            Throw New ArgumentNullException(NameOf(control))
        End If

        If m_SizeOfToolTipWhenDisabled.ContainsKey(control) Then
            Return m_SizeOfToolTipWhenDisabled(control)
        Else
            Return Size.Empty
        End If
    End Function
#End Region

#Region " Comment out this region if you are okay with the same Title/Icon for disabled controls. "
    Private m_SavedToolTipTitle As String
    Public Shadows Property ToolTipTitle() As String
        Get
            Return MyBase.ToolTipTitle
        End Get
        Set(ByVal value As String)
            MyBase.ToolTipTitle = value
            m_SavedToolTipTitle = value
        End Set
    End Property

    Private m_SavedToolTipIcon As ToolTipIcon
    Public Shadows Property ToolTipIcon() As System.Windows.Forms.ToolTipIcon
        Get
            Return MyBase.ToolTipIcon
        End Get
        Set(ByVal value As System.Windows.Forms.ToolTipIcon)
            MyBase.ToolTipIcon = value
            m_SavedToolTipIcon = value
        End Set
    End Property

    Private Sub EnhancedToolTip_Popup(ByVal sender As Object, ByVal e As System.Windows.Forms.PopupEventArgs) Handles Me.Popup
        If TypeOf e.AssociatedControl Is TransparentSheet Then
            MyBase.ToolTipTitle = ""
            MyBase.ToolTipIcon = Windows.Forms.ToolTipIcon.None
        Else
            MyBase.ToolTipTitle = m_SavedToolTipTitle
            MyBase.ToolTipIcon = m_SavedToolTipIcon
        End If
    End Sub

    ' Handles drawing the ToolTip.
    'Private Sub EnhancedToolTip_Draw(ByVal sender As System.Object, ByVal e As DrawToolTipEventArgs) Handles Me.Draw
    '    ' Draw the ToolTip differently depending on which 
    '    ' control this ToolTip is for.
    '    e.DrawBackground()
    '    e.DrawBorder()
    '    ' Customize the tooltip's appearance here
    '    Me.BackColor = Color.LightYellow
    '    e.DrawText()
    'End Sub
#End Region
End Class
