
Imports System.ComponentModel
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Collections.Generic

Public Class History

    Private _APPKEY As String = "UFCW\Claims\"
    Private DubClick As Boolean = False

    Public Event OpenClaim(ByVal ClaimID As Integer)

    <System.ComponentModel.Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")> _
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal Value As String)
            _APPKEY = Value
        End Set
    End Property

    Private Sub History_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        SetSettings()

        ClaimsHistoryDataGrid.SuspendLayout()
        ClaimsHistoryDataGrid.DataSource = CMSDALFDBMD.LoadClaimsHistoryXML
        ClaimsHistoryDataGrid.SetTableStyle()
        ClaimsHistoryDataGrid.ContextMenuPrepare(ClaimsHistoryDataGridCustomContextMenu)
        ClaimsHistoryDataGrid.ResumeLayout()

    End Sub

    Public Overloads Sub Dispose()

        If ClaimsHistoryDataGrid.DataSource IsNot Nothing Then
            If ClaimsHistoryDataGrid.GetCurrentDataTable IsNot Nothing Then
                ClaimsHistoryDataGrid.SaveColumnsSizeAndPosition(Me.AppKey, ClaimsHistoryDataGrid.Name & "\" & ClaimsHistoryDataGrid.GetCurrentDataTable.TableName & "\ColumnSettings")
                ClaimsHistoryDataGrid.SaveSortByColumnName(Me.AppKey, ClaimsHistoryDataGrid.Name & "\" & ClaimsHistoryDataGrid.GetCurrentDataTable.TableName & "\Sort", ClaimsHistoryDataGrid.GetGridSortColumn)
            End If
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

    Private Sub RefreshButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshButton.Click
        ClaimsHistoryDataGrid.RefreshData()
    End Sub

    Private Sub ClaimsHistoryDataGrid_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClaimsHistoryDataGrid.DoubleClick

        Select Case CType(sender, DataGridPlus.DataGridCustom).LastHitSpot
            Case Is = System.Windows.Forms.DataGrid.HitTestType.None

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
                DubClick = True

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                DubClick = True

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
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub SetSettings()

        Me.Top = If(CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)) < 0, 0, CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Top", Me.Top.ToString)))
        Me.Height = CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Height", Me.Height.ToString))
        Me.Left = If(CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)) < 0, 0, CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Left", Me.Left.ToString)))
        Me.Width = CInt(GetSetting(Me.AppKey, Me.Name & "\Settings", "Width", Me.Width.ToString))
        Me.WindowState = CType(GetSetting(Me.AppKey, Me.Name & "\Settings", "WindowState", CInt(Me.WindowState).ToString), FormWindowState)

    End Sub

    Private Sub SaveSettings()
        Dim lWindowState As Integer = Me.WindowState

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", CStr(Me.Top))
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", CStr(Me.Height))
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", CStr(Me.Left))
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", CStr(Me.Width))

        SaveColSettings()

    End Sub

    Private Sub SaveColSettings()

        If ClaimsHistoryDataGrid IsNot Nothing Then
            If ClaimsHistoryDataGrid.DataSource IsNot Nothing Then
                ClaimsHistoryDataGrid.SaveColumnsSizeAndPosition(Me.AppKey, ClaimsHistoryDataGrid.Name & "\" & ClaimsHistoryDataGrid.GetCurrentDataTable.TableName & "\ColumnSettings")
                ClaimsHistoryDataGrid.SaveSortByColumnName(Me.AppKey, ClaimsHistoryDataGrid.Name & "\" & ClaimsHistoryDataGrid.GetCurrentDataTable.TableName & "\Sort", ClaimsHistoryDataGrid.GetGridSortColumn)
            End If
        End If

    End Sub

    Private Sub HistoryDataGrid_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ClaimsHistoryDataGrid.MouseUp
        Dim MyGrid As DataGrid
        Dim HTI As System.Windows.Forms.DataGrid.HitTestInfo
        Dim DV As DataView

        Try

            MyGrid = CType(sender, DataGrid)
            HTI = MyGrid.HitTest(e.X, e.Y)

            If ClaimsHistoryDataGrid.GetGridRowCount > 0 Then
                Select Case HTI.Type
                    Case Is = System.Windows.Forms.DataGrid.HitTestType.None

                    Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
                        ClaimsHistoryDataGrid.CurrentRowIndex = HTI.Row
                    Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                        ClaimsHistoryDataGrid.CurrentRowIndex = HTI.Row

                        If DubClick Then
                            Using WC As New GlobalCursor

                                DV = ClaimsHistoryDataGrid.GetDefaultDataView

                                RaiseEvent OpenClaim(CInt(DV(ClaimsHistoryDataGrid.CurrentRowIndex)("CLAIM_ID")))

                            End Using

                        End If

                    Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

                    Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

                    Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption
                        ClaimsHistoryDataGrid.CurrentRowIndex = 0
                    Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows

                End Select
            End If
        Catch ex As Exception

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                Throw
            End If
        Finally
            DubClick = False
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
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
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
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DV As DataView
        Dim Cnt As Integer = 0
        Dim Docs As New List(Of Long?)
        Dim DG As DataGridPlus.DataGridCustom

        Try

            DG = CType(DirectCast(DirectCast(sender, System.Windows.Forms.ToolStripMenuItem).GetCurrentParent, ContextMenuStrip).SourceControl, DataGridPlus.DataGridCustom)


            If DG.GetGridRowCount > 0 Then

                Using WC As New GlobalCursor

                    DV = DG.GetDefaultDataView

                    For Cnt = DG.GetGridRowCount - 1 To 0 Step -1
                        If DG.IsSelected(Cnt) = True AndAlso IsDBNull(DV(Cnt)("DOCID")) = False Then
                            Docs.Add(CLng(DV(Cnt)("DOCID")))
                        End If
                    Next

                    If DG.CurrentRowIndex >= 0 AndAlso DG.IsSelected(DG.CurrentRowIndex) = False _
                                        AndAlso IsDBNull(DV(DG.CurrentRowIndex)("DOCID")) = False Then

                        Docs.Add(CLng(DV(DG.CurrentRowIndex)("DOCID")))
                    End If

                    If Docs.Count > 0 Then CMSDALFileNet.DisplayFNImage(Docs)

                End Using

            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (rethrow) Then
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally

        End Try

    End Sub
End Class