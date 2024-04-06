Option Strict On

Imports System.Reflection
Imports System.Drawing.Imaging
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Text
Imports Microsoft.Win32
Imports System.Configuration
Imports System.IO
Imports System.Runtime.CompilerServices

'<System.Diagnostics.DebuggerStepThrough()>
Public Class DataGridCustom
    Inherits System.Windows.Forms.DataGrid

#Region "Run Time Properties"

    Private _AutoSaveCols As Boolean = True
    Private _AllowAutoSize As Boolean = True
    Private _AllowColumnReorder As Boolean = True
    Private _AllowCopy As Boolean = True
    Private _AllowCustomize As Boolean = True
    Private _AllowDelete As Boolean = True
    Private _AllowDragDrop As Boolean = False
    Private _AllowEdit As Boolean = True
    Private _AllowExport As Boolean = True
    Private _AllowFilter As Boolean = True
    Private _AllowFind As Boolean = True
    Private _AllowGoTo As Boolean = True
    Private _AllowMultiSelect As Boolean = True
    Private _AllowMultiSort As Boolean = False
    Private _AllowNew As Boolean = True
    Private _AllowPrint As Boolean = True
    Private _AllowRefresh As Boolean = True

    Private _ConfirmDelete As Boolean = True
    Private _CopySelectedOnly As Boolean = False
    Private _Export_SelectedOnly As Boolean = False
    Private _MultiSortAllowed As Boolean = False
    Private _SetRowOnClick As Boolean = True
    Private _SingleClickBooleanCols As Boolean = True

    Private _ADGroupsThatCanExport As String = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
    Private _ADGroupsThatCanPrint As String = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
    Private _ADGroupsThatCanFind As String = ""
    Private _ADGroupsThatCanMultiSort As String = ""
    Private _ADGroupsThatCanCopy As String = "CMSUsers"
    Private _ADGroupsThatCanCustomize As String = "CMSUsers"
    Private _ADGroupsThatCanFilter As String = "CMSCanAudit,CMSCanRunReports,CMSAdministrators"
    Private _FilterMappingName As String
    Private _FilterColumnHeading As String
    Private _FilterValue As String
    Private _FilterFormattedValue As String
    Private _LastGoToLine As Object = ""
    Private _SuppressTriangle As Boolean = False
    Private _ShowModifiedRows As Boolean = False
    Private _SuppressMouseClick As Boolean = False
    Private _DefaultViewFilter As String
    Private _DefaultSort As String = Nothing
    Private _XMLFileName As String = Me.Name
    Private _ImageList As System.Windows.Forms.ImageList
    Private _SubKey As String = ""
    Private _StyleName As String = ""

    Private _APPKEY As String = "UFCW\Claims\"

    Private _FindDialog As FindDialog
    Private _DoubleClick As Boolean = False
    Private _IsMouseDown As Boolean = False
    Private _DragColumn As Integer
    Private _ColumnHeaderLabel As Label
    Private _DragPic As PictureBox

    Private _MultiSortString As String = ""
    Private _Disposed As Boolean = False
    Private _LastHitTestInfo As DataGrid.HitTestInfo

    Private _ShiftPressed As Boolean = False
    Private _MultiRowInProgress As Boolean = False
    Private _LastColPoint As New Point(3, 3)
    Private _ColumnResizing As Boolean = False
    Private _ColumnRePositioning As Boolean = False
    Private _ColumnSorting As Boolean = False
    Private _RetainRowSelectionAfterSort As Boolean = True

    Private _DraggedMouseDownRow As Integer = -(1)
    Private _DraggedMouseDownCol As Integer = -(1)

    Private _HighlightedCell As New DataGridCell(-1, -1)
    Private _HighlightedRow As Integer? = Nothing
    Private _LastRowCount As Integer? = Nothing

    Public _PreviousRowID As Int64?
    Public _PreviousSelectedDR As DataRow

    Private _PreviousBSPosition As Integer = -1
    Public _PreviousBSRowCount As Integer?
    Public _PreviousBSPositionDR As DataRow

    Public _CurrentRowID As Int64?
    Public _CurrentSelectedDR As DataRow

    Public _CurrentBSRowCount As Integer?
    Private _CurrentBSPosition As Integer = -1
    Public _CurrentBSPositionDR As DataRow

    Private _LastSelectedHitTestRow As Integer? = Nothing 'These are only set if row is selected
    Private _PreviousSelectedHitTestRow As Integer? = Nothing

    Private _LastColumnIndex As Integer? = Nothing

    Private _LastCell As New DataGridCell(-1, -1)

    Private _PreventKeys As New PreventionKeys
    Private _BMB As BindingManagerBase

#End Region
#Region "Run Time Events"
    Public Event OnDelete(ByRef Cancel As Boolean)
    Public Event RefreshGridData()
    Public Event ResetTableStyle(ByVal sender As Object, ByVal e As EventArgs)
    Public Event TableFilterApplied(ByVal dg As DataGridCustom, ByVal filterColumn As String, ByVal filterValue As String)
    Public Event GridRowDoubleClick(ByVal hitTestInfo As System.Windows.Forms.DataGrid.HitTestInfo)
    Public Event BeginAddNew(ByVal rowIndex As Integer)

    'associated to Bound datasource
    Public Event CurrentDataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    Public Event PositionChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    Public Event MouseDownSuppressed(ByVal sender As Object, ByVal e As System.EventArgs)
    Public Event MouseUpSuppressed(ByVal sender As Object, ByVal e As System.EventArgs)
    Public Event MouseClickSuppressed(ByVal sender As Object, ByVal e As System.EventArgs)

    Public Event GridSorted(ByVal sender As Object, ByVal currentRowChangedArgs As CurrentRowChangedEventArgs)
    Public Event CurrentItemChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    Public Event CurrentChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    Public Event CurrentRowChanged(ByVal newPosition As Integer, ByVal oldPosition As Integer)

    Public Event RowChanged(ByVal sender As Object, ByVal CurrentRowChangedInfo As CurrentRowChangedEventArgs)

    Public Event CurrentColumnChanged(ByVal lastColumnIndex As Integer?, ByVal currentColumnIndex As Integer?)

    Public Event RowCountChanged(ByVal previousRowCount As Integer?, ByVal currentRowCount As Integer)
    Public Event ColumnPositionChanged(ByVal column As DataGridColumnStyle, ByVal lastPosition As Integer?, ByVal newPosition As Integer)
    Public Event CurrentColumnResized(ByVal CurrentColumnIndex As Integer?)

    Public Event EscapePressed()
    Public Event EnterPressed(ByVal Cell As DataGridCell)
    Public Event PreventingKey(ByVal Key As System.Windows.Forms.Keys, ByRef Cancel As Boolean)
    Public Event KeyPushed(ByVal msg As System.Windows.Forms.Message, ByVal key As System.Windows.Forms.Keys, ByRef Cancel As Boolean)
    Public Event GridSortChanged(ByVal sorting As String)
    Public Event OnGoTo(ByRef Index As Integer)
#End Region

    <System.Diagnostics.DebuggerStepThrough()>
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)

#If DEBUG Then
        Select Case m.Msg
            Case NativeMethods.WM_MOUSELEAVE, NativeMethods.WM_MOUSEHOVER

                'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & m.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

                MyBase.WndProc(m)

                'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & m.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Case NativeMethods.WM_NCHITTEST, NativeMethods.WM_MOUSEMOVE, NativeMethods.WM_SETCURSOR, NativeMethods.WM_CTLCOLORSCROLLBAR, NativeMethods.WM_CTLCOLORSTATIC

                MyBase.WndProc(m)

            Case Else
                'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & m.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

                MyBase.WndProc(m)

                'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & m.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Select
#Else
        Call MyBase.WndProc(m)
#End If

    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)

        If _Disposed Then Return

        ' Release any managed resources here.
        If disposing Then

            If _PreventKeys IsNot Nothing Then _PreventKeys.Dispose()
            _PreventKeys = Nothing

            If _FindDialog IsNot Nothing Then _FindDialog.Dispose()
            _FindDialog = Nothing

            If _ColumnHeaderLabel IsNot Nothing Then _ColumnHeaderLabel.Dispose()
            _ColumnHeaderLabel = Nothing

            If _DragPic IsNot Nothing Then _DragPic.Dispose()
            _DragPic = Nothing

            If _BMB IsNot Nothing Then

                RemoveHandler _BMB.BindingComplete, AddressOf DataGridPlus_BindingComplete
                RemoveHandler _BMB.DataError, AddressOf DataGridPlus_DataError
                RemoveHandler _BMB.CurrentChanged, AddressOf DataGridPlus_CurrentChanged
                RemoveHandler _BMB.CurrentItemChanged, AddressOf DataGridPlus_CurrentItemChanged
                RemoveHandler _BMB.PositionChanged, AddressOf DataGridPlus_PositionChanged

            End If
            _BMB = Nothing

            If (components IsNot Nothing) Then
                components.Dispose()
            End If

        End If

        _Disposed = True
        ' Release any unmanaged resources not wrapped by safe handles here.

        ' Call the base class implementation.
        MyBase.Dispose(disposing)
    End Sub

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        OnLoad()

    End Sub


    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents IList As System.Windows.Forms.ImageList
    Friend WithEvents MenuAutoSize As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuFind As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuGoTo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuExport As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuExportSelectCols As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuPrint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuRefresh As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuMultiColSort As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuChooseSortColumns As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SortTmr As System.Timers.Timer
    Public WithEvents BlankPic As System.Windows.Forms.PictureBox
    Public WithEvents DefaultContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripSortSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripExportSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuCustomizeColumns As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuFilterByColumn As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripCustomizeSeparator As System.Windows.Forms.ToolStripSeparator
    Public WithEvents SubExportContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MenuExportSelected As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuExportAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripDeleteSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents SubAutoSizeContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MenuAutoSizeByWidth As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuAutoSizeByHeight As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuAutoSizeByBoth As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents ArrowPic As System.Windows.Forms.PictureBox

    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DataGridCustom))
        Me.MenuMultiColSort = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuChooseSortColumns = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuAutoSize = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuFind = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuGoTo = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuExport = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuExportSelectCols = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuPrint = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuRefresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.IList = New System.Windows.Forms.ImageList(Me.components)
        Me.ArrowPic = New System.Windows.Forms.PictureBox()
        Me.SortTmr = New System.Timers.Timer()
        Me.BlankPic = New System.Windows.Forms.PictureBox()
        Me.DefaultContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripDeleteSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuCustomizeColumns = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuFilterByColumn = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripCustomizeSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripSortSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripExportSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.SubExportContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuExportSelected = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuExportAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SubAutoSizeContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuAutoSizeByWidth = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuAutoSizeByHeight = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuAutoSizeByBoth = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.ArrowPic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SortTmr, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BlankPic, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DefaultContextMenu.SuspendLayout()
        Me.SubExportContextMenuStrip.SuspendLayout()
        Me.SubAutoSizeContextMenuStrip.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MenuMultiColSort
        '
        Me.MenuMultiColSort.Checked = True
        Me.MenuMultiColSort.CheckState = System.Windows.Forms.CheckState.Checked
        Me.MenuMultiColSort.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuChooseSortColumns})
        Me.MenuMultiColSort.Name = "MenuMultiColSort"
        Me.MenuMultiColSort.Size = New System.Drawing.Size(230, 22)
        Me.MenuMultiColSort.Text = "&Multi-Sort"
        Me.MenuMultiColSort.ToolTipText = "Displays a dialog allowing complex sort sequences"
        '
        'MenuChooseSortColumns
        '
        Me.MenuChooseSortColumns.Name = "MenuChooseSortColumns"
        Me.MenuChooseSortColumns.Size = New System.Drawing.Size(122, 22)
        Me.MenuChooseSortColumns.Text = "&Columns"
        '
        'MenuAutoSize
        '
        Me.MenuAutoSize.Name = "MenuAutoSize"
        Me.MenuAutoSize.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.MenuAutoSize.Size = New System.Drawing.Size(230, 22)
        Me.MenuAutoSize.Text = "&AutoSize All Columns"
        Me.MenuAutoSize.ToolTipText = "Changes width of columns to display all available content"
        '
        'MenuFind
        '
        Me.MenuFind.Name = "MenuFind"
        Me.MenuFind.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.MenuFind.Size = New System.Drawing.Size(230, 22)
        Me.MenuFind.Text = "&Find..."
        Me.MenuFind.ToolTipText = "Searchs current grid content for specified string"
        '
        'MenuGoTo
        '
        Me.MenuGoTo.Name = "MenuGoTo"
        Me.MenuGoTo.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.G), System.Windows.Forms.Keys)
        Me.MenuGoTo.Size = New System.Drawing.Size(230, 22)
        Me.MenuGoTo.Text = "&Go To Line..."
        Me.MenuGoTo.ToolTipText = "Jumps to line specified"
        '
        'MenuCopy
        '
        Me.MenuCopy.Name = "MenuCopy"
        Me.MenuCopy.Size = New System.Drawing.Size(230, 22)
        Me.MenuCopy.Text = "&Copy..."
        Me.MenuCopy.ToolTipText = "Copy selected text to clipboard"
        '
        'MenuDelete
        '
        Me.MenuDelete.Name = "MenuDelete"
        Me.MenuDelete.Size = New System.Drawing.Size(230, 22)
        Me.MenuDelete.Text = "&Delete"
        Me.MenuDelete.ToolTipText = "deletes selected items"
        '
        'MenuExport
        '
        Me.MenuExport.Name = "MenuExport"
        Me.MenuExport.Size = New System.Drawing.Size(230, 22)
        Me.MenuExport.Text = "E&xport..."
        Me.MenuExport.ToolTipText = "Exports selected columns to either Excel or delimited file"
        '
        'MenuExportSelectCols
        '
        Me.MenuExportSelectCols.Name = "MenuExportSelectCols"
        Me.MenuExportSelectCols.Size = New System.Drawing.Size(230, 22)
        Me.MenuExportSelectCols.Text = "&Select Export Cols..."
        Me.MenuExportSelectCols.ToolTipText = "Displays dialog where export columns for export can be selected"
        '
        'MenuPrint
        '
        Me.MenuPrint.Name = "MenuPrint"
        Me.MenuPrint.Size = New System.Drawing.Size(230, 22)
        Me.MenuPrint.Text = "&Print..."
        Me.MenuPrint.ToolTipText = "Prints current grid content to default printer"
        '
        'MenuRefresh
        '
        Me.MenuRefresh.Name = "MenuRefresh"
        Me.MenuRefresh.Size = New System.Drawing.Size(230, 22)
        Me.MenuRefresh.Text = "&Refresh"
        Me.MenuRefresh.ToolTipText = "Reruns data selection query"
        '
        'IList
        '
        Me.IList.ImageStream = CType(resources.GetObject("IList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.IList.TransparentColor = System.Drawing.Color.Transparent
        Me.IList.Images.SetKeyName(0, "")
        Me.IList.Images.SetKeyName(1, "")
        '
        'ArrowPic
        '
        Me.ArrowPic.BackColor = System.Drawing.Color.Transparent
        Me.ArrowPic.Location = New System.Drawing.Point(203, 17)
        Me.ArrowPic.Name = "ArrowPic"
        Me.ArrowPic.Size = New System.Drawing.Size(8, 8)
        Me.ArrowPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.ArrowPic.TabIndex = 0
        Me.ArrowPic.TabStop = False
        Me.ArrowPic.Visible = False
        '
        'SortTmr
        '
        Me.SortTmr.SynchronizingObject = Me
        '
        'BlankPic
        '
        Me.BlankPic.BackColor = System.Drawing.Color.Transparent
        Me.BlankPic.Image = CType(resources.GetObject("BlankPic.Image"), System.Drawing.Image)
        Me.BlankPic.Location = New System.Drawing.Point(390, 17)
        Me.BlankPic.Name = "BlankPic"
        Me.BlankPic.Size = New System.Drawing.Size(32, 32)
        Me.BlankPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.BlankPic.TabIndex = 0
        Me.BlankPic.TabStop = False
        Me.BlankPic.Visible = False
        '
        'DefaultContextMenu
        '
        Me.DefaultContextMenu.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.DefaultContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuDelete, Me.ToolStripDeleteSeparator, Me.MenuCustomizeColumns, Me.MenuFilterByColumn, Me.ToolStripCustomizeSeparator, Me.MenuMultiColSort, Me.ToolStripSortSeparator, Me.MenuAutoSize, Me.MenuCopy, Me.MenuFind, Me.MenuGoTo, Me.MenuRefresh, Me.ToolStripExportSeparator, Me.MenuExportSelectCols, Me.MenuExport, Me.MenuPrint})
        Me.DefaultContextMenu.Name = "DefaultContextMenu"
        Me.DefaultContextMenu.Size = New System.Drawing.Size(231, 292)
        '
        'ToolStripDeleteSeparator
        '
        Me.ToolStripDeleteSeparator.Name = "ToolStripDeleteSeparator"
        Me.ToolStripDeleteSeparator.Size = New System.Drawing.Size(227, 6)
        '
        'MenuCustomizeColumns
        '
        Me.MenuCustomizeColumns.Name = "MenuCustomizeColumns"
        Me.MenuCustomizeColumns.Size = New System.Drawing.Size(230, 22)
        Me.MenuCustomizeColumns.Text = "Customize Columns"
        Me.MenuCustomizeColumns.ToolTipText = "Hides/Adds column(s) from display"
        '
        'MenuFilterByColumn
        '
        Me.MenuFilterByColumn.Name = "MenuFilterByColumn"
        Me.MenuFilterByColumn.Size = New System.Drawing.Size(230, 22)
        Me.MenuFilterByColumn.Text = "Filter By Column"
        Me.MenuFilterByColumn.ToolTipText = "Applies a filter to what is displayed"
        '
        'ToolStripCustomizeSeparator
        '
        Me.ToolStripCustomizeSeparator.Name = "ToolStripCustomizeSeparator"
        Me.ToolStripCustomizeSeparator.Size = New System.Drawing.Size(227, 6)
        '
        'ToolStripSortSeparator
        '
        Me.ToolStripSortSeparator.Name = "ToolStripSortSeparator"
        Me.ToolStripSortSeparator.Size = New System.Drawing.Size(227, 6)
        '
        'ToolStripExportSeparator
        '
        Me.ToolStripExportSeparator.Name = "ToolStripExportSeparator"
        Me.ToolStripExportSeparator.Size = New System.Drawing.Size(227, 6)
        '
        'SubExportContextMenuStrip
        '
        Me.SubExportContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuExportSelected, Me.MenuExportAll})
        Me.SubExportContextMenuStrip.Name = "DefaultContextMenu"
        Me.SubExportContextMenuStrip.Size = New System.Drawing.Size(182, 48)
        '
        'MenuExportSelected
        '
        Me.MenuExportSelected.Name = "MenuExportSelected"
        Me.MenuExportSelected.Size = New System.Drawing.Size(181, 22)
        Me.MenuExportSelected.Text = "Export Selected data"
        '
        'MenuExportAll
        '
        Me.MenuExportAll.Name = "MenuExportAll"
        Me.MenuExportAll.Size = New System.Drawing.Size(181, 22)
        Me.MenuExportAll.Text = "Export ALL Data"
        '
        'SubAutoSizeContextMenuStrip
        '
        Me.SubAutoSizeContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuAutoSizeByWidth, Me.MenuAutoSizeByHeight, Me.MenuAutoSizeByBoth})
        Me.SubAutoSizeContextMenuStrip.Name = "DefaultContextMenu"
        Me.SubAutoSizeContextMenuStrip.Size = New System.Drawing.Size(270, 70)
        '
        'MenuAutoSizeByWidth
        '
        Me.MenuAutoSizeByWidth.Name = "MenuAutoSizeByWidth"
        Me.MenuAutoSizeByWidth.Size = New System.Drawing.Size(269, 22)
        Me.MenuAutoSizeByWidth.Text = "AutoSize By Column Width"
        '
        'MenuAutoSizeByHeight
        '
        Me.MenuAutoSizeByHeight.Name = "MenuAutoSizeByHeight"
        Me.MenuAutoSizeByHeight.Size = New System.Drawing.Size(269, 22)
        Me.MenuAutoSizeByHeight.Text = "AutoSize By Column Height"
        '
        'MenuAutoSizeByBoth
        '
        Me.MenuAutoSizeByBoth.Name = "MenuAutoSizeByBoth"
        Me.MenuAutoSizeByBoth.Size = New System.Drawing.Size(269, 22)
        Me.MenuAutoSizeByBoth.Text = "AutoSize By Column Height && Width"
        '
        'DataGridCustom
        '
        Me.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BackgroundColor = System.Drawing.SystemColors.Window
        Me.ContextMenuStrip = Me.DefaultContextMenu
        Me.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.Size = New System.Drawing.Size(150, 93)
        CType(Me.ArrowPic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SortTmr, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BlankPic, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DefaultContextMenu.ResumeLayout(False)
        Me.SubExportContextMenuStrip.ResumeLayout(False)
        Me.SubAutoSizeContextMenuStrip.ResumeLayout(False)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
#End Region

#Region "Public Properties"

    <System.ComponentModel.Browsable(True), System.ComponentModel.Description("Auto Save Col Width & Position.")>
    Public Property AutoSaveCols As Boolean
        Get
            Return _AutoSaveCols
        End Get
        Set(value As Boolean)
            _AutoSaveCols = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Current BindingSource Position.")>
    Public Property CurrentBSPosition As Integer
        Get
            Return _CurrentBSPosition
        End Get
        Set(value As Integer)
            _CurrentBSPosition = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Previous BindingSource Position.")>
    Public Property PreviousBSPosition As Integer
        Get
            Return _PreviousBSPosition
        End Get
        Set(value As Integer)
            _PreviousBSPosition = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Last Cell Highlighted.")>
    Public Property HighlightedCell As DataGridCell
        Get
            Return _HighlightedCell
        End Get
        Set(value As DataGridCell)
            _HighlightedCell = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Last Row Highlighted.")>
    Public Property HighlightedRow As Integer?
        Get
            Return _HighlightedRow
        End Get
        Set(value As Integer?)
            _HighlightedRow = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Prevention Keys")>
    Public ReadOnly Property PreventKeys As PreventionKeys
        Get
            Return _PreventKeys
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Last Cursor Location.")>
    Public ReadOnly Property LastHitSpot As System.Windows.Forms.DataGrid.HitTestInfo
        Get
            Return _LastHitTestInfo
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Last Cell accessed.")>
    Public Property LastCell As DataGridCell
        Get
            Return _LastCell
        End Get
        Set(ByVal Value As DataGridCell)

            _LastCell = Value

        End Set
    End Property

    <System.ComponentModel.Description("Specify groups that can use Filter context menu function.")>
    Public Overridable Property ADGroupsThatCanFilter() As String
        Get
            Return _ADGroupsThatCanFilter
        End Get
        Set(ByVal Value As String)

            _ADGroupsThatCanFilter = Value

        End Set
    End Property

    <System.ComponentModel.Description("Specify groups that can use Customize context menu function.")>
    Public Overridable Property ADGroupsThatCanCustomize() As String
        Get
            Return _ADGroupsThatCanCustomize
        End Get
        Set(ByVal Value As String)

            _ADGroupsThatCanCustomize = Value

        End Set
    End Property

    <System.ComponentModel.Description("Specify groups that can use export context menu function.")>
    Public Overridable Property ADGroupsThatCanExport() As String
        Get
            Return _ADGroupsThatCanExport
        End Get
        Set(ByVal Value As String)

            _ADGroupsThatCanExport = Value

        End Set
    End Property

    <System.ComponentModel.Description("Specify groups that can use Print context menu function.")>
    Public Overridable Property ADGroupsThatCanPrint() As String
        Get
            Return _ADGroupsThatCanPrint
        End Get
        Set(ByVal Value As String)

            _ADGroupsThatCanPrint = Value

        End Set
    End Property

    <System.ComponentModel.Description("Specify groups that can use Copy context menu function.")>
    Public Overridable Property ADGroupsThatCanCopy() As String
        Get
            Return _ADGroupsThatCanCopy
        End Get
        Set(ByVal Value As String)

            _ADGroupsThatCanCopy = Value

        End Set
    End Property

    <System.ComponentModel.Description("Specify groups that can use Find context menu function.")>
    Public Overridable Property ADGroupsThatCanFind() As String
        Get
            Return _ADGroupsThatCanFind
        End Get
        Set(ByVal Value As String)

            _ADGroupsThatCanFind = Value

        End Set
    End Property

    <System.ComponentModel.Description("Specify groups that can use Sort context menu function.")>
    Public Overridable Property ADGroupsThatCanMultiSort() As String
        Get
            Return _ADGroupsThatCanMultiSort
        End Get
        Set(ByVal Value As String)

            _ADGroupsThatCanMultiSort = Value

        End Set
    End Property

    <System.ComponentModel.Description("Specify the application grouping criteria (e.g Claims, Worflow, etc).")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal Value As String)
            _APPKEY = Value & If(Value.Trim.EndsWith("\"), "", "\")

        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Column Header Label.")>
    Public Overridable Property ColumnHeaderLabel() As Label
        Get
            Return _ColumnHeaderLabel
        End Get
        Set(ByVal Value As Label)
            _ColumnHeaderLabel = Value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Indicates Column Resizing state.")>
    Public Overridable Property ColumnResizing() As Boolean
        Get
            Return _ColumnResizing
        End Get
        Set(ByVal Value As Boolean)
            _ColumnResizing = Value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Indicates Column reposition Action in progress.")>
    Public Overridable Property ColumnRePositioning() As Boolean
        Get
            Return _ColumnRePositioning
        End Get
        Set(ByVal Value As Boolean)
            _ColumnRePositioning = Value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Indicates Row and Select status are maintained during Sort operations.")>
    Public Overridable Property RetainRowSelectionAfterSort() As Boolean
        Get
            Return _RetainRowSelectionAfterSort
        End Get
        Set(ByVal Value As Boolean)
            _RetainRowSelectionAfterSort = Value
        End Set
    End Property

    <System.ComponentModel.Description("Allows New Rows to be added by the user.")>
    Public Overridable Property AllowNew() As Boolean
        Get
            Return _AllowNew
        End Get
        Set(ByVal Value As Boolean)

            _AllowNew = Value

            If Me Is Nothing Then Exit Property
            If Me.DataSource Is Nothing Then Exit Property
            If TypeOf Me.DataSource Is DataSet AndAlso Me.DataMember = "" Then Exit Property

            If CType(DataSource, BindingSource) IsNot Nothing AndAlso CType(DataSource, BindingSource).AllowNew <> _AllowNew Then
                CType(DataSource, BindingSource).AllowNew = _AllowNew
            End If

            If GetCurrentDataView() IsNot Nothing AndAlso GetCurrentDataView.AllowNew <> _AllowNew Then
                GetCurrentDataView.AllowNew = _AllowNew
            End If

        End Set

    End Property

    <System.ComponentModel.Description("Enables Editing of datasource rows.")>
    Public Overridable Property AllowEdit() As Boolean
        Get
            Return _AllowEdit
        End Get
        Set(ByVal value As Boolean)

            _AllowEdit = value

            If Me Is Nothing Then Exit Property
            If Me.DataSource Is Nothing Then Exit Property

            If GetCurrentDataView() IsNot Nothing AndAlso GetCurrentDataView.AllowEdit <> _AllowEdit Then
                GetCurrentDataView.AllowEdit = _AllowEdit
            End If

        End Set

    End Property

    <System.ComponentModel.Description("Allows the user to delete rows. No specific data level setting and AllowEdit must also be True")>
    Public Overridable Property AllowDelete() As Boolean
        Get
            Return _AllowDelete
        End Get
        Set(ByVal value As Boolean)

            _AllowDelete = value

        End Set
    End Property

    <System.ComponentModel.Description("Allows the Grid to be AutoSized based upon the column contents.")>
    Public Overridable Property AllowAutoSize() As Boolean
        Get
            Return _AllowAutoSize
        End Get
        Set(ByVal Value As Boolean)
            _AllowAutoSize = Value
        End Set
    End Property

    <System.ComponentModel.Description("Allows the Find Dialog to be used to search for a string.")>
    Public Overridable Property AllowFind() As Boolean
        Get
            Return _AllowFind
        End Get
        Set(ByVal value As Boolean)
            _AllowFind = value
        End Set
    End Property

    <System.ComponentModel.Description("Allows the Customize Dialog to be used to limit columns displayed.")>
    Public Overridable Property AllowCustomize() As Boolean
        Get
            Return _AllowCustomize
        End Get
        Set(ByVal value As Boolean)
            _AllowCustomize = value
        End Set
    End Property

    <System.ComponentModel.Description("Allows the Filter Dialog to be used to limit values displayed based upon column values.")>
    Public Overridable Property AllowFilter() As Boolean
        Get
            Return _AllowFilter
        End Get
        Set(ByVal value As Boolean)
            _AllowFilter = value
        End Set
    End Property

    <System.ComponentModel.Description("Allows the GoTo Dialog to be used to navigate to a certain row.")>
    Public Overridable Property AllowGoTo() As Boolean
        Get
            Return _AllowGoTo
        End Get
        Set(ByVal Value As Boolean)
            _AllowGoTo = Value
        End Set
    End Property

    <System.ComponentModel.Description("Gets or sets the index of the last line the GoTo Dialog navigated to.")>
    Public Overridable Property LastGoToLine() As Object
        Get
            Return _LastGoToLine
        End Get
        Set(ByVal value As Object)
            _LastGoToLine = value

            If value.ToString <> "" Then
                RaiseEvent OnGoTo(CInt(value) - 1)
            End If
        End Set
    End Property

    <System.ComponentModel.Description("Allows the user to copy rows.")>
    Public Overridable Property AllowCopy() As Boolean
        Get
            Return _AllowCopy
        End Get
        Set(ByVal value As Boolean)
            _AllowCopy = value
        End Set
    End Property

    <System.ComponentModel.Description("Allows the user to export rows to text or Excel.")>
    Public Overridable Property AllowExport() As Boolean
        Get
            Return _AllowExport
        End Get
        Set(ByVal value As Boolean)
            _AllowExport = value
        End Set
    End Property

    <System.ComponentModel.Description("Allows the user to Print rows.")>
    Public Overridable Property AllowPrint() As Boolean
        Get
            Return _AllowPrint
        End Get
        Set(ByVal value As Boolean)
            _AllowPrint = value
        End Set
    End Property

    <System.ComponentModel.Description("Allows the user to refresh the grid data.")>
    Public Overridable Property AllowRefresh() As Boolean
        Get
            Return _AllowRefresh
        End Get
        Set(ByVal value As Boolean)
            _AllowRefresh = value
        End Set
    End Property

    <System.ComponentModel.Description("Allows the user to re-order columns.")>
    Public Overridable Property AllowColumnReorder() As Boolean
        Get
            Return _AllowColumnReorder
        End Get
        Set(ByVal value As Boolean)
            _AllowColumnReorder = value
        End Set
    End Property

    <System.ComponentModel.Description("Allows the user to select more than one row.")>
    Public Overridable Property AllowMultiSelect() As Boolean
        Get
            Return _AllowMultiSelect
        End Get
        Set(ByVal value As Boolean)
            _AllowMultiSelect = value
        End Set
    End Property

    <System.ComponentModel.Description("Allows the user to sort on more than one column.")>
    Public Overridable Property AllowMultiSort() As Boolean
        Get
            Return _AllowMultiSort
        End Get
        Set(ByVal value As Boolean)
            _AllowMultiSort = value
        End Set
    End Property

    <System.ComponentModel.Description("Allows the user to Drag and Drop.")>
    Public Overridable Property AllowDragDrop() As Boolean
        Get
            Return _AllowDragDrop
        End Get
        Set(ByVal value As Boolean)
            _AllowDragDrop = value

            Me.AllowDrop = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Indicates Mouse Button is Pressed.")>
    Public Overridable Property IsMouseDown() As Boolean
        Get
            Return _IsMouseDown
        End Get
        Set(ByVal value As Boolean)
            _IsMouseDown = value
        End Set
    End Property

    <System.ComponentModel.Description("Gets or Sets Multi-Sort mode.")>
    Public Overridable Property MultiSort() As Boolean
        Get
            Return _MultiSortAllowed
        End Get
        Set(ByVal value As Boolean)
            _MultiSortAllowed = value
        End Set
    End Property

    <System.ComponentModel.Description("Gets or Sets Copy Selected Rows Only mode.")>
    Public Overridable Property CopySelectedOnly() As Boolean
        Get
            Return _CopySelectedOnly
        End Get
        Set(ByVal value As Boolean)
            _CopySelectedOnly = value
        End Set
    End Property

    <System.ComponentModel.Description("Gets or Sets Confirm On Delete mode.")>
    Public Overridable Property ConfirmDelete() As Boolean
        Get
            Return _ConfirmDelete
        End Get
        Set(ByVal value As Boolean)
            _ConfirmDelete = value
        End Set
    End Property

    <System.ComponentModel.Description("Gets or Sets Export Selected Rows Only mode.")>
    Public Overridable Property ExportSelectedOnly() As Boolean
        Get
            Return _Export_SelectedOnly
        End Get
        Set(ByVal value As Boolean)
            _Export_SelectedOnly = value
        End Set
    End Property

    <System.ComponentModel.Description("Gets or Sets the stylename associated with the datagrid to use.")>
    Public Overridable Property StyleName() As String
        Get
            Return _StyleName
        End Get
        Set(ByVal value As String)
            _StyleName = value
        End Set
    End Property

    <System.ComponentModel.Description("Gets or Sets the registry subkey associated with the datagrid to use.")>
    Public Overridable Property SubKey() As String
        Get
            Return _SubKey
        End Get
        Set(ByVal value As String)
            _SubKey = value
        End Set
    End Property

    <System.ComponentModel.Description("Gets or Sets the ContextMenuStip of the Grid.")>
    Public Overrides Property ContextMenuStrip() As ContextMenuStrip
        Get
            Return MyBase.ContextMenuStrip
        End Get
        Set(ByVal value As ContextMenuStrip)
            MyBase.ContextMenuStrip = value
        End Set
    End Property

    <System.ComponentModel.Description("Gets or Sets the if Boolean Columns Value Change on Single Click.")>
    Public Overridable Property SingleClickBooleanColumns() As Boolean
        Get
            Return _SingleClickBooleanCols
        End Get
        Set(ByVal value As Boolean)
            _SingleClickBooleanCols = value
        End Set
    End Property

    <System.ComponentModel.Description("Gets or Sets the right-click sets current row mode.")>
    Public Overridable Property SetRowOnRightClick() As Boolean
        Get
            Return _SetRowOnClick
        End Get
        Set(ByVal value As Boolean)
            _SetRowOnClick = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Indicates Shift is Pressed.")>
    Public Overridable Property ShiftPressed() As Boolean
        Get
            Return _ShiftPressed
        End Get
        Set(ByVal value As Boolean)
            _ShiftPressed = value
        End Set
    End Property

    <System.ComponentModel.Description("Hides row carat in header")>
    Public Overridable Property SuppressTriangle() As Boolean
        Get
            Return _SuppressTriangle
        End Get
        Set(ByVal value As Boolean)
            _SuppressTriangle = value
        End Set
    End Property

    <System.ComponentModel.Description("Shows Row Modified Pencil even if Grid is declared Not Editable")>
    Public Overridable Property HighLightModifiedRows() As Boolean
        Get
            Return _ShowModifiedRows
        End Get
        Set(ByVal value As Boolean)
            _ShowModifiedRows = value
        End Set
    End Property

    <System.ComponentModel.Description("Instructs onMouseDown to take no action")>
    Public Overridable Property SuppressMouseDown() As Boolean
        Get
            Return _SuppressMouseClick
        End Get
        Set(ByVal value As Boolean)
            _SuppressMouseClick = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets the column selected to filter against.")>
    Public ReadOnly Property FilterMappingName() As String
        Get
            Return _FilterMappingName
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets the column value selected to filter against.")>
    Public ReadOnly Property FilterValue() As String
        Get
            Return _FilterValue
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets the default filter.")>
    Public ReadOnly Property DefaultViewFilter() As String
        Get
            Return _DefaultViewFilter
        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets the default Sort.")>
    Public ReadOnly Property DefaultSort(Optional ByVal xmlName As String = Nothing) As String
        Get
            Try

                If _DefaultSort Is Nothing AndAlso Me.Name IsNot Nothing Then
                    Using DGTS As DataSet = GetTableStyle(If(xmlName, Me.Name))

                        If DGTS.Tables.Count > 0 AndAlso DGTS.Tables(If(xmlName, Me.Name) & "Style") Is Nothing Then
                            Throw New ApplicationException("Grid Name: " & Me.Name & " does not match expected Style Name: " & If(xmlName, Me.Name) & "Style. Possible solutions 1. Move sort after setstyle, supply matching XML Name, validate top tag of XML or change Grid Name")
                        End If

                        If DGTS.Tables.Count > 0 AndAlso DGTS.Tables(If(xmlName, Me.Name) & "Style").Columns.Contains("GridDefaultSort") Then
                            _DefaultSort = DGTS.Tables(If(xmlName, Me.Name) & "Style").Rows(0)("GridDefaultSort").ToString
                        End If

                        Return _DefaultSort

                    End Using

                Else
                    Return _DefaultSort
                End If

            Catch ex As Exception
                Throw
            Finally
            End Try

        End Get
    End Property

    <System.ComponentModel.Description("Gets Or Sets the Height Of the Specified Row.")>
    Public Overridable Property RowHeight(ByVal row As Integer) As Integer
        Get
            Dim mi As MethodInfo = Me.GetType().BaseType.GetMethod("get_DataGridRows", BindingFlags.FlattenHierarchy Or BindingFlags.IgnoreCase Or BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public Or BindingFlags.Static)
            Dim DGRows As System.Array = CType(mi.Invoke(Me, Nothing), System.Array)

            If row < 0 Then row = 0
            If row > DGRows.GetLength(0) - 1 Then row = DGRows.GetLength(0) - 1

            If Me.GetGridRowCount > 0 Then
                Return CInt(DGRows.GetValue(row).GetType().GetProperty("Height").GetValue(DGRows.GetValue(row), Nothing))
            Else
                Return 0
            End If
        End Get
        Set(ByVal Value As Integer)

            Dim mi As MethodInfo = Me.GetType().BaseType.GetMethod("get_DataGridRows", BindingFlags.FlattenHierarchy Or BindingFlags.IgnoreCase Or BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public Or BindingFlags.Static)
            Dim DGRows As System.Array = CType(mi.Invoke(Me, Nothing), System.Array)

            If row < 0 Then Exit Property
            If row > DGRows.GetLength(0) - 1 Then Exit Property

            DGRows.GetValue(row).GetType().GetProperty("Height").SetValue(DGRows.GetValue(row), Value, Nothing)
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Indicates Column involved In Drag operation.")>
    Public Overridable Property DragColumn() As Integer
        Get
            Return _DragColumn
        End Get
        Set(ByVal value As Integer)

            _DragColumn = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Indicates the row BEING selected, any currencymanger associated to the datasource would not have been updated yet.")>
    Public ReadOnly Property SelectedRowPreview() As DataRow
        Get
            Dim DR As DataRow
            Dim DV As DataView

            Try

                If Me.LastHitSpot IsNot Nothing AndAlso Me.LastHitSpot.Row > -1 Then
                    DV = Me.GetCurrentDataView
                    If DV.Count > Me.LastHitSpot.Row Then DR = DV(Me.LastHitSpot.Row).Row
                End If

                Return DR

            Catch ex As Exception
                Throw
            End Try

        End Get
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Indicates Most recent row selected, Nothing = No row selected for current grid.")>
    Public Overridable Property OldSelectedRow() As Integer?
        Get
            Return _PreviousSelectedHitTestRow
        End Get
        Set(ByVal value As Integer?)

            _PreviousSelectedHitTestRow = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Is the last data row selected from bound datasource.")>
    Public Overridable ReadOnly Property LastSelectedRow() As DataRow
        Get
            Return _CurrentBSPositionDR
        End Get
    End Property

    <System.ComponentModel.Description("Gets Or Sets the Width of the Specified Column.")>
    Public Overridable Property ColumnWidth(ByVal column As Integer) As Integer
        Get
            Return Me.GetCurrentTableStyle.GridColumnStyles(column).Width
        End Get
        Set(ByVal value As Integer)
            Dim style As DataGridTableStyle = GetCurrentTableStyle()

            style.GridColumnStyles(column).Width = value
        End Set
    End Property

#End Region

#Region "Shared Methods"

    Public Shared Function IdentifyChanges(ByVal dr As DataRow, ByVal dg As DataGridCustom) As String

        Return UFCWGeneral.IdentifyChanges(dr, dg.GetCurrentTableStyle)

    End Function

    Public Shared Function IdentifyChanges(ByVal dr As DataRow, ByVal xmlStyleName As String) As String

        Return UFCWGeneral.IdentifyChanges(dr, TableStyleColumns(xmlStyleName))

    End Function

    Public Shared Function TableStyleColumns(ByRef xmlName As String) As DataGridTableStyle

        Dim DGTS As New DataGridTableStyle

        Dim TextCol As DataGridFormattableTextBoxColumn

        Dim ColsDV As DataView
        Dim DefaultStyleDS As DataSet

        Try

            DefaultStyleDS = GetTableStyle(xmlName)

            If DefaultStyleDS Is Nothing OrElse DefaultStyleDS.Tables.Count < 1 Then Return Nothing

            DGTS.MappingName = xmlName
            DGTS.GridColumnStyles.Clear()

            ColsDV = New DataView(DefaultStyleDS.Tables("Column"))

            For IntCol As Integer = 0 To ColsDV.Count - 1

                If (IsDBNull(ColsDV(IntCol).Item("Visible")) OrElse ColsDV(IntCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(IntCol).Item("Visible")) = True) Then
                    TextCol = New DataGridFormattableTextBoxColumn(IntCol) With {
                        .MappingName = CStr(ColsDV(IntCol).Item("Mapping")),
                        .HeaderText = CStr(ColsDV(IntCol).Item("HeaderText"))
                    }

                    If IsDBNull(ColsDV(IntCol).Item("Format")) = False AndAlso ColsDV(IntCol).Item("Format").ToString.Trim <> "" Then
                        TextCol.Format = CStr(ColsDV(IntCol).Item("Format"))
                    End If

                    DGTS.GridColumnStyles.Add(TextCol)
                End If

            Next

            Return DGTS

        Catch ex As Exception
        Finally

            If DefaultStyleDS IsNot Nothing Then DefaultStyleDS.Dispose()
            DefaultStyleDS = Nothing

            If TextCol IsNot Nothing Then TextCol.Dispose()
            TextCol = Nothing

            If ColsDV IsNot Nothing Then ColsDV.Dispose()
            ColsDV = Nothing

            If DGTS IsNot Nothing Then DGTS.Dispose()
            DGTS = Nothing

        End Try

    End Function

    Private Shared Function CreateDataSetFromXML(ByVal xmlFile As String) As DataSet

        'load xml into a dataset to use here
        Dim DS As DataSet
        Dim FileStream As FileStream


        'open the xml file so we can use it to fill the dataset
        Try
            DS = New DataSet

            FileStream = New FileStream(System.Windows.Forms.Application.StartupPath & "\" & xmlFile, FileMode.Open, FileAccess.Read)

            DS.ReadXml(FileStream)

            'add required columns if they were missing from xml
            If Not DS.Tables("Column").Columns.Contains("Visible") Then DS.Tables("Column").Columns.Add("Visible")
            If Not DS.Tables("Column").Columns.Contains("FormatIsRegEx") Then DS.Tables("Column").Columns.Add("FormatIsRegEx")
            If Not DS.Tables("Column").Columns.Contains("WordWrap") Then DS.Tables("Column").Columns.Add("WordWrap")
            If Not DS.Tables("Column").Columns.Contains("ReadOnly") Then DS.Tables("Column").Columns.Add("ReadOnly")
            If Not DS.Tables("Column").Columns.Contains("MinimumCharWidth") Then DS.Tables("Column").Columns.Add("MinimumCharWidth")
            If Not DS.Tables("Column").Columns.Contains("MaximumCharWidth") Then DS.Tables("Column").Columns.Add("MaximumCharWidth")

            Return DS

        Catch ex As Exception
            Throw
        Finally

            If FileStream IsNot Nothing Then FileStream.Close()
            FileStream = Nothing
            If DS IsNot Nothing Then DS.Dispose()
            DS = Nothing

        End Try

    End Function

    <System.ComponentModel.Description("Creates dataset with 2 tables representing Grid defaults and the associated style.")>
    Public Shared Function GetTableStyle(ByVal xmlName As String) As DataSet

        Dim XMLStyleFile As String

        Try

            If ConfigurationManager.GetSection(xmlName & If(xmlName.Contains(".xml"), "", ".xml")) IsNot Nothing Then
                XMLStyleFile = CStr(CType(ConfigurationManager.GetSection(xmlName & If(xmlName.Contains(".xml"), "", ".xml")), IDictionary)("StyleLocation"))
            Else
                XMLStyleFile = xmlName & If(xmlName.Contains(".xml"), "", ".xml")
            End If

            Return CreateDataSetFromXML(XMLStyleFile)

        Catch ex As Exception When TypeOf (ex) Is NullReferenceException OrElse TypeOf (ex) Is FileNotFoundException
            MessageBox.Show("Please check < " & XMLStyleFile & " > entry in Config files, and check for file in execution folder. ", "Missing Entry", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Return New DataSet

        Catch ex As Exception

            Throw
        End Try

    End Function

    Private Shared Function GetColumns(ByVal appKEY As String, ByVal sectionKey As String) As String()

        Dim Settings As String(,)
        Dim FilteredSettings As New ArrayList
        Dim GetUserGroups As StringCollection

        Try

            GetUserGroups = UFCWGeneralAD.GetUserGroupMembership()

            Settings = GetAllSettings(appKEY, sectionKey)
            If Settings IsNot Nothing Then

                For X As Integer = 0 To UBound(Settings) 'ignore 2nd dimension
                    Dim ColName As String = Replace(Replace(Settings(X, 0), "Col ", ""), " Export", "")

                    If GetUserGroups.Contains("CMSLocalsAccess") Then
                        Select Case ColName.Trim
                            Case "USERID", "CREATE_USERID"
                                Continue For
                        End Select
                    End If

                    If Not GetUserGroups.Contains("CMSCanReprocessAccess") AndAlso GetUserGroups.Contains("CMSEligibilityAccess") Then
                        Select Case ColName.Trim
                            Case "PENDED_TO", "PROCESSED_BY"
                                Continue For
                        End Select
                    End If

                    If Not GetUserGroups.Contains("CMSCanRunReports") Then
                        Select Case ColName.Trim
                            Case "EMPLOYEE"
                                Continue For
                        End Select
                    End If

                    FilteredSettings.Add(ColName)

                Next

            End If

            Return DirectCast(FilteredSettings.ToArray(GetType(String)), String())

        Catch ex As Exception
            Throw
        End Try


    End Function

    Public Shared Function MeasureWidthInChars(ByVal controlWidth As Integer, ByVal fontName As String, ByVal fontSize As Single) As Integer

        Try

            ' Compute the visible characters for a control in the given font 
            Using B As New Bitmap(1, 1, PixelFormat.Format32bppArgb)
                Using G As Graphics = Graphics.FromImage(B)
                    Using F As New Font(fontName, fontSize)
                        Dim CharacterSize As SizeF = G.MeasureString(CChar("A"), F)
                        Return CInt(controlWidth \ CInt(CharacterSize.Width))
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

    Public Shared Sub UnFormattingCurrency(ByRef value As Object, ByVal rowNum As Integer)

        Try

            If value Is System.DBNull.Value Then
            ElseIf value Is "" Then
                value = 0
            Else
                value = value.ToString.Replace("$", "")
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub
    Public Shared Sub FormattingLOWER(ByRef value As Object, ByVal rowNum As Integer)

        Try

            If value Is System.DBNull.Value Then
            Else
                value = CStr(value).ToLower
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub FormattingNoZero(ByRef value As Object, ByVal rowNum As Integer)
        Try
            If IsDBNull(value) = False Then
                If (CStr(value).Trim) = "0" Then
                    value = ""
                Else
                    value = value
                End If
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub FormattingUPPER(ByRef value As Object, ByVal rowNum As Integer)

        Try

            If value Is System.DBNull.Value Then
            Else
                value = CStr(value).ToUpper
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Shared Sub FormattingORIGINAL(ByRef value As Object, ByVal rowNum As Integer)
        Try
            If IsDBNull(value) = False AndAlso (value Is Nothing OrElse value.ToString.Trim = "-1") Then
                value = "Original"
            Else
                value = ""
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub FormattingArchive(ByRef value As Object, ByVal rowNum As Integer)
        Try
            If IsDBNull(value) = False Then
                Select Case True
                    Case value.GetType Is GetType(System.String)
                        If value.ToString.Trim = "1" Then
                            value = "Archived"
                        Else
                            value = ""
                        End If
                    Case value.GetType Is GetType(System.Boolean)
                        If CBool(value) Then
                            value = "Archived"
                        Else
                            value = ""
                        End If
                    Case Else 'assume numeric
                        If value.ToString.Trim = "1" Then
                            value = "Archived"
                        Else
                            value = ""
                        End If
                End Select
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub Formatting1STCHARACTERMASK(ByRef value As Object, ByVal rowNum As Integer)
        Try

            If Not IsDBNull(value) Then

                Dim FirstChar As String = CStr(value).Substring(0, 1)
                Dim format As String

                Select Case FirstChar
                    Case "T"
                        format = "{0:00'-'0000000}"
                    Case "S"
                        format = "{0:000'-'00'-'0000}"
                    Case Else
                        format = "{0}" ' instructs format to reflect original value
                End Select

                value = String.Format(format, CInt(CStr(value).Substring(1)))

            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Shared Sub FormattingYesNo(ByRef value As Object, ByVal rowNum As Integer)
        Try
            If IsDBNull(value) = False Then
                Select Case True
                    Case value.GetType Is GetType(System.String)
                        If value.ToString.Trim = "1" Then
                            value = "Yes"
                        Else
                            value = "No"
                        End If
                    Case value.GetType Is GetType(System.Boolean)
                        If CBool(value) Then
                            value = "Yes"
                        Else
                            value = "No"
                        End If
                    Case Else 'assume numeric
                        If value.ToString.Trim = "1" Then
                            value = "Yes"
                        Else
                            value = "No"
                        End If
                End Select
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

#End Region

    Private Sub OnLoad()

        ArrowPic.Image = IList.Images(0)
        _DragPic = ArrowPic

        AddHandler Me.DragOver, New System.Windows.Forms.DragEventHandler(AddressOf HandleDragOver)
        AddHandler Me.DragDrop, New System.Windows.Forms.DragEventHandler(AddressOf HandleDragDrop)
        AddHandler Me.DragEnter, New System.Windows.Forms.DragEventHandler(AddressOf HandleDragEnter)
        AddHandler Me.MouseDown, New System.Windows.Forms.MouseEventHandler(AddressOf HandleDragMouseDown)
        AddHandler Me.MouseMove, New System.Windows.Forms.MouseEventHandler(AddressOf HandleDragMouseMove)
        AddHandler Me.MouseUp, New System.Windows.Forms.MouseEventHandler(AddressOf HandleDragMouseUp)

    End Sub

    Public Sub Delete()
        Dim Msg As Message

        Try
            Msg = New Message With {
                .Msg = KeyState.WM_KEYDOWN,
                .WParam = New IntPtr(46)
            }
            PreProcessMessage(Msg)

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During Delete", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub

    Public Overrides Function PreProcessMessage(ByRef msg As System.Windows.Forms.Message) As Boolean

        Dim KeyCode As Keys
        Dim Amt As Decimal = 0
        Dim Cancel As Boolean = False

        Try

            KeyCode = CType((msg.WParam.ToInt32 And Keys.KeyCode), Keys)

            If msg.Msg = KeyState.WM_KEYDOWN AndAlso KeyCode = Keys.Escape Then
                RaiseEvent EscapePressed()
            End If

            If PreventKeys.KeyExists(KeyCode, CType(msg.Msg, KeyState)) = True Then
                RaiseEvent PreventingKey(KeyCode, Cancel)
                If Cancel = False Then Return True
            End If

            Return MyBase.PreProcessMessage(msg)

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During PreProcessMessage", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            KeyCode = Nothing
        End Try

    End Function

    Protected Overrides Function ProcessKeyPreview(ByRef m As System.Windows.Forms.Message) As Boolean
        Dim KeyCode As Keys

        Try

            KeyCode = CType((m.WParam.ToInt32 And Keys.KeyCode), Keys)

            If KeyCode = Keys.Enter AndAlso m.Msg = KeyState.WM_KEYDOWN Then
                RaiseEvent EnterPressed(Me.CurrentCell)
            ElseIf KeyCode = Keys.Enter AndAlso m.Msg = KeyState.WM_KEYUP Then
                Return True
            ElseIf KeyCode = Keys.Escape AndAlso m.Msg = KeyState.WM_KEYDOWN Then
                RaiseEvent EscapePressed()
            End If

            Return MyBase.ProcessKeyPreview(m)

        Catch ex As Exception
            Return False
        End Try

    End Function

    Protected Overrides Function ProcessKeyMessage(ByRef m As System.Windows.Forms.Message) As Boolean
        Return MyBase.ProcessKeyMessage(m)
    End Function

    Protected Overrides Function ProcessDialogKey(ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Return MyBase.ProcessDialogKey(keyData)
    End Function

    Protected Overrides Function ProcessKeyEventArgs(ByRef m As System.Windows.Forms.Message) As Boolean
        Return MyBase.ProcessKeyEventArgs(m)
    End Function

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Dim KeyCode As Keys
        Dim Cancel As Boolean = False

        Try

            KeyCode = CType((msg.WParam.ToInt32 And Keys.KeyCode), Keys)

            If msg.Msg = KeyState.WM_KEYDOWN AndAlso KeyCode = Keys.ShiftKey Then
                _ShiftPressed = True
            ElseIf msg.Msg = KeyState.WM_KEYUP AndAlso KeyCode = Keys.ShiftKey Then
                _ShiftPressed = False
            End If

            If Me.DataSource IsNot Nothing Then
                If msg.Msg = KeyState.WM_KEYDOWN Then

                    If _AllowRefresh AndAlso KeyCode = Keys.F5 Then
                        RefreshData()
                    End If

                    If Me.BindingContext(Me.DataSource, Me.DataMember).Count > 0 Then
                        If _AllowFind AndAlso KeyCode = Keys.F AndAlso (keyData = Keys.F + Keys.Control) Then
                            ShowFindDialog()
                        ElseIf _AllowFind AndAlso KeyCode = Keys.F3 Then
                            Find()
                        ElseIf _AllowGoTo AndAlso KeyCode = Keys.F4 Then
                            ShowGoToDialog()
                        ElseIf _AllowGoTo AndAlso KeyCode = Keys.G AndAlso (keyData = Keys.G + Keys.Control) Then
                            ShowGoToDialog()
                        End If
                    End If
                End If
            End If

            'Test if they used keys to select rows
            If keyData = Keys.Down + Keys.Shift OrElse
                        keyData = Keys.Up + Keys.Shift OrElse
                       keyData = Keys.Shift + Keys.PageDown OrElse
                       keyData = Keys.Shift + Keys.PageUp OrElse
                       keyData = Keys.Control + Keys.Shift + Keys.PageDown OrElse
                       keyData = Keys.Control + Keys.Shift + Keys.PageUp OrElse
                       keyData = Keys.Control + Keys.Shift + Keys.End OrElse
                       keyData = Keys.Control + Keys.Shift + Keys.Home OrElse
                       keyData = Keys.Control + Keys.A OrElse
                       keyData = Keys.Shift + Keys.Space Then

                If _AllowMultiSelect = False Then
                    If Me.GetGridRowCount > 0 Then
                        Select Case KeyCode
                            Case Is = Keys.Down
                                If Me.CurrentRowIndex < Me.GetGridRowCount - 1 Then Me.CurrentRowIndex += 1
                            Case Is = Keys.PageDown
                                Me.CurrentRowIndex = Me.GetGridRowCount - 1
                            Case Is = Keys.Up
                                If Me.CurrentRowIndex > 0 Then Me.CurrentRowIndex -= 1
                            Case Is = Keys.PageUp
                                Me.CurrentRowIndex = 0
                            Case Is = Keys.Home
                                If keyData = Keys.Control + Keys.Shift + Keys.Home Then
                                    Me.CurrentRowIndex = 0
                                End If
                            Case Is = Keys.End
                                If keyData = Keys.Control + Keys.Shift + Keys.End Then
                                    Me.CurrentRowIndex = Me.GetGridRowCount - 1
                                End If
                        End Select

                        Me.[Select](Me.CurrentRowIndex)

                    End If

                    Return True
                End If
            Else
                If msg.Msg = KeyState.WM_KEYDOWN AndAlso KeyCode <> Keys.Delete AndAlso KeyCode <> Keys.ShiftKey AndAlso KeyCode <> Keys.ControlKey Then
                    _LastHitTestInfo = Nothing
                End If
            End If

            If _LastHitTestInfo IsNot Nothing Then
                If _LastHitTestInfo.Type = DataGrid.HitTestType.RowHeader Then
                    If _ConfirmDelete Then
                        If msg.Msg = KeyState.WM_KEYDOWN AndAlso KeyCode = Keys.Delete Then
                            If AllowDelete Then
                                If MessageBox.Show("Delete Selected Rows?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                                    Return True
                                Else
                                    DuringDelete(Cancel)
                                    If Cancel Then Return True
                                End If
                            Else
                                Return True
                            End If
                        End If
                    Else
                        If msg.Msg = KeyState.WM_KEYDOWN AndAlso KeyCode = Keys.Delete Then
                            DuringDelete(Cancel)
                            If Cancel Then Return True
                        End If
                    End If
                End If
            End If

            RaiseEvent KeyPushed(msg, keyData, Cancel)
            If Cancel Then Return True

            Return MyBase.ProcessCmdKey(msg, keyData)

        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Overloads Sub [Select](ByVal selectedPosition As Integer)

        'in the event that the row has been changed by another event, the select process will no longer be honored
        Dim CM As CurrencyManager

        Try
            If Me.BindingContext IsNot Nothing AndAlso DataSource IsNot Nothing Then
                CM = CType(BindingContext(DataSource, DataMember), CurrencyManager)
            Else
                Return
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If selectedPosition > -1 AndAlso CM.Count >= selectedPosition AndAlso selectedPosition < MyBase.VisibleRowCount Then
                MyBase.Select(selectedPosition)
                _PreviousSelectedHitTestRow = _LastSelectedHitTestRow
                _LastSelectedHitTestRow = selectedPosition

            ElseIf CM.Count = 1 And selectedPosition <> 0 Then
                MyBase.Select(0)
                _PreviousSelectedHitTestRow = 0
                _LastSelectedHitTestRow = 0

            End If

        Catch ex As Exception
            Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Public Sub FollowRow(cm As CurrencyManager, rowID As Long)

        'in the event that the row has been changed by another event, the select process will no longer be honored
        'Position is a zero based reference

        Dim BSPosition As Integer

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(cm Is Nothing, "N/A", cm.Position.ToString & "/" & cm.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(cm Is Nothing OrElse DataSource Is Nothing OrElse cm.Count < 1 OrElse cm.Position < 0 OrElse cm.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(cm.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If Me.BindingContext IsNot Nothing AndAlso cm IsNot Nothing Then
                BSPosition = FindPosition(cm, rowID)

                If BSPosition <> cm.Position Then 'may be the same if filter resulted in same number of rows.
                    cm.Position = BSPosition
                End If

                If cm.Position > -1 Then Me.[Select](cm.Position)

            End If

        Catch ex As Exception

            Throw

        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): CM(" & If(cm Is Nothing, "N/A", cm.Position.ToString & "/" & cm.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(cm Is Nothing OrElse DataSource Is Nothing OrElse cm.Count < 1 OrElse cm.Position < 0 OrElse cm.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(cm.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Public Sub FollowRow(cm As CurrencyManager, overrideRow As Integer)

        'in the event that the row has been changed by another event, the select process will no longer be honored
        'Position is a zero based reference

        Try
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ": CM(" & If(cm Is Nothing, "N/A", cm.Position.ToString & "/" & cm.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(cm Is Nothing OrElse DataSource Is Nothing OrElse cm.Count < 1 OrElse cm.Position < 0 OrElse cm.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(cm.Position).ToString) & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If cm IsNot Nothing Then
                Dim BSPosition As Integer = CInt(overrideRow)
                If BSPosition <> cm.Position Then
                    cm.Position = BSPosition
                End If

                If _LastSelectedHitTestRow Is Nothing OrElse _LastSelectedHitTestRow <> cm.Position Then
                    If Not Me.IsSelected(CInt(overrideRow)) Then Me.[Select](CInt(overrideRow))
                End If

            End If

        Catch ex As Exception

            Throw

        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & ": CM(" & If(cm Is Nothing, "N/A", cm.Position.ToString & "/" & cm.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(cm Is Nothing OrElse DataSource Is Nothing OrElse cm.Count < 1 OrElse cm.Position < 0 OrElse cm.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(cm.Position).ToString) & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub
    Private Shared Function FindPosition(cm As CurrencyManager, findRowID As Long) As Integer

        Dim ListDR As DataRow
        Dim fieldInfoDV As System.Reflection.FieldInfo

        Try

            For x = 0 To cm.Count - 1

                ListDR = CType(cm.List.Item(x), DataRowView).Row
                fieldInfoDV = ListDR.GetType().GetField("_rowID", System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance)

                If findRowID = DirectCast(fieldInfoDV.GetValue(ListDR), Int64) Then
                    Return x
                End If
            Next

            Return -1

        Catch ex As Exception
            Throw
        End Try

    End Function
    Public Overloads Sub [UnSelect](ByVal oldSelectedRow As Integer?)
        Dim CM As CurrencyManager

        Try
            If DataSource IsNot Nothing Then CM = CType(BindingContext(DataSource, DataMember), CurrencyManager)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If oldSelectedRow IsNot Nothing AndAlso GetGridRowCount() > oldSelectedRow AndAlso Not Me.IsSelected(CInt(oldSelectedRow)) Then
                MyBase.UnSelect(CInt(oldSelectedRow))
            End If

        Catch IgnoreException As Exception

            Debug.Print(IgnoreException.ToString)

        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub
    Public Sub AutoSizeRowHeight(Optional columnToSetHeightBy As Integer = 1, Optional columnWidth As Integer = 600)
        ' DataGrid should be bound to a DataTable for this part to 
        ' work. 
        Dim NumRows As Integer
        Dim G As Graphics
        Dim SF As StringFormat
        Dim Size As SizeF
        Dim MI As MethodInfo

        Dim DGArray As System.Array
        Dim DataGridRows As New ArrayList()
        Dim DV As DataView

        Try

            DV = Me.GetCurrentDataView
            If DV IsNot Nothing Then
                NumRows = DV.Count
            Else
                Return
            End If

            'Dim numRows As Integer = Me.GetCurrentDataTable.Rows.Count
            G = Graphics.FromHwnd(Me.Handle)
            SF = New StringFormat(StringFormat.GenericTypographic)

            ' Since DataGridRows[] is not exposed directly by the DataGrid 
            ' we use reflection to hack internally to it.. There is actually 
            ' a method get_DataGridRows that returns the collection of rows 
            ' that is what we are doing here, and casting it to a System.Array 
            MI = Me.[GetType]().GetMethod("get_DataGridRows", BindingFlags.FlattenHierarchy Or BindingFlags.IgnoreCase Or BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.[Public] Or BindingFlags.[Static])

            DGArray = DirectCast(MI.Invoke(Me, Nothing), System.Array)

            ' Convert this to an ArrayList, little bit easier to deal with 
            ' that way, plus we can strip out the newrow row. 
            For Each dgrr As Object In DGArray
                If dgrr.ToString().EndsWith("DataGridRelationshipRow") = True Then
                    DataGridRows.Add(dgrr)
                End If
            Next

            ' Now loop through all the rows in the grid 
            For i As Integer = 0 To NumRows - 1
                ' Here we are telling it that the column width is set to 
                ' 400.. so size will contain the Height it needs to be. 
                Size = G.MeasureString(Me(i, 1).ToString(), Me.Font, 400, SF)
                Dim h As Integer = Convert.ToInt32(Size.Height)
                ' Little extra cellpadding space 
                h += 8

                ' Now we pick that row out of the DataGridRows[] Array 
                ' that we have and set it's Height property to what we 
                ' think it should be. 
                Dim pi As PropertyInfo = DataGridRows(i).[GetType]().GetProperty("Height")

                ' I have read here that after you set the Height in this manner that you should 
                ' Call the DataGrid Invalidate() method, but I haven't seen any prob with not calling it.. 

                pi.SetValue(DataGridRows(i), h, Nothing)
            Next

        Catch ex As Exception
            Throw
        Finally
            If G IsNot Nothing Then G.Dispose()
            G = Nothing

            If SF IsNot Nothing Then SF.Dispose()
            SF = Nothing

        End Try
    End Sub

    Public Overridable Shadows Sub AutoSize(Optional ByVal mode As AutoSizeType = AutoSizeType.Both)

        If Me.GetGridRowCount = 0 OrElse Me.GetGridColumnCount = 0 Then Return

        Dim TS As DataGridTableStyle
        Dim TotCols As Integer
        Dim TotRows As Integer
        Dim TotVisibleRows As Integer
        Dim TextSizeWidth As Integer
        Dim ColValue As Object

        Dim ColText As String
        Dim ColFormattedText As String
        Dim ColFormat As String


        Try

            If Me.GetGridRowCount = 0 OrElse Me.GetGridColumnCount = 0 Then Return

            TS = Me.GetCurrentTableStyle
            TotCols = Me.GetGridColumnCount
            TotRows = Me.GetGridRowCount
            TotVisibleRows = Me.VisibleRowCount

            'Column Headers, use to establish minimum width
            If Me.ColumnHeadersVisible AndAlso (mode = AutoSizeType.Both OrElse mode = AutoSizeType.Columns) Then
                For colcnt As Integer = 0 To TotCols - 1
                    ColText = TS.GridColumnStyles(colcnt).HeaderText.ToString.Trim
                    Me.ColumnWidth(colcnt) = UFCWGeneral.MeasureWidthinPixels(ColText.ToString().Length, Me.Font.Name, Me.Font.Size)
                Next
            End If

            'Data
            For cnt As Integer = 0 To TotVisibleRows - 1

                ColText = Nothing
                ColValue = Nothing
                ColFormattedText = Nothing
                ColFormat = Nothing

                For colcnt As Integer = 0 To TotCols - 1
                    If TS.GridColumnStyles(colcnt).GetType Is GetType(DataGridIconColumn) OrElse TS.GridColumnStyles(colcnt).GetType Is GetType(DataGridHighlightIconColumn) Then
                        ColText = "   " ' establish minimum width for icon columns
                    ElseIf (IsDBNull(RuntimeHelpers.GetObjectValue(Me(cnt, colcnt))) OrElse Me(cnt, colcnt) Is Nothing) Then
                        'ElseIf Me.GetCurrentDataTable Is Nothing OrElse Me.GetCurrentDataTable.Columns.Contains(TS.GridColumnStyles(colcnt).MappingName) OrElse Me(cnt, colcnt) Is Nothing OrElse IsDBNull(Me(cnt, colcnt)) Then
                        ColText = TS.GridColumnStyles(colcnt).NullText.ToString.Trim
                    Else

                        ColText = Me(cnt, colcnt).ToString().Trim()
                        ColValue = RuntimeHelpers.GetObjectValue(Me(cnt, colcnt))

                        If TS.GridColumnStyles(colcnt).GetType Is GetType(DataGridTextBoxColumn) Then
                            ColFormat = DirectCast(TS.GridColumnStyles(colcnt), DataGridTextBoxColumn).Format.Trim()
                        ElseIf TS.GridColumnStyles(colcnt).GetType Is GetType(DataGridFormattableTextBoxColumn) Then
                            ColFormat = DirectCast(TS.GridColumnStyles(colcnt), DataGridFormattableTextBoxColumn).Format.Trim()
                        ElseIf TS.GridColumnStyles(colcnt).GetType Is GetType(DataGridHighlightTextBoxColumn) Then
                            ColFormat = DirectCast(TS.GridColumnStyles(colcnt), DataGridHighlightTextBoxColumn).Format.Trim()
                        End If

                        ColFormattedText = Nothing

                        If ColFormat.Trim().Length > 0 Then
                            ColFormattedText = Format(ColValue, ColFormat).ToString
                        End If

                        If ColFormattedText IsNot Nothing AndAlso ColFormattedText <> ColFormat Then
                            ColText = ColFormattedText
                        End If

                    End If

                    'Rows
                    If (mode = AutoSizeType.Both OrElse mode = AutoSizeType.Rows) Then
                        Me.AutoSizeRowHeight(1, 600)
                    End If

                    'Columns
                    If mode = AutoSizeType.Both OrElse mode = AutoSizeType.Columns AndAlso ColText IsNot Nothing Then
                        TextSizeWidth = UFCWGeneral.MeasureWidthinPixels(ColText.Length, Me.Font.Name, Me.Font.Size)
                        If Me.ColumnWidth(colcnt) < TextSizeWidth Then
                            Me.ColumnWidth(colcnt) = TextSizeWidth
                        End If
                    End If
                Next
            Next

            If _AutoSaveCols Then
                SaveColumnsSizeAndPosition(Me.Name & "\" & Me.GetCurrentTableStyle.MappingName & "\ColumnSettings")
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Public Overridable Sub AutoSizeVisible(Optional ByVal mode As AutoSizeType = AutoSizeType.Both)

        Dim ActualRowCount As Integer
        Dim G As Graphics
        Dim TS As DataGridTableStyle
        Dim TotCols As Integer
        Dim TotRows As Integer
        Dim FirstCol As Integer?
        Dim FirstRow As Integer?
        Dim TextSize As Decimal = 0
        Dim ColText As String = ""


        Try
            ActualRowCount = Me.GetGridRowCount

            If ActualRowCount = 0 OrElse Me.GetGridColumnCount = 0 Then Exit Sub

            G = Me.CreateGraphics
            TS = Me.GetCurrentTableStyle

            If TotRows > ActualRowCount Then TotRows = ActualRowCount

            TotCols = Me.VisibleColumnCount
            TotRows = Me.VisibleRowCount
            FirstCol = Me.FirstVisibleColumn
            FirstRow = Me.FirstVisibleRow

            If FirstCol Is Nothing OrElse FirstCol < 0 Then FirstCol = 0
            If FirstCol > TotCols Then FirstCol = TotCols

            If FirstRow Is Nothing OrElse FirstRow < 0 Then FirstRow = 0
            If FirstRow > TotRows Then FirstRow = TotRows

            'Column Headers
            If mode = AutoSizeType.Both OrElse mode = AutoSizeType.Columns Then
                For ColCnt As Integer = 0 To TotCols - 1
                    ColText = TS.GridColumnStyles(CInt(ColCnt + FirstCol)).HeaderText

                    TextSize = G.MeasureString(ColText, Me.Font).ToSize.Width + 13
                    If Me.ColumnWidth(CInt(ColCnt + FirstCol)) < TextSize Then
                        Me.ColumnWidth(CInt(ColCnt + FirstCol)) = CInt(TextSize + 7)
                    End If
                Next
            End If

            'Data
            For Cnt As Integer = 0 To TotRows - 1
                For colcnt As Integer = 0 To TotCols - 1
                    If IsDBNull(Me(CInt(Cnt + FirstRow), CInt(colcnt + FirstCol))) = True Then
                        ColText = TS.GridColumnStyles(CInt(colcnt + FirstCol)).NullText
                    Else
                        ColText = CStr(Me(CInt(Cnt + FirstRow), CInt(colcnt + FirstCol)))
                    End If

                    'Rows
                    If mode = AutoSizeType.Both OrElse mode = AutoSizeType.Rows Then
                        TextSize = G.MeasureString(ColText, Me.Font).ToSize.Height
                        If Me.RowHeight(CInt(Cnt + FirstRow)) < TextSize Then
                            Me.RowHeight(CInt(Cnt + FirstRow)) = CInt((TextSize * 0.15))
                        End If
                    End If

                    'Columns
                    If mode = AutoSizeType.Both OrElse mode = AutoSizeType.Columns Then
                        TextSize = G.MeasureString(ColText, Me.Font).ToSize.Width
                        If Me.ColumnWidth(CInt(colcnt + FirstCol)) < ((TextSize * 0.15) + 7) Then
                            Me.ColumnWidth(CInt(colcnt + FirstCol)) = CInt((TextSize * 0.15) + 7)
                        End If
                    End If
                Next
            Next
        Catch ex As Exception
            Throw
        Finally
            If G IsNot Nothing Then G.Dispose()
            G = Nothing
        End Try
    End Sub

    Public Sub EndCurrentEdit(Optional ByVal cancel As Boolean = False)
        Dim CM As CurrencyManager

        Try

            If Me.DataSource IsNot Nothing AndAlso Me.BindingContext IsNot Nothing Then
                If DataSource IsNot Nothing Then CM = CType(BindingContext(DataSource, DataMember), CurrencyManager)
                If CM IsNot Nothing Then
                    If cancel Then
                        CM.CancelCurrentEdit()
                    Else
                        CM.EndCurrentEdit()
                    End If
                End If
            End If

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During EndCurrentEdit", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub

    Private Sub DuringDelete(ByRef cancel As Boolean)
        Try
            'Call To Allow User To Add Code during a Delete
            RaiseEvent OnDelete(cancel)
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During DuringDelete", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub

    Public Sub RefreshData()
        Try

            If Me.GetCurrentTableStyle IsNot Nothing Then RemoveHighlights()

            'Call To Allow User To Add Code To Refresh Their Data
            RaiseEvent RefreshGridData()

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During RefreshData", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub

    Public Sub RemoveHighlights()
        Dim GCS As DataGridColumnStyle
        Dim TextCol As DataGridHighlightTextBoxColumn
        Dim BoolCol As DataGridHighlightBoolColumn

        Try

            TextCol = CType(GCS, DataGridHighlightTextBoxColumn)
            BoolCol = CType(GCS, DataGridHighlightBoolColumn)

            _HighlightedCell = New DataGridCell(-1, -1)
            _HighlightedRow = Nothing

            For Each GCS In Me.GetCurrentTableStyle.GridColumnStyles
                If GCS IsNot Nothing Then
                    If TypeOf GCS Is DataGridHighlightTextBoxColumn Then
                        TextCol = CType(GCS, DataGridHighlightTextBoxColumn)
                        TextCol.HighlightRow = Nothing
                    ElseIf TypeOf GCS Is DataGridHighlightBoolColumn Then
                        BoolCol = CType(GCS, DataGridHighlightBoolColumn)
                        BoolCol.HighlightRow = Nothing
                    End If
                End If
            Next
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During RemoveHighlights", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

            If GCS IsNot Nothing Then GCS.Dispose()
            GCS = Nothing

            TextCol = Nothing
            BoolCol = Nothing

        End Try

    End Sub

    Public Sub RemoveAt(ByVal row As Integer)
        Dim CM As CurrencyManager

        Try

            If Me.DataSource Is Nothing Then Exit Sub
            If TypeOf Me.DataSource Is DataSet AndAlso Me.DataMember = "" Then Exit Sub

            If TypeOf Me.DataSource Is DataView Then
                CType(Me.DataSource, DataView).Delete(row)
            Else
                If DataSource IsNot Nothing Then CM = CType(BindingContext(DataSource, DataMember), CurrencyManager)

                Try
                    If CM IsNot Nothing AndAlso CM.Position > -1 AndAlso TypeOf CM.Current IsNot DataRowView Then Return

                    CType(CM.Current, DataRowView).DataView.Delete(row)

                Catch IgnoreException As Exception
                    'Debug.Print(IgnoreException.ToString)
                End Try
            End If
        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Private Sub Row_DoubleClick(ByVal lastHitTestInfo As System.Windows.Forms.DataGrid.HitTestInfo)
        Try
            'Call To Allow User To Add Code during a Delete
            RaiseEvent GridRowDoubleClick(lastHitTestInfo)
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During Row_DoubleClick", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub

    Private Sub DataGridPlus_StyleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.StyleChanged
        Try
            RemoveHighlights()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub DataGridPlus_DataSourceChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataSourceChanged

        Dim CM As CurrencyManager

        Try

            If Me.DataSource IsNot Nothing Then
                If Me.BindingContext IsNot Nothing Then

                    CM = CType(BindingContext(DataSource, DataMember), CurrencyManager)

                End If
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  BS(" & If(CM Is Nothing, "N/A", CM.Position.ToString) & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Me.CaptionText = Me.CaptionText.ToString.Substring(0, CInt(If(InStr(Me.CaptionText, "-> Filtered by") = 0, Me.CaptionText.Length, InStr(Me.CaptionText, "-> Filtered by") - 1)))
            MenuFilterByColumn.Text = "Filter By Column"

            _CurrentBSPositionDR = Nothing
            _CurrentBSPosition = -1
            _CurrentBSRowCount = Nothing
            _CurrentRowID = Nothing

            _PreviousBSPositionDR = Nothing
            _PreviousBSPosition = -1
            _PreviousBSRowCount = Nothing
            _PreviousRowID = Nothing

            If Me.DataSource IsNot Nothing Then
                If Me.BindingContext IsNot Nothing Then

                    If DataSource IsNot Nothing Then CM = CType(BindingContext(DataSource, DataMember), CurrencyManager)
                    _BMB = BindingContext(DataSource, DataMember)

                    RemoveHandler _BMB.BindingComplete, AddressOf DataGridPlus_BindingComplete
                    RemoveHandler _BMB.DataError, AddressOf DataGridPlus_DataError
                    RemoveHandler _BMB.CurrentChanged, AddressOf DataGridPlus_CurrentChanged
                    RemoveHandler _BMB.CurrentItemChanged, AddressOf DataGridPlus_CurrentItemChanged
                    RemoveHandler _BMB.PositionChanged, AddressOf DataGridPlus_PositionChanged
                    RemoveHandler CM.ListChanged, AddressOf DataGridPlus_ListChanged

                    AddHandler _BMB.BindingComplete, AddressOf DataGridPlus_BindingComplete
                    AddHandler _BMB.DataError, AddressOf DataGridPlus_DataError
                    AddHandler _BMB.CurrentChanged, AddressOf DataGridPlus_CurrentChanged
                    AddHandler _BMB.CurrentItemChanged, AddressOf DataGridPlus_CurrentItemChanged
                    AddHandler _BMB.PositionChanged, AddressOf DataGridPlus_PositionChanged
                    AddHandler CM.ListChanged, AddressOf DataGridPlus_ListChanged

                End If
            End If

            If AllowNew <> _AllowNew Then AllowNew = _AllowNew
            If AllowEdit <> _AllowEdit Then AllowEdit = _AllowEdit
            If AllowDelete <> _AllowDelete Then AllowDelete = _AllowDelete

            If CM IsNot Nothing Then
                _CurrentBSRowCount = CM.Count
            End If

            RaiseEvent CurrentDataSourceChanged(sender, e)

        Catch ex As Exception
            Throw

        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid):  BS(" & If(CM Is Nothing, "N/A", CM.Position.ToString) & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            _LastColumnIndex = Nothing
            _PreviousSelectedHitTestRow = Nothing
            _LastSelectedHitTestRow = Nothing
            _LastCell = Nothing
            _LastHitTestInfo = Nothing

        End Try
    End Sub

    Private Sub DataGridPlus_AddingNew(sender As Object, e As AddingNewEventArgs)
        Dim CM As CurrencyManager

        Try
            If sender.GetType Is GetType(CurrencyManager) Then CM = CType(sender, CurrencyManager)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub DataGridPlus_BindingComplete(sender As Object, e As BindingCompleteEventArgs)
        Dim CM As CurrencyManager

        Try
            If sender.GetType Is GetType(CurrencyManager) Then CM = CType(sender, CurrencyManager)

            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & __PreviousBSPosition & ") CurPos(" & __CurrentBSPosition & ") " & Me.Name  & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & __PreviousBSPosition & ") CurPos(" & __CurrentBSPosition & ") " & Me.Name  & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub DataGridPlus_DataError(sender As Object, e As BindingManagerDataErrorEventArgs)
        Dim CM As CurrencyManager

        Try
            If sender.GetType Is GetType(CurrencyManager) Then CM = CType(sender, CurrencyManager)
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid): " & e.Exception.ToString)
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub DataGridPlus_CurrentItemChanged(sender As Object, e As EventArgs)
        Dim CM As CurrencyManager

        Try
            If sender.GetType Is GetType(CurrencyManager) Then
                CM = CType(sender, CurrencyManager)
                'ElseIf sender.GetType Is GetType(System.Windows.Forms.RelatedCurrencyManager) Then
            Else
                If sender.GetType.ToString.Contains("RelatedCurrencyManager") Then
                    CM = CType(sender, CurrencyManager)
                Else
                    Stop
                End If
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            'ManageCurrencyTokens(CM)

            RaiseEvent CurrentItemChanged(sender, e) 'allow parent the opportunity to handle the event

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub DataGridPlus_CurrentChanged(sender As Object, e As EventArgs)

        Dim CM As CurrencyManager

        Try
            If sender.GetType Is GetType(CurrencyManager) Then
                CM = CType(sender, CurrencyManager)
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If CM IsNot Nothing Then
                If _LastRowCount Is Nothing OrElse _LastRowCount <> CM.Count Then

                    RaiseEvent RowCountChanged(_LastRowCount, CInt(CM.Count))

                    _LastRowCount = CM.Count

                End If

                RaiseEvent CurrentRowChanged(_CurrentBSPosition, _PreviousBSPosition)
                RaiseEvent CurrentChanged(Me, e)
            End If

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub DataGridPlus_ListChanged(sender As Object, e As ListChangedEventArgs)

        Dim SortRequest As String

        Dim CM As CurrencyManager

        Try

            If sender.GetType IsNot GetType(CurrencyManager) Then Return

            CM = CType(sender, CurrencyManager)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") OldIndex(" & e.OldIndex.ToString & ") NewIndex(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Select Case e.ListChangedType
                Case ListChangedType.ItemDeleted 'triggered generally by Cancellation of an Add

                    'Stop

                Case ListChangedType.ItemChanged

                Case ListChangedType.Reset
                    'typically triggered by a sort or filter change on bindingsource

                    If Not (_ColumnResizing OrElse _ColumnRePositioning) AndAlso Me.AllowSorting AndAlso _LastHitTestInfo IsNot Nothing AndAlso _LastHitTestInfo.Type = DataGridCustom.HitTestType.ColumnHeader AndAlso _ColumnSorting Then

                        If CM.List.GetType = GetType(DataView) Then
                            SortRequest = DirectCast(CM.List, System.Data.DataView).Sort
                        Else

                            If CM Is Nothing Then
                                SortRequest = TryCast(CM.List, System.Data.DataView).Sort 'RelatedView
                            Else
                                SortRequest = DirectCast(DirectCast(CM.List, System.Windows.Forms.BindingSource).List, System.Data.DataView).Sort
                            End If

                            SortRequest = DirectCast(DirectCast(CM.List, System.Windows.Forms.BindingSource).List, System.Data.DataView).Sort
                        End If

                        ProcessSortRequest(CM, SortRequest)
                    Else
                        If _CurrentRowID IsNot Nothing AndAlso CM IsNot Nothing Then FollowRow(CM, CLng(_CurrentRowID))
                    End If

                Case ListChangedType.ItemAdded

                    'If _RetainRowSelectionAfterSort Then FollowRow()

                Case ListChangedType.ItemMoved

                    'If _RetainRowSelectionAfterSort Then FollowRow()

                Case ListChangedType.PropertyDescriptorChanged

                Case Else
            End Select

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") OldIndex(" & e.OldIndex.ToString & ") NewIndex(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub DataGridPlus_ColumnChanged(sender As Object, e As DataColumnChangeEventArgs)
        Dim CM As CurrencyManager

        Try
            If DataSource IsNot Nothing Then CM = CType(BindingContext(DataSource, DataMember), CurrencyManager)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub DataGridPlus_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs)
        Dim CM As CurrencyManager

        Try
            If DataSource IsNot Nothing Then CM = CType(BindingContext(DataSource, DataMember), CurrencyManager)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") Col(" & e.Column.ColumnName & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub DataGridPlus_RowChanged(sender As Object, e As DataRowChangeEventArgs)
        Dim CM As CurrencyManager

        Try
            If DataSource IsNot Nothing Then CM = CType(BindingContext(DataSource, DataMember), CurrencyManager)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub DataGridPlus_RowChanging(sender As Object, e As DataRowChangeEventArgs)

        Dim CM As CurrencyManager

        Try
            If DataSource IsNot Nothing Then CM = CType(BindingContext(DataSource, DataMember), CurrencyManager)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try
    End Sub

    Private Sub DataGridPlus_PositionChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim CM As CurrencyManager

        Try

            If sender.GetType IsNot GetType(CurrencyManager) Then Return

            CM = CType(sender, CurrencyManager)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _HighlightedRow IsNot Nothing AndAlso _HighlightedRow <> CM.Position Then
                Me.RemoveHighlights() 'need a way to force repaint without refresh
            End If

            ManageCurrencyTokens(CM)
            ManageRowID(CM)

            RaiseEvent PositionChanged(sender, e) 'allow parent the opportunity to handle the event

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub ManageRowID(cm As CurrencyManager)
        Dim fieldInfo As System.Reflection.FieldInfo
        Dim HoldRowID As Long

        Try

            If cm IsNot Nothing AndAlso cm.Position > -1 AndAlso cm.Count > cm.Position AndAlso cm.Current IsNot Nothing Then

                fieldInfo = _CurrentBSPositionDR.GetType().GetField("_rowID", System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance)
                HoldRowID = DirectCast(fieldInfo.GetValue(CType(cm.Current, DataRowView).Row), Int64)

                If _CurrentRowID Is Nothing OrElse _CurrentRowID <> HoldRowID Then

                    _PreviousRowID = _CurrentRowID
                    _CurrentRowID = HoldRowID

                End If
            Else
                _CurrentRowID = Nothing
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ManageCurrencyTokens(cm As CurrencyManager)
        Try

            If cm IsNot Nothing AndAlso cm.Position > -1 AndAlso cm.Count > cm.Position AndAlso cm.Current IsNot Nothing Then

                If _CurrentBSPosition = -1 OrElse _CurrentBSPosition <> cm.Position OrElse _CurrentBSRowCount <> cm.Count Then

                    _PreviousBSPosition = _CurrentBSPosition
                    _PreviousBSPositionDR = _CurrentBSPositionDR
                    _PreviousBSRowCount = _CurrentBSRowCount

                    _CurrentBSPositionDR = CType(cm.Current, DataRowView).Row
                    _CurrentBSPosition = cm.Position
                    _CurrentBSRowCount = cm.Count

                End If
            Else
                _CurrentBSPositionDR = Nothing
                _CurrentBSPosition = -1
                _CurrentBSRowCount = 0
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub DataGridPlus_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.CurrentCellChanged
        Dim Cell As DataGridCell

        Try

            Cell = CType(sender, DataGridCustom).CurrentCell

            If _LastColumnIndex Is Nothing OrElse _LastColumnIndex <> Cell.ColumnNumber Then
                RaiseEvent CurrentColumnChanged(_LastColumnIndex, Cell.ColumnNumber)
                _LastColumnIndex = Cell.ColumnNumber
            End If

            _LastCell = Me.CurrentCell

        Catch ex As Exception
            'Debug.Print(ex.ToString)
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During DataGridPlus_CurrentCellChanged", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
        End Try
    End Sub

    Public Sub DataGrid_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.DoubleClick
        Try
            _DoubleClick = True
        Catch ex As Exception
            'Debug.Print(ex.ToString)
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During DataGrid_MouseUp", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub DataGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Click
        Try
            _DoubleClick = False
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During DataGrid_MouseUp", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    <System.ComponentModel.Description("Call this method to reset state of mouse.")>
    Public Sub ResetMouseEvents()

        Try

            ResetMouseEventArgs()

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Protected Overloads Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)

        Dim HTI As DataGrid.HitTestInfo

        Try

            HTI = Me.HitTest(New Point(e.X, e.Y))

            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & If(HTI Is Nothing, "N/A", HTI.ToString) & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _AllowMultiSelect Then
                MyBase.OnMouseMove(e)
            Else
                'don't call the base class if left mouse down
                If (e.Button <> MouseButtons.Left) OrElse HTI.Type <> DataGrid.HitTestType.RowHeader Then
                    MyBase.OnMouseMove(e)
                End If
            End If

        Catch ex As Exception
            Throw
        Finally

            _LastHitTestInfo = HTI 'only update last position once mouse events have completed

            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & ":" & _LastHitTestInfo.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Private Sub DataGrid_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove

        Dim DG As DataGrid
        Dim OrigSorting As Boolean
        Dim ColumnHeaderText As Label
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo

        Try

            DG = CType(sender, DataGrid)
            HTI = DG.HitTest(e.X, e.Y)

            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & HTI.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If e.Button <> MouseButtons.Left Then
                _ColumnRePositioning = False
                If _ColumnHeaderLabel IsNot Nothing Then
                    If Me.Contains(_ColumnHeaderLabel) Then
                        Me.Controls.Remove(_ColumnHeaderLabel)
                        _ColumnHeaderLabel = Nothing
                        Me.Controls.Remove(_ColumnHeaderLabel)
                    End If
                End If
            End If

            ColumnHeaderText = _ColumnHeaderLabel

            If _ColumnHeaderLabel IsNot Nothing AndAlso HTI.Column > -1 Then 'AndAlso _LastHitTestInfo.Type = DataGrid.HitTestType.ColumnHeader Then

                Dim DGTS As DataGridTableStyle = Me.GetCurrentTableStyle

                If _AllowColumnReorder AndAlso DGTS IsNot Nothing Then 'capture for possible use in move logic

                    ColumnHeaderText = New Label With {.Text = DGTS.GridColumnStyles(HTI.Column).HeaderText}
                End If

            End If

            If _ColumnHeaderLabel Is Nothing OrElse _ColumnHeaderLabel.Text = ColumnHeaderText.Text Then
            Else

                OrigSorting = DG.AllowSorting

                _ColumnRePositioning = True

                Select Case HTI.Type
                    Case Is = System.Windows.Forms.DataGrid.HitTestType.None
                        If e.X < 0 Then
                            _DragPic.Left = 0
                        ElseIf e.X > DG.Left + DG.Width Then
                            _DragPic.Left = DG.Left + DG.Width
                        Else
                            _DragPic.Left = e.X
                        End If

                        If _DragColumn <> HTI.Column AndAlso _IsMouseDown Then
                            HandleImageDraw(DG)
                        Else
                            DG.AllowSorting = OrigSorting
                        End If

                    Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
                        If e.X < 0 Then
                            _DragPic.Left = 0
                        ElseIf e.X > DG.Left + DG.Width Then
                            _DragPic.Left = DG.Left + DG.Width
                        Else
                            _DragPic.Left = e.X
                        End If

                        If _DragColumn <> HTI.Column AndAlso _IsMouseDown Then
                            HandleImageDraw(DG)
                        Else
                            DG.AllowSorting = OrigSorting
                        End If

                    Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader
                        If e.X < 0 Then
                            _DragPic.Left = 0
                        ElseIf e.X > DG.Left + DG.Width Then
                            _DragPic.Left = DG.Left + DG.Width
                        Else
                            _DragPic.Left = e.X
                        End If

                        If _DragColumn <> HTI.Column AndAlso _IsMouseDown Then
                            HandleImageDraw(DG)
                        Else
                            DG.AllowSorting = OrigSorting
                        End If

                    Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                        If e.X < 0 Then
                            _DragPic.Left = 0
                        ElseIf e.X > DG.Left + DG.Width Then
                            _DragPic.Left = DG.Left + DG.Width
                        Else
                            _DragPic.Left = e.X + _DragPic.Width 'DragPic.Width '(DragPic.Width / 2)
                        End If

                        If _DragColumn <> HTI.Column AndAlso _IsMouseDown Then
                            HandleImageDraw(DG)
                        Else
                            DG.AllowSorting = OrigSorting
                        End If

                    Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

                    Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

                    Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption

                    Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

                End Select
            End If

            If _ColumnHeaderLabel IsNot Nothing Then
                _ColumnHeaderLabel.Left = CInt(e.X - _ColumnHeaderLabel.Width + (_ColumnHeaderLabel.Width / 3))
                'ColHdr.Top = e.Y - ColHdr.Height + 5
                If Me.CaptionVisible = True Then
                    _ColumnHeaderLabel.Top = 22
                Else
                    _ColumnHeaderLabel.Top = 3
                End If
            End If

            If HTI.Type = DataGrid.HitTestType.Cell AndAlso (_LastHitTestInfo Is Nothing OrElse (HTI.Row <> _LastHitTestInfo.Row OrElse HTI.Column <> _LastHitTestInfo.Column)) Then
                'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Mid: " & Me.Name & ":" & If(_LastHitTestInfo Is Nothing, "N/A", _LastHitTestInfo.ToString) & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
                ResetMouseEvents()
            End If

        Catch ex As Exception
            Throw

        Finally

            If ColumnHeaderText IsNot Nothing Then ColumnHeaderText.Dispose()
            ColumnHeaderText = Nothing

            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & ":" & If(_LastHitTestInfo Is Nothing, "N/A", _LastHitTestInfo.ToString) & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Private Sub HandleImageDraw(ByVal dg As DataGrid)

        Try

            _ColumnHeaderLabel.Visible = True
            _ColumnHeaderLabel.Refresh()

            'DragPic.Visible = True
            If dg.Controls.Contains(_DragPic) = False Then dg.Controls.Add(_DragPic)

        Catch IgnoreException As Exception
            'Debug.Print(IgnoreException.ToString)
        End Try

    End Sub
    Protected Overloads Overrides Sub OnMouseClick(ByVal e As MouseEventArgs)

        If _SuppressMouseClick Then
            RaiseEvent MouseClickSuppressed(Me, New EventArgs)
            Return
        End If

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " X/Y " & e.X.ToString & ":" & e.Y.ToString & ":" & _LastHitTestInfo.Type.ToString & ":" & _LastHitTestInfo.Row.ToString & " - " & _LastSelectedHitTestRow.ToString & " - " & _PreviousBSPosition.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            MyBase.OnMouseClick(e)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " X/Y " & e.X.ToString & ":" & e.Y.ToString & ":" & _LastHitTestInfo.Type.ToString & ":" & _LastHitTestInfo.Row.ToString & " - " & _LastSelectedHitTestRow.ToString & " - " & _PreviousBSPosition.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Protected Overloads Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)

        'Order of events on click
        'OnMouseDown()
        'MouseDown()
        'CurrentChanged()
        'CurrentItemChanged()
        'PositionChanged()
        'ListChanged() <- if bindingsource binding is available
        'OnMouseUp()
        'MouseUp()

        If _SuppressMouseClick Then
            RaiseEvent MouseDownSuppressed(Me, New EventArgs)
            Return
        End If

        'Dim CM As CurrencyManager = DirectCast(BindingContext(Me.DataSource, Me.DataMember), CurrencyManager)

        Dim HTI As DataGrid.HitTestInfo

        Try

            HTI = Me.HitTest(New Point(e.X, e.Y))

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & HTI.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            _ColumnResizing = False
            _ColumnRePositioning = False
            _ColumnSorting = False
            _MultiRowInProgress = False

            If (Control.ModifierKeys = Keys.Shift OrElse Control.ModifierKeys = Keys.Control) AndAlso _AllowMultiSelect Then
                _MultiRowInProgress = True
            End If

            If _ColumnHeaderLabel IsNot Nothing Then
                If Me.Contains(_ColumnHeaderLabel) Then
                    Me.Controls.Remove(_ColumnHeaderLabel)
                    _ColumnHeaderLabel = Nothing
                End If
            End If

            'If e.X > -1 AndAlso e.Y > -1 AndAlso e.Button = MouseButtons.Right Then
            '    If (_LastHitTestInfo.Type = DataGrid.HitTestType.Cell OrElse _LastHitTestInfo.Type = DataGrid.HitTestType.RowHeader) AndAlso Not Me.IsSelected(_LastHitTestInfo.Row) Then
            '        Me.Select(_LastHitTestInfo.Row, True)
            '    End If
            'End If

            If e.X > -1 AndAlso e.Y > -1 Then

                Select Case HTI.Type
                    Case Is = DataGrid.HitTestType.Cell, DataGrid.HitTestType.RowHeader 'Row selection does not occur unless in multiselect mode for either button

                        If Not _MultiRowInProgress AndAlso Not (e.Button = MouseButtons.Right AndAlso Me.IsSelected(HTI.Row)) Then
                            ResetSelection()
                        End If

                        'If e.Button = MouseButtons.Right AndAlso Not Me.IsSelected(_LastHitTestInfo.Row) Then 'select row if row not already selected using right button
                        '    Me.[Select](_LastHitTestInfo.Row)
                        'End If

                        If _AllowMultiSelect = False OrElse (_AllowMultiSelect AndAlso Not (Control.ModifierKeys And Keys.Shift) = 0) Then 'single row only, so old row gets deselected

                            'don't call the base class if in header

                            'If _PreviousSelectedHitTestRow IsNot Nothing AndAlso Not Me.IsSelected(_LastHitTestInfo.Row) Then 'unselect if selected row is different from prior selected row
                            '    Me.UnSelect(_PreviousSelectedHitTestRow)
                            'End If

                            'If _PreviousSelectedHitTestRow Is Nothing Then
                            '    Me.[Select](_LastHitTestInfo.Row)
                            'End If

                        End If

                        'If _LastHitTestInfo.Type = DataGrid.HitTestType.Cell Then
                        '    Me.CurrentCell = New DataGridCell(_LastHitTestInfo.Row, _LastHitTestInfo.Column)
                        'End If

                    Case Is = DataGrid.HitTestType.ColumnResize

                        _ColumnResizing = True

                    Case Is = DataGrid.HitTestType.ColumnHeader

                        Dim DGTS As DataGridTableStyle = Me.GetCurrentTableStyle

                        If _AllowColumnReorder AndAlso DGTS IsNot Nothing Then 'capture for possible use in move logic

                            _IsMouseDown = True
                            _DragColumn = HTI.Column
                            _ColumnHeaderLabel = New Label With {
                                .Width = DGTS.GridColumnStyles(HTI.Column).Width,
                                .Height = 20, 'Me.PreferredRowHeight
                                .Text = DGTS.GridColumnStyles(HTI.Column).HeaderText,
                                .BackColor = System.Drawing.Color.Transparent, 'Style.HeaderBackColor
                                .ForeColor = DGTS.HeaderForeColor,
                                .Font = DGTS.HeaderFont,
                                .BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D,
                                .Visible = False
                            }
                            Me.Controls.Add(_ColumnHeaderLabel)
                        End If

                        If Not Me.AllowSorting Then Exit Sub 'basically ignore mousedown

                End Select

            End If

            MyBase.OnMouseDown(e)

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & ":" & HTI.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)

        'Dim CM As CurrencyManager = DirectCast(BindingContext(Me.DataSource, Me.DataMember), CurrencyManager)

        'Dim DGStyle As DataGridTableStyle = Me.GetCurrentTableStyle
        'If DGStyle IsNot Nothing Then
        '    'Debug.Print("OnMouseUp In _LastHitTestInfo: Col#: " & _LastHitTestInfo.Column.ToString & " ColName " & DGStyle.GridColumnStyles(_LastHitTestInfo.Column).MappingName & " X:" & e.X.ToString & " Y:" & e.Y.ToString)
        'End If

        If _SuppressMouseClick Then
            RaiseEvent MouseUpSuppressed(Me, New EventArgs)
            Return
        End If

        Dim CM As CurrencyManager
        Dim HTI As DataGrid.HitTestInfo

        Try
            HTI = Me.HitTest(New Point(e.X, e.Y))

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & HTI.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If DataSource IsNot Nothing Then CM = CType(BindingContext(DataSource, DataMember), CurrencyManager)

            MyBase.OnMouseUp(e)

            If e.Button = MouseButtons.Left Then
                If _ColumnResizing AndAlso (HTI.Type = DataGridCustom.HitTestType.Cell OrElse HTI.Type = DataGridCustom.HitTestType.ColumnHeader OrElse HTI.Type = DataGridCustom.HitTestType.ColumnResize OrElse HTI.Type = DataGridCustom.HitTestType.None) Then

                    If _AutoSaveCols Then
                        SaveColumnsSizeAndPosition(Me.Name & "\" & Me.GetCurrentTableStyle.MappingName & "\ColumnSettings")
                    End If

                    RaiseEvent CurrentColumnResized(CInt(HTI.Column))

                    Return

                ElseIf _ColumnRePositioning AndAlso (HTI.Type = DataGridCustom.HitTestType.Cell OrElse HTI.Type = DataGridCustom.HitTestType.ColumnHeader OrElse HTI.Type = DataGridCustom.HitTestType.ColumnResize OrElse HTI.Type = DataGridCustom.HitTestType.None) Then

                    ProcessRepositionRequest(e)

                End If
            End If

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During OnMouseUp", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & ":" & HTI.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Public Sub DataGrid_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseUp

        Dim DG As DataGrid
        Dim CM As CurrencyManager

        Dim HTI As DataGrid.HitTestInfo

        Try
            DG = CType(sender, DataGridCustom)
            HTI = DG.HitTest(e.X, e.Y)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & HTI.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If DataSource IsNot Nothing Then CM = CType(BindingContext(DataSource, DataMember), CurrencyManager)

            Select Case HTI.Type
                Case Is = DataGrid.HitTestType.Cell, DataGrid.HitTestType.RowHeader 'Row selection does not occur unless in multiselect mode for either button

                    If e.Button = MouseButtons.Right Then 'context menu
                        If _SetRowOnClick Then FollowRow(CM, HTI.Row)
                    End If

                    If e.Button = MouseButtons.Left AndAlso HTI.Type = DataGrid.HitTestType.Cell Then
                        Me.CurrentCell = New DataGridCell(HTI.Row, HTI.Column)
                    End If

                    If e.Button = MouseButtons.Left Then
                        'Capture both DataGrid Row and CurrencyRow (if available)
                        Dim DR As DataRow = SelectedRowPreview

                    End If

                Case Is = DataGrid.HitTestType.ColumnResize

                Case Is = DataGrid.HitTestType.ColumnHeader

            End Select

            If Me.AllowSorting AndAlso Not (_ColumnResizing OrElse _ColumnRePositioning) AndAlso HTI.Type = DataGridCustom.HitTestType.ColumnHeader AndAlso e.Button = MouseButtons.Left Then
                _ColumnSorting = True
            End If

            'If _ColumnSorting Then 'Debug.Print("MouseUp (Sorting)")

            _IsMouseDown = False

            'Change the value of a Bool column on first click if mode is on
            If _SingleClickBooleanCols AndAlso e.Button = MouseButtons.Left Then
                If HTI.Type = DataGrid.HitTestType.Cell Then

                    Using DGTSCol As DataGridColumnStyle = Me.GetCurrentTableStyle.GridColumnStyles(HTI.Column)
                        If DGTSCol.GetType.Name = "DataGridBoolColumn" OrElse DGTSCol.GetType.BaseType.Name = "DataGridBoolColumn" Then
                            If DG.ReadOnly = False AndAlso DGTSCol.ReadOnly = False Then
                                Dim PI As PropertyInfo = DGTSCol.GetType.GetProperty("AllowNull")
                                If PI IsNot Nothing Then
                                    If CBool(DGTSCol.GetType.InvokeMember("AllowNull", BindingFlags.GetProperty, Nothing, DGTSCol, Array.Empty(Of Object)())) Then
                                        If IsDBNull(Me(HTI.Row, HTI.Column)) Then
                                            Me(HTI.Row, HTI.Column) = False
                                        ElseIf CBool(Me(HTI.Row, HTI.Column)) Then
                                            Me(HTI.Row, HTI.Column) = DBNull.Value
                                        Else
                                            Me(HTI.Row, HTI.Column) = True
                                        End If
                                    Else
                                        If IsDBNull(Me(HTI.Row, HTI.Column)) Then
                                            Me(HTI.Row, HTI.Column) = True
                                        Else
                                            Me(HTI.Row, HTI.Column) = Not CBool(Me(HTI.Row, HTI.Column))
                                        End If
                                    End If
                                Else
                                    Me(HTI.Row, HTI.Column) = Not CBool(Me(HTI.Row, HTI.Column))
                                End If
                            End If
                        End If
                    End Using
                End If
            End If

            If HTI.Type = DataGrid.HitTestType.Cell OrElse HTI.Type = DataGrid.HitTestType.RowHeader Then
                If _DoubleClick AndAlso e.Button = MouseButtons.Left Then
                    RaiseEvent GridRowDoubleClick(HTI)
                End If
            End If

        Catch ex As Exception
            Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & ":" & HTI.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Private Sub ProcessRepositionRequest(ByVal e As System.Windows.Forms.MouseEventArgs)

        Dim Col As Integer
        Dim T As DataTable
        Dim ColLastHitTestInfo As System.Windows.Forms.DataGrid.HitTestInfo

        Try

            If _ColumnHeaderLabel.Visible Then

                _ColumnHeaderLabel.Visible = False
                _DragPic.Visible = False

                T = Me.GetCurrentDataTable

                If T Is Nothing Then Exit Sub

                If e.X < Me.Left OrElse e.X <= Me.RowHeaderWidth Then
                    Col = Me.FirstVisibleColumn
                ElseIf e.X > Me.Width Then
                    Col = Me.FirstVisibleColumn + Me.VisibleColumnCount - 1
                Else
                    Col = _LastHitTestInfo.Column
                End If

                If Col = -1 Then
                    ColLastHitTestInfo = Me.HitTest(_DragPic.Location.X, _DragPic.Location.Y + _DragPic.Height)
                    If ColLastHitTestInfo.Column <> -1 Then
                        Col = ColLastHitTestInfo.Column
                    Else
                        If ColLastHitTestInfo.Type = System.Windows.Forms.DataGrid.HitTestType.None Then
                            Col = Me.FirstVisibleColumn + Me.VisibleColumnCount - 1
                        Else
                            Col = _DragColumn
                        End If
                    End If
                End If

                Me.Controls.Remove(_ColumnHeaderLabel)

                MoveColumn(_DragColumn, Col)

                'Me.Refresh()
                Me.Invalidate()

                ''Debug.Print("MouseUp Out1:" & Me.GetGridSortColumn)

            End If

            If _AutoSaveCols Then
                SaveColumnsSizeAndPosition(Me.Name & "\" & Me.GetCurrentTableStyle.MappingName & "\ColumnSettings")
            End If

            RaiseEvent ColumnPositionChanged(Me.GetCurrentTableStyle.GridColumnStyles(_LastHitTestInfo.Column), _DragColumn, _LastHitTestInfo.Column)

        Catch ex As Exception
        Finally
            _ColumnHeaderLabel = Nothing
            _ColumnRePositioning = False

        End Try

    End Sub


    Private Sub ProcessSortRequest(cm As CurrencyManager, requestedSort As String)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(cm Is Nothing, "N/A", cm.Position.ToString & "/" & cm.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(cm Is Nothing OrElse DataSource Is Nothing OrElse cm.Count < 1 OrElse cm.Position < 0 OrElse cm.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(cm.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            _ColumnSorting = False

            If _LastHitTestInfo.Column = -1 AndAlso _LastHitTestInfo.Row = -1 Then Exit Sub

            If _AutoSaveCols Then
                SaveSortBy(requestedSort)
            End If

            RaiseEvent GridSorted(Me, New CurrentRowChangedEventArgs(_CurrentBSPosition, If(_LastSelectedHitTestRow IsNot Nothing AndAlso CInt(_LastSelectedHitTestRow) = CInt(_CurrentBSPosition), True, False), _CurrentBSPositionDR))
            RaiseEvent GridSortChanged(Sort)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Ou1(Grid): CM(" & If(cm Is Nothing, "N/A", cm.Position.ToString & "/" & cm.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(cm Is Nothing OrElse DataSource Is Nothing OrElse cm.Count < 1 OrElse cm.Position < 0 OrElse cm.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(cm.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If _CurrentRowID IsNot Nothing AndAlso _RetainRowSelectionAfterSort AndAlso cm IsNot Nothing Then
                FollowRow(cm, CLng(_CurrentRowID))
            End If

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Ou2(Grid): CM(" & If(cm Is Nothing, "N/A", cm.Position.ToString & "/" & cm.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(cm Is Nothing OrElse DataSource Is Nothing OrElse cm.Count < 1 OrElse cm.Position < 0 OrElse cm.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(cm.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try


    End Sub

    Private Sub ProcessMultiSortRequest(requestedSort As String)

        If _LastHitTestInfo.Column = -1 AndAlso _LastHitTestInfo.Row = -1 Then Exit Sub

        If requestedSort.Split(CChar(",")).Length > 1 Then
            MenuMultiColSort.Checked = True
        Else
            MenuMultiColSort.Checked = False
        End If

        Me.Sort = requestedSort

        If _AutoSaveCols Then
            SaveSortBy(requestedSort)
        End If

        RaiseEvent GridSortChanged(requestedSort)


    End Sub
    Private Sub DataGridPlus_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint

        Dim DG As DataGridCustom
        DG = CType(sender, DataGridCustom)

        Try

            If _SuppressTriangle Then
                RemoveTriangle(sender, e)
            End If

            If _ShowModifiedRows AndAlso DG.AllowEdit = False Then
                ShowModifiedIcon(sender, e)
            End If

        Catch ex As Exception
            'Debug.Print(ex.ToString)
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During DataGridPlus_Paint", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub

    Private Sub RemoveTriangle(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim T As Type
        Dim Rect As Rectangle
        Dim FirstVisibleIndex As Integer
        Dim LastVisibleIndex As Integer
        Dim DG As DataGridCustom

        Try

            DG = CType(sender, DataGridCustom)
            T = DG.[GetType]()

            FirstVisibleIndex = CInt(T.InvokeMember("firstVisibleRow", BindingFlags.GetField Or BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, DG, Nothing))
            LastVisibleIndex = FirstVisibleIndex + DG.VisibleRowCount

            For j As Integer = FirstVisibleIndex To LastVisibleIndex - 1
                Rect = DG.GetCellBounds(j, 0)

                Rect.X -= DG.RowHeaderWidth
                Rect.Width = DG.RowHeaderWidth

                Rect.X += 1
                Rect.Width -= 2
                Rect.Y += 1
                Rect.Height -= 2

                Using BR As New SolidBrush(DG.HeaderBackColor)
                    e.Graphics.FillRectangle(BR, Rect)
                End Using

            Next

        Catch IgnoreException As Exception
            'Debug.Print(IgnoreException.ToString)
        Finally
            T = Nothing
            Rect = Nothing
        End Try
    End Sub

    Private Sub ShowModifiedIcon(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim T As Type
        Dim Rect As Rectangle
        Dim FirstVisibleIndex As Integer
        Dim LastVisibleIndex As Integer
        Dim DG As DataGridCustom
        Dim rm As Resources.ResourceManager = My.Resources.ResourceManager
        Dim bmp As Bitmap = CType(rm.GetObject("Pencil"), Bitmap)

        Try

            DG = CType(sender, DataGridCustom)
            Dim DV As DataView = DG.GetCurrentDataView

            If DV IsNot Nothing Then

                T = DG.[GetType]()

                FirstVisibleIndex = CInt(T.InvokeMember("firstVisibleRow", BindingFlags.GetField Or BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, DG, Nothing))
                LastVisibleIndex = FirstVisibleIndex + DG.VisibleRowCount

                For j As Integer = FirstVisibleIndex To LastVisibleIndex - 1

                    If DV(j).Row.RowState = DataRowState.Modified Then
                        Rect = DG.GetCellBounds(j, 0)

                        Rect.X -= DG.RowHeaderWidth \ 2
                        Rect.Width = DG.RowHeaderWidth \ 2

                        Rect.X += 1
                        Rect.Width -= 2
                        Rect.Y += 2
                        Rect.Height -= 4I

                        e.Graphics.DrawImage(bmp, Rect)

                    End If
                Next

            End If

        Catch IgnoreException As Exception

        Finally
            T = Nothing
            Rect = Nothing
        End Try
    End Sub

    Public Sub ShowFindDialog()

        Try

            If Me.BindingContext(Me.DataSource, Me.DataMember).Count > 0 Then
                If _FindDialog Is Nothing Then
                    _FindDialog = New FindDialog(GetCurrentDataTable.TableName, Me) With {
                        .TopMost = False,
                        .Owner = Me.FindForm.FindForm
                    }

                    Try
                        _FindDialog.Show()
                        _FindDialog.FindStr.Select()
                    Catch ex As Exception
                        _FindDialog = Nothing
                        _FindDialog = New FindDialog(GetCurrentDataTable.TableName, Me) With {
                            .TopMost = False,
                            .Owner = Me.FindForm.FindForm
                        }
                        _FindDialog.Show()
                        _FindDialog.FindStr.Select()
                    End Try
                Else
                    Try
                        _FindDialog.Show()
                        _FindDialog.FindStr.Select()
                    Catch ex As Exception
                        _FindDialog = Nothing
                        _FindDialog = New FindDialog(GetCurrentDataTable.TableName, Me) With {
                            .TopMost = False,
                            .Owner = Me.FindForm.FindForm
                        }
                        _FindDialog.Show()
                        _FindDialog.FindStr.Select()
                    End Try
                End If
            End If

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During ShowFindDialog", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
        End Try
    End Sub

    Public Sub ShowFilterDialog()
        Dim FilterSelect As FilterDialog
        Dim DV As DataView

        Try

            FilterSelect = New FilterDialog(Me, If(_APPKEY, "")) With {
                .StartPosition = FormStartPosition.CenterParent
            }

            If FilterSelect.ShowDialog(Me) = DialogResult.Cancel Then
                _FilterMappingName = Nothing
                _FilterValue = Nothing
            Else

                _FilterMappingName = FilterSelect.FilterMappingName
                _FilterColumnHeading = FilterSelect.FilterColumnHeading
                _FilterValue = FilterSelect.FilterValue
                _FilterFormattedValue = FilterSelect.FilterFormattedValue

                DV = Me.GetCurrentDataView

                If _FilterMappingName Is Nothing Then
                    DV.RowFilter = Me.DefaultViewFilter
                    Me.CaptionText = Me.CaptionText.ToString.Substring(0, CInt(If(InStr(Me.CaptionText, "-> Filtered by") = 0, Me.CaptionText.Length, InStr(Me.CaptionText, "-> Filtered by") - 1)))
                    MenuFilterByColumn.Text = "Filter By Column"
                Else
                    Select Case True
                        Case Me.GetCurrentDataTable.Columns(_FilterMappingName).DataType Is GetType(System.DateTime)
                            DV.RowFilter = If(DV.RowFilter.Length > 0, DV.RowFilter & " AND " & _FilterMappingName & " >= #" & String.Format("{0:MM/dd/yyyy}", CDate(_FilterValue)) & " 00:00:00# AND " & _FilterMappingName & " <= #" & String.Format("{0:MM/dd/yyyy}", CDate(_FilterValue)) & " 23:59:59#", _FilterMappingName & " >= #" & String.Format("{0:MM/dd/yyyy}", CDate(_FilterValue)) & " 00:00:00# AND " & _FilterMappingName & " <= #" & String.Format("{0:MM/dd/yyyy}", CDate(_FilterValue)) & " 23:59:59#")
                        Case Me.GetCurrentDataTable.Columns(_FilterMappingName).DataType Is GetType(System.String)
                            DV.RowFilter = CStr(If(DV.RowFilter.Length > 0, DV.RowFilter & " AND " & _FilterMappingName & " = '" & _FilterValue & "'", _FilterMappingName & " = '" & _FilterValue & "'"))
                        Case Else
                            DV.RowFilter = CStr(If(DV.RowFilter.Length > 0, DV.RowFilter & " AND " & _FilterMappingName & " = " & _FilterValue, _FilterMappingName & " = " & _FilterValue))
                    End Select

                    Me.CaptionText = Me.CaptionText & " -> Filtered by " & _FilterColumnHeading & " (" & _FilterFormattedValue & ")"
                    MenuFilterByColumn.Text = "Filter By Column (Add / Reset)"

                End If

                If Len(DV.RowFilter) > 0 Then
                    Me.CaptionVisible = True
                End If

                RaiseEvent TableFilterApplied(Me, FilterSelect.FilterMappingName, FilterSelect.FilterValue)
            End If

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During Copy", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If FilterSelect IsNot Nothing Then FilterSelect.Dispose()
            FilterSelect = Nothing

        End Try
    End Sub

    Public Sub Find()

        Try

            If Me.BindingContext(Me.DataSource, Me.DataMember).Count > 0 Then
                If _FindDialog Is Nothing Then
                    _FindDialog = New FindDialog(GetCurrentDataTable.TableName, Me) With {
                        .TopMost = False,
                        .Owner = Me.FindForm.FindForm
                    }
                    Try
                        _FindDialog.Show()
                        _FindDialog.FindStr.Select()

                    Catch ex As Exception
                        _FindDialog = Nothing
                        _FindDialog = New FindDialog(GetCurrentDataTable.TableName, Me) With {
                            .TopMost = False,
                            .Owner = Me.FindForm.FindForm
                        }
                        _FindDialog.Show()
                        _FindDialog.FindStr.Select()
                    End Try
                Else
                    Try
                        If _FindDialog.FindStr.Text <> "" Then
                            _FindDialog.SearchNext()
                            Me.Select()
                        Else
                            _FindDialog.Show()
                            _FindDialog.FindStr.Select()
                        End If
                    Catch ex As Exception
                        _FindDialog = Nothing
                        _FindDialog = New FindDialog(GetCurrentDataTable.TableName, Me) With {
                            .TopMost = False,
                            .Owner = Me.FindForm.FindForm
                        }
                        _FindDialog.Show()
                        _FindDialog.FindStr.Select()
                    End Try
                End If
            End If

        Catch ex As Exception
            'Debug.Print(ex.ToString)
            Throw
        Finally
        End Try
    End Sub

    Public Sub Find(ByVal Value As String)

        Try

            If Me.BindingContext(Me.DataSource, Me.DataMember).Count > 0 Then
                If _FindDialog Is Nothing Then
                    _FindDialog = New FindDialog(GetCurrentDataTable.TableName, Me) With {
                        .TopMost = False,
                        .Owner = Me.FindForm.FindForm
                    }

                    Try
                        _FindDialog.FindStr.Select()
                    Catch ex As Exception
                        _FindDialog = New FindDialog(GetCurrentDataTable.TableName, Me) With {
                            .TopMost = False,
                            .Owner = Me.FindForm.FindForm
                        }
                        _FindDialog.FindStr.Select()
                    End Try
                Else

                    Try
                        If _FindDialog.FindStr.Text <> "" Then
                            _FindDialog.SearchNext()
                            Me.Select()
                        Else
                            _FindDialog.FindStr.Select()
                        End If
                    Catch ex As Exception
                        _FindDialog = Nothing
                        _FindDialog = New FindDialog(GetCurrentDataTable.TableName, Me) With {
                            .TopMost = False,
                            .Owner = Me.FindForm.FindForm
                        }
                        _FindDialog.FindStr.Select()
                    End Try
                End If

                _FindDialog.SearchText = Value
                _FindDialog.SearchNext()
            End If

        Catch ex As Exception
            'Debug.Print(ex.ToString)
            Throw
        Finally
        End Try
    End Sub

    Public Sub ResetFindForm()
        Try
            If _FindDialog IsNot Nothing Then
                _FindDialog.ResetLastFoundPosition()
                _FindDialog.Close()
                _FindDialog = Nothing
            End If
        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub

    Public Overloads Sub ResetSelection()

        Try

            'Dim CM As CurrencyManager = DirectCast(BindingContext(Me.DataSource, Me.DataMember), CurrencyManager)

            ''Debug.Print("DataGrid_ResetSelection (In): " & Me.Name  & " " & _LastSelectedHitTestRow.ToString & ":" & _LastBSPosition.ToString & ":" & CM.Position.ToString)

            MyBase.ResetSelection()

        Catch ex As Exception
            'Debug.Print(ex.ToString)
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During ResetFindForm", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try
    End Sub

    Public Sub ShowGoToDialog()

        Dim GoToDialog As GoToDialog
        Dim CM As CurrencyManager

        Try
            If DataSource IsNot Nothing Then CM = CType(BindingContext(DataSource, DataMember), CurrencyManager)

            If CM.Count > 0 Then
                GoToDialog = New GoToDialog(Me)
                GoToDialog.LineNum.Text = CStr(LastGoToLine)

                Select Case GoToDialog.ShowDialog(Me)
                    Case DialogResult.OK
                        CM.Position = CInt(GoToDialog.LineNum.Text) - 1
                    Case Else
                End Select

            End If

        Catch ex As Exception
            Throw
        Finally

            If GoToDialog IsNot Nothing Then GoToDialog.Dispose()
            GoToDialog = Nothing

        End Try
    End Sub

    Public Sub CopyRows()

        Dim DGExport As Export

        Try

            Me.FindForm.Cursor = Cursors.WaitCursor

            DGExport = New Export

            DGExport.Copy(Me, GetCurrentDataTable.TableName, _CopySelectedOnly)

        Catch ex As Exception
            Throw
        Finally

            Me.FindForm.Cursor = Cursors.Default

            If DGExport IsNot Nothing Then DGExport.Dispose()
            DGExport = Nothing

        End Try
    End Sub

    Public Sub CustomizeCols()

        Dim ColSel As ColumnSelectorDialog

        Try

            ColSel = New ColumnSelectorDialog(If(_APPKEY, ""), Me, 1)

            If ColSel.ShowDialog(Me) = DialogResult.Cancel Then
            Else
                Select Case ColSel.ColumnsSelected
                    Case 0
                        MenuCustomizeColumns.Text = "Customize Columns"
                    Case Else
                        MenuCustomizeColumns.Text = "Customize Columns (Add/Reset)"
                End Select

                If _XMLFileName.Length > 4 Then
                    TableStyleReset(Me, New EventArgs)
                End If

                RaiseEvent ResetTableStyle(Me, New EventArgs)
            End If

        Catch ex As Exception
            Throw
        Finally
            If ColSel IsNot Nothing Then ColSel.Dispose()
            ColSel = Nothing

        End Try

    End Sub

    Public Sub SelectExportColsRowsWithStyleSubKey(ByVal styleName As String, ByVal subKey As String)

        Try

            _SubKey = subKey
            _StyleName = styleName

            SelectExportCols()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub SelectExportCols()

        Dim ColSel As ColumnSelectorDialog

        Try

            ColSel = New ColumnSelectorDialog(If(_APPKEY, ""), Me, 0, _SubKey)

            ColSel.ShowDialog(Me)

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During Copy", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If ColSel IsNot Nothing Then ColSel.Dispose()
            ColSel = Nothing

        End Try

    End Sub

    Public Sub ExportRowsWithStyleSubKey(ByVal styleName As String, ByVal subKey As String, Optional formatOutput As Boolean = True)

        Try

            _SubKey = subKey
            _StyleName = styleName

            ExportRows(formatOutput, subKey.Contains("\ALL"))

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ExportRows(formatOutput As Boolean, Optional AllData As Boolean = False)

        Dim SpecificColumns() As String = Nothing
        Dim DGExport As Export

        Try

            If Not AllData Then
                SpecificColumns = GetColumns(If(_APPKEY, ""), Me.Name & "\" & Me.GetCurrentTableStyle.MappingName & _SubKey & "\Export\ColumnSelector")
                If SpecificColumns Is Nothing OrElse Not SpecificColumns.Any Then

                    SelectExportCols()
                    SpecificColumns = GetColumns(If(_APPKEY, ""), Me.Name & "\" & Me.GetCurrentTableStyle.MappingName & _SubKey & "\Export\ColumnSelector")

                    If SpecificColumns Is Nothing Then
                        MessageBox.Show("No columns selected therefore Export has been cancelled", "Export Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If

                End If
            End If

            DGExport = New Export(_StyleName)

            Dim FileName As String = DGExport.Export(Me, Me.GetCurrentDataTable.TableName, _Export_SelectedOnly, SpecificColumns, formatOutput, "", True)

            If FileName IsNot Nothing AndAlso FileName.Length > 0 Then
                If MessageBox.Show("Selected content has been exported. Do you want to open the file now?" & vbCrLf & DGExport.SaveAsName, "Export Successful", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
                    FileName = """" & FileName & """"
                    Process.Start("EXCEL.EXE", FileName)
                End If
            End If

        Catch ex As Exception
            Throw
        Finally

            Me.FindForm.Cursor = Cursors.Default

            If DGExport IsNot Nothing Then DGExport.Dispose()
            DGExport = Nothing

        End Try

    End Sub

    Public Sub PrintRowsWithStyleSubKey(ByVal styleName As String, ByVal subKey As String, Optional formatOutput As Boolean = True)
        _SubKey = subKey
        _StyleName = styleName

        PrintRows(formatOutput)
    End Sub

    Private Sub PrintRows(Optional formatOutput As Boolean = True)

        Dim TempFileName As String
        Dim DGExport As Export

        Dim Setting(,) As String = Nothing


        Try
            Me.FindForm.Cursor = Cursors.WaitCursor

            Setting = GetAllSettings(If(_APPKEY, ""), Me.Name & "\" & Me.GetCurrentDataTable.TableName & _SubKey & "\Export\ColumnSelector")
            If Setting Is Nothing Then
                SelectExportCols()
                Setting = GetAllSettings(If(_APPKEY, ""), Me.Name & "\" & Me.GetCurrentDataTable.TableName & _SubKey & "\Export\ColumnSelector")
                If Setting Is Nothing Then
                    MessageBox.Show("No columns selected therefore Print has been cancelled", "Print Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If

            End If

            Dim SpecificColumns(Setting.GetUpperBound(0)) As String

            DGExport = New Export(_StyleName)
            Dim y As Integer = -1
            For x As Integer = 0 To Setting.GetUpperBound(0)
                y += 1
                SpecificColumns(y) = Replace(Replace(Setting(x, 0), "Col ", ""), " Export", "")
            Next

            TempFileName = DGExport.Export(Me, GetCurrentDataTable.TableName, False, SpecificColumns, formatOutput, IO.Path.GetTempFileName, True)

            If TempFileName.Length > 0 Then
                Export.Print(TempFileName)
                MessageBox.Show("Selected content has been Sent To The Printer", "Print Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If


        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During Export", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

            Me.FindForm.Cursor = Cursors.Default

            Try
                Kill(TempFileName)
            Catch IgnoreException As Exception
                'Debug.Print(IgnoreException.ToString)
            End Try

            If DGExport IsNot Nothing Then DGExport.Dispose()
            DGExport = Nothing

        End Try
    End Sub

    Public Sub MoveColumn(ByVal fromCol As Integer, ByVal toCol As Integer)

        ''Debug.Print("MoveColumn In:" & Me.GetGridSortColumn)

        Dim OldDTTS As DataGridTableStyle
        Dim NewDTTS As DataGridTableStyle
        Dim TextCol As DataGridHighlightTextBoxColumn

        Try
            If toCol < 0 Then toCol = 0

            If fromCol = toCol Then Return

            OldDTTS = Me.GetTableStyleFromGrid
            If OldDTTS Is Nothing Then Return

            NewDTTS = New DataGridTableStyle(CType(Me.BindingContext(Me.DataSource, Me.DataMember), CurrencyManager))
            NewDTTS.GridColumnStyles.Clear()

            If toCol > OldDTTS.GridColumnStyles.Count - 1 Then toCol = OldDTTS.GridColumnStyles.Count - 1

            NewDTTS.MappingName = OldDTTS.MappingName

            NewDTTS.AlternatingBackColor = OldDTTS.AlternatingBackColor
            NewDTTS.BackColor = OldDTTS.BackColor
            NewDTTS.ForeColor = OldDTTS.ForeColor
            NewDTTS.GridLineColor = OldDTTS.GridLineColor
            NewDTTS.GridLineStyle = OldDTTS.GridLineStyle
            NewDTTS.HeaderBackColor = OldDTTS.HeaderBackColor
            NewDTTS.HeaderFont = OldDTTS.HeaderFont
            NewDTTS.HeaderForeColor = OldDTTS.HeaderForeColor
            NewDTTS.SelectionBackColor = OldDTTS.SelectionBackColor
            NewDTTS.SelectionForeColor = OldDTTS.SelectionForeColor
            NewDTTS.LinkColor = OldDTTS.LinkColor
            NewDTTS.PreferredColumnWidth = OldDTTS.PreferredColumnWidth
            NewDTTS.PreferredRowHeight = OldDTTS.PreferredRowHeight
            NewDTTS.AllowSorting = OldDTTS.AllowSorting
            NewDTTS.ColumnHeadersVisible = OldDTTS.ColumnHeadersVisible
            NewDTTS.RowHeadersVisible = OldDTTS.RowHeadersVisible
            NewDTTS.ReadOnly = OldDTTS.ReadOnly
            NewDTTS.RowHeaderWidth = OldDTTS.RowHeaderWidth

            Dim i As Integer

            i = 0
            While i < OldDTTS.GridColumnStyles.Count
                If i <> fromCol AndAlso fromCol < toCol Then
                    NewDTTS.GridColumnStyles.Add(OldDTTS.GridColumnStyles(i))
                End If

                If i = toCol Then
                    NewDTTS.GridColumnStyles.Add(OldDTTS.GridColumnStyles(fromCol))
                End If

                If i <> fromCol AndAlso fromCol > toCol Then
                    NewDTTS.GridColumnStyles.Add(OldDTTS.GridColumnStyles(i))
                End If

                i += 1
            End While

            If Me.TableStyles.Contains(OldDTTS) Then Me.TableStyles.Remove(OldDTTS)

            Me.TableStyles.Add(NewDTTS)
            Me.Refresh()

            Try
                If Me.GetGridRowCount > 0 Then
                    Me.CurrentCell = New DataGridCell(0, toCol)
                    Me.CurrentRowIndex = 0
                Else
                    Me.ScrollToColumn(toCol)
                End If

                Dim GCS As DataGridColumnStyle
                For Each GCS In NewDTTS.GridColumnStyles
                    If TypeOf GCS Is DataGridHighlightTextBoxColumn Then
                        TextCol = CType(GCS, DataGridHighlightTextBoxColumn)
                        _HighlightedRow = Nothing
                        TextCol.HighlightRow = Nothing
                    End If
                Next
            Catch IgnoreException As Exception
                'Debug.Print(IgnoreException.ToString)
            End Try

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During MoveColumn", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If OldDTTS IsNot Nothing Then OldDTTS.Dispose()
            OldDTTS = Nothing
            If NewDTTS IsNot Nothing Then NewDTTS.Dispose()
            NewDTTS = Nothing

            ''Debug.Print("MoveColumn Out:" & Me.GetGridSortColumn)
        End Try
    End Sub

    Public Sub ScrollToRow(ByVal row As Integer)
        Me.MoveGridToRow(row)
    End Sub

    Public Sub ScrollToColumn(ByVal column As Integer)
        Me.MoveGridToColumn(column)
    End Sub

    Public Sub SaveColumnSizeAndPosition(ByVal section As String, ByVal Col As DataGridColumnStyle, ByVal colPosition As Integer)

        If Col Is Nothing Then Return

        SaveSetting(If(_APPKEY, ""), section, "Col " & Col.MappingName & " Pos", CStr(colPosition))
        If Col.Width > 0 Then SaveSetting(If(_APPKEY, ""), section, "Col " & Col.MappingName, Col.Width.ToString)

    End Sub

    Public Sub SaveColumnsSizeAndPosition(ByVal section As String)
        Dim DGStyle As DataGridTableStyle
        Dim ColPosition As Integer = 0

        Try

            If Me.DataSource Is Nothing Then Return

            DGStyle = Me.GetCurrentTableStyle

            If DGStyle IsNot Nothing Then
                ColPosition = 0
                For Each Col As DataGridColumnStyle In DGStyle.GridColumnStyles
                    SaveColumnSizeAndPosition(section, Col, ColPosition)
                    ColPosition += 1
                Next

            End If

        Catch ex As Exception
            Throw
        Finally
            DGStyle = Nothing
        End Try

    End Sub

    Public Function LoadColumnsSizeAndPosition(ByVal section As String, ByVal columnTable As DataTable) As DataTable
        ' <summary>  
        '<para>This method will Load Column size and Position of column from Registry </para>  
        ' </summary>  
        ' <param name="APPKEY">Name of the application or project to which the setting applies.</param> 
        '  <param name="Section">The name of the section in which the key setting is being saved.</param> 
        '  <param name="ColumnTable">Columns from the xml file in the form of a DataTable </param> 

        Dim ResultDT As DataTable
        Dim StoreRowsDT As DataTable
        Dim DV2 As DataView

        Try

            StoreRowsDT = columnTable.Clone

            For Each DR As DataRow In columnTable.Rows
                StoreRowsDT.ImportRow(DR)
            Next

            StoreRowsDT.Columns.Add("LastSavedPosition", System.Type.GetType("System.Int32"))
            StoreRowsDT.Columns.Add("CurrentPosition", System.Type.GetType("System.Int32"))
            StoreRowsDT.Columns.Add("SizeInPixels", System.Type.GetType("System.Int32"))
            StoreRowsDT.Columns.Add("LastSavedSizeInPixels")

            ResultDT = StoreRowsDT.Clone

            For Each DR As DataRow In StoreRowsDT.Rows
                DR("LastSavedPosition") = GetSetting(If(_APPKEY, ""), section, "Col " & DR("Mapping").ToString.Trim & " Pos", "-1")
                DR("LastSavedSizeInPixels") = GetSetting(If(_APPKEY, ""), section, "Col " & DR("Mapping").ToString.Trim, "-1")

                DR("SizeInPixels") = If(DR("LastSavedSizeInPixels").ToString = "-1", UFCWGeneral.MeasureWidthinPixels(CInt(DR("defaultcharwidth")), Me.Font.Name.ToString, Me.Font.SizeInPoints), DR("LastSavedSizeInPixels"))
            Next

            DV2 = New DataView(StoreRowsDT, "LastSavedPosition <> '-1'", "", DataViewRowState.CurrentRows)

            Dim PositionsFound As Integer = DV2.Count

            For Each DR As DataRow In StoreRowsDT.Rows
                Select Case DR("LastSavedPosition").ToString.Trim
                    Case "-1"
                        PositionsFound += 1
                        DR("DefaultOrder") = PositionsFound
                    Case Else
                        DR("DefaultOrder") = CInt(DR("LastSavedPosition"))
                End Select

                DR("CurrentPosition") = CInt(DR("DefaultOrder")) 'convert to integer so sort works correctly
            Next

            DV2 = New DataView(StoreRowsDT, "", "CurrentPosition", DataViewRowState.CurrentRows)

            For Each DRV2 As DataRowView In DV2
                ResultDT.ImportRow(DRV2.Row)
            Next

            Return ResultDT

        Catch ex As Exception
            Throw
        Finally

            If DV2 IsNot Nothing Then DV2.Dispose()
            DV2 = Nothing

            If ResultDT IsNot Nothing Then ResultDT.Dispose()
            ResultDT = Nothing

            If StoreRowsDT IsNot Nothing Then StoreRowsDT.Dispose()
            StoreRowsDT = Nothing

        End Try
    End Function

    Public Sub SaveSortBy(ByVal sortBy As String)
        SaveSortBy(Me.Name & "\" & Me.GetCurrentTableStyle.MappingName & "\Sort", sortBy)
    End Sub

    Private Sub SaveSortBy(ByVal section As String, ByVal sortBy As String)
        ' <summary>  
        '<para>When sort by a column in DG this method will save the column name in Registry </para>  
        ' </summary> 
        '  <param name="APPKEY">Name of the application or project to which the setting applies.</param> 
        '  <param name="Section">The name of the section in which the key setting is being saved.</param> 
        '  <param name="ColumnName">Column Name of the Sorted DG </param>  

        Dim Setting(,) As String = Nothing
        Dim RegKey As Microsoft.Win32.RegistryKey

        Try

            If Me.DataSource Is Nothing Then Exit Sub

            If sortBy IsNot Nothing AndAlso sortBy.Trim.Length > 0 Then
            Else
                sortBy = If(sortBy, "").Trim
            End If

            Setting = GetAllSettings(If(_APPKEY, ""), section)
            If Setting IsNot Nothing OrElse sortBy.Length < 1 Then
                Registry.CurrentUser.DeleteSubKeyTree("Software\VB and VBA Program Settings\" & If(Me.AppKey & "\", "") & section, False)
            End If

            If sortBy.Length < 1 Then
                Exit Sub
            End If

            RegKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\VB and VBA Program Settings\" & If(Me.AppKey & "\", "") & section, Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree)
            RegKey.SetValue("Col " & sortBy, "0")
            RegKey.Flush()

        Catch ex As Exception

            Throw

        Finally
            RegKey = Nothing
        End Try
    End Sub

    Public Function LastSortedBy() As String

        If Me.Name IsNot Nothing AndAlso Me.GetCurrentTableStyle IsNot Nothing Then
            Return If(LastSortedBy(Me.Name & "\" & Me.GetCurrentTableStyle.MappingName & "\Sort"), Nothing)
        End If

        Return Nothing

    End Function

    Private Function LastSortedBy(ByVal section As String) As String
        ' <summary>  
        '<para>This method will retrieve Saved Sorted column name in Registry </para>  
        ' </summary>
        ' <param name="APPKEY">Name of the application or project to which the setting applies.</param> 
        ' <param name="Section">The name of the section in which the key setting is being saved.</param> 
        ' <param name="DT">Columns from the xml file in the form of a DataTable </param> 

        Dim ColumnName As New StringBuilder
        Dim Setting(,) As String

        Try
            Setting = GetAllSettings(If(_APPKEY, ""), section)

            If Setting Is Nothing Then Return Nothing

            ''Debug.Print(section & " : " & Setting.GetLength(0).ToString)

            For x = 0 To Setting.GetLength(0) - 1
                Setting(x, 0) = Setting(x, 0).Replace("Col ", "")
            Next

            For x = 0 To Setting.GetLength(0) - 1
                Dim SortCols() As String = Setting(x, 0).Split(","c).[Select](Function(p) p.Trim()).ToArray()
                For y = 0 To SortCols.GetLength(0) - 1
                    If Me.GetCurrentDataTable.Columns.Contains(SortCols(y).ToUpper.Replace(" DESC", "").Replace(" ASC", "").Replace("[", "").Replace("]", "")) Then
                        ColumnName.Append(If(ColumnName.Length > 0, ", ", "") & SortCols(y).Trim)
                    End If
                Next
            Next

            If ColumnName.ToString.Split(CChar(",")).Length > 1 Then
                MenuMultiColSort.Checked = True
            Else
                MenuMultiColSort.Checked = False
            End If

            Return ColumnName.ToString

        Catch ex As Exception
            Throw
        End Try
    End Function

    Sub MoveGridToRow(ByVal row As Integer)
        Try
            If Me.DataSource IsNot Nothing Then
                Me.GridVScrolled(Me, New ScrollEventArgs(ScrollEventType.LargeIncrement, row))
            End If
        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub

    Sub MoveGridToColumn(ByVal column As Integer)
        Dim Value As Integer = 0

        Try

            If Me.DataSource IsNot Nothing Then
                Me.GridHScrolled(Me, New ScrollEventArgs(ScrollEventType.First, 0))

                If Me.FirstVisibleColumn < column Then
                    Do Until Me.FirstVisibleColumn >= column
                        'If Me.FirstVisibleColumn + Me.VisibleColumnCount >= column Then Exit Do
                        If Value = Me.HorizScrollBar.Maximum Then Exit Do

                        Me.GridHScrolled(Me, New ScrollEventArgs(ScrollEventType.SmallIncrement, Value))
                        Value += 1
                    Loop
                ElseIf Me.FirstVisibleColumn = column Then
                    'do nothing
                End If

            End If
        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub

    Public Overridable Function GetCurrentTableStyle() As DataGridTableStyle
        Dim DT As DataTable
        Dim DGTS As DataGridTableStyle

        Try

            If Me.DataSource IsNot Nothing Then

                DT = GetCurrentDataTable()
                If DT IsNot Nothing Then
                    If Me.TableStyles(DT.TableName) Is Nothing AndAlso Me.BindingContext IsNot Nothing Then
                        DGTS = New DataGridTableStyle(CType(Me.BindingContext(Me.DataSource, Me.DataMember), CurrencyManager))
                        Me.TableStyles.Add(DGTS)
                    Else
                        DGTS = Me.TableStyles(DT.TableName)
                    End If

                    Return DGTS

                End If
            End If

            Return Nothing

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During GetCurrentTableStyle", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            DT = Nothing
        End Try
    End Function

    Public Overridable Function GetTableStyleFromGrid() As DataGridTableStyle

        Dim DT As DataTable
        Dim DGTS As DataGridTableStyle


        Try

            If Me.DataSource Is Nothing Then Return Nothing

            DT = GetCurrentDataTable()

            If Me.TableStyles(DT.TableName) Is Nothing Then
                DGTS = New DataGridTableStyle(CType(Me.BindingContext(Me.DataSource, Me.DataMember), CurrencyManager))
            Else
                DGTS = Me.TableStyles(DT.TableName)
            End If

            Return DGTS

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During GetCurrentTableStyle", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

            If DGTS IsNot Nothing Then DGTS.Dispose()
            DGTS = Nothing

        End Try
    End Function
    <System.ComponentModel.Description("Gets current row from datasource. This is not necassarily the selected grid row but the row identified as CURRENT by the Currency Manager.")>
    Public Overridable Function GetCMCurrentRow() As DataRow
        Try

            If Me.DataSource IsNot Nothing Then

                If Me.DataSource.GetType.GetInterface("IList") IsNot Nothing Then
                    'the datasource Implements IList
                    Dim CM As CurrencyManager = CType(Me.BindingContext(Me.DataSource, Me.DataMember), CurrencyManager)

                    If CM.Position < 0 Then Return Nothing

                    If CM.Current.GetType Is GetType(DataView) Then
                        Return CType(Me.BindingContext(Me.DataSource).Current, DataRowView).Row
                    ElseIf CM.Current.GetType Is GetType(DataRowView) Then
                        'If bindingcontext(Me.datasource.tables(0)).Current.gettype Is GetType(DataRowView) Then
                        Return CType(CM.Current, DataRowView).Row
                    ElseIf DirectCast(Me.DataSource, System.Windows.Forms.BindingSource).List.GetType Is GetType(DataViewManager) Then
                        Dim DVM As DataViewManager = CType(DirectCast(Me.DataSource, System.Windows.Forms.BindingSource).List, DataViewManager)

                        If DVM.DataSet.Tables.Count < 1 Then
                            Return Nothing
                        End If

                        Dim ChildCM As CurrencyManager = CType(Me.BindingContext(DVM.DataSet, DVM.DataSet.Tables(CM.Position).TableName), CurrencyManager)

                        If ChildCM.Position < -0 Then Return Nothing

                        If ChildCM.Current.GetType Is GetType(DataView) Then
                            Return CType(Me.BindingContext(Me.DataSource).Current, DataRowView).Row
                        ElseIf ChildCM.Current.GetType Is GetType(DataRowView) Then
                            'If bindingcontext(Me.datasource.tables(0)).Current.gettype Is GetType(DataRowView) Then
                            Return CType(ChildCM.Current, DataRowView).Row
                        End If
                    End If

                    Throw New Exception("IList Current not DataView or DataRowView")

                ElseIf Me.DataSource.GetType.GetInterface("IListSource") IsNot Nothing Then

                    Dim ParentCM As CurrencyManager = CType(Me.BindingContext(Me.DataSource), CurrencyManager)
                    Dim BSPosition As Integer = ParentCM.Position

                    If ParentCM.Position < 0 Then Return Nothing

                    If ParentCM.Current.GetType Is GetType(DataView) Then
                        Return CType(Me.BindingContext(Me.DataSource).Current, DataRowView).Row
                    ElseIf ParentCM.Current.GetType Is GetType(DataRowView) Then
                        'If bindingcontext(Me.datasource.tables(0)).Current.gettype Is GetType(DataRowView) Then
                        Return CType(ParentCM.Current, DataRowView).Row
                    ElseIf CType(Me.DataSource, IListSource).GetList.GetType Is GetType(DataViewManager) Then
                        Dim DVM As DataViewManager = CType(CType(Me.DataSource, IListSource).GetList, DataViewManager)
                        Dim ChildCM As CurrencyManager = CType(Me.BindingContext(DVM.DataSet, DVM.DataSet.Tables(BSPosition).TableName), CurrencyManager)

                        If ChildCM.Position < -0 Then Return Nothing

                        If ChildCM.Current.GetType Is GetType(DataView) Then
                            Return CType(Me.BindingContext(Me.DataSource).Current, DataRowView).Row
                        ElseIf ChildCM.Current.GetType Is GetType(DataRowView) Then
                            'If bindingcontext(Me.datasource.tables(0)).Current.gettype Is GetType(DataRowView) Then
                            Return CType(ChildCM.Current, DataRowView).Row
                        End If
                    End If

                    Throw New Exception("IListSource Current not DataView, DataRowView or DataViewManager")

                Else
                    'This should never really happen because the DataGrid throws an  
                    'exception if you try to set its datasource property to anything that 
                    'doesnt implement IList or IListSource.

                    Throw New Exception("A DataSource must implement IList or IListSource!")

                End If

            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    <System.ComponentModel.Description("Gets DataTable from Grid Datasource.")>
    Public Overridable Function GetCurrentDataTable() As DataTable
        Dim DVS As DataViewSetting

        Try

            If Me Is Nothing Then Return Nothing
            If Me.DataSource Is Nothing Then Return Nothing

            If Me.DataSource.GetType.GetInterface("IList") IsNot Nothing Then
                'the datasource Implements IList

                If TypeOf Me.DataSource Is BindingSource Then

                    If DirectCast(Me.DataSource, BindingSource) IsNot Nothing AndAlso DirectCast(Me.DataSource, BindingSource).DataSource IsNot Nothing Then

                        If DirectCast(CType(Me.DataSource, IList), BindingSource).List.GetType = GetType(DataViewManager) Then

                            If Me.DataMember IsNot Nothing AndAlso Me.DataMember.Length > 0 Then
                                DVS = CType(CType(CType(Me.DataSource, IList), BindingSource).List, DataViewManager).DataViewSettings(Me.DataMember)
                            ElseIf CType(CType(CType(Me.DataSource, IList), BindingSource).List, DataViewManager).DataViewSettings.Count = 0 Then
                                Return Nothing 'Datasource not yet setup for use
                            Else
                                DVS = CType(CType(CType(Me.DataSource, IList), BindingSource).List, DataViewManager).DataViewSettings(0)
                            End If

                            If DVS IsNot Nothing Then
                                Return DVS.Table
                            Else
                                Throw New Exception("DataViewSettings not matched.")
                            End If

                        Else
                            Return CType(DirectCast(CType(Me.DataSource, IList), BindingSource).List, DataView).Table
                        End If

                    ElseIf DirectCast(Me.DataSource, BindingSource).DataSource Is Nothing Then
                        Return Nothing
                    End If

                    Throw New Exception("DataSource Type not recognized. (" & Me.DataSource.GetType.ToString & ")")
                Else
                    Return CType(CType(Me.DataSource, IList), DataView).Table
                End If

            ElseIf Me.DataSource.GetType.GetInterface("IListSource") IsNot Nothing Then

                If CType(Me.DataSource, IListSource).GetList.GetType = GetType(DataViewManager) Then

                    If Me.DataMember IsNot Nothing AndAlso Me.DataMember.Length > 0 Then
                        DVS = CType(CType(Me.DataSource, IListSource).GetList, DataViewManager).DataViewSettings(Me.DataMember)
                    Else
                        Dim DVM As DataViewManager = CType(CType(Me.DataSource, IListSource).GetList, DataViewManager)
                        If DVM.DataViewSettings.Count = 1 Then
                            DVS = CType(CType(Me.DataSource, IListSource).GetList, DataViewManager).DataViewSettings(0)
                        End If
                    End If

                    If DVS IsNot Nothing Then
                        Return DVS.Table
                    Else
                        Throw New Exception("DataViewSettings not matched.")
                    End If

                ElseIf CType(Me.DataSource, IListSource).GetList.GetType = GetType(DataView) Then
                    Return CType(CType(Me.DataSource, IListSource).GetList, DataView).Table
                End If

                Throw New Exception("GetList is neither a DataViewSetting or DataView!")
            Else
                'This should never really happen because the DataGrid throws an  
                'exception if you try to set its datasource property to anything that 
                'doesnt implement IList or IListSource.

                Throw New Exception("A DataSource must implement IList or IListSource!")

            End If

        Catch ex As Exception
            Throw

        Finally
        End Try

    End Function

    <System.ComponentModel.Description("Gets Currency Manager supplied DataView from Grid Datasource. Do NOT Sort through this when using dataset binding.")>
    Public Overridable Function GetCurrentDataView() As DataView

        Dim CM As CurrencyManager

        Try

            If Me Is Nothing Then Return Nothing
            If Me.BindingContext Is Nothing Then Return Nothing
            If Me.DataSource Is Nothing Then Return Nothing

            If TypeOf Me.DataSource Is DataSet AndAlso Me.DataMember.Length > 0 AndAlso Me.BindingContext Is Nothing Then

                Return CType(Me.DataSource, DataSet).Tables(Me.DataMember).DefaultView

            ElseIf TypeOf Me.DataSource Is DataSet AndAlso Me.DataMember.Length > 0 Then

                CM = CType(Me.BindingContext(CType(Me.DataSource, DataSet), Me.DataMember), CurrencyManager)
                Return CType(CM.List, DataView)

            ElseIf TypeOf Me.DataSource Is DataView Then
                Return CType(Me.DataSource, DataView)
            ElseIf TypeOf Me.DataSource Is DataTable AndAlso Me.BindingContext Is Nothing Then
                Return CType(Me.DataSource, DataTable).DefaultView
            ElseIf TypeOf Me.DataSource Is DataTable Then
                CM = CType(Me.BindingContext(Me.DataSource), CurrencyManager)
                Return CType(CM.List, DataView)
            ElseIf TypeOf Me.DataSource Is BindingSource Then
                If DirectCast(Me.DataSource, BindingSource) IsNot Nothing AndAlso DirectCast(Me.DataSource, BindingSource).DataSource IsNot Nothing Then

                    If DirectCast(CType(Me.DataSource, IList), BindingSource).List.GetType = GetType(DataViewManager) Then

                        Dim DVS As DataViewSetting

                        If Me.DataMember IsNot Nothing AndAlso Me.DataMember.Length > 0 Then
                            DVS = CType(CType(CType(Me.DataSource, IList), BindingSource).List, DataViewManager).DataViewSettings(Me.DataMember)
                        ElseIf CType(CType(CType(Me.DataSource, IList), BindingSource).List, DataViewManager).DataViewSettings.Count = 0 Then
                            Return Nothing 'Datasource not yet setup for use
                        Else
                            DVS = CType(CType(CType(Me.DataSource, IList), BindingSource).List, DataViewManager).DataViewSettings(0)
                        End If

                        If DVS IsNot Nothing Then
                            Return DVS.Table.DefaultView
                        Else
                            Throw New Exception("DataViewSettings not matched.")
                        End If

                    Else
                        Return CType(DirectCast(CType(Me.DataSource, IList), BindingSource).List, DataView)
                    End If

                ElseIf DirectCast(Me.DataSource, BindingSource).DataSource Is Nothing Then
                    Return Nothing
                End If

            End If

            Return Nothing

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

    <System.ComponentModel.Description("Gets DefaultView from Grid Datasource.")>
    Public Overridable Function GetDefaultDataView() As DataView

        Try

            If Me Is Nothing Then Return Nothing
            If DataSource Is Nothing Then Return Nothing

            If TypeOf Me.DataSource Is DataSet AndAlso Me.DataMember <> "" Then
                Return CType(Me.DataSource, DataSet).Tables(Me.DataMember).DefaultView
            ElseIf TypeOf Me.DataSource Is DataView Then
                Return CType(Me.DataSource, DataView).ToTable.DefaultView
            ElseIf TypeOf Me.DataSource Is DataTable Then
                Return CType(Me.DataSource, DataTable).DefaultView
            ElseIf TypeOf Me.DataSource Is BindingSource Then

                If TypeOf CType(Me.DataSource, BindingSource).DataSource Is DataSet AndAlso CType(Me.DataSource, BindingSource).DataMember.Length > 0 Then
                    Return CType(CType(Me.DataSource, BindingSource).DataSource, DataSet).Tables(CType(Me.DataSource, BindingSource).DataMember).DefaultView
                ElseIf TypeOf CType(Me.DataSource, BindingSource).DataSource Is DataView Then
                    Return CType(CType(Me.DataSource, BindingSource).DataSource, DataView).ToTable.DefaultView
                ElseIf TypeOf CType(Me.DataSource, BindingSource).DataSource Is DataTable Then
                    Return CType(CType(Me.DataSource, BindingSource).DataSource, DataTable).DefaultView
                End If

            End If

            Return Nothing

        Catch ex As Exception
            Throw

        Finally
        End Try
    End Function

    Public Function GetColumnPosition(ByVal mappingName As String) As Integer?
        Dim DGTS As DataGridTableStyle

        Try
            DGTS = GetCurrentTableStyle()

            If DGTS Is Nothing Then Return Nothing

            If DGTS.GridColumnStyles(mappingName) IsNot Nothing Then
                Return DGTS.GridColumnStyles.IndexOf(DGTS.GridColumnStyles(mappingName))
            End If

            Return Nothing

        Catch ex As Exception
            Throw
        Finally
            DGTS = Nothing
        End Try
    End Function

    Public Function GetColumnPosition(ByVal gridColumnStyle As DataGridColumnStyle) As Integer?
        Dim DGTS As DataGridTableStyle

        Try
            DGTS = GetCurrentTableStyle()

            If DGTS Is Nothing Then Return Nothing

            If DGTS.GridColumnStyles(gridColumnStyle.MappingName) IsNot Nothing Then
                Return DGTS.GridColumnStyles.IndexOf(DGTS.GridColumnStyles(gridColumnStyle.MappingName))
            End If

            Return Nothing

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During GetColumnPosition", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            DGTS = Nothing
        End Try
    End Function

    Public Function GetColumnMapping(ByVal position As Integer) As String
        Dim DGTS As DataGridTableStyle

        Try
            DGTS = GetCurrentTableStyle()

            If DGTS Is Nothing OrElse position < 0 Then Return Nothing

            If DGTS.GridColumnStyles(position) IsNot Nothing Then
                Return DGTS.GridColumnStyles(position).MappingName
            End If

            Return Nothing

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During GetColumnMapping", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            DGTS = Nothing
        End Try
    End Function

    <System.ComponentModel.Description("Get/Set Sort appropriately based upon source datatype assigned to datasource.")>
    Public Overridable Property [Sort] As String
        Set(value As String)
            Dim DVS As DataViewSetting

            If Me.DataSource IsNot Nothing Then

                If Me.DataSource.GetType.GetInterface("IList") IsNot Nothing Then
                    'the datasource Implements IList

                    If TypeOf Me.DataSource Is BindingSource Then

                        If DirectCast(Me.DataSource, BindingSource) IsNot Nothing AndAlso DirectCast(Me.DataSource, BindingSource).DataSource IsNot Nothing Then

                            If DirectCast(CType(Me.DataSource, IList), BindingSource).List.GetType = GetType(DataViewManager) Then

                                If Me.DataMember IsNot Nothing AndAlso Me.DataMember.Length > 0 Then
                                    DVS = CType(CType(Me.DataSource, IListSource).GetList, DataViewManager).DataViewSettings(Me.DataMember)
                                ElseIf CType(CType(CType(Me.DataSource, IList), BindingSource).List, DataViewManager).DataViewSettings.Count = 0 Then
                                    Return 'datasource not set up
                                Else
                                    DVS = CType(CType(CType(Me.DataSource, IList), BindingSource).List, DataViewManager).DataViewSettings(0)
                                End If

                                If DVS IsNot Nothing Then
                                    If value Is Nothing OrElse value.ToString.Split(CChar(",")).Length > 1 OrElse DVS.Table.Columns.Contains(value.Replace("[", "").Replace("]", "").Replace(" DESC", "").Replace(" ASC", "")) Then
                                        If DVS.Sort <> value Then DVS.Sort = value
                                    End If
                                Else
                                    Throw New Exception("DataViewSettings not matched.")
                                End If

                            ElseIf DirectCast(CType(Me.DataSource, IList), BindingSource).List.GetType = GetType(DataView) Then

                                If value Is Nothing OrElse value.ToString.Split(CChar(",")).Length > 1 OrElse CType(DirectCast(CType(Me.DataSource, IList), BindingSource).List, DataView).Table.Columns.Contains(value.Replace("[", "").Replace("]", "").Replace(" DESC", "").Replace(" ASC", "")) Then
                                    If CType(DirectCast(CType(Me.DataSource, IList), BindingSource).List, DataView).Sort <> value Then CType(DirectCast(CType(Me.DataSource, IList), BindingSource).List, DataView).Sort = value
                                End If
                            Else
                                Throw New Exception("IList.GetList is neither a DataViewSetting or DataView!")
                            End If

                        Else
                            Return 'datasource not set up
                        End If

                    ElseIf TypeOf Me.DataSource Is DataView Then

                        If value Is Nothing OrElse value.ToString.Split(CChar(",")).Length > 1 OrElse DirectCast(Me.DataSource, DataView).Table.Columns.Contains(value.Replace("[", "").Replace("]", "").Replace(" DESC", "").Replace(" ASC", "")) Then
                            If DirectCast(Me.DataSource, DataView).Sort <> value Then DirectCast(Me.DataSource, DataView).Sort = value
                        End If

                    End If

                ElseIf Me.DataSource.GetType.GetInterface("IListSource") IsNot Nothing Then

                    If DirectCast(Me.DataSource, IListSource) IsNot Nothing Then

                        If CType(Me.DataSource, IListSource).GetList.GetType = GetType(DataViewManager) Then
                            If CType(Me.DataSource, IListSource).GetList.GetType = GetType(DataViewManager) AndAlso Me.DataMember IsNot Nothing AndAlso Me.DataMember.Length > 0 Then
                                DVS = CType(CType(Me.DataSource, IListSource).GetList, DataViewManager).DataViewSettings(Me.DataMember)
                            Else
                                DVS = CType(CType(Me.DataSource, IListSource).GetList, DataViewManager).DataViewSettings(GetCurrentDataTable.TableName)
                            End If

                            If DVS IsNot Nothing Then
                                If value Is Nothing OrElse value.ToString.Split(CChar(",")).Length > 1 OrElse DVS.Table.Columns.Contains(value.Replace("[", "").Replace("]", "").Replace(" DESC", "").Replace(" ASC", "")) Then
                                    If DVS.Sort <> value Then DVS.Sort = value
                                End If
                            Else
                                Throw New Exception("DataViewSettings not matched.")
                            End If

                        ElseIf CType(Me.DataSource, IListSource).GetList.GetType = GetType(DataView) Then

                            If value Is Nothing OrElse value.ToString.Split(CChar(",")).Length > 1 OrElse CType(CType(Me.DataSource, IListSource).GetList, DataView).Table.Columns.Contains(value.Replace("[", "").Replace("]", "").Replace(" DESC", "").Replace(" ASC", "")) Then
                                If CType(CType(Me.DataSource, IListSource).GetList, DataView).Sort <> value Then CType(CType(Me.DataSource, IListSource).GetList, DataView).Sort = value
                            End If
                        Else
                            Throw New Exception("IListSource.GetList is neither a DataViewSetting or DataView!")

                        End If

                    Else
                        Return 'datasource not set up
                    End If
                Else
                    'This should never really happen because the DataGrid throws an  
                    'exception if you try to set its datasource property to anything that 
                    'doesnt implement IList or IListSource.

                    Throw New Exception("A DataSource must implement IList or IListSource!")

                End If
            Else
            End If

            If value IsNot Nothing AndAlso value.ToString.Split(CChar(",")).Length > 1 Then
                MenuMultiColSort.Checked = True
            Else
                MenuMultiColSort.Checked = False
            End If

        End Set

        Get
            If Me.DataSource IsNot Nothing AndAlso Not IsDBNull(Me.DataSource) Then

                If Me.DataSource.GetType.GetInterface("IList") IsNot Nothing Then
                    'the datasource Implements IList

                    If TypeOf Me.DataSource Is BindingSource Then
                        Return CType(DirectCast(CType(Me.DataSource, IList), BindingSource).List, DataView).Sort.Trim
                    Else
                        Return CType(CType(Me.DataSource, IList), DataView).Sort.Trim
                    End If

                ElseIf Me.DataSource.GetType.GetInterface("IListSource") IsNot Nothing Then

                    If CType(Me.DataSource, IListSource).GetList.GetType = GetType(DataViewManager) Then

                        Dim DVS As DataViewSetting

                        If CType(Me.DataSource, IListSource).GetList.GetType = GetType(DataViewManager) AndAlso Me.DataMember IsNot Nothing AndAlso Me.DataMember.Length > 0 Then
                            DVS = CType(CType(Me.DataSource, IListSource).GetList, DataViewManager).DataViewSettings(Me.DataMember)
                        Else
                            DVS = CType(CType(Me.DataSource, IListSource).GetList, DataViewManager).DataViewSettings(GetCurrentDataTable.TableName)
                        End If

                        Return DVS.Sort.Trim

                    ElseIf CType(Me.DataSource, IListSource).GetList.GetType = GetType(DataView) Then

                        Return CType(CType(Me.DataSource, IListSource).GetList, DataView).Sort.Trim

                    Else
                        Throw New ApplicationException("GetList is neither a DataViewSetting or DataView!")

                    End If

                Else
                    'This should never really happen because the DataGrid throws an  
                    'exception if you try to set its datasource property to anything that 
                    'doesnt implement IList or IListSource.

                    Throw New ApplicationException("A DataSource must implement IList or IListSource!")

                End If
            End If
        End Get
    End Property

    Public Function GetGridRowCount() As Integer

        Dim RowCount As Integer = 0

        Try
            If Me.DataSource IsNot Nothing Then

                If Me.DataSource.GetType.GetInterface("IList") IsNot Nothing Then
                    'the datasource Implements IList

                    RowCount = CType(Me.DataSource, IList).Count

                ElseIf Me.DataSource.GetType.GetInterface("IListSource") IsNot Nothing Then

                    RowCount = CType(Me.DataSource, System.ComponentModel.IListSource).GetList.Count

                ElseIf Me.DataSource.GetType.GetInterface("BindingSource") IsNot Nothing Then

                    RowCount = CType(Me.DataSource, DataTable).Rows.Count

                Else
                    'This should never really happen because the DataGrid throws an  
                    'exception if you try to set its datasource property to anything that 
                    'doesnt implement IList or IListSource.

                    Throw New Exception("A DataSource must implement IList or IListSource!")

                End If

            End If

            Return RowCount

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During GetGridRowCount", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    Return 0
        End Try
    End Function

    Public Function GetGridColumnCount() As Integer

        Dim DGTS As DataGridTableStyle

        Try
            DGTS = Me.GetTableStyleFromGrid

            If DGTS Is Nothing Then Return 0

            Return DGTS.GridColumnStyles.Count

        Catch ex As Exception
            Throw
        Finally
            DGTS = Nothing
        End Try
    End Function

    Public Function GetTableRowFromGridPosition(ByVal gridRowIndex As Integer) As DataRow

        Dim BM As BindingManagerBase
        Dim DV As DataView
        Dim DRV As DataRowView

        Try
            BM = Me.BindingContext(Me.DataSource, Me.DataMember)
            DV = CType(BM.Current, DataRowView).DataView
            DRV = DV(gridRowIndex)

            Return DRV.Row

        Catch ex As Exception
            Throw
        Finally
            DRV = Nothing
            DV = Nothing
            BM = Nothing
        End Try
    End Function

    Public Function FirstVisibleRow() As Integer?


        Dim Pos As Integer

        Try
            If Me.GetGridRowCount > 0 Then
                Pos = Me.Height - 1

                _LastHitTestInfo = Me.HitTest(Pos, Me.RowHeaderWidth + 8)

                Do Until Pos <= 0 OrElse _LastHitTestInfo.Type = DataGrid.HitTestType.Cell
                    _LastHitTestInfo = Me.HitTest(Pos, Me.RowHeaderWidth + 8)
                    Pos -= 1
                Loop

                If _LastHitTestInfo.Type = DataGrid.HitTestType.Cell Then
                    Return _LastHitTestInfo.Row - 1
                End If
            End If

            Return Nothing

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During FirstVisibleRow", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
        End Try

    End Function

    Public Function GetVerticalScrollBar() As VScrollBar
        Return CType(Me.VertScrollBar, VScrollBar)
    End Function

    Public Function GetHorizontalScrollBar() As HScrollBar
        Return CType(Me.HorizScrollBar, HScrollBar)
    End Function

    Public Function GetSelectedDataRows() As ArrayList

        Dim AL As New ArrayList()
        Dim DV As DataView

        Try

            If Me.GetCurrentDataView IsNot Nothing Then
                DV = Me.GetCurrentDataView

                For I As Integer = 0 To DV.Count - 1
                    If Me.IsSelected(I) Then
                        AL.Add(DV(I).Row)
                        'ElseIf (Me.LastHitSpot IsNot Nothing AndAlso Me.LastHitSpot.Row = I) Then
                        '    Stop
                        '    AL.Add(DV(I).Row)
                    End If
                Next

            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & AL.Count.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Return AL

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Sub MenuRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuRefresh.Click
        RefreshData()
    End Sub

    Private Sub MenuAutoSizeByBoth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAutoSizeByBoth.Click

        Dim MenuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)

        AutoSize(AutoSizeType.Both)

    End Sub

    Private Sub MenuAutoSizeByHeight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAutoSizeByHeight.Click

        Dim MenuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)

        AutoSize(AutoSizeType.Rows)

    End Sub

    Private Sub MenuAutoSizeByWidth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuAutoSizeByWidth.Click

        Dim MenuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)

        AutoSize(AutoSizeType.Columns)

    End Sub

    Private Sub MenuFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuFind.Click
        ShowFindDialog()
    End Sub

    Private Sub MenuGoTo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuGoTo.Click
        ShowGoToDialog()
    End Sub

    Private Sub MenuCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuCopy.Click
        CopyRows()
    End Sub

    Private Sub MenuExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuExportAll.Click, MenuExportSelected.Click

        Select Case CType(sender, ToolStripMenuItem).Name
            Case "mnuExportSelected"
                _Export_SelectedOnly = True
            Case Else
                _Export_SelectedOnly = False
        End Select

        ExportRows(False)

    End Sub

    Private Sub MenuExportSelectCols_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuExportSelectCols.Click
        SelectExportCols()
    End Sub

    Private Sub MenuPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuPrint.Click
        PrintRows(False)
    End Sub

    Private Sub MenuDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuDelete.Click
        Delete()
    End Sub

    Private Sub MenuCustomizeColumns_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuCustomizeColumns.Click
        CustomizeCols()
    End Sub

    Private Sub MenuChooseColumns_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuChooseSortColumns.Click

        Try

            Using SD As New SortDialog(Me)
                If SD.ShowDialog() = DialogResult.OK Then

                    ProcessMultiSortRequest(SD.SortColumns)

                End If
            End Using

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During MenuChooseColumns_Click", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
        End Try
    End Sub

    Private Sub SortTmr_Elapsed(ByVal sender As System.Object, ByVal e As System.Timers.ElapsedEventArgs) Handles SortTmr.Elapsed
        Try
            Me.GetCurrentDataTable.DefaultView.Sort = _MultiSortString

            SortTmr.Enabled = False
        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During SortTmr_Elapsed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub HandleDragMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
        If _AllowDragDrop Then
            _DraggedMouseDownRow = -(1)
            _DraggedMouseDownCol = -(1)
        End If
        'add remove items from default context menu here ?

    End Sub

    Private Sub HandleDragMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
        If _AllowDragDrop Then
            _LastHitTestInfo = Me.HitTest(New Point(e.X, e.Y))
            _DraggedMouseDownRow = _LastHitTestInfo.Row
            _DraggedMouseDownCol = _LastHitTestInfo.Column
        End If
    End Sub

    Private Sub HandleDragMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
        Dim data As [String]

        If _AllowDragDrop Then
            _LastHitTestInfo = Me.HitTest(New Point(e.X, e.Y))
            If (e.Button = MouseButtons.Left) Then
                If (((_DraggedMouseDownRow = -(1)) _
                            OrElse (_DraggedMouseDownCol = -(1))) _
                            OrElse ((_LastHitTestInfo.Row = _DraggedMouseDownRow) _
                            AndAlso (_LastHitTestInfo.Column = _DraggedMouseDownCol))) Then
                    Return
                End If

                data = Me(_DraggedMouseDownRow, _DraggedMouseDownCol).ToString
                Me.DoDragDrop(data, DragDropEffects.Copy)

                _DraggedMouseDownRow = -(1)
                _DraggedMouseDownCol = -(1)
            End If
        End If
    End Sub

    Private Sub HandleDragOver(ByVal sender As Object, ByVal e As DragEventArgs)

        Try

            If _AllowDragDrop Then
                _LastHitTestInfo = Me.HitTest(Me.PointToClient(New Point(e.X, e.Y)))
                If (((_LastHitTestInfo.Row = _DraggedMouseDownRow) _
                            AndAlso (_LastHitTestInfo.Column = _DraggedMouseDownCol)) _
                            OrElse Not (e.Data.GetDataPresent("Text"))) Then
                    e.Effect = DragDropEffects.None
                Else
                    e.Effect = DragDropEffects.Copy
                End If
            End If

        Catch IgnoreException As Exception
            'Debug.Print(IgnoreException.ToString)
        Finally
        End Try

    End Sub

    Private Sub HandleDragEnter(ByVal sender As Object, ByVal e As DragEventArgs)
        If _AllowDragDrop Then
            If e.Data.GetDataPresent("Text") Then
                e.Effect = DragDropEffects.Copy
            End If
        End If
    End Sub

    Private Sub HandleDragDrop(ByVal sender As Object, ByVal e As DragEventArgs)

        Try

            If _AllowDragDrop Then
                If e.Data.GetDataPresent("Text") Then
                    _LastHitTestInfo = Me.HitTest(Me.PointToClient(New Point(e.X, e.Y)))
                    If ((_LastHitTestInfo.Row = -1) OrElse (_LastHitTestInfo.Column = -1)) Then
                        Return
                    Else
                        Me(_LastHitTestInfo.Row, _LastHitTestInfo.Column) = CType(e.Data.GetData("Text"), String)
                    End If
                End If
            End If
        Catch IgnoreException As Exception
            'Debug.Print(IgnoreException.ToString)
        Finally
        End Try
    End Sub

    Private Sub DefaultContextMenu_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles DefaultContextMenu.Opening

        DefaultContextMenuPrepare()

    End Sub

    Public Sub ContextMenuPrepare(Optional ByVal dataGridCustomContextMenu As System.Windows.Forms.ContextMenuStrip = Nothing)

        Dim DataGridCustomContextMenuCopy As ContextMenuStrip

        Try

            Me.ContextMenuStrip = Nothing

            If Me.DefaultContextMenu IsNot Nothing Then

                DataGridCustomContextMenuCopy = New ContextMenuStrip

                DefaultContextMenuPrepare() 'need to modify default menu to exclude items not permitted for use

                If dataGridCustomContextMenu IsNot Nothing Then

                    Dim eHandlers As EventHandlerList = dataGridCustomContextMenu.GetEventHandlerList

                    DataGridCustomContextMenuCopy.AddHandlers(dataGridCustomContextMenu)

                    For Each MenuItem In dataGridCustomContextMenu.Items

                        Select Case MenuItem.GetType
                            Case GetType(ToolStripMenuItem)
                                DataGridCustomContextMenuCopy.Items.Add(CType(MenuItem, ToolStripMenuItem).Clone)
                            Case GetType(ToolStripSeparator)
                                DataGridCustomContextMenuCopy.Items.Add(New ToolStripSeparator)
                            Case Else
                                DataGridCustomContextMenuCopy.Items.Add(New ToolStripSeparator)
                        End Select

                    Next

                    If DataGridCustomContextMenuCopy.Items.Count > 0 Then
                        If (DataGridCustomContextMenuCopy.Items.Count > 0 AndAlso Me.DefaultContextMenu.Items.Count > 0) AndAlso Not (TypeOf DataGridCustomContextMenuCopy.Items(DataGridCustomContextMenuCopy.Items.Count - 1) Is ToolStripSeparator) Then
                            DataGridCustomContextMenuCopy.Items.Add(New ToolStripSeparator())
                        End If

                        If dataGridCustomContextMenu IsNot Nothing Then ToolStripManager.RevertMerge(dataGridCustomContextMenu) 'prevent duplication of menu items

                        ToolStripManager.Merge(Me.DefaultContextMenu, DataGridCustomContextMenuCopy)
                    End If
                End If
            Else
                If dataGridCustomContextMenu IsNot Nothing Then

                    Dim eHandlers As EventHandlerList = dataGridCustomContextMenu.GetEventHandlerList

                    DataGridCustomContextMenuCopy.AddHandlers(dataGridCustomContextMenu)

                    For Each MenuItem In dataGridCustomContextMenu.Items

                        Select Case MenuItem.GetType
                            Case GetType(ToolStripMenuItem)
                                DataGridCustomContextMenuCopy.Items.Add(CType(MenuItem, ToolStripMenuItem).Clone)
                            Case GetType(ToolStripSeparator)
                                DataGridCustomContextMenuCopy.Items.Add(New ToolStripSeparator)
                            Case Else
                                DataGridCustomContextMenuCopy.Items.Add(New ToolStripSeparator)
                        End Select

                    Next

                    If DataGridCustomContextMenuCopy.Items.Count > 0 Then
                        If DataGridCustomContextMenuCopy.Items.Count > 0 AndAlso Not (TypeOf DataGridCustomContextMenuCopy.Items(DataGridCustomContextMenuCopy.Items.Count - 1) Is ToolStripSeparator) Then
                            DataGridCustomContextMenuCopy.Items.Add(New ToolStripSeparator())
                        End If

                        If dataGridCustomContextMenu IsNot Nothing Then ToolStripManager.RevertMerge(dataGridCustomContextMenu) 'prevent duplication of menu items

                    End If
                End If

            End If

            If DataGridCustomContextMenuCopy IsNot Nothing AndAlso DataGridCustomContextMenuCopy.Items.Count > 0 Then
                MyBase.ContextMenuStrip = DataGridCustomContextMenuCopy
            ElseIf Me.DefaultContextMenu IsNot Nothing AndAlso Me.DefaultContextMenu.Items.Count > 0 Then
                MyBase.ContextMenuStrip = Me.DefaultContextMenu
            End If

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Public Sub DefaultContextMenuPrepare()

        Dim GetUserGroups As StringCollection
        Dim ADGroupsThatCanExport() As String
        Dim ADGroupsThatCanPrint() As String
        Dim ADGroupsThatCanCopy() As String
        Dim ADGroupsThatCanFilter() As String
        Dim ADGroupsThatCanCustomize() As String
        Dim ADGroupsThatCanMultiSort() As String
        Dim ADGroupsThatCanFind() As String


        Try

            Me.MenuDelete.Available = False
            Me.ToolStripDeleteSeparator.Available = False
            Me.MenuCustomizeColumns.Available = False
            Me.MenuFilterByColumn.Available = False
            Me.ToolStripCustomizeSeparator.Available = False
            Me.MenuMultiColSort.Available = False
            Me.ToolStripSortSeparator.Available = False
            Me.MenuAutoSize.Available = False
            Me.MenuCopy.Available = False
            Me.MenuFind.Available = False
            Me.MenuGoTo.Available = False
            Me.MenuRefresh.Available = False
            Me.ToolStripExportSeparator.Available = False
            Me.MenuExportSelectCols.Available = False
            Me.MenuExport.Available = False
            Me.MenuPrint.Available = False

            Me.DefaultContextMenu.Items.Clear()

            If Me.DataSource Is Nothing Then Exit Sub

            GetUserGroups = UFCWGeneralAD.GetUserGroupMembership()

            ADGroupsThatCanExport = _ADGroupsThatCanExport.Split(CChar(","))
            ADGroupsThatCanPrint = _ADGroupsThatCanPrint.Split(CChar(","))
            ADGroupsThatCanCopy = _ADGroupsThatCanCopy.Split(CChar(","))
            ADGroupsThatCanFilter = _ADGroupsThatCanFilter.Split(CChar(","))
            ADGroupsThatCanCustomize = _ADGroupsThatCanCustomize.Split(CChar(","))
            ADGroupsThatCanFind = _ADGroupsThatCanFind.Split(CChar(","))
            ADGroupsThatCanMultiSort = _ADGroupsThatCanMultiSort.Split(CChar(","))


            If Not Me.ReadOnly AndAlso _AllowDelete AndAlso Me.GetGridRowCount > 0 Then
                Me.ToolStripDeleteSeparator.Available = True
                Me.MenuDelete.Available = True
            End If

            If _AllowFilter AndAlso Me.TableStyles("Default") IsNot Nothing AndAlso Me.GetGridRowCount > 0 Then
                For Each ADGroup As String In ADGroupsThatCanFilter
                    If GetUserGroups.Contains(ADGroup) OrElse ADGroup.Trim.Length = 0 Then
                        ToolStripCustomizeSeparator.Available = True
                        MenuFilterByColumn.Available = True
                        Exit For
                    End If
                Next
            End If

            If _AllowCustomize AndAlso Me.TableStyles("Default") IsNot Nothing Then
                For Each ADGroup As String In ADGroupsThatCanCustomize
                    If GetUserGroups.Contains(ADGroup) OrElse ADGroup.Trim.Length = 0 Then
                        ToolStripCustomizeSeparator.Available = True
                        MenuCustomizeColumns.Available = True
                        Exit For
                    End If
                Next
            End If

            If _AllowExport AndAlso Me.TableStyles("Default") IsNot Nothing AndAlso Me.GetGridRowCount > 0 Then
                For Each ADGroup As String In ADGroupsThatCanExport
                    If GetUserGroups.Contains(ADGroup) OrElse ADGroup.Trim.Length = 0 Then
                        ToolStripExportSeparator.Available = True
                        Me.MenuExport.DropDownItems.Clear()
                        Me.MenuExport.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuExportSelected, Me.MenuExportAll})
                        MenuExportSelectCols.Available = True
                        MenuExport.Available = True
                        Exit For
                    End If
                Next
            End If

            If _AllowPrint AndAlso Me.GetGridRowCount > 0 Then
                For Each ADGroup As String In ADGroupsThatCanPrint
                    If GetUserGroups.Contains(ADGroup) OrElse ADGroup.Trim.Length = 0 Then
                        ToolStripExportSeparator.Available = True
                        MenuPrint.Visible = True
                        Exit For
                    End If
                Next
            End If

            If _AllowMultiSort AndAlso Me.AllowSorting Then
                For Each ADGroup As String In ADGroupsThatCanMultiSort
                    If GetUserGroups.Contains(ADGroup) OrElse ADGroup.Trim.Length = 0 Then
                        ToolStripSortSeparator.Available = True
                        MenuMultiColSort.Available = True
                        Exit For
                    End If
                Next
            End If

            If _AllowAutoSize AndAlso Me.GetGridRowCount > 0 Then
                Me.MenuAutoSize.Available = True
                Me.MenuAutoSize.DropDownItems.Clear()
                Me.MenuAutoSize.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuAutoSizeByWidth, Me.MenuAutoSizeByHeight, Me.MenuAutoSizeByBoth})
            End If

            If _AllowCopy AndAlso Me.GetGridRowCount > 0 Then
                For Each ADGroup As String In ADGroupsThatCanCopy
                    If GetUserGroups.Contains(ADGroup) OrElse ADGroup.Trim.Length = 0 Then
                        Me.MenuCopy.Available = True
                        Exit For
                    End If
                Next
            End If

            If _AllowFind AndAlso Me.GetGridRowCount > 0 Then
                For Each ADGroup As String In ADGroupsThatCanFind
                    If GetUserGroups.Contains(ADGroup) OrElse ADGroup.Trim.Length = 0 Then
                        Me.MenuFind.Available = True
                        Exit For
                    End If
                Next
            End If

            If _AllowGoTo AndAlso Me.GetGridRowCount > 0 Then MenuGoTo.Available = True
            If _AllowRefresh Then MenuRefresh.Available = True

            Me.DefaultContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuDelete, Me.ToolStripDeleteSeparator, Me.MenuCustomizeColumns, Me.MenuFilterByColumn, Me.ToolStripCustomizeSeparator, Me.MenuMultiColSort, Me.ToolStripSortSeparator, Me.MenuAutoSize, Me.MenuCopy, Me.MenuFind, Me.MenuGoTo, Me.MenuRefresh, Me.ToolStripExportSeparator, Me.MenuExportSelectCols, Me.MenuExport, Me.MenuPrint})

            If DefaultContextMenu IsNot Nothing Then

                Using RestrictedContextMenu As New ContextMenuStrip
                    For x = DefaultContextMenu.Items.Count - 1 To 0 Step -1
                        If DefaultContextMenu.Items(x).Available = True Then RestrictedContextMenu.Items.Add(DefaultContextMenu.Items(x))
                    Next

                    Me.DefaultContextMenu.Items.Clear()
                    'reverse menu back to original order
                    For x = RestrictedContextMenu.Items.Count - 1 To 0 Step -1
                        DefaultContextMenu.Items.Add(RestrictedContextMenu.Items(x))
                    Next

                End Using

            End If

        Catch ex As Exception
            Throw
            'MessageBox.Show(ex.Message & ex.StackTrace, "Error During DefaultMenu_Popup", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally

        End Try

    End Sub

    Private Sub TableStyleReset(ByVal sender As Object, e As EventArgs)

        Dim dg As DataGridCustom = CType(sender, DataGridCustom)

        SetTableStyleColumns(dg.StyleName)

    End Sub

    <System.ComponentModel.Description("Call this method when the name of the datagrid matches the XML TableStyle Source for the datagrid.")>
    Public Sub SetTableStyle(ByVal ImageList As System.Windows.Forms.ImageList, Optional appKey As String = Nothing)
        Try
            _XMLFileName = Me.Name
            _ImageList = ImageList
            SetTableStyle(_XMLFileName, appKey)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    <System.ComponentModel.Description("Call this method when the name of the datagrid matches the XML TableStyle Source for the datagrid.")>
    Public Sub SetTableStyle()
        Try
            _XMLFileName = Me.Name
            SetTableStyle(_XMLFileName, Nothing)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    <System.ComponentModel.Description("Call this method when the requirement exists to provide an alternative XML TableStyle Source for the datagrid.")>
    Public Sub SetTableStyle(ByVal xmlName As String, Optional appKey As String = Nothing)

        Try

            If appKey IsNot Nothing Then
                _APPKEY = appKey
            End If

            SetTableStyleColumns(xmlName)

            RemoveHandler ResetTableStyle, AddressOf TableStyleReset 'remove to ensure not adding over and over again
            AddHandler ResetTableStyle, AddressOf TableStyleReset 'this assume that style processing is being handled entirely internally

            ContextMenuPrepare()

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub
    Private Sub SetTableStyleColumns(ByRef xmlName As String)

        Dim DGTS As New DataGridTableStyle
        Dim DGTSDefault As DataGridTableStyle

        Dim TextCol As DataGridFormattableTextBoxColumn
        Dim BoolCol As DataGridColorBoolColumn
        Dim IconCol As DataGridHighlightIconColumn
        Dim MultiLineTextCol As DataGridFormattableMultiLineTextBoxColumn

        Dim ColsDV As DataView
        Dim DefaultStyleDS As DataSet
        Dim ResultDT As DataTable
        Dim ColumnSequenceDV As DataView

        Dim TableDCs As DataColumnCollection

        Dim GetUserGroups As StringCollection

        Try

            _XMLFileName = xmlName.Replace(".XML", ".xml")
            _XMLFileName &= If(_XMLFileName.Contains(".xml"), "", ".xml")

            Me.StyleName = _XMLFileName.Replace(".xml", "")

            DefaultStyleDS = GetTableStyle(_XMLFileName)

            If DefaultStyleDS Is Nothing OrElse DefaultStyleDS.Tables.Count < 1 Then Return

            If Me.GetCurrentDataTable Is Nothing Then
                DGTS.MappingName = Me.Name
            Else
                DGTS.MappingName = Me.GetCurrentDataTable.TableName
                TableDCs = Me.GetCurrentDataTable.Columns
            End If

            DGTS.GridColumnStyles.Clear()

            If DefaultStyleDS.Tables.Contains(Me.StyleName & "Style") Then
                If DefaultStyleDS.Tables(Me.StyleName & "Style").Columns.Contains("GridLineStyle") Then
                    DGTS.GridLineStyle = If(CBool(DefaultStyleDS.Tables(Me.StyleName & "Style").Rows(0)("GridLineStyle")), DataGridLineStyle.Solid, DataGridLineStyle.None)
                End If
                If DefaultStyleDS.Tables(Me.StyleName & "Style").Columns.Contains("RowHeadersVisible") Then
                    DGTS.RowHeadersVisible = CBool(DefaultStyleDS.Tables(Me.StyleName & "Style").Rows(0)("RowHeadersVisible"))
                End If
                If DefaultStyleDS.Tables(Me.StyleName & "Style").Columns.Contains("GridDefaultSort") Then
                    _DefaultSort = DefaultStyleDS.Tables(Me.StyleName & "Style").Rows(0)("GridDefaultSort").ToString
                End If
            End If

            ResultDT = Me.LoadColumnsSizeAndPosition(Me.Name & "\" & DGTS.MappingName & "\ColumnSettings", DefaultStyleDS.Tables("Column"))

            ColumnSequenceDV = New DataView(ResultDT)
            ColsDV = ColumnSequenceDV

            DGTSDefault = New DataGridTableStyle With {
                .MappingName = "Default"
            } 'This can be used to establish the columns displayed by default

            For IntCol As Integer = 0 To ColsDV.Count - 1

                GetUserGroups = UFCWGeneralAD.GetUserGroupMembership()

                If ColsDV.Table.Columns.Contains("GAC") AndAlso ColsDV(IntCol).Item("GAC").ToString.Trim.Length > 0 Then

                    If Not GetUserGroups.Contains(ColsDV(IntCol).Item("GAC").ToString) Then
                        Continue For
                    End If

                End If

                If GetUserGroups.Contains("CMSLocals") Then
                    Select Case ColsDV(IntCol).Item("Mapping").ToString
                        Case "USERID", "CREATE_USERID", "SCANDATE", "INDEXDATE", "COMPLETEDBY"
                            Continue For
                    End Select
                End If

                If Not GetUserGroups.Contains("CMSCanReprocess") AndAlso Not GetUserGroups.Contains("CMSEligibility") Then
                    Select Case ColsDV(IntCol).Item("Mapping").ToString
                        Case "PENDED_TO", "PROCESSED_BY"
                            Continue For
                    End Select
                End If

                If Not GetUserGroups.Contains("CMSCanRunReports") Then
                    Select Case ColsDV(IntCol).Item("Mapping").ToString
                        Case "EMPLOYEE"
                            Continue For
                    End Select
                End If

                ' Hide column if its DefaultCharWidth has been set to 0 (zero) in the Xml.
                If (IsDBNull(ColsDV(IntCol).Item("DefaultCharWidth")) = False And CInt(ColsDV(IntCol).Item("DefaultCharWidth")) = 0) Then
                    ColsDV(IntCol).Item("Visible") = False
                End If

                If (IsDBNull(ColsDV(IntCol).Item("Visible")) OrElse ColsDV(IntCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(IntCol).Item("Visible")) = True) Then
                    TextCol = New DataGridFormattableTextBoxColumn(IntCol) With {
                        .MappingName = CStr(ColsDV(IntCol).Item("Mapping")),
                        .HeaderText = CStr(ColsDV(IntCol).Item("HeaderText"))
                    }

                    If IsDBNull(ColsDV(IntCol).Item("Format")) = False AndAlso ColsDV(IntCol).Item("Format").ToString.Trim <> "" Then
                        TextCol.Format = CStr(ColsDV(IntCol).Item("Format"))
                    End If

                    DGTSDefault.GridColumnStyles.Add(TextCol)
                End If

                If ((IsDBNull(ColsDV(IntCol).Item("Visible")) OrElse ColsDV(IntCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(IntCol).Item("Visible")) = True) AndAlso (GetAllSettings(If(_APPKEY, ""), Me.Name & "\" & DGTS.MappingName & "\Customize\ColumnSelector") Is Nothing OrElse CDbl(GetSetting(If(_APPKEY, ""), Me.Name & "\" & DGTS.MappingName & "\Customize\ColumnSelector", "Col " & ColsDV(IntCol).Item("Mapping").ToString & " Customize", "0")) = 1)) Then
                    If ColsDV(IntCol).Item("Type").ToString = "Text" AndAlso Not IsDBNull(ColsDV(IntCol).Item("WordWrap")) AndAlso ColsDV(IntCol).Item("WordWrap").ToString.Trim.Length > 0 AndAlso CBool(ColsDV(IntCol).Item("WordWrap").ToString) = True Then
                        MultiLineTextCol = New DataGridFormattableMultiLineTextBoxColumn With {
                            .MappingName = CStr(ColsDV(IntCol).Item("Mapping")),
                            .HeaderText = CStr(ColsDV(IntCol).Item("HeaderText")),
                            .Width = CInt(ColsDV(IntCol).Item("SizeInPixels"))
                        }

                        If Not IsDBNull(ColsDV(IntCol).Item("MinimumCharWidth")) AndAlso ColsDV(IntCol).Item("MinimumCharWidth").ToString.Trim.Length > 0 Then
                            MultiLineTextCol.MinimumCharWidth = CInt(ColsDV(IntCol).Item("MinimumCharWidth"))
                        End If
                        If Not IsDBNull(ColsDV(IntCol).Item("MaximumCharWidth")) AndAlso ColsDV(IntCol).Item("MaximumCharWidth").ToString.Trim.Length > 0 Then
                            MultiLineTextCol.MinimumCharWidth = CInt(ColsDV(IntCol).Item("MaximumCharWidth"))
                        End If

                        MultiLineTextCol.NullText = CStr(ColsDV(IntCol).Item("NullText"))

                        If Not IsDBNull(ColsDV(IntCol).Item("WordWrap")) AndAlso ColsDV(IntCol).Item("WordWrap").ToString.Trim.Length > 0 AndAlso CBool(ColsDV(IntCol).Item("WordWrap")) = True Then
                            MultiLineTextCol.TextBox.WordWrap = CBool(ColsDV(IntCol).Item("WordWrap"))
                        End If

                        If Not IsDBNull(ColsDV(IntCol).Item("ReadOnly")) AndAlso ColsDV(IntCol).Item("ReadOnly").ToString.Trim.Length > 0 AndAlso CBool(ColsDV(IntCol).Item("ReadOnly")) = True Then
                            MultiLineTextCol.ReadOnly = True
                        End If

                        If IsDBNull(ColsDV(IntCol).Item("Format")) = False AndAlso ColsDV(IntCol).Item("Format").ToString.Trim <> "" Then
                            Select Case ColsDV(IntCol).Item("Format").ToString.ToUpper
                                Case "YESNO", "DECIMALBOOLEAN2YESNO"
                                    AddHandler MultiLineTextCol.Formatting, AddressOf FormattingYesNo
                                Case "1STCHARACTERMASK"
                                    AddHandler MultiLineTextCol.Formatting, AddressOf Formatting1STCHARACTERMASK
                                Case "LOWER"
                                    AddHandler MultiLineTextCol.Formatting, AddressOf FormattingLOWER
                                Case "UPPER"
                                    AddHandler MultiLineTextCol.Formatting, AddressOf FormattingUPPER
                                Case "ORIGINAL"
                                    AddHandler MultiLineTextCol.Formatting, AddressOf FormattingORIGINAL
                                Case "ARCHIVE"
                                    AddHandler MultiLineTextCol.Formatting, AddressOf FormattingArchive
                                Case Else
                                    MultiLineTextCol.Format = CStr(ColsDV(IntCol).Item("Format"))
                                    Select Case TextCol.Format.ToUpper
                                        Case "C"
                                            AddHandler TextCol.UnFormatting, AddressOf UnFormattingCurrency
                                    End Select
                            End Select

                        End If

                        If IsDBNull(ColsDV(IntCol).Item("FormatIsRegEx")) = False AndAlso ColsDV(IntCol).Item("FormatIsRegEx").ToString.Trim <> "" Then
                            MultiLineTextCol.FormatIsRegEx = CBool(ColsDV(IntCol).Item("FormatIsRegEx"))
                        End If

                        DGTS.GridColumnStyles.Add(MultiLineTextCol)

                    ElseIf ColsDV(IntCol).Item("Type").ToString = "Text" Then

                        TextCol = New DataGridFormattableTextBoxColumn(IntCol) With {
                            .MappingName = CStr(ColsDV(IntCol).Item("Mapping")),
                            .HeaderText = CStr(ColsDV(IntCol).Item("HeaderText")),
                            .Width = CInt(ColsDV(IntCol).Item("SizeInPixels"))
                        }

                        If Not IsDBNull(ColsDV(IntCol).Item("MinimumCharWidth")) AndAlso ColsDV(IntCol).Item("MinimumCharWidth").ToString.Trim.Length > 0 Then
                            TextCol.MinimumCharWidth = CInt(ColsDV(IntCol).Item("MinimumCharWidth"))
                        End If
                        If Not IsDBNull(ColsDV(IntCol).Item("MaximumCharWidth")) AndAlso ColsDV(IntCol).Item("MaximumCharWidth").ToString.Trim.Length > 0 Then
                            TextCol.MinimumCharWidth = CInt(ColsDV(IntCol).Item("MaximumCharWidth"))
                        End If

                        TextCol.NullText = ColsDV(IntCol).Item("NullText").ToString

                        If Not IsDBNull(ColsDV(IntCol).Item("WordWrap")) AndAlso ColsDV(IntCol).Item("WordWrap").ToString.Trim.Length > 0 AndAlso CBool(ColsDV(IntCol).Item("WordWrap")) = True Then
                            TextCol.TextBox.WordWrap = CBool(ColsDV(IntCol).Item("WordWrap"))
                        End If

                        If Not IsDBNull(ColsDV(IntCol).Item("ReadOnly")) AndAlso ColsDV(IntCol).Item("ReadOnly").ToString.Trim.Length > 0 AndAlso CBool(ColsDV(IntCol).Item("ReadOnly")) = True Then
                            TextCol.ReadOnly = True
                        End If

                        If IsDBNull(ColsDV(IntCol).Item("Format")) = False AndAlso ColsDV(IntCol).Item("Format").ToString.Trim <> "" Then
                            Select Case ColsDV(IntCol).Item("Format").ToString.ToUpper
                                Case "YESNO", "DECIMALBOOLEAN2YESNO"
                                    AddHandler TextCol.Formatting, AddressOf FormattingYesNo
                                Case "1STCHARACTERMASK"
                                    AddHandler TextCol.Formatting, AddressOf Formatting1STCHARACTERMASK
                                Case "LOWER"
                                    AddHandler TextCol.Formatting, AddressOf FormattingLOWER
                                Case "UPPER"
                                    AddHandler TextCol.Formatting, AddressOf FormattingUPPER
                                Case "NOZERO"
                                    AddHandler TextCol.Formatting, AddressOf FormattingNoZero
                                Case "ARCHIVE"
                                    AddHandler TextCol.Formatting, AddressOf FormattingArchive
                                Case "NULLTEXT"
                                    AddHandler TextCol.NullTextSubstitution, AddressOf NullTextSubstitution
                                Case Else
                                    TextCol.Format = CStr(ColsDV(IntCol).Item("Format"))
                                    Select Case TextCol.Format.ToUpper
                                        Case "C"
                                            AddHandler TextCol.UnFormatting, AddressOf UnFormattingCurrency
                                    End Select

                            End Select

                        End If

                        If IsDBNull(ColsDV(IntCol).Item("FormatIsRegEx")) = False AndAlso ColsDV(IntCol).Item("FormatIsRegEx").ToString.Trim <> "" Then
                            TextCol.FormatIsRegEx = CBool(ColsDV(IntCol).Item("FormatIsRegEx"))
                        End If

                        DGTS.GridColumnStyles.Add(TextCol)

                    ElseIf ColsDV(IntCol).Item("Type").ToString = "Bool" OrElse ColsDV(IntCol).Item("Type").ToString = "Boolean" Then

                        BoolCol = New DataGridColorBoolColumn(IntCol) With {
                                                                                .MappingName = CStr(ColsDV(IntCol).Item("Mapping")),
                                                                                .HeaderText = CStr(ColsDV(IntCol).Item("HeaderText")),
                                                                                .Width = CInt(ColsDV(IntCol).Item("SizeInPixels")),
                                                                                .NullText = CStr(ColsDV(IntCol).Item("NullText")),
                                                                                .TrueValue = CType("1", Integer),
                                                                                .FalseValue = CType("0", Integer),
                                                                                .AllowNull = False
                                                                                }

                        If TableDCs IsNot Nothing AndAlso TableDCs.Contains(CStr(ColsDV(IntCol).Item("Mapping"))) Then

                            If TableDCs(CStr(ColsDV(IntCol).Item("Mapping"))).DataType Is GetType(Integer) Then

                                BoolCol.TrueValue = CType("1", Integer)
                                BoolCol.FalseValue = CType("0", Integer)

                            ElseIf TableDCs(CStr(ColsDV(IntCol).Item("Mapping"))).DataType Is GetType(Decimal) Then

                                BoolCol.TrueValue = CType("1", Decimal)
                                BoolCol.FalseValue = CType("0", Decimal)

                            ElseIf TableDCs(CStr(ColsDV(IntCol).Item("Mapping"))).DataType Is GetType(Boolean) Then

                                BoolCol.TrueValue = True
                                BoolCol.FalseValue = False

                            End If

                        Else
                            BoolCol.TrueValue = CType("1", Decimal)
                            BoolCol.FalseValue = CType("0", Decimal)
                        End If

                        Try

                            If Not IsDBNull(ColsDV(IntCol).Item("ReadOnly")) AndAlso ColsDV(IntCol).Item("ReadOnly").ToString.Trim.Length > 0 AndAlso CBool(ColsDV(IntCol).Item("ReadOnly")) = True Then
                                BoolCol.ReadOnly = True
                            End If

                        Catch IgnoreException As Exception
                            'Debug.Print(IgnoreException.ToString)
                        End Try

                        DGTS.GridColumnStyles.Add(BoolCol)

                    ElseIf ColsDV(IntCol).Item("Type").ToString = "BoolDecimal" Then
                        BoolCol = New DataGridColorBoolColumn(IntCol) With {
                            .MappingName = CStr(ColsDV(IntCol).Item("Mapping")),
                            .HeaderText = CStr(ColsDV(IntCol).Item("HeaderText")),
                            .Width = CInt(ColsDV(IntCol).Item("SizeInPixels")),
                            .NullText = CStr(ColsDV(IntCol).Item("NullText")),
                            .TrueValue = CType("1", Decimal),
                            .FalseValue = CType("0", Decimal),
                            .AllowNull = False
                        }

                        Try

                            If Not IsDBNull(ColsDV(IntCol).Item("ReadOnly")) AndAlso ColsDV(IntCol).Item("ReadOnly").ToString.Trim.Length > 0 AndAlso CBool(ColsDV(IntCol).Item("ReadOnly")) = True Then
                                BoolCol.ReadOnly = True
                            End If

                        Catch IgnoreException As Exception
                            'Debug.Print(IgnoreException.ToString)
                        End Try

                        DGTS.GridColumnStyles.Add(BoolCol)

                    ElseIf ColsDV(IntCol).Item("Type").ToString = "BoolInteger" Then
                        BoolCol = New DataGridColorBoolColumn(IntCol) With {
                            .MappingName = CStr(ColsDV(IntCol).Item("Mapping")),
                            .HeaderText = CStr(ColsDV(IntCol).Item("HeaderText")),
                            .Width = CInt(ColsDV(IntCol).Item("SizeInPixels")),
                            .NullText = CStr(ColsDV(IntCol).Item("NullText")),
                            .TrueValue = CType("1", Integer),
                            .FalseValue = CType("0", Integer),
                            .AllowNull = False
                        }

                        Try

                            If Not IsDBNull(ColsDV(IntCol).Item("ReadOnly")) AndAlso ColsDV(IntCol).Item("ReadOnly").ToString.Trim.Length > 0 AndAlso CBool(ColsDV(IntCol).Item("ReadOnly")) = True Then
                                BoolCol.ReadOnly = True
                            End If

                        Catch IgnoreException As Exception
                            'Debug.Print(IgnoreException.ToString)
                        End Try

                        DGTS.GridColumnStyles.Add(BoolCol)

                    ElseIf ColsDV(IntCol).Item("Type").ToString = "RealBoolean" Then
                        BoolCol = New DataGridColorBoolColumn(IntCol) With {
                            .MappingName = CStr(ColsDV(IntCol).Item("Mapping")),
                            .HeaderText = CStr(ColsDV(IntCol).Item("HeaderText")),
                            .Width = CInt(ColsDV(IntCol).Item("SizeInPixels")),
                            .NullText = CStr(ColsDV(IntCol).Item("NullText")),
                            .TrueValue = True,
                            .FalseValue = False,
                            .AllowNull = False
                        }

                        Try

                            If Not IsDBNull(ColsDV(IntCol).Item("ReadOnly")) AndAlso ColsDV(IntCol).Item("ReadOnly").ToString.Trim.Length > 0 AndAlso CBool(ColsDV(IntCol).Item("ReadOnly")) = True Then
                                BoolCol.ReadOnly = True
                            End If

                        Catch IgnoreException As Exception
                            'Debug.Print(IgnoreException.ToString)
                        End Try

                        DGTS.GridColumnStyles.Add(BoolCol)

                    ElseIf ColsDV(IntCol).Item("Type").ToString = "Icon" Then

                        IconCol = New DataGridHighlightIconColumn(Me, _ImageList) With {
                            .MappingName = CStr(ColsDV(IntCol).Item("Mapping")),
                            .HeaderText = CStr(ColsDV(IntCol).Item("HeaderText")),
                            .Width = CInt(ColsDV(IntCol).Item("SizeInPixels")),
                            .NullText = CStr(ColsDV(IntCol).Item("NullText")),
                            .MinimumCharWidth = CInt(ColsDV(IntCol).Item("MinimumCharWidth")),
                            .MaximumCharWidth = CInt(ColsDV(IntCol).Item("MaximumCharWidth"))
                        }

                        DGTS.GridColumnStyles.Add(IconCol)
                    End If
                End If

            Next

            Me.TableStyles.Clear()
            Me.TableStyles.Add(DGTS)
            Me.TableStyles.Add(DGTSDefault)

        Catch ex As Exception
        Finally
            If TextCol IsNot Nothing Then TextCol.Dispose()
            TextCol = Nothing

            If BoolCol IsNot Nothing Then BoolCol.Dispose()
            BoolCol = Nothing

            If IconCol IsNot Nothing Then IconCol.Dispose()
            IconCol = Nothing

            If ColsDV IsNot Nothing Then ColsDV.Dispose()
            ColsDV = Nothing

            If DGTSDefault IsNot Nothing Then DGTSDefault.Dispose()
            DGTSDefault = Nothing

            If ColumnSequenceDV IsNot Nothing Then ColumnSequenceDV.Dispose()
            ColumnSequenceDV = Nothing

            If DGTS IsNot Nothing Then DGTS.Dispose()
            DGTS = Nothing

            If DefaultStyleDS IsNot Nothing Then DefaultStyleDS.Dispose()
            DefaultStyleDS = Nothing

        End Try

    End Sub

    Private Sub NullTextSubstitution(ByRef Value As Object, NullText As String)

        Try
            If IsDBNull(Value) OrElse CStr(Value).Trim.Length = 0 Then
                Value = NullText

                'Select Case True
                '    Case Value.GetType Is GetType(System.String)
                '        Value = NullText
                '    Case Value.GetType Is GetType(System.Boolean)
                '        Value = CBool(NullText)
                '    Case Else 'assume numeric
                '        Value = CInt(NullText)
                'End Select
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub MenuFilterByColumn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MenuFilterByColumn.Click
        ShowFilterDialog()
    End Sub

    Private Sub DataGridCustom_LostFocus(sender As Object, e As EventArgs) Handles Me.LostFocus

        Dim CM As CurrencyManager

        Try
            If Me.BindingContext IsNot Nothing AndAlso DataSource IsNot Nothing Then
                CM = CType(BindingContext(DataSource, DataMember), CurrencyManager)
            Else
                Return
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub
    Protected Overrides Sub OnLeave(e As EventArgs)

        Dim CM As CurrencyManager

        Try

            MyBase.OnLeave(e)

            If Me.BindingContext IsNot Nothing AndAlso DataSource IsNot Nothing Then
                CM = CType(BindingContext(DataSource, DataMember), CurrencyManager)
            Else
                Return
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    Private Sub DataGridCustom_Leave(sender As Object, e As EventArgs) Handles Me.Leave

        Dim CM As CurrencyManager

        Try
            If Me.BindingContext IsNot Nothing AndAlso DataSource IsNot Nothing Then
                CM = CType(BindingContext(DataSource, DataMember), CurrencyManager)
            Else
                Return
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Private Sub DataGridCustom_GotFocus(sender As Object, e As EventArgs) Handles Me.GotFocus

        Dim CM As CurrencyManager

        Try
            If Me.BindingContext IsNot Nothing AndAlso DataSource IsNot Nothing Then
                CM = CType(BindingContext(DataSource, DataMember), CurrencyManager)
            Else
                Return
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try
    End Sub

    Private Sub DataGridCustom_Enter(sender As Object, e As EventArgs) Handles Me.Enter

        Dim CM As CurrencyManager

        Try
            If Me.BindingContext IsNot Nothing AndAlso DataSource IsNot Nothing Then
                CM = CType(BindingContext(DataSource, DataMember), CurrencyManager)
            Else
                Return
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid): CM(" & If(CM Is Nothing, "N/A", CM.Position.ToString & "/" & CM.Count.ToString) & ") PrevRowID(" & _PreviousRowID & ") RowID(" & _CurrentRowID & ") PrevPos(" & _PreviousBSPosition & ") CurPos(" & _CurrentBSPosition & ") SEL(" & If(CM Is Nothing OrElse DataSource Is Nothing OrElse CM.Count < 1 OrElse CM.Position < 0 OrElse CM.Position >= MyBase.VisibleRowCount, "N/A", Me.IsSelected(CM.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        End Try

    End Sub

    'Private Sub DataGrid_BindingManagerBase_CurrentChanged(sender As Object, e As EventArgs)

    '    Dim CurCnt As Integer?
    '    Dim CM As CurrencyManager

    '    Try


    '        CM = DirectCast(sender, CurrencyManager)

    '        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In(Grid):  PrevPos(" & __PreviousBSPosition & ") CurPos(" & __CurrentBSPosition & ") BSPos(" & CM.Position.ToString & ") " & Me.Name  & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    '        CurCnt = CType(sender, CurrencyManager).Count

    '        If _LastRowCount Is Nothing OrElse _LastRowCount <> CurCnt Then

    '            _LastRowCount = CurCnt

    '        End If

    '        RaiseEvent CurrentChanged(Me, e)

    '        'Debug.Print("DataGrid_CurrentChanged (Out): " & Me.Name  & " " & _LastSelectedHitTestRow.ToString & ":" & _LastBSPosition.ToString & ":" & CM.Position.ToString)

    '    Catch ex As Exception
    '        Throw
    '    Finally
    '        Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out(Grid):  PrevPos(" & __PreviousBSPosition & ") CurPos(" & __CurrentBSPosition & ") BSPos(" & CM.Position.ToString & ") " & Me.Name  & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

    '    End Try
    'End Sub

End Class

<System.Diagnostics.DebuggerStepThrough()>
Public Class ButtonValue
    Inherits Button

    Private ButtonValue As Object = ""

    Public Event ValueChanged(ByVal sender As Object, ByVal Value As Object)
    Public Event TagChanged(ByVal sender As Object, ByVal Tag As Object)

    Private _TagSet As Boolean = False
    Private _TagValue As Boolean = True

    Public Overridable Property ChangeValueWithTag() As Boolean
        Get
            Return _TagValue
        End Get
        Set(ByVal Value As Boolean)
            _TagValue = Value
        End Set
    End Property

    Public Overridable Property Value() As Object
        Get
            Return ButtonValue
        End Get
        Set(ByVal Value As Object)
            If ChangeValueWithTag Then Me.Tag = Value

            If IsDBNull(ButtonValue) AndAlso IsDBNull(Value) Then
                'do nothing
            ElseIf IsDBNull(ButtonValue) AndAlso IsDBNull(Value) Then
                RaiseEvent ValueChanged(Me, Value)
            ElseIf IsDBNull(ButtonValue) = False AndAlso IsDBNull(Value) Then
                RaiseEvent ValueChanged(Me, Value)
            ElseIf ButtonValue.ToString.Trim <> Value.ToString.Trim Then
                RaiseEvent ValueChanged(Me, Value)
            End If

            ButtonValue = Value
        End Set
    End Property

    Public Overridable Overloads Property Tag() As Object
        Get
            Return MyBase.Tag
        End Get
        Set(ByVal Value As Object)

            If _TagSet Then MyBase.Tag = Value : Exit Property
            _TagSet = True

            If MyBase.Tag Is Nothing OrElse (IsDBNull(MyBase.Tag) AndAlso IsDBNull(Value)) Then
                'do nothing
            ElseIf IsDBNull(MyBase.Tag) AndAlso IsDBNull(Value) = False Then
                RaiseEvent TagChanged(Me, Value)
            ElseIf IsDBNull(MyBase.Tag) = False AndAlso IsDBNull(Value) Then
                RaiseEvent TagChanged(Me, Value)
            ElseIf MyBase.Tag.ToString.Trim <> Value.ToString.Trim Then
                RaiseEvent TagChanged(Me, Value)
            End If

            If ChangeValueWithTag Then Me.Value = Value

            _TagSet = False
        End Set
    End Property
End Class

<System.Diagnostics.DebuggerStepThrough()>
Public Class ComboBoxNoKeyUp
    Inherits ComboBox

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Dim keyCode As Keys = CType((m.WParam.ToInt32 And Keys.KeyCode), Keys)

        If m.Msg = KeyState.WM_KEYUP AndAlso keyCode = Keys.Tab Then
            'ignore keyup to avoid problem with tabbing & dropdownlist;
            Return
        End If
        MyBase.WndProc(m)
    End Sub
End Class

<System.Diagnostics.DebuggerStepThrough()>
Public Class DateTimePickerNoKeyUp
    Inherits DateTimePicker

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Dim keyCode As Keys = CType((m.WParam.ToInt32 And Keys.KeyCode), Keys)

        If m.Msg = KeyState.WM_KEYUP And keyCode = Keys.Tab Then
            'ignore keyup to avoid problem with tabbing & dropdownlist;
            Return
        End If
        MyBase.WndProc(m)
    End Sub
End Class

<System.Diagnostics.DebuggerStepThrough()>
Public Class BoolValueChangedEventArgs
    Inherits EventArgs

    Private _column As Integer
    Private _row As Integer
    Private _value As Object

    Public Sub New(ByVal row As Integer, ByVal col As Integer, ByVal val As Object)
        MyBase.New()
        _row = row
        _column = col
        _value = val
    End Sub
    Public Property Column() As Integer
        Get
            Return _column
        End Get
        Set(ByVal Value As Integer)
            _column = Value
        End Set
    End Property
    Public Property Row() As Integer
        Get
            Return _row
        End Get
        Set(ByVal Value As Integer)
            _row = Value
        End Set
    End Property
    Public Property BoolValue() As Object
        Get
            Return _value
        End Get
        Set(ByVal Value As Object)
            _value = Value
        End Set
    End Property
End Class

<System.Diagnostics.DebuggerStepThrough()>
Public Class CurrentRowChangedEventArgs
    Inherits EventArgs

    Private _LastPosition As Integer?
    Private _LastRowIsSelected As Boolean?
    Private _LastDR As DataRow

    Public Sub New(ByVal lastPosition As Integer?, ByVal lastRowIsSelected As Boolean?, ByVal lastDR As DataRow)
        MyBase.New()
        _LastPosition = Me.LastPosition
        _LastDR = lastDR
        _LastRowIsSelected = lastRowIsSelected
    End Sub
    Public ReadOnly Property LastPosition() As Integer?
        Get
            Return _LastPosition
        End Get
    End Property
    Public ReadOnly Property LastDataRow() As DataRow
        Get
            Return _LastDR
        End Get
    End Property
    Public ReadOnly Property LastRowIsSelected() As Boolean?
        Get
            Return _LastRowIsSelected
        End Get
    End Property
End Class
'<System.Diagnostics.DebuggerStepThrough()>
Public Class PreventionKeys
    Implements IDisposable

    Private _KeyPreventDT As DataTable

    Sub New()
        _KeyPreventDT = New DataTable("Keys")

        _KeyPreventDT.Columns.Add("KeyID", System.Type.GetType("System.String"))
        _KeyPreventDT.Columns.Add("KeyState", System.Type.GetType("System.Int32"))
    End Sub

    Sub Add(ByVal key As Keys, ByVal keyState As KeyState)

        Dim DV As DataView
        Dim DR As DataRow

        Try

            DV = New DataView(_KeyPreventDT, "KeyID = '" & key.ToString & "' And KeyState = " & CInt(keyState) & "", "KeyID, KeyState", DataViewRowState.CurrentRows)

            If DV.Count < 1 Then
                DR = _KeyPreventDT.NewRow

                DR.Item("KeyID") = key.ToString
                DR.Item("KeyState") = CInt(keyState)

                _KeyPreventDT.Rows.Add(DR)
            End If

        Catch ex As Exception
            Throw
        Finally
            If DV IsNot Nothing Then DV.Dispose()
        End Try
    End Sub

    Sub Remove(ByVal key As Keys, ByVal keyState As KeyState)
        Dim DV As DataView

        Try

            DV = New DataView(_KeyPreventDT, "KeyID = '" & key.ToString & "' And KeyState = " & CInt(keyState) & "", "KeyID, KeyState", DataViewRowState.CurrentRows)

            If DV.Count > 0 Then
                DV.Delete(0)
            End If

        Catch ex As Exception
            Throw
        Finally
            If DV IsNot Nothing Then DV.Dispose()
        End Try

    End Sub

    Sub Clear()
        _KeyPreventDT.Rows.Clear()
    End Sub

    Function KeyExists(ByVal Key As Keys, ByVal KeyState As KeyState) As Boolean

        Dim DV As DataView
        Try

            DV = New DataView(_KeyPreventDT, "KeyID = '" & Key.ToString & "' And KeyState = " & CInt(KeyState) & "", "KeyID, KeyState", DataViewRowState.CurrentRows)

            If DV.Count > 0 Then Return True

            Return False

        Catch ex As Exception
            Throw
        Finally
            If DV IsNot Nothing Then DV.Dispose()
        End Try
    End Function

#Region "IDisposable Support"
    Private _Disposed As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not _Disposed Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                _KeyPreventDT?.Dispose()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        _Disposed = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class

Public Enum KeyState
    WM_SETCURSOR = &H20
    WM_KEYDOWN = &H100
    WM_KEYUP = &H101
    WM_CHAR = &H102
End Enum

Public Enum AutoSizeType
    Both = 0
    Columns = 1
    Rows = 2
End Enum



