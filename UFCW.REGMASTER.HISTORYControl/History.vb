Option Strict On

Imports System.Windows.Forms
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class RegMasterHistoryForm

    Private _APPKEY As String = "UFCW\RegMaster\History\"
    Private _FamilyID As Integer
    Private _RelationID As Integer
    Private _HistoryDS As New DataSet
    Private _HistoryBS As New BindingSource
    Private _CachedDT As New DataTable
    Private _DubClick As Boolean = False
    Private _ModeFilter As String

    <System.ComponentModel.Browsable(False), System.ComponentModel.Description("Gets or Sets the RelationID to display History for.")>
    Public Property Mode() As REGMasterHistoryMode
        Get
            Return _Mode
        End Get
        Set(ByVal Value As REGMasterHistoryMode)
            _Mode = Value
        End Set
    End Property

    Private Property _Mode As REGMasterHistoryMode = REGMasterHistoryMode.All

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

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then

            If _HistoryBS IsNot Nothing Then
                _HistoryBS.DataMember = Nothing
                _HistoryBS.DataSource = Nothing
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

    Private Sub History_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If _FamilyID = -1 Then Return

        Try
            SetSettings()

            _CachedDT = REGMasterHistoryViewerForm.RMHistory   '' retrieving already cached value from address tab

            Me.Text = _Mode.ToString & " History (Family: " & _FamilyID.ToString & If(_RelationID > -1, " Relation: " & _RelationID.ToString, "") & ")"

            HistoryViewer1.LoadHistory(_FamilyID, _RelationID, _Mode, _CachedDT)

        Catch ex As Exception
            Throw
        End Try
    End Sub



End Class


