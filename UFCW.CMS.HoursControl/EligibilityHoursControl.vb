Option Infer On
Imports System.ComponentModel
Imports System.Text

Public Class EligibilityHoursControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private _FamilyID As Integer = -1
    Private _ReadOnlyMode As Boolean = True
    Public ReadOnly Property [ReadOnly] As Boolean = True

    Private _APPKEY As String = "UFCW\Claims\"
    Private _Loading As Boolean = True

    Public Event BeforeRefresh(ByVal sender As Object, ByRef Cancel As Boolean)
    Public Event AfterRefresh(ByVal sender As Object)

    Private _HoursDS As DataSet
    Private WithEvents _HoursBS As BindingSource

    Private _ResultTypeDT As DataTable
    Private _TermColor As New SolidBrush(Color.Salmon) '' Red
    Private _EligibilityCalculate As Boolean = False

    Public Event LoadRegEligMsg()

    Private _Memtype As String = ""    '' For the maintenance screen
    Private _Dualfamily As Boolean = False

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            If EligibilityHoursDataGrid IsNot Nothing Then
                EligibilityHoursDataGrid.Dispose()
            End If
            EligibilityHoursDataGrid = Nothing

            If _HoursDS IsNot Nothing Then
                _HoursDS.Dispose()
            End If
            _HoursDS = Nothing

            If _ResultTypeDT IsNot Nothing Then
                _ResultTypeDT.Dispose()
            End If
            _ResultTypeDT = Nothing

            If _TermColor IsNot Nothing Then
                _TermColor.Dispose()
            End If
            _TermColor = Nothing

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

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
    End Sub

#Region "Constructor"

    Public Sub New(ByVal FamilyID As Integer)
        Me.New()

        _FamilyID = FamilyID

    End Sub
#End Region

#Region "Properties"
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")>
    Public Property FamilyID() As Integer
        Get
            Return _FamilyID
        End Get
        Set(ByVal Value As Integer)
            _FamilyID = Value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets the Hours Table")>
    Public ReadOnly Property HoursDataTable() As DataTable
        Get
            Return _HoursDS.Tables(0)
        End Get
    End Property

    <Browsable(True), System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal Value As String)
            _APPKEY = Value
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

    <System.ComponentModel.Browsable(True), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Determines if Eligibility calculated or not")>
    Public Property EligibiltyCalculate() As Boolean
        Get
            Return _EligibilityCalculate
        End Get
        Set(ByVal Value As Boolean)
            _EligibilityCalculate = Value
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

#Region "Custom Subs\Functions"

    Public Sub LoadHours(ByVal familyID As Integer, Optional hoursDS As DataSet = Nothing)
        Try
            _FamilyID = familyID

            If hoursDS IsNot Nothing Then _HoursDS = hoursDS

            LoadHours()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadHours()
        Try

            EligibilityHoursDataGrid.DataSource = Nothing

            If _HoursDS Is Nothing OrElse _HoursDS.Tables Is Nothing OrElse _HoursDS.Tables.Count < 1 Then
                _HoursDS = CMSDALFDBEL.GetEligibilityHours(_FamilyID)
            End If

            '' For displaying gaps
            If _HoursDS.Tables.Count > 0 AndAlso _HoursDS.Tables(0).Rows.Count > 0 Then
                Dim DT As DataTable
                Dim FromDate As Date? = (From value In _HoursDS.Tables(0).AsEnumerable
                                         Select value.Field(Of Date?)("ELIGIBILITY_MONTH")).Min

                If FromDate IsNot Nothing AndAlso Not _HoursDS.Tables.Contains("HOURS_GAP_MONTH_YEAR") Then
                    DT = CMSDALFDBMD.RetrieveMonthsGapInfoinEligHours(_FamilyID, CDate(FromDate), (UFCWGeneral.NowDate.Date.AddMonths(-1)).AddDays(1), Nothing)
                Else
                    DT = _HoursDS.Tables("HOURS_GAP_MONTH_YEAR")
                End If

                Dim GapPeriods =
                                 From period In (DT).AsEnumerable
                                 Where period.IsNull("ELIG_PERIOD") Select period.Field(Of Date)("DIA")

                For Each Period In GapPeriods
                    _HoursDS.Tables(0).Rows.Add(New Object() {Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, "No Hours", Nothing, Period, Nothing, Nothing})
                Next
            End If

            _HoursBS = New BindingSource
            _HoursBS.DataSource = _HoursDS.Tables(0)
            _HoursBS.Sort = If(EligibilityHoursDataGrid.LastSortedBy, EligibilityHoursDataGrid.DefaultSort)

            EligibilityHoursDataGrid.DataSource = _HoursBS

            SetTableStyle(EligibilityHoursDataGrid, EligAcctHrsContextMenuStrip)
            EligibilityHoursDataGrid.CaptionText = "Eligibility Hours for Family (" & _FamilyID.ToString & ") "

            LoadEligSpecialValues()

            ppCheckBox.Enabled = True
            EPCheckBox.Enabled = True
            cmbSpecial.Enabled = True

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub ClearAll()

        EligibilityHoursDataGrid.DataSource = Nothing
        EligibilityHoursDataGrid.CaptionText = ""

        ppCheckBox.Checked = False
        EPCheckBox.Checked = False
        ppCheckBox.Enabled = False
        EPCheckBox.Enabled = False
        cmbSpecial.Enabled = False

        cmbSpecial.DataSource = Nothing

        If cmbSpecial.DataSource Is Nothing Then cmbSpecial.Items.Clear()

        _EligibilityCalculate = False
        _ReadOnlyMode = True

        If _HoursDS IsNot Nothing AndAlso _HoursDS.Tables.Count > 0 Then _HoursDS.Tables.Clear()

        _Memtype = ""

        _Dualfamily = False

    End Sub

    Private Sub HoursControl_dispose(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed

        If EligibilityHoursDataGrid IsNot Nothing Then
            EligibilityHoursDataGrid.Dispose()
        End If
        EligibilityHoursDataGrid = Nothing

        Me.Dispose()
    End Sub

    Private Sub LoadEligSpecialValues()
        If _HoursDS Is Nothing OrElse _HoursDS.Tables.Count = 0 Then Return
        Try

            _ResultTypeDT = CMSDALCommon.SelectDistinctAndSorted("TYPE", _HoursDS.Tables(0), "TYPE", True)
            cmbSpecial.DataSource = _ResultTypeDT
            cmbSpecial.DisplayMember = "TYPE"

        Catch ex As Exception
            Throw

        Finally

        End Try
    End Sub

    Private Sub EligibilityHoursDataGrid_MouseUp(sender As Object, e As MouseEventArgs) Handles EligibilityHoursDataGrid.MouseUp
        Dim RegTotal As Decimal = 0
        Dim StrSum As String = ""
        Dim DRs As ArrayList

        Try

            DRs = EligibilityHoursDataGrid.GetSelectedDataRows

            If DRs Is Nothing Then Return

            For Each DR As DataRow In DRs
                If Not IsDBNull(DR("HOURS")) Then
                    RegTotal += CDec(DR("HOURS"))
                End If
            Next

            StrSum = "       Hours Worked =  " & CStr(RegTotal)

            EligibilityHoursDataGrid.CaptionText = ""
            EligibilityHoursDataGrid.CaptionText = " Hours for Family (" & _FamilyID.ToString & ") " & StrSum

        Catch ex As Exception

            Throw

        End Try
    End Sub


    Private Sub SetTableStyle(ByVal dg As DataGridCustom, ByVal dataGridCustomContextMenu As System.Windows.Forms.ContextMenuStrip)

        Try

            SetTableStyleColumns(dg)
            dg.StyleName = dg.Name
            dg.AppKey = _APPKEY

            dg.ContextMenuPrepare(dataGridCustomContextMenu)

            RemoveHandler dg.ResetTableStyle, AddressOf TableStyleReset
            AddHandler dg.ResetTableStyle, AddressOf TableStyleReset

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub TableStyleReset(ByVal sender As Object, e As EventArgs)
        Dim dg As DataGridCustom = CType(sender, DataGridCustom)
        SetTableStyleColumns(dg)
    End Sub

    Private Sub SetTableStyleColumns(ByVal dg As DataGridCustom)

        Dim DGTS As DataGridTableStyle
        Dim DGTSDefault As DataGridTableStyle

        Dim TextCol As DataGridFormattableTextBoxColumn
        Dim BoolCol As DataGridColorBoolColumn
        Dim intCol As Integer
        Dim ColsDV As DataView
        Dim DefaultStyleDS As DataSet
        Dim ResultDT As DataTable
        Dim XMLStyleName As String
        Dim ColumnSequenceDV As DataView

        Try

            XMLStyleName = dg.Name

            DefaultStyleDS = DataGridCustom.GetTableStyle(XMLStyleName)

            If DefaultStyleDS Is Nothing OrElse DefaultStyleDS.Tables.Count < 1 Then Return

            DGTS = New DataGridTableStyle()

            If dg.GetCurrentDataTable Is Nothing Then
                DGTS.MappingName = dg.Name
            Else
                DGTS.MappingName = dg.GetCurrentDataTable.TableName
            End If

            DGTS.GridColumnStyles.Clear()

            If DefaultStyleDS.Tables.Contains(XMLStyleName & "Style") Then
                If DefaultStyleDS.Tables(XMLStyleName & "Style").Columns.Contains("GridLineStyle") Then
                    DGTS.GridLineStyle = If(CBool(DefaultStyleDS.Tables(XMLStyleName & "Style").Rows(0)("GridLineStyle")), DataGridLineStyle.Solid, DataGridLineStyle.None)
                End If
                If DefaultStyleDS.Tables(XMLStyleName & "Style").Columns.Contains("RowHeadersVisible") Then
                    DGTS.RowHeadersVisible = CBool(DefaultStyleDS.Tables(XMLStyleName & "Style").Rows(0)("RowHeadersVisible"))
                End If
            End If

            ResultDT = dg.LoadColumnsSizeAndPosition(dg.Name & "\" & DGTS.MappingName & "\ColumnSettings", DefaultStyleDS.Tables("Column"))

            ColumnSequenceDV = New DataView(ResultDT)
            ColsDV = ColumnSequenceDV

            DGTSDefault = New DataGridTableStyle() 'This can be used to establish the columns displayed by default
            DGTSDefault.MappingName = "Default"

            For intCol = 0 To ColsDV.Count - 1

                If (IsDBNull(ColsDV(intCol).Item("Visible")) OrElse ColsDV(intCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(intCol).Item("Visible")) = True) Then
                    TextCol = New DataGridFormattableTextBoxColumn(intCol)
                    TextCol.MappingName = CStr(ColsDV(intCol).Item("Mapping"))
                    TextCol.HeaderText = CStr(ColsDV(intCol).Item("HeaderText"))

                    If IsDBNull(ColsDV(intCol).Item("Format")) = False AndAlso CStr(ColsDV(intCol).Item("Format")) <> "" Then
                        TextCol.Format = CStr(ColsDV(intCol).Item("Format"))
                    End If

                    DGTSDefault.GridColumnStyles.Add(TextCol)

                End If

                If ((IsDBNull(ColsDV(intCol).Item("Visible")) OrElse ColsDV(intCol).Item("Visible").ToString.Trim.Length = 0 OrElse CBool(ColsDV(intCol).Item("Visible")) = True) AndAlso (GetAllSettings(dg.AppKey, dg.Name & "\" & DGTS.MappingName & "\Customize\ColumnSelector") Is Nothing OrElse CDbl(GetSetting(dg.AppKey, dg.Name & "\" & DGTS.MappingName & "\Customize\ColumnSelector", "Col " & CStr(ColsDV(intCol).Item("Mapping")) & " Customize", CStr(0))) = 1)) Then

                    If CStr(ColsDV(intCol).Item("Type")) = "Text" Then
                        TextCol = New DataGridFormattableTextBoxColumn(intCol)
                        TextCol.MappingName = CStr(ColsDV(intCol).Item("Mapping"))
                        TextCol.HeaderText = CStr(ColsDV(intCol).Item("HeaderText"))
                        TextCol.Width = CInt(ColsDV(intCol).Item("SizeInPixels"))
                        TextCol.NullText = CStr(ColsDV(intCol).Item("NullText"))
                        TextCol.TextBox.WordWrap = True

                        Try

                            If CBool(ColsDV(intCol).Item("ReadOnly")) Then
                                TextCol.ReadOnly = True
                            End If

                        Catch ex As Exception
                        End Try

                        If IsDBNull(ColsDV(intCol).Item("Format")) = False Then
                            If ColsDV(intCol).Item("Format").ToString = "YesNo" Then
                                AddHandler TextCol.Formatting, AddressOf DataGridCustom.FormattingYesNo
                            ElseIf ColsDV(intCol).Item("Format").ToString.Trim.Length > 0 Then
                                TextCol.Format = CStr(ColsDV(intCol).Item("Format"))
                            End If
                        End If

                        AddHandler TextCol.SetCellFormat, AddressOf HighlightGridCell

                        DGTS.GridColumnStyles.Add(TextCol)

                    ElseIf CStr(ColsDV(intCol).Item("Type")) = "Bool" Then
                        BoolCol = New DataGridColorBoolColumn(intCol)
                        BoolCol.MappingName = CStr(ColsDV(intCol).Item("Mapping"))
                        BoolCol.HeaderText = CStr(ColsDV(intCol).Item("HeaderText"))
                        BoolCol.Width = CInt(ColsDV(intCol).Item("SizeInPixels"))
                        BoolCol.NullValue = If(IsNumeric(ColsDV(intCol).Item("NullText").ToString.Trim), CDec(ColsDV(intCol).Item("NullText")), 0)
                        BoolCol.TrueValue = CType("1", Decimal)
                        BoolCol.FalseValue = CType("0", Decimal)
                        BoolCol.AllowNull = False

                        Try

                            If Not IsDBNull(ColsDV(intCol).Item("ReadOnly")) AndAlso ColsDV(intCol).Item("ReadOnly").ToString.Trim.Length > 0 AndAlso CBool(ColsDV(intCol).Item("ReadOnly")) = True Then
                                BoolCol.ReadOnly = True
                            End If

                        Catch ex As Exception
                        End Try

                        AddHandler BoolCol.SetCellFormat, AddressOf HighlightGridCell

                        DGTS.GridColumnStyles.Add(BoolCol)
                    End If
                End If
            Next

            dg.TableStyles.Clear()
            dg.TableStyles.Add(DGTS)
            dg.TableStyles.Add(DGTSDefault)

        Catch ex As Exception
            Throw
        Finally

            If ColumnSequenceDV IsNot Nothing Then ColumnSequenceDV.Dispose()
            ColumnSequenceDV = Nothing

            If ResultDT IsNot Nothing Then ResultDT.Dispose()
            ResultDT = Nothing

            If DGTS IsNot Nothing Then DGTS.Dispose()
            DGTS = Nothing

        End Try

    End Sub

    Private Sub HighlightGridCell(ByVal sender As Object, ByVal e As DataGridFormatCellEventArgs)

        Dim DR As DataRow
        Dim DV As DataView
        Dim DG As DataGridCustom

        Try

            Select Case sender.GetType
                Case GetType(DataGridFormattableTextBoxColumn), GetType(DataGridHighlightTextBoxColumn)

                    DG = CType(CType(sender, DataGridHighlightTextBoxColumn).DataGridTableStyle.DataGrid, DataGridCustom)

                Case GetType(DataGridHighlightBoolColumn), GetType(DataGridColorBoolColumn)

                    DG = CType(CType(sender, DataGridColorBoolColumn).DataGridTableStyle.DataGrid, DataGridCustom)

                Case Else
                    Throw New ApplicationException("unexpected column type" & sender.GetType.ToString)
            End Select

            If DG.DataSource Is Nothing Then Return

            DV = DG.GetCurrentDataView

            If DV IsNot Nothing Then

                If DV.Table.Columns.Contains("GROUP_FUNCTIONALITY") AndAlso Not IsDBNull(DV(e.RowNumber)("GROUP_FUNCTIONALITY")) Then
                    If CStr(DV(e.RowNumber)("GROUP_FUNCTIONALITY")).ToUpper.Trim = "TERMINATION" Then
                        e.BackBrush = _TermColor
                        e.ForeBrush = Brushes.Black
                    End If
                End If

                If DV.Table.Columns.Contains("PROCESS_STATUS") AndAlso Not IsDBNull(DV(e.RowNumber)("PROCESS_STATUS")) Then
                    If DV(e.RowNumber)("PROCESS_STATUS").ToString.Trim.ToUpper = "PENDING" Then
                        e.TextFont = New Font(e.TextFont.Name, e.TextFont.Size, FontStyle.Bold)
                    End If
                End If

            End If

        Catch ex As Exception
            Throw
        Finally
            DV = Nothing
        End Try

    End Sub

#End Region

#Region "Form\Button Events"

    Private Sub ppCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ppCheckBox.CheckedChanged
        PPToDateTimePicker.Enabled = ppCheckBox.Checked
        PPFromDateTimePicker.Enabled = ppCheckBox.Checked
        Me.Label1.Enabled = ppCheckBox.Checked
        Me.Label2.Enabled = ppCheckBox.Checked
    End Sub

    Private Sub EPCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles EPCheckBox.CheckedChanged
        EPToDateTimePicker.Enabled = EPCheckBox.Checked
        epFromDateTimePicker.Enabled = EPCheckBox.Checked
        Me.BetweenLabel3.Enabled = EPCheckBox.Checked
        Me.AndLabel3.Enabled = EPCheckBox.Checked
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click

        If EligibilityHoursDataGrid.DataSource Is Nothing Then Return

        Dim DV As DataView
        Dim DT As DataTable = New DataTable
        Dim DRV As DataRowView
        Dim SearchFilter As New StringBuilder()

        Try

            If _HoursDS.Tables(0) IsNot Nothing AndAlso _HoursDS.Tables(0).Rows.Count > 0 Then
                DT = _HoursDS.Tables(0).Clone()

                If ppCheckBox.Checked = True Then
                    SearchFilter.Append(" MONTH_HRS_WORKED >= " & "'" & Format(Me.PPFromDateTimePicker.Value, "yyyy-MM-dd") & "' AND  MONTH_HRS_WORKED <= '" & Format(PPToDateTimePicker.Value, "yyyy-MM-dd") & "'")
                End If

                If EPCheckBox.Checked = True Then
                    If SearchFilter.ToString.Trim.Length > 0 Then
                        SearchFilter.Append(" AND ")
                    End If
                    SearchFilter.Append(" ELIGIBILITY_MONTH  >= " & "'" & Format(Me.epFromDateTimePicker.Value, "yyyy-MM-dd") & "' AND ELIGIBILITY_MONTH <= '" & Format(EPToDateTimePicker.Value, "yyyy-MM-dd") & "'")
                End If

                If cmbSpecial.Text <> "(all)" Then
                    If SearchFilter.ToString.Trim.Length > 0 Then
                        SearchFilter.Append(" AND ")
                    End If
                    SearchFilter.Append(" TYPE LIKE '" & cmbSpecial.Text.Trim.ToString & "%'")

                ElseIf cmbSpecial.Text = "(all)" Then
                    SearchFilter.Append("")
                End If

                DV = New DataView(_HoursDS.Tables(0), SearchFilter.ToString, "TYPE", DataViewRowState.CurrentRows)
                If DV.Count > 0 Then
                    For Each DRV In DV
                        DT.LoadDataRow(DRV.Row.ItemArray, True)
                    Next
                    EligibilityHoursDataGrid.DataSource = Nothing
                    EligibilityHoursDataGrid.DataSource = DT
                End If

                If EligibilityHoursDataGrid.GetCurrentDataTable IsNot Nothing Then
                    EligibilityHoursDataGrid.Sort = If(EligibilityHoursDataGrid.LastSortedBy, EligibilityHoursDataGrid.DefaultSort)
                End If

                EligibilityHoursDataGrid.CaptionText = EligibilityHoursDataGrid.GetGridRowCount & " of " & _HoursDS.Tables(0).Rows.Count & " matches "
            End If

        Catch ex As Exception
            Throw
        Finally

            If DT IsNot Nothing Then DT.Dispose()
            DT = Nothing

            If DV IsNot Nothing Then DV.Dispose()
            DV = Nothing

            If SearchFilter IsNot Nothing Then
                SearchFilter = Nothing
            End If
        End Try
    End Sub

    Private Sub SpecialAccountsMaintenanceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SpecialAccountsMaintenanceToolStripMenuItem.Click

        Dim FRM As EligAcctHrsMaintForm

        Try

            _EligibilityCalculate = False

            FRM = New EligAcctHrsMaintForm(_FamilyID, _ReadOnlyMode, If(_HoursBS Is Nothing OrElse _HoursBS.Position < 0 OrElse _HoursBS.Current Is Nothing OrElse IsDBNull(CType(_HoursBS.Current, DataRowView).Row("ELIGIBILITY_MONTH")), Nothing, CType(CType(_HoursBS.Current, DataRowView).Row("ELIGIBILITY_MONTH"), Date?)))

            FRM.EligHoursDataSet = _HoursDS  '' used to display when user selected different month
            FRM.Memtype = _Memtype

            FRM.LoadEligAccountHours()

            Dim Result As DialogResult = FRM.ShowDialog()

            If FRM.EligibiltyCalculated Then
                RaiseEvent LoadRegEligMsg()
            End If

            If Result = System.Windows.Forms.DialogResult.Yes OrElse FRM.EligibiltyCalculated Then
                LoadHours(_FamilyID, New DataSet) 'forces a refetch of the displayed data
            End If

        Catch ex As Exception
            Throw
        Finally
            If FRM IsNot Nothing Then
                FRM.Dispose()
            End If
            FRM = Nothing
        End Try
    End Sub

    Private Sub SpecialAccountRemarksToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SpecialAccountRemarksToolStripMenuItem.Click

        Dim EligSpecialRemarksFrm As SpecialRemarksViewerForm
        Dim Result As DialogResult
        Dim SelectedMonthDV As DataView
        Dim DT As DataTable
        Dim DR As DataRow

        Try
            If _HoursBS Is Nothing OrElse _HoursBS.Position < 0 OrElse _HoursBS.Current Is Nothing Then Return

            DR = CType(_HoursBS.Current, DataRowView).Row

            If Not IsDate(DR("ELIGIBILITY_MONTH")) Then Return

            SelectedMonthDV = New DataView(_HoursDS.Tables(0), "ELIGIBILITY_MONTH = '" & Format(DR("ELIGIBILITY_MONTH"), "yyyy-MM-dd") & "' AND ACCTNO = " & CInt(DR("ACCTNO")), "", DataViewRowState.CurrentRows)
            If SelectedMonthDV.Count > 0 Then
                DT = SelectedMonthDV.ToTable
            End If

            EligSpecialRemarksFrm = New SpecialRemarksViewerForm(_FamilyID, DT)
            Result = EligSpecialRemarksFrm.ShowDialog()

        Catch ex As Exception
            Throw
        Finally

            If EligSpecialRemarksFrm IsNot Nothing Then EligSpecialRemarksFrm.Dispose()
            EligSpecialRemarksFrm = Nothing

            If SelectedMonthDV IsNot Nothing Then SelectedMonthDV.Dispose()
            SelectedMonthDV = Nothing

        End Try
    End Sub

    Private Sub CalculateEligibilityToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CalculateEligibilityToolStripMenuItem.Click

        Dim EligCalcDS As CalculateEligibility
        Dim FrmWait As New Waitmessage

        Dim DR As DataRow

        Try
            If _HoursBS Is Nothing OrElse _HoursBS.Position < 0 OrElse _HoursBS.Current Is Nothing Then Return

            DR = CType(_HoursBS.Current, DataRowView).Row

            If Not IsDate(DR("ELIGIBILITY_MONTH")) Then Return

            If CDate(DR("ELIGIBILITY_MONTH")) > CMSDALFDBEL.GlobalEligPeriod Then
                MessageBox.Show("You cannot calculate Eligibility for the future period", "Future Eligibility", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ''Initialising 
            EligCalcDS = New CalculateEligibility(_FamilyID, CDate(DR("ELIGIBILITY_MONTH")))

            ''check status of rows as they are new to calculate eligibilty
            If EligCalcDS.RetrieveRowStatusbeforeEligcalculation(_FamilyID, CDate(DR("ELIGIBILITY_MONTH"))) = False Then
                Return
            End If

            '' There are changes in elig_acct_hours row status
            Dim ReturnedStatus As Boolean = False
            FrmWait.Show()
            FrmWait.Activate()

            Cursor.Current = Cursors.WaitCursor
            Application.DoEvents()

            ReturnedStatus = EligCalcDS.DetermineEligibility(_FamilyID, CDate(DR("ELIGIBILITY_MONTH")))
            Cursor.Current = Cursors.Default
            FrmWait.Hide()

            If ReturnedStatus Then
                RaiseEvent LoadRegEligMsg()
                MessageBox.Show("Eligibility calculated Sucessfully. Please check Eligibilty Tab for results", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadHours()   '' this is to get elig_acct_hours from database to show pending status / or not after calculating Eligibility.
            Else
                MessageBox.Show("Error while Calculating Eligibility." & vbCrLf & "Please try again ", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Cursor.Current = Cursors.Default
                FrmWait.Hide()
            End If

        Catch ex As Exception
            Throw
        Finally
            If EligCalcDS IsNot Nothing Then
                EligCalcDS.disposeobjects()
                EligCalcDS = Nothing
            End If
            If FrmWait IsNot Nothing Then
                FrmWait.Dispose()
            End If
            FrmWait = Nothing
        End Try

    End Sub

    Private Sub TotalHoursViewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TotalHoursViewToolStripMenuItem.Click

        If _HoursDS Is Nothing OrElse _HoursDS.Tables.Count = 0 Then Return
        Dim EligTotalHrsViewFrm As frmMainframeView

        Try

            EligTotalHrsViewFrm = New frmMainframeView(_FamilyID)
            EligTotalHrsViewFrm.ShowDialog()

        Catch ex As Exception
            Throw
        Finally

            If EligTotalHrsViewFrm IsNot Nothing Then EligTotalHrsViewFrm.Dispose()
            EligTotalHrsViewFrm = Nothing
        End Try
    End Sub

    Private Sub EligibilityDataGrid_RefreshGridData() Handles EligibilityHoursDataGrid.RefreshGridData
        Dim Cancel As Boolean = False

        Try

            RaiseEvent BeforeRefresh(Me, Cancel)

            If Cancel = False Then
                LoadHours()
            End If

            RaiseEvent AfterRefresh(Me)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SetForRecalculationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SetForRecalculationToolStripMenuItem.Click

        Dim EligCalcDS As CalculateEligibility

        Dim DR As DataRow

        Try
            If _HoursBS Is Nothing OrElse _HoursBS.Position < 0 OrElse _HoursBS.Current Is Nothing Then Return

            DR = CType(_HoursBS.Current, DataRowView).Row

            If Not IsDate(DR("ELIGIBILITY_MONTH")) Then Return

            If CDate(DR("ELIGIBILITY_MONTH")) > CMSDALFDBEL.GlobalEligPeriod Then
                MessageBox.Show("You cannot Reset the switches for the future period", "Future Eligibility", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ''Initialising 
            EligCalcDS = New CalculateEligibility(_FamilyID, CDate(DR("ELIGIBILITY_MONTH")))

            If EligCalcDS.ResetSwitchesForEligcalculation(_FamilyID, CDate(DR("ELIGIBILITY_MONTH"))) Then
                MessageBox.Show("Reset the switches for Calculation", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Error while resetting the switches." & vbCrLf & "Please try again ", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            LoadHours()

        Catch ex As Exception
            Throw
        Finally
            If EligCalcDS IsNot Nothing Then
                EligCalcDS.disposeobjects()
                EligCalcDS = Nothing
            End If
        End Try

    End Sub

    Private Sub EligAcctHrsContextMenuStrip_Opening(sender As Object, e As CancelEventArgs) Handles EligAcctHrsContextMenuStrip.Opening


        Dim DGContextMenu As ContextMenuStrip

        DGContextMenu = CType(sender, ContextMenuStrip)
        DGContextMenu.Items("SpecialAccountsMaintenanceToolStripMenuItem").Available = False
        DGContextMenu.Items("SpecialAccountRemarksToolStripMenuItem").Available = False
        DGContextMenu.Items("CalculateEligibilityToolStripMenuItem").Available = False

        DGContextMenu.Items("SpecialAccountsMaintenanceToolStripMenuItem").Enabled = False
        DGContextMenu.Items("SpecialAccountRemarksToolStripMenuItem").Enabled = False
        DGContextMenu.Items("CalculateEligibilityToolStripMenuItem").Enabled = False

        DGContextMenu.Items("SetForRecalculationToolStripMenuItem").Available = False

        If Not Me.DualFamily Then
            If UFCWGeneralAD.REGMEligMaintenanceAccess Then
                DGContextMenu.Items("SpecialAccountsMaintenanceToolStripMenuItem").Available = True
                DGContextMenu.Items("SpecialAccountRemarksToolStripMenuItem").Available = True
                DGContextMenu.Items("CalculateEligibilityToolStripMenuItem").Available = True

                DGContextMenu.Items("SpecialAccountsMaintenanceToolStripMenuItem").Enabled = True
                DGContextMenu.Items("SpecialAccountRemarksToolStripMenuItem").Enabled = True

                If Not _ReadOnlyMode Then
                    DGContextMenu.Items("CalculateEligibilityToolStripMenuItem").Enabled = True
                End If

            End If

            If UFCWGeneralAD.REGMSupervisor Then
                DGContextMenu.Items("SetForRecalculationToolStripMenuItem").Available = True
            End If
        End If

    End Sub

    'Private Sub EligibilityHoursDataGrid_PositionChanged(sender As Object, e As EventArgs) Handles EligibilityHoursDataGrid.PositionChanged

    '    Dim DR As DataRow
    '    Dim CM As CurrencyManager

    '    Try

    '        If EligibilityHoursDataGrid.DataSource IsNot Nothing Then

    '            CM = CType(sender, CurrencyManager)
    '            DR = CType(CM.Current, DataRowView).Row

    '            If DR IsNot Nothing AndAlso DR.Table.Columns.Contains("ELIGIBILITY_MONTH") AndAlso Not IsDBNull(DR("ELIGIBILITY_MONTH")) Then

    '                _SelectedEligPeriod = CDate(DR("ELIGIBILITY_MONTH"))

    '            End If

    '            If DR IsNot Nothing AndAlso DR.Table.Columns.Contains("ACCTNO") AndAlso Not IsDBNull(DR("ACCTNO")) Then

    '                _SelectedAcctno = CInt(DR("ACCTNO"))

    '            End If

    '        End If

    '    Catch ex As Exception
    '        Throw
    '    Finally
    '    End Try

    'End Sub

#End Region

End Class