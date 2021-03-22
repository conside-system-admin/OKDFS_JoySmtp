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
    public partial class Form1 : Form
    {
        private InitProject InitProject = new InitProject ();
        //private SendData i_sendMes;

        private Boolean i_AutoFlg = false;
        //private int SYG_INTERVAL= 30;

        //private int i_SYGCnt = 0;
        //private Boolean i_FirstFlg_O = false;
        //private Boolean i_FirstFlg_S = false;

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
            sb.AppendFormat("TO MAIL : {0}",HPFData.ToMailAddress).AppendLine();
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
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// フォーム画面表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //ログファイルの初期化
            LogOut.InitLogOut();

            LogOut.InfoOut("起動", this.Name, MethodBase.GetCurrentMethod().Name);
            //LogOut.ErrorOut("起動", this.Name, MethodBase.GetCurrentMethod().Name);
            //LogOut.ConnectOut("1234", "OTA", strInfo:"Connect Success\r\nTest OK");

            i_logs = new BindingList<Item>();
            i_sendList = new BindingList<SendClass>();

            itemBindingSource.DataSource = i_logs;

            //データベースアクセス関連
            InitProject.InitPublicInfo("");

            //タイマー設定
            this.toolStripTime.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            timerNowTime.Interval = 1000;
            timerNowTime.Enabled = true;
            this.i_AutoFlg = false;

            //フォーム画面設定表示
            SetTxtAppConfig();

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
            this.taskSYGget = null;

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
                    SetShowLog("GET", string.Format("{0}:{1}", no, s));
                    if (no == 1)
                    {
                        HPFData.ServerAddress = s;
                    }
                    else if (no == 2)
                    {
                        HPFData.ServerBackAddress = s;
                    }
                    no++;
                }

                string[] answers2 = DnsConnect.LookupDns(hostOrAddress, recordType, HPFData.DnsServerSub, dnsPort);
                no = 1;
                foreach (string s in answers2)
                {
                    SetShowLog("GET2", string.Format("{0}:{1}", no, s));
                    no++;
                }
            }

            //フォーム画面設定表示
            SetTxtAppConfig();

            //var st = ProcessInputModel. GetPaxNoFromDenbunHandover("12345-2");

            //byte[] dd = TestData();
            //if (dd != null)
            //{
            //    Encoding s_euc = Encoding.GetEncoding(51932);

            //    MailData mail = new MailData(
            //        HPFData.ToMailAddress,
            //        HPFData.FromMailAddress, 
            //        "TEST RESULT",
            //        s_euc.GetString(dd));

            //    LogOut.FileOut(mail.GetMIMEstring(), NACCS_REPORTTYPE.RESULT );
            //}
            ////送信用セッションのスレッドが起動しているか確認する
            //List<InfoOutModel> lst = new List<InfoOutModel>();
            //InfoOutModel info = new InfoOutModel();
            //info.INFO_CD.SetData("XX0011X");
            //info.UNSENT_NUM.SetData("1");
            //lst.Add(info);
            //info = new InfoOutModel();
            //info.INFO_CD.SetData("YY0011Y");
            //info.UNSENT_NUM.SetData("1");
            //lst.Add(info);
            //info = new InfoOutModel();
            //info.INFO_CD.SetData("ZZ0011Z");
            //info.UNSENT_NUM.SetData("1");
            //lst.Add(info);

            //this.taskSYGget = new Task(SYGResultFunc,(object)lst);
            //this.taskSYGget.Start();
            //string ret;

            //string dt = "201912251051  ";
            //NacCollumnDateTime ndt = new NacCollumnDateTime(14, false);

            //ndt.SetData(dt);

            //MessageBox.Show( ndt.DataDatetime.ToString("yyyy MM dd HH dd ss fff"));

            //NacCollumnNPD nlen = new NacCollumnNPD(5, 4);

            //nlen.SetData("99");
            //var n = nlen.GetByteLength();
            //var ss = nlen.GetByteData();
            //var strRet = "A0034-TJG -0001";
            //var data = strRet.Split('-');
            //if (data.Count() > 2)
            //{
            //    DataRow[] findRow = HPFData.NaccsErrCd.Select("KBN_CODE = '" + data[0].Trim() + "'");
            //    if (findRow.Length > 0)
            //    {
            //        DataRow dr = findRow[0];
            //        ret = string.Format("共通異常：{0}[{1}]({2})", dr[3].ToString(), strRet, dr[5].ToString());
            //        if (ret.Length > 50) ret = ret.Substring(0, 100);
            //    }
            //    else
            //    {
            //        DataRow[] findRow2 = HPFData.NaccsErrCdSub.Select("KBN_CODE = '" + data[1].Trim() + "'");
            //        if (findRow2.Length > 0)
            //        {
            //            DataRow dr = findRow2[0];
            //            ret = string.Format("入力項目異常:{0}({1})", dr[3].ToString(), strRet);
            //        }
            //    }
            //}
            //SettingFileRead();
        }

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
            Invoke((MethodInvoker)(() => { _sessions.Add(new SessionViewModel(e.Session)); }));
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
#if DEBUG
                    SetShowLog("受信", resultstring);
#endif
                }
                else
                {
#if DEBUG
                    SetShowLog("異常", resultstring);
#endif
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

        /// <summary>
        /// 開始ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            this.btnStop.Enabled = true;
            this.i_AutoFlg = true;

            SetShowLog("開始","自動処理開始ボタンをクリックしました。");

            StartServer();
            //i_sendList = new BindingList<SendClass>();

            this.btnStart.Enabled = false;

#if DEBUG
            OTHERFunc();
#endif
            if( HPFData.IntervalSendTimer > 0 )
            {
                //送信用セッションのスレッドが起動しているか確認する
                if (this.taskSend.Status != TaskStatus.Running)
                {
                    this.taskSend = new Task(SendFunc);
                    this.taskSend.Start();
                }
                this.timerSend.Start();
            }
            if (HPFData.IntervalSYGTimer > 0)
            {
                if (this.taskSYGlist.Status != TaskStatus.Running)
                {
                    this.taskSYGlist = new Task(SYGFunc);
                    this.taskSYGlist.Start();
                }
                this.timerSYG.Start();
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerSend_Tick(object sender, EventArgs e)
        {
           if (this.i_AutoFlg)
            {
                //送信用セッションのスレッドが起動しているか確認する
                if (this.taskSend.Status != TaskStatus.Running)
                {
                    this.taskSend = new Task(SendFunc);

                    this.taskSend.Start();
                }
            }
        }
        private void timerSYG_Tick(object sender, EventArgs e)
        {
            if (this.i_AutoFlg)
            {
                //SYGが送信済みの場合削除する★

                //送信用セッションのスレッドが起動しているか確認する
                if (this.taskSYGlist.Status != TaskStatus.Running)
                {
                    this.taskSYGlist = new Task(SYGFunc);

                    this.taskSYGlist.Start();
                }
            }
        }
        /// <summary>
        /// 時刻表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerDate_Tick(object sender, EventArgs e)
        {
            this.toolStripTime.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
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
                    LogOut.FileOut(model.Message.Session.Log.ToString(), common.REPORT_TYPE, common.GetFileName());
#if DEBUG
                    LogOut.FileOut(model.Parts.Body, common.REPORT_TYPE, "DEBUG_" + common.GetFileName());
#endif

                    //LogOut.FileOut(model.Parts.Body, common.REPORT_TYPE); 
                    var hd = model.Parts.Header;

                    var md = model.Message.Session.Log.ToString();

                    switch (common.REPORT_TYPE)
                    {
                        case NACCS_REPORTTYPE.SAD4011:
                            ProcessSad401Model nac401 = new ProcessSad401Model();
                            if (nac401.SetByteData(btRecv))
                            {
                                //DENBUN_HANDOVERが同じものを探す
                                var rln = ProcessInputModel.SetRecvDataToDB(nac401);
                                if (rln)
                                {
                                    resultstring = string.Format("処理結果通知を登録しました({0}:{1})", nac401.GetHandover, nac401.YunyuShinkokuNo.Data);
                                    this.Invoke(ssd, "正常", resultstring);
                                    ret = true;
                                }
                                else
                                {
                                    resultstring = string.Format("登録異常　該当なし({0}:{1})", nac401.GetHandover, nac401.GetInfoNo);
                                    this.Invoke(ssd, "異常", resultstring);
                                }
                                ret = true;
                            }
                            else
                            {
                                //401ではない？
                                resultstring = string.Format("処理結果通知　解析異常({0}:{1})", nac401.GetHandover, nac401.GetInfoNo);
                                this.Invoke(ssd, "異常", resultstring);
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
                            ProcessSad403Model nac403 = new ProcessSad403Model();
                            if (nac403.SetByteData(btRecv))
                            {
                                //輸入申告番号を確認する？
                                var strYNo = nac403.YunyuShinkokuNo.Data;

                                ret = true;
                            }
                            break;
                        //case NACCS_REPORTTYPE.SAD4131:
                        //    break;
                        case NACCS_REPORTTYPE.SAF0010:
                            nac = new ProcessSaf001Model();
                            if (nac.SetByteData(btRecv))
                            {
                                ret = true;
                            }
                            break;
                        case NACCS_REPORTTYPE.SAF0021:
                            nac = new ProcessSaf002Model();
                            if (nac.SetByteData(btRecv))
                            {
                                ret = true;
                            }
                            break;
                        case NACCS_REPORTTYPE.SAF0221:
                            nac = new ProcessSaf022Model();
                            if (nac.SetByteData(btRecv))
                            {
                                ret = true;
                            }
                            break;
                        case NACCS_REPORTTYPE.SYG_RESULT:
                            nac = new ProcessSYGRecvModel();
                            if (nac.SetByteData(btRecv))
                            {
                                ret = true;
                            }
                            break;
                        case NACCS_REPORTTYPE.SYG_LIST:
                            ProcessSYGRecvModel syglist = new ProcessSYGRecvModel();
                            if (syglist.SetByteData(btRecv))
                            {
                                if (syglist.InfoNum.GetIntData > 0)
                                {
                                    this.taskSYGget = new Task(SYGResultFunc, (object)syglist.InfoList);
                                    this.taskSYGget.Start();
                                }
                                ret = true;
                            }
                            break;
 
                        case NACCS_REPORTTYPE.OTA:
                        case NACCS_REPORTTYPE.NONE:
                        case NACCS_REPORTTYPE.RESULT:
                            ProcessResultModel nacret = new ProcessResultModel();
                            if (nacret.SetByteData(btRecv))
                            {
                                var rln = ProcessInputModel.SetRecvDataToDB(nacret.NaccsCommon.DENBUN_HANDOVER.Data, nacret.OUTPUT_RESULT, lngSeq);
                                if (rln)
                                {
                                    resultstring = string.Format("処理結果通知を登録しました({0}:{1})", nacret.NaccsCommon.DENBUN_HANDOVER.Data, nacret.ResultList[0].Data);
                                    this.Invoke(ssd, "正常", resultstring);
                                    ret = true;
                                }
                                else
                                {
                                    resultstring = string.Format("登録異常　該当なし({0}:{1})", nacret.GetHandover, nacret.ResultList[0].Data);
                                    this.Invoke(ssd, "異常", resultstring);
                                }
                            }
                            else
                            {
                                resultstring = string.Format("処理結果通知　解析異常({0}:{1})", nacret.GetHandover, nacret.ResultList[0].Data);
                                this.Invoke(ssd, "異常", resultstring);

                            }

                            //ProcessResultModel nacret = new ProcessResultModel();
                            //if (nacret.SetByteData(btRecv))
                            //{
                            //    bool findflg = false;
                            //    ProcessInputModel input;
                            //    //送信リストから同じHANDOVERを検索する
                            //    foreach (var mail in i_sendList)
                            //    {
                            //        if (mail.IsSend(nacret.GetHandover, out input, nacret.GetInfoNo))
                            //        {
                            //            //あれば送信モデルのlngSeqをUpdateDataを実行する
                            //            if (input != null)
                            //            {
                            //                findflg = true;
                            //                var rln = input.SetRecvDataToDB(nacret.YunyuShinkokuNo.Data,
                            //                    nacret.NaccsCommon.INPUT_INFO_NO.Data,
                            //                    nacret.OUTPUT_RESULT, lngSeq);
                            //                if (rln)
                            //                {
                            //                    resultstring = "処理結果通知を登録しました";
                            //                    this.Invoke(ssd, "正常", "処理結果通知を登録しました(" + nacret.ResultList[0].Data + ")");
                            //                    ret = true;
                            //                }
                            //                else
                            //                {
                            //                    resultstring = string.Format("処理結果通知　登録異常（{0}:{1})", nacret.GetHandover, nacret.GetInfoNo);
                            //                    this.Invoke(ssd, "異常", string.Format("処理結果通知　登録異常（{0}:{1})", nacret.GetHandover, nacret.GetInfoNo));
                            //                }
                            //            }
                            //            else
                            //            {
                            //                //読み込めなかった場合は次へ
                            //                //findflg = false;
                            //            }
                            //        }
                            //    }
                            //    if (!findflg)
                            //    {
                            //        DataBaseSQL objDb = new DataBaseSQL();
                            //        objDb.DBLogIn();

                            //        input = new ProcessInputModel();
                            //        //なければデータベースを確認する
                            //        string sql = ProcessInputModel.GetSQL_SELECT_PAXNO(ProcessInputModel.GetPaxNoFromDenbunHandover(nacret.NaccsCommon.DENBUN_HANDOVER.Data));

                            //        DataTable dt = objDb.GetDataTable(sql);
                            //        if (dt.Rows.Count > 0)
                            //        {
                            //            //同じPAX番号のデータが受信完了されていなかったら
                            //            //lngseqを上書きする。
                            //            //ProcessInputModel.GetSQL_UPDATE_RECV_T_PAX_H
                            //            DateTime dtDep = Convert.ToDateTime(dt.Rows[0]["TIME_OF_DEPARTURE"]);
                            //            string strFId = Common.ConvertToString(dt.Rows[0]["FLIGHT_ID"]);
                            //            string strPax = Common.ConvertToString(dt.Rows[0]["PAX_NO"]);

                            //            var rln = input.SetRecvDataToDB(
                            //                dtDep, strFId, strPax,
                            //                nacret.YunyuShinkokuNo.Data, nacret.NaccsCommon.INPUT_INFO_NO.Data, nacret.OUTPUT_RESULT, lngSeq);
                            //            if (rln)
                            //            {
                            //                resultstring = "処理結果通知を登録しました";
                            //                this.Invoke(ssd, "正常", "処理結果通知を登録しました(" + nacret.ResultList[0].Data + ")");
                            //                ret = true;
                            //            }
                            //            else
                            //            {
                            //                resultstring = "処理結果通知の登録で異常が出ています";
                            //                this.Invoke(ssd, "異常", "処理結果通知の登録で異常が出ています");
                            //            }
                            //        }
                            //        else
                            //        {
                            //            resultstring = string.Format("処理結果通知　登録異常　該当なし（{0}:{1})", nacret.GetHandover, nacret.GetInfoNo);
                            //            this.Invoke(ssd, "異常", string.Format("処理結果通知　登録異常　該当なし（{0}:{1})", nacret.GetHandover, nacret.GetInfoNo));

                            //        }
                            //    }
                            //}
                            break;
                        default:
                            resultstring = string.Format("処理結果通知　内容該当なし（{0}:{1})", common.DENBUN_HANDOVER.Data, common.INPUT_INFO_NO.Data);
                            this.Invoke(ssd, "警告", string.Format("処理結果通知　内容該当なし（{0}:{1})", common.DENBUN_HANDOVER.Data, common.INPUT_INFO_NO.Data));
                            break;
                    }
                }
                return ret;
            }
            catch (Exception e)
            {
                LogOut.ErrorOut(e.Message, MethodBase.GetCurrentMethod().Name);
                this.Invoke(ssd, "異常", e.Message);
                //throw e;
                return false;
            }
        }
        /// <summary>
        /// 未送信データの有無確認
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

                dsRecord = objDb.GetDataSet(ProcessInputModel.GetSQL_UNSENTCOUNT(), "T_PAX_D", false);

                if (dsRecord.Tables[0].Rows.Count > 0)
                {
                    return dsRecord.Tables[0].Rows.Count;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //if (objDb != null)
                //{
                //    objDb.RollBack();
                //}
                LogOut.ErrorOut(ex.Message, MethodBase.GetCurrentMethod().Name);
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
        /// 送信確認タスク
        /// </summary>
        public void SendFunc()
        {
            DataBaseSQL objDb = null;
            DataSet dsRecord = null;
            objDb = new DataBaseSQL();
            Boolean sflg = false;
            SetShowLogDelegate ssd = new SetShowLogDelegate(SetShowLog);

            try
            {
#if DEBUG
                this.Invoke(ssd, "確認", "送信確認タスク開始");
#endif
                int n = ConfirmSnedData();
                if (n > 0)
                {
                    objDb.DBLogIn();

                    dsRecord = objDb.GetDataSet(ProcessInputModel.GetSQL_SELECT(), "T_PAX", false);

                    if (dsRecord != null)
                    {
                        this.Invoke(ssd, "確認", "未送信 " + n.ToString() + "件");
                        LogOut.InfoOut("EDI送信処理　開始:" + n.ToString() + "件", this.Name, MethodBase.GetCurrentMethod().Name);

                        List<DataTable> dtList = new List<DataTable>();

                        //PAX？毎に振り分け
                        ProcessInputModel.AnalyzedDataSet(dsRecord, out dtList);

                        ProcessInputModel input;
                        InputCommonModel common = new InputCommonModel();

                        SendClass smail = new SendClass(
                            HPFData.ServerAddress,
                            HPFData.PortNo, 
                            HPFData.ServerDomain, 
                            HPFData.FromMailAddress, 
                            HPFData.ToMailAddress,
                            HPFData.MyServerName
                            );

                        foreach (var dtTbl in dtList)
                        {
                            input = new ProcessInputModel();
                            input.SetData(dtTbl);
                            smail.MessageList.Add(input);
                            
                            i_sendList.Add(smail);

                            smail.SmtpSend();

                            objDb.BeginTransaction();
                            sflg = true;

                            objDb.ExecuteNonQuery(input.GetSQL_UPDATE_SEND_H(), null);
                            objDb.ExecuteNonQuery(input.GetSQL_UPDATE_SEND_D(), null);

                            objDb.Commit();

                            this.Invoke(ssd, "送信", String.Format("PAX NO:{0}", input.PAX_NO));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (sflg)
                {
                    objDb.RollBack();
                }
                LogOut.ErrorOut(ex.Message, MethodBase.GetCurrentMethod().Name);
                this.Invoke(ssd, "異常", ex.Message);
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
        /// 送信確認タスク static
        /// </summary>
        public void SYGFunc()
        {
            SetShowLogDelegate ssd = new SetShowLogDelegate(SetShowLog);
            try
            {
                LogOut.InfoOut("SYG送信処理開始", this.Name, MethodBase.GetCurrentMethod().Name);

                ProcessSYGSendModel syg = new ProcessSYGSendModel(ProcessSYGSendModel.SendType.GET_LIST);

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
                
#if DEBUG
                this.Invoke(ssd, "確認", "SYG通信確認");
#endif

            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, MethodBase.GetCurrentMethod().Name);
                this.Invoke(ssd, "異常", ex.Message);
            }
        }

        /// <summary>
        /// 送信確認タスク static
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
                        !string.IsNullOrEmpty(strSysType) )
                    {
                        ProcessTCCSendModel syg = new ProcessTCCSendModel(strSendMes1: "TEST VALUE1", strSendMes2: "INFO VALUE2", strSendMes3: strSendMes);
                        //ProcessSYGSendModel syg = new ProcessSYGSendModel(strInfoCD: strInfoCd, strSysType: strSysType, strSendMes: strSendMes);

                        SendClass smail = new SendClass(
                            HPFData.ServerAddress,
                            HPFData.PortNo,
                            HPFData.ServerDomain,
                            HPFData.FromMailAddress,
                            HPFData.ToMailAddress,
                            HPFData.MyServerName
                            );

                        //smail.MessageSYG = syg;

                        i_sendList.Add(smail);

                        smail.SmtpSend(true, syg);

                        this.Invoke(ssd, "送信", "OTHER通信確認");
                    }
                }
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, MethodBase.GetCurrentMethod().Name);
                this.Invoke(ssd, "異常", ex.Message);
            }
        }

        /// <summary>
        /// 送信要求タスク static
        /// </summary>
        public void SYGResultFunc(object lstInfo)
        {
            SetShowLogDelegate ssd = new SetShowLogDelegate(SetShowLog);
            try
            {
                LogOut.InfoOut("SYG再送信要求処理開始", this.Name, MethodBase.GetCurrentMethod().Name);

                foreach (InfoOutModel info in (List<InfoOutModel>)lstInfo)
                {
                    ProcessSYGSendModel syg = new ProcessSYGSendModel(ProcessSYGSendModel.SendType.GET_RESULT, info.INFO_CD.Data);

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
                }
                this.Invoke(ssd, "送信", "SYG再送信要求");
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, MethodBase.GetCurrentMethod().Name);
                this.Invoke(ssd, "異常", ex.Message);
            }
        }

        public static byte[] TestData()
        {
            //SYG CONFIRM
            StringBuilder strData = new StringBuilder();

            strData.Append("   ");
            strData.Append("OTA  ");
            strData.Append("*SOTA  ");
            strData.Append("20190922041545");
            strData.Append("CON01");
            for (int i = 0; i < 17; i++)
            {
                strData.Append(" ");
            }
            strData.Append("12345@6789.CON");
            for (int i = 0; i < 50; i++)
            {
                strData.Append(" ");
            }
            strData.Append("SUBJECT:999999");
            for (int i = 0; i < 50; i++)
            {
                strData.Append(" ");
            }
            for (int i = 0; i < 40; i++)
            {
                strData.Append(" ");
            }
            strData.Append("ABCDEABCDEABCDEABCDE123456");//26
            strData.Append("001");//3
            strData.Append("E");//1
            strData.Append("R");//1
            strData.Append("   ");//3
            strData.Append("PAX1234567");//10
            strData.Append("SAKUJOINFO");//10
            for (int i = 0; i < 90; i++)
            {
                strData.Append(" ");
            }
            strData.Append("Z");//1
            for (int i = 0; i < 28; i++)
            {
                strData.Append(" ");
            }
            int n = 400 + 75+ 2+ 11+ 2;
            strData.AppendFormat("{0:000000}", n).AppendLine();//6

            strData.Append("00000-0000-0000");//75
            strData.Append("11111-0000-0000");//75
            strData.Append("22222-0000-0000");//75
            strData.Append("33333-0000-0000");//75
            strData.AppendLine("44444-0000-0000");//75
            strData.AppendLine("OTA-ABC-DEF");//11


            //strData.Append("001ER   11111     ");
            //for (int i = 0; i < 100; i++)
            //{
            //    strData.Append(" ");
            //}
            //strData.Append("Z");
            //for (int i = 0; i < 28; i++)
            //{
            //    strData.Append(" ");
            //}
            //strData.Append("000544");
            //strData.AppendLine();
            //strData.Append("12344-5678-9012");
            //for (int i = 0; i < 60; i++)
            //{
            //    strData.Append(" ");
            //}
            //strData.AppendLine();
            //strData.AppendLine("0011");
            //for (int i = 0; i < 3; i++)
            //{
            //    strData.AppendLine("ABC000"+i.ToString());
            //    strData.AppendLine("0001");
            //}
            //strData.AppendLine();
            //System.Text.Encoding enc = System.Text.Encoding.GetEncoding(51932);

            //ProcessSYGRecvModel recv = new ProcessSYGRecvModel();
            //recv.SetByteData( enc.GetBytes(strData.ToString()));

            //ProcessInputModel send = new ProcessInputModel();

            //InputCommonModel input = new InputCommonModel();
            //input.GyomuCode .SetData ("11111111111");
            //input.UserCode.SetData("222222222");
            //input.ShikibetsuNo.SetData("3333333333333");
            //input.UserPass.SetData("444444444444444");
            //input.DenbunHandover.SetData("555555555555");
            //input.InputInfoNo.SetData("66666666666666");
            //input.SakuinHandover.SetData("7777777777777");
            //input.SystemType.SetData("88888888888888");
            ////input.DenbunLen.SetData("99999999999999");

            //send.InputCommon = input;
            //send.YUNYU_SHINKOKU_NO.SetData("No55555");
            //send.SHORI_SHIKIBETSU.SetData("");
            //send.ATESAKI_KANSHO_CD.SetData("");
            //send.ATESAKI_BUMON_CD.SetData("");
            //send.PASSENGER.SetData("");
            //send.ZIPCODE.SetData("");
            //send.ADDRESS1.SetData("");
            //send.ADDRESS2.SetData("");
            //send.ADDRESS3.SetData("");
            //send.ADDRESS4.SetData("");
            //send.AIRLINE_CD_ICAO.SetData("");
            //send.FLIGHT_NO.SetData("");
            //send.TUKANYOTEI_CHOZO_OKIBA_CD.SetData("");
            //send.NOFU_HOHO_SHIKIBETSU.SetData("");
            //send.KOZA_NO.SetData("");
            //send.BP_SHINSEI_JIYU.SetData("");
            //send.TANPO_TOROKU_NO.SetData("");
            //send.KANZEI_MENJOZUMI_GAKU.SetData("");
            //send.KIJI_KANZEI .SetData("");
            //send.KIJI_HANBAITEN .SetData("");
            //send.KIJI_SONOTA .SetData("");
            //send.SHANAI_SEIRI_NO.SetData("");       


            //ShohinModel shohin = new ShohinModel();
            //shohin.KANZEI_TAISHO.SetData("Y");
            //shohin.SHOHIN_KANRI_CD .SetData("9991");
            //shohin.HINMEI.SetData("NO123456789TEST");
            //shohin.HANBAI_TANKA .SetData("100");
            //shohin.HANBAI_SURYO.SetData("1");
            //shohin.HINMOKU_CD.SetData("88");
            //shohin.NACCS_CD.SetData("NACCS");
            //shohin.GENZANCHI_CD.SetData("00");
            //shohin.GENZANCHI_SHOMEI_SHIKIBETSU.SetData("11");
            //shohin.INN_TAISHO.SetData("Y");
            //shohin.SURYO1.SetData("1");
            //shohin.SURYO_TANI_CD1 .SetData("T1");
            //shohin.SURYO2_1.SetData("1");
            //shohin.SURYO_TANI_CD2.SetData("1");
            //shohin.KAZEI_KAKAKU .SetData("10");
            //shohin.NAIKOKU_SHOHIZEITOU_SHUBETSU_CD.SetData("TX1");
            //send.Shohin.Add(shohin);

            //shohin = new ShohinModel();
            //shohin.KANZEI_TAISHO.SetData("");
            //shohin.SHOHIN_KANRI_CD.SetData("9921");
            //shohin.HINMEI.SetData("N2123456789TEST");
            //shohin.HANBAI_TANKA.SetData("200");
            //shohin.HANBAI_SURYO.SetData("2");
            //shohin.HINMOKU_CD.SetData("77");
            //shohin.NACCS_CD.SetData("NACCS");
            //shohin.GENZANCHI_CD.SetData("00");
            //shohin.GENZANCHI_SHOMEI_SHIKIBETSU.SetData("22");
            //shohin.INN_TAISHO.SetData("");
            //shohin.SURYO1.SetData("2");
            //shohin.SURYO_TANI_CD1.SetData("T2");
            //shohin.SURYO2_1.SetData("2");
            //shohin.SURYO_TANI_CD2.SetData("2");
            //shohin.KAZEI_KAKAKU.SetData("20");
            //shohin.NAIKOKU_SHOHIZEITOU_SHUBETSU_CD.SetData("TX2");
            //send.Shohin.Add(shohin);

            //shohin = new ShohinModel();
            //shohin.KANZEI_TAISHO.SetData("");
            //shohin.SHOHIN_KANRI_CD.SetData("9931");
            //shohin.HINMEI.SetData("N3123456789TEST");
            //shohin.HANBAI_TANKA.SetData("300");
            //shohin.HANBAI_SURYO.SetData("3");
            //shohin.HINMOKU_CD.SetData("66");
            //shohin.NACCS_CD.SetData("NACCS");
            //shohin.GENZANCHI_CD.SetData("00");
            //shohin.GENZANCHI_SHOMEI_SHIKIBETSU.SetData("33");
            //shohin.INN_TAISHO.SetData("");
            //shohin.SURYO1.SetData("3");
            //shohin.SURYO_TANI_CD1.SetData("T3");
            //shohin.SURYO2_1.SetData("3");
            //shohin.SURYO_TANI_CD2.SetData("3");
            //shohin.KAZEI_KAKAKU.SetData("30");
            //shohin.NAIKOKU_SHOHIZEITOU_SHUBETSU_CD.SetData("TX3");
            //send.Shohin.Add(shohin);

            //byte[] dd = send.GetByteData();
            //string str = "";

            //if (dd != null)
            //{
            //    Encoding s_euc = Encoding.GetEncoding(51932);

            //    MailData mail = new MailData(
            //        "192.168.10.206",
            //        "127.0.0.1",
            //        "OAT",
            //        s_euc.GetString(dd));

            //    str = mail.GetMIMEstring();

            //    LogOut.FileOut(str, LogOut.NACCS_TYPE.OTA);
            //}

            //NacCollumnDate ndt = new NacCollumnDate(8, false);
            //ndt.SetData(DateTime.Now.ToString("yyyyMMddhhmm  "));
            //var str = ndt.Data;

            //NacCollumnDateTime ndtm = new NacCollumnDateTime(14, false);
            //ndtm.SetData(DateTime.Now.ToString("yyyyMMddhhmmss"));
            //str = ndtm.Data;

            ////SYG CONFIRM
            //StringBuilder strData = new StringBuilder();
            //strData.Append("   ");
            //strData.Append("SYG  ");
            //strData.Append("SYG0001");
            //strData.Append("20190917041545");
            //strData.Append("CON01");
            //strData.Append("                 ");
            //strData.Append("conside@naccs.com       ");//24
            //for (int i = 0; i < (40+64+40+26); i++)
            //{
            //    strData.Append(" ");
            //}
            //strData.Append("001ER   11111     ");
            //for (int i = 0; i < 100; i++)
            //{
            //    strData.Append(" ");
            //}
            //strData.Append("Z");
            //for (int i = 0; i < 28; i++)
            //{
            //    strData.Append(" ");
            //}
            //strData.Append("000544");
            //strData.AppendLine();
            //strData.Append("12344-5678-9012");
            //for (int i = 0; i < 60; i++)
            //{
            //    strData.Append(" ");
            //}
            //strData.AppendLine();
            //strData.AppendLine("0011");
            //for (int i = 0; i < 3; i++)
            //{
            //    strData.AppendLine("ABC000"+i.ToString());
            //    strData.AppendLine("0001");
            //}
            //strData.AppendLine();
            //System.Text.Encoding enc = System.Text.Encoding.GetEncoding(51932);

            //ProcessSYGRecvModel recv = new ProcessSYGRecvModel();
            //recv.SetByteData( enc.GetBytes(strData.ToString()));

            //ProcessInputModel send = new ProcessInputModel();

            //InputCommonModel input = new InputCommonModel();
            //input.GyomuCode .SetData ("11111111111");
            //input.UserCode.SetData("222222222");
            //input.ShikibetsuNo.SetData("3333333333333");
            //input.UserPass.SetData("444444444444444");
            //input.DenbunHandover.SetData("555555555555");
            //input.InputInfoNo.SetData("66666666666666");
            //input.SakuinHandover.SetData("7777777777777");
            //input.SystemType.SetData("88888888888888");
            ////input.DenbunLen.SetData("99999999999999");

            //send.InputCommon = input;
            //send.YUNYU_SHINKOKU_NO.SetData("No55555");
            //send.SHORI_SHIKIBETSU.SetData("");
            //send.ATESAKI_KANSHO_CD.SetData("");
            //send.ATESAKI_BUMON_CD.SetData("");
            //send.PASSENGER.SetData("");
            //send.ZIPCODE.SetData("");
            //send.ADDRESS1.SetData("");
            //send.ADDRESS2.SetData("");
            //send.ADDRESS3.SetData("");
            //send.ADDRESS4.SetData("");
            //send.AIRLINE_CD_ICAO.SetData("");
            //send.FLIGHT_NO.SetData("");
            //send.TUKANYOTEI_CHOZO_OKIBA_CD.SetData("");
            //send.NOFU_HOHO_SHIKIBETSU.SetData("");
            //send.KOZA_NO.SetData("");
            //send.BP_SHINSEI_JIYU.SetData("");
            //send.TANPO_TOROKU_NO.SetData("");
            //send.KANZEI_MENJOZUMI_GAKU.SetData("");
            //send.KIJI_KANZEI .SetData("");
            //send.KIJI_HANBAITEN .SetData("");
            //send.KIJI_SONOTA .SetData("");
            //send.SHANAI_SEIRI_NO.SetData("");       


            //ShohinModel shohin = new ShohinModel();
            //shohin.KANZEI_TAISHO.SetData("Y");
            //shohin.SHOHIN_KANRI_CD .SetData("9991");
            //shohin.HINMEI.SetData("NO123456789TEST");
            //shohin.HANBAI_TANKA .SetData("100");
            //shohin.HANBAI_SURYO.SetData("1");
            //shohin.HINMOKU_CD.SetData("88");
            //shohin.NACCS_CD.SetData("NACCS");
            //shohin.GENZANCHI_CD.SetData("00");
            //shohin.GENZANCHI_SHOMEI_SHIKIBETSU.SetData("11");
            //shohin.INN_TAISHO.SetData("Y");
            //shohin.SURYO1.SetData("1");
            //shohin.SURYO_TANI_CD1 .SetData("T1");
            //shohin.SURYO2_1.SetData("1");
            //shohin.SURYO_TANI_CD2.SetData("1");
            //shohin.KAZEI_KAKAKU .SetData("10");
            //shohin.NAIKOKU_SHOHIZEITOU_SHUBETSU_CD.SetData("TX1");
            //send.Shohin.Add(shohin);

            //shohin = new ShohinModel();
            //shohin.KANZEI_TAISHO.SetData("");
            //shohin.SHOHIN_KANRI_CD.SetData("9921");
            //shohin.HINMEI.SetData("N2123456789TEST");
            //shohin.HANBAI_TANKA.SetData("200");
            //shohin.HANBAI_SURYO.SetData("2");
            //shohin.HINMOKU_CD.SetData("77");
            //shohin.NACCS_CD.SetData("NACCS");
            //shohin.GENZANCHI_CD.SetData("00");
            //shohin.GENZANCHI_SHOMEI_SHIKIBETSU.SetData("22");
            //shohin.INN_TAISHO.SetData("");
            //shohin.SURYO1.SetData("2");
            //shohin.SURYO_TANI_CD1.SetData("T2");
            //shohin.SURYO2_1.SetData("2");
            //shohin.SURYO_TANI_CD2.SetData("2");
            //shohin.KAZEI_KAKAKU.SetData("20");
            //shohin.NAIKOKU_SHOHIZEITOU_SHUBETSU_CD.SetData("TX2");
            //send.Shohin.Add(shohin);

            //shohin = new ShohinModel();
            //shohin.KANZEI_TAISHO.SetData("");
            //shohin.SHOHIN_KANRI_CD.SetData("9931");
            //shohin.HINMEI.SetData("N3123456789TEST");
            //shohin.HANBAI_TANKA.SetData("300");
            //shohin.HANBAI_SURYO.SetData("3");
            //shohin.HINMOKU_CD.SetData("66");
            //shohin.NACCS_CD.SetData("NACCS");
            //shohin.GENZANCHI_CD.SetData("00");
            //shohin.GENZANCHI_SHOMEI_SHIKIBETSU.SetData("33");
            //shohin.INN_TAISHO.SetData("");
            //shohin.SURYO1.SetData("3");
            //shohin.SURYO_TANI_CD1.SetData("T3");
            //shohin.SURYO2_1.SetData("3");
            //shohin.SURYO_TANI_CD2.SetData("3");
            //shohin.KAZEI_KAKAKU.SetData("30");
            //shohin.NAIKOKU_SHOHIZEITOU_SHUBETSU_CD.SetData("TX3");
            //send.Shohin.Add(shohin);

            //byte[] dd = send.GetByteData();
            //string str = "";

            //if (dd != null)
            //{
            //    Encoding s_euc = Encoding.GetEncoding(51932);

            //    MailData mail = new MailData(
            //        "192.168.10.206",
            //        "127.0.0.1",
            //        "OAT",
            //        s_euc.GetString(dd));

            //    str = mail.GetMIMEstring();

            //    LogOut.FileOut(str, LogOut.NACCS_TYPE.OTA);
            //}
           
            Encoding s_euc = Encoding.GetEncoding(51932);
            return s_euc.GetBytes(strData.ToString());
        }


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

    }
}
