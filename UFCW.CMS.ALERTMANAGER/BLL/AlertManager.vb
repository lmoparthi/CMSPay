Imports System.Data
Imports System.Threading

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.AlertManager
''' Class	 : CMS.AlertManager.AlertManager
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class manages alerts
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	5/31/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()>
Public Class AlertManagerCollection
    Inherits CollectionBase
    Implements ICloneable, IDisposable

    Private Shared _TraceCloning As New TraceSwitch("TraceCloning", "Trace Switch in App.Config", "0")
    Private Shared _TraceParallel As New TraceSwitch("TraceParallel", "Parallel Trace Switch in App.Config", "0")

    Private _ClassGuid As System.Guid = Guid.NewGuid()
    Private _AlertManagerDataTable As DataTable
    Private _SuspendLayout As Boolean = False
    Private _Disposed As Boolean
    <NonSerialized()> Public Event AlertsChanged()

#Region "Constructors"

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Default constructor
    ' </summary>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	5/31/2006	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Sub New()
        MyBase.New()

        Try

            _AlertManagerDataTable = New DataTable("Alerts")
            _AlertManagerDataTable.Columns.Add("Message", System.Type.GetType("System.String"))
            _AlertManagerDataTable.Columns.Add("LineNumber", System.Type.GetType("System.Int16"))
            _AlertManagerDataTable.Columns.Add("Category", System.Type.GetType("System.Object"))
            _AlertManagerDataTable.Columns.Add("Severity", System.Type.GetType("System.Int32"))
            _AlertManagerDataTable.Columns.Add("Tag", System.Type.GetType("System.Object"))

        Catch ex As Exception
            Throw
        End Try

    End Sub
#End Region

#Region "Property"

    Public Property SuspendLayout As Boolean

        Get
            Return _SuspendLayout
        End Get

        Set(value As Boolean)
            _SuspendLayout = value

#If TRACE Then
            If CInt(_TraceParallel.Level) > 0 Then Trace.WriteLine(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & " AlertManager.SuspendLayout: " & _SuspendLayout.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceParallel" & vbTab)
#End If

            If _SuspendLayout = False Then RaiseEvent AlertsChanged()

        End Set

    End Property

    Public Property AlertManagerDataTable As DataTable

        Get
            Return _AlertManagerDataTable
        End Get
        Set(value As DataTable)
            _AlertManagerDataTable = value

        End Set
    End Property

#End Region

#Region "Table Specific Functions/Methods"

    Public Overloads Sub Clear()
        Try

            MyBase.Clear()
            ClearAlertRows()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub AcceptChanges()
        _AlertManagerDataTable.AcceptChanges()
    End Sub

    Public Function GetChanges() As DataTable
        Return _AlertManagerDataTable.GetChanges()
    End Function

    Public Sub ClearAlertRows()
        Try

            _AlertManagerDataTable.Clear()
            _AlertManagerDataTable.AcceptChanges()

            If Not _SuspendLayout Then RaiseEvent AlertsChanged()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub ClearDetailAlertRows()
        Dim AlertDV As DataView

        Try

            AlertDV = New DataView(_AlertManagerDataTable, "LineNumber > 0", "", DataViewRowState.CurrentRows)

            For Each AlertDataRowView As DataRowView In AlertDV
                AlertDataRowView.Row.Delete()
            Next

            _AlertManagerDataTable.AcceptChanges()

            If Not _SuspendLayout Then RaiseEvent AlertsChanged()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub ClearHeaderAlertRows()
        Dim AlertDV As DataView

        Try

            AlertDV = New DataView(_AlertManagerDataTable, "LineNumber = 0", "", DataViewRowState.CurrentRows)

            For Each AlertDataRowView As DataRowView In AlertDV
                AlertDataRowView.Row.Delete()
            Next

            _AlertManagerDataTable.AcceptChanges()

            If Not _SuspendLayout Then RaiseEvent AlertsChanged()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub AddAlertRow(ByVal severity As SeverityTypes, lineNumber As Integer, alertMessage As String, category As CategoryTypes, tag As Object)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Builds & Adds an alert
        ' </summary>
        ' <param name="alrt"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/31/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            AddAlertRow(New Object() {alertMessage, lineNumber, category, severity, tag})

            If Not _SuspendLayout Then RaiseEvent AlertsChanged()

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub AddAlertRow(ByVal alertManagerRowFields As Object())
        'checks if alert exists and adds if missing
        ' 0 = Message
        ' 1 = Line
        ' 2 = Category
        ' 3 = Priority
        ' 4 = Tags (Objects to Highlight)

        Dim AlertDV As DataView
        Dim CharsToTrim() As Char = {" "c, "'"c}


        Try

            alertManagerRowFields(0) = alertManagerRowFields(0).ToString.Trim(CharsToTrim)

            AlertDV = New DataView(_AlertManagerDataTable, "LineNumber = " & alertManagerRowFields(1).ToString & " And Message = '" & alertManagerRowFields(0).ToString.Replace("'", "''") & "'", "LineNumber, Category, Message", DataViewRowState.CurrentRows)

            If AlertDV.Count < 1 Then   'if there are alerts
                _AlertManagerDataTable.Rows.Add(alertManagerRowFields)
            End If

            If Not _SuspendLayout Then RaiseEvent AlertsChanged()

        Catch ex As Exception
            Throw
        Finally
            If AlertDV IsNot Nothing Then
                AlertDV.Dispose()
            End If
            AlertDV = Nothing
        End Try

    End Sub

    Public Sub DeleteAlertRowsByLine(ByVal lineNumber As Short)

        Dim AlertDV As DataView

        Try

            AlertDV = New DataView(_AlertManagerDataTable, "LineNumber = " & lineNumber.ToString, "", DataViewRowState.CurrentRows)

            For Each AlertDataRowView As DataRowView In AlertDV
                AlertDataRowView.Row.Delete()
            Next

            If Not _SuspendLayout Then RaiseEvent AlertsChanged()

        Catch ex As Exception
            Throw
        Finally
            If AlertDV IsNot Nothing Then
                AlertDV.Dispose()
            End If
            AlertDV = Nothing
        End Try

    End Sub

    Public Sub DeleteAlertRowsByMessageAndLine(ByVal alertManagerMessage As String, ByVal lineNumber As Integer)

        Dim AlertDV As DataView
        Dim CharsToTrim() As Char = {" "c, "'"c}

        Try
            alertManagerMessage = alertManagerMessage.Trim(CharsToTrim)

            AlertDV = New DataView(_AlertManagerDataTable, "LineNumber = " & lineNumber.ToString & " And Category = 'Detail' And Message = '" & alertManagerMessage.Replace("'", "''") & "'", "", DataViewRowState.CurrentRows)

            For Each AlertDataRowView As DataRowView In AlertDV
                AlertDataRowView.Row.Delete()
            Next

            If Not _SuspendLayout Then RaiseEvent AlertsChanged()

        Catch ex As Exception
            Throw
        Finally
            If AlertDV IsNot Nothing Then
                AlertDV.Dispose()
            End If
            AlertDV = Nothing
        End Try

    End Sub

    Public Sub DeleteAlertRowsByCategoryAndLine(ByVal alertCategory As String, ByVal lineNumber As Integer)

        Dim AlertDV As DataView
        Dim CharsToTrim() As Char = {" "c, "'"c}

        Try
            alertCategory = alertCategory.Trim(CharsToTrim)

            AlertDV = New DataView(_AlertManagerDataTable, "LineNumber = " & lineNumber.ToString & " And Category = '" & alertCategory.Replace("'", "''") & "'", "", DataViewRowState.CurrentRows)

            For Each AlertDataRowView As DataRowView In AlertDV
                AlertDataRowView.Row.Delete()
            Next

            If Not _SuspendLayout Then RaiseEvent AlertsChanged()

        Catch ex As Exception
            Throw
        Finally
            If AlertDV IsNot Nothing Then
                AlertDV.Dispose()
            End If
            AlertDV = Nothing
        End Try

    End Sub

    Public Sub DeleteAlertRowsByMessage(ByVal alertManagerMessage As String)

        Dim AlertDV As DataView
        Dim CharsToTrim() As Char = {" "c, "'"c}

        Try
            alertManagerMessage = alertManagerMessage.Trim(CharsToTrim)

            AlertDV = New DataView(_AlertManagerDataTable, "Message = '" & alertManagerMessage.Replace("'", "''") & "'", "", DataViewRowState.CurrentRows)

            For Each AlertDataRowView As DataRowView In AlertDV
                AlertDataRowView.Row.Delete()
            Next

            If Not _SuspendLayout Then RaiseEvent AlertsChanged()

        Catch ex As Exception
            Throw
        Finally
            If AlertDV IsNot Nothing Then
                AlertDV.Dispose()
            End If
            AlertDV = Nothing
        End Try

    End Sub

    Public Sub DeleteAlertRowsLikeMessageAndLine(ByVal alertManagerMessage As String, ByVal lineNumber As Integer, Optional category As String = "Detail")

        Dim AlertDV As DataView
        Dim CharsToTrim() As Char = {" "c, "'"c}

        Try
            alertManagerMessage = alertManagerMessage.Trim(CharsToTrim)

            AlertDV = New DataView(_AlertManagerDataTable, "LineNumber = " & lineNumber.ToString & " And Category = '" & category & "' And Message LIKE '%" & alertManagerMessage.Replace("'", "''") & "%'", "", DataViewRowState.CurrentRows)

            For Each AlertDataRowView As DataRowView In AlertDV
                AlertDataRowView.Row.Delete()
            Next

            If Not _SuspendLayout Then RaiseEvent AlertsChanged()

        Catch ex As Exception
            Throw
        Finally
            If AlertDV IsNot Nothing Then
                AlertDV.Dispose()
            End If
            AlertDV = Nothing
        End Try

    End Sub

    Public Sub DeleteAlertRowsByCategory(ByVal alertCategory As String)

        Dim AlertDV As DataView
        Dim CharsToTrim() As Char = {" "c, "'"c}

        Try

            alertCategory = alertCategory.Trim(CharsToTrim)

            AlertDV = New DataView(_AlertManagerDataTable, "Category = '" & alertCategory.Replace("'", "''") & "'", "", DataViewRowState.CurrentRows)

            For Each AlertDataRowView As DataRowView In AlertDV
                AlertDataRowView.Row.Delete()
            Next

            If Not _SuspendLayout Then RaiseEvent AlertsChanged()

        Catch ex As Exception
            Throw
        Finally
            If AlertDV IsNot Nothing Then
                AlertDV.Dispose()
            End If
            AlertDV = Nothing
        End Try

    End Sub

    Sub DeleteAllAlertRowsLikeMessage(ByVal alertManagerMessage As String)
        Dim AlertDV As DataView
        Dim CharsToTrim() As Char = {" "c, "'"c}

        Try

            alertManagerMessage = alertManagerMessage.Trim(CharsToTrim)

            AlertDV = New DataView(_AlertManagerDataTable, "Message LIKE '%" & alertManagerMessage.Replace("'", "''") & "%'", "", DataViewRowState.CurrentRows)

            For Each AlertDataRowView As DataRowView In AlertDV
                AlertDataRowView.Row.Delete()
            Next

            If Not _SuspendLayout Then RaiseEvent AlertsChanged()

        Catch ex As Exception
            Throw
        Finally
            If AlertDV IsNot Nothing Then
                AlertDV.Dispose()
            End If
            AlertDV = Nothing
        End Try

    End Sub

    Public Function IsAlertRowAlreadyPresent(ByVal alertManagerMessage As String, ByVal lineNumber As Integer) As Boolean

        Dim AlertDV As DataView
        Dim CharsToTrim() As Char = {" "c, "'"c}

        Try
            alertManagerMessage = alertManagerMessage.Trim(CharsToTrim)

            AlertDV = New DataView(_AlertManagerDataTable, "LineNumber = " & lineNumber.ToString & " And Category = 'Detail' And Message = '" & alertManagerMessage.Replace("'", "''") & "'", "LineNumber, Category, Message", DataViewRowState.CurrentRows)

            If AlertDV.Count > 0 Then   'if there are alerts
                Return True
            End If

            Return False

        Catch ex As Exception
            Throw
        Finally
            If AlertDV IsNot Nothing Then
                AlertDV.Dispose()
            End If
            AlertDV = Nothing
        End Try

    End Function
#End Region

#Region "List Specific Methods"

    Public Sub AddAlert(ByVal severity As SeverityTypes, lineNumber As Short, alertMessage As String, category As CategoryTypes, tag As Object)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Builds & Adds an alert
        ' </summary>
        ' <param name="alrt"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/31/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim Alert As Alert

        Try

            Alert = New Alert
            Alert.Category = category
            Alert.AlertMessage = alertMessage
            Alert.LineNumber = lineNumber
            Alert.SetSeverity(severity)
            Alert.Tag = tag

            Me.List.Add(Alert)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub AddAlert(ByVal alert As Alert)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Adds an alert
        ' </summary>
        ' <param name="alrt"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/31/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Me.List.Add(alert)
    End Sub

    Public Sub ClearHeaderAlerts()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Clears all the alerts for the header of the claim
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/31/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            For Each Alert As Alert In Me.List
                If Alert.LineNumber < 1 Then
                    Me.List.Remove(Alert)
                End If
            Next

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub ClearDetailAlerts()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Clears all the alerts for the line items of the claim
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/31/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            For Each TheAlert As Alert In Me.List
                If TheAlert.LineNumber >= 1 Then
                    Me.List.Remove(TheAlert)
                End If
            Next

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function IsAlertAlreadyPresent(ByVal message As String, ByVal lineNumber As Short) As Boolean
        Try

            For Each TheAlert As Alert In Me.List
                If TheAlert.AlertMessage.IndexOf(message) >= 0 AndAlso lineNumber = TheAlert.LineNumber Then
                    Return True
                End If
            Next

            Return False

        Catch ex As Exception
            Throw
        End Try

    End Function
#End Region

#Region "Clone"
    Public Function DeepCopy() As AlertManagerCollection

        Dim AlertManagerClone As AlertManagerCollection
        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            AlertManagerClone = Me.ShallowCopy

            AlertManagerClone.AlertManagerDataTable.BeginLoadData()
            AlertManagerClone.AlertManagerDataTable.Load(Me.AlertManagerDataTable.CreateDataReader)
            AlertManagerClone.AlertManagerDataTable.EndLoadData()

            Return AlertManagerClone

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
#End If
            AlertManagerClone = Nothing
        End Try

    End Function

    Public Function ShallowCopy() As AlertManagerCollection

        Dim BeginTime As Date = UFCWGeneral.NowDate
        Dim AlertManagerClone As AlertManagerCollection

        Try
            AlertManagerClone = CType(Me.MemberwiseClone, AlertManagerCollection)
            AlertManagerClone.InnerList.Clear()
            AlertManagerClone.AlertManagerDataTable = Nothing

            Return AlertManagerClone

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
#End If
        End Try
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone

        Dim BeginTime As Date = UFCWGeneral.NowDate

        Try

            Return DirectCast(CloneHelper.Clone(Me), AlertManagerCollection)

        Catch ex As Exception
            Throw
        Finally
#If TRACE Then
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
#End If
        End Try

    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not _Disposed Then
            If disposing Then
                MyBase.Clear()
                _AlertManagerDataTable.Dispose()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
            ' TODO: set large fields to null
            _Disposed = True
        End If
    End Sub

    ' ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
    ' Protected Overrides Sub Finalize()
    '     ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

#Region "Clean Up"



#End Region

End Class