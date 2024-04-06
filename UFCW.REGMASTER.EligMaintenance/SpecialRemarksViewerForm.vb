Option Strict On
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ComponentModel
Imports System.Configuration
Imports System.Windows.Forms

Public Class SpecialRemarksViewerForm
    Inherits System.Windows.Forms.Form

#Region "Variables"

    Private _APPKEY As String = "UFCW\RegMaster\"
    Private _FamilyID As Integer
    Private _SpecialRemarksDT As DataTable

    Friend WithEvents SpecialRemarksDataGrid As DataGridCustom
    Friend WithEvents ExitButton As System.Windows.Forms.Button

#End Region

#Region "Public Properties"
    <DefaultValue("UFCW\RegMaster\"), Browsable(True), System.ComponentModel.Description("Represents the application key used when accessing the registry.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal Value As String)
            _APPKEY = Value
        End Set
    End Property
#End Region

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    Public Sub New(ByVal FamilyID As Integer, ByVal dt As DataTable)
        Me.New()
        _FamilyID = FamilyID
        _SpecialRemarksDT = dt

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
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.SpecialRemarksDataGrid = New DataGridCustom()
        Me.ExitButton = New System.Windows.Forms.Button()
        CType(Me.SpecialRemarksDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SpecialremarksDataGrid
        '
        Me.SpecialRemarksDataGrid.ADGroupsThatCanCopy = ""
        Me.SpecialRemarksDataGrid.ADGroupsThatCanCustomize = ""
        Me.SpecialRemarksDataGrid.ADGroupsThatCanExport = ""
        Me.SpecialRemarksDataGrid.ADGroupsThatCanFilter = ""
        Me.SpecialRemarksDataGrid.ADGroupsThatCanFind = ""
        Me.SpecialRemarksDataGrid.ADGroupsThatCanPrint = ""
        Me.SpecialRemarksDataGrid.ADGroupsThatCanMultiSort = ""
        Me.SpecialRemarksDataGrid.AllowAutoSize = True
        Me.SpecialRemarksDataGrid.AllowColumnReorder = True
        Me.SpecialRemarksDataGrid.AllowCopy = True
        Me.SpecialRemarksDataGrid.AllowCustomize = True
        Me.SpecialRemarksDataGrid.AllowDelete = False
        Me.SpecialRemarksDataGrid.AllowDragDrop = False
        Me.SpecialRemarksDataGrid.AllowEdit = False
        Me.SpecialRemarksDataGrid.AllowExport = True
        Me.SpecialRemarksDataGrid.AllowFilter = True
        Me.SpecialRemarksDataGrid.AllowFind = True
        Me.SpecialRemarksDataGrid.AllowGoTo = True
        Me.SpecialRemarksDataGrid.AllowMultiSelect = False
        Me.SpecialRemarksDataGrid.AllowMultiSort = False
        Me.SpecialRemarksDataGrid.AllowNew = False
        Me.SpecialRemarksDataGrid.AllowPrint = True
        Me.SpecialRemarksDataGrid.AllowRefresh = False
        Me.SpecialRemarksDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SpecialRemarksDataGrid.AppKey = "UFCW\RegMaster\"
        Me.SpecialRemarksDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.SpecialRemarksDataGrid.CaptionVisible = False
        Me.SpecialRemarksDataGrid.ColumnHeaderLabel = Nothing
        Me.SpecialRemarksDataGrid.ColumnRePositioning = False
        Me.SpecialRemarksDataGrid.ColumnResizing = False
        Me.SpecialRemarksDataGrid.ConfirmDelete = True
        Me.SpecialRemarksDataGrid.CopySelectedOnly = True
        Me.SpecialRemarksDataGrid.DataMember = ""
        Me.SpecialRemarksDataGrid.DragColumn = 0
        Me.SpecialRemarksDataGrid.ExportSelectedOnly = True
        Me.SpecialRemarksDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.SpecialRemarksDataGrid.HighlightedRow = Nothing
        Me.SpecialRemarksDataGrid.IsMouseDown = False
        Me.SpecialRemarksDataGrid.LastGoToLine = ""
        Me.SpecialRemarksDataGrid.Location = New System.Drawing.Point(2, 3)
        Me.SpecialRemarksDataGrid.MultiSort = False
        Me.SpecialRemarksDataGrid.Name = "SpecialremarksDataGrid"
        Me.SpecialRemarksDataGrid.OldSelectedRow = 0
        Me.SpecialRemarksDataGrid.ReadOnly = True
        Me.SpecialRemarksDataGrid.SetRowOnRightClick = True
        Me.SpecialRemarksDataGrid.ShiftPressed = False
        Me.SpecialRemarksDataGrid.SingleClickBooleanColumns = True
        Me.SpecialRemarksDataGrid.Size = New System.Drawing.Size(468, 264)
        Me.SpecialRemarksDataGrid.StyleName = ""
        Me.SpecialRemarksDataGrid.SubKey = ""
        Me.SpecialRemarksDataGrid.SuppressTriangle = False
        Me.SpecialRemarksDataGrid.TabIndex = 0
        '
        'ExitButton
        '
        Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ExitButton.Location = New System.Drawing.Point(385, 273)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(75, 23)
        Me.ExitButton.TabIndex = 1
        Me.ExitButton.Text = "Exit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'SpecialRemarksViewerForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.ExitButton
        Me.ClientSize = New System.Drawing.Size(472, 302)
        Me.Controls.Add(Me.ExitButton)
        Me.Controls.Add(Me.SpecialRemarksDataGrid)
        Me.Name = "SpecialRemarksViewerForm"
        Me.Text = "Special Account Remarks"
        CType(Me.SpecialRemarksDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Custom Procedure"

    Private Sub LoadSpecialRemarks()
        Try

            SpecialRemarksDataGrid.DataSource = _SpecialRemarksDT
            SpecialRemarksDataGrid.SetTableStyle()
            SpecialRemarksDataGrid.Sort = If(SpecialRemarksDataGrid.LastSortedBy, SpecialRemarksDataGrid.DefaultSort)

            SpecialRemarksDataGrid.ResumeLayout()

        Catch ex As Exception

                Throw
        Finally
            Cursor.Current = Cursors.Default
        End Try
    End Sub

#End Region

#Region "Form Events"
    Private Sub SpecialRemarksViewerForm_load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetSettings()

        LoadSpecialRemarks()
    End Sub

    Private Sub SetSettings()
        Me.Top = If(CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(Me.AppKey, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)
    End Sub

    Private Sub SaveSettings()
        Dim lWindowState As FormWindowState = Me.WindowState
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "WindowState", CInt(lWindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(Me.AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = lWindowState

    End Sub

    Private Sub SpecialRemarksViewerForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        Try
            SaveSettings()
            SpecialRemarksDataGrid.TableStyles.Clear()
            SpecialRemarksDataGrid.DataSource = Nothing

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Private Sub ExitButton_Click(sender As System.Object, e As System.EventArgs) Handles ExitButton.Click
        Me.Close()
    End Sub

    Private Sub EligAcctHrsMaint_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        SaveSettings()
        Me.Dispose()
    End Sub

#End Region

End Class
