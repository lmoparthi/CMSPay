
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class FamilyList
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer? = Nothing

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
    Friend WithEvents FamilyListDataGrid As DataGridCustom
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents SkipButton As System.Windows.Forms.Button
    Friend WithEvents OkButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.FamilyListDataGrid = New DataGridCustom()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.SkipButton = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.FamilyListDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'FamilyListDataGrid
        '
        Me.FamilyListDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.FamilyListDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.FamilyListDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.FamilyListDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.FamilyListDataGrid.ADGroupsThatCanFind = ""
        Me.FamilyListDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.FamilyListDataGrid.ADGroupsThatCanMultiSort = ""
        Me.FamilyListDataGrid.AllowAutoSize = True
        Me.FamilyListDataGrid.AllowColumnReorder = False
        Me.FamilyListDataGrid.AllowCopy = False
        Me.FamilyListDataGrid.AllowCustomize = False
        Me.FamilyListDataGrid.AllowDelete = False
        Me.FamilyListDataGrid.AllowDragDrop = False
        Me.FamilyListDataGrid.AllowEdit = False
        Me.FamilyListDataGrid.AllowExport = False
        Me.FamilyListDataGrid.AllowFilter = False
        Me.FamilyListDataGrid.AllowFind = False
        Me.FamilyListDataGrid.AllowGoTo = False
        Me.FamilyListDataGrid.AllowMultiSelect = False
        Me.FamilyListDataGrid.AllowMultiSort = False
        Me.FamilyListDataGrid.AllowNew = False
        Me.FamilyListDataGrid.AllowPrint = False
        Me.FamilyListDataGrid.AllowRefresh = False
        Me.FamilyListDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyListDataGrid.AppKey = "UFCW\Claims\"
        Me.FamilyListDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.FamilyListDataGrid.ColumnHeaderLabel = Nothing
        Me.FamilyListDataGrid.ColumnRePositioning = False
        Me.FamilyListDataGrid.ColumnResizing = False
        Me.FamilyListDataGrid.ConfirmDelete = True
        Me.FamilyListDataGrid.CopySelectedOnly = False
        Me.FamilyListDataGrid.DataMember = ""
        Me.FamilyListDataGrid.DragColumn = 0
        Me.FamilyListDataGrid.ExportSelectedOnly = False
        Me.FamilyListDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.FamilyListDataGrid.HighlightedRow = Nothing
        Me.FamilyListDataGrid.IsMouseDown = False
        Me.FamilyListDataGrid.LastGoToLine = ""
        Me.FamilyListDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.FamilyListDataGrid.MultiSort = False
        Me.FamilyListDataGrid.Name = "FamilyListDataGrid"
        Me.FamilyListDataGrid.OldSelectedRow = Nothing
        Me.FamilyListDataGrid.ReadOnly = True
        Me.FamilyListDataGrid.SetRowOnRightClick = True
        Me.FamilyListDataGrid.ShiftPressed = False
        Me.FamilyListDataGrid.SingleClickBooleanColumns = True
        Me.FamilyListDataGrid.Size = New System.Drawing.Size(789, 280)
        Me.FamilyListDataGrid.StyleName = ""
        Me.FamilyListDataGrid.SubKey = ""
        Me.FamilyListDataGrid.SuppressTriangle = False
        Me.FamilyListDataGrid.TabIndex = 0
        '
        'OkButton
        '
        Me.OkButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OkButton.Location = New System.Drawing.Point(706, 286)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(68, 23)
        Me.OkButton.TabIndex = 1
        Me.OkButton.Text = "Ok"
        Me.ToolTip1.SetToolTip(Me.OkButton, "Select a Family and press OK")
        '
        'SkipButton
        '
        Me.SkipButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SkipButton.DialogResult = System.Windows.Forms.DialogResult.Ignore
        Me.SkipButton.Location = New System.Drawing.Point(622, 286)
        Me.SkipButton.Name = "SkipButton"
        Me.SkipButton.Size = New System.Drawing.Size(68, 23)
        Me.SkipButton.TabIndex = 2
        Me.SkipButton.Text = "Skip"
        Me.ToolTip1.SetToolTip(Me.SkipButton, "When Family selection is skipped, NO Family specific tabs will be displayed.")
        Me.SkipButton.Visible = False
        '
        'FamilyList
        '
        Me.AcceptButton = Me.OkButton
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(786, 310)
        Me.ControlBox = False
        Me.Controls.Add(Me.SkipButton)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.FamilyListDataGrid)
        Me.Name = "FamilyList"
        Me.Text = "Family List"
        Me.TopMost = True
        CType(Me.FamilyListDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
#Region "Custom Code"

    Public Sub New(ByVal searchresultsDT As DataTable, familylistDT As DataTable)
        Me.New()

        Me.SkipButton.Visible = True

        SetFormInfo(searchresultsDT, familylistDT)
    End Sub

    Public Sub New(familylistDT As DataTable)
        Me.New()

        Me.SkipButton.Visible = True

        SetFormInfo(familylistDT)
    End Sub

    Public ReadOnly Property SelectedFamilyID() As Integer
        Get
            Return CInt(_FamilyID)
        End Get
    End Property

    Public Sub SetFormInfo(ByVal familyId As Integer)

        Me.FamilyListDataGrid.DataSource = CMSDALFDBMD.RetrievePatients(familyId).Tables(0)
        FamilyListDataGrid.SetTableStyle()

    End Sub

    Public Sub SetFormInfo(familylistDT As DataTable)

        Me.FamilyListDataGrid.DataSource = familylistDT
        FamilyListDataGrid.SetTableStyle("DistinctFamilyListDataGrid")

        Me.Text = "Specified SSN is associated to multiple individuals across various Families. Select SSN to limit search and include Family specific information results."
    End Sub

    Public Sub SetFormInfo(ByVal searchresultsDT As DataTable, familylistDT As DataTable)

        Dim FamilyDT As DataTable

        Try

            FamilyDT = searchresultsDT.Clone()
            FamilyDT.BeginLoadData()
            For Each DR As DataRow In familylistDT.Rows
                searchresultsDT.DefaultView.RowFilter = "FAMILY_ID = " & DR("FAMILY_ID").ToString
                FamilyDT.ImportRow(searchresultsDT.DefaultView(0).Row)
            Next
            FamilyDT.EndLoadData()

            Me.FamilyListDataGrid.DataSource = FamilyDT
            FamilyListDataGrid.SetTableStyle("DistinctFamilyListDataGrid")

            Me.Text = "Specified SSN is associated to multiple individuals across various Families. Select SSN to limit search and include Family specific information results."

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub OkButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkButton.Click

        Dim DV As DataView
        Dim BM As BindingManagerBase
        Dim DR As DataRow

        Try

            DV = FamilyListDataGrid.GetCurrentDataView
            BM = Me.FamilyListDataGrid.BindingContext(Me.FamilyListDataGrid.DataSource, Me.FamilyListDataGrid.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            If DR IsNot Nothing Then

                If CType(DR("SSNO"), String).Contains("Restricted") Then
                    MsgBox("Your system priviledges do not allow the selection of restricted items.", CType(MsgBoxStyle.OkOnly + MsgBoxStyle.Information, MsgBoxStyle), "Invalid selection")
                    Exit Sub
                End If

                _FamilyID = CInt(DR("FAMILY_ID"))

                Me.Hide()

            End If

        Catch ex As Exception

	Throw
        End Try

    End Sub

    Private Sub FamilyList_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        FamilyListDataGrid.TableStyles.Clear()
        FamilyListDataGrid.DataSource = Nothing
        FamilyListDataGrid.Dispose()
        FamilyListDataGrid = Nothing
    End Sub

    Private Sub FamilyListDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles FamilyListDataGrid.DoubleClick

        Select Case CType(sender, DataGridCustom).LastHitSpot.type

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell, System.Windows.Forms.DataGrid.HitTestType.RowHeader

                Dim DT As DataTable = CType(CType(sender, DataGridCustom).DataSource, DataTable)

                For I As Integer = 0 To DT.Rows.Count - 1
                    If CType(sender, DataGridCustom).IsSelected(I) Then
                        _FamilyID = CInt(DT.Rows(I)("FAMILY_ID"))
                    End If

                    If _FamilyID Is Nothing OrElse _FamilyID = -1 Then
                        _FamilyID = CInt(DT.Rows(0)("FAMILY_ID"))
                    End If
                Next

                DialogResult = System.Windows.Forms.DialogResult.OK

                Me.Hide()

        End Select

    End Sub

#End Region
End Class