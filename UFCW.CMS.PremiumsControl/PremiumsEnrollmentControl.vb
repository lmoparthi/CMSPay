Option Infer On

Imports System.ComponentModel
Imports UFCW.WCF

Public Class PremiumsEnrollmentControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer = -1
    Private _RelationID As Integer = -1
    Private _APPKEY As String = "UFCW\Claims\"
    Private _PremLetterDS As DataSet

    Private _DocID As Long = -1

    Friend WithEvents PremiumContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents DisplaySentMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DisplayReceivedMenuItem As System.Windows.Forms.ToolStripMenuItem

    Public Event BeforeRefresh(ByVal sender As Object, ByRef Cancel As Boolean)
    Public Event AfterRefresh(ByVal sender As Object)

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.


            If PremLetterDataGrid IsNot Nothing Then
                PremLetterDataGrid.Dispose()
            End If
            PremLetterDataGrid = Nothing

            If _PremLetterDS IsNot Nothing Then
                _PremLetterDS.Dispose()
            End If
            _PremLetterDS = Nothing

            If Not (components Is Nothing) Then
                components.Dispose()
            End If

        End If

        ' Free any unmanaged objects here.
        '
        _Disposed = True

        ' Call base class implementation.
        MyBase.Dispose(disposing)
    End Sub

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents PremLetterDataGrid As DataGridCustom
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.PremLetterDataGrid = New DataGridCustom()
        Me.PremiumContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DisplaySentMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DisplayReceivedMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.PremLetterDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PremiumContextMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'PremLetterDataGrid
        '
        Me.PremLetterDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.PremLetterDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.PremLetterDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PremLetterDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PremLetterDataGrid.ADGroupsThatCanFind = ""
        Me.PremLetterDataGrid.ADGroupsThatCanMultiSort = ""
        Me.PremLetterDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PremLetterDataGrid.AllowAutoSize = True
        Me.PremLetterDataGrid.AllowColumnReorder = True
        Me.PremLetterDataGrid.AllowCopy = False
        Me.PremLetterDataGrid.AllowCustomize = True
        Me.PremLetterDataGrid.AllowDelete = False
        Me.PremLetterDataGrid.AllowDragDrop = False
        Me.PremLetterDataGrid.AllowEdit = False
        Me.PremLetterDataGrid.AllowExport = False
        Me.PremLetterDataGrid.AllowFilter = True
        Me.PremLetterDataGrid.AllowFind = True
        Me.PremLetterDataGrid.AllowGoTo = True
        Me.PremLetterDataGrid.AllowMultiSelect = False
        Me.PremLetterDataGrid.AllowMultiSort = False
        Me.PremLetterDataGrid.AllowNew = False
        Me.PremLetterDataGrid.AllowPrint = False
        Me.PremLetterDataGrid.AllowRefresh = False
        Me.PremLetterDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PremLetterDataGrid.AppKey = "UFCW\Claims\"
        Me.PremLetterDataGrid.AutoSaveCols = True
        Me.PremLetterDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.PremLetterDataGrid.CaptionForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.PremLetterDataGrid.CaptionText = "Premiums Enrollment Forms History"
        Me.PremLetterDataGrid.ColumnHeaderLabel = Nothing
        Me.PremLetterDataGrid.ColumnRePositioning = False
        Me.PremLetterDataGrid.ColumnResizing = False
        Me.PremLetterDataGrid.ConfirmDelete = True
        Me.PremLetterDataGrid.CopySelectedOnly = True
        Me.PremLetterDataGrid.DataMember = ""
        Me.PremLetterDataGrid.DragColumn = 0
        Me.PremLetterDataGrid.ExportSelectedOnly = True
        Me.PremLetterDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.PremLetterDataGrid.HighlightedRow = Nothing
        Me.PremLetterDataGrid.IsMouseDown = False
        Me.PremLetterDataGrid.LastGoToLine = ""
        Me.PremLetterDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.PremLetterDataGrid.MultiSort = False
        Me.PremLetterDataGrid.Name = "PremLetterDataGrid"
        Me.PremLetterDataGrid.OldSelectedRow = Nothing
        Me.PremLetterDataGrid.ParentRowsVisible = False
        Me.PremLetterDataGrid.ReadOnly = True
        Me.PremLetterDataGrid.SetRowOnRightClick = True
        Me.PremLetterDataGrid.ShiftPressed = False
        Me.PremLetterDataGrid.SingleClickBooleanColumns = True
        Me.PremLetterDataGrid.Size = New System.Drawing.Size(440, 360)
        Me.PremLetterDataGrid.Sort = Nothing
        Me.PremLetterDataGrid.StyleName = ""
        Me.PremLetterDataGrid.SubKey = ""
        Me.PremLetterDataGrid.SuppressTriangle = False
        Me.PremLetterDataGrid.TabIndex = 7
        '
        'PremiumContextMenu
        '
        Me.PremiumContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DisplaySentMenuItem, Me.DisplayReceivedMenuItem})
        Me.PremiumContextMenu.Name = "PremiumContextMenu"
        Me.PremiumContextMenu.Size = New System.Drawing.Size(204, 70)
        '
        'DisplaySentMenuItem
        '
        Me.DisplaySentMenuItem.Name = "DisplaySentMenuItem"
        Me.DisplaySentMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.DisplaySentMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.DisplaySentMenuItem.Text = "Display &Sent"
        '
        'DisplayReceivedMenuItem
        '
        Me.DisplayReceivedMenuItem.Name = "DisplayReceivedMenuItem"
        Me.DisplayReceivedMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.DisplayReceivedMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.DisplayReceivedMenuItem.Text = "Display &Received"
        '
        'PremiumsEnrollmentControl
        '
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Controls.Add(Me.PremLetterDataGrid)
        Me.Name = "PremiumsEnrollmentControl"
        Me.Size = New System.Drawing.Size(440, 360)
        CType(Me.PremLetterDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PremiumContextMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

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
    Public Property RelationID() As Integer
        Get
            Return _RelationID
        End Get
        Set(ByVal value As Integer)
            _RelationID = value
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

#Region "Constructor"
    Public Sub New(ByVal FamilyID As Integer, ByVal RelationID As Integer)
        Me.New()
        _FamilyID = FamilyID
        _RelationID = RelationID
        LoadPremiumsControl()
    End Sub
#End Region

#Region "Form\Button Events"
    Public Sub RefreshActivity()
        Try

            PremLetterDataGrid_RefreshGridData()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub PremLetterDataGrid_RefreshGridData() Handles PremLetterDataGrid.RefreshGridData
        Try
            Dim Cancel As Boolean = False

            RaiseEvent BeforeRefresh(Me, Cancel)

            If Cancel = False Then
                LoadPremiumsControl()
            End If

            RaiseEvent AfterRefresh(Me)
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Private Sub PremiumsEnrollmentControl_dispose(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Me.Dispose()
    End Sub
#End Region

#Region "Custom Subs\Functions"
    Public Sub LoadPremiumsControl(ByVal familyID As Integer, Optional premLetterDataSet As DataSet = Nothing)
        Try
            _FamilyID = familyID
            If premLetterDataSet IsNot Nothing Then _PremLetterDS = premLetterDataSet

            LoadPremiumsControl()

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Sub LoadPremiumsControl()
        Try

            PremLetterDataGrid.DataSource = Nothing

            If _PremLetterDS Is Nothing OrElse _PremLetterDS.Tables Is Nothing OrElse _PremLetterDS.Tables.Count < 1 Then
                _PremLetterDS = PremiumsDAL.GetPremLetterHistory(_FamilyID, _PremLetterDS)
            End If

            PremLetterDataGrid.SuspendLayout()

            PremLetterDataGrid.DataSource = _PremLetterDS.Tables("PREMLETTER")
            PremLetterDataGrid.SetTableStyle()
            PremLetterDataGrid.Sort = If(PremLetterDataGrid.LastSortedBy, PremLetterDataGrid.DefaultSort)

            PremLetterDataGrid.ContextMenuPrepare(PremiumContextMenu)

            PremLetterDataGrid.ResumeLayout()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub ClearAll()
        If PremLetterDataGrid IsNot Nothing Then
            PremLetterDataGrid.DataSource = Nothing
        End If
        _PremLetterDS = Nothing
    End Sub

    Private Sub PremiumContextMenu_Opening(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PremiumContextMenu.Opening

        Dim DGContextMenu As ContextMenuStrip
        Dim DG As DataGridCustom
        Dim DR As DataRow

        Try

            DGContextMenu = CType(sender, ContextMenuStrip)

            DGContextMenu.Items("DisplaySentMenuItem").Available = False
            DGContextMenu.Items("DisplayReceivedMenuItem").Available = False

            DG = CType(DirectCast(sender, System.Windows.Forms.ContextMenuStrip).SourceControl, DataGridCustom)
            If DG IsNot Nothing AndAlso DG.DataSource IsNot Nothing Then

                DR = DG.SelectedRowPreview

                If DR IsNot Nothing Then

                    If DR.Table.Columns.Contains("DOCID_OUT") AndAlso Not IsDBNull(DR("DOCID_OUT")) AndAlso IsDecimal(DR("DOCID_OUT").ToString) AndAlso CLng(DR("DOCID_OUT")) > 0 Then
                        DGContextMenu.Items("DisplaySentMenuItem").Available = True
                    End If

                    If DR.Table.Columns.Contains("DOCID_IN") AndAlso Not IsDBNull(DR("DOCID_IN")) AndAlso IsDecimal(DR("DOCID_IN").ToString) AndAlso CLng(DR("DOCID_IN")) > 0 Then
                        DGContextMenu.Items("DisplayReceivedMenuItem").Available = True
                    End If

                    'If DR.Table.Columns.Contains("MAXIM_ID") AndAlso Not IsDBNull(DR("MAXIM_ID")) AndAlso DR("MAXIM_ID").ToString.Trim.Length > 0 Then
                    '    DGContextMenu.Items("DisplayReceivedMenuItem").Available = True
                    'End If

                End If

            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub DisplaySentMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplaySentMenuItem.Click
        DisplayDocument()
    End Sub

    Private Sub DisplayDocument()

        Dim BM As BindingManagerBase
        Dim DR As DataRow
        Dim Docs() As Object
        Dim Cnt As Integer = 0
        Dim Tot As Integer = 0

        Try
            BM = Me.PremLetterDataGrid.BindingContext(Me.PremLetterDataGrid.DataSource, Me.PremLetterDataGrid.DataMember)

            If BM.Count > 0 Then

                DR = CType(BM.Current, DataRowView).Row

                If DR.Table.Columns.Contains("DOCID_OUT") AndAlso UFCWGeneral.IsNullLongHandler(DR("DOCID_OUT")) IsNot Nothing Then
                    ReDim Docs(1)

                    Docs(0) = DR("DOCID_OUT")

                    Using FNDisplay As New Display
                        FNDisplay.Display(Docs)
                        _DocID = CLng(DR("DOCID_OUT"))
                    End Using

                ElseIf DR.Table.Columns.Contains("DOCID") AndAlso UFCWGeneral.IsNullLongHandler(DR("DOCID")) IsNot Nothing Then
                    ReDim Docs(1)

                    Docs(0) = DR("DOCID")

                    Using FNDisplay As New Display
                        FNDisplay.Display(Docs)
                        _DocID = CLng(DR("DOCID"))
                    End Using

                ElseIf DR.Table.Columns.Contains("MAXIM_ID") AndAlso UFCWGeneral.IsNullStringHandler(DR("MAXIM_ID")) IsNot Nothing Then

                    Using FNDisplay As New Display
                        Dim FNDocProperties As FileNet.FileNetDocumentProperties = FNDisplay.Display(DR("MAXIM_ID").ToString)
                        _DocID = FNDocProperties.ID
                    End Using
                Else
                    MessageBox.Show("There is no document to display.", "No Document", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            End If

        Catch ex As ApplicationException

            MessageBox.Show(ex.Message, "FileNet unavailable, Restarting application may resolve connectivity issues.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch ex As Exception
            Throw
        End Try
    End Sub


    Private Sub DisplayReceivedMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayReceivedMenuItem.Click
        Dim DV As DataView
        Dim Cnt As Integer = 0
        Dim Docs() As Long

        Try
            If PremLetterDataGrid.GetGridRowCount > 0 Then

                ReDim Docs(0)

                DV = PremLetterDataGrid.GetDefaultDataView

                For Cnt = PremLetterDataGrid.GetGridRowCount - 1 To 0 Step -1
                    If PremLetterDataGrid.IsSelected(Cnt) = True AndAlso IsDBNull(DV(Cnt)("DOCID_IN")) = False AndAlso CInt(DV(Cnt)("DOCID_IN")) > 0 AndAlso DV(Cnt)("DOCID_IN").ToString.Contains("Restricted") = False Then
                        Docs(UBound(Docs, 1)) = CLng(DV(Cnt)("DOCID_IN"))

                        ReDim Preserve Docs(UBound(Docs, 1) + 1)
                    End If
                Next

                ReDim Preserve Docs(UBound(Docs, 1) - 1)

                If PremLetterDataGrid.CurrentRowIndex >= 0 AndAlso PremLetterDataGrid.IsSelected(PremLetterDataGrid.CurrentRowIndex) = False _
                                    AndAlso IsDBNull(DV(PremLetterDataGrid.CurrentRowIndex)("DOCID_IN")) = False AndAlso DV(PremLetterDataGrid.CurrentRowIndex)("DOCID_IN").ToString.Contains("Restricted") = False Then
                    ReDim Preserve Docs(UBound(Docs, 1) + 1)

                    Docs(UBound(Docs, 1)) = CLng(DV(PremLetterDataGrid.CurrentRowIndex)("DOCID_IN"))
                End If

                If Docs.Length > 0 Then

                    Using FNDisplay As New Display
                        FNDisplay.Display(Docs)
                    End Using

                Else
                    For Cnt = PremLetterDataGrid.GetGridRowCount - 1 To 0 Step -1
                        If PremLetterDataGrid.IsSelected(Cnt) = True AndAlso IsDBNull(DV(Cnt)("DOCID_IN")) = False AndAlso DV(Cnt)("DOCID_IN").ToString.Contains("Restricted") Then
                            MsgBox("You do not have privileges to view documents for this individual.", MsgBoxStyle.Exclamation, "Invalid Request")
                            Exit For
                        End If
                    Next
                End If

            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

#End Region

End Class