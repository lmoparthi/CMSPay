Option Infer On


Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Configuration
Imports System.Threading.Tasks

Public Class ReversalForm
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

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
    Friend WithEvents OkButton As System.Windows.Forms.Button
    Friend WithEvents LineDetailDataGrid As DataGridCustom
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents AccumulatorValues As AccumulatorValues
    Friend WithEvents TotalGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents TotalHRAAppliedTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TotalPaidTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label69 As System.Windows.Forms.Label
    Friend WithEvents TotalAllowedTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label68 As System.Windows.Forms.Label
    Friend WithEvents TotalChargesTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label67 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ProviderTaxID As System.Windows.Forms.TextBox
    Friend WithEvents ReverseButton As System.Windows.Forms.Button
    Friend WithEvents TheCancelButton As System.Windows.Forms.Button
    Friend WithEvents HraActivityControl1 As HRAActivityControl
    Friend WithEvents CommentsRichTextBox As System.Windows.Forms.RichTextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.LineDetailDataGrid = New DataGridCustom()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.TotalGroupBox = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ProviderTaxID = New System.Windows.Forms.TextBox()
        Me.TotalHRAAppliedTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TotalPaidTextBox = New System.Windows.Forms.TextBox()
        Me.Label69 = New System.Windows.Forms.Label()
        Me.TotalAllowedTextBox = New System.Windows.Forms.TextBox()
        Me.Label68 = New System.Windows.Forms.Label()
        Me.TotalChargesTextBox = New System.Windows.Forms.TextBox()
        Me.Label67 = New System.Windows.Forms.Label()
        Me.ReverseButton = New System.Windows.Forms.Button()
        Me.TheCancelButton = New System.Windows.Forms.Button()
        Me.CommentsRichTextBox = New System.Windows.Forms.RichTextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.HraActivityControl1 = New HRAActivityControl()
        Me.AccumulatorValues = New AccumulatorValues()
        CType(Me.LineDetailDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.TotalGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'OkButton
        '
        Me.OkButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.OkButton.Location = New System.Drawing.Point(0, 0)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 6
        '
        'LineDetailDataGrid
        '
        Me.LineDetailDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.LineDetailDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.LineDetailDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineDetailDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineDetailDataGrid.ADGroupsThatCanFind = ""
        Me.LineDetailDataGrid.ADGroupsThatCanMultiSort = ""
        Me.LineDetailDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LineDetailDataGrid.AllowAutoSize = True
        Me.LineDetailDataGrid.AllowColumnReorder = True
        Me.LineDetailDataGrid.AllowCopy = True
        Me.LineDetailDataGrid.AllowCustomize = True
        Me.LineDetailDataGrid.AllowDelete = False
        Me.LineDetailDataGrid.AllowDragDrop = False
        Me.LineDetailDataGrid.AllowEdit = True
        Me.LineDetailDataGrid.AllowExport = True
        Me.LineDetailDataGrid.AllowFilter = True
        Me.LineDetailDataGrid.AllowFind = True
        Me.LineDetailDataGrid.AllowGoTo = True
        Me.LineDetailDataGrid.AllowMultiSelect = True
        Me.LineDetailDataGrid.AllowMultiSort = False
        Me.LineDetailDataGrid.AllowNew = True
        Me.LineDetailDataGrid.AllowPrint = True
        Me.LineDetailDataGrid.AllowRefresh = False
        Me.LineDetailDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineDetailDataGrid.AppKey = "UFCW\Claims\"
        Me.LineDetailDataGrid.AutoSaveCols = True
        Me.LineDetailDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.LineDetailDataGrid.ColumnHeaderLabel = Nothing
        Me.LineDetailDataGrid.ColumnRePositioning = False
        Me.LineDetailDataGrid.ColumnResizing = False
        Me.LineDetailDataGrid.ConfirmDelete = True
        Me.LineDetailDataGrid.CopySelectedOnly = True
        Me.LineDetailDataGrid.CurrentBSPosition = -1
        Me.LineDetailDataGrid.DataMember = ""
        Me.LineDetailDataGrid.DragColumn = 0
        Me.LineDetailDataGrid.ExportSelectedOnly = True
        Me.LineDetailDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.LineDetailDataGrid.HighlightedRow = Nothing
        Me.LineDetailDataGrid.HighLightModifiedRows = False
        Me.LineDetailDataGrid.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.LineDetailDataGrid.IsMouseDown = False
        Me.LineDetailDataGrid.LastGoToLine = ""
        Me.LineDetailDataGrid.Location = New System.Drawing.Point(0, 34)
        Me.LineDetailDataGrid.MultiSort = False
        Me.LineDetailDataGrid.Name = "LineDetailDataGrid"
        Me.LineDetailDataGrid.OldSelectedRow = Nothing
        Me.LineDetailDataGrid.PreviousBSPosition = -1
        Me.LineDetailDataGrid.ReadOnly = True
        Me.LineDetailDataGrid.RetainRowSelectionAfterSort = True
        Me.LineDetailDataGrid.SetRowOnRightClick = True
        Me.LineDetailDataGrid.ShiftPressed = False
        Me.LineDetailDataGrid.SingleClickBooleanColumns = True
        Me.LineDetailDataGrid.Size = New System.Drawing.Size(661, 156)
        Me.LineDetailDataGrid.Sort = Nothing
        Me.LineDetailDataGrid.StyleName = ""
        Me.LineDetailDataGrid.SubKey = ""
        Me.LineDetailDataGrid.SuppressMouseDown = False
        Me.LineDetailDataGrid.SuppressTriangle = False
        Me.LineDetailDataGrid.TabIndex = 2
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.LineDetailDataGrid)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.AccumulatorValues)
        Me.SplitContainer1.Size = New System.Drawing.Size(664, 588)
        Me.SplitContainer1.SplitterDistance = 193
        Me.SplitContainer1.SplitterWidth = 8
        Me.SplitContainer1.TabIndex = 7
        '
        'TotalGroupBox
        '
        Me.TotalGroupBox.Controls.Add(Me.Label2)
        Me.TotalGroupBox.Controls.Add(Me.ProviderTaxID)
        Me.TotalGroupBox.Controls.Add(Me.TotalHRAAppliedTextBox)
        Me.TotalGroupBox.Controls.Add(Me.Label1)
        Me.TotalGroupBox.Controls.Add(Me.TotalPaidTextBox)
        Me.TotalGroupBox.Controls.Add(Me.Label69)
        Me.TotalGroupBox.Controls.Add(Me.TotalAllowedTextBox)
        Me.TotalGroupBox.Controls.Add(Me.Label68)
        Me.TotalGroupBox.Controls.Add(Me.TotalChargesTextBox)
        Me.TotalGroupBox.Controls.Add(Me.Label67)
        Me.TotalGroupBox.Location = New System.Drawing.Point(0, 0)
        Me.TotalGroupBox.Name = "TotalGroupBox"
        Me.TotalGroupBox.Size = New System.Drawing.Size(664, 32)
        Me.TotalGroupBox.TabIndex = 8
        Me.TotalGroupBox.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(546, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(36, 13)
        Me.Label2.TabIndex = 99
        Me.Label2.Text = "TaxID"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProviderTaxID
        '
        Me.ProviderTaxID.BackColor = System.Drawing.SystemColors.Window
        Me.ProviderTaxID.Location = New System.Drawing.Point(588, 9)
        Me.ProviderTaxID.Name = "ProviderTaxID"
        Me.ProviderTaxID.ReadOnly = True
        Me.ProviderTaxID.Size = New System.Drawing.Size(70, 20)
        Me.ProviderTaxID.TabIndex = 98
        Me.ProviderTaxID.TabStop = False
        '
        'TotalHRAAppliedTextBox
        '
        Me.TotalHRAAppliedTextBox.BackColor = System.Drawing.SystemColors.Window
        Me.TotalHRAAppliedTextBox.Location = New System.Drawing.Point(458, 8)
        Me.TotalHRAAppliedTextBox.Name = "TotalHRAAppliedTextBox"
        Me.TotalHRAAppliedTextBox.ReadOnly = True
        Me.TotalHRAAppliedTextBox.Size = New System.Drawing.Size(75, 20)
        Me.TotalHRAAppliedTextBox.TabIndex = 96
        Me.TotalHRAAppliedTextBox.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(384, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 97
        Me.Label1.Text = "HRA Applied"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TotalPaidTextBox
        '
        Me.TotalPaidTextBox.BackColor = System.Drawing.SystemColors.Window
        Me.TotalPaidTextBox.Location = New System.Drawing.Point(303, 8)
        Me.TotalPaidTextBox.Name = "TotalPaidTextBox"
        Me.TotalPaidTextBox.ReadOnly = True
        Me.TotalPaidTextBox.Size = New System.Drawing.Size(75, 20)
        Me.TotalPaidTextBox.TabIndex = 92
        Me.TotalPaidTextBox.TabStop = False
        '
        'Label69
        '
        Me.Label69.AutoSize = True
        Me.Label69.Location = New System.Drawing.Point(269, 12)
        Me.Label69.Name = "Label69"
        Me.Label69.Size = New System.Drawing.Size(28, 13)
        Me.Label69.TabIndex = 95
        Me.Label69.Text = "Paid"
        Me.Label69.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TotalAllowedTextBox
        '
        Me.TotalAllowedTextBox.BackColor = System.Drawing.SystemColors.Window
        Me.TotalAllowedTextBox.Location = New System.Drawing.Point(188, 9)
        Me.TotalAllowedTextBox.Name = "TotalAllowedTextBox"
        Me.TotalAllowedTextBox.ReadOnly = True
        Me.TotalAllowedTextBox.Size = New System.Drawing.Size(75, 20)
        Me.TotalAllowedTextBox.TabIndex = 91
        Me.TotalAllowedTextBox.TabStop = False
        '
        'Label68
        '
        Me.Label68.AutoSize = True
        Me.Label68.Location = New System.Drawing.Point(138, 12)
        Me.Label68.Name = "Label68"
        Me.Label68.Size = New System.Drawing.Size(44, 13)
        Me.Label68.TabIndex = 94
        Me.Label68.Text = "Allowed"
        Me.Label68.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TotalChargesTextBox
        '
        Me.TotalChargesTextBox.BackColor = System.Drawing.SystemColors.Window
        Me.TotalChargesTextBox.Location = New System.Drawing.Point(57, 8)
        Me.TotalChargesTextBox.Name = "TotalChargesTextBox"
        Me.TotalChargesTextBox.ReadOnly = True
        Me.TotalChargesTextBox.Size = New System.Drawing.Size(75, 20)
        Me.TotalChargesTextBox.TabIndex = 90
        Me.TotalChargesTextBox.TabStop = False
        '
        'Label67
        '
        Me.Label67.AutoSize = True
        Me.Label67.Location = New System.Drawing.Point(3, 12)
        Me.Label67.Name = "Label67"
        Me.Label67.Size = New System.Drawing.Size(46, 13)
        Me.Label67.TabIndex = 93
        Me.Label67.Text = "Charges"
        Me.Label67.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ReverseButton
        '
        Me.ReverseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ReverseButton.Location = New System.Drawing.Point(565, 716)
        Me.ReverseButton.Name = "ReverseButton"
        Me.ReverseButton.Size = New System.Drawing.Size(75, 23)
        Me.ReverseButton.TabIndex = 9
        Me.ReverseButton.Text = "Reverse"
        Me.ReverseButton.UseVisualStyleBackColor = True
        '
        'TheCancelButton
        '
        Me.TheCancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TheCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.TheCancelButton.Location = New System.Drawing.Point(475, 716)
        Me.TheCancelButton.Name = "TheCancelButton"
        Me.TheCancelButton.Size = New System.Drawing.Size(75, 23)
        Me.TheCancelButton.TabIndex = 10
        Me.TheCancelButton.Text = "Cancel"
        Me.TheCancelButton.UseVisualStyleBackColor = True
        '
        'CommentsRichTextBox
        '
        Me.CommentsRichTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CommentsRichTextBox.DetectUrls = False
        Me.CommentsRichTextBox.Location = New System.Drawing.Point(428, 599)
        Me.CommentsRichTextBox.Name = "CommentsRichTextBox"
        Me.CommentsRichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
        Me.CommentsRichTextBox.Size = New System.Drawing.Size(230, 75)
        Me.CommentsRichTextBox.TabIndex = 12
        Me.CommentsRichTextBox.Text = ""
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(495, 582)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(104, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Reason for Reversal"
        '
        'HraActivityControl1
        '
        Me.HraActivityControl1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HraActivityControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.HraActivityControl1.Location = New System.Drawing.Point(12, 599)
        Me.HraActivityControl1.Name = "HraActivityControl1"
        Me.HraActivityControl1.Size = New System.Drawing.Size(409, 140)
        Me.HraActivityControl1.TabIndex = 11
        '
        'AccumulatorValues
        '
        Me.AccumulatorValues.AutoSize = True
        Me.AccumulatorValues.BackColor = System.Drawing.SystemColors.Control
        Me.AccumulatorValues.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AccumulatorValues.IsInEditMode = False
        Me.AccumulatorValues.Location = New System.Drawing.Point(0, 0)
        Me.AccumulatorValues.MinimumSize = New System.Drawing.Size(200, 150)
        Me.AccumulatorValues.Name = "AccumulatorValues"
        Me.AccumulatorValues.ShowClaimView = False
        Me.AccumulatorValues.ShowHistory = False
        Me.AccumulatorValues.ShowLineDetails = False
        Me.AccumulatorValues.Size = New System.Drawing.Size(664, 387)
        Me.AccumulatorValues.TabIndex = 1
        '
        'ReversalForm
        '
        Me.AcceptButton = Me.ReverseButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(668, 751)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.CommentsRichTextBox)
        Me.Controls.Add(Me.HraActivityControl1)
        Me.Controls.Add(Me.TheCancelButton)
        Me.Controls.Add(Me.ReverseButton)
        Me.Controls.Add(Me.TotalGroupBox)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.OkButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "ReversalForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Reversal Details"
        CType(Me.LineDetailDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.TotalGroupBox.ResumeLayout(False)
        Me.TotalGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"

    Private _FamilyID As Integer = -1
    Private _ClaimDate As Date
    Private _ClaimID As Integer
    Private _ClaimDetails As ClaimDataset.MEDDTLDataTable
    Private _ClaimReasons As ClaimDataset.REASONDataTable

    Private WithEvents boolColumn As New DataGridBoolColumn

    Dim _D As [Delegate]

    Private _HoveringOverCell As New DataGridCell

    <System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")> _
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)

            _APPKEY = Value
        End Set
    End Property

    Private Sub LineDetailsForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            Me.OkButton.PerformClick()
        End If
    End Sub

    Private Sub LineDetailsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        SetSettings()

        LineDetailDataGrid.ReadOnly = True
        Me.AccumulatorValues.ShowLineDetails = True
        Me.AccumulatorValues.IsInEditMode = False

        If UFCWGeneralAD.CMSLocals() Then
            With LineDetailDataGrid
                .AllowFind = False
                .AllowGoTo = False
                .AllowExport = False
                .AllowPrint = False
                .AllowCopy = False
            End With
        End If

        AddHandler Me.KeyUp, AddressOf LineDetailsForm_KeyUp

    End Sub

    Public Sub SetClaimID(ByVal claimID As Integer, duplicatesClaimDS As DataSet)

        Dim UISyncContext As TaskScheduler
        Dim RetrieveClaimReasonsTask As Task(Of DataSet)
        Dim RetrieveClaimReasonsCallBack As Task
        Dim RetrieveDetailWithRulesTask As Task(Of DataSet)
        Dim RetrieveDetailWithRulesCallBack As Task

        Try

            _ClaimID = claimID

            Me.Text = "Line Details - Claim # " & claimID

            UISyncContext = TaskScheduler.FromCurrentSynchronizationContext()

            RetrieveClaimReasonsTask = New Task(Of DataSet)(Function() CMSDALFDBMD.RetrieveClaimReasons(claimID))
            RetrieveClaimReasonsCallBack = RetrieveClaimReasonsTask.ContinueWith(Sub(t)


                                                                                     If _ClaimReasons Is Nothing Then
                                                                                         _ClaimReasons = New ClaimDataset.REASONDataTable
                                                                                         _ClaimReasons.Load(t.Result.Tables(0).CreateDataReader)
                                                                                     End If

                                                                                 End Sub)

            RetrieveClaimReasonsTask.Start()

            RetrieveDetailWithRulesTask = New Task(Of DataSet)(Function() CMSDALFDBMD.RetrieveDetailWithRules(claimID))
            RetrieveDetailWithRulesCallBack = RetrieveDetailWithRulesTask.ContinueWith(Sub(t)

                                                                                           If _ClaimDetails Is Nothing Then
                                                                                               _ClaimDetails = New ClaimDataset.MEDDTLDataTable
                                                                                               _ClaimDetails.Load(t.Result.Tables(0).CreateDataReader)
                                                                                           End If

                                                                                           SumCharges()
                                                                                           SumAllowed()
                                                                                           SumPaid()
                                                                                           SumHRAApplied()

                                                                                           LineDetailDataGrid.SuspendLayout()
                                                                                           LineDetailDataGrid.DataSource = _ClaimDetails
                                                                                           SetTableStyle(LineDetailDataGrid)
                                                                                           LineDetailDataGrid.ResumeLayout()

                                                                                           If _ClaimDetails.Rows.Count > 0 Then Me.ProviderTaxID.Text = _ClaimDetails(0)("PROV_TIN").ToString

                                                                                       End Sub)

            RetrieveDetailWithRulesTask.Start()

            Task.WaitAll(New Task() {RetrieveClaimReasonsCallBack, RetrieveDetailWithRulesCallBack}, New TimeSpan(0, 0, 1, 0, 0))

        Catch ex As Exception
            Throw
        Finally
#If DEBUG Then
            Debug.Print("SetClaimID Finished")
#End If
        End Try

    End Sub

    Public Sub DisplayLineItemsByPatient(ByVal dr As DataRow)

        Dim CM As CurrencyManager
        Dim DGDRV As DataRowView
        Dim DGRow As DataRow

        Try
            If LineDetailDataGrid.GetGridRowCount > 0 Then
                CM = DirectCast(LineDetailDataGrid.BindingContext(LineDetailDataGrid.DataSource, LineDetailDataGrid.DataMember), CurrencyManager)
                DGDRV = DirectCast(CM.Current, DataRowView)
                DGRow = DGDRV.Row
            End If

            Me.AccumulatorValues.SetFormInfo(CInt(dr("FAMILY_ID")), CShort(dr("RELATION_ID")), UFCWGeneral.NowDate, CInt(dr("CLAIM_ID")))

            If DGRow IsNot Nothing Then
                Me.AccumulatorValues.FilterByLineNumber(CShort(DGRow("LINE_NBR")))
            Else
                Me.AccumulatorValues.ErrorLabel.Text = "Cannot show details for this claim. There is no Date of Service"
                Me.AccumulatorValues.ErrorLabel.ForeColor = Color.Black
                Me.AccumulatorValues.BringToFront()
                Me.AccumulatorValues.LineDetailsDataGrid.SendToBack()
                Me.AccumulatorValues.ErrorLabel.Visible = True
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Public Sub DisplayLineItemsByClaim(ByVal claimID As Integer, duplicatesClaimDS As DataSet)

        Dim CM As CurrencyManager
        Dim DGDRV As DataRowView
        Dim DGRow As DataRow
        Dim FamilyID As Integer
        Dim RelationID As Short
        Dim DateOfService As Date

        Try
            If LineDetailDataGrid.GetGridRowCount > 0 Then
                CM = DirectCast(LineDetailDataGrid.BindingContext(LineDetailDataGrid.DataSource, LineDetailDataGrid.DataMember), CurrencyManager)
                DGDRV = DirectCast(CM.Current, DataRowView)
                DGRow = DGDRV.Row
            End If

            Me.AccumulatorValues.SetFormInfo(FamilyID, RelationID, DateOfService, claimID)

            If DGRow IsNot Nothing Then
                Me.AccumulatorValues.FilterByLineNumber(CShort(DGRow("LINE_NBR")))
            Else
                Me.AccumulatorValues.ErrorLabel.Text = "Cannot show details for this claim. There are no line items"
                Me.AccumulatorValues.ErrorLabel.ForeColor = Color.Black
                Me.AccumulatorValues.BringToFront()
                Me.AccumulatorValues.LineDetailsDataGrid.SendToBack()
                Me.AccumulatorValues.ErrorLabel.Visible = True
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub OkButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkButton.Click
        Me.Close()
    End Sub

    Private Sub SetTableStyle(ByVal dg As DataGridCustom)

        Try

            SetTableStyleColumns(dg)
            dg.StyleName = dg.Name
            dg.AppKey = _APPKEY
            dg.ContextMenuPrepare()

            RemoveHandler dg.ResetTableStyle, AddressOf TableStyleReset
            AddHandler dg.ResetTableStyle, AddressOf TableStyleReset

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub
    Private Sub TableStyleReset(ByVal sender As Object, e As EventArgs)
        Dim dg As DataGridCustom = CType(sender, DataGridCustom)
        SetTableStyleColumns(dg)
    End Sub

    Private Sub SetTableStyleColumns(ByVal dg As DataGridCustom)

        Dim DGTS As DataGridTableStyle
        Dim DGTSDefault As DataGridTableStyle

        Dim TextCol As DataGridFormattableTextBoxColumn
        Dim ButtCol As DataGridHighlightButtonColumn
        Dim BoolCol As DataGridHighlightBoolColumn

        Dim IntCol As Integer
        Dim cols As DataView
        Dim DefaultStyleDS As DataSet

        Try
            Try

                Dim XMLStyleName As String = CStr(CType(ConfigurationManager.GetSection(dg.Name), IDictionary)("StyleLocation"))
                DefaultStyleDS = CMSXMLHandler.CreateDataSetFromXML(XMLStyleName)
            Catch ex As System.NullReferenceException
                MessageBox.Show("Please check < " & dg.Name & ".xml > entry in Config files ", "Missing Entry", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Catch ex As System.IO.FileNotFoundException
                MessageBox.Show("Please check < " & dg.Name & ".xml > entry in Config files ", "Missing Entry", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Catch ex As Exception
                Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                If (Rethrow) Then
                    Throw
                Else
                    MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Try

            DGTS = New DataGridTableStyle()
            DGTS.MappingName = dg.GetCurrentDataTable.TableName

            If DefaultStyleDS.Tables.Contains(dg.Name & "Style") Then
                If DefaultStyleDS.Tables(dg.Name & "Style").Columns.Contains("GridLineStyle") Then
                    DGTS.GridLineStyle = If(CBool(DefaultStyleDS.Tables(dg.Name & "Style").Rows(0)("GridLineStyle")), DataGridLineStyle.Solid, DataGridLineStyle.None)
                End If
            End If

            Dim ResultDT As DataTable = dg.LoadColumnsSizeAndPosition(dg.Name & "\" & DGTS.MappingName & "\ColumnSettings", DefaultStyleDS.Tables("Column"))

            Dim ColumnSequenceDV As New DataView(ResultDT)
            cols = ColumnSequenceDV

            DGTSDefault = New DataGridTableStyle() 'This can be used to establish the columns displayed by default
            DGTSDefault.MappingName = "Default"

            For IntCol = 0 To cols.Count - 1

                If UFCWGeneralAD.CMSLocals() OrElse Not UFCWGeneralAD.CMSCanReprocess Then
                    Select Case cols(IntCol).Item("Mapping").ToString
                        Case "RULE_SET_NAME", "PUBLISH_BATCH_NBR"
                            Continue For
                    End Select
                End If

                If (IsDBNull(cols(IntCol).Item("Visible")) OrElse cols(IntCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(cols(IntCol).Item("Visible"))) Then
                    TextCol = New DataGridFormattableTextBoxColumn
                    TextCol.MappingName = CStr(cols(IntCol).Item("Mapping"))
                    TextCol.HeaderText = CStr(cols(IntCol).Item("HeaderText"))
                    If IsDBNull(cols(IntCol).Item("Format")) = False AndAlso cols(IntCol).Item("Format").ToString.Trim.Length > 0 Then
                        TextCol.Format = CStr(cols(IntCol).Item("Format"))
                    End If
                    DGTSDefault.GridColumnStyles.Add(TextCol)
                End If

                If ((IsDBNull(cols(IntCol).Item("Visible")) OrElse cols(IntCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(cols(IntCol).Item("Visible"))) AndAlso (GetAllSettings(_APPKEY, dg.Name & "\" & DGTS.MappingName.ToString & "\Customize\ColumnSelector") Is Nothing OrElse CDbl(GetSetting(_APPKEY, dg.Name & "\" & DGTS.MappingName.ToString & "\Customize\ColumnSelector", "Col " & cols(IntCol).Item("Mapping").ToString & " Customize", "0")) = 1)) Then

                    If cols(IntCol).Item("Type").ToString.Trim = "Text" Then
                        TextCol = New DataGridFormattableTextBoxColumn
                        TextCol.MappingName = CStr(cols(IntCol).Item("Mapping"))
                        TextCol.HeaderText = CStr(cols(IntCol).Item("HeaderText"))
                        TextCol.Width = CInt(GetSetting(_APPKEY, dg.Name & "\" & DGTS.MappingName.ToString & "\ColumnSettings", "Col " & TextCol.MappingName, CStr(UFCWGeneral.MeasureWidthinPixels(CInt(cols(IntCol).Item("DefaultCharWidth")), dg.Font.Name, dg.Font.Size))))
                        TextCol.NullText = CStr(cols(IntCol).Item("NullText"))
                        If IsDBNull(cols(IntCol).Item("Format")) = False AndAlso cols(IntCol).Item("Format").ToString.Trim.Length > 0 Then
                            TextCol.Format = CStr(cols(IntCol).Item("Format"))
                        End If
                        DGTS.GridColumnStyles.Add(TextCol)
                    ElseIf cols(IntCol).Item("Type").ToString.Trim = "Bool" Then
                        BoolCol = New DataGridHighlightBoolColumn
                        BoolCol.MappingName = CStr(cols(IntCol).Item("Mapping"))
                        BoolCol.HeaderText = CStr(cols(IntCol).Item("HeaderText"))
                        BoolCol.Width = CInt(GetSetting(_APPKEY, dg.Name & "\" & DGTS.MappingName.ToString & "\ColumnSettings", "Col " & TextCol.MappingName, CStr(UFCWGeneral.MeasureWidthinPixels(CInt(cols(IntCol).Item("DefaultCharWidth")), dg.Font.Name, dg.Font.Size))))
                        BoolCol.NullValue = False ' CType("0", Decimal) 'cols(intCol).Item("NullValue")
                        BoolCol.TrueValue = True 'CType("1", Decimal) 'cols(intCol).Item("TrueValue")
                        BoolCol.FalseValue = False 'CType("0", Decimal) 'cols(intCol).Item("FalseValue")
                        BoolCol.AllowNull = False
                        DGTS.GridColumnStyles.Add(BoolCol)
                    ElseIf cols(IntCol).Item("Type").ToString.Trim = "Button" Then
                        ButtCol = New DataGridHighlightButtonColumn(LineDetailDataGrid)
                        ButtCol.MappingName = CStr(cols(IntCol).Item("Mapping"))
                        ButtCol.HeaderText = CStr(cols(IntCol).Item("HeaderText"))
                        ButtCol.Width = CInt(GetSetting(_APPKEY, dg.Name & "\" & DGTS.MappingName.ToString & "\ColumnSettings", "Col " & TextCol.MappingName, CStr(UFCWGeneral.MeasureWidthinPixels(CInt(cols(IntCol).Item("DefaultCharWidth")), dg.Font.Name, dg.Font.Size))))
                        ButtCol.NullText = CStr(cols(IntCol).Item("NullText"))

                        _D = [Delegate].CreateDelegate(GetType(System.EventHandler), Me, cols(IntCol).Item("Method").ToString)
                        AddHandler ButtCol.ColumnButton.Click, CType(_D, Global.System.EventHandler)

                        If CStr(cols(IntCol).Item("Mapping")).ToUpper = "REASON_SW" Then
                            AddHandler ButtCol.Formatting, AddressOf FormattingReason
                            AddHandler ButtCol.UnFormatting, AddressOf UnFormattingReason
                        End If

                        DGTS.PreferredRowHeight = ButtCol.ColumnButton.Height + 2
                        DGTS.GridColumnStyles.Add(ButtCol)
                    End If
                End If

            Next

            dg.TableStyles.Clear()
            dg.TableStyles.Add(DGTS)
            dg.TableStyles.Add(DGTSDefault)

        Catch ex As Exception
            Throw
        Finally

            DGTS = Nothing
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
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Saves the basic form settings.  Windowstate, height, width, top, and left.
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	11/16/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim TheWindowState As FormWindowState = Me.WindowState
        SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(TheWindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = TheWindowState


    End Sub

    Private Sub LineDetailsForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        SaveSettings()
        If _ClaimDetails IsNot Nothing Then _ClaimDetails.Dispose()
        If _ClaimReasons IsNot Nothing Then _ClaimReasons.Dispose()

        If LineDetailDataGrid IsNot Nothing Then
            LineDetailDataGrid.TableStyles.Clear()
            LineDetailDataGrid.Dispose()
            LineDetailDataGrid.DataSource = Nothing
        End If

        If Me.AccumulatorValues IsNot Nothing Then Me.AccumulatorValues.Dispose()
        Me.AccumulatorValues = Nothing
    End Sub
    Private Sub LineDetailDataGrid_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles LineDetailDataGrid.MouseMove
        Dim HTI As DataGrid.HitTestInfo
        Dim ToolTipDR As DataRow
        Dim ToolTipText As String = ""
        Dim ProcedureDRs As DataRow()
        Dim BillTypeDRs As DataRow()
        Dim POSDRs As DataRow()
        Dim Reasons As String()
        Dim Modifiers As String()
        Dim PricingReasons As String()
        Dim Diagnoses As String()

        Try
            HTI = CType(sender, DataGridCustom).HitTest(e.X, e.Y)

            ' Do not display hover text if it is a drag event
            If (e.Button <> MouseButtons.Left) Then

                ' Check if the target is a different cell from the previous one
                If HTI.Type = DataGrid.HitTestType.Cell AndAlso _
                  HTI.Row <> _HoveringOverCell.RowNumber OrElse HTI.Column <> _HoveringOverCell.ColumnNumber Then

                    ' Store the new hit row
                    _HoveringOverCell.RowNumber = HTI.Row
                    _HoveringOverCell.ColumnNumber = HTI.Column

                End If

            End If

            If _HoveringOverCell.RowNumber > -1 AndAlso _HoveringOverCell.RowNumber <= (CType(sender, DataGridCustom).GetGridRowCount) Then

                ToolTipText = ""

                If LineDetailDataGrid.GetColumnMapping(_HoveringOverCell.ColumnNumber) = "PROC_CODE" Then ''If hoverCell.ColumnNumber = 3 Then
                    ProcedureDRs = CustomerServiceControl.ProcedureListOfValues.Select("PROC_VALUE = '" & LineDetailDataGrid.Item(_HoveringOverCell).ToString & "'")

                    If ProcedureDRs IsNot Nothing AndAlso ProcedureDRs.Length > 0 Then
                        ToolTipText = CStr(ProcedureDRs(0).Item("FULL_DESC"))
                    End If

                ElseIf LineDetailDataGrid.GetColumnMapping(_HoveringOverCell.ColumnNumber) = "REV_CODE" Then
                    ProcedureDRs = CustomerServiceControl.ProcedureListOfValues.Select("PROC_VALUE = '" & LineDetailDataGrid.Item(_HoveringOverCell).ToString & "'")

                    If ProcedureDRs IsNot Nothing AndAlso ProcedureDRs.Length > 0 Then
                        ToolTipText = CStr(ProcedureDRs(0).Item("FULL_DESC"))
                    End If

                ElseIf LineDetailDataGrid.GetColumnMapping(_HoveringOverCell.ColumnNumber) = "BILL_TYPE" Then
                    BillTypeDRs = CustomerServiceControl.BillTypeListOfValues.Select("BILL_TYPE_VALUE = '" & LineDetailDataGrid.Item(_HoveringOverCell).ToString & "'")

                    If BillTypeDRs IsNot Nothing AndAlso BillTypeDRs.Length > 0 Then
                        ToolTipText = CStr(BillTypeDRs(0).Item("FULL_DESC"))
                    End If

                ElseIf LineDetailDataGrid.GetColumnMapping(_HoveringOverCell.ColumnNumber) = "PLACE_OF_SERV" Then
                    POSDRs = CustomerServiceControl.POSListOfValues.Select("PLACE_OF_SERV_VALUE = '" & LineDetailDataGrid.Item(_HoveringOverCell).ToString & "'")

                    If POSDRs IsNot Nothing AndAlso POSDRs.Length > 0 Then
                        ToolTipText = CStr(POSDRs(0).Item("FULL_DESC"))
                    End If

                ElseIf LineDetailDataGrid.GetColumnMapping(_HoveringOverCell.ColumnNumber) = "REASON_SW" Then

                    Reasons = CType(sender, DataGridCustom).Item(_HoveringOverCell).ToString.Split(New Char() {CChar(","), CChar(" "), CChar("|")})

                    For Each Reason As String In Reasons
                        If Reason.Trim.Length > 0 Then
                            ToolTipDR = CMSDALFDBMD.RetrieveReasonValuesInformation(Reason.Trim, CType(CType(sender, DataGridCustom).GetTableRowFromGridPosition(_HoveringOverCell.RowNumber)("OCC_FROM_DATE"), Date?))
                            If ToolTipDR IsNot Nothing Then
                                ToolTipText &= ToolTipDR.Item("DESCRIPTION").ToString & Environment.NewLine
                            End If
                        End If
                    Next

                ElseIf LineDetailDataGrid.GetColumnMapping(_HoveringOverCell.ColumnNumber) = "MODIFIER" Then

                    Modifiers = CType(sender, DataGridCustom).Item(_HoveringOverCell).ToString.Split(New Char() {CChar(","), CChar(" "), CChar("|")})

                    For Each Modifier As String In Modifiers
                        If Modifier.Trim.Length > 0 Then
                            ToolTipDR = CMSDALFDBMD.RetrieveModifierValuesInformation(Modifier.Trim)
                            If ToolTipDR IsNot Nothing Then
                                ToolTipText &= ToolTipDR.Item("FULL_DESC").ToString & Environment.NewLine
                            End If
                        End If
                    Next

                ElseIf LineDetailDataGrid.GetColumnMapping(_HoveringOverCell.ColumnNumber) = "PRICING_REASON" Then

                    PricingReasons = CType(sender, DataGridCustom).Item(_HoveringOverCell).ToString.Split(New Char() {CChar(","), CChar(" "), CChar("|")})

                    For Each PricingReason As String In PricingReasons
                        If PricingReason.Trim.Length > 0 Then
                            ToolTipDR = CMSDALFDBMD.RetrieveBCDetailReasonCodeByCode(CStr(If(PricingReason.Trim.Length < 4, PricingReason.Trim, PricingReason.Trim.Substring(0, 3)))) 'limit reason code to 3 characters (specifically for AB88 -> AB8)
                            If ToolTipDR IsNot Nothing Then
                                ToolTipText &= ToolTipDR.Item("DESCRIPTION").ToString & Environment.NewLine
                            End If
                        End If
                    Next

                ElseIf LineDetailDataGrid.GetColumnMapping(_HoveringOverCell.ColumnNumber) = "DIAGNOSIS" OrElse LineDetailDataGrid.GetColumnMapping(_HoveringOverCell.ColumnNumber) = "DIAGNOSES" Then ''If hoverCell.ColumnNumber = 7 Then

                    Diagnoses = CType(sender, DataGridCustom).Item(_HoveringOverCell).ToString.Split(New Char() {CChar(","), CChar(" "), CChar("|")})

                    For Each Diagnosis As String In Diagnoses
                        If Diagnosis.Trim.Length > 0 Then
                            ToolTipDR = CMSDALFDBMD.RetrieveDiagnosisValuesInformation(Diagnosis.Trim)
                            If ToolTipDR IsNot Nothing Then
                                ToolTipText &= ToolTipDR.Item("FULL_DESC").ToString & Environment.NewLine
                            End If
                        End If
                    Next

                End If
            End If

            If ToolTipText IsNot Nothing AndAlso ToolTipText.ToString.Trim.Length > 0 Then
                If String.Compare(Me.ToolTip1.GetToolTip(CType(sender, DataGridCustom)), ToolTipText.ToString) <> 0 OrElse ToolTip1.Active = False Then
                    ToolTip1.Active = True
                    ToolTip1.SetToolTip(CType(sender, DataGridCustom), ToolTipText.ToString)
                End If
            Else
                ToolTip1.SetToolTip(CType(sender, DataGridCustom), "")
            End If

        Catch ex As Exception
        Finally
        End Try
    End Sub
    Private Sub ReasonColumnButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim CM As CurrencyManager
        Dim DGDRV As DataRowView
        Dim DGRow As DataRow

        Try
            If LineDetailDataGrid.GetGridRowCount = 0 Then Return

            CM = DirectCast(LineDetailDataGrid.BindingContext(LineDetailDataGrid.DataSource, LineDetailDataGrid.DataMember), CurrencyManager)
            DGDRV = DirectCast(CM.Current, DataRowView)
            DGRow = DGDRV.Row

            ShowDetailLineReasons(CInt(DGRow("CLAIM_ID")), CShort(DGRow("LINE_NBR")))

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub ShowDetailLineReasons(ByVal claimId As Integer, ByVal detailLine As Short)

        Dim ReasonForm As LineReasonsForm
        Dim DR As DataRow
        Dim DV As DataView
        Dim DetailDV As DataView
        Dim AlertDV As DataView
        Dim ApplyStatus As String = ""

        Try

            RemoveHandler Me.KeyUp, AddressOf LineDetailsForm_KeyUp ' prevent escape key from being handled by parent form

            ReasonForm = New LineReasonsForm(claimId, detailLine, _ClaimReasons)

            If ReasonForm.ShowDialog(Me) = DialogResult.OK Then
                DV = New DataView(_ClaimReasons, "LINE_NBR = " & detailLine, "Line_NBR", DataViewRowState.CurrentRows)
                Do Until DV.Count = 0
                    DV(0).Row.Delete()
                Loop

                DV = New DataView(_ClaimDetails, "LINE_NBR = " & detailLine, "Line_NBR", DataViewRowState.CurrentRows)
                DetailDV = LineDetailDataGrid.GetDefaultDataView

                For Cnt As Integer = ReasonForm.ClaimDataset.REASON.Rows.Count - 1 To 0 Step -1
                    DR = ReasonForm.ClaimDataset.REASON.Rows(Cnt)

                    If DR.RowState <> DataRowState.Deleted Then
                        _ClaimReasons.Rows.Add(DR.ItemArray)

                        'DENY overrides all; if already deny leave deny
                        If IsDBNull(DR("APPLY_STATUS")) = False AndAlso ApplyStatus <> "DENY" Then
                            ApplyStatus = CStr(DR("APPLY_STATUS")).ToUpper
                        End If

                    End If
                Next

                If DV.Count > 0 Then

                    If ReasonForm.ClaimDataset.REASON.Rows.Count > 0 Then

                        If Not CBool(DV(0)("REASON_SW")) Then
                            DV(0).Row("REASON_SW") = 1
                        End If

                        If ApplyStatus.ToString.Trim.Length > 0 Then

                            If DV(0)("STATUS").ToString.Trim <> ApplyStatus Then
                                DV(0).Row("STATUS") = ApplyStatus
                            End If

                            If ApplyStatus = "DENY" AndAlso Not IsDBNull(DV(0)("PAID_AMT")) Then
                                If CDbl(DV(0)("PAID_AMT")) <> 0D Then
                                    DV(0).Row("PAID_AMT") = 0D
                                End If
                            End If
                        End If

                        'if paid = 0 and there is reasons delete alert
                        If IsDBNull(DV(0)("PAID_AMT")) OrElse CDbl(DV(0)("PAID_AMT")) = 0D Then
                            If AlertDV.Count > 0 Then
                                AlertDV(0).Row.Delete()
                            End If
                        End If

                        If ApplyStatus = "DENY" AndAlso Not IsDBNull(DV(0)("PAID_AMT")) Then

                        End If
                    Else
                        If CBool(DV(0)("REASON_SW")) Then
                            DV(0).Row("REASON_SW") = 0
                        End If

                    End If
                End If

            End If

        Catch ex As Exception
            Throw
        Finally
            If ReasonForm IsNot Nothing Then ReasonForm.Dispose()
            ReasonForm = Nothing

            AddHandler Me.KeyUp, AddressOf LineDetailsForm_KeyUp

        End Try

    End Sub
    Private Sub FormattingReason(ByRef value As Object, ByVal rowNum As Integer)
        Dim DV As DataView
        Dim ReasonDV As DataView
        Dim Output As String = ""

        Try
            DV = LineDetailDataGrid.GetDefaultDataView

            If DV IsNot Nothing Then
                ReasonDV = New DataView(_ClaimReasons, "LINE_NBR = " & DV(rowNum)("LINE_NBR").ToString, "LINE_NBR, PRIORITY", DataViewRowState.CurrentRows)
                If ReasonDV.Count > 0 Then
                    For Cnt As Integer = 0 To ReasonDV.Count - 1
                        If ReasonDV(Cnt).Row.RowState <> DataRowState.Deleted Then
                            Output &= If(Output = "", "", ", ") & If(ReasonDV(Cnt)("REASON").ToString = "LTR", ReasonDV(Cnt)("REASON").ToString & If(IsDBNull(ReasonDV(Cnt)("LETTER_NAMES")), "", ": " & ReasonDV(Cnt)("LETTER_NAMES").ToString), ReasonDV(Cnt)("REASON").ToString)
                        End If
                    Next
                    value = Output
                Else
                    value = "None"
                End If
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub UnFormattingReason(ByRef value As Object, ByVal rowNum As Integer)
        Dim DV As DataView

        Try
            DV = LineDetailDataGrid.GetDefaultDataView
            'value
            If DV IsNot Nothing Then
                value = DV(rowNum)("REASON_SW")
            Else
                value = 0
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub LineDetailDataGrid_CurrentRowChanged(ByVal CurrentRowIndex As Integer?, ByVal LastRowIndex As Integer?) Handles LineDetailDataGrid.CurrentRowChanged
        Dim CM As CurrencyManager
        Dim DGDRV As DataRowView
        Dim DGRow As DataRow

        Try

            If CurrentRowIndex IsNot Nothing Then

                CM = DirectCast(LineDetailDataGrid.BindingContext(LineDetailDataGrid.DataSource, LineDetailDataGrid.DataMember), CurrencyManager)
                DGDRV = DirectCast(CM.Current, DataRowView)
                DGRow = DGDRV.Row
                If DGRow IsNot Nothing Then
                    Me.AccumulatorValues.FilterByLineNumber(CShort(DGRow("LINE_NBR")))
                End If

            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    'Private Sub LineDetailDataGrid_PositionChanged(sender As Object, e As EventArgs) Handles LineDetailDataGrid.PositionChanged

    '    Dim BS As BindingSource

    '    Try

    '        BS = DirectCast(sender, BindingSource)

    '        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  CM(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    '        If BS.Position > -1 AndAlso BS.Current IsNot Nothing Then
    '            Dim DR As DataRow = CType(BS.Current, DataRowView).Row
    '            If DR IsNot Nothing Then
    '                Me.AccumulatorValues.FilterByLineNumber(CShort(DR("LINE_NBR")))
    '            End If
    '        End If

    '        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out:  CM(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    '    Catch ex As Exception
    '        Throw
    '    Finally

    '    End Try

    'End Sub

    Private Function SumCharges() As Decimal
        Try
            Dim DV As New DataView(_ClaimDetails, "ISNULL(CHRG_AMT,-.000001) <> -.000001  AND STATUS <> 'MERGED'", "CHRG_AMT", DataViewRowState.CurrentRows)
            Dim Total As Decimal = 0

            If DV.Count > 0 Then
                For Cnt As Integer = 0 To DV.Count - 1
                    Total = CDec(Total) + CDec(DV(Cnt)("CHRG_AMT"))
                Next

                TotalChargesTextBox.Text = Format(Total, "0.00;-0.00")

                Return Total
            Else
                TotalChargesTextBox.Text = ""

                Return 0
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function SumPaid() As Decimal
        Try
            Dim DV As New DataView(_ClaimDetails, "ISNULL(PAID_AMT,-.000001) <> -.000001  AND STATUS = 'PAY'", "PAID_AMT", DataViewRowState.CurrentRows)
            Dim Total As Decimal = 0

            If DV.Count > 0 Then
                For Cnt As Integer = 0 To DV.Count - 1
                    Total += CDec(DV(Cnt)("PAID_AMT"))
                Next

                TotalPaidTextBox.Text = Format(Total, "0.00;-0.00")

                Return Total
            Else
                TotalPaidTextBox.Text = ""

                Return 0
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function SumHRAApplied() As Decimal
        Try
            Dim dv As New DataView(_ClaimDetails, "ISNULL(HRA_AMT,-.000001) <> -.000001  AND STATUS <> 'MERGED'", "HRA_AMT", DataViewRowState.CurrentRows)
            Dim Total As Decimal = 0

            If dv.Count > 0 Then
                For cnt As Integer = 0 To dv.Count - 1
                    Total += CDec(dv(cnt)("HRA_AMT"))
                Next

                TotalHRAAppliedTextBox.Text = Format(Total, "0.00;-0.00")

                Return Total
            Else
                TotalHRAAppliedTextBox.Text = ""

                Return 0
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Function SumAllowed() As Decimal
        Try
            Me.BindingContext(_ClaimDetails).EndCurrentEdit()

            Dim dv As New DataView(_ClaimDetails, "ISNULL(ALLOWED_AMT,-.000001) <> -.000001 AND STATUS <> 'MERGED'", "ALLOWED_AMT", DataViewRowState.CurrentRows)
            Dim Total As Decimal = 0

            If dv.Count > 0 Then
                For cnt As Integer = 0 To dv.Count - 1
                    Total += CDec(dv(cnt)("ALLOWED_AMT"))
                Next

                TotalAllowedTextBox.Text = Format(Total, "0.00;-0.00")

                Return Total
            Else
                TotalAllowedTextBox.Text = ""

                Return 0
            End If

        Catch ex As Exception
            Throw
        End Try
    End Function

End Class