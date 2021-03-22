Imports System.Reflection.MethodBase
Imports System.Runtime.InteropServices.Marshal
Imports System.IO

Imports JoySock.LogOut.HPFLogOut
Imports JoySock.Data.HPFData
Imports JoySock.Data
Imports System.Text

Namespace FileOperation
    Public Class HPFCsvReader
        Inherits HPFReader

#Region "定数"

#End Region

#Region "変数"
        Private i_dtCsvTable As DataTable = Nothing
#End Region

        Public ReadOnly Property CsvDataTable As DataTable
            Get
                Return i_dtCsvTable
            End Get
        End Property

#Region "ファイルの操作"
        ''' <summary>
        ''' ファイル選択ダイアログでファイルを開きます
        ''' </summary>
        ''' <returns>開き結果</returns>
        ''' <remarks></remarks>
        Public Function OpenCsvFileByDialog(Optional ByVal strDefCsv As String = "", _
                                            Optional ByVal blnReadCsv As Boolean = True) As FileOutputStatus
            Try
                'パスまたはファイル名がシステム定義の最大長よりも長いとき、エラーを表示します
                If ShowFileDialog(FILE_EXT_NAME_CSV, FILE_FILTER_CSV, , strDefCsv) = False Then
                    Return FileOutputStatus.FileNameOutOfSize
                End If

                If String.Empty.Equals(i_FileName) = True Then
                    Return FileOutputStatus.Cancel
                End If

                If blnReadCsv = True Then
                    Return OpenCsvFile()
                Else
                    Return FileOutputStatus.Success
                End If
            Catch ex As Exception
                ErrorOut(ex.Message, "HPFReader.HPFCsvReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        ''' <summary>
        ''' ファイルを開きます
        ''' </summary>
        ''' <param name="strFilePath">ファイルパス</param>
        ''' <param name="blnClmnKbnSign">列区分ふごう</param>
        ''' <returns>開き結果</returns>
        ''' <remarks></remarks>
        Public Function OpenCsvFile(Optional ByVal strFilePath As String = "", _
                                    Optional ByVal blnClmnKbnSign As Boolean = False) As FileOutputStatus
            Try
                Dim objFileOutStatus As FileOutputStatus = FileOutputStatus.Success
                If String.Empty.Equals(strFilePath) = False Then
                    objFileOutStatus = SetCsvFilePath(strFilePath)
                End If

                If objFileOutStatus <> FileOutputStatus.Success Then
                    Return objFileOutStatus
                End If

                If String.Empty.Equals(i_FileName) = True Then
                    objFileOutStatus = FileOutputStatus.Cancel
                End If

                Return ReadFromCsvFile(blnClmnKbnSign)
            Catch ex As Exception
                ErrorOut(ex.Message, "HPFReader.HPFCsvReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        ''' <summary>
        ''' CSVファイルのパスの設定
        ''' </summary>
        ''' <param name="strFilePath">ファイルパス</param>
        ''' <returns>設定結果</returns>
        ''' <remarks></remarks>
        Private Function SetCsvFilePath(ByVal strFilePath As String) As FileOutputStatus
            Try
                If String.Empty.Equals(strFilePath) = True Then
                    Return FileOutputStatus.Faild
                End If

                If File.Exists(strFilePath) = False Then
                    Return FileOutputStatus.FileFound
                End If

                i_FileName = strFilePath
                Return FileOutputStatus.Success
            Catch ex As Exception
                ErrorOut(ex.Message, "HPFReader.HPFCsvReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

        ''' <summary>
        ''' CSVファイルを読み出す
        ''' </summary>
        ''' <param name="blnClmnKbnSign">列区分ふごう</param>
        ''' <returns>
        ''' FileOutputStatus：結果
        ''' </returns>
        ''' <remarks></remarks>
        Private Function ReadFromCsvFile(Optional ByVal blnClmnKbnSign As Boolean = False) As FileOutputStatus
            Dim objCsvStream As StreamReader = Nothing
            Try
                Dim strLine As String = String.Empty
                Dim strColumns() As String = Nothing

                '元の内容をクリアします
                If IsNothing(i_dtCsvTable) = False Then
                    i_dtCsvTable.Rows.Clear()
                    i_dtCsvTable.Columns.Clear()
                End If
                i_dtCsvTable = Nothing

                objCsvStream = New StreamReader(i_FileName, Encoding.GetEncoding("SHIFT-JIS"))

                While objCsvStream.EndOfStream = False
                    strLine = objCsvStream.ReadLine
                    strLine = strLine.Replace(FILE_VBLF, vbLf)
                    ''最後の行のとき、処理を終了します
                    'If IsNothing(strLine) = True Then
                    '    Exit While
                    'End If

                    If strLine.Equals(String.Empty) Then
                        Continue While
                    End If

                    If blnClmnKbnSign AndAlso Not IsNothing(i_dtCsvTable) Then
                        'Dim intIndex As Integer = 0
                        'Dim intStart As Integer = 0
                        'Dim intEnd As Integer = 0
                        'Dim intTemp As Integer = 0
                        'Dim intSignLen As Integer = strLine.Length - strLine.Replace("""", "").Length
                        'If intSignLen = 0 OrElse intSignLen Mod 2 <> 0 Then
                        '    '""がないの場所、""一ペアがないの場所
                        '    Return FileOutputStatus.Faild
                        'Else
                        '    ReDim strColumns(CInt(intSignLen / 2 - 1))
                        '    While (intIndex < intSignLen / 2)
                        '        intStart = InStr(intTemp + 1, strLine, """")
                        '        intEnd = InStr(intStart + 1, strLine, """")
                        '        strColumns(intIndex) = strLine.Substring(intStart, intEnd - intStart - 1)
                        '        intTemp = intEnd
                        '        intIndex += 1
                        '    End While
                        'End If
                        Dim str As String = ""","""
                        strColumns = strLine.Replace(str, "|").Split("|")
                        strColumns(0) = strColumns(0).Replace("""", "")
                        strColumns(strColumns.Length - 1) = strColumns(strColumns.Length - 1).Replace("""", "")
                    Else
                        strColumns = strLine.Split(",")
                    End If

                    'If blnClmnKbnSign AndAlso Not IsNothing(i_dtCsvTable) Then
                    '    strLine = strLine.Replace("""", "")
                    '    strColumns = strLine.Split("	")
                    'Else
                    '    strColumns = strLine.Split("	")
                    'End If

                    If IsNothing(i_dtCsvTable) Then
                        '第一行目の時、CSVテーブルの初期化
                        InitCsvTable(strColumns.Count)
                    End If

                    '新の行を追加します
                    If AddToCsvTable(strColumns) = False Then
                        Return FileOutputStatus.Faild
                    End If
                End While

                If IsNothing(i_dtCsvTable) = False Then
                    'レコードがあれば、Truｅを戻します
                    i_dtCsvTable.AcceptChanges()
                    Return FileOutputStatus.Success
                Else
                    'レコードがなければ、Falseを戻します
                    Return FileOutputStatus.DataNotFound
                End If
            Catch ex As Exception
                ErrorOut(ex.Message, "HPFReader.HPFCsvReader", GetCurrentMethod.Name)
                Throw ex
            Finally
                If IsNothing(objCsvStream) = False Then
                    objCsvStream.Close()
                    objCsvStream.Dispose()
                End If
                objCsvStream = Nothing
            End Try
        End Function

        ''' <summary>
        ''' CSVのテーブルの初期化
        ''' </summary>
        ''' <param name="intColumnCount">コラムのカウント数</param>
        ''' <remarks></remarks>
        Private Sub InitCsvTable(ByVal intColumnCount As Integer)
            Try
                Dim intIndex As Integer = 0
                i_dtCsvTable = New DataTable()

                For intIndex = 1 To intColumnCount
                    i_dtCsvTable.Columns.Add(String.Format("Column{0}", intIndex), Type.GetType("System.String"))

                Next
            Catch ex As Exception
                ErrorOut(ex.Message, "HPFReader.HPFCsvReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' 新の行を追加
        ''' </summary>
        ''' <param name="strColunms">コラムの配列</param>
        ''' <returns>
        ''' True :正常
        ''' False:異常
        ''' </returns>
        ''' <remarks></remarks>
        Private Function AddToCsvTable(ByVal strColunms() As String) As Boolean
            Try
                Dim drNew As DataRow = Nothing
                Dim intIndex As Integer = 0

                '項目の数とテーブルの不一致であれば、Falseを戻します
                If strColunms.Length <> 1 AndAlso strColunms.Length <> i_dtCsvTable.Columns.Count Then
                    Return False
                End If

                drNew = i_dtCsvTable.NewRow
                For intIndex = 0 To i_dtCsvTable.Columns.Count - 1
                    Dim strColumn As String = String.Empty
                    If intIndex < strColunms.Length Then
                        strColumn = strColunms(intIndex)
                        strColumn = strColumn.Replace("%", "").Replace("'", "").Replace("""", "")
                    End If
                    '必要であれば、CSVの項目のフォーマットを行います
                    drNew.Item(intIndex) = strColumn.Trim()
                Next

                i_dtCsvTable.Rows.Add(drNew)
                Return True
            Catch ex As Exception
                ErrorOut(ex.Message, "HPFReader.HPFCsvReader", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function
#End Region

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If i_IsDisposed = False Then
                    If disposing Then
                        ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                        If IsNothing(i_dtCsvTable) = False Then
                            i_dtCsvTable.Rows.Clear()
                        End If
                        i_dtCsvTable = Nothing
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

