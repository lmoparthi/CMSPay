Option Strict On

<ToolboxBitmap(GetType(CheckedListBox))>
Public Class ExtendedCheckedListBox
    'Show the CheckedListBox icon in the Toolbox for our component
    Inherits System.Windows.Forms.CheckedListBox
    '----------------------------------------------------------
    '  Class level variables 
    '----------------------------------------------------------
    Dim AllowChecks As Boolean 'Controls whether checkstate can be changed
    Dim ChkMember As String 'The DataColumn to use for checkstate
    Dim dt As DataTable 'Data to display

    '----------------------------------------------------------
    '  Bug Fix
    '----------------------------------------------------------
    '  When the CheckedListBox control is in a Tabcontrol 
    '  and that the Datasource property is used to fill
    '  up the item list, setting the clb's visible property
    '  to false, then to true, or flipping the tabs would
    '  cause the clb to "forget"THE checks.
    '
    '  Implement Carl Mercier's workaround
    '  http://www.codeproject.com/cs/combobox/FixedCheckedListBox.asp
    '----------------------------------------------------------
    Public Overloads Property DataSource() As Object
        Get
            'Return our datatable variable
            DataSource = CType(dt, Object)
        End Get
        Set(ByVal value As Object)
            'Set our datatable variable
            dt = CType(value, DataTable)
            LoadData()
        End Set
    End Property
    Private Sub LoadData()
        Dim bufAllowChecks As Boolean = AllowChecks
        If AllowChecks = False Then
            'This is needed so we can change checkstates
            AllowChecks = True
        End If
        'Clear items
        MyBase.Items.Clear()
        'Fill it again
        Dim i As Integer
        For i = 0 To dt.DefaultView.Count - 1
            'Determine whether to check each item or not
            If dt.Rows(i).Item(ChkMember).ToString = "1" OrElse CBool(dt.Rows(i).Item(ChkMember)) Then
                MyBase.Items.Add(dt.DefaultView.Item(i), True)
            Else
                MyBase.Items.Add(dt.DefaultView.Item(i), False)
            End If
        Next
        AllowChecks = CBool(bufAllowChecks)
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
    <System.ComponentModel.Description("Allow checkstate to be changed"),
        System.ComponentModel.Category("Behavior")>
    Public Property AutoCheck() As Boolean
        Get
            'Return our checkstate variable
            AutoCheck = AllowChecks
        End Get
        Set(ByVal value As Boolean)
            'Set our checkstate variable
            AllowChecks = value
        End Set
    End Property
    'Override the ItemCheck event with our own
    Protected Overrides Sub OnItemCheck(ByVal ice As System.Windows.Forms.ItemCheckEventArgs)
        'Allow checks to be changed only if AutoCheck = True
        If AllowChecks = False Then
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
    <System.ComponentModel.Description("Gets or sets the CheckMember")>
    Public Property CheckMember() As String
        Get
            CheckMember = ChkMember
        End Get
        Set(ByVal value As String)
            ChkMember = value
        End Set
    End Property
    <System.ComponentModel.Description("Gets or sets an items's checkstate")>
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
            Dim bufAllowChecks As Boolean = AllowChecks
            If AllowChecks = False Then
                AllowChecks = True
            End If
            MyBase.SetItemChecked(index, value)
            AllowChecks = CBool(bufAllowChecks)
        End Set
    End Property

    <System.ComponentModel.Description("Gets or sets an items's DisplayMember")>
    Public Overloads Property Text(ByVal index As Integer) As String
        Get
            Text = CStr(dt.Rows(index).Item(MyBase.DisplayMember))
        End Get
        Set(ByVal value As String)
            dt.Rows(index).Item(MyBase.DisplayMember) = value
        End Set
    End Property
    <System.ComponentModel.Description("Gets or sets an items's ValueMember")>
    Public Property Value(ByVal index As Integer) As String
        Get
            Value = CStr(dt.Rows(index).Item(MyBase.ValueMember))
        End Get
        Set(ByVal value As String)
            dt.Rows(index).Item(MyBase.ValueMember) = value
        End Set
    End Property
End Class

