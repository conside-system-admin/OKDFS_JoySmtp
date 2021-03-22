using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using JoySmtp.Data;
using JoySmtp.CLogOut;
using System.Data.SqlClient;
using JoySmtp.DataBase;

//using FarPoint.Win.Spread.Model;
//using FarPoint.Win.Spread;
//using GrapeCity.Win.Editors;
//using System.Threading;
//using JoySmtp.SeqBarCode.HPFSeqBarCode;
using System.Configuration;


namespace JoySmtp.JoyCommon
{
    public class InitProject
    {
        private const string APPSETTING_LANGUAGE = "Language";
        private const string XML_FILE_MESSAGEN = "Message.xml";
        private const string XML_FILE_RESOURCE = "Resource.xml";
        private const string APP_USER_NAME = "AppUserName";
        private const string AOOSETTING_DBHOST = "DbHost";
        private const string AOOSETTING_DBUSERb = "DbUser";
        private const string AOOSETTING_DBPWD = "DbPwd";
        private const string AOOSETTING_DBNAME = "DbName";
        private const string AOOSETTING_TIMEOUT = "Timeout";

        private const string AOOSETTING_SYSTEM_TBL_NAME = "DbSysTblName";
        private const string AOOSETTING_SYSTEM_KEY_NAME = "DbSysKeyName";
        private const string AOOSETTING_KBN_TBL_NAME = "DbKbnTblName";
        private const string AOOSETTING_KBN_KEY_NAME = "DbKbnKeyName";

        private const string Path_CRM_SATEI_DB = "Path_CRM_SATEI_DB";
        private const string Path_CRM_STAFF_DB = "Path_CRM_STAFF_DB";
        private const string Path_MstRecv = "Path_MstRecv";
        private const string Path_MstRecvBack = "Path_MstRecvBack";
        private const string Path_MstRecvTemp = "Path_MstRecvTemp";
        private const string Path_RaaFtpRecvAddress = "Path_RaaFtpRecvAddress";
        private const string Path_RaaFtpSendAddress = "Path_RaaFtpSendAddress";
        private const string Path_RaaFtpRecvTemp = "Path_RaaFtpRecvTemp";
        private const string Path_RaaFtpSendTemp = "Path_RaaFtpSendTemp";
        private const string Path_RaaRecv = "Path_RaaRecv";
        private const string Path_RaaRecvBack = "Path_RaaRecvBack";
        private const string Path_RaaRecvTemp = "Path_RaaRecvTemp";
        private const string Path_RaaSend = "Path_RaaSend";
        private const string Path_RaaSendBack = "Path_RaaSendBack";
        private const string Path_RaaSendTemp = "Path_RaaSendTemp";
        private const string Raa_RecordKbn = "Raa_RecordKbn";
        private const string Cd_RaaFtpId = "Cd_RaaFtpId";
        private const string Cd_RaaFtpPw = "Cd_RaaFtpPw";
        private const string MailAddrFrom = "MailAddrFrom";

        private Common i_Common;
        protected static string i_SystemTblName = string.Empty;
        protected static string i_SystemKeyName = string.Empty;
        protected static string i_KbnTblName = string.Empty;
        protected static string i_KbnKeyName = string.Empty;

        private const string NACCS_TO_ADDR = "NACCSToAddress";
        private const string NACCS_FROM_ADDR = "NACCSFromAddress";
        private const string NACCS_IP = "NACCSIP";
        private const string NACCS_IP_BACK = "NACCSIPBACK";
        private const string NACCS_PORT_NO = "NACCSPortNo";
        private const string NACCS_DNS_SERVER = "NACCSDnsServer";
        private const string NACCS_DNS_SERVER_SUB = "NACCSDnsServerSub";
        private const string NACCS_DOMAIN = "NACCSDomain";
        private const string NACCS_SEND_MAX_NUM = "NaccsSendMailMaxNum";
        
        private const string NACCS_USER_CODE = "NaccsInputUserCode";
        private const string NACCS_USER_PASS = "NaccsInputUserPass";
        //private const string NACCS_INPUT_CODE = "NaccsInputCode";
        private const string NACCS_SHIKIBETSU_NO = "NaccsShikibetsuNo";
        private const string NACCS_MY_SERVER_NAME = "MyServerName";
        private const string NACCS_TEMP_SEND_FILE_NAME = "NACCSSendMessageFile";

        private const string INTERVAL_SEND_TIMER = "IntervalSendTimer";
        private const string INTERVAL_SYG_TIMER = "IntervalSYGTimer";
        private const int INTERVAL_SEND_TIMER_DEFAULT = 60;
        private const int INTERVAL_SYG_TIMER_DEFAULT = 300;


        
        /// <summary>
        ///         ''' 共通データを取得します
        ///         ''' </summary>
        ///         ''' <remarks></remarks>
        public bool InitPublicInfo(string strDBConenctString)
        {
            try
            {
                i_Common = new Common();

                // Appconfigの内容を取得します
                InitAppConfigInfo(strDBConenctString);

                // If CheckDataBase() = False Then
                // MessageBox.Show("データベースの接続に失敗しました。システム管理者に連絡してください。", "SpPrimaシステム")
                // Return False
                // End If

                // データベースから設定用のデータを取得します
                InitPublicData();

                return true;
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }
        /// <summary>
        ///         ''' AppConfigの内容を取得します
        ///         ''' </summary>
        ///         ''' <remarks></remarks>
        private void InitAppConfigInfo(string strDBConenctString)
        {
            try
            {
                // i_EmunStatus = EnumStatus.GET_APP_INFO
                // GcPrcStep.DisplayTextFormat = PROC_MSG_GET_APP_INFO
                // Application.DoEvents()

                HPFData.LangeuageInfo = GetAppSetting(APPSETTING_LANGUAGE);


                if (strDBConenctString == "")
                    HPFData.DBConenctString = GetAppDbSetting();
                else
                    HPFData.DBConenctString = strDBConenctString;
            }

            // GcPrcStep.Value = PROG_STEP_DEF * i_EmunStatus
            // Application.DoEvents()

            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }

        /// <summary>
        ///         ''' AppConfigの内容をセット
        ///         ''' </summary>
        ///         ''' <remarks></remarks>
        private void SetAppConfigInfo(string strKey)
        {
            DataRow drTemp;
            string strValue = string.Empty;
            try
            {
                if (HPFData.AppSettingInfo == null || HPFData.AppSettingInfo.Rows.Count == 0)
                {
                    HPFData.AppSettingInfo.Columns.Add("key", Type.GetType("System.String"));
                    HPFData.AppSettingInfo.Columns.Add("value", Type.GetType("System.String"));
                    HPFData.AppSettingInfo.Clear();
                }

                strValue = GetAppSetting(strKey);
                if (strValue != string.Empty)
                {
                    drTemp = HPFData.AppSettingInfo.NewRow();
                    drTemp["key"] = strKey;
                    drTemp["value"] = GetAppSetting(strKey);
                    HPFData.AppSettingInfo.Rows.Add(drTemp);
                }
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }

        /// <summary>
        ///         ''' Appconfigから設定値を取得します
        ///         ''' </summary>
        ///         ''' <param name="strKey"></param>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        private string GetAppSetting(string strKey)
        {
            string strValue = string.Empty;
            try
            {
                strValue = Common.ConvertToString(ConfigurationManager.AppSettings[strKey]);
                return strValue;
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }

        /// <summary>
        ///         ''' Appconfigから設定値を取得します
        ///         ''' </summary> 
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        private string GetAppDbSetting()
        {
            string strValue = string.Empty;
            string strPwdValue = string.Empty;
            StringBuilder w_StrConnectionStrings = new StringBuilder(string.Empty);
            try
            {

                // パスワードの復号化処理を行う
                strPwdValue = i_Common.DecryptDESString(ConfigurationManager.AppSettings[AOOSETTING_DBPWD], HPFData.DECRYPT_KEY);
                // strPwdValue = ConfigurationManager.AppSettings(AOOSETTING_DBPWD)

                strValue = Common.ConvertToString(w_StrConnectionStrings
                    .Append("Data Source=").Append(ConfigurationManager.AppSettings[AOOSETTING_DBHOST])
                    .Append(";Initial Catalog=").Append(ConfigurationManager.AppSettings[AOOSETTING_DBNAME])
                    .Append(";User Id=").Append(ConfigurationManager.AppSettings[AOOSETTING_DBUSERb])
                    .Append(";Password=").Append(strPwdValue)
                    .Append(";Connection Timeout=").Append(ConfigurationManager.AppSettings[AOOSETTING_TIMEOUT])
                    .Append(";").ToString());

                i_SystemTblName = GetAppSetting(AOOSETTING_SYSTEM_TBL_NAME);
                i_SystemKeyName = GetAppSetting(AOOSETTING_SYSTEM_KEY_NAME);

                if (string.Empty.Equals(i_SystemTblName))
                    i_SystemTblName = "C_SYS_CTRL";
                if (string.Empty.Equals(i_SystemKeyName))
                    i_SystemKeyName = "SYS_CTRL_KEY";

                i_KbnTblName = GetAppSetting(AOOSETTING_KBN_TBL_NAME);
                i_KbnKeyName = GetAppSetting(AOOSETTING_KBN_KEY_NAME);

                if (string.Empty.Equals(i_KbnTblName))
                    i_KbnTblName = "C_KBN";
                if (string.Empty.Equals(i_KbnKeyName))
                    i_KbnKeyName = "SHIKIBETSU_KBN";

                return strValue;
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
        }

        /// <summary>
        ///         ''' データベースから共通情報を取得します
        ///         ''' </summary>
        ///         ''' <remarks></remarks>
        private void InitPublicData()
        {
            DataBaseSQL objSQL = null/* TODO Change to default(_) if this is not a reference type */;
            DataSet dsTemp = new DataSet();

            try
            {
                objSQL = new DataBaseSQL();
                objSQL.DBLogIn();

                // 'システムコントロールの情報
                // i_EmunStatus = EnumStatus.GET_SYSTEM_CTRL
                // GcPrcStep.DisplayTextFormat = PROC_MSG_GET_SYSTEM_CTRL
                // Application.DoEvents()


                // dsTemp = New DataSet
                // dsTemp.ReadXml(XML_FILE_SYS_CTRL)
                // SysCtrlInfo = dsTemp.Tables(0)
                HPFData.SysCtrlInfo = objSQL.GetDataTable(GetMST_SYS_CTRL(), "MST_SYS_CTRL", false);
                // GcPrcStep.Value = PROG_STEP_DEF * i_EmunStatus
                // Application.DoEvents()

                //// リソースの情報
                //// i_EmunStatus = EnumStatus.GET_RESOURCE
                //// GcPrcStep.DisplayTextFormat = PROC_MSG_GET_RESOURCE
                //// Application.DoEvents()
                //dsTemp = new DataSet();
                //dsTemp.ReadXml(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + XML_FILE_RESOURCE);
                //HPFData.ResourceInfo = dsTemp.Tables[0];
                //// GcPrcStep.Value = PROG_STEP_DEF * i_EmunStatus
                //// Application.DoEvents()

                //// メッセージ情報
                //// i_EmunStatus = EnumStatus.GET_MESSAGE
                //// GcPrcStep.DisplayTextFormat = PROC_MSG_GET_MESSAGE
                //dsTemp = new DataSet();
                //dsTemp.ReadXml(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + XML_FILE_MESSAGEN);
                //HPFData.MessageInfo = dsTemp.Tables[0];
                //// Application.DoEvents() 

                // GcPrcStep.Value = PROG_STEP_DEF * i_EmunStatus
                // Application.DoEvents()

                // 共通区分情報
                // i_EmunStatus = EnumStatus.GET_PUBLICKBN
                // GcPrcStep.DisplayTextFormat = PROC_MSG_GET_PUBLICKBN
                // Application.DoEvents()
                HPFData.PublicKbnInfo = objSQL.GetDataTable(GetC_KYOTHU_KBN_SQL(), "C_KYOTHU_KBN", false);

                HPFData.AppUserName = GetAppSetting(APP_USER_NAME);

                HPFData.ServerAddress = GetAppSetting(NACCS_IP);
                HPFData.ServerBackAddress = GetAppSetting(NACCS_IP_BACK);
                HPFData.FromMailAddress = GetAppSetting(NACCS_FROM_ADDR);
                HPFData.ToMailAddress = GetAppSetting(NACCS_TO_ADDR);
                HPFData.PortNo = Convert.ToInt32(GetAppSetting(NACCS_PORT_NO));
                HPFData.ServerDomain = GetAppSetting(NACCS_DOMAIN);
                HPFData.DnsServer = GetAppSetting(NACCS_DNS_SERVER);
                HPFData.DnsServerSub = GetAppSetting(NACCS_DNS_SERVER_SUB);
                HPFData.SendMaxNum = Convert.ToInt32(GetAppSetting(NACCS_SEND_MAX_NUM));
                

                HPFData.MyServerName = GetAppSetting(NACCS_MY_SERVER_NAME);
                HPFData.TempMessageFileName = GetAppSetting(NACCS_TEMP_SEND_FILE_NAME);

                HPFData.InputUserCode = GetAppSetting(NACCS_USER_CODE);
                HPFData.InputUserPass = GetAppSetting(NACCS_USER_PASS);
                //HPFData.NaccsInputCode  = GetAppSetting(NACCS_INPUT_CODE);
                HPFData.NaccsShikibetsuNo = GetAppSetting(NACCS_SHIKIBETSU_NO);

                HPFData.InputUserCodeDefault = GetAppSetting(NACCS_USER_CODE);
                HPFData.InputUserPassDefault = GetAppSetting(NACCS_USER_PASS);
                HPFData.NaccsShikibetsuNoDefault = GetAppSetting(NACCS_SHIKIBETSU_NO);

                HPFData.DataBaseHostName = GetAppSetting(AOOSETTING_DBHOST);
                HPFData.DataBaseName = GetAppSetting(AOOSETTING_DBNAME);

                var str = GetAppSetting(INTERVAL_SEND_TIMER);
                if (string.IsNullOrWhiteSpace(str)){    HPFData.IntervalSendTimer = INTERVAL_SEND_TIMER_DEFAULT;    }
                else { HPFData.IntervalSendTimer = Convert.ToInt32(str); }
                str = GetAppSetting(INTERVAL_SYG_TIMER);
                if (string.IsNullOrWhiteSpace(str)) { HPFData.IntervalSYGTimer = INTERVAL_SYG_TIMER_DEFAULT; }
                else { HPFData.IntervalSYGTimer = Convert.ToInt32(str); }

                str = GetAppSetting(Data.HPFData.TEST_FLG_SHORI_SHIKIBETSU);
                if (str.Length > 0)
                {
                    HPFData.ShoriShikibetsuTestFlg = true;
                }
                else
                {
                    HPFData.ShoriShikibetsuTestFlg = false;
                }

                HPFData.NaccsErrCd = objSQL.GetDataTable(GetC_KYOTHU_KBN_SQL(HPFData.SIKIBETU_KBN_NACCS_ERR), i_KbnTblName, false);
                HPFData.NaccsErrCdSub = objSQL.GetDataTable(GetC_KYOTHU_KBN_SQL(HPFData.SIKIBETU_KBN_NACCS_ERR_SUB), i_KbnTblName, false);
                HPFData.NaccsOmitErrCd = objSQL.GetDataTable(GetC_KYOTHU_KBN_SQL(HPFData.SIKIBETU_KBN_NACCS_OMIT_ERR), i_KbnTblName, false);

                
                //this.SendErrorMail("テストメールを送信しました。");

            }
            // GcPrcStep.Value = PROG_STEP_DEF * i_EmunStatus
            // Application.DoEvents()

            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.Message, MethodBase.GetCurrentMethod().Name);
                throw ex;
            }
            finally
            {
                objSQL.DBLogOut();
                objSQL = null/* TODO Change to default(_) if this is not a reference type */;
            }
        }

        /// <summary>
        ///         ''' システムコントロールマスタからデータを取得します
        ///         ''' </summary>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        private string GetMST_SYS_CTRL()
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendLine(" SELECT ");
            strSQL.AppendLine(" " + i_SystemKeyName + " ,");
            strSQL.AppendLine(" SETTING_INFO ");
            strSQL.AppendLine();
            strSQL.AppendLine(" FROM " + i_SystemTblName);
            strSQL.AppendLine(" ORDER BY " + i_SystemKeyName);

            return strSQL.ToString();
        }

        /// <summary>
        ///         ''' 共通区分マスタからデータを取得します
        ///         ''' </summary>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        private string GetC_KYOTHU_KBN_SQL(string strShikibetsuCd = "")
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendLine(" SELECT ");

            strSQL.AppendLine(" * ");

            strSQL.AppendLine(" FROM " + i_KbnTblName);

            if (strShikibetsuCd != "")
            {
                strSQL.AppendLine(" WHERE " + i_KbnKeyName + " = " + strShikibetsuCd);
            }
            strSQL.AppendLine(" ORDER BY " + i_KbnKeyName + ", ");
            strSQL.AppendLine(" HYOJI_JUN");

            return strSQL.ToString();
        }
        
        /// <summary>
        /// ホスト名からIPアドレスを取得する
        /// </summary>
        /// <returns></returns>
        private IPAddress[] GetIpAddressFromHost(IPAddress dnsIp)
        {
            // 自身のホスト名を取得
            var host = Dns.GetHostName();

            // ホスト名からIPアドレスを取得
            var ips = Dns.GetHostAddresses(host);

            return ips;
            //// IPアドレスを列挙
            //foreach (var ip in ips)
            //{
            //    Console.WriteLine(ip);
            //}
        }


 
    }
}
