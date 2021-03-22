Imports JoySock.LogOut.HPFLogOut

Imports System.IO
Imports System.Reflection.MethodBase

Namespace FileOperation
    Public Class HPFWriter
        Implements IDisposable
#Region "変数"
        Protected i_FileName As String = String.Empty
        Protected i_IsOpend As Boolean = False
        Protected i_IsDisposed As Boolean = False
#End Region

#Region "プロパティ"
        ''' <summary>
        ''' ファイル名
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FileName As String
            Set(ByVal value As String)
                i_FileName = value
            End Set
            Get
                Return i_FileName
            End Get
        End Property
#End Region

        Protected Sub New()
            Try
                i_FileName = String.Empty
            Catch ex As Exception
                ErrorOut(ex.Message, "HPFCommon.Form", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' ファイル保存ダイアログを表示します
        ''' </summary>
        ''' <param name="strDefaultExt">拡張子</param>
        ''' <param name="strFilter">選択ダイアログのファイルの種類</param>
        ''' <param name="strDefFileName">デフォルトのファイル名前</param>
        ''' <param name="blnRestoreDirectory">ディレクトリを復元するフラグ</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function ShowFileDialog(Optional ByVal strDefaultExt As String = "", _
                                          Optional ByVal strFilter As String = "", _
                                          Optional ByVal strDefFileName As String = "", _
                                          Optional ByVal blnRestoreDirectory As Boolean = False) As Boolean
            Try
                Dim objFileDialog As SaveFileDialog = New SaveFileDialog()
                i_FileName = String.Empty

                With objFileDialog
                    .DefaultExt = strDefaultExt
                    .FileName = strDefFileName
                    .Filter = strFilter
                    .RestoreDirectory = blnRestoreDirectory
                End With

                'ファイル選択ダイアログを表示します
                If objFileDialog.ShowDialog = DialogResult.OK Then
                    'OKであれば、ファイル名を取得します
                    i_FileName = objFileDialog.FileName

                    'ファイルが不存在であれば、空のファイルを作成します
                    If File.Exists(i_FileName) = False Then
                        File.Create(i_FileName).Close()
                    End If

                End If

                Return True
            Catch exFile As PathTooLongException
                Return False
            Catch ex As Exception
                ErrorOut(ex.Message, "FileOperation.HPFWriter", GetCurrentMethod.Name)
                Throw ex
            End Try
        End Function

#Region "IDisposable Support"
        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If i_IsDisposed = False Then
                If disposing Then
                    ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                End If

                ' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
                ' TODO: 大きなフィールドを null に設定します。
            End If
            i_IsDisposed = True
        End Sub

        ' TODO: 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
        'Protected Overrides Sub Finalize()
        '    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
        Public Sub Dispose() Implements IDisposable.Dispose
            ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
            Dispose(True)
            GC.SuppressFinalize(Me)
            GC.Collect()
        End Sub
#End Region

    End Class
End Namespace
