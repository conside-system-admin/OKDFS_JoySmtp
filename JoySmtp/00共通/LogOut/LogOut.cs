using System;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms; 
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using JoySmtp.DataBase;
using JoySmtp.Data;
using System.Data.SqlClient;
using JoySmtp.JoyCommon;
using JoySmtp.Nac;

namespace JoySmtp.CLogOut
{
    public class LogOut
    {
        private const string LOGFILE_SUB_PATH_DEF = @"LOG\";                       // ログファイルのサブパス
        // Private Const LOGFILE_PRE_DEF As String = "HPF"                            ' ログファイルの前
        private const string LOGFILE_PRE_DEF = "JoySystem";                            // ログファイルの前
        private const string LOGFILE_FILE_EXT_DEF = "log";                       // ログファイルの拡張名
        private const string LOGFILE_APP_KEY_SUBPATH = "SubPath";                 // コンフィグファイルのサブパス
        private const string LOGFILE_APP_KEY_LOGLEVEL = "LogLevel";               // コンフィグファイルのパス
        private const string LOGFILE_APP_KEY_PRE = "LogPreName";                  // コンフィグファイルの前
        private const string LOGFILE_APP_KEY_EXT = "LogExtName";                  // コンフィグファイルの拡張名

        private const string LOG_ERR_MAIL_ADDR = "logErrorMailAddress";       //エラーメールアドレス

        private const string LOGFILE_APP_KEY_PRE_CON = "LogPreNameConnect";       // コンフィグファイルの前
        private const string LOGFILE_APP_KEY_USER_CODE = "NACCSUserCode";         // NACCSで使用する使用者コード
        private const string LOGFILE_USER_CODE_DEF = "System";                    // NACCSで使用する使用者コードのデフォルト

        private const string MAILFILE_DIR_PATH = "MailSavePath";
        private const string MAILFILE_APP_KEY_PRE = "MailPreName";                  // コンフィグファイルの前
        private const string MAILFILE_APP_KEY_EXT = "MailExtName";                  // コンフィグファイルの拡張名
        private const string MAILFILE_PRE_DEF = "MAIL";                       // 拡張名
        private const string MAILFILE_FILE_EXT_DEF = "txt";                       // 拡張名

        private const string MAILFILE_DIR_PATH_SEND = "MailSavePathSend"; // 送信データの保存場所
        private const string MAILFILE_DIR_PATH_RECV = "MailSavePathRecv"; // 受信データの保存場所
        
        protected static string i_LogSubPath = string.Empty;                        // ログファイルのサブパス
        protected static string i_LogFilePre = string.Empty;                        // ログファイルの前
        protected static string i_LogFileExt = string.Empty;                        // ログファイルの拡張名
        protected static string i_LogLevel = string.Empty;

        protected static string i_LogOutPath = string.Empty;
        protected static string i_LogFileName = string.Empty;

        protected static string i_LogErrMailAddr = string.Empty;

        protected static string i_LogFilePreCon = string.Empty;
        // Protected Shared i_LogOutPathCon As String = String.Empty
        protected static string i_LogFileNameCon = string.Empty;
        protected static string i_LogFileUserCode = string.Empty;

        protected static string i_MailSavePath = string.Empty;
        protected static string i_MailOutPath = string.Empty;
        protected static string i_MailFileName = string.Empty;
        protected static string i_MailFileExt = string.Empty;

        protected static string i_MailSavePathSend = string.Empty;
        protected static string i_MailSavePathRecv = string.Empty;

        private static DateTime BeforeSendDate = DateTime.MinValue;
        private static string BeforeMailString = "";

        private static Common i_Common = new Common();

        public const Int32 LOG_STRING_MAX = 100;

        //public enum NACCS_TYPE
        //{
        //    OTA,
        //    RESULT,
        //    SAD401,
        //    SAD403,
        //    SAF001,
        //    SAF002,
        //    SAF022,
        //    SEND,
        //    RECV,
        //    OTHER
        //}

        /// <summary>
        /// クラスの初期化を行います
        /// </summary>
        /// <remarks></remarks>
        public static void InitLogOut()
        {
            try
            {
                // 初期の時、ログ・ファイルの初期化を行います
                i_LogSubPath = GetAppSetting(LOGFILE_APP_KEY_SUBPATH);
                i_LogLevel = GetAppSetting(LOGFILE_APP_KEY_LOGLEVEL);
                i_LogFilePre = GetAppSetting(LOGFILE_APP_KEY_PRE);
                i_LogFileExt = GetAppSetting(LOGFILE_APP_KEY_EXT);

                //ERRメール送信アドレス
                i_LogErrMailAddr = GetAppSetting(LOG_ERR_MAIL_ADDR);

                i_MailSavePath = GetAppSetting(MAILFILE_DIR_PATH).TrimEnd('\\');
                i_MailOutPath = GetAppSetting(MAILFILE_APP_KEY_PRE);
                i_MailFileExt = GetAppSetting(MAILFILE_APP_KEY_EXT);

                i_MailSavePathSend = GetAppSetting(MAILFILE_DIR_PATH_SEND);
                i_MailSavePathRecv = GetAppSetting(MAILFILE_DIR_PATH_RECV);

                if (string.Empty.Equals(i_MailSavePath))
                    i_MailSavePath = Application.StartupPath;

                if (string.Empty.Equals(i_LogSubPath))
                    i_LogSubPath = LOGFILE_SUB_PATH_DEF;

                if (string.Empty.Equals(i_LogLevel))
                    i_LogLevel = HPFData.LOG_LEVEL_ALL;

                if (string.Empty.Equals(i_LogFilePre))
                    i_LogFilePre = LOGFILE_PRE_DEF;

                if (string.Empty.Equals(i_LogFileExt))
                    i_LogFileExt = LOGFILE_FILE_EXT_DEF;

                // 通常ログファイル出力
                i_LogOutPath = string.Format(@"{0}\{1}", Application.StartupPath, LOGFILE_SUB_PATH_DEF);

                // ログのフォルダを作成します
                SetLogFileFolder();

                // ログファイルの初期化を行います
                SetLogFileName();

                // 送受履歴用ファイル
                i_LogFilePreCon = GetAppSetting(LOGFILE_APP_KEY_PRE_CON);
                if (string.Empty.Equals(i_LogFilePreCon))
                    i_LogFilePreCon = LOGFILE_PRE_DEF + "Socket";

                i_LogFileUserCode = GetAppSetting(LOGFILE_APP_KEY_USER_CODE);
                if (string.Empty.Equals(i_LogFileUserCode))
                    i_LogFileUserCode = LOGFILE_USER_CODE_DEF;

                //i_LogOutPath = string.Format(@"{0}\{1}", Application.StartupPath, LOGFILE_SUB_PATH_DEF);

                //// ログのフォルダを作成します
                //SetLogFileFolder();

                // ログファイルの初期化を行います
                SetLogFileNameByEmptyCon();

                //メールログの設定
                if (string.Empty.Equals(i_MailOutPath))
                    i_MailOutPath = MAILFILE_PRE_DEF;

                i_MailOutPath = i_MailSavePath + "\\" + i_MailOutPath;

                if (string.Empty.Equals(i_MailFileExt))
                    i_MailFileExt = MAILFILE_FILE_EXT_DEF;

                SetMailFileFolder("", MAIL_PATH.OTHER_PATH, false);

                if (string.Empty.Equals(i_MailSavePathRecv))
                {
                    i_MailSavePathRecv = i_MailOutPath;
                }
                else
                {
                    SetMailFileFolder("", MAIL_PATH.RECV_MAIL_PATH, false);
                }
                if (string.Empty.Equals(i_MailSavePathSend))
                {
                    i_MailSavePathSend = i_MailOutPath;
                }
                else
                {
                    SetMailFileFolder("", MAIL_PATH.SEND_MAIL_PATH, false);
                }

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ログファイル名の設定
        /// </summary>
        /// <param name="strUserID"></param>
        /// <remarks></remarks>
        public static void SetLogFileName(string strUserID = "")
        {
            try
            {
                if (string.Empty.Equals(strUserID))
                    SetLogFileNameByEmpty();
                else
                    SetLogFileNameByUserID(strUserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// エラーログを出力します
        /// </summary>
        /// <param name="strInfo">ログの内容</param>
        /// <param name="strMethondName">メソッド名前</param>
        /// <remarks></remarks>
        public static void ErrorOut(string strInfo, string strClassName = "", string strMethondName = "", Boolean blnSendMail = false, string strType = "")
        {
            try
            {
                if (string.Empty.Equals(i_LogFileName))
                    return;

                WriteToLogFile(GetLogInfo(HPFData.LOG_LEVEL_ERROR,
                    (strInfo.Length>LOG_STRING_MAX)?strInfo.Substring(0, LOG_STRING_MAX):strInfo
                    , strClassName, strMethondName));

                if (blnSendMail)
                {
                    SendErrorMail(string.Format("【{0}】 ERROR", Application.ProductName),
                        GetLogInfo(HPFData.LOG_LEVEL_ERROR,strInfo, strClassName, strMethondName),
                        strType
                        );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 情報ログを出力します
        /// </summary>
        /// <param name="strInfo">ログの内容</param>
        /// <param name="strMethondName">メソッド名前</param>
        /// <remarks></remarks>
        public static void InfoOut(string strInfo, string strClassName = "", string strMethondName = "")
        {
            try
            {
                if (string.Empty.Equals(i_LogFileName))
                    return;

                if (IsWriteLog(HPFData.LOG_LEVEL_INFO) == false)
                    return;

                WriteToLogFile(GetLogInfo(HPFData.LOG_LEVEL_INFO, strInfo, strClassName, strMethondName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// SQL文のログ出力
        /// </summary>
        /// <param name="strInfo"></param>
        /// <param name="strMethondName"></param>
        /// <remarks></remarks>
        public static void SQLOut(string strInfo, string strClassName = "", string strMethondName = "")
        {
            try
            {
                InfoOut("[SQL]:" + strInfo.Replace(ControlChars.NewLine, string.Empty), strClassName, strMethondName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通信ログを出力します
        /// </summary>
        /// <param name="strUserCode">利用者コード</param>
        /// <param name="strSystemNo">識別番号</param>
        /// <param name="strGyomuCode">業務コード</param>
        /// <param name="strInfo">送信内容</param>
        /// <remarks></remarks>
        public static void ConnectOut(string strSystemNo, string strGyomuCode, string strUserCode = "", string strInfo = "")
        {
            try
            {
                if (string.Empty.Equals(i_LogFileNameCon))
                    return;

                string strLogInfoOut = string.Empty;

                // 利用者コード
                if (string.Empty.Equals(strUserCode) == false)
                    strLogInfoOut = strUserCode;
                else
                    strLogInfoOut = i_LogFileUserCode;

                // 識別番号
                strLogInfoOut = strLogInfoOut + ControlChars.Tab + strSystemNo;

                // 業務コード
                strLogInfoOut = strLogInfoOut + ControlChars.Tab + strGyomuCode;

                // 日付
                strLogInfoOut = strLogInfoOut + ControlChars.Tab + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");

                // 送信内容
                if (string.Empty.Equals(strInfo) == false)
                    strLogInfoOut = strLogInfoOut + ControlChars.Tab + strInfo.Replace(Constants.vbCrLf, ";");

                WriteToLogFile(strLogInfoOut, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// ストアドプロシージャのログ出力
        /// </summary>
        /// <param name="strStoredProcedureName">ストアドプロシージャの名前</param>
        /// <param name="objSQLParams">ストアドプロシージャのパラメータ</param>
        /// <param name="strClassName"></param>
        /// <param name="strMethondName"></param>
        /// <remarks></remarks>
        public static void ProcedureSQLOut(string strStoredProcedureName, SqlParameter[] objSQLParams, string strClassName = "", string strMethondName = "")
        {
            try
            {
                StringBuilder strSQL = new StringBuilder();
                //SqlParameter objParam = null;
                bool blnIsFirst = true;

                strSQL.Append(" DECLARE	@return_value int ");
                strSQL.AppendFormat(" EXEC @return_value = {0} ", strStoredProcedureName);
                foreach (SqlParameter objParam in objSQLParams)
                {
                    if (Information.IsNothing(objParam.Value) == false)
                    {
                        if (blnIsFirst == false)
                            strSQL.Append(" ,");
                        blnIsFirst = false;
                        strSQL.AppendFormat(" {0} = N'{1}' ", objParam.ParameterName, Convert.ToString(objParam.Value).Replace("'", "''"));
                    }
                }

                InfoOut("[SQL]:" + strSQL.ToString().Replace(ControlChars.NewLine, string.Empty), strClassName, strMethondName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ログの内容を作成
        /// </summary>
        /// <param name="strLogLevel">ログのレベル</param>
        /// <param name="strLogInfo">ログの内容</param>
        /// <param name="strMethondName">メソッド名</param>
        /// <returns>ログの内容</returns>
        /// <remarks></remarks>
        private static string GetLogInfo(string strLogLevel, string strLogInfo, string strClassName = "", string strMethondName = "")
        {
            try
            {
                string strLogInfoOut = string.Empty;

                // 日付
                strLogInfoOut = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");

                // ログのラベル
                strLogInfoOut = strLogInfoOut + ControlChars.Tab + strLogLevel;

                // クラス名
                if (string.Empty.Equals(strClassName) == false)
                    strLogInfoOut = strLogInfoOut + ControlChars.Tab + strClassName;

                // メソッド名
                if (string.Empty.Equals(strMethondName) == false)
                    strLogInfoOut = strLogInfoOut + ControlChars.Tab + strMethondName;

                // ログの情報
                strLogInfoOut = strLogInfoOut + ControlChars.Tab + strLogInfo;

                //// T_LOGに記入
                //WriteToLogTable(strLogLevel, strClassName, strMethondName, strLogInfo);
                return strLogInfoOut;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///// <summary>
        ///// ログの内容をT_LOGに記録します
        ///// </summary>
        ///// <param name="strLogInfo">ログの情報</param>
        ///// <remarks></remarks>
        //private static void WriteToLogTable(string strLogLevel, string strClassName, string strMethondName, string strLogInfo)
        //{
        //    DataBaseSQL objDb = new DataBaseSQL();
        //    string strSQL = string.Empty;
        //    string strUserId = i_Common.GetLoginUserInfo(HPFData.LOGINUSERINFO_KEY_ID);
        //    string strTanmatu = i_Common.GetLoginUserInfo(HPFData.LOGINUSERINFO_KEY_LOGIN_TANMATU_ID);
        //    try
        //    {
        //        if (strLogInfo.Length > HPFData.LOGJYOHOMAX)
        //            strLogInfo = strLogInfo.Substring(0, HPFData.LOGJYOHOMAX);
        //        objDb.DBLogIn();
        //        strSQL = Get_Ins_T_Log(strLogLevel, strClassName, strMethondName, strLogInfo, strUserId, strTanmatu);
        //        // ログをT_LOGに書き込む。
        //        objDb.ExecuteNonQuery_T_Log(strSQL);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        objDb.DBLogOut();
        //        objDb = null/* TODO Change to default(_) if this is not a reference type */;
        //    }
        //}


        private static string Get_Ins_T_Log(string strLogLevel, string strClassName, string strMethondName, string strLogInfo, string strUserId, string strTanmatu)
        {
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            try
            {
                strSql.Append("    	INSERT INTO T_LOG   ").Append(Constants.vbCrLf);
                strSql.Append("    	([DateTime],	  ").Append(Constants.vbCrLf);
                strSql.Append("    	LogLevel,	  ").Append(Constants.vbCrLf);
                strSql.Append("    	ClassName,	  ").Append(Constants.vbCrLf);
                strSql.Append("    	MethodName,	  ").Append(Constants.vbCrLf);
                strSql.Append("    	LogJyoho,	  ").Append(Constants.vbCrLf);
                strSql.Append("    	UserId,	  ").Append(Constants.vbCrLf);
                strSql.Append("    	Tanmatu )  ").Append(Constants.vbCrLf);
                strSql.Append(" VALUES " + Constants.vbCrLf).Append(Constants.vbCrLf);
                strSql.Append("  (GETDATE() ").Append(Constants.vbCrLf);
                strSql.AppendFormat("  ,'{0}' ", strLogLevel).Append(Constants.vbCrLf);
                strSql.AppendFormat("  ,'{0}' ", strClassName).Append(Constants.vbCrLf);
                strSql.AppendFormat("  ,'{0}' ", strMethondName).Append(Constants.vbCrLf);
                strSql.AppendFormat("  ,'{0}' ", strLogInfo.Replace("'", "''")).Append(Constants.vbCrLf);
                strSql.AppendFormat("  ,'{0}' ", strUserId).Append(Constants.vbCrLf);
                strSql.AppendFormat("  ,'{0}' ) ", strTanmatu).Append(Constants.vbCrLf);

                return strSql.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ログの内容をログファイルに記録します
        /// </summary>
        /// <param name="strLogInfo">ログの情報</param>
        /// <remarks></remarks>
        private static void WriteToLogFile(string strLogInfo, bool blnOther = false)
        {
            FileStream objFile = null;

            try
            {
                strLogInfo = string.Format("{0}{1}", strLogInfo, ControlChars.NewLine);
                if (blnOther)
                    objFile = new FileStream(string.Format(@"{0}\{1}", i_LogOutPath, i_LogFileNameCon), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                else
                    objFile = new FileStream(string.Format(@"{0}\{1}", i_LogOutPath, i_LogFileName), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                objFile.Seek(0, SeekOrigin.End);
                objFile.Write(Encoding.Default.GetBytes(strLogInfo), 0, Encoding.Default.GetByteCount(strLogInfo));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Information.IsNothing(objFile) == false)
                {
                    objFile.Close();
                    objFile.Dispose();
                }
            }
        }

        /// <summary>
        /// ログ出力するか判断
        /// </summary>
        /// <param name="strLogLevel">ログのレベル</param>
        /// <returns>
        /// True :出力
        /// False:不出力
        /// </returns>
        /// <remarks></remarks>
        private static bool IsWriteLog(string strLogLevel)
        {
            try
            {
                if (HPFData.LOG_LEVEL_ALL.Equals(i_LogLevel) || HPFData.LOG_LEVEL_DEBUG.Equals(i_LogLevel))
                    return true;

                return i_LogLevel.Equals(strLogLevel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ユーザーIDでログファイルの作成
        /// </summary>
        /// <param name="strUserID">ユーザーID</param>
        /// <remarks></remarks>
        private static void SetLogFileNameByUserID(string strUserID)
        {
            try
            {
                i_LogFileName = string.Format("{0}_{1}_{2}.{3}", DateTime.Now.ToString("yyyyMMdd"), i_LogFilePre, strUserID, i_LogFileExt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 空のログファイルの作成
        /// </summary>
        /// <remarks></remarks>
        private static void SetLogFileNameByEmpty()
        {
            try
            {
                i_LogFileName = string.Format("{0}_{1}.{2}", DateTime.Now.ToString("yyyyMMdd"), i_LogFilePre, i_LogFileExt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 空のログファイルの作成(Connect)
        /// </summary>
        /// <remarks></remarks>
        private static void SetLogFileNameByEmptyCon()
        {
            try
            {
                i_LogFileNameCon = string.Format("{0}_{1}.{2}", DateTime.Now.ToString("yyyyMMdd"), i_LogFilePreCon, i_LogFileExt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// メールファイルのフォルダを作成
        /// </summary>
        /// <remarks></remarks>
        private static void SetMailFileFolder(string subDir, MAIL_PATH hozonType, Boolean flg = true)
        {
            DirectoryInfo objDirect = null;
            try
            {
                objDirect = new DirectoryInfo(GetFileHozonDir(hozonType));
                //objDirect = new DirectoryInfo(i_MailOutPath);


                if (objDirect.Exists == false)
                    objDirect.Create();

                if (flg)
                {
                    objDirect = new DirectoryInfo(string.Format("{0}\\{1}", GetFileHozonDir(hozonType), subDir));
                    if (objDirect.Exists == false)
                        objDirect.Create();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// ログファイルのフォルダを作成
        /// </summary>
        /// <remarks></remarks>
        private static void SetLogFileFolder()
        {
            DirectoryInfo objDirect = null;

            try
            {
                objDirect = new DirectoryInfo(i_LogOutPath);
                if (objDirect.Exists == false)
                    objDirect.Create();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Appconfigから設定値を取得します
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetAppSetting(string strKey)
        {
            string strValue = string.Empty;
            try
            {
                strValue = ConfigurationManager.AppSettings[strKey];
                if (Information.IsNothing(strValue))
                    strValue = string.Empty;
                return strValue;
            }
            catch (Exception ex)
            {
                ErrorOut(ex.ToString(), "GetAppSetting");
                throw ex;
            }
        }
        /// <summary>
        /// 保存場所
        /// </summary>
        public enum MAIL_PATH
        {
            /// <summary>
            /// 生データ
            /// </summary>
            OTHER_PATH,
            /// <summary>
            /// （naccsからの受信メール）
            /// C:\JoySuite\JoyFlight\JoyData\NACCS\Recv\Back 
            /// </summary>
            RECV_MAIL_PATH,
            /// <summary>
            /// （naccsへの送信メール（？）BC????.txtやNU????.txt等）
            /// C:\JoySuite\JoyFlight\JoyData\NACCS\Send\Back
            /// </summary>
            SEND_MAIL_PATH
        }
        private static string GetFileHozonDir(MAIL_PATH hozonType)
        {
            switch (hozonType)
            {
                case MAIL_PATH.RECV_MAIL_PATH: return i_MailSavePathRecv;
                case MAIL_PATH.SEND_MAIL_PATH: return i_MailSavePathSend;
                default:
                    return i_MailOutPath;
            }
        }
        /// <summary>
        /// 内容をそのまま一つのファイルとして保存
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sendflg"></param>
        /// <returns></returns>
        public static bool FileOut(string strData, NACCS_REPORTTYPE contype, string fname = "")
        {
            MAIL_PATH hozonType = MAIL_PATH.OTHER_PATH;
            try
            {
                string strLogInfoOut = string.Empty;
                string dir = DateTime.Now.ToString("yyyyMMdd");
                string str = "";

                //当日のフォルダを作成する
                SetMailFileFolder(dir, MAIL_PATH.OTHER_PATH);

                if (fname != "")
                {
                    ////同名ファイルチェック             
                    strLogInfoOut = GetDifferentName(fname, string.Format("{0}\\{1}", GetFileHozonDir(hozonType), dir), i_MailFileExt, out str);
                }
                if (strLogInfoOut == "")
                {
                    string strType = ((contype == NACCS_REPORTTYPE.OTA) || (contype == NACCS_REPORTTYPE.SYG_LIST) || (contype == NACCS_REPORTTYPE.OTHER)) ? "SEND" : "RECV";
                    strLogInfoOut = string.Format("{0}\\{1}\\{2}_{3}_{4}.{5}",
                        GetFileHozonDir(hozonType), dir, strType, contype, DateTime.Now.ToString("yyyyMMddhhmmssfff"), i_MailFileExt);
                }
                //if (fname == "")
                //{
                //    //ファイル名の取得
                //    string strType = ((contype == NACCS_REPORTTYPE.OTA) || (contype == NACCS_REPORTTYPE.SYG_LIST) || (contype == NACCS_REPORTTYPE.OTHER)) ? "SEND" : "RECV";
                //    strLogInfoOut = string.Format("{0}\\{1}\\{2}_{3}_{4}.{5}",
                //        GetFileHozonDir(hozonType), dir, strType, contype, DateTime.Now.ToString("yyyyMMddhhmmssfff"), i_MailFileExt);
                //}
                //else
                //{
                //    strLogInfoOut = string.Format("{0}\\{1}\\{2}.{3}", GetFileHozonDir(hozonType), dir, fname, i_MailFileExt);

                //    //同名のファイルがあるか？
                //    if (System.IO.File.Exists(strLogInfoOut))
                //    {
                //        strLogInfoOut = string.Format("{0}\\{1}\\{2}_{3}.{4}",
                //            GetFileHozonDir(hozonType), dir, fname, DateTime.Now.ToString("ss"), i_MailFileExt);
                //    }
                //}
                //ファイルに書込
                WriteToMailFile(strLogInfoOut, strData);

                return true;
            }
            catch (Exception ex)
            {
                ErrorOut(ex.ToString(), "FileOut");
                return false;
                //throw ex;
            }
        }

        /// <summary>
        /// 内容をそのまま一つのファイルとして保存
        /// </summary>
        /// <param name="strData"></param>
        /// <param name="fname"></param>
        /// <returns></returns>
        public static string FileOutOTA(string strData, MAIL_PATH hozonType, string fname = "")
        {
            string result = "";
            try
            {
                string strLogInfoOut = string.Empty;
                string dir;
                DateTime now = DateTime.Now;


                dir = now.ToString("yyyy");
                SetMailFileFolder(dir, hozonType);

                dir = string.Format("{0}\\{1}",dir, now.ToString("MM"));
                SetMailFileFolder(dir, hozonType);


                dir = string.Format("{0}\\{1}",dir, now.ToString("dd"));
                SetMailFileFolder(dir, hozonType);


                if (fname != "")
                {
                    ////同名ファイルチェック             
                    strLogInfoOut = GetDifferentName(fname, string.Format("{0}\\{1}", GetFileHozonDir(hozonType), dir), i_MailFileExt, out result);
                    if (strLogInfoOut == "")
                    {
                        //ファイル名の取得
                        result = "";
                    }
                }
                if(result=="")
                {
                    result = string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMddhhmmssfff"), i_MailFileExt);
                    //ファイル名の取得
                    strLogInfoOut = string.Format("{0}\\{1}\\{2}", GetFileHozonDir(hozonType), dir, result);
                }
                //ファイルに書込
                WriteToMailFile(strLogInfoOut, strData);

                return result;
            }
            catch (Exception ex)
            {
                //throw ex;
                ErrorOut(ex.ToString(), "FileOutOTA");
                return "";
            }
        }
        /// <summary>
        /// 同ファイル名があった場合に末尾に数字をつける関数
        /// </summary>
        /// <param name="strfname"></param>
        /// <param name="strpath"></param>
        /// <param name="strext"></param>
        /// <returns></returns>
        public static string GetDifferentName(string strfname, string strpath, string strext, out string strsetfname)
        {
            string newfile = "";
            bool ret = false;
            int num = 0;
            strsetfname = "";
            try
            {
                while (!ret)
                {
                    newfile = string.Format("{0}\\{1}{2}.{3}", strpath, strfname, (num == 0 ? "" : "_" + num.ToString()), strext);
                    if (!System.IO.File.Exists(newfile))
                    {
                        strsetfname = string.Format("{0}{1}.{2}", strfname, (num == 0 ? "" : "_" + num.ToString()), strext);
                        return newfile;
                    }
                    else
                    {
                        num++;
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                ErrorOut(ex.ToString(), "GetDifferentName");
                return "";
            }
        }
        /// <summary>
        /// ログの内容をログファイルに記録します
        /// </summary>
        /// <param name="strLogInfo">ログの情報</param>
        /// <remarks></remarks>
        private static void WriteToMailFile(string strFilePath, string strData)
        {
            FileStream objFile = null;
            // 50220   iso-2022-jp     日本語 (JIS)
            // 50221   csISO2022JP     日本語 (JIS 1 バイト カタカナ可)
            // 50222   iso-2022-jp     日本語 (JIS 1 バイト カタカナ可 - SO/SI)
            // 51932   euc-jp  日本語 (EUC)
            var s_euc = Encoding.Default;//.GetEncoding(51932);

            try
            {
                objFile = new FileStream(strFilePath, FileMode.CreateNew, FileAccess.Write);
                objFile.Write(s_euc.GetBytes (strData), 0, s_euc.GetByteCount(strData));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Information.IsNothing(objFile) == false)
                {
                    objFile.Close();
                    objFile.Dispose();
                }
            }
        }

        /// <summary>
        /// エラーメール送信
        /// </summary>
        /// <param name="strTitle"></param>
        /// <param name="strErr"></param>
        public static void SendErrorMail(string strTitle, string strErr, string strType)
        {
            if (i_LogErrMailAddr == string.Empty)
                return;

            try
            {
                if (BeforeMailString.Equals(strType))
                {
                    if(BeforeSendDate.AddMinutes(20) > DateTime.Now)
                    {
                        return;
                    }
                }
                BeforeMailString = strType;
                BeforeSendDate = DateTime.Now;

                i_Common.MailAddrTo = i_LogErrMailAddr;
                i_Common.MailSubject = strTitle;
                i_Common.MailBody = strErr;
                i_Common.SendMail(i_Common.MailSubject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
