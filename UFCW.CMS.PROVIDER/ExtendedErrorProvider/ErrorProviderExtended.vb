Option Strict Off

Imports System.Windows.Forms

'Following class is inherited from basic ErrorProvider class
#Region "Error Provider Extended"
Public Class ErrorProviderExtended
    Inherits System.Windows.Forms.ErrorProvider

    Private _validationcontrols As New ValidationControlCollection
    Private _mandatorymessage As String = "Please correct the following mandatory field(s),"
    Private _conditionalmessage As String = "A valid address must be supplied: Enter either a Valid U.S address, or provide a Country,"
    Private _validationmessage As String = "Please correct the following data issue(s),"

    Public Property ConditionalMessage() As String
        Get
            Return _conditionalmessage
        End Get
        Set(ByVal Value As String)
            _conditionalmessage = Value
        End Set
    End Property

    Public Property MandatoryMessage() As String
        Get
            Return _mandatorymessage
        End Get
        Set(ByVal Value As String)
            _mandatorymessage = Value
        End Set
    End Property

    'This property will be used for displaying a summary message about all empty fields
    'Default value is "Please enter following mandatory fields,". You can set any other 
    'message using this property.
    Public Property ValidationMessage() As String
        Get
            Return _validationmessage
        End Get
        Set(ByVal Value As String)
            _validationmessage = Value
        End Set
    End Property
    'Controls property is of type ValidationControlCollection which is inherited from CollectionBase 
    'Controls holds all those objects which should be validated.
    Public Property Controls() As ValidationControlCollection
        Get
            Return _validationcontrols
        End Get
        Set(ByVal Value As ValidationControlCollection)
            _validationcontrols = Value
        End Set
    End Property

    'Following function returns true if all fields on form pass validation.
    'If not all fields are entered, this function displays a message box which contains all those field names 
    'which are empty and returns FALSE.
    Public Function ValidateAndShowSummaryErrorMessage() As Boolean
        If Controls.Count <= 0 Then
            Return True
        End If
        Dim i As Integer
        Dim msg As String = ValidationMessage + vbNewLine + vbNewLine
        Dim berrors As Boolean = False
        For i = 0 To Controls.Count - 1
            If Controls(i).Validate Then
                If Me.GetError(CType(Controls(i).ControlObj, Control)).Trim(CChar(" ")).Length > 0 Then
                    msg &= "> " & Controls(i).DisplayName & vbNewLine
                    SetError(CType(Controls(i).ControlObj, Control), Controls(i).ErrorMessage)
                    berrors = True
                Else
                    SetError(CType(Controls(i).ControlObj, Control), "")
                End If
            Else
                SetError(CType(Controls(i).ControlObj, Control), "")
            End If
        Next
        If berrors Then
            System.Windows.Forms.MessageBox.Show(msg, "Missing Information", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Stop)
            Return False
        Else
            Return True
        End If
    End Function

    'Following function returns true if all fields on form are entered.
    'If not all fields are entered, this function displays a message box which contains all those field names 
    'which are empty and returns FALSE.
    Public Function CheckRequiredAndShowSummaryErrorMessage() As Boolean

        If Controls.Count <= 0 Then
            Return True
        End If

        Dim iOuter As Integer
        Dim iOnForm As Integer

        Dim ConditionsMet As Integer
        Dim ConditionalErrors As Integer = 0
        Dim RequiredErrors As Integer = 0
        Dim ConditionalAlternative As String

        Dim Conditionalmsg As String = ConditionalMessage & vbNewLine & vbNewLine
        Dim Mandatorymsg As String = MandatoryMessage & vbNewLine & vbNewLine
        Dim Summarymsg As String = ""

        For iOuter = 0 To Controls.Count - 1
            If (Controls(iOuter).Required AndAlso CType(Controls(iOuter).ControlObj.text, String).Trim(" ").Length < 1) AndAlso Controls(iOuter).Conditional Then 'For Conditional relationships between fields

                ConditionsMet = 0

                For Each ConditionalAlternative In Controls(iOuter).Conditions 'grab the conditional fields associated to a control
                    'A Required condition is only true if the conditional fields are blank

                    For iOnForm = 0 To Controls.Count - 1 'loop back through all form controls to see if they satisfy the required conditions

                        If ConditionalAlternative = Controls(iOnForm).ControlObj.Name Then 'And FieldWithConditions <> Controls(iOuter).ControlObj.Name Then
                            If CType(Controls(iOnForm).ControlObj.text, String).Trim(" ").Length > 0 Then
                                ConditionsMet += 1 'Conditional (Alternative) Field is populated
                            Else
                                Conditionalmsg &= "> " & Controls(iOnForm).DisplayName & vbNewLine
                                ''SetError(Controls(iOuter).ControlObj, Controls(iOuter).ErrorMessage)
                                ConditionalErrors += 1 'Conditional (Alternative) Field is not populated
                            End If
                        End If
                    Next

                Next

                If ConditionsMet >= Controls(iOuter).Conditions.GetLength(0) Then
                    SetError(CType(Controls(iOuter).ControlObj, Control), "") 'All conditions for field have been met
                Else
                    Conditionalmsg &= "> " & Controls(iOuter).DisplayName & vbNewLine
                    SetError(CType(Controls(iOuter).ControlObj, Control), Controls(iOuter).ErrorMessage)
                    ConditionalErrors += 1
                End If

            ElseIf Controls(iOuter).Required Then
                If Trim(Controls(iOuter).ControlObj.text) = "" Then
                    Mandatorymsg &= "> " & Controls(iOuter).DisplayName & vbNewLine
                    SetError(CType(Controls(iOuter).ControlObj, Control), Controls(iOuter).ErrorMessage)
                    RequiredErrors += 1
                Else
                    SetError(CType(Controls(iOuter).ControlObj, Control), "")
                End If
            Else
                SetError(CType(Controls(iOuter).ControlObj, Control), "")
            End If
        Next

        If RequiredErrors > 0 Then
            Summarymsg = Mandatorymsg
        End If

        If ConditionalErrors > 0 Then
            Summarymsg = Summarymsg & CStr(IIf(Summarymsg Is Nothing, Conditionalmsg, vbCrLf & Conditionalmsg))
        End If

        If ConditionalErrors > 0 Or RequiredErrors > 0 Then
            System.Windows.Forms.MessageBox.Show(Summarymsg & vbNewLine & vbNewLine & "Note: Suspend Address to disable validation!", "Validation Failed", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Stop)
            Return False
        Else
            Return True
        End If
    End Function

    'Following function clears error messages from all controls.
    Public Sub ClearAllErrorMessages()
        Dim i As Integer
        For i = 0 To Controls.Count - 1
            SetError(CType(Controls(i).ControlObj, Control), "")
        Next
    End Sub

    'This function hooks validation event with all controls.
    Public Sub SetErrorEvents()
        Dim i As Integer
        For i = 0 To Controls.Count - 1
            AddHandler CType(Controls(i).ControlObj, System.Windows.Forms.Control).Validating, AddressOf Validation_Event
        Next
    End Sub

    'Following event is hooked for all controls, it sets an error message with the use of ErrorProvider.
    Private Sub Validation_Event(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) 'Handles txtCompanyName.Validating

        If Controls(sender).Validate Then
            If Trim(sender.Text) = "" Then
                MyBase.SetError(CType(sender, Control), Controls(sender).ErrorMessage)
            Else
                MyBase.SetError(CType(sender, Control), "")
            End If
        End If
    End Sub
End Class
#End Region

'Following class is inherited from CollectionBase class. It is used for holding all Validation Controls.
'This class is collection of ValidationControl class objects.
'This class is used by ErrorProviderExtended class.
#Region "ValidationControlCollection"
Public Class ValidationControlCollection
    Inherits CollectionBase
    Default Public Property Item(ByVal ListIndex As Integer) As ValidationControl
        Get
            Return CType(Me.List(ListIndex), ValidationControl)
        End Get
        Set(ByVal Value As ValidationControl)
            Me.List(ListIndex) = Value
        End Set
    End Property

    Default Public Property Item(ByVal pControl As Object) As ValidationControl
        Get
            If pControl Is Nothing Then Return Nothing

            If GetIndex(pControl.Name) < 0 Then
                Return New ValidationControl
            End If
            Return Me.List(GetIndex(pControl.Name))
        End Get
        Set(ByVal Value As ValidationControl)
            If pControl Is Nothing Then Exit Property
            If GetIndex(pControl.Name) < 0 Then
                Exit Property
            End If
            Me.List(GetIndex(pControl.Name)) = Value
        End Set
    End Property

    Function GetIndex(ByVal ControlName As String) As Integer
        Dim i As Integer
        For i = 0 To Count - 1
            If Item(i).ControlObj.name.toupper = ControlName.ToUpper Then
                Return i
            End If
        Next
        Return -1
    End Function

    Public Sub Add(ByRef pControl As TextBox, ByVal pDisplayName As String)
        If pControl Is Nothing Then Exit Sub
        Dim obj As New ValidationControl

        Try

            obj.ControlObj = pControl
            obj.DisplayName = pDisplayName
            obj.ErrorMessage = "Please enter " + pDisplayName
            Me.List.Add(obj)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub Add(ByRef pControl As ListBox, ByVal pDisplayName As String)
        If pControl Is Nothing Then Exit Sub
        Dim obj As New ValidationControl

        Try

            obj.ControlObj = pControl
            obj.DisplayName = pDisplayName
            obj.ErrorMessage = "Please enter " + pDisplayName
            Me.List.Add(obj)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Add(ByRef pControl As ComboBox, ByVal pDisplayName As String)
        If pControl Is Nothing Then Exit Sub
        Dim obj As New ValidationControl

        Try

            obj.ControlObj = pControl
            obj.DisplayName = pDisplayName
            obj.ErrorMessage = "Please enter " + pDisplayName
            Me.List.Add(obj)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Add(ByRef pControl As Object, ByVal pDisplayName As String)
        If pControl Is Nothing Then Exit Sub
        Dim obj As New ValidationControl

        Try

            obj.ControlObj = pControl
            obj.DisplayName = pDisplayName
            obj.ErrorMessage = "Please enter " + pDisplayName
            Me.List.Add(obj)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub Add(ByRef pControl As Object, ByVal pDisplayName As String, ByVal pErrorMessage As String)
        If pControl Is Nothing Then Exit Sub
        Dim obj As New ValidationControl
        obj.ControlObj = pControl
        obj.DisplayName = pDisplayName
        obj.ErrorMessage = pErrorMessage
        Me.List.Add(obj)
    End Sub
    Public Sub Add(ByRef pControl As Object)
        If pControl Is Nothing Then Exit Sub
        Dim obj As New ValidationControl
        obj.ControlObj = pControl
        obj.DisplayName = pControl.Name
        obj.ErrorMessage = "Please enter " + pControl.Name
        Me.List.Add(obj)
    End Sub
    Public Sub Add(ByVal pControl As ValidationControl)
        If pControl Is Nothing Then Exit Sub
        Me.List.Add(pControl)
    End Sub
    Public Sub Remove(ByVal pControl As Object)
        If pControl Is Nothing Then Exit Sub
        Dim i As Integer = Me.GetIndex(pControl.Name)
        If i >= 0 Then
            Me.List.RemoveAt(i)
        End If
    End Sub
End Class
#End Region

'ValidationControl class is used to hold any control from windows form. 
'It holds any control in ControlObj property.
#Region "ValidationControl"
Public Class ValidationControl
    Private _control As Object
    Private _displayname As String
    Private _errormessage As String
    Private _validate As Boolean = True
    Private _required As Boolean = False
    Private _conditional As Boolean = False
    Private _conditions() As String = {}


    'Validate property decides weather control is to be validated. Default value is TRUE.
    Public Property Validate() As Boolean
        Get
            Return _validate
        End Get
        Set(ByVal Value As Boolean)
            _validate = Value
        End Set
    End Property
    'Validate property decides whether control is Required. Default value is False
    Public Property Required() As Boolean
        Get
            Return _required
        End Get
        Set(ByVal Value As Boolean)
            _required = Value
        End Set
    End Property

    'Validate property decides whether control is Required in the absence of associated entries. Default value is False
    Public Property Conditions() As Array
        Get
            Return _conditions
        End Get
        Set(ByVal Value As Array)
            _conditions = CType(Value, String())
        End Set
    End Property

    'Validate property decides whether control is Required in the absence of associated entries. Default value is False
    Public Property Conditional() As Boolean
        Get
            Return _conditional
        End Get
        Set(ByVal Value As Boolean)
            _conditional = Value
        End Set
    End Property
    'ControlObj is a control from windows form which is to be validated.
    'For example txtStudentName
    Public Property ControlObj() As Object
        Get
            Return _control
        End Get
        Set(ByVal Value As Object)
            _control = Value
        End Set
    End Property

    'DisplayName property is used for displaying summary message to user.
    'For example, for txtStudentName you can set 'Student Full Name' as field name.
    'This field name will be displayed in summary message.
    Public Property DisplayName() As String
        Get
            Return _displayname
        End Get
        Set(ByVal Value As String)
            _displayname = Value
        End Set
    End Property

    'ErrorMessage is also used for displaying summary message.
    'For example, you can enter 'Student Name is mandatory' as an error message.
    Public Property ErrorMessage() As String
        Get
            Return _errormessage
        End Get
        Set(ByVal Value As String)
            _errormessage = Value
        End Set
    End Property
End Class
#End Region

