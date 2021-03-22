using System;
using JoySmtp.CLogOut;
using JoySmtp.Data;

namespace JoySmtp.JoyCommon
{
    public partial class Common
    {

        // --------------------------------------
        // SMTPの認証情報
        // --------------------------------------
        private string SmptServerHost = "103.241.129.20";
        private int SmptServerPort = 587;

        private string SmptUserName = "nagisa1020tanaka@conside.co.jp";
        private string SmptUserPWD = "tanaka5310";

        private bool UseDefaultCredentials = true;
        private bool EnableSsl = false;

        // --------------------------------------
        // Mail内容
        // --------------------------------------
        public string MailAddrFrom = "nagisa1020tanaka@conside.co.jp";
        public string MailAddrTo = "nagisalife@gmail.com";

        // 出荷指示自動アップロード対応 --Start Y.Yokota
        public string MailAddrCc = "";
        // 出荷指示自動アップロード対応 --End   Y.Yokota

        public string MailSubject = "";
        public string MailBody = "";
        public string MailAttachment = "";

        public Boolean SendMail(string strTemp)
        {
            System.Net.Mail.SmtpClient SmtpClient = new System.Net.Mail.SmtpClient();
            System.Net.Mail.MailMessage MailMessage = new System.Net.Mail.MailMessage();

            try
            {
                if (MailAddrTo.Trim() == "")
                    return false;

                // --------------------------
                // SMTP
                // --------------------------
                // SMTPサーバー()
                SmtpClient.Host = SmptServerHost;
                // ポート番号()
                SmtpClient.Port = SmptServerPort;

                // --------------------------
                // 認証
                // --------------------------
                if (UseDefaultCredentials)
                {
                    SmtpClient.UseDefaultCredentials = true;
                    SmtpClient.Credentials = new System.Net.NetworkCredential(SmptUserName, SmptUserPWD);
                }
                else
                    SmtpClient.UseDefaultCredentials = UseDefaultCredentials;

                // --------------------------
                // SSL通信
                // --------------------------
                if (EnableSsl)
                    SmtpClient.EnableSsl = true;
                else
                    SmtpClient.EnableSsl = false;

                // --------------------------
                // MailMessageの作成
                // --------------------------
                string strAdd1 = "";
                string[] strAddArr;

                // 宛先
                strAddArr = MailAddrTo.Trim().Split(';');
                strAdd1 = strAddArr[0];
                MailMessage = new System.Net.Mail.MailMessage(MailAddrFrom, strAdd1, MailSubject, MailBody);
                for (int intI = 1; intI <= strAddArr.Length - 1; intI++)
                {
                    if (strAddArr[intI].Trim() != "")
                        MailMessage.To.Add(new System.Net.Mail.MailAddress(strAddArr[intI].Trim()));
                }

                // CC
                if (MailAddrCc.Trim() != "")
                {
                    strAddArr = MailAddrCc.Trim().Split(';');
                    for (int intI = 0; intI <= strAddArr.Length - 1; intI++)
                    {
                        if (strAddArr[intI].Trim() != "")
                            MailMessage.CC.Add(new System.Net.Mail.MailAddress(strAddArr[intI].Trim()));
                    }
                }

                // 出荷指示自動アップロード対応 --Start Y.Yokota
                // Dim strAdd2 As String = ""
                // Dim strAddArr2() As String
                // strAddArr2 = MailAddrCc.Trim.Split(";")

                // 送信者
                MailMessage.From = new System.Net.Mail.MailAddress(MailAddrFrom);
                // 件名
                MailMessage.Subject = MailSubject;
                // 本文
                MailMessage.Body = MailBody;
                // 出荷指示自動アップロード対応 --End   Y.Yokota

                // --------------------------
                // Mail重要度
                // --------------------------
                MailMessage.Priority = System.Net.Mail.MailPriority.Normal;

                // --------------------------
                // Mail添付ファイル
                // --------------------------
                if (MailAttachment.Trim() != "")
                {
                    if (System.IO.File.Exists(MailAttachment))
                        MailMessage.Attachments.Add(new System.Net.Mail.Attachment(MailAttachment));
                }

                // --------------------------
                // Mail送信
                // --------------------------
                SmtpClient.Send(MailMessage);

                // 出荷指示自動アップロード対応 --Start Y.Yokota
                switch (strTemp)
                {
                    case "Excel添付メール":
                    case "Excel出力対象データなしメール":
                        {
                            LogOut.InfoOut(strTemp + "メールを送信しました。");
                            break;
                        }

                    default:
                        {
                            LogOut.InfoOut(strTemp + "が発生しました、エラーメールを送信しました。");
                            break;
                        }
                }
                // 出荷指示自動アップロード対応 --End Y.Yokota

                // InfoOut(strTemp & "が発生しました、エラーメールを送信しました。")

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                MailMessage.Dispose();
            }
        }
    }
}
