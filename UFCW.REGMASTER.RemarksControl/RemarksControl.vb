Option Strict On
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ComponentModel
Imports DDTek.DB2
Imports System.Data.Common
Imports System.Reflection

Public Class REMARKSControl
    Inherits System.Windows.Forms.UserControl

    'Private Shared _TraceSwitch As New BooleanSwitch("CloneTraceSwitch", "Trace Switch in App.Config")

    Public Event Closing()
    Public Event Save()
    Public Event Cancel()
    Public Event Yes()
    Public Event No()

    Public Delegate Sub LoadCompleteEventHandler(ByVal sender As [Object], ByVal e As LoadEventArgs)
    ' declare a delegate 

    Public Event RowsLoaded As LoadCompleteEventHandler

    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

    Private _FamilyID As Integer = -1
    Private _RelationID As Integer = -1
    Private _PartSSN As Integer = -1
    Private _PatSSN As Integer = -1
    Private _DialogResult As DialogResult
    Private _APPKEY As String = "UFCW\RegMaster\"
    Private _ReadOnlyMode As Boolean = True

    Private _UIState As UIStates

    Private WithEvents _PatientRemarksBS As New BindingSource

    ReadOnly _REGMReadOnlyAccess As Boolean = UFCWGeneralAD.REGMReadOnlyAccess

    Private _ChangedDRs As RemarksDS
    Private _FamilyRemarksDS As New RemarksDS

    Private _HoverCell As New DataGridCell

    Private _Disposed As Boolean = False

    Protected Overrides Sub Dispose(disposing As Boolean)
        If _Disposed Then Return

        If disposing Then
            ' Free any other managed objects here.
            '
            If RemarksDataGrid IsNot Nothing Then
                RemarksDataGrid.Dispose()
            End If
            RemarksDataGrid = Nothing

            If _PatientRemarksBS IsNot Nothing Then
                _PatientRemarksBS.Dispose()
            End If
            _PatientRemarksBS = Nothing

            If _ChangedDRs IsNot Nothing Then
                _ChangedDRs.Dispose()
            End If
            _ChangedDRs = Nothing

            If _FamilyRemarksDS IsNot Nothing Then
                _FamilyRemarksDS.Dispose()
            End If
            _FamilyRemarksDS = Nothing

            If Not (components Is Nothing) Then
                components.Dispose()
            End If

        End If

        ' Free any unmanaged objects here.
        '
        _Disposed = True

        ' Call base class implementation.
        MyBase.Dispose(disposing)
    End Sub

#Region " Windows Form Designer generated code "
    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
        'Add any initialization after the InitializeComponent() call

    End Sub
#End Region

#Region " Properties "

    <BrowsableAttribute(False)> Protected Shadows ReadOnly Property DesignMode() As Boolean

        Get

            ' Returns true if this control or any of its ancestors is in design mode()

            If MyBase.DesignMode Then

                Return True

            Else

                Dim ParentCtrl As Control = Me.Parent

                While ParentCtrl IsNot Nothing

                    Dim Site As ISite = ParentCtrl.Site

                    If Site IsNot Nothing AndAlso Site.DesignMode Then

                        Return True

                    End If

                    ParentCtrl = ParentCtrl.Parent

                End While

                Return False

            End If

        End Get

    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the FamilyID of the Document.")>
    Public Property FamilyID() As Integer
        Get
            Return _FamilyID
        End Get
        Set(ByVal value As Integer)
            _FamilyID = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the RelationID of the Document.")>
    Public Property RelationID() As Integer
        Get
            Return _RelationID
        End Get
        Set(ByVal value As Integer)
            _RelationID = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Gets or Sets the Participant SSN of the Document.")>
    Public Property PartSSN() As Integer
        Get
            Return _PartSSN
        End Get
        Set(ByVal value As Integer)
            _PartSSN = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Can provide COB data to process for caching purpose")>
    Public Property REMARKDataSet() As DataSet
        Get
            Return _FamilyRemarksDS
        End Get
        Set(ByVal value As DataSet)
            _FamilyRemarksDS = CType(value, RemarksDS)
        End Set
    End Property

    <System.ComponentModel.Browsable(True), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Determines if control is in Read Only Mode.")>
    Public Property ReadOnlyMode() As Boolean
        Get
            Return _ReadOnlyMode
        End Get
        Set(ByVal value As Boolean)
            _ReadOnlyMode = value
            ProcessControls(CType(grpEditPanel, Object), _ReadOnlyMode)
        End Set
    End Property

#End Region

#Region "Form\Button Events"

    Private Sub REMARKControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not DesignMode Then
            Try

                Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
                LoadREMARKS()

                Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))


            Catch ex As Exception
                Throw
            End Try
        End If

    End Sub

    Private Sub REMARKControl_dispose(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Disposed
        Me.Dispose()
    End Sub

    Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddActionButton.Click

        If _FamilyID = -1 Then Return

        Try

            AddActionButton.Enabled = False

            AddREMARKLine()

            txtRemarks.Focus()

        Catch ex As Exception


                Throw

        Finally

        End Try

    End Sub

    Private Sub SaveActionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveActionButton.Click

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            SaveActionButton.Enabled = False
            CancelActionButton.Enabled = False

            If Not Me.ValidateChildren(ValidationConstraints.Enabled) Then
                SaveActionButton.Enabled = True
                CancelActionButton.Enabled = True
                Return
            End If

            _PatientRemarksBS.EndEdit()

            _ChangedDRs = CType(_PatientRemarksDS.GetChanges(), RemarksDS)

            SaveActionButton.Enabled = False
            CancelActionButton.Enabled = False

            If SaveREMARKSChanges() Then

                MessageBox.Show("Remarks Saved Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

                _FamilyRemarksDS = Nothing
                _PatientRemarksDS.Tables("REG_REMARKS").Rows.Clear()

                LoadREMARKS()

            Else
                MessageBox.Show("Error while saving Remark." & vbCrLf & "Please try again ", "Unsuccessful", MessageBoxButtons.OK, MessageBoxIcon.Error)
                SaveActionButton.Enabled = True
                CancelActionButton.Enabled = True

            End If

        Catch ex As Exception

            SaveActionButton.Enabled = True
            CancelActionButton.Enabled = True ' this is done in the error handler just in case a failure occurs


                Throw

        Finally

            Me.AutoValidate = HoldAutoValidate

        End Try

    End Sub

    Private Sub CancelActionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelActionButton.Click

        Dim Result As DialogResult = DialogResult.None 'indicates no changes to roll back

        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            _ChangedDRs = CType(_PatientRemarksDS.GetChanges(), RemarksDS)

            CancelActionButton.Enabled = False
            SaveActionButton.Enabled = False

            If _ChangedDRs IsNot Nothing Then
                Result = MessageBox.Show(Me, "Do you want to Cancel the changes?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            End If

            If Result = DialogResult.Yes OrElse Result = DialogResult.None Then

                _PatientRemarksBS.EndEdit()
                _PatientRemarksDS.Tables("REG_REMARKS").RejectChanges()

                ClearErrors()

                _PatientRemarksBS.ResetBindings(False)

                _UIState = UIStates.Modifiable

            ElseIf Result = DialogResult.No Then

                CancelActionButton.Enabled = True
                SaveActionButton.Enabled = True

            End If

        Catch ex As Exception
            Throw
        Finally
        End Try
    End Sub

    Public Sub ClearErrors()
        ErrorProvider1.Clear()
    End Sub

    Private Sub RemarkDataGrid_RefreshGridData() Handles RemarksDataGrid.RefreshGridData

        If _PatientRemarksDS.HasChanges = False Then
            LoadREMARKS()
        End If

    End Sub

#End Region

#Region "Custom Subs\Functions"

    Public Sub LoadREMARKS(ByVal familyID As Integer, ByVal relationID As Integer, ByVal remarksDS As DataSet)

        _FamilyID = familyID
        _RelationID = relationID

        _FamilyRemarksDS = CType(remarksDS, RemarksDS)

        LoadREMARKS()

    End Sub

    Public Sub LoadREMARKS(ByVal familyID As Integer)
        Try
            _FamilyID = familyID
            _RelationID = -1

            LoadREMARKS()

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Public Sub LoadREMARKS(ByVal familyID As Integer, ByVal dt As DataTable)

        Try
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            _FamilyRemarksDS = New RemarksDS

            _FamilyID = familyID
            _RelationID = -1

            For Each DR As DataRow In dt.Rows
                _FamilyRemarksDS.Tables("REG_REMARKS").ImportRow(DR)
            Next
            _FamilyRemarksDS.AcceptChanges()

            ProcessControls(Me, True, False)

            LoadREMARKS()

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        End Try
    End Sub

    Public Sub LoadREMARKS()

        Dim DRs As DataRow()
        Dim Args As New LoadEventArgs()

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ClearErrors()
            ClearDataBindings(Me)

            If _FamilyRemarksDS Is Nothing OrElse _FamilyRemarksDS.Tables Is Nothing OrElse _FamilyRemarksDS.Tables.Count = 0 OrElse _FamilyRemarksDS.Tables("REG_REMARKS").Rows.Count = 0 Then  '' only retrieve data for first time
                _FamilyRemarksDS = New RemarksDS
                _FamilyRemarksDS = CType(RegMasterDAL.RetrieveRegRemarksByFamilyID(_FamilyID, _FamilyRemarksDS), RemarksDS)
            End If

            _PatientRemarksDS = New RemarksDS

            If _FamilyRemarksDS.Tables("REG_REMARKS").Rows.Count > 0 Then

                DRs = _FamilyRemarksDS.Tables("REG_REMARKS").Select("RELATION_ID=" & _RelationID)
                For Each DR In DRs
                    _PatientRemarksDS.Tables("REG_REMARKS").ImportRow(DR)  '' Filter the rows basing on selected record from regemail grid
                Next

            End If

            Args.RowCount = _PatientRemarksDS.Tables("REG_REMARKS").Rows.Count
            Args.FamilyOnlyCount = _PatientRemarksDS.Tables("REG_REMARKS").Select("RELATION_ID=-1").Length

            RaiseEvent RowsLoaded(Me, Args)

            _PatientRemarksBS = New BindingSource
            _PatientRemarksBS.RaiseListChangedEvents = False

            _PatientRemarksBS.DataSource = _PatientRemarksDS.Tables("REG_REMARKS")
            _PatientRemarksBS.Sort = If(RemarksDataGrid.LastSortedBy, RemarksDataGrid.DefaultSort)

            RemarksDataGrid.DataSource = _PatientRemarksBS
            RemarksDataGrid.SetTableStyle()

            LoadDataBinding()

            _PatientRemarksBS.RaiseListChangedEvents = True
            _PatientRemarksBS.ResetBindings(False)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
            DRs = Nothing

        End Try

    End Sub

    Private Sub SetUIElements(readOnlyMode As Boolean)

        Dim DR As DataRow
        Dim HoldAutoValidate As AutoValidate = Me.AutoValidate

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " BS(" & If(_PatientRemarksBS Is Nothing, "N/A", _PatientRemarksBS.Position.ToString) & ") ReadOnly: " & _ReadOnlyMode.ToString & " (" & _UIState.ToString & ") " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Me.AutoValidate = Windows.Forms.AutoValidate.Disable 'This prevents validation from ocurring when buttons are disabled

            grpEditPanel.SuspendLayout()

            If Not readOnlyMode Then readOnlyMode = _REGMReadOnlyAccess
            If _PatientRemarksBS IsNot Nothing AndAlso _PatientRemarksBS.Position > -1 AndAlso _PatientRemarksBS.Current IsNot Nothing AndAlso _PatientRemarksBS.Count > 0 Then
                DR = CType(_PatientRemarksBS.Current, DataRowView).Row
            End If

            Dim AddedQuery = (From Added As DataRow In _PatientRemarksDS.Tables("REG_REMARKS").AsEnumerable()
                              Where Added.RowState = DataRowState.Added)

            Dim ModifiedQuery = (From Modified As DataRow In _PatientRemarksDS.Tables("REG_REMARKS").AsEnumerable()
                                 Where Modified.RowState <> DataRowState.Unchanged)

            Me.SaveActionButton.Enabled = False
            Me.CancelActionButton.Enabled = False

            ProcessSubControls(CType(grpEditPanel, Object), True, True) 'lock everything down except buttons

            AddActionButton.Visible = False

            CancelActionButton.Visible = False
            SaveActionButton.Visible = False

            If Not readOnlyMode Then

                AddActionButton.Visible = True

                CancelActionButton.Visible = True
                SaveActionButton.Visible = True

                AddActionButton.Enabled = True

                If DR IsNot Nothing Then 'based upon row status / content decide how to present controls

                    If DR.RowState = DataRowState.Added Then

                        txtRemarks.ReadOnly = False

                        SaveActionButton.Enabled = True

                    ElseIf DR.RowState = DataRowState.Modified Then

                        txtRemarks.ReadOnly = False

                        SaveActionButton.Enabled = True

                    ElseIf DR.RowState = DataRowState.Unchanged Then

                    End If

                    If AddedQuery.Any Then
                        AddActionButton.Enabled = False
                    End If

                    If ModifiedQuery.Any Then
                        CancelActionButton.Enabled = True
                    End If

                End If
            End If

            grpEditPanel.ResumeLayout() 'needed to ensure transparent controls child controls draw correctly

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub

    Public Sub ProcessControls()
        'Impact Entire control

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ProcessControls(Me, _ReadOnlyMode, False)

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub ProcessControls(ByRef controlContainer As Object, readOnlyMode As Boolean, Optional excludeButtons As Boolean = True)

        Try

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            ProcessControlsInContainer(CType(controlContainer, Object), readOnlyMode, excludeButtons)

            SetUIElements(readOnlyMode)

        Catch ex As Exception

                Throw
        Finally
            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub ProcessControlsInContainer(ByRef controlContainer As Object, ByVal readOnlyMode As Boolean, Optional ByVal excludeButtons As Boolean = False)

        Dim Ctrl As Control
        Dim CtrlName As String

        Try
            '            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If controlContainer Is Nothing Then Return

            Ctrl = CType(controlContainer, Control)

            If Ctrl Is Nothing Then Return

            CtrlName = Ctrl.Name.ToUpper

            For Each ChildCtrl As Object In Ctrl.Controls 'recursive to accomodate groupings

                Dim CtrlMethod As MethodInfo
                Dim CtrlProperty As PropertyInfo
                Dim result As Object
                Dim SharedCtrl As Control

                If TypeOf ChildCtrl Is UserControl Then
                    If ExtensionMethods.HasProperty(ChildCtrl, "ReadOnly") Then
                        If Not ExtensionMethods.HasMethod(ChildCtrl, "ProcessControls") Then

                            CtrlProperty = ChildCtrl.GetType().GetProperty("ReadOnly")
                            If Not CtrlProperty.CanWrite Then
                                result = CtrlProperty.GetValue(ChildCtrl)
                                If CBool(result) = True Then Continue For '
                            End If

                        End If
                    End If
                End If

                If Not (ExtensionMethods.HasProperty(ChildCtrl, NameOf(readOnlyMode)) OrElse ExtensionMethods.HasMethod(ChildCtrl, "ProcessControls")) Then
                    ProcessSubControls(ChildCtrl, readOnlyMode, excludeButtons)

                Else

                    SharedCtrl = DirectCast(ChildCtrl, Control)

                    'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Mid: " & Me.Name & " : (" & readOnlyMode.ToString & ") " & If(SharedCtrl.Parent.Name IsNot Nothing, SharedCtrl.Parent.Name & " : ", "") & SharedCtrl.Name & " : " & SharedCtrl.GetType.ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

                    If ExtensionMethods.HasProperty(ChildCtrl, NameOf(readOnlyMode)) Then
                        CtrlProperty = ChildCtrl.GetType().GetProperty("ReadOnlyMode")
                        CtrlProperty.SetValue(ChildCtrl, readOnlyMode, Nothing)
                    End If

                    If ExtensionMethods.HasMethod(ChildCtrl, "ProcessControls") Then
                        CtrlMethod = ChildCtrl.GetType().GetMethod("ProcessControls")

                        Select Case CtrlMethod.GetParameters().Length
                            Case 0
                                result = CtrlMethod.Invoke(ChildCtrl, Array.Empty(Of Object))
                            Case 1
                                result = CtrlMethod.Invoke(ChildCtrl, New Object() {readOnlyMode})
                            Case 2
                                result = CtrlMethod.Invoke(ChildCtrl, New Object() {readOnlyMode, excludeButtons})
                        End Select
                    End If

                End If
            Next

        Catch ex As Exception

                Throw
        Finally
            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Public Sub ProcessSubControls(ByRef ctrl As Object, ByVal readOnlyMode As Boolean, Optional ByVal excludeButtons As Boolean = False)

        Dim ParentCtrl As Control

        Try
            ParentCtrl = DirectCast(ctrl, Control)

            If ParentCtrl.IsDisposed Then Return

            '  Ignore the control unless it's a textbox.
            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " : (" & readOnlyMode.ToString & ") " & If(ParentCtrl.Parent.Name IsNot Nothing, ParentCtrl.Parent.Name & " : ", "") & ParentCtrl.Name & " : " & ctrl.GetType.ToString & " : " & If(TypeOf (ctrl) Is TextBox, CType(ctrl, TextBox).ReadOnly, ParentCtrl.Enabled).ToString & " -> " & If(TypeOf (ctrl) Is TextBox, readOnlyMode, Not readOnlyMode).ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If TypeOf (ctrl) Is RadioButton OrElse TypeOf (ctrl) Is TextBox OrElse TypeOf (ctrl) Is ComboBox OrElse TypeOf (ctrl) Is DateTimePicker OrElse TypeOf (ctrl) Is Button OrElse TypeOf (ctrl) Is CheckBox OrElse TypeOf (ctrl) Is Label OrElse TypeOf (ctrl) Is DataGrid Then
                If TypeOf (ctrl) Is DataGrid Then
                ElseIf TypeOf (ctrl) Is Label Then
                    CType(ctrl, Label).Enabled = True 'remain enabled to maintain color
                ElseIf TypeOf (ctrl) Is TextBox Then
                    If CType(ctrl, TextBox).ReadOnly <> readOnlyMode Then
                        CType(ctrl, TextBox).ReadOnly = readOnlyMode
                        'CType(ctrl, TextBox).TabStop = Not readOnlyMode
                    End If
                ElseIf TypeOf (ctrl) Is ExComboBox Then
                    If CType(ctrl, ExComboBox).ReadOnly <> readOnlyMode Then
                        CType(ctrl, ExComboBox).ReadOnly = readOnlyMode
                        'CType(ctrl, ExComboBox).TabStop = Not readOnlyMode
                    End If
                ElseIf TypeOf (ctrl) Is ComboBox Then
                    If CType(ctrl, ComboBox).Enabled = readOnlyMode Then
                        CType(ctrl, ComboBox).Enabled = Not readOnlyMode
                    End If
                ElseIf TypeOf (ctrl) Is DateTimePicker Then
                    If CType(ctrl, DateTimePicker).Enabled = readOnlyMode Then
                        CType(ctrl, DateTimePicker).Enabled = Not readOnlyMode
                    End If
                ElseIf TypeOf (ctrl) Is Button Then
                    If Not excludeButtons Then 'Use this when Buttons should not be affected by Read Only processes
                        If CType(ctrl, Button).Enabled = readOnlyMode Then
                            CType(ctrl, Button).Enabled = Not readOnlyMode
                        End If
                    End If
                ElseIf TypeOf (ctrl) Is RadioButton Then
                    If CType(ctrl, RadioButton).Enabled = readOnlyMode Then
                        CType(ctrl, RadioButton).Enabled = Not readOnlyMode
                    End If
                ElseIf TypeOf (ctrl) Is CheckBox Then
                    If CType(ctrl, CheckBox).Enabled = readOnlyMode Then
                        CType(ctrl, CheckBox).Enabled = Not readOnlyMode
                    End If
                End If
            Else

                'continue down container chain
                For Each ChildCtrl As Object In ParentCtrl.Controls

                    'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Mid: " & Me.Name & " : (" & readOnlyMode.ToString & ") " & If(ParentCtrl.Parent.Name IsNot Nothing, ParentCtrl.Parent.Name & " : ", "") & ParentCtrl.Name & " : " & ChildCtrl.GetType.ToString & " : " & If(TypeOf (ChildCtrl) Is TextBox, CType(ChildCtrl, TextBox).ReadOnly, CType(ParentCtrl, Control).Enabled).ToString & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

                    Dim CtrlMethod As MethodInfo
                    Dim CtrlProperty As PropertyInfo
                    Dim result As Object
                    Dim SharedCtrl As Control

                    'To prevent unnecassary processing of readonly controls add this property to the control Public ReadOnly Property [ReadOnly] As Boolean = True
                    If TypeOf ChildCtrl Is UserControl Then
                        If ExtensionMethods.HasProperty(ChildCtrl, "ReadOnly") Then
                            If Not ExtensionMethods.HasMethod(ChildCtrl, "ProcessControls") Then

                                CtrlProperty = ChildCtrl.GetType().GetProperty("ReadOnly")
                                If Not CtrlProperty.CanWrite Then
                                    result = CtrlProperty.GetValue(ChildCtrl)
                                    If CBool(result) = True Then Continue For '
                                End If

                            End If
                        End If
                    End If

                    ProcessSubControls(ChildCtrl, readOnlyMode, excludeButtons)
                Next

            End If

        Catch ex As Exception

                Throw
        Finally
            'Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " : (" & readOnlyMode.ToString & ") " & If(ParentCtrl.Parent.Name IsNot Nothing, ParentCtrl.Parent.Name & " : ", "") & ParentCtrl.Name & " : " & ctrl.GetType.ToString & " : " & If(TypeOf (ctrl) Is TextBox, CType(ctrl, TextBox).ReadOnly, ParentCtrl.Enabled).ToString & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub AddREMARKLine()
        Dim DR As DataRow

        Try
            _PatientRemarksDS.EnforceConstraints = False

            DR = _PatientRemarksDS.REG_REMARKS.NewRow

            DR("FAMILY_ID") = _FamilyID
            DR("RELATION_ID") = _RelationID
            DR("POSTING_DATE") = RegMasterDAL.NowDate
            DR("CREATE_USERID") = UFCWGeneral.DomainUser.ToUpper
            DR("CREATE_DATE") = RegMasterDAL.NowDate
            DR("BATCH_USERID") = UFCWGeneral.DomainUser.ToUpper
            DR("BATCH_DATE") = RegMasterDAL.NowDate
            DR("ONLINE_USERID") = UFCWGeneral.DomainUser.ToUpper
            DR("ONLINE_DATE") = RegMasterDAL.NowDate
            DR("DELETE_FLAG") = 0

            _PatientRemarksDS.REG_REMARKS.Rows.Add(DR)

        Catch ex As Exception


                Throw
        Finally
        End Try
    End Sub

    Private Sub LoadDataBinding()
        Dim Bind As Binding

        Try

            txtRemarks.DataBindings.Clear()
            Bind = New Binding("Text", _PatientRemarksBS, "REMARKS", True)
            txtRemarks.DataBindings.Add(Bind)

        Catch ex As Exception
            Throw
        Finally

        End Try

    End Sub

    Private Sub ClearDataBindings(parentCtrl As Control)
        Try

            For Each C As Control In parentCtrl.Controls

                If C.Controls.Count > 0 Then ClearDataBindings(C) 'recursive for grouping controls

                C.DataBindings.Clear()

                If TypeOf (C) Is RadioButton OrElse TypeOf (C) Is TextBox OrElse TypeOf (C) Is ComboBox OrElse TypeOf (C) Is DateTimePicker OrElse TypeOf (C) Is CheckBox OrElse TypeOf (C) Is DataGrid Then
                    If TypeOf (C) Is DataGrid OrElse TypeOf (C) Is DataGridCustom Then
                        CType(C, DataGridCustom).CaptionText = ""
                        CType(C, DataGridCustom).DataMember = ""
                        CType(C, DataGridCustom).DataSource = Nothing
                    ElseIf TypeOf (C) Is CheckBox Then
                        CType(C, CheckBox).CheckState = CheckState.Unchecked
                    ElseIf TypeOf (C) Is ComboBox Then
                        CType(C, ComboBox).SelectedIndex = -1
                    Else
                        C.ResetText()
                    End If

                End If

            Next

        Catch ex As Exception
            Throw
        Finally

        End Try
    End Sub

    Public Sub ClearAll()

        Try

            Me.SuspendLayout()

            ClearErrors()
            ClearDataBindings(Me)

            _PatientRemarksDS = New RemarksDS
            _FamilyRemarksDS = New RemarksDS

            _ChangedDRs = Nothing

            _HoverCell = Nothing

            _PartSSN = -1
            _PatSSN = -1
            _FamilyID = -1
            _RelationID = -1

            _ReadOnlyMode = True

            Me.ResumeLayout()

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Function SaveREMARKSChanges() As Boolean

        Dim HistSum As String = ""
        Dim HistDetail As String = ""

        Dim ActivityTimeStamp As DateTime = Date.Now

        Dim Transaction As DbTransaction

        Try
            Transaction = RegMasterDAL.BeginTransaction

            _ChangedDRs = CType(_PatientRemarksDS.GetChanges(), RemarksDS)

            For Each DR As DataRow In _ChangedDRs.Tables("REG_REMARKS").Rows

                HistSum = ""
                HistDetail = ""

                HistDetail = DataGridCustom.IdentifyChanges(DR, RemarksDataGrid)

                If DR.RowState <> DataRowState.Added AndAlso DR.RowState <> DataRowState.Deleted Then

                    If HistDetail.Length > 0 Then

                        HistSum = "REMARK OF FAMILYID: " & CStr(DR("FAMILY_ID")) & " WAS MODIFIED"
                        HistDetail = UFCWGeneral.DomainUser.ToUpper & " MODIFIED THE REMARK " & Environment.NewLine & "MODIFICATIONS WERE: " & Environment.NewLine & HistDetail

                        RegMasterDAL.UpdateRegRemarksByFamilyIDRelationID(CInt(DR("REMARK_ID")), CInt(DR("FAMILY_ID")),
                                                                                                   CInt(DR("RELATION_ID")), CStr(DR("REMARKS")),
                                                                                                   UFCWGeneral.DomainUser.ToUpper, Transaction)

                        RegMasterDAL.CreateRegHistory(CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")), _PartSSN, Nothing, "REGREMARKUPDATE", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)

                    End If

                ElseIf DR.RowState = DataRowState.Added Then

                    HistSum = "REMARK OF FAMILYID: " & _FamilyID & " RELATIONID " & _RelationID & " WAS ADDED"
                    HistDetail = UFCWGeneral.DomainUser.ToUpper & " ADDED THE REMARK " & Environment.NewLine & "MODIFICATIONS WERE: " & Environment.NewLine & HistDetail

                    RegMasterDAL.CreateRegRemarks(CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")),
                                                  CType(DR("POSTING_DATE"), Date?),
                                                  CStr(DR("REMARKS")), SystemInformation.UserName.ToUpper, CInt(DR("DELETE_FLAG")), Transaction)

                    RegMasterDAL.CreateRegHistory(CInt(DR("FAMILY_ID")), CInt(DR("RELATION_ID")), _PartSSN, Nothing, "REGREMARKADD", Nothing, Nothing, Nothing, HistSum, HistDetail, UFCWGeneral.DomainUser.ToUpper, Transaction)
                End If

            Next

            RegMasterDAL.CommitTransaction(Transaction)

            _PatientRemarksDS.AcceptChanges()

            Return True

        Catch db2ex As DB2Exception

            Try
                If Transaction IsNot Nothing Then
                    RegMasterDAL.RollbackTransaction(Transaction)
                End If
            Catch ex2 As Exception
            End Try

            Select Case db2ex.Number
                Case -438, -1822
                    MessageBox.Show("The item(s) you are attempting to update has been changed by another process." & vbCrLf & "Exit and re-enter the Remarks Tab to refresh the data.", "Save rejected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Case Else
                    Throw
            End Select

        Catch ex As Exception

            Try
                If Transaction IsNot Nothing Then
                    RegMasterDAL.RollbackTransaction(Transaction)
                End If
            Catch ex2 As Exception
            End Try


                Throw
        Finally
            If Transaction IsNot Nothing Then Transaction.Dispose()
            Transaction = Nothing
        End Try

    End Function

    <System.ComponentModel.Browsable(False), System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), System.ComponentModel.Description("Indicates that Save has not been used.")>
    Public ReadOnly Property ChangesPending() As Boolean
        Get
            Return UnCommittedChangesExist()
        End Get
    End Property

    Private Function UnCommittedChangesExist() As Boolean

        Dim Modifications As String = ""
        Try

            If _PatientRemarksDS Is Nothing Then Return False

            _ChangedDRs = CType(_PatientRemarksDS.GetChanges(), RemarksDS)

            If RemarksDataGrid IsNot Nothing AndAlso _ChangedDRs IsNot Nothing AndAlso _ChangedDRs.Tables("REG_REMARKS").Rows.Count > 0 Then

                For Each DR As DataRow In _ChangedDRs.Tables("REG_REMARKS").Rows
                    If DR.RowState <> DataRowState.Added Then
                        'attempt to exclude rows accidently changed during navigation operations
                        Modifications = DataGridCustom.IdentifyChanges(DR, RemarksDataGrid)

                        If Modifications IsNot Nothing AndAlso Modifications.Length > 0 Then
                            Return True
                        End If

                    ElseIf DR.RowState = DataRowState.Added Then
                        Return True
                    End If
                Next

            End If

        Catch ex As Exception
            Throw
        End Try

    End Function

    Private Sub txtRemarks_Validating(sender As Object, e As CancelEventArgs) Handles txtRemarks.Validating

        Dim Tbox As TextBox = CType(sender, TextBox)

        Try

            ErrorProvider1.ClearError(Tbox)

            If _PatientRemarksBS Is Nothing OrElse _PatientRemarksBS.Position < 0 OrElse Tbox.ReadOnly Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If Tbox.Text.Trim.Length < 1 Then
                ErrorProvider1.SetErrorWithTracking(Tbox, " Remark is required.")
            End If

            If ErrorProvider1.GetError(Tbox).Trim.Length > 0 Then 'are there any errors 
                e.Cancel = True
            End If

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try

    End Sub

    Private Sub PatientRemarksBS_ListChanged(sender As Object, e As ListChangedEventArgs) Handles _PatientRemarksBS.ListChanged

        Dim BS As BindingSource

        Try
            BS = DirectCast(_PatientRemarksBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") RS(" & If(BS.Position < 0 OrElse BS.Current Is Nothing, "N/A", DirectCast(BS.Current, System.Data.DataRowView).Row.RowState.ToString) & ") O(" & e.OldIndex.ToString & ") N(" & e.NewIndex.ToString & ") CT(" & e.ListChangedType.ToString & ") SEL(" & If(BS Is Nothing OrElse BS.Count < 1 OrElse BS.Position < 0 OrElse RemarksDataGrid.DataSource Is Nothing, "N/A", RemarksDataGrid.IsSelected(BS.Position).ToString) & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            Select Case e.ListChangedType
                Case ListChangedType.ItemDeleted 'account for rows deleted due to a cancel action

                    If BS.Position > -1 AndAlso BS.Position <> e.NewIndex AndAlso e.NewIndex > -1 Then
                        BS.Position = e.NewIndex
                    End If

                Case ListChangedType.ItemMoved

                    Select Case DirectCast(BS.Current, System.Data.DataRowView).Row.RowState
                        Case DataRowState.Modified

                        Case DataRowState.Added

                            If e.OldIndex <> e.NewIndex OrElse (BS.Position > -1 AndAlso BS.Count = 1) Then
                                RemarksDataGrid.Select(BS.Position)
                            End If

                    End Select

                Case ListChangedType.ItemChanged

                    If BS.Count = 0 Then 'item was changed in some way that excludes it from list due to filter exclusion
                        Return
                    End If

                    Select Case DirectCast(BS.Current, System.Data.DataRowView).Row.RowState
                        Case DataRowState.Modified


                        Case DataRowState.Added

                    End Select

                Case ListChangedType.Reset 'triggered by sorts or changes in grid filter

                    If BS.Position > -1 AndAlso BS.Position <> e.NewIndex Then
                        If e.NewIndex > -1 Then
                            BS.Position = e.NewIndex
                        End If

                        BS.ResetCurrentItem()

                    End If

                Case ListChangedType.ItemAdded 'includes items reincluded when filters change 

                    If BS.Position <> e.NewIndex OrElse BS.Position > -1 AndAlso BS.Count = 1 Then 'first item added
                        If e.NewIndex > -1 Then BS.Position = e.NewIndex
                        If e.NewIndex > -1 Then RemarksDataGrid.Select(e.NewIndex)
                    End If

            End Select

            ProcessControls()

        Catch ex As Exception
            Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub PatientRemarksBS_CurrentChanged(sender As Object, e As EventArgs) Handles _PatientRemarksBS.CurrentChanged

        Dim BS As BindingSource

        Try

            BS = DirectCast(_PatientRemarksBS, BindingSource)

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

            If BS Is Nothing OrElse BS.Position < 0 OrElse BS.Count < 1 Then Return 'no current row, most likely an item filter value was changed

            Dim DR As DataRow = DirectCast(BS.Current, DataRowView).Row

            ProcessControls(CType(grpEditPanel, Object), _ReadOnlyMode)

        Catch ex As Exception
            Throw
        Finally

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: BS(" & BS.Position.ToString & ") " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
        End Try

    End Sub

    Private Sub SplitContainer1_Validating(sender As Object, e As CancelEventArgs) Handles SplitContainer1.Validating
        Dim SCont As SplitContainer = CType(sender, SplitContainer)

        Try

            If _PatientRemarksBS Is Nothing OrElse _PatientRemarksBS.Position < 0 Then Return

            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))


            Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

        Catch ex As Exception

                Throw
        Finally
        End Try
    End Sub

    Private Sub REMARKSControl_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged

        Try
            'reset controls to appropriate state
            If Not DesignMode AndAlso Not _Disposed Then

                If Me.Visible Then
                    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " In:  " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))

                    SetUIElements(_ReadOnlyMode)

                    Debug.Print(UFCWGeneral.NowDate.ToString("HH:mm:ss.fffffff") & " Out: " & Me.Name & " " & UFCWGeneral.FlattenStack(New System.Diagnostics.StackTrace(True)))
                End If
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

#End Region
End Class

Public Class LoadEventArgs
    Inherits EventArgs
    Private _rowCount As Integer?
    Private _familyOnlyCount As Integer?

    Public Property RowCount As Integer?
        Get
            Return _rowCount
        End Get
        Set(value As Integer?)
            _rowCount = value
        End Set
    End Property

    Public Property FamilyOnlyCount As Integer?
        Get
            Return _familyOnlyCount
        End Get
        Set(value As Integer?)
            _familyOnlyCount = value
        End Set
    End Property

    Public Sub New()
    End Sub
End Class