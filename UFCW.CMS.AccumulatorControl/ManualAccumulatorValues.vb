Imports System.ComponentModel
Imports System.Configuration

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.CustomerServiceControl
''' Class	 : CMS.CustomerServiceControl.AccumulatorValues
'''
''' -----------------------------------------------------------------------------
''' <summary>
'''
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	10/31/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class ManualAccumulatorValuesControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents FamilyAccumulatorsDataGrid As ConfirmDeleteDataGrid
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Friend WithEvents CloseButton As System.Windows.Forms.Button
    Friend WithEvents SaveAndCloseButton As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.FamilyAccumulatorsDataGrid = New ConfirmDeleteDataGrid
        Me.SaveButton = New System.Windows.Forms.Button
        Me.CloseButton = New System.Windows.Forms.Button
        Me.SaveAndCloseButton = New System.Windows.Forms.Button
        CType(Me.FamilyAccumulatorsDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'FamilyAccumulatorsDataGrid
        '
        Me.FamilyAccumulatorsDataGrid.AlternatingBackColor = System.Drawing.Color.LightGray
        Me.FamilyAccumulatorsDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FamilyAccumulatorsDataGrid.BackColor = System.Drawing.Color.DarkGray
        Me.FamilyAccumulatorsDataGrid.BackgroundColor = System.Drawing.SystemColors.Window
        Me.FamilyAccumulatorsDataGrid.CaptionBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.FamilyAccumulatorsDataGrid.CaptionForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.FamilyAccumulatorsDataGrid.CaptionText = "Family Manual Accumulators "
        Me.FamilyAccumulatorsDataGrid.DataMember = ""
        Me.FamilyAccumulatorsDataGrid.ForeColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsDataGrid.GridLineColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsDataGrid.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.FamilyAccumulatorsDataGrid.HeaderBackColor = System.Drawing.Color.Silver
        Me.FamilyAccumulatorsDataGrid.HeaderForeColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsDataGrid.LinkColor = System.Drawing.Color.Navy
        Me.FamilyAccumulatorsDataGrid.Location = New System.Drawing.Point(0, 0)
        Me.FamilyAccumulatorsDataGrid.Name = "FamilyAccumulatorsDataGrid"
        Me.FamilyAccumulatorsDataGrid.ParentRowsBackColor = System.Drawing.Color.White
        Me.FamilyAccumulatorsDataGrid.ParentRowsForeColor = System.Drawing.Color.Black
        Me.FamilyAccumulatorsDataGrid.ReadOnly = True
        Me.FamilyAccumulatorsDataGrid.RowHeadersVisible = False
        Me.FamilyAccumulatorsDataGrid.SelectionBackColor = System.Drawing.Color.Navy
        Me.FamilyAccumulatorsDataGrid.SelectionForeColor = System.Drawing.Color.White
        Me.FamilyAccumulatorsDataGrid.Size = New System.Drawing.Size(272, 320)
        Me.FamilyAccumulatorsDataGrid.TabIndex = 42
        '
        'SaveButton
        '
        Me.SaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.SaveButton.Location = New System.Drawing.Point(8, 336)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.TabIndex = 50
        Me.SaveButton.Text = "Save"
        '
        'CloseButton
        '
        Me.CloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CloseButton.Location = New System.Drawing.Point(185, 336)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.TabIndex = 51
        Me.CloseButton.Text = "Cancel"
        '
        'SaveAndCloseButton
        '
        Me.SaveAndCloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SaveAndCloseButton.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.SaveAndCloseButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.SaveAndCloseButton.Location = New System.Drawing.Point(93, 336)
        Me.SaveAndCloseButton.Name = "SaveAndCloseButton"
        Me.SaveAndCloseButton.Size = New System.Drawing.Size(83, 23)
        Me.SaveAndCloseButton.TabIndex = 52
        Me.SaveAndCloseButton.Text = "Save && Close"
        '
        'ManualAccumulatorValues
        '
        Me.Controls.Add(Me.SaveAndCloseButton)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.FamilyAccumulatorsDataGrid)
        Me.Name = "ManualAccumulatorValues"
        Me.Size = New System.Drawing.Size(272, 368)
        CType(Me.FamilyAccumulatorsDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Private Variables and Properties"
    'Dim conds As Conditions
    Private Const _NonClaimId As Integer = -5

    Private _LastEditedDR As DataRow
    Private _NewCurrentRow As Integer = -1
    Private _NewCurrentCol As Integer = -1
    Private _OldCurrentRow As Integer = -1
    Private _OldCurrentCol As Integer = -1

    Private _Manager As MemberAccumulatorManager
    Private _ReadOnly As Boolean = True
    Private _EditMode As Boolean = False
    Private _GridEditableHeight As Integer
    Private _UserID As String = SystemInformation.UserName.ToUpper

    Private _Loading As Boolean = True
    Private _FamilyID As Integer
    Private _RelationID As Integer
    Private _RefreshPending As Boolean = False
    Private _EffectiveDate As Date
    Private _SSN As String = ""
    Private _APPKEY As String = "UFCW\Claims\"

    Private _HighestEntryID As Integer

    Public Event CloseRequested()
    Public Shadows Event Resize As EventHandler(Of EventArgs)

    Public Event ManualAccumulatorsModified()
    Private _Disposed As Boolean


#End Region
#Region "CleanUp"

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)

        If Not _Disposed Then
            If disposing Then
                If (components IsNot Nothing) Then
                    components.Dispose()
                End If

                If FamilyAccumulatorsDataGrid IsNot Nothing Then
                    FamilyAccumulatorsDataGrid.TableStyles.Clear()
                    FamilyAccumulatorsDataGrid.DataSource = Nothing
                    FamilyAccumulatorsDataGrid.Dispose()
                End If
                FamilyAccumulatorsDataGrid = Nothing

                'If _Manager IsNot Nothing Then _Manager.Dispose()
                If _Manager IsNot Nothing Then _Manager = Nothing
            End If

            _Disposed = True
        End If

        ' Call base class implementation.
        MyBase.Dispose(disposing)
    End Sub
#End Region

#Region "Public Properties & Methods"

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets the Current FamilyID.")>
    Public ReadOnly Property FamilyID() As Integer
        Get
            Return _FamilyID
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets the Current EffectiveDate.")>
    Public ReadOnly Property EffectiveDate() As Date
        Get
            Return _EffectiveDate
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets the Current RelationID")>
    Public ReadOnly Property RelationID() As Integer
        Get
            Return _RelationID
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Indicates if a Refresh should be performed")>
    Public ReadOnly Property RefreshPending() As Boolean
        Get
            Return _RefreshPending
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(True), System.ComponentModel.Description("Gets or Sets the Patients Last Name.")>
    Public ReadOnly Property SSN() As String
        Get
            Return _SSN
        End Get
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets Accumulator Manager.")>
    Public Property MemberAccumulatorManager() As MemberAccumulatorManager
        Get
            Return _Manager
        End Get
        Set(ByVal value As MemberAccumulatorManager)
            'If loading = False Then
            If value Is Nothing Then Return
            _Manager = value
            'End If
        End Set
    End Property
    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Domain User.")>
    Public Property UserID() As String
        Get
            Return _UserID
        End Get
        Set(ByVal value As String)
            If _Loading = False Then
                If value Is Nothing Then Return
                _UserID = value
            End If
        End Set
    End Property
    <Browsable(True), DefaultValue(True), System.ComponentModel.Description("Determines if the control is ReadOnly.")>
    Public Property ControlIsReadOnly() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value
            Me.FamilyAccumulatorsDataGrid.ReadOnly = value
        End Set
    End Property
    <Browsable(True), DefaultValue(True), System.ComponentModel.Description("Determines if Accumulator content can be modified.")>
    Public Property IsInEditMode() As Boolean
        Get
            Return _EditMode
        End Get
        Set(ByVal value As Boolean)

            Dim designMode As Boolean = (LicenseManager.UsageMode = LicenseUsageMode.Designtime)
            If Not designMode Then
                _EditMode = value
                Me.SaveButton.Visible = _EditMode
                Me.SaveAndCloseButton.Visible = _EditMode
                Me.CloseButton.Visible = _EditMode
                ControlIsReadOnly = Not _EditMode

                If _EditMode = False Then
                    FamilyAccumulatorsDataGrid.Height = FamilyAccumulatorsDataGrid.Height + (Me.Height - FamilyAccumulatorsDataGrid.Height - FamilyAccumulatorsDataGrid.Top)
                Else
                    FamilyAccumulatorsDataGrid.Height = _GridEditableHeight
                End If

            End If

        End Set
    End Property
    Public Sub DisplayManualAccumulators(ByVal familyID As Integer, ByVal relationID As Integer)
        DisplayManualAccumulators(familyID, relationID, CDate("1/1/" & Year(UFCWGeneral.NowDate)))
    End Sub
    Public Sub DisplayManualAccumulators(ByVal familyID As Integer, ByVal relationID As Integer, ByVal effectiveDate As Date)

        Dim AccumulatorDT As DataTable

        Try

            Using WC As New GlobalCursor

                _FamilyID = familyID
                _RelationID = relationID
                _EffectiveDate = effectiveDate

                AccumulatorDT = CollectManualAccumulators()

                SetTableStyle(FamilyAccumulatorsDataGrid, AccumulatorDT)

                Me.FamilyAccumulatorsDataGrid.DataSource = AccumulatorDT

                RemoveHandler FamilyAccumulatorsDataGrid.CurrentCellChanged, AddressOf FamilyAccumulatorsDataGrid_CurrentCellChanged
                AddHandler FamilyAccumulatorsDataGrid.CurrentCellChanged, AddressOf FamilyAccumulatorsDataGrid_CurrentCellChanged

                _HighestEntryID = _Manager.GetHighestEntryIdForFamily

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub
    Public Sub RefreshManualAccumulators()
        Dim AccumulatorDT As DataTable

        Try

            _RefreshPending = False

            Using WC As New GlobalCursor

                AccumulatorDT = CollectManualAccumulators(True)

                SetTableStyle(FamilyAccumulatorsDataGrid, AccumulatorDT)

                Me.FamilyAccumulatorsDataGrid.DataSource = AccumulatorDT.DefaultView

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Sub

    Private Function CollectManualAccumulators(Optional ByRef refreshRequested As Boolean = False) As DataTable
        Dim ManualAccumulatorDT As DataTable
        Dim Conditions As Conditions

        Try

            Using WC As New GlobalCursor

                If _Manager Is Nothing Then
                    _Manager = New MemberAccumulatorManager(CShort(_RelationID), _FamilyID)
                End If

                If refreshRequested Then
                    _Manager.RefreshAccumulatorSummariesForMember(True, True)
                End If

                Conditions = PlanController.GetDistinctConditions

                ManualAccumulatorDT = Conditions.GetAccumulatorValues(_RelationID, _FamilyID, _EffectiveDate, True, _Manager, True)
                ManualAccumulatorDT.DefaultView.Sort = "ACCUM_NAME ASC"
                ManualAccumulatorDT.AcceptChanges()
                ManualAccumulatorDT.DefaultView.RowFilter = "ORIGINAL_ACCUM_VALUE <> 0"
                ManualAccumulatorDT.Columns("ORIGINAL_ACCUM_VALUE").DefaultValue = 1

                ManualAccumulatorDT.TableName = "ACCUMULATORS"

                Return ManualAccumulatorDT

            End Using

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function
    'Public Function GetManualAccumulators(ByVal familyID As Integer, ByVal relationID As Integer, ByVal effectiveDate As Date) As DataTable
    '    Dim ManualAccumulatorDT As DataTable
    '    Dim Conditions As Conditions

    '    Try

    '        Using WC As New GlobalCursor
    '            _FamilyID = familyID

    '            _RelationID = relationID
    '            _EffectiveDate = effectiveDate

    '            If _Manager Is Nothing Then
    '                _Manager = New MemberAccumulatorManager(CShort(_RelationID), _FamilyID)
    '                '_manager.RefreshAccumulatorSummariesForMember(True)
    '            End If

    '            Conditions = PlanController.GetUniqueConditions

    '            ManualAccumulatorDT = Conditions.GetAccumulatorValues(_RelationID, _FamilyID, _EffectiveDate, True, _Manager, True)
    '            ManualAccumulatorDT.DefaultView.Sort = "ACCUM_NAME ASC"
    '            ManualAccumulatorDT.AcceptChanges()
    '            ManualAccumulatorDT.DefaultView.RowFilter = "ORIGINAL_ACCUM_VALUE > 0"
    '            ManualAccumulatorDT.Columns("ORIGINAL_ACCUM_VALUE").DefaultValue = 1

    '            ManualAccumulatorDT.TableName = "ACCUMULATORS"

    '            Return ManualAccumulatorDT

    '        End Using

    '    Catch ex As Exception
    '        Throw
    '    Finally
    '    End Try

    'End Function
    Private Function HasChanges() As Boolean
        Dim DT As DataTable

        Try

            If FamilyAccumulatorsDataGrid IsNot Nothing Then
                If FamilyAccumulatorsDataGrid.DataSource IsNot Nothing Then

                    Select Case FamilyAccumulatorsDataGrid.DataSource.GetType
                        Case GetType(DataTable)
                            DT = CType(FamilyAccumulatorsDataGrid.DataSource, DataTable)
                        Case GetType(DataView)
                            DT = CType(FamilyAccumulatorsDataGrid.DataSource, DataView).Table
                    End Select

                    If DT IsNot Nothing Then
                        If DT.GetChanges IsNot Nothing Then
                            If DT.GetChanges.Rows IsNot Nothing Then
                                If DT.GetChanges.Rows.Count > 0 Then
                                    _RefreshPending = True
                                    Return True
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            Return False

        Catch ex As Exception
            Throw
        End Try
    End Function
    Private Shared Function IndentifyDuplicateRows(ByVal dt As DataTable, ByVal colName As String) As DataRow

        '//Datatable which contains unique records will be return as output.

        Dim CopyDT As DataTable

        'create constraint
        Dim DC(1) As DataColumn
        Dim DV As DataView
        Dim DRV As System.Data.DataRowView

        Try

            Using WC As New GlobalCursor

                CopyDT = dt.Clone

                'create constraint
                DC(0) = CopyDT.Columns(colName)
                CopyDT.PrimaryKey = DC

                DV = dt.DefaultView

                Try

                    For Each DRV In DV
                        CopyDT.ImportRow(DRV.Row)
                    Next

                Catch ex As Exception
                    Return DRV.Row
                End Try

            End Using

        Catch ex As Exception
            Throw
        End Try

    End Function
    Private Sub Save()

        Dim DT As DataTable
        Dim ApplyDate As Date = UFCWGeneral.NowDate
        Dim DR As DataRow
        Dim Val As Decimal
        Dim OrgVal As Decimal
        Dim NewVal As Decimal

        Try
            DT = CType(FamilyAccumulatorsDataGrid.DataSource, DataTable)
            DR = IndentifyDuplicateRows(DT, "ACCUM_NAME")

            If DR IsNot Nothing Then
                MessageBox.Show("You have specified Accumulator (" & DR("ACCUM_NAME").ToString & ") multiple times." & vbCrLf &
                                " Duplicate Accumulators must be removed to continue." & vbCrLf & "  Please make changes as necessary.", "Duplicate Accumulators", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            If _HighestEntryID <> _Manager.GetHighestEntryIdForFamily Then
                MessageBox.Show("Changes have occurred since you retrieved the Accumulators." & vbCrLf & "  The " &
                " Accumulators will now be refreshed to their current values." & vbCrLf & "  Please make changes as necessary.", "Changes occurred", MessageBoxButtons.OK, MessageBoxIcon.Information)

                DisplayManualAccumulators(_FamilyID, _RelationID, New Date(UFCWGeneral.NowDate.Year, 12, 31))

                Return
            End If

            If FamilyAccumulatorsDataGrid.DataSource IsNot Nothing Then
                DT = CType(FamilyAccumulatorsDataGrid.DataSource, DataTable)
                For I As Integer = 0 To DT.Rows.Count - 1
                    Select Case DT.Rows(I).RowState
                        Case DataRowState.Unchanged
                        Case DataRowState.Added
                            NewVal = CDec(DT.Rows(I)("ACCUM_VALUE", DataRowVersion.Current))

                            Val = NewVal - 0
                            _Manager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID(CStr(DT.Rows(I)("ACCUM_NAME")))), ApplyDate, Val, True, _UserID)
                        Case DataRowState.Modified
                            NewVal = CDec(DT.Rows(I)("ACCUM_VALUE", DataRowVersion.Current))
                            OrgVal = CDec(DT.Rows(I)("ACCUM_VALUE", DataRowVersion.Original))
                            Val = NewVal - OrgVal
                            _Manager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID(CStr(DT.Rows(I)("ACCUM_NAME")))), ApplyDate, Val, True, _UserID)

                        Case DataRowState.Deleted
                            OrgVal = CDec(DT.Rows(I)("ACCUM_VALUE", DataRowVersion.Original))
                            Val = 0 - OrgVal
                            _Manager.InsertEntry(CInt(AccumulatorController.GetAccumulatorID(CStr(DT.Rows(I)("ACCUM_NAME")))), ApplyDate, Val, True, _UserID)

                    End Select

                Next
            End If

            If _Manager IsNot Nothing Then

                _Manager.CommitAll()
                _Manager.RefreshAccumulatorSummariesForMember()
                DT = CType(FamilyAccumulatorsDataGrid.DataSource, DataTable)
                DT.AcceptChanges()
                _HighestEntryID = _Manager.GetHighestEntryIdForFamily

            End If

        Catch ex As Exception
            DT = CType(FamilyAccumulatorsDataGrid.DataSource, DataTable)
            DT.RejectChanges()
            Throw
        Finally
            DT.DefaultView.RowFilter = "ORIGINAL_ACCUM_VALUE <> 0"
        End Try
    End Sub

#End Region
#Region "Form and Control Events"

    Private Sub CloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseButton.Click
        If HasChanges() Then
            If MessageBox.Show("Accumulator(s) have been modified. Do you want to discard any changes ? ", "Discard Changes", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = DialogResult.OK Then
                If _LastEditedDR IsNot Nothing Then _LastEditedDR.ClearErrors()
                RaiseEvent CloseRequested()
            End If
        Else
            RaiseEvent CloseRequested()
        End If
    End Sub

    Private Sub SaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveButton.Click

        Dim DT As DataTable

        Try

            If _LastEditedDR IsNot Nothing Then
                _LastEditedDR.ClearErrors()

                If IsDBNull(_LastEditedDR("ACCUM_VALUE")) AndAlso Not IsDBNull(_LastEditedDR("ACCUM_NAME")) Then
                    Debug.WriteLine("Save: Missing Accum Value Alert " & " Rows State " & _LastEditedDR.RowState)
                    _LastEditedDR.SetColumnError("ACCUM_VALUE", "You must specify an amount to continue, press cancel to exit.")
                    FamilyAccumulatorsDataGrid.CurrentCell = New DataGridCell(_OldCurrentRow, _OldCurrentCol)
                End If
            End If

            DT = CType(FamilyAccumulatorsDataGrid.DataSource, DataTable)

            If DT.HasErrors Then
                MsgBox("You must complete the modified Accumulator before saving.", MsgBoxStyle.Exclamation, "Accumulator requires Amount")
            ElseIf HasChanges() Then
                Save()
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub SaveAndCloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAndCloseButton.Click
        Dim DT As DataTable

        Try

            If _LastEditedDR IsNot Nothing Then
                _LastEditedDR.ClearErrors()

                If IsDBNull(_LastEditedDR("ACCUM_VALUE")) AndAlso Not IsDBNull(_LastEditedDR("ACCUM_NAME")) Then
                    Debug.WriteLine("Save: Missing Accum Value Alert " & " Rows State " & _LastEditedDR.RowState)
                    _LastEditedDR.SetColumnError("ACCUM_VALUE", "You must specify an amount to continue, press cancel to exit.")
                    FamilyAccumulatorsDataGrid.CurrentCell = New DataGridCell(_OldCurrentRow, _OldCurrentCol)
                End If
            End If

            DT = CType(FamilyAccumulatorsDataGrid.DataSource, DataTable)

            If DT.HasErrors Then
                MsgBox("You must complete the modified Accumulator before saving.", MsgBoxStyle.Exclamation, "Accumulator requires Amount")
            Else

                If HasChanges() Then Save()

                RaiseEvent CloseRequested()
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub
#End Region

#Region "Grid Processing"

    Private Sub SetTableStyle(ByVal dg As ConfirmDeleteDataGrid, ByVal dt As DataTable)

        Dim DGTS As DataGridTableStyle
        Dim TextCol As ConfirmDeleteDataGridHighlightTextBoxColumn
        Dim ComboCol As ConfirmDeleteDataGridComboBoxColumn
        'Dim ComboCol As DataGridComboBoxColumnExtn
        Dim IntCol As Integer
        Dim CurMan As CurrencyManager
        Dim ColsDV As DataView
        Dim FormatNumber As Globalization.NumberFormatInfo
        Dim DSDefaultStyle As DataSet
        Dim XMLStyleName As String
        Dim ColumnSequenceDV As DataView

        Try

            FormatNumber = Application.CurrentCulture.NumberFormat

            XMLStyleName = CStr(CType(ConfigurationManager.GetSection(dt.TableName & "DataGrid"), IDictionary)("StyleLocation"))
            DSDefaultStyle = CMSXMLHandler.CreateDataSetFromXML(XMLStyleName)
            ColumnSequenceDV = New DataView(DSDefaultStyle.Tables(1)) With {
                .Sort = "DefaultOrder"
            }

            ColsDV = ColumnSequenceDV

            CurMan = CType(Me.BindingContext(dt), CurrencyManager)

            DGTS = New DataGridTableStyle(CurMan) With {
                .MappingName = dt.TableName
            }

            DGTS.GridColumnStyles.Clear()
            DGTS.GridLineStyle = DataGridLineStyle.None

            If DSDefaultStyle.Tables.Contains(XMLStyleName & "Style") Then
                If DSDefaultStyle.Tables(XMLStyleName & "Style").Columns.Contains("GridLineStyle") Then
                    DGTS.GridLineStyle = If(CBool(DSDefaultStyle.Tables(XMLStyleName & "Style").Rows(0)("GridLineStyle")), DataGridLineStyle.Solid, DataGridLineStyle.None)
                End If
                If DSDefaultStyle.Tables(XMLStyleName & "Style").Columns.Contains("RowHeadersVisible") Then
                    DGTS.RowHeadersVisible = CBool(DSDefaultStyle.Tables(XMLStyleName & "Style").Rows(0)("RowHeadersVisible"))
                End If
            End If

            For IntCol = 0 To ColsDV.Count - 1
                'in the event that a control is to be displayed as read only Combo boxes are reverted back to Textboxes
                Dim IsReadOnly As Boolean
                Dim IsBoolean As Boolean = Boolean.TryParse(ColsDV(IntCol).Item("ReadOnly").ToString, IsReadOnly)

                If ((IsBoolean And IsReadOnly) OrElse _EditMode = False) AndAlso ColsDV(IntCol).Item("Type").ToString = "Combo" Then
                    ColsDV(IntCol).Item("Type") = "Text"
                End If

                Select Case ColsDV(IntCol).Item("Type").ToString
                    Case "Text"
                        TextCol = New ConfirmDeleteDataGridHighlightTextBoxColumn With {
                            .MappingName = CStr(ColsDV(IntCol).Item("Mapping")),
                            .HeaderText = CStr(ColsDV(IntCol).Item("HeaderText")),
                            .Width = CInt(GetSetting(_APPKEY, dg.Name & "\ColumnSettings", "Col " & ColsDV(IntCol).Item("Mapping").ToString, CStr(UFCWGeneral.MeasureWidthinPixels(CInt(ColsDV(IntCol).Item("DefaultCharWidth")), dg.Font.Name, dg.Font.Size)))),
                            .NullText = ColsDV(IntCol).Item("NullText").ToString
                        }
                        TextCol.TextBox.WordWrap = True

                        If (IsBoolean And IsReadOnly) OrElse _EditMode = False Then
                            TextCol.ReadOnly = True
                        End If

                        If Not IsDBNull(ColsDV(IntCol).Item("Format")) AndAlso ColsDV(IntCol).Item("Format").ToString <> "" Then
                            TextCol.Format = ColsDV(IntCol).Item("Format").ToString
                        End If

                        'RemoveHandler TextCol.TextBox.KeyPress, New System.Windows.Forms.KeyPressEventHandler(AddressOf HandleKeyPress)
                        'AddHandler TextCol.TextBox.KeyPress, New System.Windows.Forms.KeyPressEventHandler(AddressOf HandleKeyPress)

                        DGTS.GridColumnStyles.Add(TextCol)

                    Case "Combo"

                        ComboCol = New ConfirmDeleteDataGridComboBoxColumn With {
                            .Alignment = HorizontalAlignment.Left,
                            .MappingName = CStr(ColsDV(IntCol).Item("Mapping")),
                            .HeaderText = CStr(ColsDV(IntCol).Item("HeaderText")),
                            .Width = CInt(GetSetting(_APPKEY, dg.Name & "\ColumnSettings", "Col " & ColsDV(IntCol).Item("Mapping").ToString, CStr(UFCWGeneral.MeasureWidthinPixels(CInt(ColsDV(IntCol).Item("DefaultCharWidth")), dg.Font.Name, dg.Font.Size)))),
                            .NullText = CStr(ColsDV(IntCol).Item("NullText"))
                        }

                        ComboCol.ColumnComboBox.DisplayMember = CStr(ColsDV(IntCol).Item("Mapping"))
                        ComboCol.ColumnComboBox.ValueMember = CStr(ColsDV(IntCol).Item("Mapping"))

                        If CType(ColsDV(IntCol).Item("SQL"), String).Trim.Length > 0 Then
                            ComboCol.ColumnComboBox.DataSource = CMSDALFDBMD.RUNIMMEDIATESELECT(CType(ColsDV(IntCol).Item("SQL"), String).Trim)
                            ComboCol.ColumnComboBox.DisplayMember = CStr(ColsDV(IntCol).Item("SQLDisplayColumn"))
                            ComboCol.ColumnComboBox.ValueMember = CStr(ColsDV(IntCol).Item("SQLDisplayColumn"))
                        End If
                        ' ComboCol.ColumnComboBox.DropDownStyle = ComboBoxStyle.DropDownList
                        'DGTS.PreferredRowHeight = (ComboCol.ColumnComboBox.Height) + 2
                        DGTS.PreferredRowHeight = (DGTS.PreferredRowHeight * 2) + 2
                        DGTS.GridColumnStyles.Add(ComboCol)

                        RemoveHandler ComboCol.CheckCellEnabled, New EnableCellEventHandler(AddressOf SetEnableValues)
                        AddHandler ComboCol.CheckCellEnabled, New EnableCellEventHandler(AddressOf SetEnableValues)

                End Select

            Next

        Catch ex As Exception
            Throw
        Finally

            If dg IsNot Nothing Then

                dg.TableStyles.Clear()
                dg.TableStyles.Add(DGTS)

                RemoveHandler dg.KeyPress, AddressOf CellKeyPress
                AddHandler dg.KeyPress, AddressOf CellKeyPress

            End If

            dt = Nothing
            CurMan = Nothing

            If ColumnSequenceDV IsNot Nothing Then ColumnSequenceDV.Dispose()
            ColumnSequenceDV = Nothing

            If DGTS IsNot Nothing Then DGTS.Dispose()
            DGTS = Nothing

            If DSDefaultStyle IsNot Nothing Then DSDefaultStyle.Dispose()
            DSDefaultStyle = Nothing

        End Try

    End Sub

    'Private Sub HandleKeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)

    '    Debug.WriteLine(e.KeyChar)

    'End Sub

    Sub CellKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        'prevent right arrow from leaving cell
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Right) Then
            e.Handled = True
        End If

    End Sub

    Shared Sub DataGrid_CellFormat(ByVal column As DataGridTextBoxColumn, ByVal rowIndex As Integer, ByRef value As Object, ByRef format As String)

        Try

            If column.Format.Equals("1stCharacterMask") Then
                Dim FirstChar As String = CStr(value).Substring(0, 1)

                Select Case FirstChar
                    Case "T"
                        format = "{0:00'-'0000000}"
                    Case "S"
                        format = "{0:000'-'00'-'0000}"
                    Case Else
                        format = "{0}" ' instructs format to reflect original value
                End Select

                value = CInt(CStr(value).Substring(1))

            ElseIf column.Format.Equals("Yes/No") Then
                format = "{0:Yes;;No}"
            ElseIf column.Format = "Currency" Then
                format = ""
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub FamilyAccumulatorsDataGrid_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles FamilyAccumulatorsDataGrid.CurrentCellChanged

        'prevent recursive calls
        RemoveHandler FamilyAccumulatorsDataGrid.CurrentCellChanged, AddressOf FamilyAccumulatorsDataGrid_CurrentCellChanged

        Dim BM As BindingManagerBase
        Dim DR As DataRow

        Try

            BM = FamilyAccumulatorsDataGrid.BindingContext(FamilyAccumulatorsDataGrid.DataSource, FamilyAccumulatorsDataGrid.DataMember)
            DR = CType(BM.Current, DataRowView).Row

            _NewCurrentRow = FamilyAccumulatorsDataGrid.CurrentCell.RowNumber
            _NewCurrentCol = FamilyAccumulatorsDataGrid.CurrentCell.ColumnNumber

            If _OldCurrentRow = -1 OrElse _OldCurrentCol = -1 Then
                _OldCurrentRow = FamilyAccumulatorsDataGrid.CurrentCell.RowNumber
                _OldCurrentCol = FamilyAccumulatorsDataGrid.CurrentCell.ColumnNumber
            End If

            Try
                If _LastEditedDR IsNot Nothing Then _LastEditedDR.ClearErrors()
            Catch ex As Exception
#If DEBUG Then
                Debug.WriteLine("No Row to clear edits against.")
#End If
            End Try

            If IsDBNull(DR("ACCUM_NAME")) AndAlso DR.RowState = DataRowState.Detached AndAlso _NewCurrentCol = 1 Then
#If DEBUG Then
                Debug.WriteLine("Missing ACCUM_NAME " & " Rows State " & DR.RowState)
#End If
                FamilyAccumulatorsDataGrid.CurrentCell = New DataGridCell(_OldCurrentRow, 0)
                Exit Try
            Else
#If DEBUG Then
                Debug.WriteLine(DR("ACCUM_NAME").ToString() & " " & DR("ACCUM_VALUE").ToString() & " Rows State " & DR.RowState)
#End If
                If _OldCurrentRow <> _NewCurrentRow Then
                    Try

                        If IsDBNull(_LastEditedDR("ACCUM_VALUE")) AndAlso Not IsDBNull(_LastEditedDR("ACCUM_NAME")) Then
#If DEBUG Then
                            Debug.WriteLine("Missing Accum Value Alert " & " Rows State " & _LastEditedDR.RowState)
#End If
                            _LastEditedDR.SetColumnError("ACCUM_VALUE", "You must specify an amount to continue, press cancel to exit.")
                            'MsgBox("You must enter an Accumulator amount to continue.", MsgBoxStyle.Exclamation + MsgBoxStyle.OKOnly, "Missing Accumulator Amount")
                            FamilyAccumulatorsDataGrid.CurrentCell = New DataGridCell(_OldCurrentRow, _OldCurrentCol)
                            Exit Try
                        End If

                    Catch ex As Exception
#If DEBUG Then
                        Debug.WriteLine("Previous row no longer exists. ")
#End If
                        'if previous row dows not exist allow grid reposition
                    End Try
                ElseIf _OldCurrentRow = _NewCurrentRow AndAlso Not (_NewCurrentCol = 0) Then 'allow back tab

                    If IsDBNull(DR("ACCUM_VALUE")) AndAlso Not IsDBNull(DR("ACCUM_NAME")) Then
#If DEBUG Then
                        Debug.WriteLine("Missing Accum Value Alert " & " Rows State " & DR.RowState)
#End If
                        DR.SetColumnError("ACCUM_VALUE", "You must specify an amount to continue, press cancel to exit.")
                        'MsgBox("You must enter an Accumulator amount to continue.", MsgBoxStyle.Exclamation + MsgBoxStyle.OKOnly, "Missing Accumulator Amount")
                        FamilyAccumulatorsDataGrid.CurrentCell = New DataGridCell(_OldCurrentRow, 1)
                        Exit Try
                    End If

                End If
            End If

            'skip hidden columns
            If CType(CType(CType(CType(CType(sender, System.Windows.Forms.DataGrid).TableStyles, System.Windows.Forms.GridTableStylesCollection).Item(0), System.Windows.Forms.DataGridTableStyle).GridColumnStyles, System.Windows.Forms.GridColumnStylesCollection).Item(FamilyAccumulatorsDataGrid.CurrentCell.ColumnNumber), System.Windows.Forms.DataGridColumnStyle).Width() = 0 Then

                If FamilyAccumulatorsDataGrid.LastKey = Keys.Tab AndAlso Control.ModifierKeys <> Keys.Shift Then
                    FamilyAccumulatorsDataGrid.CurrentCell = New DataGridCell(FamilyAccumulatorsDataGrid.CurrentCell.RowNumber + 1, 0)
                ElseIf FamilyAccumulatorsDataGrid.LastKey = Keys.Tab AndAlso Control.ModifierKeys = Keys.Shift And FamilyAccumulatorsDataGrid.CurrentCell.RowNumber > 0 Then
                    FamilyAccumulatorsDataGrid.CurrentCell = New DataGridCell(FamilyAccumulatorsDataGrid.CurrentCell.RowNumber, 1)
                End If

            End If

        Catch ex As Exception
            Throw
        Finally

            _LastEditedDR = DR

            AddHandler FamilyAccumulatorsDataGrid.CurrentCellChanged, AddressOf FamilyAccumulatorsDataGrid_CurrentCellChanged

            _OldCurrentRow = _NewCurrentRow
            _OldCurrentCol = _NewCurrentCol

        End Try

    End Sub

    Public Sub SetEnableValues(ByVal sender As Object, ByVal e As DataGridEnableEventArgs)

        Dim BM As BindingManagerBase
        Dim DR As DataRow

        Try

            'do something based on the row & column to set enable flag
            If TypeOf (sender) Is ConfirmDeleteDataGridComboBoxColumn Then
                BM = FamilyAccumulatorsDataGrid.BindingContext(FamilyAccumulatorsDataGrid.DataSource, FamilyAccumulatorsDataGrid.DataMember)
                DR = CType(BM.Current, DataRowView).Row

                If DR.RowState = DataRowState.Detached OrElse IsDBNull(DR("ACCUM_VALUE")) OrElse DR.RowState = DataRowState.Added Then
                    e.EnableValue = True
                Else
                    e.EnableValue = False
                End If
            Else
                e.EnableValue = True
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ManualAccumulatorValuesControl_ClientSizeChanged(sender As Object, e As EventArgs) Handles Me.ClientSizeChanged

        _GridEditableHeight = FamilyAccumulatorsDataGrid.Height

    End Sub

#End Region

End Class