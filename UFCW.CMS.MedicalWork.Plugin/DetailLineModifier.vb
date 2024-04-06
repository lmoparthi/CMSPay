Option Infer On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Public Class DetailLineModifierForm
    Inherits System.Windows.Forms.Form

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _ClaimID As Integer
    Private _LineNumber As Short
    Private _APPKEY As String = "UFCW\Claims\"
    Private _Status As String = ""

    Const MAXMODIFIER As Integer = 5

    Private _ModifiersDT As DataTable
    Private _ModifiersBS As BindingSource

#Region "Constructors"
    Public Sub New(ByVal claimID As Integer, ByRef medDtlDR As DataRow, ByRef medModDT As DataTable)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        _ClaimID = claimID
        _LineNumber = CShort(medDtlDR("LINE_NBR"))

        _ModifiersDT = medModDT

    End Sub

#End Region

#Region "Public Properties"
    <System.ComponentModel.Description("Gets the modified ClaimDataset.")>
    Public ReadOnly Property MedMod() As DataTable
        Get
            Return _ModifiersDT
        End Get
    End Property

    <System.ComponentModel.Description("Gets or Sets the AppKey to use when saving control information.")>
    Public Property AppKey() As String
        Get
            Return _APPKEY
        End Get
        Set(ByVal value As String)

            _APPKEY = value
        End Set
    End Property

    Public Property Status() As String
        Get
            Return _Status
        End Get
        Set(ByVal value As String)
            _Status = value
        End Set
    End Property
#End Region

#Region "Form Events"
    Private Sub DetailLineModifier_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            SetSettings()
            Me.Text = "Claim [" & _ClaimID & "] Line " & _LineNumber & " Modifier(s)"

            LoadModifier()

            LineModifiersDataGrid.ContextMenuPrepare(ModifierCustomContextMenu)

        Catch ex As Exception
            Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
            If (Rethrow) Then
                'Throw
                MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End Try
    End Sub

    Private Sub DetailLineModifier_FormClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.FormClosing
        SaveSettings()
    End Sub

    Private Sub CancelActionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelActionButton.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ModifierLookupButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ModifierLookupButton.Click

        Dim Frm As New ModifierLookupForm
        Dim Codes As String = ""

        Try
            If Frm.ShowDialog(Me) = DialogResult.OK Then
                For Each dr As DataRow In Frm.ModifierDataGrid.GetSelectedDataRows
                    Codes &= If(Codes <> "", ", ", "") & dr("MODIFIER_VALUE").ToString
                Next

                If Codes <> "" Then
                    If ModifierCodesTextBox.Text = "" Then
                        ModifierCodesTextBox.Text = Codes
                    Else
                        ModifierCodesTextBox.Text &= ", " & Codes
                    End If
                End If
            End If

        Catch ex As Exception
            Throw
        Finally
            If Frm IsNot Nothing Then
                Frm.Dispose()
            End If
            ModifierCodesTextBox.Focus()
        End Try
    End Sub

    Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddButton.Click

        If ValidateAddedModifiers() Then AddModifiers()

    End Sub

    'Private Sub AddMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddMenuItem.Click
    '    Try
    '        AddButton.PerformClick()
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    Private Sub DeleteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteButton.Click, DeleteMenuItem.Click
        Try
            Delete()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SortUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SortUpButton.Click
        Try
            SortItemsUp()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SortDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SortDownButton.Click
        Try
            SortItemsDown()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ModifierCodesTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ModifierCodesTextBox.TextChanged

        If ModifierCodesTextBox.Text.Length = 0 Then
            AddButton.Enabled = False
        Else
            If LineModifiersDataGrid.GetGridRowCount < MAXMODIFIER Then
                AddButton.Enabled = True
            Else
                AddButton.Enabled = False
            End If
        End If
    End Sub

    Private Sub LineModifierDataGrid_RowCountChanged(ByVal previousRowCount As Integer?, ByVal currentRowCount As Integer) Handles LineModifiersDataGrid.RowCountChanged
        currentRowCount = LineModifiersDataGrid.GetGridRowCount
        If currentRowCount = 0 Then
            DeleteButton.Enabled = False
            SortUpButton.Enabled = False
            SortDownButton.Enabled = False
        Else
            DeleteButton.Enabled = True
            SortUpButton.Enabled = True
            SortDownButton.Enabled = True
        End If

        If currentRowCount >= MAXMODIFIER Then
            ModifierCodesTextBox.Enabled = False
            AddButton.Enabled = False
        Else
            ModifierCodesTextBox.Enabled = True

            If ModifierCodesTextBox.Text.Length > 0 Then
                AddButton.Enabled = True
            End If
        End If
    End Sub

    Private Sub ModifierCodesTextBox_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ModifierCodesTextBox.EnabledChanged
        ModifierLookupButton.Enabled = ModifierCodesTextBox.Enabled
    End Sub

    Private Sub UpdateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateAllButton.Click, UpdateLineButton.Click

        If ValidateAddedModifiers() Then

            If AddModifiers() Then

                If CType(sender, Button).Name = "UpdateLineButton" Then
                    _Status = "UPDATELINE"
                Else
                    _Status = "UPDATEALL"
                End If

                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        End If

    End Sub

    Private Sub ClearAllButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClearAllButton.Click
        _Status = "CLEARALL"
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub ClearLineButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClearLineButton.Click
        _Status = "CLEARLINE"
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

#End Region

#Region "Methods"
    Private Sub LoadModifier()

        Try
            _ModifiersBS = New BindingSource
            _ModifiersBS.DataSource = _ModifiersDT
            _ModifiersBS.Sort = "PRIORITY"

            LineModifiersDataGrid.SuspendLayout()
            LineModifiersDataGrid.DataSource = _ModifiersBS
            LineModifiersDataGrid.ResumeLayout()
            LineModifiersDataGrid.SetTableStyle()
            If _ModifiersBS IsNot Nothing AndAlso _ModifiersBS.Position > -1 Then
                LineModifiersDataGrid.Select(_ModifiersBS.Position)
            End If
            LineModifierDataGrid_RowCountChanged(-1, -1)
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
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Saves the basic form settings.  Windowstate, height, width, top, and left.
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[Nick Snyder]	11/16/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim lWindowState As FormWindowState = Me.WindowState
        SaveSetting(_APPKEY, Me.Name & "\Settings", "WindowState", CInt(lWindowState).ToString)

        Me.WindowState = FormWindowState.Normal
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Top", Me.Top.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Height", Me.Height.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Left", Me.Left.ToString)
        SaveSetting(_APPKEY, Me.Name & "\Settings", "Width", Me.Width.ToString)
        Me.WindowState = lWindowState

    End Sub

    Private Sub SortItemsUp()

        Dim DR As DataRow

        Try
            If _ModifiersBS Is Nothing AndAlso _ModifiersBS.Position < 0 Then Return

            Using WC As New GlobalCursor

                DR = CType(_ModifiersBS.Current, DataRowView).Row

                Dim AdjustItemsQuery = (From r In _ModifiersDT.AsEnumerable
                                        Where r.Field(Of Integer)("PRIORITY") < CInt(DR("PRIORITY"))
                                        Order By r.Field(Of Integer)("PRIORITY") Descending).FirstOrDefault

                If AdjustItemsQuery Is Nothing Then Return

                'if gaps in priority existed in the original list this will retain gaps
                Dim PrioritySwap As Integer = CInt(AdjustItemsQuery("PRIORITY"))
                AdjustItemsQuery("PRIORITY") = DR("PRIORITY")
                DR("PRIORITY") = PrioritySwap
                _ModifiersBS.EndEdit()

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub SortItemsDown()

        Dim DR As DataRow

        Try

            If _ModifiersBS Is Nothing AndAlso _ModifiersBS.Position < 0 Then Return

            Using WC As New GlobalCursor

                DR = CType(_ModifiersBS.Current, DataRowView).Row

                Dim AdjustItemsQuery = (From r In _ModifiersDT.AsEnumerable
                                        Where r.Field(Of Integer)("PRIORITY") > CInt(DR("PRIORITY"))
                                        Order By r.Field(Of Integer)("PRIORITY")).FirstOrDefault

                If AdjustItemsQuery Is Nothing Then Return

                'if gaps in priority existed in the original list this will retain gaps
                Dim PrioritySwap As Integer = CInt(AdjustItemsQuery("PRIORITY"))
                AdjustItemsQuery("PRIORITY") = DR("PRIORITY")
                DR("PRIORITY") = PrioritySwap
                _ModifiersBS.EndEdit()

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Sub Delete()

        Dim DR As DataRow
        Dim GridDV As DataView
        Try

            Using WC As New GlobalCursor

                If LineModifiersDataGrid.GetSelectedDataRows.Count <> 1 Then Return

                DR = CType(_ModifiersBS.Current, DataRowView).Row
                DR.Delete()

                'Re-Prioritize
                GridDV = LineModifiersDataGrid.GetDefaultDataView

                For Cnt As Integer = 0 To GridDV.Count - 1
                    GridDV(Cnt).Row.Item("PRIORITY") = Cnt
                Next

                '    DirectCast(_ModifiersBS.DataSource, DataTable).AcceptChanges()

                _ModifiersBS.EndEdit()

                LineModifiersDataGrid.RefreshData()

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Function ValidateAddedModifiers() As Boolean

        Dim Source As String = ModifierCodesTextBox.Text
        Dim Modifiers2Add() As String
        ' Dim DR As DataRow
        Dim ModifierDR As DataRow

        Try
            Source = Source.Replace(" ", "")

            Modifiers2Add = Source.ToUpper.Split(CChar(","))
            If Modifiers2Add.Length + _ModifiersBS.Count > MAXMODIFIER Then
                ModifierCodesTextBox.SelectionStart = 0
                ModifierCodesTextBox.SelectionLength = ModifierCodesTextBox.Text.Length

                MessageBox.Show("Only " & MAXMODIFIER & " Modifiers Are Allowed. Please Select The Best Match", "To Many Modifiers", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ModifierCodesTextBox.Focus()

                Return False

            End If

            If ModifierCodesTextBox.Text.Trim.Length > 0 Then
                For Each Modifier As String In Modifiers2Add

                    ModifierDR = CMSDALFDBMD.RetrieveModifierValuesInformation(Modifier)
                    If ModifierDR Is Nothing Then
                        ModifierCodesTextBox.SelectionStart = InStr(ModifierCodesTextBox.Text.ToUpper, Modifier) - 1
                        ModifierCodesTextBox.SelectionLength = Modifier.Length

                        MessageBox.Show("Modifier " & """" & Modifier & """" & " is not valid.", "Invalid Modifier", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ModifierCodesTextBox.Focus()

                        Return False

                    End If
                Next
            End If

            Return True

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Function

    Private Function AddModifiers() As Boolean

        Dim Source As String = ModifierCodesTextBox.Text
        Dim Modifiers() As String
        Dim ModifierDR As DataRow

        Try

            If ModifierCodesTextBox.Text.Trim.Length > 0 Then

                Source = Source.Replace(" ", "")

                Modifiers = Source.ToUpper.Split(CChar(","))

                For Each Modifier As String In Modifiers

                    ModifierDR = CMSDALFDBMD.RetrieveModifierValuesInformation(Modifier)

                    If _ModifiersBS.Find("MODIFIER", Modifier) < 0 Then

                        Dim DR As DataRow = _ModifiersDT.NewRow

                        DR("CLAIM_ID") = _ClaimID
                        DR("LINE_NBR") = _LineNumber
                        DR("MODIFIER") = ModifierDR("MODIFIER_VALUE")
                        DR("FULL_DESC") = ModifierDR("FULL_DESC")

                        Dim NextPriorityQuery As Integer
                        If _ModifiersDT.Rows.Count > 0 Then
                            NextPriorityQuery = (From r In _ModifiersDT.AsEnumerable Select r.Field(Of Integer)("PRIORITY")).Max
                            NextPriorityQuery += 1
                        End If

                        DR("PRIORITY") = NextPriorityQuery '1st entry should be zero.

                        _ModifiersDT.Rows.Add(DR)

                    End If
                Next

                ModifierCodesTextBox.Text = ""
            End If
            _ModifiersBS.EndEdit()
            Return True

        Catch ex As Exception
            Throw
        End Try
    End Function

#End Region
End Class