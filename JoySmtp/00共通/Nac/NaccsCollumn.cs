using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using JoySmtp.CLogOut;

namespace JoySmtp.Nac
{

    interface INaccs
    {
        /// <summary>
        /// 添付用データの作成
        /// </summary>
        /// <returns></returns>
        byte[] GetByteData();
        /// <summary>
        /// 取得データから設定
        /// </summary>
        /// <returns></returns>
        Boolean SetByteData(byte[] btData);
    }

    public class NaccsModel : INaccs
    {
        public System.Text.Encoding EUC = System.Text.Encoding.GetEncoding(51932);
        /// <summary>
        /// 送信用データの作成
        /// </summary>
        /// <returns></returns>
        public virtual byte[] GetByteData()
        {
            return null;
        }
        /// <summary>
        /// バイトデータから内容の取得
        /// </summary>
        /// <param name="btData"></param>
        /// <returns></returns>
        public virtual Boolean SetByteData(byte[] btData)
        {
            return false;
        }
        /// <summary>
        /// 排他制御　WHERE句を必ず記述すること
        /// </summary>
        /// <param name="strTblName">テーブル名</param>
        /// <param name="strWhere">Where句</param>
        /// <param name="blnHaita">true:排他する　false:排他解除</param>
        /// <returns></returns>
        public static string GetSQL_HAITA(string strTblName, string strWhere, Boolean blnHaita = false)
        {
            StringBuilder strSql = new StringBuilder();
            if (string.IsNullOrWhiteSpace(strWhere))
            {
                strSql.AppendFormat("UPDATE {0} SET", strTblName);
                if (blnHaita)
                {
                    //書き込み禁止にする
                    strSql.AppendFormat("  HAITA_FLG = 1", "").AppendLine();
                    strSql.AppendFormat("  ,HAITA_NICHIJI = GETDATE()", "").AppendLine();
                    strSql.AppendFormat("  ,HAITA_USER = '{0}'",Environment.MachineName).AppendLine();
                }
                else
                {
                    //解除する
                    strSql.AppendLine("  HAITA_FLG = NULL");
                    strSql.AppendLine("  ,HAITA_NICHIJI = NULL");
                    strSql.AppendLine("  ,HAITA_USER = NULL");
                }
                strSql.AppendLine("WHERE 1=1");

                strSql.AppendFormat("  AND EDI_SENDDATE IS NULL").AppendLine();
                strSql.AppendFormat("  AND DELETE_FLG IS NULL").AppendLine();

                strSql.AppendLine(strWhere);
            }
            return strSql.ToString();
        }
    }
    /// <summary>
    /// NACCS送信用モデル
    /// </summary>
    public class NaccsSendModel : NaccsModel
    {
        public NACCS_REPORTTYPE ReportType = NACCS_REPORTTYPE.NONE;
        /// <summary>
        /// 入力共通項目398
        /// </summary>
        public InputCommonModel NaccsCommon = new InputCommonModel();
        public string GetHandover { get { return NaccsCommon.DENBUN_HANDOVER.Data.ToString(); } }
        public string GetInfoNo { get { return NaccsCommon.INPUT_INFO_NO.Data.ToString(); } }

        public static List<NACCS_REPORTTYPE> SetRecvRequiredList(NACCS_REPORTTYPE report, Boolean ShoriFlg = false)
        {
            List<NACCS_REPORTTYPE> list = new List<NACCS_REPORTTYPE>();
            switch(report)
            {
                case NACCS_REPORTTYPE.OTA:
                    list.Add(NACCS_REPORTTYPE.RESULT);
                    list.Add(NACCS_REPORTTYPE.SAD4011);
                    if (ShoriFlg)
                    {
                        list.Add(NACCS_REPORTTYPE.SAD4031);
                        list.Add(NACCS_REPORTTYPE.SAD4071);
                        list.Add(NACCS_REPORTTYPE.SAD4081);
                    }
                    break;
                case NACCS_REPORTTYPE.OTA01:
                    list.Add(NACCS_REPORTTYPE.RESULT);
                    list.Add(NACCS_REPORTTYPE.SAD4021);
                    list.Add(NACCS_REPORTTYPE.SAD4041);
                    if (ShoriFlg)
                    {
                        list.Add(NACCS_REPORTTYPE.SAD4091);
                        list.Add(NACCS_REPORTTYPE.SAD4101);
                        list.Add(NACCS_REPORTTYPE.SAD4131);
                    }
                    break;
                case NACCS_REPORTTYPE.OTC:
                    list.Add(NACCS_REPORTTYPE.RESULT);
                    list.Add(NACCS_REPORTTYPE.SAD4011);
                    list.Add(NACCS_REPORTTYPE.SAD4031);
                    if (ShoriFlg)
                    {
                        list.Add(NACCS_REPORTTYPE.SAD4071);
                        list.Add(NACCS_REPORTTYPE.SAD4081);
                        list.Add(NACCS_REPORTTYPE.SAD4131);
                    }
                    break;
                case NACCS_REPORTTYPE.SYG_LIST:
                case NACCS_REPORTTYPE.SYG_RESULT:
                    list.Add(NACCS_REPORTTYPE.RESULT);
                    break;
                default:
                    break;
            }
            return list;
        }
    }
    /// <summary>
    /// NACCS受信用モデル
    /// </summary>
    public class NaccsRecvModel : NaccsModel
    {
        //public NACCS_REPORTTYPE ReportType = NACCS_REPORTTYPE.NONE;
        protected const string LOGSTR = "RECV"; 
        /// <summary>
        /// 出力共通項目398
        /// </summary>
        public OutputCommonModel NaccsCommon = new OutputCommonModel();
        /// <summary>
        /// 電文引継ぎ情報の文字列取得
        /// </summary>
        public string GetHandover { get { return NaccsCommon.DENBUN_HANDOVER.Data.ToString(); } }
        /// <summary>
        /// 入力情報特定番号の文字列取得
        /// </summary>
        public string GetInfoNo { get { return NaccsCommon.INPUT_INFO_NO.Data.ToString(); } }
        /// <summary>
        /// ファイル名の取得
        /// </summary>
        /// <returns></returns>
        public virtual string GetFileName()
        {
            return this.NaccsCommon.GetFileName();
        }
        /// <summary>
        /// 輸入申告番号
        /// </summary>
        public NacCollumnAN YunyuShinkokuNo = new NacCollumnAN(11);
        /// <summary>
        /// 輸入申告番号の文字列取得
        /// </summary>
        public string GetYunyuShinkokuNo { get { return YunyuShinkokuNo.Data; } }
        /// <summary>
        /// レポートタイプの取得
        /// </summary>
        public NACCS_REPORTTYPE GetReportType
        {
            get {
                return this.NaccsCommon.REPORT_TYPE;
            }            
        }
        /// <summary>
        /// ファイル用の文字列※※※継承させること
        /// </summary>
        /// <returns></returns>
        public virtual string GetFileData()
        {
            try
            {
                var btdata = this.GetByteData();
                return System.Text.Encoding.GetEncoding(51932).GetString(btdata);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    /// <summary>
    /// 項目用のインターフェース
    /// </summary>
    interface INaccsCollumn : INaccs 
    {
        //string Name { }
        /// <summary>
        /// 表示桁数
        /// </summary>
        int Max { get;}
        /// <summary>
        /// 表示型
        /// </summary>
        Type DType { get;}
        /// <summary>
        /// NACCSで使用するデータタイプ
        /// </summary>
        NACCS_CHARTYPE NaccsType { get; }
        /// <summary>
        /// 内容
        /// </summary>
        string Data { get; }
        /// <summary>
        /// データの最後にCRLFを追加するかどうか？
        /// </summary>
        Boolean ReturnFlg { get; }
        /// <summary>
        /// 改行含めたデータ長
        /// </summary>
        /// <returns></returns>
        int GetByteLength();
        /// <summary>
        /// 内容のセット
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool SetData(string obj);

        ///// <summary>
        ///// ファイル用の文字列取得
        ///// </summary>
        ///// <returns></returns>
        //string GetFileData();
    }
    public enum NACCS_CHARTYPE
    {
        n,
        an,
        sn,
        j,
        anr,
        date,
        other
    }
    /// <summary>
    /// 電文の種類
    /// </summary>
    public enum NACCS_REPORTTYPE
    {
        NONE,
        /// <summary>
        /// 輸入申告登録
        /// </summary>
        OTA,
        /// <summary>
        /// 輸入申告登録
        /// </summary>
        OTA01,
        /// <summary>
        /// 輸入申告
        /// </summary>
        OTC,
        /// <summary>
        /// SYGのList結果
        /// </summary>
        SYG_LIST,
        /// <summary>
        /// SYGのデータ結果取得
        /// </summary>
        SYG_RESULT,
        /// <summary>
        /// その他（TEST用）
        /// </summary>
        OTHER,
        /// <summary>
        /// 処理結果通知  ＊ＳＯＴＡ   ○   △   R
        /// </summary>
        RESULT,
        /// <summary>
        /// 輸入申告入力控（沖縄特免制度）情報   ＳＡＤ４０１１ ○   C
        /// </summary>
        SAD4011,
        /// <summary>
        /// 輸入申告控（沖縄特免制度）情報 ＳＡＤ４０３１ ○   P
        /// </summary>
        SAD4031,
        /// <summary>
        /// 輸入許可前貨物引取承認申請控（沖縄特免制度）情報    ＳＡＤ４０４１ ○   P
        /// </summary>
        SAD4041,
        /// <summary>
        /// 輸入許可通知兼申告控（沖縄特免制度）情報    ＳＡＤ４０７１ ○   P
        /// </summary>
        SAD4071,
        /// <summary>
        /// 輸入許可前貨物引取承認通知兼申請控（沖縄特免制度）情報 ＳＡＤ４０８１ ○   P
        /// </summary>
        SAD4081,
        /// <summary>
        /// 納付書情報（直納）   ＳＡＦ００１０ ○   P
        /// </summary>
        SAF0010,
        /// <summary>
        /// 許可・承認貨物（沖縄特免制度）情報   ＳＡＤ４１３１ ○   ○   P
        /// </summary>
        SAD4131,
        /// <summary>
        /// 口座使用不可通知情報
        /// </summary>
        SAF0211,
        /// <summary>
        /// 担保不足通知情報    ＳＡＦ０２２１ ○   P
        /// </summary>
        SAF0221,
        /// <summary>
        /// 納付番号通知情報    ＳＡＦ００２１ ○   P
        /// </summary>
        SAF0021,
        /// <summary>
        /// 輸入申告変更入力控（沖縄特免制度）情報
        /// </summary>
        SAD4021,
        /// <summary>
        /// 輸入申告変更控（沖縄特免制度）情報
        /// </summary>
        SAD4051,
        /// <summary>
        /// 輸入許可前貨物引取承認申請変更控（沖縄特免制度）情報
        /// </summary>
        SAD4061,
        /// <summary>
        /// 輸入許可通知兼申告変更控（沖縄特免制度）情報
        /// </summary>
        SAD4091,
        /// <summary>
        /// 輸入許可前貨物引取承認通知兼申請変更控（沖縄特免制度）情報
        /// </summary>
        SAD4101,
        /// <summary>
        /// 輸入許可情報
        /// </summary>
        SAD4111
    }

    /// <summary>
    /// NaccsCollumn
    /// </summary>
    public class NaccsCollumn : INaccsCollumn
    {
        public int Max { get; protected set; }
        public Type DType { get; protected set; }
        public string Data { get; protected set; }
        public NACCS_CHARTYPE NaccsType { get; protected set; }
        public Boolean ReturnFlg { get; protected set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="maxlen"></param>
        /// <param name="data"></param>
        /// <param name="flg"></param>
        public NaccsCollumn(int maxlen, string data, Boolean flg = true)
        {
            Max = maxlen;
            DType = Type.GetType("System.String");
            NaccsType = NACCS_CHARTYPE.j;
            SetData(data);
            ReturnFlg = flg;
        }
        /// <summary>
        /// 設定文字数の長さ
        /// </summary>
        /// <returns></returns>
        public int GetByteLength()
        {
            return this.ReturnFlg ? this.Max + 2 : this.Max;
        }
        /// <summary>
        /// 値の設定
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual bool SetData(string obj)
        {
            string strRet = "";
            if (string.IsNullOrWhiteSpace(obj))
            {
                Data = "";
                return true;
            }
            else if (IsRegexData(obj.Trim(), out strRet))
            {
                if (strRet.Length <= Max)
                {
                    Data = strRet;
                    return true;
                }
                else
                {
                    Data = strRet.Substring(0, Max);
                    LogOut.ErrorOut("設定値入力警告（文字数OVER）["+obj.ToString()+"]", this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                    return true;
                }
            }
            else
            {
                Data = "";
                LogOut.ErrorOut("設定値入力異常（正規表現チェック）[" + obj.ToString() + "]", this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                return false;
            }            
        }
        /// <summary>
        /// データの正規表現
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual bool IsRegexData(string target, out string strRet)
        {
            strRet = target;
            return true;
        }
        /// <summary>
        /// 送信用データの作成
        /// </summary>
        /// <returns></returns>
        public virtual byte[] GetByteData()
        {
            try
            {
                if (Max > 0)
                {
                    return GetByteLeft((string)Data);
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
        /// <summary>
        /// 右詰めデータの作成
        /// </summary>
        /// <param name="byteData"></param>
        /// <returns></returns>
        protected  byte[] GetByteRight(string byteData)
        {
            byte[] bytRet = null;
            bytRet = new Byte[this.GetByteLength()];
            if (Max > 0)
            {
                byte[] arr = System.Text.Encoding.GetEncoding(51932).GetBytes(byteData);
                int i;
                for (i = 0; i < Max; i++)
                {
                    if ((Max - arr.Length) > i)
                    {
                        bytRet[i] = 0x20;
                    }
                    else
                    {
                        bytRet[i] = arr[i - (Max - arr.Length)];
                    }
                }
                if (ReturnFlg)
                {
                    bytRet[i++] = 0x0D;
                    bytRet[i++] = 0x0A;
                }
            }
            return bytRet;
        }
        /// <summary>
        /// 左詰めデータの作成
        /// </summary>
        /// <returns></returns>
        protected  byte[] GetByteLeft(string byteData)
        {
            byte[] bytRet = null;
            bytRet = new Byte[this.GetByteLength()];
            if (Max > 0)
            {
                int i;
                bytRet = new Byte[this.GetByteLength()];
                byte[] arr = System.Text.Encoding.GetEncoding(51932).GetBytes(byteData);
                for (i = 0; i < Max; i++)
                {
                    if (i < arr.Length)
                    {
                        bytRet[i] = arr[i];
                    }
                    else
                    {
                        bytRet[i] = 0x20;
                    }
                }
                if (ReturnFlg)
                {
                    bytRet[i++] = 0x0D;
                    bytRet[i++] = 0x0A;
                }
            }
            return bytRet;
        }
        /// <summary>
        /// バイトデータから内容の取得
        /// </summary>
        /// <param name="btData"></param>
        /// <returns></returns>
        public Boolean SetByteData(byte[] btData)
        {
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding(51932);

            try
            {
                string str = enc.GetString(btData);
                return this.SetData(str);
            }
            catch (Exception e)
            {
                LogOut.ErrorOut(e.ToString(), this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                throw e;
            }
        }
    }
    /// <summary>
    /// 半角数字のみ（表記：n）右詰
    /// </summary>
    public class NacCollumnN : NaccsCollumn
    {
        /// <summary>
        /// 小数点の有効桁数
        /// </summary>
        public int Limit { get; private set; }
        public NacCollumnN(int maxlen, int limit = 0, Boolean flg= true) : this(maxlen, "", limit, flg) { }
        public NacCollumnN(int maxlen, string data, int limit = 0, Boolean flg = true)
            : base(maxlen, data, flg)
        {
            Limit = limit;
            DType = Type.GetType("System.Double");
            NaccsType = NACCS_CHARTYPE.n;
        }
        /// <summary>
        /// データの正規表現
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool IsRegexData(string target, out string strRet)
        {
            strRet = "";
            if (new Regex("^[\x0A\x0d\x20-\x23\x25-\x40]+$").IsMatch(target))
            {
                strRet = target;
                decimal dec;
                if (decimal.TryParse(target, out dec))
                {
                    strRet = dec.ToString(string.Format("F{0}",Limit));
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// データの取得
        /// </summary>
        /// <returns></returns>
        public override byte[] GetByteData()
        {
            if (Max > 0)
            {
                Decimal dec;
                if (decimal.TryParse(Data, out dec))
                {
                    Decimal data = Math.Round(dec, Limit, MidpointRounding.AwayFromZero);
                    return base.GetByteRight(data.ToString());
                }
                return base.GetByteRight("");
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 整数型を取得
        /// </summary>
        public int GetIntData
        {
            get
            {
                int n;
                if (int.TryParse(this.Data, out n))
                {
                    return n;
                }
                return 0;
            }
        }
        /// <summary>
        /// 小数点の数値を取得
        /// </summary>
        public Double GetDoubleData
        {
            get
            {
                Double dbl;
                if (Double.TryParse(this.Data, out dbl))
                {
                    return dbl;
                }
                return 0;
            }
        }
    }
    /// <summary>
    /// 0埋め数字（表記：n）右詰 整数
    /// </summary>
    public class NacCollumnNLEN : NaccsCollumn
    {
        /// <summary>
        /// 有効桁数
        /// </summary>
        public int Digits { get; private set; }
        public int GetIntData
        {
            get
            {
                int n;
                if (int.TryParse(this.Data, out n))
                {
                    return n;
                }
                return 0;
            }
        }


        public NacCollumnNLEN(int maxlen, int limit = 0, Boolean flg = true) : this(maxlen, "", limit, flg) { }
        public NacCollumnNLEN(int maxlen, string data, int digits = 0, Boolean flg = true)
            : base(maxlen, data, flg)
        {
            if (digits == 0)
            {
                Digits = maxlen;
            }
            else
            {
                Digits = digits;
            }
            DType = Type.GetType("System.Int32");
            NaccsType = NACCS_CHARTYPE.n;
        }
        /// <summary>
        /// データの正規表現
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool IsRegexData(string target, out string strRet)
        {
            strRet = "";
            if (new Regex("^[\x0A\x0d\x20-\x23\x25-\x40]+$").IsMatch(target))
            {
                strRet = target;
                UInt32 dec;
                if (UInt32.TryParse(target, out dec))
                {
                    var str = "{0:D" + Digits.ToString() + "}";
                    strRet = String.Format(str, dec);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// データの取得
        /// </summary>
        /// <returns></returns>
        public override byte[] GetByteData()
        {
            if (Max > 0)
            {
                UInt32 dec;
                if (UInt32.TryParse(Data, out dec))
                {
                    var str = "{0:D" + Digits.ToString() + "}";
                    return base.GetByteRight(String.Format(str, dec));
                }
                return base.GetByteRight("");
            }
            else
            {
                return null;
            }
        }
    }
    /// <summary>
    /// 0埋め数字（表記：n）左詰 整数
    /// </summary>
    public class NacCollumnNPD : NaccsCollumn
    {
        /// <summary>
        /// 有効桁数
        /// </summary>
        public int Digits { get; private set; }
        public int GetIntData
        {
            get
            {
                int n;
                if (int.TryParse(this.Data, out n))
                {
                    return n;
                }
                return 0;
            }
        }


        public NacCollumnNPD(int maxlen, int limit = 0, Boolean flg = true) : this(maxlen, "", limit, flg) { }
        public NacCollumnNPD(int maxlen, string data, int digits = 0, Boolean flg = true)
            : base(maxlen, data, flg)
        {
            if (digits == 0)
            {
                Digits = maxlen;
            }
            else
            {
                Digits = digits;
            }
            DType = Type.GetType("System.Int32");
            NaccsType = NACCS_CHARTYPE.n;
        }
        /// <summary>
        /// データの正規表現
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool IsRegexData(string target, out string strRet)
        {
            strRet = "";
            if (new Regex("^[\x0A\x0d\x20-\x23\x25-\x40]+$").IsMatch(target))
            {
                strRet = target;
                UInt32 dec;
                if (UInt32.TryParse(target, out dec))
                {
                    var str = "{0:D" + Digits.ToString() + "}";
                    strRet = String.Format(str, dec);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// データの取得
        /// </summary>
        /// <returns></returns>
        public override byte[] GetByteData()
        {
            if (Max > 0)
            {
                UInt32 dec;
                if (UInt32.TryParse(Data, out dec))
                {
                    var str = "{0:D" + Digits.ToString() + "}";
                    return base.GetByteLeft(String.Format(str, dec));
                }
                return base.GetByteLeft("");
            }
            else
            {
                return null;
            }
        }
    }
    /// <summary>
    /// 半角数字のみ（表記：n）日時
    /// </summary>
    public class NacCollumnDate : NaccsCollumn
    {
        public DateTime DataDatetime { get; protected set; }
        public NacCollumnDate(int maxlen, Boolean flg = true) : this(maxlen, "",flg) { }
        public NacCollumnDate(int maxlen, string data, Boolean flg = true) : base(maxlen, data, flg)
        {
            DType = Type.GetType("System.DateTime");
            NaccsType = NACCS_CHARTYPE.date;
        }
        /// <summary>
        /// データの正規表現
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool IsRegexData(string target, out string strRet)
        {
            strRet = "";
            DateTime dt;
            string str;

            if (new Regex(@"^\d{4}\d{2}\d{2}[\d|\s]*$").IsMatch(target))
            {
                str = target.Substring(0, 4) + "/" + target.Substring(4, 2) + "/" + target.Substring(6, 2);
            }
            else
            {
                str = target.Trim();
            }

            if (DateTime.TryParse(str, out dt))
            {
                this.DataDatetime = dt;
                strRet = target;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// データの取得
        /// </summary>
        /// <returns></returns>
        public override byte[] GetByteData()
        {
            if (Max > 0)
            {
                if (!string.IsNullOrWhiteSpace(this.Data))
                {
                    if (this.DataDatetime != null)
                    {
                        return base.GetByteLeft(this.DataDatetime.ToString("yyyyMMdd"));
                    }
                    else
                    {
                        return base.GetByteLeft(this.Data);
                    }
                }
                else
                {
                    return base.GetByteLeft("");
                }
            }
            else
            {
                return null;
            }
         }
    }
    /// <summary>
    /// 半角数字のみ（表記：n）日時
    /// </summary>
    public class NacCollumnDateTime : NaccsCollumn
    {
        public DateTime DataDatetime { get; protected set; }
        public NacCollumnDateTime(int maxlen, Boolean flg = true) : this(maxlen, "", flg) { }
        public NacCollumnDateTime(int maxlen, string data, Boolean flg = true)
            : base(maxlen, data, flg)
        {
            DType = Type.GetType("System.DateTime");
            NaccsType = NACCS_CHARTYPE.date;
            this.DataDatetime = DateTime.MinValue;
        }
        /// <summary>
        /// データの正規表現
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool IsRegexData(string target, out string strRet)
        {
            strRet = "";
            DateTime dt;
            string str;

            if (new Regex(@"^\d{4}\d{2}\d{2}\d{2}\d{2}[\d|\s]*$").IsMatch(target))
            {
                str = target.Substring(0, 4) + "/" + target.Substring(4, 2) + "/" + target.Substring(6, 2) + " " +
                target.Substring(8, 2) + ":" + target.Substring(10, 2) + ":00";
            }
            else
            {
                str = target.Trim();
            }

            if (DateTime.TryParse(str, out dt))
            {
                this.DataDatetime = dt;
                strRet = target;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// データの取得
        /// </summary>
        /// <returns></returns>
        public override byte[] GetByteData()
        {
            if (Max > 0)
            {
                DateTime dt;
                if (this.DataDatetime != null)
                {
                    return base.GetByteLeft(this.DataDatetime.ToString("yyyyMMddhhmm"));
                }
                //else if (DateTime.TryParse(this.Data, out dt))
                //{
                //    return base.GetByteLeft(dt.ToString("yyyyMMddhhmm"));
                //}
                else
                {
                    return base.GetByteLeft(this.Data);
//                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
    /// <summary>
    /// 全角+半角英数字+記号（表記：j）
    /// </summary>
    public class NacCollumnJ : NaccsCollumn
    {
        public NacCollumnJ(int maxlen, Boolean flg = true) : this(maxlen, "", flg) { }
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="maxlen"></param>
        /// <param name="data"></param>
        public NacCollumnJ(int maxlen, string data, Boolean flg = true)
            : base(maxlen, data, flg)
        {
            DType = Type.GetType("System.String");
            NaccsType = NACCS_CHARTYPE.j;
        }
        /// <summary>
        /// データの正規表現
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool IsRegexData(string target, out string strRet)
        {
            strRet = target;
            return true;
        }
        public override byte[] GetByteData()
        {
            return base.GetByteData();
        }
    }
    /// <summary>
    /// 全角+半角英数字+記号JISコード（表記：j）
    /// </summary>
    public class NacCollumnJJ : NaccsCollumn
    {
        public static byte[] JIS_START = new byte[] { 0x1b, 0x24, 0x42 };
        public static byte[] JIS_END = new byte[] { 0x1b, 0x28, 0x42 };

        public NacCollumnJJ(int maxlen, Boolean flg = true) : this(maxlen, "", flg) { }
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="maxlen"></param>
        /// <param name="data"></param>
        public NacCollumnJJ(int maxlen, string data, Boolean flg = true)
            : base(maxlen, data, flg)
        {
            DType = Type.GetType("System.String");
            NaccsType = NACCS_CHARTYPE.j;
        }
        /// <summary>
        /// データの正規表現
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool IsRegexData(string target, out string strRet)
        {
            strRet = target;

            byte[] bytData = System.Text.Encoding.GetEncoding(50220).GetBytes(strRet);
            if (bytData[0] == JIS_START[0] && bytData[1] == JIS_START[1] && bytData[2] == JIS_START[2])
            {
                strRet = System.Text.Encoding.GetEncoding(50220).GetString(bytData);
            }
            else
            {
                byte[] bytRet = new byte[bytData.Length + 6];

                Array.Copy(JIS_START, 0, bytRet, 0, JIS_START.Length);
                Array.Copy(bytData, 0, bytRet, 3, bytData.Length);
                Array.Copy(JIS_END, 0, bytRet, (JIS_START.Length + bytData.Length), JIS_END.Length);

                strRet = System.Text.Encoding.GetEncoding(50220).GetString(bytRet);
            }
            return true;
        }
        public override byte[] GetByteData()
        {
            return base.GetByteData();
        }
    }
    /// <summary>
    /// 半角英数字+記号※英小文字不可（表記：an）左詰
    /// </summary>
    public class NacCollumnAN : NaccsCollumn
    {
        public override bool IsRegexData(string target, out string strRet)
        {
            strRet = "";
            if (new Regex(@"^[\x0A\x0d\x20-\x23\x25-\x5A]+$").IsMatch(target))
            {
                strRet = target;
                return true;
            }
            else return false;
        }
        public NacCollumnAN(int maxlen, Boolean flg = true) : this(maxlen, "", flg) { }
        public NacCollumnAN(int maxlen, string data, Boolean flg = true)
            : base(maxlen, data, flg)
        {
            NaccsType = NACCS_CHARTYPE.an;  
        }
    }
    /// <summary>
    /// 半角英数字+記号※英小文字可（表記：sn）左詰
    /// </summary>
    public class NacCollumnSN : NaccsCollumn
    {
        public override bool IsRegexData(string target, out string strRet)
        {
            strRet = "";
            if (new Regex("^[\x0A\x0d\x20-\x7E]+$").IsMatch(target))
            {
                strRet = target;
                return true;
            }
            else return false;

        }
        public NacCollumnSN(int maxlen, Boolean flg = true) : this(maxlen, "", flg) { }
        public NacCollumnSN(int maxlen, string data, Boolean flg = true)
            : base(maxlen, data, flg)
        {
            NaccsType = NACCS_CHARTYPE.sn;
        }
    }
    /// <summary>
    /// 受信用　半角英数字+記号※英小文字不可（表記：an）左詰
    /// </summary>
    public class NacCollumnANR : NaccsCollumn
    {
        public override bool IsRegexData(string target, out string strRet)
        {
            strRet = "";
            if (new Regex("^[\x0A\x0d\x20-\x5F]+$").IsMatch(target))
            {
                strRet = target;
                return true;
            }
            else return false;
        }
        public NacCollumnANR(int maxlen, Boolean flg = true) : this(maxlen, "", flg) { }
        public NacCollumnANR(int maxlen, string data, Boolean flg = true)
            : base(maxlen, data, flg)
        {
            NaccsType = NACCS_CHARTYPE.anr;
        }
    }
    /// <summary>
    /// リザーブ
    /// </summary>
    public class NacCollumnReserve : NaccsCollumn
    {
        public override bool IsRegexData(string target, out string strRet)
        {
            strRet = "";
            if (new Regex(@"^[\x0A\x0d\x20-\x23\x25-\x5A]+$").IsMatch(target))
            {
                strRet = target;
                return true;
            }
            else return false;
        }
        public NacCollumnReserve(int maxlen, Boolean flg = true) : this(maxlen, "", flg) { }
        public NacCollumnReserve(int maxlen, string data, Boolean flg = true)
            : base(maxlen, data, flg)
        {
            NaccsType = NACCS_CHARTYPE.an;
        }
        /// <summary>
        /// 値の設定
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool SetData(string obj)
        {
            string strRet = "";
            if (string.IsNullOrWhiteSpace(obj))
            {
                Data = "";
                return true;
            }
            else
            {
                if (obj.Length <= Max)
                {
                    Data = obj;
                    return true;
                }
                else
                {
                    Data = obj.Substring(0, Max);
                    LogOut.ErrorOut("設定値入力警告（文字数OVER）[" + obj.ToString() + "]", this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                    return true;
                }
            }
        }
    }
    /// <summary>
    /// 半角数字のみ（表記：n）フラグ
    /// </summary>
    public class NacCollumnFLG : NaccsCollumn
    {
        public NacCollumnFLG(int maxlen, Boolean flg = true) : this(maxlen, "", flg) { }
        public NacCollumnFLG(int maxlen, string data, Boolean flg = true)
            : base(maxlen, data, flg)
        {
            Max = 1;
            NaccsType = NACCS_CHARTYPE.an;
        }
        /// <summary>
        /// データの正規表現
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool IsRegexData(string target, out string strRet)
        {
            strRet = "";
            if (target == "1" || target == "*" || target == "true")
            {
                strRet = "1";
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// データの取得
        /// </summary>
        /// <returns></returns>
        public override byte[] GetByteData()
        {
            if (Max > 0)
            {
                if (this.Data == "1")
                {
                    return base.GetByteRight("*");
                }
                else
                {
                    return base.GetByteRight("");
                }
            }
            else
            {
                return null;
            }
        }
    }
}
