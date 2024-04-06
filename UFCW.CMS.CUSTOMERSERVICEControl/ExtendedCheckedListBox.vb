<ToolboxBitmap(GetType(CheckedListBox))> _
Public Class ExtendedCheckedListBox
    'Show the CheckedListBox icon in the Toolbox for our component
    Inherits System.Windows.Forms.CheckedListBox

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    '----------------------------------------------------------
    '  Class level variables
    '----------------------------------------------------------
    Dim _AllowChecks As Boolean 'Controls whether checkstate can be changed
    Dim _ChkMember As String 'The DataColumn to use for checkstate
    Dim _DT As DataTable 'Data to display

    '----------------------------------------------------------
    '  Bug Fix
    '----------------------------------------------------------
    '  When the CheckedListBox control is in a Tabcontrol
    '  and that the Datasource property is used to fill
    '  up the item list, setting the clb's visible property
    '  to false, then to true, or flipping the tabs would
    '  cause the clb to "forget" the checks.
    '
    '  Implement Carl Mercier's workaround
    '  http://www.codeproject.com/cs/combobox/FixedCheckedListBox.asp
    '----------------------------------------------------------
    Public Overloads Property DataSource() As Object
        Get
            'Return our datatable variable
            DataSource = CType(_DT, Object)
        End Get
        Set(ByVal value As Object)
            'Set our datatable variable
            _DT = CType(value, DataTable)
            LoadData()
        End Set
    End Property
    Private Sub LoadData()
        Dim AllowChecks As Boolean = _AllowChecks

        If _AllowChecks = False Then
            'This is needed so we can change checkstates
            _AllowChecks = True
        End If

        'Clear items
        MyBase.Items.Clear()
        'Fill it again

        For I As Integer = 0 To _DT.DefaultView.Count - 1
            'Determine whether to check each item or not
            If _DT.Rows(I).Item(_ChkMember).ToString = "1" OrElse _DT.Rows(I).Item(_ChkMember).ToString = "True" Then
                MyBase.Items.Add(_DT.DefaultView.Item(I), True)
            Else
                MyBase.Items.Add(_DT.DefaultView.Item(I), False)
            End If
        Next
        _AllowChecks = AllowChecks
    End Sub
    'Added or deleted records won't show without a refresh
    Public Overrides Sub Refresh()
        LoadData()
    End Sub

    '----------------------------------------------------------
    '  Allow / Lock Checkstate Functionality
    '----------------------------------------------------------
    '  Only let users check or uncheck items when you let them.
    '  Note that you can't programmatically check or uncheck
    '  items either unless AllowChecks is True.
    '
    '  This is an emulation of the Checkbox control's AutoCheck
    '  property.
    '----------------------------------------------------------
    'Show our property under the Behavior section of the Properties window
    'So that it can be set at design time
    <System.ComponentModel.Description("Allow checkstate to be changed"), _
        System.ComponentModel.Category("Behavior")> _
    Public Property AutoCheck() As Boolean
        Get
            'Return our checkstate variable
            AutoCheck = _AllowChecks
        End Get
        Set(ByVal value As Boolean)
            'Set our checkstate variable
            _AllowChecks = value
        End Set
    End Property
    'Override the ItemCheck event with our own
    Protected Overrides Sub OnItemCheck(ByVal ice As System.Windows.Forms.ItemCheckEventArgs)
        'Allow checks to be changed only if AutoCheck = True
        If _AllowChecks = False Then
            ice.NewValue = ice.CurrentValue
        End If
    End Sub

    '----------------------------------------------------------
    '  Reference Item By It's Index Functionality
    '----------------------------------------------------------
    '  Wouldn't it be nice if you could reference items in a
    '  CheckedListBox control just like you can with a
    '  ComboBox? Well now you can!
    '
    '  Obtain Checkstatus, Text, and Values via an items index
    '----------------------------------------------------------
    <System.ComponentModel.Description("Gets or sets the CheckMember")> _
    Public Property CheckMember() As String
        Get
            CheckMember = _ChkMember
        End Get
        Set(ByVal value As String)
            _ChkMember = value
        End Set
    End Property
    <System.ComponentModel.Description("Gets or sets an items's checkstate")> _
    Public Property Checked(ByVal index As Integer) As Boolean
        Get
            'Search for the item in CheckedIndices to see if its checked or not
            If MyBase.CheckedIndices.IndexOf(index) <> -1 Then
                Checked = True
            Else
                Checked = False
            End If
        End Get
        Set(ByVal value As Boolean)
            'Set item's checkstate
            Dim bufAllowChecks As Boolean = _AllowChecks
            If _AllowChecks = False Then
                _AllowChecks = True
            End If
            MyBase.SetItemChecked(index, value)
            _AllowChecks = bufAllowChecks
        End Set
    End Property
    <System.ComponentModel.Description("Gets or sets an items's DisplayMember")> _
    Public Overloads Property Text(ByVal index As Integer) As String
        Get
            Text = CStr(_DT.Rows(index).Item(MyBase.DisplayMember))
        End Get
        Set(ByVal value As String)
            _DT.Rows(index).Item(MyBase.DisplayMember) = value
        End Set
    End Property
    <System.ComponentModel.Description("Gets or sets an items's ValueMember")> _
    Public Property Value(ByVal index As Integer) As String
        Get
            Value = CStr(_DT.Rows(index).Item(MyBase.ValueMember))
        End Get
        Set(ByVal value As String)
            _DT.Rows(index).Item(MyBase.ValueMember) = value
        End Set
    End Property
End Class