using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Data;
using JoySmtp.JoyCommon;
using JoySmtp.CLogOut;
using JoySmtp.Data;
using JoySmtp.DataBase;

namespace JoySmtp.Nac
{

    #region "モデル"
    public class DBEdiSend
    {
        /// <summary>
        /// SEQ
        /// </summary>
        public long? DATA_SEQ { get; set; }
        /// <summary>
        /// 出発日時
        /// </summary>
        public DateTime? TIME_OF_DEPARTURE { get; set; }
        /// <summary>
        /// 便名
        /// </summary>
        public string FLIGHT_ID { get; set; }
        /// <summary>
        /// PAX番号
        /// </summary>
        public string PAX_NO { get; set; }
        /// <summary>
        /// 枝番号
        /// </summary>
        public string EDA_NO { get; set; }
        /// <summary>
        /// PAX受信回数
        /// </summary>
        public Int32? RCV_SEQ { get; set; }
        /// <summary>
        /// 送信タイプ
        /// </summary>
        public NACCS_REPORTTYPE SEND_TYPE { get; set; }
        /// <summary>
        /// EDI送信フラグ
        /// </summary>
        public DateTime? EDI_SENDDATE { get; set; }
        /// <summary>
        /// 電文引継コード
        /// </summary>
        public string DENBUN_HANDOVER { get; set; }
        /// <summary>
        /// 輸入申告番号
        /// </summary>
        public string YUNYU_SHINKOKU_NO { get; set; }
        /// <summary>
        /// 取込状態
        /// </summary>
        public string TORIKOMI_JOTAI { get; set; }
        /// <summary>
        /// エラー内容
        /// </summary>
        public string ERROR_NAIYO { get; set; }
        /// <summary>
        /// EDI出力結果
        /// </summary>
        public string EDI_RESULT { get; set; }        
        /// <summary>
        /// 受信RESULT
        /// </summary>
        public long? FLG_RESULT { get; set; }
        /// <summary>
        /// SAD4011
        /// </summary>
        public long? SEQ_4011 { get; set; }
        /// <summary>
        /// SAD4031
        /// </summary>
        public long? SEQ_4031 { get; set; }
        /// <summary>
        /// SAD4071
        /// </summary>
        public long? SEQ_4071 { get; set; }
        /// <summary>
        /// SAD4081
        /// </summary>
        public long? SEQ_4081 { get; set; }
        /// <summary>
        /// SAD_OTHER
        /// </summary>
        public long? SEQ_SAD_OTHER { get; set; }
        /// <summary>
        /// SAF02_OTHER
        /// </summary>
        public long? SEQ_SAF { get; set; }
        ///// <summary>
        ///// 作成者
        ///// </summary>
        //public string CRT_STAFF_CD { get; set; }
        ///// <summary>
        ///// 作成日時
        ///// </summary>
        //public DateTime CRT_DATE { get; set; }
        ///// <summary>
        ///// 作成端末ID
        ///// </summary>
        //public string CRT_TANMATU_ID { get; set; }
        ///// <summary>
        ///// 更新者
        ///// </summary>
        //public string UPD_STAFF_CD { get; set; }
        ///// <summary>
        ///// 更新日時
        ///// </summary>
        //public DateTime UPD_DATE { get; set; }
        ///// <summary>
        ///// 更新端末ID
        ///// </summary>
        //public string UPD_TANMATU_ID { get; set; }
        ///// <summary>
        ///// 更新ﾊﾞｰｼﾞｮﾝNo
        ///// </summary>
        //public Int32 VERSION_NO { get; set; }
        ///// <summary>
        ///// 削除FLG
        ///// </summary>
        //public DateTime DELETE_FLG { get; set; }

        public static List<DBEdiSend> GetDBEdiSendModel(DataTable dtTbl, NACCS_REPORTTYPE rtype = NACCS_REPORTTYPE.NONE)
        {
            List<DBEdiSend> list = new List<DBEdiSend>();
            if (dtTbl != null && dtTbl.Rows.Count > 0)
            {
                DBEdiSend model;
                foreach (DataRow row in dtTbl.Rows)
                {
                    model = new DBEdiSend();
                    model.TIME_OF_DEPARTURE = (Common.ConvertToDateTime(row["TIME_OF_DEPARTURE"]));
                    model.FLIGHT_ID = (Common.ConvertToString(row["FLIGHT_ID"]));
                    model.PAX_NO = (Common.ConvertToString(row["PAX_NO"]));
                    model.EDA_NO = (Common.ConvertToString(row["EDA_NO"]));
                    model.RCV_SEQ = (Common.ConvertToInteger(row["RCV_SEQ"]));

                    model.SEND_TYPE = rtype;
                    model.DENBUN_HANDOVER = "";
                    model.YUNYU_SHINKOKU_NO = "";
                    model.EDI_SENDDATE = null;
                    model.TORIKOMI_JOTAI = "";
                    model.ERROR_NAIYO = "";
                    model.EDI_RESULT = "";

                    model.FLG_RESULT = null;
                    model.SEQ_4011 = null;
                    model.SEQ_4031 = null;
                    model.SEQ_4071 = null;
                    model.SEQ_4081 = null;
                    model.SEQ_SAD_OTHER = null;
                    model.SEQ_SAF = null;

                    list.Add(model);
                }
            }
            return list;
        }
        public string GETSQL_SELECT_CNT()
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("SELECT *");
            strSql.AppendLine("From T_EDI_SEND");
            strSql.AppendLine("WHERE 1=1");
            strSql.AppendFormat("  AND TIME_OF_DEPARTURE = convert(datetime, '{0}')", Common.ConvertToDateTimeString(this.TIME_OF_DEPARTURE)).AppendLine();
            strSql.AppendFormat("  AND FLIGHT_ID = '{0}'", this.FLIGHT_ID).AppendLine();
            strSql.AppendFormat("  AND PAX_NO = '{0}'", this.PAX_NO).AppendLine();
            strSql.AppendFormat("  AND EDA_NO = '{0}'", this.EDA_NO).AppendLine();
            strSql.AppendFormat("  AND (").AppendLine();
            //strSql.AppendFormat("    (EDI_SENDDATE > DATEADD(minute, -30, GETDATE() ))").AppendLine();
            //strSql.AppendFormat("    OR").AppendLine();
            strSql.AppendFormat("    (TORIKOMI_JOTAI <> '{0}')", HPFData.TORIKOMI_JOTAI.NACCS_ERROR).AppendLine();
            strSql.AppendFormat("  )").AppendLine();
            //strSql.AppendFormat("  AND DELETE_FLG IS NULL").AppendLine();

            return strSql.ToString();
        }
        /// <summary>
        /// 自データからSQL作成DBEdiSend model, 
        /// </summary>
        /// <returns></returns>
        public string GetSQL_INSERT()
        {
            if (this.TIME_OF_DEPARTURE == null ||
                this.FLIGHT_ID == null ||
                this.PAX_NO == null ||
                this.EDA_NO == null ||
                this.RCV_SEQ == null)
            {
                return "";
            }
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("INSERT INTO T_EDI_SEND (");
            strSql.AppendLine("  TIME_OF_DEPARTURE");
            strSql.AppendLine("  ,FLIGHT_ID");
            strSql.AppendLine("  ,PAX_NO");
            strSql.AppendLine("  ,EDA_NO");
            strSql.AppendLine("  ,RCV_SEQ");
            strSql.AppendLine("  ,SEND_TYPE");
            strSql.AppendLine("  ,DENBUN_HANDOVER");
            strSql.AppendLine("  ,EDI_SENDDATE");
            strSql.AppendLine("  ,TORIKOMI_JOTAI");
            strSql.AppendLine("  ,ERROR_NAIYO");

            strSql.AppendLine("  ,CRT_STAFF_CD");
            strSql.AppendLine("  ,CRT_DATE");
            strSql.AppendLine("  ,CRT_TANMATU_ID");
            //strSql.AppendLine("  ,UPD_STAFF_CD");
            //strSql.AppendLine("  ,UPD_DATE");
            //strSql.AppendLine("  ,UPD_TANMATU_ID");
            strSql.AppendLine("  ,VERSION_NO");

            strSql.AppendLine(") OUTPUT INSERTED.DATA_SEQ");
            strSql.AppendLine(" VALUES (");
            strSql.AppendFormat("  '{0}'", Common.ConvertToDateTimeString(this.TIME_OF_DEPARTURE)).AppendLine();
            strSql.AppendFormat("  ,'{0}'", this.FLIGHT_ID).AppendLine();
            strSql.AppendFormat("  ,'{0}'", this.PAX_NO).AppendLine();
            strSql.AppendFormat("  ,'{0}'", this.EDA_NO).AppendLine();
            strSql.AppendFormat("  ,{0}", this.RCV_SEQ).AppendLine();
            strSql.AppendFormat("  ,'{0}'", this.SEND_TYPE.ToString()).AppendLine();
            strSql.AppendFormat("  ,'{0}'", this.DENBUN_HANDOVER).AppendLine();
            strSql.AppendFormat("  ,GETDATE()", "").AppendLine();
            strSql.AppendFormat("  ,'{0}'", HPFData.TORIKOMI_JOTAI.KYOKA_MACHI).AppendLine();
            strSql.AppendFormat("  ,'{0}'", this.ERROR_NAIYO).AppendLine();

            strSql.AppendFormat("  ,'{0}'", HPFData.AppUserName).AppendLine();
            strSql.AppendFormat("  ,GETDATE()", "").AppendLine();
            strSql.AppendFormat("  ,'{0}'", Environment.MachineName).AppendLine();
            //strSql.AppendFormat("  ,null","").AppendLine();
            //strSql.AppendFormat("  ,null", "").AppendLine();
            //strSql.AppendFormat("  ,null", "").AppendLine();
            strSql.AppendFormat("  ,1", "").AppendLine();

            strSql.AppendLine(")");

            return strSql.ToString();
        }
        ///// <summary>
        ///// 自データからSQL作成DBEdiSend model, 
        ///// </summary>
        ///// <returns></returns>
        //public string GetSQL_UPDATE_HANDOVER(string strHandover)
        //{
        //    StringBuilder strSql = new StringBuilder();

        //    strSql.AppendLine("UPDATE T_EDI_SEND SET");
        //    strSql.AppendFormat("  DENBUN_HANDOVER = '{0}'", strHandover).AppendLine();

        //    strSql.AppendFormat("  ,UPD_STAFF_CD = {0}", HPFData.AppUserName).AppendLine();
        //    strSql.AppendFormat("  ,UPD_DATE = GETDATE()", "");
        //    strSql.AppendFormat("  ,UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
        //    strSql.AppendLine("  ,VERSION_NO = VERSION_NO + 1");

        //    strSql.AppendLine("WHERE 1=1");
        //    strSql.AppendFormat("  AND TIME_OF_DEPARTURE = convert(datetime, '{0}')", Common.ConvertToDateTimeString(this.TIME_OF_DEPARTURE)).AppendLine();
        //    strSql.AppendFormat("  AND FLIGHT_ID = '{0}'", this.FLIGHT_ID).AppendLine();
        //    strSql.AppendFormat("  AND PAX_NO = '{0}'", this.PAX_NO).AppendLine();
        //    strSql.AppendFormat("  AND EDA_NO = '{0}'", this.EDA_NO).AppendLine();
        //    //strSql.AppendFormat("  AND RCV_SEQ = {0}", this.RCV_SEQ).AppendLine();

        //    return strSql.ToString();
        //}
        /// <summary>
        /// システム間排他制御の値変更
        /// </summary>
        /// <param name="useflg"></param>
        /// <returns></returns>
        public Boolean SetSendTblRecv(NACCS_REPORTTYPE recvtype, long lngseq, string strYunyu = "", string strTorikomi = "", string strErr = "", string strErrDetail = "")
        {
            DataBaseSQL objDb = null;
            int ret = 0;
            if (string.IsNullOrWhiteSpace(this.PAX_NO))
            {
                return false;
            }
            try
            {
                objDb = new DataBaseSQL();
                objDb.DBLogIn();

                objDb.BeginTransaction();

                objDb.ExecuteNonQuery(GetSQL_UPDATE_SEND_TBL_RECV(recvtype, lngseq, strYunyu, strTorikomi, strErr, strErrDetail), null, out ret);

                objDb.Commit();
                if (ret > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
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
        /// DBの値変更
        /// </summary>
        /// <param name="recvtype"></param>
        /// <param name="lngseq"></param>
        /// <param name="strYunyu"></param>
        /// <returns></returns>
        public string GetSQL_UPDATE_SEND_TBL_RECV(NACCS_REPORTTYPE recvtype, long lngseq, string strYunyu = "", string strTorikomi = "", string strErr = "", string strErrDetail = "")
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("UPDATE T_EDI_SEND SET");

            strSql.AppendFormat("  UPD_STAFF_CD = '{0}'", HPFData.AppUserName).AppendLine();
            strSql.AppendFormat("  ,UPD_DATE = GETDATE()", "");
            strSql.AppendFormat("  ,UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
            strSql.AppendLine("  ,VERSION_NO = VERSION_NO + 1");

            if (!string.IsNullOrWhiteSpace(strYunyu))
            {
                strSql.AppendFormat("  ,YUNYU_SHINKOKU_NO = '{0}'", strYunyu).AppendLine();
                this.YUNYU_SHINKOKU_NO = strYunyu;
            }

            if (!string.IsNullOrWhiteSpace(strTorikomi))
            {
                strSql.AppendFormat("  ,TORIKOMI_JOTAI = '{0}'", strTorikomi).AppendLine();
                this.TORIKOMI_JOTAI = strTorikomi;
            }

            if (!string.IsNullOrWhiteSpace(strErr))
            {
                strSql.AppendFormat("  ,ERROR_NAIYO = '{0}'", strErr).AppendLine();
                this.ERROR_NAIYO = strErr;
            }
            switch (recvtype)
            {
                case NACCS_REPORTTYPE.RESULT:
                    strSql.AppendFormat("  ,FLG_RESULT = {0}", lngseq).AppendLine();
                    this.FLG_RESULT = lngseq;
                    break;
                case NACCS_REPORTTYPE.SAD4011:
                    strSql.AppendFormat("  ,SEQ_4011 = {0}", lngseq).AppendLine();
                    this.SEQ_4011 = lngseq;
                    break;
                case NACCS_REPORTTYPE.SAD4031:
                    strSql.AppendFormat("  ,SEQ_4031 = {0}", lngseq).AppendLine();
                    this.SEQ_4031 = lngseq;
                    break;
                case NACCS_REPORTTYPE.SAD4071:
                    strSql.AppendFormat("  ,SEQ_4071 = {0}", lngseq).AppendLine();
                    this.SEQ_4071 = lngseq;
                    break;
                case NACCS_REPORTTYPE.SAD4081:
                    strSql.AppendFormat("  ,SEQ_4081 = {0}", lngseq).AppendLine();
                    this.SEQ_4081 = lngseq;
                    break;
                case NACCS_REPORTTYPE.SAD4021:
                case NACCS_REPORTTYPE.SAD4041:
                case NACCS_REPORTTYPE.SAD4051:
                case NACCS_REPORTTYPE.SAD4061:
                case NACCS_REPORTTYPE.SAD4091:
                case NACCS_REPORTTYPE.SAD4101:
                case NACCS_REPORTTYPE.SAD4111:
                case NACCS_REPORTTYPE.SAD4131:
                    strSql.AppendFormat("  ,SEQ_SAD_OTHER = {0}", lngseq).AppendLine();
                    this.SEQ_SAD_OTHER = lngseq;
                    break;
                case NACCS_REPORTTYPE.SAF0010:
                case NACCS_REPORTTYPE.SAF0021:
                case NACCS_REPORTTYPE.SAF0211:
                case NACCS_REPORTTYPE.SAF0221:
                    strSql.AppendFormat("  ,SEQ_SAF = {0}", lngseq).AppendLine();
                    this.SEQ_SAF = lngseq;
                    break;
                default: break;
            }
            if (!string.IsNullOrWhiteSpace(strErrDetail))
            {
                strSql.AppendFormat("  ,EDI_RESULT = (IIF(EDI_RESULT IS NULL, '{0}', EDI_RESULT + ',' + '{0}'))", strErrDetail).AppendLine();
            }
            else
            {
                strSql.AppendFormat("  ,EDI_RESULT = (IIF(EDI_RESULT IS NULL, '{0}', EDI_RESULT + ',' + '{0}'))", recvtype.ToString()).AppendLine();
            }
            strSql.AppendLine("WHERE 1=1");
            if (this.DATA_SEQ != null)
            {
                strSql.AppendFormat("  AND DATA_SEQ = {0}", this.DATA_SEQ).AppendLine();
            }
            else
            {
                strSql.AppendFormat("  AND TIME_OF_DEPARTURE = convert(datetime, '{0}')", Common.ConvertToDateTimeString(this.TIME_OF_DEPARTURE)).AppendLine();
                strSql.AppendFormat("  AND FLIGHT_ID = '{0}'", this.FLIGHT_ID).AppendLine();
                strSql.AppendFormat("  AND PAX_NO = '{0}'", this.PAX_NO).AppendLine();
                strSql.AppendFormat("  AND EDA_NO = '{0}'", this.EDA_NO).AppendLine();
                strSql.AppendFormat("  AND DELETE_FLG IS NULL").AppendLine();
            }
            return strSql.ToString();
        }
        public string GetSQL_DELETE_SEND()
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("UPDATE T_EDI_SEND SET");

            strSql.AppendFormat("  UPD_STAFF_CD = '{0}'", HPFData.AppUserName).AppendLine();
            strSql.AppendFormat("  ,UPD_DATE = GETDATE()", "");
            strSql.AppendFormat("  ,UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
            strSql.AppendLine("  ,VERSION_NO = VERSION_NO + 1");
            strSql.AppendLine("  ,DELETE_FLG = GETDATE()");

            strSql.AppendLine("WHERE 1=1");
            strSql.AppendFormat("  AND TIME_OF_DEPARTURE = convert(datetime, '{0}')", Common.ConvertToDateTimeString(this.TIME_OF_DEPARTURE)).AppendLine();
            strSql.AppendFormat("  AND FLIGHT_ID = '{0}'", this.FLIGHT_ID).AppendLine();
            strSql.AppendFormat("  AND PAX_NO = '{0}'", this.PAX_NO).AppendLine();
            strSql.AppendFormat("  AND EDA_NO = '{0}'", this.EDA_NO).AppendLine();
            strSql.AppendFormat("  AND DELETE_FLG IS NULL").AppendLine();

            return strSql.ToString();
        }


        public Boolean IsFinFlag
        {
            get
            {

                Boolean ret = false;
                if (this.FLG_RESULT != null)
                {
                    if (this.TORIKOMI_JOTAI == HPFData.TORIKOMI_JOTAI.NACCS_ERROR)
                    {
                        //結果が異常だった場合、待たない
                        ret = true;
                    }
                    else
                    {
                        switch (this.SEND_TYPE)
                        {
                            case NACCS_REPORTTYPE.OTA:
                                if (this.SEQ_4011 != null)
                                {
                                    if (HPFData.ShoriShikibetsuTestFlg)
                                    {
                                        ret = true;
                                    }
                                    else
                                    {
                                        if (this.SEQ_4031 != null ||
                                            this.SEQ_4071 != null ||
                                            this.SEQ_4081 != null ||
                                            this.SEQ_SAF != null)
                                        {
                                            ret = true;
                                        }
                                    }
                                }
                                break;
                            case NACCS_REPORTTYPE.OTA01:
                                if (this.SEQ_SAD_OTHER != null)
                                {
                                    ret = true;
                                }
                                break;
                            case NACCS_REPORTTYPE.OTC:
                                if (this.SEQ_4011 != null)
                                {
                                    if (HPFData.ShoriShikibetsuTestFlg)
                                    {
                                        ret = true;
                                    }
                                    else
                                    {
                                        if (this.SEQ_4031 != null ||
                                            this.SEQ_4071 != null ||
                                            this.SEQ_4081 != null ||
                                            this.SEQ_SAD_OTHER != null ||
                                            this.SEQ_SAF != null)
                                        {
                                            ret = true;
                                        }
                                    }
                                }
                                break;
                            case NACCS_REPORTTYPE.SYG_LIST:
                            case NACCS_REPORTTYPE.SYG_RESULT:
                            default:
                                ret = true;
                                break;
                        }
                    }
                }
                return ret;
            }
        }
    }
    #endregion 


}
