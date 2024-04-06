
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
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ResultsMenu = New System.Windows.Forms.ContextMenu()
        Me.ExportMenuItem = New System.Windows.Forms.MenuItem()
        Me.ColumnSelectorMenuItem = New System.Windows.Forms.MenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ProviderSearchControl = New ProviderSearchControl()
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
        'ProviderSearchControl
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

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"
    Private _ProvID As Integer?
    Private _ProvTIN As Integer?
    Private _ProvNPI As Decimal?
    Private _ProvName As String
    Private _ProviderDR As DataRow

#Region "Properties"

    Public Property ProvNPI() As Decimal?
        Get
            Return _ProvNPI
        End Get
        Set(ByVal value As Decimal?)
            _ProvNPI = value
        End Set
    End Property

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
            _APPKEY = value
        End Set
    End Property

    Public Property ProvName() As String
        Get
            Return _ProvName
        End Get
        Set(ByVal value As String)
            _ProvName = value
        End Set
    End Property

    Public Property ProviderID() As Integer?
        Get
            Return _ProvID
        End Get
        Set(ByVal value As Integer?)
            _ProvID = value
        End Set
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
                _ProvNPI = CDec(If(IsDBNull(dr("NPI")), Nothing, dr("NPI")))
                _ProvID = CInt(dr("PROVIDER_ID"))
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
            _ProvID = Nothing
            _ProvNPI = Nothing

        Catch ex As Exception

            Throw

        Finally
            Me.Hide()
            Me.DialogResult = DialogResult.Cancel

        End Try

    End Sub

    Private Sub ProviderLookUpForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        UFCWGeneral.SaveFormPosition(Me, _APPKEY)
    End Sub

    Private Sub ProviderLookUpForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()
    End Sub

#End Region

End Class