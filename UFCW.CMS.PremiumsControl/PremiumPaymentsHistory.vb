Option Strict On

Imports System.ComponentModel
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class PremiumPaymentsHistoryControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer = -1
    Private _RelationID As Integer = -1
    Private _APPKEY As String = "UFCW\Claims\"

    Public Event BeforeRefresh(ByVal sender As Object, ByRef Cancel As Boolean)
    Public Event AfterRefresh(ByVal sender As Object)

    Public Overloads Sub Dispose()

        ''If Not PremiumPaymentsDataSet Is Nothing Then PremiumPaymentsDataSet = Nothing
        ''PremiumPaymentsDataSet.Dispose()
        If PremiumPaymentsDataGrid IsNot Nothing Then
            PremiumPaymentsDataGrid.TableStyles.Clear()
            PremiumPaymentsDataGrid.DataSource = Nothing
            PremiumPaymentsDataGrid.Dispose()
        End If
        MyBase.Dispose()
    End Sub
#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'PremiumPaymentsControl overrides dispose to clean up the component list.
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
    Friend WithEvents _PremiumPaymentsDS As PremiumPaymentsDataSet
    Friend WithEvents PremiumPaymentsDataGrid As DataGridCustom
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.PremiumPaymentsDataGrid = New DataGridCustom()
        Me._PremiumPaymentsDS = New PremiumPaymentsDataSet()
        CType(Me.PremiumPaymentsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._PremiumPaymentsDS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PremiumPaymentsDataGrid
        '
        Me.PremiumPaymentsDataGrid.ADGroupsThatCanCopy = "CMSUsers"
        Me.PremiumPaymentsDataGrid.ADGroupsThatCanCustomize = "CMSUsers"
        Me.PremiumPaymentsDataGrid.ADGroupsThatCanExport = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PremiumPaymentsDataGrid.ADGroupsThatCanFilter = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PremiumPaymentsDataGrid.ADGroupsThatCanFind = ""
        Me.PremiumPaymentsDataGrid.ADGroupsThatCanPrint = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
        Me.PremiumPaymentsDataGrid.ADGroupsThatCanMultiSort = ""
        Me.PremiumPaymentsDataGrid.AllowAutoSize = True
        Me.PremiumPaymentsDataGrid.AllowColumnReorder = True
        Me.PremiumPaymentsDataGrid.AllowCopy = False
        Me.PremiumPaymentsDataGrid.AllowCustomize = True
        Me.PremiumPaymentsDataGrid.AllowDelete = False
        Me.PremiumPaymentsDataGrid.AllowDragDrop = False
        Me.PremiumPaymentsDataGrid.AllowEdit = False
        Me.PremiumPaymentsDataGrid.AllowExport = False
        Me.PremiumPaymentsDataGrid.AllowFilter = True
        Me.PremiumPaymentsDataGrid.AllowFind = True
        Me.PremiumPaymentsDataGrid.AllowGoTo = True
        Me.PremiumPaymentsDataGrid.AllowMultiSelect = False
        Me.PremiumPaymentsDataGrid.AllowMultiSort = False
        Me.PremiumPaymentsDataGrid.AllowNew = False
        Me.PremiumPaymentsDataGrid.AllowPrint = False
        Me.PremiumPaymentsDataGrid.AllowRefresh = False
        Me.PremiumPaymentsDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PremiumPaymentsDataGrid.AppKey = "UFCW\Claims\"
        Me.PremiumPaymentsDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.PremiumPaymentsDataGrid.CaptionForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.PremiumPaymentsDataGrid.CaptionText = "Premium Payments History"
        Me.PremiumPaymentsDataGrid.ColumnHeaderLabel = Nothing
        Me.PremiumPaymentsDataGrid.ColumnRePositioning = False
        Me.PremiumPaymentsDataGrid.ColumnResizing = False
        Me.PremiumPaymentsDataGrid.ConfirmDelete = True
        Me.PremiumPaymentsDataGrid.CopySelectedOnly = True
        Me.PremiumPaymentsDataGrid.DataMember = ""
        Me.PremiumPaymentsDataGrid.DragColumn = 0
        Me.PremiumPaymentsDataGrid.ExportSelectedOnly = True
        Me.PremiumPaymentsDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.PremiumPaymentsDataGrid.HighlightedRow = Nothing
        Me.PremiumPaymentsDataGrid.IsMouseDown = False
        Me.PremiumPaymentsDataGrid.LastGoToLine = ""
        Me.PremiumPaymentsDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.PremiumPaymentsDataGrid.MultiSort = False
        Me.PremiumPaymentsDataGrid.Name = "PremiumPaymentsDataGrid"
        Me.PremiumPaymentsDataGrid.OldSelectedRow = Nothing
        Me.PremiumPaymentsDataGrid.ParentRowsVisible = False
        Me.PremiumPaymentsDataGrid.ReadOnly = True
        Me.PremiumPaymentsDataGrid.SetRowOnRightClick = True
        Me.PremiumPaymentsDataGrid.ShiftPressed = False
        Me.PremiumPaymentsDataGrid.SingleClickBooleanColumns = True
        Me.PremiumPaymentsDataGrid.Size = New System.Drawing.Size(440, 360)
        Me.PremiumPaymentsDataGrid.StyleName = ""
        Me.PremiumPaymentsDataGrid.SubKey = ""
        Me.PremiumPaymentsDataGrid.SuppressTriangle = False
        Me.PremiumPaymentsDataGrid.TabIndex = 7
        '
        'PremiumPaymentsDataSet
        '
        Me._PremiumPaymentsDS.DataSetName = "PremiumPaymentsDataSet"
        Me._PremiumPaymentsDS.Locale = New System.Globalization.CultureInfo("en-US")
        Me._PremiumPaymentsDS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'PremiumPaymentsHistoryControl
        '
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Controls.Add(Me.PremiumPaymentsDataGrid)
        Me.Name = "PremiumPaymentsHistoryControl"
        Me.Size = New System.Drawing.Size(440, 360)
        CType(Me.PremiumPaymentsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._PremiumPaymentsDS, System.ComponentModel.ISupportInitialize).EndInit()
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
    Public Sub New(ByVal familyID As Integer, ByVal relationID As Integer)
        Me.New()
        _FamilyID = familyID
        _RelationID = relationID

        LoadPremiumPaymentsControl()

    End Sub
#End Region

#Region "Form\Button Events"
    Private Sub PremiumPaymentsControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        With PremiumPaymentsDataGrid
            .AllowCopy = False
            .AllowFind = False
            .AllowGoTo = False
            .AllowRefresh = False
            .AllowExport = False
        End With

    End Sub

    Public Sub RefreshActivity()
        Try

            PremiumPaymentsDataGrid_RefreshGridData()

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Sub PremiumPaymentsDataGrid_RefreshGridData() Handles PremiumPaymentsDataGrid.RefreshGridData
        Try
            Dim Cancel As Boolean = False

            RaiseEvent BeforeRefresh(Me, Cancel)

            If Cancel = False Then
                LoadPremiumPaymentsControl()
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

    Private Sub PremiumPaymentsHistoryControl_dispose(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Me.Dispose()
    End Sub
#End Region

#Region "Custom Subs\Functions"
    Public Sub LoadPremiumPaymentsControl(ByVal familyID As Integer)
        Try
            _FamilyID = familyID

            LoadPremiumPaymentsControl()

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Throw
            End If
        End Try
    End Sub
    Public Sub LoadPremiumPaymentsControl()
        Try
            PremiumPaymentsDataGrid.SuspendLayout()
            PremiumPaymentsDataGrid.DataSource = Nothing

            If _PremiumPaymentsDS Is Nothing Then
                _PremiumPaymentsDS = New PremiumPaymentsDataSet
            End If

            _PremiumPaymentsDS.Tables.Clear()

            _PremiumPaymentsDS = CType(PremiumsDAL.GetPremiumPaymentsInformation(_FamilyID, _PremiumPaymentsDS), PremiumPaymentsDataSet)

            PremiumPaymentsDataGrid.DataSource = _PremiumPaymentsDS.Tables(0)
            PremiumPaymentsDataGrid.SetTableStyle()
            PremiumPaymentsDataGrid.Sort = If(PremiumPaymentsDataGrid.LastSortedBy, PremiumPaymentsDataGrid.DefaultSort)

            PremiumPaymentsDataGrid.ResumeLayout()

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Throw
            End If
        End Try
    End Sub

    Public Sub ClearAll()

        If PremiumPaymentsDataGrid IsNot Nothing Then
            PremiumPaymentsDataGrid.DataSource = Nothing
        End If
        _PremiumPaymentsDS = Nothing
    End Sub

#End Region

End Class