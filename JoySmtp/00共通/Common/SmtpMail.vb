Imports System.Net.Mail.SmtpClient
Imports System.Xml
Imports System.Windows.Forms
Imports System.Configuration
Imports System.Text
Imports System.Reflection.MethodBase
Imports JoyWater.Data.HPFData
Imports JoyWater.LogOut.HPFLogOut

Namespace Common

    Partial Class HPFCommon

        '--------------------------------------
        'SMTPの認証情報
        '--------------------------------------
        Private SmptServerHost As String = "103.241.129.20"
        Private SmptServerPort As Integer = 587

        Private SmptUserName As String = "hpf_wms@conside.co.jp"
        Private SmptUserPWD As String = "wmsadmin123"

        Private UseDefaultCredentials As Boolean = True
        Private EnableSsl As Boolean = False

        '--------------------------------------
        'Mail内容
        '--------------------------------------
        Public MailAddrFrom As String = "joy_water@conside.co.jp"
        Public MailAddrTo As String = ""

        '出荷指示自動アップロード対応 --Start Y.Yokota
        Public MailAddrCc As String = ""
        '出荷指示自動アップロード対応 --End   Y.Yokota

        Public MailSubject As String = ""
        Public MailBody As String = ""
        Public MailAttachment As String = ""
        Public MailAttachment2 As String = ""
        Public MailAttachment3 As String = ""
        Public MailAttachment4 As String = ""
        Public MailAttachment5 As String = ""

        Public Function SendMail(ByVal strTemp As String)

            Dim SmtpClient As New System.Net.Mail.SmtpClient()
            Dim MailMessage As New System.Net.Mail.MailMessage

            Try
                If MailAddrTo.Trim = "" Then
                    Return False
                End If

                ''XMLのドキュメント対象
                'Dim w_ObjXmlDoc As New XmlDocument()
                'w_ObjXmlDoc.Load(Application.StartupPath & "\" & M_REPORT_XML)
                ''---------------------------------------
                ''共通属性を読み込む
                ''---------------------------------------
                ''XMLファイルの共通属性のノード
                'Dim w_ObjXmlPubNode As XmlNode = w_ObjXmlDoc.SelectSingleNode("ReportOutSet/PublicSet")
                'With w_ObjXmlPubNode
                '    '
                '    SmptServerHost = .Attributes.GetNamedItem("SmptServerHost").Value
                '    SmptServerPort = .Attributes.GetNamedItem("SmptServerPort").Value

                '    SmptUserName = .Attributes.GetNamedItem("SmptUserName").Value
                '    SmptUserPWD = .Attributes.GetNamedItem("SmptUserPWD").Value
                '    MailAddrFrom = .Attributes.GetNamedItem("SmptMailAddrFrom").Value

                'End With

                ' ''SmptServerHost = ConfigurationManager.AppSettings("SmptServerHost").ToString
                ' ''SmptServerPort = ConfigurationManager.AppSettings("SmptServerPort").ToString
                ' ''SmptUserName = ConfigurationManager.AppSettings("SmptUserName").ToString

                '' ''パスワードの復号化処理を行う
                ' ''SmptUserPWD = i_Common.DecryptDESString(ConfigurationManager.AppSettings("SmptUserPWD").ToString, DECRYPT_KEY)
                ' ''MailAddrFrom = ConfigurationManager.AppSettings("SmptMailAddrFrom").ToString

                '--------------------------
                'SMTP
                '--------------------------
                'SMTPサーバー()
                SmtpClient.Host = SmptServerHost
                'ポート番号()
                SmtpClient.Port = SmptServerPort

                '--------------------------
                '認証
                '--------------------------
                If UseDefaultCredentials Then
                    SmtpClient.UseDefaultCredentials = True
                    SmtpClient.Credentials = New System.Net.NetworkCredential(SmptUserName, SmptUserPWD)
                Else
                    SmtpClient.UseDefaultCredentials = UseDefaultCredentials
                End If

                '--------------------------
                'SSL通信
                '--------------------------
                If EnableSsl Then
                    SmtpClient.EnableSsl = True
                Else
                    SmtpClient.EnableSsl = False
                End If

                '--------------------------
                'MailMessageの作成
                '--------------------------
                Dim strAdd1 As String = ""
                Dim strAddArr() As String

                '宛先
                strAddArr = MailAddrTo.Trim.Split(";")
                strAdd1 = strAddArr(0)
                MailMessage = New System.Net.Mail.MailMessage(MailAddrFrom, strAdd1, MailSubject, MailBody)
                For intI As Integer = 1 To strAddArr.Length - 1
                    If strAddArr(intI).Trim <> "" Then
                        MailMessage.To.Add(New System.Net.Mail.MailAddress(strAddArr(intI).Trim))
                    End If
                Next intI

                'CC
                If MailAddrCc.Trim <> "" Then
                    strAddArr = MailAddrCc.Trim.Split(";")
                    For intI As Integer = 0 To strAddArr.Length - 1
                        If strAddArr(intI).Trim <> "" Then
                            MailMessage.CC.Add(New System.Net.Mail.MailAddress(strAddArr(intI).Trim))
                        End If
                    Next intI
                End If

                '出荷指示自動アップロード対応 --Start Y.Yokota
                'Dim strAdd2 As String = ""
                'Dim strAddArr2() As String
                'strAddArr2 = MailAddrCc.Trim.Split(";")

                '送信者
                MailMessage.From = New System.Net.Mail.MailAddress(MailAddrFrom)
                '件名
                MailMessage.Subject = MailSubject
                '本文
                MailMessage.Body = MailBody
                '出荷指示自動アップロード対応 --End   Y.Yokota

                '--------------------------
                'Mail重要度
                '--------------------------
                MailMessage.Priority = System.Net.Mail.MailPriority.Normal

                '--------------------------
                'Mail添付ファイル
                '--------------------------
                If Not MailAttachment.Trim = "" Then
                    If System.IO.File.Exists(MailAttachment) Then
                        MailMessage.Attachments.Add(New System.Net.Mail.Attachment(MailAttachment))
                    End If
                End If

                If Not MailAttachment2.Trim = "" Then
                    If System.IO.File.Exists(MailAttachment2) Then
                        MailMessage.Attachments.Add(New System.Net.Mail.Attachment(MailAttachment2))
                    End If
                End If

                If Not MailAttachment3.Trim = "" Then
                    If System.IO.File.Exists(MailAttachment3) Then
                        MailMessage.Attachments.Add(New System.Net.Mail.Attachment(MailAttachment3))
                    End If
                End If

                If Not MailAttachment4.Trim = "" Then
                    If System.IO.File.Exists(MailAttachment4) Then
                        MailMessage.Attachments.Add(New System.Net.Mail.Attachment(MailAttachment4))
                    End If
                End If

                If Not MailAttachment5.Trim = "" Then
                    If System.IO.File.Exists(MailAttachment5) Then
                        MailMessage.Attachments.Add(New System.Net.Mail.Attachment(MailAttachment5))
                    End If
                End If

                '--------------------------
                'Mail送信
                '--------------------------
                SmtpClient.Send(MailMessage)

                '出荷指示自動アップロード対応 --Start Y.Yokota
                Select Case strTemp
                    Case "Excel添付メール", "Excel出力対象データなしメール"
                        InfoOut(strTemp & "メールを送信しました。")
                    Case Else
                        InfoOut(strTemp & "が発生しました、エラーメールを送信しました。")
                End Select
                '出荷指示自動アップロード対応 --End Y.Yokota

                'InfoOut(strTemp & "が発生しました、エラーメールを送信しました。")

                Return True

            Catch ex As Exception
                Return False
            Finally
                MailMessage.Dispose()
            End Try

            Return True

        End Function

    End Class

End Namespace