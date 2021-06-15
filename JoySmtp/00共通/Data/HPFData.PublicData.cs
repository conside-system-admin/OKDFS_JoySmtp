using System;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace JoySmtp.Data
{
    /// <summary>
    ///     ''' 共通データの初期化処理で共通データを取得します
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    partial class HPFData
    {
        private static DataTable i_MessageInfo = new DataTable();           // メッセージ用のテーブル
        private static DataTable i_ResourceInfo = new DataTable();          // 画面表示用のテーブル
        private static string i_LanguageInfo = string.Empty;              // 言語の内容
        private static DataTable i_LoginUserInfo = new DataTable();         // ﾛｸﾞｲﾝﾕｰｻﾞｰ情報のテーブル
        private static DataTable i_SysCtrlInfo = new DataTable();           // 画面の属性のテーブル
        private static DataTable i_PublicKbnInfo = new DataTable();         // 共通区分のテーブル
        private static string i_DBConnectString = string.Empty;           // DB接続の文字列
        private static DataTable i_AppSettingInfo = new DataTable();        // appSettingsのテーブル

        private static String i_strAppUserName;
        private static String i_strServerAddr;
        private static String i_strServerBackAddr;
        private static int i_intPort = 25;
        private static String i_strFromAddr;
        private static String i_strToAddr;
        private static String i_strDns;
        private static String i_strDnsSub;
        private static String i_strDomain;
        private static Int32 i_intSendMaxNum = 1;
        private static String i_strMyServerName;
        private static String i_strInUserCD;
        private static String i_strInUserPW;
        //private static String i_strInCd;
        private static String i_strShikibetsuNo;

        private static String i_strInUserCDDefault;
        private static String i_strInUserPWDefault;
        private static String i_strShikibetsuNoDefault;

        private static Boolean i_blnShoriShikibetsu;

        private static String i_strDBHostName;
        private static String i_strDBName;
        private static int i_intSendTimer;
        private static int i_intSYGTimer;
        private static int i_intMailSendLimit;
        private static int i_intTimeoutLimitTime;

        private static DataTable i_tblNaccsErrCd;
        private static DataTable i_tblNaccsErrCdSub;
        private static DataTable i_tblNaccsOmitErrCd;

        private static String i_strMessageFileName;


        public static DataTable AppSettingInfo
        {
            set
            {
                i_AppSettingInfo = value;
            }
            get
            {
                return i_AppSettingInfo;
            }
        }

        public static DataTable MessageInfo
        {
            set
            {
                i_MessageInfo = value;
            }
            get
            {
                return i_MessageInfo;
            }
        }

        public static DataTable ResourceInfo
        {
            set
            {
                i_ResourceInfo = value;
            }
            get
            {
                return i_ResourceInfo;
            }
        }

        public static DataTable SysCtrlInfo
        {
            set
            {
                i_SysCtrlInfo = value;
            }
            get
            {
                return i_SysCtrlInfo;
            }
        }

        public static string LangeuageInfo
        {
            set
            {
                i_LanguageInfo = value;
            }
            get
            {
                return i_LanguageInfo;
            }
        }

        public static DataTable LoginUserInfo
        {
            set
            {
                i_LoginUserInfo = value;
            }
            get
            {
                return i_LoginUserInfo;
            }
        }

        public static string DBConenctString
        {
            set
            {
                i_DBConnectString = value;
            }
            get
            {
                return i_DBConnectString;
            }
        }

        public static DataTable PublicKbnInfo
        {
            set
            {
                i_PublicKbnInfo = value;
            }
            get
            {
                return i_PublicKbnInfo;
            }
        }
        public static String AppUserName
        {
            set { i_strAppUserName = value; }
            get { return i_strAppUserName; }
        }

        public static String ServerAddress
        {
            set { i_strServerAddr = value; }
            get { return i_strServerAddr; }
        }
        public static String ServerBackAddress
        {
            set { i_strServerBackAddr = value; }
            get { return i_strServerBackAddr; }
        }
        public static int PortNo
        {
            set { i_intPort = value; }
            get { return i_intPort; }
        }
        public static String FromMailAddress
        {
            set { i_strFromAddr = value; }
            get { return i_strFromAddr; }
        }
        public static String ToMailAddress
        {
            set { i_strToAddr = value; }
            get { return i_strToAddr; }
        }
        public static String ServerDomain
        {
            set { i_strDomain = value; }
            get { return i_strDomain; }
        }
        public static String DnsServer
        {
            set { i_strDns = value; }
            get { return i_strDns; }
        }
        public static String DnsServerSub
        {
            set { i_strDnsSub = value; }
            get { return i_strDnsSub; }
        }
        public static Int32 SendMaxNum
        {
            set { i_intSendMaxNum = value; }
            get { return i_intSendMaxNum; }
        }
        public static String MyServerName
        {
            set { i_strMyServerName = value; }
            get { return i_strMyServerName; }
        }
        public static String InputUserCode
        {
            set { i_strInUserCD = value; }
            get { return i_strInUserCD; }
        }
        public static String InputUserPass
        {
            set { i_strInUserPW = value; }
            get { return i_strInUserPW; }
        }
        //public static String NaccsInputCode
        //{
        //    set { i_strInCd = value; }
        //    get { return i_strInCd; }
        //}

        public static String NaccsShikibetsuNo
        {
            set { i_strShikibetsuNo = value; }
            get { return i_strShikibetsuNo; }
        }

        public static String InputUserCodeDefault
        {
            set { i_strInUserCDDefault = value; }
            get { return i_strInUserCDDefault; }
        }
        public static String InputUserPassDefault
        {
            set { i_strInUserPWDefault = value; }
            get { return i_strInUserPWDefault; }
        }
        public static String NaccsShikibetsuNoDefault
        {
            set { i_strShikibetsuNoDefault = value; }
            get { return i_strShikibetsuNoDefault; }
        }

        /// <summary>
        /// テスト用一時ファイル名
        /// </summary>
        public static String TempMessageFileName
        {
            set
            {
                i_strMessageFileName = value;
            }
            get
            {
                return i_strMessageFileName;
            }
        }

        /// <summary>
        /// 表示用　DBホスト名
        /// </summary>
        public static String DataBaseHostName
        {
            set { i_strDBHostName = value; }
            get { return i_strDBHostName; }
        }
        /// <summary>
        /// 表示用　DB名
        /// </summary>
        public static String DataBaseName
        {
            set { i_strDBName = value; }
            get { return i_strDBName; }
        }
        /// <summary>
        /// DB確認のタイマー期間
        /// </summary>
        public static int IntervalSendTimer
        {
            set { i_intSendTimer = value; }
            get { return i_intSendTimer; }
        }
        /// <summary>
        /// SYGタイマーの期間
        /// </summary>
        public static int IntervalSYGTimer
        {
            set { i_intSYGTimer = value; }
            get { return i_intSYGTimer; }
        }
        /// <summary>
        /// メール送信の期間
        /// </summary>
        public static int MailSendLimit
        {
            set { i_intMailSendLimit = value; }
            get { return i_intMailSendLimit; }
        }
        /// <summary>
        /// DB確認のタイマー期間
        /// </summary>
        public static int TimeoutLimitTime
        {
            set { i_intTimeoutLimitTime = value; }
            get { return i_intTimeoutLimitTime; }
        }
        /// <summary>
        /// NaccsErrCdデータテーブル
        /// </summary>
        public static DataTable NaccsErrCd
        {
            set { i_tblNaccsErrCd = value; }
            get { return i_tblNaccsErrCd; }
        }
        /// <summary>
        /// NaccsErrCdSubデータテーブル
        /// </summary>
        public static DataTable NaccsErrCdSub
        {
            set { i_tblNaccsErrCdSub = value; }
            get { return i_tblNaccsErrCdSub; }
        }
        /// <summary>
        /// NaccsErrC除外データテーブル
        /// </summary>
        public static DataTable NaccsOmitErrCd
        {
            set { i_tblNaccsOmitErrCd = value; }
            get { return i_tblNaccsOmitErrCd; }
        }

        /// <summary>
        /// 処理識別固定フラグ　
        /// </summary>
        public static Boolean ShoriShikibetsuTestFlg
        {
            set { i_blnShoriShikibetsu = value; }
            get { return i_blnShoriShikibetsu; }
        
        }
    }
}
