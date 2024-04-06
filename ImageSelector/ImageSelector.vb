Option Strict On

Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.ComponentModel.Design.Serialization
Imports System.Windows.Forms.Design

''' -----------------------------------------------------------------------------
''' Project	 : ImageSelector
''' Class	 : ImageSelector
''' 
''' -----------------------------------------------------------------------------
''' <summary>
''' Displays and Tracks selected images
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[Nick Snyder]	7/11/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Designer(GetType(ImageSelectorDesigner)), DefaultProperty("ImageList")> _
Public Class ImageSelector
    Inherits System.Windows.Forms.UserControl

    Public Event SelectAllItems()
    Public Event UnSelectAllItems()

    Private WithEvents _ImageList As System.Windows.Forms.ImageList
    Private _Items As ImageItemCollection
    Private WithEvents _SizedItems As New ImageItem
    Private _SelectedIndices As New IndexCollection
    Private _SelectedItems As New SelectedImageCollection
    Private _SelectedIndex As Integer = -1
    Private _SelectedItem As Object = Nothing
    Private LastButton As Button = Nothing
    Private _ImageAutoSize As Boolean = False
    Private _ImageSize As System.Drawing.Size = New Size(16, 16)
    Private _MultiSelect As Boolean
    Private highlightedButton As ImageItem
    'Private _BorderStyle As Border3DStyle = Border3DStyle.Sunken
    Private _BorderStyle As BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D

#Region " Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.DoubleBuffer, True)
        _Items = New ImageItemCollection(Me)

        MyBase.AutoScroll = True
        Me.Size = New Size(150, 45)
    End Sub

    'UserControl overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        '
        'ImageSelector
        '
        Me.Name = "ImageSelector"

    End Sub
#End Region

#Region "Properties"
    <DefaultValue(CBool(True)), Browsable(False)> _
    Public Overrides Property AutoScroll() As Boolean
        Get
            Return MyBase.AutoScroll
        End Get
        Set(ByVal Value As Boolean)
            MyBase.AutoScroll = Value
        End Set
    End Property

    <DefaultValue(GetType(ImageList), "Nothing"), TypeConverter(GetType(ImageList)), System.ComponentModel.Description("Gets or Sets the ImageList for this ImageSelector.")> _
    Public Property ImageList() As ImageList
        Get
            Return _ImageList
        End Get
        Set(ByVal Value As ImageList)
            _ImageList = Value

            CalculateLayout()
        End Set
    End Property

    <DefaultValue(CBool(False)), System.ComponentModel.Description("Allows more than one item to be selected."), Browsable(True)> _
    Public Property AllowMultiSelect() As Boolean
        Get
            Return _MultiSelect
        End Get
        Set(ByVal Value As Boolean)
            _MultiSelect = Value
            If Value = False Then
                Dim indx As Integer = _SelectedIndex
                UnSelectAll()
                _Items(indx).Select()
            End If
        End Set
    End Property

    <DefaultValue(GetType(System.Windows.Forms.BorderStyle), "Fixed3D"), System.ComponentModel.Description("Controls what type of border the ImageSelector should have."), Browsable(True)> _
    Public Property BorderStyle() As BorderStyle
        Get
            Return _BorderStyle
        End Get
        Set(ByVal Value As BorderStyle)
            _BorderStyle = Value
            'If Value = Nothing Then
            '    _BorderStyle = Nothing
            'Else
            '    _BorderStyle = Value
            'End If
        End Set
    End Property

    <System.ComponentModel.Description("The Images stored in this ImageSelector."), Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    Public ReadOnly Property Items() As ImageItemCollection
        Get
            Return _Items
        End Get
    End Property

    <System.ComponentModel.Description("The Images stored in this ImageSelector."), Browsable(True), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    Default Public ReadOnly Property Item(ByVal Index As Integer) As ImageItem
        Get
            Return _Items.Item(Index)
        End Get
    End Property

    <DefaultValue(CInt(-1)), System.ComponentModel.Description("Gets or Sets the Selected Item Index."), Browsable(True)> _
    Public Property SelectedIndex() As Integer
        Get
            Return _SelectedIndex
        End Get
        Set(ByVal Value As Integer)
            '_SelectedIndex = Value
            If Value = -1 Then
                If _SelectedIndex <> -1 Then
                    Me.Items(_SelectedIndex).UnSelect()
                End If
                _SelectedIndex = Value
                _SelectedItem = Nothing
            End If

            If _Items Is Nothing Then Exit Property

            If Value > -1 And Value < _Items.Count Then
                _SelectedIndex = Value
                _SelectedItem = Me.Items(Value)
                Me.Items(Value).Select()
            Else
                'not a valid selection index
                If _Items.Count > 0 Then
                    If Value >= _Items.Count Then
                        _SelectedIndex = _Items.Count - 1
                        _SelectedItem = Me.Items(_Items.Count - 1)
                        Me.Items(_Items.Count - 1).Select()
                    Else
                        If Value <> -1 Then
                            _SelectedIndex = 0
                            _SelectedItem = Me.Items(0)
                            Me.Items(0).Select()
                        End If
                    End If
                End If
            End If
        End Set
    End Property

    <DefaultValue(GetType(Object), Nothing), System.ComponentModel.Description("Gets the Selected Item."), Browsable(False)> _
    Public ReadOnly Property SelectedItem() As Image
        Get
            If Not _SelectedItem Is Nothing Then
                Return CType(_SelectedItem, Image) 'CType(_SelectedItem, ImageItem)
            Else
                Return Nothing
            End If
        End Get
        'Set(ByVal Value As ImageItem)
        '    Dim indx As Integer

        '    If _Items.Count > 0 Then
        '        indx = _Items.IndexOf(Value)
        '        If indx >= 0 Then
        '            _SelectedIndex = indx
        '            _SelectedItem = Value
        '            Me.Items(indx).Select()
        '        Else
        '            _SelectedIndex = -1
        '            _SelectedItem = Nothing
        '        End If
        '    End If
        'End Set
    End Property

    <System.ComponentModel.Description("Gets the Selected Items."), System.ComponentModel.Browsable(False)> _
    Public ReadOnly Property SelectedItems() As SelectedImageCollection
        Get
            Return _SelectedItems
        End Get
    End Property

    <System.ComponentModel.Description("Gets the Selected Indices."), System.ComponentModel.Browsable(False)> _
    Public ReadOnly Property SelectedIndices() As IndexCollection
        Get
            Return _SelectedIndices
        End Get
    End Property
#End Region

    Friend Sub CalculateLayout()
        Const PADDING As Integer = 4

        Dim ItemSize, x, i As Integer 'x is the current horizontal position
        Dim It As ImageItem
        Dim wrct As Rectangle
        Dim ImgHeight As Decimal
        Dim ImgWidth As Decimal

        x = PADDING
        ItemSize = ClientRectangle.Height - (2 * PADDING)
        For i = 0 To _Items.Count - 1
            It = _Items(i)

            It.Visible = False

            If Not _ImageList Is Nothing AndAlso Not _ImageList.Images Is Nothing AndAlso _
                            _ImageList.Images.Count > i Then
                ImgHeight = Me.ImageList.ImageSize.Height
                ImgWidth = Me.ImageList.ImageSize.Width

                It.Image = Me._ImageList.Images(i)
            ElseIf Not It.Image Is Nothing Then
                ImgHeight = It.Image.Size.Height
                ImgWidth = It.Image.Size.Width
            Else
                ImgHeight = 16
                ImgWidth = 16

                It.Image = Nothing
            End If

            It.Height = CInt(ImgHeight + (4 * PADDING))
            It.Width = CInt(ImgWidth + (4 * PADDING))
            It.Left = x
            It.Top = PADDING

            Me.Controls.Add(It)

            wrct = New Rectangle(x, PADDING, CInt(ImgWidth + (2 * PADDING)), CInt(ImgHeight + (2 * PADDING)))
            It.Bounds = wrct

            It.Visible = True

            x = CInt(x + (ImgWidth + 10))
        Next

        'Mark the control as invalid so it gets redrawn
        Invalidate()
    End Sub

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)

        If Not Me.Parent Is Nothing Then Me.Parent.Invalidate()
        Invalidate()
        'CalculateLayout()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        Dim borderWidth As Integer = 2
        Dim Border As Border3DStyle

        'Dim rct As Rectangle = e.ClipRectangle
        'Dim rct As Rectangle = New Rectangle(Me.Left - 3, Me.Top - 3, Me.Width + 6, Me.Height + 6)
        Dim rct As Rectangle = New Rectangle(Me.Left - borderWidth, Me.Top - borderWidth, Me.Width + (2 * borderWidth), Me.Height + (2 * borderWidth))

        'rct.Inflate(-1 * borderWidth, -1 * borderWidth)

        'ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, borderColor, borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid)
        'ControlPaint.DrawBorder(Me.Parent.CreateGraphics, rct, borderColor, borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid)
        'If Not _BorderStyle Is Nothing Then

        'End If

        'ControlPaint.DrawBorder3D(e.Graphics, rct, _BorderStyle, Border3DSide.All)

        If _BorderStyle <> BorderStyle.None Then
            Select Case _BorderStyle
                Case Is = BorderStyle.Fixed3D
                    Border = Border3DStyle.Sunken
                Case Is = BorderStyle.FixedSingle
                    Border = Border3DStyle.Flat
            End Select
            ControlPaint.DrawBorder3D(Me.Parent.CreateGraphics, rct, Border, Border3DSide.All)
            'rct = e.ClipRectangle
            'ControlPaint.DrawBorder3D(e.Graphics, rct, Border, Border3DSide.All)
        End If
    End Sub

    'Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
    '    Dim It As ImageItem
    '    'Dim b As Brush
    '    Dim wrct As Rectangle

    '    For Each It In _Items
    '        If It.Focused = True Then
    '            Dim g As Graphics = Me.CreateGraphics 'Me.CreateGraphics
    '            Dim rct As Rectangle = It.Bounds
    '            rct.Inflate(-1, -1)

    '            'g.DrawRectangle(SystemPens.ControlDarkDark, rct.X, rct.Y, rct.Width, rct.Height)
    '            g.FillRectangle(SystemBrushes.ControlDarkDark, rct.X, rct.Y, rct.Width, rct.Height)

    '            'Invalidate()
    '        End If

    '        ''Create brush from button colour
    '        ''If Not (b Is Nothing) Then b.Dispose()
    '        ''b = New SolidBrush(Color.DarkKhaki)
    '        ''b = New SolidBrush(button.Colour)

    '        ''Fill rectangle with this colour
    '        'wrct = It.Bounds
    '        ''wrct.Width = DefaultImageList.ImageSize.Width
    '        ''wrct.Height = DefaultImageList.ImageSize.Height

    '        ''If highlightedButton Is It Or It.ImageIndex = _SelectedIndex Then
    '        ''    'e.Graphics.FillRectangle(SystemBrushes.Highlight, RectangleF.op_Implicit(wrct))
    '        ''    wrct.Inflate(-3, -3)
    '        ''    e.Graphics.DrawRectangle(SystemPens.ControlDarkDark, wrct.X, wrct.Y, wrct.Width, wrct.Height)
    '        ''End If
    '        ''wrct.Inflate(-3, -3)
    '        'e.Graphics.DrawImage(It.Image, wrct)
    '        ''e.Graphics.FillRectangle(b, wrct)
    '    Next
    'End Sub

    Friend Sub OnSelectionChanged()
        Dim button As ImageItem
        Dim newHighlightedButton As ImageItem = Nothing
        Dim s As ISelectionService = DirectCast(GetService(GetType(ISelectionService)), ISelectionService)

        'See if the primary selection is one of our buttons
        For Each button In Items
            'button.UnSelect()

            'If button.Focused = True Then
            '    Dim g As Graphics = Me.CreateGraphics 'Me.CreateGraphics
            '    Dim rct As Rectangle = Me.Bounds
            '    rct.Inflate(-1, -1)

            '    g.DrawRectangle(SystemPens.ControlDarkDark, rct.X, rct.Y, rct.Width, rct.Height)

            '    'Invalidate()
            'End If

            If s.PrimarySelection Is button Then
                button.Select()

                'If button.Selected = True Then
                '    button.UnSelect()
                'Else
                '    button.Select()
                'End If

                'SelectedDesignerControl = button

                newHighlightedButton = button
                'Exit For
                'ElseIf Not TypeOf s.PrimarySelection Is ImageItem And Not TypeOf s.PrimarySelection Is ImageSelector Then
            ElseIf Not TypeOf s.PrimarySelection Is ImageItem Then
                button.UnSelect()
            End If
        Next

        'Apply if necessary
        If Not newHighlightedButton Is highlightedButton Then
            highlightedButton = newHighlightedButton
            If DesignMode = False Then Invalidate()
        End If

        If DesignMode = True Then Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim It As ImageItem
        Dim wrct As Rectangle
        Dim s As ISelectionService
        Dim a As ArrayList
        Dim c As IComponentChangeService = DirectCast(GetService(GetType(IComponentChangeService)), IComponentChangeService)

        If DesignMode Then
            For Each It In _Items
                wrct = It.Bounds
                If wrct.Contains(e.X, e.Y) Then
                    s = DirectCast(GetService(GetType(ISelectionService)), ISelectionService)
                    a = New ArrayList
                    a.Add(It)
                    s.SetSelectedComponents(a)

                    'c.OnComponentChanging(Me, Nothing)
                    'If It.ImageIndex = SelectedIndex Then
                    '    It.Select()
                    'End If
                    'c.OnComponentChanged(Me, Nothing, Nothing, Nothing)

                    '&Invalidate()

                    Exit For
                End If
            Next
        Else
            For Each It In _Items
                wrct = It.Bounds
                If wrct.Contains(e.X, e.Y) Then
                    _SelectedItem = It
                    Me.Refresh()
                    Exit For
                End If
            Next
        End If

        MyBase.OnMouseDown(e)
    End Sub

    Public Sub ItemSelectionChange(ByVal sender As Object, ByVal Selected As Boolean)
        Try
            If Selected = True Then
                If _MultiSelect = False Or DesignMode = True Then
                    UnSelectAll()
                End If

                _SelectedIndex = CType(sender, ImageItem).ImageIndex
                _SelectedItem = CType(sender, ImageItem).Image

                If _SelectedIndices.Contains(_SelectedIndex) = True Then
                    _SelectedIndices.Remove(_SelectedIndex)
                End If
                If Not _SelectedIndex = -1 Then
                    _SelectedIndices.Add(_SelectedIndex)
                End If

                If _SelectedItems.Contains(CType(_SelectedItem, Image)) = True Then
                    _SelectedItems.Remove(CType(_SelectedItem, Image))
                End If
                If Not _SelectedItem Is Nothing Then
                    _SelectedItems.Add(CType(_SelectedItem, Image))
                End If
            Else
                If _SelectedIndices.Contains(CType(sender, ImageItem).ImageIndex) = True Then
                    _SelectedIndices.Remove(CType(sender, ImageItem).ImageIndex)
                End If

                If _SelectedItems.Contains(CType(sender, ImageItem).Image) = True Then
                    _SelectedItems.Remove(CType(sender, ImageItem).Image)
                End If

                If _SelectedIndices.Count > 0 Then
                    _SelectedIndex = _SelectedIndices.Item(_SelectedIndices.Count - 1)
                Else
                    _SelectedIndex = -1
                End If

                If _SelectedItems.Count > 0 Then
                    _SelectedItem = _SelectedItems.Item(_SelectedItems.Count - 1)
                Else
                    _SelectedItem = Nothing
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub SelectAll()
        Try
            RaiseEvent SelectAllItems()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub UnSelectAll()
        Try
            RaiseEvent UnSelectAllItems()
        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class

Friend Class ImageSelectorDesigner
    Inherits ControlDesigner

    Private MyControl As ImageSelector

    Public Overrides Sub Initialize(ByVal component As System.ComponentModel.IComponent)
        MyBase.Initialize(component)

        'Record instance of control we're designing
        MyControl = DirectCast(component, ImageSelector)

        'Hook up events
        Dim s As ISelectionService = DirectCast(GetService(GetType(ISelectionService)), ISelectionService)
        Dim c As IComponentChangeService = DirectCast(GetService(GetType(IComponentChangeService)), IComponentChangeService)
        AddHandler s.SelectionChanged, AddressOf OnSelectionChanged
        AddHandler c.ComponentRemoving, AddressOf OnComponentRemoving
        AddHandler c.ComponentAdding, AddressOf OnComponentAdding
        AddHandler c.ComponentChanging, AddressOf OnComponentChanging
    End Sub

    Private Sub OnSelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        MyControl.OnSelectionChanged()
    End Sub

    Private Sub OnComponentRemoving(ByVal sender As Object, ByVal e As ComponentEventArgs)
        Dim c As IComponentChangeService = DirectCast(GetService(GetType(IComponentChangeService)), IComponentChangeService)
        Dim button As ImageItem
        Dim h As IDesignerHost = DirectCast(GetService(GetType(IDesignerHost)), IDesignerHost)
        Dim i As Integer

        'If the user is removing a button
        If TypeOf e.Component Is ImageItem Then
            button = DirectCast(e.Component, ImageItem)
            If MyControl.Items.Contains(button) Then
                c.OnComponentChanging(MyControl, Nothing)
                MyControl.Items.Remove(button)

                RemoveHandler button.SelectionChanging, AddressOf MyControl.ItemSelectionChange

                c.OnComponentChanged(MyControl, Nothing, Nothing, Nothing)
                Return
            End If
        End If

        'If the user is removing the control itself
        If e.Component Is MyControl Then
            For i = MyControl.Items.Count - 1 To 0 Step -1
                button = MyControl.Items(i)
                c.OnComponentChanging(MyControl, Nothing)
                MyControl.Items.Remove(button)

                RemoveHandler button.SelectionChanging, AddressOf MyControl.ItemSelectionChange

                h.DestroyComponent(button)
                c.OnComponentChanged(MyControl, Nothing, Nothing, Nothing)
            Next
        End If

        MyControl.Invalidate()
    End Sub

    Private Sub OnComponentAdding(ByVal sender As Object, ByVal e As ComponentEventArgs)
        MyControl.CalculateLayout()
    End Sub

    Private Sub OnComponentChanging(ByVal sender As Object, ByVal e As ComponentChangingEventArgs)
        MyControl.CalculateLayout()
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        Dim s As ISelectionService = DirectCast(GetService(GetType(ISelectionService)), ISelectionService)
        Dim c As IComponentChangeService = DirectCast(GetService(GetType(IComponentChangeService)), IComponentChangeService)

        'Unhook events
        RemoveHandler s.SelectionChanged, AddressOf OnSelectionChanged
        RemoveHandler c.ComponentRemoving, AddressOf OnComponentRemoving

        MyBase.Dispose(disposing)
    End Sub

    Public Overrides ReadOnly Property AssociatedComponents() As System.Collections.ICollection
        Get
            Return MyControl.Items
        End Get
    End Property

    Public Overrides ReadOnly Property Verbs() As System.ComponentModel.Design.DesignerVerbCollection
        Get
            Dim v As New DesignerVerbCollection

            'Verb to add buttons
            v.Add(New DesignerVerb("&Add Image Item", AddressOf OnAddItem))

            Return v
        End Get
    End Property

    Private Sub OnAddItem(ByVal sender As Object, ByVal e As EventArgs)
        Dim button As ImageItem
        Dim h As IDesignerHost = DirectCast(GetService(GetType(IDesignerHost)), IDesignerHost)
        Dim dt As DesignerTransaction
        Dim c As IComponentChangeService = DirectCast(getservice(GetType(IComponentChangeService)), IComponentChangeService)

        'Add a new button to the collection
        dt = h.CreateTransaction("Add Button")
        button = DirectCast(h.CreateComponent(GetType(ImageItem)), ImageItem)
        c.OnComponentChanging(MyControl, Nothing)
        MyControl.Items.Add(button)
        c.OnComponentChanged(MyControl, Nothing, Nothing, Nothing)
        dt.Commit()

        AddHandler button.SelectionChanging, AddressOf MyControl.ItemSelectionChange

        MyControl.CalculateLayout()
    End Sub

    Protected Overrides Function GetHitTest(ByVal point As System.Drawing.Point) As Boolean
        Dim button As ImageItem
        Dim wrct As Rectangle

        point = MyControl.PointToClient(point)

        For Each button In MyControl.Items
            wrct = button.Bounds
            If wrct.Contains(point) Then Return True
        Next

        'Return MyBase.GetHitTest(point)
        Return False
    End Function

    'Protected Overrides Sub OnMouseEnter()

    'End Sub

    'Protected Overrides Sub OnMouseLeave()

    'End Sub
End Class