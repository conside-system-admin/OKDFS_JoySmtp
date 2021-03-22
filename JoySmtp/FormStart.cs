using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.Win32;
using Microsoft.VisualBasic;
using JoySmtp.CLogOut;
using JoySmtp.JoyCommon;
using JoySmtp.DataBase;
using JoySmtp.Nac;
using JoySmtp.Data;
using System.Threading;
using System.Threading.Tasks;

using SmtpServer;


namespace JoySmtp
{
    public partial class FormStart : Form
    {
        private Boolean blnFirstStartFlg = false;
        private InitProject InitProject = new InitProject ();
        /// <summary>
        /// 強制終了フラグ
        /// </summary>
        private Boolean i_SendStop;

        private Boolean i_AutoFlg = false;
        //private int SYG_INTERVAL= 30;

        //private int i_SYGCnt = 0;
        //private Boolean i_FirstFlg_O = false;
        //private Boolean i_FirstFlg_S = false;

        private DateTime? nextSendTime;
        private DateTime? nextSYGTime;

        //非同期処理
        private Task taskSend;
        private Task taskSYGlist;
        private Task taskSYGget;

        private readonly BindingList<MessageViewModel> _messages = new BindingList<MessageViewModel>();
        private readonly BindingList<SessionViewModel> _sessions = new BindingList<SessionViewModel>();
        private ServerNaccs _server;
        ////受信データ保存DB
        //messageBindingSource.DataSource = _messages;
        ////セッションデータ保存DB
        //sessionBindingSource.DataSource = _sessions;

        static public BindingList<Item> i_logs;

        static BindingList<SendClass> i_sendList = new BindingList<SendClass>();
        static BindingList<SendOnlyDataClass> i_sendDataList = new BindingList<SendOnlyDataClass>();
        //未送信PAX_DATA
        static List<DataRow> i_sendstacklist = new List<DataRow>();
        //送信PAX_DATA
        static List<DataRow> i_sendfinlist = new List<DataRow>();

        //未送信PAX_DATA
        static List<DBEdiSend> i_sendstacklist2 = new List<DBEdiSend>();
        //送信PAX_DATA
        static List<DBEdiSend> i_sendfinlist2 = new List<DBEdiSend>();

        //未送信SYG_DATA
        static List<string> i_sygstacklist = new List<string>();
        //送信SYG_DATA
        static List<string> i_sygfinlist = new List<string>();


        /// <summary>
        /// 受信したけど送信データが何かわからないデータ
        /// </summary>
        static List<NaccsRecvModel> i_recvUnknownList = new List<NaccsRecvModel>(); 
        //////送信用メールアドレス
        //static SendOnlyDataClass i_sendData = null;

        delegate void SetShowLogDelegate(string strState, string strDetail);
        /// <summary>
        /// 画面表示追加
        /// </summary>
        /// <param name="strState"></param>
        /// <param name="strDetail"></param>
        static void SetShowLog(string strState , string strDetail)
        {
            Item logItem = new Item() { Date = DateTime.Now, State = strState, Detail = strDetail  };
            i_logs.Insert(0, logItem);
            if (i_logs.Count > 1000)
            {
                i_logs.RemoveAt(i_logs.Count-1);
            }
        }

        /// <summary>
        /// 設定表示
        /// </summary>
        public void SetTxtAppConfig()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("NACCS SERVER : {0}",HPFData.ServerAddress).AppendLine();
            sb.AppendFormat("PORT NO : {0}",HPFData.PortNo).AppendLine();
            sb.AppendFormat("FROM MAIL : {0}",HPFData.FromMailAddress).AppendLine();
            sb.AppendFormat("TO MAIL{0} : {1}", "1", HPFData.ToMailAddress).AppendLine();
            sb.AppendFormat("DOMAIN : {0}",HPFData.ServerDomain).AppendLine();
            sb.AppendFormat("USER CODE : {0}",HPFData.InputUserCode).AppendLine();
            sb.AppendFormat("DB SERVER : {0}", HPFData.DataBaseHostName).AppendLine();
            sb.AppendFormat("DB TABLE : {0}", HPFData.DataBaseName);

            this.txtDetail.Text = sb.ToString();

            System.Reflection.Assembly assembly = Assembly.GetExecutingAssembly();

            this.Text = this.Text + assembly.GetName().Version;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormStart(string[] args)
        {
            InitializeComponent();

            foreach (var arg in args)
            {
                if (arg.ToUpper() == "AUTO")
                {
                   this.blnFirstStartFlg = true;
                }
            }
        }
        /// <summary>
        /// フォーム画面表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //イベントをイベントハンドラに関連付ける
            //SystemEvents.SessionEnding +=
            //    new SessionEndingEventHandler(SystemEvents_SessionEnding);

            // ウィンドウを画面を左下に表示させる
            int top = Screen.PrimaryScreen.WorkingArea.Height - this.Height;
            DesktopBounds = new Rectangle(this.Left, top, this.Width, this.Height);

            //ログファイルの初期化
            LogOut.InitLogOut();

            LogOut.InfoOut("起動", this.Name, MethodBase.GetCurrentMethod().Name);

            i_logs = new BindingList<Item>();
            i_sendList = new BindingList<SendClass>();

            itemBindingSource.DataSource = i_logs;

            //データベースアクセス関連
            InitProject.InitPublicInfo("");

            //
            this.KeyPreview = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);

            //タイマー設定
            this.toolStripTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            timerNowTime.Interval = 1000;
            timerNowTime.Enabled = true;
            this.i_AutoFlg = false;

            //フォーム画面設定表示
            SetTxtAppConfig();


            if (HPFData.IntervalSendTimer == 0)
            {
                this.nextSendTime = null;
            }
            if (HPFData.IntervalSYGTimer == 0)
            {
                this.nextSYGTime = null;
            }

            //以下は要らない？
            if (HPFData.IntervalSendTimer > 0)
            {
                timerSend.Interval = 1000 * HPFData.IntervalSendTimer;
            }
            else
            {
                timerSend.Interval = 1000;
            }


            if (HPFData.IntervalSYGTimer > 0)
            {
                timerSYG.Interval = 1000 * HPFData.IntervalSYGTimer;
            }
            else
            {
                timerSYG.Interval = 1000;
            }

            this.taskSend = new Task(SendFunc);
            this.taskSYGlist = new Task(SYGFunc);
            this.taskSYGget = new Task(SYGResultFunc);

            //server = new ServerNaccs(serverBehaviour);

            SetShowLog("起動", "アプリを起動しました。");

            if (!string.IsNullOrEmpty(HPFData.DnsServer))
            {
                //検索するホスト名（FQDN）またはIPアドレス
                string hostOrAddress = HPFData.ToMailAddress.Substring(HPFData.ToMailAddress.LastIndexOf('@') + 1);

                //検索するレコード
                //A=1 NS=2 CNAME=5 PTR=12 MX=15 AAAA=28
                byte recordType = 1;
                //DNSサーバーのポート番号
                int dnsPort = 53;

                //DNSサーバーに問い合わせをする
                string[] answers = DnsConnect.LookupDns(hostOrAddress, recordType, HPFData.DnsServer, dnsPort);
                int no = 1;
                foreach (string s in answers)
                {
                    //SetShowLog("GET", string.Format("{0}:{1}", no, s));
                    if (no == 1)
                    {
                        HPFData.ServerAddress = s;
                    }
                    no++;
                }

                string[] answers2 = DnsConnect.LookupDns(hostOrAddress, recordType, HPFData.DnsServerSub, dnsPort);
                no = 1;
                foreach (string s in answers2)
                {
                    //SetShowLog("GET2", string.Format("{0}:{1}", no, s));
                    no++;
                }
                LogOut.InfoOut(string.Format("CONNECT IP : {0}", HPFData.ServerAddress), this.Name, MethodBase.GetCurrentMethod().Name);
            }
            if(this.blnFirstStartFlg)
            {
                this.btnStart.PerformClick();
            }

        }


        #region "受信用SMTPサーバ"

        /// <summary>
        /// 受信スレッド開始
        /// </summary>
        private void StartServer()
        {
            new Thread(ServerWork).Start();
            LogOut.InfoOut("SMTP受信処理開始", this.Name, MethodBase.GetCurrentMethod().Name);
            SetShowLog("起動", "SMTP受信処理開始");
        }

        /// <summary>
        /// 新規セッション受信した際の、スレッド開始処理
        /// </summary>
        private void ServerWork()
        {
            try
            {
                Application.DoEvents();

                DefaultServerBehaviour b = new  DefaultServerBehaviour();
                b.MessageReceived += OnMessageReceived;
                b.SessionCompleted += OnSessionCompleted;

                _server = new ServerNaccs(b);
                _server.Run();
            }
            catch (Exception exception)
            {
                LogOut.InfoOut(exception.Message, this.Name, MethodBase.GetCurrentMethod().Name);
                Invoke((MethodInvoker)(() =>
                {

                    StopServer();

                }));
            }
        }
        /// <summary>
        /// セッション終了時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSessionCompleted(object sender, SessionEventArgs e)
        {
            try
            {
                Invoke((MethodInvoker)(() => { _sessions.Add(new SessionViewModel(e.Session)); }));
            }
            catch
            {

            }
        }

        /// <summary>
        /// メッセージの受信時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMessageReceived(object sender, MessageEventArgs e)
        {
            MessageViewModel message = new MessageViewModel(e.Message);

            Invoke((MethodInvoker)(() =>
            {
                _messages.Add(message);
                string resultstring;
                if (ConfirmRecvData(message, out resultstring))
                {
                    LogOut.InfoOut(resultstring, this.Name, MethodBase.GetCurrentMethod().Name);
//#if DEBUG
//                    SetShowLog("受信", resultstring);
//#endif
                }
                else
                {
                    LogOut.ErrorOut(resultstring, this.Name, MethodBase.GetCurrentMethod().Name);
//#if DEBUG
//                    SetShowLog("異常", resultstring);
//#endif
                }

                this.dataGridViewLog.Refresh();
            }));
        }
        /// <summary>
        /// 
        /// </summary>
        private void StopServer()
        {
            if (_server.IsRunning)
            {
                _server.Stop();
                LogOut.InfoOut("SMTP受信処理停止", this.Name, MethodBase.GetCurrentMethod().Name);
                SetShowLog("終了", "SMTP受信処理停止");
            }
         }

        #endregion "受信用SMTPサーバ"


        #region "コントロール"
        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && (e.KeyCode == Keys.B)))
            {
                this.btnStart.PerformClick();
            }
            if ((e.Control && (e.KeyCode == Keys.Y)))
            {
                this.btnStop.PerformClick();
            }
        }


        /// <summary>
        /// 開始ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            this.btnStop.Enabled = true;
            this.i_AutoFlg = true;
            this.i_SendStop = false;

            SetShowLog("開始","自動処理開始ボタンをクリックしました。");

            StartServer();
            //i_sendList = new BindingList<SendClass>();

            this.btnStart.Enabled = false;
            //i_sendData = null;

            //メールアドレスの設定？
            i_sendDataList = new BindingList<SendOnlyDataClass>();
            SendOnlyDataClass mail =  new SendOnlyDataClass(
                HPFData.ServerAddress,
                HPFData.PortNo,
                HPFData.ServerDomain,
                HPFData.FromMailAddress,
                HPFData.ToMailAddress,
                HPFData.MyServerName
            );
            i_sendDataList.Add(mail);

            i_sendstacklist = new List<DataRow>();
            i_sendfinlist = new List<DataRow>();
            i_sygstacklist = new List<string>();
            i_sygfinlist = new List<string>();

            i_recvUnknownList = new List<NaccsRecvModel>();
#if DEBUG
            //OTHERFunc();
#endif

            if (HPFData.IntervalSYGTimer > 0)
            {
                this.nextSYGTime = DateTime.Now;
            }
            if( HPFData.IntervalSendTimer > 0 )
            {
                this.nextSendTime = DateTime.Now.AddSeconds(5);
            }
        }

        /// <summary>
        /// 停止ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show(
                "自動処理を停止してもよろしいですか？", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                SetShowLog("停止", "自動処理停止ボタンをクリックしました。");

                this.btnStart.Enabled = true;
                this.i_AutoFlg = false;

                StopServer();

                //各スレッド処理を停止する
                this.timerSend.Stop();
                this.timerSYG.Stop();

                this.btnStop.Enabled = false;
            }
        }

        /// <summary>
        /// フォームが閉じる前の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr;

            dr = MessageBox.Show("アプリを終了させますか？", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                LogOut.InfoOut("終了", this.Name, MethodBase.GetCurrentMethod().Name);
            }
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerSend_Tick(object sender, EventArgs e)
        {
            //if (this.i_AutoFlg)
            //{
            //    //送信用セッションのスレッドが起動しているか確認する
            //    if (this.taskSend.Status != TaskStatus.Running)
            //    {
            //        this.taskSend = new Task(SendFunc);

            //        this.taskSend.Start();
            //    }
            //}
        }
        private void timerSYG_Tick(object sender, EventArgs e)
        {
            //if (this.i_AutoFlg)
            //{
            //    //SYGが送信済みの場合削除する★

            //    //送信用セッションのスレッドが起動しているか確認する
            //    if (this.taskSYGlist.Status != TaskStatus.Running)
            //    {
            //        this.taskSYGlist = new Task(SYGFunc);

            //        this.taskSYGlist.Start();
            //    }
            //}
        }


        /// <summary>
        /// 時刻表示　タイマースレッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerDate_Tick(object sender, EventArgs e)
        {
            SetShowLogDelegate ssd = new SetShowLogDelegate(SetShowLog);
            try
            {
                this.toolStripTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                if (i_AutoFlg)
                {
                        //SYGデータ蓄積があるか？
                        if (i_sygstacklist.Count > 0)
                        {
                            if (!IsRunningTask())
                            {
                                //使用可能なメールアドレスがあるか？
                                if (i_sendDataList.Where(m => m.IsUse == true).Count() > 0)
                                {
                                    this.taskSYGget = new Task(SYGResultFunc);
                                    this.taskSYGget.Start();
                                }
                            }
                        }
                        else if (i_sendstacklist.Count > 0)
                        {
                            if (!IsRunningTask())
                            {
                                //使用可能なメールアドレスがあるか？
                                if (i_sendDataList.Where(m => m.IsUse == true).Count() > 0)
                                {
                                    this.i_SendStop = false;
                                    this.taskSend = new Task(SendFunc);
                                    this.taskSend.Start();
                                }
                            }
                        }
                        else if (this.nextSendTime != null && this.nextSendTime < DateTime.Now)
                        {
                            this.nextSendTime = DateTime.Now.AddSeconds(HPFData.IntervalSendTimer);
                            //送信用セッションのスレッドが起動しているか確認する
                            if (!IsRunningTask())
                            {
                                //使用可能なメールアドレスがあるか？
                                if (i_sendDataList.Where(m => m.IsUse == true).Count() > 0)
                                {
                                    //送信して良いか？
                                    if (ConfirmGetJotaiAndTanto())
                                    {
                                        //DBに未送信データを確認する
                                        ConfirmSnedData();

                                        //送信する内容があるか？
                                        if (i_sendstacklist2.Count > 0)
                                        {
                                            this.i_SendStop = false;
                                            this.taskSend = new Task(SendFunc);
                                            this.taskSend.Start();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //処理中の場合どうする？★★★
                                this.nextSendTime = DateTime.Now.AddSeconds(30);
                            }
                        }//SYGタスク
                        else if (this.nextSYGTime != null && this.nextSYGTime < DateTime.Now)
                        {
                            this.nextSYGTime = DateTime.Now.AddMinutes(HPFData.IntervalSYGTimer);

                            //送信用セッションのスレッドが起動しているか確認する
                            if (!IsRunningTask())
                            {
                                //送信して良いか？
                                if (ConfirmGetJotaiAndTanto())
                                {
                                    this.taskSYGlist = new Task(SYGFunc);
                                    this.taskSYGlist.Start();
                                }
                            }
                            else
                            {
                                //処理中の場合5分後に再確認
                                this.nextSYGTime = DateTime.Now.AddMinutes(5);
                            }
                        }
                    }
                    if (i_sendDataList.Where(m => m.IsUse == false).Count() > 0)
                    {
                        ConfirmUseAddress();
                    }
                
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                this.Invoke(ssd, "異常", ex.Message);
            }
        }
        /// <summary>
        /// タスクが動作中ならTRUE
        /// </summary>
        /// <returns></returns>
        public Boolean IsRunningTask()
        {
            if (this.taskSend.Status == TaskStatus.Running) 
                return true;
            if (this.taskSYGget.Status == TaskStatus.Running)
                return true;
            if (this.taskSYGlist.Status == TaskStatus.Running)
                return true;
            return false;
        }
        /// <summary>
        /// 受信データを処理する。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ConfirmRecvData(MessageViewModel model, out string resultstring)
        {
            bool ret = false;
            SetShowLogDelegate ssd = new SetShowLogDelegate(SetShowLog);
            SendOnlyDataClass mail = null;

            resultstring = "";
            try
            {
                //受信したデータの出力共通項目を確認する
                OutputCommonModel common = new OutputCommonModel();
                INaccs nac;
                var btRecv = common.EUC.GetBytes(model.Parts.Body + "\r\n");
                long lngSeq = 0;

                if (!common.SetData(btRecv, model.ReceivedDate, out lngSeq))
                {
                    //読み込めなかった場合
                    LogOut.FileOut(model.Message.Session.Log.ToString(), NACCS_REPORTTYPE.NONE, common.GetFileName());

                    resultstring = "フォーマット異常(出力共通項目):" + model.Subject;
                    this.Invoke(ssd, "異常", "フォーマット異常(出力共通項目):" + model.Subject);
                }
                else
                {
                    //解析する
                    LogOut.FileOut(model.Message.Session.Log, common.REPORT_TYPE, common.GetFileName()); 
//#if DEBUG
//                    LogOut.FileOut(model.Parts.Body, common.REPORT_TYPE, "DEBUG_"+ common.GetFileName()); 
//#endif

                    switch (common.REPORT_TYPE)
                    {
                        case NACCS_REPORTTYPE.SAD4021:
                        case NACCS_REPORTTYPE.SAD4011://輸入申告入力控　OTA
                            ProcessSad401Model nac401 = new ProcessSad401Model();
                            if (nac401.SetByteData(btRecv))
                            {
                                var fname = LogOut.FileOutOTA(nac401.GetFileData(), LogOut.MAIL_PATH.RECV_MAIL_PATH, nac401.GetFileName());
                                nac401.NaccsCommon.SetFileNameResultDB(fname, lngSeq);

                                //受信結果をDBに上書きする
                                var rln = ProcessInputModel.SetRecvDataToDB(nac401);
                                if (rln)
                                {
                                    //4031以降のデータが登録されていれば、EDI_STATUSを更新する
                                    nac401.NaccsCommon.SetSameYNOResultDB(lngSeq);

                                    resultstring = string.Format("処理結果通知を登録しました({0}:{1})", nac401.GetHandover, nac401.GetYunyuShinkokuNo);
                                    mail = FindSendMail(common);
                                    if (mail != null)
                                    {
                                        mail.RecvList.Add(nac401);
                                        mail.SetRecvListReportType(common.REPORT_TYPE);
                                        mail.YUNYU_SHINKOKU_NO = nac401.GetYunyuShinkokuNo;
                                        this.Invoke(ssd, "正常", resultstring);
                                    }
                                    else
                                    {
                                        //見つからなかった場合は？
                                        resultstring = string.Format("受信警告:送信データ該当なし({0})", nac401.GetYunyuShinkokuNo);
                                        this.Invoke(ssd, "警告", resultstring);
                                        ////一時保存リストに追加
                                        //i_recvUnknownList.Add(nac401);
                                    }
                                    ret = true;
                                }
                                else
                                {
                                    //DB内に同じ電文がない
                                    resultstring = string.Format("登録異常　DB該当なし({0}:{1})", nac401.GetHandover, nac401.GetInfoNo);
                                    nac401.NaccsCommon.SetDBStatusMemo(lngSeq, strStatus:resultstring);
                                    ////一時保存リストに追加
                                    //i_recvUnknownList.Add(nac401);
                                }
                                ret = true;
                            }
                            else
                            {
                                //401ではない？
                                resultstring = string.Format("処理結果通知　解析異常", nac401.GetHandover, nac401.GetInfoNo);
                                common.GetSQL_UPDATE_RESULT_MEMO_STAT(lngSeq, strStatus:resultstring);
                            }
                            break;
                        case NACCS_REPORTTYPE.SAD4031:
                        case NACCS_REPORTTYPE.SAD4041:
                        case NACCS_REPORTTYPE.SAD4051:
                        case NACCS_REPORTTYPE.SAD4061:
                        case NACCS_REPORTTYPE.SAD4071:
                        case NACCS_REPORTTYPE.SAD4081:
                        case NACCS_REPORTTYPE.SAD4091:
                        case NACCS_REPORTTYPE.SAD4101:
                        case NACCS_REPORTTYPE.SAD4111:
                            ProcessSad403Model nac403 = new ProcessSad403Model();
                            Boolean flg = false;
                            if (nac403.SetByteData(btRecv))
                            {
                                var fname = LogOut.FileOutOTA(nac403.GetFileData(), LogOut.MAIL_PATH.RECV_MAIL_PATH, nac403.GetFileName());
                                nac403.NaccsCommon.SetFileNameResultDB(fname, lngSeq);

                                //受信結果をDBに上書きする
                                flg = ProcessInputModel.SetResultRecvDataToDB(nac403.GetYunyuShinkokuNo, nac403.GetReportType);

                                if (flg)
                                {
                                    flg = false;
                                    //送信メールから同じ輸入申告番号を探す
                                    foreach (var send in i_sendDataList)
                                    {
                                        if (send.YUNYU_SHINKOKU_NO == nac403.GetYunyuShinkokuNo)
                                        {
                                            send.RecvList.Add(nac403);
                                            send.SetRecvListReportType(common.REPORT_TYPE);
                                            mail = send;
                                            this.Invoke(ssd, "受信", string.Format("登録No:{0}({1})", nac403.GetYunyuShinkokuNo, nac403.GetReportType.ToString()));
                                            resultstring = "";
                                            flg = true;
                                            break;
                                        }
                                    }
                                    if (flg)
                                    {
                                        resultstring = string.Format("異常:送信データ 該当なし({0})", nac403.GetYunyuShinkokuNo);
                                        //一時保存リストに追加
                                        i_recvUnknownList.Add(nac403);
                                        this.Invoke(ssd, "警告", resultstring);
                                    }
                                }
                                else
                                {
                                    resultstring = string.Format("異常:DB 該当なし({0})", nac403.GetYunyuShinkokuNo);
                                    //一時保存リストに追加
                                    i_recvUnknownList.Add(nac403);
                                    this.Invoke(ssd, "異常", resultstring);
                                }
                                ret = true;
                            }
                            else
                            {
                                resultstring = string.Format("処理結果通知　解析異常({0})", nac403.NaccsCommon.SUBJECT);
                                common.GetSQL_UPDATE_RESULT_MEMO_STAT(lngSeq, strStatus: resultstring);
                                this.Invoke(ssd, "異常", resultstring);
                            }
                            break;
                        //case NACCS_REPORTTYPE.SAD4131:
                        //    break;
                        case NACCS_REPORTTYPE.SAF0010:
                            ProcessSaf001Model nac10 = new ProcessSaf001Model();
                            if (nac10.SetByteData(btRecv))
                            {
                                //結果ファイルを加工して保存
                                var fname = LogOut.FileOutOTA(nac10.GetFileData(), LogOut.MAIL_PATH.RECV_MAIL_PATH, nac10.GetFileName());
                                nac10.NaccsCommon.SetFileNameResultDB(fname, lngSeq);
                                ret = ProcessInputModel.SetResultRecvDataToDB(nac10.GetYunyuShinkokuNo, nac10.GetReportType);

                                resultstring = string.Format("通知　受信 No:{0}({1})", nac10.GetYunyuShinkokuNo, nac10.GetReportType.ToString());
                                this.Invoke(ssd, "正常", resultstring);
                                flg = true;
                            }
                            else
                            {
                                resultstring = string.Format("通知　解析異常({0}:{1})", common.REPORT_TYPE, common.SUBJECT);
                                this.Invoke(ssd, "異常", resultstring);
                                flg = false;
                            }
                            break;
                        case NACCS_REPORTTYPE.SAF0021:
                            ProcessSaf002Model nac21 = new ProcessSaf002Model();
                            if (nac21.SetByteData(btRecv))
                            {
                                //結果ファイルを加工して保存
                                var fname = LogOut.FileOutOTA(nac21.GetFileData(), LogOut.MAIL_PATH.RECV_MAIL_PATH, nac21.GetFileName());
                                nac21.NaccsCommon.SetFileNameResultDB(fname, lngSeq);
                                ret = ProcessInputModel.SetResultRecvDataToDB(nac21.GetYunyuShinkokuNo, nac21.GetReportType);

                                resultstring = string.Format("通知　受信 No:{0}({1})", nac21.GetYunyuShinkokuNo, nac21.GetReportType.ToString());
                                this.Invoke(ssd, "正常", resultstring);
                                flg = true;
                            }
                            else
                            {
                                resultstring = string.Format("通知　解析異常({0}:{1})", common.REPORT_TYPE, common.SUBJECT);
                                this.Invoke(ssd, "異常", resultstring);
                                flg = false;
                            }
                            break;
                        case NACCS_REPORTTYPE.SAF0211:
                            ProcessSaf021Model nac211 = new ProcessSaf021Model();
                            if (nac211.SetByteData(btRecv))
                            {
                                //結果ファイルを加工して保存
                                var fname = LogOut.FileOutOTA(nac211.GetFileData(), LogOut.MAIL_PATH.RECV_MAIL_PATH, nac211.GetFileName());
                                nac211.NaccsCommon.SetFileNameResultDB(fname, lngSeq);
                                ret = ProcessInputModel.SetResultRecvDataToDB(nac211.GetYunyuShinkokuNo, nac211.GetReportType);

                                resultstring = string.Format("通知　警告受信 No:{0}({1})", nac211.GetYunyuShinkokuNo, nac211.GetReportType.ToString());
                                this.Invoke(ssd, "正常", resultstring);
                                flg = true;
                            }
                            else
                            {
                                resultstring = string.Format("通知　解析異常({0}:{1})", common.REPORT_TYPE, common.SUBJECT);
                                this.Invoke(ssd, "異常", resultstring);
                                flg = false;
                            }
                            break;
                        case NACCS_REPORTTYPE.SAF0221:
                            ProcessSaf022Model nac221 = new ProcessSaf022Model();
                            if (nac221.SetByteData(btRecv))
                            {
                                //結果ファイルを加工して保存
                                var fname = LogOut.FileOutOTA(nac221.GetFileData(), LogOut.MAIL_PATH.RECV_MAIL_PATH, nac221.GetFileName());
                                nac221.NaccsCommon.SetFileNameResultDB(fname, lngSeq);
                                ret = ProcessInputModel.SetResultRecvDataToDB(nac221.GetYunyuShinkokuNo, nac221.GetReportType);

                                resultstring = string.Format("通知　警告受信 No:{0}({1})", nac221.GetYunyuShinkokuNo, nac221.GetReportType.ToString());
                                this.Invoke(ssd, "正常", resultstring);
                                flg = true;
                            }
                            else
                            {
                                resultstring = string.Format("通知　解析異常({0}:{1})", common.REPORT_TYPE, common.SUBJECT);
                                this.Invoke(ssd, "異常", resultstring);
                                flg = false;
                            }
                            break;
                        case NACCS_REPORTTYPE.SYG_RESULT:
                            ProcessSYGRecvModel syg = new ProcessSYGRecvModel();
                            if (syg.SetByteData(btRecv))
                            {
                                mail = FindSendMail(syg.NaccsCommon);
                                if (mail != null)
                                {
                                    mail.RecvList.Add(syg);
                                    mail.SetRecvListReportType(NACCS_REPORTTYPE.RESULT);
                                    mail.ProcessFinFLG = true;
                                }
                                ret = true;
                            }
                            else
                            {
                                resultstring = "";
                            }
                            break;
                        case NACCS_REPORTTYPE.SYG_LIST:
                            ProcessSYGRecvModel syglist = new ProcessSYGRecvModel();
                            if (syglist.SetByteData(btRecv))
                            {
                                mail = FindSendMail(syglist.NaccsCommon);
                                if (mail != null)
                                {
                                    mail.RecvList.Add(syglist);
                                    mail.SetRecvListReportType(NACCS_REPORTTYPE.RESULT);
                                    mail.ProcessFinFLG = true;
                                }
                                if (syglist.InfoNum.GetIntData > 0)
                                {
                                    string log = "";
                                    foreach(var info in syglist.InfoList)
                                    {
                                        i_sygstacklist.Add(info.INFO_CD.Data);
                                        log += info.INFO_CD.Data +";";
                                    }
                                    LogOut.SendErrorMail(string.Format("【{0}】 SYG", Application.ProductName),
                                        string.Format("SYG受信　{0}件\r\n{1}\r\n{2}",syglist.InfoNum.GetIntData, log, syglist.GetFileName()),
                                        string.Format("SYG受信　{0}件\r\n{1}\r\n{2}",syglist.InfoNum.GetIntData, log, syglist.GetFileName())
                                        );                        
                                }
                                ret = true;
                            }
                            break;
                        case NACCS_REPORTTYPE.RESULT:
                            ProcessResultModel nacret = new ProcessResultModel();
                            if (nacret.SetByteData(btRecv))
                            {
                                ////結果ファイルを加工して保存
                                //var fname = LogOut.FileOutOTA(nacret.GetFileData(), LogOut.MAIL_PATH.RECV_MAIL_PATH, nacret.GetFileName());
                                //nacret.NaccsCommon.SetFileNameResultDB(fname, lngSeq);

                                //通常業務
                                var rln = ProcessInputModel.SetRecvDataToDB(nacret.NaccsCommon.DENBUN_HANDOVER.Data, nacret.OUTPUT_RESULT, lngSeq, nacret.GetYunyuShinkokuNo );
                                if (rln)
                                {
                                    resultstring = string.Format("処理結果通知を登録しました({0}:{1})", nacret.NaccsCommon.DENBUN_HANDOVER.Data, nacret.ResultList[0].Data);
                                    this.Invoke(ssd, "正常", resultstring);
                                    ret = true;
                                }
                                else
                                {
                                    i_recvUnknownList.Add(nacret);
                                    resultstring = string.Format("登録異常　該当なし({0}:{1})", nacret.GetHandover, nacret.ResultList[0].Data);
                                    this.Invoke(ssd, "異常", resultstring);
                                }
                            }
                            else
                            {
                                resultstring = string.Format("処理結果通知　解析異常({0}:{1})", nacret.GetHandover, nacret.ResultList[0].Data);
                                this.Invoke(ssd, "異常", resultstring);
                            }
                            //送信メール検索
                            mail = FindSendMail(nacret.NaccsCommon);
                            if (mail != null)
                            {
                                mail.RecvList.Add(nacret);
                                mail.SetRecvListReportType(common.REPORT_TYPE);
                                if (!nacret.RESULT_FLG)
                                {
                                    //異常状態なので次へ
                                    mail.ProcessFinFLG = true;
                                }                                
                            }
                            else
                            {
                                //i_recvUnknownList.Add(nacret);
                                //見つからなかった場合は？
                                resultstring += ((resultstring.Length > 0) ? "　　" : "") + string.Format("受信異常:送信データ該当なし({0})", nacret.GetHandover);
                                //結果の中に異常を通知
                                nacret.NaccsCommon.GetSQL_UPDATE_RESULT_MEMO_STAT(lngSeq, strStatus: resultstring);
                            }
                            break;
                        case NACCS_REPORTTYPE.OTA:
                        case NACCS_REPORTTYPE.OTC:
                        case NACCS_REPORTTYPE.NONE:
                        default:
                            resultstring = string.Format("処理結果通知　内容該当なし（{0}:{1})", common.DENBUN_HANDOVER.Data, common.INPUT_INFO_NO.Data);
                            this.Invoke(ssd, "警告", string.Format("処理結果通知　内容該当なし（{0}:{1})", common.DENBUN_HANDOVER.Data, common.INPUT_INFO_NO.Data));
                            break;
                    }
                }

                //未処理データの確認
                AssignRecvUnknownData(ref i_recvUnknownList);

                //if (mail != null)
                //{
                //    if (mail.IsUse && i_sendstacklist.Count > 0)
                //    {
                //        //送信タスクを起動
                //        this.taskSend = new Task(SendFunc);
                //        this.taskSend.Start();
                //    }
                //}
                return ret;
            }
            catch (Exception e)
            {
                LogOut.ErrorOut(e.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name, true, e.Message);
                this.Invoke(ssd, "異常", e.Message);
                //throw e;
                return false;
            }
        }

        /// <summary>
        /// 受信した結果を送信メールアドレスと一致するものを登録する
        /// </summary>
        /// <param name="recvData"></param>
        /// <returns></returns>
        public SendOnlyDataClass FindSendMail(OutputCommonModel outCom)
        {
            SendOnlyDataClass sendData = null;
            try
            {
                //送信メールから同じ電文引継情報を探す
                foreach (var mail in i_sendDataList)
                {
                    if (mail.MessageSYG != null)
                    {
                        if (mail.MessageSYG.GetHandover == outCom.DENBUN_HANDOVER.Data)
                        {
                            sendData = (SendOnlyDataClass)mail;
                            break;
                        }
                    }
                    else
                    {
                        if (mail.Message != null && mail.Message.GetHandover == outCom.DENBUN_HANDOVER.Data)
                        {
                            sendData = (SendOnlyDataClass)mail;
                            break;
                        }
                    }
                }
                
                return sendData;
            }
            catch
            {
                throw;
            }
        }
 
        /// <summary>
        /// 未送信データの有無確認 ★★★
        /// </summary>
        /// <returns></returns>
        public static int ConfirmSnedData()
        {
            DataBaseSQL objDb = null;
            DataSet dsRecord = null;
            objDb = new DataBaseSQL();

            try
            {
                objDb.DBLogIn();
//#if DEBUG
//                LogOut.InfoOut("DB確認", MethodBase.GetCurrentMethod().Name);
//#endif

                dsRecord = objDb.GetDataSet(ProcessInputModel.GetSQL_UNSENTCOUNT(), "T_PAX_H", false);

                if (dsRecord.Tables[0].Rows.Count > 0)
                {
                    i_sendstacklist2 = DBEdiSend.GetDBEdiSendModel(dsRecord.Tables[0], NACCS_REPORTTYPE.OTA);
                    i_sendfinlist2 = new List<DBEdiSend>();
                    return dsRecord.Tables[0].Rows.Count;
                    //List<DataRow> data = dsRecord.Tables[0].AsEnumerable().ToList<DataRow>();//Cast<String>().ToArray();
                    //i_sendstacklist = new List<DataRow>(data);
                    //i_sendfinlist = new List<DataRow>();
                    //return dsRecord.Tables[0].Rows.Count;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, "FormStart", MethodBase.GetCurrentMethod().Name);
                return 0;
            }
            finally
            {
                if (objDb != null)
                {
                    objDb.DBLogOut();
                }
            }

        }
        
        /// <summary>
        /// 使用できるメールアドレスを確認する
        /// </summary>
        /// <returns></returns>
        public static void ConfirmUseAddress()
        {
            try
            {
                //送信メールから同じ電文引継情報を探す
                foreach (var mail in i_sendDataList)
                {
                    if (mail.SendDate == DateTime.MinValue)
                    {
                        break;
                    }
                    if (mail.SendDate.AddSeconds(SendOnlyDataClass.RECV_TIME_OUT_SEC) < DateTime.Now)
                    {
                        string str = "送信タイムアウト";
                        string str2 = "サーバー未応答";
                        if (mail.Message != null)
                        {
                            str2 += string.Format("(PAX_NO:{0}_{1})", mail.Message.PAX_NO, mail.Message.EDA_NO);
                        }
                        else
                        {
                            str2 += "(SYG通信)";
                        }
                        //if (mail.SendNum < HPFData.SendMaxNum)
                        //{
                        //    LogOut.InfoOut(string.Format("{0}:NACCS{1} 再送信[{2}]", str, str2, mail.SendNum), MethodBase.GetCurrentMethod().Name);
                        //    //再送信
                        //    mail.SmtpSend((mail.MessageSYG != null) ? true : false);
                        //}
                        //else
                        //{
                            if (mail.Message != null)
                            {
                                //タイムアウトの詳細を追記
                                mail.Message.SetResultDB(str, str2);
                            }
                            //処理終了
                            mail.ProcessFinFLG = true;
                            
                            LogOut.ErrorOut(string.Format("{0}:NACCS{1}", str, str2), "FormStart", MethodBase.GetCurrentMethod().Name, blnSendMail:true, strType:str);
                        //}
                    }
                }
            }
            catch(Exception ex)
            {
                throw 
                    ex;
            }

        }
        /// <summary>
        /// NACCS送信するためのユーザー情報等を取得する
        /// </summary>
        /// <returns></returns>
        public Boolean ConfirmGetJotaiAndTanto()
        {
            DataBaseSQL objDb = null;
            DataSet dsRecord = null;
            objDb = new DataBaseSQL();
            Boolean blnRet = false;
            
            try
            {
                objDb.DBLogIn();

                dsRecord = objDb.GetDataSet(GetSQL_CALENDAR_TANTO(DateTime.Now), "M_CALENDAR", false);

                if (dsRecord.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dsRecord.Tables[0].Rows[0];
                    var flg = Common.ConvertToInteger(row["YUNYUSHINKOKU_KADOJOTAI"]);
                    if (flg == 1)
                    {
                        var kbn = Common.ConvertToInteger(row["TIME_KBN"]);
                        var strUser = "";
                        var strPass = "";
                        if (kbn == 1)
                        {
                            strUser = Common.ConvertToString(row["CD1"]);
                            strPass = Common.ConvertToString(row["PW1"]);
                        }
                        else
                        {
                            strUser = Common.ConvertToString(row["CD2"]);
                            strPass = Common.ConvertToString(row["PW2"]);
                        }
                        if (string.IsNullOrEmpty(strUser) || string.IsNullOrEmpty(strPass) || strUser.Length < 8)
                        {
                            HPFData.InputUserCode = HPFData.InputUserCodeDefault;
                            HPFData.InputUserPass = HPFData.InputUserPassDefault;
                            HPFData.NaccsShikibetsuNo = HPFData.NaccsShikibetsuNoDefault;
                            LogOut.InfoOut("UserCode or UserPass Nothing.", "FormStart", MethodBase.GetCurrentMethod().Name);
                        }
                        else
                        {
                            HPFData.InputUserCode = strUser.Substring(0, strUser.Length - 3);
                            HPFData.NaccsShikibetsuNo = strUser.Substring(strUser.Length - 3, 3);
                            HPFData.InputUserPass = strPass;
                        }
                        blnRet = true;
                    }
                    else
                    {
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, "FormStart", MethodBase.GetCurrentMethod().Name);
            }
            finally
            {
                if (objDb != null)
                {
                    objDb.DBLogOut();
                }
            }
            return blnRet;
        }
        /// <summary>
        /// DBからデータを取得
        /// </summary>
        /// <returns></returns>
        public static string GetSQL_CALENDAR_TANTO(DateTime dtm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("SELECT");
            strSql.AppendLine("  (");
            strSql.AppendLine("    SELECT TOP 1");
            strSql.AppendLine("      KADOJOTAI");
            strSql.AppendLine("    FROM T_DECLARATION_STATUS");
            strSql.AppendFormat("    WHERE SHORI_SHURUI = '{0}'", HPFData.SHORI_SHURUI_DECLARATION).AppendLine();
            strSql.AppendLine("  ) AS YUNYUSHINKOKU_KADOJOTAI");
            strSql.AppendLine("  ,(");
            strSql.AppendLine("    SELECT TOP 1");
            strSql.AppendLine("      KADOJOTAI");
            strSql.AppendLine("    FROM T_DECLARATION_STATUS");
            strSql.AppendFormat("    WHERE SHORI_SHURUI = '{0}'", HPFData.SHORI_SHURUI_PAXTRAX).AppendLine();
            strSql.AppendLine("  ) AS PAXTRAX");
            strSql.AppendLine("	 ,(");
            strSql.AppendLine("    SELECT TOP 1 KBN_CODE FROM C_KYOTHU_KBN AS C");
            strSql.AppendLine("    WHERE 1=1");
            strSql.AppendLine("      AND C.SIKIBETU_KBN = '420'");
            strSql.AppendFormat("      AND SONOTA1 <= '{0}'",dtm.ToString("HH:mm")).AppendLine();
            strSql.AppendFormat("      AND SONOTA2 >= '{0}'", dtm.ToString("HH:mm")).AppendLine();
            strSql.AppendLine("  ) AS TIME_KBN");
            strSql.AppendLine("  ,CALENDAR_KBN");
            strSql.AppendLine("  ,CALENDAR_DATE");
            strSql.AppendLine("  ,TANTO1");
            strSql.AppendLine("  ,STF1.TSUKANSHI_CD AS CD1");
            strSql.AppendLine("  ,STF1.TSUKANSHI_PW AS PW1");
            strSql.AppendLine("  ,TANTO2");
            strSql.AppendLine("  ,STF2.TSUKANSHI_CD AS CD2");
            strSql.AppendLine("  ,STF2.TSUKANSHI_PW AS PW2");
            strSql.AppendLine("FROM M_CALENDAR AS CAL");
            strSql.AppendLine("LEFT JOIN M_STAFF AS STF1");
            strSql.AppendLine("ON STF1.STAFF_CD = CAL.TANTO1");
            strSql.AppendLine("AND STF1.DELETE_FLG is NULL");
            strSql.AppendLine("LEFT JOIN M_STAFF AS STF2");
            strSql.AppendLine("ON STF2.STAFF_CD = CAL.TANTO2");
            strSql.AppendLine("AND STF2.DELETE_FLG is NULL");
            strSql.AppendLine("WHERE 1=1");
            strSql.AppendFormat("  AND CAL.CALENDAR_DATE = CONVERT (date, '{0}')", dtm.ToString("yyyy/MM/dd")).AppendLine();
            return strSql.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="useflg"></param>
        /// <returns></returns>
        public static string GetSQL_UPDATE_T_DECLARATION_STATUS(Boolean useflg = false)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("UPDATE T_DECLARATION_STATUS");
            strSql.AppendLine("  SET");
            strSql.AppendFormat("  UPD_STAFF_CD = '{0}'", HPFData.AppUserName).AppendLine();
            strSql.AppendFormat("  ,UPD_DATE = GETDATE()", "").AppendLine();
            strSql.AppendFormat("  ,UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
            strSql.AppendFormat("  ,VERSION_NO = VERSION_NO + 1", "").AppendLine();

            strSql.AppendFormat("  ,KADOJOTAI = {0}", (useflg?"1":"0")).AppendLine();

            strSql.AppendLine("WHERE 1=1");
            strSql.AppendFormat("  AND SHORI_SHURUI = '{0}'", HPFData.SHORI_SHURUI_NACCS).AppendLine();
            if(useflg)
            {
                strSql.AppendFormat("  AND 0 = ").AppendLine();
                strSql.AppendFormat("    (SELECT KADOJOTAI FROM T_DECLARATION_STATUS WHERE SHORI_SHURUI = '{0}')",
                    HPFData.SHORI_SHURUI_PAXTRAX).AppendLine();
            }
            return strSql.ToString();
        }



        /// <summary>
        /// 判明していない受信データの割り振り
        /// </summary>
        public static void AssignRecvUnknownData(ref List<NaccsRecvModel> recvUnknownlist)
        {
            if (recvUnknownlist != null && recvUnknownlist.Count > 0)
            {
                Mutex mut = new Mutex();
                mut.WaitOne();
                try
                {
                    foreach (var recv in recvUnknownlist)
                    {
                        //受信結果をDBに上書きする
                        Boolean flg = ProcessInputModel.SetResultRecvDataToDB(recv.GetYunyuShinkokuNo, recv.GetReportType);
                        if (flg)
                        {
                            foreach (var send in i_sendDataList)
                            {
                                if (send.YUNYU_SHINKOKU_NO == recv.GetYunyuShinkokuNo)
                                {
                                    send.RecvList.Add(recv);
                                    //recv.NaccsCommon.SetDBResultMemo(strMemo: "");
                                }
                            }
                            recvUnknownlist.Remove(recv);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogOut.ErrorOut(ex.Message, "FormStart", MethodBase.GetCurrentMethod().Name);
                }
                finally
                {
                    mut.ReleaseMutex();
                }
            }
        }

        ///// <summary>
        ///// 送信確認タスク　OTA-OTE
        ///// </summary>
        //public void SendFunc()
        //{
        //    DataBaseSQL objDb = null;
        //    DataSet dsRecord = null;
        //    objDb = new DataBaseSQL();
        //    Boolean sflg = false;
        //    SetShowLogDelegate ssd = new SetShowLogDelegate(SetShowLog);
        //    string strWhere = "";

        //    try
        //    {
        //        //使われていないメールがあるか？
        //        //メールListがNULLまたはFinishの場合
        //        //なければERRORで返す？
        //        foreach (var send in i_sendDataList)
        //        {
        //            if (send.IsUse)
        //            {
        //                objDb.DBLogIn();

        //                DataRow selectRow = i_sendstacklist.First();
        //                i_sendfinlist.Add(selectRow);
        //                i_sendstacklist.Remove(selectRow);
                        
        //                string pno = selectRow.Field<string>("PAX_NO");
        //                string eno = selectRow.Field<string>("EDA_NO");

        //                //一件だけ取得する
        //                dsRecord = objDb.GetDataSet(ProcessInputModel.GetSQL_SELECT(pno, eno), "T_PAX", false);

        //                if (dsRecord != null)
        //                {
        //                    //送信内容のリセット
        //                    send.Reset();
        //                    List<DataTable> dtList = new List<DataTable>();

        //                    //PAX？毎に振り分け
        //                    ProcessInputModel.AnalyzedDataSet(dsRecord, out dtList, out strWhere);

        //                    if (dtList.Count > 0)
        //                    {
        //                        this.Invoke(ssd, "送信", "EDI送信処理　開始 = " + (dtList[0].Rows[0])["PAX_NO"].ToString() + " " +(dtList[0].Rows[0])["EDA_NO"].ToString());
        //                        LogOut.InfoOut("EDI送信処理　開始 :" + (dtList[0].Rows[0])["PAX_NO"].ToString() + " " + (dtList[0].Rows[0])["EDA_NO"].ToString(), this.Name, MethodBase.GetCurrentMethod().Name);

        //                        ProcessInputModel input;
        //                        InputCommonModel common = new InputCommonModel();

        //                        ////取得データをすべて排他処理をする。
        //                        //objDb.BeginTransaction();
        //                        //objDb.ExecuteNonQuery(ProcessInputModel.GetSQL_HAITA("T_PAX_H", string.Format("AND PAX_NO = '{0}'", pno), true), null);
        //                        //objDb.Commit();

        //                        if (this.i_SendStop)
        //                        {
        //                            this.Invoke(ssd, "警告", "送信処理一時停止");
        //                            break;
        //                        }

        //                        input = new ProcessInputModel();
        //                        input.SetData(dtList[0]);

        //                        send.Message = input;

        //                        objDb.BeginTransaction();
        //                        sflg = true;
        //                        int intRet = 0;
        //                        objDb.ExecuteNonQuery(input.GetSQL_UPDATE_SEND_H(), null, out intRet);
        //                        if (intRet == 0)
        //                        {
        //                            LogOut.InfoOut("T_PAX_H 該当データなし", "FormStart", MethodBase.GetCurrentMethod().Name);
        //                        }
        //                        objDb.ExecuteNonQuery(input.GetSQL_UPDATE_SEND_D(), null);

        //                        objDb.Commit();

        //                        //送信処理
        //                        send.SmtpSend();

        //                        //i_sendfinlist.Add(selectRow);
        //                        //i_sendstacklist.Remove(selectRow);

        //                        this.Invoke(ssd, "送信", String.Format("PAX NO:{0} {1}", input.PAX_NO, input.EDA_NO));
        //                    }//PAX毎に振り分け
        //                    else
        //                    {
        //                        LogOut.InfoOut(string.Format("EDI送信処理　T_PAX_D データなし :{0}",pno), this.Name, MethodBase.GetCurrentMethod().Name);
        //                        objDb.BeginTransaction();
        //                        sflg = true;

        //                        objDb.ExecuteNonQuery(ProcessInputModel.GetSQL_UPDATE_SEND_H_ERR(pno, eno, "T_PAX_D データなし"), null);

        //                        objDb.Commit();

        //                        //i_sendfinlist.Add(selectRow);
        //                        //i_sendstacklist.Remove(selectRow);
        //                    }
        //                }
        //                else
        //                {
        //                    //task終了
        //                }
        //            }
        //            else
        //            {
        //                //空きがないので終了
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (sflg)
        //        {
        //            objDb.RollBack();
        //        }
        //        LogOut.ErrorOut(ex.Message, "FormStart", MethodBase.GetCurrentMethod().Name, true, ex.Message);
        //        this.Invoke(ssd, "異常", ex.Message);
        //    }
        //    finally
        //    {
        //        if (objDb != null)
        //        {
        //            if (strWhere != "")
        //            {
        //                ////取得データをすべて排他処理の解除をする
        //                //objDb.BeginTransaction();
        //                //objDb.ExecuteNonQuery(ProcessInputModel.GetSQL_HAITA("T_PAX_H", strWhere), null);
        //                //objDb.Commit();
        //            }
        //            objDb.DBLogOut();
        //        }
        //    }
        //}

        /// <summary>
        /// 送信確認タスク　OTA-OTE
        /// </summary>
        public void SendFunc()
        {
            DataBaseSQL objDb = null;
            DataSet dsRecord = null;
            objDb = new DataBaseSQL();
            Boolean sflg = false;
            SetShowLogDelegate ssd = new SetShowLogDelegate(SetShowLog);
            string strWhere = "";

            try
            {
                //使われていないメールがあるか？
                //メールListがNULLまたはFinishの場合
                //なければERRORで返す？
                foreach (var send in i_sendDataList)
                {
                    if (send.IsUse)
                    {
                        //このメールアドレスを使用する宣言！
                        send.ProcessFinFLG = false;

                        objDb.DBLogIn();

                        DBEdiSend selectRow = i_sendstacklist2.FirstOrDefault();

                        if (selectRow == null)
                        {
                            break;
                        }
                        i_sendstacklist2.Remove(selectRow);

                        DataTable dtcnt = objDb.GetDataTable(selectRow.GETSQL_SELECT_CNT(), "T_EDI_SEND", false);
                        if (dtcnt != null)
                        {
                            //送信済み！！
                            continue;
                        }
                        else
                        {
                            //T_EDI_SENDに登録
                            var seq = objDb.ExecuteScalar(selectRow.GetSQL_INSERT());
                            selectRow.DATA_SEQ = seq;
                        }

                        //一件だけ取得する
                        dsRecord = objDb.GetDataSet(ProcessInputModel.GetSQL_SELECT(selectRow), "T_PAX", false);

                        if (dsRecord != null)
                        {
                            //送信内容のリセット
                            send.Reset();
                            List<DataTable> dtList = new List<DataTable>();

                            //PAX？毎に振り分け
                            ProcessInputModel.AnalyzedDataSet(dsRecord, out dtList, out strWhere);

                            if (dtList.Count > 0)
                            {
                                this.Invoke(ssd, "送信", "EDI送信処理　開始 = " + (dtList[0].Rows[0])["PAX_NO"].ToString() + " " + (dtList[0].Rows[0])["EDA_NO"].ToString());
                                LogOut.InfoOut("EDI送信処理　開始 :" + (dtList[0].Rows[0])["PAX_NO"].ToString() + " " + (dtList[0].Rows[0])["EDA_NO"].ToString(), this.Name, MethodBase.GetCurrentMethod().Name);

                                ProcessInputModel input;
                                InputCommonModel common = new InputCommonModel();

                                ////取得データをすべて排他処理をする。
                                //objDb.BeginTransaction();
                                //objDb.ExecuteNonQuery(ProcessInputModel.GetSQL_HAITA("T_PAX_H", string.Format("AND PAX_NO = '{0}'", pno), true), null);
                                //objDb.Commit();

                                if (this.i_SendStop)
                                {
                                    this.Invoke(ssd, "警告", "送信処理一時停止");
                                    break;
                                }

                                input = new ProcessInputModel();
                                input.SetData(dtList[0]);

                                send.Message = input;

                                objDb.BeginTransaction();

                                objDb.ExecuteNonQuery(selectRow.GetSQL_UPDATE_HANDOVER(input.GetHandover), null);

                                sflg = true;
                                int intRet = 0;
                                objDb.ExecuteNonQuery(input.GetSQL_UPDATE_SEND_H(), null, out intRet);
                                if (intRet == 0)
                                {
                                    LogOut.InfoOut("T_PAX_H 該当データなし", "FormStart", MethodBase.GetCurrentMethod().Name);
                                }
                                objDb.ExecuteNonQuery(input.GetSQL_UPDATE_SEND_D(), null);

                                objDb.Commit();

                                //送信処理
                                send.SmtpSend();

                                //i_sendfinlist.Add(selectRow);
                                //i_sendstacklist.Remove(selectRow);

                                this.Invoke(ssd, "送信", String.Format("PAX NO:{0} {1}", input.PAX_NO, input.EDA_NO));
                            }//PAX毎に振り分け
                            else
                            {
                                LogOut.InfoOut(string.Format("EDI送信処理　T_PAX_D データなし :{0}", selectRow.PAX_NO), this.Name, MethodBase.GetCurrentMethod().Name);
                                objDb.BeginTransaction();
                                sflg = true;

                                objDb.ExecuteNonQuery(ProcessInputModel.GetSQL_UPDATE_SEND_H_ERR(selectRow.PAX_NO, selectRow.EDA_NO, "T_PAX_D データなし"), null);

                                objDb.Commit();

                                //i_sendfinlist.Add(selectRow);
                                //i_sendstacklist.Remove(selectRow);
                            }
                        }
                        else
                        {
                            //task終了
                        }
                    }
                    else
                    {
                        //空きがないので終了
                    }
                }
            }
            catch (Exception ex)
            {
                if (sflg)
                {
                    objDb.RollBack();
                }
                LogOut.ErrorOut(ex.Message, "FormStart", MethodBase.GetCurrentMethod().Name, true, ex.Message);
                this.Invoke(ssd, "異常", ex.Message);
            }
            finally
            {
                if (objDb != null)
                {
                    if (strWhere != "")
                    {
                        ////取得データをすべて排他処理の解除をする
                        //objDb.BeginTransaction();
                        //objDb.ExecuteNonQuery(ProcessInputModel.GetSQL_HAITA("T_PAX_H", strWhere), null);
                        //objDb.Commit();
                    }
                    objDb.DBLogOut();
                }
            }
        }

        /// <summary>
        /// 送信確認タスク SYG
        /// </summary>
        public void SYGFunc()
        {
            SetShowLogDelegate ssd = new SetShowLogDelegate(SetShowLog);
            try
            {
                LogOut.InfoOut("SYG送信処理開始", this.Name, MethodBase.GetCurrentMethod().Name);

                //使われていないメールがあるか？
                //メールListがNULLまたはFinishの場合
                //なければERRORで返す？
                foreach (var send in i_sendDataList)
                {
                    if (send.IsUse)
                    {
                        //送信内容のリセット
                        send.Reset();

                        ProcessSYGSendModel syg = new ProcessSYGSendModel(ProcessSYGSendModel.SendType.GET_LIST);
                        send.MessageSYG = syg;

                        send.SmtpSend(true);

#if DEBUG
                        this.Invoke(ssd, "確認", "SYG通信確認");
#endif
                    }
                }
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                this.Invoke(ssd, "異常", ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 送信タスク OTHER
        /// </summary>
        public void OTHERFunc()
        {
            SetShowLogDelegate ssd = new SetShowLogDelegate(SetShowLog);
            try
            {
                LogOut.InfoOut("OTHER送信処理開始", this.Name, MethodBase.GetCurrentMethod().Name);

                string fname = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + HPFData.TempMessageFileName;
                //ファイルがあるか確認する

                if (System.IO.File.Exists(fname))
                {
                    //内容を確認する。
                    //xmlファイルを指定する
                    XElement xml = XElement.Load(fname);

                    string strInfoCd = xml.Element("GyomuCd").Value;
                    string strSysType = xml.Element("SystemType").Value;
                    string strSendMes = xml.Element("Message").Value;

                    if (!string.IsNullOrEmpty(strInfoCd) &&
                        !string.IsNullOrEmpty(strSysType) &&
                        !string.IsNullOrEmpty(strSendMes))
                    {
                        ProcessSYGSendModel syg = new ProcessSYGSendModel(strInfoCD: strInfoCd, strSysType: strSysType, strSendMes: strSendMes);

                        SendClass smail = new SendClass(
                            HPFData.ServerAddress,
                            HPFData.PortNo,
                            HPFData.ServerDomain,
                            HPFData.FromMailAddress,
                            HPFData.ToMailAddress,
                            HPFData.MyServerName
                            );

                        smail.MessageSYG = syg;

                        i_sendList.Add(smail);

                        smail.SmtpSend(true);

                        this.Invoke(ssd, "送信", "OTHER通信確認");
                    }
                }
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                this.Invoke(ssd, "異常", ex.Message);
            }
        }

        /// <summary>
        /// 送信結果再取得タスク
        /// </summary>
        public void SYGResultFunc()
        {
            SetShowLogDelegate ssd = new SetShowLogDelegate(SetShowLog);
            try
            {
                LogOut.InfoOut("SYG再送信要求処理開始", this.Name, MethodBase.GetCurrentMethod().Name);

                foreach (var send in i_sendDataList)
                {
                    if (send.IsUse)
                    {
                        //送信内容のリセット
                        send.Reset();

                        string info = i_sygstacklist[0];
                        if (!string.IsNullOrEmpty(info))
                        {
                            ProcessSYGSendModel syg = new ProcessSYGSendModel(ProcessSYGSendModel.SendType.GET_RESULT, info);
                            send.MessageSYG = syg;

                            send.SmtpSend(true);

                            this.Invoke(ssd, "送信", "SYG再送信要求");
                            i_sygfinlist.Add(i_sygstacklist[0]);
                            i_sygstacklist.RemoveAt(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                this.Invoke(ssd, "異常", ex.Message);
            }
        }


        /// <summary>
        /// AppConfig Read
        /// </summary>
        /// <returns></returns>
        public bool SettingFileRead()
        {
            try
            {
                string fname = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + HPFData.TempMessageFileName;
                //ファイルがあるか確認する

                if (System.IO.File.Exists(fname))
                {
                    StringBuilder sb = new StringBuilder();
                    //内容を確認する。
                    //xmlファイルを指定する
                    XElement xml = XElement.Load(fname);

                    IEnumerable<XElement> infos = from item in xml.Elements("NaccsSendMessage")
                                                  select item;
                    foreach (XElement info in infos)
                    {
                        sb.AppendLine(info.Element("GyomuCd").Value);
                        sb.AppendLine(info.Element("SystemType").Value);
                        sb.AppendLine(info.Element("Message").Value).AppendLine();
                    }
                    MessageBox.Show(sb.ToString());
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        //private void FormStart_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    //イベントを解放する
        //    //フォームDisposeメソッド内の基本クラスのDisposeメソッド呼び出しの前に
        //    //記述してもよい
        //    SystemEvents.SessionEnding -=
        //        new SessionEndingEventHandler(SystemEvents_SessionEnding);
        //}

        ////ログオフ、シャットダウンしようとしているとき
        //private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        //{
        //    string s = "";
        //    if (e.Reason == SessionEndReasons.Logoff)
        //    {
        //        s = "ログオフ";
        //    }
        //    else if (e.Reason == SessionEndReasons.SystemShutdown)
        //    {
        //        s = "シャットダウン";
        //    }
        //    LogOut.ErrorOut(string.Format("システム{0}により終了します", s), MethodBase.GetCurrentMethod().Name);
        //}
    }
}
