Imports System.Windows.Forms

Public Class ComboboxEx
    Inherits ComboBox

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

#Region "Private Backing Fields"
    ''' <summary>
    ''' Private backing field for the shadowed enabled property
    ''' </summary>
    Private _ReadOnly As Boolean = True

    ''' <summary>
    ''' BackColor of control when it is enabled
    ''' </summary>
    Private _EnabledBackColor As Color = MyBase.BackColor

    ''' <summary>
    ''' BackColor of a control when it is disabled
    ''' </summary>
    ''' <remarks></remarks>
    Private _DisabledBackColor As Color = Color.LightGray
#End Region

#Region "Public Exposed Fields"

    ''' <summary>
    ''' Gets or Sets the value indicating if this control is enabled
    ''' </summary>
    ''' <value>Boolean</value>
    ''' <returns>True of control is enabled, otherwise false</returns>
    ''' <remarks>This property shadows the ComboBox base class enabled property</remarks>
    Public Shadows Property [ReadOnly]() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            If _ReadOnly <> Value Then
                _ReadOnly = Value
                OnEnabledChanged(New EventArgs)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the BackColor of the control when it is disabled
    ''' </summary>
    ''' <value>Color Structure</value>
    Public Property DisabledBackColor() As Color
        Get
            Return _DisabledBackColor
        End Get
        Set(ByVal value As Color)
            _DisabledBackColor = value
            If Not _ReadOnly Then
                MyBase.BackColor = _DisabledBackColor
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or Sets the BackColor of the control when it is enabled
    ''' </summary>
    ''' <value>Color Structure</value>
    ''' <remarks>Shadows the base class BackColor property</remarks>
    Public Shadows Property BackColor() As Color
        Get
            Return _EnabledBackColor
        End Get
        Set(ByVal value As Color)
            If _EnabledBackColor <> value Then
                _EnabledBackColor = value
                MyBase.BackColor = _EnabledBackColor
            End If
        End Set
    End Property

#End Region

#Region "Overrides"

    ''' <summary>
    ''' When the shadowed enabled property changes, this method is called
    ''' </summary>
    Protected Overrides Sub OnEnabledChanged(ByVal e As System.EventArgs)

        'COMMON ROUTINE FOR TOGGLING ENABLED STATUS
        ToggleEnabled()

        'SEND NOTIFICATION TO BASE CLASS
        MyBase.OnEnabledChanged(e)

    End Sub

    'OVERRIDE PreProcessMessage TO LOOK FOR KEY PRESSES AND FILTER THEM
    Public Overrides Function PreProcessMessage(ByRef msg As Message) As Boolean

        'PREVENT KEYBOARD ENTRY IF CONTROL IS DISABLED
        If Not _ReadOnly Then

            'CHECK IF ITS A KEYDOWN MESSAGE (&H100)
            If msg.Msg = &H100 Then

                'GET THE KEY THAT WAS PRESSED
                Dim key As Int32 = msg.WParam.ToInt32

                'ALLOW TAB, LEFT, OR RIGHT KEYS
                If key <> Keys.Tab OrElse _
                   key <> Keys.Left OrElse _
                   key <> Keys.Right Then

                    Return True

                End If
            End If
        End If

        'CALL BASE METHOD SO DELEGATES RECEIVE EVENT
        Return MyBase.PreProcessMessage(msg)

    End Function

    'OVERRIDE WndProc TO LOOK FOR DROP DOWN MESSAGES AND FILTER THEM
    Protected Overrides Sub WndProc(ByRef m As Message)

        'PREVENT DROPDOWN LIST DISPLAYING IF READONLY
        If Not _ReadOnly Then
            If m.Msg = &H201 OrElse m.Msg = &H203 Then
                Return
            End If
        End If

        'CALL BASE METHOD SO DELEGATES RECEIVE EVENT
        MyBase.WndProc(m)
    End Sub

    'WHEN THE CONTROL IS IN A CONTAINER, AND THE CONTAINER'S ENABLED PROPERTY
    'IS SET TO FALSE, THIS CONTROL GETS ITS OnParentEnabledChanged CALLED
    'NOT JUST THE CONTROLS ENABLED PROPERTY SET SO WE OVERRIDE THIS, AND
    'TOGGLE ENABLED STATE ACCORDINGLY
    Protected Overrides Sub OnParentEnabledChanged(ByVal e As System.EventArgs)

        _ReadOnly = MyBase.Parent.Enabled

        If _ReadOnly Then
            MyBase.OnParentEnabledChanged(e)
        Else
            ToggleEnabled()
        End If
    End Sub

#End Region

#Region "Support Methods"
    'COMMON ROUTINE FOR TOGGLING ENABLED STATE
    Private Sub ToggleEnabled()

        'IF THE CONTROL IS DISABLED, TURN OFF ITS TABSTOP
        MyBase.TabStop = _ReadOnly

        'IF THE CONTROL IS DISABLED, SET ITS CONTEXT MENU TO
        'A DUMMY NEW CONTEXT MENU SO WE DON'T GET THE
        'DEFAULT CONTEXT MENU, OTHERWISE SETTING IT TO
        'NOTHING REAPPLIES THE DEFAULT CONTEXT MENU
        'ALSO SET BACK COLOR ACCORDINGLY
        If Not _ReadOnly Then
            MyBase.ContextMenuStrip = New ContextMenuStrip
            MyBase.BackColor = _DisabledBackColor
        Else
            MyBase.ContextMenuStrip = Nothing
            MyBase.BackColor = _EnabledBackColor
        End If

        'DESELECT ANY TEXT FROM COMBOBOX
        Me.SelectionLength = 0

    End Sub
#End Region

End Class