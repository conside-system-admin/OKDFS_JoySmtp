Imports System.Reflection.MethodBase

Imports JoySock.Common
Imports JoySock.LogOut.HPFLogOut
Imports JoySock.Data.HPFData
Imports JoySock.Data
Imports System.IO
Imports System.Text

Namespace FileOperation
    Public Class HPFCsvWriter
        Inherits HPFWriter

#Region "変数"
        Private i_Common As HPFCommon = Nothing
        Private i_Record As DataTable = Nothing
        Private i_Titles As String() = Nothing
        Private i_EmptyIndex As Integer = -1
#End Region
        Public Sub New()
            i_Common = New HPFCommon
            i_Record = Nothing
            i_Titles = Nothing
        End Sub

        Public WriteOnly Property EmptyIndex As Integer
            Set(ByVal value As Integer)
                i_EmptyIndex = value
            End Set
        End Property

        Public Function WriteToCsvWithOutDlg(ByVal dtRecord As DataTable, _
                                             ByVal strFilePath As String, _
                                             ByVal strFileName As String, _
                                             ByVal ParamArray strTitles As String())
            Try
                If strTitles.Length > 0 AndAlso dtRecord.Columns.Count <> strTitles.Count Then
                    Return FileOutputStatus.Faild
                End If

                If String.Empty.Equals(strFilePath) = True Or String.Empty.Equals(strFileName) = True Then
                    Return FileOutputStatus.Faild
                End If
                i_Titles = strTitles

                i_FileName = String.Format("{0}\{1}", strFilePath, strFileName)

                If Not Directory.Exists(strFilePath) Then
                    Directory.CreateDirectory(strFilePath)
                End If

                If File.Exists(i_FileName) = False Then
                    File.Create(i_FileName).Close()
                End If

                i_Record = dtRecord.Copy()

                Return WriteToCsvFile(True, strFileName)

            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFCsvWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        ''' <summary>
        ''' CSVファイル作成用で追加
        ''' </summary>
        ''' <param name="dtRecord"></param>
        ''' <param name="strFilePath"></param>
        ''' <param name="strFileName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function WriteToCsvWithOutDlg(ByVal dtRecord As DataTable,
                                             ByVal strFilePath As String,
                                             ByVal strFileName As String,
                                             Optional ByVal blnHasQuote As Boolean = True,
                                             Optional ByVal blnHasTitle As Boolean = True)

            Try
                If String.Empty.Equals(strFilePath) = True Or String.Empty.Equals(strFileName) = True Then
                    Return FileOutputStatus.Faild
                End If

                If blnHasTitle = True Then
                    Dim intIndex As Integer = 0
                    ReDim i_Titles(i_Record.Columns.Count - 1)

                    For intIndex = 0 To i_Record.Columns.Count - 1
                        i_Titles(intIndex) = i_Record.Columns(intIndex).ColumnName
                    Next
                End If

                i_FileName = String.Format("{0}\{1}", strFilePath, strFileName)

                If Not Directory.Exists(strFilePath) Then
                    Directory.CreateDirectory(strFilePath)
                End If

                If File.Exists(i_FileName) = False Then
                    File.Create(i_FileName).Close()
                End If

                i_Record = dtRecord.Copy()

                Return WriteToCsvFile(blnHasQuote, strFileName)

            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFCsvWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        ''' <summary>
        ''' CSV出力
        ''' </summary>
        ''' <param name="dtRecord">CSVデータ</param>
        ''' <param name="strTitles">CSVタイトル</param>
        ''' <param name="strDefFileName">ファイル名</param>
        ''' <param name="blnHasQuote">引用符号の有無</param>
        ''' <remarks></remarks>
        Public Function WriteToCsvBy(ByVal dtRecord As DataTable, _
                                     ByVal strTitles As String(), _
                                     Optional ByVal strDefFileName As String = "", _
                                     Optional ByVal blnHasQuote As Boolean = True) As FileOutputStatus
            Try
                If dtRecord.Columns.Count <> strTitles.Count Then
                    Return FileOutputStatus.Faild
                End If

                'パスまたはファイル名がシステム定義の最大長よりも長いとき、エラーを表示します
                If ShowFileDialog(FILE_EXT_NAME_CSV, FILE_FILTER_CSV, strDefFileName) = False Then
                    Return FileOutputStatus.FileNameOutOfSize
                End If

                If String.Empty.Equals(i_FileName) = True Then
                    Return FileOutputStatus.Cancel
                End If

                i_Record = dtRecord.Copy()
                i_Titles = strTitles

                Return WriteToCsvFile(blnHasQuote)
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFCsvWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        Public Function WriteToCsv(ByVal dtRecord As DataTable, _
                                   Optional ByVal strDefFilename As String = "", _
                                   Optional ByVal blnHasTitle As Boolean = True) As FileOutputStatus
            Try
                'パスまたはファイル名がシステム定義の最大長よりも長いとき、エラーを表示します
                If ShowFileDialog(FILE_EXT_NAME_CSV, FILE_FILTER_CSV, strDefFilename) = False Then
                    Return FileOutputStatus.FileNameOutOfSize
                End If

                If String.Empty.Equals(i_FileName) = True Then
                    Return FileOutputStatus.Cancel
                End If

                i_Record = dtRecord.Copy()

                If blnHasTitle = True Then
                    Dim intIndex As Integer = 0
                    ReDim i_Titles(i_Record.Columns.Count - 1)

                    For intIndex = 0 To i_Record.Columns.Count - 1
                        i_Titles(intIndex) = i_Record.Columns(intIndex).ColumnName
                    Next
                End If

                Return WriteToCsvFile()
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFCsvWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        ''' <summary>
        ''' CSV出力
        ''' </summary>
        ''' <param name="blnHasQuote">引用符号がある</param>
        ''' <remarks></remarks>
        Private Function WriteToCsvFile(Optional ByVal blnHasQuote As Boolean = True, Optional ByVal strFileName As String = "") As FileOutputStatus
            Dim objCsvStream As StreamWriter = Nothing
            Try
                Dim strName As String = String.Empty
                Dim strTemp As String = String.Empty
                Dim strLine As String = String.Empty
                Dim drRow As DataRow = Nothing
                Dim intColumIndex As Integer = 0

                objCsvStream = New StreamWriter(i_FileName, False, Encoding.GetEncoding("SHIFT-JIS"))

                'タイトル
                If IsNothing(i_Titles) = False Then
                    For Each strTemp In i_Titles
                        strLine = strLine + String.Format("{0}{1}", strTemp, FILE_FILTER_CSV_SAPATER)
                    Next

                    If strLine.Length > 0 Then
                        objCsvStream.WriteLine(strLine.Substring(0, strLine.Length - 1))
                    End If
                End If

                'CSVデータ(引用符号がある)
                If blnHasQuote Then

                    For Each drRow In i_Record.Rows

                        Dim blnIsEmptyRow As Boolean = False

                        strLine = String.Empty
                        For intColumIndex = 0 To drRow.ItemArray.Count - 1
                            strTemp = i_Common.ConvertToString(drRow.Item(intColumIndex))
                            strName = i_Common.ConvertToString(i_Record.Columns(intColumIndex).ColumnName)
                            If intColumIndex.Equals(i_EmptyIndex) AndAlso strTemp.Equals(String.Empty) Then
                                blnIsEmptyRow = True
                            End If

                            If blnIsEmptyRow = False Then

                                'CSV出力時、対象カラムに値がない場合、0を出力する。値がある場合、1を出力する。(無効FLG,取消FLG,削除FLG)
                                If ((strName = "MUKO_FLG") OrElse (strName = "MUKO_FLG1") _
                                    OrElse (strName = "DELETE_FLG") OrElse (strName = "DELETE_FLG1")) And _
                                        Not (strFileName.IndexOf("エラー") > -1) Then
                                    If (strTemp.Equals(String.Empty)) Then
                                        strLine = strLine + String.Format("{0}{1}{2}{3}", FILE_FILTER_CSV_QUOTE, _
                                                                          0, FILE_FILTER_CSV_QUOTE, FILE_FILTER_CSV_SAPATER)
                                    Else
                                        strLine = strLine + String.Format("{0}{1}{2}{3}", FILE_FILTER_CSV_QUOTE, _
                                                                          1, FILE_FILTER_CSV_QUOTE, FILE_FILTER_CSV_SAPATER)
                                    End If

                                    '日付形式：YYYY/MM/DD
                                ElseIf (strName = "YUKO_START") OrElse (strName = "YUKO_END") OrElse (strName = "CALENDAR_DATE") _
                                    OrElse (strName = "JUCHU_START_DATE") OrElse (strName = "JUCHU_END_DATE") OrElse _
                                    (strName = "HACHU_START_DATE") OrElse (strName = "HACHU_END_DATE") Then
                                    strLine = strLine + String.Format("{0}{1}{2}{3}", FILE_FILTER_CSV_QUOTE, _
                                                                      Left(strTemp, 10), FILE_FILTER_CSV_QUOTE, FILE_FILTER_CSV_SAPATER)

                                    'CSV出力時、「〒」と「-」を抜いて出力するように修正する。〒___-___
                                ElseIf strName = "YUBIN" Then
                                    strLine = strLine + String.Format("{0}{1}{2}{3}", FILE_FILTER_CSV_QUOTE, _
                                                                      strTemp.Replace("〒", "").Replace("_", "").Replace("-", ""), _
                                                                      FILE_FILTER_CSV_QUOTE, FILE_FILTER_CSV_SAPATER)
                                Else
                                    strLine = strLine + String.Format("{0}{1}{2}{3}", FILE_FILTER_CSV_QUOTE, _
                                                                      strTemp.Replace(vbLf, FILE_VBLF), FILE_FILTER_CSV_QUOTE, FILE_FILTER_CSV_SAPATER)
                                End If


                            End If
                        Next

                        If strLine.Length > 0 Then
                            objCsvStream.WriteLine(strLine.Substring(0, strLine.Length - 1))
                        Else
                            If blnIsEmptyRow Then
                                objCsvStream.WriteLine(String.Empty)
                            End If
                        End If
                    Next

                Else

                    'CSVデータ(引用符号がない)
                    For Each drRow In i_Record.Rows
                        Dim blnIsEmptyRow As Boolean = False

                        strLine = String.Empty
                        For intColumIndex = 0 To drRow.ItemArray.Count - 1
                            strTemp = i_Common.ConvertToString(drRow.Item(intColumIndex))
                            If intColumIndex.Equals(i_EmptyIndex) AndAlso strTemp.Equals(String.Empty) Then
                                blnIsEmptyRow = True
                            End If

                            If blnIsEmptyRow = False Then
                                strLine = strLine + String.Format("{0}{1}", strTemp, FILE_FILTER_CSV_SAPATER)
                            End If
                        Next

                        If strLine.Length > 0 Then
                            objCsvStream.WriteLine(strLine.Substring(0, strLine.Length - 1))
                        Else
                            If blnIsEmptyRow Then
                                objCsvStream.WriteLine(String.Empty)
                            End If
                        End If
                    Next
                End If

                Return FileOutputStatus.Success

            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFCsvWriter", GetCurrentMethod.Name)
                Throw ex
            Finally
                If IsNothing(objCsvStream) = False Then
                    objCsvStream.Close()
                    objCsvStream.Dispose()
                End If
                objCsvStream = Nothing
            End Try
        End Function



#Region "入出力管理画面"

        'ADD BY TXG 2014/05/06

        Public Function WriteToCsvByWithoutTitle(ByVal dtRecord As DataTable, _
                                    Optional ByVal strDefFileName As String = "") As FileOutputStatus
            Try

                'パスまたはファイル名がシステム定義の最大長よりも長いとき、エラーを表示します
                If ShowFileDialog(FILE_EXT_NAME_CSV, FILE_FILTER_CSV, strDefFileName) = False Then
                    Return FileOutputStatus.FileNameOutOfSize
                End If

                If String.Empty.Equals(i_FileName) = True Then
                    Return FileOutputStatus.Cancel
                End If

                i_Record = dtRecord.Copy()
                Return WriteToCsvFileWithoutTitle()
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFCsvWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        Private Function WriteToCsvFileWithoutTitle() As FileOutputStatus
            Dim objCsvStream As StreamWriter = Nothing
            Try
                Dim strTemp As String = String.Empty
                Dim strLine As String = String.Empty
                Dim drRow As DataRow = Nothing
                Dim intColumIndex As Integer = 0
                objCsvStream = New StreamWriter(i_FileName, False, Encoding.GetEncoding("SHIFT-JIS"))

                For Each drRow In i_Record.Rows
                    Dim blnIsEmptyRow As Boolean = False

                    strLine = String.Empty
                    For intColumIndex = 0 To drRow.ItemArray.Count - 1
                        strTemp = i_Common.ConvertToString(drRow.Item(intColumIndex))
                        If intColumIndex.Equals(i_EmptyIndex) AndAlso strTemp.Equals(String.Empty) Then
                            blnIsEmptyRow = True
                        End If

                        If blnIsEmptyRow = False Then
                            strLine = strLine + String.Format("{0}{1}{2}{3}", FILE_FILTER_CSV_QUOTE, strTemp, FILE_FILTER_CSV_QUOTE, FILE_FILTER_CSV_SAPATER)
                        End If
                    Next

                    If strLine.Length > 0 Then
                        objCsvStream.WriteLine(strLine.Substring(0, strLine.Length - 1))
                    Else
                        If blnIsEmptyRow Then
                            objCsvStream.WriteLine(String.Empty)
                        End If
                    End If
                Next
                Return FileOutputStatus.Success
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFCsvWriter", GetCurrentMethod.Name)
                Throw ex
            Finally
                If IsNothing(objCsvStream) = False Then
                    objCsvStream.Close()
                    objCsvStream.Dispose()
                End If
                objCsvStream = Nothing
            End Try
        End Function

        'END BY TXG 2014/05/06

#End Region

    End Class
End Namespace

