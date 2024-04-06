Option Strict On

Imports System.ComponentModel
Imports System.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class PremiumsHistoryControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer = -1
    Private _RelationID As Integer = -1
    Private _APPKEY As String = "UFCW\Claims\"
    Private _PremiumsDS As DataSet

    Public Event BeforeRefresh(ByVal sender As Object, ByRef Cancel As Boolean)
    Public Event AfterRefresh(ByVal sender As Object)

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.


            If PremiumsDataGrid IsNot Nothing Then
                PremiumsDataGrid.Dispose()
            End If
            PremiumsDataGrid = Nothing

            If _PremiumsDS IsNot Nothing Then
                _PremiumsDS.Dispose()
            End If
            _PremiumsDS = Nothing

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
    Friend WithEvents PremiumsDataGrid As DataGridCustom
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.PremiumsDataGrid = New DataGridCustom()
        CType(Me.PremiumsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PremiumsDataGrid
        '
        Me.PremiumsDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.PremiumsDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.PremiumsDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PremiumsDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PremiumsDataGrid.ADGroupsThatCanFind = ""
        Me.PremiumsDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PremiumsDataGrid.ADGroupsThatCanMultiSort = ""
        Me.PremiumsDataGrid.AllowAutoSize = True
        Me.PremiumsDataGrid.AllowColumnReorder = True
        Me.PremiumsDataGrid.AllowCopy = False
        Me.PremiumsDataGrid.AllowCustomize = True
        Me.PremiumsDataGrid.AllowDelete = False
        Me.PremiumsDataGrid.AllowDragDrop = False
        Me.PremiumsDataGrid.AllowEdit = False
        Me.PremiumsDataGrid.AllowExport = False
        Me.PremiumsDataGrid.AllowFilter = True
        Me.PremiumsDataGrid.AllowFind = False
        Me.PremiumsDataGrid.AllowGoTo = False
        Me.PremiumsDataGrid.AllowMultiSelect = False
        Me.PremiumsDataGrid.AllowMultiSort = False
        Me.PremiumsDataGrid.AllowNew = False
        Me.PremiumsDataGrid.AllowPrint = False
        Me.PremiumsDataGrid.AllowRefresh = False
        Me.PremiumsDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PremiumsDataGrid.AppKey = "UFCW\Claims\"
        Me.PremiumsDataGrid.AutoSaveCols = True
        Me.PremiumsDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.PremiumsDataGrid.CaptionForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.PremiumsDataGrid.CaptionText = "Payment History"
        Me.PremiumsDataGrid.ColumnHeaderLabel = Nothing
        Me.PremiumsDataGrid.ColumnRePositioning = False
        Me.PremiumsDataGrid.ColumnResizing = False
        Me.PremiumsDataGrid.ConfirmDelete = True
        Me.PremiumsDataGrid.CopySelectedOnly = True
        Me.PremiumsDataGrid.DataMember = ""
        Me.PremiumsDataGrid.DragColumn = 0
        Me.PremiumsDataGrid.ExportSelectedOnly = True
        Me.PremiumsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.PremiumsDataGrid.HighlightedRow = Nothing
        Me.PremiumsDataGrid.IsMouseDown = False
        Me.PremiumsDataGrid.LastGoToLine = ""
        Me.PremiumsDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.PremiumsDataGrid.MultiSort = False
        Me.PremiumsDataGrid.Name = "PremiumsDataGrid"
        Me.PremiumsDataGrid.OldSelectedRow = Nothing
        Me.PremiumsDataGrid.ParentRowsVisible = False
        Me.PremiumsDataGrid.ReadOnly = True
        Me.PremiumsDataGrid.SetRowOnRightClick = True
        Me.PremiumsDataGrid.ShiftPressed = False
        Me.PremiumsDataGrid.SingleClickBooleanColumns = True
        Me.PremiumsDataGrid.Size = New System.Drawing.Size(440, 360)
        Me.PremiumsDataGrid.Sort = Nothing
        Me.PremiumsDataGrid.StyleName = ""
        Me.PremiumsDataGrid.SubKey = ""
        Me.PremiumsDataGrid.SuppressTriangle = False
        Me.PremiumsDataGrid.TabIndex = 7
        '
        'PremiumsHistoryControl
        '
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Controls.Add(Me.PremiumsDataGrid)
        Me.Name = "PremiumsHistoryControl"
        Me.Size = New System.Drawing.Size(440, 360)
        CType(Me.PremiumsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
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

            PremiumsDataGrid_RefreshGridData()

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Private Sub PremiumsDataGrid_RefreshGridData() Handles PremiumsDataGrid.RefreshGridData
        Try
            Dim Cancel As Boolean = False

            RaiseEvent BeforeRefresh(Me, Cancel)

            If Cancel = False Then
                LoadPremiumsControl()
            End If

            RaiseEvent AfterRefresh(Me)
        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                'Throw
            End If
        End Try
    End Sub

    Private Sub PremiumsHistoryControl_dispose(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Me.Dispose()
    End Sub
#End Region

#Region "Custom Subs\Functions"
    Public Sub LoadPremiumsControl(ByVal familyID As Integer, Optional premiumsDataSet As DataSet = Nothing)
        Try
            _FamilyID = familyID

            If premiumsDataSet IsNot Nothing Then _PremiumsDS = premiumsDataSet

            LoadPremiumsControl()

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Throw
            End If
        End Try
    End Sub
    Public Sub LoadPremiumsControl()
        Try
            PremiumsDataGrid.SuspendLayout()
            PremiumsDataGrid.DataSource = Nothing

            If _PremiumsDS Is Nothing OrElse _PremiumsDS.Tables Is Nothing OrElse _PremiumsDS.Tables.Count < 1 Then
                _PremiumsDS = PremiumsDAL.GetPremiumInformation(_FamilyID, _PremiumsDS)
            End If

            PremiumsDataGrid.DataSource = _PremiumsDS.Tables("PREMSUM")
            PremiumsDataGrid.SetTableStyle()
            PremiumsDataGrid.Sort = If(PremiumsDataGrid.LastSortedBy, PremiumsDataGrid.DefaultSort)

            PremiumsDataGrid.ResumeLayout()

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Throw
            End If
        End Try
    End Sub

    Public Sub ClearAll()

        If PremiumsDataGrid IsNot Nothing Then
            PremiumsDataGrid.DataSource = Nothing
        End If

        _PremiumsDS = Nothing

    End Sub

#End Region

End Class