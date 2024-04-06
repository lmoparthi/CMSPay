Option Strict On

Imports System.ComponentModel
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class HRQControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer? = Nothing
    Private _RelationID As Short? = Nothing
    Private _APPKEY As String = "UFCW\Claims\"
    Private _Loading As Boolean = True

    Public Event BeforeRefresh(ByVal sender As Object, ByRef cancel As Boolean)
    Public Event AfterRefresh(ByVal sender As Object)

    Dim _HRQDS As New DataSet

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

                If _HRQDS IsNot Nothing Then
                    _HRQDS.Dispose()
                End If

                If HRQDataGrid IsNot Nothing AndAlso HRQDataGrid.DataSource IsNot Nothing Then
                    HRQDataGrid.TableStyles.Clear()
                    HRQDataGrid.DataSource = Nothing
                    HRQDataGrid.Dispose()
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
    Friend WithEvents HRQDataGrid As DataGridCustom

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.HRQDataGrid = New DataGridCustom()
        CType(Me.HRQDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'HRQDataGrid
        '
        Me.HRQDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.HRQDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.HRQDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.HRQDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.HRQDataGrid.ADGroupsThatCanFind = ""
        Me.HRQDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.HRQDataGrid.ADGroupsThatCanMultiSort = ""
        Me.HRQDataGrid.AllowAutoSize = True
        Me.HRQDataGrid.AllowColumnReorder = True
        Me.HRQDataGrid.AllowCopy = True
        Me.HRQDataGrid.AllowCustomize = True
        Me.HRQDataGrid.AllowDelete = False
        Me.HRQDataGrid.AllowDragDrop = False
        Me.HRQDataGrid.AllowEdit = False
        Me.HRQDataGrid.AllowExport = True
        Me.HRQDataGrid.AllowFilter = True
        Me.HRQDataGrid.AllowFind = True
        Me.HRQDataGrid.AllowGoTo = True
        Me.HRQDataGrid.AllowMultiSelect = True
        Me.HRQDataGrid.AllowMultiSort = False
        Me.HRQDataGrid.AllowNew = False
        Me.HRQDataGrid.AllowPrint = True
        Me.HRQDataGrid.AllowRefresh = False
        Me.HRQDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HRQDataGrid.AppKey = "UFCW\Claims\"
        Me.HRQDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.HRQDataGrid.CaptionForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.HRQDataGrid.CaptionText = "HRQ"
        Me.HRQDataGrid.ColumnHeaderLabel = Nothing
        Me.HRQDataGrid.ColumnRePositioning = False
        Me.HRQDataGrid.ColumnResizing = False
        Me.HRQDataGrid.ConfirmDelete = True
        Me.HRQDataGrid.CopySelectedOnly = True
        Me.HRQDataGrid.DataMember = ""
        Me.HRQDataGrid.DragColumn = 0
        Me.HRQDataGrid.ExportSelectedOnly = True
        Me.HRQDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.HRQDataGrid.HighlightedRow = Nothing
        Me.HRQDataGrid.IsMouseDown = False
        Me.HRQDataGrid.LastGoToLine = ""
        Me.HRQDataGrid.Location = New System.Drawing.Point(-4, 1)
        Me.HRQDataGrid.MultiSort = False
        Me.HRQDataGrid.Name = "HRQDataGrid"
        Me.HRQDataGrid.OldSelectedRow = Nothing
        Me.HRQDataGrid.ParentRowsVisible = False
        Me.HRQDataGrid.ReadOnly = True
        Me.HRQDataGrid.SetRowOnRightClick = True
        Me.HRQDataGrid.ShiftPressed = False
        Me.HRQDataGrid.SingleClickBooleanColumns = True
        Me.HRQDataGrid.Size = New System.Drawing.Size(440, 360)
        Me.HRQDataGrid.StyleName = ""
        Me.HRQDataGrid.SubKey = ""
        Me.HRQDataGrid.SuppressTriangle = False
        Me.HRQDataGrid.TabIndex = 8
        '
        'HRQControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.HRQDataGrid)
        Me.Name = "HRQControl"
        Me.Size = New System.Drawing.Size(433, 362)
        CType(Me.HRQDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Properties"
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")>
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
    Public Sub New(ByVal familyID As Integer, ByVal relationID As Short?)
        Me.New()
        _FamilyID = familyID
        _RelationID = relationID

        LoadHRQControl()
    End Sub
#End Region

#Region "Form\Button Events"
    Private Sub HRQControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            _Loading = False
        Catch ex As Exception

	Throw
        End Try
    End Sub

    Public Sub RefreshActivity()
        Try

            HRQDataGrid_RefreshGridData()

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Private Sub HRQDataGrid_RefreshGridData() Handles HRQDataGrid.RefreshGridData
        Try
            Dim Cancel As Boolean = False

            RaiseEvent BeforeRefresh(Me, Cancel)

            If Cancel = False Then
                LoadHRQControl()
            End If

            RaiseEvent AfterRefresh(Me)
        Catch ex As Exception

	Throw
        End Try
    End Sub

    Private Sub HRQControl_dispose(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Me.Dispose()
    End Sub
#End Region

#Region "Custom Subs\Functions"
    Public Sub LoadHRQControl(ByVal familyID As Integer, ByVal relationID As Short?)
        Try
            _FamilyID = familyID
            _RelationID = relationID

            LoadHRQControl()

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Public Sub LoadHRQControl(ByVal familyID As Integer, ByVal relationID As Short?, hrqDS As DataSet)
        Try
            _FamilyID = familyID
            _RelationID = relationID

            LoadHRQControl(hrqDS)

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Public Sub LoadHRQControl(Optional hrqDS As DataSet = Nothing)
        Try
            HRQDataGrid.SuspendLayout()

            HRQDataGrid.DataSource = Nothing
            _HRQDS.Tables.Clear()

            If hrqDS IsNot Nothing Then
                _HRQDS = hrqDS
            Else
                _HRQDS = HRADAL.GetHRQInformation(CInt(_FamilyID), _RelationID, _HRQDS)
            End If

            HRQDataGrid.DataSource = _HRQDS.Tables("HRQ")
            HRQDataGrid.Sort = If(HRQDataGrid.LastSortedBy, HRQDataGrid.DefaultSort)

            HRQDataGrid.SetTableStyle()

            HRQDataGrid.ResumeLayout()

        Catch ex As Exception

	Throw
        End Try
    End Sub

    Public Sub ClearAll()
        HRQDataGrid.DataSource = Nothing
    End Sub

#End Region

End Class