
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Collections.Generic
Imports UFCW.WCF
Public Class ClaimHistory

    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"
    Private _DubClick As Boolean = False
    Private _ClaimHistoryDT As DataTable = CMSDALFDBMD.LoadClaimsHistoryXML()
    Private _ClaimHistoryBS As New BindingSource

    Public Event OpenClaim(ByVal sender As Object, ByVal claimID As Integer)

    <System.ComponentModel.Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")> _
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = Value
        End Set
    End Property

    Private Sub History_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            SetSettings()
            ClaimsHistoryDataGrid.SuspendLayout()
            _ClaimHistoryBS.DataSource = _ClaimHistoryDT
            If _ClaimHistoryDT IsNot Nothing AndAlso _ClaimHistoryDT.Rows.Count > 0 Then
                _ClaimHistoryBS.Sort = "LAST_ACCESSED_DATE DESC"
            End If
            _ClaimHistoryBS.RaiseListChangedEvents = True
            ClaimsHistoryDataGrid.DataSource = _ClaimHistoryBS
            ClaimsHistoryDataGrid.ContextMenuPrepare(ClaimsHistoryDataGridCustomContextMenu)
            ClaimsHistoryDataGrid.ResumeLayout()

            ClaimsHistoryDataGrid.SetTableStyle()
            ClaimsHistoryDataGrid.Sort = If(ClaimsHistoryDataGrid.LastSortedBy, ClaimsHistoryDataGrid.DefaultSort(ClaimsHistoryDataGrid.Name))

            AddHandler CMSDALFDBMD.RecentClaimRefreshAvailable, Sub(arg As RecentClaimEventArgs)
                                                                    If ClaimsHistoryDataGrid.InvokeRequired Then
                                                                        ClaimsHistoryDataGrid.Invoke(Sub()
                                                                                                         _ClaimHistoryBS.DataSource = New DataView(arg.RecentClaimsDT, "", "LAST_ACCESSED_DATE DESC", DataViewRowState.CurrentRows)
                                                                                                     End Sub)

                                                                    Else
                                                                        _ClaimHistoryBS.DataSource = New DataView(arg.RecentClaimsDT, "", "LAST_ACCESSED_DATE DESC", DataViewRowState.CurrentRows)
                                                                    End If
                                                                End Sub
            _ClaimHistoryBS.EndEdit()
            _ClaimHistoryBS.ResetBindings(False)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Overloads Sub Dispose()

        If ClaimsHistoryDataGrid.DataSource IsNot Nothing Then
            ClaimsHistoryDataGrid.Dispose()
        End If

        ClaimsHistoryDataGrid = Nothing
        MyBase.Dispose()

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub ClaimsHistoryDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClaimsHistoryDataGrid.DoubleClick

        Select Case CType(sender, DataGridCustom).LastHitSpot.type
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

        Me.Top = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)

    End Sub

    Private Sub SaveSettings()
        Dim WindowState As Integer = Me.WindowState

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", CStr(Me.Top))
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", CStr(Me.Height))
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", CStr(Me.Left))
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", CStr(Me.Width))

    End Sub

    Private Sub HistoryDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ClaimsHistoryDataGrid.MouseUp
        Dim DG As DataGridCustom
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo
        Dim BS As BindingSource
        Dim DR As DataRow
        Try

            DG = CType(sender, DataGridCustom)
            HTI = DG.HitTest(e.X, e.Y)
            BS = CType(DG.DataSource, BindingSource)
            If BS.Current Is Nothing Then Return

            DR = CType(BS.Current, DataRowView).Row

            If DR Is Nothing Then Return

            If ClaimsHistoryDataGrid.GetGridRowCount > 0 Then
                Select Case HTI.Type
                    Case Is = System.Windows.Forms.DataGrid.HitTestType.None

                    Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
                        BS.Position = HTI.Row
                    Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                        BS.Position = HTI.Row
                        If _DubClick Then
                            Using WC As New GlobalCursor
                                RaiseEvent OpenClaim(Me, CInt(DR("CLAIM_ID")))
                            End Using
                        End If
                    Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

                    Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

                    Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption
                        BS.Position = 0
                    Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

                End Select
            End If
        Catch ex As Exception
            Throw
        Finally
            _DubClick = False
        End Try
    End Sub

    Private Sub DataGridCustomContextMenu_Popup(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClaimsHistoryDataGridCustomContextMenu.Opening
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' hides the display image option if nothing is able to display
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	3/24/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try
            If ClaimsHistoryDataGrid.GetGridRowCount > 0 Then
                DisplayMenuItem.Enabled = True
            Else
                DisplayMenuItem.Enabled = False
            End If

        Catch ex As Exception
            Throw

        End Try
    End Sub

    Private Sub DisplayMenuItem_Click(sender As Object, e As EventArgs) Handles DisplayMenuItem.Click

        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Calls to Display the FileNet Image in the IDM Viewer
        ' </summary>
        ' <param name="sender"></param>
        ' <param name="e"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	2/27/2006	Created
        ' Lalitha Moparthi 11/29/2022
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Docs As New List(Of Long?)
        Dim DG As DataGridCustom
        Dim Doclist As ArrayList
        Try

            DG = CType(DirectCast(DirectCast(sender, System.Windows.Forms.ToolStripMenuItem).GetCurrentParent, ContextMenuStrip).SourceControl, DataGridCustom)

            Using WC As New GlobalCursor

                Doclist = DG.GetSelectedDataRows()

                For Each DR As DataRow In Doclist
                    If IsDBNull(DR("DOCID")) = False Then
                        Docs.Add(CLng(DR("DOCID")))
                    End If
                Next

                If Docs.Count > 0 Then
                    Using FNDisplay As New Display
                        FNDisplay.Display(Docs)
                    End Using
                End If
            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub ExitButton_Click(sender As Object, e As EventArgs) Handles ExitButton.Click
        Me.Close()
    End Sub
End Class