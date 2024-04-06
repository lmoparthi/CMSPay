Imports UFCW.CMS.Accumulator
Imports UFCW.CMS.Plan
Imports UFCW.CMS.DAL
Imports System.Security.Principal
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
Public Class AccumulatorValues
    Inherits System.Windows.Forms.UserControl

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'UserControl overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Public Overloads Sub Dispose()
        If Not PersonalAccumulatorDataGrid Is Nothing Then
            PersonalAccumulatorDataGrid.TableStyles.Clear()
            PersonalAccumulatorDataGrid.DataSource = Nothing
            PersonalAccumulatorDataGrid.Dispose()
        End If
        PersonalAccumulatorDataGrid = Nothing
        If Not FamilyAccumulatorsDataGrid Is Nothing Then
            FamilyAccumulatorsDataGrid.TableStyles.Clear()
            FamilyAccumulatorsDataGrid.DataSource = Nothing
            FamilyAccumulatorsDataGrid.Dispose()
        End If
        FamilyAccumulatorsDataGrid = Nothing

        If Not LineDetailsDataGrid Is Nothing Then
            LineDetailsDataGrid.TableStyles.Clear()
            LineDetailsDataGrid.DataSource = Nothing
            LineDetailsDataGrid.Dispose()
        End If
        LineDetailsDataGrid = Nothing

        If Not FamilyAccumulatorsPriorDataGrid Is Nothing Then
            FamilyAccumulatorsPriorDataGrid.TableStyles.Clear()
            FamilyAccumulatorsPriorDataGrid.DataSource = Nothing
            FamilyAccumulatorsPriorDataGrid.Dispose()
        End If
        FamilyAccumulatorsPriorDataGrid = Nothing

        If Not PersonalAccumulatorPriorDataGrid Is Nothing Then
            PersonalAccumulatorPriorDataGrid.TableStyles.Clear()
            PersonalAccumulatorPriorDataGrid.DataSource = Nothing
            PersonalAccumulatorPriorDataGrid.Dispose()
        End If
        PersonalAccumulatorPriorDataGrid = Nothing

        _manager = Nothing
        Me.ImageList1.Images.Clear()
        Me.ImageList1.Dispose()
        DateTimePicker1.Dispose()
        MyBase.Dispose()
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents PersonalAccumulatorLabel As System.Windows.Forms.Label
    Friend WithEvents FamilyAccumulatorsLabel As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents AccumulatorListSummary As System.Windows.Forms.ComboBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents DateStartOrEnd As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateQuantity As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents DateDirection As System.Windows.Forms.ComboBox
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
    Friend WithEvents GetValueGroupBoxViewButton As System.Windows.Forms.Button
    Friend WithEvents PersonalAccumulatorDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents FamilyAccumulatorsDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents LineDetailsDataGrid As DataGridPlus.DataGridCustom
    Public WithEvents ErrorLabel As System.Windows.Forms.Label
    Friend WithEvents LineDetailsLabel As System.Windows.Forms.Label
    Friend WithEvents FamilyAccumulatorsPriorDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents PersonalAccumulatorPriorDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Friend WithEvents CancelButton As System.Windows.Forms.Button
    Friend WithEvents SaveAndCloseButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(AccumulatorValues))
        Me.PersonalAccumulatorLabel = New System.Windows.Forms.Label
        Me.FamilyAccumulatorsLabel = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.AccumulatorListSummary = New System.Windows.Forms.ComboBox
        Me.GetValueGroupBox = New System.Windows.Forms.GroupBox
        Me.Label28 = New System.Windows.Forms.Label
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.DateStartOrEnd = New System.Windows.Forms.DateTimePicker
        Me.DateQuantity = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.DateDirection = New System.Windows.Forms.ComboBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.DateType = New System.Windows.Forms.ComboBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.GetLatest = New System.Windows.Forms.Button
        Me.AccumulatorValueSummary = New System.Windows.Forms.TextBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.UseYear = New System.Windows.Forms.CheckBox
        Me.CurrentValueYear = New System.Windows.Forms.TextBox
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.GetValueGroupBoxViewButton = New System.Windows.Forms.Button
        Me.PersonalAccumulatorDataGrid = New System.Windows.Forms.DataGrid
        Me.FamilyAccumulatorsDataGrid = New System.Windows.Forms.DataGrid
        Me.LineDetailsDataGrid = New DataGridPlus.DataGridCustom
        Me.LineDetailsLabel = New System.Windows.Forms.Label
        Me.ErrorLabel = New System.Windows.Forms.Label
        Me.FamilyAccumulatorsPriorDataGrid = New System.Windows.Forms.DataGrid
        Me.PersonalAccumulatorPriorDataGrid = New System.Windows.Forms.DataGrid
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.SaveButton = New System.Windows.Forms.Button
        Me.CancelButton = New System.Windows.Forms.Button
        Me.SaveAndCloseButton = New System.Windows.Forms.Button
        Me.GetValueGroupBox.SuspendLayout()
        CType(Me.PersonalAccumulatorDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FamilyAccumulatorsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LineDetailsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FamilyAccumulatorsPriorDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PersonalAccumulatorPriorDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PersonalAccumulatorLabel
        '
        Me.PersonalAccumulatorLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.PersonalAccumulatorLabel.Location = New System.Drawing.Point(6, 6)
        Me.PersonalAccumulatorLabel.Name = "PersonalAccumulatorLabel"
        Me.PersonalAccumulatorLabel.Size = New System.Drawing.Size(170, 15)
        Me.PersonalAccumulatorLabel.TabIndex = 2
        Me.PersonalAccumulatorLabel.Text = "Personal Accumulators (Current)"
        '
        'FamilyAccumulatorsLabel
        '
        Me.FamilyAccumulatorsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.FamilyAccumulatorsLabel.Location = New System.Drawing.Point(210, 6)
        Me.FamilyAccumulatorsLabel.Name = "FamilyAccumulatorsLabel"
        Me.FamilyAccumulatorsLabel.Size = New System.Drawing.Size(164, 15)
        Me.FamilyAccumulatorsLabel.TabIndex = 3
        Me.FamilyAccumulatorsLabel.Text = "Family Accumulators (Current)"
        '
        'Label20
        '
        Me.Label20.Location = New System.Drawing.Point(18, 26)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(86, 23)
        Me.Label20.TabIndex = 17
        Me.Label20.Text = "Accumulator"
        '
        'AccumulatorListSummary
        '
        Me.AccumulatorListSummary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.AccumulatorListSummary.Location = New System.Drawing.Point(113, 25)
        Me.AccumulatorListSummary.Name = "AccumulatorListSummary"
        Me.AccumulatorListSummary.Size = New System.Drawing.Size(121, 21)
        Me.AccumulatorListSummary.TabIndex = 16
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
        Me.GetValueGroupBox.Controls.Add(Me.DateDirection)
        Me.GetValueGroupBox.Controls.Add(Me.Label17)
        Me.GetValueGroupBox.Controls.Add(Me.DateType)
        Me.GetValueGroupBox.Controls.Add(Me.Label18)
        Me.GetValueGroupBox.Controls.Add(Me.GetLatest)
        Me.GetValueGroupBox.Controls.Add(Me.AccumulatorValueSummary)
        Me.GetValueGroupBox.Controls.Add(Me.Label19)
        Me.GetValueGroupBox.Controls.Add(Me.UseYear)
        Me.GetValueGroupBox.Controls.Add(Me.CurrentValueYear)
        Me.GetValueGroupBox.Controls.Add(Me.Label20)
        Me.GetValueGroupBox.Controls.Add(Me.AccumulatorListSummary)
        Me.GetValueGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GetValueGroupBox.Location = New System.Drawing.Point(8, 336)
        Me.GetValueGroupBox.Name = "GetValueGroupBox"
        Me.GetValueGroupBox.Size = New System.Drawing.Size(360, 20)
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
        Me.GroupBox6.Location = New System.Drawing.Point(8, 243)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(344, 8)
        Me.GroupBox6.TabIndex = 17
        Me.GroupBox6.TabStop = False
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(16, 186)
        Me.Label15.Name = "Label15"
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
        Me.DateQuantity.Text = ""
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
        Me.DateDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DateDirection.Items.AddRange(New Object() {"Future", "Past", "Current"})
        Me.DateDirection.Location = New System.Drawing.Point(112, 89)
        Me.DateDirection.Name = "DateDirection"
        Me.DateDirection.Size = New System.Drawing.Size(98, 21)
        Me.DateDirection.TabIndex = 12
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
        Me.GetLatest.TabIndex = 8
        Me.GetLatest.Text = "Get Value"
        '
        'AccumulatorValueSummary
        '
        Me.AccumulatorValueSummary.Location = New System.Drawing.Point(120, 299)
        Me.AccumulatorValueSummary.Name = "AccumulatorValueSummary"
        Me.AccumulatorValueSummary.Size = New System.Drawing.Size(192, 20)
        Me.AccumulatorValueSummary.TabIndex = 6
        Me.AccumulatorValueSummary.Text = ""
        '
        'Label19
        '
        Me.Label19.Location = New System.Drawing.Point(16, 300)
        Me.Label19.Name = "Label19"
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
        Me.CurrentValueYear.Text = ""
        '
        'ImageList1
        '
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.SystemColors.Control
        '
        'GetValueGroupBoxViewButton
        '
        Me.GetValueGroupBoxViewButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.GetValueGroupBoxViewButton.ImageIndex = 0
        Me.GetValueGroupBoxViewButton.ImageList = Me.ImageList1
        Me.GetValueGroupBoxViewButton.Location = New System.Drawing.Point(8, 328)
        Me.GetValueGroupBoxViewButton.Name = "GetValueGroupBoxViewButton"
        Me.GetValueGroupBoxViewButton.Size = New System.Drawing.Size(17, 10)
        Me.GetValueGroupBoxViewButton.TabIndex = 40
        Me.GetValueGroupBoxViewButton.Visible = False
        '
        'PersonalAccumulatorDataGrid
        '
        Me.PersonalAccumulatorDataGrid.AlternatingBackColor = System.Drawing.Color.LightGray
        Me.PersonalAccumulatorDataGrid.BackColor = System.Drawing.Color.DarkGray
        Me.PersonalAccumulatorDataGrid.CaptionBackColor = System.Drawing.Color.White
        Me.PersonalAccumulatorDataGrid.CaptionFont = New System.Drawing.Font("Verdana", 10.0!)
        Me.PersonalAccumulatorDataGrid.CaptionForeColor = System.Drawing.Color.Navy
        Me.PersonalAccumulatorDataGrid.CaptionVisible = False
        Me.PersonalAccumulatorDataGrid.DataMember = ""
        Me.PersonalAccumulatorDataGrid.ForeColor = System.Drawing.Color.Black
        Me.PersonalAccumulatorDataGrid.GridLineColor = System.Drawing.Color.Black
        Me.PersonalAccumulatorDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.PersonalAccumulatorDataGrid.HeaderBackColor = System.Drawing.Color.Silver
        Me.PersonalAccumulatorDataGrid.HeaderForeColor = System.Drawing.Color.Black
        Me.PersonalAccumulatorDataGrid.LinkColor = System.Drawing.Color.Navy
        Me.PersonalAccumulatorDataGrid.Location = New System.Drawing.Point(5, 22)
        Me.PersonalAccumulatorDataGrid.Name = "PersonalAccumulatorDataGrid"
        Me.PersonalAccumulatorDataGrid.ParentRowsBackColor = System.Drawing.Color.White
        Me.PersonalAccumulatorDataGrid.ParentRowsForeColor = System.Drawing.Color.Black
        Me.PersonalAccumulatorDataGrid.ReadOnly = True
        Me.PersonalAccumulatorDataGrid.RowHeadersVisible = False
        Me.PersonalAccumulatorDataGrid.SelectionBackColor = System.Drawing.Color.Navy
        Me.PersonalAccumulatorDataGrid.SelectionForeColor = System.Drawing.Color.White
        Me.PersonalAccumulatorDataGrid.Size = New System.Drawing.Size(192, 144)
        Me.PersonalAccumulatorDataGrid.TabIndex = 41
        '
        'FamilyAccumulatorsDataGrid
        '
        Me.FamilyAccumulatorsDataGrid.AlternatingBackColor = System.Drawing.Color.LightGray
        Me.FamilyAccumulatorsDataGrid.BackColor = System.Drawing.Color.DarkGray
        Me.FamilyAccumulatorsDataGrid.CaptionBackColor = System.Drawing.Color.White
        Me.FamilyAccumulatorsDataGrid.CaptionFont = New System.Drawing.Font("Verdana", 10.0!)
        Me.FamilyAccumulatorsDataGrid.CaptionForeColor = System.Drawing.Color.Navy
        Me.FamilyAccumulatorsDataGrid.CaptionVisible = False
        Me.FamilyAccumulatorsDataGrid.DataMember = ""
        Me.FamilyAccumulatorsDataGrid.ForeColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsDataGrid.GridLineColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.FamilyAccumulatorsDataGrid.HeaderBackColor = System.Drawing.Color.Silver
        Me.FamilyAccumulatorsDataGrid.HeaderForeColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsDataGrid.LinkColor = System.Drawing.Color.Navy
        Me.FamilyAccumulatorsDataGrid.Location = New System.Drawing.Point(208, 22)
        Me.FamilyAccumulatorsDataGrid.Name = "FamilyAccumulatorsDataGrid"
        Me.FamilyAccumulatorsDataGrid.ParentRowsBackColor = System.Drawing.Color.White
        Me.FamilyAccumulatorsDataGrid.ParentRowsForeColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsDataGrid.ReadOnly = True
        Me.FamilyAccumulatorsDataGrid.RowHeadersVisible = False
        Me.FamilyAccumulatorsDataGrid.SelectionBackColor = System.Drawing.Color.Navy
        Me.FamilyAccumulatorsDataGrid.SelectionForeColor = System.Drawing.Color.White
        Me.FamilyAccumulatorsDataGrid.Size = New System.Drawing.Size(192, 144)
        Me.FamilyAccumulatorsDataGrid.TabIndex = 42
        '
        'LineDetailsDataGrid
        '
        Me.LineDetailsDataGrid.AllowColumnReorder = True
        Me.LineDetailsDataGrid.AllowCopy = True
        Me.LineDetailsDataGrid.AllowDelete = True
        Me.LineDetailsDataGrid.AllowDragDrop = False
        Me.LineDetailsDataGrid.AllowEdit = True
        Me.LineDetailsDataGrid.AllowExport = True
        Me.LineDetailsDataGrid.AllowFind = True
        Me.LineDetailsDataGrid.AllowGoTo = True
        Me.LineDetailsDataGrid.AllowMultiSelect = True
        Me.LineDetailsDataGrid.AllowMultiSort = False
        Me.LineDetailsDataGrid.AllowNew = True
        Me.LineDetailsDataGrid.AllowPrint = True
        Me.LineDetailsDataGrid.AllowRefresh = True
        Me.LineDetailsDataGrid.AllowSorting = False
        Me.LineDetailsDataGrid.AlternatingBackColor = System.Drawing.Color.LightGray
        Me.LineDetailsDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineDetailsDataGrid.BackColor = System.Drawing.Color.DarkGray
        Me.LineDetailsDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.LineDetailsDataGrid.CaptionBackColor = System.Drawing.Color.White
        Me.LineDetailsDataGrid.CaptionFont = New System.Drawing.Font("Verdana", 10.0!)
        Me.LineDetailsDataGrid.CaptionForeColor = System.Drawing.Color.Navy
        Me.LineDetailsDataGrid.CaptionVisible = False
        Me.LineDetailsDataGrid.ConfirmDelete = True
        Me.LineDetailsDataGrid.CopySelectedOnly = True
        Me.LineDetailsDataGrid.DataMember = ""
        Me.LineDetailsDataGrid.ExportSelectedOnly = True
        Me.LineDetailsDataGrid.ForeColor = System.Drawing.Color.Black
        Me.LineDetailsDataGrid.GridLineColor = System.Drawing.Color.Black
        Me.LineDetailsDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.LineDetailsDataGrid.HeaderBackColor = System.Drawing.Color.Silver
        Me.LineDetailsDataGrid.HeaderForeColor = System.Drawing.Color.Black
        Me.LineDetailsDataGrid.LastGoToLine = ""
        Me.LineDetailsDataGrid.LinkColor = System.Drawing.Color.Navy
        Me.LineDetailsDataGrid.Location = New System.Drawing.Point(402, 22)
        Me.LineDetailsDataGrid.MultiSort = False
        Me.LineDetailsDataGrid.Name = "LineDetailsDataGrid"
        Me.LineDetailsDataGrid.ParentRowsBackColor = System.Drawing.Color.White
        Me.LineDetailsDataGrid.ParentRowsForeColor = System.Drawing.Color.Black
        Me.LineDetailsDataGrid.ReadOnly = True
        Me.LineDetailsDataGrid.RowHeadersVisible = False
        Me.LineDetailsDataGrid.SelectionBackColor = System.Drawing.Color.Navy
        Me.LineDetailsDataGrid.SelectionForeColor = System.Drawing.Color.White
        Me.LineDetailsDataGrid.SetRowOnRightClick = True
        Me.LineDetailsDataGrid.SingleClickBooleanColumns = True
        Me.LineDetailsDataGrid.Size = New System.Drawing.Size(208, 333)
        Me.LineDetailsDataGrid.TabIndex = 43
        '
        'LineDetailsLabel
        '
        Me.LineDetailsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.LineDetailsLabel.Location = New System.Drawing.Point(80, 336)
        Me.LineDetailsLabel.Name = "LineDetailsLabel"
        Me.LineDetailsLabel.Size = New System.Drawing.Size(232, 16)
        Me.LineDetailsLabel.TabIndex = 44
        Me.LineDetailsLabel.Text = "Line Details"
        '
        'ErrorLabel
        '
        Me.ErrorLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ErrorLabel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ErrorLabel.ForeColor = System.Drawing.Color.Red
        Me.ErrorLabel.Location = New System.Drawing.Point(8, 96)
        Me.ErrorLabel.Name = "ErrorLabel"
        Me.ErrorLabel.Size = New System.Drawing.Size(608, 16)
        Me.ErrorLabel.TabIndex = 45
        Me.ErrorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ErrorLabel.Visible = False
        '
        'FamilyAccumulatorsPriorDataGrid
        '
        Me.FamilyAccumulatorsPriorDataGrid.AlternatingBackColor = System.Drawing.Color.LightGray
        Me.FamilyAccumulatorsPriorDataGrid.BackColor = System.Drawing.Color.DarkGray
        Me.FamilyAccumulatorsPriorDataGrid.CaptionBackColor = System.Drawing.Color.White
        Me.FamilyAccumulatorsPriorDataGrid.CaptionFont = New System.Drawing.Font("Verdana", 10.0!)
        Me.FamilyAccumulatorsPriorDataGrid.CaptionForeColor = System.Drawing.Color.Navy
        Me.FamilyAccumulatorsPriorDataGrid.CaptionVisible = False
        Me.FamilyAccumulatorsPriorDataGrid.DataMember = ""
        Me.FamilyAccumulatorsPriorDataGrid.ForeColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsPriorDataGrid.GridLineColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsPriorDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.FamilyAccumulatorsPriorDataGrid.HeaderBackColor = System.Drawing.Color.Silver
        Me.FamilyAccumulatorsPriorDataGrid.HeaderForeColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsPriorDataGrid.LinkColor = System.Drawing.Color.Navy
        Me.FamilyAccumulatorsPriorDataGrid.Location = New System.Drawing.Point(208, 183)
        Me.FamilyAccumulatorsPriorDataGrid.Name = "FamilyAccumulatorsPriorDataGrid"
        Me.FamilyAccumulatorsPriorDataGrid.ParentRowsBackColor = System.Drawing.Color.White
        Me.FamilyAccumulatorsPriorDataGrid.ParentRowsForeColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsPriorDataGrid.ReadOnly = True
        Me.FamilyAccumulatorsPriorDataGrid.RowHeadersVisible = False
        Me.FamilyAccumulatorsPriorDataGrid.SelectionBackColor = System.Drawing.Color.Navy
        Me.FamilyAccumulatorsPriorDataGrid.SelectionForeColor = System.Drawing.Color.White
        Me.FamilyAccumulatorsPriorDataGrid.Size = New System.Drawing.Size(192, 144)
        Me.FamilyAccumulatorsPriorDataGrid.TabIndex = 49
        '
        'PersonalAccumulatorPriorDataGrid
        '
        Me.PersonalAccumulatorPriorDataGrid.AlternatingBackColor = System.Drawing.Color.LightGray
        Me.PersonalAccumulatorPriorDataGrid.BackColor = System.Drawing.Color.DarkGray
        Me.PersonalAccumulatorPriorDataGrid.CaptionBackColor = System.Drawing.Color.White
        Me.PersonalAccumulatorPriorDataGrid.CaptionFont = New System.Drawing.Font("Verdana", 10.0!)
        Me.PersonalAccumulatorPriorDataGrid.CaptionForeColor = System.Drawing.Color.Navy
        Me.PersonalAccumulatorPriorDataGrid.CaptionVisible = False
        Me.PersonalAccumulatorPriorDataGrid.DataMember = ""
        Me.PersonalAccumulatorPriorDataGrid.ForeColor = System.Drawing.Color.Black
        Me.PersonalAccumulatorPriorDataGrid.GridLineColor = System.Drawing.Color.Black
        Me.PersonalAccumulatorPriorDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.PersonalAccumulatorPriorDataGrid.HeaderBackColor = System.Drawing.Color.Silver
        Me.PersonalAccumulatorPriorDataGrid.HeaderForeColor = System.Drawing.Color.Black
        Me.PersonalAccumulatorPriorDataGrid.LinkColor = System.Drawing.Color.Navy
        Me.PersonalAccumulatorPriorDataGrid.Location = New System.Drawing.Point(5, 183)
        Me.PersonalAccumulatorPriorDataGrid.Name = "PersonalAccumulatorPriorDataGrid"
        Me.PersonalAccumulatorPriorDataGrid.ParentRowsBackColor = System.Drawing.Color.White
        Me.PersonalAccumulatorPriorDataGrid.ParentRowsForeColor = System.Drawing.Color.Black
        Me.PersonalAccumulatorPriorDataGrid.ReadOnly = True
        Me.PersonalAccumulatorPriorDataGrid.RowHeadersVisible = False
        Me.PersonalAccumulatorPriorDataGrid.SelectionBackColor = System.Drawing.Color.Navy
        Me.PersonalAccumulatorPriorDataGrid.SelectionForeColor = System.Drawing.Color.White
        Me.PersonalAccumulatorPriorDataGrid.Size = New System.Drawing.Size(192, 144)
        Me.PersonalAccumulatorPriorDataGrid.TabIndex = 48
        '
        'Label1
        '
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label1.Location = New System.Drawing.Point(208, 167)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(164, 15)
        Me.Label1.TabIndex = 47
        Me.Label1.Text = "Family Accumulators (Prior)"
        '
        'Label2
        '
        Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label2.Location = New System.Drawing.Point(8, 167)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(170, 15)
        Me.Label2.TabIndex = 46
        Me.Label2.Text = "Personal Accumulators (Prior)"
        '
        'SaveButton
        '
        Me.SaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.SaveButton.Location = New System.Drawing.Point(8, 336)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.TabIndex = 50
        Me.SaveButton.Text = "Save"
        '
        'CancelButton
        '
        Me.CancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CancelButton.Location = New System.Drawing.Point(185, 336)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.TabIndex = 51
        Me.CancelButton.Text = "Cancel"
        '
        'SaveAndCloseButton
        '
        Me.SaveAndCloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SaveAndCloseButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.SaveAndCloseButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SaveAndCloseButton.Location = New System.Drawing.Point(93, 336)
        Me.SaveAndCloseButton.Name = "SaveAndCloseButton"
        Me.SaveAndCloseButton.Size = New System.Drawing.Size(83, 23)
        Me.SaveAndCloseButton.TabIndex = 52
        Me.SaveAndCloseButton.Text = "Save and Close"
        '
        'AccumulatorValues
        '
        Me.Controls.Add(Me.SaveAndCloseButton)
        Me.Controls.Add(Me.CancelButton)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.FamilyAccumulatorsPriorDataGrid)
        Me.Controls.Add(Me.PersonalAccumulatorPriorDataGrid)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ErrorLabel)
        Me.Controls.Add(Me.LineDetailsLabel)
        Me.Controls.Add(Me.LineDetailsDataGrid)
        Me.Controls.Add(Me.FamilyAccumulatorsDataGrid)
        Me.Controls.Add(Me.PersonalAccumulatorDataGrid)
        Me.Controls.Add(Me.GetValueGroupBoxViewButton)
        Me.Controls.Add(Me.GetValueGroupBox)
        Me.Controls.Add(Me.FamilyAccumulatorsLabel)
        Me.Controls.Add(Me.PersonalAccumulatorLabel)
        Me.Name = "AccumulatorValues"
        Me.Size = New System.Drawing.Size(616, 368)
        Me.GetValueGroupBox.ResumeLayout(False)
        CType(Me.PersonalAccumulatorDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FamilyAccumulatorsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LineDetailsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FamilyAccumulatorsPriorDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PersonalAccumulatorPriorDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Private Variables and Properties"
    'Dim conds As Conditions
    Private Const nonClaimId As Integer = -5
    Private _manager As MemberAccumulatorManager
    Private _showLineDetails As Boolean
    Private _readOnly As Boolean = True
    Private _editMode As Boolean = False
    Private _userId As String = ""
    Public Event Closing()
    Private loaded As Boolean = False
    Private loading As Boolean = True
    Private _familyId As Integer
    Private _relationId As Integer
    Private highestEntryId As Integer
    Private _ssn As String = ""
    Private _load As Integer
    Private _lastName As String
    Private _firstName As String
    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets or Sets FamilyID")> _
    Public ReadOnly Property FamilyId() As Integer
        Get
            Return _familyId
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets or Sets RelationID")> _
    Public ReadOnly Property RelationId() As Integer
        Get
            Return _relationId
        End Get
    End Property

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
    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets or Sets Accumulator Manager")> _
    Public Property MemberAccumulatorManager() As MemberAccumulatorManager
        Get
            Return _manager
        End Get
        Set(ByVal Value As MemberAccumulatorManager)
            'If loading = False Then
            LoadData()
            If Value Is Nothing Then Return
            _manager = Value
            'End If
        End Set
    End Property
    <System.ComponentModel.Browsable(True), System.ComponentModel.defaultvalue(True), System.ComponentModel.Description("Gets or Sets if Line Item details are displayed")> _
    Public Property ShowLineDetails() As Boolean
        Get
            Return _showLineDetails
        End Get
        Set(ByVal Value As Boolean)
            If loading = False Then
                LoadData()
                _showLineDetails = Value
                Me.LineDetailsDataGrid.Visible = _showLineDetails
                Me.LineDetailsLabel.Visible = _showLineDetails
                ToggleLineDetailsArea()
            End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets or Sets UserID")> _
    Public Property UserId() As String
        Get
            Return _userId
        End Get
        Set(ByVal Value As String)
            If loading = False Then
                LoadData()
                If Value Is Nothing Then Return
                _userId = Value
            End If
        End Set
    End Property
    'Public WriteOnly Property SSN() As String
    '    Set(ByVal Value As String)
    '        If Value Is Nothing Then Return
    '        If Value.Length < 1 Then Return
    '        Dim dr As DataRow = ClaimDAL.RetrieveParticipantInfo(Value)
    '        If Not dr("FAMILY_ID") Is System.DBNull.Value And Not dr("RELATION_ID") Is System.DBNull.Value Then
    '            _manager = New MemberAccumulatorManager(dr("RELATION_ID"), dr("FAMILY_ID"))

    '        Else
    '            _manager = Nothing
    '        End If
    '    End Set
    'End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.DefaultValue(True), System.ComponentModel.Description("Gets or Sets firstName")> _
    Public Property ControlIsReadOnly() As Boolean
        Get
            Return _readOnly
        End Get
        Set(ByVal Value As Boolean)
            If loading = False Then
                LoadData()
                _readOnly = Value
                Me.PersonalAccumulatorDataGrid.ReadOnly = Value
                Me.PersonalAccumulatorPriorDataGrid.ReadOnly = Value
                Me.FamilyAccumulatorsDataGrid.ReadOnly = Value
                Me.FamilyAccumulatorsPriorDataGrid.ReadOnly = Value
            End If
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.DefaultValue(True), System.ComponentModel.Description("Determines if the control is editable")> _
    Public Property IsInEditMode() As Boolean
        Get
            Return _editMode
        End Get
        Set(ByVal Value As Boolean)
            If loading = False Then
                LoadData()
                _editMode = Value
                'TODO:  Uncomment next two lines to bring back
                '  dynamic ACCUMULATOR queries.
                'GetValueGroupBox.Visible = Not _editMode
                'Me.GetValueGroupBoxViewButton.Visible = Not _editMode
                Me.SaveButton.Visible = _editMode
                Me.SaveAndCloseButton.Visible = _editMode
                Me.CancelButton.Visible = _editMode
                ControlIsReadOnly = Not _editMode
            End If
        End Set
    End Property
    Public Shadows Event Resize(ByVal changedHeight As Integer)
    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets or Sets SSN")> _
    Public Property SSN() As String
        Get
            Return _ssn
        End Get
        Set(ByVal Value As String)
            _ssn = Value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets or Sets firstName")> _
    Public Property LoadID() As Integer
        Get
            Return _load
        End Get
        Set(ByVal Value As Integer)
            _load = Value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets or Sets LastName")> _
        Public Property LastName() As String
        Get
            Return _lastName
        End Get
        Set(ByVal Value As String)
            _lastName = Value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets or Sets firstName")> _
        Public Property FirstName() As String
        Get
            Return _firstName
        End Get
        Set(ByVal Value As String)
            _firstName = Value
        End Set
    End Property
#End Region

#Region "Form and Control Events"

    Private Sub AccumulatorValues_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            loading = False
            SetDetailsGridStyle()
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "")
        End Try
    End Sub

    Private Sub GetLatest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetLatest.Click
        'Try
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Dim accumId As Integer = AccumulatorController.GetAccumulatorId(Me.AccumulatorListSummary.Text)
        'InstantiateManagers(MemberIdSummary.Text, Me.FamilyIdSummary.Text)

        Select Case Me.DateType.Text
            Case "Quarters"
                '       BuildDataGrid(accumId)
                If Me.DateDirection.Text.Trim.Length < 1 Then
                    MessageBox.Show("Please Select a Date Direction", "Accumulator Values", MessageBoxButtons.OK)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Return
                ElseIf Me.DateQuantity.Text.Length < 1 OrElse Not IsNumeric(Me.DateQuantity.Text.Trim) Then
                    MessageBox.Show("Please Select a Valid Quantity", "Accumulator Values", MessageBoxButtons.OK)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Return
                End If
                Me.AccumulatorValueSummary.Text = _manager.GetProposedValue(accumId, MemberAccumulator.DateTypes.Quarters, Me.DateQuantity.Text, Me.DateStartOrEnd.Value, IIf(Me.DateDirection.Text = "Future", MemberAccumulator.DateDirection.Forward, MemberAccumulator.DateDirection.Reverse))
            Case "Months"
                If Me.DateDirection.Text.Trim.Length < 1 Then
                    MessageBox.Show("Please Select a Date Direction", "Accumulator Values", MessageBoxButtons.OK)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Return
                ElseIf Me.DateQuantity.Text.Length < 1 OrElse Not IsNumeric(Me.DateQuantity.Text.Trim) Then
                    MessageBox.Show("Please Select a Valid Quantity", "Accumulator Values", MessageBoxButtons.OK)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Return
                End If
                '      BuildDataGrid(accumId)
                Me.AccumulatorValueSummary.Text = _manager.GetProposedValue(accumId, MemberAccumulator.DateTypes.Months, Me.DateQuantity.Text, Me.DateStartOrEnd.Value, IIf(Me.DateDirection.Text = "Future", MemberAccumulator.DateDirection.Forward, MemberAccumulator.DateDirection.Reverse))
            Case "Weeks"
                If Me.DateDirection.Text.Trim.Length < 1 Then
                    MessageBox.Show("Please Select a Date Direction", "Accumulator Values", MessageBoxButtons.OK)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Return
                ElseIf Me.DateQuantity.Text.Length < 1 OrElse Not IsNumeric(Me.DateQuantity.Text.Trim) Then
                    MessageBox.Show("Please Select a Valid Quantity", "Accumulator Values", MessageBoxButtons.OK)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Return
                End If
                '     BuildDataGrid(accumId)
                Me.AccumulatorValueSummary.Text = _manager.GetProposedValue(accumId, MemberAccumulator.DateTypes.Weeks, Me.DateQuantity.Text, Me.DateStartOrEnd.Value, IIf(Me.DateDirection.Text = "Future", MemberAccumulator.DateDirection.Forward, MemberAccumulator.DateDirection.Reverse))
            Case "Days"
                If Me.DateDirection.Text.Trim.Length < 1 Then
                    MessageBox.Show("Please Select a Date Direction", "Accumulator Values", MessageBoxButtons.OK)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Return
                ElseIf Me.DateQuantity.Text.Length < 1 OrElse Not IsNumeric(Me.DateQuantity.Text.Trim) Then
                    MessageBox.Show("Please Select a Valid Quantity", "Accumulator Values", MessageBoxButtons.OK)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Return
                End If
                '    BuildDataGrid(accumId)
                Me.AccumulatorValueSummary.Text = _manager.GetProposedValue(accumId, MemberAccumulator.DateTypes.Days, Me.DateQuantity.Text, Me.DateStartOrEnd.Value, IIf(Me.DateDirection.Text = "Future", MemberAccumulator.DateDirection.Forward, MemberAccumulator.DateDirection.Reverse))
            Case "Lifetime"
                '   BuildDataGrid(accumId)
                Me.AccumulatorValueSummary.Text = _manager.GetProposedLifetimeValue(accumId)
            Case "Rollover"
                If Me.UseYear.Checked And (CurrentValueYear.Text.Length < 1 OrElse Not IsNumeric(Me.CurrentValueYear.Text.Trim)) Then
                    MessageBox.Show("Please Select a Valid Year", "Accumulator Values", MessageBoxButtons.OK)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Return
                End If
                If UseYear.Checked Then
                    Me.AccumulatorValueSummary.Text = _manager.GetProposedRolloverValue(accumId, CInt(Me.CurrentValueYear.Text))
                Else
                    Me.AccumulatorValueSummary.Text = _manager.GetProposedRolloverValue(accumId, Now.Year)
                End If
            Case "Current"
                'BuildDataGrid(accumId)
                If UseYear.Checked Then
                    Me.AccumulatorValueSummary.Text = _manager.GetProposedYearValue(accumId, CInt(Me.CurrentValueYear.Text))
                Else
                    Me.AccumulatorValueSummary.Text = _manager.GetProposedCurrentValueForCurrentYear(accumId)
                End If
            Case "Annual"
                If Me.UseYear.Checked And (CurrentValueYear.Text.Length < 1 OrElse Not IsNumeric(Me.CurrentValueYear.Text.Trim)) Then
                    MessageBox.Show("Please Select a Valid Year", "Accumulator Values", MessageBoxButtons.OK)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Return
                End If
                If UseYear.Checked Then
                    Me.AccumulatorValueSummary.Text = _manager.GetProposedYearValue(accumId, CInt(Me.CurrentValueYear.Text))
                Else
                    Me.AccumulatorValueSummary.Text = _manager.GetProposedCurrentValueForCurrentYear(accumId)
                End If
                'Case "Years"
                '    Me.AccumulatorValueSummary.Text = manager.GetProposedValue(AccumulatorController.GetAccumulatorId(Me.AccumulatorListSummary.Text), MemberAccumulator.DateTypes.Years, Me.DateQuantity.Text, Me.DateStartOrEnd.Value, IIf(Me.DateDirection.Text = "Future", MemberAccumulator.DateDirection.Forward, MemberAccumulator.DateDirection.Reverse))

        End Select
        Me.Cursor = System.Windows.Forms.Cursors.Default
    End Sub

    Private Sub ClaimGroupBoxViewButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetValueGroupBoxViewButton.Click
        If Me.GetValueGroupBox.Height = 20 Then
            Me.GetValueGroupBox.Height = 325
            Me.Height += 305
            RaiseEvent Resize(305)
        Else
            Me.GetValueGroupBox.Height = 20
            Me.Height -= 305
            RaiseEvent Resize(-305)
        End If

        If Me.GetValueGroupBoxViewButton.ImageIndex = 0 Then
            Me.GetValueGroupBoxViewButton.ImageIndex = 1
        Else
            Me.GetValueGroupBoxViewButton.ImageIndex = 0
        End If

    End Sub

    Private Sub DateType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateType.SelectedIndexChanged
        Select Case DateType.Text
            Case "Lifetime", "Rollover", "Annual"
                Label17.Visible = False : Label16.Visible = False : Label15.Visible = False
                DateDirection.Visible = False : DateQuantity.Visible = False : Me.DateStartOrEnd.Visible = False
                If DateType.Text = "Lifetime" Then
                    UseYear.Visible = False : CurrentValueYear.Visible = False
                Else
                    UseYear.Visible = True : CurrentValueYear.Visible = True
                End If
                Label28.Visible = False
                DateTimePicker1.Visible = False
            Case Else
                Label17.Visible = True : Label16.Visible = True : Label15.Visible = True
                DateDirection.Visible = True : DateQuantity.Visible = True : Me.DateStartOrEnd.Visible = True
                UseYear.Visible = False : CurrentValueYear.Visible = False
                Label28.Visible = True
                DateTimePicker1.Visible = True
        End Select
        Me.GetLatest.Enabled = True
    End Sub

    Private Sub UseYear_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UseYear.CheckedChanged
        CurrentValueYear.Enabled = UseYear.Checked
    End Sub

    Private Sub DateDirection_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateDirection.SelectedIndexChanged
        Try
            If DateDirection.SelectedItem = "Current" Then
                DateQuantity.Text = 0
                DateQuantity.Enabled = False
                Label15.Text = "Current Date"
                Label28.Visible = False
                DateTimePicker1.Visible = False
            Else
                Label28.Visible = True
                DateTimePicker1.Visible = True
                If DateDirection.SelectedItem = "Past" Then
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
            MsgBox(ex.Message.ToString)
        End Try
    End Sub
#End Region

#Region "Style"

    Private Sub SetGridStyle()
        SetPersonGridStyle()
        SetPersonPriorGridStyle()
        SetFamilyGridStyle()
        SetFamilyPriorGridStyle()
        SetDetailsGridStyle()
    End Sub
    Private Sub SetFamilyGridStyle()
        Try
            'Step 1: Create a DataGridTableStyle & 
            '        set mappingname to table.
            Dim tableStyle As New DataGridTableStyle
            tableStyle.MappingName = ""
            'Step 2: Create DataGridColumnStyle for each col 
            '        we want to see in the grid and in the 
            '        order that we want to see them.

            Dim column As New DataGridTextBoxColumn
            column.MappingName = "ACCUM_NAME"
            column.HeaderText = "Accumulator"
            column.Width = 65
            tableStyle.GridColumnStyles.Add(column)

            column = New DataGridTextBoxColumn
            column.MappingName = "ACCUM_VALUE"
            column.HeaderText = "Value"
            column.Format = "#.##"
            column.Width = 70
            tableStyle.GridColumnStyles.Add(column)

            'Step 3: Add the tablestyle to the datagrid
            FamilyAccumulatorsDataGrid.TableStyles.Add(tableStyle)
            'FamilyAccumulatorsPriorDataGrid.TableStyles.Add(tableStyle)

        Catch ex As Exception
            MsgBox("An Error Has Occured", MsgBoxStyle.Exclamation, "UFCW Accumulator Value")
        End Try
    End Sub

    Private Sub SetFamilyPriorGridStyle()
        Try
            'Step 1: Create a DataGridTableStyle & 
            '        set mappingname to table.
            Dim tableStyle As New DataGridTableStyle
            tableStyle.MappingName = ""
            'Step 2: Create DataGridColumnStyle for each col 
            '        we want to see in the grid and in the 
            '        order that we want to see them.

            Dim column As New DataGridTextBoxColumn
            column.MappingName = "ACCUM_NAME"
            column.HeaderText = "Accumulator"
            column.Width = 65
            tableStyle.GridColumnStyles.Add(column)

            column = New DataGridTextBoxColumn
            column.MappingName = "ACCUM_VALUE"
            column.HeaderText = "Value"
            column.Format = "#.##"
            column.Width = 70
            tableStyle.GridColumnStyles.Add(column)

            'Step 3: Add the tablestyle to the datagrid
            'FamilyAccumulatorsDataGrid.TableStyles.Add(tableStyle)
            FamilyAccumulatorsPriorDataGrid.TableStyles.Add(tableStyle)

        Catch ex As Exception
            MsgBox("An Error Has Occured", MsgBoxStyle.Exclamation, "UFCW Accumulator Value")
        End Try
    End Sub
    Private Sub SetPersonGridStyle()
        Try
            'Step 1: Create a DataGridTableStyle & 
            '        set mappingname to table.
            Dim tableStyle As New DataGridTableStyle
            tableStyle.MappingName = ""
            'Step 2: Create DataGridColumnStyle for each col 
            '        we want to see in the grid and in the 
            '        order that we want to see them.

            Dim column As New DataGridTextBoxColumn
            column.MappingName = "ACCUM_NAME"
            column.HeaderText = "Accumulator"
            column.Width = 65
            tableStyle.GridColumnStyles.Add(column)

            column = New DataGridTextBoxColumn
            column.MappingName = "ACCUM_VALUE"
            column.HeaderText = "Value"
            column.Format = "#.##"
            column.Width = 70
            tableStyle.GridColumnStyles.Add(column)

            'Step 3: Add the tablestyle to the datagrid
            PersonalAccumulatorDataGrid.TableStyles.Add(tableStyle)
            'PersonalAccumulatorPriorDataGrid.TableStyles.Add(tableStyle)

        Catch ex As Exception
            MsgBox("An Error Has Occured", MsgBoxStyle.Exclamation, "UFCW Accumulator Values")
        End Try
    End Sub
    Private Sub SetPersonPriorGridStyle()
        Try
            'Step 1: Create a DataGridTableStyle & 
            '        set mappingname to table.
            Dim tableStyle As New DataGridTableStyle
            tableStyle.MappingName = ""
            'Step 2: Create DataGridColumnStyle for each col 
            '        we want to see in the grid and in the 
            '        order that we want to see them.

            Dim column As New DataGridTextBoxColumn
            column.MappingName = "ACCUM_NAME"
            column.HeaderText = "Accumulator"
            column.Width = 65
            tableStyle.GridColumnStyles.Add(column)

            column = New DataGridTextBoxColumn
            column.MappingName = "ACCUM_VALUE"
            column.HeaderText = "Value"
            column.Format = "#.##"
            column.Width = 70
            tableStyle.GridColumnStyles.Add(column)

            'Step 3: Add the tablestyle to the datagrid
            'PersonalAccumulatorDataGrid.TableStyles.Add(tableStyle)
            PersonalAccumulatorPriorDataGrid.TableStyles.Add(tableStyle)

        Catch ex As Exception
            MsgBox("An Error Has Occured", MsgBoxStyle.Exclamation, "UFCW Accumulator Values")
        End Try
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
        Try
            'Step 1: Create a DataGridTableStyle & 
            '        set mappingname to table.
            Dim tableStyle As New DataGridTableStyle
            tableStyle.MappingName = "DTL_ACCUMS"
            'Step 2: Create DataGridColumnStyle for each col 
            '        we want to see in the grid and in the 
            '        order that we want to see them.

            Dim column As New DataGridTextBoxColumn
            'column = New DataGridTextBoxColumn
            'column.MappingName = "CLAIM_ID"
            'column.HeaderText = "Claim #"
            'column.Width = 45
            'tableStyle.GridColumnStyles.Add(column)

            'column = New DataGridTextBoxColumn
            column.MappingName = "LINE_NBR"
            column.HeaderText = "Line"
            column.Width = 50
            tableStyle.GridColumnStyles.Add(column)

            column = New DataGridTextBoxColumn
            column.MappingName = "ACCUM_NAME"
            column.HeaderText = "Accumulator"
            column.Width = 70
            tableStyle.GridColumnStyles.Add(column)

            column = New DataGridTextBoxColumn
            column.MappingName = "ENTRY_VALUE"
            column.HeaderText = "Value"
            column.Width = 50
            tableStyle.GridColumnStyles.Add(column)

            column = New DataGridTextBoxColumn
            column.MappingName = "OVERRIDE_SW"
            column.HeaderText = ""
            column.Width = 0
            'column.
            tableStyle.GridColumnStyles.Add(column)

            column = New DataGridTextBoxColumn
            column.MappingName = "DISPLAY_ORDER"
            column.HeaderText = ""
            column.Width = 0
            tableStyle.GridColumnStyles.Add(column)
            tableStyle.AllowSorting = False

            Me.LineDetailsDataGrid.TableStyles.Clear()
            Me.LineDetailsDataGrid.TableStyles.Add(tableStyle)
        Catch ex As Exception
            MsgBox("An Error Has Occured", MsgBoxStyle.Exclamation, "UFCW Accumulator Values")
        End Try
    End Sub
#End Region

#Region "Misc"
    Private Sub ToggleLineDetailsArea()
        If _showLineDetails Then
            Me.Width += 215
            Me.LineDetailsDataGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
            Me.FamilyAccumulatorsDataGrid.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.FamilyAccumulatorsPriorDataGrid.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            GetValueGroupBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.FamilyAccumulatorsDataGrid.Location = New System.Drawing.Point(208, 22)
            Me.FamilyAccumulatorsPriorDataGrid.Location = New System.Drawing.Point(208, 183)
            Me.GetValueGroupBox.Location = New Point(8, 336)
            Me.GetValueGroupBox.Width = Me.FamilyAccumulatorsDataGrid.Width + Me.PersonalAccumulatorDataGrid.Width
            Me.FamilyAccumulatorsDataGrid.Width = Me.PersonalAccumulatorDataGrid.Width
            Me.FamilyAccumulatorsPriorDataGrid.Width = Me.PersonalAccumulatorDataGrid.Width
            'Me.SaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            'Me.CancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.Height = 380
            PersonalAccumulatorPriorDataGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Else
            Me.Width -= Me.LineDetailsDataGrid.Width - 10
            Me.FamilyAccumulatorsDataGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.FamilyAccumulatorsPriorDataGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
            GetValueGroupBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.FamilyAccumulatorsDataGrid.Location = New System.Drawing.Point(208, 22)
            Me.FamilyAccumulatorsPriorDataGrid.Location = New System.Drawing.Point(208, 183)
            Me.GetValueGroupBox.Location = New Point(8, 336)
            Me.Height = 380
            PersonalAccumulatorPriorDataGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            'Me.SaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            'Me.CancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        End If
    End Sub

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' 
    ' </summary>
    ' <param name="familyId"></param>
    ' <param name="personId"></param>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	10/31/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub SetFormInfo(ByVal familyId As Integer, ByVal personId As Integer, ByVal relevantDate As Date, ByVal claimId As Integer)
        Try
            Me.SetGridStyle()
            Dim lineDataTable As DataTable
            Dim dt3 As DataTable
            'If _manager Is Nothing Then
            _familyId = familyId
            _relationId = personId
            If _manager Is Nothing OrElse _manager.FamilyId <> familyId OrElse _manager.MemberId <> personId Then
                _manager = New MemberAccumulatorManager(personId, familyId)
                _manager.RefreshAccumulatorSummariesForMember()
            End If
            'End If

            Application.DoEvents()

            Dim conds As Conditions = PlanController.GetUniqueConditions
            Dim accidentEntries As DataTable = MemberAccumulatorManager.GetAccidentAccumulatorEntries

            conds = PlanController.ReplaceAccidentGenericWithActualAccidents(conds, Now.Year, accidentEntries)

            Dim dt As DataTable = conds.GetAccumulatorValues(personId, familyId, relevantDate, False, _manager)
            dt.DefaultView.RowFilter = "MANUAL_SW = 0"
            dt.DefaultView.Sort = "DISPLAY_ORDER ASC"
            dt.AcceptChanges()
            Me.PersonalAccumulatorDataGrid.DataSource = dt

            Dim dt2 As DataTable = conds.GetAccumulatorValues(personId, familyId, relevantDate, True, _manager)
            dt2.DefaultView.Sort = "DISPLAY_ORDER ASC"
            dt2.AcceptChanges()
            Me.FamilyAccumulatorsDataGrid.DataSource = dt2

            'Prior Year
            conds = PlanController.ReplaceAccidentGenericWithActualAccidents(conds, Now.Year - 1, accidentEntries)

            Dim dt4 As DataTable = conds.GetAccumulatorValues(personId, familyId, relevantDate.AddYears(-1), False, _manager)
            dt4.DefaultView.RowFilter = "MANUAL_SW = 0"
            dt4.DefaultView.Sort = "DISPLAY_ORDER ASC"
            dt4.AcceptChanges()
            Me.PersonalAccumulatorPriorDataGrid.DataSource = dt4

            Dim dt5 As DataTable = conds.GetAccumulatorValues(personId, familyId, relevantDate.AddYears(-1), True, _manager)
            dt5.DefaultView.Sort = "DISPLAY_ORDER ASC"
            dt5.DefaultView.RowFilter = "MANUAL_SW = 0" 'Don't display Manual accumulators for Prior Years (They are LifeTime and always apply to current year)
            dt5.AcceptChanges()
            Me.FamilyAccumulatorsPriorDataGrid.DataSource = dt5

            dt3 = _manager.GetAccumulatorEntryValues(False, claimId)
            If lineDataTable Is Nothing OrElse lineDataTable.Columns.Count < 2 Then
                lineDataTable = dt3.Copy
                lineDataTable.Rows.Clear()
            End If
            dt3.AcceptChanges()
            'lineDataTable.Columns("DISPLAY_ORDER").DataType = Type.GetType("System.Int32")
            For i As Integer = 0 To dt3.Rows.Count - 1
                lineDataTable.ImportRow(dt3.Rows(i))
            Next
            lineDataTable.DefaultView.Sort = "DISPLAY_ORDER ASC"
            Me.LineDetailsDataGrid.DataSource = lineDataTable
            Me.SetDetailsGridStyle()
            lineDataTable.AcceptChanges()
            Me.SetGridStyle()

            highestEntryId = _manager.GetHighestEntryIdForFamily

        Catch ex As Exception
            MessageBox.Show("An Error Has Occured" & vbCrLf & ex.Message & ex.StackTrace, "UFCW Accumulator Values", MessageBoxButtons.OK)

        End Try
    End Sub

    Public Sub SetFormInfo(ByVal ssn As String, ByVal relevantDate As Date)
        Try
            Dim famId As Integer, perId As Integer
            Dim dr As DataRow
            Dim dt As DataTable = ClaimDAL.RetrievePersonInfo(ssn.Replace("-"c, ""))
            If Not dt Is Nothing Then
                If dt.Rows.Count > 1 Then
                    Dim frm As New FamilyChooser
                    frm.FamilyRelationDataGrid.DataSource = dt
                    frm.SetStyleGrid()
                    frm.ShowDialog()
                    famId = frm.SelectFamilyId
                    perId = frm.SelectRelationId
                    FirstName = frm.SelectFirstname
                    LastName = frm.SelectLastname
                    Me.MemberAccumulatorManager = Nothing
                    If famId = -1 Then
                        MessageBox.Show("No valid Family ID was chose", "Family ID", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        frm.Close()
                        Return
                    End If
                    SetFormInfo(famId, perId, relevantDate, nonClaimId)
                    frm.Close()
                Else
                    dr = dt.Rows(0)
                    If Not dr Is Nothing Then
                        If Not dr("FAMILY_ID") Is System.DBNull.Value And Not dr("RELATION_ID") Is System.DBNull.Value Then
                            famId = dr("FAMILY_ID")
                            perId = dr("RELATION_ID")
                            FirstName = dr("FIRST_NAME")
                            LastName = dr("LAST_NAME")
                        End If
                        Me.MemberAccumulatorManager = Nothing
                        SetFormInfo(famId, perId, relevantDate, nonClaimId)
                    Else
                        Me.PersonalAccumulatorDataGrid.DataSource = Nothing
                        Me.PersonalAccumulatorPriorDataGrid.DataSource = Nothing
                        Me.FamilyAccumulatorsDataGrid.DataSource = Nothing
                        Me.FamilyAccumulatorsPriorDataGrid.DataSource = Nothing
                    End If
                End If
            Else
                Me.PersonalAccumulatorDataGrid.DataSource = Nothing
                Me.PersonalAccumulatorPriorDataGrid.DataSource = Nothing
                Me.FamilyAccumulatorsDataGrid.DataSource = Nothing
                Me.FamilyAccumulatorsPriorDataGrid.DataSource = Nothing
            End If
        Catch ex As Exception
            MessageBox.Show("An Error Has Occured" & vbCrLf & ex.Message & ex.StackTrace, "UFCW Accumulator Values", MessageBoxButtons.OK)
        End Try
    End Sub

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
    Public Sub FilterByLineNumber(ByVal lineNumber As Integer)

        If lineNumber = -1 Then
            CType(Me.LineDetailsDataGrid.DataSource, DataTable).DefaultView.RowFilter = ""
        Else
            CType(Me.LineDetailsDataGrid.DataSource, DataTable).DefaultView.RowFilter = "LINE_NBR = " & lineNumber
        End If
        CType(Me.LineDetailsDataGrid.DataSource, DataTable).DefaultView.Sort = "DISPLAY_ORDER ASC"
        'Me.LineDetailsDataGrid.DataSource 
        Me.LineDetailsDataGrid.Refresh()
    End Sub
#End Region

    Private Sub LineDetailsDataGrid_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles LineDetailsDataGrid.MouseDown
        Dim myGrid As DataGrid = CType(sender, DataGrid)
        Dim hti As System.Windows.Forms.DataGrid.HitTestInfo
        hti = myGrid.HitTest(e.X, e.Y)

        If hti.Type = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader Then
            Return
        End If

    End Sub

    Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        _manager = Nothing '.Dispose()
        RaiseEvent Closing()
        Me.Dispose()
    End Sub

    Private Sub SaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveButton.Click
        Save()
    End Sub
    Public Sub Save()
        Dim dt As DataTable
        Dim applyDate As Date
        Dim val As Double
        Dim org As Double
        Dim nw As Double
        Try
            If highestEntryId <> _manager.GetHighestEntryIdForFamily Then
                MessageBox.Show("Changes have occured since you retrieved the Accumulators.\n  The " & _
                " Accumulators will now be refreshed to their current values.\n  Please make changes as neccessary.", "Changes Occured", MessageBoxButtons.OK, MessageBoxIcon.Information)
                SetFormInfo(_familyId, _relationId, New Date(Now.Year, 12, 31), -5)
                Return
            End If
            If Not PersonalAccumulatorDataGrid.DataSource Is Nothing Then
                dt = PersonalAccumulatorDataGrid.DataSource
                For i As Integer = 0 To dt.Rows.Count - 1
                    If dt.Rows(i).RowState <> DataRowState.Unchanged Then
                        nw = dt.Rows(i)("ACCUM_VALUE", DataRowVersion.Current)
                        org = dt.Rows(i)("ACCUM_VALUE", DataRowVersion.Original)
                        val = nw - org
                        applyDate = New Date(Now.Year, 1, 1)
                        _manager.InsertEntry(AccumulatorController.GetAccumulatorId(dt.Rows(i)("ACCUM_NAME")), applyDate, val, True, _userId)
                    End If
                Next
            End If
            If Not PersonalAccumulatorPriorDataGrid.DataSource Is Nothing Then
                dt = PersonalAccumulatorPriorDataGrid.DataSource
                For i As Integer = 0 To dt.Rows.Count - 1
                    If dt.Rows(i).RowState <> DataRowState.Unchanged Then
                        nw = dt.Rows(i)("ACCUM_VALUE", DataRowVersion.Current)
                        org = dt.Rows(i)("ACCUM_VALUE", DataRowVersion.Original)
                        val = nw - org
                        applyDate = New Date(Now.Year - 1, 1, 1)
                        _manager.InsertEntry(AccumulatorController.GetAccumulatorId(dt.Rows(i)("ACCUM_NAME")), applyDate, val, True, UserId)
                    End If
                Next
            End If
            If Not FamilyAccumulatorsDataGrid.DataSource Is Nothing Then
                dt = FamilyAccumulatorsDataGrid.DataSource
                For i As Integer = 0 To dt.Rows.Count - 1
                    If dt.Rows(i).RowState <> DataRowState.Unchanged Then
                        nw = dt.Rows(i)("ACCUM_VALUE", DataRowVersion.Current)
                        org = dt.Rows(i)("ACCUM_VALUE", DataRowVersion.Original)
                        val = nw - org
                        applyDate = New Date(Now.Year, 1, 1)
                        _manager.InsertEntry(AccumulatorController.GetAccumulatorId(dt.Rows(i)("ACCUM_NAME")), applyDate, val, True, UserId)
                    End If
                Next
            End If
            If Not FamilyAccumulatorsPriorDataGrid.DataSource Is Nothing Then
                dt = FamilyAccumulatorsPriorDataGrid.DataSource
                For i As Integer = 0 To dt.Rows.Count - 1
                    If dt.Rows(i).RowState <> DataRowState.Unchanged Then
                        nw = dt.Rows(i)("ACCUM_VALUE", DataRowVersion.Current)
                        org = dt.Rows(i)("ACCUM_VALUE", DataRowVersion.Original)
                        val = nw - org
                        applyDate = New Date(Now.Year - 1, 1, 1)
                        _manager.InsertEntry(AccumulatorController.GetAccumulatorId(dt.Rows(i)("ACCUM_NAME")), applyDate, val, True, UserId)
                    End If
                Next
            End If
            If Not _manager Is Nothing Then
                If _manager.GetProposedLifetimeValue(AccumulatorController.GetAccumulatorId("FIXAC")) = 1 Then
                    _manager.InsertEntry(AccumulatorController.GetAccumulatorId("FIXAC"), Now, 2, True, SystemInformation.UserName)
                End If

                _manager.CommitAll()
                _manager.RefreshAccumulatorSummariesForMember()
                dt = PersonalAccumulatorDataGrid.DataSource
                dt.AcceptChanges()
                dt = PersonalAccumulatorPriorDataGrid.DataSource
                dt.AcceptChanges()
                dt = FamilyAccumulatorsDataGrid.DataSource
                dt.AcceptChanges()
                dt = FamilyAccumulatorsPriorDataGrid.DataSource
                dt.AcceptChanges()
            End If
        Catch ex As Exception
            dt = PersonalAccumulatorDataGrid.DataSource
            dt.RejectChanges()
            PersonalAccumulatorDataGrid.DataSource = dt
            dt = PersonalAccumulatorPriorDataGrid.DataSource
            dt.RejectChanges()
            PersonalAccumulatorPriorDataGrid.DataSource = dt
            dt = FamilyAccumulatorsDataGrid.DataSource
            dt.RejectChanges()
            FamilyAccumulatorsDataGrid.DataSource = dt
            dt = FamilyAccumulatorsPriorDataGrid.DataSource
            dt.RejectChanges()
            FamilyAccumulatorsPriorDataGrid.DataSource = dt

            MessageBox.Show("An Error Has Occured" & vbCrLf & ex.Message & ex.StackTrace, "Error Committing Change(s)", MessageBoxButtons.OK)

        End Try
    End Sub
    'Private Sub IsAccumulatorRollOver(ByVal familyId As Integer, ByVal accumId As Integer)
    '    'If conds Is Nothing Then
    '    '    conds = PlanController.GetPlanConditionsByFamily(
    '    '    'conds.
    '    '    Dim dt = conds.GetAccumulatorValues(personId, familyId, relevantDate, False)
    '    'End If
    '    'CType(conds(0), Condition)()

    'End Sub
    Private Sub LoadData()
        If AccumulatorListSummary Is Nothing Then
            Dim dt As DataTable = AccumulatorController.GetAccumulators()
            AccumulatorListSummary.Items.Clear()
            For i As Integer = 0 To dt.Rows.Count - 1
                AccumulatorListSummary.Items.Add(dt.Rows(i)("ACCUM_NAME"))
            Next
            SetGridStyle()
            Me.GetLatest.Enabled = False
            loading = True
        End If
        loaded = True
    End Sub

    Public Function HasChanges() As Boolean
        Dim dt As DataTable
        If Not PersonalAccumulatorDataGrid Is Nothing Then
            If Not PersonalAccumulatorDataGrid.DataSource Is Nothing Then
                dt = PersonalAccumulatorDataGrid.DataSource
                If Not dt Is Nothing Then
                    If Not dt.GetChanges Is Nothing Then
                        If Not dt.GetChanges.Rows Is Nothing Then
                            If dt.GetChanges.Rows.Count > 0 Then Return True
                        End If
                    End If
                End If
            End If
        End If
        If Not PersonalAccumulatorPriorDataGrid Is Nothing Then
            If Not PersonalAccumulatorPriorDataGrid.DataSource Is Nothing Then
                dt = PersonalAccumulatorPriorDataGrid.DataSource
                If Not dt Is Nothing Then
                    If Not dt.GetChanges Is Nothing Then
                        If Not dt.GetChanges.Rows Is Nothing Then
                            If dt.GetChanges.Rows.Count > 0 Then Return True
                        End If
                    End If
                End If
            End If
        End If

        If Not FamilyAccumulatorsDataGrid Is Nothing Then
            If Not FamilyAccumulatorsDataGrid.DataSource Is Nothing Then
                dt = FamilyAccumulatorsDataGrid.DataSource
                If Not dt Is Nothing Then
                    If Not dt.GetChanges Is Nothing Then
                        If Not dt.GetChanges.Rows Is Nothing Then
                            If dt.GetChanges.Rows.Count > 0 Then Return True
                        End If
                    End If
                End If
            End If
        End If
        If Not FamilyAccumulatorsPriorDataGrid Is Nothing Then
            If Not FamilyAccumulatorsPriorDataGrid.DataSource Is Nothing Then
                dt = FamilyAccumulatorsPriorDataGrid.DataSource
                If Not dt Is Nothing Then
                    If Not dt.GetChanges Is Nothing Then
                        If Not dt.GetChanges.Rows Is Nothing Then
                            If dt.GetChanges.Rows.Count > 0 Then Return True
                        End If
                    End If
                End If
            End If
        End If
        Return False
    End Function

    Private Sub SaveAndCloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAndCloseButton.Click
        Save()
        _manager = Nothing
        RaiseEvent Closing()
    End Sub
    Public Sub VisibleAllControls()
        Me.FamilyAccumulatorsDataGrid.Visible = True
        Me.FamilyAccumulatorsLabel.Visible = True
        Me.FamilyAccumulatorsPriorDataGrid.Visible = True
        Me.Label2.Visible = True
        Me.PersonalAccumulatorLabel.Visible = True
        Me.PersonalAccumulatorDataGrid.Visible = True
        Me.PersonalAccumulatorPriorDataGrid.Visible = True
        Me.Label1.Visible = True
    End Sub
    Public Sub InVisibleAllControls()
        Me.FamilyAccumulatorsDataGrid.Visible = False
        Me.FamilyAccumulatorsLabel.Visible = False
        Me.FamilyAccumulatorsPriorDataGrid.Visible = False
        Me.Label1.Visible = False
        Me.PersonalAccumulatorLabel.Visible = False
        Me.PersonalAccumulatorDataGrid.Visible = False
        Me.PersonalAccumulatorPriorDataGrid.Visible = False
        Me.Label2.Visible = False
        Me.LineDetailsLabel.Visible = False
    End Sub
End Class
