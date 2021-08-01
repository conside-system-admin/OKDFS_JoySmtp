using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Net.Sockets;

using SmtpServer;
using JoySmtp.CLogOut;
using System.ComponentModel;
using anmar.SharpMimeTools;
using System.Web;
using JoySmtp.Nac;

namespace JoySmtp
{

    #region "SendOnlyDataClass"

    public class SendOnlyDataClass
    {
        /// <summary>
        /// 結果受信タイムアウト秒
        /// </summary>
        public const Int32 RECV_TIME_OUT_SEC = 120;

        /// <summary>
        /// 処理完了フラグ
        /// </summary>
        public Boolean ProcessFinFLG = true;

        public DBEdiSend SelectData;

        /// <summary>
        /// 送信用のデータ格納
        /// </summary>
        public ProcessInputModel Message
        {
            get;
            set;
        }
        /// <summary>
        /// 送信用のデータ格納
        /// </summary>
        public ProcessSYGSendModel MessageSYG
        {
            get;
            set;
        }
        /// <summary>
        /// 受信数
        /// </summary>
        public int GetRecvCount
        {
            get { return this.RecvList.Count; }
        }
        /// <summary>
        /// 受信データ
        /// </summary>
        public List<NaccsRecvModel> RecvList = new List<NaccsRecvModel>();
        private List<NACCS_REPORTTYPE> i_RecvListReportType = new List<NACCS_REPORTTYPE>();
        public bool SetRecvListReportType(NACCS_REPORTTYPE val)
        {
            if (!i_RecvListReportType.Contains(val))
            {
                i_RecvListReportType.Add(val);
                return true;
            }
            else
            {
                return false;
            }
        }
        public string YUNYU_SHINKOKU_NO
        {
            get
            {
                if (Message != null)
                {
                    return Message.YUNYU_SHINKOKU_NO.Data;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (Message != null)
                {
                    Message.YUNYU_SHINKOKU_NO.SetData(value);
                }
            }
        }

        /// <summary>
        /// メール送信時刻
        /// </summary>
        public DateTime SendDate
        {
            get;
            set;
        }
        public Int32 SendNum;
        /// <summary>
        /// このメールアドレスが使用できるか？
        /// </summary>
        public Boolean IsUse
        {
            get
            {
                return this.ProcessFinFLG;
                //if (this.ProcessFinFLG)
                //{
                //    return true;
                //}
                //else
                //{
                //    if (Message != null || MessageSYG != null)
                //    {
                //        NACCS_REPORTTYPE type = (Message != null) ? Message.ReportType : MessageSYG.ReportType;
                //        var list = NaccsSendModel.SetRecvRequiredList(type);
                //        if (list.All(x => i_RecvListReportType.Contains(x)))
                //        {
                //            return true;
                //        }
                //        return false;
                //    }
                //    else
                //    {
                //        return false;
                //    }
                //}
            }
        }
        public Boolean IsRecvComplete(Boolean flg = false)
        {
            if (Message != null || MessageSYG != null)
            {
                NACCS_REPORTTYPE type = (Message != null) ? Message.ReportType : MessageSYG.ReportType;
                var list = NaccsSendModel.SetRecvRequiredList(type, flg);
                if (list.All(x => i_RecvListReportType.Contains(x)))
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="serv">サーバーIPアドレス</param>
        /// <param name="port">ポート番号</param>
        /// <param name="domain">メールのドメイン（IPで通信の場合は"")</param>
        /// <param name="fadr">自分の宛先アドレス</param>
        /// <param name="tadr">送りたい宛先アドレス</param>
        public SendOnlyDataClass(String serv, int port, String dns, String fadr, String tadr, String servername)
        {
            this.Message = null;
            this.MessageSYG = null;

            i_strServerAddr = serv;
            i_intPort = port;
            i_strFromAddr = fadr;
            i_strToAddr = tadr;
            i_strDns = dns;
            i_strServerName = servername;
            i_strDomain = i_strToAddr.Substring(i_strToAddr.LastIndexOf('@') + 1);

            this.ProcessFinFLG = true;
            this.SendDate = DateTime.MinValue;
            this.SendNum = 0;

            this.SelectData = new DBEdiSend();
        }
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="serv">サーバーIPアドレス</param>
        /// <param name="port">ポート番号</param>
        /// <param name="domain">メールのドメイン（IPで通信の場合は"")</param>
        /// <param name="fadr">自分の宛先アドレス</param>
        /// <param name="tadr">送りたい宛先アドレス</param>
        public void Reset()
        {
            this.Message = null;
            this.MessageSYG = null;

            this.ProcessFinFLG = false;
            this.SendDate = DateTime.MinValue;
            this.RecvList = new List<NaccsRecvModel>();
            this.i_RecvListReportType = new List<NACCS_REPORTTYPE>();

            //this.YUNYU_SHINKOKU_NO = "";
            this.SendNum = 0;
            this.SelectData = new DBEdiSend();
        }

        public enum SEND_STATUS
        {
            NO_CONNECT,
            HELO,
            MAIL,
            RCPT,
            DATA,
            QUIT
        }

        protected String i_strServerAddr;
        protected String i_strServerSubAddr;
        protected int i_intPort = 25;
        protected String i_strFromAddr;
        public string GetFromAddr
        {
            get { return this.i_strFromAddr; }
        }
        protected String i_strToAddr;
        public string GetToAddr
        {
            get { return this.i_strToAddr; }
        }
        protected String i_strDns;
        protected String i_strDomain;
        protected String i_strServerName;
        private SEND_STATUS i_stat = SEND_STATUS.NO_CONNECT;
        public SEND_STATUS GetStatus
        {
            get { return i_stat; }
        }

        /// <summary>
        /// メッセージリストからメールを送信する。
        /// </summary>
        /// <param name="sygFlg"></param>
        public void SmtpSend(Boolean sygFlg = false)
        {
            NACCS_REPORTTYPE nctype = NACCS_REPORTTYPE.NONE;
            System.Net.Sockets.NetworkStream stream = null;
            System.Net.Sockets.TcpClient client = null;
            i_stat = SEND_STATUS.NO_CONNECT;
            this.ProcessFinFLG = false;
            try
            {
                //List<NaccsSendModel> list = new List<NaccsSendModel>();
                NaccsSendModel input;
                if (sygFlg)
                {
                    input = this.MessageSYG;
                    nctype = NACCS_REPORTTYPE.SYG_LIST;
                }
                else
                {
                    input = this.Message;
                    //YUNYU_SHINKOKU_NO = this.Message.YUNYU_SHINKOKU_NO.Data;
                }

                //SettingFromDns();

                if (i_strServerAddr == null || i_strServerAddr.Length == 0)
                {
                    throw new Exception("サーバーアドレス未設定");
                }

                this.SendDate = DateTime.Now;

                //通信開始
                String rstr;
                client = new System.Net.Sockets.TcpClient();

                // POPサーバーに接続
                client.Connect(i_strServerAddr, i_intPort);
                stream = client.GetStream();

                // POPサーバー接続時のレスポンス受信
                rstr = WriteAndRead(stream, "");
                if (rstr.IndexOf("220") != 0)
                {
                    throw new Exception("サーバー接続エラー");
                }

                i_stat = SEND_STATUS.HELO;
                rstr = WriteAndRead(stream, "HELO " + i_strServerName + "\r\n");
                if (rstr.IndexOf("250") != 0)
                {
                    throw new Exception("HELOエラー");
                }

                //送信データの作成
                i_stat = SEND_STATUS.MAIL;
                //"MAIL", new MailVerb());//250, FROM:"自分"
                rstr = WriteAndRead(stream, "MAIL FROM:" + i_strFromAddr + "\r\n");
                if (rstr.IndexOf("250") != 0)
                {
                    throw new Exception("MAILエラー");
                }

                i_stat = SEND_STATUS.RCPT;
                //"RCPT", new RcptVerb());//250, TO:"NACCS"
                rstr = WriteAndRead(stream, "RCPT TO:<" + i_strToAddr + ">\r\n");
                if (rstr.IndexOf("250") != 0)
                {
                    throw new Exception("RCPTエラー");
                }

                i_stat = SEND_STATUS.DATA;
                //"DATA", new DataVerb());//354が来たら本文OK, 1行ずつ1000文字メッセージデータ送信　最後は".CRLF"
                rstr = WriteAndRead(stream, "DATA" + "\r\n");
                if (rstr.Length != 0 && rstr.IndexOf("354") != 0)
                {
                    throw new Exception("DATAエラー");
                }
                else
                {
                    //MailData msg = new MailData(fadr, tadr, "title", "");

                    //rstr = WriteAndRead(stream, msg.GetMIMEstring());
                    //rstr = WriteAndRead(stream, "." + "\r\n");

                    //input.NaccsCommon.DENBUN_LEN.SetData(String.Format("{0}", Convert.ToInt32(input.NaccsCommon.DENBUN_LEN.Data) + 2));
                    
                    byte[] dd = input.GetByteData();

                    if (dd != null)
                    {
                        Encoding s_euc = Encoding.GetEncoding(51932);

                        MailData mail = new MailData(
                            i_strFromAddr,
                            i_strToAddr,
                            input.NaccsCommon.SUBJECT,
                            s_euc.GetString(dd));

                        if (Message != null)
                        {
                            //LogOut.FileOutOTA(mail.GetMIMEstring(), LogOut.MAIL_PATH.SEND_MAIL_PATH, Message.KIJI_HANBAITEN.Data + ((this.Message.EDA_NO != "1")? ("-" + this.Message.EDA_NO) : "") + ((this.SendNum > 0) ? ("_" + (this.SendNum + 1).ToString()) : ""));
                            LogOut.FileOutOTA(mail.GetMIMEstring(), LogOut.MAIL_PATH.SEND_MAIL_PATH, Message.KIJI_HANBAITEN.Data + ((this.SendNum > 0) ? ("_" + (this.SendNum + 1).ToString()) : ""));
                        }
                        LogOut.FileOut(mail.GetMIMEstring(), input.ReportType);
                        
                        rstr = WriteAndRead(stream, mail.GetMIMEstring());
                        rstr = WriteAndRead(stream, "." + "\r\n");
                    }

                }
                this.SendNum++;

                i_stat = SEND_STATUS.QUIT;
                // 終了の送信
                rstr = WriteAndRead(stream, "QUIT" + "\r\n");
                if (!sygFlg)
                {
                    LogOut.InfoOut(string.Format("NACCS送信（{0}）", this.Message.PAX_NO), "SendOnlyDataClass", "PopBeforeSmtp");
                }
                ////処理済みにする
                //this.Message.NaccsCommon.IsGetProcessResult = true;
            }
            catch (SocketException Socex)
            {
                LogOut.ErrorOut(Socex.ToString() + ":" + Socex.ErrorCode.ToString(), "SendOnlyDataClass", "PopBeforeSmtp");
                throw Socex;
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.ToString(), "SendOnlyDataClass", "PopBeforeSmtp");
                throw ex;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
                if (client != null)
                {
                    client.Close();
                }
            }
        }

        /// <summary>
        /// ドメインからIPアドレスの取得
        /// </summary>
        private void SettingFromDns()
        {
            try
            {
                if (i_strDns.Length > 0)
                {
                    //検索するレコード
                    //A=1 NS=2 CNAME=5 PTR=12 MX=15 AAAA=28
                    byte recordType = 1;
                    //DNSサーバーのポート番号
                    int dnsPort = 53;

                    //DNSサーバーに問い合わせをする
                    string[] adList = DnsConnect.LookupDns(i_strDomain, recordType, i_strDns, dnsPort);

                    //IPアドレスを列挙
                    if (adList.Count() > 0)
                    {
                        i_strServerAddr = adList[0].ToString();
                        if (adList.Count() > 1)
                        {
                            i_strServerSubAddr = adList[1].ToString();
                        }
                    }
                    else
                    {
                        //最初に入力されたアドレスのままで何もしない
                    }
                }
                else
                {
                    //何もしない
                }
            }
            catch (SocketException Socex)
            {
                throw Socex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// POPサーバ送受信
        /// </summary>
        /// <param name="stm">ストリーム</param>
        /// <param name="req">リクエスト</param>
        /// <returns>レスポンス</returns>
        private String WriteAndRead(
            System.Net.Sockets.NetworkStream stm, String req)
        {

            // POPサーバへリクエスト送信
            if (req != "")
            {
                Byte[] sdata;
                sdata = System.Text.Encoding.ASCII.GetBytes(req);
                stm.Write(sdata, 0, sdata.Length);
            }
            for (int i = 1; i < 300; i++)
            {
                if (stm.DataAvailable) break;
                System.Threading.Thread.Sleep(10);
            }

            // POPサーバからのレスポンス受信
            String rtn = "";
            Byte[] rdata = new Byte[1024];
            while (stm.DataAvailable)
            {
                int l = stm.Read(rdata, 0, rdata.Length);
                if (l > 0)
                {
                    Array.Resize<Byte>(ref rdata, l);
                    rtn = rtn + System.Text.Encoding.ASCII.GetString(rdata);
                }
            }

            // レスポンス返信
            return rtn;
        }

        private string ToEuc(String unicodeStrings)
        {
            var unicode = Encoding.UTF8;
            var unicodeByte = unicode.GetBytes(unicodeStrings);
            var s_euc = Encoding.GetEncoding(51932);
            var s_eucByte = Encoding.Convert(unicode, s_euc, unicodeByte);
            var s_eucChars = new char[s_euc.GetCharCount(s_eucByte, 0, s_eucByte.Length)];
            s_euc.GetChars(s_eucByte, 0, s_eucByte.Length, s_eucChars, 0);
            return new string(s_eucChars);
        }
        private string ToBase64(String unicodeStrings)
        {
            var unicode = Encoding.Unicode;
            var unicodeByte = unicode.GetBytes(unicodeStrings);
            var s_euc = Encoding.GetEncoding(50220);
            var s_eucByte = Encoding.Convert(unicode, s_euc, unicodeByte);
            var s_eucChars = new char[s_euc.GetCharCount(s_eucByte, 0, s_eucByte.Length)];
            s_euc.GetChars(s_eucByte, 0, s_eucByte.Length, s_eucChars, 0);
            return new string(s_eucChars);
        }
    }

    ///// <summary>
    ///// A simple example use of SmtpServer.
    ///// Prints a message to the console when a session is established, completed
    ///// or a message is received.
    ///// </summary>
    //public class SendData: IDisposable
    //{
    //    //private readonly BindingList<MessageViewModel> _messages = new BindingList<MessageViewModel>();
    //    //private readonly BindingList<SessionViewModel> _sessions = new BindingList<SessionViewModel>();
    //    private ServerNaccs _server;
    //    private bool _quitting;

    //    private readonly SessionViewModel _session;
        

    //    /// <summary>
    //    /// 送信用のデータ格納
    //    /// </summary>
    //    private List<ProcessInputModel> MessageList = new List<ProcessInputModel>();

    //    //初期表示フラグ
    //    private bool _firstShown = true;

    //    public ServerNaccs i_server;
    //    public DefaultServerBehaviour serverBehaviour = new DefaultServerBehaviour();

    //    //public SendData(String serv, int port, String domain, String fadr, String tadr)
    //    //{
    //    //    SendClass smail = new SendClass(serv, port, domain, fadr, tadr);

    //    //    //// Pop Before Smtp認証でメール送信
    //    //    //smail.SmtpSend("192.168.10.206", 25, "", "", "from@xxx.com", "to@xxx.com",
    //    //    //    "タイトル", "本文", @"c:\temp\a.txt");

    //    //    smail.SmtpSend();//"192.168.10.206", 25, "", "", "from@xxx.com", "to@xxx.com", "title", "text", @"c:\temp\a.txt"); 

    //    //    //this.serverBehaviour = new DefaultServerBehaviour();
    //    //    //serverBehaviour.SessionStarted += SessionStarted;
    //    //    //serverBehaviour.SessionCompleted += SessionCompleted;
    //    //    //serverBehaviour.MessageReceived += MessageReceived;

    //    //    //i_server = new ServerNaccs(serverBehaviour);
    //    //    //i_server.Start();

    //    //    //////受信データ保存DB
    //    //    ////messageBindingSource.DataSource = _messages;
    //    //    //////セッションデータ保存DB
    //    //    ////sessionBindingSource.DataSource = _sessions;

    //    //    //LogOut.InfoOut("Server running. Press ENTER to stop and exit");
    //    //}

    //    private static void SessionCompleted(object sender, SessionEventArgs e)
    //    {
    //        LogOut.InfoOut(string.Format("SESSION END - Address:{0} NoOfMessages:{1} Error:{2}",
    //                                        e.Session.ClientAddress, e.Session.Messages.Count, e.Session.SessionError));
    //    }

    //    private static void SessionStarted(object sender, SessionEventArgs e)
    //    {
    //        LogOut.InfoOut(string.Format("SESSION START - Address:{0}", e.Session.ClientAddress));
    //    }

    //    private static void MessageReceived(object sender, MessageEventArgs e)
    //    {
    //        //this.serverBehaviour
    //        LogOut.InfoOut(string.Format("MESSAGE RECEIVED - Envelope From:{0} Envelope To:{1}", e.Message.From,
    //                                        string.Join(", ", e.Message.To)));

    //        //If you wanted to write the message out to a file, then could do this...
    //        //File.WriteAllBytes("myfile.eml", e.Message.Data);
    //    }

    //    public void Dispose()
    //    {
    //        i_server.Stop();
    //    }

    //    ///// <summary>
    //    ///// メッセージ送信処理
    //    ///// </summary>
    //    ///// <param name="clientSocket"></param>
    //    ///// <param name="data"></param>

    //    //private void Send(Socket clientSocket, String data)
    //    //{
    //    //    // 受信データをUTF8文字列に変換し送信
    //    //    var bytes = Encoding.UTF8.GetBytes(data);
    //    //    clientSocket.BeginSend(bytes, 0, bytes.Length, 0, new AsyncCallback(SendCallback), clientSocket);
    //    //}

    //    //// 送信時のコールバック処理
    //    //private static void SendCallback(IAsyncResult asyncResult)
    //    //{
    //    //    try
    //    //    {
    //    //        // クライアントソケットへのデータ送信処理を完了する
    //    //        var clientSocket = asyncResult.AsyncState as Socket;
    //    //        var byteSize = clientSocket.EndSend(asyncResult);
    //    //        Console.WriteLine($"送信結果: {byteSize}バイト [{clientSocket.RemoteEndPoint}]");
    //    //    }
    //    //    catch (Exception e)
    //    //    {
    //    //        Console.WriteLine(e.Message);
    //    //    }
    //    //}
    //}

    #endregion "SendOnlyDataClass"


    #region "SendClass"

    public class SendClass
    {
        /// <summary>
        /// 送信用のデータ格納
        /// </summary>
        public List<ProcessInputModel> MessageList
        {
            get;
            set;
        }
        /// <summary>
        /// 送信用のデータ格納
        /// </summary>
        public NaccsSendModel MessageSYG
        {
            get;
            set;
        }
        //public SendClass()
        //{
        //    this.MessageList = new List<ProcessInputModel>();

        //}
        /// <summary>
        /// 登録されているメールリストの中に探しているデータがあるか確認する
        /// </summary>
        /// <param name="strHandover">電文引継情報</param>
        /// <param name="strInfoNo">入力情報特定番号</param>
        /// <returns></returns>
        public Boolean IsSend(string strHandover, out ProcessInputModel inputdata, string strInfoNo = "")
        {
            Boolean flg = false;
            inputdata = null;
            try
            {
                if (this.MessageSYG != null)
                {
                    if (strHandover.Trim().Equals(this.MessageSYG.GetHandover))
                    {
                        if (strInfoNo != "")
                        {
                            if (strInfoNo.Trim().Equals(this.MessageSYG.GetInfoNo))
                            {
                                flg = true;
                            }
                        }
                        else
                        {
                            flg = true;
                        }
                    }
                }
                if (!flg && this.MessageList.Count > 0)
                {
                    foreach (var mes in this.MessageList)
                    {
                        if (strHandover.Trim().Equals(mes.GetHandover))
                        {
                            if (strInfoNo != "")
                            {
                                if (strInfoNo.Trim().Equals(mes.GetInfoNo))
                                {
                                    flg = true;
                                    inputdata = mes;
                                }
                            }
                            else
                            {
                                flg = true;
                                inputdata = mes;
                            }
                        }
                    }
                }
                return flg;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="serv">サーバーIPアドレス</param>
        /// <param name="port">ポート番号</param>
        /// <param name="domain">メールのドメイン（IPで通信の場合は"")</param>
        /// <param name="fadr">自分の宛先アドレス</param>
        /// <param name="tadr">送りたい宛先アドレス</param>
        public SendClass(String serv, int port, String domain, String fadr, String tadr, String servername)
        {
            this.MessageList = new List<ProcessInputModel>();
            this.MessageSYG = null;

            i_strServerAddr = serv;
            i_intPort = port;
            i_strFromAddr = fadr;
            i_strToAddr = tadr;
            i_strDomain = domain;
            i_strServerName = servername;
        }
        public enum SEND_STATUS
        {
            NO_CONNECT,
            HELO,
            MAIL,
            RCPT,
            DATA,
            QUIT
        }

        protected String i_strServerAddr;
        protected String i_strServerSubAddr;
        protected int i_intPort = 25;
        protected String i_strFromAddr;
        protected String i_strToAddr;

        protected String i_strDomain;
        protected String i_strServerName;
        private SEND_STATUS i_stat = SEND_STATUS.NO_CONNECT;
        public SEND_STATUS GetStatus
        {
            get { return i_stat; }
        }

        /// <summary>
        /// メッセージリストからメールを送信する。
        /// </summary>
        /// <param name="sygFlg"></param>
        public void SmtpSend(Boolean sygFlg = false, NaccsSendModel model = null)
        {
            NACCS_REPORTTYPE nctype = NACCS_REPORTTYPE.NONE;
            System.Net.Sockets.NetworkStream stream = null;
            System.Net.Sockets.TcpClient client = null;
            i_stat = SEND_STATUS.NO_CONNECT;
            try
            {
                List<NaccsSendModel> list = new List<NaccsSendModel>();
                if (model != null)
                {
                    list.Add(model);
                    nctype = NACCS_REPORTTYPE.OTHER;
                }
                else if (sygFlg)
                {
                    list.Add(this.MessageSYG);
                    nctype = NACCS_REPORTTYPE.SYG_LIST;
                }
                else
                {
                    list = new List<NaccsSendModel>(this.MessageList);
                }

                //if (this.MessageList.Count > 0 || this.MessageSYG != null)
                if (list.Count > 0)
                {
                    SettingFromDns();

                    if (i_strServerAddr == null || i_strServerAddr.Length == 0)
                    {
                        throw new Exception("サーバーアドレス未設定");
                    }

                    //通信開始
                    String rstr;
                    client = new System.Net.Sockets.TcpClient();

                    // POPサーバーに接続
                    client.Connect(i_strServerAddr, i_intPort);
                    stream = client.GetStream();

                    // POPサーバー接続時のレスポンス受信
                    rstr = WriteAndRead(stream, "");
                    if (rstr.IndexOf("220") != 0)
                    {
                        throw new Exception("サーバー接続エラー");
                    }

                    i_stat = SEND_STATUS.HELO;
                    rstr = WriteAndRead(stream, "HELO " + i_strServerName + "\r\n");
                    if (rstr.IndexOf("250") != 0)
                    {
                        throw new Exception("HELOエラー");
                    }

                    //// ユーザIDの送信
                    //rstr = WriteAndRead(stream, "USER " + user + "\r\n");
                    //if (rstr.IndexOf("+OK") != 0)
                    //{
                    //    throw new Exception("ユーザIDエラー");
                    //}

                    //// パスワードの送信
                    //rstr = WriteAndRead(stream, "PASS " + pass + "\r\n");
                    //if (rstr.IndexOf("+OK") != 0)
                    //{
                    //    throw new Exception("パスワードエラー");
                    //}

                    //// APOPの場合は[ユーザID送信]と[パスワード送信]の処理を以下のように変更します
                    //// POPサーバー接続時のレスポンスからAPOP用のキー(<>で囲まれた部分)を取得して
                    //// パスワードと連結(例:"<999.999@mxg999.xxx.com>PASS")してMD5(HEX)変換して
                    //// "APOP user MD5(HEX)"形式で送信します
                    //Byte[] byt = System.Text.Encoding.ASCII.GetBytes("<999.999@mxg999.xxx.com>" + pass);
                    //System.Security.Cryptography.MD5CryptoServiceProvider md5 =
                    //    new System.Security.Cryptography.MD5CryptoServiceProvider();
                    //Byte[] res = md5.ComputeHash(byt);
                    //String aps = BitConverter.ToString(res).Replace("-", "").ToLower();
                    //rstr = WriteAndRead(stream, "APOP " + user + " " + aps + "\r\n");
                    //if (rstr.IndexOf("+OK") != 0)
                    //{
                    //    throw new Exception("ユーザIDまたはパスワードエラー");
                    //}

                    foreach (var input in list)
                    //                  foreach (var input in this.MessageList)
                    {
                        i_stat = SEND_STATUS.MAIL;
                        //"MAIL", new MailVerb());//250, FROM:"自分"
                        rstr = WriteAndRead(stream, "MAIL FROM:" + i_strFromAddr + "\r\n");
                        if (rstr.IndexOf("250") != 0)
                        {
                            throw new Exception("MAILエラー");
                        }

                        i_stat = SEND_STATUS.RCPT;
                        //"RCPT", new RcptVerb());//250, TO:"NACCS"
                        rstr = WriteAndRead(stream, "RCPT TO:<" + i_strToAddr + ">\r\n");
                        if (rstr.IndexOf("250") != 0)
                        {
                            throw new Exception("RCPTエラー");
                        }

                        i_stat = SEND_STATUS.DATA;
                        //"DATA", new DataVerb());//354が来たら本文OK, 1行ずつ1000文字メッセージデータ送信　最後は".CRLF"
                        rstr = WriteAndRead(stream, "DATA" + "\r\n");
                        if (rstr.Length != 0 && rstr.IndexOf("354") != 0)
                        {
                            throw new Exception("DATAエラー");
                        }
                        else
                        {
                            //MailData msg = new MailData(fadr, tadr, "title", "");

                            //rstr = WriteAndRead(stream, msg.GetMIMEstring());
                            //rstr = WriteAndRead(stream, "." + "\r\n");

                            //input.NaccsCommon.DENBUN_LEN.SetData(String.Format("{0}", Convert.ToInt32(input.NaccsCommon.DENBUN_LEN.Data) + 2));
                            byte[] dd = input.GetByteData();

                            if (dd != null)
                            {
                                Encoding s_euc = Encoding.GetEncoding(51932);

                                MailData mail = new MailData(
                                    i_strFromAddr,
                                    i_strToAddr,
                                    input.NaccsCommon.SUBJECT,
                                    s_euc.GetString(dd));

                                LogOut.FileOut(mail.GetMIMEstring(), input.ReportType);
                                rstr = WriteAndRead(stream, mail.GetMIMEstring());
                                rstr = WriteAndRead(stream, "." + "\r\n");
                            }
                        }
                    }

                    i_stat = SEND_STATUS.QUIT;
                    // 終了の送信
                    rstr = WriteAndRead(stream, "QUIT" + "\r\n");
                }
            }
            catch (SocketException Socex)
            {
                LogOut.ErrorOut(Socex.ToString() + ":" + Socex.ErrorCode.ToString(), "SendClass", "PopBeforeSmtp");
                throw Socex;
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.ToString(), "SendClass", "PopBeforeSmtp");
                throw ex;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
                if (client != null)
                {
                    client.Close();
                }
            }
        }

        /// <summary>
        /// ドメインからIPアドレスの取得
        /// </summary>
        private void SettingFromDns()
        {
            try
            {
                if (i_strDomain.Length > 0)
                {
                    ////IPHostEntryオブジェクトを取得
                    //System.Net.IPHostEntry iphe = System.Net.Dns.GetHostEntry(i_strDomain);
                    ////IPアドレスのリストを取得
                    //System.Net.IPAddress[] adList = iphe.AddressList;

                    System.Net.IPAddress[] adList = System.Net.Dns.GetHostAddresses(i_strDomain);

                    //IPアドレスを列挙
                    if (adList.Count() > 0)
                    {
                        i_strServerAddr = adList[0].ToString();
                        if (adList.Count() > 1)
                        {
                            i_strServerSubAddr = adList[1].ToString();
                        }
                    }
                    else
                    {
                        //最初に入力されたアドレスのままで何もしない
                    }
                }
                else
                {
                    //何もしない
                }
            }
            catch (SocketException Socex)
            {
                throw Socex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// POPサーバ送受信
        /// </summary>
        /// <param name="stm">ストリーム</param>
        /// <param name="req">リクエスト</param>
        /// <returns>レスポンス</returns>
        private String WriteAndRead(
            System.Net.Sockets.NetworkStream stm, String req)
        {

            // POPサーバへリクエスト送信
            if (req != "")
            {
                Byte[] sdata;
                sdata = System.Text.Encoding.ASCII.GetBytes(req);
                stm.Write(sdata, 0, sdata.Length);
            }
            for (int i = 1; i < 300; i++)
            {
                if (stm.DataAvailable) break;
                System.Threading.Thread.Sleep(10);
            }

            // POPサーバからのレスポンス受信
            String rtn = "";
            Byte[] rdata = new Byte[1024];
            while (stm.DataAvailable)
            {
                int l = stm.Read(rdata, 0, rdata.Length);
                if (l > 0)
                {
                    Array.Resize<Byte>(ref rdata, l);
                    rtn = rtn + System.Text.Encoding.ASCII.GetString(rdata);
                }
            }

            // レスポンス返信
            return rtn;
        }

        public static string ToEuc(String unicodeStrings)
        {
            var unicode = Encoding.UTF8;
            var unicodeByte = unicode.GetBytes(unicodeStrings);
            var s_euc = Encoding.GetEncoding(51932);
            var s_eucByte = Encoding.Convert(unicode, s_euc, unicodeByte);
            var s_eucChars = new char[s_euc.GetCharCount(s_eucByte, 0, s_eucByte.Length)];
            s_euc.GetChars(s_eucByte, 0, s_eucByte.Length, s_eucChars, 0);
            return new string(s_eucChars);
        }
        public static string ToBase64(String unicodeStrings)
        {
            var unicode = Encoding.Unicode;
            var unicodeByte = unicode.GetBytes(unicodeStrings);
            var s_euc = Encoding.GetEncoding(50220);
            var s_eucByte = Encoding.Convert(unicode, s_euc, unicodeByte);
            var s_eucChars = new char[s_euc.GetCharCount(s_eucByte, 0, s_eucByte.Length)];
            s_euc.GetChars(s_eucByte, 0, s_eucByte.Length, s_eucChars, 0);
            return new string(s_eucChars);
        }
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
       
    #endregion "SendClass"
}