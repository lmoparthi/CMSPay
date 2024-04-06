Imports System.ComponentModel

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class RouteDialog
    Inherits System.Windows.Forms.Form

    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _DocType As String
    Private _ClaimID As Integer
    Private _Mode As String

    ReadOnly _DomainUser As String = SystemInformation.UserName

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal ClaimID As Integer, ByVal DocType As String, ByVal Mode As String)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _ClaimID = ClaimID
        _DocType = DocType
        _Mode = Mode
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
    Friend WithEvents CancelActionButton As System.Windows.Forms.Button
    Friend WithEvents RouteButton As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents RouteToComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents CommentTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim Resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(RouteDialog))
        Me.CancelActionButton = New System.Windows.Forms.Button
        Me.RouteButton = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.RouteToComboBox = New System.Windows.Forms.ComboBox
        Me.CommentTextBox = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'CancelActionButton
        '
        Me.CancelActionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelActionButton.Location = New System.Drawing.Point(8, 174)
        Me.CancelActionButton.Name = "CancelActionButton"
        Me.CancelActionButton.TabIndex = 4
        Me.CancelActionButton.Text = "&Cancel"
        '
        'RouteButton
        '
        Me.RouteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RouteButton.Enabled = False
        Me.RouteButton.Location = New System.Drawing.Point(312, 174)
        Me.RouteButton.Name = "RouteButton"
        Me.RouteButton.TabIndex = 3
        Me.RouteButton.Text = "&Route"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(54, 16)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Route To:"
        '
        'RouteToComboBox
        '
        Me.RouteToComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RouteToComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.RouteToComboBox.Location = New System.Drawing.Point(64, 12)
        Me.RouteToComboBox.Name = "RouteToComboBox"
        Me.RouteToComboBox.Size = New System.Drawing.Size(320, 21)
        Me.RouteToComboBox.TabIndex = 1
        '
        'CommentTextBox
        '
        Me.CommentTextBox.AcceptsReturn = True
        Me.CommentTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CommentTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.CommentTextBox.Location = New System.Drawing.Point(16, 64)
        Me.CommentTextBox.MaxLength = 300
        Me.CommentTextBox.Multiline = True
        Me.CommentTextBox.Name = "CommentTextBox"
        Me.CommentTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.CommentTextBox.Size = New System.Drawing.Size(368, 104)
        Me.CommentTextBox.TabIndex = 2
        Me.CommentTextBox.Text = ""
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(99, 16)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Routing Comment:"
        '
        'RouteDialog
        '
        Me.AcceptButton = Me.RouteButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(394, 206)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.CommentTextBox)
        Me.Controls.Add(Me.RouteToComboBox)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.CancelActionButton)
        Me.Controls.Add(Me.RouteButton)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RouteDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Route Claim - "
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Properties"
    <Browsable(True), System.ComponentModel.Description("Gets or Sets the Doc Type.")> _
    Public Property DocType() As String
        Get
            Return _DocType
        End Get
        Set(ByVal value As String)
            _DocType = Value
        End Set
    End Property

    <Browsable(True), System.ComponentModel.Description("Gets or Sets the Routing Recipient.")> _
    Public Property Recipient() As String
        Get
            Return RouteToComboBox.Text
        End Get
        Set(ByVal value As String)
            RouteToComboBox.Text = Value
        End Set
    End Property

    <Browsable(True), System.ComponentModel.Description("Gets or Sets the Routing Comment.")> _
    Public Property Comment() As String
        Get
            Return CommentTextBox.Text
        End Get
        Set(ByVal value As String)
            CommentTextBox.Text = Value
        End Set
    End Property
#End Region

    Private Sub RouteDialog_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.Text = "Route Claim - " & _ClaimID
            LoadRecipients()

            RouteToComboBox.Select()

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub LoadRecipients()
        Dim DT As DataTable
        Dim BlankRow As DataRow

        Try
            DT = CMSDALFDBMD.RetrieveRoutingUsers(_DocType, _DomainUser.ToUpper, _Mode.ToUpper)
            BlankRow = DT.NewRow
            BlankRow("USERID") = ""
            BlankRow("DOC_CLASS") = ""
            BlankRow("DOC_TYPE") = ""
            DT.Rows.InsertAt(BlankRow, 0)

            RouteToComboBox.DataSource = DT
            RouteToComboBox.ValueMember = "USERID"
            RouteToComboBox.DisplayMember = "USERID"

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CancelActionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelActionButton.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub RouteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RouteButton.Click
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub RouteToComboBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RouteToComboBox.TextChanged
        Try
            If RouteToComboBox.Text <> "" AndAlso CommentTextBox.Text <> "" Then
                RouteButton.Enabled = True
            Else
                RouteButton.Enabled = False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RouteToComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RouteToComboBox.SelectedIndexChanged
        Try
            If RouteToComboBox.Text <> "" AndAlso CommentTextBox.Text <> "" Then
                RouteButton.Enabled = True
            Else
                RouteButton.Enabled = False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub CommentTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CommentTextBox.TextChanged
        Try
            If CommentTextBox.Text <> "" AndAlso RouteToComboBox.Text <> "" Then
                RouteButton.Enabled = True
            Else
                RouteButton.Enabled = False
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class