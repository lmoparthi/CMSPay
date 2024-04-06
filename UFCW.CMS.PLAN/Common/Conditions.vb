Option Strict On

Imports System.Threading

''' -----------------------------------------------------------------------------
''' Project	 : UFCW.CMS.ProcessingEngine
''' Class	 : CMS.ProcessorEngine.Conditions
'''
''' -----------------------------------------------------------------------------
''' <summary>
''' Represents a collection of Conditions
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[paulw]	4/14/2006	Created
''' </history>
''' -----------------------------------------------------------------------------
<Serializable()> _
Public Class Conditions
    Inherits SortedList
    Implements ICloneable

    Private Shared ReadOnly _TraceCloning As New TraceSwitch("TraceCloning", "Trace Switch in App.Config", "0")

#Region "Constants"

    Private Const LifetimeDuration As Integer = 1000

#End Region

#Region "Properties"
    Default Public Property Item(ByVal index As Integer) As Condition
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' This property is used for 'looping' purposes (ex. condition(i) )
        ' </summary>
        ' <param name="index"></param>
        ' <value></value>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/14/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Get
            Return CType(MyBase.GetByIndex(index), Condition)
        End Get
        Set(ByVal value As Condition)
            Me.Item(index) = value
        End Set
    End Property
#End Region

#Region "Constructors"

    Public Sub New()
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' default constructor
        ' </summary>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        MyBase.New()
    End Sub

#End Region

#Region "Methods"
    Friend Function GetFirstNonFamilyCondition() As Condition
        Try

            For I As Integer = 0 To Me.Count - 1
                If Not AccumulatorController.GetAccumulatorIsFamily(CInt(AccumulatorController.GetAccumulatorID(Me(I).AccumulatorName))) Then
                    Return Me(I)
                End If
            Next

            Return Nothing

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function IsConfiguredForNonPar() As Boolean
        Try

            For I As Integer = 0 To Me.Count - 1
                If Not Me(I).UseInHeadroomCheck Then
                    Return True
                End If
            Next

            Return False

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function IsAccumulatorInConditions(ByVal accumulatorName As String) As Boolean
        Try

            For I As Integer = 0 To Me.Count - 1
                If Me(I).AccumulatorName.ToUpper.Trim = accumulatorName.ToUpper.Trim Then
                    Return True
                End If
            Next

            Return False

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shadows Sub Add(ByVal condition As Condition)
        ' -----------------------------------------------------------------------------
        ' <summary>
        ' Adds an action to this list of actions
        ' </summary>
        ' <param name="act"></param>
        ' <remarks>
        ' </remarks>
        ' <history>
        ' 	[paulw]	4/12/2006	Created
        ' </history>
        ' -----------------------------------------------------------------------------
        Try

            If Not MyBase.ContainsKey(condition.SortKey) Then MyBase.Add(condition.SortKey, condition)
            'Me.Sort()

        Catch ex As Exception
            Throw
        End Try

    End Sub

    'Public Sub Sort()
    '    ' -----------------------------------------------------------------------------
    '    ' <summary>
    '    ' Sorts the collection based off operand
    '    ' </summary>
    '    ' <remarks>
    '    ' </remarks>
    '    ' <history>
    '    ' 	[paulw]	11/2/2006	Created for purposes of Alerts.  The processor needs to process
    '    '                         the lowest operand first to check for headroom.  if headroom is violated
    '    '                         then all conditions with the same operand are exceeded and alerts for
    '    '                         all those accumulators should be thrown
    '    ' </history>
    '    ' -----------------------------------------------------------------------------

    '    Dim Collection As Collection
    '    Dim SortedList As SortedList

    '    Try
    '        Collection = New Collection
    '        SortedList = New SortedList

    '        For I As Integer = 0 To Me.List.Count - 1
    '            If Not SortedList.ContainsKey(DirectCast(Me.List(I), Condition).Operand) Then
    '                SortedList.Add(DirectCast(Me.List(I), Condition).Operand, DirectCast(Me.List(I), Condition).Operand)
    '            End If
    '        Next

    '        For I As Integer = 0 To SortedList.Count - 1
    '            For J As Integer = 0 To Me.List.Count - 1
    '                If CDbl(SortedList.GetByIndex(I)) = DirectCast(Me.List(J), Condition).Operand Then
    '                    Collection.Add(Me.List(J))
    '                End If
    '            Next
    '        Next

    '        Me.List.Clear()

    '        For I As Integer = 1 To Collection.Count
    '            Me.List.Add(Collection(I))
    '        Next

    '    Catch ex As Exception
    '        Throw
    '    Finally
    '        Collection = Nothing
    '        SortedList = Nothing
    '    End Try

    'End Sub

    'Public Function IndexOf(ByVal item As Condition) As Int32
    '    ' -----------------------------------------------------------------------------
    '    ' <summary>
    '    ' Gets the index of the condition
    '    ' </summary>
    '    ' <param name="item"></param>
    '    ' <returns></returns>
    '    ' <remarks>
    '    ' </remarks>
    '    ' <history>
    '    ' 	[paulw]	4/14/2006	Created
    '    ' </history>
    '    ' -----------------------------------------------------------------------------

    '    Dim Condition As Condition

    '    Try

    '        If Item Is Nothing Then Throw New ArgumentNullException("item")

    '        For I As Integer = 0 To Me.List.Count - 1
    '            Condition = DirectCast(Me.List(I), Condition)
    '        Next

    '    Catch ex As Exception
    '        Throw
    '    Finally
    '        Condition = Nothing
    '    End Try
    'End Function

    Public Function GetAccumulatorValues(ByVal relationID As Integer, ByVal familyID As Integer, ByVal relevantDate As Date, ByVal familyAccumulators As Boolean, ByVal memberAccumulatorManager As MemberAccumulatorManager) As DataTable
        Try

            Return GetAccumulatorValues(relationID, familyID, relevantDate, familyAccumulators, memberAccumulatorManager, False)

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function GetAccumulatorValues(ByVal relationID As Integer, ByVal familyID As Integer, ByVal relevantDate As Date, ByVal showFamilyAccumulators As Boolean, ByVal memberAccumulatorManager As MemberAccumulatorManager, ByVal ManualOnly As Boolean) As DataTable

        Dim Condition As Condition
        Dim DT As DataTable
        Dim DR As DataRow

        Dim IsFamilyAccumulator As Boolean = False
        Dim ManualAccum As Boolean = False
        Dim Contains As Boolean = False

        Try
            DT = CMSDALCommon.CreateAccumulatorTypeDT

            'Manual Only indicates only Manual Accumulators should be displayed (e.g MANUAL_SW = 1)
            memberAccumulatorManager.RefreshAccumulatorSummariesForMember(ManualOnly)

            '            DT.Columns.Add(New DataColumn("ACCUM_NAME", Type.GetType("System.String")))
            '            DT.Columns.Add(New DataColumn("ACCUM_VALUE", Type.GetType("System.Decimal")))
            '            DT.Columns.Add(New DataColumn("ORIGINAL_ACCUM_VALUE", Type.GetType("System.Decimal")))
            '            DT.Columns.Add(New DataColumn("DISPLAY_ORDER", Type.GetType("System.Int32")))
            '            DT.Columns.Add(New DataColumn("ACTIVE_SW", Type.GetType("System.Int32")))
            '            DT.Columns.Add(New DataColumn("MANUAL_SW", Type.GetType("System.Int32")))
            '            DT.Columns.Add(New DataColumn("BATCH_SW", Type.GetType("System.Int32")))
            '            DT.Columns.Add(New DataColumn("DURATION_TYPE", Type.GetType("System.Int32")))
            '            DT.Columns.Add(New DataColumn("YEARS", Type.GetType("System.Int32")))

            For I As Integer = 0 To Me.Count - 1
                Condition = CType(Me.Item(I), Condition)

                If Condition.AccumulatorYear = 0 OrElse Condition.AccumulatorYear = Year(relevantDate) Then
                    Contains = False
                    If Condition.AccumulatorName.Length > 0 Then
                        If Condition.AccumulatorName = "AC" Then    'if this is an accident condition
                            For F As Integer = 0 To AccumulatorController.GetAccumulators.Rows.Count - 1
                                If CStr(AccumulatorController.GetAccumulators().Rows(F)("ACCUM_NAME")).Trim.IndexOf("AC") >= 0 Then
                                    If CStr(AccumulatorController.GetAccumulators().Rows(F)("ACCUM_NAME")).Trim.Length > 3 Then
                                        Condition.AccumulatorName = CStr(AccumulatorController.GetAccumulators().Rows(F)("ACCUM_NAME"))
                                        IsFamilyAccumulator = AccumulatorController.GetAccumulatorIsFamily(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)))

                                        If IsFamilyAccumulator = showFamilyAccumulators Then
                                            If memberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)), Condition.DurationType, Condition.Duration, relevantDate, Condition.Direction) <> 0 OrElse Condition.AccumulatorName = "ACC01" Then
                                                DR = DT.NewRow
                                                DR("ACCUM_NAME") = Condition.AccumulatorName
                                                DR("ACTIVE_SW") = Condition.Active
                                                DR("MANUAL_SW") = Condition.Manual
                                                DR("BATCH_SW") = Condition.Batch
                                                DR("DURATION_TYPE") = Condition.DurationType
                                                DR("YEARS") = Condition.Duration
                                                DR("ACCUM_VALUE") = memberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)), Condition.DurationType, Condition.Duration, relevantDate, Condition.Direction)
                                                DR("ORIGINAL_ACCUM_VALUE") = DR("ACCUM_VALUE")
                                                DR("DISPLAY_ORDER") = AccumulatorController.GetAccumulatorDisplayOrder(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)))

                                                For X As Integer = 0 To DT.Rows.Count - 1
                                                    If DT.Rows(X)("ACCUM_NAME").ToString = DR("ACCUM_NAME").ToString Then
                                                        Contains = True
                                                    End If
                                                Next

                                                If Not Contains Then
                                                    DT.Rows.Add(DR)
                                                End If

                                            End If
                                        End If
                                    End If
                                End If
                            Next
                        Else    'not an accident condition

                            IsFamilyAccumulator = AccumulatorController.GetAccumulatorIsFamily(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)))

                            If ManualOnly Then
                                If AccumulatorController.GetAccumulatorIsManual(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName))) Then
                                    DR = DT.NewRow
                                    DR("ACCUM_NAME") = Condition.AccumulatorName
                                    DR("ACTIVE_SW") = Condition.Active
                                    DR("MANUAL_SW") = Condition.Manual
                                    DR("BATCH_SW") = Condition.Batch
                                    DR("DURATION_TYPE") = Condition.DurationType
                                    DR("YEARS") = Condition.Duration
                                    DR("ACCUM_VALUE") = memberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)), Condition.DurationType, Condition.Duration, relevantDate, Condition.Direction)
                                    DR("ORIGINAL_ACCUM_VALUE") = DR("ACCUM_VALUE")
                                    DR("DISPLAY_ORDER") = AccumulatorController.GetAccumulatorDisplayOrder(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)))

                                    For X As Integer = 0 To DT.Rows.Count - 1
                                        If DT.Rows(X)("ACCUM_NAME").ToString = DR("ACCUM_NAME").ToString Then
                                            Contains = True
                                        End If
                                    Next

                                    If Not Contains Then
                                        DT.Rows.Add(DR)
                                    End If

                                End If
                            Else
                                If IsFamilyAccumulator = showFamilyAccumulators AndAlso Condition.Duration <> LifetimeDuration Then 'Note LifeTime Accumulators will be handled elsewhere
                                    DR = DT.NewRow
                                    DR("ACCUM_NAME") = Condition.AccumulatorName
                                    DR("ACTIVE_SW") = Condition.Active
                                    DR("MANUAL_SW") = Condition.Manual
                                    DR("BATCH_SW") = Condition.Batch
                                    DR("DURATION_TYPE") = Condition.DurationType
                                    DR("YEARS") = Condition.Duration
                                    DR("ACCUM_VALUE") = memberAccumulatorManager.GetOriginalValue(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)), Condition.DurationType, Condition.Duration, relevantDate, Condition.Direction)
                                    DR("ORIGINAL_ACCUM_VALUE") = DR("ACCUM_VALUE")
                                    DR("DISPLAY_ORDER") = AccumulatorController.GetAccumulatorDisplayOrder(CInt(AccumulatorController.GetAccumulatorID(Condition.AccumulatorName)))

                                    For X As Integer = 0 To DT.Rows.Count - 1
                                        If DT.Rows(X)("ACCUM_NAME").ToString = DR("ACCUM_NAME").ToString Then
                                            Contains = True
                                        End If
                                    Next

                                    If Not Contains Then
                                        DT.Rows.Add(DR)
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next

            Return DT

        Catch ex As Exception
            Throw
        Finally

            Condition = Nothing
            DT = Nothing
            DR = Nothing

        End Try
    End Function

#End Region

#Region "Clone"
    Public Function DeepCopy() As Conditions

        Dim BeginTime As Date = UFCWGeneral.NowDate
        Dim ConditionsClone As Conditions

        Try

            ConditionsClone = ShallowCopy()

            For Each DictionaryEntry As DictionaryEntry In Me
                ConditionsClone.Add(DirectCast(DictionaryEntry.Value, Condition).DeepCopy)
            Next

            Return ConditionsClone

        Catch ex As Exception
            Throw
        Finally
            If CInt(_TraceCloning.Level) > 1 Then Trace.WriteLine(BeginTime.ToString("HH:mm:ss.fffffff") & " : Thread " & Environment.CurrentManagedThreadId.ToString & vbTab & New StackFrame(True).GetFileLineNumber.ToString & vbTab & New StackFrame(True).GetMethod.ToString & vbTab & vbTab & " : " & "Cloning duration: " & UFCWGeneral.NowDate.Subtract(BeginTime).TotalMilliseconds.ToString & "ms" & vbTab & " : Object: " & Me.GetType.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.Module.ToString & " : " & New System.Diagnostics.StackTrace(True).GetFrame(0).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(0).GetFileLineNumber.ToString & ")" & " < " & New System.Diagnostics.StackTrace(True).GetFrame(1).GetMethod.ToString & "  (" & New System.Diagnostics.StackTrace(True).GetFrame(1).GetFileLineNumber.ToString & ")", "TraceCloning" & vbTab)
        End Try

    End Function

    Public Shared Function ShallowCopy() As Conditions

        Try

            Return New Conditions

        Catch
            Throw
        Finally
        End Try

    End Function

    Public Function Clone() As Object Implements ICloneable.Clone

        Try

            Return DirectCast(CloneHelper.Clone(Me), ICloneable)

        Catch ex As Exception
            Throw
        Finally
        End Try

    End Function

#End Region
End Class