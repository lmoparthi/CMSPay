Option Strict On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class AlertHistory

    Private _APPKEY As String = "UFCW\RegMaster\AlertControl\"
    Private _FamilyID As Integer
    Private _RelationID As Integer
    Private _HistoryDT As New DataTable
    Private _CachedDT As New DataTable
    Private _DubClick As Boolean = False

    Public CallingAppname As String = System.Configuration.ConfigurationManager.AppSettings("AppName")

    <System.ComponentModel.Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets or Sets the FamilyID to display History for.")>
    Public Property FamilyID() As Integer
        Get
            Return _FamilyID
        End Get
        Set(ByVal value As Integer)
            _FamilyID = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets or Sets the RelationID to display History for.")>
    Public Property RelationID() As Integer
        Get
            Return _RelationID
        End Get
        Set(ByVal value As Integer)
            _RelationID = value
        End Set
    End Property

    Private _Disposed As Boolean = False
    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.

            If _HistoryDT IsNot Nothing Then
                _HistoryDT.Dispose()
            End If
            _HistoryDT = Nothing

            If _CachedDT IsNot Nothing Then
                _CachedDT.Dispose()
            End If
            _CachedDT = Nothing

            If components IsNot Nothing Then
                components.Dispose()
            End If

        End If

        ' Free any unmanaged objects here.
        '
        _Disposed = True

        ' Call base class implementation.
        MyBase.Dispose(disposing)
    End Sub

    Private Sub LoadHistory()
        Dim DRs As DataRow()

        Try

            If _CachedDT IsNot Nothing Then
                _HistoryDT = _CachedDT.Clone
                DRs = _CachedDT.Select("RELATION_ID=" & _RelationID & "AND TRANSACTION_TYPE LIKE '%ALERT%'")
                For Each DR As DataRow In DRs
                    _HistoryDT.ImportRow(DR)
                Next
            End If

            AlertHistoryDataGrid.DataSource = _HistoryDT
            AlertHistoryDataGrid.SetTableStyle()
            AlertHistoryDataGrid.Sort = If(AlertHistoryDataGrid.LastSortedBy, AlertHistoryDataGrid.DefaultSort)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadHistory(ByVal familyID As Integer)
        Try
            _FamilyID = familyID
            _HistoryDT = _CachedDT.Clone

            Dim DRs As DataRow() = _CachedDT.Select(" TRANSACTION_TYPE LIKE '%ALERT%'")
            For Each DR As DataRow In DRs
                _HistoryDT.ImportRow(DR)
            Next

            AlertHistoryDataGrid.DataSource = _HistoryDT
            AlertHistoryDataGrid.SetTableStyle()
            AlertHistoryDataGrid.Sort = If(AlertHistoryDataGrid.LastSortedBy, AlertHistoryDataGrid.DefaultSort)

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Private Sub History_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            SetSettings()
            If Not IsNothing(CallingAppname) AndAlso CallingAppname.Length > 0 AndAlso CallingAppname.ToUpper = "ELIGIBILITY" Then
                _CachedDT = REGMasterHistoryViewerForm.RMHistory   '' retrieving already cached value from  tab
                LoadHistory()
            Else
                _CachedDT = RegMasterDAL.RetrieveRegHistoryByFamily(_FamilyID)
                LoadHistory(_FamilyID)
            End If
        Catch ex As Exception

            Throw
        End Try
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        AlertHistoryDataGrid.TableStyles.Clear()

    End Sub

    Private Sub SetSettings()

        Me.Top = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString))
        Me.Height = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString))
        Me.Width = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString))

    End Sub

    Private Sub SaveSettings()
        Dim lWindowState As Integer = Me.WindowState

        Me.WindowState = 0
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
    End Sub

    Private Sub AlertHistoryDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles AlertHistoryDataGrid.DoubleClick
        _DubClick = True
    End Sub

    Public Overloads Sub Dispose()
        If AlertHistoryDataGrid.DataSource IsNot Nothing AndAlso AlertHistoryDataGrid.DataSource IsNot Nothing Then
            SaveSettings()
            AlertHistoryDataGrid.Dispose()
        End If

        AlertHistoryDataGrid = Nothing

        If _HistoryDT IsNot Nothing Then _HistoryDT = Nothing
        If _CachedDT IsNot Nothing Then _CachedDT = Nothing
        MyBase.Dispose()
    End Sub

    Private Sub AlertHistoryDataGrid_RefreshGridData() Handles AlertHistoryDataGrid.RefreshGridData
        _HistoryDT.Rows.Clear()
        _CachedDT = RegMasterDAL.RetrieveRegHistoryByFamily(_FamilyID)
        LoadHistory()
    End Sub

    Private Sub HistoryDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles AlertHistoryDataGrid.MouseUp
        Dim MyGrid As DataGrid = CType(sender, DataGrid)
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo
        HTI = MyGrid.HitTest(e.X, e.Y)

        Try
            Select Case HTI.Type
                Case Is = System.Windows.Forms.DataGrid.HitTestType.None

                Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
                    AlertHistoryDataGrid.CurrentRowIndex = HTI.Row
                Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

                Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                    AlertHistoryDataGrid.CurrentRowIndex = HTI.Row
                    Dim DV As DataView = AlertHistoryDataGrid.GetDefaultDataView
                    If UFCWGeneralAD.CMSLocals Then
                        _DubClick = False
                    End If

                    If e.Button = MouseButtons.Left Then
                        txtHistory.Text = CType(DV(AlertHistoryDataGrid.CurrentRowIndex)("DETAIL"), String)
                    End If

                    If e.Button = MouseButtons.Left And _DubClick = True Then

                        Dim DetailForm As New RegMasterDetailViewerForm(CInt(DV(AlertHistoryDataGrid.CurrentRowIndex)("FAMILY_ID")), CStr(DV(AlertHistoryDataGrid.CurrentRowIndex)("SUMMARY")), CStr(DV(AlertHistoryDataGrid.CurrentRowIndex)("DETAIL")), CStr(DV(AlertHistoryDataGrid.CurrentRowIndex)("CREATE_USERID")), CDate(DV(AlertHistoryDataGrid.CurrentRowIndex)("CREATE_DATE")))

                        DetailForm.ShowDialog(Me)
                        DetailForm.Dispose()
                        DetailForm = Nothing

                    End If
                Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

                Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

                Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption
                    AlertHistoryDataGrid.CurrentRowIndex = 0
                Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

            End Select

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally
            _DubClick = False
        End Try
    End Sub

End Class