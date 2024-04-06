Imports System.ComponentModel

Public Class DentalControl

    Private _FamilyID As Integer = -1
    Private _RelationID As Short? = Nothing
    Private _SSN As Integer? = Nothing
    Private _ClaimID As Integer = -1
    Private _GridType As String
    Private _ProsSTATUS As String
    Private _Procedure As String
    Private _APPKEY As String = "UFCW\Dental\"

    Public Event BeforeRefresh(ByVal sender As Object, ByRef Cancel As Boolean)
    Public Event AfterRefresh(ByVal sender As Object)

    Private _ReadOnlyMode As Boolean = True

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
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the SSN of the Document.")>
    Public Property SSN() As Integer?
        Get
            Return _SSN
        End Get
        Set(ByVal value As Integer?)
            _SSN = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the RelationID of the Document.")>
    Public Property RelationID() As Short?
        Get
            Return _RelationID
        End Get
        Set(ByVal value As Short?)
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
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property DentalFrom() As Date
        Get
            Return DentalFromDate.Value
        End Get
        Set(ByVal value As Date)
            DentalFromDate.Value = value
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property DentalTo() As Date
        Get
            Return DentalToDate.Value.Date

        End Get
        Set(ByVal value As Date)
            DentalToDate.Value = value
        End Set
    End Property

#End Region

    Private _Disposed As Boolean ' To detect redundant calls

    Protected Overrides Sub Dispose(disposing As Boolean)

        If _Disposed Then Return

        ' Release any managed resources here.
        If disposing Then

            DentalDataGrid.TableStyles.Clear()
            DentalDataGrid.DataSource = Nothing
            DentalDataGrid.Dispose()

            DentalPREAuthDataGrid.TableStyles.Clear()
            DentalPREAuthDataGrid.DataSource = Nothing
            DentalPREAuthDataGrid.Dispose()

            DentalPENDDataGrid.TableStyles.Clear()
            DentalPENDDataGrid.DataSource = Nothing
            DentalPENDDataGrid.Dispose()

            If (components IsNot Nothing) Then
                components.Dispose()
            End If

        End If

        _Disposed = True
        ' Release any unmanaged resources not wrapped by safe handles here.

        ' Call the base class implementation.
        MyBase.Dispose(disposing)
    End Sub

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Dim DesignMode As Boolean = (LicenseManager.UsageMode = LicenseUsageMode.Designtime)

        If Not DesignMode Then
            DentalFromDate.MinDate = DateTime.Today.AddMonths(-36)
            DentalFromDate.MaxDate = DateTime.Today()
            DentalFromDate.Value = DateTime.Today.AddMonths(-36)
            DentalFromDate.Checked = False

            DentalToDate.MinDate = DateTime.Today.AddMonths(-36)
            DentalToDate.MaxDate = DateTime.Today()
            DentalToDate.Value = DateTime.Today()
            DentalToDate.Checked = False

        End If

    End Sub

    Public Sub New(ByVal familyID As Integer, ByVal relationID As Short?, ByVal dentalFrom As Date, ByVal dentalTo As Date)
        Me.New()


        _FamilyID = familyID
        _RelationID = relationID
        _SSN = Nothing

        DentalFromDate.Value = dentalFrom.Date
        DentalFromDate.Checked = True
        DentalToDate.Value = dentalTo.Date
        DentalToDate.Checked = True

        ''LoadDentalControl()
        ''LoadPREAuthDentalControl()
        ''LoadPENDDentalControl()
    End Sub
    Public Sub New(ByVal ssn As Integer, ByVal dentalFrom As Date, ByVal dentalTo As Date)
        Me.New()

        _FamilyID = Nothing
        _RelationID = Nothing
        _SSN = ssn

        DentalFromDate.Value = dentalFrom.Date
        DentalFromDate.Checked = True
        DentalToDate.Value = dentalTo.Date
        DentalToDate.Checked = True

        ''LoadDentalControl()
        ''LoadPREAuthDentalControl()
        ''LoadPENDDentalControl()
    End Sub

    Private Sub RePrintEOBToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RePrintEOBToolStripMenuItem.Click

        Dim DT As DataTable
        Dim ContextMenu As ContextMenuStrip
        Dim DG As DataGridCustom
        Dim DR As DataRow
        Dim BM As BindingManagerBase

        Try
            ContextMenu = CType(CType(sender, ToolStripMenuItem).Owner, ContextMenuStrip)

            DG = CType(ContextMenu.SourceControl, DataGridCustom)

            DT = _DentalDS.Tables("Dental")

            DG.DataSource = DT

            BM = DG.BindingContext(DG.DataSource, DG.DataMember)
            DR = DG.SelectedRowPreview 'used To accomodate context menu click Of row that Is yet To be selected

            If DG Is Nothing OrElse DG.DataSource Is Nothing Then Return
            If DR IsNot Nothing Then

                If IsDBNull(DR("FAMILY_ID")) Then Return

                If Not IsDBNull(DR("FAMILY_ID")) Then
                    _FamilyID = CInt(DR("FAMILY_ID"))
                End If

                If Not IsDBNull(DR("RELATION_ID")) Then
                    _RelationID = CShort(DR("RELATION_ID"))
                End If

                If Not IsDBNull(DR("CLAIM")) Then
                    _ClaimID = CInt(DR("CLAIM"))
                End If

                DentalDAL.UpdateDentalHistoryStatus(_FamilyID, _RelationID, _ClaimID, "REPRINT_EOB")
                LoadDentalControl()

            End If

        Catch ex As Exception

            Throw
        End Try

    End Sub

    Private Sub LineDetailsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LineDetailsToolStripMenuItem.Click

        Dim DT As DataTable
        Dim ContextMenu As ContextMenuStrip
        Dim DG As DataGridCustom
        Dim DR As DataRow
        Dim BM As BindingManagerBase

        Try
            ContextMenu = CType(CType(sender, ToolStripMenuItem).Owner, ContextMenuStrip)

            DG = CType(ContextMenu.SourceControl, DataGridCustom)

            DT = _DentalDS.Tables("Dental")

            DG.DataSource = DT

            BM = DG.BindingContext(DG.DataSource, DG.DataMember)
            DR = DG.SelectedRowPreview 'used To accomodate context menu click Of row that Is yet To be selected


            If DG Is Nothing OrElse DG.DataSource Is Nothing Then Exit Sub
            If DR IsNot Nothing Then
                If IsDBNull(DR("FAMILY_ID")) Then Return


                If Not IsDBNull(DR("FAMILY_ID")) Then
                    _FamilyID = CInt(DR("FAMILY_ID"))
                End If

                If Not IsDBNull(DR("RELATION_ID")) Then
                    _RelationID = CShort(DR("RELATION_ID"))
                End If

                If Not IsDBNull(DR("CLAIM")) Then
                    _ClaimID = CInt(DR("CLAIM"))
                End If

                Dim f As New DentalLineDetail(_FamilyID, _RelationID, _ClaimID)
                f.Show()

            End If
        Catch ex As Exception

            Throw
        End Try
    End Sub

#Region "Custom Subs\Functions"

    Public Sub LoadDentalControl(ByVal dentalFrom As Date, ByVal dentalTo As Date, ByVal ssn As Integer?, Optional ByRef dentalDataSet As DataSet = Nothing)
        Try
            _FamilyID = Nothing
            _RelationID = Nothing
            _SSN = ssn

            DentalFromDate.Value = dentalFrom.Date
            DentalFromDate.Checked = True
            DentalToDate.Value = dentalTo.Date
            DentalToDate.Checked = True

            LoadDentalControl(dentalDataSet)

        Catch ex As Exception

            Throw
        End Try
    End Sub
    Public Sub LoadDentalControl(ByVal dentalFrom As Date, ByVal dentalTo As Date, ByVal familyID As Integer, ByVal relationID As Short?, Optional ByRef dentalDataSet As DataSet = Nothing)
        Try
            _FamilyID = familyID
            _RelationID = relationID
            _SSN = Nothing

            DentalFromDate.Value = dentalFrom.Date
            DentalFromDate.Checked = True
            DentalToDate.Value = dentalTo.Date
            DentalToDate.Checked = True

            LoadDentalControl(dentalDataSet)

        Catch ex As Exception

            Throw
        End Try
    End Sub
    Public Sub LoadDentalControl(ByVal familyID As Integer, ByVal relationID As Short?, Optional ByRef dentalDataSet As DataSet = Nothing)
        Try
            _FamilyID = familyID
            _RelationID = relationID
            _SSN = Nothing

            LoadDentalControl(dentalDataSet)

        Catch ex As Exception

            Throw
        End Try
    End Sub
    Public Sub LoadDentalControl(Optional dentalDataSet As DataSet = Nothing)
        Try


            DentalDataGrid.DataSource = Nothing
            _DentalDS.Tables.Clear()

            If dentalDataSet IsNot Nothing Then
                _DentalDS = dentalDataSet
            Else
                '' _FamilyID = 1359723
                If _FamilyID <= 0 Then
                    _DentalDS = CType(DentalDAL.GetDentalInformation(DentalFromDate.Value.Date, DentalToDate.Value.Date, _SSN, _DentalDS), DataSet)
                Else
                    _DentalDS = CType(DentalDAL.GetDentalInformation(_FamilyID, _RelationID, _DentalDS), DataSet)
                End If

            End If

            DentalDataGrid.DataSource = _DentalDS.Tables(0)
            DentalDataGrid.SetTableStyle()
            DentalDataGrid.Sort = If(DentalDataGrid.LastSortedBy, DentalDataGrid.DefaultSort)
            DentalDataGrid.ContextMenuPrepare(DentalContextMenuStrip)

            If Not DentalFromDate.Checked AndAlso _DentalDS.Tables(0).Rows.Count > 0 Then
                DentalFromDate.MinDate = (From r In _DentalDS.Tables(0).AsEnumerable Select r.Field(Of Date)("FDOS")).Min  'remove because SPs has modified
                DentalFromDate.Value = DentalFromDate.MinDate
                DentalFromDate.Checked = False

                DentalToDate.MinDate = DentalFromDate.MinDate
                DentalToDate.Checked = False
            End If

        Catch ex As Exception

            Throw
        End Try
    End Sub
    Public Sub LoadPREAuthDentalControl(ByVal familyID As Integer, ByVal relationID As Short?, Optional ByRef dentalPREAuthDataSet As DataSet = Nothing)
        Try
            _FamilyID = familyID
            _RelationID = relationID
            _SSN = Nothing

            LoadPREAuthDentalControl(dentalPREAuthDataSet)

        Catch ex As Exception

            Throw
        End Try
    End Sub
    Public Sub LoadPREAuthDentalControl(Optional dentalPREAuthDataSet As DataSet = Nothing)
        Try

            DentalPREAuthDataGrid.DataSource = Nothing
            _DentalPREAuthDS.Tables.Clear()

            If dentalPREAuthDataSet IsNot Nothing Then
                _DentalPREAuthDS = dentalPREAuthDataSet
            Else

                If _FamilyID > 0 And (_RelationID Is Nothing Or _RelationID < 0) Then
                    _DentalPREAuthDS = CType(DentalDAL.GetDentalPendOrPreAuthInformation(_FamilyID, Nothing, "PREAUTH", _DentalPREAuthDS), DataSet)
                Else
                    _DentalPREAuthDS = CType(DentalDAL.GetDentalPendOrPreAuthInformation(_FamilyID, _RelationID, "PREAUTH", _DentalPREAuthDS), DataSet)
                End If

            End If

            DentalPREAuthDataGrid.DataSource = _DentalPREAuthDS.Tables(0)
            DentalPREAuthDataGrid.SetTableStyle()
            DentalPREAuthDataGrid.Sort = If(DentalPREAuthDataGrid.LastSortedBy, DentalPREAuthDataGrid.DefaultSort)
            DentalPREAuthDataGrid.ContextMenuPrepare(DentalPreAuthContextMenuStrip)

        Catch ex As Exception

            Throw
        End Try
    End Sub
    Public Sub LoadPENDDentalControl(ByVal familyID As Integer, ByVal relationID As Short?, Optional ByRef dentalPENDDataSet As DataSet = Nothing)
        Try
            _FamilyID = familyID
            _RelationID = relationID
            _SSN = Nothing

            LoadPENDDentalControl(dentalPENDDataSet)

        Catch ex As Exception

            Throw
        End Try
    End Sub
    Public Sub LoadPENDDentalControl(Optional dentalPENDDataSet As DataSet = Nothing)
        Try

            DentalPENDDataGrid.DataSource = Nothing
            _DentalPendDS.Tables.Clear()

            If dentalPENDDataSet IsNot Nothing Then
                _DentalPendDS = dentalPENDDataSet
            Else

                If _FamilyID > 0 And (_RelationID Is Nothing Or _RelationID < 0) Then
                    _DentalPendDS = CType(DentalDAL.GetDentalPendOrPreAuthInformation(_FamilyID, Nothing, "PEND", _DentalPendDS), DataSet)
                Else
                    _DentalPendDS = CType(DentalDAL.GetDentalPendOrPreAuthInformation(_FamilyID, _RelationID, "PEND", _DentalPendDS), DataSet)
                End If

            End If

            DentalPENDDataGrid.DataSource = _DentalPendDS.Tables(0)
            DentalPENDDataGrid.SetTableStyle()
            DentalPENDDataGrid.Sort = If(DentalPENDDataGrid.LastSortedBy, DentalPENDDataGrid.DefaultSort)

        Catch ex As Exception

            Throw
        End Try
    End Sub
    Public Sub ClearAll()

        If DentalDataGrid IsNot Nothing Then DentalDataGrid.DataSource = Nothing
        If DentalPREAuthDataGrid IsNot Nothing Then DentalPREAuthDataGrid.DataSource = Nothing
        If DentalPENDDataGrid IsNot Nothing Then DentalPENDDataGrid.DataSource = Nothing


        DentalFromDate.MinDate = DateTime.Today.AddMonths(-36)
        DentalFromDate.MaxDate = DateTime.Today()
        DentalFromDate.Value = DateTime.Today.AddMonths(-36)
        DentalFromDate.Checked = False

        DentalToDate.MinDate = DateTime.Today.AddMonths(-36)
        DentalToDate.MaxDate = DateTime.Today()
        DentalToDate.Value = DateTime.Today()
        DentalToDate.Checked = False

    End Sub
    Private Sub OpenLineDetails()

        Dim Frm As DentalLineDetail
        Dim BM As BindingManagerBase
        Dim DR As DataRow

        Try

            Using WC As New GlobalCursor

                BM = Me.DentalDataGrid.BindingContext(Me.DentalDataGrid.DataSource, Me.DentalDataGrid.DataMember)
                DR = CType(BM.Current, DataRowView).Row

                If DR IsNot Nothing Then
                    If IsDBNull(DR("FAMILY_ID")) Then Return

                    If Not IsDBNull(DR("FAMILY_ID")) Then
                        _FamilyID = CInt(DR("FAMILY_ID"))
                    End If

                    If Not IsDBNull(DR("RELATION_ID")) Then
                        _RelationID = CShort(DR("RELATION_ID"))
                    End If

                    If Not IsDBNull(DR("CLAIM")) Then
                        _ClaimID = CInt(DR("CLAIM"))
                    End If

                    ' MessageBox.Show("Line details " & DR("FAMILY_ID").ToString() & "-" & DR("RELATION_ID").ToString() & "-" & DR("CLAIM").ToString())
                    Frm = New DentalLineDetail(_FamilyID, _RelationID, _ClaimID)
                    Frm.Show()

                End If

            End Using


        Catch ex As Exception

            Throw
        Finally
        End Try

    End Sub
    Private Sub OpenPREAuthPENDLineDetails(ByVal gridtype As String)

        Dim Frm As DentalPREAuthPENDLineDetail
        Dim BM As BindingManagerBase
        Dim DR As DataRow

        Try

            Using WC As New GlobalCursor

                If gridtype.Contains("PREAUTH") Then
                    BM = Me.DentalPREAuthDataGrid.BindingContext(Me.DentalPREAuthDataGrid.DataSource, Me.DentalPREAuthDataGrid.DataMember)
                Else
                    BM = Me.DentalPENDDataGrid.BindingContext(Me.DentalPENDDataGrid.DataSource, Me.DentalPENDDataGrid.DataMember)
                End If

                DR = CType(BM.Current, DataRowView).Row

                If DR IsNot Nothing Then
                    If IsDBNull(DR("FAMILY_ID")) Then Return

                    If Not IsDBNull(DR("FAMILY_ID")) Then
                        _FamilyID = CInt(DR("FAMILY_ID"))
                    End If

                    If Not IsDBNull(DR("RELATION_ID")) Then
                        _RelationID = CShort(DR("RELATION_ID"))
                    End If

                    If Not IsDBNull(DR("CLAIM")) Then
                        _ClaimID = CInt(DR("CLAIM"))
                    End If

                    Frm = New DentalPREAuthPENDLineDetail(_FamilyID, _RelationID, _ClaimID, _GridType)
                    Frm.Show()

                End If

            End Using


        Catch ex As Exception

            Throw
        Finally
        End Try

    End Sub
    Private Sub RefreshButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshButton.Click

        Try

            DirectCast(DentalDataGrid.DataSource, DataTable).DefaultView.RowFilter = "FDOS >= #" & DentalFromDate.Value & "# AND LDOS <= #" & DentalToDate.Value & "#"

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub DentalContextMenuStrip_Opening(sender As Object, e As CancelEventArgs) Handles DentalContextMenuStrip.Opening

        Dim DGContextMenu As ContextMenuStrip

        Try

            DGContextMenu = CType(sender, ContextMenuStrip)
            DGContextMenu.Items("RePrintEOBToolStripMenuItem").Enabled = False

            If UFCWGeneralAD.CMSCanRePrintEOB Then
                DGContextMenu.Items("RePrintEOBToolStripMenuItem").Enabled = True
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub
    Private Sub DentalDataGrid_DoubleClick(sender As Object, e As EventArgs) Handles DentalDataGrid.DoubleClick

        Select Case CType(sender, DataGridCustom).LastHitSpot.Type

            Case Is = System.Windows.Forms.DataGrid.HitTestType.None

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                OpenLineDetails()
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows


        End Select

    End Sub
    Private Sub DentalPREAuthDataGrid_DoubleClick(sender As Object, e As EventArgs) Handles DentalPREAuthDataGrid.DoubleClick
        Select Case CType(sender, DataGridCustom).LastHitSpot.Type

            Case Is = System.Windows.Forms.DataGrid.HitTestType.None

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                _GridType = "PREAUTH"
                OpenPREAuthPENDLineDetails(_GridType)
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows


        End Select
    End Sub
    Private Sub DentalPENDDataGrid_DoubleClick(sender As Object, e As EventArgs) Handles DentalPENDDataGrid.DoubleClick
        Select Case CType(sender, DataGridCustom).LastHitSpot.Type

            Case Is = System.Windows.Forms.DataGrid.HitTestType.None

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Cell
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnHeader

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowHeader
                _GridType = "PEND"
                OpenPREAuthPENDLineDetails(_GridType)
            Case Is = System.Windows.Forms.DataGrid.HitTestType.ColumnResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.RowResize

            Case Is = System.Windows.Forms.DataGrid.HitTestType.Caption

            Case Is = System.Windows.Forms.DataGrid.HitTestType.ParentRows


        End Select
    End Sub

    Private Sub PreAuthLineDetailsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PreAuthLineDetailsToolStripMenuItem.Click

        Try
            _GridType = "PREAUTH"
            OpenPREAuthPENDLineDetails(_GridType)

        Catch ex As Exception

            Throw
        End Try

    End Sub
#End Region
End Class
