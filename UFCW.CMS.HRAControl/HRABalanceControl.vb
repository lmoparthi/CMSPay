Imports System.ComponentModel
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class HRABalanceControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer? = Nothing
    Private _RelationID As Short? = Nothing
    Private _ClaimID As Integer? = Nothing
    Private _BalanceAmt As Decimal = 0
    Private _APPKEY As String = "UFCW\Claims\"
    Private _Loading As Boolean = True
    Private _BalanceDS As New DataSet

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

                If _BalanceDS IsNot Nothing Then
                    _BalanceDS.Dispose()
                End If

                If HRABalanceDataGrid IsNot Nothing Then
                    HRABalanceDataGrid.TableStyles.Clear()
                    HRABalanceDataGrid.DataSource = Nothing
                    HRABalanceDataGrid.Dispose()
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

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents HRABalanceDataGrid As DataGridCustom
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.HRABalanceDataGrid = New DataGridCustom()
        CType(Me.HRABalanceDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'HRABalanceDataGrid
        '
        Me.HRABalanceDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.HRABalanceDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.HRABalanceDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.HRABalanceDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.HRABalanceDataGrid.ADGroupsThatCanFind = ""
        Me.HRABalanceDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.HRABalanceDataGrid.ADGroupsThatCanMultiSort = ""
        Me.HRABalanceDataGrid.AllowAutoSize = True
        Me.HRABalanceDataGrid.AllowColumnReorder = False
        Me.HRABalanceDataGrid.AllowCopy = True
        Me.HRABalanceDataGrid.AllowCustomize = False
        Me.HRABalanceDataGrid.AllowDelete = False
        Me.HRABalanceDataGrid.AllowDragDrop = False
        Me.HRABalanceDataGrid.AllowEdit = False
        Me.HRABalanceDataGrid.AllowExport = True
        Me.HRABalanceDataGrid.AllowFilter = False
        Me.HRABalanceDataGrid.AllowFind = False
        Me.HRABalanceDataGrid.AllowGoTo = False
        Me.HRABalanceDataGrid.AllowMultiSelect = False
        Me.HRABalanceDataGrid.AllowMultiSort = False
        Me.HRABalanceDataGrid.AllowNavigation = False
        Me.HRABalanceDataGrid.AllowNew = False
        Me.HRABalanceDataGrid.AllowPrint = True
        Me.HRABalanceDataGrid.AllowRefresh = False
        Me.HRABalanceDataGrid.AllowSorting = False
        Me.HRABalanceDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HRABalanceDataGrid.AppKey = "UFCW\Claims\"
        Me.HRABalanceDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.HRABalanceDataGrid.CaptionForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.HRABalanceDataGrid.CaptionText = "HRA Balance"
        Me.HRABalanceDataGrid.ColumnHeaderLabel = Nothing
        Me.HRABalanceDataGrid.ColumnRePositioning = False
        Me.HRABalanceDataGrid.ColumnResizing = False
        Me.HRABalanceDataGrid.ConfirmDelete = True
        Me.HRABalanceDataGrid.CopySelectedOnly = True
        Me.HRABalanceDataGrid.DataMember = ""
        Me.HRABalanceDataGrid.DragColumn = 0
        Me.HRABalanceDataGrid.ExportSelectedOnly = True
        Me.HRABalanceDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.HRABalanceDataGrid.HighlightedRow = Nothing
        Me.HRABalanceDataGrid.IsMouseDown = False
        Me.HRABalanceDataGrid.LastGoToLine = ""
        Me.HRABalanceDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.HRABalanceDataGrid.MultiSort = False
        Me.HRABalanceDataGrid.Name = "HRABalanceDataGrid"
        Me.HRABalanceDataGrid.OldSelectedRow = Nothing
        Me.HRABalanceDataGrid.ParentRowsVisible = False
        Me.HRABalanceDataGrid.ReadOnly = True
        Me.HRABalanceDataGrid.SetRowOnRightClick = True
        Me.HRABalanceDataGrid.ShiftPressed = False
        Me.HRABalanceDataGrid.SingleClickBooleanColumns = True
        Me.HRABalanceDataGrid.Size = New System.Drawing.Size(440, 416)
        Me.HRABalanceDataGrid.StyleName = ""
        Me.HRABalanceDataGrid.SubKey = ""
        Me.HRABalanceDataGrid.SuppressTriangle = False
        Me.HRABalanceDataGrid.TabIndex = 9
        '
        'HRABalanceControl
        '
        Me.Controls.Add(Me.HRABalanceDataGrid)
        Me.Name = "HRABalanceControl"
        Me.Size = New System.Drawing.Size(440, 416)
        CType(Me.HRABalanceDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Properties"
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets the FamilyID HRA Remaining Balance.")>
    Public ReadOnly Property BalanceAmt() As Decimal
        Get
            Return _BalanceAmt
        End Get
    End Property
    Public Property FamilyID() As Integer?
        Get
            Return _FamilyID
        End Get
        Set(ByVal value As Integer?)
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
    Public Sub New(ByVal familyID As Integer?, ByVal relationID As Short?)
        Me.New()
        _FamilyID = familyID
        _RelationID = relationID

        HRABalance()
    End Sub
#End Region

#Region "Custom Subs\Functions"
    Public Sub RefreshBalance()
        Try

            HRABalance()

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Public Sub HRABalance(ByVal claimID As Integer)
        Try
            _ClaimID = claimID

            HRABalance()

        Catch ex As Exception

	Throw
        End Try
    End Sub
    Public Sub HRABalance(ByVal familyID As Integer, ByVal relationID As Short?)
        Try
            _FamilyID = familyID
            _RelationID = relationID

            HRABalance()

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Public Sub HRABalance(ByVal familyID As Integer, ByVal relationID As Short?, balanceDS As DataSet)
        Try
            _FamilyID = familyID
            _RelationID = relationID

            HRABalance(balanceDS)

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Public Sub HRABalance(Optional balanceDS As DataSet = Nothing)
        Try

            HRABalanceDataGrid.SuspendLayout()

            If _ClaimID IsNot Nothing Then
                _BalanceDS = HRADAL.GetHRABalanceByClaimID(CInt(_ClaimID))
            Else
                If balanceDS IsNot Nothing Then
                    _BalanceDS = balanceDS
                Else
                    _BalanceDS = CMSDALFDBHRA.GetHRABalanceByFamilyIDRelationID(CInt(_FamilyID), _RelationID)
                End If
            End If

            HRABalanceDataGrid.DataSource = _BalanceDS.Tables("HRABalance")

            HRABalanceDataGrid.SetTableStyle()

            If Not IsDBNull(_BalanceDS.Tables("HRABalance").Rows(0)(0)) Then
                _BalanceAmt = CDec(_BalanceDS.Tables("HRABalance").Rows(0)(0))
            Else
                _BalanceAmt = 0
            End If

            HRABalanceDataGrid.ResumeLayout()

        Catch ex As Exception

	Throw

        End Try
    End Sub

    Public Sub ClearAll()

        HRABalanceDataGrid.SuspendLayout()
        HRABalanceDataGrid.DataSource = Nothing
        HRABalanceDataGrid.ResumeLayout()
        HRABalanceDataGrid.Refresh()

    End Sub

    Private Sub HRABalanceControl_dispose(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Me.Dispose()
    End Sub

    Private Sub HRABalanceControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            _Loading = False
        Catch ex As Exception

	Throw
        End Try

    End Sub

#End Region

End Class