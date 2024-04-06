Imports System.Windows.Forms
Imports System.Threading

Public Class DataGridTextBoxDecimal
    Inherits DataGridTextBoxColumn

    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")
    Private Shared _TraceBinding As New TraceSwitch("TraceBinding", "Trace Switch in App.Config", "0")

    Public Event TextBoxCellFormat(ByVal Column As DataGridTextBoxColumn, ByVal RowNum As Integer, ByRef Value As Object, ByRef NewFormat As String)

#Region " Component Designer generated code "

    Public Sub New(ByVal container As System.ComponentModel.IContainer)
        MyClass.New()

        'Required for Windows.Forms Class Composition Designer support
        container.Add(Me)
    End Sub

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        AddHandler MyBase.TextBox.Validating, AddressOf Decimal_Validating
    End Sub

    'Component overrides dispose to clean up the component list.
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
    End Sub

#End Region

    Private Sub Decimal_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs)

        ' Do not write data if not editing.
        e = OptionalDecimal(sender, e)

        If _TraceBinding.TraceInfo Then Trace.WriteLine(Now.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame().GetMethod.ToString & ": " & CType(sender, TextBox).Name & " Proposed: " & CType(sender, TextBox).Text.ToString & " Cancel: " & e.Cancel.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""))

    End Sub

    Private Function OptionalDecimal(sender As Object, e As System.ComponentModel.CancelEventArgs, Optional required As Boolean = False) As System.ComponentModel.CancelEventArgs

        Try

            If Not String.IsNullOrEmpty(CType(sender, TextBox).Text.Trim) AndAlso Not IsDecimal(CType(sender, TextBox).Text.Trim) Then
                e.Cancel = True 'this will cancel the Validated event
                MessageBox.Show("Invalid Quantity Specified", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                CType(sender, TextBox).SelectAll()
            End If

            Return e

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

    Protected Overrides Function GetColumnValueAtRow(ByVal CM As CurrencyManager, ByVal RowNum As Integer) As Object
        '
        ' Get data from the underlying record and format for display.
        '
        Dim CellValue As Object = MyBase.GetColumnValueAtRow(CM, RowNum)

        If CellValue.GetType Is GetType(DBNull) Then
            Return ""                         ' String to display for DBNull.
        Else
            ' CDec on next statement will throw an exception if this
            ' column style is bound to a column containing non-numeric data.
            Dim Temp As Decimal = CDec(CellValue)
            If Temp = 0 Then
                Return ""                         ' String to display for 0
            ElseIf Temp >= 0 Then
                Return Temp.ToString("0.00")             ' positive number
            Else
                Return (-Temp).ToString("0.00")    ' negative number
            End If
        End If

    End Function
    Protected Overrides Function Commit(ByVal CM As CurrencyManager, ByVal RowNum As Integer) As Boolean
        '
        ' Parse the data and write to underlying record.
        '
        Dim TextBox As DataGridTextBox

        Try

            TextBox = CType(Me.TextBox, DataGridTextBox)

            ' Do not write data if not editing.
            If TextBox.IsInEditOrNavigateMode Then Return True

            If TextBox.Text = "" Then   ' in this example, "" maps to DBNull
                SetColumnValueAtRow(CM, RowNum, DBNull.Value)
            ElseIf IsDecimal(TextBox.Text.Trim) Then
                SetColumnValueAtRow(CM, RowNum, Decimal.Parse(TextBox.Text))   ' Write new value.
            Else
                Return False    ' invalid value for database

            End If

            Me.EndEdit()   ' Let the DataGrid know that processing is completed.

            Return True    ' success

        Catch ex As Exception
        Finally
            If _TraceBinding.TraceInfo Then Trace.WriteLine(Now.ToString("HH:mm:ss.fffffff") & " : Thread " & Thread.CurrentThread.ManagedThreadId.ToString & vbTab & New StackFrame().GetMethod.ToString & ": " & CType(Me.TextBox, DataGridTextBox).Name & " Proposed: " & CType(Me.TextBox, DataGridTextBox).Text.ToString & If(_TraceBinding.TraceVerbose, vbCrLf & Environment.StackTrace & vbCrLf, ""))

        End Try

    End Function
End Class