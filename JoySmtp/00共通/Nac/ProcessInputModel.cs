using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using JoySmtp.JoyCommon;
using JoySmtp.CLogOut;
using JoySmtp.DataBase;
using JoySmtp.Data;


namespace JoySmtp.Nac
{

    #region 入力処理モデル　OTA

    /// <summary>
    /// 処理要求モデル
    /// </summary>
    public class ProcessInputModel : NaccsSendModel
    {
        public string TIME_OF_DEPARTURE;
        public string FLIGHT_ID;
        public string PAX_NO;
        public string EDA_NO;

        protected DateTime CREATE_DATE;

        ///// <summary>
        ///// 受信確認があった場合TRUE
        ///// </summary>
        //public Dictionary<NACCS_REPORTTYPE, string> GetResponse
        //{
        //    get
        //    {
        //        return this.i_RltResponse;
        //    }
        //}
        //public Dictionary<NACCS_REPORTTYPE,string> i_RltResponse;

        /// <summary>
        /// 電文引継情報（26文字）
        /// PAX番号が10桁以上の場合は考慮が必要
        /// </summary>
        public string DEFAULT_DENBUN_HANDOVER
        {
            get
            {
                return string.Format("{0}:{1}-{2}",
                    Common.ConvertToStringUpper(this.PAX_NO),
                    Common.ConvertToStringUpper(this.EDA_NO),
                    this.CREATE_DATE.ToString("ddHHmmss")
                    );
                //(string.IsNullOrWhiteSpace(this.YUNYU_SHINKOKU_NO.Data))? "1" : "2");
            }
        }
        ///// <summary>
        ///// 電文引継情報からPAXデータの取得
        ///// </summary>
        //public static string GetPaxNoFromDenbunHandover(string strDH)
        //{
        //    if (strDH[strDH.Length - 2] == '-')
        //    {
        //        return strDH.Substring(0, strDH.Length - 2);
        //    }
        //    return strDH;
        //}

        /// <summary>
        /// 入力情報番号
        /// </summary>
        public string DEFAULT_INPUT_INFO_NO
        {
            get { return (string.IsNullOrWhiteSpace(this.YUNYU_SHINKOKU_NO.Data))? "1" : "2"; }
        }

        private const string NACCS_USER_CODE = "NaccsInputUserCode";
        private const string NACCS_USER_PASS = "NaccsInputUserPass";
        private const string NACCS_SHIKIBETSU_NO = "NaccsInputCode";
        
        protected static string i_UserCode = string.Empty;
        protected static string i_ShikibetsuNo = string.Empty;
        protected static string i_UserPass = string.Empty;

        ///// <summary>
        ///// 入力共通項目398
        ///// </summary>
        //public InputCommonModel InputCommon;
        /// <summary>
        /// 輸入申告番号 2
        /// </summary>
        public NacCollumnAN YUNYU_SHINKOKU_NO = new NacCollumnAN(11);
        /// <summary>
        /// 処理識別 3
        /// </summary>
        public NacCollumnAN SHORI_SHIKIBETSU = new NacCollumnAN(1);
        /// <summary>
        /// あて先官署コード 4
        /// </summary>
        public NacCollumnAN ATESAKI_KANSHO_CD = new NacCollumnAN(2);
        /// <summary>
        /// あて先部門コード 5
        /// </summary>
        public NacCollumnAN ATESAKI_BUMON_CD = new NacCollumnAN(2);
        /// <summary>
        /// 旅客者氏名 6
        /// </summary>
        public NacCollumnAN PASSENGER = new NacCollumnAN(70);
        /// <summary>
        /// 郵便番号 7
        /// </summary>
        public NacCollumnAN ZIPCODE = new NacCollumnAN(7);
        /// <summary>
        /// 住所１ 8
        /// </summary>
        public NacCollumnAN ADDRESS1 = new NacCollumnAN(15);
        /// <summary>
        /// 住所２ 9
        /// </summary>
        public NacCollumnAN ADDRESS2 = new NacCollumnAN(35);
        /// <summary>
        /// 住所３ 10
        /// </summary>
        public NacCollumnAN ADDRESS3 = new NacCollumnAN(35);
        /// <summary>
        /// 住所４ 11
        /// </summary>
        public NacCollumnAN ADDRESS4 = new NacCollumnAN(70);
        /// <summary>
        /// 航空会社コード（ICAO） 12
        /// </summary>
        public NacCollumnAN AIRLINE_CD_ICAO = new NacCollumnAN(5);
        /// <summary>
        /// 搭乗便名 13
        /// </summary>
        public NacCollumnNPD FLIGHT_NO = new NacCollumnNPD(5,3);
        /// <summary>
        /// 通関予定貯蔵置場コード 14
        /// </summary>
        public NacCollumnAN TUKANYOTEI_CHOZO_OKIBA_CD = new NacCollumnAN(5);
        /// <summary>
        /// 納付方法識別 15
        /// </summary>
        public NacCollumnAN NOFU_HOHO_SHIKIBETSU = new NacCollumnAN(1);
        /// <summary>
        /// 口座番号 16
        /// </summary>
        public NacCollumnAN KOZA_NO = new NacCollumnAN(14);
        /// <summary>
        /// BP申請事由コード 17
        /// </summary>
        public NacCollumnAN BP_SHINSEI_JIYU = new NacCollumnAN(2);
        /// <summary>
        /// 担保登録番号 18
        /// </summary>
        public NacCollumnAN TANPO_TOROKU_NO = new NacCollumnAN(9);
        /// <summary>
        /// 関税免除済額 19
        /// </summary>
        public NacCollumnN KANZEI_MENJOZUMI_GAKU = new NacCollumnN(9);
        /// <summary>
        /// 記事（税関用） 20
        /// </summary>
        public NacCollumnJ KIJI_KANZEI = new NacCollumnJ(140);
        /// <summary>
        /// 記事（販売店用） 21
        /// </summary>
        public NacCollumnJ KIJI_HANBAITEN = new NacCollumnJ(70);
        /// <summary>
        /// 記事（その他） 22
        /// </summary>
        public NacCollumnJ KIJI_SONOTA = new NacCollumnJ(70);
        /// <summary>
        /// 社内整理番号 23
        /// </summary>
        public NacCollumnAN SHANAI_SEIRI_NO = new NacCollumnAN(20);

        /// <summary>
        /// 商品リスト
        /// </summary>
        public List<ShohinModel> Shohin = new List<ShohinModel>();
                    
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProcessInputModel()
        {
            NaccsCommon = new InputCommonModel();
            ReportType = NACCS_REPORTTYPE.OTA;
            CREATE_DATE = DateTime.Now;

            Shohin = new List<ShohinModel>();
            //ShohinModel sm = new ShohinModel();
            //Shohin.Add(sm);

            i_UserCode = GetAppSetting(NACCS_USER_CODE);
            i_UserPass = GetAppSetting(NACCS_USER_PASS);
            i_ShikibetsuNo = GetAppSetting(NACCS_SHIKIBETSU_NO);

            //this.i_RltResponse = new Dictionary<NACCS_REPORTTYPE, string>();
        }

        public int GetByteLength()
        {
            int intlen = 0;
            intlen += NaccsCommon.GetByteLength();

            intlen += GetByteLengthMain();
            return intlen;
        }
        private int GetByteLengthMain()
        {
            int intlen = 0;
            intlen += YUNYU_SHINKOKU_NO.GetByteLength();
            intlen += SHORI_SHIKIBETSU.GetByteLength();
            intlen += ATESAKI_KANSHO_CD.GetByteLength();
            intlen += ATESAKI_BUMON_CD.GetByteLength();
            intlen += PASSENGER.GetByteLength();
            intlen += ZIPCODE.GetByteLength();
            intlen += ADDRESS1.GetByteLength();
            intlen += ADDRESS2.GetByteLength();
            intlen += ADDRESS3.GetByteLength();
            intlen += ADDRESS4.GetByteLength();
            intlen += AIRLINE_CD_ICAO.GetByteLength();
            intlen += FLIGHT_NO.GetByteLength();
            intlen += TUKANYOTEI_CHOZO_OKIBA_CD.GetByteLength();
            intlen += NOFU_HOHO_SHIKIBETSU.GetByteLength();
            intlen += KOZA_NO.GetByteLength();
            intlen += BP_SHINSEI_JIYU.GetByteLength();
            intlen += TANPO_TOROKU_NO.GetByteLength();
            intlen += KANZEI_MENJOZUMI_GAKU.GetByteLength();
            intlen += KIJI_KANZEI.GetByteLength();
            intlen += KIJI_HANBAITEN.GetByteLength();
            intlen += KIJI_SONOTA.GetByteLength();
            intlen += SHANAI_SEIRI_NO.GetByteLength();

            for (int i = 0; i < Shohin.Count(); i++)
            {
                intlen += Shohin[i].GetByteLength();
            }
            return intlen;
        }

        /// <summary>
        /// 添付用データの作成
        /// </summary>
        /// <returns></returns>
        public override byte[] GetByteData()
        {
            int intLast = this.GetByteLength();
            byte[] btData = new byte[intLast];
            int pos = 0, intLen = 0;

            try
            {
                intLen = this.GetByteLengthMain();
                if (intLen > 0)
                {
                    //intLast += 2;
                    Array.Copy(this.NaccsCommon.GetByteData(intLast), 0, btData, pos, this.NaccsCommon.GetByteLength());
                    pos += this.NaccsCommon.GetByteLength();

                    Array.Copy(this.YUNYU_SHINKOKU_NO.GetByteData(), 0, btData, pos, this.YUNYU_SHINKOKU_NO.GetByteLength());
                    pos += YUNYU_SHINKOKU_NO.GetByteLength();

                    if (HPFData.ShoriShikibetsuTestFlg)// && this.SHORI_SHIKIBETSU.Data!="I")
                    {
                        this.SHORI_SHIKIBETSU.SetData(" ");
                    }
                    Array.Copy(this.SHORI_SHIKIBETSU.GetByteData(), 0, btData, pos, this.SHORI_SHIKIBETSU.GetByteLength());
                    pos += SHORI_SHIKIBETSU.GetByteLength();

                    Array.Copy(this.ATESAKI_KANSHO_CD.GetByteData(), 0, btData, pos, this.ATESAKI_KANSHO_CD.GetByteLength());
                    pos += ATESAKI_KANSHO_CD.GetByteLength();

                    Array.Copy(this.ATESAKI_BUMON_CD.GetByteData(), 0, btData, pos, this.ATESAKI_BUMON_CD.GetByteLength());
                    pos += ATESAKI_BUMON_CD.GetByteLength();

                    Array.Copy(this.PASSENGER.GetByteData(), 0, btData, pos, this.PASSENGER.GetByteLength());
                    pos += PASSENGER.GetByteLength();

                    Array.Copy(this.ZIPCODE.GetByteData(), 0, btData, pos, this.ZIPCODE.GetByteLength());
                    pos += ZIPCODE.GetByteLength();

                    Array.Copy(this.ADDRESS1.GetByteData(), 0, btData, pos, this.ADDRESS1.GetByteLength());
                    pos += ADDRESS1.GetByteLength();

                    Array.Copy(this.ADDRESS2.GetByteData(), 0, btData, pos, this.ADDRESS2.GetByteLength());
                    pos += ADDRESS2.GetByteLength();

                    Array.Copy(this.ADDRESS3.GetByteData(), 0, btData, pos, this.ADDRESS3.GetByteLength());
                    pos += ADDRESS3.GetByteLength();

                    Array.Copy(this.ADDRESS4.GetByteData(), 0, btData, pos, this.ADDRESS4.GetByteLength());
                    pos += ADDRESS4.GetByteLength();

                    Array.Copy(this.AIRLINE_CD_ICAO.GetByteData(), 0, btData, pos, this.AIRLINE_CD_ICAO.GetByteLength());
                    pos += AIRLINE_CD_ICAO.GetByteLength();

                    Array.Copy(this.FLIGHT_NO.GetByteData(), 0, btData, pos, this.FLIGHT_NO.GetByteLength());
                    pos += FLIGHT_NO.GetByteLength();

                    Array.Copy(this.TUKANYOTEI_CHOZO_OKIBA_CD.GetByteData(), 0, btData, pos, this.TUKANYOTEI_CHOZO_OKIBA_CD.GetByteLength());
                    pos += TUKANYOTEI_CHOZO_OKIBA_CD.GetByteLength();

                    Array.Copy(this.NOFU_HOHO_SHIKIBETSU.GetByteData(), 0, btData, pos, this.NOFU_HOHO_SHIKIBETSU.GetByteLength());
                    pos += NOFU_HOHO_SHIKIBETSU.GetByteLength();

                    Array.Copy(this.KOZA_NO.GetByteData(), 0, btData, pos, this.KOZA_NO.GetByteLength());
                    pos += KOZA_NO.GetByteLength();

                    Array.Copy(this.BP_SHINSEI_JIYU.GetByteData(), 0, btData, pos, this.BP_SHINSEI_JIYU.GetByteLength());
                    pos += BP_SHINSEI_JIYU.GetByteLength();

                    Array.Copy(this.TANPO_TOROKU_NO.GetByteData(), 0, btData, pos, this.TANPO_TOROKU_NO.GetByteLength());
                    pos += TANPO_TOROKU_NO.GetByteLength();

                    Array.Copy(this.KANZEI_MENJOZUMI_GAKU.GetByteData(), 0, btData, pos, this.KANZEI_MENJOZUMI_GAKU.GetByteLength());
                    pos += KANZEI_MENJOZUMI_GAKU.GetByteLength();

                    Array.Copy(this.KIJI_KANZEI.GetByteData(), 0, btData, pos, this.KIJI_KANZEI.GetByteLength());
                    pos += KIJI_KANZEI.GetByteLength();

                    Array.Copy(this.KIJI_HANBAITEN.GetByteData(), 0, btData, pos, this.KIJI_HANBAITEN.GetByteLength());
                    pos += KIJI_HANBAITEN.GetByteLength();

                    Array.Copy(this.KIJI_SONOTA.GetByteData(), 0, btData, pos, this.KIJI_SONOTA.GetByteLength());
                    pos += KIJI_SONOTA.GetByteLength();

                    Array.Copy(this.SHANAI_SEIRI_NO.GetByteData(), 0, btData, pos, this.SHANAI_SEIRI_NO.GetByteLength());
                    pos += SHANAI_SEIRI_NO.GetByteLength();

                    for (int i = 0; i < Shohin.Count(); i++)
                    {
                        Array.Copy(this.Shohin[i].GetByteData(), 0, btData, pos, this.Shohin[i].GetByteLength());
                        pos += Shohin[i].GetByteLength();
                    }
                    return btData;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                LogOut.ErrorOut(e.ToString().Substring(0, 50), this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                throw e;
            }
        }
        /// <summary>
        /// DBからデータを取得
        /// </summary>
        /// <returns></returns>
        public static string GetSQL_UNSENTCOUNT(Dictionary<string, string> dicWhere = null)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("SELECT");
            strSql.AppendLine("  H.TIME_OF_DEPARTURE");
            strSql.AppendLine("  ,H.FLIGHT_ID");
            strSql.AppendLine("  ,H.PAX_NO");
            strSql.AppendLine("  ,H.EDA_NO");
            strSql.AppendLine("  ,H.RCV_SEQ");
            strSql.AppendLine("FROM T_PAX_H AS H");

            strSql.AppendLine("WHERE 1=1");
            strSql.AppendLine("  AND H.DELETE_FLG IS NULL");

            strSql.AppendLine("  AND H.EDI_SENDDATE IS NULL");
            strSql.AppendLine("  AND H.RCV_SEQ = 0");
            strSql.AppendLine("  AND H.TORIKOMI_JOTAI = 2");

            strSql.AppendLine("ORDER BY H.TIME_OF_DEPARTURE, H.FLIGHT_ID, H.PAX_NO");

            return strSql.ToString();
        }
        /// <summary>
        /// DBからデータを未送信データを取得２
        /// </summary>
        /// <returns></returns>
        public static string GetSQL_UNSENTCOUNT2(Dictionary<string, string> dicWhere = null)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("SELECT");
            strSql.AppendLine("  H.PAX_NO");
            strSql.AppendLine("  ,H.EDA_NO");
            strSql.AppendLine("FROM T_PAX_H AS H");
            //strSql.AppendLine("LEFT JOIN T_PAX_D AS D");
            //strSql.AppendLine("  ON H.PAX_NO = D.PAX_NO");
            //strSql.AppendLine("  AND H.FLIGHT_ID = D.FLIGHT_ID");
            //strSql.AppendLine("  AND H.TIME_OF_DEPARTURE = D.TIME_OF_DEPARTURE");
            //strSql.AppendLine("  AND D.DELETE_FLG IS NULL");
            strSql.AppendLine("WHERE 1=1");
            strSql.AppendLine("  AND H.DELETE_FLG IS NULL");

            strSql.AppendLine("  AND H.EDI_SENDDATE IS NULL");
            strSql.AppendLine("  AND H.RCV_SEQ = 0");
            strSql.AppendLine("  AND H.TORIKOMI_JOTAI = 2");
            //strSql.AppendLine("  AND (H.EDI_SENDDATE IS NULL OR D.EDI_SENDDATE IS NULL)");
            
            if (dicWhere != null && dicWhere.Count > 0)
            {
                foreach (KeyValuePair<string, string> item in dicWhere)
                {
                    strSql.AppendFormat(" AND {0} = {1}", item.Key, item.Value).AppendLine();
                }
            }
            strSql.AppendLine("GROUP BY H.TIME_OF_DEPARTURE, H.FLIGHT_ID, H.PAX_NO, H.EDA_NO");
            strSql.AppendLine("ORDER BY H.TIME_OF_DEPARTURE, H.FLIGHT_ID, H.PAX_NO, H.EDA_NO");

            return strSql.ToString();
        }
        ///// <summary>
        ///// DBからデータを取得
        ///// </summary>
        ///// <returns></returns>
        //public static string GetSQL_SELECT(Dictionary<string, string> dicWhere = null, string strDTbl = "D", bool blnAll = false)
        //{
        //    string strHTbl = "H";
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.AppendLine("SELECT");
        //    strSql.AppendFormat("  {0}.YUNYU_SHINKOKU_NO as YUNYU_SHINKOKU_NO", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SHORI_SHIKIBETSU as SHORI_SHIKIBETSU", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ATESAKI_KANSHO_CD as ATESAKI_KANSHO_CD", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ATESAKI_BUMON_CD as ATESAKI_BUMON_CD", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.PASSENGER as PASSENGER", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ZIPCODE as ZIPCODE", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ADDRESS1 as ADDRESS1", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ADDRESS2 as ADDRESS2", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ADDRESS3 as ADDRESS3", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ADDRESS4 as ADDRESS4", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.AIRLINE_CD_ICAO as AIRLINE_CD_ICAO", strHTbl).AppendLine();
        //    //strSql.AppendFormat("  ,ISNULL(C_201.KBN_CODE, {0}.AIRLINE_CD_ICAO) as AIRLINE_CD_ICAO", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.FLIGHT_NO as FLIGHT_NO", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.TUKANYOTEI_CHOZO_OKIBA_CD as TUKANYOTEI_CHOZO_OKIBA_CD", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.NOFU_HOHO_SHIKIBETSU as NOFU_HOHO_SHIKIBETSU", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KOZA_NO as KOZA_NO", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.BP_SHINSEI_JIYU as BP_SHINSEI_JIYU", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.TANPO_TOROKU_NO as TANPO_TOROKU_NO", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KANZEI_MENJOZUMI_GAKU as KANZEI_MENJOZUMI_GAKU", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KIJI_KANZEI as KIJI_KANZEI", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KIJI_HANBAITEN as KIJI_HANBAITEN", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KIJI_SONOTA as KIJI_SONOTA", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SHANAI_SEIRI_NO as SHANAI_SEIRI_NO", strHTbl).AppendLine();

        //    strSql.AppendFormat("  ,{0}.TIME_OF_DEPARTURE as TIME_OF_DEPARTURE", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.FLIGHT_ID as FLIGHT_ID", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.PAX_NO as PAX_NO", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.MEISAI_NO as MEISAI_NO", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KANZEI_TAISHO as KANZEI_TAISHO", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SHOHIN_KANRI_CD as SHOHIN_KANRI_CD", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.HINMEI as HINMEI", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.HANBAI_TANKA as HANBAI_TANKA", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.HANBAI_SURYO as HANBAI_SURYO", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.HINMOKU_CD as HINMOKU_CD", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.NACCS_CD as NACCS_CD", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.GENZANCHI_CD as GENZANCHI_CD", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.GENZANCHI_SHOMEI_SHIKIBETSU as GENZANCHI_SHOMEI_SHIKIBETSU", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.INN_TAISHO as INN_TAISHO", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SURYO1 as SURYO1", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SURYO_TANI_CD1 as SURYO_TANI_CD1", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SURYO2 as SURYO2", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SURYO_TANI_CD2 as SURYO_TANI_CD2", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KAZEI_KAKAKU as KAZEI_KAKAKU", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.NAIKOKU_SHOHIZEITOU_SHUBETSU_CD as NAIKOKU_SHOHIZEITOU_SHUBETSU_CD", strDTbl).AppendLine();

        //    strSql.AppendFormat("FROM T_PAX_D AS {0}", strDTbl).AppendLine();

        //    strSql.AppendFormat("LEFT JOIN T_PAX_H AS {0}", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ON {0}.TIME_OF_DEPARTURE = {1}.TIME_OF_DEPARTURE", strDTbl, strHTbl).AppendLine();
        //    strSql.AppendFormat("  AND {0}.FLIGHT_ID = {1}.FLIGHT_ID ", strDTbl, strHTbl).AppendLine();
        //    strSql.AppendFormat("  AND {0}.PAX_NO = {1}.PAX_NO", strDTbl, strHTbl).AppendLine();

        //    strSql.AppendLine("LEFT JOIN C_KYOTHU_KBN AS C_201");
        //    strSql.AppendFormat("  ON C_201.SONOTA1 = {0}.AIRLINE_CD_ICAO", strHTbl).AppendLine();
        //    strSql.AppendLine("  AND C_201.SIKIBETU_KBN = '201'");
            
        //    strSql.AppendLine("WHERE 1=1");
        //    strSql.AppendFormat("AND {0}.DELETE_FLG IS NULL", strHTbl).AppendLine();
        //    strSql.AppendFormat("AND {0}.DELETE_FLG IS NULL", strDTbl).AppendLine();
        //    //★★★排他時間も考慮する？30分とか。。。
        //    strSql.AppendFormat("AND ({0}.HAITA_FLG IS NULL OR {0}.HAITA_FLG <> 1)", strDTbl).AppendLine();
        //    strSql.AppendFormat("AND ({0}.HAITA_FLG IS NULL OR {0}.HAITA_FLG <> 1)", strHTbl).AppendLine();
        //    if (!blnAll)
        //    {
        //        strSql.AppendFormat("  AND ({0}.EDI_SENDDATE IS NULL OR {1}.EDI_SENDDATE IS NULL)", strHTbl, strDTbl).AppendLine();
        //    }
        //    if (dicWhere != null && dicWhere.Count>0)
        //    {
        //        foreach (KeyValuePair<string, string> item in dicWhere)
        //        {
        //            strSql.AppendFormat(" AND {0} = {1}", item.Key, item.Value).AppendLine();
        //        }
        //    }
        //    strSql.AppendLine("ORDER BY D.TIME_OF_DEPARTURE, D.FLIGHT_ID, D.PAX_NO, D.MEISAI_NO");

        //    return strSql.ToString();
        //}

        /// <summary>
        /// DBからデータを取得
        /// </summary>
        /// <returns></returns>
        public static string GetSQL_SELECT(DBEdiSend model)
        {
            string strHTbl = "H";
            string strDTbl = "D";
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("SELECT");
            strSql.AppendFormat("  {0}.YUNYU_SHINKOKU_NO as YUNYU_SHINKOKU_NO", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.SHORI_SHIKIBETSU as SHORI_SHIKIBETSU", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.ATESAKI_KANSHO_CD as ATESAKI_KANSHO_CD", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.ATESAKI_BUMON_CD as ATESAKI_BUMON_CD", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.PASSENGER as PASSENGER", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.ZIPCODE as ZIPCODE", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.ADDRESS1 as ADDRESS1", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.ADDRESS2 as ADDRESS2", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.ADDRESS3 as ADDRESS3", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.ADDRESS4 as ADDRESS4", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.AIRLINE_CD_ICAO as AIRLINE_CD_ICAO", strHTbl).AppendLine();
            //strSql.AppendFormat("  ,ISNULL(C_201.KBN_CODE, {0}.AIRLINE_CD_ICAO) as AIRLINE_CD_ICAO", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.FLIGHT_NO as FLIGHT_NO", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.TUKANYOTEI_CHOZO_OKIBA_CD as TUKANYOTEI_CHOZO_OKIBA_CD", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.NOFU_HOHO_SHIKIBETSU as NOFU_HOHO_SHIKIBETSU", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.KOZA_NO as KOZA_NO", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.BP_SHINSEI_JIYU as BP_SHINSEI_JIYU", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.TANPO_TOROKU_NO as TANPO_TOROKU_NO", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.KANZEI_MENJOZUMI_GAKU as KANZEI_MENJOZUMI_GAKU", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.KIJI_KANZEI as KIJI_KANZEI", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.KIJI_HANBAITEN as KIJI_HANBAITEN", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.KIJI_SONOTA as KIJI_SONOTA", strHTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.SHANAI_SEIRI_NO as SHANAI_SEIRI_NO", strHTbl).AppendLine();

            strSql.AppendFormat("  ,{0}.TIME_OF_DEPARTURE as TIME_OF_DEPARTURE", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.FLIGHT_ID as FLIGHT_ID", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.PAX_NO as PAX_NO", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.EDA_NO as EDA_NO", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.MEISAI_NO as MEISAI_NO", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.KANZEI_TAISHO as KANZEI_TAISHO", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.SHOHIN_KANRI_CD as SHOHIN_KANRI_CD", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.HINMEI as HINMEI", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.HANBAI_TANKA as HANBAI_TANKA", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.HANBAI_SURYO as HANBAI_SURYO", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.HINMOKU_CD as HINMOKU_CD", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.NACCS_CD as NACCS_CD", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.GENZANCHI_CD as GENZANCHI_CD", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.GENZANCHI_SHOMEI_SHIKIBETSU as GENZANCHI_SHOMEI_SHIKIBETSU", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.INN_TAISHO as INN_TAISHO", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.SURYO1 as SURYO1", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.SURYO_TANI_CD1 as SURYO_TANI_CD1", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.SURYO2 as SURYO2", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.SURYO_TANI_CD2 as SURYO_TANI_CD2", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.KAZEI_KAKAKU as KAZEI_KAKAKU", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.NAIKOKU_SHOHIZEITOU_SHUBETSU_CD as NAIKOKU_SHOHIZEITOU_SHUBETSU_CD", strDTbl).AppendLine();
            strSql.AppendFormat("  ,{0}.NAIKOKU_SHOHIZEITOU_SHUBETSU_CD2 as NAIKOKU_SHOHIZEITOU_SHUBETSU_CD2", strDTbl).AppendLine();

            strSql.AppendFormat("FROM T_PAX_D AS {0}", strDTbl).AppendLine();

            strSql.AppendFormat("LEFT JOIN T_PAX_H AS {0}", strHTbl).AppendLine();
            strSql.AppendFormat("  ON {0}.PAX_NO = {1}.PAX_NO", strDTbl, strHTbl).AppendLine();
            strSql.AppendFormat("  AND {0}.EDA_NO = {1}.EDA_NO", strDTbl, strHTbl).AppendLine();

            //strSql.AppendFormat("  AND {0}.TIME_OF_DEPARTURE = {1}.TIME_OF_DEPARTURE", strDTbl, strHTbl).AppendLine();
            //strSql.AppendFormat("  AND {0}.FLIGHT_ID = {1}.FLIGHT_ID ", strDTbl, strHTbl).AppendLine();

            strSql.AppendLine("LEFT JOIN C_KYOTHU_KBN AS C_201");
            strSql.AppendFormat("  ON C_201.SONOTA1 = {0}.AIRLINE_CD_ICAO", strHTbl).AppendLine();
            strSql.AppendLine("  AND C_201.SIKIBETU_KBN = '201'");

            strSql.AppendLine("WHERE 1=1");
            strSql.AppendFormat("AND {0}.DELETE_FLG IS NULL", strDTbl).AppendLine();
            strSql.AppendFormat("AND {0}.TIME_OF_DEPARTURE = '{1}'", strDTbl, Common.ConvertToDateTimeString(model.TIME_OF_DEPARTURE)).AppendLine();
            strSql.AppendFormat("AND {0}.FLIGHT_ID = '{1}'", strDTbl, model.FLIGHT_ID).AppendLine();
            strSql.AppendFormat("AND {0}.PAX_NO = '{1}'", strDTbl, model.PAX_NO).AppendLine();
            strSql.AppendFormat("AND {0}.EDA_NO = '{1}'", strDTbl, model.EDA_NO).AppendLine();
            strSql.AppendFormat("AND {0}.RCV_SEQ = '{1}'", strDTbl, model.RCV_SEQ).AppendLine();

            strSql.AppendLine("ORDER BY D.TIME_OF_DEPARTURE, D.FLIGHT_ID, D.PAX_NO, D.EDA_NO, D.MEISAI_NO");

            return strSql.ToString();
        }

        ///// <summary>
        ///// DBからデータを取得
        ///// </summary>
        ///// <returns></returns>
        //public static string GetSQL_SELECT(string strPaxNo, string strEdaNo)
        //{
        //    string strHTbl = "H";
        //    string strDTbl = "D";
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.AppendLine("SELECT");
        //    strSql.AppendFormat("  {0}.YUNYU_SHINKOKU_NO as YUNYU_SHINKOKU_NO", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SHORI_SHIKIBETSU as SHORI_SHIKIBETSU", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ATESAKI_KANSHO_CD as ATESAKI_KANSHO_CD", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ATESAKI_BUMON_CD as ATESAKI_BUMON_CD", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.PASSENGER as PASSENGER", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ZIPCODE as ZIPCODE", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ADDRESS1 as ADDRESS1", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ADDRESS2 as ADDRESS2", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ADDRESS3 as ADDRESS3", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ADDRESS4 as ADDRESS4", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.AIRLINE_CD_ICAO as AIRLINE_CD_ICAO", strHTbl).AppendLine();
        //    //strSql.AppendFormat("  ,ISNULL(C_201.KBN_CODE, {0}.AIRLINE_CD_ICAO) as AIRLINE_CD_ICAO", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.FLIGHT_NO as FLIGHT_NO", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.TUKANYOTEI_CHOZO_OKIBA_CD as TUKANYOTEI_CHOZO_OKIBA_CD", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.NOFU_HOHO_SHIKIBETSU as NOFU_HOHO_SHIKIBETSU", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KOZA_NO as KOZA_NO", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.BP_SHINSEI_JIYU as BP_SHINSEI_JIYU", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.TANPO_TOROKU_NO as TANPO_TOROKU_NO", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KANZEI_MENJOZUMI_GAKU as KANZEI_MENJOZUMI_GAKU", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KIJI_KANZEI as KIJI_KANZEI", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KIJI_HANBAITEN as KIJI_HANBAITEN", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KIJI_SONOTA as KIJI_SONOTA", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SHANAI_SEIRI_NO as SHANAI_SEIRI_NO", strHTbl).AppendLine();

        //    strSql.AppendFormat("  ,{0}.TIME_OF_DEPARTURE as TIME_OF_DEPARTURE", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.FLIGHT_ID as FLIGHT_ID", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.PAX_NO as PAX_NO", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.EDA_NO as EDA_NO", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.MEISAI_NO as MEISAI_NO", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KANZEI_TAISHO as KANZEI_TAISHO", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SHOHIN_KANRI_CD as SHOHIN_KANRI_CD", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.HINMEI as HINMEI", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.HANBAI_TANKA as HANBAI_TANKA", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.HANBAI_SURYO as HANBAI_SURYO", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.HINMOKU_CD as HINMOKU_CD", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.NACCS_CD as NACCS_CD", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.GENZANCHI_CD as GENZANCHI_CD", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.GENZANCHI_SHOMEI_SHIKIBETSU as GENZANCHI_SHOMEI_SHIKIBETSU", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.INN_TAISHO as INN_TAISHO", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SURYO1 as SURYO1", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SURYO_TANI_CD1 as SURYO_TANI_CD1", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SURYO2 as SURYO2", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SURYO_TANI_CD2 as SURYO_TANI_CD2", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KAZEI_KAKAKU as KAZEI_KAKAKU", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.NAIKOKU_SHOHIZEITOU_SHUBETSU_CD as NAIKOKU_SHOHIZEITOU_SHUBETSU_CD", strDTbl).AppendLine();

        //    strSql.AppendFormat("FROM T_PAX_D AS {0}", strDTbl).AppendLine();

        //    strSql.AppendFormat("LEFT JOIN T_PAX_H AS {0}", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ON {0}.PAX_NO = {1}.PAX_NO", strDTbl, strHTbl).AppendLine();
        //    strSql.AppendFormat("  AND {0}.EDA_NO = {1}.EDA_NO", strDTbl, strHTbl).AppendLine();

        //    //strSql.AppendFormat("  AND {0}.TIME_OF_DEPARTURE = {1}.TIME_OF_DEPARTURE", strDTbl, strHTbl).AppendLine();
        //    //strSql.AppendFormat("  AND {0}.FLIGHT_ID = {1}.FLIGHT_ID ", strDTbl, strHTbl).AppendLine();

        //    strSql.AppendLine("LEFT JOIN C_KYOTHU_KBN AS C_201");
        //    strSql.AppendFormat("  ON C_201.SONOTA1 = {0}.AIRLINE_CD_ICAO", strHTbl).AppendLine();
        //    strSql.AppendLine("  AND C_201.SIKIBETU_KBN = '201'");

        //    strSql.AppendLine("WHERE 1=1");
        //    if (!string.IsNullOrEmpty(strPaxNo))
        //    {
        //        strSql.AppendFormat("AND {0}.DELETE_FLG IS NULL", strDTbl).AppendLine(); 
        //        strSql.AppendFormat("AND {0}.PAX_NO = '{1}'", strDTbl, strPaxNo).AppendLine();
        //        if (!string.IsNullOrEmpty(strEdaNo))
        //        {
        //            strSql.AppendFormat("AND {0}.EDA_NO = '{1}'", strDTbl, strEdaNo).AppendLine();
        //        }
        //    }
        //    else
        //    {
        //        strSql.AppendFormat("AND {0}.DELETE_FLG IS NULL", strHTbl).AppendLine();
        //        strSql.AppendFormat("AND {0}.DELETE_FLG IS NULL", strDTbl).AppendLine();
        //        ////★★★排他時間も考慮する？30分とか。。。
        //        //strSql.AppendFormat("AND ({0}.HAITA_FLG IS NULL OR {0}.HAITA_FLG <> 1)", strDTbl).AppendLine();
        //        //strSql.AppendFormat("AND ({0}.HAITA_FLG IS NULL OR {0}.HAITA_FLG <> 1)", strHTbl).AppendLine();
        //        strSql.AppendFormat("AND ({0}.EDI_SENDDATE IS NULL OR {1}.EDI_SENDDATE IS NULL)", strHTbl, strDTbl).AppendLine();
        //    }
        //    strSql.AppendLine("ORDER BY D.TIME_OF_DEPARTURE, D.FLIGHT_ID, D.PAX_NO, D.EDA_NO, D.MEISAI_NO");

        //    return strSql.ToString();
        //}
        ///// <summary>
        ///// DBからデータを取得
        ///// </summary>
        ///// <returns></returns>
        //public static string GetSQL_SELECT_TOP()
        //{
        //    string strHTbl = "H";
        //    string strDTbl = "D";
        //    StringBuilder strSql = new StringBuilder();

        //    strSql.AppendLine("SELECT ");
        //    strSql.AppendFormat("  {0}.YUNYU_SHINKOKU_NO as YUNYU_SHINKOKU_NO", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SHORI_SHIKIBETSU as SHORI_SHIKIBETSU", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ATESAKI_KANSHO_CD as ATESAKI_KANSHO_CD", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ATESAKI_BUMON_CD as ATESAKI_BUMON_CD", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.PASSENGER as PASSENGER", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ZIPCODE as ZIPCODE", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ADDRESS1 as ADDRESS1", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ADDRESS2 as ADDRESS2", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ADDRESS3 as ADDRESS3", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.ADDRESS4 as ADDRESS4", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.AIRLINE_CD_ICAO as AIRLINE_CD_ICAO", strHTbl).AppendLine();
        //    //strSql.AppendFormat("  ,ISNULL(C_201.KBN_CODE, {0}.AIRLINE_CD_ICAO) as AIRLINE_CD_ICAO", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.FLIGHT_NO as FLIGHT_NO", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.TUKANYOTEI_CHOZO_OKIBA_CD as TUKANYOTEI_CHOZO_OKIBA_CD", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.NOFU_HOHO_SHIKIBETSU as NOFU_HOHO_SHIKIBETSU", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KOZA_NO as KOZA_NO", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.BP_SHINSEI_JIYU as BP_SHINSEI_JIYU", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.TANPO_TOROKU_NO as TANPO_TOROKU_NO", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KANZEI_MENJOZUMI_GAKU as KANZEI_MENJOZUMI_GAKU", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KIJI_KANZEI as KIJI_KANZEI", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KIJI_HANBAITEN as KIJI_HANBAITEN", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KIJI_SONOTA as KIJI_SONOTA", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SHANAI_SEIRI_NO as SHANAI_SEIRI_NO", strHTbl).AppendLine();

        //    strSql.AppendFormat("  ,{0}.TIME_OF_DEPARTURE as TIME_OF_DEPARTURE", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.FLIGHT_ID as FLIGHT_ID", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.PAX_NO as PAX_NO", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.MEISAI_NO as MEISAI_NO", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KANZEI_TAISHO as KANZEI_TAISHO", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SHOHIN_KANRI_CD as SHOHIN_KANRI_CD", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.HINMEI as HINMEI", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.HANBAI_TANKA as HANBAI_TANKA", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.HANBAI_SURYO as HANBAI_SURYO", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.HINMOKU_CD as HINMOKU_CD", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.NACCS_CD as NACCS_CD", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.GENZANCHI_CD as GENZANCHI_CD", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.GENZANCHI_SHOMEI_SHIKIBETSU as GENZANCHI_SHOMEI_SHIKIBETSU", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.INN_TAISHO as INN_TAISHO", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SURYO1 as SURYO1", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SURYO_TANI_CD1 as SURYO_TANI_CD1", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SURYO2 as SURYO2", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.SURYO_TANI_CD2 as SURYO_TANI_CD2", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.KAZEI_KAKAKU as KAZEI_KAKAKU", strDTbl).AppendLine();
        //    strSql.AppendFormat("  ,{0}.NAIKOKU_SHOHIZEITOU_SHUBETSU_CD as NAIKOKU_SHOHIZEITOU_SHUBETSU_CD", strDTbl).AppendLine();

        //    strSql.AppendFormat("FROM T_PAX_D AS {0}", strDTbl).AppendLine();

        //    strSql.AppendFormat("LEFT JOIN T_PAX_H AS {0}", strHTbl).AppendLine();
        //    strSql.AppendFormat("  ON {0}.TIME_OF_DEPARTURE = {1}.TIME_OF_DEPARTURE", strDTbl, strHTbl).AppendLine();
        //    strSql.AppendFormat("  AND {0}.FLIGHT_ID = {1}.FLIGHT_ID ", strDTbl, strHTbl).AppendLine();
        //    strSql.AppendFormat("  AND {0}.PAX_NO = {1}.PAX_NO", strDTbl, strHTbl).AppendLine();

        //    strSql.AppendLine("LEFT JOIN C_KYOTHU_KBN AS C_201");
        //    strSql.AppendFormat("  ON C_201.SONOTA1 = {0}.AIRLINE_CD_ICAO", strHTbl).AppendLine();
        //    strSql.AppendLine("  AND C_201.SIKIBETU_KBN = '201'");

        //    strSql.AppendLine("WHERE 1=1");
        //    strSql.AppendLine("  AND PAX_NO = ( SELECT TOP 1 PAX_NO FROM T_PAX_H WHERE 1=1 ");
        //    strSql.AppendLine("    AND DELETE_FLG IS NULL");
        //    //★★★排他時間も考慮する？30分とか。。。
        //    strSql.AppendLine("    AND HAITA_FLG <> 1");
        //    strSql.AppendLine("    AND EDI_SENDDATE IS NULL");
        //    strSql.AppendLine("    ORDER BY UPD_DATE, CRT_DATE )");

        //    strSql.AppendLine("ORDER BY D.MEISAI_NO");

        //    return strSql.ToString();
        //}
        /// <summary>
        /// EDIの送信済みフラグを立てるH①
        /// </summary>
        /// <returns></returns>
        public string GetSQL_UPDATE_SEND_H()
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("UPDATE T_PAX_H SET");
            strSql.AppendFormat("  EDI_SENDDATE = GETDATE()", "").AppendLine();
            
            //状態を【4:許可待ち】にする suzuki st
            strSql.AppendFormat("  ,TIME_OF_IMPORT_DECLARATION = GETDATE()", "").AppendLine();
            //strSql.AppendFormat("  ,TORIKOMI_JOTAI = '{0}'", HPFData.TORIKOMI_JOTAI.KYOKA_MACHI).AppendLine();
            strSql.AppendFormat("  ,TORIKOMI_JOTAI = (CASE WHEN TORIKOMI_JOTAI > '{0}' THEN TORIKOMI_JOTAI ELSE '{0}' END)", HPFData.TORIKOMI_JOTAI.KYOKA_MACHI).AppendLine();
            //状態を【4:許可待ち】にする suzuki ed

            //再送信時に初期化する？
            strSql.AppendFormat("  ,ERROR_NAIYO = NULL", "").AppendLine();
            strSql.AppendFormat("  ,EDI_RESULT = NULL", "").AppendLine();
            strSql.AppendFormat("  ,EDI_RESULT_SEQ = NULL", "").AppendLine();
            strSql.AppendFormat("  ,EDI_INFO_CD = '{0}'", this.GetHandover).AppendLine();

            strSql.AppendFormat("  ,UPD_STAFF_CD = '{0}'", HPFData.AppUserName).AppendLine();
            strSql.AppendFormat("  ,UPD_DATE = GETDATE()", "").AppendLine();
            strSql.AppendFormat("  ,UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
            strSql.AppendFormat("  ,VERSION_NO = VERSION_NO + 1", "").AppendLine();

            strSql.AppendLine("WHERE 1=1");
            strSql.AppendFormat("  AND EDI_SENDDATE IS NULL").AppendLine();
            strSql.AppendFormat("  AND TIME_OF_DEPARTURE = CONVERT(DATETIME, '{0}')", this.TIME_OF_DEPARTURE).AppendLine();
            strSql.AppendFormat("  AND FLIGHT_ID = '{0}'", this.FLIGHT_ID).AppendLine();
            strSql.AppendFormat("  AND PAX_NO = '{0}'", this.PAX_NO).AppendLine();
            strSql.AppendFormat("  AND EDA_NO = '{0}'", this.EDA_NO).AppendLine();
            strSql.AppendFormat("  AND DELETE_FLG IS NULL").AppendLine();

            return strSql.ToString();
        }
        /// <summary>
        /// EDIの送信済みフラグを立てるH①
        /// </summary>
        /// <returns></returns>
        public static string GetSQL_UPDATE_SEND_H_ERR(string strPaxNo, string strEdaNo, string strErr, string strErrDetail)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("UPDATE T_PAX_H SET");
            strSql.AppendFormat("  EDI_SENDDATE = GETDATE()", "").AppendLine();

            //状態を【5:NACCSエラー】にする suzuki st
            strSql.AppendFormat("  ,TIME_OF_IMPORT_DECLARATION = GETDATE()", "").AppendLine();
            strSql.AppendFormat("  ,TORIKOMI_JOTAI = '{0}'", HPFData.TORIKOMI_JOTAI.NACCS_ERROR).AppendLine();
            //状態を【5:NACCSエラー】にする suzuki ed

            strSql.AppendFormat("  ,ERROR_NAIYO = '{0}'",strErr).AppendLine();
            strSql.AppendFormat("  ,EDI_RESULT = '{0}'", strErrDetail).AppendLine();

            strSql.AppendFormat("  ,EDI_RESULT_SEQ = NULL", "").AppendLine();
            strSql.AppendFormat("  ,EDI_INFO_CD = NULL", "").AppendLine();

            strSql.AppendFormat("  ,UPD_STAFF_CD = '{0}'", HPFData.AppUserName).AppendLine();
            strSql.AppendFormat("  ,UPD_DATE = GETDATE()", "").AppendLine();
            strSql.AppendFormat("  ,UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
            strSql.AppendFormat("  ,VERSION_NO = VERSION_NO + 1", "").AppendLine();

            strSql.AppendLine("WHERE 1=1");
            //strSql.AppendFormat("  AND EDI_SENDDATE IS NULL").AppendLine();
            strSql.AppendFormat("  AND PAX_NO = '{0}'", strPaxNo).AppendLine();
            strSql.AppendFormat("  AND EDA_NO = '{0}'", strEdaNo).AppendLine();
            strSql.AppendFormat("  AND DELETE_FLG IS NULL").AppendLine();

            return strSql.ToString();
        }
        /// <summary>
        /// EDIの送信済みフラグを立てるD
        /// </summary>
        /// <returns></returns>
        public string GetSQL_UPDATE_SEND_D()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("UPDATE T_PAX_D");
            strSql.AppendLine("  SET EDI_SENDDATE = GETDATE()");
            strSql.AppendFormat("  ,UPD_STAFF_CD = '{0}'", HPFData.AppUserName).AppendLine();
            strSql.AppendFormat("  ,UPD_DATE = GETDATE()", "").AppendLine();
            strSql.AppendFormat("  ,UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
            strSql.AppendFormat("  ,VERSION_NO = VERSION_NO + 1", "").AppendLine();

            strSql.AppendLine("WHERE 1=1");
            strSql.AppendFormat("  AND TIME_OF_DEPARTURE = CONVERT(DATETIME, '{0}')", this.TIME_OF_DEPARTURE).AppendLine();
            strSql.AppendFormat("  AND FLIGHT_ID = '{0}'", this.FLIGHT_ID).AppendLine();
            strSql.AppendFormat("  AND PAX_NO = '{0}'", this.PAX_NO).AppendLine();
            strSql.AppendFormat("  AND EDA_NO = '{0}'", this.EDA_NO).AppendLine();
            strSql.AppendFormat("  AND DELETE_FLG IS NULL").AppendLine();

            return strSql.ToString();
        }
        /// <summary>
        /// 自データからSQL作成
        /// </summary>
        /// <returns></returns>
        public string GetSQL_UPDATE_RESULT(string strPaxNo, string strEdaNo, string strErrnaiyo = "", string strEdiResult="")
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("UPDATE T_PAX_H SET");
            strSql.AppendFormat("  UPD_STAFF_CD = '{0}'", HPFData.AppUserName).AppendLine();
            strSql.AppendFormat("  ,UPD_DATE = GETDATE()", "").AppendLine();
            strSql.AppendFormat("  ,UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
            strSql.AppendFormat("  ,VERSION_NO = VERSION_NO + 1", "").AppendLine();
            if (strErrnaiyo != "")
            {
                //状態を【5:NACCSエラー】にする suzuki st
                strSql.AppendFormat("  ,TORIKOMI_JOTAI = '{0}'", HPFData.TORIKOMI_JOTAI.NACCS_ERROR).AppendLine();
                strSql.AppendFormat("  ,ERROR_NAIYO = '{0}'", strErrnaiyo).AppendLine();
                //状態を【5:NACCSエラー】にする suzuki ed
            }
            if (strEdiResult != "")
            {
                //strSql.AppendFormat("  ,EDI_RESULT = '{0}'", strEdiResult).AppendLine();
                strSql.AppendFormat("  ,EDI_RESULT = (IIF(EDI_RESULT IS NULL, '{0}', EDI_RESULT + ',' + '{0}'))", strEdiResult).AppendLine();
            }

            strSql.AppendLine("WHERE 1=1");
            strSql.AppendFormat("  AND PAX_NO = '{0}'", strPaxNo).AppendLine();
            strSql.AppendFormat("  AND EDA_NO = '{0}'", strEdaNo).AppendLine();

            return strSql.ToString();
        }
        ///// <summary>
        ///// DBからデータを取得
        ///// </summary>
        ///// <param name="dicWhere">new Dictionary<string,string>{ { "TIME_OF_DEPARTURE",""},{ "FLIGHT_ID",""}, {"PAX_NO", ""}};</param>
        ///// <returns></returns>
        //public string GetSQL_UPDATE_RECV_H(string strYunyuNo, string strInfoCd, string strRlt, long lngSeq = 0 )
        //{
        //    return ProcessInputModel.GetSQL_UPDATE_RECV_T_PAX_H(
        //        Convert.ToDateTime(this.TIME_OF_DEPARTURE), this.FLIGHT_ID, this.PAX_NO,
        //        strYunyuNo, strInfoCd, strRlt, lngSeq);

        //    //StringBuilder strSql = new StringBuilder();

        //    //strSql.AppendLine("UPDATE T_PAX_H SET");

        //    //strSql.AppendFormat("  YUNYU_SHINKOKU_NO = '{0}'", strYunyuNo).AppendLine();
        //    //strSql.AppendFormat("  ,EDI_RESULT = '{0}'", strRlt).AppendLine ();
        //    //if (lngSeq != 0)
        //    //{
        //    //    strSql.AppendFormat("  ,EDI_RESULT_SEQ = {0}", lngSeq).AppendLine();
        //    //}
        //    //strSql.AppendFormat("  ,EDI_INFO_CD = '{0}'", strInfoCd).AppendLine();
        //    //strSql.AppendFormat("  ,UPD_STAFF_CD = '{0}'", HPFData.AppUserName ).AppendLine();
        //    //strSql.AppendFormat("  ,UPD_DATE = GETDATE()", "").AppendLine();
        //    //strSql.AppendFormat("  ,UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
        //    //strSql.AppendFormat("  ,VERSION_NO = VERSION_NO + 1", "").AppendLine();

        //    //strSql.AppendLine("WHERE 1=1");
        //    ////strSql.AppendFormat("  AND EDI_SENDDATE IS NULL").AppendLine();
        //    //strSql.AppendFormat("  AND TIME_OF_DEPARTURE = CONVERT(DATETIME, '{0}')", this.TIME_OF_DEPARTURE).AppendLine();
        //    //strSql.AppendFormat("  AND FLIGHT_ID = '{0}'", this.FLIGHT_ID).AppendLine();
        //    //strSql.AppendFormat("  AND PAX_NO = '{0}'", this.PAX_NO).AppendLine();
        //    //strSql.AppendFormat("  AND DELETE_FLG IS NULL").AppendLine();

        //    //return strSql.ToString();
        //}


        ///// <summary>
        ///// 受信したデータ結果をUPDATEする
        ///// </summary>
        ///// <param name="strYunyuNo">輸入申告番号</param>
        ///// <param name="strInfoCd">入力情報</param>
        ///// <param name="strRlt">結果文字列</param>
        ///// <param name="lngSeq">結果DBに登録したSEQ番号</param>
        ///// <returns></returns>
        //public Boolean SetRecvDataToDB(string strYunyuNo, string strInfoCd, string strRlt, long lngSeq = 0)
        //{
        //    return SetRecvDataToDB( Convert.ToDateTime(this.TIME_OF_DEPARTURE), this.FLIGHT_ID, this.PAX_NO,
        //                            strYunyuNo, strInfoCd, strRlt, lngSeq);
        //}

        ///// <summary>
        ///// 受信したデータ結果をUPDATEする
        ///// </summary>
        ///// <param name="dtDep">出発時刻</param>
        ///// <param name="strFlightID">フライトCD</param>
        ///// <param name="strPaxNo">PAX番号</param>
        ///// <param name="strYunyuNo">輸入申告番号</param>
        ///// <param name="strInfoCd">入力情報</param>
        ///// <param name="strRlt">結果文字列</param>
        ///// <param name="lngSeq">結果DBに登録したSEQ番号</param>
        ///// <returns></returns>        
        //public Boolean SetRecvDataToDB(DateTime dtDep, string strFlightID, string strPaxNo,
        //    //string strYunyuNo, string strInfoCd,
        //    string strRlt, long lngSeq = 0)
        //{
        //    DataBaseSQL objDb = null;
        //    try
        //    {
        //        objDb = new DataBaseSQL();
        //        objDb.DBLogIn();
                
        //        objDb.BeginTransaction();

        //        objDb.ExecuteNonQuery(
        //            GetSQL_UPDATE_RECV_T_PAX_H( dtDep, strFlightID, strPaxNo,
        //        //strYunyuNo, strInfoCd,
        //        strRlt, lngSeq), null);

        //        objDb.Commit();
                
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        if (objDb != null)
        //        {
        //            objDb.RollBack();
        //        }
        //        LogOut.ErrorOut(e.Message, MethodBase.GetCurrentMethod().Name);
        //        return false;
        //    }
        //    finally
        //    {
        //        if (objDb != null)
        //        {
        //            objDb.DBLogOut();
        //        }
        //    }
        //}
        /// <summary>
        /// 受信したデータ結果をUPDATEする
        /// </summary>
        /// <param name="strDenbunHandover">電文引継</param>
        /// <param name="strRlt">結果文字列</param>
        /// <param name="lngSeq">結果DBに登録したSEQ番号</param>
        /// <returns></returns>        
        public static Boolean SetRecvDataToDB(string strDenbunHandover, string strRlt, long lngSeq = 0, string strYunyuNo = "")
        {
            DataBaseSQL objDb = null;
            try
            {
                objDb = new DataBaseSQL();
                objDb.DBLogIn();
                int intRet = 0;

                objDb.BeginTransaction();

                objDb.ExecuteNonQuery(
                    GetSQL_UPDATE_RECV_T_PAX_H(strDenbunHandover, strRlt, lngSeq, strYunyuNo), null, out intRet);
                if (intRet == 0)
                {
                    string strtype = "T_PAX_Hに該当データなし";
                    string str = string.Format("{0} (lngSeq:{1} strYunyuNo:{2} strDenbunHandover:{3} strRlt{4} ",strtype, lngSeq, strYunyuNo, strDenbunHandover, strRlt);
                    LogOut.ErrorOut(str, "ProcessInputModel", MethodBase.GetCurrentMethod().Name, true, strtype);
                }
                objDb.Commit();

                return true;
            }
            catch (Exception e)
            {
                if (objDb != null)
                {
                    objDb.RollBack();
                }
                LogOut.ErrorOut(e.Message, "ProcessInputModel", MethodBase.GetCurrentMethod().Name);
                return false;
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
        /// 受信したデータ結果をUPDATEする
        /// </summary>
        /// <param name="recv">SAD401</param>
        /// <returns></returns>        
        public static Boolean SetRecvDataToDB(ProcessSad401Model recv)
        {
            DataBaseSQL objDb = null;
            try
            {
                objDb = new DataBaseSQL();
                objDb.DBLogIn();
                int intRet = 0;

                objDb.BeginTransaction();

                objDb.ExecuteNonQuery(
                    GetSQL_UPDATE_YUNYU_NO_T_PAX_H(recv.NaccsCommon.DENBUN_HANDOVER.Data, recv.GetYunyuShinkokuNo, recv.GetReportType), null, out intRet);
                if (intRet == 0)
                {
                    string strtype = "T_PAX_Hに該当データなし";
                    string str = string.Format("{0}(YunyuShinkokuNo:{1} DENBUN_HANDOVER:{2} ReportType:{3}",strtype, recv.GetYunyuShinkokuNo, recv.NaccsCommon.DENBUN_HANDOVER.Data, recv.GetReportType.ToString());
                    LogOut.ErrorOut(str, "ProcessInputModel", MethodBase.GetCurrentMethod().Name, true, strtype);
                }
                objDb.Commit();

                return true;
            }
            catch (Exception e)
            {
                if (objDb != null)
                {
                    objDb.RollBack();
                }
                LogOut.ErrorOut(e.Message, "ProcessInputModel", MethodBase.GetCurrentMethod().Name);
                return false;
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
        /// 受信結果保存
        /// </summary>
        /// <param name="strYunyuNo">更新したいレコードの輸入申告番号</param>
        /// <param name="type">レポートタイプ</param>
        /// <returns></returns>
        public static Boolean SetResultRecvDataToDB(string strYunyuNo,NACCS_REPORTTYPE type)
        {
            DataBaseSQL objDb = null;
            try
            {
                objDb = new DataBaseSQL();
                objDb.DBLogIn();
                int intRet = 0;

                objDb.BeginTransaction();

                objDb.ExecuteNonQuery(
                    GetSQL_UPDATE_ADD_RESULT_T_PAX_H(strYunyuNo, type), null, out intRet);
                if (intRet == 0)
                {
                    string strtype = "T_PAX_Hに該当データなし";
                    string str = string.Format("{0}(strYunyuNo:{1} type:{2})", strtype, strYunyuNo, type.ToString() );
                    LogOut.ErrorOut(str, "ProcessInputModel", MethodBase.GetCurrentMethod().Name, false, strType: strtype);
                    objDb.RollBack();
                    return false;
                }

                objDb.Commit();
                return true;
            }
            catch (Exception e)
            {
                if (objDb != null)
                {
                    objDb.RollBack();
                }
                LogOut.ErrorOut(e.Message, "ProcessInputModel", MethodBase.GetCurrentMethod().Name);
                return false;
            }
            finally
            {
                if (objDb != null)
                {
                    objDb.DBLogOut();
                }
            }
        }
        public static Boolean SetDBErrorT_PAX_H(DBEdiSend senddata,string strErr, string strErrDetail)
        {
            DataBaseSQL objDb = null;
            try
            {
                objDb = new DataBaseSQL();
                objDb.DBLogIn();
                int intRet = 0;

                objDb.BeginTransaction();

                objDb.ExecuteNonQuery(
                    GetSQL_UPDATE_SEND_H_ERR(senddata.PAX_NO, senddata.EDA_NO, strErr, strErrDetail), null, out intRet);
                if (intRet == 0)
                {
                    string strtype = "T_PAX_Hに該当データなし";
                    string str = string.Format("{0}(strYunyuNo:{1} eda:{2})", strtype, senddata.PAX_NO, senddata.EDA_NO);
                    LogOut.ErrorOut(str, "ProcessInputModel", MethodBase.GetCurrentMethod().Name);
                    objDb.RollBack();
                    return false;
                }

                objDb.Commit();
                return true;
            }
            catch (Exception e)
            {
                if (objDb != null)
                {
                    objDb.RollBack();
                }
                LogOut.ErrorOut(e.Message, "ProcessInputModel", MethodBase.GetCurrentMethod().Name);
                return false;
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
        /// 受信したデータ結果をUPDATEする
        /// </summary>
        /// <param name="recv">SAD401</param>
        /// <returns></returns>        
        public static Boolean SetDBT_PAX_H(DBEdiSend senddata)
        {
            DataBaseSQL objDb = null;
            try
            {
                objDb = new DataBaseSQL();
                objDb.DBLogIn();
                int intRet = 0;

                objDb.BeginTransaction();

                objDb.ExecuteNonQuery(GetSQL_UPDATE_T_PAX_H(senddata), null, out intRet);

                objDb.Commit();

                return true;
            }
            catch (Exception e)
            {
                if (objDb != null)
                {
                    objDb.RollBack();
                }
                LogOut.ErrorOut(e.Message, "ProcessInputModel", MethodBase.GetCurrentMethod().Name);
                return false;
            }
            finally
            {
                if (objDb != null)
                {
                    objDb.DBLogOut();
                }
            }
        }
        ///// <summary>
        ///// DBから更新処理されていないデータを取得
        ///// </summary>
        ///// <returns></returns>
        //public static string GetSQL_SELECT_PAXNO(string strPaxNo)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.AppendLine("SELECT");
        //    strSql.AppendLine("  TIME_OF_DEPARTURE");
        //    strSql.AppendLine("  ,FLIGHT_ID");
        //    strSql.AppendLine("  ,PAX_NO");
        //    strSql.AppendLine("FROM T_PAX_H");

        //    strSql.AppendLine("WHERE 1=1");
        //    strSql.AppendLine("  AND DELETE_FLG IS NULL");
        //    //strSql.AppendLine("  AND EDI_SENDDATE IS NOT NULL");
        //    strSql.AppendLine("  AND EDI_RESULT IS NULL");
        //    strSql.AppendLine("  AND EDI_RESULT_SEQ IS NULL");
        //    if (!string.IsNullOrEmpty(strPaxNo))
        //    {
        //        strSql.AppendFormat("  AND PAX_NO = '{0}'", strPaxNo).AppendLine();
        //    }
        //    return strSql.ToString();
        //}
        /// <summary>
        /// DBを電文情報から更新②(Resultのみ)
        /// </summary>
        /// <returns></returns>
        public static string GetSQL_UPDATE_RECV_T_PAX_H(string strDenbunHandover, string strRlt, long lngSeq = 0, string strYunyuNo = "" )
        {
            StringBuilder strSql = new StringBuilder();
            Boolean blnErr;

            string strlogret = ProcessResultModel.GetLogResult(strRlt, out blnErr);

            strSql.AppendLine("UPDATE T_PAX_H SET");
            strSql.AppendFormat("   ERROR_NAIYO = '{0}'", strlogret).AppendLine();
            //strSql.AppendFormat("  ,EDI_RESULT = '{0}'", strRlt).AppendLine();
            strSql.AppendFormat("  ,EDI_RESULT = (IIF(EDI_RESULT IS NULL, '{0}', EDI_RESULT + ',' + '{0}'))", strRlt).AppendLine();

            if (strYunyuNo != "")
            {
                strSql.AppendFormat("  ,YUNYU_SHINKOKU_NO = '{0}'", strYunyuNo).AppendLine();
            }


            if (blnErr)
            {
                //状態を【5:NACCSエラー】にする suzuki st
                strSql.AppendFormat("  ,TORIKOMI_JOTAI = '{0}'", HPFData.TORIKOMI_JOTAI.NACCS_ERROR).AppendLine();
                //状態を【5:NACCSエラー】にする suzuki ed
            }
            
            if (lngSeq != 0)
            {
                strSql.AppendFormat("  ,EDI_RESULT_SEQ = {0}", lngSeq).AppendLine();
            }
            strSql.AppendFormat("  ,UPD_STAFF_CD = '{0}'", HPFData.AppUserName).AppendLine();
            strSql.AppendFormat("  ,UPD_DATE = GETDATE()", "").AppendLine();
            strSql.AppendFormat("  ,UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
            strSql.AppendFormat("  ,VERSION_NO = VERSION_NO + 1", "").AppendLine();

            strSql.AppendLine("WHERE 1=1");
            strSql.AppendFormat("  AND DELETE_FLG IS NULL").AppendLine();
            strSql.AppendFormat("  AND EDI_INFO_CD = '{0}'", strDenbunHandover);
            return strSql.ToString();
        }

        /// <summary>
        /// DBを更新③輸入申告番号のみ（type=SAD4011でのみ使用）
        /// </summary>
        /// <returns></returns>
        public static string GetSQL_UPDATE_YUNYU_NO_T_PAX_H(string strDenbunHandover, string strYunyuNo, NACCS_REPORTTYPE type)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("UPDATE T_PAX_H SET");
            if (strYunyuNo != "")
            {
                strSql.AppendFormat("  YUNYU_SHINKOKU_NO = '{0}'", strYunyuNo).AppendLine();
                strSql.AppendFormat("  ,ERROR_NAIYO = ''").AppendLine();
                strSql.AppendFormat("  ,EDI_RESULT = (IIF(EDI_RESULT IS NULL, '{0}', EDI_RESULT + ',' + '{0}'))", type.ToString()).AppendLine();

                //輸入申告Noを持っていて、処理識別固定フラグ＝１（'C'を使用不可）の時のみ
                if (ConfigurationManager.AppSettings[HPFData.TEST_FLG_SHORI_SHIKIBETSU] == "1")
                {
                    //状態を【6:許可済】にする suzuki st
                    strSql.AppendFormat("  ,TORIKOMI_JOTAI = '{0}'", HPFData.TORIKOMI_JOTAI.KYOKA_ZUMI).AppendLine();
                    strSql.AppendFormat("  ,TIME_OF_IMPORT_PERMIT = GETDATE()", "").AppendLine();
                    strSql.AppendFormat("  ,IMPORT_PERMIT_NO = '{0}'", strYunyuNo).AppendLine();
                    //状態を【6:許可済】にする suzuki ed
                }
            }
            strSql.AppendFormat("  ,UPD_STAFF_CD = '{0}'", HPFData.AppUserName).AppendLine();
            strSql.AppendFormat("  ,UPD_DATE = GETDATE()", "").AppendLine();
            strSql.AppendFormat("  ,UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
            strSql.AppendFormat("  ,VERSION_NO = VERSION_NO + 1", "").AppendLine();

            strSql.AppendLine("WHERE 1=1");
            strSql.AppendFormat("  AND EDI_INFO_CD = '{0}'", strDenbunHandover).AppendLine();
            strSql.AppendFormat("  AND DELETE_FLG IS NULL").AppendLine();

            return strSql.ToString();
        }
        /// <summary>
        /// DBを輸入申告番号から更新④(その他 type=SAD4031、SAD4071、SAD4081)
        /// </summary>
        /// <returns></returns>
        public static string GetSQL_UPDATE_ADD_RESULT_T_PAX_H(string strYunyuNo, NACCS_REPORTTYPE type)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("UPDATE T_PAX_H SET");
            strSql.AppendFormat("EDI_RESULT = IIF(EDI_RESULT IS NULL, '{0}', EDI_RESULT + ',' + '{0}')", type.ToString()).AppendLine();

            //SAD4071、SAD4081の時
            if (type == NACCS_REPORTTYPE.SAD4071 || type == NACCS_REPORTTYPE.SAD4081)
            {
                //状態を【6:許可済】にする suzuki st
                strSql.AppendFormat("  ,TORIKOMI_JOTAI = '{0}'", HPFData.TORIKOMI_JOTAI.KYOKA_ZUMI).AppendLine();
                strSql.AppendFormat("  ,TIME_OF_IMPORT_PERMIT = GETDATE()", "").AppendLine();
                strSql.AppendFormat("  ,IMPORT_PERMIT_NO = '{0}'", strYunyuNo).AppendLine();
                //状態を【6:許可済】にする suzuki ed
            }
            else if (type == NACCS_REPORTTYPE.SAF0211 || type == NACCS_REPORTTYPE.SAF0221)
            {
                //状態を【6:許可済】にする　強制的に
                strSql.AppendFormat("  ,TORIKOMI_JOTAI = '{0}'", HPFData.TORIKOMI_JOTAI.KYOKA_ZUMI).AppendLine();
                strSql.AppendFormat("  ,TIME_OF_IMPORT_PERMIT = GETDATE()", "").AppendLine();
                strSql.AppendFormat("  ,IMPORT_PERMIT_NO = '{0}'", strYunyuNo).AppendLine();
                //状態を【6:許可済】にする

                strSql.AppendFormat("  ,ERROR_NAIYO = '警告通知：{0}'", type.ToString()).AppendLine();
            }

            strSql.AppendFormat("  ,UPD_STAFF_CD = '{0}'", HPFData.AppUserName).AppendLine();
            strSql.AppendFormat("  ,UPD_DATE = GETDATE()", "").AppendLine();
            strSql.AppendFormat("  ,UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
            strSql.AppendFormat("  ,VERSION_NO = VERSION_NO + 1", "").AppendLine();

            strSql.AppendLine("WHERE 1=1");
            strSql.AppendFormat("  AND DELETE_FLG IS NULL").AppendLine();
            strSql.AppendFormat("  AND YUNYU_SHINKOKU_NO = '{0}'", strYunyuNo);
            return strSql.ToString();
        }


        /// <summary>
        /// DBを更新 一括更新
        /// </summary>
        /// <returns></returns>
        public static string GetSQL_UPDATE_T_PAX_H(DBEdiSend select)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("UPDATE T_PAX_H SET");
            strSql.AppendFormat("  T_PAX_H.UPD_STAFF_CD = '{0}'", HPFData.AppUserName).AppendLine();
            strSql.AppendFormat("  ,T_PAX_H.UPD_DATE = GETDATE()", "").AppendLine();
            strSql.AppendFormat("  ,T_PAX_H.UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
            strSql.AppendFormat("  ,T_PAX_H.VERSION_NO = T_PAX_H.VERSION_NO + 1", "").AppendLine();

            strSql.AppendLine("  ,T_PAX_H.YUNYU_SHINKOKU_NO = T_EDI_SEND.YUNYU_SHINKOKU_NO");
            strSql.AppendLine("  ,T_PAX_H.ERROR_NAIYO = T_EDI_SEND.ERROR_NAIYO");
            strSql.AppendLine("  ,T_PAX_H.EDI_RESULT = T_EDI_SEND.EDI_RESULT");
            if (select.FLG_RESULT != null && select.FLG_RESULT > 0)
            {
                strSql.AppendLine("  ,T_PAX_H.EDI_RESULT_SEQ = T_EDI_SEND.FLG_RESULT");
            }
            strSql.AppendLine("  ,T_PAX_H.TORIKOMI_JOTAI = T_EDI_SEND.TORIKOMI_JOTAI");
            strSql.AppendLine("  ,T_PAX_H.TIME_OF_IMPORT_PERMIT = GETDATE()");
            strSql.AppendLine("  ,T_PAX_H.IMPORT_PERMIT_NO = T_EDI_SEND.YUNYU_SHINKOKU_NO");
            strSql.AppendLine("FROM T_EDI_SEND");

            strSql.AppendLine("WHERE 1=1");

            strSql.AppendFormat("  AND DATA_SEQ = {0}", select.DATA_SEQ).AppendLine();
            strSql.AppendLine("  AND T_PAX_H.TIME_OF_DEPARTURE = T_EDI_SEND.TIME_OF_DEPARTURE");
            strSql.AppendLine("  AND T_PAX_H.FLIGHT_ID = T_EDI_SEND.FLIGHT_ID");
            strSql.AppendLine("  AND T_PAX_H.PAX_NO = T_EDI_SEND.PAX_NO");
            strSql.AppendLine("  AND T_PAX_H.EDA_NO = T_EDI_SEND.EDA_NO");
            strSql.AppendLine("  AND T_PAX_H.DELETE_FLG IS NULL").AppendLine();


            //strSql.AppendLine("UPDATE T_PAX_H SET");
            //strSql.AppendFormat("  UPD_STAFF_CD = '{0}'", HPFData.AppUserName).AppendLine();
            //strSql.AppendFormat("  ,UPD_DATE = GETDATE()", "").AppendLine();
            //strSql.AppendFormat("  ,UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
            //strSql.AppendFormat("  ,VERSION_NO = VERSION_NO + 1", "").AppendLine();
            
            //strSql.AppendFormat("  ,YUNYU_SHINKOKU_NO = '{0}'", select.YUNYU_SHINKOKU_NO).AppendLine();
            //strSql.AppendFormat("  ,ERROR_NAIYO = (SELECT TOP 1 ERROR_NAIYO FROM T_EDI_SEND WHERE DATA_SEQ = '{0}')", select.DATA_SEQ).AppendLine();
            //strSql.AppendFormat("  ,EDI_RESULT = (SELECT TOP 1 EDI_RESULT FROM T_EDI_SEND WHERE DATA_SEQ = '{0}')", select.DATA_SEQ).AppendLine();
            //if (select.FLG_RESULT != null && select.FLG_RESULT > 0)
            //{
            //    strSql.AppendFormat("  ,EDI_RESULT_SEQ = {0}", select.FLG_RESULT).AppendLine();
            //}
            //strSql.AppendFormat("  ,TORIKOMI_JOTAI = '{0}'", select.TORIKOMI_JOTAI).AppendLine();
            //strSql.AppendFormat("  ,TIME_OF_IMPORT_PERMIT = GETDATE()", "").AppendLine();
            //strSql.AppendFormat("  ,IMPORT_PERMIT_NO = '{0}'", select.YUNYU_SHINKOKU_NO).AppendLine();

            //strSql.AppendLine("WHERE 1=1");
            //strSql.AppendFormat("  AND TIME_OF_DEPARTURE = CONVERT(DATETIME, '{0}')", select.TIME_OF_DEPARTURE).AppendLine();
            //strSql.AppendFormat("  AND FLIGHT_ID = '{0}'", select.FLIGHT_ID).AppendLine();
            //strSql.AppendFormat("  AND PAX_NO = '{0}'", select.PAX_NO).AppendLine();
            //strSql.AppendFormat("  AND EDA_NO = '{0}'", select.EDA_NO).AppendLine();
            //strSql.AppendFormat("  AND DELETE_FLG IS NULL").AppendLine();

            return strSql.ToString();
        }

        /// <summary>
        /// データの設定（ユーザ毎のテーブルデータを入力する）
        /// </summary>
        /// <returns></returns>
        public bool SetData(DataTable dtTbl)
        {
            if (dtTbl != null && dtTbl.Rows.Count > 0)
            {
                bool first = true;

                string strTimeDep, strFId, strPaxNo, strEdaNo;
                strTimeDep = strFId = strPaxNo = strEdaNo = "";
                int errnum = 0;

                foreach (DataRow row in dtTbl.Rows)
                {
                    strTimeDep = (Common.ConvertToString(row["TIME_OF_DEPARTURE"]));
                    strFId = (Common.ConvertToString(row["FLIGHT_ID"]));
                    strPaxNo = (Common.ConvertToString(row["PAX_NO"]));
                    strEdaNo = (Common.ConvertToString(row["EDA_NO"]));

                    if (first)
                    {
                        this.TIME_OF_DEPARTURE = (Common.ConvertToString(row["TIME_OF_DEPARTURE"]));
                        this.FLIGHT_ID = (Common.ConvertToString(row["FLIGHT_ID"]));
                        this.PAX_NO = (Common.ConvertToString(row["PAX_NO"]));
                        this.EDA_NO = (Common.ConvertToString(row["EDA_NO"]));


                        if (!this.YUNYU_SHINKOKU_NO.SetData(Common.ConvertToStringUpper(row["YUNYU_SHINKOKU_NO"]))) errnum++;
                        if (!this.SHORI_SHIKIBETSU.SetData(Common.ConvertToStringUpper(row["SHORI_SHIKIBETSU"]))) errnum++;
                        if (!this.ATESAKI_KANSHO_CD.SetData(Common.ConvertToStringUpper(row["ATESAKI_KANSHO_CD"]))) errnum++;
                        if (!this.ATESAKI_BUMON_CD.SetData(Common.ConvertToStringUpper(row["ATESAKI_BUMON_CD"]))) errnum++;
                        if (!this.PASSENGER.SetData(Common.ConvertToStringUpper(row["PASSENGER"]))) errnum++;
                        if (!this.ZIPCODE.SetData(Common.ConvertToStringUpper(row["ZIPCODE"]))) errnum++;
                        if (!this.ADDRESS1.SetData(Common.ConvertToStringUpper(row["ADDRESS1"]))) errnum++;
                        if (!this.ADDRESS2.SetData(Common.ConvertToStringUpper(row["ADDRESS2"]))) errnum++;
                        if (!this.ADDRESS3.SetData(Common.ConvertToStringUpper(row["ADDRESS3"]))) errnum++;
                        if (!this.ADDRESS4.SetData(Common.ConvertToStringUpper(row["ADDRESS4"]))) errnum++;
                        if (!this.AIRLINE_CD_ICAO.SetData(Common.ConvertToStringUpper(row["AIRLINE_CD_ICAO"]))) errnum++;
                        if (!this.FLIGHT_NO.SetData(Common.ConvertToStringUpper(row["FLIGHT_NO"]))) errnum++;
                        if (!this.TUKANYOTEI_CHOZO_OKIBA_CD.SetData(Common.ConvertToStringUpper(row["TUKANYOTEI_CHOZO_OKIBA_CD"]))) errnum++;
                        if (!this.NOFU_HOHO_SHIKIBETSU.SetData(Common.ConvertToStringUpper(row["NOFU_HOHO_SHIKIBETSU"]))) errnum++;
                        if (!this.KOZA_NO.SetData(Common.ConvertToStringUpper(row["KOZA_NO"]))) errnum++;
                        if (!this.BP_SHINSEI_JIYU.SetData(Common.ConvertToStringUpper(row["BP_SHINSEI_JIYU"]))) errnum++;
                        if (!this.TANPO_TOROKU_NO.SetData(Common.ConvertToStringUpper(row["TANPO_TOROKU_NO"]))) errnum++;
                        if (!this.KANZEI_MENJOZUMI_GAKU.SetData(Common.ConvertToString(row["KANZEI_MENJOZUMI_GAKU"]))) errnum++;
                        if (!this.KIJI_KANZEI.SetData(Common.ConvertToString(row["KIJI_KANZEI"]))) errnum++;
                        if (!this.KIJI_HANBAITEN.SetData(Common.ConvertToString(row["KIJI_HANBAITEN"]))) errnum++;
                        if (!this.KIJI_SONOTA.SetData(Common.ConvertToString(row["KIJI_SONOTA"]))) errnum++;
                        if (!this.SHANAI_SEIRI_NO.SetData(Common.ConvertToStringUpper(row["SHANAI_SEIRI_NO"]))) errnum++;

                        if (this.YUNYU_SHINKOKU_NO.Data.Length > 0)
                        {
                            //更新処理
                            this.NaccsCommon.GYOMU_CD.SetData("OTA01");//
                        }
                        else
                        {
                            //新規登録
                            this.NaccsCommon.GYOMU_CD.SetData("OTA");//
                        }
                        this.NaccsCommon.USER_CD.SetData(HPFData.InputUserCode);
                        this.NaccsCommon.SHIKIBETSU_NO.SetData(HPFData.NaccsShikibetsuNo);
                        this.NaccsCommon.USER_PASS.SetData(HPFData.InputUserPass);

                        this.NaccsCommon.DENBUN_HANDOVER.SetData(DEFAULT_DENBUN_HANDOVER);
                        this.NaccsCommon.INPUT_INFO_NO.SetData(DEFAULT_INPUT_INFO_NO);
                        this.NaccsCommon.SAKUIN_HANDOVER.SetData("");//照会業務を行う場合
                        this.NaccsCommon.SYSTEM_TYPE.SetData("2");//NACCS:航空:1海上:2

                        first = false;
                        //if (errnum > 0)
                        //{
                        //    return false;
                        //}
                    }
                    else
                    {
                        if (strTimeDep != this.TIME_OF_DEPARTURE ||
                            strFId != this.FLIGHT_ID ||
                            strPaxNo != this.PAX_NO || 
                            strEdaNo != this.EDA_NO)
                        {
                            continue;
                        }
                    }
                    errnum = 0;
                    ShohinModel shohindata = new ShohinModel();

                    if (!shohindata.KANZEI_TAISHO.SetData(Common.ConvertToStringUpper(row["KANZEI_TAISHO"]))) errnum++;
                    if (!shohindata.SHOHIN_KANRI_CD.SetData(Common.ConvertToStringUpper(row["SHOHIN_KANRI_CD"]))) errnum++;
                    if (!shohindata.HINMEI.SetData(Common.ConvertToStringUpper(row["HINMEI"]))) errnum++;
                    if (!shohindata.HANBAI_TANKA.SetData(Common.ConvertToString(row["HANBAI_TANKA"]))) errnum++;
                    if (!shohindata.HANBAI_SURYO.SetData(Common.ConvertToString(row["HANBAI_SURYO"]))) errnum++;
                    if (!shohindata.HINMOKU_CD.SetData(Common.ConvertToStringUpper(row["HINMOKU_CD"]))) errnum++;
                    if (!shohindata.NACCS_CD.SetData(Common.ConvertToStringUpper(row["NACCS_CD"]))) errnum++;
                    if (!shohindata.GENZANCHI_CD.SetData(Common.ConvertToStringUpper(row["GENZANCHI_CD"]))) errnum++;
                    if (!shohindata.GENZANCHI_SHOMEI_SHIKIBETSU.SetData(Common.ConvertToStringUpper(row["GENZANCHI_SHOMEI_SHIKIBETSU"]))) errnum++;
                    if (!shohindata.INN_TAISHO.SetData(Common.ConvertToStringUpper(row["INN_TAISHO"]))) errnum++;
                    if (!shohindata.SURYO1.SetData(Common.ConvertToString(row["SURYO1"]))) errnum++;
                    if (!shohindata.SURYO_TANI_CD1.SetData(Common.ConvertToStringUpper(row["SURYO_TANI_CD1"]))) errnum++;
                    if (!shohindata.SURYO2.SetData(Common.ConvertToString(row["SURYO2"]))) errnum++;
                    if (!shohindata.SURYO_TANI_CD2.SetData(Common.ConvertToStringUpper(row["SURYO_TANI_CD2"]))) errnum++;
                    if (!shohindata.KAZEI_KAKAKU.SetData(Common.ConvertToString(row["KAZEI_KAKAKU"]))) errnum++;
                    if (!shohindata.NAIKOKU_SHOHIZEITOU_SHUBETSU_CD1.SetData(Common.ConvertToStringUpper(row["NAIKOKU_SHOHIZEITOU_SHUBETSU_CD"]))) errnum++;
                    if (!shohindata.NAIKOKU_SHOHIZEITOU_SHUBETSU_CD2.SetData(Common.ConvertToStringUpper(row["NAIKOKU_SHOHIZEITOU_SHUBETSU_CD2"]))) errnum++;

                    //if (string.IsNullOrWhiteSpace(shohindata.KANZEI_TAISHO.Data))
                    //{
                    //    Int32 intz;
                    //    if (Int32.TryParse(shohindata.KAZEI_KAKAKU.Data, out intz))
                    //    {
                    //        if (intz > 0)
                    //        {
                    //            shohindata.KANZEI_TAISHO.SetData("Y");
                    //        }
                    //    }
                    //}
                    //if (errnum == 0)
                    //{
                        this.Shohin.Add(shohindata);
                    //}
                    //else
                    //{
                    //    return false;
                    //}
                }
                //最後に電文長の登録
                this.NaccsCommon.DENBUN_LEN.SetData(this.GetByteLength().ToString());

                return true;
            }
            return false;
        }
        /// <summary>
        /// データの設定
        /// </summary>
        /// <returns></returns>
        public static bool AnalyzedDataSet(DataSet dsData, out List<DataTable> dtList)
        {
            string strWhere;
            return ProcessInputModel.AnalyzedDataSet(dsData, out dtList, out strWhere); 
        }
        /// <summary>
        /// データの設定
        /// </summary>
        /// <returns></returns>
        public static bool AnalyzedDataSet(DataSet dsData, out List<DataTable> dtList, out string strWhere)
        {
            Boolean ret = false;
            DataTable dtP;
            dtList = new List<DataTable>();
            strWhere = "";

            if (dsData != null && dsData.Tables.Count > 0)
            {
                dtP = dsData.Tables[0];

                //var order = dtP.AsEnumerable().GroupBy(
                //    row => new {
                //        row.TIME_OF_DEPARTURE = row.Field<string>("TIME_OF_DEPARTURE") });

                string strTimeDep, strFId, strPaxNo, strEdaNo;
                strTimeDep= strFId = strPaxNo = strEdaNo ="";
                DataTable dt = dtP.Clone();
                 
                foreach (DataRow row in dtP.Rows)
                {
                    if (
                        strTimeDep == Common.ConvertToString(row["TIME_OF_DEPARTURE"]) &&
                        strFId == Common.ConvertToString(row["FLIGHT_ID"]) &&
                        strPaxNo == Common.ConvertToString(row["PAX_NO"]) &&
                        strEdaNo == Common.ConvertToString(row["EDA_NO"]) )
                    {
                        dt.ImportRow(row);
                    }
                    else
                    {
                        if (dt.Rows.Count > 0)
                        {
                            dtList.Add(dt);
                            ret = true;
                            dt = dtP.Clone();
                            //strWhere += ((strWhere == "") ? "" : ", ") + strPaxNo;
                            strWhere += ((strWhere == "") ? "" : " OR ") + string.Format("(PAX_NO = '{0}' AND EDA_NO = '{1}')", strPaxNo, strEdaNo);
                        }                        
                        dt.ImportRow(row);

                        strTimeDep = (Common.ConvertToString(row["TIME_OF_DEPARTURE"]));
                        strFId = (Common.ConvertToString(row["FLIGHT_ID"]));
                        strPaxNo = (Common.ConvertToString(row["PAX_NO"]));
                        strEdaNo = (Common.ConvertToString(row["EDA_NO"]));
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    //strWhere = string.Format("PAX_NO in ({0}, {1})", strWhere, strPaxNo);
                    strWhere = string.Format("({0}{1} (PAX_NO = '{2}' AND EDA_NO = '{3}'))", strWhere, ((strWhere == "") ? "": " OR"), strPaxNo, strEdaNo);
                    dtList.Add(dt);
                    ret = true;
                }
            }
            return ret;
        }
        /// <summary>
        /// タイムアウトをDBに追記
        /// </summary>
        /// <param name="btData"></param>
        /// <returns></returns>
        public Boolean SetResultDB(string strErrNaiyo, string strResult)
        {
            DataBaseSQL objDb = null;
            try
            {
                if (this.PAX_NO != null)
                {
                    objDb = new DataBaseSQL();
                    objDb.DBLogIn();

                    objDb.BeginTransaction();

                    objDb.ExecuteNonQuery(this.GetSQL_UPDATE_RESULT(this.PAX_NO, this.EDA_NO, strErrNaiyo, strResult), null);

                    objDb.Commit();
                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {
                if (objDb != null)
                {
                    objDb.RollBack();
                }
                LogOut.ErrorOut(e.Message, "ProcessInputModel", MethodBase.GetCurrentMethod().Name);
                return false;
            }
            finally
            {
                if (objDb != null)
                {
                    objDb.DBLogOut();
                }
            }
        }
        ///// <summary>
        ///// byte配列からInsertまでこなす
        ///// </summary>
        ///// <param name="btData"></param>
        ///// <returns></returns>
        //public Boolean UpdateData()
        //{
        //    int err = 0;
        //    DataBaseSQL objDb = null;
        //    try
        //    {
        //        objDb = new DataBaseSQL();
        //        objDb.DBLogIn();

        //        objDb.BeginTransaction();

        //        objDb.ExecuteNonQuery(this.GetSQL_UPDATE_SEND_H(), null);

        //        objDb.ExecuteNonQuery(this.GetSQL_UPDATE_SEND_D(), null);

        //        objDb.Commit();
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        if (objDb != null)
        //        {
        //            objDb.RollBack();
        //        }
        //        LogOut.ErrorOut(e.Message, MethodBase.GetCurrentMethod().Name);
        //        return false;
        //    }
        //    finally
        //    {
        //        if (objDb != null)
        //        {
        //            objDb.DBLogOut();
        //        }
        //    }
        //}
        /// <summary>
        /// Appconfigから設定値を取得します
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string GetAppSetting(string strKey)
        {
            string strValue = string.Empty;
            try
            {
                strValue = ConfigurationManager.AppSettings[strKey];
                if (string.IsNullOrWhiteSpace(strValue))
                    strValue = string.Empty;
                return strValue;
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.ToString(), "ProcessInputModel", "GetAppSetting");
                throw ex;
            }
        }
    }

    #endregion "入力処理モデル　OTA"


    #region "入力処理モデル　OTC"

    /// <summary>
    /// 処理要求モデル
    /// </summary>
    public class ProcessDeclareModel : NaccsSendModel
    {
        public string TIME_OF_DEPARTURE;
        public string FLIGHT_ID;
        public string PAX_NO;
        public string EDA_NO;
        protected DateTime CREATE_DATE;

        /// <summary>
        /// 受信確認があった場合TRUE
        /// </summary>
        public Dictionary<NACCS_REPORTTYPE, string> GetResponse
        {
            get
            {
                return this.i_RltResponse;
            }
        }
        private Dictionary<NACCS_REPORTTYPE, string> i_RltResponse;

        /// <summary>
        /// 電文引継情報（26文字）
        /// PAX番号が10桁以上の場合は考慮が必要
        /// </summary>
        public string DEFAULT_DENBUN_HANDOVER
        {
            get
            {
                return string.Format("{0}{1}-{2}",
                    Common.ConvertToStringUpper(this.PAX_NO),
                    Common.ConvertToStringUpper(this.EDA_NO),
                    this.CREATE_DATE.ToString("ddHHmmss")
                    );
                //(string.IsNullOrWhiteSpace(this.YUNYU_SHINKOKU_NO.Data))? "1" : "2");
            }
        }
        ///// <summary>
        ///// 電文引継情報からPAXデータの取得
        ///// </summary>
        //public static string GetPaxNoFromDenbunHandover(string strDH)
        //{
        //    if (strDH[strDH.Length - 2] == '-')
        //    {
        //        return strDH.Substring(0, strDH.Length - 2);
        //    }
        //    return strDH;
        //}

        /// <summary>
        /// 入力情報番号
        /// </summary>
        public string DEFAULT_INPUT_INFO_NO
        {
            get { return (string.IsNullOrWhiteSpace(this.YUNYU_SHINKOKU_NO.Data)) ? "1" : "2"; }
        }

        private const string NACCS_USER_CODE = "NaccsInputUserCode";
        private const string NACCS_USER_PASS = "NaccsInputUserPass";
        private const string NACCS_SHIKIBETSU_NO = "NaccsInputCode";

        protected static string i_UserCode = string.Empty;
        protected static string i_ShikibetsuNo = string.Empty;
        protected static string i_UserPass = string.Empty;

        /// <summary>
        /// 入力共通項目398
        /// </summary>
        public InputCommonModel InputCommon;
        /// <summary>
        /// 輸入申告番号 2
        /// </summary>
        public NacCollumnAN YUNYU_SHINKOKU_NO = new NacCollumnAN(11);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProcessDeclareModel()
        {
            InputCommon = new InputCommonModel();
            ReportType = NACCS_REPORTTYPE.OTC;
            CREATE_DATE = DateTime.Now;

            i_UserCode = GetAppSetting(NACCS_USER_CODE);
            i_UserPass = GetAppSetting(NACCS_USER_PASS);
            i_ShikibetsuNo = GetAppSetting(NACCS_SHIKIBETSU_NO);

            this.i_RltResponse = new Dictionary<NACCS_REPORTTYPE, string>();
        }
        public int GetByteLength()
        {
            int intlen = 0;
            intlen += InputCommon.GetByteLength();

            intlen += GetByteLengthMain();
            return intlen;
        }
        private int GetByteLengthMain()
        {
            int intlen = 0;
            intlen += YUNYU_SHINKOKU_NO.GetByteLength();

            return intlen;
        }

        /// <summary>
        /// 添付用データの作成
        /// </summary>
        /// <returns></returns>
        public override byte[] GetByteData()
        {
            int intLast = this.GetByteLength();
            byte[] btData = new byte[intLast];
            int pos = 0, intLen = 0;

            try
            {
                intLen = this.GetByteLengthMain();
                if (intLen > 0)
                {
                    //intLast += 2;
                    Array.Copy(this.InputCommon.GetByteData(intLast), 0, btData, pos, this.InputCommon.GetByteLength());
                    pos += this.InputCommon.GetByteLength();

                    Array.Copy(this.YUNYU_SHINKOKU_NO.GetByteData(), 0, btData, pos, this.YUNYU_SHINKOKU_NO.GetByteLength());
                    pos += YUNYU_SHINKOKU_NO.GetByteLength();

                    return btData;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                LogOut.ErrorOut(e.ToString().Substring(0, 50), "ProcessDeclareModel", MethodBase.GetCurrentMethod().Name);
                throw e;
            }
        }
         /// <summary>
        /// データの設定（ユーザ毎のテーブルデータを入力する）
        /// </summary>
        /// <returns></returns>
        public bool SetData(DataTable dtTbl)
        {
            if (dtTbl != null && dtTbl.Rows.Count > 0)
            {
                bool first = true;

                string strTimeDep, strFId, strPaxNo, strEdaNo;
                strTimeDep = strFId = strPaxNo = strEdaNo = "";
                int errnum = 0;

                foreach (DataRow row in dtTbl.Rows)
                {
                    strTimeDep = (Common.ConvertToString(row["TIME_OF_DEPARTURE"]));
                    strFId = (Common.ConvertToString(row["FLIGHT_ID"]));
                    strPaxNo = (Common.ConvertToString(row["PAX_NO"]));
                    strEdaNo = (Common.ConvertToString(row["EDA_NO"]));
                    if (first)
                    {
                        this.TIME_OF_DEPARTURE = (Common.ConvertToString(row["TIME_OF_DEPARTURE"]));
                        this.FLIGHT_ID = (Common.ConvertToString(row["FLIGHT_ID"]));
                        this.PAX_NO = (Common.ConvertToString(row["PAX_NO"]));
                        this.EDA_NO = (Common.ConvertToString(row["EDA_NO"])); 


                        if (!this.YUNYU_SHINKOKU_NO.SetData(Common.ConvertToStringUpper(row["YUNYU_SHINKOKU_NO"]))) errnum++;

                        this.InputCommon.GYOMU_CD.SetData(this.ReportType.ToString());//

                        this.InputCommon.USER_CD.SetData(HPFData.InputUserCode);
                        this.InputCommon.SHIKIBETSU_NO.SetData(HPFData.NaccsShikibetsuNo);
                        this.InputCommon.USER_PASS.SetData(HPFData.InputUserPass);

                        this.InputCommon.DENBUN_HANDOVER.SetData(DEFAULT_DENBUN_HANDOVER);
                        this.InputCommon.INPUT_INFO_NO.SetData(DEFAULT_INPUT_INFO_NO);
                        this.InputCommon.SAKUIN_HANDOVER.SetData("");//照会業務を行う場合
                        this.InputCommon.SYSTEM_TYPE.SetData("2");//NACCS:航空:1海上:2

                        first = false;
                        //if (errnum > 0)
                        //{
                        //    return false;
                        //}
                    }
                    else
                    {
                        if (strTimeDep != this.TIME_OF_DEPARTURE ||
                            strFId != this.FLIGHT_ID ||
                            strPaxNo != this.PAX_NO || 
                            strEdaNo != this.EDA_NO)
                        {
                            continue;
                        }
                    }
                    errnum = 0;
 
                }
                //最後に電文長の登録
                this.InputCommon.DENBUN_LEN.SetData(this.GetByteLength().ToString());

                return true;
            }
            return false;
        }
        ///// <summary>
        ///// データの設定
        ///// </summary>
        ///// <returns></returns>
        //public static bool AnalyzedDataSet(DataSet dsData, out List<DataTable> dtList)
        //{
        //    Boolean ret = false;
        //    DataTable dtP;
        //    dtList = new List<DataTable>();

        //    if (dsData != null && dsData.Tables.Count > 0)
        //    {
        //        dtP = dsData.Tables[0];

        //        //var order = dtP.AsEnumerable().GroupBy(
        //        //    row => new {
        //        //        row.TIME_OF_DEPARTURE = row.Field<string>("TIME_OF_DEPARTURE") });

        //        string strTimeDep, strFId, strPaxNo;
        //        strTimeDep = strFId = strPaxNo = "";
        //        DataTable dt = dtP.Clone();

        //        foreach (DataRow row in dtP.Rows)
        //        {
        //            if (
        //                strTimeDep == Common.ConvertToString(row["TIME_OF_DEPARTURE"]) &&
        //                strFId == Common.ConvertToString(row["FLIGHT_ID"]) &&
        //                strPaxNo == Common.ConvertToString(row["PAX_NO"]))
        //            {
        //                dt.ImportRow(row);
        //            }
        //            else
        //            {
        //                if (dt.Rows.Count > 0)
        //                {
        //                    dtList.Add(dt);
        //                    ret = true;
        //                    dt = dtP.Clone();
        //                }
        //                dt.ImportRow(row);

        //                strTimeDep = (Common.ConvertToString(row["TIME_OF_DEPARTURE"]));
        //                strFId = (Common.ConvertToString(row["FLIGHT_ID"]));
        //                strPaxNo = (Common.ConvertToString(row["PAX_NO"]));
        //            }
        //        }
        //        if (dt.Rows.Count > 0)
        //        {
        //            dtList.Add(dt);
        //            ret = true;
        //        }
        //    }
        //    return ret;
        //}

        /// <summary>
        /// Appconfigから設定値を取得します
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string GetAppSetting(string strKey)
        {
            string strValue = string.Empty;
            try
            {
                strValue = ConfigurationManager.AppSettings[strKey];
                if (string.IsNullOrWhiteSpace(strValue))
                    strValue = string.Empty;
                return strValue;
            }
            catch (Exception ex)
            {
                LogOut.ErrorOut(ex.ToString(), "ProcessDeclareModel", "GetAppSetting");
                throw ex;
            }
        }
    }

    #endregion "入力処理モデル　OTA"


    #region 入力共通モデル

    /// <summary>
    /// 入力共通項目
    /// </summary>
    public class InputCommonModel : NaccsModel 
    {
        /// <summary>
        /// レポートタイプ
        /// </summary>
        public NACCS_REPORTTYPE REPORT_TYPE = NACCS_REPORTTYPE.NONE;
        /// <summary>
        /// LOG識別用文字列
        /// </summary>
        protected const string LOGSTR = "SEND";
        /// <summary>
        /// 保存用ファイル名（拡張子なし）
        /// </summary>
        public string GetFileName(DateTime senddate)
        {
            return string.Format("{0}_{1}_{2}", LOGSTR, this.REPORT_TYPE, senddate.ToString("yyyyMMddhhmmssfff"));
        }
        ///// <summary>
        ///// 受信確認があった場合TRUE
        ///// </summary>
        //public Boolean IsGetProcessResult = false;

        /// <summary>
        /// メールのサブジェクト
        /// </summary>
        public string SUBJECT
        {
            get { return _subject; }
            set { _subject = value; }
        }
        private string _subject = "";

        /// <summary>
        /// 制御情報
        /// </summary>
        private NacCollumnAN ReserveTop = new NacCollumnAN(3,"", false);
        /// <summary>
        /// 業務コード
        /// </summary>
        public NacCollumnAN GYOMU_CD = new NacCollumnAN(5, false);
        /// <summary>
        /// 予約コード３
        /// </summary>
        private NacCollumnAN Reserve3 = new NacCollumnAN(21, "", false);
        /// <summary>
        /// 利用者コード
        /// </summary>
        public NacCollumnAN USER_CD = new NacCollumnAN(5, false);
        /// <summary>
        /// 識別番号
        /// </summary>
        public NacCollumnAN SHIKIBETSU_NO = new NacCollumnAN(3, false);
        /// <summary>
        /// 利用者パスワード
        /// </summary>
        public NacCollumnAN USER_PASS = new NacCollumnAN(8, false);
        /// <summary>
        /// 予約コード７
        /// </summary>
        private NacCollumnAN Reserve7 = new NacCollumnAN(174, "", false);
        /// <summary>
        /// 電文引継コード
        /// </summary>
        public NacCollumnAN DENBUN_HANDOVER = new NacCollumnAN(26, false);
        /// <summary>
        /// 予約コード９
        /// </summary>
        private NacCollumnAN Reserve9 = new NacCollumnAN(8, "", false);
        /// <summary>
        /// 入力情報特定番号
        /// </summary>
        public NacCollumnAN INPUT_INFO_NO = new NacCollumnAN(10, false);
        /// <summary>
        /// 索引引継情報
        /// </summary>
        public NacCollumnAN SAKUIN_HANDOVER = new NacCollumnAN(100, false);
        /// <summary>
        /// 予約コード１２
        /// </summary>
        private NacCollumnAN Reserve12 = new NacCollumnAN(1, "", false);      
        /// <summary>
        /// システム識別
        /// </summary>
        public NacCollumnAN SYSTEM_TYPE = new NacCollumnAN(1, false);
        /// <summary>
        /// 予約コード14
        /// </summary>
        private NacCollumnAN Reserve14 = new NacCollumnAN(27, "", false);
        /// <summary>
        /// 電文長
        /// </summary>
        public NacCollumnNLEN DENBUN_LEN = new NacCollumnNLEN(6, 6, false);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InputCommonModel()
        {
            this._subject = "";
        }
        public InputCommonModel(InputCommonModel copy)
        {
            this.GYOMU_CD.SetData(copy.GYOMU_CD.Data);
            this.USER_CD.SetData(copy.USER_CD.Data);
            this.SHIKIBETSU_NO.SetData(copy.SHIKIBETSU_NO.Data);
            this.USER_PASS.SetData(copy.USER_PASS.Data);
            this.DENBUN_HANDOVER.SetData(copy.DENBUN_HANDOVER.Data);
            this.INPUT_INFO_NO.SetData(copy.INPUT_INFO_NO.Data);
            this.SAKUIN_HANDOVER.SetData(copy.SAKUIN_HANDOVER.Data);
            this.SYSTEM_TYPE.SetData(copy.SYSTEM_TYPE.Data);
        }

        /// <summary>
        /// byte配列から各値を設定
        /// </summary>
        /// <param name="btData"></param>
        /// <returns></returns>
        public override Boolean SetByteData(byte[] btData)
        {
            int intLast = this.GetByteLength();
            int pos = 0;

            try
            {
                if (btData.Length >= intLast)
                {
                    pos += this.ReserveTop.GetByteLength();

                    this.GYOMU_CD.SetData(EUC.GetString(btData, pos, this.GYOMU_CD.GetByteLength()));
                    pos += GYOMU_CD.GetByteLength();

                    pos += Reserve3.GetByteLength();

                    this.USER_CD.SetData(EUC.GetString(btData, pos, this.USER_CD.GetByteLength()));
                    pos += USER_CD.GetByteLength();

                    this.SHIKIBETSU_NO.SetData(EUC.GetString(btData, pos, this.SHIKIBETSU_NO.GetByteLength()));
                    pos += SHIKIBETSU_NO.GetByteLength();

                    this.USER_PASS.SetData(EUC.GetString(btData, pos, this.USER_PASS.GetByteLength()));
                    pos += USER_PASS.GetByteLength();

                    pos += Reserve7.GetByteLength();

                    this.DENBUN_HANDOVER.SetData(EUC.GetString(btData, pos, this.DENBUN_HANDOVER.GetByteLength()));
                    pos += DENBUN_HANDOVER.GetByteLength();

                    pos += Reserve9.GetByteLength();

                    this.INPUT_INFO_NO.SetData(EUC.GetString(btData, pos, this.INPUT_INFO_NO.GetByteLength()));
                    pos += INPUT_INFO_NO.GetByteLength();
                    
                    this.SAKUIN_HANDOVER.SetData(EUC.GetString(btData, pos, this.SAKUIN_HANDOVER.GetByteLength()));
                    pos += SAKUIN_HANDOVER.GetByteLength();

                    pos += Reserve12.GetByteLength();

                    this.SYSTEM_TYPE.SetData(EUC.GetString(btData, pos, this.SYSTEM_TYPE.GetByteLength()));
                    pos += SYSTEM_TYPE.GetByteLength();

                    pos += Reserve14.GetByteLength();

                    this.DENBUN_LEN.SetData(EUC.GetString(btData, pos, this.DENBUN_LEN.GetByteLength()));
                    pos += DENBUN_LEN.GetByteLength();

                    this.REPORT_TYPE = OutputCommonModel.GetReportType("", this.GYOMU_CD.Data); 

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                LogOut.ErrorOut(e.ToString(), this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                throw e;
            }
        }
        /// <summary>
        ///         
        /// </summary>
        /// <returns></returns>
        public int GetByteLength()
        {
            int intlen = 0;

            intlen += ReserveTop.GetByteLength();
            intlen += GYOMU_CD.GetByteLength();
            intlen += Reserve3.GetByteLength();
            intlen += USER_CD.GetByteLength();
            intlen += SHIKIBETSU_NO.GetByteLength();
            intlen += USER_PASS.GetByteLength();
            intlen += Reserve7.GetByteLength();
            intlen += DENBUN_HANDOVER.GetByteLength();
            intlen += Reserve9.GetByteLength();
            intlen += INPUT_INFO_NO.GetByteLength();
            intlen += SAKUIN_HANDOVER.GetByteLength();
            intlen += Reserve12.GetByteLength();
            intlen += SYSTEM_TYPE.GetByteLength();
            intlen += Reserve14.GetByteLength();
            intlen += DENBUN_LEN.GetByteLength();

            intlen += 2;

            return intlen;
        }
        /// <summary>
        /// 添付用データの作成
        /// </summary>
        /// <returns></returns>
        public byte[] GetByteData(int denbunLength)
        {
            this.DENBUN_LEN.SetData(denbunLength.ToString());
            return this.GetByteData();
        }
        /// <summary>
        /// 添付用データの作成
        /// </summary>
        /// <returns></returns>
        public override byte[] GetByteData()
        {
            int intLast = this.GetByteLength();
            byte[] btData = new byte[intLast];
            int pos = 0, intD = 0;

            try
            {

                if (int.TryParse(this.DENBUN_LEN.Data.ToString(), out intD))
                {
                    if (intD > 0)
                    {

                        Array.Copy(this.ReserveTop.GetByteData(), 0, btData, pos, this.ReserveTop.GetByteLength());
                        pos += this.ReserveTop.GetByteLength();

                        Array.Copy(this.GYOMU_CD.GetByteData(), 0, btData, pos, this.GYOMU_CD.GetByteLength());
                        pos += this.GYOMU_CD.GetByteLength();

                        Array.Copy(this.Reserve3.GetByteData(), 0, btData, pos, this.Reserve3.GetByteLength());
                        pos += this.Reserve3.GetByteLength();

                        Array.Copy(this.USER_CD.GetByteData(), 0, btData, pos, this.USER_CD.GetByteLength());
                        pos += this.USER_CD.GetByteLength();

                        Array.Copy(this.SHIKIBETSU_NO.GetByteData(), 0, btData, pos, this.SHIKIBETSU_NO.GetByteLength());
                        pos += this.SHIKIBETSU_NO.GetByteLength();

                        Array.Copy(this.USER_PASS.GetByteData(), 0, btData, pos, this.USER_PASS.GetByteLength());
                        pos += this.USER_PASS.GetByteLength();

                        Array.Copy(this.Reserve7.GetByteData(), 0, btData, pos, this.Reserve7.GetByteLength());
                        pos += this.Reserve7.GetByteLength();

                        Array.Copy(this.DENBUN_HANDOVER.GetByteData(), 0, btData, pos, this.DENBUN_HANDOVER.GetByteLength());
                        pos += this.DENBUN_HANDOVER.GetByteLength();

                        Array.Copy(this.Reserve9.GetByteData(), 0, btData, pos, this.Reserve9.GetByteLength());
                        pos += this.Reserve9.GetByteLength();

                        Array.Copy(this.INPUT_INFO_NO.GetByteData(), 0, btData, pos, this.INPUT_INFO_NO.GetByteLength());
                        pos += this.INPUT_INFO_NO.GetByteLength();

                        Array.Copy(this.SAKUIN_HANDOVER.GetByteData(), 0, btData, pos, this.SAKUIN_HANDOVER.GetByteLength());
                        pos += this.SAKUIN_HANDOVER.GetByteLength();

                        Array.Copy(this.Reserve12.GetByteData(), 0, btData, pos, this.Reserve12.GetByteLength());
                        pos += this.Reserve12.GetByteLength();

                        Array.Copy(this.SYSTEM_TYPE.GetByteData(), 0, btData, pos, this.SYSTEM_TYPE.GetByteLength());
                        pos += this.SYSTEM_TYPE.GetByteLength();

                        Array.Copy(this.Reserve14.GetByteData(), 0, btData, pos, this.Reserve14.GetByteLength());
                        pos += this.Reserve14.GetByteLength();

                        Array.Copy(this.DENBUN_LEN.GetByteData(), 0, btData, pos, this.DENBUN_LEN.GetByteLength());
                        pos += this.DENBUN_LEN.GetByteLength();

                        btData[pos++] = 0x0D;
                        btData[pos++] = 0x0A;
                        return btData;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                LogOut.ErrorOut(e.ToString(), this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                throw e;
            }
        }
    }
    #endregion "入力共通モデル"


    #region 商品モデル

    /// <summary>
    /// 商品モデル（送信用）
    /// </summary>
    public class ShohinModel : NaccsModel
    {
        /// <summary>
        /// 関税対象 24
        /// </summary>
        public NacCollumnAN KANZEI_TAISHO = new NacCollumnAN(1);
        /// <summary>
        /// 商品管理コード 25
        /// </summary>
        public NacCollumnAN SHOHIN_KANRI_CD = new NacCollumnAN(6);
        /// <summary>
        /// 品名 26
        /// </summary>
        public NacCollumnAN HINMEI = new NacCollumnAN(40);
        /// <summary>
        /// 販売単価 27
        /// </summary>
        public NacCollumnN HANBAI_TANKA = new NacCollumnN(9);
        /// <summary>
        /// 販売数量 28
        /// </summary>
        public NacCollumnN HANBAI_SURYO = new NacCollumnN(4);
        /// <summary>
        /// 品目コード 29
        /// </summary>
        public NacCollumnAN HINMOKU_CD = new NacCollumnAN(9);
        /// <summary>
        /// NACCS用コード 30
        /// </summary>
        public NacCollumnAN NACCS_CD = new NacCollumnAN(1);
        /// <summary>
        /// 原産地 31
        /// </summary>
        public NacCollumnAN GENZANCHI_CD = new NacCollumnAN(2);
        /// <summary>
        /// 原産地証明書識別 32
        /// </summary>
        public NacCollumnAN GENZANCHI_SHOMEI_SHIKIBETSU = new NacCollumnAN(4);
        /// <summary>
        /// INN等対象 33
        /// </summary>
        public NacCollumnAN INN_TAISHO = new NacCollumnAN(1);
        /// <summary>
        /// 数量１ 34
        /// </summary>
        public NacCollumnN SURYO1 = new NacCollumnN(12);
        /// <summary>
        /// 数量単位コード１ 35
        /// </summary>
        public NacCollumnAN SURYO_TANI_CD1 = new NacCollumnAN(4);
        /// <summary>
        /// 数量２ 36
        /// </summary>
        public NacCollumnN SURYO2 = new NacCollumnN(12);
        /// <summary>
        /// 数量単位コード２ 37
        /// </summary>
        public NacCollumnAN SURYO_TANI_CD2 = new NacCollumnAN(4);
        /// <summary>
        /// 課税価格 38
        /// </summary>
        public NacCollumnN KAZEI_KAKAKU = new NacCollumnN(13);
        /// <summary>
        /// 内国消費税等種別コード 39-1
        /// </summary>
        public NacCollumnAN NAIKOKU_SHOHIZEITOU_SHUBETSU_CD1 = new NacCollumnAN(10);
        /// <summary>
        /// 内国消費税等種別コード 39-2
        /// </summary>
        public NacCollumnAN NAIKOKU_SHOHIZEITOU_SHUBETSU_CD2 = new NacCollumnAN(10);
        
        public int GetByteLength()
        {
            int intlen = 0;
            intlen += KANZEI_TAISHO.GetByteLength();
            intlen += SHOHIN_KANRI_CD.GetByteLength();
            intlen += HINMEI.GetByteLength();
            intlen += HANBAI_TANKA.GetByteLength();
            intlen += HANBAI_SURYO.GetByteLength();
            intlen += HINMOKU_CD.GetByteLength();
            intlen += NACCS_CD.GetByteLength();
            intlen += GENZANCHI_CD.GetByteLength();
            intlen += GENZANCHI_SHOMEI_SHIKIBETSU.GetByteLength();
            intlen += INN_TAISHO.GetByteLength();
            intlen += SURYO1.GetByteLength();
            intlen += SURYO_TANI_CD1.GetByteLength();
            intlen += SURYO2.GetByteLength();
            intlen += SURYO_TANI_CD2.GetByteLength();
            intlen += KAZEI_KAKAKU.GetByteLength();
            intlen += NAIKOKU_SHOHIZEITOU_SHUBETSU_CD1.GetByteLength();
            intlen += NAIKOKU_SHOHIZEITOU_SHUBETSU_CD2.GetByteLength();

            return intlen;


    //        //TestClassクラスのTypeオブジェクトを取得する
    //        Type t = typeof(ShohinModel);

    //        //メンバを取得する
    //        MemberInfo[] members = t.GetMembers(
    //BindingFlags.Public | BindingFlags.NonPublic |
    //BindingFlags.Instance | BindingFlags.Static |
    //BindingFlags.DeclaredOnly
    //            );
    //        foreach (MemberInfo m in members)
    //        {
    //            //if (typeof(INaccsCollumn).IsAssignableFrom(typeof(m)))
    //            //{
    //                // プロパティ情報の取得
    //                var property = typeof(INaccsCollumn).GetProperty("Max");

    //                //// インスタンスの値を取得
    //                //var beforeName = property.GetValue(m);

    //                //((INaccsCollumn)m).Max;
    //                //メンバの型と、名前を表示する
    //                Console.WriteLine("{0} - {1}", m.MemberType, m.Name);
    //            //}
    //        }
    //        return 1;
        }
        /// <summary>
        /// 添付用データの作成
        /// </summary>
        /// <returns></returns>
        public override byte[] GetByteData()
        {
            int intLast = this.GetByteLength();
            byte[] btData = new byte[intLast];
            int pos = 0;
            try
            {
                Array.Copy(this.KANZEI_TAISHO.GetByteData(), 0, btData, pos, this.KANZEI_TAISHO.GetByteLength());
                pos += KANZEI_TAISHO.GetByteLength();

                Array.Copy(this.SHOHIN_KANRI_CD.GetByteData(), 0, btData, pos, this.SHOHIN_KANRI_CD.GetByteLength());
                pos += SHOHIN_KANRI_CD.GetByteLength();

                Array.Copy(this.HINMEI.GetByteData(), 0, btData, pos, this.HINMEI.GetByteLength());
                pos += HINMEI.GetByteLength();

                Array.Copy(this.HANBAI_TANKA.GetByteData(), 0, btData, pos, this.HANBAI_TANKA.GetByteLength());
                pos += HANBAI_TANKA.GetByteLength();

                Array.Copy(this.HANBAI_SURYO.GetByteData(), 0, btData, pos, this.HANBAI_SURYO.GetByteLength());
                pos += HANBAI_SURYO.GetByteLength();

                Array.Copy(this.HINMOKU_CD.GetByteData(), 0, btData, pos, this.HINMOKU_CD.GetByteLength());
                pos += HINMOKU_CD.GetByteLength();

                Array.Copy(this.NACCS_CD.GetByteData(), 0, btData, pos, this.NACCS_CD.GetByteLength());
                pos += NACCS_CD.GetByteLength();

                Array.Copy(this.GENZANCHI_CD.GetByteData(), 0, btData, pos, this.GENZANCHI_CD.GetByteLength());
                pos += GENZANCHI_CD.GetByteLength();

                Array.Copy(this.GENZANCHI_SHOMEI_SHIKIBETSU.GetByteData(), 0, btData, pos, this.GENZANCHI_SHOMEI_SHIKIBETSU.GetByteLength());
                pos += GENZANCHI_SHOMEI_SHIKIBETSU.GetByteLength();

                Array.Copy(this.INN_TAISHO.GetByteData(), 0, btData, pos, this.INN_TAISHO.GetByteLength());
                pos += INN_TAISHO.GetByteLength();

                Array.Copy(this.SURYO1.GetByteData(), 0, btData, pos, this.SURYO1.GetByteLength());
                pos += SURYO1.GetByteLength();

                Array.Copy(this.SURYO_TANI_CD1.GetByteData(), 0, btData, pos, this.SURYO_TANI_CD1.GetByteLength());
                pos += SURYO_TANI_CD1.GetByteLength();

                Array.Copy(this.SURYO2.GetByteData(), 0, btData, pos, this.SURYO2.GetByteLength());
                pos += SURYO2.GetByteLength();

                Array.Copy(this.SURYO_TANI_CD2.GetByteData(), 0, btData, pos, this.SURYO_TANI_CD2.GetByteLength());
                pos += SURYO_TANI_CD2.GetByteLength();

                Array.Copy(this.KAZEI_KAKAKU.GetByteData(), 0, btData, pos, this.KAZEI_KAKAKU.GetByteLength());
                pos += KAZEI_KAKAKU.GetByteLength();

                Array.Copy(this.NAIKOKU_SHOHIZEITOU_SHUBETSU_CD1.GetByteData(), 0, btData, pos, this.NAIKOKU_SHOHIZEITOU_SHUBETSU_CD1.GetByteLength());
                pos += NAIKOKU_SHOHIZEITOU_SHUBETSU_CD1.GetByteLength();

                Array.Copy(this.NAIKOKU_SHOHIZEITOU_SHUBETSU_CD2.GetByteData(), 0, btData, pos, this.NAIKOKU_SHOHIZEITOU_SHUBETSU_CD2.GetByteLength());
                pos += NAIKOKU_SHOHIZEITOU_SHUBETSU_CD2.GetByteLength();

                return btData;
            }
            catch (Exception e)
            {
                LogOut.ErrorOut(e.ToString(), this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                throw e;
            }
        }

    }

    #endregion "商品モデル"

}
