Option Strict On
Imports System.Windows.Forms
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling


Public Class CoverageAndNetworkHistoryForm
    Implements IDisposable

    Private _APPKEY As String = "UFCW\RegMaster\HMONetworkHistory\"
    Private _FamilyID As Integer
    Private _RelationID As Integer
    Private _HistoryDS As New DataSet
    Private _HistoryBS As New BindingSource
    Private _CachedDT As New DataTable
    Private _DubClick As Boolean = False
    Private _ModeFilter As String

    <System.ComponentModel.Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal Value As String)
            _APPKEY = Value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets or Sets the FamilyID to display History for.")>
    Public Property FamilyID() As Integer
        Get
            Return _FamilyID
        End Get
        Set(ByVal Value As Integer)
            _FamilyID = Value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets or Sets the RelationID to display History for.")>
    Public Property RelationID() As Integer
        Get
            Return _RelationID
        End Get
        Set(ByVal Value As Integer)
            _RelationID = Value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets or Sets the RelationID to display History for.")>
    Public Property Mode() As REGMasterHistoryMode
        Get
            Return _Mode
        End Get
        Set(ByVal Value As REGMasterHistoryMode)
            _Mode = Value
            Select Case _Mode
                Case REGMasterHistoryMode.All
                    _ModeFilter = "ELIG%"
                Case REGMasterHistoryMode.HMONetwork
                    _ModeFilter = "ELIGCOVHMO%"
                Case REGMasterHistoryMode.MedicalCoverage
                    _ModeFilter = "ELIGCOV%"
                Case REGMasterHistoryMode.DentalCoverage
                    _ModeFilter = "ELIGCOVDEN%"
            End Select
        End Set
    End Property

    Private Property _Mode As REGMasterHistoryMode

    'Form overrides dispose to clean up the component list.
    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then

            SaveSettings()

            If HMONetworkHistoryDataGrid IsNot Nothing Then
                HMONetworkHistoryDataGrid.Dispose()
            End If
            HMONetworkHistoryDataGrid = Nothing

            If _HistoryBS IsNot Nothing Then
                _HistoryBS.Dispose()
            End If
            _HistoryBS = Nothing

            If _HistoryDS IsNot Nothing Then
                _HistoryDS.Dispose()
            End If
            _HistoryDS = Nothing

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

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        HMONetworkHistoryDataGrid.TableStyles.Clear()

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

    Private Sub LoadHistory()
        Try
            RemoveHandler _HistoryBS.PositionChanged, AddressOf HistoryBSPositionChanged

            If _CachedDT Is Nothing Then
                _CachedDT = RegMasterDAL.RetrieveRegHistoryByFamily(_FamilyID)
            End If

            _HistoryDS.Clear()

            If _CachedDT IsNot Nothing Then
                _HistoryDS.Tables.Add(_CachedDT.Clone)
                Dim DRs As DataRow() = _CachedDT.Select("RELATION_ID=" & _RelationID & "AND TRANSACTION_TYPE LIKE '" & _ModeFilter & "'")

                For Each DR In DRs
                    _HistoryDS.Tables(0).ImportRow(DR)
                Next
            End If

            AddHandler _HistoryBS.PositionChanged, AddressOf HistoryBSPositionChanged

            _HistoryBS.DataMember = "REG_HISTORY"
            _HistoryBS.DataSource = _HistoryDS

            HMONetworkHistoryDataGrid.DataSource = _HistoryBS
            HMONetworkHistoryDataGrid.SetTableStyle()
            HMONetworkHistoryDataGrid.Sort = If(HMONetworkHistoryDataGrid.LastSortedBy, HMONetworkHistoryDataGrid.DefaultSort)

            If _HistoryDS.Tables(0).Rows.Count > 0 Then _HistoryBS.Position = 0

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Private Sub HistoryBSPositionChanged(sender As Object, e As EventArgs)

        Dim DR As DataRow
        DR = HMONetworkHistoryDataGrid.GetCMCurrentRow

        If DR IsNot Nothing AndAlso DR.Table.Columns.Contains("DETAIL") Then
            txtHistoryDetail.Text = DR("DETAIL").ToString
        End If

    End Sub

    Private Sub History_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If _FamilyID = -1 OrElse _RelationID = -1 Then Return
        Try
            SetSettings()

            _CachedDT = REGMasterHistoryViewerForm.RMHistory   '' retrieving already cached value from address tab

            LoadHistory()

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Private Sub HMONetworkHistoryDataGrid_RefreshGridData() Handles HMONetworkHistoryDataGrid.RefreshGridData

        If _FamilyID = -1 OrElse _RelationID = -1 Then Return

        Try

            LoadHistory()

        Catch ex As Exception

                Throw
        End Try
    End Sub

End Class


