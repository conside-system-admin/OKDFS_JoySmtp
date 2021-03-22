Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Excel
Imports System.Reflection.MethodBase
Imports System.Runtime.InteropServices.Marshal

Imports JoySock.LogOut.HPFLogOut
Imports JoySock.Data.HPFData
Imports JoySock.Data

Namespace FileOperation
    Public Class HPFExcelReader
        Inherits HPFReader
#Region "定数"
        Private Const FORMAT_STRING_DATE As String = "yyyy/MM/dd"
        Private Const FORMAT_STRING_DATETIME As String = "yyyy/MM/dd HH:mm:ss"
        Private Const FORMAT_SPACE As String = "/"
#End Region

#Region "変数"
        Private i_xlsApplication As Application = Nothing
        Private i_xlsBook As Workbook = Nothing
        Private i_xlsSheet As Worksheet = Nothing
        Private i_xlsSheets As Worksheets = Nothing
#End Region

#Region "ファイルの操作"
        ''' <summary>
        ''' ファイル選択ダイアログでファイルを開きます
        ''' </summary>
        ''' <returns>開き結果</returns>
        ''' <remarks></remarks>
        Public Function OpenExcelFileByDialog() As FileOutputStatus
            Try
                'パスまたはファイル名がシステム定義の最大長よりも長いとき、エラーを表示します
                If ShowFileDialog(FILE_EXT_NAME_XLSX, FILE_FILTER_XLSX) = False Then
                    Return FileOutputStatus.FileNameOutOfSize
                End If

                If String.Empty.Equals(i_FileName) = True Then
                    Return FileOutputStatus.Cancel
                End If

                Return OpenExcelFile()
            Catch ex As Exception
                ErrorOut(ex.Message, "HPFReader.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        ''' <summary>
        ''' ファイル名でファイルを開きます
        ''' </summary>
        ''' <param name="strFileName">ファイル名</param>
        ''' <returns>開き結果</returns>
        ''' <remarks></remarks>
        Public Function OpenExcelFileByFileName(ByVal strFileName As String) As FileOutputStatus
            Try
                i_FileName = strFileName

                Return OpenExcelFile()
            Catch ex As Exception
                ErrorOut(ex.Message, "HPFReader.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        ''' <summary>
        ''' ファイルを開きます
        ''' </summary>
        ''' <returns>開き結果</returns>
        ''' <remarks></remarks>
        Public Function OpenExcelFile() As FileOutputStatus
            Try
                If String.Empty.Equals(i_FileName) = True Then
                    Return FileOutputStatus.Cancel
                End If

                i_xlsApplication = New Application
                If IsNothing(i_xlsApplication) = True Then
                    Return FileOutputStatus.Faild
                End If

                i_xlsApplication.DisplayAlerts = False

                i_xlsBook = i_xlsApplication.Workbooks.Open(i_FileName, _
                                                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, _
                                                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, _
                                                            Type.Missing, Type.Missing, Type.Missing, Type.Missing)
                i_xlsSheet = i_xlsBook.Sheets(1)
                i_IsOpend = True
                Return FileOutputStatus.Success
            Catch ex As Exception
                ErrorOut(ex.Message, "HPFReader.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        Public Sub EndXlsOutput()
            Try
                ReleaseLocalObject(i_xlsSheet)
                If String.Empty.Equals(i_FileName) = False And i_IsOpend = True Then
                    i_xlsBook.Close()
                End If
                ReleaseLocalObject(i_xlsBook)
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

#Region "エクセルの内容の読み込み"

#Region "読み込まれたシートを選択します"
        Public Function SetActiveSheet(ByVal strSheetName As String) As Integer
            Try
                i_xlsSheet = Nothing
                i_xlsSheet = i_xlsBook.Sheets(strSheetName)
                i_xlsSheet.Select(Type.Missing)
                Return i_xlsSheet.UsedRange.Rows.Count
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function
        Public Function GetActiveSheetUsedRange_ColumnsCount(Optional ByVal strSheetName As String = "") As Decimal
            Try
                Dim decRet As Decimal = 0
                If strSheetName.Length > 0 Then
                    decRet = i_xlsBook.Sheets(strSheetName).UsedRange.Columns.Count
                Else
                    decRet = i_xlsSheet.UsedRange.Columns.Count
                End If
                Return decRet
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function
        Public Function GetActiveSheetUsedRange_RowsCount(Optional ByVal strSheetName As String = "") As Decimal
            Try
                Dim decRet As Decimal = 0
                If strSheetName.Length > 0 Then
                    decRet = i_xlsBook.Sheets(strSheetName).UsedRange.Rows.Count
                Else
                    decRet = i_xlsSheet.UsedRange.Rows.Count
                End If
                Return decRet
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function
        Public Function GetWorkBook_Version() As String
            Try
                Return i_xlsBook.Application.Version
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        Public Function GetAllSheetName() As String()
            Try
                Dim strSheetName As String()
                ReDim strSheetName(i_xlsBook.Sheets.Count - 1)
                For i = 1 To i_xlsBook.Sheets.Count
                    strSheetName(i - 1) = i_xlsBook.Sheets(i).name
                Next
                Return strSheetName
            Catch ex As Exception
                ErrorOut(ex.Message, "i_xlsBook.Sheets(i).name", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function
#End Region

#Region "数字-Integer"
        Public Function GetInteger(ByVal intRangeIndex As Integer) As Integer
            Try
                Dim objRange As Range = i_xlsSheet.get_Range(intRangeIndex, Type.Missing)

                Return ConvertToInteger(objRange)

            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        Public Function GetInteger(ByVal intRowIndex As Integer, _
                                   ByVal intColIndex As Integer) As Integer
            Try
                Dim objRange As Range = i_xlsSheet.Cells(intRowIndex, intColIndex)

                Return ConvertToInteger(objRange)
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        Private Function ConvertToInteger(ByVal objRange As Range) As Integer
            Try
                If IsNothing(objRange) = True Then
                    Return 0
                End If

                If IsNothing(objRange.Value2) = False Then
                    Return Convert.ToInt32(objRange.Value2)
                End If

                If IsNothing(objRange.get_Value(Type.Missing)) = False Then
                    Return Convert.ToInt32(objRange.get_Value(Type.Missing))
                End If

                Return 0
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function
#End Region

#Region "数字-Double"
        Public Function GetDouble(ByVal intRangeIndex As Integer) As Double
            Try
                Dim objRange As Range = i_xlsSheet.get_Range(intRangeIndex, Type.Missing)

                Return ConvertToDouble(objRange)

            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        Public Function GetDouble(ByVal intRowIndex As Integer, _
                                  ByVal intColIndex As Integer) As Double
            Try
                Dim objRange As Range = i_xlsSheet.Cells(intRowIndex, intColIndex)

                Return ConvertToDouble(objRange)
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        Private Function ConvertToDouble(ByVal objRange As Range) As Double
            Try
                If IsNothing(objRange) = True Then
                    Return 0.0
                End If

                If IsNothing(objRange.Value2) = False Then
                    Return Convert.ToDouble(objRange.Value2)
                End If

                If IsNothing(objRange.get_Value(Type.Missing)) = False Then
                    Return Convert.ToDouble(objRange.get_Value(Type.Missing))
                End If

                Return 0.0
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function
#End Region

#Region "文字列"
        Public Function GetString(ByVal intRangeIndex As Integer) As String
            Try
                Dim objRange As Range = i_xlsSheet.get_Range(intRangeIndex, Type.Missing)

                Return ConvertToString(objRange)

            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        Public Function GetString(ByVal intRowIndex As Integer, _
                                  ByVal intColIndex As Integer) As String
            Try
                Dim objRange As Range = i_xlsSheet.Cells(intRowIndex, intColIndex)

                Return ConvertToString(objRange)
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        Private Function ConvertToString(ByVal objRange As Range) As String
            Try
                If IsNothing(objRange.Text) = True Then
                    Return String.Empty
                End If

                If String.Empty.Equals(objRange.Text) = True Then
                    Return String.Empty
                End If

                If IsNothing(objRange.Value2) = False Then
                    Return Convert.ToString(objRange.Value2)
                End If

                If IsNothing(objRange.get_Value(Type.Missing)) = False Then
                    Return Convert.ToString(objRange.get_Value(Type.Missing))
                End If

                Return String.Empty
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function
#End Region

#Region "日付"
        Public Function GetDateString(ByVal intRangeIndex As Integer) As String
            Try
                Dim objRange As Range = i_xlsSheet.get_Range(intRangeIndex, Type.Missing)

                Return ConvertToDateTimeString(objRange, FORMAT_STRING_DATE)
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        Public Function GetDateString(ByVal intRowIndex As Integer, _
                                 ByVal intColIndex As Integer) As String
            Try
                Dim objRange As Range = i_xlsSheet.Cells(intRowIndex, intColIndex)

                Return ConvertToDateTimeString(objRange, FORMAT_STRING_DATE)
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function
#End Region

#Region "日時"
        Public Function GetDateTimeString(ByVal intRangeIndex As Integer) As String
            Try
                Dim objRange As Range = i_xlsSheet.get_Range(intRangeIndex, Type.Missing)

                Return ConvertToDateTimeString(objRange)
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        Public Function GetDateTimeString(ByVal intRowIndex As Integer, _
                                 ByVal intColIndex As Integer) As String
            Try
                Dim objRange As Range = i_xlsSheet.Cells(intRowIndex, intColIndex)

                Return ConvertToDateTimeString(objRange)
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        Private Function ConvertToDateTimeString(ByVal objRange As Range, _
                                                 Optional ByVal strFormat As String = FORMAT_STRING_DATETIME) As String
            Try
                Dim strTemp1 As String = ConvertToString(objRange)
                Dim strTemp2 As String = objRange.Text
                Dim dtValue As DateTime

                If strTemp1.IndexOf(FORMAT_SPACE) = -1 And strTemp2.IndexOf(FORMAT_SPACE) > 0 Then
                    dtValue = DateTime.FromOADate(Convert.ToDouble(strTemp1))
                Else
                    If DateTime.TryParse(strTemp1, dtValue) = False Then
                        Return String.Empty
                    End If
                End If

                Return dtValue.ToString(strFormat)
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFExcelReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function
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
