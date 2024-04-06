Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ComponentModel

Public Class ClaimsHistoryViewerForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _APPKEY As String = "UFCW\Claims\"

    <DefaultValue("UFCW\Claims\"), Browsable(True), System.ComponentModel.Description("Represents the application key used when accessing the registry.")> _
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)
            _APPKEY = Value
        End Set
    End Property

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents HistoryViewer As ClaimDocumentHistoryViewer
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ClaimsHistoryViewerForm))
        Me.HistoryViewer = New ClaimDocumentHistoryViewer()
        Me.SuspendLayout()
        '
        'HistoryViewer
        '
        Me.HistoryViewer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HistoryViewer.AppKey = "UFCW\Claims\Queue\"
        Me.HistoryViewer.Location = New System.Drawing.Point(4, 4)
        Me.HistoryViewer.Name = "HistoryViewer"
        Me.HistoryViewer.ShowClose = True
        Me.HistoryViewer.Size = New System.Drawing.Size(464, 296)
        Me.HistoryViewer.TabIndex = 0
        '
        'ClaimsHistoryViewerForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(472, 302)
        Me.Controls.Add(Me.HistoryViewer)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ClaimsHistoryViewerForm"
        Me.Text = "Claim(s) History"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private _ClaimID As Integer
    Private _FamilyID As Integer
    Private _RelationID As Integer
    Private _PartSSN As Integer
    Private _PatSSN As Integer
    Private _PartFName As String
    Private _PartLName As String
    Private _PatFName As String
    Private _PatLName As String
    Private _AnnotationsDT As DataTable

    Private Sub HistoryViewerForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not UFCWGeneral.SetFormPosition(Me, _APPKEY) Then Me.CenterToScreen()
    End Sub

    Private Sub HistoryViewer_Close(ByVal sender As Object) Handles HistoryViewer.Close
        Try
            Me.Close()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub RefreshForm(ByVal claimID As Integer, ByVal familyID As Integer, ByVal relationID As Integer, ByVal partSSN As Integer, ByVal patSSN As Integer, ByVal partFName As String, ByVal partLName As String, ByVal patFName As String, ByVal patLName As String, ByVal annotationsDT As DataTable)
        HistoryViewer.ClaimID = claimID
        HistoryViewer.FamilyID = familyID
        HistoryViewer.RelationID = relationID
        HistoryViewer.ParticipantSSN = partSSN
        HistoryViewer.PatientSSN = patSSN
        HistoryViewer.ParticipantFirst = partFName
        HistoryViewer.ParticipantLast = partLName
        HistoryViewer.PatientFirst = patFName
        HistoryViewer.PatientLast = patLName
        HistoryViewer.Annotations = annotationsDT

        _ClaimID = claimID
        _FamilyID = familyID
        _RelationID = relationID
        _PartSSN = partSSN
        _PatSSN = patSSN
        _PartFName = partFName
        _PartLName = partLName
        _PatFName = patFName
        _PatLName = patLName
        _AnnotationsDT = annotationsDT

        HistoryViewer.RefreshHistory()
    End Sub

    'Private Sub HistoryViewer_ShowingAnnotations(ByVal sender As Object, ByVal e As ShowingAnnotationsEvent) Handles HistoryViewer.ShowingAnnotations
    '    HistoryViewer.ClaimID = _ClaimID
    '    HistoryViewer.FamilyID = _FamilyID
    '    HistoryViewer.RelationID = _RelationID
    '    HistoryViewer.ParticipantSSN = _PartSSN
    '    HistoryViewer.PatientSSN = _PatSSN
    '    HistoryViewer.ParticipantFirst = _PartFName
    '    HistoryViewer.ParticipantLast = _PartLName
    '    HistoryViewer.PatientFirst = _PatFName
    '    HistoryViewer.PatientLast = _PatLName
    '    HistoryViewer.Annotations = e.AnnotationsTable
    '    HistoryViewer.RefreshHistory()
    'End Sub

    Private Sub HistoryViewer_SavingAnnotations(ByVal sender As Object, ByVal e As AnnotationsEvent) Handles HistoryViewer.AnnotationsModified
        Dim DR As DataRow
        Dim AddDR As DataRow
        Dim DT As DataTable

        Try
            DT = e.AnnotationsTable.GetChanges(DataRowState.Added)

            If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                For Each DR In DT.Rows
                    AddDR = _AnnotationsDT.NewRow

                    For Each DC As DataColumn In _AnnotationsDT.Columns
                        If DC.AutoIncrement = False Then
                            AddDR.Item(DC.ColumnName) = DR(DC.ColumnName)
                        End If
                    Next

                    _AnnotationsDT.Rows.Add(AddDR)
                Next

                _AnnotationsDT.AcceptChanges()
            End If

        Catch ex As Exception

            Throw
        End Try
    End Sub

    Private Sub HistoryViewerForm_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        Try
            UFCWGeneral.SaveFormPosition(Me, _APPKEY)

        Catch ex As Exception
            Throw
        Finally
            Me.HistoryViewer.Dispose()
            Me.HistoryViewer = Nothing

        End Try
    End Sub
End Class