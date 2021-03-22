#region

using System;
using System.Text;
using System.IO;

using SmtpServer;
using JoySmtp.CLogOut;
using System.ComponentModel;
using anmar.SharpMimeTools;
using System.Web;
#endregion

namespace JoySmtp
{
    public class MailData
    {
        private string _fromadd;//FROMアドレス</param>
        private string _toadd;//FROMアドレス</param>
        private string _subject;
        private string _body;

        public string FromAddress
        {
            get { return _fromadd; }
            set { _fromadd = value; }
        }
        public string ToAddress
        {
            get { return _toadd; }
            set { _toadd = value; }
        }
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }
        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }
        public MailData(string fradd, string toadd, string title, string body)
        {
            this._fromadd = fradd;
            this._toadd = toadd;
            this._subject = title;
            this._body = body;
        }

        public string GetMIMEstring()
        {
            StringBuilder msg = new StringBuilder();

            msg.AppendLine("From:" + this._fromadd);
            msg.AppendLine("To:" + this._toadd);
            msg.AppendLine("Mime-Version:1.0");
            msg.AppendLine("Content-Type:Text/xml;charset=\"EUC-JP\"");
            msg.AppendLine("Content-Transfer-Encoding:8bit");
            if (this._subject != "")
            {
                msg.AppendLine("Subject:" + this._subject);
                //msg += "Subject:=?utf-8?" + base64.Encode("sub") + "?=\r\n";//=?utf-8?タイトル?=
            }
            msg.AppendLine();
            if (this._body != "")
            {
                msg.AppendLine(this._body);
            }
            //msg.AppendLine();

            return msg.ToString();
        }

        public class MyBase64str
        {
            private Encoding enc;

            public MyBase64str(string encStr)
            {
                enc = Encoding.GetEncoding(encStr);
            }

            public string Encode(string str)
            {
                return Convert.ToBase64String(enc.GetBytes(str));
            }

            public string Decode(string str)
            {
                return enc.GetString(Convert.FromBase64String(str));
            }
        }
    }
}