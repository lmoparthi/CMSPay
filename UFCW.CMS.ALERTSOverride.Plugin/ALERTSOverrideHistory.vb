Option Strict On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ComponentModel

<SharedInterfaces.PlugInAttribute("ALERTS Override", 5, "Main", 13)> Public Class ALERTSOverrideHistory
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private Shared ReadOnly _DomainUser As String = SystemInformation.UserName

    Private _APPKEY As String = "UFCW\Claims\"

    Private _IMessage As SharedInterfaces.IMessage
    Private _OpenIndex As Integer
    Private _UniqueID As String
    Private _FamilyID As Integer
    Private _RelationIDs As ArrayList


    Private _ALERTSOverrideHistoryFrm As ALERTSOverrideHistory

    Friend WithEvents RelationIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnHistory As System.Windows.Forms.Button

#Region " Windows Form Designer generated code "

    Public Sub New(ByVal objMsg As SharedInterfaces.IMessage, ByVal openIndex As Integer)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        'Add any initialization after the InitializeComponent() call
        _IMessage = objMsg
        _OpenIndex = openIndex

        Me.WindowState = FormWindowState.Normal
    End Sub

    Public Sub New()

        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.WindowState = FormWindowState.Normal
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
    Public Overloads Sub Dispose()
        If _ALERTSOverrideHistoryFrm IsNot Nothing Then _ALERTSOverrideHistoryFrm.Dispose()
        _ALERTSOverrideHistoryFrm = Nothing
        MyBase.Dispose()
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ALERTSOverrideHistory))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SelectFamilyButton = New System.Windows.Forms.Button()
        Me.FamilyPanel = New System.Windows.Forms.Panel()
        Me.RelationIDTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnshow = New System.Windows.Forms.Button()
        Me.FamilyIDTextBox = New System.Windows.Forms.TextBox()
        Me.FamilyIdLabel = New System.Windows.Forms.Label()
        Me.AlertControl = New AlertControl()
        Me.btnHistory = New System.Windows.Forms.Button()
        Me.FamilyPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'SelectFamilyButton
        '
        Me.SelectFamilyButton.Location = New System.Drawing.Point(140, 20)
        Me.SelectFamilyButton.Name = "SelectFamilyButton"
        Me.SelectFamilyButton.Size = New System.Drawing.Size(24, 20)
        Me.SelectFamilyButton.TabIndex = 1
        Me.SelectFamilyButton.Text = "..."
        Me.ToolTip1.SetToolTip(Me.SelectFamilyButton, "Search for Family Id by Name")
        '
        'FamilyPanel
        '
        Me.FamilyPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FamilyPanel.Controls.Add(Me.RelationIDTextBox)
        Me.FamilyPanel.Controls.Add(Me.Label1)
        Me.FamilyPanel.Controls.Add(Me.btnClear)
        Me.FamilyPanel.Controls.Add(Me.btnshow)
        Me.FamilyPanel.Controls.Add(Me.FamilyIDTextBox)
        Me.FamilyPanel.Controls.Add(Me.FamilyIdLabel)
        Me.FamilyPanel.Controls.Add(Me.SelectFamilyButton)
        Me.FamilyPanel.Location = New System.Drawing.Point(12, 12)
        Me.FamilyPanel.Name = "FamilyPanel"
        Me.FamilyPanel.Size = New System.Drawing.Size(864, 48)
        Me.FamilyPanel.TabIndex = 7
        '
        'txtRelationid
        '
        Me.RelationIDTextBox.Location = New System.Drawing.Point(242, 20)
        Me.RelationIDTextBox.MaxLength = 3
        Me.RelationIDTextBox.Name = "txtRelationid"
        Me.RelationIDTextBox.Size = New System.Drawing.Size(35, 20)
        Me.RelationIDTextBox.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(179, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Relation ID"
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(415, 17)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(72, 23)
        Me.btnClear.TabIndex = 4
        Me.btnClear.Text = "Clear"
        '
        'btnshow
        '
        Me.btnshow.Location = New System.Drawing.Point(322, 18)
        Me.btnshow.Name = "btnshow"
        Me.btnshow.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnshow.Size = New System.Drawing.Size(72, 23)
        Me.btnshow.TabIndex = 3
        Me.btnshow.Text = "Search"
        '
        'FamilyIdTextBox
        '
        Me.FamilyIDTextBox.Location = New System.Drawing.Point(70, 20)
        Me.FamilyIDTextBox.MaxLength = 9
        Me.FamilyIDTextBox.Name = "FamilyIdTextBox"
        Me.FamilyIDTextBox.Size = New System.Drawing.Size(64, 20)
        Me.FamilyIDTextBox.TabIndex = 0
        '
        'FamilyIdLabel
        '
        Me.FamilyIdLabel.AutoSize = True
        Me.FamilyIdLabel.Location = New System.Drawing.Point(16, 21)
        Me.FamilyIdLabel.Name = "FamilyIdLabel"
        Me.FamilyIdLabel.Size = New System.Drawing.Size(50, 13)
        Me.FamilyIdLabel.TabIndex = 3
        Me.FamilyIdLabel.Text = "Family ID"
        '
        'AlertControl
        '
        Me.AlertControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AlertControl.AppKey = "UFCW\RegMaster\"
        Me.AlertControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.AlertControl.Enabled = False
        Me.AlertControl.Location = New System.Drawing.Point(0, 65)
        Me.AlertControl.Name = "AlertControl"
        Me.AlertControl.ReadOnlyMode = False
        Me.AlertControl.Size = New System.Drawing.Size(876, 408)
        Me.AlertControl.TabIndex = 5
        Me.AlertControl.VisibleHistory = True
        '
        'btnHistory
        '
        Me.btnHistory.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnHistory.Location = New System.Drawing.Point(792, 479)
        Me.btnHistory.Name = "btnHistory"
        Me.btnHistory.Size = New System.Drawing.Size(72, 23)
        Me.btnHistory.TabIndex = 8
        Me.btnHistory.Text = "History"
        '
        'ALERTSOverrideHistory
        '
        Me.AcceptButton = Me.btnshow
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(876, 510)
        Me.Controls.Add(Me.btnHistory)
        Me.Controls.Add(Me.FamilyPanel)
        Me.Controls.Add(Me.AlertControl)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(500, 500)
        Me.Name = "ALERTSOverrideHistory"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Alerts"
        Me.ToolTip1.SetToolTip(Me, "Manage Alerts")
        Me.TopMost = True
        Me.FamilyPanel.ResumeLayout(False)
        Me.FamilyPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents AlertControl As AlertControl
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents FamilyPanel As System.Windows.Forms.Panel
    Friend WithEvents btnshow As System.Windows.Forms.Button
    Friend WithEvents FamilyIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents FamilyIdLabel As System.Windows.Forms.Label
    Friend WithEvents SelectFamilyButton As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button

#End Region

#Region "Properties"

    Public Property UniqueID() As String
        Get
            Return _UniqueID
        End Get
        Set(ByVal value As String)
            _UniqueID = Value
        End Set
    End Property

    <DefaultValue(CBool(False)), Browsable(True), System.ComponentModel.Description("Shows or Hides the Close Button.")> _
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = Value
        End Set
    End Property

    Public Property FamilyId() As Integer
        Get
            Return CInt(Me.FamilyIDTextBox.Text)
        End Get
        Set(ByVal value As Integer)
            Me.FamilyIDTextBox.Text = CStr(Value)
            _FamilyID = value
        End Set
    End Property

#End Region

#Region "Form Events"

    Private Sub ALERTSOverrideHistory_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            SetSettings()
            If _OpenIndex = 0 Then
                Me.Text = "Manage Alerts "
            Else
                Me.Text = "Manage Alerts - " & _OpenIndex
            End If

            Me.WindowState = FormWindowState.Normal
            Me.FamilyIDTextBox.Focus()
            AddHandler FamilyIDTextBox.KeyPress, New System.Windows.Forms.KeyPressEventHandler(AddressOf HandleKeyPress)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub ALERTSOverrideHistory_FormClosing(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.FormClosing
        Try
            Me.Visible = False
            Me.MdiParent = Nothing
            If _ALERTSOverrideHistoryFrm IsNot Nothing Then _ALERTSOverrideHistoryFrm.Close()
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub SetSettings()
        Me.Top = If(CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(_AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(_AppKey, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)
    End Sub

    Private Sub SaveSettings()

        Dim WindowState As FormWindowState = Me.WindowState
        SaveSetting(_AppKey, Me.Name & "\Settings", "WindowState", CInt(WindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = WindowState

        Me.Opacity = 100

    End Sub

    Private Sub SelectFamilyButton_Click(sender As System.Object, e As System.EventArgs) Handles SelectFamilyButton.Click
        Dim PatientLookUpForm As PatientLookUpForm

        Try
            PatientLookUpForm = New PatientLookUpForm()

            If PatientLookUpForm.ShowDialog(Me) = DialogResult.OK Then
                Me.FamilyIDTextBox.Text = CStr(PatientLookUpForm.FamilyID)
            Else
                Me.FamilyIDTextBox.Text = Nothing
                Me.AlertControl.ClearAll()
            End If

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            Else
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally
            If PatientLookUpForm IsNot Nothing Then PatientLookUpForm.Dispose()
            PatientLookUpForm = Nothing
        End Try
    End Sub

    Private Sub btnshow_Click(sender As System.Object, e As System.EventArgs) Handles btnshow.Click

        Dim DT As DataTable
        Dim DV As DataView

        Try
            _RelationIDs = New ArrayList

            If Me.FamilyIDTextBox.Text.Length > 0 AndAlso IsNumeric(Me.FamilyIDTextBox.Text) Then
                Me.AlertControl.CallingAppID = 1
                Me.AlertControl.Enabled = True

                '' Get the relation id 's from this family

                DT = CMSDALFDBMD.RetrieveRegMasterByFamilyID(CInt(Me.FamilyIDTextBox.Text))

                If DT IsNot Nothing AndAlso (DT.Rows.Count = 0) Then
                    MessageBox.Show("No Family Information.", "Family Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    AlertControl.Enabled = False
                Else
                    DV = New DataView(DT, "", "RELATION_ID", DataViewRowState.OriginalRows)
                    If DV.Count > 0 Then
                        For I As Int16 = 0 To CShort(DV.Count - 1)
                            _RelationIDs.Add(DV(I)("RELATION_ID"))
                        Next
                    End If

                    Me.AlertControl.LoadALERTS(CInt(Me.FamilyIDTextBox.Text), CType(If(RelationIDTextBox.Text.Length = 0, CType(Nothing, Integer?), CType(Me.RelationIDTextBox.Text, Integer?)), Integer?), 1, _RelationIDs)
                End If


            End If
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            Else
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally
        End Try
    End Sub

    Private Sub btnClear_Click(sender As System.Object, e As System.EventArgs) Handles btnClear.Click
        Try
            Me.FamilyIDTextBox.Clear()
            Me.RelationIDTextBox.Clear()

            _RelationIDs = Nothing

            Me.AlertControl.ClearAll()
            Me.FamilyIDTextBox.Focus()
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            Else
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally
        End Try
    End Sub

    Private Sub HandleKeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)

        'ignore if not digit or control key
        If (Not (System.Char.IsDigit(e.KeyChar)) AndAlso Not (System.Char.IsControl(e.KeyChar))) Then
            e.Handled = True
        End If

    End Sub

    Private Sub btnHistory_Click(sender As System.Object, e As System.EventArgs) Handles btnHistory.Click
        Dim HistoryF As AlertHistory

        If Me.FamilyIDTextBox.Text.Length = 0 Then Exit Sub
        Try
            HistoryF = New AlertHistory

            HistoryF.FamilyID = CInt(Me.FamilyIDTextBox.Text)
            HistoryF.RelationID = 0
            HistoryF.ShowDialog(Me)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        Finally
            If HistoryF IsNot Nothing Then HistoryF.Dispose()
            HistoryF = Nothing
        End Try
    End Sub

#End Region

    Private Sub NumericOnlyTextBox_TextChanged(sender As Object, e As EventArgs) Handles FamilyIDTextBox.TextChanged, RelationIDTextBox.TextChanged

        Dim TBox As TextBox = CType(sender, TextBox)
        Dim intCnt As Integer
        Dim strTmp As String

        If IsNumeric(TBox.Text) = False AndAlso Len(TBox.Text) > 0 Then
            strTmp = TBox.Text
            For intCnt = 1 To Len(strTmp)
                If IsNumeric(Mid(strTmp, intCnt, 1)) = False AndAlso Len(strTmp) > 0 Then
                    strTmp = Replace(strTmp, Mid(strTmp, intCnt, 1), "")
                End If
            Next
            TBox.Text = strTmp
        End If

    End Sub
End Class
