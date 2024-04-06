Option Strict Off
Option Infer On

Imports System.IO
Imports Microsoft.Office.Interop
Imports System.Text
Imports System.Runtime.InteropServices
Imports ClosedXML.Excel
Imports System.Data.DataTableExtensions

Public Class Export
    Implements IDisposable

    Private _DGTS As New DataGridTableStyle()
    Private _Cancel As Boolean = True

    Private _SaveAsName As String
    Private _StyleName As String = ""

    Public ReadOnly Property SaveAsName() As String
        Get
            SaveAsName = _SaveAsName
        End Get
    End Property

    Public Sub New(Optional ByVal styleName As String = "")
        _StyleName = styleName
    End Sub

#Region "IDisposable Support"
    Private _Disposed As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)

        If Not _Disposed Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).

                If _DGTS IsNot Nothing Then
                    _DGTS.Dispose()
                End If
                _DGTS = Nothing

            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        _Disposed = True
    End Sub

    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        GC.SuppressFinalize(Me)
    End Sub
#End Region

    Private Shared Function PrepareExcelWorkBook(ByRef workBookFileName As String, ByRef exportDT As DataTable) As String


        Dim WorkSheet As IXLWorksheet
        Try

            Dim DatetimeQuery = From dc As DataColumn In exportDT.Columns
                                Where dc.DataType Is GetType(System.DateTime)

            For Each DC In DatetimeQuery

                Dim DatetimeUpdateQuery = (From dr In exportDT.AsEnumerable()
                                           Where dr(DC.ColumnName) IsNot DBNull.Value AndAlso dr(DC.ColumnName) = New Date)

                DatetimeUpdateQuery.ToList.ForEach(Sub(x As DataRow) x(DC.ColumnName) = DBNull.Value)

            Next

            Using Workbook As New XLWorkbook
                exportDT.TableName = "Data"
                WorkSheet = Workbook.Worksheets.Add(exportDT)

                Workbook.SaveAs(workBookFileName)
                Return workBookFileName

            End Using

        Catch ex As Exception
            Throw
        Finally
            WorkSheet = Nothing
        End Try

    End Function

    Public Function Export(ByVal dg As DataGridCustom, ByVal tableName As String, ByVal selectedItemsOnly As Boolean, ByVal specificColumns() As String, formatOutput As Boolean, Optional ByVal saveAsName As String = "", Optional ByVal access As Boolean = False) As String

        If access = False Then Return False

        Dim Tot As Integer = dg.BindingContext(dg.DataSource, dg.DataMember).Count - 1

        If Tot < 0 Then
            MessageBox.Show("There is nothing to export.", "Nothing To Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return Nothing
        End If

        Dim TotCols As Integer = 0
        Dim FI As FileInfo
        Dim FF As Integer = FreeFile()
        Dim SB As New StringBuilder("")
        Dim TotRowsExported As Integer = 0
        Dim SpecificColumnsDT As DataTable

        Dim DT As DataTable

        Try
            DT = dg.GetCurrentDataTable

            _DGTS = CType(dg, DataGridCustom).TableStyles(If(dg.DataSource IsNot Nothing AndAlso dg.GetCurrentDataTable.TableName.Trim.Length > 0, dg.GetCurrentDataTable.TableName, "Default"))

            If saveAsName.Trim.Length = 0 Then
                _Cancel = True

                Using SDialog As New SaveFileDialog()

                    SDialog.Filter = "Text Files|*.txt|Excel Files|*.xlsx|All Files|*.*"
                    SDialog.Title = "Export File As..."
                    SDialog.DefaultExt = "xlsx"
                    SDialog.FilterIndex = 2
                    SDialog.FileName = "Report-" & Now.ToString("yyyymmdd")

                    AddHandler SDialog.FileOk, AddressOf SaveDialog_FileOk

                    SDialog.ShowDialog()

                    If _Cancel Then Return Nothing

                    saveAsName = SDialog.FileName
                End Using

            End If

            SB.Append(saveAsName & " after filename dialog ")
            Cursor.Current = Cursors.WaitCursor

            If specificColumns IsNot Nothing Then
                SpecificColumnsDT = New DataTable
                For x As Integer = 0 To specificColumns.Length - 1

                    SpecificColumnsDT.Columns.Add(specificColumns(x), If(DT.Columns(specificColumns(x)) Is Nothing, System.Type.GetType("System.String"), DT.Columns(specificColumns(x)).DataType))
                Next
            Else
                SpecificColumnsDT = DT.Clone
            End If

            If selectedItemsOnly Then
                Dim DRs As ArrayList = dg.GetSelectedDataRows

                For Each DR As DataRow In DRs
                    SpecificColumnsDT.ImportRow(DR)
                Next
            Else
                'For x As Integer = 0 To SpecificColumnsDT.Columns.Count - 1
                '    Debug.Print(SpecificColumnsDT.Columns(x).ToString)
                'Next
                For Each DR As DataRow In DT.Rows
                    SpecificColumnsDT.ImportRow(DR)
                Next
            End If

            SB.Append("Before PrepareExcelWorkBook")

            saveAsName = PrepareExcelWorkBook(saveAsName.Trim.ToUpper.Replace("TMP", "XLSX"), SpecificColumnsDT)

            SB.Append(saveAsName & " After PrepareExcelWorkBook")

            FI = New FileInfo(saveAsName)

            TotCols = SpecificColumnsDT.Columns.Count - 1
            TotRowsExported = SpecificColumnsDT.Rows.Count

            If LCase(FI.Extension) = ".xlsx" OrElse LCase(FI.Extension) = ".tmp" Then

                If formatOutput Then

                    ProcessExcel(FI, Tot, TotCols, TotRowsExported)

                End If

            End If

            Return FI.FullName

        Catch ex As IOException

            MessageBox.Show(ex.Message, SB.ToString, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            '            MessageBox.Show("The file you are attempting to export to is in use. Close the file or select a different file name to complete the export.", "File Already open", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return Nothing

        Catch ex As Exception
            Throw
        Finally
            If SpecificColumnsDT IsNot Nothing Then SpecificColumnsDT.Dispose()
            SpecificColumnsDT = Nothing
        End Try


    End Function

    Private Shared Sub ProcessExcel(ByRef fi As FileInfo, ByVal tot As Integer, ByVal totCols As Integer, ByVal totRowsExported As Integer)
        Try
            Using Workbook As New XLWorkbook(fi.FullName)
                Dim WorkSheet = Workbook.Worksheet("Data")
                WorkSheet.Range(WorkSheet.Cell(1, 1).Address, WorkSheet.Cell(totRowsExported + 1, totCols + 1).Address).Style.Border.OutsideBorder = XLBorderStyleValues.Thick
                WorkSheet.Range(WorkSheet.Cell(1, 1).Address, WorkSheet.Cell(1, totCols + 1).Address).Style.Border.OutsideBorder = XLBorderStyleValues.Thick
                WorkSheet.Range(WorkSheet.Cell(1, 1).Address, WorkSheet.Cell(1, totCols + 1).Address).Style.Font.SetBold(True)

                For Cnt = 0 To totCols - 1
                    WorkSheet.Range(WorkSheet.Cell(1, Cnt + 1), WorkSheet.Cell(totRowsExported + 1, Cnt + 2)).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thick)
                Next

                WorkSheet.PageSetup.PrintAreas.Add(WorkSheet.Cell(1, 1).Address, WorkSheet.Cell(totRowsExported + 1, totCols + 1).Address)
                WorkSheet.PageSetup.ShowGridlines = True
                WorkSheet.PageSetup.Margins.Left = 0.5
                WorkSheet.PageSetup.Margins.Right = 0.5
                WorkSheet.PageSetup.Margins.Top = 0.5
                WorkSheet.PageSetup.Margins.Bottom = 1
                WorkSheet.PageSetup.Margins.Footer = 0.15
                WorkSheet.PageSetup.Footer.Center.AddText(XLHFPredefinedText.PageNumber, XLHFOccurrence.AllPages)
                WorkSheet.PageSetup.Footer.Center.AddText(" / ", XLHFOccurrence.AllPages)
                WorkSheet.PageSetup.Footer.Center.AddText(XLHFPredefinedText.NumberOfPages, XLHFOccurrence.AllPages)
                Dim Totpages As Double = (totRowsExported) / 45

                WorkSheet.PageSetup.FitToPages(1, If(Totpages > 1, CInt(Totpages), 1))
                If totCols > 4 Then
                    WorkSheet.PageSetup.PageOrientation = XLPageOrientation.Landscape
                Else
                    WorkSheet.PageSetup.PageOrientation = XLPageOrientation.Portrait
                End If
                WorkSheet.PageSetup.ScaleHFWithDocument = True

                WorkSheet.Columns.AdjustToContents()

                Workbook.Save()
            End Using

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub Print(fileName As String)

        Dim XLApp As Excel.Application
        Dim XLBook As Excel.Workbook
        Dim XLBooks As Excel.Workbooks
        Dim XLSheet As Excel.Worksheet

        Try
            Using Workbook As New XLWorkbook(fileName)
                Dim WorkSheet = Workbook.Worksheet("Data")

                WorkSheet.PageSetup.PageOrientation = XLPageOrientation.Landscape
                WorkSheet.PageSetup.ScaleHFWithDocument = True
                WorkSheet.PageSetup.FitToPages(If((WorkSheet.Columns.Count \ 8) < 1, 1, (WorkSheet.Columns.Count \ 8)), If((WorkSheet.Rows.Count \ 40) < 1, 1, (WorkSheet.Rows.Count \ 40)))
                WorkSheet.PageSetup.Margins.Top = 0.5
                WorkSheet.PageSetup.Margins.Bottom = 1
                WorkSheet.PageSetup.Margins.Footer = 0.15
                WorkSheet.PageSetup.Footer.Center.AddText(XLHFPredefinedText.PageNumber, XLHFOccurrence.AllPages)
                WorkSheet.PageSetup.Footer.Center.AddText(" / ", XLHFOccurrence.AllPages)
                WorkSheet.PageSetup.Footer.Center.AddText(XLHFPredefinedText.NumberOfPages, XLHFOccurrence.AllPages)

                Workbook.Save()
            End Using

            XLApp = New Excel.Application With {
                    .EnableEvents = False,
                    .Visible = False,
                    .DisplayAlerts = False
                }

            XLBooks = XLApp.Workbooks
            XLBook = XLBooks.Open(fileName)
            XLSheet = CType(XLBook.Sheets("Data"), Excel.Worksheet)

            XLApp.ActiveWindow.Activate()
            XLApp.Dialogs(Excel.XlBuiltInDialog.xlDialogPrinterSetup).Show()

            XLBook.CheckCompatibility = False
            XLApp.Visible = False

            If XLSheet IsNot Nothing Then XLSheet.PrintOut()

            XLBook.Close()
            XLBooks.Close()
            XLApp.Quit()

        Catch ex As Exception
            Throw
        Finally

            If XLSheet IsNot Nothing Then Marshal.FinalReleaseComObject(XLSheet)
            If XLBook IsNot Nothing Then Marshal.FinalReleaseComObject(XLBook)
            If XLBooks IsNot Nothing Then Marshal.FinalReleaseComObject(XLBooks)
            If XLApp IsNot Nothing Then Marshal.FinalReleaseComObject(XLApp)

        End Try
    End Sub

    Public Sub Copy(ByVal grid As DataGrid, ByVal tableName As String, ByVal selectedItemsOnly As Boolean)

        Dim cnt As Integer
        Dim cpy As String = ""
        Dim ColCnt As Integer = 0
        Dim TotCols As Integer = 0
        Dim gcStyle As DataGridColumnStyle
        Dim tmpstr As String = ""
        Dim TotRowsExported As Integer = 0
        Dim tot As Integer = grid.BindingContext(grid.DataSource, grid.DataMember).Count - 1

        _DGTS = CType(grid, DataGridCustom).GetCurrentTableStyle

        If tot < 0 Then
            MessageBox.Show("There is nothing to Copy.", "Nothing To Copy", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Application.DoEvents()

        Try


            For Each gcStyle In _DGTS.GridColumnStyles
                TotCols += 1
            Next

            If selectedItemsOnly = False Then
ExportAll:
                For cnt = 0 To tot
                    tmpstr = ""

                    For ColCnt = 0 To TotCols - 1
                        If ColCnt = 0 Then
                            If IsDBNull(grid(cnt, ColCnt)) = False Then
                                tmpstr = CStr(grid(cnt, ColCnt))
                            Else
                                tmpstr = ""
                            End If
                        Else
                            If IsDBNull(grid(cnt, ColCnt)) = False Then
                                tmpstr += Microsoft.VisualBasic.vbTab & Replace(CStr(grid(cnt, ColCnt)), Microsoft.VisualBasic.vbCrLf, "    ")
                            Else
                                tmpstr += Microsoft.VisualBasic.vbTab & ""
                            End If
                        End If
                    Next ColCnt

                    If cpy = "" Then
                        cpy += tmpstr
                    Else
                        cpy += Microsoft.VisualBasic.vbCrLf & tmpstr
                    End If

                    TotRowsExported += 1
                Next cnt
            Else
                For cnt = 0 To tot
                    tmpstr = ""

                    If grid.IsSelected(cnt) = True Then
                        For ColCnt = 0 To TotCols - 1
                            If ColCnt = 0 Then
                                If IsDBNull(grid(cnt, ColCnt)) = False Then
                                    tmpstr = CStr(grid(cnt, ColCnt))
                                Else
                                    tmpstr = ""
                                End If
                            Else
                                If IsDBNull(grid(cnt, ColCnt)) = False Then
                                    tmpstr += Microsoft.VisualBasic.vbTab & Replace(CStr(grid(cnt, ColCnt)), Microsoft.VisualBasic.vbCrLf, "    ")
                                Else
                                    tmpstr += Microsoft.VisualBasic.vbTab & ""
                                End If
                            End If
                        Next ColCnt

                        If cpy = "" Then
                            cpy += tmpstr
                        Else
                            cpy += Microsoft.VisualBasic.vbCrLf & tmpstr
                        End If

                        TotRowsExported += 1
                    End If
                Next cnt

                If TotRowsExported = 0 Then
                    tmpstr = ""
                    For ColCnt = 0 To TotCols - 1
                        If tmpstr = "" Then
                            If IsDBNull(grid(grid.CurrentRowIndex, ColCnt)) = False Then
                                tmpstr = CStr(grid(grid.CurrentRowIndex, ColCnt))
                            Else
                                tmpstr = ""
                            End If
                        Else
                            If IsDBNull(grid(grid.CurrentRowIndex, ColCnt)) = False Then
                                tmpstr += Microsoft.VisualBasic.vbTab & Replace(CStr(grid(grid.CurrentRowIndex, ColCnt)), Microsoft.VisualBasic.vbCrLf, "    ")
                            Else
                                tmpstr += Microsoft.VisualBasic.vbTab & ""
                            End If
                        End If
                    Next ColCnt

                    If cpy = "" Then
                        cpy += tmpstr
                    Else
                        cpy += Microsoft.VisualBasic.vbCrLf & tmpstr
                    End If

                    TotRowsExported += 1
                End If
            End If

            Clipboard.SetDataObject(cpy, True)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub SaveDialog_FileOk(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        _Cancel = False
    End Sub

End Class

