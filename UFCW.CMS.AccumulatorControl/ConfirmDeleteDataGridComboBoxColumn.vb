Option Strict On
Option Explicit On

Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Data.Common
Imports System.Data.OleDb
Imports System.Drawing
Imports System.Windows.Forms

Public Delegate Sub EnableCellEventHandler(ByVal sender As Object, ByVal e As DataGridEnableEventArgs)

' Step 1. Derive a custom column style from DataGridTextBoxColumn
'	a) add a ComboBox member
'  b) track when the combobox has focus in Enter and Leave events
'  c) override Edit to allow the ComboBox to replace the TextBox
'  d) override Commit to save the changed data
Public Class ConfirmDeleteDataGridComboBoxColumn
    Inherits DataGridTextBoxColumn

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _RowCount As Integer = -1
    Private _IsEditing As Boolean = False

    Private _Source As System.Windows.Forms.CurrencyManager
    Private _RowNum As Integer
    Private _Col As Integer
    Private _ColumnComboBox As NoKeyUpCombo

    Public Event CheckCellEnabled As EnableCellEventHandler

    Public ReadOnly Property ColumnComboBox As NoKeyUpCombo
        Get
            ColumnComboBox = _ColumnComboBox
        End Get
    End Property

    Public Sub New()

        _ColumnComboBox = New NoKeyUpCombo
        _ColumnComboBox.DropDownStyle = ComboBoxStyle.DropDownList

        AddHandler _ColumnComboBox.Leave, AddressOf LeaveComboBox
        AddHandler _ColumnComboBox.SelectionChangeCommitted, AddressOf ComboStartEditing

    End Sub 'New

    Private Sub HandleScroll(ByVal sender As Object, ByVal e As EventArgs)
        If _ColumnComboBox.Visible Then
            _ColumnComboBox.Hide()
        End If
    End Sub 'HandleScroll

    Private Sub ComboStartEditing(ByVal sender As Object, ByVal e As EventArgs)
        _IsEditing = True
        MyBase.ColumnStartedEditing(CType(sender, Control))
    End Sub 'ComboMadeCurrent

    Private Sub LeaveComboBox(ByVal sender As Object, ByVal e As EventArgs)
        Try

            If _IsEditing Then
                SetColumnValueAtRow(_Source, _RowNum, _ColumnComboBox.Text)
                _IsEditing = False
                Invalidate()
            End If
            _ColumnComboBox.Hide()

            RemoveHandler Me.DataGridTableStyle.DataGrid.Scroll, New EventHandler(AddressOf HandleScroll)
            AddHandler Me.DataGridTableStyle.DataGrid.Scroll, New EventHandler(AddressOf HandleScroll)

        Catch ex As Exception
            Throw
        End Try
    End Sub 'LeaveComboBox

    Protected Overloads Overrides Sub Edit(ByVal [source] As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer, ByVal bounds As System.Drawing.Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)

        Dim enabled As Boolean = True
        Dim e As DataGridEnableEventArgs

        e = New DataGridEnableEventArgs(rowNum, _Col, enabled)

        RaiseEvent CheckCellEnabled(Me, e)

        If e.EnableValue Then 'provide check to approve edit of cell
            MyBase.Edit([source], rowNum, bounds, [readOnly], instantText, cellIsVisible)

            _RowNum = rowNum
            _Source = [source]

            _ColumnComboBox.Parent = Me.TextBox.Parent
            _ColumnComboBox.Location = Me.TextBox.Location
            _ColumnComboBox.Size = New Size(Me.TextBox.Size.Width, _ColumnComboBox.Size.Height)
            _ColumnComboBox.SelectedIndex = _ColumnComboBox.FindStringExact(Me.TextBox.Text)
            _ColumnComboBox.Text = Me.TextBox.Text

            'combo box is breaking somewhere in here
            Me.TextBox.Visible = False
            _ColumnComboBox.Visible = True

            RemoveHandler Me.DataGridTableStyle.DataGrid.Scroll, AddressOf HandleScroll
            AddHandler Me.DataGridTableStyle.DataGrid.Scroll, AddressOf HandleScroll

            _ColumnComboBox.BringToFront()
            _ColumnComboBox.Focus()
        End If

    End Sub 'Edit

    Protected Overrides Function Commit(ByVal dataSource As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer) As Boolean

        If _IsEditing Then
            _IsEditing = False
            SetColumnValueAtRow(dataSource, rowNum, _ColumnComboBox.Text)
        End If
        Return True
    End Function 'Commit

    Protected Overrides Sub ConcedeFocus()
        MyBase.ConcedeFocus()
    End Sub 'ConcedeFocus

    Protected Overrides Function GetColumnValueAtRow(ByVal [source] As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer) As Object

        Dim s As Object
        Dim DV As DataView
        Dim RowCount As Integer
        Dim i As Integer = 0
        Dim s1 As Object

        Try

            s = MyBase.GetColumnValueAtRow([source], rowNum)
            DV = CType(Me._ColumnComboBox.DataSource, System.Data.DataTable).DefaultView
            RowCount = DV.Count

            'if things are slow, you could order your dataview
            '& use binary search instead of this linear one
            While i < RowCount
                s1 = DV(i)(Me._ColumnComboBox.ValueMember)
                If (Not s1 Is DBNull.Value) AndAlso
                    (Not s Is DBNull.Value) AndAlso
                            s Is s1 Then
                    Exit While
                End If
                i = i + 1
            End While

            If i < RowCount Then
                Return DV(i)(Me._ColumnComboBox.DisplayMember)
            End If

            Return DBNull.Value
        Catch ex As Exception
            Throw
        End Try
    End Function 'GetColumnValueAtRow

    Protected Overrides Sub SetColumnValueAtRow(ByVal [source] As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer, ByVal value As Object)
        Dim s As Object

        Dim DV As DataView
        Dim RowCount As Integer
        Dim i As Integer = 0
        Dim s1 As Object

        Try

            s = value

            DV = CType(Me._ColumnComboBox.DataSource, System.Data.DataTable).DefaultView
            RowCount = DV.Count

            'if things are slow, you could order your dataview
            '& use binary search instead of this linear one
            While i < RowCount
                s1 = DV(i)(Me._ColumnComboBox.DisplayMember)
                If (Not s1 Is DBNull.Value) AndAlso
                            s Is s1 Then
                    Exit While
                End If
                i = i + 1
            End While
            If i < RowCount Then
                s = DV(i)(Me._ColumnComboBox.ValueMember)
            Else
                s = DBNull.Value
            End If
            MyBase.SetColumnValueAtRow([source], rowNum, s)
        Catch ex As Exception
            Throw
        End Try

    End Sub 'SetColumnValueAtRow
    Private Sub HandleKeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)

        'ignore if not digit or control key
        If (Not (System.Char.IsDigit(e.KeyChar)) AndAlso Not (System.Char.IsControl(e.KeyChar)) AndAlso Not (e.KeyChar = ".")) Then
            e.Handled = True
        End If

        'ignore if more than 3 digits
        'If ((Me.TextBox.Text.Length >= 3) _
        '            AndAlso Not (System.Char.IsControl(e.KeyChar)) AndAlso Me.TextBox.SelectionLength = 0) Then
        '    e.Handled = True
        'End If

    End Sub

End Class 'DataGridComboBoxColumn