Option Strict On


Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

''' -----------------------------------------------------------------------------
''' Project	 : MedicalWork
''' Class	 : CLAIMS.UI.AccidentAccumulatorSelector
'''
''' -----------------------------------------------------------------------------
''' <summary>
'''
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	5/7/2007	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class AccidentAccumulatorSelector
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _RecentAccidentsDV As DataView
    Private _AccidentEntriesDT As DataTable
    Private _RelationID As Integer
    Private _FamilyID As Integer
    Private _CurrentClaimID As Integer
    Private _IncidentDate As Date
    Private _SelectedID As Integer?
    Private _SelectedName As String = String.Empty '""
    Private _SelectedClaimID As Integer?
    Private _SelectedDate As Date?

    Private _APPKEY As String = "UFCW\Claims\"

#Region " Windows Form Designer generated code "

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents AccidentAccumulatorDataGrid As DataGridCustom
    Friend WithEvents SelectButton As System.Windows.Forms.Button
    Friend WithEvents NewButton As System.Windows.Forms.Button

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.AccidentAccumulatorDataGrid = New DataGridCustom()
        Me.SelectButton = New System.Windows.Forms.Button()
        Me.NewButton = New System.Windows.Forms.Button()
        CType(Me.AccidentAccumulatorDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AccidentAccumulatorDataGrid
        '
        Me.AccidentAccumulatorDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.AccidentAccumulatorDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.AccidentAccumulatorDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.AccidentAccumulatorDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.AccidentAccumulatorDataGrid.ADGroupsThatCanFind = ""
        Me.AccidentAccumulatorDataGrid.ADGroupsThatCanMultiSort = ""
        Me.AccidentAccumulatorDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.AccidentAccumulatorDataGrid.AllowAutoSize = True
        Me.AccidentAccumulatorDataGrid.AllowColumnReorder = False
        Me.AccidentAccumulatorDataGrid.AllowCopy = False
        Me.AccidentAccumulatorDataGrid.AllowCustomize = False
        Me.AccidentAccumulatorDataGrid.AllowDelete = False
        Me.AccidentAccumulatorDataGrid.AllowDragDrop = False
        Me.AccidentAccumulatorDataGrid.AllowEdit = False
        Me.AccidentAccumulatorDataGrid.AllowExport = False
        Me.AccidentAccumulatorDataGrid.AllowFilter = False
        Me.AccidentAccumulatorDataGrid.AllowFind = False
        Me.AccidentAccumulatorDataGrid.AllowGoTo = False
        Me.AccidentAccumulatorDataGrid.AllowMultiSelect = False
        Me.AccidentAccumulatorDataGrid.AllowMultiSort = False
        Me.AccidentAccumulatorDataGrid.AllowNavigation = False
        Me.AccidentAccumulatorDataGrid.AllowNew = False
        Me.AccidentAccumulatorDataGrid.AllowPrint = False
        Me.AccidentAccumulatorDataGrid.AllowRefresh = False
        Me.AccidentAccumulatorDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AccidentAccumulatorDataGrid.AppKey = "UFCW\Claims\"
        Me.AccidentAccumulatorDataGrid.AutoSaveCols = True
        Me.AccidentAccumulatorDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.AccidentAccumulatorDataGrid.ColumnHeaderLabel = Nothing
        Me.AccidentAccumulatorDataGrid.ColumnRePositioning = False
        Me.AccidentAccumulatorDataGrid.ColumnResizing = False
        Me.AccidentAccumulatorDataGrid.ConfirmDelete = True
        Me.AccidentAccumulatorDataGrid.CopySelectedOnly = True
        Me.AccidentAccumulatorDataGrid.CurrentBSPosition = -1
        Me.AccidentAccumulatorDataGrid.DataMember = ""
        Me.AccidentAccumulatorDataGrid.DragColumn = 0
        Me.AccidentAccumulatorDataGrid.ExportSelectedOnly = True
        Me.AccidentAccumulatorDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.AccidentAccumulatorDataGrid.HighlightedRow = Nothing
        Me.AccidentAccumulatorDataGrid.HighLightModifiedRows = False
        Me.AccidentAccumulatorDataGrid.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.AccidentAccumulatorDataGrid.IsMouseDown = False
        Me.AccidentAccumulatorDataGrid.LastGoToLine = ""
        Me.AccidentAccumulatorDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.AccidentAccumulatorDataGrid.MultiSort = False
        Me.AccidentAccumulatorDataGrid.Name = "AccidentAccumulatorDataGrid"
        Me.AccidentAccumulatorDataGrid.OldSelectedRow = Nothing
        Me.AccidentAccumulatorDataGrid.PreviousBSPosition = -1
        Me.AccidentAccumulatorDataGrid.ReadOnly = True
        Me.AccidentAccumulatorDataGrid.RetainRowSelectionAfterSort = True
        Me.AccidentAccumulatorDataGrid.SetRowOnRightClick = True
        Me.AccidentAccumulatorDataGrid.ShiftPressed = False
        Me.AccidentAccumulatorDataGrid.SingleClickBooleanColumns = True
        Me.AccidentAccumulatorDataGrid.Size = New System.Drawing.Size(496, 208)
        Me.AccidentAccumulatorDataGrid.Sort = Nothing
        Me.AccidentAccumulatorDataGrid.StyleName = ""
        Me.AccidentAccumulatorDataGrid.SubKey = ""
        Me.AccidentAccumulatorDataGrid.SuppressMouseDown = False
        Me.AccidentAccumulatorDataGrid.SuppressTriangle = False
        Me.AccidentAccumulatorDataGrid.TabIndex = 0
        '
        'SelectButton
        '
        Me.SelectButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SelectButton.Location = New System.Drawing.Point(155, 214)
        Me.SelectButton.Name = "SelectButton"
        Me.SelectButton.Size = New System.Drawing.Size(75, 23)
        Me.SelectButton.TabIndex = 1
        Me.SelectButton.Text = "Select"
        '
        'NewButton
        '
        Me.NewButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.NewButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.NewButton.Location = New System.Drawing.Point(267, 214)
        Me.NewButton.Name = "NewButton"
        Me.NewButton.Size = New System.Drawing.Size(75, 23)
        Me.NewButton.TabIndex = 2
        Me.NewButton.Text = "New"
        '
        'AccidentAccumulatorSelector
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(496, 246)
        Me.Controls.Add(Me.NewButton)
        Me.Controls.Add(Me.SelectButton)
        Me.Controls.Add(Me.AccidentAccumulatorDataGrid)
        Me.Name = "AccidentAccumulatorSelector"
        Me.Text = "Select Accident Accumulator"
        CType(Me.AccidentAccumulatorDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Properties"
    <System.ComponentModel.Description("If no accident accumulators are associated to member, the display of a selection dialog can be surpressed.")>
    Public ReadOnly Property SuppressDisplay As Boolean
        Get
            Return _RecentAccidentsDV.Count = 0
        End Get
    End Property

    <System.ComponentModel.Description("Represents the application key used when accessing the registry.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property
#End Region

    Public Sub New(ByVal relationID As Integer, ByVal familyID As Integer, ByVal currentClaimID As Integer, ByVal incidentDate As Date, ByVal memberAccumulatorManager As MemberAccumulatorManager)

        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _RelationID = relationID
        _FamilyID = familyID
        _IncidentDate = incidentDate
        _CurrentClaimID = currentClaimID

        _AccidentEntriesDT = memberAccumulatorManager.GetAccidentAccumulatorEntries()

        _RecentAccidentsDV = New DataView(_AccidentEntriesDT, "APPLY_DATE <= '" & _IncidentDate.AddDays(90).ToShortDateString & "' AND APPLY_DATE >= '" & _IncidentDate.AddDays(-90).ToShortDateString & "'", "ACCUM_NAME", DataViewRowState.CurrentRows)

        If _RecentAccidentsDV.Count = 0 Then
            If _AccidentEntriesDT.Rows.Count = 0 Then
                _SelectedName = GetNewAccidentAccumulator()
            Else
                'ACC replace is to accomodate earlier accumulator mistakes made by examiners (should not use generic accum ACC)
                _SelectedName = GetNewAccidentAccumulator(CInt(CStr(_AccidentEntriesDT.Rows(_AccidentEntriesDT.Rows.Count - 1)("ACCUM_NAME")).Replace("ACC", "000").Replace("AC", "")) + 1)
            End If
        End If

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal thisDisposing As Boolean)
        If thisDisposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(thisDisposing)
    End Sub

    Public ReadOnly Property SelectedAccumulatorID() As Integer?
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/7/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return _SelectedID
        End Get
    End Property
    Public ReadOnly Property SelectedClaimID() As Integer?
        Get
            Return _SelectedClaimID
        End Get
    End Property
    Public ReadOnly Property SelectedDate() As Date?
        Get
            Return _SelectedDate
        End Get
    End Property
    Public ReadOnly Property SelectedName() As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/7/2007	Created
        ' </history>
        '     ' -----------------------------------------------------------------------------
        Get
            Return _SelectedName
        End Get
    End Property
    Private Sub AccidentAccumulatorSelector_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            SetSettings()

            If _RecentAccidentsDV.Count > 0 Then

                _RecentAccidentsDV.Sort = "APPLY_DATE,CLAIM_ID"

                AccidentAccumulatorDataGrid.DataSource = _RecentAccidentsDV
                AccidentAccumulatorDataGrid.SetTableStyle()

            End If

            AccidentAccumulatorDataGrid.CaptionText = "Claim: " & _CurrentClaimID.ToString & " Incident Date: " & _IncidentDate.ToShortDateString

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub SelectButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectButton.Click
        _SelectedID = CInt(CType(Me.AccidentAccumulatorDataGrid.DataSource, DataView).Item(Me.AccidentAccumulatorDataGrid.CurrentRowIndex)("ACCUM_ID"))
        _SelectedClaimID = CInt(CType(Me.AccidentAccumulatorDataGrid.DataSource, DataView).Item(Me.AccidentAccumulatorDataGrid.CurrentRowIndex)("CLAIM_ID"))
        _SelectedName = CStr(CType(Me.AccidentAccumulatorDataGrid.DataSource, DataView).Item(Me.AccidentAccumulatorDataGrid.CurrentRowIndex)("ACCUM_NAME"))
        _SelectedDate = CDate(CType(Me.AccidentAccumulatorDataGrid.DataSource, DataView).Item(Me.AccidentAccumulatorDataGrid.CurrentRowIndex)("APPLY_DATE"))
        Me.Close()
    End Sub

    Private Sub NewButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewButton.Click

        Dim DV As DataView
        Dim Highest As Integer = -1

        Try

            DV = _AccidentEntriesDT.DefaultView 'CType(Me.AccidentAccumulatorDataGrid.DataSource, DataView)
            DV.Sort = "ACCUM_ID ASC"
            _SelectedName = GetNewAccidentAccumulator(CInt(CStr(DV.Item(DV.Count - 1)("ACCUM_NAME")).Replace("AC", "")) + 1)

            Me.Close()

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Function GetNewAccidentAccumulator(ByVal id As Integer) As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="id"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/7/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim accumName As String

        If id >= 999 Then Throw New ArgumentException("Next Accumulator ID must be less than 1000")

        Try

            If id < 10 Then
                accumName = "AC00" & id.ToString
            ElseIf id >= 10 AndAlso id <= 99 Then
                accumName = "AC0" & id.ToString
            Else 'id >= 100
                accumName = "AC" & id.ToString
            End If

            _SelectedID = AccumulatorController.GetNextAccidentAccumulator(accumName)

            Return accumName

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Function GetNewAccidentAccumulator() As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/7/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            Return GetNewAccidentAccumulator(1)

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub AccidentAccumulatorSelector_FormClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.FormClosing

        SaveSettings()

        If Me._SelectedID <= -1 Then
            If MessageBox.Show("You did not select an accumulator. Continue closing window?", "Accumulator Selector", MessageBoxButtons.YesNo) = DialogResult.No Then
                e.Cancel = True
            Else
                e.Cancel = False
            End If
        End If

    End Sub

    Private Sub SetSettings()
        Me.Top = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)
    End Sub

    Private Sub SaveSettings()
        Dim lWindowState As FormWindowState = Me.WindowState

        SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(lWindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = lWindowState

    End Sub

End Class