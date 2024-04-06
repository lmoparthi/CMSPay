Public Class Queue
    Implements IDisposable

    Private Shared _TraceWCF As New BooleanSwitch("TraceWCF", "Trace Switch in App.Config")

    'Public WriteOnly Property LibraryName() As String
    '    Set(ByVal value As String)
    '        _LibraryName = Value
    '    End Set
    'End Property

    'Public WriteOnly Property WorkSpaceName() As String
    '    Set(ByVal value As String)
    '        _WorkSpaceName = Value
    '    End Set
    'End Property

    'Public WriteOnly Property QueueName() As String
    '    Set(ByVal value As String)
    '        _QueueName = Value
    '    End Set
    'End Property

    'Sub New()

    '    _FNErrManager = CType(CreateObject("IDMError.ErrorManager"), IDMError.ErrorManager)

    'End Sub

    'Sub New(ByVal LibraryName As String)

    '    _LibraryName = LibraryName
    '    _FNErrManager = CType(CreateObject("IDMError.ErrorManager"), IDMError.ErrorManager)

    'End Sub

    'Sub New(ByVal LibraryName As String, ByVal WorkSpaceName As String)

    '    _LibraryName = LibraryName
    '    _WorkSpaceName = WorkSpaceName

    'End Sub

    'Sub New(ByVal LibraryName As String, ByVal WorkSpaceName As String, ByVal QueueName As String)

    '    _LibraryName = LibraryName
    '    _WorkSpaceName = WorkSpaceName
    '    _QueueName = QueueName

    'End Sub

    'Private Function LogonLibrary() As IDMObjects.Library

    '    Try

    '        For Each oLib As IDMObjects.Library In _Hood.Libraries
    '            If oLib.Label = _LibraryName Then
    '                If oLib.GetState(IDMObjects.idmLibraryState.idmLibraryLoggedOn) Then
    '                Else
    '                    oLib.Logon("SysAdmin", "sysadmin", , IDMObjects.idmLibraryLogon.idmLogonOptNoUI)
    '                End If
    '                Return oLib
    '            End If
    '        Next oLib

    '    Catch ex As Exception
    '        Throw

    '    End Try

    'End Function
    'Private Function GetWorkspace() As IDMObjects.QueueWorkspace

    '    Try

    '        If IsNothing(_Library) Then
    '            _Library = LogonLibrary()
    '        End If

    '        Return CType(_Library.GetObject(IDMObjects.idmObjectType.idmObjTypeQueueWorkspace, _WorkSpaceName.ToString), IDMObjects.QueueWorkspace)

    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Function

    'Private Function OpenQueue(ByVal QName As String) As IDMObjects.Queue

    '    Try
    '        If IsNothing(_Workspace) Then
    '            _Workspace = GetWorkspace()
    '        End If

    '        Return _Workspace.GetQueue(QName.ToString)

    '    Catch ex As Exception
    '        Throw
    '    End Try

    'End Function
    'Public Function CountQueueEntries(ByVal WorkSpaceName As String, ByVal QueueName As String, ByVal SSN As Integer, ByVal MaximID As String, ByVal DocID As Integer) As Integer

    '    _WorkSpaceName = WorkSpaceName

    '    Dim oQueueQuerySpec As IDMObjects.QueueQuerySpecification ' the queue query spec object
    '    Dim oQueue As IDMObjects.Queue
    '    Dim oProperties As IDMObjects.Properties

    '    Dim oProperty As IDMObjects.Property


    '    Try

    '        oQueue = OpenQueue(QueueName)
    '        oQueueQuerySpec = oQueue.CreateQuerySpecification()
    '        oProperties = oQueueQuerySpec.Filters

    '        For Each oProperty In oProperties
    '            Select Case UCase(oProperty.Name)
    '                Case "DOCID"
    '                    oProperty.Value = DocID.ToString
    '                Case "MAXIMID"
    '                    oProperty.Value = MaximID.ToString
    '                Case "SSN"
    '                    oProperty.Value = SSN.ToString

    '            End Select

    '        Next

    '        Return oQueueQuerySpec.Count

    '    Catch ex As Exception

    '    End Try

    'End Function

    'Public Sub CreateQueueEntry(ByVal WorkSpaceName As String, ByVal QueueName As String, ByVal SSN As Integer, ByVal MaximID As String, ByVal DocID As Integer)

    '    _QueueName = QueueName
    '    _WorkSpaceName = WorkSpaceName
    '    CreateDistQueueEntry(SSN, MaximID, DocID)

    'End Sub
    'Public Sub CreateQueueEntry(ByVal QueueName As String, ByVal SSN As Integer, ByVal MaximID As String, ByVal DocID As Integer)

    '    _QueueName = QueueName
    '    CreateDistQueueEntry(SSN, MaximID, DocID)

    'End Sub

    'Public Sub CreateDistQueueEntry(ByVal SSN As Integer, ByVal MaximID As String, ByVal DocID As Integer)

    '    Dim oProperty As IDMObjects.Property
    '    Dim oQueueEntry As IDMObjects.QueueEntry

    '    Try

    '        If IsNothing(_Queue) Then
    '            _Queue = OpenQueue(_QueueName)
    '        End If

    '        oQueueEntry = _Queue.CreateEmptyEntry

    '        For Each oProperty In oQueueEntry.Properties
    '            Select Case oProperty.Name
    '                Case "Docid"
    '                    oProperty.Value = DocID.ToString
    '                Case "MaximID"
    '                    oProperty.Value = MaximID.ToString
    '                Case "SSN"
    '                    oProperty.Value = SSN.ToString
    '                Case "F_Priority"
    '                    oProperty.Value = "5"
    '                Case "F_Busy", "F_Delay", "F_EntryTime", "F_GroupID", "F_TimeOut", "F_UserID"
    '                    oProperty.Value = Nothing
    '                Case Else
    '                    oProperty.Value = DBNull.Value
    '            End Select

    '        Next

    '        oQueueEntry.Insert()


    '    Catch ex As Exception

    '        Throw
    '    Finally

    '        oQueueEntry = Nothing

    '    End Try


    'End Sub

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
