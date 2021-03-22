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
        public Int32 RCV_SEQ { get; set; }
        /// <summary>
        /// 送信タイプ
        /// </summary>
        public NACCS_REPORTTYPE SEND_TYPE { get; set; }
        /// <summary>
        /// 電文引継コード
        /// </summary>
        public string DENBUN_HANDOVER { get; set; }
        /// <summary>
        /// 輸入申告番号
        /// </summary>
        public string YUNYU_SHINKOKU_NO { get; set; }
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

            strSql.AppendLine("SELECT count(*)");
            strSql.AppendLine("From T_EDI_SEND");
            strSql.AppendLine("WHERE 1=1");
            strSql.AppendFormat("  AND TIME_OF_DEPARTURE = convert(datetime, '{0}')", Common.ConvertToDateTimeString(this.TIME_OF_DEPARTURE)).AppendLine();
            strSql.AppendFormat("  AND FLIGHT_ID = '{0}'", this.FLIGHT_ID).AppendLine();
            strSql.AppendFormat("  AND PAX_NO = '{0}'", this.PAX_NO).AppendLine();
            strSql.AppendFormat("  AND EDA_NO = '{0}'", this.EDA_NO).AppendLine();

            //strSql.AppendFormat("  AND RCV_SEQ = {0}", this.RCV_SEQ).AppendLine();
            //if (this.SEND_TYPE != NACCS_REPORTTYPE.NONE)
            //{
            //    strSql.AppendFormat("  AND SEND_TYPE = '{0}'", this.SEND_TYPE.ToString()).AppendLine();
            //}

            return strSql.ToString();
        }
        /// <summary>
        /// 自データからSQL作成DBEdiSend model, 
        /// </summary>
        /// <returns></returns>
        public string GetSQL_INSERT()
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("INSERT INTO T_EDI_SEND (");
            strSql.AppendLine("  TIME_OF_DEPARTURE");
            strSql.AppendLine("  ,FLIGHT_ID");
            strSql.AppendLine("  ,PAX_NO");
            strSql.AppendLine("  ,EDA_NO");
            strSql.AppendLine("  ,RCV_SEQ");
            strSql.AppendLine("  ,SEND_TYPE");
            //strSql.AppendLine("  ,DENBUN_HANDOVER");

            strSql.AppendLine("  ,CRT_STAFF_CD");
            strSql.AppendLine("  ,CRT_DATE");
            strSql.AppendLine("  ,CRT_TANMATU_ID");
            //strSql.AppendLine("  ,UPD_STAFF_CD");
            //strSql.AppendLine("  ,UPD_DATE");
            //strSql.AppendLine("  ,UPD_TANMATU_ID");
            strSql.AppendLine("  ,VERSION_NO");

            strSql.AppendLine(") OUTPUT INSERTED.DATA_SEQ");
            strSql.AppendLine(" VALUES (");
            strSql.AppendFormat("  ,'{0}'", Common.ConvertToDateTimeString(this.TIME_OF_DEPARTURE)).AppendLine();
            strSql.AppendFormat("  ,'{0}'", this.FLIGHT_ID).AppendLine();
            strSql.AppendFormat("  ,'{0}'", this.PAX_NO).AppendLine();
            strSql.AppendFormat("  ,'{0}'", this.EDA_NO).AppendLine();
            strSql.AppendFormat("  ,{0}", this.RCV_SEQ).AppendLine();
            strSql.AppendFormat("  ,'{0}'", this.SEND_TYPE.ToString()).AppendLine();
            //strSql.AppendFormat("  ,'{0}'", strHandover).AppendLine();

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
        /// <summary>
        /// 自データからSQL作成DBEdiSend model, 
        /// </summary>
        /// <returns></returns>
        public string GetSQL_UPDATE_HANDOVER(string strHandover)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("UPDATE T_EDI_SEND SET");
            strSql.AppendFormat("  DENBUN_HANDOVER = '{0}'", strHandover).AppendLine();

            strSql.AppendFormat("  ,UPD_STAFF_CD = {0}", HPFData.AppUserName).AppendLine();
            strSql.AppendFormat("  ,UPD_DATE = GETDATE()", "");
            strSql.AppendFormat("  ,UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
            strSql.AppendLine("  ,VERSION_NO = VERSION_NO + 1");

            strSql.AppendLine("WHERE 1=1");
            strSql.AppendFormat("  AND TIME_OF_DEPARTURE = convert(datetime, '{0}')", Common.ConvertToDateTimeString(this.TIME_OF_DEPARTURE)).AppendLine();
            strSql.AppendFormat("  AND FLIGHT_ID = '{0}'", this.FLIGHT_ID).AppendLine();
            strSql.AppendFormat("  AND PAX_NO = '{0}'", this.PAX_NO).AppendLine();
            strSql.AppendFormat("  AND EDA_NO = '{0}'", this.EDA_NO).AppendLine();
            //strSql.AppendFormat("  AND RCV_SEQ = {0}", this.RCV_SEQ).AppendLine();

            return strSql.ToString();
        }

        public string GetSQL_UPDATE_RECV(NACCS_REPORTTYPE recvtype, long lngseq, string strYunyu = "")
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("UPDATE T_EDI_SEND SET");

            strSql.AppendFormat("  UPD_STAFF_CD = {0}", HPFData.AppUserName).AppendLine();
            strSql.AppendFormat("  ,UPD_DATE = GETDATE()", "");
            strSql.AppendFormat("  ,UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
            strSql.AppendLine("  ,VERSION_NO = VERSION_NO + 1");

            if (string.IsNullOrWhiteSpace(strYunyu))
            {
                strSql.AppendFormat("  ,YUNYU_SHINKOKU_NO = {0}", strYunyu).AppendLine();
            }
            switch (recvtype)
            {
                case NACCS_REPORTTYPE.RESULT:
                    strSql.AppendFormat("  ,FLG_RESULT = {0}", lngseq).AppendLine();
                    break;
                case NACCS_REPORTTYPE.SAD4011:
                    strSql.AppendFormat("  ,SEQ_4011 = {0}", lngseq).AppendLine();
                    break;
                case NACCS_REPORTTYPE.SAD4031:
                    strSql.AppendFormat("  ,SEQ_4031 = {0}", lngseq).AppendLine();
                    break;
                case NACCS_REPORTTYPE.SAD4071:
                    strSql.AppendFormat("  ,SEQ_4071 = {0}", lngseq).AppendLine();
                    break;
                case NACCS_REPORTTYPE.SAD4081:
                    strSql.AppendFormat("  ,SEQ_4081 = {0}", lngseq).AppendLine();
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
                    break;
                case NACCS_REPORTTYPE.SAF0010:
                case NACCS_REPORTTYPE.SAF0021:
                case NACCS_REPORTTYPE.SAF0211:
                case NACCS_REPORTTYPE.SAF0221:
                    strSql.AppendFormat("  ,SEQ_SAF = {0}", lngseq).AppendLine();
                    break;
                default: break;
            }
            strSql.AppendLine("WHERE 1=1");
            strSql.AppendFormat("  AND TIME_OF_DEPARTURE = convert(datetime, '{0}')", Common.ConvertToDateTimeString(this.TIME_OF_DEPARTURE)).AppendLine();
            strSql.AppendFormat("  AND FLIGHT_ID = '{0}'", this.FLIGHT_ID).AppendLine();
            strSql.AppendFormat("  AND PAX_NO = '{0}'", this.PAX_NO).AppendLine();
            strSql.AppendFormat("  AND EDA_NO = '{0}'", this.EDA_NO).AppendLine();
            //strSql.AppendFormat("  AND RCV_SEQ = {0}", this.RCV_SEQ).AppendLine();

            return strSql.ToString();
        }
    }
    #endregion 


}
