Option Strict On

Imports System.ComponentModel
Imports System.Security.Principal
Imports System.Data.Common
Imports System.Collections.Generic
Imports UFCW.WCF

Public Class LETTERSHistoryControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer?
    Private _RelationID As Short?
    Private _ClaimID As Integer?
    Private _LetterID As Integer
    Private _MaxID As String
    Private _DocID As Long
    Private _APPKEY As String = "UFCW\Medical\"
    Private _LetterDS As DataSet

    Private _Loading As Boolean = True
    Private _HoverCell As New DataGridCell
    Private _UserPrincipal As WindowsPrincipal
    Private _UserIdentity As WindowsIdentity

    Friend WithEvents DisplayLetterToolStripMenuItem As ToolStripMenuItem

    Shared Event CancelLetterRequest()
    Shared Event ReprintLetterRequest()
    Shared Event DisplayLetterRequest()

    ReadOnly _DomainUser As String = SystemInformation.UserName

    ' -----------------------------------------------------------------------------
    ' <summary>
    '
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	6/27/2007	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Overloads Sub Dispose()

        If LettersHistoryDataGrid IsNot Nothing Then
            If _LetterDS IsNot Nothing Then
                _LetterDS.Dispose()
                _LetterDS = Nothing
            End If
            LettersHistoryDataGrid.DataSource = Nothing
            LettersHistoryDataGrid.Dispose()
        End If
        LettersHistoryDataGrid = Nothing
        MyBase.Dispose()
    End Sub
#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Dim DesignMode As Boolean = (LicenseManager.UsageMode = LicenseUsageMode.Designtime)

    End Sub

    'UserControl1 overrides dispose to clean up the component list.
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
    Friend WithEvents DataGridTableStyle1 As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents LettersHistoryDataGrid As DataGridCustom
    Friend WithEvents LettersHistoryContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents CancelRequestMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReprintMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteRequestMenuItem As System.Windows.Forms.ToolStripMenuItem

    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.LettersHistoryDataGrid = New DataGridCustom()
        Me.LettersHistoryContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CancelRequestMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReprintMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteRequestMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DisplayLetterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DataGridTableStyle1 = New System.Windows.Forms.DataGridTableStyle()
        CType(Me.LettersHistoryDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LettersHistoryContextMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'LettersHistoryDataGrid
        '
        Me.LettersHistoryDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.LettersHistoryDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.LettersHistoryDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LettersHistoryDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LettersHistoryDataGrid.ADGroupsThatCanFind = ""
        Me.LettersHistoryDataGrid.ADGroupsThatCanMultiSort = ""
        Me.LettersHistoryDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.LettersHistoryDataGrid.AllowAutoSize = True
        Me.LettersHistoryDataGrid.AllowColumnReorder = False
        Me.LettersHistoryDataGrid.AllowCopy = False
        Me.LettersHistoryDataGrid.AllowCustomize = False
        Me.LettersHistoryDataGrid.AllowDelete = False
        Me.LettersHistoryDataGrid.AllowDragDrop = False
        Me.LettersHistoryDataGrid.AllowEdit = False
        Me.LettersHistoryDataGrid.AllowExport = False
        Me.LettersHistoryDataGrid.AllowFilter = False
        Me.LettersHistoryDataGrid.AllowFind = True
        Me.LettersHistoryDataGrid.AllowGoTo = True
        Me.LettersHistoryDataGrid.AllowMultiSelect = False
        Me.LettersHistoryDataGrid.AllowMultiSort = False
        Me.LettersHistoryDataGrid.AllowNew = False
        Me.LettersHistoryDataGrid.AllowPrint = False
        Me.LettersHistoryDataGrid.AllowRefresh = False
        Me.LettersHistoryDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LettersHistoryDataGrid.AppKey = "UFCW\Claims\"
        Me.LettersHistoryDataGrid.AutoSaveCols = True
        Me.LettersHistoryDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.LettersHistoryDataGrid.CaptionText = "Letter History"
        Me.LettersHistoryDataGrid.ColumnHeaderLabel = Nothing
        Me.LettersHistoryDataGrid.ColumnRePositioning = False
        Me.LettersHistoryDataGrid.ColumnResizing = False
        Me.LettersHistoryDataGrid.ConfirmDelete = True
        Me.LettersHistoryDataGrid.ContextMenuStrip = Me.LettersHistoryContextMenu
        Me.LettersHistoryDataGrid.CopySelectedOnly = True
        Me.LettersHistoryDataGrid.DataMember = ""
        Me.LettersHistoryDataGrid.DragColumn = 0
        Me.LettersHistoryDataGrid.ExportSelectedOnly = True
        Me.LettersHistoryDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.LettersHistoryDataGrid.HighlightedRow = Nothing
        Me.LettersHistoryDataGrid.HighLightModifiedRows = False
        Me.LettersHistoryDataGrid.IsMouseDown = False
        Me.LettersHistoryDataGrid.LastGoToLine = ""
        Me.LettersHistoryDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.LettersHistoryDataGrid.MultiSort = False
        Me.LettersHistoryDataGrid.Name = "LettersHistoryDataGrid"
        Me.LettersHistoryDataGrid.OldSelectedRow = Nothing
        Me.LettersHistoryDataGrid.ReadOnly = True
        Me.LettersHistoryDataGrid.RetainRowSelectionAfterSort = True
        Me.LettersHistoryDataGrid.RowHeadersVisible = False
        Me.LettersHistoryDataGrid.SetRowOnRightClick = True
        Me.LettersHistoryDataGrid.ShiftPressed = False
        Me.LettersHistoryDataGrid.SingleClickBooleanColumns = False
        Me.LettersHistoryDataGrid.Size = New System.Drawing.Size(424, 336)
        Me.LettersHistoryDataGrid.Sort = Nothing
        Me.LettersHistoryDataGrid.StyleName = ""
        Me.LettersHistoryDataGrid.SubKey = ""
        Me.LettersHistoryDataGrid.SuppressTriangle = False
        Me.LettersHistoryDataGrid.TabIndex = 0
        Me.LettersHistoryDataGrid.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.DataGridTableStyle1})
        '
        'LettersHistoryContextMenu
        '
        Me.LettersHistoryContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CancelRequestMenuItem, Me.ReprintMenuItem, Me.DeleteRequestMenuItem, Me.DisplayLetterToolStripMenuItem})
        Me.LettersHistoryContextMenu.Name = "DataGridCustomContextMenu"
        Me.LettersHistoryContextMenu.Size = New System.Drawing.Size(189, 92)
        '
        'CancelRequestMenuItem
        '
        Me.CancelRequestMenuItem.Name = "CancelRequestMenuItem"
        Me.CancelRequestMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.CancelRequestMenuItem.Text = "Cancel Letter Request"
        '
        'ReprintMenuItem
        '
        Me.ReprintMenuItem.Name = "ReprintMenuItem"
        Me.ReprintMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.ReprintMenuItem.Text = "Re-print"
        '
        'DeleteRequestMenuItem
        '
        Me.DeleteRequestMenuItem.Name = "DeleteRequestMenuItem"
        Me.DeleteRequestMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.DeleteRequestMenuItem.Text = "Delete Letter Request"
        '
        'DisplayLetterToolStripMenuItem
        '
        Me.DisplayLetterToolStripMenuItem.Name = "DisplayLetterToolStripMenuItem"
        Me.DisplayLetterToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.DisplayLetterToolStripMenuItem.Text = "Display Letter"
        '
        'DataGridTableStyle1
        '
        Me.DataGridTableStyle1.DataGrid = Me.LettersHistoryDataGrid
        Me.DataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText
        '
        'LETTERSHistoryControl
        '
        Me.Controls.Add(Me.LettersHistoryDataGrid)
        Me.Name = "LETTERSHistoryControl"
        Me.Size = New System.Drawing.Size(424, 344)
        CType(Me.LettersHistoryDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LettersHistoryContextMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Public Properties"
    <System.ComponentModel.Description("Represents the application key used when accessing the registry.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID.")>
    Public Property FamilyID() As Integer?
        Get
            Return _FamilyID
        End Get
        Set(ByVal value As Integer?)
            _FamilyID = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the RelationID.")>
    Public Property RelationID() As Short?
        Get
            Return _RelationID
        End Get
        Set(ByVal value As Short?)
            _RelationID = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the ClaimID.")>
    Public Property ClaimID() As Integer?
        Get
            Return _ClaimID
        End Get
        Set(ByVal value As Integer?)
            _ClaimID = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Returns the generated LetterID.")>
    Public Property LetterID() As Integer
        Get
            Return _LetterID
        End Get
        Set(ByVal value As Integer)
            _LetterID = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Returns the generated MaxID.")>
    Public ReadOnly Property MaxID() As String
        Get
            Return _MaxID
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Returns the generated DocID.")>
    Public ReadOnly Property DocID() As Long
        Get
            Return _DocID
        End Get
    End Property
#End Region

#Region "Constructor"
    Public Sub New(ByVal claimID As Integer)
        Me.New()

        _ClaimID = claimID
    End Sub

#End Region

#Region "Form\Button Events"

    Private Sub LetterHistoryControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        _Loading = False
    End Sub
    Private Sub CancelRequestMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelRequestMenuItem.Click

        Dim Transaction As DbTransaction
        Dim BM As BindingManagerBase
        Dim DR As DataRow

        Try
            If MessageBox.Show("Are you sure you want to cancel the print request for this item?", "Cancel Print Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                BM = LettersHistoryDataGrid.BindingContext(LettersHistoryDataGrid.DataSource, LettersHistoryDataGrid.DataMember)
                DR = CType(BM.Current, DataRowView).Row

                _LetterID = CInt(DR("LETTERID"))
                _MaxID = CStr(DR("MAXID"))

                Transaction = LettersDAL.BeginTransaction()

                If _APPKEY.Split(CType("\", Char()))(1).ToUpper = "MEDICAL" Then
                    LettersDAL.UpdateLetters(CInt(DR("LETTERID")), If(DR("MAIL_STATUS").ToString.Trim = "T", "S", "X"), _DomainUser.ToUpper, CDate(DR("LASTUPDT")), "MEDICAL", Transaction)
                    LettersDAL.CreateClaimHistory(Nothing, 0, "PRINTCANCELLED", CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), "", "LETTERID :" & CStr(LettersHistoryDataGrid.Item(_HoverCell.RowNumber, 0)) & " updated.", "MAIL_STATUS updated. " & "Adjuster " & _DomainUser & " cancelled the letter request.", Transaction)
                ElseIf _APPKEY.Split(CType("\", Char()))(1).ToUpper = "CLAIMS" Then
                    LettersDAL.UpdateLetters(CInt(DR("LETTERID")), If(DR("MAIL_STATUS").ToString.Trim = "T", "S", "X"), _DomainUser.ToUpper, CDate(DR("LASTUPDT")), "CLAIMS", Transaction)
                    LettersDAL.CreateClaimHistory(CInt(DR("CLAIM_ID")), 0, "PRINTCANCELLED", CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), "", "LETTERID :" & CStr(LettersHistoryDataGrid.Item(_HoverCell.RowNumber, 0)) & " updated.", "MAIL_STATUS updated. " & "Adjuster " & _DomainUser & " cancelled the letter request.", Transaction)
                ElseIf _APPKEY.Split(CType("\", Char()))(1).ToUpper = "ELIGIBILITY" Then
                    LettersDAL.UpdateLetters(CInt(DR("LETTERID")), If(DR("MAIL_STATUS").ToString.Trim = "T", "S", "X"), _DomainUser.ToUpper, CDate(DR("LASTUPDT")), "ELIGIBILITY", Transaction)
                    LettersDAL.CreateEligibilityHistory(0, "PRINTCANCELLED", CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), "", "LETTERID :" & CStr(LettersHistoryDataGrid.Item(_HoverCell.RowNumber, 0)) & " updated.", "MAILSTATUS updated. " & "Adjuster " & _DomainUser & " cancelled the letter request.", Transaction)
                ElseIf _APPKEY.Split(CType("\", Char()))(1).ToUpper = "PENSION" Then
                    LettersDAL.UpdateLetters(CInt(DR("LETTERID")), If(DR("MAIL_STATUS").ToString.Trim = "T", "S", "X"), _DomainUser.ToUpper, CDate(DR("LASTUPDT")), "PENSION", Transaction)
                End If

                LettersDAL.CommitTransaction(Transaction)

                LoadLettersHistoryDS()
                LettersHistoryDataGrid.CaptionText = "Letter print request cancelled ID/MaxID( " & CStr(Me.LetterID) & " / " & Me.MaxID & ")"

            End If

        Catch ex As DDTek.DB2.DB2Exception When ex.SQLState = "20028"

            MessageBox.Show("Item has been updated by another. Requested change cannot be completed.", "Cancel Print Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        Catch ex As Exception
            Try

                If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing AndAlso Transaction.Connection.State <> ConnectionState.Closed Then
                    LettersDAL.RollbackTransaction(Transaction)
                End If

            Finally
            End Try

            Throw
        Finally
            If Transaction IsNot Nothing Then Transaction.Dispose()
            Transaction = Nothing

        End Try

    End Sub
    Private Sub DeleteRequestMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeleteRequestMenuItem.Click
        Dim Transaction As DbTransaction
        Dim BM As BindingManagerBase
        Dim DR As DataRow

        Try

            If MessageBox.Show("Are you sure you want to delete the request for this item?", "delete Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                BM = LettersHistoryDataGrid.BindingContext(LettersHistoryDataGrid.DataSource, LettersHistoryDataGrid.DataMember)
                DR = CType(BM.Current, DataRowView).Row

                _LetterID = CInt(DR("LETTERID"))
                _MaxID = CStr(DR("MAXID"))

                Transaction = LettersDAL.BeginTransaction()

                If _APPKEY.Split(CType("\", Char()))(1).ToUpper = "MEDICAL" Then
                    LettersDAL.UpdateLetters(CInt(DR("LETTERID")), "D", _DomainUser.ToUpper, CDate(DR("LASTUPDT")), "MEDICAL", Transaction)
                    LettersDAL.CreateClaimHistory(CInt(DR("CLAIM_ID")), 0, "DELETED", CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), "", "LETTERID :" & CStr(LettersHistoryDataGrid.Item(_HoverCell.RowNumber, 0)) & " Deleted.", "MAIL_STATUS updated to: D. " & "Adjuster " & _DomainUser & " deleted the letter request.", Transaction)
                ElseIf _APPKEY.Split(CType("\", Char()))(1).ToUpper = "CLAIMS" Then
                    LettersDAL.UpdateLetters(CInt(DR("LETTERID")), "D", _DomainUser.ToUpper, CDate(DR("LASTUPDT")), "MEDICAL", Transaction)
                    LettersDAL.CreateClaimHistory(CInt(DR("CLAIM_ID")), 0, "DELETED", CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), "", "LETTERID :" & CStr(LettersHistoryDataGrid.Item(_HoverCell.RowNumber, 0)) & " Deleted.", "MAIL_STATUS updated to: D. " & "Adjuster " & _DomainUser & " deleted the letter request.", Transaction)
                ElseIf _APPKEY.Split(CType("\", Char()))(1).ToUpper = "ELIGIBILITY" Then
                    LettersDAL.UpdateLetters(CInt(DR("LETTERID")), "D", _DomainUser.ToUpper, CDate(DR("LASTUPDT")), "ELIGIBILITY", Transaction)
                    LettersDAL.CreateEligibilityHistory(Nothing, "DELETED", CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), "", "LETTERID :" & CStr(LettersHistoryDataGrid.Item(_HoverCell.RowNumber, 0)) & " Deleted.", "MAIL_STATUS updated to: D. " & "Adjuster " & _DomainUser & " deleted the letter request.", Transaction)
                ElseIf _APPKEY.Split(CType("\", Char()))(1).ToUpper = "PENSION" Then
                    LettersDAL.UpdateLetters(CInt(DR("LETTERID")), "D", _DomainUser.ToUpper, CDate(DR("LASTUPDT")), "PENSION", Transaction)
                End If

                LettersDAL.CommitTransaction(Transaction)

                LoadLettersHistoryDS()
                LettersHistoryDataGrid.CaptionText = "Letter print request deleted id/maxid( " & CStr(Me.LetterID) & " / " & Me.MaxID & ")"
            End If

        Catch ex As DDTek.DB2.DB2Exception When ex.SQLState = "20028"

            MessageBox.Show("Item has been updated by another. Requested change cannot be completed.", "Cancel Delete Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        Catch ex As Exception
            Try

                If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing AndAlso Transaction.Connection.State <> ConnectionState.Closed Then
                    LettersDAL.RollbackTransaction(Transaction)
                End If

            Finally
            End Try

            Throw
        Finally
            If Transaction IsNot Nothing Then Transaction.Dispose()
            Transaction = Nothing

        End Try
    End Sub
    Private Sub ReprintMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ReprintMenuItem.Click
        Dim Transaction As DbTransaction
        Dim BM As BindingManagerBase
        Dim DR As DataRow

        Try
            BM = LettersHistoryDataGrid.BindingContext(LettersHistoryDataGrid.DataSource, LettersHistoryDataGrid.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            _LetterID = CInt(DR("LETTERID"))
            _MaxID = CStr(DR("MAXID"))

            Transaction = LettersDAL.BeginTransaction()

            If _APPKEY.Split(CType("\", Char()))(1).ToUpper = "CLAIMS" Then
                LettersDAL.UpdateLetters(CInt(DR("LETTERID")), "T", _DomainUser.ToUpper, CDate(DR("LASTUPDT")), "CLAIMS", Transaction)
                LettersDAL.CreateClaimHistory(UFCWGeneral.IsNullIntegerHandler(DR("CLAIM_ID")), CLng(DR("DOCID")), "REPRINTLTR", CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), "", "LETTERID :" & CStr(LettersHistoryDataGrid.Item(_HoverCell.RowNumber, 0)) & " updated.", " MAIL_STATUS updated to: T." & " Adjuster " & _DomainUser & " requested a re-print.", Transaction)
            ElseIf _APPKEY.Split(CType("\", Char()))(1).ToUpper = "MEDICAL" Then
                LettersDAL.UpdateLetters(CInt(DR("LETTERID")), "T", _DomainUser.ToUpper, CDate(DR("LASTUPDT")), "MEDICAL", Transaction)
                LettersDAL.CreateClaimHistory(Nothing, CLng(DR("DOCID")), "REPRINTLTR", CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), "", "LETTERID :" & CStr(LettersHistoryDataGrid.Item(_HoverCell.RowNumber, 0)) & " updated.", " MAIL_STATUS updated to: T." & " Adjuster " & _DomainUser & " requested a re-print.", Transaction)
            ElseIf _APPKEY.Split(CType("\", Char()))(1).ToUpper = "ELIGIBILITY" Then
                LettersDAL.UpdateLetters(CInt(DR("LETTERID")), "T", _DomainUser.ToUpper, CDate(DR("LASTUPDT")), "ELIGIBILITY", Transaction)
                LettersDAL.CreateEligibilityHistory(CLng(DR("DOCID")), "REPRINTLTR", CInt(DR("FAMILY_ID")), CShort(DR("RELATION_ID")), CInt(DR("PART_SSN")), CInt(DR("PAT_SSN")), "", "LETTERID :" & CStr(LettersHistoryDataGrid.Item(_HoverCell.RowNumber, 0)) & " updated.", " MAIL_STATUS updated to: T." & " Adjuster " & _DomainUser & " requested a re-print.", Transaction)
            ElseIf _APPKEY.Split(CType("\", Char()))(1).ToUpper = "PENSION" Then
                LettersDAL.UpdateLetters(CInt(DR("LETTERID")), "T", _DomainUser.ToUpper, CDate(DR("LASTUPDT")), "PENSION", Transaction)
            End If

            LettersDAL.CommitTransaction(Transaction)

            LoadLettersHistoryDS()
            LettersHistoryDataGrid.CaptionText = "Letter scheduled for Reprint ID/MaxID( " & CStr(Me.LetterID) & " / " & Me.MaxID & ")"

        Catch ex As DDTek.DB2.DB2Exception When ex.SQLState = "20028"

            MessageBox.Show("Item has been updated by another. Requested change cannot be completed.", "Cancel RePrint Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        Catch ex As Exception

            Try

                If Transaction IsNot Nothing AndAlso Transaction.Connection IsNot Nothing AndAlso Transaction.Connection.State <> ConnectionState.Closed Then
                    LettersDAL.RollbackTransaction(Transaction)
                End If

            Finally
            End Try

            Throw
        Finally
            If Transaction IsNot Nothing Then Transaction.Dispose()
            Transaction = Nothing

        End Try
    End Sub
    Private Sub DisplayLetterToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DisplayLetterToolStripMenuItem.Click
        Dim BM As BindingManagerBase
        Dim DR As DataRow

        Try
            BM = LettersHistoryDataGrid.BindingContext(LettersHistoryDataGrid.DataSource, LettersHistoryDataGrid.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            _DocID = CInt(DR("DocID"))

            If IsDBNull(DR("DOCID")) Then
                MessageBox.Show("There is no document to display.", "No Document", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else


                Using FNDisplay As New Display
                    FNDisplay.Display(New List(Of Long?) From {CLng(DR("DOCID"))})
                End Using

            End If

        Catch ex As ApplicationException

            MessageBox.Show(ex.Message, "FileNet unavailable, Restarting application may resolve connectivity issues.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch ex As TimeoutException

            MessageBox.Show(ex.Message, "FileNet unavailable, Restarting application may resolve connectivity issues.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch ex As Exception
            Throw
        End Try

    End Sub
    'Private Sub LettersHistoryDataGrid_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles LettersHistoryDataGrid.MouseDown

    '    Dim DG As DataGridCustom = CType(sender, DataGridCustom)

    '    If DG.DataSource Is Nothing Then Exit Sub

    '    Dim HTI As DataGrid.HitTestInfo
    '    HTI = DG.HitTest(e.X, e.Y)

    '    If (e.Button <> MouseButtons.Left) Then

    '        ' Check if the target is a different ROW or CELL from the previous one
    '        If (HTI.Type = DataGrid.HitTestType.Cell And HTI.Row <> _HoverCell.RowNumber) OrElse (HTI.Type = DataGrid.HitTestType.RowHeader And HTI.Row <> _HoverCell.RowNumber) Then

    '            ' Store the new hit row
    '            _HoverCell.RowNumber = HTI.Row
    '            _HoverCell.ColumnNumber = HTI.Column
    '            DG.Select(_HoverCell.RowNumber)
    '        Else
    '            ReprintMenuItem.Visible = False
    '            CancelRequestMenuItem.Visible = False
    '            DeleteRequestMenuItem.Visible = False
    '        End If
    '    End If

    '    Try

    '        Select Case HTI.Type
    '            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
    '                If DG.Item(_HoverCell.RowNumber, 4).ToString = "R " Then
    '                    CancelRequestMenuItem.Visible = True
    '                    ReprintMenuItem.Visible = False
    '                    DeleteRequestMenuItem.Visible = False
    '                ElseIf DG.Item(_HoverCell.RowNumber, 4).ToString = "X " Then
    '                    ReprintMenuItem.Visible = False
    '                    CancelRequestMenuItem.Visible = False
    '                    DeleteRequestMenuItem.Visible = False
    '                ElseIf DG.Item(_HoverCell.RowNumber, 4).ToString = "S " AndAlso LettersDAL.CMSCanOverrideAccumulatorsAccess Then
    '                    DeleteRequestMenuItem.Visible = True
    '                Else
    '                    ReprintMenuItem.Visible = True
    '                    CancelRequestMenuItem.Visible = False
    '                End If
    '            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
    '                If DG.Item(_HoverCell.RowNumber, 4).ToString = "R " Then
    '                    CancelRequestMenuItem.Visible = True
    '                    ReprintMenuItem.Visible = False
    '                    DeleteRequestMenuItem.Visible = False
    '                ElseIf DG.Item(_HoverCell.RowNumber, 4).ToString = "X " Then
    '                    ReprintMenuItem.Visible = False
    '                    CancelRequestMenuItem.Visible = False
    '                    DeleteRequestMenuItem.Visible = False
    '                ElseIf DG.Item(_HoverCell.RowNumber, 4).ToString = "S " AndAlso LettersDAL.CMSCanOverrideAccumulatorsAccess Then
    '                    DeleteRequestMenuItem.Visible = True
    '                Else
    '                    ReprintMenuItem.Visible = True
    '                    CancelRequestMenuItem.Visible = False
    '                End If
    '            Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption
    '            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize
    '            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader
    '        End Select

    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Sub
#End Region

#Region "Custom Subs\Functions"
    Public Sub LoadLettersHistory(ByVal claimID As Integer?)
        Try
            _ClaimID = claimID

            LoadLettersHistoryDS()

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Sub LoadLettersHistory(ByVal familyID As Integer, ByVal relationID As Short?)
        Try
            _FamilyID = familyID
            _RelationID = relationID

            LoadLettersHistoryDS()

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Sub ClearLettersHistory()

        If _LetterDS IsNot Nothing AndAlso _LetterDS.Tables.Contains("LETTERS") Then _LetterDS.Tables("LETTERS").Rows.Clear()
        LettersHistoryDataGrid.DataSource = Nothing

    End Sub
    Public Sub RefreshLettersHistory()
        LoadLettersHistoryDS()
    End Sub
    Private Sub LoadLettersHistoryDS()
        Try
            LettersHistoryDataGrid.DataSource = Nothing
            If _LetterDS IsNot Nothing AndAlso _LetterDS.Tables.Contains("LETTERS") Then _LetterDS.Tables("LETTERS").Rows.Clear()

            _LetterDS = If(_ClaimID IsNot Nothing, LettersDAL.GetLettersHistoryByCLAIMID(_ClaimID, _LetterDS), LettersDAL.GetLettersHistoryByFamilyIDRelationID(_APPKEY.Split(CType("\", Char()))(1), CInt(_FamilyID), _RelationID, _LetterDS))

            If _APPKEY.Split(CType("\", Char()))(1).ToUpper <> "CLAIMS" Then
                If _LetterDS.Tables("LETTERS").Columns.Contains("CLAIM_ID") Then _LetterDS.Tables("LETTERS").Columns.Remove("CLAIM_ID")
            End If
            Dim DV As DataView = _LetterDS.Tables("LETTERS").DefaultView
            DV.Sort = "CREATE_DATE DESC"

            LettersHistoryDataGrid.DataSource = DV

            LettersHistoryDataGrid.SetTableStyle()
            LettersHistoryDataGrid.ContextMenuPrepare(LettersHistoryContextMenu)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub LettersHistoryContextMenu_Opening(sender As Object, e As CancelEventArgs) Handles LettersHistoryContextMenu.Opening

        Dim DR As DataRow
        Dim DG As DataGridCustom

        Dim DataGridCustomContextMenu As ContextMenuStrip

        Try
            DataGridCustomContextMenu = CType(sender, ContextMenuStrip)

            DataGridCustomContextMenu.Items("CancelRequestMenuItem").Available = False
            DataGridCustomContextMenu.Items("ReprintMenuItem").Available = False
            DataGridCustomContextMenu.Items("DeleteRequestMenuItem").Available = False
            DataGridCustomContextMenu.Items("DisplayLetterToolStripMenuItem").Available = False

            DG = CType(DirectCast(sender, System.Windows.Forms.ContextMenuStrip).SourceControl, DataGridCustom)
            If DG IsNot Nothing AndAlso DG.DataSource IsNot Nothing Then

                DR = DG.SelectedRowPreview

                If DR IsNot Nothing Then

                    Select Case DR("MAIL_STATUS").ToString.Trim
                        Case "R" 'ready to print
                            If LettersDAL.REGMSupervisorAccess OrElse LettersDAL.CMSCanRunReports Then
                                DataGridCustomContextMenu.Items("DeleteRequestMenuItem").Available = True
                            End If
                            DataGridCustomContextMenu.Items("CancelRequestMenuItem").Available = True
                        Case "X", "E" 'invalid / unable to print
                        Case "T" 'reprint requested
                            DataGridCustomContextMenu.Items("CancelRequestMenuItem").Available = True
                            If DR("DOCID").ToString.Trim.Length > 0 Then
                                DataGridCustomContextMenu.Items("DisplayLetterToolStripMenuItem").Available = True
                            End If
                        Case "S" 'printed
                            DataGridCustomContextMenu.Items("ReprintMenuItem").Available = True
                            If DR("DOCID").ToString.Trim.Length > 0 Then
                                DataGridCustomContextMenu.Items("DisplayLetterToolStripMenuItem").Available = True
                            End If
                        Case Else
                    End Select

                End If
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

#End Region
End Class