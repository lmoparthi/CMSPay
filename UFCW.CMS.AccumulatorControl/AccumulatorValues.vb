Option Infer On


Imports System.ComponentModel

Public Class AccumulatorValues
    Inherits System.Windows.Forms.UserControl
    ''' -----------------------------------------------------------------------------
    ''' Project	 : UFCW.CMS.CustomerServiceControl
    ''' Class	 : CMS.CustomerServiceControl.AccumulatorValues
    '''
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    '''
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[paulw]	10/31/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        Dim DesignMode As Boolean = (LicenseManager.UsageMode = LicenseUsageMode.Designtime)

        If Not DesignMode Then
            Me.LineDetailsDataGrid.Visible = _ShowLineDetails
            Me.LineDetailsGroupBox.Visible = _ShowLineDetails
        End If

    End Sub

    'UserControl overrides dispose to clean up the component list.
    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.

            If components IsNot Nothing Then
                components.Dispose()
            End If

            If PersonalAccumulatorDataGrid IsNot Nothing Then
                PersonalAccumulatorDataGrid.TableStyles.Clear()
                PersonalAccumulatorDataGrid.DataSource = Nothing
                PersonalAccumulatorDataGrid.Dispose()
            End If
            PersonalAccumulatorDataGrid = Nothing

            If FamilyAccumulatorsDataGrid IsNot Nothing Then
                FamilyAccumulatorsDataGrid.TableStyles.Clear()
                FamilyAccumulatorsDataGrid.DataSource = Nothing
                FamilyAccumulatorsDataGrid.Dispose()
            End If
            FamilyAccumulatorsDataGrid = Nothing

            If LineDetailsDataGrid IsNot Nothing Then
                LineDetailsDataGrid.TableStyles.Clear()
                LineDetailsDataGrid.DataSource = Nothing
                LineDetailsDataGrid.Dispose()
            End If
            LineDetailsDataGrid = Nothing

            If FamilyAccumulatorsPriorDataGrid IsNot Nothing Then
                FamilyAccumulatorsPriorDataGrid.TableStyles.Clear()
                FamilyAccumulatorsPriorDataGrid.DataSource = Nothing
                FamilyAccumulatorsPriorDataGrid.Dispose()
            End If
            FamilyAccumulatorsPriorDataGrid = Nothing

            If PersonalAccumulatorPriorDataGrid IsNot Nothing Then
                PersonalAccumulatorPriorDataGrid.TableStyles.Clear()
                PersonalAccumulatorPriorDataGrid.DataSource = Nothing
                PersonalAccumulatorPriorDataGrid.Dispose()
            End If
            PersonalAccumulatorPriorDataGrid = Nothing

            If ImageList1 IsNot Nothing Then
                ImageList1.Images.Clear()
                ImageList1.Dispose()
            End If

            DateTimePicker1.Dispose()

            'If _MemberAccumulatorManager IsNot Nothing Then _MemberAccumulatorManager.Dispose()
            If _MemberAccumulatorManager IsNot Nothing Then _MemberAccumulatorManager = Nothing
        End If

        ' Free any unmanaged objects here.
        '
        _Disposed = True

        ' Call base class implementation.
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents DateStartOrEnd As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateQuantity As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents cmbDateDirection As System.Windows.Forms.ComboBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents DateType As System.Windows.Forms.ComboBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents GetLatest As System.Windows.Forms.Button
    Friend WithEvents AccumulatorValueSummary As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents UseYear As System.Windows.Forms.CheckBox
    Friend WithEvents CurrentValueYear As System.Windows.Forms.TextBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents GetValueGroupBox As System.Windows.Forms.GroupBox
    Public WithEvents ErrorLabel As System.Windows.Forms.Label
    Friend WithEvents AccumulatorPanel As System.Windows.Forms.Panel
    Friend WithEvents CommandActionPanel As System.Windows.Forms.Panel
    Public WithEvents SaveAndCloseButton As System.Windows.Forms.Button
    Public WithEvents CancelButton As System.Windows.Forms.Button
    Public WithEvents SaveButton As System.Windows.Forms.Button
    Friend WithEvents AccumulatorsPanel As System.Windows.Forms.Panel
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Public WithEvents FamilyAccumulatorsDataGrid As DataGridCustom
    Public WithEvents PersonalAccumulatorDataGrid As DataGridCustom
    Public WithEvents FamilyAccumulatorsCurrentLabel As System.Windows.Forms.Label
    Public WithEvents PersonalAccumulatorCurrentLabel As System.Windows.Forms.Label
    Public WithEvents FamilyAccumulatorsPriorDataGrid As DataGridCustom
    Public WithEvents PersonalAccumulatorPriorDataGrid As DataGridCustom
    Public WithEvents FamilyAccumulatorsPriorLabel As System.Windows.Forms.Label
    Public WithEvents PersonalAccumulatorPriorLabel As System.Windows.Forms.Label
    Public WithEvents AccumulatorPatientName As System.Windows.Forms.Label
    Public WithEvents AccumulatorListSummary As System.Windows.Forms.ComboBox
    Public WithEvents LineDetailsGroupBox As System.Windows.Forms.GroupBox
    Public WithEvents LineDetailsDataGrid As DataGridCustom
    Public WithEvents ClaimViewButton As System.Windows.Forms.Button
    Public WithEvents AccumulatorsHistoryButton As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents FamilyAccumulatorsCDPTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents FamilyAccumulatorsLIATextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents FamilyAccumulatorsCDOTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents FamilyAccumulatorsMM3TextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents FamilyAccumulatorsRNATextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents FamilyAccumulatorsRNTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents FamilyAccumulatorsODSTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents GetValueGroupBoxViewButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AccumulatorValues))
        Me.GetValueGroupBox = New System.Windows.Forms.GroupBox()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.DateStartOrEnd = New System.Windows.Forms.DateTimePicker()
        Me.DateQuantity = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.cmbDateDirection = New System.Windows.Forms.ComboBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.DateType = New System.Windows.Forms.ComboBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.GetLatest = New System.Windows.Forms.Button()
        Me.AccumulatorValueSummary = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.UseYear = New System.Windows.Forms.CheckBox()
        Me.CurrentValueYear = New System.Windows.Forms.TextBox()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.GetValueGroupBoxViewButton = New System.Windows.Forms.Button()
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
        Me.FamilyAccumulatorsODSTextBox = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.FamilyAccumulatorsRNATextBox = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.FamilyAccumulatorsRNTextBox = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.FamilyAccumulatorsCDPTextBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.FamilyAccumulatorsLIATextBox = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.FamilyAccumulatorsCDOTextBox = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.FamilyAccumulatorsMM3TextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.FamilyAccumulatorsDataGrid = New DataGridCustom()
        Me.PersonalAccumulatorDataGrid = New DataGridCustom()
        Me.FamilyAccumulatorsCurrentLabel = New System.Windows.Forms.Label()
        Me.PersonalAccumulatorCurrentLabel = New System.Windows.Forms.Label()
        Me.FamilyAccumulatorsPriorDataGrid = New DataGridCustom()
        Me.PersonalAccumulatorPriorDataGrid = New DataGridCustom()
        Me.FamilyAccumulatorsPriorLabel = New System.Windows.Forms.Label()
        Me.PersonalAccumulatorPriorLabel = New System.Windows.Forms.Label()
        Me.AccumulatorPatientName = New System.Windows.Forms.Label()
        Me.AccumulatorListSummary = New System.Windows.Forms.ComboBox()
        Me.LineDetailsGroupBox = New System.Windows.Forms.GroupBox()
        Me.LineDetailsDataGrid = New DataGridCustom()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GetValueGroupBox.SuspendLayout()
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
        CType(Me.LineDetailsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GetValueGroupBox
        '
        Me.GetValueGroupBox.Controls.Add(Me.Label28)
        Me.GetValueGroupBox.Controls.Add(Me.DateTimePicker1)
        Me.GetValueGroupBox.Controls.Add(Me.GroupBox6)
        Me.GetValueGroupBox.Controls.Add(Me.Label15)
        Me.GetValueGroupBox.Controls.Add(Me.DateStartOrEnd)
        Me.GetValueGroupBox.Controls.Add(Me.DateQuantity)
        Me.GetValueGroupBox.Controls.Add(Me.Label16)
        Me.GetValueGroupBox.Controls.Add(Me.cmbDateDirection)
        Me.GetValueGroupBox.Controls.Add(Me.Label17)
        Me.GetValueGroupBox.Controls.Add(Me.DateType)
        Me.GetValueGroupBox.Controls.Add(Me.Label18)
        Me.GetValueGroupBox.Controls.Add(Me.GetLatest)
        Me.GetValueGroupBox.Controls.Add(Me.AccumulatorValueSummary)
        Me.GetValueGroupBox.Controls.Add(Me.Label19)
        Me.GetValueGroupBox.Controls.Add(Me.UseYear)
        Me.GetValueGroupBox.Controls.Add(Me.CurrentValueYear)
        Me.GetValueGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GetValueGroupBox.Location = New System.Drawing.Point(458, 445)
        Me.GetValueGroupBox.Name = "GetValueGroupBox"
        Me.GetValueGroupBox.Size = New System.Drawing.Size(245, 20)
        Me.GetValueGroupBox.TabIndex = 15
        Me.GetValueGroupBox.TabStop = False
        Me.GetValueGroupBox.Text = "Get Value"
        Me.GetValueGroupBox.Visible = False
        '
        'Label28
        '
        Me.Label28.Location = New System.Drawing.Point(16, 210)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(88, 16)
        Me.Label28.TabIndex = 19
        Me.Label28.Text = "Date Start/End"
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Enabled = False
        Me.DateTimePicker1.Location = New System.Drawing.Point(120, 210)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(197, 20)
        Me.DateTimePicker1.TabIndex = 18
        '
        'GroupBox6
        '
        Me.GroupBox6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GroupBox6.BackColor = System.Drawing.Color.Red
        Me.GroupBox6.Location = New System.Drawing.Point(8, 243)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(229, 8)
        Me.GroupBox6.TabIndex = 17
        Me.GroupBox6.TabStop = False
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(16, 186)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(100, 23)
        Me.Label15.TabIndex = 16
        Me.Label15.Text = "Date Start/End"
        '
        'DateStartOrEnd
        '
        Me.DateStartOrEnd.Location = New System.Drawing.Point(120, 186)
        Me.DateStartOrEnd.Name = "DateStartOrEnd"
        Me.DateStartOrEnd.Size = New System.Drawing.Size(196, 20)
        Me.DateStartOrEnd.TabIndex = 15
        '
        'DateQuantity
        '
        Me.DateQuantity.Location = New System.Drawing.Point(112, 146)
        Me.DateQuantity.Name = "DateQuantity"
        Me.DateQuantity.Size = New System.Drawing.Size(96, 20)
        Me.DateQuantity.TabIndex = 14
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(16, 146)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(96, 24)
        Me.Label16.TabIndex = 13
        Me.Label16.Text = "Date Quantity"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateDirection
        '
        Me.cmbDateDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDateDirection.Items.AddRange(New Object() {"Future", "Past", "Current"})
        Me.cmbDateDirection.Location = New System.Drawing.Point(112, 89)
        Me.cmbDateDirection.Name = "DateDirection"
        Me.cmbDateDirection.Size = New System.Drawing.Size(98, 21)
        Me.cmbDateDirection.TabIndex = 12
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(24, 90)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(80, 24)
        Me.Label17.TabIndex = 11
        Me.Label17.Text = "Date Direction"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateType
        '
        Me.DateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DateType.Items.AddRange(New Object() {"Lifetime", "Rollover", "Annual", "Quarters", "Months", "Weeks", "Days"})
        Me.DateType.Location = New System.Drawing.Point(113, 58)
        Me.DateType.Name = "DateType"
        Me.DateType.Size = New System.Drawing.Size(96, 21)
        Me.DateType.TabIndex = 10
        '
        'Label18
        '
        Me.Label18.Location = New System.Drawing.Point(22, 55)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(72, 24)
        Me.Label18.TabIndex = 9
        Me.Label18.Text = "Date Type"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GetLatest
        '
        Me.GetLatest.Location = New System.Drawing.Point(24, 265)
        Me.GetLatest.Name = "GetLatest"
        Me.GetLatest.Size = New System.Drawing.Size(75, 23)
        Me.GetLatest.TabIndex = 8
        Me.GetLatest.Text = "Get Value"
        '
        'AccumulatorValueSummary
        '
        Me.AccumulatorValueSummary.Location = New System.Drawing.Point(120, 299)
        Me.AccumulatorValueSummary.Name = "AccumulatorValueSummary"
        Me.AccumulatorValueSummary.Size = New System.Drawing.Size(192, 20)
        Me.AccumulatorValueSummary.TabIndex = 6
        '
        'Label19
        '
        Me.Label19.Location = New System.Drawing.Point(16, 300)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(100, 23)
        Me.Label19.TabIndex = 7
        Me.Label19.Text = "Accumulator Value"
        '
        'UseYear
        '
        Me.UseYear.Location = New System.Drawing.Point(32, 114)
        Me.UseYear.Name = "UseYear"
        Me.UseYear.Size = New System.Drawing.Size(72, 24)
        Me.UseYear.TabIndex = 11
        Me.UseYear.Text = "Use Year"
        Me.UseYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'CurrentValueYear
        '
        Me.CurrentValueYear.Enabled = False
        Me.CurrentValueYear.Location = New System.Drawing.Point(112, 115)
        Me.CurrentValueYear.Name = "CurrentValueYear"
        Me.CurrentValueYear.Size = New System.Drawing.Size(72, 20)
        Me.CurrentValueYear.TabIndex = 12
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.SystemColors.Control
        Me.ImageList1.Images.SetKeyName(0, "")
        Me.ImageList1.Images.SetKeyName(1, "")
        '
        'GetValueGroupBoxViewButton
        '
        Me.GetValueGroupBoxViewButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.GetValueGroupBoxViewButton.ImageIndex = 0
        Me.GetValueGroupBoxViewButton.ImageList = Me.ImageList1
        Me.GetValueGroupBoxViewButton.Location = New System.Drawing.Point(6, 428)
        Me.GetValueGroupBoxViewButton.Name = "GetValueGroupBoxViewButton"
        Me.GetValueGroupBoxViewButton.Size = New System.Drawing.Size(17, 10)
        Me.GetValueGroupBoxViewButton.TabIndex = 56
        Me.GetValueGroupBoxViewButton.Visible = False
        '
        'ErrorLabel
        '
        Me.ErrorLabel.BackColor = System.Drawing.Color.Red
        Me.ErrorLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.ErrorLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ErrorLabel.ForeColor = System.Drawing.Color.Red
        Me.ErrorLabel.Location = New System.Drawing.Point(0, 0)
        Me.ErrorLabel.Name = "ErrorLabel"
        Me.ErrorLabel.Size = New System.Drawing.Size(748, 16)
        Me.ErrorLabel.TabIndex = 68
        Me.ErrorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ErrorLabel.Visible = False
        '
        'AccumulatorPanel
        '
        Me.AccumulatorPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AccumulatorPanel.Controls.Add(Me.CommandActionPanel)
        Me.AccumulatorPanel.Controls.Add(Me.AccumulatorsPanel)
        Me.AccumulatorPanel.Location = New System.Drawing.Point(3, 4)
        Me.AccumulatorPanel.Name = "AccumulatorPanel"
        Me.AccumulatorPanel.Size = New System.Drawing.Size(427, 354)
        Me.AccumulatorPanel.TabIndex = 67
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
        Me.CommandActionPanel.Location = New System.Drawing.Point(0, 328)
        Me.CommandActionPanel.Name = "CommandActionPanel"
        Me.CommandActionPanel.Size = New System.Drawing.Size(427, 26)
        Me.CommandActionPanel.TabIndex = 68
        '
        'ClaimViewButton
        '
        Me.ClaimViewButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClaimViewButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ClaimViewButton.Location = New System.Drawing.Point(347, 2)
        Me.ClaimViewButton.Name = "ClaimViewButton"
        Me.ClaimViewButton.Size = New System.Drawing.Size(75, 23)
        Me.ClaimViewButton.TabIndex = 76
        Me.ClaimViewButton.Text = "Claim View"
        Me.ToolTip1.SetToolTip(Me.ClaimViewButton, "Changes displayed dates to represent Claim Current/Prior Years")
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
        Me.AccumulatorsPanel.Location = New System.Drawing.Point(-4, -2)
        Me.AccumulatorsPanel.Name = "AccumulatorsPanel"
        Me.AccumulatorsPanel.Size = New System.Drawing.Size(446, 331)
        Me.AccumulatorsPanel.TabIndex = 67
        '
        'GroupBox1
        '
        Me.GroupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GroupBox1.Controls.Add(Me.FamilyAccumulatorsODSTextBox)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.FamilyAccumulatorsRNATextBox)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.FamilyAccumulatorsRNTextBox)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.FamilyAccumulatorsCDPTextBox)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.FamilyAccumulatorsLIATextBox)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.FamilyAccumulatorsCDOTextBox)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.FamilyAccumulatorsMM3TextBox)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 15)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(424, 57)
        Me.GroupBox1.TabIndex = 67
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "LifeTime Accumulators: "
        '
        'FamilyAccumulatorsODSTextBox
        '
        Me.FamilyAccumulatorsODSTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyAccumulatorsODSTextBox.Location = New System.Drawing.Point(348, 13)
        Me.FamilyAccumulatorsODSTextBox.Name = "FamilyAccumulatorsODSTextBox"
        Me.FamilyAccumulatorsODSTextBox.ReadOnly = True
        Me.FamilyAccumulatorsODSTextBox.Size = New System.Drawing.Size(69, 20)
        Me.FamilyAccumulatorsODSTextBox.TabIndex = 84
        '
        'Label7
        '
        Me.Label7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.Location = New System.Drawing.Point(315, 17)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(30, 13)
        Me.Label7.TabIndex = 83
        Me.Label7.Text = "ODS"
        '
        'FamilyAccumulatorsRNATextBox
        '
        Me.FamilyAccumulatorsRNATextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyAccumulatorsRNATextBox.Location = New System.Drawing.Point(243, 33)
        Me.FamilyAccumulatorsRNATextBox.Name = "FamilyAccumulatorsRNATextBox"
        Me.FamilyAccumulatorsRNATextBox.ReadOnly = True
        Me.FamilyAccumulatorsRNATextBox.Size = New System.Drawing.Size(69, 20)
        Me.FamilyAccumulatorsRNATextBox.TabIndex = 82
        '
        'Label5
        '
        Me.Label5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.Location = New System.Drawing.Point(212, 36)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(30, 13)
        Me.Label5.TabIndex = 81
        Me.Label5.Text = "RNA"
        '
        'FamilyAccumulatorsRNTextBox
        '
        Me.FamilyAccumulatorsRNTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyAccumulatorsRNTextBox.Location = New System.Drawing.Point(243, 13)
        Me.FamilyAccumulatorsRNTextBox.Name = "FamilyAccumulatorsRNTextBox"
        Me.FamilyAccumulatorsRNTextBox.ReadOnly = True
        Me.FamilyAccumulatorsRNTextBox.Size = New System.Drawing.Size(69, 20)
        Me.FamilyAccumulatorsRNTextBox.TabIndex = 80
        '
        'Label6
        '
        Me.Label6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.Location = New System.Drawing.Point(212, 17)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(30, 13)
        Me.Label6.TabIndex = 79
        Me.Label6.Text = "RN"
        '
        'FamilyAccumulatorsCDPTextBox
        '
        Me.FamilyAccumulatorsCDPTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyAccumulatorsCDPTextBox.Location = New System.Drawing.Point(140, 33)
        Me.FamilyAccumulatorsCDPTextBox.Name = "FamilyAccumulatorsCDPTextBox"
        Me.FamilyAccumulatorsCDPTextBox.ReadOnly = True
        Me.FamilyAccumulatorsCDPTextBox.Size = New System.Drawing.Size(69, 20)
        Me.FamilyAccumulatorsCDPTextBox.TabIndex = 78
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.Location = New System.Drawing.Point(110, 36)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(30, 13)
        Me.Label4.TabIndex = 77
        Me.Label4.Text = "CDP"
        '
        'FamilyAccumulatorsLIATextBox
        '
        Me.FamilyAccumulatorsLIATextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyAccumulatorsLIATextBox.Location = New System.Drawing.Point(37, 33)
        Me.FamilyAccumulatorsLIATextBox.Name = "FamilyAccumulatorsLIATextBox"
        Me.FamilyAccumulatorsLIATextBox.ReadOnly = True
        Me.FamilyAccumulatorsLIATextBox.Size = New System.Drawing.Size(69, 20)
        Me.FamilyAccumulatorsLIATextBox.TabIndex = 76
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.Location = New System.Drawing.Point(3, 36)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(30, 13)
        Me.Label3.TabIndex = 75
        Me.Label3.Text = "LIA"
        '
        'FamilyAccumulatorsCDOTextBox
        '
        Me.FamilyAccumulatorsCDOTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyAccumulatorsCDOTextBox.Location = New System.Drawing.Point(140, 13)
        Me.FamilyAccumulatorsCDOTextBox.Name = "FamilyAccumulatorsCDOTextBox"
        Me.FamilyAccumulatorsCDOTextBox.ReadOnly = True
        Me.FamilyAccumulatorsCDOTextBox.Size = New System.Drawing.Size(69, 20)
        Me.FamilyAccumulatorsCDOTextBox.TabIndex = 74
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.Location = New System.Drawing.Point(110, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(30, 13)
        Me.Label2.TabIndex = 73
        Me.Label2.Text = "CDO"
        '
        'FamilyAccumulatorsMM3TextBox
        '
        Me.FamilyAccumulatorsMM3TextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyAccumulatorsMM3TextBox.Location = New System.Drawing.Point(37, 13)
        Me.FamilyAccumulatorsMM3TextBox.Name = "FamilyAccumulatorsMM3TextBox"
        Me.FamilyAccumulatorsMM3TextBox.ReadOnly = True
        Me.FamilyAccumulatorsMM3TextBox.Size = New System.Drawing.Size(69, 20)
        Me.FamilyAccumulatorsMM3TextBox.TabIndex = 72
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(3, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(30, 13)
        Me.Label1.TabIndex = 71
        Me.Label1.Text = "MM3"
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
        Me.SplitContainer1.Size = New System.Drawing.Size(408, 249)
        Me.SplitContainer1.SplitterDistance = 117
        Me.SplitContainer1.TabIndex = 66
        '
        'FamilyAccumulatorsDataGrid
        '
        Me.FamilyAccumulatorsDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.FamilyAccumulatorsDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.FamilyAccumulatorsDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.FamilyAccumulatorsDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.FamilyAccumulatorsDataGrid.ADGroupsThatCanFind = ""
        Me.FamilyAccumulatorsDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.FamilyAccumulatorsDataGrid.ADGroupsThatCanMultiSort = ""
        Me.FamilyAccumulatorsDataGrid.AllowAutoSize = False
        Me.FamilyAccumulatorsDataGrid.AllowColumnReorder = False
        Me.FamilyAccumulatorsDataGrid.AllowCopy = False
        Me.FamilyAccumulatorsDataGrid.AllowCustomize = False
        Me.FamilyAccumulatorsDataGrid.AllowDelete = False
        Me.FamilyAccumulatorsDataGrid.AllowDragDrop = False
        Me.FamilyAccumulatorsDataGrid.AllowEdit = False
        Me.FamilyAccumulatorsDataGrid.AllowExport = True
        Me.FamilyAccumulatorsDataGrid.AllowFilter = False
        Me.FamilyAccumulatorsDataGrid.AllowFind = False
        Me.FamilyAccumulatorsDataGrid.AllowGoTo = False
        Me.FamilyAccumulatorsDataGrid.AllowMultiSelect = False
        Me.FamilyAccumulatorsDataGrid.AllowMultiSort = False
        Me.FamilyAccumulatorsDataGrid.AllowNavigation = False
        Me.FamilyAccumulatorsDataGrid.AllowNew = False
        Me.FamilyAccumulatorsDataGrid.AllowPrint = True
        Me.FamilyAccumulatorsDataGrid.AllowRefresh = False
        Me.FamilyAccumulatorsDataGrid.AlternatingBackColor = System.Drawing.Color.LightGray
        Me.FamilyAccumulatorsDataGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FamilyAccumulatorsDataGrid.AppKey = "UFCW\Claims\"
        Me.FamilyAccumulatorsDataGrid.BackColor = System.Drawing.Color.DarkGray
        Me.FamilyAccumulatorsDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.FamilyAccumulatorsDataGrid.CaptionBackColor = System.Drawing.Color.White
        Me.FamilyAccumulatorsDataGrid.CaptionFont = New System.Drawing.Font("Verdana", 10.0!)
        Me.FamilyAccumulatorsDataGrid.CaptionForeColor = System.Drawing.Color.Navy
        Me.FamilyAccumulatorsDataGrid.CaptionVisible = False
        Me.FamilyAccumulatorsDataGrid.ColumnHeaderLabel = Nothing
        Me.FamilyAccumulatorsDataGrid.ColumnRePositioning = False
        Me.FamilyAccumulatorsDataGrid.ColumnResizing = False
        Me.FamilyAccumulatorsDataGrid.ConfirmDelete = True
        Me.FamilyAccumulatorsDataGrid.CopySelectedOnly = False
        Me.FamilyAccumulatorsDataGrid.DataMember = ""
        Me.FamilyAccumulatorsDataGrid.DragColumn = 0
        Me.FamilyAccumulatorsDataGrid.ExportSelectedOnly = False
        Me.FamilyAccumulatorsDataGrid.ForeColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsDataGrid.GridLineColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.FamilyAccumulatorsDataGrid.HeaderBackColor = System.Drawing.Color.Silver
        Me.FamilyAccumulatorsDataGrid.HeaderForeColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsDataGrid.HighlightedRow = Nothing
        Me.FamilyAccumulatorsDataGrid.IsMouseDown = False
        Me.FamilyAccumulatorsDataGrid.LastGoToLine = ""
        Me.FamilyAccumulatorsDataGrid.LinkColor = System.Drawing.Color.Navy
        Me.FamilyAccumulatorsDataGrid.Location = New System.Drawing.Point(205, 18)
        Me.FamilyAccumulatorsDataGrid.MultiSort = False
        Me.FamilyAccumulatorsDataGrid.Name = "FamilyAccumulatorsDataGrid"
        Me.FamilyAccumulatorsDataGrid.OldSelectedRow = Nothing
        Me.FamilyAccumulatorsDataGrid.ParentRowsBackColor = System.Drawing.Color.White
        Me.FamilyAccumulatorsDataGrid.ParentRowsForeColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsDataGrid.ReadOnly = True
        Me.FamilyAccumulatorsDataGrid.RowHeadersVisible = False
        Me.FamilyAccumulatorsDataGrid.SelectionBackColor = System.Drawing.Color.Navy
        Me.FamilyAccumulatorsDataGrid.SelectionForeColor = System.Drawing.Color.White
        Me.FamilyAccumulatorsDataGrid.SetRowOnRightClick = True
        Me.FamilyAccumulatorsDataGrid.ShiftPressed = False
        Me.FamilyAccumulatorsDataGrid.SingleClickBooleanColumns = True
        Me.FamilyAccumulatorsDataGrid.Size = New System.Drawing.Size(201, 96)
        Me.FamilyAccumulatorsDataGrid.StyleName = ""
        Me.FamilyAccumulatorsDataGrid.SubKey = ""
        Me.FamilyAccumulatorsDataGrid.SuppressTriangle = False
        Me.FamilyAccumulatorsDataGrid.TabIndex = 67
        '
        'PersonalAccumulatorDataGrid
        '
        Me.PersonalAccumulatorDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.PersonalAccumulatorDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.PersonalAccumulatorDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PersonalAccumulatorDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PersonalAccumulatorDataGrid.ADGroupsThatCanFind = ""
        Me.PersonalAccumulatorDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PersonalAccumulatorDataGrid.ADGroupsThatCanMultiSort = ""
        Me.PersonalAccumulatorDataGrid.AllowAutoSize = False
        Me.PersonalAccumulatorDataGrid.AllowColumnReorder = False
        Me.PersonalAccumulatorDataGrid.AllowCopy = False
        Me.PersonalAccumulatorDataGrid.AllowCustomize = False
        Me.PersonalAccumulatorDataGrid.AllowDelete = False
        Me.PersonalAccumulatorDataGrid.AllowDragDrop = False
        Me.PersonalAccumulatorDataGrid.AllowEdit = False
        Me.PersonalAccumulatorDataGrid.AllowExport = True
        Me.PersonalAccumulatorDataGrid.AllowFilter = False
        Me.PersonalAccumulatorDataGrid.AllowFind = False
        Me.PersonalAccumulatorDataGrid.AllowGoTo = False
        Me.PersonalAccumulatorDataGrid.AllowMultiSelect = False
        Me.PersonalAccumulatorDataGrid.AllowMultiSort = False
        Me.PersonalAccumulatorDataGrid.AllowNavigation = False
        Me.PersonalAccumulatorDataGrid.AllowNew = False
        Me.PersonalAccumulatorDataGrid.AllowPrint = True
        Me.PersonalAccumulatorDataGrid.AllowRefresh = False
        Me.PersonalAccumulatorDataGrid.AlternatingBackColor = System.Drawing.Color.LightGray
        Me.PersonalAccumulatorDataGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PersonalAccumulatorDataGrid.AppKey = "UFCW\Claims\"
        Me.PersonalAccumulatorDataGrid.BackColor = System.Drawing.Color.DarkGray
        Me.PersonalAccumulatorDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.PersonalAccumulatorDataGrid.CaptionBackColor = System.Drawing.Color.White
        Me.PersonalAccumulatorDataGrid.CaptionFont = New System.Drawing.Font("Verdana", 10.0!)
        Me.PersonalAccumulatorDataGrid.CaptionForeColor = System.Drawing.Color.Navy
        Me.PersonalAccumulatorDataGrid.CaptionVisible = False
        Me.PersonalAccumulatorDataGrid.ColumnHeaderLabel = Nothing
        Me.PersonalAccumulatorDataGrid.ColumnRePositioning = False
        Me.PersonalAccumulatorDataGrid.ColumnResizing = False
        Me.PersonalAccumulatorDataGrid.ConfirmDelete = True
        Me.PersonalAccumulatorDataGrid.CopySelectedOnly = False
        Me.PersonalAccumulatorDataGrid.DataMember = ""
        Me.PersonalAccumulatorDataGrid.DragColumn = 0
        Me.PersonalAccumulatorDataGrid.ExportSelectedOnly = False
        Me.PersonalAccumulatorDataGrid.ForeColor = System.Drawing.Color.Black
        Me.PersonalAccumulatorDataGrid.GridLineColor = System.Drawing.Color.Black
        Me.PersonalAccumulatorDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.PersonalAccumulatorDataGrid.HeaderBackColor = System.Drawing.Color.Silver
        Me.PersonalAccumulatorDataGrid.HeaderForeColor = System.Drawing.Color.Black
        Me.PersonalAccumulatorDataGrid.HighlightedRow = Nothing
        Me.PersonalAccumulatorDataGrid.IsMouseDown = False
        Me.PersonalAccumulatorDataGrid.LastGoToLine = ""
        Me.PersonalAccumulatorDataGrid.LinkColor = System.Drawing.Color.Navy
        Me.PersonalAccumulatorDataGrid.Location = New System.Drawing.Point(2, 18)
        Me.PersonalAccumulatorDataGrid.MultiSort = False
        Me.PersonalAccumulatorDataGrid.Name = "PersonalAccumulatorDataGrid"
        Me.PersonalAccumulatorDataGrid.OldSelectedRow = Nothing
        Me.PersonalAccumulatorDataGrid.ParentRowsBackColor = System.Drawing.Color.White
        Me.PersonalAccumulatorDataGrid.ParentRowsForeColor = System.Drawing.Color.Black
        Me.PersonalAccumulatorDataGrid.ReadOnly = True
        Me.PersonalAccumulatorDataGrid.RowHeadersVisible = False
        Me.PersonalAccumulatorDataGrid.SelectionBackColor = System.Drawing.Color.Navy
        Me.PersonalAccumulatorDataGrid.SelectionForeColor = System.Drawing.Color.White
        Me.PersonalAccumulatorDataGrid.SetRowOnRightClick = True
        Me.PersonalAccumulatorDataGrid.ShiftPressed = False
        Me.PersonalAccumulatorDataGrid.SingleClickBooleanColumns = True
        Me.PersonalAccumulatorDataGrid.Size = New System.Drawing.Size(201, 96)
        Me.PersonalAccumulatorDataGrid.StyleName = ""
        Me.PersonalAccumulatorDataGrid.SubKey = ""
        Me.PersonalAccumulatorDataGrid.SuppressTriangle = False
        Me.PersonalAccumulatorDataGrid.TabIndex = 66
        '
        'FamilyAccumulatorsCurrentLabel
        '
        Me.FamilyAccumulatorsCurrentLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.FamilyAccumulatorsCurrentLabel.Location = New System.Drawing.Point(211, 2)
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
        Me.FamilyAccumulatorsPriorDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.FamilyAccumulatorsPriorDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.FamilyAccumulatorsPriorDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.FamilyAccumulatorsPriorDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.FamilyAccumulatorsPriorDataGrid.ADGroupsThatCanFind = ""
        Me.FamilyAccumulatorsPriorDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.FamilyAccumulatorsPriorDataGrid.ADGroupsThatCanMultiSort = ""
        Me.FamilyAccumulatorsPriorDataGrid.AllowAutoSize = False
        Me.FamilyAccumulatorsPriorDataGrid.AllowColumnReorder = False
        Me.FamilyAccumulatorsPriorDataGrid.AllowCopy = False
        Me.FamilyAccumulatorsPriorDataGrid.AllowCustomize = False
        Me.FamilyAccumulatorsPriorDataGrid.AllowDelete = False
        Me.FamilyAccumulatorsPriorDataGrid.AllowDragDrop = False
        Me.FamilyAccumulatorsPriorDataGrid.AllowEdit = False
        Me.FamilyAccumulatorsPriorDataGrid.AllowExport = True
        Me.FamilyAccumulatorsPriorDataGrid.AllowFilter = False
        Me.FamilyAccumulatorsPriorDataGrid.AllowFind = False
        Me.FamilyAccumulatorsPriorDataGrid.AllowGoTo = False
        Me.FamilyAccumulatorsPriorDataGrid.AllowMultiSelect = False
        Me.FamilyAccumulatorsPriorDataGrid.AllowMultiSort = False
        Me.FamilyAccumulatorsPriorDataGrid.AllowNavigation = False
        Me.FamilyAccumulatorsPriorDataGrid.AllowNew = False
        Me.FamilyAccumulatorsPriorDataGrid.AllowPrint = True
        Me.FamilyAccumulatorsPriorDataGrid.AllowRefresh = False
        Me.FamilyAccumulatorsPriorDataGrid.AlternatingBackColor = System.Drawing.Color.LightGray
        Me.FamilyAccumulatorsPriorDataGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FamilyAccumulatorsPriorDataGrid.AppKey = "UFCW\Claims\"
        Me.FamilyAccumulatorsPriorDataGrid.BackColor = System.Drawing.Color.DarkGray
        Me.FamilyAccumulatorsPriorDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.FamilyAccumulatorsPriorDataGrid.CaptionBackColor = System.Drawing.Color.White
        Me.FamilyAccumulatorsPriorDataGrid.CaptionFont = New System.Drawing.Font("Verdana", 10.0!)
        Me.FamilyAccumulatorsPriorDataGrid.CaptionForeColor = System.Drawing.Color.Navy
        Me.FamilyAccumulatorsPriorDataGrid.CaptionVisible = False
        Me.FamilyAccumulatorsPriorDataGrid.ColumnHeaderLabel = Nothing
        Me.FamilyAccumulatorsPriorDataGrid.ColumnRePositioning = False
        Me.FamilyAccumulatorsPriorDataGrid.ColumnResizing = False
        Me.FamilyAccumulatorsPriorDataGrid.ConfirmDelete = True
        Me.FamilyAccumulatorsPriorDataGrid.CopySelectedOnly = False
        Me.FamilyAccumulatorsPriorDataGrid.DataMember = ""
        Me.FamilyAccumulatorsPriorDataGrid.DragColumn = 0
        Me.FamilyAccumulatorsPriorDataGrid.ExportSelectedOnly = False
        Me.FamilyAccumulatorsPriorDataGrid.ForeColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsPriorDataGrid.GridLineColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsPriorDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.FamilyAccumulatorsPriorDataGrid.HeaderBackColor = System.Drawing.Color.Silver
        Me.FamilyAccumulatorsPriorDataGrid.HeaderForeColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsPriorDataGrid.HighlightedRow = Nothing
        Me.FamilyAccumulatorsPriorDataGrid.IsMouseDown = False
        Me.FamilyAccumulatorsPriorDataGrid.LastGoToLine = ""
        Me.FamilyAccumulatorsPriorDataGrid.LinkColor = System.Drawing.Color.Navy
        Me.FamilyAccumulatorsPriorDataGrid.Location = New System.Drawing.Point(206, 19)
        Me.FamilyAccumulatorsPriorDataGrid.MultiSort = False
        Me.FamilyAccumulatorsPriorDataGrid.Name = "FamilyAccumulatorsPriorDataGrid"
        Me.FamilyAccumulatorsPriorDataGrid.OldSelectedRow = Nothing
        Me.FamilyAccumulatorsPriorDataGrid.ParentRowsBackColor = System.Drawing.Color.White
        Me.FamilyAccumulatorsPriorDataGrid.ParentRowsForeColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsPriorDataGrid.ReadOnly = True
        Me.FamilyAccumulatorsPriorDataGrid.RowHeadersVisible = False
        Me.FamilyAccumulatorsPriorDataGrid.SelectionBackColor = System.Drawing.Color.Navy
        Me.FamilyAccumulatorsPriorDataGrid.SelectionForeColor = System.Drawing.Color.White
        Me.FamilyAccumulatorsPriorDataGrid.SetRowOnRightClick = True
        Me.FamilyAccumulatorsPriorDataGrid.ShiftPressed = False
        Me.FamilyAccumulatorsPriorDataGrid.SingleClickBooleanColumns = True
        Me.FamilyAccumulatorsPriorDataGrid.Size = New System.Drawing.Size(201, 108)
        Me.FamilyAccumulatorsPriorDataGrid.StyleName = ""
        Me.FamilyAccumulatorsPriorDataGrid.SubKey = ""
        Me.FamilyAccumulatorsPriorDataGrid.SuppressTriangle = False
        Me.FamilyAccumulatorsPriorDataGrid.TabIndex = 55
        '
        'PersonalAccumulatorPriorDataGrid
        '
        Me.PersonalAccumulatorPriorDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.PersonalAccumulatorPriorDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.PersonalAccumulatorPriorDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PersonalAccumulatorPriorDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PersonalAccumulatorPriorDataGrid.ADGroupsThatCanFind = ""
        Me.PersonalAccumulatorPriorDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PersonalAccumulatorPriorDataGrid.ADGroupsThatCanMultiSort = ""
        Me.PersonalAccumulatorPriorDataGrid.AllowAutoSize = False
        Me.PersonalAccumulatorPriorDataGrid.AllowColumnReorder = False
        Me.PersonalAccumulatorPriorDataGrid.AllowCopy = False
        Me.PersonalAccumulatorPriorDataGrid.AllowCustomize = False
        Me.PersonalAccumulatorPriorDataGrid.AllowDelete = False
        Me.PersonalAccumulatorPriorDataGrid.AllowDragDrop = False
        Me.PersonalAccumulatorPriorDataGrid.AllowEdit = False
        Me.PersonalAccumulatorPriorDataGrid.AllowExport = True
        Me.PersonalAccumulatorPriorDataGrid.AllowFilter = False
        Me.PersonalAccumulatorPriorDataGrid.AllowFind = False
        Me.PersonalAccumulatorPriorDataGrid.AllowGoTo = False
        Me.PersonalAccumulatorPriorDataGrid.AllowMultiSelect = False
        Me.PersonalAccumulatorPriorDataGrid.AllowMultiSort = False
        Me.PersonalAccumulatorPriorDataGrid.AllowNavigation = False
        Me.PersonalAccumulatorPriorDataGrid.AllowNew = False
        Me.PersonalAccumulatorPriorDataGrid.AllowPrint = True
        Me.PersonalAccumulatorPriorDataGrid.AllowRefresh = False
        Me.PersonalAccumulatorPriorDataGrid.AlternatingBackColor = System.Drawing.Color.LightGray
        Me.PersonalAccumulatorPriorDataGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PersonalAccumulatorPriorDataGrid.AppKey = "UFCW\Claims\"
        Me.PersonalAccumulatorPriorDataGrid.BackColor = System.Drawing.Color.DarkGray
        Me.PersonalAccumulatorPriorDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.PersonalAccumulatorPriorDataGrid.CaptionBackColor = System.Drawing.Color.White
        Me.PersonalAccumulatorPriorDataGrid.CaptionFont = New System.Drawing.Font("Verdana", 10.0!)
        Me.PersonalAccumulatorPriorDataGrid.CaptionForeColor = System.Drawing.Color.Navy
        Me.PersonalAccumulatorPriorDataGrid.CaptionVisible = False
        Me.PersonalAccumulatorPriorDataGrid.ColumnHeaderLabel = Nothing
        Me.PersonalAccumulatorPriorDataGrid.ColumnRePositioning = False
        Me.PersonalAccumulatorPriorDataGrid.ColumnResizing = False
        Me.PersonalAccumulatorPriorDataGrid.ConfirmDelete = True
        Me.PersonalAccumulatorPriorDataGrid.CopySelectedOnly = False
        Me.PersonalAccumulatorPriorDataGrid.DataMember = ""
        Me.PersonalAccumulatorPriorDataGrid.DragColumn = 0
        Me.PersonalAccumulatorPriorDataGrid.ExportSelectedOnly = False
        Me.PersonalAccumulatorPriorDataGrid.ForeColor = System.Drawing.Color.Black
        Me.PersonalAccumulatorPriorDataGrid.GridLineColor = System.Drawing.Color.Black
        Me.PersonalAccumulatorPriorDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.PersonalAccumulatorPriorDataGrid.HeaderBackColor = System.Drawing.Color.Silver
        Me.PersonalAccumulatorPriorDataGrid.HeaderForeColor = System.Drawing.Color.Black
        Me.PersonalAccumulatorPriorDataGrid.HighlightedRow = Nothing
        Me.PersonalAccumulatorPriorDataGrid.IsMouseDown = False
        Me.PersonalAccumulatorPriorDataGrid.LastGoToLine = ""
        Me.PersonalAccumulatorPriorDataGrid.LinkColor = System.Drawing.Color.Navy
        Me.PersonalAccumulatorPriorDataGrid.Location = New System.Drawing.Point(3, 19)
        Me.PersonalAccumulatorPriorDataGrid.MultiSort = False
        Me.PersonalAccumulatorPriorDataGrid.Name = "PersonalAccumulatorPriorDataGrid"
        Me.PersonalAccumulatorPriorDataGrid.OldSelectedRow = Nothing
        Me.PersonalAccumulatorPriorDataGrid.ParentRowsBackColor = System.Drawing.Color.White
        Me.PersonalAccumulatorPriorDataGrid.ParentRowsForeColor = System.Drawing.Color.Black
        Me.PersonalAccumulatorPriorDataGrid.ReadOnly = True
        Me.PersonalAccumulatorPriorDataGrid.RowHeadersVisible = False
        Me.PersonalAccumulatorPriorDataGrid.SelectionBackColor = System.Drawing.Color.Navy
        Me.PersonalAccumulatorPriorDataGrid.SelectionForeColor = System.Drawing.Color.White
        Me.PersonalAccumulatorPriorDataGrid.SetRowOnRightClick = True
        Me.PersonalAccumulatorPriorDataGrid.ShiftPressed = False
        Me.PersonalAccumulatorPriorDataGrid.SingleClickBooleanColumns = True
        Me.PersonalAccumulatorPriorDataGrid.Size = New System.Drawing.Size(201, 108)
        Me.PersonalAccumulatorPriorDataGrid.StyleName = ""
        Me.PersonalAccumulatorPriorDataGrid.SubKey = ""
        Me.PersonalAccumulatorPriorDataGrid.SuppressTriangle = False
        Me.PersonalAccumulatorPriorDataGrid.TabIndex = 54
        '
        'FamilyAccumulatorsPriorLabel
        '
        Me.FamilyAccumulatorsPriorLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.FamilyAccumulatorsPriorLabel.Location = New System.Drawing.Point(212, 3)
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
        Me.AccumulatorPatientName.Size = New System.Drawing.Size(446, 18)
        Me.AccumulatorPatientName.TabIndex = 63
        Me.AccumulatorPatientName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AccumulatorListSummary
        '
        Me.AccumulatorListSummary.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AccumulatorListSummary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.AccumulatorListSummary.Location = New System.Drawing.Point(533, 335)
        Me.AccumulatorListSummary.Name = "AccumulatorListSummary"
        Me.AccumulatorListSummary.Size = New System.Drawing.Size(121, 21)
        Me.AccumulatorListSummary.TabIndex = 66
        Me.AccumulatorListSummary.Visible = False
        '
        'LineDetailsGroupBox
        '
        Me.LineDetailsGroupBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineDetailsGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.LineDetailsGroupBox.Controls.Add(Me.LineDetailsDataGrid)
        Me.LineDetailsGroupBox.Location = New System.Drawing.Point(435, 16)
        Me.LineDetailsGroupBox.Name = "LineDetailsGroupBox"
        Me.LineDetailsGroupBox.Size = New System.Drawing.Size(309, 312)
        Me.LineDetailsGroupBox.TabIndex = 65
        Me.LineDetailsGroupBox.TabStop = False
        Me.LineDetailsGroupBox.Text = "Line Details"
        '
        'LineDetailsDataGrid
        '
        Me.LineDetailsDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.LineDetailsDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.LineDetailsDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineDetailsDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineDetailsDataGrid.ADGroupsThatCanFind = ""
        Me.LineDetailsDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineDetailsDataGrid.ADGroupsThatCanMultiSort = ""
        Me.LineDetailsDataGrid.AllowAutoSize = False
        Me.LineDetailsDataGrid.AllowColumnReorder = False
        Me.LineDetailsDataGrid.AllowCopy = False
        Me.LineDetailsDataGrid.AllowCustomize = False
        Me.LineDetailsDataGrid.AllowDelete = False
        Me.LineDetailsDataGrid.AllowDragDrop = False
        Me.LineDetailsDataGrid.AllowEdit = False
        Me.LineDetailsDataGrid.AllowExport = False
        Me.LineDetailsDataGrid.AllowFilter = False
        Me.LineDetailsDataGrid.AllowFind = False
        Me.LineDetailsDataGrid.AllowGoTo = False
        Me.LineDetailsDataGrid.AllowMultiSelect = False
        Me.LineDetailsDataGrid.AllowMultiSort = False
        Me.LineDetailsDataGrid.AllowNavigation = False
        Me.LineDetailsDataGrid.AllowNew = False
        Me.LineDetailsDataGrid.AllowPrint = False
        Me.LineDetailsDataGrid.AllowRefresh = False
        Me.LineDetailsDataGrid.AllowSorting = False
        Me.LineDetailsDataGrid.AlternatingBackColor = System.Drawing.Color.LightGray
        Me.LineDetailsDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineDetailsDataGrid.AppKey = "UFCW\Claims\"
        Me.LineDetailsDataGrid.BackColor = System.Drawing.Color.DarkGray
        Me.LineDetailsDataGrid.BackgroundColor = System.Drawing.SystemColors.Control
        Me.LineDetailsDataGrid.CaptionBackColor = System.Drawing.Color.White
        Me.LineDetailsDataGrid.CaptionFont = New System.Drawing.Font("Verdana", 10.0!)
        Me.LineDetailsDataGrid.CaptionForeColor = System.Drawing.Color.Navy
        Me.LineDetailsDataGrid.CaptionVisible = False
        Me.LineDetailsDataGrid.ColumnHeaderLabel = Nothing
        Me.LineDetailsDataGrid.ColumnRePositioning = False
        Me.LineDetailsDataGrid.ColumnResizing = False
        Me.LineDetailsDataGrid.ConfirmDelete = True
        Me.LineDetailsDataGrid.CopySelectedOnly = True
        Me.LineDetailsDataGrid.DataMember = ""
        Me.LineDetailsDataGrid.DragColumn = 0
        Me.LineDetailsDataGrid.ExportSelectedOnly = True
        Me.LineDetailsDataGrid.ForeColor = System.Drawing.Color.Black
        Me.LineDetailsDataGrid.GridLineColor = System.Drawing.Color.Black
        Me.LineDetailsDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.LineDetailsDataGrid.HeaderBackColor = System.Drawing.Color.Silver
        Me.LineDetailsDataGrid.HeaderForeColor = System.Drawing.Color.Black
        Me.LineDetailsDataGrid.HighlightedRow = Nothing
        Me.LineDetailsDataGrid.IsMouseDown = False
        Me.LineDetailsDataGrid.LastGoToLine = ""
        Me.LineDetailsDataGrid.LinkColor = System.Drawing.Color.Navy
        Me.LineDetailsDataGrid.Location = New System.Drawing.Point(6, 18)
        Me.LineDetailsDataGrid.MultiSort = False
        Me.LineDetailsDataGrid.Name = "LineDetailsDataGrid"
        Me.LineDetailsDataGrid.OldSelectedRow = Nothing
        Me.LineDetailsDataGrid.ParentRowsBackColor = System.Drawing.Color.White
        Me.LineDetailsDataGrid.ParentRowsForeColor = System.Drawing.Color.Black
        Me.LineDetailsDataGrid.ReadOnly = True
        Me.LineDetailsDataGrid.RowHeadersVisible = False
        Me.LineDetailsDataGrid.SelectionBackColor = System.Drawing.Color.Navy
        Me.LineDetailsDataGrid.SelectionForeColor = System.Drawing.Color.White
        Me.LineDetailsDataGrid.SetRowOnRightClick = True
        Me.LineDetailsDataGrid.ShiftPressed = False
        Me.LineDetailsDataGrid.SingleClickBooleanColumns = True
        Me.LineDetailsDataGrid.Size = New System.Drawing.Size(297, 288)
        Me.LineDetailsDataGrid.StyleName = ""
        Me.LineDetailsDataGrid.SubKey = ""
        Me.LineDetailsDataGrid.SuppressTriangle = False
        Me.LineDetailsDataGrid.TabIndex = 44
        '
        'AccumulatorValues
        '
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.ErrorLabel)
        Me.Controls.Add(Me.AccumulatorPanel)
        Me.Controls.Add(Me.AccumulatorListSummary)
        Me.Controls.Add(Me.LineDetailsGroupBox)
        Me.Controls.Add(Me.GetValueGroupBoxViewButton)
        Me.Controls.Add(Me.GetValueGroupBox)
        Me.MinimumSize = New System.Drawing.Size(200, 150)
        Me.Name = "AccumulatorValues"
        Me.Size = New System.Drawing.Size(748, 360)
        Me.GetValueGroupBox.ResumeLayout(False)
        Me.GetValueGroupBox.PerformLayout()
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
        CType(Me.LineDetailsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Private Variables and Properties"

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private Shared ReadOnly _TraceBinding As New TraceSwitch("TraceBinding", "Trace Switch in App.Config", "0")

    Private Const _NonClaimID As Integer = -5

    Private _Loading As Boolean = True
    Private _MemberAccumulatorManager As MemberAccumulatorManager
    Private _ShowLineDetails As Boolean
    Private _ShowHistory As Boolean = False
    Private _ShowClaimView As Boolean = False
    Private _ReadOnly As Boolean = True
    Private _EditMode As Boolean = False
    Private _UserID As String = ""
    Private _FamilyID As Integer
    Private _RelationID As Short
    Private _ClaimID As Integer
    Private _FDos As Date?

    Private _HighestEntryID As Integer
    Private _SSN As Integer
    Private _PartSSN As Integer
    Private _Load As Integer
    Private _LastName As String
    Private _FirstName As String

    Private _NewCurrentRow(3) As Integer
    Private _NewCurrentCol(3) As Integer
    Private _NewCurrentText(3) As String
    Private _OldCurrentRow(3) As Integer
    Private _OldCurrentCol(3) As Integer
    Private _OldCurrent(3) As String

    Private _OKToValidate As Boolean

#End Region

#Region "Public Variables and Properties"

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets familyID")>
    Public ReadOnly Property FamilyID() As Integer
        Get
            Return _FamilyID
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets relationID")>
    Public ReadOnly Property RelationID() As Short
        Get
            Return _RelationID
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets Accumulator Manager")>
    Public Property MemberAccumulatorManager() As MemberAccumulatorManager
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	11/22/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _MemberAccumulatorManager
        End Get
        Set(ByVal value As MemberAccumulatorManager)

            If value Is Nothing Then Return

            _MemberAccumulatorManager = value

            If _Loading = False Then
                LoadData()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.DefaultValue(True), System.ComponentModel.Description("Gets or Sets if Accumulator History is displayed")>
    Public Property ShowHistory() As Boolean
        Get
            Return _ShowHistory
        End Get
        Set(ByVal value As Boolean)

            _ShowHistory = value
            Me.AccumulatorsHistoryButton.Visible = _ShowHistory

            If _Loading = False Then
                LoadData()
            End If

        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.DefaultValue(True), System.ComponentModel.Description("Gets or Sets if Accumulator allows the Claims Date Accumulators button to be used")>
    Public Property ShowClaimView() As Boolean
        Get
            Return _ShowClaimView
        End Get
        Set(ByVal value As Boolean)

            _ShowClaimView = value
            Me.ClaimViewButton.Visible = _ShowClaimView

            If _Loading = False Then
                LoadData()
            End If

        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.DefaultValue(True), System.ComponentModel.Description("Gets or Sets if Line Item details are displayed")>
    Public Property ShowLineDetails() As Boolean
        Get
            Return _ShowLineDetails
        End Get
        Set(ByVal value As Boolean)

            _ShowLineDetails = value
            Me.LineDetailsDataGrid.Visible = _ShowLineDetails
            Me.LineDetailsGroupBox.Visible = _ShowLineDetails

            If _Loading = False Then
                LoadData()
            End If

        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets UserID")>
    Public Property UserID() As String
        Get
            Return _UserID
        End Get
        Set(ByVal value As String)

            If value Is Nothing Then Return
            _UserID = value

            If _Loading = False Then
                LoadData()
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.DefaultValue(True), System.ComponentModel.Description("Prevents editing of control data when True")>
    Public Property [ReadOnly]() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)

            _ReadOnly = value

            Me.PersonalAccumulatorDataGrid.ReadOnly = _ReadOnly
            Me.PersonalAccumulatorPriorDataGrid.ReadOnly = _ReadOnly
            Me.FamilyAccumulatorsDataGrid.ReadOnly = _ReadOnly
            Me.FamilyAccumulatorsPriorDataGrid.ReadOnly = _ReadOnly
            Me.FamilyAccumulatorsMM3TextBox.ReadOnly = _ReadOnly
            Me.FamilyAccumulatorsLIATextBox.ReadOnly = _ReadOnly
            Me.FamilyAccumulatorsCDOTextBox.ReadOnly = _ReadOnly
            Me.FamilyAccumulatorsCDPTextBox.ReadOnly = _ReadOnly
            Me.FamilyAccumulatorsRNTextBox.ReadOnly = _ReadOnly
            Me.FamilyAccumulatorsRNATextBox.ReadOnly = _ReadOnly
            Me.FamilyAccumulatorsODSTextBox.ReadOnly = _ReadOnly

            If _Loading = False Then
                LoadData()
            End If

        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.DefaultValue(True), System.ComponentModel.Description("Determines if UI displays Modification elements.")>
    Public Property IsInEditMode() As Boolean
        Get
            Return _EditMode
        End Get
        Set(ByVal value As Boolean)

            _EditMode = value

            Me.SaveButton.Visible = _EditMode
            Me.SaveAndCloseButton.Visible = _EditMode
            Me.CancelButton.Visible = _EditMode

            If _Loading = False Then
                LoadData()
            End If

        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets SSN")>
    Public Property SSN() As Integer
        Get
            Return _SSN
        End Get
        Set(ByVal value As Integer)
            _SSN = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets Participant SSN")>
    Public Property PartSSN() As Integer
        Get
            Return _PartSSN
        End Get
        Set(ByVal value As Integer)
            _PartSSN = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets firstName")>
    Public Property LoadID() As Integer
        Get
            Return _Load
        End Get
        Set(ByVal value As Integer)
            _Load = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets LastName")>
    Public Property LastName() As String
        Get
            Return _LastName
        End Get
        Set(ByVal value As String)
            _LastName = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets firstName")>
    Public Property FirstName() As String
        Get
            Return _FirstName
        End Get
        Set(ByVal value As String)
            _FirstName = value
        End Set
    End Property

    <BrowsableAttribute(False)>
    Protected Shadows ReadOnly Property DesignMode() As Boolean

        Get

            ' Returns true if this control or any of its ancestors is in design mode()

            If MyBase.DesignMode Then

                Return True

            Else

                Dim TheParent As Control = Me.Parent

                While TheParent IsNot Nothing

                    Dim TheSite As ISite = TheParent.Site

                    If TheSite IsNot Nothing AndAlso TheSite.DesignMode Then

                        Return True

                    End If

                    TheParent = TheParent.Parent

                End While

                Return False

            End If

        End Get

    End Property

#End Region

#Region "Private Form and Control Events"

    Private Sub HistoryButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AccumulatorsHistoryButton.Click

        Dim AccumulatorHistoryForm As AccumulatorHistoryForm

        Try

            AccumulatorHistoryForm = New AccumulatorHistoryForm

            AccumulatorHistoryForm.AccumulatorsHistoryCtrl.SetFormInfo(_FamilyID, _RelationID)
            AccumulatorHistoryForm.ShowDialog()

        Catch ex As Exception
            Throw
        Finally
            If AccumulatorHistoryForm IsNot Nothing Then AccumulatorHistoryForm.Dispose()
            AccumulatorHistoryForm = Nothing
        End Try

    End Sub

    Private Sub ClaimViewButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClaimViewButton.Click

        Select Case ClaimViewButton.Text
            Case "Claim View"
                SetFormInfo(_FamilyID, _RelationID, _FDos, _ClaimID, True)
                ClaimViewButton.Text = "Member View"

                AccumulatorPatientName.Text &= " - CLAIM SPECIFIC"

            Case Else
                SetFormInfo(_FamilyID, _RelationID, _FDos, _ClaimID, False)
                ClaimViewButton.Text = "Claim View"

                AccumulatorPatientName.Text = AccumulatorPatientName.Text.Replace(" - CLAIM SPECIFIC", "")
        End Select

    End Sub

    Private Sub AccumulatorValues_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            _Loading = False

            If Not Me.DesignMode Then SetDetailsGridStyle()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub GetLatest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetLatest.Click

        Dim AccumID As Integer

        Try

            AccumID = CInt(AccumulatorController.GetAccumulatorID(Me.AccumulatorListSummary.Text))

            Select Case Me.DateType.Text
                Case "Quarters"
                    If Me.cmbDateDirection.Text.Trim.Length < 1 Then
                        MessageBox.Show("Please Select a Date Direction", "Accumulator Values", MessageBoxButtons.OK)
                        Return
                    ElseIf Me.DateQuantity.Text.Length < 1 OrElse Not IsNumeric(Me.DateQuantity.Text.Trim) Then
                        MessageBox.Show("Please Select a Valid Quantity", "Accumulator Values", MessageBoxButtons.OK)
                        Return
                    End If
                    Me.AccumulatorValueSummary.Text = CStr(_MemberAccumulatorManager.GetProposedValue(AccumID, Global.DateType.Quarters, CInt(Me.DateQuantity.Text), Me.DateStartOrEnd.Value, If(Me.cmbDateDirection.Text = "Future", DateDirection.Forward, DateDirection.Reverse)))
                Case "Months"
                    If Me.cmbDateDirection.Text.Trim.Length < 1 Then
                        MessageBox.Show("Please Select a Date Direction", "Accumulator Values", MessageBoxButtons.OK)
                        Return
                    ElseIf Me.DateQuantity.Text.Length < 1 OrElse Not IsNumeric(Me.DateQuantity.Text.Trim) Then
                        MessageBox.Show("Please Select a Valid Quantity", "Accumulator Values", MessageBoxButtons.OK)
                        Return
                    End If
                    Me.AccumulatorValueSummary.Text = CStr(_MemberAccumulatorManager.GetProposedValue(AccumID, Global.DateType.Months, CInt(Me.DateQuantity.Text), Me.DateStartOrEnd.Value, If(Me.cmbDateDirection.Text = "Future", DateDirection.Forward, DateDirection.Reverse)))
                Case "Weeks"
                    If Me.cmbDateDirection.Text.Trim.Length < 1 Then
                        MessageBox.Show("Please Select a Date Direction", "Accumulator Values", MessageBoxButtons.OK)
                        Return
                    ElseIf Me.DateQuantity.Text.Length < 1 OrElse Not IsNumeric(Me.DateQuantity.Text.Trim) Then
                        MessageBox.Show("Please Select a Valid Quantity", "Accumulator Values", MessageBoxButtons.OK)
                        Return
                    End If
                    Me.AccumulatorValueSummary.Text = CStr(_MemberAccumulatorManager.GetProposedValue(AccumID, Global.DateType.Weeks, CInt(Me.DateQuantity.Text), Me.DateStartOrEnd.Value, If(Me.cmbDateDirection.Text = "Future", DateDirection.Forward, DateDirection.Reverse)))
                Case "Days"
                    If Me.cmbDateDirection.Text.Trim.Length < 1 Then
                        MessageBox.Show("Please Select a Date Direction", "Accumulator Values", MessageBoxButtons.OK)
                        Return
                    ElseIf Me.DateQuantity.Text.Length < 1 OrElse Not IsNumeric(Me.DateQuantity.Text.Trim) Then
                        MessageBox.Show("Please Select a Valid Quantity", "Accumulator Values", MessageBoxButtons.OK)
                        Return
                    End If
                    Me.AccumulatorValueSummary.Text = CStr(_MemberAccumulatorManager.GetProposedValue(AccumID, Global.DateType.Days, CInt(Me.DateQuantity.Text), Me.DateStartOrEnd.Value, If(Me.cmbDateDirection.Text = "Future", DateDirection.Forward, DateDirection.Reverse)))
                Case "Lifetime"
                    Me.AccumulatorValueSummary.Text = CStr(_MemberAccumulatorManager.GetProposedLifetimeValue(AccumID))
                Case "Rollover"
                    If Me.UseYear.Checked AndAlso (CurrentValueYear.Text.Length < 1 OrElse Not IsNumeric(Me.CurrentValueYear.Text.Trim)) Then
                        MessageBox.Show("Please Select a Valid Year", "Accumulator Values", MessageBoxButtons.OK)
                        Return
                    End If

                    If UseYear.Checked Then
                        Me.AccumulatorValueSummary.Text = CStr(_MemberAccumulatorManager.GetProposedRolloverValue(AccumID, CInt(Me.CurrentValueYear.Text)))
                    Else
                        Me.AccumulatorValueSummary.Text = CStr(_MemberAccumulatorManager.GetProposedRolloverValue(AccumID, UFCWGeneral.NowDate.Year))
                    End If
                Case "Current"

                    If UseYear.Checked Then
                        Me.AccumulatorValueSummary.Text = CStr(_MemberAccumulatorManager.GetProposedYearValue(AccumID, CInt(Me.CurrentValueYear.Text)))
                    Else
                        Me.AccumulatorValueSummary.Text = CStr(_MemberAccumulatorManager.GetProposedCurrentValueForCurrentYear(AccumID))
                    End If

                Case "Annual"

                    If Me.UseYear.Checked AndAlso (CurrentValueYear.Text.Length < 1 OrElse Not IsNumeric(Me.CurrentValueYear.Text.Trim)) Then
                        MessageBox.Show("Please Select a Valid Year", "Accumulator Values", MessageBoxButtons.OK)
                        Return
                    End If

                    If UseYear.Checked Then
                        Me.AccumulatorValueSummary.Text = CStr(_MemberAccumulatorManager.GetProposedYearValue(AccumID, CInt(Me.CurrentValueYear.Text)))
                    Else
                        Me.AccumulatorValueSummary.Text = CStr(_MemberAccumulatorManager.GetProposedCurrentValueForCurrentYear(AccumID))
                    End If
                    'Case "Years"
                    '    Me.AccumulatorValueSummary.Text = manager.GetProposedValue(AccumulatorController.GetAccumulatorId(Me.AccumulatorListSummary.Text), MemberAccumulator.DateTypes.Years, Me.DateQuantity.Text, Me.DateStartOrEnd.Value, IF(Me.DateDirection.Text = "Future", MemberAccumulator.DateDirection.Forward, MemberAccumulator.DateDirection.Reverse))

            End Select

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub ClaimGroupBoxViewButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Try

            If Me.GetValueGroupBox.Height = 20 Then
                Me.GetValueGroupBox.Height = 325
                Me.Height += 305
                RaiseEvent Resize(Me, New IntEventArgs(305))
            Else
                Me.GetValueGroupBox.Height = 20
                Me.Height -= 305
                RaiseEvent Resize(Me, New IntEventArgs(-305))
            End If

            If Me.GetValueGroupBoxViewButton.ImageIndex = 0 Then
                Me.GetValueGroupBoxViewButton.ImageIndex = 1
            Else
                Me.GetValueGroupBoxViewButton.ImageIndex = 0
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub DateType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateType.SelectedIndexChanged
        Select Case DateType.Text
            Case "Lifetime", "Rollover", "Annual"
                Label17.Visible = False : Label16.Visible = False : Label15.Visible = False
                cmbDateDirection.Visible = False : DateQuantity.Visible = False : Me.DateStartOrEnd.Visible = False
                If DateType.Text = "Lifetime" Then
                    UseYear.Visible = False : CurrentValueYear.Visible = False
                Else
                    UseYear.Visible = True : CurrentValueYear.Visible = True
                End If
                Label28.Visible = False
                DateTimePicker1.Visible = False
            Case Else
                Label17.Visible = True : Label16.Visible = True : Label15.Visible = True
                cmbDateDirection.Visible = True : DateQuantity.Visible = True : Me.DateStartOrEnd.Visible = True
                UseYear.Visible = False : CurrentValueYear.Visible = False
                Label28.Visible = True
                DateTimePicker1.Visible = True
        End Select
        Me.GetLatest.Enabled = True
    End Sub

    Private Sub UseYear_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UseYear.CheckedChanged
        CurrentValueYear.Enabled = UseYear.Checked
    End Sub

    Private Sub DateDirection_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDateDirection.SelectedIndexChanged
        Try
            If cmbDateDirection.SelectedItem.ToString = "Current" Then
                DateQuantity.Text = "0"
                DateQuantity.Enabled = False
                Label15.Text = "Current Date"
                Label28.Visible = False
                DateTimePicker1.Visible = False
            Else
                Label28.Visible = True
                DateTimePicker1.Visible = True
                If cmbDateDirection.SelectedItem.ToString = "Past" Then
                    Label15.Text = "End Date"
                    Label28.Text = "Begin Date"
                Else
                    Label15.Text = "Begin Date"
                    Label28.Text = "End Date"
                End If
                'DateQuantity.Text = ""
                DateQuantity.Enabled = True
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LineDetailsDataGrid_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        Dim DG As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try

            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)

            If HTI.Type = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader Then
                Return
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        _MemberAccumulatorManager = Nothing '.Dispose()
        RaiseEvent Closing(Me, New EventArgs)
        Me.Dispose()
    End Sub

    Private Sub SaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveButton.Click

        Try

            If _MemberAccumulatorManager IsNot Nothing Then
                Save()
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub SaveAndCloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAndCloseButton.Click
        Try

            If _MemberAccumulatorManager IsNot Nothing Then
                Save()
            End If

            RaiseEvent Closing(Me, New EventArgs)

        Catch ex As Exception
            Throw
        Finally
            _MemberAccumulatorManager = Nothing
        End Try

    End Sub

    Private Sub Accumulator_ColumnChanging(ByVal sender As Object, ByVal e As System.EventArgs) Handles FamilyAccumulatorsDataGrid.CurrentCellChanged, FamilyAccumulatorsPriorDataGrid.CurrentCellChanged, PersonalAccumulatorDataGrid.CurrentCellChanged, PersonalAccumulatorPriorDataGrid.CurrentCellChanged

        Dim I As Integer
        Dim DG As DataGrid

        If _TraceBinding.TraceInfo Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & CType(sender, DataGrid).Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceBinding.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""))

        Try

            DG = CType(sender, System.Windows.Forms.DataGrid)

            Select Case DG.Name
                Case "FamilyAccumulatorsDataGrid"
                    I = 0
                Case "FamilyAccumulatorsPriorDataGrid"
                    I = 1
                Case "PersonalAccumulatorDataGrid"
                    I = 2
                Case "PersonalAccumulatorPriorDataGrid"
                    I = 3
            End Select

            _NewCurrentRow(I) = DG.CurrentCell.RowNumber
            _NewCurrentCol(I) = DG.CurrentCell.ColumnNumber

            'determine if last row was an added row that was not completed
            If DG.DataSource IsNot Nothing AndAlso _OldCurrentRow(I) <= DG.BindingContext(DG.DataSource, DG.DataMember).Count - 1 Then
                _NewCurrentText(I) = DG(_OldCurrentRow(I), _OldCurrentCol(I)).ToString() 'look at prior column and get text

                If Not [ReadOnly] AndAlso _OKToValidate Then
                    Select Case _NewCurrentText(I).ToString.ToUpper.Trim
                        Case "MM3", "LIA", "CDP", "CDO", "RN", "RNA", "ODS"

                            MessageBox.Show(_NewCurrentText(I).ToString.ToUpper.Trim & " accumulator data can only be entered in text box labeled " & _NewCurrentText(I).ToString.ToUpper.Trim, "Invalid use of " & _NewCurrentText(I).ToString.ToUpper.Trim, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                            _OKToValidate = False
                            DG.CurrentCell = New DataGridCell(_OldCurrentRow(I), _OldCurrentCol(I))
                            _OKToValidate = True
                    End Select

                End If
            End If

            _OldCurrentRow(I) = _NewCurrentRow(I)
            _OldCurrentCol(I) = _NewCurrentCol(I)
            _OldCurrent(I) = _NewCurrentText(I)

        Catch ex As Exception
            If _TraceBinding.TraceInfo Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & CType(sender, DataGrid).Name & vbCrLf & New StackTrace(0, True).ToString & vbCrLf & ex.ToString & vbCrLf)
            Throw
        End Try

    End Sub

    'Private Sub DataGrid_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles FamilyAccumulatorsDataGrid.Validating, FamilyAccumulatorsPriorDataGrid.Validating, PersonalAccumulatorDataGrid.Validating, PersonalAccumulatorPriorDataGrid.Validating
    '    Dim I As Integer
    '    Dim DG As DataGrid

    '    If _TraceBinding.TraceInfo Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & " : " & CType(sender, DataGrid).Name & " : " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)) & If(_TraceBinding.TraceVerbose, vbCrLf & New StackTrace(0, True).ToString & vbCrLf, ""))

    '    Try
    '        DG = CType(sender, System.Windows.Forms.DataGrid)

    '        Select Case DG.Name
    '            Case "FamilyAccumulatorsDataGrid"
    '                I = 0
    '            Case "FamilyAccumulatorsPriorDataGrid"
    '                I = 1
    '            Case "PersonalAccumulatorDataGrid"
    '                I = 2
    '            Case "PersonalAccumulatorPriorDataGrid"
    '                I = 3
    '        End Select

    '        If Not ControlIsReadOnly AndAlso DG.DataSource IsNot Nothing AndAlso DG.BindingContext(DG.DataSource, DG.DataMember).Count > 0 Then
    '            Select Case DG(_OldCurrentRow(I), _OldCurrentCol(I)).ToString.Trim.ToUpper 'points to last cell
    '                Case "MM3", "LIA", "CDP", "CDO", "RN", "RNA", "ODS"
    '                    MessageBox.Show(DG(_OldCurrentRow(I), _OldCurrentCol(I)).ToString.Trim.ToUpper & " accumulator data can only be entered in text box labeled " & DG(_OldCurrentRow(I), _OldCurrentCol(I)).ToString.Trim.ToUpper, "Invalid use of " & DG(_OldCurrentRow(I), _OldCurrentCol(I)).ToString.Trim.ToUpper, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '                    e.Cancel = True
    '            End Select

    '        End If

    '    Catch ex As Exception
    '        If _TraceBinding.TraceInfo Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New stackframe(True).getmethod.tostring & vbTab & " : " & CType(sender, DataGrid).Name & vbCrLf & New Stacktrace(0, True).ToString & vbCrLf & ex.ToString & vbCrLf)
    '        Throw
    '    Finally
    '    End Try

    'End Sub

#End Region

#Region "Public Form and Control Events"

    Public Event Closing As EventHandler
    Public Shadows Event Resize As EventHandler(Of EventArgs)

    Private Sub LoadData()

        Dim DT As DataTable

        Try

            If AccumulatorListSummary Is Nothing Then
                DT = AccumulatorController.GetAccumulators()
                AccumulatorListSummary.Items.Clear()

                For I As Integer = 0 To DT.Rows.Count - 1
                    AccumulatorListSummary.Items.Add(DT.Rows(I)("ACCUM_NAME"))
                Next

                SetGridsStyle()

                Me.GetLatest.Enabled = False
                _Loading = True
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Saves Accumulator changes")>
    Public Sub Save()
        Dim DT As DataTable
        Dim ApplyDate As Date
        Dim Amt As Decimal
        Dim OriginalAmt As Decimal
        Dim NewAmt As Decimal

        Try
            If _HighestEntryID <> _MemberAccumulatorManager.GetHighestEntryIdForFamily Then
                MessageBox.Show(String.Format("Changes have occurred since you retrieved the Accumulators.{0}The " &
                "Accumulators will now be refreshed to their current values.{0}Please make changes as necessary.", Environment.NewLine), "Changes Occurred", MessageBoxButtons.OK, MessageBoxIcon.Information)
                SetFormInfo(_FamilyID, _RelationID, New Date(UFCWGeneral.NowDate.Year, 12, 31), -5)
                Return
            End If

            If PersonalAccumulatorDataGrid.DataSource IsNot Nothing Then
                DT = CType(PersonalAccumulatorDataGrid.DataSource, DataTable)
                For I As Integer = 0 To DT.Rows.Count - 1
                    If DT.Rows(I).RowState <> DataRowState.Unchanged Then
                        NewAmt = CDec(DT.Rows(I)("ACCUM_VALUE", DataRowVersion.Current))
                        OriginalAmt = CDec(DT.Rows(I)("ACCUM_VALUE", DataRowVersion.Original))
                        Amt = NewAmt - OriginalAmt
                        ApplyDate = New Date(UFCWGeneral.NowDate.Year, 1, 1)
                        _MemberAccumulatorManager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID(CStr(DT.Rows(I)("ACCUM_NAME")))), ApplyDate, Amt, True, _UserID)
                    End If
                Next
            End If

            If PersonalAccumulatorPriorDataGrid.DataSource IsNot Nothing Then
                DT = CType(PersonalAccumulatorPriorDataGrid.DataSource, DataTable)
                For I As Integer = 0 To DT.Rows.Count - 1
                    If DT.Rows(I).RowState <> DataRowState.Unchanged Then
                        NewAmt = CDec(DT.Rows(I)("ACCUM_VALUE", DataRowVersion.Current))
                        OriginalAmt = CDec(DT.Rows(I)("ACCUM_VALUE", DataRowVersion.Original))
                        Amt = NewAmt - OriginalAmt
                        ApplyDate = New Date(UFCWGeneral.NowDate.Year - 1, 1, 1)
                        _MemberAccumulatorManager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID(CStr(DT.Rows(I)("ACCUM_NAME")))), ApplyDate, Amt, True, UserID)
                    End If
                Next
            End If

            If FamilyAccumulatorsDataGrid.DataSource IsNot Nothing Then
                DT = CType(FamilyAccumulatorsDataGrid.DataSource, DataTable)
                For I As Integer = 0 To DT.Rows.Count - 1
                    If DT.Rows(I).RowState <> DataRowState.Unchanged Then
                        NewAmt = CDec(DT.Rows(I)("ACCUM_VALUE", DataRowVersion.Current))
                        OriginalAmt = CDec(DT.Rows(I)("ACCUM_VALUE", DataRowVersion.Original))
                        Amt = NewAmt - OriginalAmt
                        ApplyDate = New Date(UFCWGeneral.NowDate.Year, 1, 1)
                        _MemberAccumulatorManager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID(CStr(DT.Rows(I)("ACCUM_NAME")))), ApplyDate, Amt, True, UserID)
                    End If
                Next
            End If

            If FamilyAccumulatorsPriorDataGrid.DataSource IsNot Nothing Then
                DT = CType(FamilyAccumulatorsPriorDataGrid.DataSource, DataTable)
                For I As Integer = 0 To DT.Rows.Count - 1
                    If DT.Rows(I).RowState <> DataRowState.Unchanged Then
                        NewAmt = CDec(DT.Rows(I)("ACCUM_VALUE", DataRowVersion.Current))
                        OriginalAmt = CDec(DT.Rows(I)("ACCUM_VALUE", DataRowVersion.Original))
                        Amt = NewAmt - OriginalAmt
                        ApplyDate = New Date(UFCWGeneral.NowDate.Year - 1, 1, 1)
                        _MemberAccumulatorManager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID(CStr(DT.Rows(I)("ACCUM_NAME")))), ApplyDate, Amt, True, UserID)
                    End If
                Next
            End If

            If _MemberAccumulatorManager IsNot Nothing Then
                If _MemberAccumulatorManager.GetProposedLifetimeValue(CInt(AccumulatorController.GetAccumulatorID("FIXAC"))) = 1 Then
                    _MemberAccumulatorManager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID("FIXAC")), UFCWGeneral.NowDate, 2, True, SystemInformation.UserName)
                End If

                If FamilyAccumulatorsMM3TextBox.Text.ToString.Trim.Length = 0 Then FamilyAccumulatorsMM3TextBox.Text = "0.00"
                If CDec(FamilyAccumulatorsMM3TextBox.Text.ToString) <> CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("MM3")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString) Then
                    NewAmt = CDec(FamilyAccumulatorsMM3TextBox.Text.ToString)
                    OriginalAmt = CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("MM3")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString)
                    Amt = NewAmt - OriginalAmt
                    ApplyDate = New Date(UFCWGeneral.NowDate.Year - 1, 1, 1)
                    _MemberAccumulatorManager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID("MM3")), ApplyDate, Amt, True, UserID)
                End If

                If FamilyAccumulatorsLIATextBox.Text.ToString.Trim.Length = 0 Then FamilyAccumulatorsLIATextBox.Text = "0.00"
                If CDec(FamilyAccumulatorsLIATextBox.Text.ToString) <> CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("LIA")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString) Then
                    NewAmt = CDec(FamilyAccumulatorsLIATextBox.Text.ToString)
                    OriginalAmt = CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("LIA")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString)
                    Amt = NewAmt - OriginalAmt
                    ApplyDate = New Date(UFCWGeneral.NowDate.Year - 1, 1, 1)
                    _MemberAccumulatorManager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID("LIA")), ApplyDate, Amt, True, UserID)
                End If

                If FamilyAccumulatorsCDOTextBox.Text.ToString.Trim.Length = 0 Then FamilyAccumulatorsCDOTextBox.Text = "0.00"
                If CDec(FamilyAccumulatorsCDOTextBox.Text.ToString) <> CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("CDO")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString) Then
                    NewAmt = CDec(FamilyAccumulatorsCDOTextBox.Text.ToString)
                    OriginalAmt = CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("CDO")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString)
                    Amt = NewAmt - OriginalAmt
                    ApplyDate = New Date(UFCWGeneral.NowDate.Year - 1, 1, 1)
                    _MemberAccumulatorManager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID("CDO")), ApplyDate, Amt, True, UserID)
                End If

                If FamilyAccumulatorsCDPTextBox.Text.ToString.Trim.Length = 0 Then FamilyAccumulatorsCDPTextBox.Text = "0.00"
                If CDec(FamilyAccumulatorsCDPTextBox.Text.ToString) <> CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("CDP")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString) Then
                    NewAmt = CDec(FamilyAccumulatorsCDPTextBox.Text.ToString)
                    OriginalAmt = CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("CDP")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString)
                    Amt = NewAmt - OriginalAmt
                    ApplyDate = New Date(UFCWGeneral.NowDate.Year - 1, 1, 1)
                    _MemberAccumulatorManager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID("CDP")), ApplyDate, Amt, True, UserID)
                End If

                If FamilyAccumulatorsRNTextBox.Text.ToString.Trim.Length = 0 Then FamilyAccumulatorsRNTextBox.Text = "0.00"
                If CDec(FamilyAccumulatorsRNTextBox.Text.ToString) <> CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("RN")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString) Then
                    NewAmt = CDec(FamilyAccumulatorsRNTextBox.Text.ToString)
                    OriginalAmt = CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("RN")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString)
                    Amt = NewAmt - OriginalAmt
                    ApplyDate = New Date(UFCWGeneral.NowDate.Year - 1, 1, 1)
                    _MemberAccumulatorManager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID("RN")), ApplyDate, Amt, True, UserID)
                End If

                If FamilyAccumulatorsRNATextBox.Text.ToString.Trim.Length = 0 Then FamilyAccumulatorsRNATextBox.Text = "0.00"
                If CDec(FamilyAccumulatorsRNATextBox.Text.ToString) <> CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("RNA")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString) Then
                    NewAmt = CDec(FamilyAccumulatorsRNATextBox.Text.ToString)
                    OriginalAmt = CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("RNA")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString)
                    Amt = NewAmt - OriginalAmt
                    ApplyDate = New Date(UFCWGeneral.NowDate.Year - 1, 1, 1)
                    _MemberAccumulatorManager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID("RNA")), ApplyDate, Amt, True, UserID)
                End If

                If FamilyAccumulatorsODSTextBox.Text.ToString.Trim.Length = 0 Then FamilyAccumulatorsODSTextBox.Text = "0.00"
                If CDec(FamilyAccumulatorsODSTextBox.Text.ToString) <> CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("ODS")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString) Then
                    NewAmt = CDec(FamilyAccumulatorsODSTextBox.Text.ToString)
                    OriginalAmt = CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("ODS")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString)
                    Amt = NewAmt - OriginalAmt
                    ApplyDate = New Date(UFCWGeneral.NowDate.Year - 1, 1, 1)
                    _MemberAccumulatorManager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID("ODS")), ApplyDate, Amt, True, UserID)
                End If

                _MemberAccumulatorManager.CommitAll()
                _MemberAccumulatorManager.RefreshAccumulatorSummariesForMember()

                DT = CType(PersonalAccumulatorDataGrid.DataSource, DataTable)
                DT.AcceptChanges()
                DT = CType(PersonalAccumulatorPriorDataGrid.DataSource, DataTable)
                DT.AcceptChanges()
                DT = CType(FamilyAccumulatorsDataGrid.DataSource, DataTable)
                DT.AcceptChanges()
                DT = CType(FamilyAccumulatorsPriorDataGrid.DataSource, DataTable)
                DT.AcceptChanges()
            End If

        Catch ex As Exception

            Try

                DT = CType(PersonalAccumulatorDataGrid.DataSource, DataTable)
                DT.RejectChanges()
                PersonalAccumulatorDataGrid.DataSource = DT
                DT = CType(PersonalAccumulatorPriorDataGrid.DataSource, DataTable)
                DT.RejectChanges()
                PersonalAccumulatorPriorDataGrid.DataSource = DT
                DT = CType(FamilyAccumulatorsDataGrid.DataSource, DataTable)
                DT.RejectChanges()
                FamilyAccumulatorsDataGrid.DataSource = DT
                DT = CType(FamilyAccumulatorsPriorDataGrid.DataSource, DataTable)
                DT.RejectChanges()
                FamilyAccumulatorsPriorDataGrid.DataSource = DT

            Catch ex2 As Exception

            End Try

            Throw

        End Try
    End Sub

    Public Function HasChanges() As Boolean

        Dim DT As DataTable

        If PersonalAccumulatorDataGrid IsNot Nothing Then
            If PersonalAccumulatorDataGrid.DataSource IsNot Nothing Then
                DT = CType(PersonalAccumulatorDataGrid.DataSource, DataTable)
                If DT IsNot Nothing Then
                    If DT.GetChanges IsNot Nothing Then
                        If DT.GetChanges.Rows IsNot Nothing Then
                            If DT.GetChanges.Rows.Count > 0 Then Return True
                        End If
                    End If
                End If
            End If
        End If

        If PersonalAccumulatorPriorDataGrid IsNot Nothing Then
            If PersonalAccumulatorPriorDataGrid.DataSource IsNot Nothing Then
                DT = CType(PersonalAccumulatorPriorDataGrid.DataSource, DataTable)
                If DT IsNot Nothing Then
                    If DT.GetChanges IsNot Nothing Then
                        If DT.GetChanges.Rows IsNot Nothing Then
                            If DT.GetChanges.Rows.Count > 0 Then Return True
                        End If
                    End If
                End If
            End If
        End If

        If FamilyAccumulatorsDataGrid IsNot Nothing Then
            If FamilyAccumulatorsDataGrid.DataSource IsNot Nothing Then
                DT = CType(FamilyAccumulatorsDataGrid.DataSource, DataTable)
                If DT IsNot Nothing Then
                    If DT.GetChanges IsNot Nothing Then
                        If DT.GetChanges.Rows IsNot Nothing Then
                            If DT.GetChanges.Rows.Count > 0 Then Return True
                        End If
                    End If
                End If
            End If
        End If

        If FamilyAccumulatorsPriorDataGrid IsNot Nothing Then
            If FamilyAccumulatorsPriorDataGrid.DataSource IsNot Nothing Then
                DT = CType(FamilyAccumulatorsPriorDataGrid.DataSource, DataTable)
                If DT IsNot Nothing Then
                    If DT.GetChanges IsNot Nothing Then
                        If DT.GetChanges.Rows IsNot Nothing Then
                            If DT.GetChanges.Rows.Count > 0 Then Return True
                        End If
                    End If
                End If
            End If
        End If

        If _MemberAccumulatorManager IsNot Nothing AndAlso CDec(If(IsNumeric(FamilyAccumulatorsMM3TextBox.Text.ToString), CDec(FamilyAccumulatorsMM3TextBox.Text.ToString), 0)) <> CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("MM3")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString) Then
            Return True
        End If

        If _MemberAccumulatorManager IsNot Nothing AndAlso CDec(If(IsNumeric(FamilyAccumulatorsLIATextBox.Text.ToString), CDec(FamilyAccumulatorsLIATextBox.Text.ToString), 0)) <> CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("LIA")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString) Then
            Return True
        End If

        If _MemberAccumulatorManager IsNot Nothing AndAlso CDec(If(IsNumeric(FamilyAccumulatorsCDOTextBox.Text.ToString), CDec(FamilyAccumulatorsCDOTextBox.Text.ToString), 0)) <> CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("CDO")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString) Then
            Return True
        End If

        If _MemberAccumulatorManager IsNot Nothing AndAlso CDec(If(IsNumeric(FamilyAccumulatorsCDPTextBox.Text.ToString), CDec(FamilyAccumulatorsCDPTextBox.Text.ToString), 0)) <> CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("CDP")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString) Then
            Return True
        End If

        If _MemberAccumulatorManager IsNot Nothing AndAlso CDec(If(IsNumeric(FamilyAccumulatorsRNTextBox.Text.ToString), CDec(FamilyAccumulatorsRNTextBox.Text.ToString), 0)) <> CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("RN")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString) Then
            Return True
        End If

        If _MemberAccumulatorManager IsNot Nothing AndAlso CDec(If(IsNumeric(FamilyAccumulatorsRNATextBox.Text.ToString), CDec(FamilyAccumulatorsRNATextBox.Text.ToString), 0)) <> CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("RNA")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString) Then
            Return True
        End If

        If _MemberAccumulatorManager IsNot Nothing AndAlso CDec(If(IsNumeric(FamilyAccumulatorsODSTextBox.Text.ToString), CDec(FamilyAccumulatorsODSTextBox.Text.ToString), 0)) <> CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("ODS")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString) Then
            Return True
        End If

        Return False

    End Function

    Public Sub ClearAllDataSources()

        _OKToValidate = False

        Me.LineDetailsDataGrid.DataSource = Nothing
        Me.FamilyAccumulatorsDataGrid.DataSource = Nothing
        Me.FamilyAccumulatorsPriorDataGrid.DataSource = Nothing
        Me.PersonalAccumulatorDataGrid.DataSource = Nothing
        Me.PersonalAccumulatorPriorDataGrid.DataSource = Nothing

        For X As Integer = 0 To 3
            _NewCurrentRow(X) = -1
            _NewCurrentCol(X) = -1
            _OldCurrentRow(X) = 0
            _OldCurrentCol(X) = 0
            _NewCurrentText(X) = Nothing
            _OldCurrent(X) = Nothing
        Next

        _OKToValidate = True

    End Sub

#End Region

#Region "Style"

    Private Sub SetGridsStyle()
        FamilyAccumulatorsDataGrid.SetTableStyle("AccumulatorsDataGrid.xml")
        FamilyAccumulatorsPriorDataGrid.SetTableStyle("AccumulatorsDataGrid.xml")
        PersonalAccumulatorDataGrid.SetTableStyle("AccumulatorsDataGrid.xml")
        PersonalAccumulatorPriorDataGrid.SetTableStyle("AccumulatorsDataGrid.xml")
        SetDetailsGridStyle()
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    '
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/31/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Private Sub SetDetailsGridStyle()
        Dim TS As DataGridTableStyle
        Dim TextBoxColumn As ConfirmDeleteDataGridFormattableTextBoxColumn

        Try
            'Step 1: Create a DataGridTableStyle &
            '        set mappingname to table.
            TS = New DataGridTableStyle With {
                .MappingName = "DTL_ACCUMS"
            }
            'Step 2: Create DataGridColumnStyle for each col
            '        we want to see in the grid and in the
            '        order that we want to see them.

            TextBoxColumn = New ConfirmDeleteDataGridFormattableTextBoxColumn With {
                .MappingName = "LINE_NBR",
                .HeaderText = "Line",
                .Width = 50
            }
            TS.GridColumnStyles.Add(TextBoxColumn)

            TextBoxColumn = New ConfirmDeleteDataGridFormattableTextBoxColumn With {
                .MappingName = "ACCUM_NAME",
                .HeaderText = "Accumulator",
                .Width = 70
            }
            TextBoxColumn.TextBox.CharacterCasing = CharacterCasing.Upper
            TS.GridColumnStyles.Add(TextBoxColumn)

            TextBoxColumn = New ConfirmDeleteDataGridFormattableTextBoxColumn With {
                .MappingName = "ENTRY_VALUE",
                .HeaderText = "Value",
                .Format = "0.00;-0.00;#####",
                .Width = 50
            }
            TS.GridColumnStyles.Add(TextBoxColumn)

            TextBoxColumn = New ConfirmDeleteDataGridFormattableTextBoxColumn With {
                .MappingName = "OVERRIDE_SW",
                .HeaderText = "",
                .Width = 0
            }
            'column.
            TS.GridColumnStyles.Add(TextBoxColumn)

            TextBoxColumn = New ConfirmDeleteDataGridFormattableTextBoxColumn With {
                .MappingName = "DISPLAY_ORDER",
                .HeaderText = "",
                .Width = 0
            }
            TS.GridColumnStyles.Add(TextBoxColumn)
            TS.AllowSorting = False

            LineDetailsDataGrid.TableStyles.Clear()
            LineDetailsDataGrid.TableStyles.Add(TS)

        Catch ex As Exception
            Throw
        Finally
            If TS IsNot Nothing Then TS.Dispose()
            TS = Nothing
            If TextBoxColumn IsNot Nothing Then TextBoxColumn.Dispose()
            TextBoxColumn = Nothing
        End Try
    End Sub
#End Region

#Region "Misc"
    Private Shared Function IsAbletoView(theFamilyID As Integer, theRelationID As Short) As Boolean

        If UFCWGeneralAD.CMSCanAdjudicateEmployee() OrElse UFCWGeneralAD.CMSEligibilityEmployee() Then Return True 'avoid db2 call by testing here

        Dim DR As DataRow

        Try

            DR = CMSDALFDBMD.RetrievePatientInfo(theFamilyID, theRelationID)

            If DR IsNot Nothing Then

                If CBool(DR("TRUST_SW")) Then Return False
                If CBool(DR("LOCAL_SW")) AndAlso Not (UFCWGeneralAD.CMSUsers() OrElse UFCWGeneralAD.CMSDental() OrElse UFCWGeneralAD.CMSLocalsEmployee() OrElse UFCWGeneralAD.CMSEligibility()) Then Return False

            End If

            Return True

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Sub SetFormInfo(ByVal theFamilyID As Integer, ByVal theRelationID As Short)

        Try
            SetFormInfo(theFamilyID, theRelationID, UFCWGeneral.NowDate, _NonClaimID)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub SetFormInfo(ByVal theFamilyID As Integer, ByVal theRelationID As Short, ByVal planType As String, ByVal searchDate As Date?, ByVal claimID As Integer)
        Dim AccidentEntriesDT As DataTable
        Dim TheConditions As Conditions

        Try
            ClearAllDataSources()

            If Not IsAbletoView(CInt(theFamilyID), CShort(theRelationID)) Then
                _MemberAccumulatorManager = Nothing
                MessageBox.Show("Access to this Family ID is restricted.", "Family ID", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            SetGridsStyle()

            _FamilyID = theFamilyID
            _RelationID = theRelationID

            If _MemberAccumulatorManager Is Nothing OrElse _MemberAccumulatorManager.FamilyID <> theFamilyID OrElse _MemberAccumulatorManager.RelationID <> theRelationID Then
                _MemberAccumulatorManager = New MemberAccumulatorManager(CShort(theRelationID), theFamilyID)
                _MemberAccumulatorManager.RefreshAccumulatorSummariesForMember()
            End If

            AccidentEntriesDT = MemberAccumulatorManager.GetAccidentAccumulatorEntries
            AccidentEntriesDT.DefaultView.Sort = "APPLY_DATE DESC"

            TheConditions = PlanController.GetPlanConditionsForDOS(planType, CDate(searchDate))
            TheConditions = PlanController.ReplaceAccidentGenericWithActualAccidents(TheConditions, CDate(searchDate).Year, AccidentEntriesDT)

            SetFormInfoCommon(TheConditions, AccidentEntriesDT, theFamilyID, theRelationID, CDate(searchDate), claimID)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub SetFormInfo(ByVal theFamilyID As Integer, ByVal theRelationID As Short, ByVal searchDate As Date?, ByVal claimID As Integer)
        SetFormInfo(theFamilyID, theRelationID, searchDate, claimID, False)
    End Sub

    Public Sub SetFormInfo(ByVal theFamilyID As Integer, ByVal theRelationID As Short, ByVal searchDate As Date?, ByVal claimID As Integer, ByVal useClaimDate As Boolean)

        Dim TheConditions As Conditions
        Dim AccidentEntriesDT As DataTable

        Try

            ClearAllDataSources()

            If Not IsAbletoView(theFamilyID, theRelationID) Then
                _MemberAccumulatorManager = Nothing
                MessageBox.Show("Access to this Family ID is restricted.", "Family ID", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Using WC As New GlobalCursor

                Me.Enabled = False

                SetGridsStyle()

                _FamilyID = theFamilyID
                _RelationID = theRelationID

                If claimID <> _NonClaimID Then
                    _ClaimID = claimID
                    _FDos = searchDate
                Else
                    _ClaimID = -5
                    _FDos = Nothing
                End If

                If Not useClaimDate Then searchDate = UFCWGeneral.NowDate ' for default view the current years accumulators should be used.

                If _MemberAccumulatorManager Is Nothing OrElse _MemberAccumulatorManager.FamilyID <> theFamilyID OrElse _MemberAccumulatorManager.RelationID <> theRelationID Then
                    _MemberAccumulatorManager = New MemberAccumulatorManager(CShort(theRelationID), theFamilyID)
                    _MemberAccumulatorManager.RefreshAccumulatorSummariesForMember() 'Load ALL summary records
                End If

                AccidentEntriesDT = MemberAccumulatorManager.GetAccidentAccumulatorEntries
                AccidentEntriesDT.DefaultView.Sort = "APPLY_DATE DESC"

                'Conditions are Accumulators with utilization info
                TheConditions = PlanController.GetDistinctConditionsForDOS(CDate(searchDate))
                TheConditions = PlanController.ReplaceAccidentGenericWithActualAccidents(TheConditions, CDate(searchDate).Year, AccidentEntriesDT)

            End Using

            SetFormInfoCommon(TheConditions, AccidentEntriesDT, theFamilyID, theRelationID, CDate(searchDate), claimID)

        Catch ex As Exception
            Throw
        Finally

            Me.Enabled = True

        End Try
    End Sub

    Public Sub SetFormInfo(ByVal theFamilyID As Integer, ByVal theRelationID As Short, ByVal searchDate As Date, ByVal claimID As Integer, ByVal ruleSetID As Integer)

        Dim TheConditions As Conditions
        Dim AccidentEntriesDT As DataTable

        Try
            ClearAllDataSources()

            If Not IsAbletoView(CInt(theFamilyID), CShort(theRelationID)) Then
                _MemberAccumulatorManager = Nothing
                MessageBox.Show("Access to this Family ID is restricted.", "Family ID", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            SetGridsStyle()

            _FamilyID = theFamilyID
            _RelationID = theRelationID

            If _MemberAccumulatorManager Is Nothing OrElse _MemberAccumulatorManager.FamilyID <> theFamilyID OrElse _MemberAccumulatorManager.RelationID <> theRelationID Then
                _MemberAccumulatorManager = New MemberAccumulatorManager(CShort(theRelationID), theFamilyID)
                _MemberAccumulatorManager.RefreshAccumulatorSummariesForMember()
            End If

            TheConditions = PlanController.GetPlanConditionsForClaim(ruleSetID)
            AccidentEntriesDT = MemberAccumulatorManager.GetAccidentAccumulatorEntries

            TheConditions = PlanController.ReplaceAccidentGenericWithActualAccidents(TheConditions, searchDate.Year, AccidentEntriesDT)

            SetFormInfoCommon(TheConditions, AccidentEntriesDT, theFamilyID, theRelationID, searchDate, claimID)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub SetFormInfo(ByVal theFamilyID As Integer, ByVal theRelationID As Short, ByVal relevantDate As Date)

        Try

            ClearAllDataSources()

            If Not IsAbletoView(theFamilyID, theRelationID) Then
                _MemberAccumulatorManager = Nothing
                MessageBox.Show("Access to this Family ID is restricted.", "Family ID", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            SetFormInfo(theFamilyID, theRelationID, relevantDate, _NonClaimID)

        Catch ex As Exception
            Throw
        Finally

            Me.Enabled = True

        End Try

    End Sub
    Public Sub SetFormInfo(ByVal theSSN As String, ByVal relevantDate As Date)

        Dim TheFamilyID As Integer? = Nothing
        Dim TheRelationID As Short? = Nothing
        Dim DT As DataTable
        Dim DR As DataRow
        Dim AccumulatorFamilyChooserFrm As AccumulatorFamilyChooser

        Try
            ClearAllDataSources()

            DT = CMSDALFDBMD.RetrievePatientsBySSN(CInt(theSSN.Replace("-"c, "")))

            If DT IsNot Nothing Then
                If DT.Rows.Count > 1 Then
                    AccumulatorFamilyChooserFrm = New AccumulatorFamilyChooser

                    AccumulatorFamilyChooserFrm.FamilyRelationDataGrid.DataSource = DT
                    AccumulatorFamilyChooserFrm.SetStyleGrid()
                    AccumulatorFamilyChooserFrm.ShowDialog()

                    TheFamilyID = AccumulatorFamilyChooserFrm.SelectFamilyID
                    TheRelationID = AccumulatorFamilyChooserFrm.SelectRelationID
                    FirstName = AccumulatorFamilyChooserFrm.SelectFirstName
                    LastName = AccumulatorFamilyChooserFrm.SelectLastName
                    PartSSN = CInt(AccumulatorFamilyChooserFrm.SelectPartSSN)

                    AccumulatorFamilyChooserFrm.Close()

                Else
                    DR = DT.Rows(0)
                    If DR IsNot Nothing Then
                        If DR("FAMILY_ID") IsNot System.DBNull.Value AndAlso DR("RELATION_ID") IsNot System.DBNull.Value Then
                            TheFamilyID = CInt(DR("FAMILY_ID"))
                            TheRelationID = UFCWGeneral.IsNullShortHandler(DR("RELATION_ID"))
                            FirstName = CStr(DR("FIRST_NAME"))
                            LastName = CStr(DR("LAST_NAME"))
                            PartSSN = CInt(DR("PART_SSNO"))
                        End If
                    End If
                End If

                MemberAccumulatorManager = Nothing

                If TheFamilyID Is Nothing OrElse TheFamilyID = -1 Then
                    MessageBox.Show("No valid Family ID was selected.", "Family ID", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                ElseIf PartSSN = 0 Then
                    MessageBox.Show("Access to this Family ID is restricted.", "Family ID", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                SetFormInfo(CInt(TheFamilyID), CShort(TheRelationID), relevantDate, _NonClaimID)

            End If

        Catch ex As Exception
            Throw
        Finally
            If AccumulatorFamilyChooserFrm IsNot Nothing Then AccumulatorFamilyChooserFrm.Dispose()
            AccumulatorFamilyChooserFrm = Nothing
        End Try
    End Sub

    Public Sub SetFormInfoCommon(ByVal theConditions As Conditions, ByVal accidentEntriesDT As DataTable, ByVal theFamilyID As Integer, ByVal theRelationID As Integer, ByVal searchDate As Date, ByVal claimID As Integer)

        Dim CPADT As DataTable
        Dim CFADT As DataTable
        Dim PPADT As DataTable
        Dim PFADT As DataTable
        Dim LineDT As DataTable
        Dim DT3 As DataTable

        Try

            PersonalAccumulatorCurrentLabel.Text = "Personal Accumulators (" & searchDate.Year & ")"

            CPADT = theConditions.GetAccumulatorValues(theRelationID, theFamilyID, searchDate, False, _MemberAccumulatorManager)

            'Current Year

            CPADT.Columns.Item("ACCUM_NAME").DefaultValue = String.Empty
            CPADT.Columns.Item("ACCUM_VALUE").DefaultValue = 0
            CPADT.AcceptChanges()

            'filter out manual accumulators that are not defined as annual
            Dim QueryCurrentPersonalAccumulators =
                From CPA In CPADT.AsEnumerable()
                Where (CInt(CPA("MANUAL_SW")) = 0 OrElse (CInt(CPA("MANUAL_SW")) = 1 AndAlso CInt(CPA("DURATION_TYPE")) = 4 AndAlso CInt(CPA("Years")) < 1))
                Order By CPA("DISPLAY_ORDER") Ascending

            Dim QCPADT As DataTable = QueryCurrentPersonalAccumulators.CopyToDataTable
            QCPADT.TableName = "QCPA"

            PersonalAccumulatorDataGrid.DataSource = QCPADT

            FamilyAccumulatorsCurrentLabel.Text = "Family Accumulators (" & searchDate.Year & ")"

            CFADT = theConditions.GetAccumulatorValues(theRelationID, theFamilyID, searchDate, True, _MemberAccumulatorManager)

            CFADT.Columns.Item("ACCUM_NAME").DefaultValue = String.Empty
            CFADT.Columns.Item("ACCUM_VALUE").DefaultValue = 0
            CFADT.AcceptChanges()

            'filter out manual accumulators that are not defined as annual
            Dim QueryCurrentFamilyAccumulators =
                From CFA In CFADT.AsEnumerable()
                Where (CInt(CFA("MANUAL_SW")) = 0 OrElse (CInt(CFA("MANUAL_SW")) = 1 AndAlso CInt(CFA("DURATION_TYPE")) = 4 AndAlso CInt(CFA("Years")) < 1))
                Order By CFA("DISPLAY_ORDER") Ascending

            Dim QCFADT As DataTable = QueryCurrentFamilyAccumulators.CopyToDataTable
            QCFADT.TableName = "QCFA"

            FamilyAccumulatorsDataGrid.DataSource = QCFADT

            'Prior Year
            theConditions = PlanController.GetDistinctConditionsForDOS(searchDate.AddYears(-1))
            theConditions = PlanController.ReplaceAccidentGenericWithActualAccidents(theConditions, searchDate.Year - 1, accidentEntriesDT)

            PersonalAccumulatorPriorLabel.Text = "Personal Accumulators (" & searchDate.AddYears(-1).Year & ")"

            PPADT = theConditions.GetAccumulatorValues(theRelationID, theFamilyID, searchDate.AddYears(-1), False, _MemberAccumulatorManager)

            PPADT.Columns.Item("ACCUM_NAME").DefaultValue = String.Empty
            PPADT.Columns.Item("ACCUM_VALUE").DefaultValue = 0
            PPADT.AcceptChanges()

            'filter out manual accumulators that are not defined as annual
            Dim QueryPriorPersonalAccumulators =
                From PPA In PPADT.AsEnumerable()
                Where (CInt(PPA("MANUAL_SW")) = 0 OrElse (CInt(PPA("MANUAL_SW")) = 1 AndAlso CInt(PPA("DURATION_TYPE")) = 4 AndAlso CInt(PPA("Years")) < 1))
                Order By PPA("DISPLAY_ORDER") Ascending

            Dim QPPADT As DataTable = QueryPriorPersonalAccumulators.CopyToDataTable
            QPPADT.TableName = "QPPA"

            PersonalAccumulatorPriorDataGrid.DataSource = QPPADT

            FamilyAccumulatorsPriorLabel.Text = "Family Accumulators (" & searchDate.AddYears(-1).Year & ")"

            PFADT = theConditions.GetAccumulatorValues(theRelationID, theFamilyID, searchDate.AddYears(-1), True, _MemberAccumulatorManager)

            PFADT.Columns.Item("ACCUM_NAME").DefaultValue = String.Empty
            PFADT.Columns.Item("ACCUM_VALUE").DefaultValue = 0
            PFADT.AcceptChanges()

            'filter out manual accumulators that are not defined as annual
            Dim QueryPriorFamilyAccumulators =
                From PFA In PFADT.AsEnumerable()
                Where (CInt(PFA("MANUAL_SW")) = 0 OrElse (CInt(PFA("MANUAL_SW")) = 1 AndAlso CInt(PFA("DURATION_TYPE")) = 4 AndAlso CInt(PFA("Years")) < 1))
                Order By PFA("DISPLAY_ORDER") Ascending

            Dim QPFADT As DataTable = QueryPriorFamilyAccumulators.CopyToDataTable
            QPFADT.TableName = "QPFA"

            FamilyAccumulatorsPriorDataGrid.DataSource = QPFADT

            DT3 = _MemberAccumulatorManager.GetAccumulatorEntryValues(False, claimID)

            If LineDT Is Nothing OrElse LineDT.Columns.Count < 2 Then
                LineDT = DT3.Copy
                LineDT.Rows.Clear()
            End If
            DT3.AcceptChanges()

            LineDT.BeginLoadData()
            For I As Integer = 0 To DT3.Rows.Count - 1
                LineDT.ImportRow(DT3.Rows(I))
            Next
            LineDT.EndLoadData()

            LineDT.DefaultView.Sort = "DISPLAY_ORDER ASC"
            Me.LineDetailsDataGrid.DataSource = LineDT
            Me.SetDetailsGridStyle()
            LineDT.AcceptChanges()

            SetGridsStyle()

            _HighestEntryID = _MemberAccumulatorManager.GetHighestEntryIdForFamily

            'Line Detail Info

            FamilyAccumulatorsMM3TextBox.Text = String.Format("{0:0.00;-0.00;#####}", CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("MM3")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString))
            FamilyAccumulatorsLIATextBox.Text = String.Format("{0:0.00;-0.00;#####}", CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("LIA")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString))
            FamilyAccumulatorsCDOTextBox.Text = String.Format("{0:0.00;-0.00;#####}", CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("CDO")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString))
            FamilyAccumulatorsCDPTextBox.Text = String.Format("{0:0.00;-0.00;#####}", CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("CDP")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString))
            FamilyAccumulatorsRNTextBox.Text = String.Format("{0:0.00;-0.00;#####}", CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("RN")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString))
            FamilyAccumulatorsRNATextBox.Text = String.Format("{0:0.00;-0.00;#####}", CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("RNA")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString))
            FamilyAccumulatorsODSTextBox.Text = String.Format("{0:0.00;-0.00;#####}", CDec(_MemberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID("ODS")), Global.DateType.Years, 1000, UFCWGeneral.NowDate, DateDirection.Reverse).ToString))

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub FilterByLineNumber(ByVal lineNumber As Short?)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Filters by Line Number.  Value of -1 turns off filtering
        ' </summary>
        ' <param name="lineNumber"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/8/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            If Me.LineDetailsDataGrid.DataSource IsNot Nothing Then
                If lineNumber Is Nothing OrElse lineNumber = -1 Then
                    CType(Me.LineDetailsDataGrid.DataSource, DataTable).DefaultView.RowFilter = ""
                Else
                    CType(Me.LineDetailsDataGrid.DataSource, DataTable).DefaultView.RowFilter = "LINE_NBR = " & lineNumber
                End If
                CType(Me.LineDetailsDataGrid.DataSource, DataTable).DefaultView.Sort = "DISPLAY_ORDER ASC"

                Me.LineDetailsDataGrid.Refresh()

            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub FamilyAccumulatorsTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FamilyAccumulatorsMM3TextBox.TextChanged, FamilyAccumulatorsCDOTextBox.TextChanged, FamilyAccumulatorsCDPTextBox.TextChanged, FamilyAccumulatorsLIATextBox.TextChanged, FamilyAccumulatorsRNTextBox.TextChanged, FamilyAccumulatorsRNATextBox.TextChanged, FamilyAccumulatorsODSTextBox.TextChanged

        Dim TBox As TextBox
        Dim StrTmp As String

        Try

            TBox = CType(sender, TextBox)

            If Not IsNumeric(TBox.Text) AndAlso Len(TBox.Text) > 0 Then
                StrTmp = TBox.Text
                For Cnt As Integer = 1 To Len(StrTmp)
                    If Not IsNumeric(Mid(StrTmp, Cnt, 1)) AndAlso Len(StrTmp) > 0 Then
                        StrTmp = Replace(StrTmp, Mid(StrTmp, Cnt, 1), "")
                    End If
                Next
                TBox.Text = StrTmp
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

#End Region

End Class
Public Class IntEventArgs
    Inherits EventArgs
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Property Target As Integer
    Public Sub New(value As Integer)
        Target = value
    End Sub
End Class

Public Class GenericEventArgs(Of t)
    Inherits EventArgs

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _TargetObject As t

    Public Sub New(target As t)
        _TargetObject = target
    End Sub
    Public Property TargetObject() As t
        Get
            Return _TargetObject
        End Get
        Set(value As t)
            _TargetObject = value
        End Set
    End Property
End Class

