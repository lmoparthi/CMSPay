
Public Class History

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"
    Private _FamilyID As Integer
    Private _RelationID As Short?
    Private _DubClick As Boolean = False

    Public Event OpenClaim(ByVal ClaimID As Integer)

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
    Public Property RelationID() As Short?
        Get
            Return _RelationID
        End Get
        Set(ByVal value As Short?)
            _RelationID = value
        End Set
    End Property

    Private Sub History_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            SetSettings()

            COBHistoryDataGrid.DataSource = CMSDALFDBMD.RetrieveHistory(_FamilyID, CShort(If(_RelationID Is Nothing, 0S, _RelationID)), "COB")
            COBHistoryDataGrid.SetTableStyle()
            COBHistoryDataGrid.Sort = If(COBHistoryDataGrid.LastSortedBy, COBHistoryDataGrid.DefaultSort)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then

            If COBHistoryDataGrid IsNot Nothing Then

                COBHistoryDataGrid.Dispose()
            End If

            COBHistoryDataGrid = Nothing

            If components IsNot Nothing Then
                components.Dispose()
            End If

        End If

        _Disposed = True
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        COBHistoryDataGrid.TableStyles.Clear()

    End Sub

    Private Sub COBHistoryDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles COBHistoryDataGrid.DoubleClick
        Select Case CType(sender, DataGridCustom).LastHitSpot.Type
            Case Is = System.Windows.Forms.DataGrid.HitTestType.None

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
                _DubClick = True
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                _DubClick = True
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

        End Select
    End Sub

    Private Sub History_FormClosing(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.FormClosing
        Try
            SaveSettings()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SetSettings()

        Me.Top = CInt(If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString))))
        Me.Height = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = CInt(If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString))))
        Me.Width = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)

    End Sub

    Private Sub SaveSettings()
        Dim WindowState As FormWindowState = Me.WindowState
        SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(WindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = WindowState
    End Sub

End Class