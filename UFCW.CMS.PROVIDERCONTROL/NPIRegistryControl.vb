Imports System.ComponentModel

Public Class NPIRegistryControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _NPI As Decimal = 0

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the NPI.")> _
    Public Property NPI() As Decimal
        Get
            Return If(_NPI = -1, Nothing, _NPI)
        End Get
        Set(ByVal value As Decimal)
            _NPI = value
        End Set
    End Property

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    Public Sub New(ByVal npi As Decimal)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        _NPI = npi

    End Sub
    Public Sub LoadNPI(ByVal npi As Decimal)
        Try
            _NPI = npi

            LoadNPI()

        Catch ex As Exception
            Throw
        End Try
    End Sub
    Public Sub ClearNPI()

        ProvDS.NPI_REGISTRY.Rows.Clear()
        ProvDS.NPI_REGISTRY_LICENSES.Rows.Clear()
        ProvDS.NPI_REGISTRY_OTHER_PROVIDERS.Rows.Clear()
        _NPI = 0

        Application.DoEvents()

    End Sub
    Public Sub LoadNPI(ByVal dt As DataTable)
        Dim DR As DataRow

        Try

            ProvDS.NPI_REGISTRY.Clear()

            If dt.Rows.Count > 0 Then
                ProvDS.NPI_REGISTRY.ImportRow(dt.Rows(0))
                _NPI = CDec(dt.Rows(0)("NPI").ToString)

                If ProvDS.Tables("NPI_REGISTRY").Rows.Count > 0 Then

                    For X As Integer = 1 To 15
                        If IsDBNull(ProvDS.Tables("NPI_REGISTRY").Rows(0)("HLTHCARE_PROV_TXNMY_CD_" & X)) AndAlso IsDBNull(ProvDS.Tables("NPI_REGISTRY").Rows(0)("PROV_LIC_NUM_" & X)) AndAlso IsDBNull(ProvDS.Tables("NPI_REGISTRY").Rows(0)("PROV_LIC_NUM_STATE_CD_" & X)) AndAlso IsDBNull(ProvDS.Tables("NPI_REGISTRY").Rows(0)("HLTHCARE_PROV_PRIM_TXNMY_SW_" & X)) Then
                        Else
                            Dim columnValuesArray() As Object = New Object() {ProvDS.Tables("NPI_REGISTRY").Rows(0)("HLTHCARE_PROV_TXNMY_CD_" & X), ProvDS.Tables("NPI_REGISTRY").Rows(0)("PROV_LIC_NUM_" & X), ProvDS.Tables("NPI_REGISTRY").Rows(0)("PROV_LIC_NUM_STATE_CD_" & X), ProvDS.Tables("NPI_REGISTRY").Rows(0)("HLTHCARE_PROV_PRIM_TXNMY_SW_" & X)}
                            DR = ProvDS.NPI_REGISTRY_LICENSES.NewRow
                            DR.ItemArray = columnValuesArray
                            ProvDS.NPI_REGISTRY_LICENSES.AddNPI_REGISTRY_LICENSESRow(CType(DR, ProvDS.NPI_REGISTRY_LICENSESRow))
                        End If
                    Next

                    ProvDS.NPI_REGISTRY_OTHER_PROVIDERS.Rows.Clear()

                    For X As Integer = 1 To 50
                        If IsDBNull(ProvDS.Tables("NPI_REGISTRY").Rows(0)("OTH_PROV_IDENT_" & X)) AndAlso IsDBNull(ProvDS.Tables("NPI_REGISTRY").Rows(0)("OTH_PROV_IDENT_TYPE_CD_" & X)) AndAlso IsDBNull(ProvDS.Tables("NPI_REGISTRY").Rows(0)("OTH_PROV_IDENT_STATE_" & X)) AndAlso IsDBNull(ProvDS.Tables("NPI_REGISTRY").Rows(0)("OTH_PROV_IDENT_ISSUER_" & X)) Then
                        Else
                            Dim columnValuesArray() As Object = New Object() {ProvDS.Tables("NPI_REGISTRY").Rows(0)("OTH_PROV_IDENT_" & X), ProvDS.Tables("NPI_REGISTRY").Rows(0)("OTH_PROV_IDENT_TYPE_CD_" & X), ProvDS.Tables("NPI_REGISTRY").Rows(0)("OTH_PROV_IDENT_STATE_" & X), ProvDS.Tables("NPI_REGISTRY").Rows(0)("OTH_PROV_IDENT_ISSUER_" & X)}
                            DR = ProvDS.NPI_REGISTRY_OTHER_PROVIDERS.NewRow
                            DR.ItemArray = columnValuesArray
                            ProvDS.NPI_REGISTRY_OTHER_PROVIDERS.AddNPI_REGISTRY_OTHER_PROVIDERSRow(CType(DR, ProvDS.NPI_REGISTRY_OTHER_PROVIDERSRow))
                        End If
                    Next

                End If
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub LoadNPI()

        Dim DR As DataRow

        Try

            'ProvDS = New ProvDS
            ProvDS = CType(ProviderDAL.GetNPIRegistryByNPI(_NPI, ProvDS), ProvDS)

            ProvDS.NPI_REGISTRY_LICENSES.Rows.Clear()

            If ProvDS.Tables("NPI_REGISTRY").Rows.Count > 0 Then

                For X As Integer = 1 To 15
                    If IsDBNull(ProvDS.Tables("NPI_REGISTRY").Rows(0)("HLTHCARE_PROV_TXNMY_CD_" & X)) AndAlso IsDBNull(ProvDS.Tables("NPI_REGISTRY").Rows(0)("PROV_LIC_NUM_" & X)) AndAlso IsDBNull(ProvDS.Tables("NPI_REGISTRY").Rows(0)("PROV_LIC_NUM_STATE_CD_" & X)) AndAlso IsDBNull(ProvDS.Tables("NPI_REGISTRY").Rows(0)("HLTHCARE_PROV_PRIM_TXNMY_SW_" & X)) Then
                    Else
                        Dim columnValuesArray() As Object = New Object() {ProvDS.Tables("NPI_REGISTRY").Rows(0)("HLTHCARE_PROV_TXNMY_CD_" & X), ProvDS.Tables("NPI_REGISTRY").Rows(0)("PROV_LIC_NUM_" & X), ProvDS.Tables("NPI_REGISTRY").Rows(0)("PROV_LIC_NUM_STATE_CD_" & X), ProvDS.Tables("NPI_REGISTRY").Rows(0)("HLTHCARE_PROV_PRIM_TXNMY_SW_" & X)}
                        DR = ProvDS.NPI_REGISTRY_LICENSES.NewRow
                        DR.ItemArray = columnValuesArray
                        ProvDS.NPI_REGISTRY_LICENSES.AddNPI_REGISTRY_LICENSESRow(CType(DR, ProvDS.NPI_REGISTRY_LICENSESRow))
                    End If
                Next

                ProvDS.NPI_REGISTRY_OTHER_PROVIDERS.Rows.Clear()

                For x As Integer = 1 To 50
                    If IsDBNull(ProvDS.Tables("NPI_REGISTRY").Rows(0)("OTH_PROV_IDENT_" & x)) AndAlso IsDBNull(ProvDS.Tables("NPI_REGISTRY").Rows(0)("OTH_PROV_IDENT_TYPE_CD_" & x)) AndAlso IsDBNull(ProvDS.Tables("NPI_REGISTRY").Rows(0)("OTH_PROV_IDENT_STATE_" & x)) AndAlso IsDBNull(ProvDS.Tables("NPI_REGISTRY").Rows(0)("OTH_PROV_IDENT_ISSUER_" & x)) Then
                    Else
                        Dim columnValuesArray() As Object = New Object() {ProvDS.Tables("NPI_REGISTRY").Rows(0)("OTH_PROV_IDENT_" & x), ProvDS.Tables("NPI_REGISTRY").Rows(0)("OTH_PROV_IDENT_TYPE_CD_" & x), ProvDS.Tables("NPI_REGISTRY").Rows(0)("OTH_PROV_IDENT_STATE_" & x), ProvDS.Tables("NPI_REGISTRY").Rows(0)("OTH_PROV_IDENT_ISSUER_" & x)}
                        DR = ProvDS.NPI_REGISTRY_OTHER_PROVIDERS.NewRow
                        DR.ItemArray = columnValuesArray
                        ProvDS.NPI_REGISTRY_OTHER_PROVIDERS.AddNPI_REGISTRY_OTHER_PROVIDERSRow(CType(DR, ProvDS.NPI_REGISTRY_OTHER_PROVIDERSRow))
                    End If
                Next
            Else
                _NPI = -1 'signal that NPI was not found
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

End Class