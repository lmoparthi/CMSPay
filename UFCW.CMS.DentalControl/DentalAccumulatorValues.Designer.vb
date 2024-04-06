Imports System.ComponentModel
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DentalAccumulatorValues
    Inherits System.Windows.Forms.UserControl
    Private _FamilyID As Integer = -1
    Private _RelationID As Short? = Nothing
    Private _ClaimID As Integer = -1
    Private _DateOfService As Date
    Private _FirstDay As Date
    Private _LastDay As Date
    Private _APPKEY As String = "UFCW\Claims\"
    Public Event BeforeRefresh(ByVal sender As Object, ByRef Cancel As Boolean)
    Public Event AfterRefresh(ByVal sender As Object)

    Private _ReadOnlyMode As Boolean = True
    'UserControl overrides dispose to clean up the component list.
#Region "Properties"

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")>
    Public Property FamilyID() As Integer
        Get
            Return _FamilyID
        End Get
        Set(ByVal value As Integer)
            _FamilyID = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the RelationID of the Document.")>
    Public Property RelationID() As Short?
        Get
            Return _RelationID
        End Get
        Set(ByVal value As Short?)
            _RelationID = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the First Day of select row.")>
    Public Property FirstDay() As Date
        Get
            Return _FirstDay
        End Get
        Set(ByVal value As Date)
            _FirstDay = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Last Day of select row.")>
    Public Property LastDay() As Date
        Get
            Return _LastDay
        End Get
        Set(ByVal value As Date)
            _LastDay = value
        End Set
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
    Private _Disposed As Boolean ' To detect redundant calls
    Protected Overrides Sub Dispose(disposing As Boolean)

        If _Disposed Then Return

        ' Release any managed resources here.
        If disposing Then

            If PersonalAccumulatorDataGrid IsNot Nothing Then
                PersonalAccumulatorDataGrid.DataSource = Nothing
                PersonalAccumulatorDataGrid.Dispose()
            End If
            PersonalAccumulatorDataGrid = Nothing

            If FamilyAccumulatorsDataGrid IsNot Nothing Then
                FamilyAccumulatorsDataGrid.DataSource = Nothing
                FamilyAccumulatorsDataGrid.Dispose()
            End If
            FamilyAccumulatorsDataGrid = Nothing

            If DentalAccumulatorDetailsDataGrid IsNot Nothing Then
                DentalAccumulatorDetailsDataGrid.DataSource = Nothing
                DentalAccumulatorDetailsDataGrid.Dispose()
            End If
            DentalAccumulatorDetailsDataGrid = Nothing

            If FamilyAccumulatorsPriorDataGrid IsNot Nothing Then
                FamilyAccumulatorsPriorDataGrid.DataSource = Nothing
                FamilyAccumulatorsPriorDataGrid.Dispose()
            End If
            FamilyAccumulatorsPriorDataGrid = Nothing

            If PersonalAccumulatorPriorDataGrid IsNot Nothing Then
                PersonalAccumulatorPriorDataGrid.DataSource = Nothing
                PersonalAccumulatorPriorDataGrid.Dispose()
            End If
            PersonalAccumulatorPriorDataGrid = Nothing

            If (components IsNot Nothing) Then
                components.Dispose()
            End If

        End If

        _Disposed = True
        ' Release any unmanaged resources not wrapped by safe handles here.

        ' Call the base class implementation.
        MyBase.Dispose(disposing)
    End Sub
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Dim DesignMode As Boolean = (LicenseManager.UsageMode = LicenseUsageMode.Designtime)


    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ErrorLabel = New System.Windows.Forms.Label()
        Me.AccumulatorPanel = New System.Windows.Forms.Panel()
        Me.CommandActionPanel = New System.Windows.Forms.Panel()
        Me.ClaimViewButton = New System.Windows.Forms.Button()
        Me.AccumulatorsHistoryButton = New System.Windows.Forms.Button()
        Me.SaveAndCloseButton = New System.Windows.Forms.Button()
        Me.CancelButton = New System.Windows.Forms.Button()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.AccumulatorsPanel = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.FamilyRolloverMAXPriorTextBox = New System.Windows.Forms.TextBox()
        Me.FamilyRolloverMAXTextBox = New System.Windows.Forms.TextBox()
        Me.PersonalRolloverMAXPriorTextBox = New System.Windows.Forms.TextBox()
        Me.PersonalRolloverMAXTextBox = New System.Windows.Forms.TextBox()
        Me.FamilyMAXPriorTextBox = New System.Windows.Forms.TextBox()
        Me.FamilyMAXPriorLabel = New System.Windows.Forms.Label()
        Me.FamilyMAXTextBox = New System.Windows.Forms.TextBox()
        Me.FamilyMAXLabel = New System.Windows.Forms.Label()
        Me.PersonalMAXPriorTextBox = New System.Windows.Forms.TextBox()
        Me.PersonalMAXPriorLabel = New System.Windows.Forms.Label()
        Me.PersonalMAXTextBox = New System.Windows.Forms.TextBox()
        Me.PersonalMAXLabel = New System.Windows.Forms.Label()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.FamilyAccumulatorsDataGrid = New System.Windows.Forms.DataGridView()
        Me.PersonalAccumulatorDataGrid = New System.Windows.Forms.DataGridView()
        Me.FamilyAccumulatorsCurrentLabel = New System.Windows.Forms.Label()
        Me.PersonalAccumulatorCurrentLabel = New System.Windows.Forms.Label()
        Me.FamilyAccumulatorsPriorDataGrid = New System.Windows.Forms.DataGridView()
        Me.PersonalAccumulatorPriorDataGrid = New System.Windows.Forms.DataGridView()
        Me.FamilyAccumulatorsPriorLabel = New System.Windows.Forms.Label()
        Me.PersonalAccumulatorPriorLabel = New System.Windows.Forms.Label()
        Me.AccumulatorPatientName = New System.Windows.Forms.Label()
        Me.LineDetailsGroupBox = New System.Windows.Forms.GroupBox()
        Me.DentalAccumulatorDetailsDataGrid = New DataGridCustom()
        Me.AccumulatorPanel.SuspendLayout()
        Me.CommandActionPanel.SuspendLayout()
        Me.AccumulatorsPanel.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.FamilyAccumulatorsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PersonalAccumulatorDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FamilyAccumulatorsPriorDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PersonalAccumulatorPriorDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LineDetailsGroupBox.SuspendLayout()
        CType(Me.DentalAccumulatorDetailsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ErrorLabel
        '
        Me.ErrorLabel.BackColor = System.Drawing.Color.Red
        Me.ErrorLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.ErrorLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ErrorLabel.ForeColor = System.Drawing.Color.Red
        Me.ErrorLabel.Location = New System.Drawing.Point(0, 0)
        Me.ErrorLabel.Name = "ErrorLabel"
        Me.ErrorLabel.Size = New System.Drawing.Size(1010, 16)
        Me.ErrorLabel.TabIndex = 72
        Me.ErrorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ErrorLabel.Visible = False
        '
        'AccumulatorPanel
        '
        Me.AccumulatorPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AccumulatorPanel.Controls.Add(Me.CommandActionPanel)
        Me.AccumulatorPanel.Controls.Add(Me.AccumulatorsPanel)
        Me.AccumulatorPanel.Location = New System.Drawing.Point(3, 5)
        Me.AccumulatorPanel.Name = "AccumulatorPanel"
        Me.AccumulatorPanel.Size = New System.Drawing.Size(586, 461)
        Me.AccumulatorPanel.TabIndex = 71
        '
        'CommandActionPanel
        '
        Me.CommandActionPanel.BackColor = System.Drawing.SystemColors.Control
        Me.CommandActionPanel.CausesValidation = False
        Me.CommandActionPanel.Controls.Add(Me.ClaimViewButton)
        Me.CommandActionPanel.Controls.Add(Me.AccumulatorsHistoryButton)
        Me.CommandActionPanel.Controls.Add(Me.SaveAndCloseButton)
        Me.CommandActionPanel.Controls.Add(Me.CancelButton)
        Me.CommandActionPanel.Controls.Add(Me.SaveButton)
        Me.CommandActionPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.CommandActionPanel.Location = New System.Drawing.Point(0, 435)
        Me.CommandActionPanel.Name = "CommandActionPanel"
        Me.CommandActionPanel.Size = New System.Drawing.Size(586, 26)
        Me.CommandActionPanel.TabIndex = 68
        '
        'ClaimViewButton
        '
        Me.ClaimViewButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClaimViewButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ClaimViewButton.Location = New System.Drawing.Point(506, 2)
        Me.ClaimViewButton.Name = "ClaimViewButton"
        Me.ClaimViewButton.Size = New System.Drawing.Size(75, 23)
        Me.ClaimViewButton.TabIndex = 76
        Me.ClaimViewButton.Text = "Claim View"
        Me.ClaimViewButton.Visible = False
        '
        'AccumulatorsHistoryButton
        '
        Me.AccumulatorsHistoryButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AccumulatorsHistoryButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.AccumulatorsHistoryButton.Location = New System.Drawing.Point(3, 2)
        Me.AccumulatorsHistoryButton.Name = "AccumulatorsHistoryButton"
        Me.AccumulatorsHistoryButton.Size = New System.Drawing.Size(75, 23)
        Me.AccumulatorsHistoryButton.TabIndex = 75
        Me.AccumulatorsHistoryButton.Text = "History"
        Me.AccumulatorsHistoryButton.Visible = False
        '
        'SaveAndCloseButton
        '
        Me.SaveAndCloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SaveAndCloseButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.SaveAndCloseButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SaveAndCloseButton.Location = New System.Drawing.Point(171, 2)
        Me.SaveAndCloseButton.Name = "SaveAndCloseButton"
        Me.SaveAndCloseButton.Size = New System.Drawing.Size(83, 23)
        Me.SaveAndCloseButton.TabIndex = 74
        Me.SaveAndCloseButton.Text = "Save && Close"
        Me.SaveAndCloseButton.Visible = False
        '
        'CancelButton
        '
        Me.CancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelButton.CausesValidation = False
        Me.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CancelButton.Location = New System.Drawing.Point(263, 2)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelButton.TabIndex = 73
        Me.CancelButton.Text = "Cancel"
        Me.CancelButton.Visible = False
        '
        'SaveButton
        '
        Me.SaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.SaveButton.Location = New System.Drawing.Point(87, 2)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(75, 23)
        Me.SaveButton.TabIndex = 72
        Me.SaveButton.Text = "Save"
        Me.SaveButton.Visible = False
        '
        'AccumulatorsPanel
        '
        Me.AccumulatorsPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AccumulatorsPanel.Controls.Add(Me.GroupBox1)
        Me.AccumulatorsPanel.Controls.Add(Me.SplitContainer1)
        Me.AccumulatorsPanel.Controls.Add(Me.AccumulatorPatientName)
        Me.AccumulatorsPanel.Location = New System.Drawing.Point(0, 0)
        Me.AccumulatorsPanel.Name = "AccumulatorsPanel"
        Me.AccumulatorsPanel.Size = New System.Drawing.Size(574, 411)
        Me.AccumulatorsPanel.TabIndex = 67
        '
        'GroupBox1
        '
        Me.GroupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.FamilyRolloverMAXPriorTextBox)
        Me.GroupBox1.Controls.Add(Me.FamilyRolloverMAXTextBox)
        Me.GroupBox1.Controls.Add(Me.PersonalRolloverMAXPriorTextBox)
        Me.GroupBox1.Controls.Add(Me.PersonalRolloverMAXTextBox)
        Me.GroupBox1.Controls.Add(Me.FamilyMAXPriorTextBox)
        Me.GroupBox1.Controls.Add(Me.FamilyMAXPriorLabel)
        Me.GroupBox1.Controls.Add(Me.FamilyMAXTextBox)
        Me.GroupBox1.Controls.Add(Me.FamilyMAXLabel)
        Me.GroupBox1.Controls.Add(Me.PersonalMAXPriorTextBox)
        Me.GroupBox1.Controls.Add(Me.PersonalMAXPriorLabel)
        Me.GroupBox1.Controls.Add(Me.PersonalMAXTextBox)
        Me.GroupBox1.Controls.Add(Me.PersonalMAXLabel)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 15)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(559, 62)
        Me.GroupBox1.TabIndex = 67
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "MAX Amount / Rollover Amount: "
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(448, 35)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(49, 13)
        Me.Label3.TabIndex = 88
        Me.Label3.Text = "Rollover "
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(447, 15)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(49, 13)
        Me.Label4.TabIndex = 87
        Me.Label4.Text = "Rollover "
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(178, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 13)
        Me.Label2.TabIndex = 86
        Me.Label2.Text = "Rollover "
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(177, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 13)
        Me.Label1.TabIndex = 85
        Me.Label1.Text = "Rollover "
        '
        'FamilyRolloverMAXPriorTextBox
        '
        Me.FamilyRolloverMAXPriorTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyRolloverMAXPriorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FamilyRolloverMAXPriorTextBox.Location = New System.Drawing.Point(498, 34)
        Me.FamilyRolloverMAXPriorTextBox.Name = "FamilyRolloverMAXPriorTextBox"
        Me.FamilyRolloverMAXPriorTextBox.ReadOnly = True
        Me.FamilyRolloverMAXPriorTextBox.Size = New System.Drawing.Size(55, 20)
        Me.FamilyRolloverMAXPriorTextBox.TabIndex = 84
        '
        'FamilyRolloverMAXTextBox
        '
        Me.FamilyRolloverMAXTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyRolloverMAXTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FamilyRolloverMAXTextBox.Location = New System.Drawing.Point(498, 14)
        Me.FamilyRolloverMAXTextBox.Name = "FamilyRolloverMAXTextBox"
        Me.FamilyRolloverMAXTextBox.ReadOnly = True
        Me.FamilyRolloverMAXTextBox.Size = New System.Drawing.Size(55, 20)
        Me.FamilyRolloverMAXTextBox.TabIndex = 83
        '
        'PersonalRolloverMAXPriorTextBox
        '
        Me.PersonalRolloverMAXPriorTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PersonalRolloverMAXPriorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PersonalRolloverMAXPriorTextBox.Location = New System.Drawing.Point(227, 33)
        Me.PersonalRolloverMAXPriorTextBox.Name = "PersonalRolloverMAXPriorTextBox"
        Me.PersonalRolloverMAXPriorTextBox.ReadOnly = True
        Me.PersonalRolloverMAXPriorTextBox.Size = New System.Drawing.Size(55, 20)
        Me.PersonalRolloverMAXPriorTextBox.TabIndex = 82
        '
        'PersonalRolloverMAXTextBox
        '
        Me.PersonalRolloverMAXTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PersonalRolloverMAXTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PersonalRolloverMAXTextBox.Location = New System.Drawing.Point(227, 13)
        Me.PersonalRolloverMAXTextBox.Name = "PersonalRolloverMAXTextBox"
        Me.PersonalRolloverMAXTextBox.ReadOnly = True
        Me.PersonalRolloverMAXTextBox.Size = New System.Drawing.Size(55, 20)
        Me.PersonalRolloverMAXTextBox.TabIndex = 81
        '
        'FamilyMAXPriorTextBox
        '
        Me.FamilyMAXPriorTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyMAXPriorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FamilyMAXPriorTextBox.Location = New System.Drawing.Point(390, 34)
        Me.FamilyMAXPriorTextBox.Name = "FamilyMAXPriorTextBox"
        Me.FamilyMAXPriorTextBox.ReadOnly = True
        Me.FamilyMAXPriorTextBox.Size = New System.Drawing.Size(55, 20)
        Me.FamilyMAXPriorTextBox.TabIndex = 80
        '
        'FamilyMAXPriorLabel
        '
        Me.FamilyMAXPriorLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyMAXPriorLabel.AutoSize = True
        Me.FamilyMAXPriorLabel.Location = New System.Drawing.Point(287, 35)
        Me.FamilyMAXPriorLabel.Name = "FamilyMAXPriorLabel"
        Me.FamilyMAXPriorLabel.Size = New System.Drawing.Size(95, 13)
        Me.FamilyMAXPriorLabel.TabIndex = 79
        Me.FamilyMAXPriorLabel.Text = "Family MAX (2020)"
        '
        'FamilyMAXTextBox
        '
        Me.FamilyMAXTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyMAXTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FamilyMAXTextBox.Location = New System.Drawing.Point(390, 14)
        Me.FamilyMAXTextBox.Name = "FamilyMAXTextBox"
        Me.FamilyMAXTextBox.ReadOnly = True
        Me.FamilyMAXTextBox.Size = New System.Drawing.Size(55, 20)
        Me.FamilyMAXTextBox.TabIndex = 78
        '
        'FamilyMAXLabel
        '
        Me.FamilyMAXLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyMAXLabel.AutoSize = True
        Me.FamilyMAXLabel.Location = New System.Drawing.Point(287, 16)
        Me.FamilyMAXLabel.Name = "FamilyMAXLabel"
        Me.FamilyMAXLabel.Size = New System.Drawing.Size(95, 13)
        Me.FamilyMAXLabel.TabIndex = 77
        Me.FamilyMAXLabel.Text = "Family MAX (2021)"
        '
        'PersonalMAXPriorTextBox
        '
        Me.PersonalMAXPriorTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PersonalMAXPriorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PersonalMAXPriorTextBox.Location = New System.Drawing.Point(120, 34)
        Me.PersonalMAXPriorTextBox.Name = "PersonalMAXPriorTextBox"
        Me.PersonalMAXPriorTextBox.ReadOnly = True
        Me.PersonalMAXPriorTextBox.Size = New System.Drawing.Size(55, 20)
        Me.PersonalMAXPriorTextBox.TabIndex = 76
        '
        'PersonalMAXPriorLabel
        '
        Me.PersonalMAXPriorLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PersonalMAXPriorLabel.AutoSize = True
        Me.PersonalMAXPriorLabel.Location = New System.Drawing.Point(3, 36)
        Me.PersonalMAXPriorLabel.Name = "PersonalMAXPriorLabel"
        Me.PersonalMAXPriorLabel.Size = New System.Drawing.Size(107, 13)
        Me.PersonalMAXPriorLabel.TabIndex = 75
        Me.PersonalMAXPriorLabel.Text = "Personal MAX (2020)"
        '
        'PersonalMAXTextBox
        '
        Me.PersonalMAXTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PersonalMAXTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PersonalMAXTextBox.Location = New System.Drawing.Point(120, 14)
        Me.PersonalMAXTextBox.Name = "PersonalMAXTextBox"
        Me.PersonalMAXTextBox.ReadOnly = True
        Me.PersonalMAXTextBox.Size = New System.Drawing.Size(55, 20)
        Me.PersonalMAXTextBox.TabIndex = 72
        '
        'PersonalMAXLabel
        '
        Me.PersonalMAXLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PersonalMAXLabel.AutoSize = True
        Me.PersonalMAXLabel.Location = New System.Drawing.Point(3, 16)
        Me.PersonalMAXLabel.Name = "PersonalMAXLabel"
        Me.PersonalMAXLabel.Size = New System.Drawing.Size(107, 13)
        Me.PersonalMAXLabel.TabIndex = 71
        Me.PersonalMAXLabel.Text = "Personal MAX (2021)"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Location = New System.Drawing.Point(15, 78)
        Me.SplitContainer1.MinimumSize = New System.Drawing.Size(200, 150)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Panel1.Controls.Add(Me.FamilyAccumulatorsDataGrid)
        Me.SplitContainer1.Panel1.Controls.Add(Me.PersonalAccumulatorDataGrid)
        Me.SplitContainer1.Panel1.Controls.Add(Me.FamilyAccumulatorsCurrentLabel)
        Me.SplitContainer1.Panel1.Controls.Add(Me.PersonalAccumulatorCurrentLabel)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Panel2.Controls.Add(Me.FamilyAccumulatorsPriorDataGrid)
        Me.SplitContainer1.Panel2.Controls.Add(Me.PersonalAccumulatorPriorDataGrid)
        Me.SplitContainer1.Panel2.Controls.Add(Me.FamilyAccumulatorsPriorLabel)
        Me.SplitContainer1.Panel2.Controls.Add(Me.PersonalAccumulatorPriorLabel)
        Me.SplitContainer1.Size = New System.Drawing.Size(556, 340)
        Me.SplitContainer1.SplitterDistance = 162
        Me.SplitContainer1.TabIndex = 66
        '
        'FamilyAccumulatorsDataGrid
        '
        Me.FamilyAccumulatorsDataGrid.AllowUserToAddRows = False
        Me.FamilyAccumulatorsDataGrid.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray
        Me.FamilyAccumulatorsDataGrid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.FamilyAccumulatorsDataGrid.BackgroundColor = System.Drawing.Color.White
        Me.FamilyAccumulatorsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.FamilyAccumulatorsDataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.FamilyAccumulatorsDataGrid.Location = New System.Drawing.Point(277, 20)
        Me.FamilyAccumulatorsDataGrid.MultiSelect = False
        Me.FamilyAccumulatorsDataGrid.Name = "FamilyAccumulatorsDataGrid"
        Me.FamilyAccumulatorsDataGrid.ReadOnly = True
        Me.FamilyAccumulatorsDataGrid.Size = New System.Drawing.Size(274, 140)
        Me.FamilyAccumulatorsDataGrid.TabIndex = 72
        '
        'PersonalAccumulatorDataGrid
        '
        Me.PersonalAccumulatorDataGrid.AllowUserToAddRows = False
        Me.PersonalAccumulatorDataGrid.AllowUserToDeleteRows = False
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.LightGray
        Me.PersonalAccumulatorDataGrid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle2
        Me.PersonalAccumulatorDataGrid.BackgroundColor = System.Drawing.Color.White
        Me.PersonalAccumulatorDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.PersonalAccumulatorDataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.PersonalAccumulatorDataGrid.Location = New System.Drawing.Point(3, 20)
        Me.PersonalAccumulatorDataGrid.MultiSelect = False
        Me.PersonalAccumulatorDataGrid.Name = "PersonalAccumulatorDataGrid"
        Me.PersonalAccumulatorDataGrid.ReadOnly = True
        Me.PersonalAccumulatorDataGrid.Size = New System.Drawing.Size(268, 140)
        Me.PersonalAccumulatorDataGrid.TabIndex = 71
        '
        'FamilyAccumulatorsCurrentLabel
        '
        Me.FamilyAccumulatorsCurrentLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.FamilyAccumulatorsCurrentLabel.Location = New System.Drawing.Point(280, 2)
        Me.FamilyAccumulatorsCurrentLabel.Name = "FamilyAccumulatorsCurrentLabel"
        Me.FamilyAccumulatorsCurrentLabel.Size = New System.Drawing.Size(164, 15)
        Me.FamilyAccumulatorsCurrentLabel.TabIndex = 65
        Me.FamilyAccumulatorsCurrentLabel.Text = "Family Accumulators (Current)"
        '
        'PersonalAccumulatorCurrentLabel
        '
        Me.PersonalAccumulatorCurrentLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.PersonalAccumulatorCurrentLabel.Location = New System.Drawing.Point(11, 2)
        Me.PersonalAccumulatorCurrentLabel.Name = "PersonalAccumulatorCurrentLabel"
        Me.PersonalAccumulatorCurrentLabel.Size = New System.Drawing.Size(170, 15)
        Me.PersonalAccumulatorCurrentLabel.TabIndex = 64
        Me.PersonalAccumulatorCurrentLabel.Text = "Personal Accumulators (Current)"
        '
        'FamilyAccumulatorsPriorDataGrid
        '
        Me.FamilyAccumulatorsPriorDataGrid.AllowUserToAddRows = False
        Me.FamilyAccumulatorsPriorDataGrid.AllowUserToDeleteRows = False
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.LightGray
        Me.FamilyAccumulatorsPriorDataGrid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle3
        Me.FamilyAccumulatorsPriorDataGrid.BackgroundColor = System.Drawing.Color.White
        Me.FamilyAccumulatorsPriorDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.FamilyAccumulatorsPriorDataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.FamilyAccumulatorsPriorDataGrid.Location = New System.Drawing.Point(277, 21)
        Me.FamilyAccumulatorsPriorDataGrid.MultiSelect = False
        Me.FamilyAccumulatorsPriorDataGrid.Name = "FamilyAccumulatorsPriorDataGrid"
        Me.FamilyAccumulatorsPriorDataGrid.ReadOnly = True
        Me.FamilyAccumulatorsPriorDataGrid.Size = New System.Drawing.Size(274, 140)
        Me.FamilyAccumulatorsPriorDataGrid.TabIndex = 73
        '
        'PersonalAccumulatorPriorDataGrid
        '
        Me.PersonalAccumulatorPriorDataGrid.AllowUserToAddRows = False
        Me.PersonalAccumulatorPriorDataGrid.AllowUserToDeleteRows = False
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.LightGray
        Me.PersonalAccumulatorPriorDataGrid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle4
        Me.PersonalAccumulatorPriorDataGrid.BackgroundColor = System.Drawing.Color.White
        Me.PersonalAccumulatorPriorDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.PersonalAccumulatorPriorDataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.PersonalAccumulatorPriorDataGrid.Location = New System.Drawing.Point(3, 21)
        Me.PersonalAccumulatorPriorDataGrid.MultiSelect = False
        Me.PersonalAccumulatorPriorDataGrid.Name = "PersonalAccumulatorPriorDataGrid"
        Me.PersonalAccumulatorPriorDataGrid.ReadOnly = True
        Me.PersonalAccumulatorPriorDataGrid.Size = New System.Drawing.Size(268, 140)
        Me.PersonalAccumulatorPriorDataGrid.TabIndex = 72
        '
        'FamilyAccumulatorsPriorLabel
        '
        Me.FamilyAccumulatorsPriorLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.FamilyAccumulatorsPriorLabel.Location = New System.Drawing.Point(279, 3)
        Me.FamilyAccumulatorsPriorLabel.Name = "FamilyAccumulatorsPriorLabel"
        Me.FamilyAccumulatorsPriorLabel.Size = New System.Drawing.Size(164, 15)
        Me.FamilyAccumulatorsPriorLabel.TabIndex = 51
        Me.FamilyAccumulatorsPriorLabel.Text = "Family Accumulators (Prior)"
        '
        'PersonalAccumulatorPriorLabel
        '
        Me.PersonalAccumulatorPriorLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.PersonalAccumulatorPriorLabel.Location = New System.Drawing.Point(12, 3)
        Me.PersonalAccumulatorPriorLabel.Name = "PersonalAccumulatorPriorLabel"
        Me.PersonalAccumulatorPriorLabel.Size = New System.Drawing.Size(170, 15)
        Me.PersonalAccumulatorPriorLabel.TabIndex = 50
        Me.PersonalAccumulatorPriorLabel.Text = "Personal Accumulators (Prior)"
        '
        'AccumulatorPatientName
        '
        Me.AccumulatorPatientName.BackColor = System.Drawing.SystemColors.Control
        Me.AccumulatorPatientName.Dock = System.Windows.Forms.DockStyle.Top
        Me.AccumulatorPatientName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AccumulatorPatientName.Location = New System.Drawing.Point(0, 0)
        Me.AccumulatorPatientName.Name = "AccumulatorPatientName"
        Me.AccumulatorPatientName.Size = New System.Drawing.Size(574, 18)
        Me.AccumulatorPatientName.TabIndex = 63
        Me.AccumulatorPatientName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LineDetailsGroupBox
        '
        Me.LineDetailsGroupBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineDetailsGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.LineDetailsGroupBox.Controls.Add(Me.DentalAccumulatorDetailsDataGrid)
        Me.LineDetailsGroupBox.Location = New System.Drawing.Point(595, 17)
        Me.LineDetailsGroupBox.Name = "LineDetailsGroupBox"
        Me.LineDetailsGroupBox.Size = New System.Drawing.Size(411, 397)
        Me.LineDetailsGroupBox.TabIndex = 69
        Me.LineDetailsGroupBox.TabStop = False
        Me.LineDetailsGroupBox.Text = "Line Details"
        '
        'DentalAccumulatorDetailsDataGrid
        '
        Me.DentalAccumulatorDetailsDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.DentalAccumulatorDetailsDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.DentalAccumulatorDetailsDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DentalAccumulatorDetailsDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DentalAccumulatorDetailsDataGrid.ADGroupsThatCanFind = ""
        Me.DentalAccumulatorDetailsDataGrid.ADGroupsThatCanMultiSort = ""
        Me.DentalAccumulatorDetailsDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.DentalAccumulatorDetailsDataGrid.AllowAutoSize = False
        Me.DentalAccumulatorDetailsDataGrid.AllowColumnReorder = False
        Me.DentalAccumulatorDetailsDataGrid.AllowCopy = False
        Me.DentalAccumulatorDetailsDataGrid.AllowCustomize = False
        Me.DentalAccumulatorDetailsDataGrid.AllowDelete = False
        Me.DentalAccumulatorDetailsDataGrid.AllowDragDrop = False
        Me.DentalAccumulatorDetailsDataGrid.AllowEdit = False
        Me.DentalAccumulatorDetailsDataGrid.AllowExport = False
        Me.DentalAccumulatorDetailsDataGrid.AllowFilter = False
        Me.DentalAccumulatorDetailsDataGrid.AllowFind = False
        Me.DentalAccumulatorDetailsDataGrid.AllowGoTo = False
        Me.DentalAccumulatorDetailsDataGrid.AllowMultiSelect = False
        Me.DentalAccumulatorDetailsDataGrid.AllowMultiSort = False
        Me.DentalAccumulatorDetailsDataGrid.AllowNavigation = False
        Me.DentalAccumulatorDetailsDataGrid.AllowNew = False
        Me.DentalAccumulatorDetailsDataGrid.AllowPrint = False
        Me.DentalAccumulatorDetailsDataGrid.AllowRefresh = False
        Me.DentalAccumulatorDetailsDataGrid.AllowSorting = False
        Me.DentalAccumulatorDetailsDataGrid.AlternatingBackColor = System.Drawing.Color.LightGray
        Me.DentalAccumulatorDetailsDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DentalAccumulatorDetailsDataGrid.AppKey = "UFCW\Claims\"
        Me.DentalAccumulatorDetailsDataGrid.AutoSaveCols = True
        Me.DentalAccumulatorDetailsDataGrid.BackColor = System.Drawing.Color.DarkGray
        Me.DentalAccumulatorDetailsDataGrid.BackgroundColor = System.Drawing.SystemColors.Control
        Me.DentalAccumulatorDetailsDataGrid.CaptionBackColor = System.Drawing.Color.White
        Me.DentalAccumulatorDetailsDataGrid.CaptionFont = New System.Drawing.Font("Verdana", 10.0!)
        Me.DentalAccumulatorDetailsDataGrid.CaptionForeColor = System.Drawing.Color.Navy
        Me.DentalAccumulatorDetailsDataGrid.CaptionVisible = False
        Me.DentalAccumulatorDetailsDataGrid.ColumnHeaderLabel = Nothing
        Me.DentalAccumulatorDetailsDataGrid.ColumnRePositioning = False
        Me.DentalAccumulatorDetailsDataGrid.ColumnResizing = False
        Me.DentalAccumulatorDetailsDataGrid.ConfirmDelete = True
        Me.DentalAccumulatorDetailsDataGrid.CopySelectedOnly = True
        Me.DentalAccumulatorDetailsDataGrid.CurrentBSPosition = -1
        Me.DentalAccumulatorDetailsDataGrid.DataMember = ""
        Me.DentalAccumulatorDetailsDataGrid.DragColumn = 0
        Me.DentalAccumulatorDetailsDataGrid.ExportSelectedOnly = True
        Me.DentalAccumulatorDetailsDataGrid.ForeColor = System.Drawing.Color.Black
        Me.DentalAccumulatorDetailsDataGrid.GridLineColor = System.Drawing.Color.Black
        Me.DentalAccumulatorDetailsDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.DentalAccumulatorDetailsDataGrid.HeaderBackColor = System.Drawing.Color.Silver
        Me.DentalAccumulatorDetailsDataGrid.HeaderForeColor = System.Drawing.Color.Black
        Me.DentalAccumulatorDetailsDataGrid.HighlightedRow = Nothing
        Me.DentalAccumulatorDetailsDataGrid.HighLightModifiedRows = False
        Me.DentalAccumulatorDetailsDataGrid.IsMouseDown = False
        Me.DentalAccumulatorDetailsDataGrid.LastGoToLine = ""
        Me.DentalAccumulatorDetailsDataGrid.LinkColor = System.Drawing.Color.Navy
        Me.DentalAccumulatorDetailsDataGrid.Location = New System.Drawing.Point(6, 19)
        Me.DentalAccumulatorDetailsDataGrid.MultiSort = False
        Me.DentalAccumulatorDetailsDataGrid.Name = "DentalAccumulatorDetailsDataGrid"
        Me.DentalAccumulatorDetailsDataGrid.OldSelectedRow = Nothing
        Me.DentalAccumulatorDetailsDataGrid.ParentRowsBackColor = System.Drawing.Color.White
        Me.DentalAccumulatorDetailsDataGrid.ParentRowsForeColor = System.Drawing.Color.Black
        Me.DentalAccumulatorDetailsDataGrid.PreviousBSPosition = -1
        Me.DentalAccumulatorDetailsDataGrid.ReadOnly = True
        Me.DentalAccumulatorDetailsDataGrid.RetainRowSelectionAfterSort = True
        Me.DentalAccumulatorDetailsDataGrid.RowHeadersVisible = False
        Me.DentalAccumulatorDetailsDataGrid.SelectionBackColor = System.Drawing.Color.Navy
        Me.DentalAccumulatorDetailsDataGrid.SelectionForeColor = System.Drawing.Color.White
        Me.DentalAccumulatorDetailsDataGrid.SetRowOnRightClick = True
        Me.DentalAccumulatorDetailsDataGrid.ShiftPressed = False
        Me.DentalAccumulatorDetailsDataGrid.SingleClickBooleanColumns = True
        Me.DentalAccumulatorDetailsDataGrid.Size = New System.Drawing.Size(399, 372)
        Me.DentalAccumulatorDetailsDataGrid.Sort = Nothing
        Me.DentalAccumulatorDetailsDataGrid.StyleName = ""
        Me.DentalAccumulatorDetailsDataGrid.SubKey = ""
        Me.DentalAccumulatorDetailsDataGrid.SuppressMouseDown = False
        Me.DentalAccumulatorDetailsDataGrid.SuppressTriangle = False
        Me.DentalAccumulatorDetailsDataGrid.TabIndex = 45
        '
        'DentalAccumulatorValues
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ErrorLabel)
        Me.Controls.Add(Me.AccumulatorPanel)
        Me.Controls.Add(Me.LineDetailsGroupBox)
        Me.Name = "DentalAccumulatorValues"
        Me.Size = New System.Drawing.Size(1010, 467)
        Me.AccumulatorPanel.ResumeLayout(False)
        Me.CommandActionPanel.ResumeLayout(False)
        Me.AccumulatorsPanel.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.FamilyAccumulatorsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PersonalAccumulatorDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FamilyAccumulatorsPriorDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PersonalAccumulatorPriorDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LineDetailsGroupBox.ResumeLayout(False)
        CType(Me.DentalAccumulatorDetailsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Public Sub New(ByVal familyID As Integer, ByVal relationID As Short?, ByVal firstDay As Date, ByVal lastDay As Date)
        Me.New()

        _FamilyID = familyID
        _RelationID = relationID
        _FirstDay = firstDay
        _LastDay = lastDay

        MessageBox.Show(_FamilyID & "-" & _RelationID & "-" & _FirstDay & "-" & _LastDay)
    End Sub

    Public WithEvents ErrorLabel As Label
    Friend WithEvents AccumulatorPanel As Panel
    Friend WithEvents CommandActionPanel As Panel
    Public WithEvents ClaimViewButton As Button
    Public WithEvents AccumulatorsHistoryButton As Button
    Public WithEvents SaveAndCloseButton As Button
    Public WithEvents CancelButton As Button
    Public WithEvents SaveButton As Button
    Friend WithEvents AccumulatorsPanel As Panel
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents PersonalMAXPriorTextBox As TextBox
    Friend WithEvents PersonalMAXPriorLabel As Label
    Friend WithEvents PersonalMAXTextBox As TextBox
    Friend WithEvents PersonalMAXLabel As Label
    Friend WithEvents SplitContainer1 As SplitContainer
    Public WithEvents FamilyAccumulatorsCurrentLabel As Label
    Public WithEvents PersonalAccumulatorCurrentLabel As Label
    Public WithEvents FamilyAccumulatorsPriorLabel As Label
    Public WithEvents PersonalAccumulatorPriorLabel As Label
    Public WithEvents AccumulatorPatientName As Label
    Public WithEvents LineDetailsGroupBox As GroupBox
    Friend WithEvents PersonalAccumulatorDataGrid As DataGridView
    Friend WithEvents PersonalAccumulatorPriorDataGrid As DataGridView
    Friend WithEvents FamilyAccumulatorsDataGrid As DataGridView
    Friend WithEvents FamilyAccumulatorsPriorDataGrid As DataGridView
    Friend WithEvents FamilyMAXPriorTextBox As TextBox
    Friend WithEvents FamilyMAXPriorLabel As Label
    Friend WithEvents FamilyMAXTextBox As TextBox
    Friend WithEvents FamilyMAXLabel As Label
    Friend WithEvents FamilyRolloverMAXPriorTextBox As TextBox
    Friend WithEvents FamilyRolloverMAXTextBox As TextBox
    Friend WithEvents PersonalRolloverMAXPriorTextBox As TextBox
    Friend WithEvents PersonalRolloverMAXTextBox As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Public WithEvents DentalAccumulatorDetailsDataGrid As DataGridCustom
End Class
