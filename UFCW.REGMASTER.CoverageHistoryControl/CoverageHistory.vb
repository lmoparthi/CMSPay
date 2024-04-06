Option Strict On
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Configuration
Public Class CoverageHistory
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _FamilyID As Integer = -1
    Private _RelationID As Integer
    Private _APPKEY As String = "UFCW\RegMaster\"
    Private _Loading As Boolean = True
    Private _CoverageHistoryDS As DataSet
    Private _AppName As String = If(System.Configuration.ConfigurationManager.AppSettings("AppName"), "")
    Private _HMONetworkDS As DataSet
    Private _Zipcode As Object
    Private _SubscriberStatus As String
    Private _ReadOnlyMode As Boolean = True
    Private _Plantype As String = ""
    Private _Memtype As String = ""

    Public Event BeforeRefresh(ByVal sender As Object, ByVal cancel As Boolean)
    Public Event AfterRefresh(ByVal sender As Object)

    Private _Dualfamily As Boolean = False

    ReadOnly _REGMSupervisor As Boolean = UFCWGeneralAD.REGMSupervisor

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Windows Form Designer
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

    <Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)

            _APPKEY = value
        End Set
    End Property

    Public Property Status() As String
        Get
            Return _SubscriberStatus
        End Get
        Set(ByVal value As String)

            _SubscriberStatus = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Determines if control is in Read Only Mode.")>
    Public Property ReadOnlyMode() As Boolean
        Get
            Return _ReadOnlyMode
        End Get
        Set(ByVal Value As Boolean)
            _ReadOnlyMode = Value
        End Set
    End Property

    Public Property MedPlan() As String
        Get
            Return _Plantype
        End Get
        Set(ByVal Value As String)

            _Plantype = Value
        End Set
    End Property

    Public Property Memtype() As String
        Get
            Return _Memtype
        End Get
        Set(ByVal Value As String)

            _Memtype = Value
        End Set
    End Property

    Public Property DualFamily() As Boolean
        Get
            Return _Dualfamily
        End Get
        Set(ByVal value As Boolean)

            _Dualfamily = value
        End Set
    End Property

#End Region

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            If MedCoverageHistoryDataGrid IsNot Nothing Then
                MedCoverageHistoryDataGrid.Dispose()
            End If
            MedCoverageHistoryDataGrid = Nothing

            If DenCoverageHistoryDataGrid IsNot Nothing Then
                DenCoverageHistoryDataGrid.Dispose()
            End If
            DenCoverageHistoryDataGrid = Nothing

            If _CoverageHistoryDS IsNot Nothing Then
                _CoverageHistoryDS.Dispose()
            End If
            _CoverageHistoryDS = Nothing

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

#Region "Constructor"

    Public Sub New(ByVal familyID As Integer, ByVal relationID As Integer)
        Me.New()

        _FamilyID = familyID
        _RelationID = relationID
    End Sub

#End Region

#Region "Form\Button Events"

    Private Sub CoverageHistoryControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        _Loading = False
    End Sub

    Private Sub NetworkContextMenu_Opening(sender As Object, e As CancelEventArgs) Handles NetworkContextMenu.Opening

        Dim DGContextMenu As ContextMenuStrip
        Dim DG As DataGridCustom
        Dim DR As DataRow

        Try
            'Debug.Print("NetworkContextMenu_Opening (In):")

            DGContextMenu = CType(sender, ContextMenuStrip)
            DGContextMenu.Items("HMONetworkInfoToolStripMenuItem").Available = False
            DGContextMenu.Items("CoverageMaintenanceToolStripMenuItem").Available = False

            If UFCWGeneralAD.REGMEligMaintenanceAccess Then

                If Not Me.DualFamily Then   '' don't display menu items in the bottom grid when it is dual family
                    DGContextMenu.Items("CoverageMaintenanceToolStripMenuItem").Available = True
                End If

                DG = CType(DirectCast(sender, System.Windows.Forms.ContextMenuStrip).SourceControl, DataGridCustom)

                If DG IsNot Nothing Then

                    DR = DG.SelectedRowPreview

                    If DR IsNot Nothing Then

                        If Not IsDBNull(DR("COVERAGE_CODE")) Then

                            Select Case DR("COVERAGE_CODE").ToString
                                Case "40", "41", "80", "81"
                                    DGContextMenu.Items("HMONetworkInfoToolStripMenuItem").Available = True
                            End Select

                        End If


                    End If

                End If

            End If

            'Debug.Print("NetworkContextMenu_Opening (Out): " & DG.LastHitSpot.Row.ToString)

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub HMONetworkInfoToolStripMenuItem_Click(sender As Object, e As System.EventArgs) Handles HMONetworkInfoToolStripMenuItem.Click

        Dim ContextMenu As ContextMenuStrip = CType(CType(sender, ToolStripMenuItem).Owner, ContextMenuStrip)
        Dim DG As DataGridCustom = CType(ContextMenu.SourceControl, DataGridCustom)

        If DG.DataSource Is Nothing Then Exit Sub

        Dim BM As BindingManagerBase = DG.BindingContext(DG.DataSource, DG.DataMember)
        Dim DR As DataRow = CType(BM.Current, DataRowView).Row

        Dim HMONetworkDS As New DataSet
        Dim HMONetworkForm As HMONetworkInfoForm

        Try
            HMONetworkDS.Tables.Add(_CoverageHistoryDS.Tables("HMO_NETWORK").Clone)

            For Each HMONetworkDR As DataRow In _CoverageHistoryDS.Tables("HMO_NETWORK").Rows
                HMONetworkDS.Tables("HMO_NETWORK").ImportRow(HMONetworkDR)
            Next

            If _HMONetworkDS Is Nothing Then
                HMONetworkForm = New HMONetworkInfoForm(_FamilyID, CInt(DR("COVERAGE_CODE")), HMONetworkDS, True, _Zipcode, Me.Status)  '' First time when open form
            Else
                HMONetworkForm = New HMONetworkInfoForm(_FamilyID, CInt(DR("COVERAGE_CODE")), _HMONetworkDS, True, _Zipcode, Me.Status)     '' when added new network it should show the newly added network
            End If


            HMONetworkForm.ShowDialog()
            _HMONetworkDS = HMONetworkForm.HMONetworkDataSet

            HMONetworkForm.Close()

        Catch ex As Exception


            Throw
        Finally
            If HMONetworkForm IsNot Nothing Then HMONetworkForm.Dispose()
            HMONetworkForm = Nothing

            If HMONetworkDS IsNot Nothing Then HMONetworkDS.Dispose()
            HMONetworkDS = Nothing

        End Try
    End Sub

    Private Sub CoverageMaintenanceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CoverageMaintenanceToolStripMenuItem.Click

        Dim CoverageElection As CoverageElectionForm
        Dim Result As DialogResult

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            CoverageElection = New CoverageElectionForm(_FamilyID, _ReadOnlyMode, _Zipcode, Me.Status)

            CoverageElection.MemType = _Memtype

            Result = CoverageElection.ShowDialog()

            If Result = DialogResult.Yes Then   '' To display the coverage changes in coverage history tab
                LoadCoverageHistory()
            End If

        Catch ex As Exception
            Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If CoverageElection IsNot Nothing Then
                CoverageElection.Dispose()

            End If
            CoverageElection = Nothing
        End Try

    End Sub

    Private Sub CoverageHistoryDataGrid_RefreshGridData() Handles MedCoverageHistoryDataGrid.RefreshGridData, DenCoverageHistoryDataGrid.RefreshGridData
        Dim Cancel As Boolean = False

        Try

            RaiseEvent BeforeRefresh(Me, Cancel)

            If Cancel = False Then
                LoadCoverageHistory()
            End If

            RaiseEvent AfterRefresh(Me)

        Catch ex As Exception

                Throw
        End Try
    End Sub
#End Region

#Region "Custom Subs\Functions"

    Public Sub LoadCoverageHistory(ByVal familyID As Integer, Optional coverageHistoryDS As DataSet = Nothing)
        Try
            _FamilyID = familyID

            If coverageHistoryDS IsNot Nothing Then _CoverageHistoryDS = coverageHistoryDS

            LoadCoverageHistory()

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Public Sub LoadCoverageHistory(ByVal familyID As Integer, ByVal zip As Object)
        Try

            _FamilyID = familyID
            _Zipcode = zip

            LoadCoverageHistory()

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Public Sub LoadCoverageHistory()
        Dim DRs As DataRow()
        Dim MedDT As DataTable
        Dim DenDT As DataTable

        Try

            ClearAll()

            If _CoverageHistoryDS Is Nothing OrElse _CoverageHistoryDS.Tables Is Nothing OrElse _CoverageHistoryDS.Tables.Count < 1 Then
                _CoverageHistoryDS = If(_REGMSupervisor, RegMasterDAL.RetrieveCoverageHistoryAndNetworksNoOverlap(_FamilyID, _CoverageHistoryDS), RegMasterDAL.RetrieveCoverageHistoryAndNetworks(_FamilyID, _CoverageHistoryDS))
            End If

            MedDT = _CoverageHistoryDS.Tables("COVERAGE_HISTORY").Clone
            MedDT.TableName = "MEDICAL_COVERAGE_HISTORY"
            DenDT = _CoverageHistoryDS.Tables("COVERAGE_HISTORY").Clone
            DenDT.TableName = "DENTAL_COVERAGE_HISTORY"

            MedCoverageHistoryDataGrid.CaptionText = "Medical Coverage History for Family (" & _FamilyID.ToString & ")"
            DRs = _CoverageHistoryDS.Tables("COVERAGE_HISTORY").Select("COVERAGE_TYPE='MEDICAL'")

            For Each TypeDR As DataRow In DRs
                MedDT.ImportRow(TypeDR)
            Next

            MedCoverageHistoryDataGrid.AppKey = _APPKEY
            MedCoverageHistoryDataGrid.DataSource = MedDT
            MedCoverageHistoryDataGrid.SetTableStyle()
            MedCoverageHistoryDataGrid.Sort = If(MedCoverageHistoryDataGrid.LastSortedBy, MedCoverageHistoryDataGrid.DefaultSort)

            MedCoverageHistoryDataGrid.ContextMenuPrepare(NetworkContextMenu)

            DenCoverageHistoryDataGrid.CaptionText = "Dental Coverage History for Family (" & _FamilyID.ToString & ")"
            DRs = _CoverageHistoryDS.Tables("COVERAGE_HISTORY").Select("COVERAGE_TYPE='DENTAL'")

            For Each DR As DataRow In DRs
                DenDT.ImportRow(DR)
            Next

            DenCoverageHistoryDataGrid.AppKey = _APPKEY
            DenCoverageHistoryDataGrid.DataSource = DenDT
            DenCoverageHistoryDataGrid.SetTableStyle()
            DenCoverageHistoryDataGrid.Sort = If(DenCoverageHistoryDataGrid.LastSortedBy, DenCoverageHistoryDataGrid.DefaultSort)

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Public Sub ClearAll()

        MedCoverageHistoryDataGrid.DataSource = Nothing
        MedCoverageHistoryDataGrid.CaptionText = ""

        DenCoverageHistoryDataGrid.DataSource = Nothing
        DenCoverageHistoryDataGrid.CaptionText = ""

        _HMONetworkDS = Nothing

        _CoverageHistoryDS = Nothing

    End Sub

    Private Sub CoverageHistory_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged

    End Sub

#End Region

End Class
