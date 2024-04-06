Option Infer On
Option Strict On

Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.Threading

#Region "BackThread Classes"
Public Class ExecuteDuplicates

    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _DupDataset As DataSet
    Private _ClaimDataset As ClaimDataset
    Private _TransmitClaimInfo As Boolean

    Private _SynchronizingObject As System.ComponentModel.ISynchronizeInvoke
    Private _NotifyDelegate As NotifyProgress

    Public Delegate Sub NotifyProgress(ByVal DuplicatesDS As DataSet)

    Private Sub NotifyUI(ByVal duplicatesDS As DataSet)

        Dim Args(0) As Object

        Try

            Args(0) = duplicatesDS
#If DEBUG Then
            Debug.Print("ExecuteDuplicates NotifyUI -> " & _SynchronizingObject.GetType.ToString)
#End If
            If _SynchronizingObject.InvokeRequired Then
                _SynchronizingObject.Invoke(_NotifyDelegate, Args)
            End If

        Catch ex As Exception
        End Try

    End Sub

    Public Sub New(ByVal synchronizingObject As System.ComponentModel.ISynchronizeInvoke, ByVal notifyDelegate As NotifyProgress, ByVal duplicateDS As DataSet, ByVal claimDS As ClaimDataset, ByVal transmitClaimInfo As Boolean)
        _DupDataset = duplicateDS
        _ClaimDataset = claimDS
        _TransmitClaimInfo = transmitClaimInfo
        _SynchronizingObject = synchronizingObject
        _NotifyDelegate = notifyDelegate
    End Sub

    Public Sub Execute()

        Dim MeddtlCurrentRows As DataTable

        Try
            If _TransmitClaimInfo Then

                MeddtlCurrentRows = _ClaimDataset.MEDDTL.Clone

                MeddtlCurrentRows.BeginLoadData()
                For Each DRV As DataRowView In New DataView(_ClaimDataset.MEDDTL, "STATUS <> 'MERGED'", "", DataViewRowState.CurrentRows)
                    MeddtlCurrentRows.ImportRow(DRV.Row)
                Next
                MeddtlCurrentRows.EndLoadData()

                For Each DR As DataRow In MeddtlCurrentRows.Rows
                    CMSDALFDBMD.CreateCurrentDetails(CInt(DR("CLAIM_ID")), CShort(DR("LINE_NBR")), UFCWGeneral.IsNullDateHandler(DR("OCC_FROM_DATE"), "OCC_FROM_DATE"), UFCWGeneral.IsNullDateHandler(DR("OCC_TO_DATE"), "OCC_TO_DATE"), TryCast(DR("PROC_CODE"), String), UFCWGeneral.IsNullDecimalHandler(DR("CHRG_AMT"), "CHRG_AMT"), TryCast(DR("MODIFIER"), String))
                Next
            End If

            _DupDataset = CMSDALFDBMD.RetrieveDuplicates(CInt(_ClaimDataset.MEDHDR.Rows(0)("CLAIM_ID")), CInt(_ClaimDataset.MEDHDR.Rows(0)("FAMILY_ID")), CShort(_ClaimDataset.MEDHDR.Rows(0)("RELATION_ID")), UFCWGeneral.IsNullIntegerHandler(_ClaimDataset.MEDHDR.Rows(0)("PROV_TIN"), "PROV_TIN"), UFCWGeneral.IsNullDateHandler(_ClaimDataset.MEDHDR.Rows(0)("OCC_FROM_DATE"), "OCC_FROM_DATE"), CDec(_TransmitClaimInfo))

            NotifyUI(_DupDataset)

        Catch ex As Exception
            If System.Threading.Thread.CurrentThread.ThreadState <> ThreadState.AbortRequested Then
                Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                If (Rethrow) Then
                    Throw
                Else
                    MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        End Try
    End Sub
End Class

Public Class BackThread

    Private Shared _TraceSwitch As New BooleanSwitch("TraceCloning", "Trace Switch in App.Config")

    Private _AssociatedDataset As DataSet
    Private _ClaimDataset As ClaimDataset
    Private _AuditMode As Boolean

    Private _SynchronizingObject As System.ComponentModel.ISynchronizeInvoke
    Private _NotifyDelegate As NotifyProgress

    Public Delegate Sub NotifyProgress(ByVal associatedDS As DataSet)

    Private Sub NotifyUI(ByVal associatedDS As DataSet)
        Dim Args(0) As Object

        Try
            Args(0) = associatedDS

#If DEBUG Then
            Debug.Print("Execute Associated NotifyUI -> " & _SynchronizingObject.GetType.ToString)
#End If
            If _SynchronizingObject.InvokeRequired Then
                _SynchronizingObject.Invoke(_NotifyDelegate, Args)
            End If

        Catch ex As Exception
        End Try

    End Sub

    Public Sub New(ByVal synchronizingObject As System.ComponentModel.ISynchronizeInvoke, ByVal notifyDelegate As NotifyProgress, ByVal associatedDS As DataSet, ByVal claimDS As ClaimDataset, ByVal auditMode As Boolean)
        _AssociatedDataset = associatedDS
        _ClaimDataset = claimDS
        _SynchronizingObject = synchronizingObject
        _NotifyDelegate = notifyDelegate
        _AuditMode = auditMode
    End Sub

    Public Sub Execute()

        Try

            _AssociatedDataset = CMSDALFDBMD.RetrieveAssociatedClaims(CInt(_ClaimDataset.MEDHDR.Rows(0)("CLAIM_ID")), _AuditMode)

            NotifyUI(_AssociatedDataset)

        Catch ex As Exception
            If System.Threading.Thread.CurrentThread.ThreadState <> ThreadState.AbortRequested Then
                Dim Rethrow As Boolean = ExceptionPolicy.HandleException(ex, "General")
                If (Rethrow) Then
                    Throw
                Else
                    MessageBox.Show(ex.Message & ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        End Try
    End Sub
End Class


#End Region