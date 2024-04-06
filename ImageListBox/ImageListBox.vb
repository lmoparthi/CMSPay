Option Strict On

Imports System.Drawing
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

<System.Diagnostics.DebuggerStepThrough()>
Public Class ImageListBox
    Inherits System.Windows.Forms.ListBox

    <DllImport("user32.dll")>
    Private Shared Function GetAsyncKeyState(ByVal vKey As System.Windows.Forms.Keys) As Short
    End Function

    Public Event DrawingImage(ByVal itemIndex As Integer, ByRef image As System.Drawing.Image)
    Public Event Collapse()

    Friend WithEvents DefaultIList As System.Windows.Forms.ImageList
    Friend WithEvents StandardIList As System.Windows.Forms.ImageList

    Private _Components As System.ComponentModel.IContainer
    Private _AutoSizeImage As Boolean = True

    Public Property ImageAutoSize() As Boolean
        Get
            Return _AutoSizeImage
        End Get
        Set(ByVal Value As Boolean)
            _AutoSizeImage = Value
        End Set
    End Property

    Friend WithEvents LostFocusTimer As System.Timers.Timer

    Private Sub InitializeComponent()

        Me._Components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ImageListBox))
        Me.DefaultIList = New System.Windows.Forms.ImageList(Me._Components)
        Me.StandardIList = New System.Windows.Forms.ImageList(Me._Components)
        Me.LostFocusTimer = New System.Timers.Timer
        CType(Me.LostFocusTimer, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'DefaultIList
        '
        Me.DefaultIList.ImageSize = New System.Drawing.Size(16, 16)
        Me.DefaultIList.TransparentColor = System.Drawing.Color.Transparent
        '
        'StandardIList
        '
        Me.StandardIList.ImageSize = New System.Drawing.Size(16, 16)
        Me.StandardIList.ImageStream = CType(resources.GetObject("StandardIList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.StandardIList.TransparentColor = System.Drawing.Color.Transparent
        '
        'LostFocusTimer
        '
        Me.LostFocusTimer.SynchronizingObject = Me
        '
        'ImageListBox
        '
        CType(Me.LostFocusTimer, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

    Sub New()
        MyBase.New()

        InitializeComponent()

        Me.DrawMode = DrawMode.OwnerDrawVariable

        'Dim backgroundBrush As Brush = New TextureBrush(DefaultIList.Images(0), WrapMode.Clamp)
        'Dim backgroundBrush As Brush = New TextureBrush(DefaultIList.Images(0), WrapMode.Clamp)
        'Dim lb As New LinearGradientBrush(New Rectangle(0, 0, Me.Width, 100), Color.Red, Color.Yellow, LinearGradientMode.Horizontal)

        'listBoxBrushes = New Brush() {backgroundBrush, Brushes.LemonChiffon, lb, Brushes.PeachPuff}
    End Sub

    Private Sub ImageListBox_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles MyBase.DrawItem

        Dim Selected As Boolean
        Dim DisplayText As String
        Dim StringSize As SizeF
        Dim Img As System.Drawing.Image
        Dim R As Rectangle

        If e.Index = -1 Then Exit Sub
        If Me.Items.Count = 0 Then Exit Sub

        e.DrawBackground()

        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            Selected = True
        Else
            Selected = False
        End If

        DisplayText = Me.Items(e.Index).ToString

        StringSize = e.Graphics.MeasureString(DisplayText, Me.Font)

        Img = GetImage(e.Index)

        If Img.Height > StringSize.Height Then
            R = New Rectangle(e.Bounds.X + 1, e.Bounds.Y + 1, Img.Width, Img.Height)
        Else
            R = New Rectangle(e.Bounds.X + 1, CInt(e.Bounds.Y + (StringSize.Height - Img.Height) / 2), Img.Width, Img.Height)
        End If

        e.Graphics.DrawImage(Img, R)

        If Img.Height > StringSize.Height Then
            e.Graphics.DrawString(DisplayText, Me.Font, Brushes.Black, e.Bounds.X + Img.Width + 1, e.Bounds.Y + ((Img.Height - StringSize.Height) + 1) / 2)
        Else
            e.Graphics.DrawString(DisplayText, Me.Font, Brushes.Black, e.Bounds.X + Img.Width + 1, e.Bounds.Y)
        End If

        If Img.Height > StringSize.Height Then
            'Now set height to taller of default and text height
            Me.ItemHeight = Img.Height + 1
        Else
            Me.ItemHeight = CInt(StringSize.Height)
        End If

        e.DrawFocusRectangle()

    End Sub

    Private Sub ImageListBox_MeasureItem(ByVal sender As Object, ByVal e As System.Windows.Forms.MeasureItemEventArgs) Handles MyBase.MeasureItem
        Dim DisplayText As String
        Dim StringSize As SizeF
        Dim Img As System.Drawing.Image

        If Me.Items.Count = 0 Then Exit Sub

        DisplayText = Me.Items(e.Index).ToString

        'Get width & height of string
        StringSize = e.Graphics.MeasureString(DisplayText, Me.Font)

        'Account for top margin
        StringSize.Height += 2

        Img = GetImage(e.Index)

        If Img.Height > StringSize.Height Then
            'Now set height to taller of default and text height
            e.ItemHeight = Img.Height + 1
        Else
            e.ItemHeight = CInt(StringSize.Height)
        End If
    End Sub

    Private Function GetImage(ByVal index As Integer) As Image
        Dim Img As System.Drawing.Image

        RaiseEvent DrawingImage(index, Img)

        If Img Is Nothing Then
            If TypeOf Me.Items(index) Is ImageListBoxItem AndAlso Not CType(Me.Items(index), ImageListBoxItem).Pic Is Nothing Then
                Img = CType(Me.Items(index), ImageListBoxItem).Pic
            Else
                Img = StandardIList.Images(0)
            End If

        End If

        If StandardIList.Images.Count > 1 Then
            StandardIList.Images.RemoveAt(1)
        End If
        StandardIList.Images.Add(Img)

        If _AutoSizeImage = True Then
            Return StandardIList.Images(1)
        Else
            Return Img
        End If
    End Function

    Private Sub LostFocusTimer_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles LostFocusTimer.Elapsed

        If Not LostFocusTimer.Enabled Then Exit Sub

        Dim MousePosition As Point = PointToClient(Control.MousePosition)
        Dim Rect As Rectangle = New Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width + SystemInformation.VerticalScrollBarWidth, ClientRectangle.Height)
        Dim MouseIsOver As Boolean = Rect.Contains(MousePosition)

        If (GetAsyncKeyState(CType(1, Keys)) < 0 OrElse GetAsyncKeyState(CType(2, Keys)) < 0) Then
            If Not MouseIsOver OrElse Not Me.Focused Then
                RaiseEvent Collapse()
                LostFocusTimer.Enabled = False
            End If
        End If
    End Sub

    Private Sub ImageListBox_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.GotFocus
        LostFocusTimer.Enabled = True
    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        LostFocusTimer.Enabled = False
        MyBase.Dispose(disposing)
    End Sub

    Public Sub Clear()
        Me.SuspendLayout()
        Me.Items.Clear()
        Me.ResumeLayout(True)
    End Sub
End Class

<System.Diagnostics.DebuggerStepThrough()>
Public Class ImageListBoxItem
    Private _Text As String
    Private _Pic As System.Drawing.Image
    Private _Tag As Object

    Public Sub New()
        _Text = ""
        _Pic = Nothing
        _Tag = Nothing
    End Sub
    Public Sub New(ByVal Text As String)
        _Text = Text
    End Sub
    Public Sub New(ByVal Text As String, ByVal Pic As System.Drawing.Image)
        _Text = Text
        _Pic = Pic
    End Sub
    Public Sub New(ByVal Text As String, ByVal Tag As System.Object)
        _Text = Text
        _Tag = Tag
    End Sub
    Public Sub New(ByVal Text As String, ByVal Pic As System.Drawing.Image, ByVal Tag As System.Object)
        _Text = Text
        _Pic = Pic
        _Tag = Tag
    End Sub

    Public Property Text() As String
        Get
            Return _Text
        End Get
        Set(ByVal Value As String)
            _Text = Value
        End Set
    End Property

    Public Property Pic() As System.Drawing.Image
        Get
            Return _Pic
        End Get
        Set(ByVal Value As System.Drawing.Image)
            _Pic = Value
        End Set
    End Property

    Public Property Tag() As System.Object
        Get
            Return _Tag
        End Get
        Set(ByVal Value As System.Object)
            _Tag = Value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return _Text
    End Function
End Class
