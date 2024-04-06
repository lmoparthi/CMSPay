
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class ProviderLookUpForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal ProvName As String)

        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        _ProvName = ProvName

    End Sub

    Public Sub New(ByVal ProvTIN As Integer)

        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        _ProvTIN = ProvTIN

    End Sub

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub
    'Form overrides dispose to clean up the component list.
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
    Friend WithEvents ResultsMenu As System.Windows.Forms.ContextMenu
    Friend WithEvents ExportMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ColumnSelectorMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents ProviderSearchControl As ProviderSearchControl
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ResultsMenu = New System.Windows.Forms.ContextMenu
        Me.ExportMenuItem = New System.Windows.Forms.MenuItem
        Me.ColumnSelectorMenuItem = New System.Windows.Forms.MenuItem
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.ProviderSearchControl = New ProviderSearchControl
        Me.SuspendLayout()
        '
        'ResultsMenu
        '
        Me.ResultsMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.ExportMenuItem, Me.ColumnSelectorMenuItem})
        '
        'ExportMenuItem
        '
        Me.ExportMenuItem.Index = 0
        Me.ExportMenuItem.Text = "Export"
        '
        'ColumnSelectorMenuItem
        '
        Me.ColumnSelectorMenuItem.Index = 1
        Me.ColumnSelectorMenuItem.Text = "Column Selector"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 496)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(743, 22)
        Me.StatusStrip1.TabIndex = 1
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'MemberSearchControl
        '
        Me.ProviderSearchControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProviderSearchControl.AppKey = "UFCW\Claims\"
        Me.ProviderSearchControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ProviderSearchControl.Location = New System.Drawing.Point(0, 0)
        Me.ProviderSearchControl.Name = "ProviderSearchControl"
        Me.ProviderSearchControl.Size = New System.Drawing.Size(743, 493)
        Me.ProviderSearchControl.TabIndex = 0
        '
        'ProviderLookUpForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(743, 518)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.ProviderSearchControl)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.KeyPreview = True
        Me.Name = "ProviderLookUpForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "Provider Lookup"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"
    Private _ProvTIN As Integer?
    Private _ProvName As String
    Private _ProviderDR As DataRow

#Region "Properties"

    Public Property ProvTIN() As Integer?
        Get
            Return _ProvTIN
        End Get
        Set(ByVal value As Integer?)
            _ProvTIN = value
        End Set
    End Property

    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = Value
        End Set
    End Property

    Public Property ProvName() As String
        Get
            Return _ProvName
        End Get
        Set(ByVal value As String)
            _ProvName = Value
        End Set
    End Property

    Public ReadOnly Property SelectedProvider() As DataRow
        Get
            Return _ProviderDR
        End Get
    End Property

#End Region

#Region "Form Events"

    Private Sub ProviderLookUpForm_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Enter
        Me.ProviderSearchControl.ActionButton.PerformClick()
    End Sub

    Public Sub ProviderSelected(ByVal e As Object, ByVal dr As DataRow) Handles ProviderSearchControl.ActionSelect

        Try

            If dr IsNot Nothing Then
                _ProvTIN = CInt(dr("TAXID"))
                _ProvName = CStr(dr("NAME1"))
                _ProviderDR = dr
            End If

        Catch ex As Exception
            Throw
        Finally
            Me.Hide()
            Me.DialogResult = DialogResult.OK

        End Try

    End Sub
    Private Sub CancelButton_Click(ByVal sender As System.Object) Handles ProviderSearchControl.ActionCancel
        Try

            _ProvTIN = Nothing
            _ProvName = Nothing
            _ProviderDR = Nothing

        Catch ex As Exception
            Throw
        Finally
            Me.Hide()
            Me.DialogResult = DialogResult.Cancel

        End Try

    End Sub

    Private Sub ProviderLookUpForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SaveSettings()
    End Sub

    Private Sub ProviderLookUpForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetSettings()
    End Sub

    Private Sub SetSettings()

        Me.Top = If(CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(_AppKey, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)

    End Sub

    Private Sub SaveSettings()

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString)

    End Sub

#End Region

End Class