
Public Class DentalLineDetail

    Private _APPKEY As String = "UFCW\Claims\"

    Private _FamilyID As Integer
    Private _RelationID As Short?
    Private _ClaimID As Integer
    Private _Procedure As String
    Private _DateOfService As Date
    Private _FirstDay As Date
    Private _LastDay As Date

    Public Sub New(ByVal familiyid As Integer, ByVal relationid As Short?, ByVal claimid As Integer)
        InitializeComponent()
        _FamilyID = familiyid
        _RelationID = relationid
        _ClaimID = claimid
    End Sub
    Private Sub DentalLineDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

        DentalLineDetailDataGrid.ReadOnly = True
        Me.Text = "Line Details - Claim # " & _ClaimID

        SetClaimSummary()

        If UFCWGeneralAD.CMSLocals Then
            With DentalLineDetailDataGrid
                .AllowFind = False
                .AllowGoTo = False
                .AllowExport = False
                .AllowPrint = False
                .AllowCopy = False
            End With
        End If

        AddHandler Me.KeyUp, AddressOf DentalLineDetail_KeyUp
        LoadDentalClaimDetail(_FamilyID, _RelationID, _ClaimID)
    End Sub
    Private Sub LoadDentalClaimDetail(ByVal familyID As Integer, ByVal relationID As Short?, ByVal claimID As Integer)
        _ClaimDetailDT = New DataTable
        _ClaimDetailBS = New BindingSource

        _ClaimDetailDT = DentalDAL.GetDentalClaimDetail(_FamilyID, _RelationID, _ClaimID)


        _ClaimDetailBS.DataSource = _ClaimDetailDT

        DentalLineDetailDataGrid.DataSource = _ClaimDetailBS
        DentalLineDetailDataGrid.SetTableStyle()
        DentalLineDetailDataGrid.Sort = If(DentalLineDetailDataGrid.LastSortedBy, DentalLineDetailDataGrid.DefaultSort)

        DentalLineDetailDataGrid.ResumeLayout()
    End Sub
    Private Sub LoadDentalLineDetail(ByVal familyID As Integer, ByVal relationID As Short?, ByVal claimID As Integer, ByVal procedure As String)
        _AccumulatorDetailDT = New DataTable
        _AccumulatorDetailBS = New BindingSource

        _AccumulatorDetailDT = DentalDAL.GetDentalLineDetail(_FamilyID, _RelationID, _ClaimID, _Procedure)

        _AccumulatorDetailBS.DataSource = _AccumulatorDetailDT

        DentalAccumulatorValues.DentalAccumulatorDetailsDataGrid.DataSource = _AccumulatorDetailBS
        DentalAccumulatorValues.DentalAccumulatorDetailsDataGrid.SetTableStyle()
        DentalAccumulatorValues.DentalAccumulatorDetailsDataGrid.Sort = If(DentalAccumulatorValues.DentalAccumulatorDetailsDataGrid.LastSortedBy, DentalAccumulatorValues.DentalAccumulatorDetailsDataGrid.DefaultSort)

        DentalAccumulatorValues.DentalAccumulatorDetailsDataGrid.ResumeLayout()

    End Sub
    <System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)

            _APPKEY = value
        End Set
    End Property
    Private Sub DentalLineDetail_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp

        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub DentalLineDetail_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()

    End Sub

    Private Sub DentalLineDetail_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        UFCWGeneral.SaveFormPosition(Me, _APPKEY)

        DentalLineDetailDataGrid.TableStyles.Clear()
        DentalLineDetailDataGrid.Dispose()
        DentalLineDetailDataGrid.DataSource = Nothing
    End Sub

    Private Sub DentalLineDetail_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MyBase.KeyPress

        If e.KeyChar = Chr(27) Then Me.Close()

    End Sub
    Private Sub SetClaimSummary()
        Try

            _ClaimDetailDT = New DataTable
            _ClaimDetailDT = DentalDAL.GetDentalClaimDetail(_FamilyID, _RelationID, _ClaimID)

            Dim SumCharges As Decimal = 0
            SumCharges = _ClaimDetailDT.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("CHARGED_AMT"))
            TotalChargesTextBox.Text = Format(SumCharges, "c")

            Dim OtherCharges As Decimal = 0
            OtherCharges = _ClaimDetailDT.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("OTHER_INS_AMT"))
            TotalOtherTextBox.Text = Format(OtherCharges, "c")

            Dim PaidCharges As Decimal = 0
            PaidCharges = _ClaimDetailDT.AsEnumerable().Sum(Function(row) row.Field(Of Decimal)("PAID_AMT"))
            TotalPaidTextBox.Text = Format(PaidCharges, "c")


        Catch ex As Exception

            Throw
        End Try
    End Sub
    Private Sub SetAccumulators()
        Try

            DentalAccumulatorValues.PersonalAccumulatorCurrentLabel.Text = "Personal Accumulators (" & Year(_DateOfService) & ")"
            DentalAccumulatorValues.FamilyAccumulatorsCurrentLabel.Text = "Family Accumulators (" & Year(_DateOfService) & ")"
            DentalAccumulatorValues.PersonalAccumulatorPriorLabel.Text = "Personal Accumulators (" & Year(_DateOfService.AddYears(-1)) & ")"
            DentalAccumulatorValues.FamilyAccumulatorsPriorLabel.Text = "Family Accumulators (" & Year(_DateOfService.AddYears(-1)) & ")"
            DentalAccumulatorValues.PersonalMAXLabel.Text = "Personal MAX (" & Year(_DateOfService) & ")"
            DentalAccumulatorValues.PersonalMAXPriorLabel.Text = "Personal MAX (" & Year(_DateOfService.AddYears(-1)) & ")"
            DentalAccumulatorValues.FamilyMAXLabel.Text = "Family MAX (" & Year(_DateOfService) & ")"
            DentalAccumulatorValues.FamilyMAXPriorLabel.Text = "Family MAX (" & Year(_DateOfService.AddYears(-1)) & ")"

            Dim FamilyMAXDT As DataTable
            FamilyMAXDT = DentalDAL.GetDentalAccumulators(_FamilyID, 0, "CURRENT", _FirstDay, _LastDay, "FAMILYMAX")
            If (((FamilyMAXDT) IsNot Nothing) _
            AndAlso (FamilyMAXDT.Rows.Count > 0)) Then
                If Not IsDBNull(FamilyMAXDT.Rows(0).ItemArray(0)) Then DentalAccumulatorValues.FamilyMAXTextBox.Text = FormatCurrency(FamilyMAXDT.Rows(0).ItemArray(0).ToString(), , , TriState.True, TriState.True)
                If Not IsDBNull(FamilyMAXDT.Rows(0).ItemArray(1)) Then DentalAccumulatorValues.FamilyRolloverMAXTextBox.Text = FormatCurrency(FamilyMAXDT.Rows(0).ItemArray(1).ToString(), , , TriState.True, TriState.True)
            End If

            Dim FamilyMAXPriorDT As DataTable
            FamilyMAXPriorDT = DentalDAL.GetDentalAccumulators(_FamilyID, 0, "PRIOR", _FirstDay, _LastDay, "FAMILYMAX")
            If (((FamilyMAXPriorDT) IsNot Nothing) _
            AndAlso (FamilyMAXPriorDT.Rows.Count > 0)) Then
                If Not IsDBNull(FamilyMAXPriorDT.Rows(0).ItemArray(0)) Then DentalAccumulatorValues.FamilyMAXPriorTextBox.Text = FormatCurrency(FamilyMAXPriorDT.Rows(0).ItemArray(0).ToString(), , , TriState.True, TriState.True)
                If Not IsDBNull(FamilyMAXPriorDT.Rows(0).ItemArray(1)) Then DentalAccumulatorValues.FamilyRolloverMAXPriorTextBox.Text = FormatCurrency(FamilyMAXPriorDT.Rows(0).ItemArray(1).ToString(), , , TriState.True, TriState.True)
            End If

            Dim PersonalMAXDT As DataTable
            PersonalMAXDT = DentalDAL.GetDentalAccumulators(_FamilyID, _RelationID, "CURRENT", _FirstDay, _LastDay, "PERSONALMAX")
            If (((PersonalMAXDT) IsNot Nothing) _
                AndAlso (PersonalMAXDT.Rows.Count > 0) AndAlso Not IsDBNull(PersonalMAXDT.Rows(0).ItemArray(0))) Then
                If Not IsDBNull(PersonalMAXDT.Rows(0).ItemArray(0)) Then DentalAccumulatorValues.PersonalMAXTextBox.Text = FormatCurrency(PersonalMAXDT.Rows(0).ItemArray(0).ToString(), , , TriState.True, TriState.True)
                If Not IsDBNull(PersonalMAXDT.Rows(0).ItemArray(1)) Then DentalAccumulatorValues.PersonalRolloverMAXTextBox.Text = FormatCurrency(PersonalMAXDT.Rows(0).ItemArray(1).ToString(), , , TriState.True, TriState.True)
            End If

            Dim PersonalMAXPriorDT As DataTable
            PersonalMAXPriorDT = DentalDAL.GetDentalAccumulators(_FamilyID, _RelationID, "PRIOR", _FirstDay, _LastDay, "PERSONALMAX")
            If (((PersonalMAXPriorDT) IsNot Nothing) _
            AndAlso (PersonalMAXPriorDT.Rows.Count > 0)) Then
                If Not IsDBNull(PersonalMAXPriorDT.Rows(0).ItemArray(0)) Then DentalAccumulatorValues.PersonalMAXPriorTextBox.Text = FormatCurrency(PersonalMAXPriorDT.Rows(0).ItemArray(0).ToString(), , , TriState.True, TriState.True)
                If Not IsDBNull(PersonalMAXPriorDT.Rows(0).ItemArray(1)) Then DentalAccumulatorValues.PersonalRolloverMAXPriorTextBox.Text = FormatCurrency(PersonalMAXPriorDT.Rows(0).ItemArray(1).ToString(), , , TriState.True, TriState.True)
            End If

            Dim FamilyDT As DataTable
            FamilyDT = DentalDAL.GetDentalAccumulators(_FamilyID, 0, "CURRENT", _FirstDay, _LastDay, "FAMILY")
            If (((FamilyDT) IsNot Nothing) _
            AndAlso (FamilyDT.Rows.Count > 0)) Then
                DentalAccumulatorValues.FamilyAccumulatorsDataGrid.DataSource = FamilyDT
                DentalAccumulatorValues.FamilyAccumulatorsDataGrid.ReadOnly = True
                DentalAccumulatorValues.FamilyAccumulatorsDataGrid.Columns(0).HeaderText = "Accumulator"
                DentalAccumulatorValues.FamilyAccumulatorsDataGrid.Columns(0).Width = 140
                DentalAccumulatorValues.FamilyAccumulatorsDataGrid.Columns(1).HeaderText = "Value"
                DentalAccumulatorValues.FamilyAccumulatorsDataGrid.Columns(1).Width = 70
                DentalAccumulatorValues.FamilyAccumulatorsDataGrid.Columns(1).DefaultCellStyle.Format = "c"
            End If

            Dim FamilyPriorDT As DataTable
            FamilyPriorDT = DentalDAL.GetDentalAccumulators(_FamilyID, 0, "PRIOR", _FirstDay, _LastDay, "FAMILY")
            If (((FamilyPriorDT) IsNot Nothing) _
            AndAlso (FamilyPriorDT.Rows.Count > 0)) Then
                DentalAccumulatorValues.FamilyAccumulatorsPriorDataGrid.DataSource = FamilyPriorDT
                DentalAccumulatorValues.FamilyAccumulatorsPriorDataGrid.ReadOnly = True
                DentalAccumulatorValues.FamilyAccumulatorsPriorDataGrid.Columns(0).HeaderText = "Accumulator"
                DentalAccumulatorValues.FamilyAccumulatorsPriorDataGrid.Columns(0).Width = 140
                DentalAccumulatorValues.FamilyAccumulatorsPriorDataGrid.Columns(1).HeaderText = "Value"
                DentalAccumulatorValues.FamilyAccumulatorsPriorDataGrid.Columns(1).Width = 70
                DentalAccumulatorValues.FamilyAccumulatorsPriorDataGrid.Columns(1).DefaultCellStyle.Format = "c"
            End If

            Dim PersonalDT As DataTable
            PersonalDT = DentalDAL.GetDentalAccumulators(_FamilyID, _RelationID, "CURRENT", _FirstDay, _LastDay, "PERSONAL")

            If (((PersonalDT) IsNot Nothing) _
            AndAlso (PersonalDT.Rows.Count > 0)) Then
                DentalAccumulatorValues.PersonalAccumulatorDataGrid.DataSource = PersonalDT
                DentalAccumulatorValues.PersonalAccumulatorDataGrid.ReadOnly = True
                DentalAccumulatorValues.PersonalAccumulatorDataGrid.Columns(0).HeaderText = "Accumulator"
                DentalAccumulatorValues.PersonalAccumulatorDataGrid.Columns(0).Width = 140
                DentalAccumulatorValues.PersonalAccumulatorDataGrid.Columns(1).HeaderText = "Value"
                DentalAccumulatorValues.PersonalAccumulatorDataGrid.Columns(1).Width = 70
                DentalAccumulatorValues.PersonalAccumulatorDataGrid.Columns(1).DefaultCellStyle.Format = "c"
            End If


            Dim PersonalPriorDT As DataTable
            PersonalPriorDT = DentalDAL.GetDentalAccumulators(_FamilyID, _RelationID, "PRIOR", _FirstDay, _LastDay, "PERSONAL")
            If (((PersonalPriorDT) IsNot Nothing) _
            AndAlso (PersonalPriorDT.Rows.Count > 0)) Then
                DentalAccumulatorValues.PersonalAccumulatorPriorDataGrid.DataSource = PersonalPriorDT
                DentalAccumulatorValues.PersonalAccumulatorPriorDataGrid.ReadOnly = True
                DentalAccumulatorValues.PersonalAccumulatorPriorDataGrid.Columns(0).HeaderText = "Accumulator"
                DentalAccumulatorValues.PersonalAccumulatorPriorDataGrid.Columns(0).Width = 140
                DentalAccumulatorValues.PersonalAccumulatorPriorDataGrid.Columns(1).HeaderText = "Value"
                DentalAccumulatorValues.PersonalAccumulatorPriorDataGrid.Columns(1).Width = 70
                DentalAccumulatorValues.PersonalAccumulatorPriorDataGrid.Columns(1).DefaultCellStyle.Format = "c"
            End If
        Catch ex As Exception

            Throw
        End Try
    End Sub
    Public Shared Function UnFormatSSN(ByVal strSSN As String) As String
        If Replace(Replace(Replace(strSSN, " ", ""), "-", ""), "/", "") <> "" Then
            Return Microsoft.VisualBasic.Strings.Format(CLng(Replace(Replace(Replace(strSSN, " ", ""), "-", ""), "/", "")), "0########")
        Else
            Return ""
        End If
    End Function
    Public Shared Function FormatSSN(ByVal strSSN As String) As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="strSSN"></param>
        ' <returns></returns>
        ' <remarks>
        ' "Stolen" from Nick Snyder.  Altered to have xxx-xx-xxxx instead of xxx-xxx-xxx
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/18/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim StrTemp As String

        StrTemp = UnFormatSSN(strSSN)
        If StrTemp.Trim <> "" Then
            Return StrTemp.Substring(0, 3) & "-" & StrTemp.Substring(3, 2) & "-" & StrTemp.Substring(5, 4)
        Else
            Return ""
        End If
    End Function
    Private Sub DentalLineDetailDataGrid_CurrentRowChanged(ByVal CurrentRowIndex As Integer?, ByVal LastRowIndex As Integer?) Handles DentalLineDetailDataGrid.CurrentRowChanged
        Dim CM As CurrencyManager
        Dim DGDRV As DataRowView
        Dim DGRow As DataRow

        Try

            If CurrentRowIndex IsNot Nothing Then

                CM = DirectCast(DentalLineDetailDataGrid.BindingContext(DentalLineDetailDataGrid.DataSource, DentalLineDetailDataGrid.DataMember), CurrencyManager)
                DGDRV = DirectCast(CM.Current, DataRowView)
                DGRow = DGDRV.Row
                _DateOfService = CDate(DGRow("DATE_OF_SERVICE"))
                _FirstDay = DateSerial(Year(_DateOfService), 1, 1)
                _LastDay = DateSerial(Year(_DateOfService), 12, 31)
                _FamilyID = CInt(DGRow("FAMILY_ID"))
                _RelationID = CShort(DGRow("RELATION_ID"))
                _ClaimID = CInt(DGRow("CLAIM"))
                _Procedure = CStr(DGRow("PROCEDURE"))
                SetAccumulators()
                LoadDentalLineDetail(_FamilyID, _RelationID, _ClaimID, _Procedure)

            End If

        Catch ex As Exception

            Throw

        End Try
    End Sub

    Private Sub OkButton_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
End Class