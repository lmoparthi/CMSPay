Imports System.Windows.Forms

Public Class DataGridFormatTextBoxColumn
    Inherits System.Windows.Forms.DataGridTextBoxColumn

    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Public Event ApplyCellFormat(ByVal column As DataGridFormatTextBoxColumn, ByVal rowNum As Integer, ByRef value As Object, ByRef newFormat As String)

#Region " Component Designer generated code "

    Public Sub New(ByVal Container As System.ComponentModel.IContainer)
        MyClass.New()

        'Required for Windows.Forms Class Composition Designer support
        Container.Add(Me)
    End Sub

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Component overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
    End Sub

#End Region
    Protected Overrides Function GetColumnValueAtRow(ByVal cm As CurrencyManager, ByVal rowNum As Integer) As Object
        '
        ' Get data from the underlying record and format for display.
        '
        Dim NewFormat As String = ""
        Dim CellValue As Object = MyBase.GetColumnValueAtRow(cm, RowNum)

        RaiseEvent ApplyCellFormat(Me, RowNum, CellValue, NewFormat)

        Try

            If NewFormat.Length > 0 Then

                Return String.Format(NewFormat, CellValue)
            Else
                Return CellValue
            End If

        Catch
            Return CellValue    ' Exit on error and display old "good" value.
        End Try
    End Function
End Class