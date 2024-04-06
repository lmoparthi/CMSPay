
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class PatientLookUpForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal LastName As String, Optional ByVal FirstName As String = "", Optional ByVal ReturnRestricted As Boolean = False)

        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        _LastName = LastName
        _FirstName = FirstName
        _ReturnRestricted = ReturnRestricted

        MemberSearchControl.LastName = LastName
        MemberSearchControl.FirstName = FirstName
        MemberSearchControl.ReturnRestricted = ReturnRestricted

    End Sub

    Public Sub New(ByVal PatSSN As Integer, Optional ByVal ReturnRestricted As Boolean = False)

        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        _PatSSN = PatSSN
        _ReturnRestricted = ReturnRestricted

        MemberSearchControl.PatSSN = PatSSN
        MemberSearchControl.ReturnRestricted = ReturnRestricted

    End Sub

    Public Sub New(ByVal ReturnRestricted As Boolean)

        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        _ReturnRestricted = ReturnRestricted

        MemberSearchControl.ReturnRestricted = ReturnRestricted

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
    Friend WithEvents MemberSearchControl As MemberSearchControl
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.ResultsMenu = New System.Windows.Forms.ContextMenu
        Me.ExportMenuItem = New System.Windows.Forms.MenuItem
        Me.ColumnSelectorMenuItem = New System.Windows.Forms.MenuItem
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.MemberSearchControl = New MemberSearchControl
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
        Me.MemberSearchControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MemberSearchControl.AppKey = "UFCW\Claims\"
        Me.MemberSearchControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.MemberSearchControl.Location = New System.Drawing.Point(0, 0)
        Me.MemberSearchControl.Name = "MemberSearchControl"
        Me.MemberSearchControl.ReturnRestricted = False
        Me.MemberSearchControl.Size = New System.Drawing.Size(743, 493)
        Me.MemberSearchControl.TabIndex = 0
        '
        'PatientLookUpForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(743, 518)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MemberSearchControl)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.KeyPreview = True
        Me.Name = "PatientLookUpForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "Member Lookup"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"
    Private _FamilyID As Integer = Nothing
    Private _RelationID As Integer = Nothing
    Private _PartSSN As Integer = Nothing
    Private _PatSSN As Integer = Nothing
    Private _FirstName As String = Nothing
    Private _LastName As String = Nothing
    Private _PatientRow As DataRow = Nothing
    Private _ReturnRestricted As Boolean = False

#Region "Properties"
    Public Property ReturnRestricted() As Boolean
        Get
            Return _ReturnRestricted
        End Get
        Set(ByVal value As Boolean)
            _ReturnRestricted = Value
        End Set
    End Property

    Public Property FamilyID() As Integer
        Get
            Return _FamilyID
        End Get
        Set(ByVal value As Integer)
            _FamilyID = Value
        End Set
    End Property

    Public Property RelationID() As Integer
        Get
            Return _RelationID
        End Get
        Set(ByVal value As Integer)
            _RelationID = Value
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

    Public ReadOnly Property PartSSN() As Integer
        Get
            Return _PartSSN
        End Get
    End Property

    Public Property PatSSN() As Integer
        Get
            Return _PatSSN
        End Get
        Set(ByVal value As Integer)
            _PatSSN = Value
        End Set
    End Property

    Public Property FirstName() As String
        Get
            Return _FirstName
        End Get
        Set(ByVal value As String)
            _FirstName = Value
        End Set
    End Property

    Public Property LastName() As String
        Get
            Return _LastName
        End Get
        Set(ByVal value As String)
            _LastName = Value
        End Set
    End Property

#End Region

#Region "Form Events"

    Private Sub PatientLookUpForm_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Enter
        Me.MemberSearchControl.ActionButton.PerformClick()
    End Sub

    Public Sub PatientSelected(ByVal e As Object, ByVal dr As DataRow) Handles MemberSearchControl.ActionSelect

        Try

            If dr IsNot Nothing Then

                If CType(dr("SSNO"), String).Contains("Restricted") Then
                    _PatSSN = CType(Nothing, Integer)
                Else
                    _PatSSN = CInt(dr("SSNO"))
                End If

                If CType(dr("PART_SSNO"), String).Contains("Restricted") Then
                    _PartSSN = CType(Nothing, Integer)
                Else
                    _PartSSN = CInt(dr("PART_SSNO"))
                End If

                _FirstName = CStr(dr("FIRST_NAME"))
                _LastName = CStr(dr("LAST_NAME"))
                _FamilyID = CInt(dr("FAMILY_ID"))
                _RelationID = CInt(dr("RELATION_ID"))
                _PatientRow = dr

            End If

        Catch ex As Exception

            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If

        Finally
            Me.Hide()
            Me.DialogResult = DialogResult.OK

        End Try

    End Sub
    Private Sub CancelButton_Click(ByVal sender As System.Object) Handles MemberSearchControl.ActionCancel
        Try

            _PartSSN = Nothing
            _PatSSN = Nothing
            _FamilyID = Nothing
            _RelationID = Nothing
            _FirstName = Nothing
            _LastName = Nothing
            _PatientRow = Nothing

        Catch ex As Exception

            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If

        Finally
            Me.Hide()
            Me.DialogResult = DialogResult.Cancel

        End Try

    End Sub

    Private Sub MemberLookUpForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SaveSettings()
    End Sub

    Private Sub MemberLookUpForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

        Dim lWindowState As FormWindowState = Me.WindowState
        SaveSetting(_AppKey, Me.Name & "\Settings", "WindowState", CInt(lWindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = lWindowState

    End Sub

#End Region

End Class