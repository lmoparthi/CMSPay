Option Strict On
Option Infer On

Imports System.ComponentModel
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Data.Common
Imports System.Data.DataTableExtensions

Public Class HRAOverrideControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer
    Private _RelationID As Short? = Nothing
    Private _EffectiveDate As Date = UFCWGeneral.NowDate
    Private _EventDate As Date = UFCWGeneral.NowDate
    Private _StartingBalanceOnEffectiveDate As Decimal = 0
    Private _NewBalanceOnEffectiveDate As Decimal = 0
    Private _HRAHistoryDT As DataTable
    Private _HRATransactionsDT As DataTable
    Private _HRAALLEventHistoryDS As DataSet
    Private _HRATriggerEventsDT As DataTable
    Private _MemType As String = ""
    Private _EffectiveDateDTPChecked As Boolean = False

    Friend WithEvents HRASchedulePanel As System.Windows.Forms.Panel
    Friend WithEvents HRAClaimPanel As System.Windows.Forms.Panel
    Friend WithEvents HRARxPanel As System.Windows.Forms.Panel
    Friend WithEvents ClaimIDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents CheckNumberTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents HRATriggerEventsList As System.Windows.Forms.ComboBox
    Friend WithEvents HRAEffectiveDatePanel As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents HRAAdjustmentPanel As System.Windows.Forms.Panel
    Friend WithEvents HRAEventDatePanel As System.Windows.Forms.Panel
    Friend WithEvents AdjustedBalanceTextBox As System.Windows.Forms.TextBox
    Friend WithEvents HRAExpirationDatePanel As System.Windows.Forms.Panel
    Friend WithEvents ExpirationDateDTP As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents EventDateDTP As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents bEffectiveDateDTP As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker

    Private _APPKEY As String = "UFCW\Claims\"

    Public Event BeforeRefresh(ByVal sender As Object, ByRef cancel As Boolean)
    Public Event AfterRefresh(ByVal sender As Object)

    Private _Disposed As Boolean

    ' Protected implementation of Dispose pattern.
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If Not _Disposed Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                'UserControl overrides dispose to clean up the component list.
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If

                If _HRAALLEventHistoryDS IsNot Nothing Then
                    _HRAALLEventHistoryDS.Dispose()
                End If
                If _HRATriggerEventsDT IsNot Nothing Then
                    _HRATriggerEventsDT.Dispose()
                End If

            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            ' TODO: set large fields to null.
            _Disposed = True
        End If

        ' Call the base class implementation.
        MyBase.Dispose(disposing)
    End Sub
#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents HRADataSet As DataSet
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents HRAModificationReasons As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Comments As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.HRAModificationReasons = New System.Windows.Forms.ComboBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.bEffectiveDateDTP = New System.Windows.Forms.DateTimePicker()
        Me.HRAExpirationDatePanel = New System.Windows.Forms.Panel()
        Me.ExpirationDateDTP = New System.Windows.Forms.DateTimePicker()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.HRAAdjustmentPanel = New System.Windows.Forms.Panel()
        Me.AdjustedBalanceTextBox = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.HRAEventDatePanel = New System.Windows.Forms.Panel()
        Me.EventDateDTP = New System.Windows.Forms.DateTimePicker()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.HRAClaimPanel = New System.Windows.Forms.Panel()
        Me.ClaimIDTextBox = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.HRARxPanel = New System.Windows.Forms.Panel()
        Me.CheckNumberTextBox = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.HRAEffectiveDatePanel = New System.Windows.Forms.Panel()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.Label1 = New System.Windows.Forms.Label()
        Me.HRASchedulePanel = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.HRATriggerEventsList = New System.Windows.Forms.ComboBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Comments = New System.Windows.Forms.TextBox()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.HRADataSet = New DataSet()
        Me.GroupBox1.SuspendLayout()
        Me.HRAExpirationDatePanel.SuspendLayout()
        Me.HRAAdjustmentPanel.SuspendLayout()
        Me.HRAEventDatePanel.SuspendLayout()
        Me.HRAClaimPanel.SuspendLayout()
        Me.HRARxPanel.SuspendLayout()
        Me.HRAEffectiveDatePanel.SuspendLayout()
        Me.HRASchedulePanel.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.HRADataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'HRAModificationReasons
        '
        Me.HRAModificationReasons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HRAModificationReasons.DropDownWidth = 600
        Me.HRAModificationReasons.Location = New System.Drawing.Point(104, 19)
        Me.HRAModificationReasons.MaxDropDownItems = 15
        Me.HRAModificationReasons.Name = "HRAModificationReasons"
        Me.HRAModificationReasons.Size = New System.Drawing.Size(259, 21)
        Me.HRAModificationReasons.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.HRAModificationReasons, "A standard description of the HRA transaction type")
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.bEffectiveDateDTP)
        Me.GroupBox1.Controls.Add(Me.HRAExpirationDatePanel)
        Me.GroupBox1.Controls.Add(Me.HRAAdjustmentPanel)
        Me.GroupBox1.Controls.Add(Me.HRAEventDatePanel)
        Me.GroupBox1.Controls.Add(Me.HRAClaimPanel)
        Me.GroupBox1.Controls.Add(Me.HRARxPanel)
        Me.GroupBox1.Controls.Add(Me.HRAEffectiveDatePanel)
        Me.GroupBox1.Controls.Add(Me.HRASchedulePanel)
        Me.GroupBox1.Controls.Add(Me.GroupBox2)
        Me.GroupBox1.Controls.Add(Me.SaveButton)
        Me.GroupBox1.Controls.Add(Me.HRAModificationReasons)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(374, 302)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        '
        'bEffectiveDateDTP
        '
        Me.bEffectiveDateDTP.Checked = False
        Me.bEffectiveDateDTP.CustomFormat = ""
        Me.bEffectiveDateDTP.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.bEffectiveDateDTP.Location = New System.Drawing.Point(10, 276)
        Me.bEffectiveDateDTP.Name = "bEffectiveDateDTP"
        Me.bEffectiveDateDTP.ShowCheckBox = True
        Me.bEffectiveDateDTP.Size = New System.Drawing.Size(104, 20)
        Me.bEffectiveDateDTP.TabIndex = 56
        Me.ToolTip1.SetToolTip(Me.bEffectiveDateDTP, "The date that any positive  balance change will become effective.")
        Me.bEffectiveDateDTP.Value = New Date(2016, 1, 1, 0, 0, 0, 0)
        '
        'HRAExpirationDatePanel
        '
        Me.HRAExpirationDatePanel.Controls.Add(Me.ExpirationDateDTP)
        Me.HRAExpirationDatePanel.Controls.Add(Me.Label7)
        Me.HRAExpirationDatePanel.Location = New System.Drawing.Point(256, 76)
        Me.HRAExpirationDatePanel.Name = "HRAExpirationDatePanel"
        Me.HRAExpirationDatePanel.Size = New System.Drawing.Size(116, 52)
        Me.HRAExpirationDatePanel.TabIndex = 55
        Me.HRAExpirationDatePanel.Visible = False
        '
        'ExpirationDateDTP
        '
        Me.ExpirationDateDTP.Checked = False
        Me.ExpirationDateDTP.Enabled = False
        Me.ExpirationDateDTP.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.ExpirationDateDTP.Location = New System.Drawing.Point(6, 28)
        Me.ExpirationDateDTP.Name = "ExpirationDateDTP"
        Me.ExpirationDateDTP.ShowCheckBox = True
        Me.ExpirationDateDTP.Size = New System.Drawing.Size(104, 20)
        Me.ExpirationDateDTP.TabIndex = 51
        Me.ToolTip1.SetToolTip(Me.ExpirationDateDTP, "The date that any positive balance change will expire. (Note: The display Year of" &
        " 9998 will save as 9999)")
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 7)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(79, 13)
        Me.Label7.TabIndex = 52
        Me.Label7.Text = "Expiration Date"
        '
        'HRAAdjustmentPanel
        '
        Me.HRAAdjustmentPanel.Controls.Add(Me.AdjustedBalanceTextBox)
        Me.HRAAdjustmentPanel.Controls.Add(Me.Label3)
        Me.HRAAdjustmentPanel.Location = New System.Drawing.Point(121, 76)
        Me.HRAAdjustmentPanel.Name = "HRAAdjustmentPanel"
        Me.HRAAdjustmentPanel.Size = New System.Drawing.Size(135, 52)
        Me.HRAAdjustmentPanel.TabIndex = 54
        Me.HRAAdjustmentPanel.Visible = False
        '
        'AdjustedBalanceTextBox
        '
        Me.AdjustedBalanceTextBox.Location = New System.Drawing.Point(16, 29)
        Me.AdjustedBalanceTextBox.Name = "AdjustedBalanceTextBox"
        Me.AdjustedBalanceTextBox.Size = New System.Drawing.Size(100, 20)
        Me.AdjustedBalanceTextBox.TabIndex = 52
        Me.ToolTip1.SetToolTip(Me.AdjustedBalanceTextBox, "Either the new HRA balance or the existing balance can increased or decreased by " &
        "prefixing the adjustment amount with value (+/-) .")
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(6, 1)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(121, 26)
        Me.Label3.TabIndex = 55
        Me.Label3.Text = "New Balance  or  Adjustment (+ \ -) Amt"
        '
        'HRAEventDatePanel
        '
        Me.HRAEventDatePanel.Controls.Add(Me.EventDateDTP)
        Me.HRAEventDatePanel.Controls.Add(Me.Label8)
        Me.HRAEventDatePanel.Location = New System.Drawing.Point(256, 76)
        Me.HRAEventDatePanel.Name = "HRAEventDatePanel"
        Me.HRAEventDatePanel.Size = New System.Drawing.Size(116, 52)
        Me.HRAEventDatePanel.TabIndex = 55
        Me.HRAEventDatePanel.Visible = False
        '
        'EventDateDTP
        '
        Me.EventDateDTP.Checked = False
        Me.EventDateDTP.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.EventDateDTP.Location = New System.Drawing.Point(6, 28)
        Me.EventDateDTP.Name = "EventDateDTP"
        Me.EventDateDTP.ShowCheckBox = True
        Me.EventDateDTP.Size = New System.Drawing.Size(104, 20)
        Me.EventDateDTP.TabIndex = 51
        Me.ToolTip1.SetToolTip(Me.EventDateDTP, "The date that the Event/Activity took place. ")
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(15, 7)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(61, 13)
        Me.Label8.TabIndex = 54
        Me.Label8.Text = "Event Date"
        '
        'HRAClaimPanel
        '
        Me.HRAClaimPanel.Controls.Add(Me.ClaimIDTextBox)
        Me.HRAClaimPanel.Controls.Add(Me.Label5)
        Me.HRAClaimPanel.Location = New System.Drawing.Point(4, 46)
        Me.HRAClaimPanel.Name = "HRAClaimPanel"
        Me.HRAClaimPanel.Size = New System.Drawing.Size(344, 27)
        Me.HRAClaimPanel.TabIndex = 53
        Me.HRAClaimPanel.Visible = False
        '
        'ClaimIDTextBox
        '
        Me.ClaimIDTextBox.Location = New System.Drawing.Point(100, 4)
        Me.ClaimIDTextBox.MaxLength = 9
        Me.ClaimIDTextBox.Name = "ClaimIDTextBox"
        Me.ClaimIDTextBox.Size = New System.Drawing.Size(100, 20)
        Me.ClaimIDTextBox.TabIndex = 54
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(35, 7)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(46, 13)
        Me.Label5.TabIndex = 53
        Me.Label5.Text = "Claim ID"
        '
        'HRARxPanel
        '
        Me.HRARxPanel.Controls.Add(Me.CheckNumberTextBox)
        Me.HRARxPanel.Controls.Add(Me.Label6)
        Me.HRARxPanel.Location = New System.Drawing.Point(20, 46)
        Me.HRARxPanel.Name = "HRARxPanel"
        Me.HRARxPanel.Size = New System.Drawing.Size(344, 27)
        Me.HRARxPanel.TabIndex = 53
        Me.HRARxPanel.Visible = False
        '
        'CheckNumberTextBox
        '
        Me.CheckNumberTextBox.Location = New System.Drawing.Point(85, 3)
        Me.CheckNumberTextBox.MaxLength = 7
        Me.CheckNumberTextBox.Name = "CheckNumberTextBox"
        Me.CheckNumberTextBox.Size = New System.Drawing.Size(100, 20)
        Me.CheckNumberTextBox.TabIndex = 54
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(22, 6)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(45, 13)
        Me.Label6.TabIndex = 53
        Me.Label6.Text = "Check#"
        '
        'HRAEffectiveDatePanel
        '
        Me.HRAEffectiveDatePanel.Controls.Add(Me.DateTimePicker1)
        Me.HRAEffectiveDatePanel.Controls.Add(Me.Label1)
        Me.HRAEffectiveDatePanel.Location = New System.Drawing.Point(4, 76)
        Me.HRAEffectiveDatePanel.Name = "HRAEffectiveDatePanel"
        Me.HRAEffectiveDatePanel.Size = New System.Drawing.Size(116, 52)
        Me.HRAEffectiveDatePanel.TabIndex = 53
        Me.HRAEffectiveDatePanel.Visible = False
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Checked = False
        Me.DateTimePicker1.CustomFormat = ""
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePicker1.Location = New System.Drawing.Point(9, 28)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.ShowCheckBox = True
        Me.DateTimePicker1.Size = New System.Drawing.Size(101, 20)
        Me.DateTimePicker1.TabIndex = 53
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 52
        Me.Label1.Text = "Effective Date"
        '
        'HRASchedulePanel
        '
        Me.HRASchedulePanel.Controls.Add(Me.Label4)
        Me.HRASchedulePanel.Controls.Add(Me.HRATriggerEventsList)
        Me.HRASchedulePanel.Location = New System.Drawing.Point(20, 46)
        Me.HRASchedulePanel.Name = "HRASchedulePanel"
        Me.HRASchedulePanel.Size = New System.Drawing.Size(341, 27)
        Me.HRASchedulePanel.TabIndex = 52
        Me.HRASchedulePanel.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(22, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(35, 13)
        Me.Label4.TabIndex = 53
        Me.Label4.Text = "Event"
        '
        'HRATriggerEventsList
        '
        Me.HRATriggerEventsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.HRATriggerEventsList.DropDownWidth = 600
        Me.HRATriggerEventsList.ItemHeight = 13
        Me.HRATriggerEventsList.Location = New System.Drawing.Point(110, 3)
        Me.HRATriggerEventsList.MaxDropDownItems = 20
        Me.HRATriggerEventsList.Name = "HRATriggerEventsList"
        Me.HRATriggerEventsList.Size = New System.Drawing.Size(230, 21)
        Me.HRATriggerEventsList.TabIndex = 52
        Me.ToolTip1.SetToolTip(Me.HRATriggerEventsList, "A standard description of the HRA Action to be requested")
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Comments)
        Me.GroupBox2.Location = New System.Drawing.Point(4, 130)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(364, 137)
        Me.GroupBox2.TabIndex = 49
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Comment / Remark"
        '
        'Comments
        '
        Me.Comments.Location = New System.Drawing.Point(6, 16)
        Me.Comments.Multiline = True
        Me.Comments.Name = "Comments"
        Me.Comments.Size = New System.Drawing.Size(348, 115)
        Me.Comments.TabIndex = 6
        Me.ToolTip1.SetToolTip(Me.Comments, "A description of why the change is required to modify the HRA balance")
        '
        'SaveButton
        '
        Me.SaveButton.Location = New System.Drawing.Point(288, 273)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(75, 23)
        Me.SaveButton.TabIndex = 4
        Me.SaveButton.Text = "Save"
        Me.ToolTip1.SetToolTip(Me.SaveButton, "Will save HRA Balance change to database.")
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(4, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Change Reason"
        '
        'HRADataSet
        '
        Me.HRADataSet.DataSetName = "HRADataSet"
        Me.HRADataSet.Locale = New System.Globalization.CultureInfo("en-US")
        Me.HRADataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'HRAOverrideControl
        '
        Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
        Me.CausesValidation = False
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "HRAOverrideControl"
        Me.Size = New System.Drawing.Size(381, 305)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.HRAExpirationDatePanel.ResumeLayout(False)
        Me.HRAExpirationDatePanel.PerformLayout()
        Me.HRAAdjustmentPanel.ResumeLayout(False)
        Me.HRAAdjustmentPanel.PerformLayout()
        Me.HRAEventDatePanel.ResumeLayout(False)
        Me.HRAEventDatePanel.PerformLayout()
        Me.HRAClaimPanel.ResumeLayout(False)
        Me.HRAClaimPanel.PerformLayout()
        Me.HRARxPanel.ResumeLayout(False)
        Me.HRARxPanel.PerformLayout()
        Me.HRAEffectiveDatePanel.ResumeLayout(False)
        Me.HRAEffectiveDatePanel.PerformLayout()
        Me.HRASchedulePanel.ResumeLayout(False)
        Me.HRASchedulePanel.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.HRADataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Properties"
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")>
    Public Property HRAHistory() As DataTable
        Get
            Return _HRAHistoryDT
        End Get
        Set(ByVal value As DataTable)
            _HRAHistoryDT = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")>
    Public Property FamilyID() As Integer
        Get
            Return _FamilyID
        End Get
        Set(ByVal value As Integer)
            _FamilyID = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets the Effective Date used in Transaction.")>
    Public ReadOnly Property EffectiveDate() As Date
        Get
            Return _EffectiveDate
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets the Balance after it's been adjusted.")>
    Public ReadOnly Property NewBalanceOnEffectiveDate() As Decimal
        Get
            Return _NewBalanceOnEffectiveDate
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property
#End Region

#Region "Custom Subs\Functions"
    Public Sub Clear()
        Try

            RemoveHandler HRAModificationReasons.SelectedIndexChanged, AddressOf HRAModificationReasons_SelectedIndexChanged

            Me.HRAModificationReasons.SelectedIndex = -1
            Me.HRAModificationReasons.SelectedIndex = -1 'Microsoft known issue workaround KB327244

            Me.HRATriggerEventsList.SelectedIndex = -1

            Me.Comments.Clear()
            Me.AdjustedBalanceTextBox.Clear()
            Me.ClaimIDTextBox.Clear()
            Me.CheckNumberTextBox.Clear()
            Me.DateTimePicker1.Checked = False
            Me.ExpirationDateDTP.Checked = False
            Me.ExpirationDateDTP.Enabled = False

            Me.HRAClaimPanel.Visible = False
            Me.HRASchedulePanel.Visible = False
            Me.HRAEffectiveDatePanel.Visible = False
            Me.HRAAdjustmentPanel.Visible = False
            Me.HRAEventDatePanel.Visible = False
            Me.HRARxPanel.Visible = False
            Me.HRAExpirationDatePanel.Visible = False

            Me.Refresh()

        Catch ex As Exception
            Throw
        Finally
            AddHandler HRAModificationReasons.SelectedIndexChanged, AddressOf HRAModificationReasons_SelectedIndexChanged
        End Try

    End Sub

    Public Sub LoadHRAOverrideControl(ByVal familyID As Integer)
        Try

            LoadHRAOverrideControl(familyID, Nothing, UFCWGeneral.NowDate)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Throw
            End If
        End Try
    End Sub

    Public Sub LoadHRAOverrideControl(ByVal familyID As Integer, ByVal relationID As Short?)
        Try

            LoadHRAOverrideControl(familyID, relationID, UFCWGeneral.NowDate)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Throw
            End If
        End Try
    End Sub

    Public Sub LoadHRAOverrideControl(ByVal familyID As Integer, ByVal hraHistoryDT As DataTable)
        Try

            _HRAHistoryDT = hraHistoryDT
            LoadHRAOverrideControl(familyID, Nothing, UFCWGeneral.NowDate)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Throw
            End If
        End Try
    End Sub

    Public Sub LoadHRAOverrideControl(ByVal familyID As Integer, ByVal relationID As Short?, ByVal hraHistoryDT As DataTable)
        Try

            _HRAHistoryDT = hraHistoryDT
            LoadHRAOverrideControl(familyID, relationID, UFCWGeneral.NowDate)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Throw
            End If
        End Try
    End Sub

    Public Sub LoadHRAOverrideControl(ByVal familyID As Integer, ByVal relationID As Short?, ByVal effectiveDate As Date)

        Try
            'Me.EffectiveDateDTP.Value = New Date(effectiveDate.Ticks)
            Me.bEffectiveDateDTP.Checked = False

            _EffectiveDate = effectiveDate
            _FamilyID = familyID
            _RelationID = relationID

            RemoveHandler HRAModificationReasons.SelectedIndexChanged, AddressOf HRAModificationReasons_SelectedIndexChanged
            HRAModificationReasons.DataSource = HRADAL.RetrieveHRATransactionTypes()
            HRAModificationReasons.DisplayMember = "TRANSACTION_DESCRIPTION"
            HRAModificationReasons.ValueMember = "TRANSACTION_TYPE"
            AddHandler HRAModificationReasons.SelectedIndexChanged, AddressOf HRAModificationReasons_SelectedIndexChanged

            _HRATriggerEventsDT = HRADAL.RetrieveHRAScheduleTypes

            Me.Clear()

            AddHandler AdjustedBalanceTextBox.KeyPress, New System.Windows.Forms.KeyPressEventHandler(AddressOf HandleKeyPress)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Throw
            End If
        End Try
    End Sub

    Private Sub SaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveButton.Click


        Dim HRAClaimHistoryDS As DataSet
        Dim HRADV As DataView
        Dim HRACheckHistoryDS As DataSet
        Dim HRAEventHistoryDS As DataSet
        Dim HRA31DV As DataView
        Dim HRA32DV As DataView
        Dim DTP As DateTime = New DateTime(Year(UFCWGeneral.NowDate), 1, 1, 0, 0, 0)
        Try
            SaveButton.Enabled = False

            If Me.DateTimePicker1.Checked AndAlso Me.ExpirationDateDTP.Checked AndAlso DateDiff(DateInterval.Day, Me.ExpirationDateDTP.Value, Me.DateTimePicker1.Value) > 0 Then
                MsgBox("Expiration Date must be either equal or greater than Effective Date.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid dates")
                Return
            End If

            Select Case CDec(CType(HRAModificationReasons.SelectedItem, DataRowView)("TRANSACTION_TYPE_USEDBY"))
                Case 1, 3 'HRA Adjustment F/T/A/C

                    If Me.AdjustedBalanceTextBox.Text.Length > 0 AndAlso Me.Comments.Text.Trim.Length > 0 AndAlso Me.DateTimePicker1.Checked Then
                        SaveManual()
                    Else
                        MsgBox("Please enter Amount, Effective Date(s) and Comments to continue.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid dates")
                    End If

                Case 2 'Reverse claim CL/T/C
                    If Me.Comments.Text.Trim.Length > 0 AndAlso Me.ClaimIDTextBox.Text.Length > 0 AndAlso Not Me.bEffectiveDateDTP.Checked Then

                        HRAClaimHistoryDS = HRADAL.RetrieveHRAClaimHistory(CInt(_FamilyID), CInt(Me.ClaimIDTextBox.Text))
                        HRADV = New DataView(HRAClaimHistoryDS.Tables(0), "TRANSACTION_TYPE = 30", "", DataViewRowState.CurrentRows)

                        If HRAClaimHistoryDS.Tables(0).Rows.Count > 0 AndAlso HRADV.Count < 1 Then

                            ReverseClaim(HRAClaimHistoryDS, Me.ExpirationDateDTP.Value)

                        ElseIf HRAClaimHistoryDS.Tables(0).Rows.Count > 0 Then
                            MsgBox("The Claim specified has already been reversed, 'Claim Reversal' is no longer available.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid request")
                        Else
                            MsgBox("The Claim specified does either not exist or is not associated with the FamilyID.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid request")
                        End If
                    Else
                        MsgBox("Please enter Claim# & Comments to continue.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Incomplete request")
                    End If

                Case 4 'Rx CH

                    If Me.Comments.Text.Trim.Length > 0 AndAlso Me.CheckNumberTextBox.Text.Length > 0 AndAlso Me.bEffectiveDateDTP.Checked = False Then

                        HRACheckHistoryDS = HRADAL.RetrieveHRACheckHistory(CInt(_FamilyID), CInt(Me.CheckNumberTextBox.Text))
                        HRA31DV = New DataView(HRACheckHistoryDS.Tables(0), "TRANSACTION_TYPE = 31 ", "", DataViewRowState.CurrentRows)
                        HRA32DV = New DataView(HRACheckHistoryDS.Tables(0), "TRANSACTION_TYPE = 32", "", DataViewRowState.CurrentRows)

                        Select Case HRAModificationReasons.Text.Contains("Reversal")
                            Case True
                                If HRACheckHistoryDS.Tables(0).Rows.Count > 0 AndAlso HRA31DV.Count < 1 Then
                                    ReverseCheck(HRACheckHistoryDS)
                                ElseIf HRACheckHistoryDS.Tables(0).Rows.Count > 0 Then
                                    MsgBox("The Check specified has already been reversed, 'Check Reversal' is no longer available.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid request")
                                Else
                                    MsgBox("The Check specified does either not exist, is not a Rx check or is not associated with the FamilyID.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid request")
                                End If
                            Case False

                                If HRACheckHistoryDS.Tables(0).Rows.Count > 0 AndAlso HRA32DV.Count > 0 Then
                                    MsgBox("A request to Re-issue this check has already been made, 'Check Re-issue' is not available.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid request")
                                ElseIf HRACheckHistoryDS.Tables(0).Rows.Count > 0 AndAlso HRA31DV.Count > 0 Then
                                    ReIssueCheck(HRACheckHistoryDS)
                                ElseIf HRACheckHistoryDS.Tables(0).Rows.Count > 0 Then
                                    MsgBox("The Check specified has not been reversed, 'Check Re-issue' is not available.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid request")
                                Else
                                    MsgBox("The Check specified does either not exist, is not a Rx check or is not associated with the FamilyID.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid request")
                                End If
                        End Select
                    Else
                        MsgBox("Please enter Check# & Comments to continue.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Incomplete request")
                    End If

                Case 5 'Schedule Event F 

                    If _RelationID IsNot Nothing AndAlso _RelationID > 0 Then
                        Dim DR As DataRow = CMSDALFDBMD.RetrievePatientInfo(CInt(_FamilyID), CShort(_RelationID))
                        If DR IsNot Nothing Then
                            Select Case DR("RELATION").ToString
                                Case "W", "P", "H"
                                Case Else
                                    MsgBox("Events are only valid for Participant or Spouse.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid Family Member")
                                    Return
                            End Select
                        End If
                    End If

                    If DateDiff(DateInterval.Year, Me.DateTimePicker1.Value, DTP) > 0 Then
                        MsgBox("Events are only valid for the current fund year.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid Effective Date")
                        Return
                    End If

                    If Me.Comments.Text.Trim.Length > 0 AndAlso Me.HRATriggerEventsList.Text.Length > 0 AndAlso Me.DateTimePicker1.Checked AndAlso Me.EventDateDTP.Checked Then

                        HRAEventHistoryDS = CMSDALFDBHRA.RetrieveHRAAllEventHistory(CInt(_FamilyID), _RelationID)
                        HRADV = New DataView(HRAEventHistoryDS.Tables(0), " RELATION_ID = " & If(_RelationID Is Nothing, 0S, CShort(_RelationID)) & " AND ARCHIVE_SW = 0 AND TRIGGER_ID = '" & CStr(HRATriggerEventsList.SelectedValue).ToString & "' AND EFFECTIVE_YEAR = '" & Format(DateTimePicker1.Value, "yyyy") & "'", "", DataViewRowState.CurrentRows)

                        If HRADV.Count < 1 Then

                            ScheduleManualEvent()

                        ElseIf HRADV.Count > 0 Then
                            MsgBox("The Event requested for effective year " & Format(Me.DateTimePicker1.Value, "yyyy") & If(HRADV(0)("PROCESS_STATUS").ToString = "OPEN", " has already been scheduled and will be processed shortly.", " was already processed. "), CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid request")
                        End If

                    Else
                        MsgBox("Please enter Scheduled Action, Effective Date, Event Date and Comments to continue.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Incomplete request")
                    End If

                Case 6 'Cancel Event F/A - New to this release of code. Will require possible reversal of Max transactions

                    If _RelationID IsNot Nothing AndAlso _RelationID > 0 Then
                        Dim DR As DataRow = CMSDALFDBMD.RetrievePatientInfo(CInt(_FamilyID), CShort(_RelationID))
                        If DR IsNot Nothing Then
                            Select Case DR("RELATION").ToString
                                Case "W", "P", "H"
                                Case Else
                                    MsgBox("Events are only valid for Participant or Spouse.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid Family Member")
                                    Return
                            End Select
                        End If
                    End If

                    If Me.Comments.Text.Trim.Length > 0 AndAlso Me.HRATriggerEventsList.Text.Length > 0 AndAlso Me.DateTimePicker1.Checked Then

                        HRAEventHistoryDS = CMSDALFDBHRA.RetrieveHRAAllEventHistory(CInt(_FamilyID), _RelationID)
                        Dim MatchingQuery = (
                                                From HRATransactions In HRAEventHistoryDS.Tables(0).AsEnumerable()
                                                Where HRATransactions.Field(Of String)("TRANS_CODE") IsNot Nothing AndAlso HRATransactions.Field(Of String)("TRANS_CODE") = HRATriggerEventsList.SelectedValue.ToString _
                                                AndAlso HRATransactions.Field(Of Decimal?)("ARCHIVE_SW") IsNot Nothing AndAlso HRATransactions.Field(Of Decimal?)("ARCHIVE_SW") = 0 _
                                                AndAlso HRATransactions.Field(Of DateTime)("EFFECTIVE_DATE").ToShortDateString = Me.DateTimePicker1.Value.ToShortDateString
                                                Select HRATransactions
                                            )

                        If MatchingQuery.Count = 1 Then

                            CancelManualEvent(MatchingQuery(0))

                        ElseIf Not MatchingQuery.Any() Then
                            MsgBox("The Effective date was not found for the specified Event", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid request")
                        ElseIf MatchingQuery.Count > 1 Then
                            MsgBox("Multiple matches found using The specified Effective date has multiple matches", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid request")
                        End If

                    Else
                        MsgBox("Please enter Effective Date, Event (Check Effective Date box to enable) and Comments to continue.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Incomplete request")
                    End If

                Case 7 'Fix Funding E - New to this release of code. Will require $ Amt, and possible reversal of Max transactions

                    If DateDiff(DateInterval.Year, Me.DateTimePicker1.Value, DTP) > 0 Then
                        MsgBox("Events are only valid for the current fund year.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid Effective Date")
                        Return
                    End If
                    If Me.Comments.Text.Trim.Length > 0 AndAlso Me.HRATriggerEventsList.Text.Length > 0 AndAlso Me.bEffectiveDateDTP.Checked Then

                        HRAEventHistoryDS = CMSDALFDBHRA.RetrieveHRAAllEventHistory(CInt(_FamilyID), _RelationID)
                        HRADV = New DataView(HRAEventHistoryDS.Tables(0), " RELATION_ID = " & If(_RelationID Is Nothing, 0S, CShort(_RelationID)) & " AND ARCHIVE_SW = 0 AND TRIGGER_ID = '" & CStr(Me.HRATriggerEventsList.SelectedValue).ToString & "' AND EFFECTIVE_YEAR = '" & Format(Me.DateTimePicker1.Value, "yyyy") & "'", "", DataViewRowState.CurrentRows)

                        If HRADV.Count < 1 Then

                            ScheduleManualFunding()

                        ElseIf HRADV.Count > 0 Then
                            MsgBox("The Funding requested for effective year " & Format(Me.DateTimePicker1.Value, "yyyy") & If(HRADV(0)("PROCESS_STATUS").ToString = "OPEN", " has already been scheduled and will be processed shortly.", " was already processed. "), CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid request")
                        End If

                    Else
                        MsgBox("Please enter Scheduled Action, Effective Date and Comments to continue.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Incomplete request")
                    End If

                Case 8 'Cancel Funding F/A - New to this release of code. Will require $ Amt, and possible reversal of Max transactions

                Case 9, 10 'Opt In/Out

                    If _RelationID IsNot Nothing AndAlso _RelationID > 0 Then
                        Dim DR As DataRow = CMSDALFDBMD.RetrievePatientInfo(CInt(_FamilyID), CShort(_RelationID))
                        If DR IsNot Nothing Then
                            Select Case DR("RELATION").ToString
                                Case "W", "P", "H"
                                Case Else
                                    MsgBox("Opt In only valid for Participant or Spouse.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid Family Member")
                                    Return
                            End Select
                        End If
                    End If

                    If Comments.Text.Trim.Length > 0 AndAlso Me.DateTimePicker1.Checked Then

                        'Cannot post a OptIn if a prior Optin for the year exists and is still active
                        HRAEventHistoryDS = CMSDALFDBHRA.RetrieveHRAAllEventHistory(CInt(_FamilyID), _RelationID)

                        Dim LastRxQuery = (
                                                From HRATransactions In HRAEventHistoryDS.Tables(0).AsEnumerable()
                                                Where HRATransactions.Field(Of Integer?)("TRANSACTION_TYPE") IsNot Nothing AndAlso New Integer() {98, 99}.Contains(HRATransactions.Field(Of Integer)("TRANSACTION_TYPE")) _
                                                AndAlso HRATransactions.Field(Of Decimal?)("ARCHIVE_SW") IsNot Nothing AndAlso HRATransactions.Field(Of Decimal?)("ARCHIVE_SW") = 0 _
                                                AndAlso HRATransactions.Field(Of DateTime)("EFFECTIVE_DATE").ToShortDateString = Me.DateTimePicker1.Value.ToShortDateString _
                                                AndAlso HRATransactions.Field(Of DateTime?)("HT_CREATE_DATE") IsNot Nothing
                                                Order By HRATransactions.Field(Of DateTime)("HT_CREATE_DATE") Descending
                                                Select HRATransactions.Field(Of Integer?)("TRANSACTION_TYPE")
                                            )

                        If LastRxQuery IsNot Nothing AndAlso LastRxQuery(0) IsNot Nothing AndAlso CInt(LastRxQuery(0)) = CInt(HRAModificationReasons.SelectedValue) Then
                            MsgBox("The " & If(CDec(CType(HRAModificationReasons.SelectedItem, DataRowView)("TRANSACTION_TYPE")) = 99, "Opt In", "Opt Out") & " requested for effective year " & Format(Me.DateTimePicker1.Value, "yyyy") & " already exists. ", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid request")
                        Else

                            ScheduleManualAction()

                        End If

                    Else
                        MsgBox("Please enter Scheduled Action, Effective Date, Event Date and Comments to continue.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Incomplete request")
                    End If

            End Select

        Catch ex As Exception

            Throw

        Finally

            SaveButton.Enabled = True

        End Try

    End Sub
    Private Sub ReverseClaim(ByVal hraClaimHistory As DataSet, ByVal expirationDate As Date)

        Dim DBTransaction As DbTransaction
        Dim HRADV As DataView
        Dim HRAForfeitHistoryDV As DataView

        Try

            DBTransaction = CMSDALCommon.BeginTransaction()

            HRADV = New DataView(hraClaimHistory.Tables(0), "TRANSACTION_TYPE = 7", "", DataViewRowState.CurrentRows)

            For Each DRV As DataRowView In HRADV
                'reverse claim amt but insert with new expiration date
                HRADAL.InsertHRATransaction(CInt(HRAModificationReasons.SelectedValue), CInt(_FamilyID), UFCWGeneral.IsNullShortHandler(DRV("RELATION_ID")), UFCWGeneral.IsNullIntegerHandler(DRV("CLAIM_ID")), UFCWGeneral.IsNullShortHandler(DRV("LINE_NBR")), Nothing, CDec(DRV("TRANSACTION_AMT")) * -1D, UFCWGeneral.IsNullDateHandler(DRV("EFFECTIVE_DATE")), expirationDate, Comments.Text.ToString, SystemInformation.UserName.ToUpper.ToString, Nothing, DBTransaction)

                HRAForfeitHistoryDV = New DataView(_HRAHistoryDT, "TRANSACTION_TYPE = 17 AND TRANSACTION_DATE >= '" & CDate(DRV("TRANSACTION_DATE")).ToShortDateString & "' AND EFFECTIVE_DATE = '" & CDate(DRV("EFFECTIVE_DATE")).ToShortDateString & "'", "", DataViewRowState.CurrentRows)

                If HRAForfeitHistoryDV.Count > 0 Then
                    'If reversed claim occurred after forfeiture date, then create transaction to consume funds as forfeited.
                    If CDate(DRV("TRANSACTION_DATE")) > expirationDate Then
                        HRADAL.InsertHRATransaction(CInt(HRAModificationReasons.SelectedValue), CInt(_FamilyID), UFCWGeneral.IsNullShortHandler(DRV("RELATION_ID")), UFCWGeneral.IsNullIntegerHandler(DRV("CLAIM_ID")), UFCWGeneral.IsNullShortHandler(DRV("LINE_NBR")), Nothing, UFCWGeneral.IsNullDecimalHandler(DRV("TRANSACTION_AMT")), UFCWGeneral.IsNullDateHandler(DRV("EFFECTIVE_DATE")), UFCWGeneral.IsNullDateHandler(HRAForfeitHistoryDV(0)("EXPIRATION_DATE")), Comments.Text, SystemInformation.UserName.ToUpper.ToString, Nothing, DBTransaction)
                    End If
                End If

            Next

            CMSDALCommon.CommitTransaction(DBTransaction)

            RaiseEvent AfterRefresh(Me)

        Catch ex As Exception

            Try

             If DBTransaction IsNot Nothing Then
                CMSDALCommon.RollbackTransaction(DBTransaction)
            End If
            Finally
            End Try


	Throw

        Finally
            If DBTransaction IsNot Nothing Then DBTransaction.Dispose()
            DBTransaction = Nothing
        End Try

    End Sub
    Private Sub ReverseCheck(ByVal hraCheckHistory As DataSet)

        Dim DBTransaction As DbTransaction
        Dim HRADV As DataView

        Try

            DBTransaction = CMSDALCommon.BeginTransaction()

            HRADV = New DataView(hraCheckHistory.Tables(0), "TRANSACTION_TYPE = 9", "", DataViewRowState.CurrentRows)
            For Each DRV As DataRowView In HRADV
                Call HRADAL.InsertHRATransaction(CInt(HRAModificationReasons.SelectedValue), CInt(_FamilyID), UFCWGeneral.IsNullShortHandler(DRV("RELATION_ID")), Nothing, UFCWGeneral.IsNullShortHandler(DRV("LINE_NBR")), UFCWGeneral.IsNullIntegerHandler(DRV("CHECK_NBR")), CDec(DRV("TRANSACTION_AMT")) * -1D, UFCWGeneral.IsNullDateHandler(DRV("EFFECTIVE_DATE")), UFCWGeneral.IsNullDateHandler(DRV("EXPIRATION_DATE")), Comments.Text, SystemInformation.UserName.ToUpper.ToString, Nothing, DBTransaction)
            Next

            CMSDALCommon.CommitTransaction(DBTransaction)

            RaiseEvent AfterRefresh(Me)

        Catch ex As Exception

            Try

                If DBTransaction IsNot Nothing AndAlso DBTransaction.Connection IsNot Nothing AndAlso DBTransaction.Connection.State <> ConnectionState.Closed Then
                    CMSDALCommon.RollbackTransaction(DBTransaction)
                End If

            Finally
            End Try


	Throw
        Finally
            If DBTransaction IsNot Nothing Then DBTransaction.Dispose()
            DBTransaction = Nothing

        End Try

    End Sub
    Private Sub ReIssueCheck(ByVal hraCheckHistory As DataSet)

        Dim HRADV As DataView

        Try

            HRADV = New DataView(hraCheckHistory.Tables(0), "TRANSACTION_TYPE = 31 OR TRANSACTION_TYPE = 10", "", DataViewRowState.CurrentRows)
            For Each DRV As DataRowView In HRADV
                HRADAL.InsertHRARxReIssueCheck(CInt(_FamilyID), UFCWGeneral.IsNullShortHandler(DRV("RELATION_ID")), UFCWGeneral.IsNullIntegerHandler(DRV("CLAIM_ID")), UFCWGeneral.IsNullIntegerHandler(DRV("CHECK_NBR")), UFCWGeneral.IsNullDateHandler(DRV("EFFECTIVE_DATE")), CInt(HRAModificationReasons.SelectedValue), Comments.Text, SystemInformation.UserName.ToUpper)
                Exit For 'only execute once
            Next
            RaiseEvent AfterRefresh(Me)

        Catch ex As Exception

            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Throw
            End If
        Finally

        End Try

    End Sub
    Private Sub ScheduleManualAction()

        Dim DT As DataTable

        Try
            _EffectiveDate = Me.DateTimePicker1.Value
            _EventDate = Me.EventDateDTP.Value

            DT = HRADAL.RetrieveMemType(CInt(_FamilyID), Me.DateTimePicker1.Value)
            If DT.Rows.Count > 0 Then
                _MemType = CStr(DT.Rows(0)("MEMTYPE"))
            End If

            'TransientID = HRADAL.InsertHRATransientTransaction(CStr(HRATriggerEventsList.SelectedValue), _FamilyID, If(_RelationID Is Nothing, 0S, CShort(_RelationID)), _MemType, EffectiveDateDTP.Value, EventDateDTP.Value, Comments.Text, SystemInformation.UserName.ToUpper)
            HRADAL.InsertHRATransaction(CInt(Me.HRAModificationReasons.SelectedValue), CInt(_FamilyID), 0S, Nothing, Me.DateTimePicker1.Value, CDate("9999-12-31"), CStr(Me.HRAModificationReasons.Text) & " - " & Me.Comments.Text, SystemInformation.UserName.ToUpper)

            RaiseEvent AfterRefresh(Me)

        Catch ex As Exception

            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Throw
            End If
        Finally

        End Try

    End Sub
    Private Sub ScheduleManualEvent()

        Dim DT As DataTable
        Dim TransientID As Integer

        Try
            _EffectiveDate = Me.DateTimePicker1.Value
            _EventDate = Me.EventDateDTP.Value

            DT = HRADAL.RetrieveMemType(CInt(_FamilyID), Me.DateTimePicker1.Value)
            If DT.Rows.Count > 0 Then
                _MemType = CStr(DT.Rows(0)("MEMTYPE"))
            End If

            TransientID = HRADAL.InsertHRATransientTransaction(CStr(Me.HRATriggerEventsList.SelectedValue), _FamilyID, If(_RelationID Is Nothing, 0S, CShort(_RelationID)), _MemType, Me.DateTimePicker1.Value, Me.EventDateDTP.Value, Me.Comments.Text, SystemInformation.UserName.ToUpper)
            'HRADAL.InsertHRATransaction(CInt(HRAModificationReasons.SelectedValue), CInt(_FamilyID), 0S, 0D, EffectiveDateDTP.Value, HRAScheduleReasons.Text & " - " & Comments.Text, SystemInformation.UserName.ToUpper, TransientID, DBTransaction)

            RaiseEvent AfterRefresh(Me)

        Catch ex As Exception

            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Throw
            End If
        Finally

        End Try

    End Sub
    Private Sub ScheduleManualFunding()

        Dim DT As DataTable
        Dim TransientID As Integer

        Try
            _EffectiveDate = Me.bEffectiveDateDTP.Value

            DT = HRADAL.RetrieveMemType(CInt(_FamilyID), Me.bEffectiveDateDTP.Value)
            If DT.Rows.Count > 0 Then
                _MemType = CStr(DT.Rows(0)("MEMTYPE"))
            End If

            TransientID = HRADAL.InsertHRATransientTransaction(CStr(Me.HRATriggerEventsList.SelectedValue), _FamilyID, If(_RelationID Is Nothing, 0S, CShort(_RelationID)), _MemType, Me.bEffectiveDateDTP.Value, Me.bEffectiveDateDTP.Value, Me.Comments.Text, SystemInformation.UserName.ToUpper)
            'HRADAL.InsertHRATransaction(CInt(HRAModificationReasons.SelectedValue), CInt(_FamilyID), 0S, 0D, EffectiveDateDTP.Value, HRAScheduleReasons.Text & " - " & Comments.Text, SystemInformation.UserName.ToUpper, TransientID, DBTransaction)

            RaiseEvent AfterRefresh(Me)

        Catch ex As Exception

            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Throw
            End If
        Finally

        End Try

    End Sub
    Private Function CancelManualEvent(transactionDR As DataRow) As Boolean
        Dim RowsUpdated As Decimal?

        Try
            _EffectiveDate = Me.DateTimePicker1.Value

            If IsDBNull(transactionDR("TRANSACTION_TYPE")) AndAlso IsDBNull(transactionDR("HT_TRANS_ID")) Then
                RowsUpdated = HRADAL.CancelHRATransient(transactionDR("TRANS_CODE").ToString, CInt(transactionDR("FAMILY_ID")), CType(transactionDR("RELATION_ID"), Short?), CDate(transactionDR("EFFECTIVE_DATE")), Me.Comments.Text.Trim, SystemInformation.UserName.ToUpper)
            Else
                RowsUpdated = HRADAL.CancelHRATransaction(CInt(transactionDR("TRANSACTION_TYPE")), CInt(transactionDR("FAMILY_ID")), CType(transactionDR("RELATION_ID"), Short?), CDate(transactionDR("EFFECTIVE_DATE")), Me.Comments.Text.Trim, SystemInformation.UserName.ToUpper)
            End If

            If RowsUpdated Is Nothing Then
                Return False
            End If

            RaiseEvent AfterRefresh(Me)

            Return True

        Catch ex As Exception

            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Throw
            End If
        Finally

        End Try

    End Function
    Private Sub SaveManual()

        Dim DR As DataRow
        Dim AvailableFundsDT As DataTable
        Dim Transaction As DbTransaction
        Dim BalanceDifference As Decimal = 0
        Dim TransactionAmount As Decimal = 0
        Dim ExpirationDate As Date

        _NewBalanceOnEffectiveDate = CDec(Me.AdjustedBalanceTextBox.Text)
        _StartingBalanceOnEffectiveDate = 0

        _EffectiveDate = Me.bEffectiveDateDTP.Value

        If ExpirationDateDTP.Checked Then
            ExpirationDate = Me.ExpirationDateDTP.Value
        Else
            ExpirationDate = CDate("12/31/9999")
        End If

        Try

            AvailableFundsDT = HRADAL.GetHRAAvailableFundsByDateRange(CInt(_FamilyID), 0S, Me.bEffectiveDateDTP.Value, If(ExpirationDate = CDate("12/31/9998"), CDate("12/31/9999"), ExpirationDate))

            For Each DR In AvailableFundsDT.Rows
                _StartingBalanceOnEffectiveDate += CDec(DR("REMAINING_AMT"))
            Next

            If Me.AdjustedBalanceTextBox.Text.Contains("+") OrElse Me.AdjustedBalanceTextBox.Text.Contains("-") Then
                BalanceDifference = CDec(Me.AdjustedBalanceTextBox.Text)
            Else
                BalanceDifference = (CDec(Me.AdjustedBalanceTextBox.Text) - _StartingBalanceOnEffectiveDate)
            End If

            Dim AdjustmentOK As MsgBoxResult = MsgBoxResult.Yes

            If _StartingBalanceOnEffectiveDate <= 0 AndAlso BalanceDifference < 0 Then
                AdjustmentOK = MsgBox("The effective date specified has no available funds.", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "Request Denied")
            ElseIf BalanceDifference < 0 AndAlso Me.ExpirationDateDTP.Checked Then
                AdjustmentOK = MsgBox("Expiration Date is only valid when adding funds.", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, MsgBoxStyle), "Request Denied")
            ElseIf _StartingBalanceOnEffectiveDate < Math.Abs(BalanceDifference) AndAlso Me.AdjustedBalanceTextBox.Text.Contains("-") Then
                AdjustmentOK = MsgBox("The adjustment specified exceeds the available funds for the effective date specified." + Environment.NewLine + "The available funds will be reduced to zero (0).", CType(MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, MsgBoxStyle), "Confirm Adjustment")
            End If

            If AdjustmentOK = MsgBoxResult.Yes AndAlso Me.AdjustedBalanceTextBox.Text.Length > 0 AndAlso CInt(Me.AdjustedBalanceTextBox.Text) < 0 AndAlso Me.Comments.Text.Trim.Length > 0 AndAlso Me.HRAModificationReasons.Text.Length > 0 AndAlso Me.bEffectiveDateDTP.Checked AndAlso Me.ExpirationDateDTP.Checked Then
                AdjustmentOK = MsgBox("Expiration Date is only valid when adding funds.", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, MsgBoxStyle), "Invalid dates")
            ElseIf Me.AdjustedBalanceTextBox.Text.Length > 0 AndAlso Me.Comments.Text.Trim.Length > 0 AndAlso Me.HRAModificationReasons.Text.Length > 0 AndAlso Me.bEffectiveDateDTP.Checked AndAlso ((Me.ExpirationDateDTP.Checked AndAlso DateDiff(DateInterval.Day, Me.ExpirationDateDTP.Value, Me.bEffectiveDateDTP.Value) > 0) OrElse Not Me.ExpirationDateDTP.Checked) Then
            End If

            If AdjustmentOK <> MsgBoxResult.Yes Then
                Exit Sub
            End If

            Transaction = CMSDALCommon.BeginTransaction()

            If BalanceDifference > 0 Then
                'add a single positive transaction
                If Me.ExpirationDateDTP.Checked AndAlso Me.ExpirationDateDTP.Text IsNot Nothing Then
                    HRADAL.InsertHRATransaction(CInt(Me.HRAModificationReasons.SelectedValue), CInt(_FamilyID), 0, BalanceDifference, Me.bEffectiveDateDTP.Value, Me.ExpirationDateDTP.Value, Me.Comments.Text, SystemInformation.UserName.ToUpper, Nothing, Transaction)
                Else
                    HRADAL.InsertHRATransaction(CInt(Me.HRAModificationReasons.SelectedValue), CInt(_FamilyID), 0, BalanceDifference, Me.bEffectiveDateDTP.Value, Me.Comments.Text, SystemInformation.UserName.ToUpper, Nothing, Transaction)
                End If

            ElseIf AdjustmentOK = MsgBoxResult.Yes Then
                'consume prior funding events
                BalanceDifference = Math.Abs(BalanceDifference) 'turn negatives positive for math purposes

                For Each DR In AvailableFundsDT.Rows

                    TransactionAmount = Math.Min(CDec(DR("REMAINING_AMT")), BalanceDifference)

                    HRADAL.InsertHRATransaction(CInt(HRAModificationReasons.SelectedValue), CInt(_FamilyID), 0S, TransactionAmount * -1D, UFCWGeneral.IsNullDateHandler(DR("EFFECTIVE_DATE")), UFCWGeneral.IsNullDateHandler(DR("EXPIRATION_DATE")), Me.Comments.Text, SystemInformation.UserName.ToUpper, Nothing, Transaction)

                    If CDec(DR("REMAINING_AMT")) >= BalanceDifference Then
                        Exit For
                    Else
                        BalanceDifference -= CDec(DR("REMAINING_AMT"))
                    End If
                Next
            End If

            CMSDALCommon.CommitTransaction(Transaction)

            RaiseEvent AfterRefresh(Me)

        Catch ex As Exception

            Try

                If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing AndAlso Transaction.Connection.State <> ConnectionState.Closed Then
                    CMSDALCommon.RollbackTransaction(Transaction)
                End If

            Finally
            End Try

            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Throw
            End If

        Finally
            If Transaction IsNot Nothing Then Transaction.Dispose()
            Transaction = Nothing

            DR = Nothing
            AvailableFundsDT = Nothing

        End Try

    End Sub
    Private Sub HandleKeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)

        'ignore if not digit or control key
        If Not System.Char.IsDigit(e.KeyChar) AndAlso
            Not System.Char.IsControl(e.KeyChar) AndAlso Not e.KeyChar = "." AndAlso
            Not e.KeyChar = "+" AndAlso Not e.KeyChar = "-" Then
            e.Handled = True
        End If

        If (e.KeyChar = "+" OrElse e.KeyChar = "-") AndAlso CType(sender, TextBox).Text.Trim.Length > 0 Then
            e.Handled = True 'Can only be the first character

        ElseIf e.KeyChar = "." Then
            If CharCount(CType(sender, TextBox).Text, ".") > 0 Then
                e.Handled = True
            End If
        ElseIf e.KeyChar = "+" OrElse e.KeyChar = "-" Then
            If CharCount(CType(sender, TextBox).Text, "+") > 0 OrElse CharCount(CType(sender, TextBox).Text, "-") > 0 Then
                e.Handled = True
            End If
        End If

    End Sub

    Private Sub HRAModificationReasons_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles HRAModificationReasons.SelectedIndexChanged

        Dim HRATriggerEventsDV As DataView
        Dim DTP As DateTime

        Try

            RemoveHandler HRAModificationReasons.SelectedIndexChanged, AddressOf HRAModificationReasons_SelectedIndexChanged
            RemoveHandler DateTimePicker1.ValueChanged, AddressOf DateTimePicker1_ValueChanged

            Me.Comments.Clear()
            Me.AdjustedBalanceTextBox.Clear()
            Me.ClaimIDTextBox.Clear()
            Me.CheckNumberTextBox.Clear()

            If Me.HRAModificationReasons.SelectedIndex <> -1 Then

                HRATriggerEventsDV = New DataView(_HRATriggerEventsDT) With {
                    .Sort = "TRIGGER_DESC"
                }

                If _RelationID IsNot Nothing AndAlso _RelationID > 0 Then
                    HRATriggerEventsDV.RowFilter = "PARTICIPANT_ONLY_SW = 0"
                End If

                Me.HRATriggerEventsList.Enabled = False

                Me.HRASchedulePanel.Visible = False
                Me.HRAEffectiveDatePanel.Visible = False
                Me.HRAClaimPanel.Visible = False
                Me.HRAAdjustmentPanel.Visible = False
                Me.HRAEventDatePanel.Visible = False
                Me.HRARxPanel.Visible = False
                Me.HRAExpirationDatePanel.Visible = False

                DTP = New DateTime(Year(UFCWGeneral.NowDate), 1, 1, 0, 0, 0)

                Select Case CDec(CType(Me.HRAModificationReasons.SelectedItem, DataRowView)("TRANSACTION_TYPE_USEDBY"))
                    Case 1, 3 'adjustment 

                        Me.HRAEffectiveDatePanel.Visible = True
                        Me.HRAExpirationDatePanel.Visible = True

                        Me.DateTimePicker1.Value = DTP
                        Me.ExpirationDateDTP.Value = New DateTime(9998, 12, 31, 0, 0, 0)

                        Me.ExpirationDateDTP.Enabled = True
                        Me.ExpirationDateDTP.Checked = False

                        Me.HRAAdjustmentPanel.Visible = True

                    Case 2 'claim

                        Me.HRAClaimPanel.Visible = True

                    Case 4 'Rx

                        Me.HRARxPanel.Visible = True

                    Case 5, 6 'event

                        Me.HRATriggerEventsList.DataSource = Nothing

                        _HRAALLEventHistoryDS = CMSDALFDBHRA.RetrieveHRAAllEventHistory(_FamilyID, _RelationID)

                        Me.HRAEffectiveDatePanel.Visible = True 'this triggers population of text

                        Me.DateTimePicker1.Checked = True
                        Me.DateTimePicker1.Format = DateTimePickerFormat.Custom
                        Me.DateTimePicker1.CustomFormat = "yyyy"
                        Me.DateTimePicker1.ShowUpDown = True
                        Me.DateTimePicker1.Value = DTP
                        Me.DateTimePicker1.Checked = False

                        Me.HRASchedulePanel.Visible = True

                        If CDec(CType(Me.HRAModificationReasons.SelectedItem, DataRowView)("TRANSACTION_TYPE_USEDBY")) = 5 Then Me.HRAEventDatePanel.Visible = True

                        Me.HRATriggerEventsList.SelectedIndex = -1

                    Case 7, 8 'funding

                        Me.HRASchedulePanel.Visible = True
                        Me.HRAEffectiveDatePanel.Visible = True
                        Me.HRATriggerEventsList.DataSource = Nothing

                        _HRAALLEventHistoryDS = CMSDALFDBHRA.RetrieveHRAAllEventHistory(_FamilyID, _RelationID)

                        If CDec(CType(Me.HRAModificationReasons.SelectedItem, DataRowView)("TRANSACTION_TYPE_USEDBY")) = 8 Then 'Cancel

                            Dim EventListQuery = (
                                                    From HRATransactions In _HRAALLEventHistoryDS.Tables(0).AsEnumerable()
                                                    Where HRATransactions.Field(Of Decimal?)("PARTICIPANT_ONLY_SW") IsNot Nothing _
                                                    AndAlso HRATransactions.Field(Of Integer?)("TRANSACTION_TYPE") IsNot Nothing _
                                                    AndAlso HRATransactions.Field(Of Decimal?)("MANUAL_USE_SW") IsNot Nothing AndAlso HRATransactions.Field(Of Decimal?)("MANUAL_USE_SW") = 1
                                                    Group By TRANSACTION_DESCRIPTION = HRATransactions.Field(Of String)("TRANSACTION_DESCRIPTION"),
                                                             TRANSACTION_TYPE = HRATransactions.Field(Of Integer)("TRANSACTION_TYPE"), _
                                                             PARTICIPANT_ONLY_SW = HRATransactions.Field(Of Decimal)("PARTICIPANT_ONLY_SW")
                                                    Into DistinctEvents = Group
                                                    Order By TRANSACTION_DESCRIPTION
                                                )

                            HRATriggerEventsDV = New DataView(EventListQuery.ToDataTable)

                            If HRATriggerEventsDV.Count > 0 Then
                                HRATriggerEventsDV.Sort = "TRANSACTION_DESCRIPTION"

                                If _RelationID IsNot Nothing AndAlso _RelationID > 0 Then
                                    HRATriggerEventsDV.RowFilter = "PARTICIPANT_ONLY_SW = 0"
                                End If

                                HRATriggerEventsList.DataSource = HRATriggerEventsDV
                                HRATriggerEventsList.DisplayMember = "TRANSACTION_DESCRIPTION"
                                HRATriggerEventsList.ValueMember = "TRANSACTION_TYPE"
                            End If

                        Else

                            HRATriggerEventsDV = New DataView(_HRAALLEventHistoryDS.Tables(0)) With {
                                .RowFilter = "TRANSACTION_TYPE_USEDBY = 7"
                            }

                            If _RelationID IsNot Nothing AndAlso _RelationID > 0 Then
                                HRATriggerEventsDV.RowFilter = " AND PARTICIPANT_ONLY_SW = 0"
                            End If

                            HRATriggerEventsList.DataSource = HRATriggerEventsDV

                            HRATriggerEventsList.DisplayMember = "TRIGGER_DESC"
                            HRATriggerEventsList.ValueMember = "TRIGGER_ID"

                        End If
                    Case 9, 10 'Rx Opt In/Out

                        DateTimePicker1.Value = DTP
                        DateTimePicker1.Format = DateTimePickerFormat.Custom
                        DateTimePicker1.CustomFormat = "yyyy"
                        DateTimePicker1.ShowUpDown = True

                        Me.HRAEffectiveDatePanel.Visible = True

                End Select

            End If

        Catch ex As Exception

	Throw
        Finally

            Me.DateTimePicker1.Checked = False
            Me.EventDateDTP.Checked = False
            Me.ExpirationDateDTP.Checked = False

            AddHandler HRAModificationReasons.SelectedIndexChanged, AddressOf HRAModificationReasons_SelectedIndexChanged
            AddHandler DateTimePicker1.ValueChanged, AddressOf DateTimePicker1_ValueChanged
        End Try

    End Sub
    Public Shared Function CharCount(ByVal OrigString As String,
    ByVal Chars As String, Optional ByVal CaseSensitive As Boolean = False) _
      As Long

        '**********************************************
        'PURPOSE: Returns Number of occurrences of a character or
        'or a character sequencence within a string

        'PARAMETERS:
        'OrigString: String to Search in
        'Chars: Character(s) to search for
        'CaseSensitive (Optional): Do a case sensitive search
        'Defaults to false

        'RETURNS:
        'Number of Occurrences of Chars in OrigString

        'EXAMPLES:
        'Debug.Print CharCount("FreeVBCode.com", "E") -- returns 3
        'Debug.Print CharCount("FreeVBCode.com", "E", True) -- returns 0
        'Debug.Print CharCount("FreeVBCode.com", "co") -- returns 2
        ''**********************************************

        Dim lLen As Long
        Dim lCharLen As Long
        Dim lAns As Long
        Dim sInput As String
        Dim sChar As String
        Dim lCtr As Long
        Dim lEndOfLoop As Long
        Dim bytCompareType As Byte

        sInput = OrigString
        If sInput = "" Then Exit Function
        lLen = Len(sInput)
        lCharLen = Len(Chars)
        lEndOfLoop = (lLen - lCharLen) + 1
        bytCompareType = CByte(If(CaseSensitive, vbBinaryCompare,
           vbTextCompare))

        For lCtr = 1 To lEndOfLoop
            sChar = Mid(sInput, CInt(lCtr), CInt(lCharLen))
            If StrComp(sChar, Chars, CType(bytCompareType, CompareMethod)) = 0 Then _
                lAns += 1
        Next

        CharCount = lAns

    End Function

    Private Sub ClaimID_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClaimIDTextBox.LostFocus

        Try

            If CType(sender, TextBox).Text.Trim.Length > 0 Then
                Dim HRAClaimHistory As New DataView(_HRAHistoryDT, "TRANSACTION_TYPE = 7 AND CLAIM_ID = " & CType(sender, TextBox).Text, "", DataViewRowState.CurrentRows)

                If HRAClaimHistory.Count > 0 Then

                    Dim HRAForfeitHistory As New DataView(_HRAHistoryDT, "TRANSACTION_TYPE = 17 AND TRANSACTION_DATE >= '" & CDate(HRAClaimHistory(0)("TRANSACTION_DATE")).ToShortDateString & "' AND EFFECTIVE_DATE = '" & CDate(HRAClaimHistory(0)("EFFECTIVE_DATE")).ToShortDateString & "'", "", DataViewRowState.CurrentRows)

                    Me.ExpirationDateDTP.Enabled = True
                    Me.ExpirationDateDTP.Checked = True

                    If HRAForfeitHistory.Count > 0 Then
                        Me.ExpirationDateDTP.Text = HRAForfeitHistory(0)("EXPIRATION_DATE").ToString.Replace("9999", "9998")
                    Else
                        Me.ExpirationDateDTP.Text = HRAClaimHistory(0)("EXPIRATION_DATE").ToString.Replace("9999", "9998")
                    End If
                End If
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ClaimID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClaimIDTextBox.TextChanged

        Dim TBox As TextBox = CType(sender, TextBox)
        Dim intCnt As Integer
        Dim strTmp As String

        If Not IsNumeric(TBox.Text) AndAlso Len(TBox.Text) > 0 Then
            strTmp = TBox.Text
            For intCnt = 1 To Len(strTmp)
                If Not IsNumeric(Mid(strTmp, intCnt, 1)) AndAlso Len(strTmp) > 0 Then
                    strTmp = Replace(strTmp, Mid(strTmp, intCnt, 1), "")
                End If
            Next
            TBox.Text = strTmp

        End If

    End Sub

    'Private Sub EffectiveDateDTP_Changed(sender As Object, e As EventArgs) Handles EffectiveDateDTP.ValueChanged

    '    If CType(sender, DateTimePicker).Checked <> _EffectiveDateDTPChecked Then
    '        _EffectiveDateDTPChecked = CType(sender, DateTimePicker).Checked
    '        CType(sender, DateTimePicker).Select()
    '        SendKeys.Send("{Right}")
    '    Else
    '    End If

    'End Sub

#End Region

    Private Sub DateTimePicker1_FormatChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.FormatChanged
    End Sub

    Private Sub DateTimePicker1_TextChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.TextChanged
    End Sub

    Private Sub DateTimePicker1_Validating(sender As Object, e As CancelEventArgs) Handles DateTimePicker1.Validating
        Stop
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged

        Dim HRATriggerEventsDV As DataView
        Dim CurrentFormat As DateTimePickerFormat
        Dim CurrentCustomFormat As String

        Try

            RemoveHandler CType(sender, DateTimePicker).ValueChanged, AddressOf DateTimePicker1_ValueChanged

            CurrentFormat = CType(sender, DateTimePicker).Format
            CurrentCustomFormat = CType(sender, DateTimePicker).CustomFormat

            Me.HRATriggerEventsList.Enabled = False

            If CType(sender, DateTimePicker).Checked Then 'date is being activated/changed

                Dim DTP As New DateTime(Year(CType(sender, DateTimePicker).Value), 1, 1, 0, 0, 0)

                If CDec(CType(Me.HRAModificationReasons.SelectedItem, DataRowView)("TRANSACTION_TYPE_USEDBY")) = 5 Then
                    'Join Manual events against transient entries, and only show events not used as choices valid
                    Dim UnusedEventsListQuery = (
                                                 From HRAManualEvents In _HRATriggerEventsDT.AsEnumerable().Where(Function(r) r.Field(Of Integer)("TRANSACTION_TYPE_USEDBY") = 5).Where(Function(r) r.Field(Of Decimal)("MANUAL_USE_SW") = 1D).Where(Function(r) r.Field(Of Decimal)("PARTICIPANT_ONLY_SW") = 0D) Group Join
                                                      HRATransactions In _HRAALLEventHistoryDS.Tables(0).AsEnumerable().Where(Function(r) r.Field(Of Date)("EFFECTIVE_DATE") = DTP).Where(Function(r) r.Field(Of Decimal)("ARCHIVE_SW") <> 1D) On HRAManualEvents.Field(Of String)("TRIGGER_ID") Equals HRATransactions.Field(Of String)("TRIGGER_ID")
                                                 Into TimesEventsUsed = Group _
                                                 Order By HRAManualEvents.Field(Of String)("TRIGGER_ID") _
                                                 Select New With
                                                    {
                                                       .TRIGGER_ID = HRAManualEvents.Field(Of String)("TRIGGER_ID"), _
                                                       .TRIGGER_DESC = HRAManualEvents.Field(Of String)("TRIGGER_DESC"), _
                                                       .MANUAL_USE_SW = HRAManualEvents.Field(Of Decimal)("MANUAL_USE_SW"), _
                                                       .PARTICIPANT_ONLY_SW = HRAManualEvents.Field(Of Decimal)("PARTICIPANT_ONLY_SW"), _
                                                       .TRANSACTION_TYPE_USEDBY = HRAManualEvents.Field(Of Integer)("TRANSACTION_TYPE_USEDBY"), _
                                                       .TimesEventsUsed = TimesEventsUsed.Count()
                                                    }
                                                ).Where(Function(r) r.TimesEventsUsed < 1)

                    HRATriggerEventsDV = New DataView(UnusedEventsListQuery.ToDataTable)

                ElseIf CDec(CType(Me.HRAModificationReasons.SelectedItem, DataRowView)("TRANSACTION_TYPE_USEDBY")) = 6 Then

                    'check which transactions can be canceled / archived
                    Dim InUseEventsListQuery = (
                                                 From HRAManualEvents In _HRATriggerEventsDT.AsEnumerable().Where(Function(r) r.Field(Of Integer)("TRANSACTION_TYPE_USEDBY") = 5).Where(Function(r) r.Field(Of Decimal)("MANUAL_USE_SW") = 1D).Where(Function(r) r.Field(Of Decimal)("PARTICIPANT_ONLY_SW") = 0D) Group Join
                                                      HRATransactions In _HRAALLEventHistoryDS.Tables(0).AsEnumerable().Where(Function(r) r.Field(Of Date)("EFFECTIVE_DATE") = DTP).Where(Function(r) r.Field(Of Decimal)("ARCHIVE_SW") <> 1D) On HRAManualEvents.Field(Of String)("TRIGGER_ID") Equals HRATransactions.Field(Of String)("TRIGGER_ID")
                                                 Into TimesEventsUsed = Group _
                                                 Order By HRAManualEvents.Field(Of String)("TRIGGER_ID") _
                                                 Select New With
                                                    {
                                                       .TRIGGER_ID = HRAManualEvents.Field(Of String)("TRIGGER_ID"), _
                                                       .TRIGGER_DESC = HRAManualEvents.Field(Of String)("TRIGGER_DESC"), _
                                                       .MANUAL_USE_SW = HRAManualEvents.Field(Of Decimal)("MANUAL_USE_SW"), _
                                                       .PARTICIPANT_ONLY_SW = HRAManualEvents.Field(Of Decimal)("PARTICIPANT_ONLY_SW"), _
                                                       .TRANSACTION_TYPE_USEDBY = HRAManualEvents.Field(Of Integer)("TRANSACTION_TYPE_USEDBY"), _
                                                       .TimesEventsUsed = TimesEventsUsed.Count()
                                                    }
                                                ).Where(Function(r) r.TimesEventsUsed > 0)


                    HRATriggerEventsDV = New DataView(InUseEventsListQuery.ToDataTable)

                End If

                'IF THE FORMAT IS NOT CUSTOM, CHANGE IT TO CUSTOM
                'OTHERWISE CHANGE IT TO SOMETHING OTHER THAN CUSTOM
                If CType(sender, DateTimePicker).Format <> DateTimePickerFormat.Custom Then
                    CType(sender, DateTimePicker).Format = DateTimePickerFormat.Custom
                    'SET THE CUSTOM FORMAT TO AN EMPTY STRING
                    CType(sender, DateTimePicker).CustomFormat = ""
                Else
                    CType(sender, DateTimePicker).Format = DateTimePickerFormat.Short
                End If

                'SET BACK THE CACHED VALUES SO THE ACTUAL FORMAT NEVER REALLY CHANGES
                CType(sender, DateTimePicker).Format = CurrentFormat
                CType(sender, DateTimePicker).CustomFormat = CurrentCustomFormat

                SendKeys.Send("{Right}")

                If HRATriggerEventsDV IsNot Nothing AndAlso HRATriggerEventsDV.Count > 0 Then
                    Me.HRATriggerEventsList.Enabled = True

                    Me.HRATriggerEventsList.DataSource = HRATriggerEventsDV

                    Me.HRATriggerEventsList.DisplayMember = "TRIGGER_DESC"
                    Me.HRATriggerEventsList.ValueMember = "TRIGGER_ID"

                    Me.HRATriggerEventsList.SelectedIndex = -1
                End If

            End If

        Catch ex As Exception
            Throw
        Finally
            AddHandler CType(sender, DateTimePicker).ValueChanged, AddressOf DateTimePicker1_ValueChanged
        End Try

    End Sub

    Private Sub DateTimePicker1_VisibleChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.VisibleChanged
    End Sub
End Class