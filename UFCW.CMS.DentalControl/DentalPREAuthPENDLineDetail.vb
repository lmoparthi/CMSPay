Public Class DentalPREAuthPENDLineDetail
    Private _APPKEY As String = "UFCW\Claims\"

    Private _FamilyID As Integer
    Private _RelationID As Short?
    Private _ClaimID As Integer
    Private _GridType As String
    Private _ClaimDetailBS As BindingSource
    Private _ClaimDetailDT As DataTable
    <System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)

            _APPKEY = value
        End Set
    End Property
    Public Sub New(ByVal familiyid As Integer, ByVal relationid As Short?, ByVal claimid As Integer, ByVal gridtype As String)
        InitializeComponent()
        _FamilyID = familiyid
        _RelationID = relationid
        _ClaimID = claimid
        _GridType = gridtype
    End Sub

    Private _Disposed As Boolean

    Protected Overrides Sub Finalize()
        Me.Dispose(False)
    End Sub

    ' Protected implementation of Dispose pattern.
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If Not _Disposed Then
            If disposing Then
                If components IsNot Nothing Then
                    components.Dispose()
                End If

                If _ClaimDetailBS IsNot Nothing Then _ClaimDetailBS.Dispose()
                If _ClaimDetailDT IsNot Nothing Then _ClaimDetailDT.Dispose()

                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            ' TODO: set large fields to null.
            _Disposed = True
        End If

        ' Call the base class implementation.
        MyBase.Dispose(disposing)
    End Sub

    Private Sub DentalPREAuthPENDLineDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()

        DentalPREAuthPENDLineDetailDataGrid.ReadOnly = True

        If _GridType.Contains("Auth") Then
            Me.Text = "Pre Authorizations Line Details - Claim # " & _ClaimID
        Else
            Me.Text = "Pending Review And Processing Line Details - Claim # " & _ClaimID
        End If

        SetClaimSummary()

        If UFCWGeneralAD.CMSLocals Then
            With DentalPREAuthPENDLineDetailDataGrid
                .AllowFind = False
                .AllowGoTo = False
                .AllowExport = False
                .AllowPrint = False
                .AllowCopy = False
            End With
        End If

        AddHandler Me.KeyUp, AddressOf DentalPREAuthPENDLineDetail_KeyUp
        LoadDentalPREAuthPENDClaimDetail(_FamilyID, _RelationID, _ClaimID, _GridType)
    End Sub
    Private Sub LoadDentalPREAuthPENDClaimDetail(ByVal familyID As Integer, ByVal relationID As Short?, ByVal claimID As Integer, ByVal gridType As String)
        _ClaimDetailDT = New DataTable
        _ClaimDetailBS = New BindingSource

        _ClaimDetailDT = DentalDAL.GetDentalPREAuthPENDClaimDetail(_FamilyID, _RelationID, _ClaimID, _GridType)


        _ClaimDetailBS.DataSource = _ClaimDetailDT

        DentalPREAuthPENDLineDetailDataGrid.DataSource = _ClaimDetailBS
        DentalPREAuthPENDLineDetailDataGrid.SetTableStyle()
        DentalPREAuthPENDLineDetailDataGrid.Sort = If(DentalPREAuthPENDLineDetailDataGrid.LastSortedBy, DentalPREAuthPENDLineDetailDataGrid.DefaultSort)

        DentalPREAuthPENDLineDetailDataGrid.ResumeLayout()
    End Sub

    Private Sub DentalPREAuthPENDLineDetail_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        UFCWGeneral.SaveFormPosition(Me, _APPKEY)

        DentalPREAuthPENDLineDetailDataGrid.TableStyles.Clear()
        DentalPREAuthPENDLineDetailDataGrid.Dispose()
        DentalPREAuthPENDLineDetailDataGrid.DataSource = Nothing

    End Sub
    Private Sub DentalPREAuthPENDLineDetail_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyCode = Keys.Escape Then Me.Close()

    End Sub
    Private Sub DentalPREAuthPENDLineDetail_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MyBase.KeyPress

        If e.KeyChar = "27" Then Me.Dispose()

    End Sub

    Private Sub SetClaimSummary()
        Try

            _ClaimDetailDT = New DataTable
            _ClaimDetailDT = DentalDAL.GetDentalPREAuthPENDClaimDetail(_FamilyID, _RelationID, _ClaimID, _GridType)

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

    Private Sub OkButton_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub DentalPREAuthPENDLineDetail_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
End Class