Option Strict On

Imports System.ComponentModel
Imports System.ComponentModel.Design.Serialization

<TypeConverter(GetType(ImageItemConverter)), DesignTimeVisible(True), ToolboxItem(False)> _
Public Class ImageItem
    Inherits System.Windows.Forms.UserControl

    Public Event SelectionChanging(ByVal sender As Object, ByVal Selected As Boolean)

    Private _ImageIndex As Integer = -1
    Private _Image As Image
    Private _Selected As Boolean = False

    Friend _Owner As ImageSelector = Nothing

#Region "Constructor"
    Sub New()
        MyBase.New()

        InitializeComponent()
    End Sub

    Friend WithEvents Picture As System.Windows.Forms.Button
    Private Sub InitializeComponent()
        Me.Picture = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Picture
        '
        Me.Picture.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Picture.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Picture.ForeColor = System.Drawing.SystemColors.Control
        Me.Picture.Location = New System.Drawing.Point(0, 0)
        Me.Picture.Name = "Picture"
        Me.Picture.Size = New System.Drawing.Size(24, 24)
        Me.Picture.TabIndex = 0
        '
        'ImageItem
        '
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.Picture)
        Me.Name = "ImageItem"
        Me.Size = New System.Drawing.Size(24, 24)
        Me.ResumeLayout(False)

    End Sub
#End Region

#Region "Properties"
    <Browsable(False)> _
       Public Overrides Property AutoScroll() As Boolean
        Get
            Return MyBase.AutoScroll
        End Get
        Set(ByVal Value As Boolean)
            MyBase.AutoScroll = Value
        End Set
    End Property

    <Browsable(False)> _
    Public Overrides Property Dock() As System.Windows.Forms.DockStyle
        Get
            Return MyBase.Dock
        End Get
        Set(ByVal Value As System.Windows.Forms.DockStyle)
            MyBase.Dock = Value
        End Set
    End Property

    <DefaultValue(GetType(Object), Nothing), System.ComponentModel.Description("Gets or Sets the Owner of this control."), Browsable(False)> _
    Public Property Owner() As ImageSelector
        Get
            Return _Owner
        End Get
        Set(ByVal Value As ImageSelector)
            If Not _Owner Is Nothing Then
                RemoveHandler _Owner.SelectAllItems, AddressOf [Select]
                RemoveHandler _Owner.UnSelectAllItems, AddressOf [UnSelect]
            End If

            _Owner = Value

            If Not _Owner Is Nothing Then
                AddHandler _Owner.SelectAllItems, AddressOf [Select]
                AddHandler _Owner.UnSelectAllItems, AddressOf [UnSelect]
            End If
        End Set
    End Property

    '<TypeConverter(GetType(ImageIndexConverter))> _
    <DefaultValue(CInt(-1)), Browsable(False)> _
    Public Property ImageIndex() As Integer
        Get
            Return _ImageIndex
        End Get
        Set(ByVal Value As Integer)
            _ImageIndex = Value
            If Not Owner Is Nothing Then Owner.Invalidate()
        End Set
    End Property

    <DefaultValue(GetType(Object), Nothing), System.ComponentModel.Description("Gets or Sets the Image for this control."), Browsable(True)> _
    Public Property Image() As Image
        Get
            If Not Picture Is Nothing AndAlso Not Picture.Image Is Nothing Then
                Return Picture.Image
            ElseIf Not _Image Is Nothing Then
                Return _Image
            Else
                Return Nothing
            End If
            'Return _Image
        End Get
        Set(ByVal Value As Image)
            Picture.Image = Value
            '_Image = Value

            If Not Owner Is Nothing Then Owner.Invalidate()
        End Set
    End Property

    <DefaultValue(CBool(False)), System.ComponentModel.Description("Gets or Sets the Image for this control."), Browsable(False)> _
    Public Property Selected() As Boolean
        Get
            Return _Selected
        End Get
        Set(ByVal Value As Boolean)
            If Value = True Then
                Me.Select()
            Else
                Me.UnSelect()
            End If
        End Set
    End Property

    <BrowsableAttribute(False)> Protected Shadows ReadOnly Property DesignMode() As Boolean

        Get

            ' Returns true if this control or any of its ancestors is in design mode()

            If MyBase.DesignMode Then

                Return True

            Else

                Dim parent As Control = Me.Parent

                While Not parent Is Nothing

                    Dim site As ISite = Parent.Site

                    If Not site Is Nothing AndAlso site.DesignMode Then

                        Return True

                    End If

                    parent = parent.Parent

                End While

                Return False

            End If

        End Get

    End Property

#End Region

#Region "Overrides"
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim s As System.ComponentModel.Design.ISelectionService
        Dim a As ArrayList

        Try
            If e.Button = MouseButtons.Left Then
                If DesignMode Then
                    s = DirectCast(GetService(GetType(System.ComponentModel.Design.ISelectionService)), System.ComponentModel.Design.ISelectionService)
                    a = New ArrayList
                    a.Add(Me)
                    s.SetSelectedComponents(a)

                    If Me.Selected = True Then
                        Me.UnSelect()
                    Else
                        Me.Select()
                    End If

                    Me.Refresh()
                    Owner.Invalidate()
                Else
                    ItemClicked()
                End If
            End If

            MyBase.OnMouseDown(e)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Protected Overrides Sub OnKeyUp(ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter OrElse e.KeyCode = Keys.Space Then
            Me.ItemClicked()
        End If

        MyBase.OnKeyUp(e)
    End Sub

    Protected Overrides Sub OnEnter(ByVal e As System.EventArgs)
        'Dim g As Graphics = Me.Owner.CreateGraphics 'Me.CreateGraphics
        'Dim rct As Rectangle = Me.Bounds
        'rct.Inflate(10, 10)

        'MyBase.OnEnter(e)

        'g.DrawRectangle(SystemPens.ControlDarkDark, rct.X, rct.Y, rct.Width, rct.Height)

        ''Invalidate()


        'Me.BackColor = SystemColors.ControlDarkDark
        MyBase.OnEnter(e)
    End Sub

    Protected Overrides Sub OnLeave(ByVal e As System.EventArgs)
        'If Me.Selected = True Then
        '    Me.BackColor = SystemColors.Highlight
        'Else
        '    Me.BackColor = SystemColors.Control
        'End If

        MyBase.OnLeave(e)
    End Sub
#End Region

    Private Sub Picture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Picture.Click
        Try
            'Me.OnEnter(New EventArgs)
            Me.Focus()
            ItemClicked()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Picture_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Picture.DoubleClick
        Try
            'Me.OnEnter(New EventArgs)
            Me.Focus()
            ItemClicked()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub ItemClicked()
        Try
            If _Selected = True Then
                Me.UnSelect()
            Else
                Me.Select()
            End If

            If Not Owner Is Nothing Then Owner.Invalidate()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Overloads Sub [Select]()
        Try
            RaiseEvent SelectionChanging(Me, True)

            _Selected = True
            Me.BackColor = System.Drawing.SystemColors.Highlight

            If Not Owner Is Nothing Then Owner.Invalidate()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Overloads Sub [UnSelect]()
        Try
            RaiseEvent SelectionChanging(Me, False)

            _Selected = False
            Me.BackColor = System.Drawing.SystemColors.Control

            If Not Owner Is Nothing Then Owner.Invalidate()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub SetItemImage(ByVal Image As Image)
        _Image = Image
        Picture.Image = Image
    End Sub

    'Private Sub Picture_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Picture.Enter
    '    MyBase.OnEnter(e)
    'End Sub

    'Private Sub Picture_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Picture.Leave
    '    MyBase.OnLeave(e)
    'End Sub

    'Private Sub ImageItem_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
    '    Dim wrct As Rectangle

    '    Try
    '        If Not _Image Is Nothing Then
    '            wrct = Bounds
    '            'wrct.Inflate(-3, -3)

    '            e.Graphics.DrawImage(_Image, wrct)
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    'Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
    '    Dim wrct As Rectangle

    '    Try
    '        If Not _Image Is Nothing Then
    '            wrct = Bounds
    '            wrct.Inflate(10, 10)

    '            e.Graphics.DrawImage(_Image, wrct)
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub
End Class

Friend Class ImageItemConverter
    Inherits TypeConverter

    Public Overloads Overrides Function CanConvertTo(ByVal context As ITypeDescriptorContext, ByVal destType As Type) As Boolean
        If destType Is GetType(InstanceDescriptor) Then
            Return True
        End If

        Return MyBase.CanConvertTo(context, destType)
    End Function

    Public Overloads Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destType As Type) As Object
        If destType Is GetType(InstanceDescriptor) Then
            Dim ci As System.Reflection.ConstructorInfo = GetType(ImageItem).GetConstructor(System.Type.EmptyTypes)

            Return New InstanceDescriptor(ci, Nothing, False)
        End If

        Return MyBase.ConvertTo(context, culture, value, destType)
    End Function
End Class

Public Class ImageItemCollection
    Inherits System.Collections.CollectionBase

    Private Owner As ImageSelector

    Friend Sub New(ByVal Owner As ImageSelector)
        Me.Owner = Owner
    End Sub

    Default Public ReadOnly Property Item(ByVal Index As Integer) As ImageItem
        Get
            If Index >= 0 And Index < List.Count Then
                Return DirectCast(List(Index), ImageItem)
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public Function Contains(ByVal Item As ImageItem) As Boolean
        Return List.Contains(Item)
    End Function

    Public Function IndexOf(ByVal Item As ImageItem) As Integer
        List.IndexOf(Item)
    End Function

    Public Function Add(ByVal Item As ImageItem) As Integer
        Dim i As Integer

        i = List.Add(Item)
        Item.ImageIndex = i
        Item.Owner = Owner

        If Not Item.Image Is Nothing Then
            Item.SetItemImage(Item.Image)
        ElseIf Not Owner Is Nothing AndAlso Not Owner.ImageList Is Nothing AndAlso _
                    Not Owner.ImageList.Images Is Nothing AndAlso _
                    Owner.ImageList.Images.Count > i Then
            Item.Image = Owner.ImageList.Images(i)
        Else
            Item.Image = Nothing
        End If

        AddHandler Item.SelectionChanging, AddressOf Owner.ItemSelectionChange

        Owner.CalculateLayout()

        Return i
    End Function

    Public Sub Remove(ByVal Item As ImageItem)
        List.Remove(Item)
        Item.Owner = Nothing
        Owner.CalculateLayout()
    End Sub
End Class
