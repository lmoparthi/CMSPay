Imports DocumentFormat.OpenXml.InkML
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ComponentModel

Public Class QueueHistory
    Inherits System.Windows.Forms.Form

    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _DocClass As String
    Private _ClaimID As Integer
    Private _FamilyID As Integer
    Private _RelationID As Integer
    Private _PartSSN As Integer
    Private _PatSSN As Integer
    Private _PartFName As String
    Private _PartLName As String
    Private _PatFName As String
    Private _PatLName As String

    Private _APPKEY As String = "UFCW\Claims\"

#Region " Windows Form Designer generated code "
    Public Sub New(ByVal docClass As String, ByVal claimID As Integer, ByVal familyID As Integer, ByVal relationID As Integer, ByVal partSSN As Integer, ByVal patSSN As Integer, ByVal partFName As String, ByVal partLName As String, ByVal patFName As String, ByVal patLName As String, ByVal annotationDT As DataTable)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        _DocClass = docClass
        _ClaimID = claimID
        _FamilyID = familyID
        _RelationID = relationID
        _PartSSN = partSSN
        _PatSSN = patSSN
        _PartFName = partFName
        _PartLName = partLName
        _PatFName = patFName
        _PatLName = patLName

        Dim DV As New DataView(annotationDT, "", "", DataViewRowState.CurrentRows)
        If DV.Count > 0 Then
            For Cnt As Integer = 0 To DV.Count - 1
                AnnotationsDataSet.ANNOTATIONS.Rows.Add(DV(Cnt).Row.ItemArray)
            Next

            AnnotationsDataSet.ANNOTATIONS.AcceptChanges()
        End If

        Me.Text = "History For " & docClass & " Claim " & claimID
    End Sub
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
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
    Friend WithEvents QueueHistoryViewer As ClaimDocumentHistoryViewer
    Friend WithEvents AnnotationsDataSet As AnnotationsDataSet
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim Resources As New System.ComponentModel.ComponentResourceManager(GetType(QueueHistory))
        Me.AnnotationsDataSet = New AnnotationsDataSet
        Me.QueueHistoryViewer = New ClaimDocumentHistoryViewer
        CType(Me.AnnotationsDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AnnotationsDataSet
        '
        Me.AnnotationsDataSet.DataSetName = "AnnotationsDataSet"
        Me.AnnotationsDataSet.Locale = New System.Globalization.CultureInfo("en-US")
        Me.AnnotationsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'QueueHistoryViewer
        '
        Me.QueueHistoryViewer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.QueueHistoryViewer.AppKey = "UFCW\Claims\Queue\"
        Me.QueueHistoryViewer.Location = New System.Drawing.Point(8, 8)
        Me.QueueHistoryViewer.Name = "QueueHistoryViewer"
        Me.QueueHistoryViewer.ShowClose = True
        Me.QueueHistoryViewer.Size = New System.Drawing.Size(488, 376)
        Me.QueueHistoryViewer.TabIndex = 0
        '
        'QueueHistory
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(504, 390)
        Me.Controls.Add(Me.QueueHistoryViewer)
        Me.Icon = CType(Resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "QueueHistory"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "History for Claim 0"
        CType(Me.AnnotationsDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub QueueHistory_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub QueueHistory_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            SetSettings()

            QueueHistoryViewer.ClaimID = _ClaimID
            QueueHistoryViewer.FamilyID = _FamilyID
            QueueHistoryViewer.RelationID = _RelationID
            QueueHistoryViewer.ParticipantSSN = _PartSSN
            QueueHistoryViewer.PatientSSN = _PatSSN
            QueueHistoryViewer.ParticipantFirst = _PartFName
            QueueHistoryViewer.ParticipantLast = _PartLName
            QueueHistoryViewer.PatientFirst = _PatFName
            QueueHistoryViewer.PatientLast = _PatLName
            QueueHistoryViewer.Annotations = AnnotationsDataSet.ANNOTATIONS
            QueueHistoryViewer.RefreshHistory()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub QueueHistory_FormClosing(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.FormClosing
        Try
            SaveSettings()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub QueueHistoryViewer_Close(ByVal sender As Object) Handles QueueHistoryViewer.Close
        Try
            Me.Close()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub QueueHistoryViewer_SavingAnnotations(ByVal sender As Object, ByVal e As AnnotationsEvent) Handles QueueHistoryViewer.AnnotationsModified

        Dim AddDR As DataRow
        Dim DT As DataTable

        Try
            DT = e.AnnotationsTable.GetChanges(DataRowState.Added)

            If DT IsNot Nothing AndAlso DT.Rows.Count > 0 Then
                For Each DR As DataRow In DT.Rows
                    AddDR = AnnotationsDataSet.ANNOTATIONS.NewRow

                    For Each DC As DataColumn In AnnotationsDataSet.ANNOTATIONS.Columns
                        If DC.AutoIncrement = False Then
                            AddDR.Item(DC.ColumnName) = DR(DC.ColumnName)
                        End If
                    Next

                    AnnotationsDataSet.ANNOTATIONS.Rows.Add(AddDR)
                Next

                AnnotationsDataSet.ANNOTATIONS.AcceptChanges()
            End If

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