using System;
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
using System.Configuration;
using JoySmtp.DataBase;
using JoySmtp.Data;
using System.Data.SqlClient;
using JoySmtp.JoyCommon;

namespace JoySmtp.CLogOut
{
    /// <summary>
    /// 年、月日のフォルダを作成する
    /// 年月日送信日時分‗IDファイルを作成する
    /// メールの内容を保存する
    /// </summary>
    public class LogOutMail
    {
        private const string LOGFILE_SUB_PATH_DEF = @"LOG\";                       // ログファイルのサブパス
        private const string LOGFILE_PRE_DEF = "JoySystem";                            // ログファイルの前
        private const string LOGFILE_FILE_EXT_DEF = "txt";                       // ログファイルの拡張名
        private const string LOGFILE_APP_KEY_SUBPATH = "SubPath";                 // コンフィグファイルのサブパス
        private const string LOGFILE_APP_KEY_PRE = "MailPreName";                  // コンフィグファイルの前
        private const string LOGFILE_APP_KEY_EXT = "MailExtName";                  // コンフィグファイルの拡張名

        protected static string i_LogSubPath = string.Empty;                        // ログファイルのサブパス
        protected static string i_LogFilePre = string.Empty;                        // ログファイルの前
        protected static string i_LogFileExt = string.Empty;                        // ログファイルの拡張名
        protected static string i_LogLevel = string.Empty;

        protected static string i_LogOutPath = string.Empty;
        protected static string i_LogFileName = string.Empty;

        //protected static string i_LogFilePreCon = string.Empty;
        //// Protected Shared i_LogOutPathCon As String = String.Empty
        //protected static string i_LogFileNameCon = string.Empty;
        //protected static string i_LogFileUserCode = string.Empty;

        private static Common i_Common = new Common();
        private Int32 i_No = 0;


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
                i_LogFilePre = GetAppSetting(LOGFILE_APP_KEY_PRE);
                i_LogFileExt = GetAppSetting(LOGFILE_APP_KEY_EXT);

                if (string.Empty.Equals(i_LogSubPath))
                    i_LogSubPath = LOGFILE_SUB_PATH_DEF;

                //if (string.Empty.Equals(i_LogLevel))
                //    i_LogLevel = HPFData.LOG_LEVEL_ALL;

                if (string.Empty.Equals(i_LogFilePre))
                    i_LogFilePre = LOGFILE_PRE_DEF;

                if (string.Empty.Equals(i_LogFileExt))
                    i_LogFileExt = LOGFILE_FILE_EXT_DEF;

                //// 通常ログファイル出力
                //i_LogOutPath = string.Format(@"{0}\{1}", Application.StartupPath, LOGFILE_SUB_PATH_DEF);

                // ログのフォルダを作成します
                SetLogFileFolder();

                //// ログファイルの初期化を行います
                //SetLogFileName();

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

        ///// <summary>
        ///// エラーログを出力します
        ///// </summary>
        ///// <param name="strInfo">ログの内容</param>
        ///// <param name="strMethondName">メソッド名前</param>
        ///// <remarks></remarks>
        //public static void ErrorOut(string strInfo, string strClassName = "", string strMethondName = "")
        //{
        //    try
        //    {
        //        if (string.Empty.Equals(i_LogFileName))
        //            return;

        //        WriteToLogFile(GetLogInfo(HPFData.LOG_LEVEL_ERROR, strInfo, strClassName, strMethondName));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// 情報ログを出力します
        ///// </summary>
        ///// <param name="strInfo">ログの内容</param>
        ///// <param name="strMethondName">メソッド名前</param>
        ///// <remarks></remarks>
        //public static void InfoOut(string strInfo, string strClassName = "", string strMethondName = "")
        //{
        //    try
        //    {
        //        if (string.Empty.Equals(i_LogFileName))
        //            return;

        //        if (IsWriteLog(HPFData.LOG_LEVEL_INFO) == false)
        //            return;

        //        WriteToLogFile(GetLogInfo(HPFData.LOG_LEVEL_INFO, strInfo, strClassName, strMethondName));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// SQL文のログ出力
        ///// </summary>
        ///// <param name="strInfo"></param>
        ///// <param name="strMethondName"></param>
        ///// <remarks></remarks>
        //public static void SQLOut(string strInfo, string strClassName = "", string strMethondName = "")
        //{
        //    try
        //    {
        //        InfoOut("[SQL]:" + strInfo.Replace(ControlChars.NewLine, string.Empty), strClassName, strMethondName);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// 通信ログを出力します
        ///// </summary>
        ///// <param name="strUserCode">利用者コード</param>
        ///// <param name="strSystemNo">識別番号</param>
        ///// <param name="strGyomuCode">業務コード</param>
        ///// <param name="strInfo">送信内容</param>
        ///// <remarks></remarks>
        //public static void ConnectOut(string strSystemNo, string strGyomuCode, string strUserCode = "", string strInfo = "")
        //{
        //    try
        //    {
        //        if (string.Empty.Equals(i_LogFileNameCon))
        //            return;

        //        string strLogInfoOut = string.Empty;

        //        // 利用者コード
        //        if (string.Empty.Equals(strUserCode) == false)
        //            strLogInfoOut = strUserCode;
        //        else
        //            strLogInfoOut = i_LogFileUserCode;

        //        // 識別番号
        //        strLogInfoOut = strLogInfoOut + ControlChars.Tab + strSystemNo;

        //        // 業務コード
        //        strLogInfoOut = strLogInfoOut + ControlChars.Tab + strGyomuCode;

        //        // 日付
        //        strLogInfoOut = strLogInfoOut + ControlChars.Tab + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");

        //        // 送信内容
        //        if (string.Empty.Equals(strInfo) == false)
        //            strLogInfoOut = strLogInfoOut + ControlChars.Tab + strInfo.Replace(Constants.vbCrLf, ";");

        //        WriteToLogFile(strLogInfoOut, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



        ///// <summary>
        ///// ストアドプロシージャのログ出力
        ///// </summary>
        ///// <param name="strStoredProcedureName">ストアドプロシージャの名前</param>
        ///// <param name="objSQLParams">ストアドプロシージャのパラメータ</param>
        ///// <param name="strClassName"></param>
        ///// <param name="strMethondName"></param>
        ///// <remarks></remarks>
        //public static void ProcedureSQLOut(string strStoredProcedureName, SqlParameter[] objSQLParams, string strClassName = "", string strMethondName = "")
        //{
        //    try
        //    {
        //        StringBuilder strSQL = new StringBuilder();
        //        //SqlParameter objParam = null;
        //        bool blnIsFirst = true;

        //        strSQL.Append(" DECLARE	@return_value int ");
        //        strSQL.AppendFormat(" EXEC @return_value = {0} ", strStoredProcedureName);
        //        foreach (SqlParameter objParam in objSQLParams)
        //        {
        //            if (Information.IsNothing(objParam.Value) == false)
        //            {
        //                if (blnIsFirst == false)
        //                    strSQL.Append(" ,");
        //                blnIsFirst = false;
        //                strSQL.AppendFormat(" {0} = N'{1}' ", objParam.ParameterName, Convert.ToString(objParam.Value).Replace("'", "''"));
        //            }
        //        }

        //        InfoOut("[SQL]:" + strSQL.ToString().Replace(ControlChars.NewLine, string.Empty), strClassName, strMethondName);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// ログの内容を作成
        /////         ''' </summary>
        /////         ''' <param name="strLogLevel">ログのレベル</param>
        /////         ''' <param name="strLogInfo">ログの内容</param>
        /////         ''' <param name="strMethondName">メソッド名</param>
        /////         ''' <returns>ログの内容</returns>
        /////         ''' <remarks></remarks>
        //private static string GetLogInfo(string strLogLevel, string strLogInfo, string strClassName = "", string strMethondName = "")
        //{
        //    try
        //    {
        //        string strLogInfoOut = string.Empty;

        //        // 日付
        //        strLogInfoOut = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");

        //        // ログのラベル
        //        strLogInfoOut = strLogInfoOut + ControlChars.Tab + strLogLevel;

        //        // クラス名
        //        if (string.Empty.Equals(strClassName) == false)
        //            strLogInfoOut = strLogInfoOut + ControlChars.Tab + strClassName;

        //        // メソッド名
        //        if (string.Empty.Equals(strMethondName) == false)
        //            strLogInfoOut = strLogInfoOut + ControlChars.Tab + strMethondName;

        //        // ログの情報
        //        strLogInfoOut = strLogInfoOut + ControlChars.Tab + strLogInfo;

        //        //// T_LOGに記入
        //        //WriteToLogTable(strLogLevel, strClassName, strMethondName, strLogInfo);
        //        return strLogInfoOut;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /////// <summary>
        ///////         ''' ログの内容をT_LOGに記録します
        ///////         ''' </summary>
        ///////         ''' <param name="strLogInfo">ログの情報</param>
        ///////         ''' <remarks></remarks>
        ////private static void WriteToLogTable(string strLogLevel, string strClassName, string strMethondName, string strLogInfo)
        ////{
        ////    DataBaseSQL objDb = new DataBaseSQL();
        ////    string strSQL = string.Empty;
        ////    string strUserId = i_Common.GetLoginUserInfo(HPFData.LOGINUSERINFO_KEY_ID);
        ////    string strTanmatu = i_Common.GetLoginUserInfo(HPFData.LOGINUSERINFO_KEY_LOGIN_TANMATU_ID);
        ////    try
        ////    {
        ////        if (strLogInfo.Length > HPFData.LOGJYOHOMAX)
        ////            strLogInfo = strLogInfo.Substring(0, HPFData.LOGJYOHOMAX);
        ////        objDb.DBLogIn();
        ////        strSQL = Get_Ins_T_Log(strLogLevel, strClassName, strMethondName, strLogInfo, strUserId, strTanmatu);
        ////        // ログをT_LOGに書き込む。
        ////        objDb.ExecuteNonQuery_T_Log(strSQL);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////    finally
        ////    {
        ////        objDb.DBLogOut();
        ////        objDb = null/* TODO Change to default(_) if this is not a reference type */;
        ////    }
        ////}


        //private static string Get_Ins_T_Log(string strLogLevel, string strClassName, string strMethondName, string strLogInfo, string strUserId, string strTanmatu)
        //{
        //    System.Text.StringBuilder strSql = new System.Text.StringBuilder();
        //    try
        //    {
        //        strSql.Append("    	INSERT INTO T_LOG   ").Append(Constants.vbCrLf);
        //        strSql.Append("    	([DateTime],	  ").Append(Constants.vbCrLf);
        //        strSql.Append("    	LogLevel,	  ").Append(Constants.vbCrLf);
        //        strSql.Append("    	ClassName,	  ").Append(Constants.vbCrLf);
        //        strSql.Append("    	MethodName,	  ").Append(Constants.vbCrLf);
        //        strSql.Append("    	LogJyoho,	  ").Append(Constants.vbCrLf);
        //        strSql.Append("    	UserId,	  ").Append(Constants.vbCrLf);
        //        strSql.Append("    	Tanmatu )  ").Append(Constants.vbCrLf);
        //        strSql.Append(" VALUES " + Constants.vbCrLf).Append(Constants.vbCrLf);
        //        strSql.Append("  (GETDATE() ").Append(Constants.vbCrLf);
        //        strSql.AppendFormat("  ,'{0}' ", strLogLevel).Append(Constants.vbCrLf);
        //        strSql.AppendFormat("  ,'{0}' ", strClassName).Append(Constants.vbCrLf);
        //        strSql.AppendFormat("  ,'{0}' ", strMethondName).Append(Constants.vbCrLf);
        //        strSql.AppendFormat("  ,'{0}' ", strLogInfo.Replace("'", "''")).Append(Constants.vbCrLf);
        //        strSql.AppendFormat("  ,'{0}' ", strUserId).Append(Constants.vbCrLf);
        //        strSql.AppendFormat("  ,'{0}' ) ", strTanmatu).Append(Constants.vbCrLf);

        //        return strSql.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        /////         ''' ログの内容をログファイルに記録します
        /////         ''' </summary>
        /////         ''' <param name="strLogInfo">ログの情報</param>
        /////         ''' <remarks></remarks>
        //private static void WriteToLogFile(string strLogInfo, bool blnOther = false)
        //{
        //    FileStream objFile = null;

        //    try
        //    {
        //        strLogInfo = string.Format("{0}{1}", strLogInfo, ControlChars.NewLine);
        //        if (blnOther)
        //            objFile = new FileStream(string.Format(@"{0}\{1}", i_LogOutPath, i_LogFileNameCon), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        //        else
        //            objFile = new FileStream(string.Format(@"{0}\{1}", i_LogOutPath, i_LogFileName), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        //        objFile.Seek(0, SeekOrigin.End);
        //        objFile.Write(Encoding.Default.GetBytes(strLogInfo), 0, Encoding.Default.GetByteCount(strLogInfo));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (Information.IsNothing(objFile) == false)
        //        {
        //            objFile.Close();
        //            objFile.Dispose();
        //        }
        //    }
        //}

        ///// <summary>
        /////         ''' ログ出力するか判断
        /////         ''' </summary>
        /////         ''' <param name="strLogLevel">ログのレベル</param>
        /////         ''' <returns>
        /////         ''' True :出力
        /////         ''' False:不出力
        /////         ''' </returns>
        /////         ''' <remarks></remarks>
        //private static bool IsWriteLog(string strLogLevel)
        //{
        //    try
        //    {
        //        if (HPFData.LOG_LEVEL_ALL.Equals(i_LogLevel) || HPFData.LOG_LEVEL_DEBUG.Equals(i_LogLevel))
        //            return true;

        //        return i_LogLevel.Equals(strLogLevel);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        /////         ''' ユーザーIDでログファイルの作成
        /////         ''' </summary>
        /////         ''' <param name="strUserID">ユーザーID</param>
        /////         ''' <remarks></remarks>
        //private static void SetLogFileNameByUserID(string strUserID)
        //{
        //    try
        //    {
        //        i_LogFileName = string.Format("{0}_{1}_{2}.{3}", DateTime.Now.ToString("yyyyMMdd"), i_LogFilePre, strUserID, i_LogFileExt);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        /////         ''' 空のログファイルの作成
        /////         ''' </summary>
        /////         ''' <remarks></remarks>
        //private static void SetLogFileNameByEmpty()
        //{
        //    try
        //    {
        //        i_LogFileName = string.Format("{0}_{1}.{2}", DateTime.Now.ToString("yyyyMMdd"), i_LogFilePre, i_LogFileExt);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        /////         ''' 空のログファイルの作成(Connect)
        /////         ''' </summary>
        /////         ''' <remarks></remarks>
        //private static void SetLogFileNameByEmptyCon()
        //{
        //    try
        //    {
        //        i_LogFileNameCon = string.Format("{0}_{1}.{2}", DateTime.Now.ToString("yyyyMMdd"), i_LogFilePreCon, i_LogFileExt);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        /////         ''' ログファイルのフォルダを作成
        /////         ''' </summary>
        /////         ''' <remarks></remarks>
        //private static void SetLogFileFolder()
        //{
        //    DirectoryInfo objDirect = null;

        //    try
        //    {
        //        objDirect = new DirectoryInfo(i_LogOutPath);
        //        if (objDirect.Exists == false)
        //            objDirect.Create();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        ///         ''' Appconfigから設定値を取得します
        ///         ''' </summary>
        ///         ''' <param name="strKey"></param>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        private static string GetAppSetting(string strKey)
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


    }
}
