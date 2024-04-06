Option Strict On

Imports UFCW.WCF.FileNet
Imports System.Collections.Generic
Imports System.Runtime.Serialization

Public Class Display
    Implements IDisposable

    Private Shared ReadOnly _TraceWCF As New BooleanSwitch("TraceWCF", "Trace Switch in App.Config")

    Public Event RecentDocsRefreshAvailable(sender As Object, e As RecentDocsEvent)

    Private Shared _RecentDocs As DataSet

    Private Shared _APPKEY As String = "FileNet\IDMDisplay"
    Private Shared _RecentDocsXMLFile As String = System.Windows.Forms.Application.StartupPath & "\" & "RecentIDMDocuments.xml"
    Private Shared _MaxRecentDocs As Integer = 1000

    Sub New()

        _RecentDocs = RecentDocsSerializer.GetRecentDocs(_RecentDocsXMLFile)

    End Sub

    Sub New(ByVal settingsKey As String)
        Me.New()
        _APPKEY = settingsKey
    End Sub

    Sub New(ByVal recentDocsFileName As String, ByVal maxRecentItems As Integer)
        Me.New()
        _RecentDocsXMLFile = recentDocsFileName
        _MaxRecentDocs = maxRecentItems
    End Sub

    Sub New(ByVal settingsKey As String, ByVal recentDocsFileName As String, ByVal maxRecentItems As Integer)
        Me.New()
        _APPKEY = settingsKey
        _RecentDocsXMLFile = recentDocsFileName
        _MaxRecentDocs = maxRecentItems
    End Sub

    Public Shared ReadOnly Property CurrentDocumentID() As Long?
        Get
            Dim Procs() As Process
            Dim IDMTitle As String = ""

            Procs = Process.GetProcessesByName("IDMView")

            If Procs.Length > 0 Then
                For Cnt As Integer = 0 To UBound(Procs, 1)
                    If Not Procs(Cnt).MainWindowTitle = "" AndAlso NativeMethods.IsWindowVisible(Procs(Cnt).MainWindowHandle) Then
                        IDMTitle = Microsoft.VisualBasic.Right(Procs(Cnt).MainWindowTitle, Len(Procs(0).MainWindowTitle) - InStrRev(Procs(0).MainWindowTitle, "-") - 1)
                        IDMTitle = Microsoft.VisualBasic.Left(IDMTitle, IDMTitle.Length - 1)

                        If IsNumeric(IDMTitle) = True Then
                            Return CLng(IDMTitle)
                        End If
                    End If
                Next
            End If

            Return Nothing

        End Get
    End Property

    Public Shared ReadOnly Property GetRecentDocs As DataSet
        Get
            If _RecentDocs Is Nothing Then _RecentDocs = RecentDocsSerializer.GetRecentDocs(_RecentDocsXMLFile)

            Return _RecentDocs
        End Get
    End Property

    Public Shared ReadOnly Property Visible() As Boolean
        Get
            Dim FileNetViewerProperties As FileNetViewerProperties
            Try

                FileNetViewerProperties = WCFWrapperCommon.WCFServiceHostClient.FileNetViewerProperties(WCFWrapperCommon.WCFServiceFaultException)
                Return FileNetViewerProperties.Visible

            Catch ex As Exception
                Throw
            End Try

        End Get
    End Property

    Public Shared Sub ClearImageWindow()

        If WCFWrapperCommon.WCFServiceHostClient IsNot Nothing Then WCFWrapperCommon.WCFServiceHostClient.ClearImageWindow(WCFWrapperCommon.WCFServiceFaultException)

    End Sub

    Public Shared Sub Hide()

        If WCFWrapperCommon.WCFServiceHostClient IsNot Nothing Then WCFWrapperCommon.WCFServiceHostClient.Hide(WCFWrapperCommon.WCFServiceFaultException)

    End Sub

    Public Function Display(ByVal maximID As String, ByVal fileNetUserID As String, ByVal fileNetPassword As String) As FileNetDocumentProperties

        WCFWrapperCommon.FileNetUserID = fileNetUserID.Trim
        WCFWrapperCommon.FileNetPassword = fileNetPassword.Trim

        If WCFWrapperCommon.FileNetUserID.Length = 0 OrElse WCFWrapperCommon.FileNetPassword.Length = 0 Then Throw New ApplicationException("UserID and Password are required. ")

        Return Display(maximID)

    End Function

    Public Function Display(ByVal maximID As String) As FileNetDocumentProperties

        Try

            If maximID.Trim.Length < 20 Then Throw New ApplicationException("Error: MaximID format is invalid.")

            'If you get an error using a list as a property it means the service reference config was not changed to use Generic.List
            Dim IDMDocument As FileNetDocumentProperties = WCFWrapperCommon.WCFServiceHostClient.DisplayDocumentByMaximID(WCFWrapperCommon.WCFServiceFaultException, WCFWrapperCommon.FileNetUserID, WCFWrapperCommon.FileNetPassword, maximID)

            LogDocumentToRecentFile(IDMDocument)

            Return IDMDocument

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Sub Display(ByVal documents As List(Of Long?), ByVal fileNetUserID As String, ByVal fileNetPassword As String)

        WCFWrapperCommon.FileNetUserID = fileNetUserID.Trim
        WCFWrapperCommon.FileNetPassword = fileNetPassword.Trim

        If WCFWrapperCommon.FileNetUserID.Length = 0 OrElse WCFWrapperCommon.FileNetPassword.Length = 0 Then Throw New ApplicationException("UserID and Password are required. ")

        Display(documents)

    End Sub

    Public Sub Display(ByVal documents As List(Of Long?))

        Try

            If documents.Count < 1 Then Throw New ApplicationException("Error: Document(s) is/are required to continue.")

            DisplayFNDocs(documents)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub Display(ByVal ParamArray documents() As Object)

        Dim Docs As New List(Of Long?)

        If documents Is Nothing OrElse Not documents.Any Then Throw New ApplicationException("Error: Document(s) is/are required to continue.")

        For Each Doc As Object In documents
            Docs.Add(CLng(Doc))
        Next

        DisplayFNDocs(Docs)

    End Sub
    Public Sub Display(ByVal recentDocsFile As String, ByVal ParamArray documents() As Object)

        Dim Docs As New List(Of Long?)

        If recentDocsFile IsNot Nothing AndAlso recentDocsFile.Trim.Length > 0 Then
            _RecentDocsXMLFile = recentDocsFile
        End If

        If documents Is Nothing OrElse Not documents.Any Then Throw New ApplicationException("Error: Document(s) is/are required to continue.")

        For Each Doc As Object In documents
            Docs.Add(CLng(Doc))
        Next

        DisplayFNDocs(Docs)

    End Sub
    Private Sub DisplayFNDoc(document As Long?)
        Dim IDMDocument As FileNetDocumentProperties

        Try

            'If you get an error using a list as a property it means the service reference config was not changed to use Generic.List
            IDMDocument = WCFWrapperCommon.WCFServiceHostClient.DisplayDocument(WCFWrapperCommon.WCFServiceFaultException, WCFWrapperCommon.FileNetUserID, WCFWrapperCommon.FileNetPassword, document)

            LogDocumentToRecentFile(IDMDocument)

        Catch ex As System.ServiceModel.FaultException(Of UFCW.WCF.FileNet.FileNetWCFFault)
            Throw
        Catch ex As System.ServiceModel.FaultException
            Throw
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub DisplayFNDocs(documents As List(Of Long?))

        Try

            'If you get an error using a list as a property it means the service reference config was not changed to use Generic.List
            If documents.Count = 1 Then
                DisplayFNDoc(documents(0))
            Else

                WCFWrapperCommon.WCFServiceHostClient.Display(WCFWrapperCommon.WCFServiceFaultException, WCFWrapperCommon.FileNetUserID, WCFWrapperCommon.FileNetPassword, documents)
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

#Region "Dispose"


    Dim disposed As Boolean = False

    ' Implement IDisposable.
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overridable Overloads Sub Dispose(disposing As Boolean)
        If disposed = False Then
            If disposing Then
                ' Free other state (managed objects).
                disposed = True
            End If
            ' Free your own state (unmanaged objects).
            ' Set large fields to null.
        End If
    End Sub

    Protected Overrides Sub Finalize()
        ' Simply call Dispose(False).
        Dispose(False)
    End Sub

#End Region

    Private Sub LogDocumentToRecentFile(document As FileNetDocumentProperties)

        If _RecentDocs.Tables("RecentDocs").Rows.Find(document.ID) IsNot Nothing Then
            _RecentDocs.Tables("RecentDocs").Rows.Remove(_RecentDocs.Tables("RecentDocs").Rows.Find(document.ID))
        End If

        Dim FileNetProperty As FileNetProperty = document.Properties.Find(Function(fnp As FileNetProperty) fnp.Name = "SSN")

        _RecentDocs.Tables("RecentDocs").Rows.Add(document.ID, FNProperty.NetDataContractDeSerializer(FileNetProperty.Value).ToString, Now)
        RecentDocsSerializer.SaveRecentDocs(_RecentDocs, _RecentDocsXMLFile)

        RaiseEvent RecentDocsRefreshAvailable(Me, New RecentDocsEvent(_RecentDocs))

    End Sub

    'Public Shared Sub Terminate()
    '    Try

    '        If WCFWrapperCommon.WCFServiceHostClient IsNot Nothing Then
    '            WCFWrapperCommon.DetachFromService(New WCFProcessInfo With {.ProcessID = Process.GetCurrentProcess.Id, .ProcessName = Process.GetCurrentProcess.ProcessName})
    '        End If

    '    Catch fe As FaultException When fe.Message.Contains("Terminate")
    '        'normal termination
    '    Catch ex As Exception
    '        Throw
    '    Finally
    '    End Try
    'End Sub

    'Public Sub InitializeViewer()

    '    Try

    '        If IsNothing(Common.WCFServiceHostClient) Then Throw New ApplicationException("FileNet Session required to Display Document. ")

    '        If (String.IsNullOrEmpty(Common.FileNetUserID) OrElse Common.FileNetUserID.Trim.Length = 0) OrElse Common.FileNetPassword.Length = 0 Then Throw New ApplicationException("Logon with UserID and Password to continue. ")

    '        Common.WCFServiceHostClient.InitializeViewer(Common.WCFServiceFaultException, Common.FileNetUserID, Common.FileNetPassword, ZoomModeDefault:=CType(GetSetting(_APPKEY, "IDMViewer\Size", "Fitting", "5"), idmZoomMode), WindowTop:=CInt(GetSetting(_APPKEY, "IDMViewer\Size", "Top", "100")), WindowLeft:=CInt(GetSetting(_APPKEY, "IDMViewer\Size", "Left", "100")), WindowHeight:=CInt(GetSetting(_APPKEY, "IDMViewer\Size", "Height", "100")), WindowWidth:=CInt(GetSetting(_APPKEY, "IDMViewer\Size", "Width", "100")))

    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Sub

End Class

Public Class Document
    Implements IDisposable

    Private Shared ReadOnly _TraceWCF As New BooleanSwitch("TraceWCF", "Trace Switch in App.Config")

    Private _WCFServiceFileNetDocument As FileNetDocumentProperties
    Private _Document As Long?

    Sub New()

        If WCFWrapperCommon.WCFServiceHostClient Is Nothing Then Throw New ApplicationException("FileNet Session required to display Document. ")

    End Sub

    Sub New(ByVal docID As Long)

        Me.New()

        _Document = docID

    End Sub

    Public ReadOnly Property FileNetDocument(ByVal docID As Long?) As FileNetDocumentProperties
        Get

            Return RetreiveFileNetDocumentProperties(docID)

        End Get
    End Property

    Public ReadOnly Property DocumentClass(ByVal docID As Long?) As String
        Get

            Return RetreiveFileNetDocumentProperties(docID).ClassDescription.Name

        End Get
    End Property

    Public Function UpdateSSN(ByVal docID As Long, ByVal ssn As Integer, ByVal axSecurity As String, ByVal readSecurity As String, ByVal writeSecurity As String) As Boolean

        RetreiveFileNetDocumentProperties(docID)

        Return WCFWrapperCommon.WCFServiceHostClient.UpdateSSN(WCFWrapperCommon.WCFServiceFaultException, WCFWrapperCommon.FileNetUserID, WCFWrapperCommon.FileNetPassword, docID, ssn, axSecurity, readSecurity, writeSecurity)

    End Function

    Private Function RetreiveFileNetDocumentProperties(ByVal docID As Long?) As FileNetDocumentProperties

        Dim WCFServiceFileNetDocument As FileNetDocumentProperties
        Dim WCFServiceFileNetProperties As List(Of FileNetProperty)

        Try

            If (String.IsNullOrEmpty(WCFWrapperCommon.FileNetUserID) OrElse WCFWrapperCommon.FileNetUserID.Trim.Length = 0) OrElse WCFWrapperCommon.FileNetPassword.Length = 0 Then Throw New ApplicationException("Logon with UserID and Password to continue. ")

            If docID Is Nothing Then
                Throw New ApplicationException("Error: Document ID is required.")
            ElseIf _Document <> docID OrElse _WCFServiceFileNetDocument Is Nothing Then

                'refresh from IDM if different document or 1st time accessed
                WCFServiceFileNetDocument = WCFWrapperCommon.WCFServiceHostClient.FileNetDocument(WCFWrapperCommon.WCFServiceFaultException, WCFWrapperCommon.FileNetUserID, WCFWrapperCommon.FileNetPassword, CType(docID, Long))

                WCFServiceFileNetProperties = New List(Of FileNetProperty)

                For Each FileNetProperty As FileNetProperty In WCFServiceFileNetDocument.Properties
                    Dim FNProperty As New FNProperty With {
                        .Name = FileNetProperty.Name,
                        .Label = FileNetProperty.Label,
                        .TypeID = FileNetProperty.TypeID,
                        .PropertyDescription = FileNetProperty.PropertyDescription,
                        .Value = NetDataContractDeSerializer(FileNetProperty.Value)
                    }

                    WCFServiceFileNetProperties.Add(FNProperty)
                Next

                WCFServiceFileNetDocument.Properties = WCFServiceFileNetProperties

                _WCFServiceFileNetDocument = WCFServiceFileNetDocument
            End If

            Return _WCFServiceFileNetDocument

        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Shared Function NetDataContractDeSerializer(ByVal fnObject As FileNetObject) As Object
        Dim NetDataContractSerializer As NetDataContractSerializer
        Try

            If fnObject Is Nothing Then Return fnObject

            NetDataContractSerializer = New NetDataContractSerializer
            fnObject.Item.Position = 0

            Return NetDataContractSerializer.Deserialize(fnObject.Item)

        Catch ex As Exception
            Throw
        Finally
            fnObject.Item.Close()
        End Try

    End Function
#Region "Dispose"

    Dim disposed As Boolean = False

    ' Implement IDisposable.
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overridable Overloads Sub Dispose(disposing As Boolean)
        If disposed = False Then
            If disposing Then
                ' Free other state (managed objects).
                disposed = True
            End If
            ' Free your own state (unmanaged objects).
            ' Set large fields to null.
        End If
    End Sub

    Protected Overrides Sub Finalize()
        ' Simply call Dispose(False).
        Dispose(False)
    End Sub

#End Region

End Class
Public Class FNProperty
    Inherits FileNetProperty

    Private Shared ReadOnly _TraceWCF As New BooleanSwitch("TraceWCF", "Trace Switch in App.Config")

    Shadows Property Value As Object

    Public Shared Function NetDataContractDeSerializer(ByVal FNObject As FileNetObject) As Object
        Dim NetDataContractSerializer As NetDataContractSerializer
        Try

            If FNObject Is Nothing Then Return FNObject

            NetDataContractSerializer = New NetDataContractSerializer
            FNObject.Item.Position = 0

            Return NetDataContractSerializer.Deserialize(FNObject.Item)

        Catch ex As Exception
            Throw
        Finally
            FNObject.Item.Close()
        End Try

    End Function

End Class

