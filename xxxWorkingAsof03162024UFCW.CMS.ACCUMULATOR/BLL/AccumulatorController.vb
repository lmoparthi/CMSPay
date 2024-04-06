Option Explicit On
Option Strict On
''' -----------------------------------------------------------------------------
''' Project	 : Accumulator
''' Class	 : CMS.AccumulatorController
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' This class manages Accumulators.
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	12/15/2005	Created
''' </history>
''' -----------------------------------------------------------------------------
Public NotInheritable Class AccumulatorController
#Region "Variables"
    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private Shared _DeleteList As New ArrayList
    Private Shared _AccumulatorsDT As DataTable
#End Region

#Region "Constructors"
    Shared Sub New()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Default Contrsuctor
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        '     [paulw]	12/15/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            _AccumulatorsDT = AccumulatorDAL.GetAccumulators
            _AccumulatorsDT.AcceptChanges()
        Catch ex As Exception
            Throw
        End Try

    End Sub

#End Region

#Region "CRUD"

    Public Shared Sub RefreshAccumulators()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Get the latest list of Accumulators from the database
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/15/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            _AccumulatorsDT.Clear()
            _AccumulatorsDT = AccumulatorDAL.GetAccumulators
            _AccumulatorsDT.AcceptChanges()

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Shared Sub ClearAllChanges()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Clears all of the changes made to the Accumulators DataTable
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/14/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            RefreshAccumulators()
            _DeleteList.Clear()

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Shared ReadOnly Property GetAccumulators() As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Get all accumulators that are in the database
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/15/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get

            Return _AccumulatorsDT

        End Get
    End Property

    Public Shared ReadOnly Property GetActiveAccumulators() As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	2/5/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Dim DRs() As DataRow
            Dim DT As DataTable

            Try

                DRs = _AccumulatorsDT.Select("ACTIVE_SW = true")

                DT = _AccumulatorsDT.Clone

                DT.BeginLoadData()
                For Each DR As DataRow In DRs
                    DR.EndEdit()
                    DT.ImportRow(DR)
                Next
                DT.EndLoadData()

                DT.PrimaryKey = New DataColumn() {DT.Columns("ACCUM_ID")}

                Return DT

            Catch ex As Exception
                Throw
            End Try
        End Get
    End Property

    Public Shared Function GetAccumulator(ByVal accumulatorID As Integer) As DataTable
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Get the Accumulator with a particular id
        ' </summary>
        ' <param name="accumulatorId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/15/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DT As DataTable
        Dim DRs() As DataRow

        Try
            DT = _AccumulatorsDT.Clone
            DT.BeginLoadData()

            DRs = _AccumulatorsDT.Select("ACCUM_ID = " & accumulatorID)
            For J As Integer = 0 To DRs.Length - 1
                DRs(J).EndEdit()
                DT.ImportRow(DRs(J))
            Next
            DT.EndLoadData()

            Return DT
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetAccumulatorValueType(ByVal accumulatorID As Integer) As Integer
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' returns the value type of the accumulator
        ' </summary>
        ' <param name="accumulatorId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/27/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            Return CInt(_AccumulatorsDT.Select("ACCUM_ID = " & accumulatorID.ToString)(0)("VALUE_TYPE"))
        Catch ex As Exception
            Throw New ConstraintException("Accumulator does not exist.")
        End Try
    End Function

    Public Shared Function GetAccumulatorValueTypeName(ByVal accumulatorID As Integer) As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the value type, in a english format, of that accumulator
        ' </summary>
        ' <param name="accumulatorId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	1/14/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            Return CStr(_AccumulatorsDT.Select("ACCUM_ID = " & accumulatorID.ToString)(0)("VALUE_TYPE_NAME"))
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetAccumulatorIsActive(ByVal accumulatorID As Integer) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' returns a bool based off if the Accumulator is an active Accumulator
        ' </summary>
        ' <param name="accumulatorId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/27/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            Return CBool(_AccumulatorsDT.Select("ACCUM_ID = " & accumulatorID.ToString)(0)("ACTIVE_SW"))
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function GetAccumulatorIsFamily(ByVal accumulatorID As Integer) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' returns a bool based off if the Accumulator is a Family Accumulator
        ' </summary>
        ' <param name="accumulatorId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/27/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            Return CBool(_AccumulatorsDT.Select("ACCUM_ID = " & accumulatorID.ToString)(0)("FAMILY_SW"))
        Catch ex As Exception
            Throw New ConstraintException("Accumulator does not exist.")
        End Try
    End Function

    Public Shared Function GetAccumulatorIsManual(ByVal accumulatorID As Integer) As Boolean
        Try
            Return CBool(_AccumulatorsDT.Select("ACCUM_ID = " & accumulatorID.ToString)(0)("MANUAL_SW"))
        Catch ex As Exception
            Throw New ConstraintException("Accumulator does not exist.")
        End Try
    End Function

    Public Shared Function GetAccumulatorIsBatch(ByVal accumulatorID As Integer) As Boolean
        Try
            Return CBool(_AccumulatorsDT.Select("ACCUM_ID = " & accumulatorID.ToString)(0)("BATCH_SW"))
        Catch ex As Exception
            Throw New ConstraintException("Accumulator does not exist.")
        End Try
    End Function

    Public Shared Function GetAccumulatorIsAccident(ByVal accumulatorID As Integer) As Boolean
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="accumulatorId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	5/3/2007	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            Return CBool(_AccumulatorsDT.Select("ACCUM_ID = " & accumulatorID.ToString)(0)("ACCIDENT_SW"))
        Catch ex As Exception
            Throw New ConstraintException("Accumulator does not exist.")
        End Try
    End Function

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Gets the Id of the accumulator
    ' </summary>
    ' <param name="accumulatorName"></param>
    ' <returns></returns>
    ' <remarks>
    ' </remarks>
    ' <history>
    ' 	[paulw]	12/16/2005	Created
    ' </history>
    ' -----------------------------------------------------------------------------
    Public Shared Function GetAccumulatorID(ByVal accumulatorName As String) As Integer?

        Try
            If accumulatorName.Length < 1 Then Return Nothing

            If _AccumulatorsDT.Select("ACCUM_NAME = '" & accumulatorName.ToString & "'").Length > 0 Then
                Return CInt(_AccumulatorsDT.Select("ACCUM_NAME = '" & accumulatorName.ToString & "'")(0)("ACCUM_ID"))
            End If

            Return Nothing

        Catch ex As Exception
            Throw New ConstraintException("Accumulator does not exist.")
        Finally
        End Try
    End Function

    Public Shared Function GetAccumulatorName(ByVal accumulatorID As Integer) As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' returns the name of the accumulator
        ' </summary>
        ' <param name="accumulatorId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/19/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            Return CStr(_AccumulatorsDT.Select("ACCUM_ID = " & accumulatorID.ToString)(0)("ACCUM_NAME")).Trim
        Catch ex As Exception
            Throw New ConstraintException("Accumulator does not exist.")
        End Try
    End Function

    Public Shared Function GetAccumulatorDescription(ByVal accumulatorID As Integer) As String
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Gets the description of the accumulator
        ' </summary>
        ' <param name="accumulatorId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/16/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            Return CStr(_AccumulatorsDT.Select("ACCUM_ID = " & accumulatorID.ToString)(0)("ACCUM_DESC")).Trim
        Catch ex As Exception
            Throw New ConstraintException("Accumulator does not exist.")
        End Try
    End Function

    Public Shared Sub InsertAccumulator(ByVal accumulatorName As String, ByVal valueType As MemberAccumulatorValueType, ByVal isActive As Boolean, ByVal isFamily As Boolean, ByVal description As String, ByVal isAccident As Boolean, ByVal displayOrder As Integer, ByVal userID As String)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' adds a new accumulator
        ' </summary>
        ' <param name="accumulatorName"></param>
        ' <param name="accumulatorType"></param>
        ' <param name="valueType"></param>
        ' <param name="isActive"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/16/2005	Created
        '     [paulw] 10/9/2006   Added Display Order Per ACR MED-0048
        ' </history>
        ' -----------------------------------------------------------------------------

        Dim DR As DataRow

        Try

            DR = _AccumulatorsDT.NewRow

            DR("ACCUM_ID") = System.DBNull.Value
            DR("ACCUM_NAME") = accumulatorName
            DR("VALUE_TYPE") = valueType
            DR("VALUE_TYPE_NAME") = If(valueType = MemberAccumulatorValueType.Currency, "CURRENCY", "UNITS")
            DR("ACTIVE_SW") = isActive
            DR("FAMILY_SW") = isFamily
            DR("ACCUM_DESC") = description
            DR("DISPLAY_ORDER") = displayOrder
            DR("ACCIDENT_SW") = isAccident
            DR("CREATE_USERID") = userID
            DR("CREATE_DATE") = UFCWGeneral.NowDate.ToString
            DR("UserId") = userID
            DR("LastUpdt") = UFCWGeneral.NowDate.ToString
            _AccumulatorsDT.Rows.Add(DR)

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Function GetNextAccidentAccumulator(ByVal accumulatorName As String) As Integer ', ByVal valueType As MemberAccumulator.MemberAccumulatorValueTypes, ByVal isActive As Boolean, ByVal isFamily As Boolean, ByVal description As String, ByVal isAccident As Boolean, ByVal displayOrder As Integer, ByVal userId As String)
        Return AccumulatorDAL.GetNextAccidentAccumulator(accumulatorName)
    End Function

    Public Shared Function GetAccumulatorDisplayOrder(ByVal accumulatorID As Integer) As Integer
        ' -----------------------------------------------------------------------------
        ' <summary>
        '
        ' </summary>
        ' <param name="accumulatorId"></param>
        ' <returns></returns>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	10/17/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try
            Return Convert.ToInt32(_AccumulatorsDT.Select("ACCUM_ID = " & accumulatorID.ToString)(0)("DISPLAY_ORDER"))
        Catch ex As Exception
            Throw New ConstraintException("Accumulator does not exist.")
        End Try
    End Function

    Public Shared Sub ModifyAccumulator(ByVal accumulatorID As Integer, ByVal accumulatorName As String, ByVal valueType As MemberAccumulatorValueType, ByVal isActive As Boolean, ByVal isFamily As Boolean, ByVal description As String, ByVal isAccident As Boolean, ByVal displayOrder As Integer, ByVal userId As String)

        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Modify the accumulator that matches the given id
        ' </summary>
        ' <param name="accumulatorId"></param>
        ' <param name="accumulatorName"></param>
        ' <param name="accumulatorType"></param>
        ' <param name="valueType"></param>
        ' <param name="isActive"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/16/2005	Created
        '     [paulw] 10/9/2006   Added Display Order Per ACR MED-0048
        ' </history>
        ' -----------------------------------------------------------------------------

        Try

            For I As Integer = 0 To _AccumulatorsDT.Rows.Count - 1
                If CInt(_AccumulatorsDT.Rows(I)("ACCUM_ID")) = accumulatorID Then
                    _AccumulatorsDT.Rows(I)("ACCUM_NAME") = accumulatorName
                    _AccumulatorsDT.Rows(I)("VALUE_TYPE") = valueType
                    _AccumulatorsDT.Rows(I)("VALUE_TYPE_NAME") = If(valueType = MemberAccumulatorValueType.Currency, "CURRENCY", "UNITS")
                    _AccumulatorsDT.Rows(I)("FAMILY_SW") = isFamily
                    _AccumulatorsDT.Rows(I)("ACCUM_DESC") = description
                    _AccumulatorsDT.Rows(I)("ACTIVE_SW") = isActive
                    _AccumulatorsDT.Rows(I)("DISPLAY_ORDER") = displayOrder
                    _AccumulatorsDT.Rows(I)("ACCIDENT_SW") = isAccident
                    _AccumulatorsDT.Rows(I)("UserId") = userId
                    _AccumulatorsDT.Rows(I)("LastUpdt") = UFCWGeneral.NowDate.ToString
                End If
            Next

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Shared Sub CommitAll()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Saves all chnaged/added accumulators
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/16/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Dim DT As DataTable
        Dim DR As DataRow

        Try

            DT = New DataTable

            For X As Integer = 0 To _AccumulatorsDT.Columns.Count - 1
                DT.Columns.Add(_AccumulatorsDT.Columns(X).ColumnName)
            Next

            'loop through all rows
            For I As Integer = 0 To _AccumulatorsDT.Rows.Count - 1
                'if the row had changes
                If _AccumulatorsDT.Rows(I).RowState = DataRowState.Modified Then
                    DR = DT.NewRow
                    'add all proposed changes to the new row
                    For X As Integer = 0 To _AccumulatorsDT.Columns.Count - 1
                        If _AccumulatorsDT.Rows(I).HasVersion(DataRowVersion.Current) Then
                            DR(X) = _AccumulatorsDT.Rows(I)(X, DataRowVersion.Current)
                        Else
                            DR(X) = _AccumulatorsDT.Rows(I)(X, DataRowVersion.Original)
                        End If
                    Next
                    'add the row to the new table
                    DT.Rows.Add(DR)
                ElseIf _AccumulatorsDT.Rows(I).RowState = DataRowState.Added Then
                    DR = DT.NewRow
                    For X As Integer = 0 To _AccumulatorsDT.Columns.Count - 1
                        DR(X) = _AccumulatorsDT.Rows(I)(X, DataRowVersion.Current)
                    Next
                    'add the row to the new table
                    DT.Rows.Add(DR)
                End If
            Next

            'commit all the chnages to the database
            AccumulatorDAL.CommitAccumulators(DT, _DeleteList)

            _DeleteList.Clear()

            RefreshAccumulators()

            _AccumulatorsDT.AcceptChanges()

        Catch ex As ConstraintException
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub DeleteAccumulator(ByVal accumulatorID As Integer)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Deletes an accumulator.  CommitAll must be called for this to commit to the database
        ' </summary>
        ' <param name="accumulatorId"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	12/16/2005	Created
        ' </history>
        ' -----------------------------------------------------------------------------

        Try
            For I As Integer = 0 To _AccumulatorsDT.Rows.Count - 1
                If Not _AccumulatorsDT.Rows(I).RowState = DataRowState.Deleted Then
                    If CInt(_AccumulatorsDT.Rows(I)("ACCUM_ID")) = accumulatorID Then
                        _AccumulatorsDT.Rows(I).Delete()
                        _DeleteList.Add(accumulatorID)
                        Exit For
                    End If
                End If
            Next
        Catch ex As Exception
            Throw New ConstraintException("Accumulator does not exist.")
        End Try
    End Sub
#End Region

    Protected Overrides Sub Finalize()
        If _DeleteList IsNot Nothing Then _DeleteList = Nothing
        If _AccumulatorsDT IsNot Nothing Then
            _AccumulatorsDT.Dispose()
        End If
        MyBase.Finalize()
    End Sub
End Class