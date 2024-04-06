Option Infer On
Imports System.ComponentModel

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Text

Public Class HoursControl
    Inherits System.Windows.Forms.UserControl
    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private _FamilyID As Integer = -1

    Private _APPKEY As String = "UFCW\Claims\"
    Private _Loading As Boolean = True

    Public Event BeforeRefresh(ByVal sender As Object, ByRef Cancel As Boolean)
    Public Event AfterRefresh(ByVal sender As Object)
    Public ReadOnly Property [ReadOnly] As Boolean = True

    Private _HoursDS As New DataSet

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            If HoursDataGrid IsNot Nothing Then
                HoursDataGrid.Dispose()
            End If
            HoursDataGrid = Nothing

            If _HoursDS IsNot Nothing Then
                _HoursDS.Dispose()
            End If
            _HoursDS = Nothing

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
        Set(ByVal value As Integer)
            _FamilyID = value
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
        Set(ByVal value As String)
            _APPKEY = value
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

            If HoursDataGrid IsNot Nothing Then 'save prior style changes in between searches
                HoursDataGrid.DataSource = Nothing
            End If

            If _HoursDS Is Nothing OrElse _HoursDS.Tables.Count < 1 Then
                _HoursDS = CMSDALFDBMD.GetHours(_FamilyID)
            End If

            '' For displaying gaps
            If _HoursDS.Tables.Count > 0 AndAlso _HoursDS.Tables(0).Rows.Count > 0 Then
                Dim DT As DataTable
                Dim Fromdate As Date = (From value In _HoursDS.Tables(0).AsEnumerable
                                        Select value.Field(Of Date)("WRKPERIOD")).Min

                If Not _HoursDS.Tables.Contains("HOURS_GAP_MONTH_YEAR") Then
                    DT = CMSDALFDBMD.RetrieveMonthsGapInfoinHours(_FamilyID, Fromdate, (UFCWGeneral.NowDate.Date.AddMonths(-3)).AddDays(1), Nothing)
                Else
                    DT = _HoursDS.Tables("HOURS_GAP_MONTH_YEAR")
                End If
                Dim GapPeriods =
                                 From period In (DT).AsEnumerable
                                 Where period.IsNull("PSTPERIOD") Select period.Field(Of Date)("DIA")

                For Each Period In GapPeriods
                    _HoursDS.Tables(0).Rows.Add(New Object() {Nothing, Nothing, Period, Period, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, "No Hours", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, "No Hours", Nothing, Nothing})
                Next
            End If
            ''
            HoursDataGrid.SuspendLayout()

            HoursDataGrid.DataSource = _HoursDS.Tables(0)
            HoursDataGrid.SetTableStyle()
            HoursDataGrid.Sort = If(HoursDataGrid.LastSortedBy, HoursDataGrid.DefaultSort)

            HoursDataGrid.CaptionText = "Hours for Family (" & _FamilyID.ToString & ") "

            HoursDataGrid.ResumeLayout()

            ppCheckBox.Enabled = True
            EPCheckBox.Enabled = True

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub ClearAll()
        If HoursDataGrid IsNot Nothing Then
            HoursDataGrid.DataSource = Nothing
            HoursDataGrid.CaptionText = ""
        End If

        ppCheckBox.Checked = False
        EPCheckBox.Checked = False
        ppCheckBox.Enabled = False
        EPCheckBox.Enabled = False
        _HoursDS.Tables.Clear()
    End Sub

    Private Sub HoursControl_dispose(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed

        If HoursDataGrid IsNot Nothing Then
            HoursDataGrid.Dispose()
        End If
        HoursDataGrid = Nothing

        Me.Dispose()

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
        If HoursDataGrid.DataSource Is Nothing Then Return
        Dim DV As DataView
        Dim DT As DataTable = New DataTable
        Dim DRV As DataRowView
        Dim SearchFilter As New StringBuilder()

        Try
            If _HoursDS.Tables(0) IsNot Nothing AndAlso _HoursDS.Tables(0).Rows.Count > 0 Then
                DT = _HoursDS.Tables(0).Clone()

                If ppCheckBox.Checked = True Then
                    SearchFilter.Append(" PSTPERIOD >= " & "'" & Format(Me.PPFromDateTimePicker.Value, "yyyy-MM-dd") & "' AND  PSTPERIOD <= '" & Format(PPToDateTimePicker.Value, "yyyy-MM-dd") & "'")
                End If

                If EPCheckBox.Checked = True Then
                    If SearchFilter.ToString.Trim.Length > 0 Then
                        SearchFilter.Append(" AND ")
                    End If
                    SearchFilter.Append(" ELIGIBILITY_MONTH  >= " & "'" & Format(Me.epFromDateTimePicker.Value, "yyyy-MM-dd") & "' AND ELIGIBILITY_MONTH <= '" & Format(EPToDateTimePicker.Value, "yyyy-MM-dd") & "'")
                End If

                DV = New DataView(_HoursDS.Tables(0), SearchFilter.ToString, "", DataViewRowState.CurrentRows)
                If DV.Count > 0 Then
                    For Each DRV In DV
                        DT.LoadDataRow(DRV.Row.ItemArray, True)
                    Next
                    HoursDataGrid.DataSource = Nothing
                    HoursDataGrid.DataSource = DT
                End If

                If HoursDataGrid.GetCurrentDataTable IsNot Nothing Then
                    HoursDataGrid.Sort = If(HoursDataGrid.LastSortedBy, HoursDataGrid.DefaultSort)
                End If

                HoursDataGrid.CaptionText = HoursDataGrid.GetGridRowCount & " of " & _HoursDS.Tables(0).Rows.Count & " matches "
            End If
        Catch ex As Exception
            Throw
        Finally
            If DT IsNot Nothing Then DT.Dispose()
            DT = Nothing

            If DV IsNot Nothing Then DV.Dispose()
            DV = Nothing

            SearchFilter = Nothing
        End Try
    End Sub

    Private Sub HoursDataGrid_MouseUp(sender As Object, e As MouseEventArgs) Handles HoursDataGrid.MouseUp
        Dim RegTotal As Decimal = 0
        Dim TVTotal As Decimal = 0
        Dim FamilyTotal As Decimal = 0
        Dim ContTotal As Decimal = 0
        Dim cnt As Integer = 0
        Dim ColPos As Integer = 0
        Dim StrSum As String = ""
        Dim DRs As ArrayList

        Dim MyGrid As DataGridCustom

        Try
            MyGrid = CType(sender, DataGridCustom)

            DRs = MyGrid.GetSelectedDataRows

            For Each DR As DataRow In DRs
                If Not IsDBNull(DR("CONTR_HRS")) Then
                    ContTotal += CDec(DR("CONTR_HRS"))
                End If
                If Not IsDBNull(DR("REG_HOURS")) Then
                    RegTotal += CDec(DR("REG_HOURS"))
                End If
                If Not IsDBNull(DR("FAMLVE_HRS")) Then
                    FamilyTotal += CDec(DR("FAMLVE_HRS"))
                End If
                If Not IsDBNull(DR("TVHRS")) Then
                    TVTotal += CDec(DR("TVHRS"))
                End If
            Next

            StrSum = "          Hours worked =  " & CStr(RegTotal) & ", Contri. Hours =  " & CStr(ContTotal) & ",  Family Leave =  " & CStr(FamilyTotal) & ",  Term.Vac. =  " & CStr(TVTotal)

            MyGrid.CaptionText = ""
            MyGrid.CaptionText = " Hours for Family (" & _FamilyID.ToString & ") " & StrSum

        Catch ex As Exception

            Throw

        End Try
    End Sub

#End Region

End Class