Imports System.ComponentModel


Public Class FamilySummaryRemarksViewerForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"
    Private _FamilyID As Integer
    Private _RelationID As Integer
    Friend WithEvents RemarksControl1 As REMARKSControl
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ExitButton As Button
    Friend WithEvents RemarksControl As REMARKSControl
    Private _FamilyRemarksDS As DataSet

#Region "Public Properties"
    <DefaultValue("UFCW\Claims\"), Browsable(True), System.ComponentModel.Description("Represents the application key used when accessing the registry.")> _
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = Value
        End Set
    End Property

    <Browsable(False), System.ComponentModel.Description("Used to load the Life Events from an external source.")> _
    Public Property FamilyRemarksDS() As DataSet
        Get
            Return _FamilyRemarksDS
        End Get
        Set(ByVal value As DataSet)
            _FamilyRemarksDS = Value
        End Set
    End Property
#End Region

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal FamilyID As Integer)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _FamilyID = FamilyID
        _RelationID = 0

    End Sub

    Public Sub New(ByVal FamilyID As Integer, ByVal RelationID As Integer)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _FamilyID = FamilyID
        _RelationID = RelationID

    End Sub
    Public Sub New(ByVal FamilyRemarksDS As DataSet)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        If FamilyRemarksDS.Tables.Count < 1 OrElse FamilyRemarksDS.Tables(0).Rows.Count < 1 Then Throw New ArgumentException("DataSet must be preloaded to use this method")

        _FamilyID = CInt(FamilyRemarksDS.Tables(0).Rows(0)("FAMILY_ID").ToString)
        _RelationID = CInt(FamilyRemarksDS.Tables(0).Rows(0)("RELATION_ID").ToString)

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
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.RemarksControl = New REMARKSControl()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.RemarksControl1 = New REMARKSControl()
        Me.ExitButton = New System.Windows.Forms.Button()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Top
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.RemarksControl)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel2.Controls.Add(Me.RemarksControl1)
        Me.SplitContainer1.Size = New System.Drawing.Size(627, 716)
        Me.SplitContainer1.SplitterDistance = 355
        Me.SplitContainer1.TabIndex = 4
        '
        'RemarksControl
        '
        Me.RemarksControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RemarksControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.RemarksControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.RemarksControl.BackColor = System.Drawing.SystemColors.Control
        Me.RemarksControl.Location = New System.Drawing.Point(8, 8)
        Me.RemarksControl.Name = "RemarksControl"
        Me.RemarksControl.Size = New System.Drawing.Size(627, 355)
        Me.RemarksControl.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label1.Location = New System.Drawing.Point(3, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Family Remarks"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label2.Location = New System.Drawing.Point(3, 1)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(97, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Individual Remarks"
        '
        'RemarksControl1
        '
        Me.RemarksControl1.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.RemarksControl1.BackColor = System.Drawing.SystemColors.Control
        Me.RemarksControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RemarksControl1.Location = New System.Drawing.Point(0, 0)
        Me.RemarksControl1.Name = "RemarksControl1"
        Me.RemarksControl1.Size = New System.Drawing.Size(627, 357)
        Me.RemarksControl1.TabIndex = 3
        '
        'ExitButton
        '
        Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ExitButton.Location = New System.Drawing.Point(540, 722)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(75, 23)
        Me.ExitButton.TabIndex = 5
        Me.ExitButton.Text = "Exit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'FamilySummaryRemarksViewerForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(627, 748)
        Me.Controls.Add(Me.ExitButton)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "FamilySummaryRemarksViewerForm"
        Me.Text = "Remarks Summary"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Custom Procedure"

    Private Sub LoadFamilyRemarks()
        Dim DS As DataSet
        Dim DT As DataTable

        Try
            Using WC As New GlobalCursor

                RemarksControl.ReadOnlyMode = True
                RemarksControl.LoadREMARKS(_FamilyID)

                DS = RemarksControl.REMARKDataSet
                If DS IsNot Nothing AndAlso DS.Tables.Count > 0 Then
                    DT = DS.Tables(0)
                    RemarksControl1.ReadOnlyMode = True
                    RemarksControl1.LoadREMARKS(_FamilyID, _RelationID, DS)
                End If

            End Using

        Catch ex As Exception
            Throw
        Finally

            DS = Nothing
            DT = Nothing

        End Try
    End Sub

    Private Sub FamilySummaryRemarksForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

        LoadFamilyRemarks()
    End Sub

#End Region

#Region "Form Events"

    Private Sub FamilySummaryRemarksForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        Try
            UFCWGeneral.SaveFormPosition(Me, _APPKEY)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ExitButton_Click(sender As System.Object, e As System.EventArgs) Handles ExitButton.Click
        Me.Close()
    End Sub

#End Region

End Class