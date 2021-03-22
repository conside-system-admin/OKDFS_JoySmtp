Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Excel
Imports System.Reflection.MethodBase
Imports System.Runtime.InteropServices.Marshal

Imports JoySock.LogOut.HPFLogOut
Imports JoySock.Data.HPFData
Imports JoySock.Data

Namespace FileOperation
    Public Class HPFExcelWriter
        Inherits HPFWriter

#Region "変数"
        Private i_xlsApplication As Application = Nothing
        Private i_xlsBooks As Workbooks = Nothing
        Private i_xlsBook As Workbook = Nothing
        Private i_xlsSheets As Sheets = Nothing
        Private i_xlsSheet As Worksheet = Nothing
        Private i_xlsRange As Range = Nothing
#End Region

#Region "コンストラクタ"
        Public Sub New()
        End Sub
#End Region

#Region "メソッド"
#Region "ファイルの操作"
        ''' <summary>
        ''' ファイルの開き
        ''' </summary>
        ''' <param name="strDefFileName">デフォルトファイルの名前</param>
        ''' <returns>ファイルの状態</returns>
        ''' <remarks></remarks>
        Public Function OpenExcelFile(Optional ByVal strDefFileName As String = "") As FileOutputStatus
            Try
                'パスまたはファイル名がシステム定義の最大長よりも長いとき、エラーを表示します
                If ShowFileDialog(FILE_EXT_NAME_XLSX, FILE_FILTER_XLSX, strDefFileName) = False Then
                    Return FileOutputStatus.FileNameOutOfSize
                End If

                If String.Empty.Equals(i_FileName) = True Then
                    Return FileOutputStatus.Cancel
                End If

                i_IsOpend = True
                Return FileOutputStatus.Success
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        Public Sub SaveExcelFile()
            Try
                For i As Integer = 0 To i_xlsSheets.Count - 1
                    DirectCast(i_xlsSheets(i + 1), Worksheet).Columns.NumberFormat = "@"
                Next
                i_xlsBook.SaveAs(i_FileName, _
                                 Type.Missing, Type.Missing, Type.Missing, _
                                 Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, _
                                 Type.Missing, Type.Missing, Type.Missing, _
                                 Type.Missing, Type.Missing)
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Sub

        Public Sub EndXlsOutput()
            Try
                ReleaseLocalObject(i_xlsRange)
                ReleaseLocalObject(i_xlsSheet)
                ReleaseLocalObject(i_xlsSheets)
                If String.Empty.Equals(i_FileName) = False And i_IsOpend = True Then
                    i_xlsBook.Close()
                    i_IsOpend = False
                End If
                ReleaseLocalObject(i_xlsBook)
                ReleaseLocalObject(i_xlsBooks)
                If IsNothing(i_xlsApplication) = False Then
                    i_xlsApplication.Quit()
                End If

                ReleaseLocalObject(i_xlsApplication)
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Sub

        Private Sub ReleaseLocalObject(ByVal objTarget As Object)
            Try
                If IsNothing(objTarget) = False Then
                    ReleaseComObject(objTarget)
                End If
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelWriter", GetCurrentMethod.Name)
                Throw ex

            End Try
        End Sub
#End Region

#Region "初期化"
        ''' <summary>
        ''' 初期化を行います
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub BeginXlsOutput()
            Try
                i_xlsApplication = New Application
                '保存時の確認ダイアログを表示しない
                i_xlsApplication.DisplayAlerts = False

                i_xlsBooks = i_xlsApplication.Workbooks

                i_xlsBook = i_xlsBooks.Add(XlWBATemplate.xlWBATWorksheet)

                i_xlsSheets = i_xlsBook.Worksheets
                i_xlsSheet = DirectCast(i_xlsSheets(1), Worksheet)

                 
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Sub

#End Region

#Region "シート"
        ''' <summary>
        ''' 新規のシートを追加します
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AddXlSheet()
            Try
                AddXlSheet(i_xlsBook.Sheets.Count + 1)
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' 新規のシートを追加します
        ''' </summary>
        ''' <param name="intSheetIndex">シートのインデックス</param>
        ''' <remarks></remarks>
        Public Sub AddXlSheet(ByVal intSheetIndex As Integer)
            Try
                i_xlsSheet = DirectCast(i_xlsBook.Sheets.Add(), Worksheet)
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' アクティブシートの設定
        ''' </summary>
        ''' <param name="intSheetIndex">シートのインデックス</param>
        ''' <param name="strSheetName">シートの名前</param>
        ''' <remarks></remarks>
        Public Sub SetActiveSheet(ByVal intSheetIndex As Integer, _
                                  Optional ByVal strSheetName As String = "")
            Try
                i_xlsSheet = DirectCast(i_xlsApplication.ActiveWorkbook.Sheets(intSheetIndex), Worksheet)

                If String.Empty.Equals(strSheetName) = False Then
                    i_xlsSheet.Name = strSheetName
                End If

                'i_xlsSheet.Columns.Formula = "@"
                i_xlsSheet.Select(Type.Missing)
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Sub
#End Region

#Region "範囲"
        ''' <summary>
        ''' セルに値を設定
        ''' </summary>
        ''' <param name="intRowIndex">行のインデックス</param>
        ''' <param name="intColIndex">列のインデックス</param>
        ''' <param name="objVal">値</param>
        ''' <param name="objNumFmtLcalModels">数字の形式</param>
        ''' <remarks></remarks>
        Public Sub XlsOutputRange(ByVal intRowIndex As Integer, _
                                  ByVal intColIndex As Integer, _
                                  ByVal objVal As Object, _
                                  Optional ByVal objNumFmtLcalModels As NumberFormatLocalModels = NumberFormatLocalModels.None)
            Try
                i_xlsRange = DirectCast(i_xlsSheet.Cells.Item(intRowIndex, intColIndex), Range)
                With i_xlsRange
                    .NumberFormatLocal = GetNumberFormatLocal(objNumFmtLcalModels)
                    .Value2 = objVal
                End With

            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' セルに値を設定
        ''' </summary>
        ''' <param name="intRowIndex">行のインデックス</param>
        ''' <param name="intColIndex">列のインデックス</param>
        ''' <param name="intColWidth">複数のため、列の数</param>
        ''' <param name="objVal">値</param>
        ''' <param name="objNumFmtLcalModels">数字の形式</param>
        ''' <remarks></remarks>
        Public Sub XlsOutputRange(ByVal intRowIndex As Integer, _
                                  ByVal intColIndex As Integer, _
                                  ByVal intColWidth As Integer, _
                                  ByVal objVal As Object, _
                                   ByVal objColor As Color, _
                                  Optional ByVal objNumFmtLcalModels As NumberFormatLocalModels = NumberFormatLocalModels.None)
            Try
                i_xlsRange = DirectCast(i_xlsSheet.Cells.Item(intRowIndex, intColIndex), Range)
                With i_xlsRange
                    .NumberFormatLocal = GetNumberFormatLocal(objNumFmtLcalModels)
                    .Value2 = objVal
                    .ColumnWidth = intColWidth
                    .Cells.Interior.Color = objColor
                End With

            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' セルの配置の設定
        ''' </summary>
        ''' <param name="intRowIndex">行のインデックス</param>
        ''' <param name="intColIndex">列のインデックス</param>
        ''' <param name="objHAlign">横のタイプ</param>
        ''' <param name="objVAlign">縦のタイプ</param>
        ''' <remarks></remarks>
        Public Sub SetOutputRangeAlign(ByVal intRowIndex As Integer, _
                                       ByVal intColIndex As Integer, _
                                       Optional ByVal objHAlign As XlHAlign = XlHAlign.xlHAlignLeft, _
                                       Optional ByVal objVAlign As XlVAlign = XlVAlign.xlVAlignTop)
            Try
                i_xlsRange = DirectCast(i_xlsSheet.Cells.Item(intRowIndex, intColIndex), Range)
                With i_xlsRange
                    .HorizontalAlignment = objHAlign
                    .VerticalAlignment = objVAlign
                End With
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Sub

        Public Sub SetRangBorderLine(Optional ByVal objLineStyle As XlLineStyle = XlLineStyle.xlContinuous, _
                                     Optional ByVal objWeight As XlBorderWeight = XlBorderWeight.xlThin, _
                                     Optional ByVal blnIsSetBackColor As Boolean = False)
            Try
                With i_xlsRange.Borders
                    .LineStyle = XlLineStyle.xlContinuous
                    .Weight = objWeight
                    If blnIsSetBackColor = True Then
                        .ColorIndex = 15
                    End If
                End With
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' セルのグループ化
        ''' </summary>
        ''' <param name="intRowSIndex"></param>
        ''' <param name="intColSIndex"></param>
        ''' <param name="intRowEIndex"></param>
        ''' <param name="intColEIndex"></param>
        ''' <remarks></remarks>
        Public Sub XlsOutputGroup(ByVal intRowSIndex As Integer, ByVal intColSIndex As Integer, _
                                  ByVal intRowEIndex As Integer, ByVal intColEIndex As Integer)
            Try
                Dim objRange As Range = i_xlsSheet.get_Range(i_xlsSheet.Cells(intRowSIndex, intColSIndex), _
                                                             i_xlsSheet.Cells(intRowEIndex, intColEIndex))
                i_xlsSheet.Columns.Select()
                objRange.Group(Type.Missing, Type.Missing, Type.Missing, Type.Missing)
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' 数字の形式の表示
        ''' </summary>
        ''' <param name="objNumFmtLcalModels">形式</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetNumberFormatLocal(ByVal objNumFmtLcalModels As NumberFormatLocalModels) As String
            Try
                Dim strNumFormLocal As String = String.Empty

                If objNumFmtLcalModels = NumberFormatLocalModels.Text Then
                    strNumFormLocal = STRING_AT
                End If
                Return strNumFormLocal
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

#End Region

#Region "合計公式"

#End Region
#End Region

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If i_IsDisposed = False Then
                    If disposing Then
                        ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                        EndXlsOutput()
                    End If
                End If
                i_IsDisposed = True

            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Sub
    End Class
End Namespace
