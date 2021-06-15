using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using JoySmtp.JoyCommon;
using JoySmtp.CLogOut;
using JoySmtp.Data;
using JoySmtp.DataBase;


namespace JoySmtp.Nac
{
    /// <summary>
    /// 処理結果モデル
    /// 業務仕様書番号:7092 業務コード:ROT 処理結果通知 ＊ＣＲＯＴ R
    /// </summary>
    public class ProcessResultModel : NaccsRecvModel 
    {
        public const string RESULT_SUCCESS = "00000-0000-0000";

        /// <summary>
        /// 処理結果コードchar75
        /// </summary>
        public string OUTPUT_RESULT {
            get
            {
                string str = "";
                foreach (var ret in this.ResultList)
                {
                    str += ret.Data;
                }
                return str;
            }
        }
        /// <summary>
        /// 正常終了時にTRUEになる。
        /// </summary>
        public Boolean RESULT_FLG {
            get
            {
                if(this.ResultList.Count>0)
                {
                    if(this.ResultList[0].Data.Equals(RESULT_SUCCESS))
                    {
                        return true;
                    }
                }
                return false; 
            }
        }

        ///// <summary>
        ///// 出力共通項目
        ///// </summary>
        //public OutputCommonModel OutputCommon = new OutputCommonModel();
        /// <summary>
        /// 処理結果コード
        /// </summary>
        public List<NacCollumnAN> ResultList = new List<NacCollumnAN>();
        /// <summary>
        /// 処理結果コードの最大数
        /// </summary>
        public static int RESULT_LIST_MAX = 5;
        ///// <summary>
        ///// 輸入申告番号
        ///// </summary>
        //public NacCollumnAN YunyuShinkokuNo = new NacCollumnAN(11);

        public ProcessResultModel()
        {
            //this.RESULT_FLG = false;
            //初期化
            for (int i = 0; i < RESULT_LIST_MAX; i++)
            {
                NacCollumnAN nac = new NacCollumnAN(15, false);
                this.ResultList.Add(nac);
            }
        }
        /// <summary>
        /// ファイル名取得
        /// </summary>
        /// <returns></returns>
        public override string GetFileName()
        {
            if (string.IsNullOrEmpty(YunyuShinkokuNo.Data))
            {
                return string.Format("{0}_{1}_{2}", LOGSTR, this.GetReportType.ToString(), DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            }
            else
            {
                return string.Format("{0}_{1}_{2}_{3}", NaccsCommon.USER_CD.Data, this.GetReportType.ToString(), GetYunyuShinkokuNo, DateTime.Now.ToString("yyyyMMddhhmmss"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetByteLength()
        {
            int intlen = 0;

            intlen += NaccsCommon.GetByteLength ();
            intlen += GetByteLengthMain();
            return intlen;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int GetByteLengthMain()
        {
            int intlen = 0;

            for (int i = 0; i < RESULT_LIST_MAX; i++)
            {
                intlen += this.ResultList[i].GetByteLength();
            }
            intlen += 2;//改行分
            intlen += YunyuShinkokuNo.GetByteLength();

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
            byte[] KAIGYO = new byte[]{0x0D,0x0A};

            try
            {
                intLen = GetByteLengthMain();//intLen = this.GetByteLength();
                if (intLen > 0)
                {
                    Array.Copy(this.NaccsCommon.GetByteData(), 0, btData, pos, this.NaccsCommon.GetByteLength());
                    pos += NaccsCommon.GetByteLength();

                    for (int i = 0; i < RESULT_LIST_MAX; i++)
                    {
                        Array.Copy(this.ResultList[i].GetByteData(), 0, btData, pos, this.ResultList[i].GetByteLength());
                        pos += this.ResultList[i].GetByteLength();
                    }
                    Array.Copy(KAIGYO, 0, btData, pos, KAIGYO.Length);
                    pos += KAIGYO.Length;

                    Array.Copy(this.YunyuShinkokuNo.GetByteData(), 0, btData, pos, this.YunyuShinkokuNo.GetByteLength());
                    pos += YunyuShinkokuNo.GetByteLength();

                    return btData;
                }
                else return null;
            }
            catch (Exception e)
            {
                LogOut.ErrorOut(e.ToString(), this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                throw e;
            }

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
            int intRet = 0;

            try
            {
                System.Text.Encoding enc = System.Text.Encoding.GetEncoding(51932);
                //this.RESULT_FLG = false;

                if (btData.Length > NaccsCommon.GetByteLength())
                {
                    this.NaccsCommon.SetByteData(btData);
                    pos += NaccsCommon.GetByteLength();
                }
                intRet = 0;
                for (int i = 0; i < RESULT_LIST_MAX; i++)
                {
                    //受信データの長さが想定よりも短かった場合
                    if (pos + this.ResultList[i].GetByteLength() > btData.Length)
                    {
                        intRet++;
                        break;
                    }
                    if (!this.ResultList[i].SetData(EUC.GetString(btData, pos, this.ResultList[i].GetByteLength()))) intRet++;
                    pos += ResultList[i].GetByteLength();
                }
                //if (intRet == 0) this.RESULT_FLG = true;

                if (btData.Length >= pos + this.YunyuShinkokuNo.GetByteLength())
                {
                    this.YunyuShinkokuNo.SetData(EUC.GetString(btData, pos, this.YunyuShinkokuNo.GetByteLength()));
                    pos += YunyuShinkokuNo.GetByteLength();
                }

                return true;

            }
            catch (Exception e)
            {
                LogOut.ErrorOut(e.ToString(), this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                throw e;
            }
        }

        /// <summary>
        /// 結果コードから表示用文字列を取得
        /// </summary>
        /// <param name="strRet"></param>
        /// <returns></returns>
        public static string GetLogResult(string strRet, out Boolean errflg)
        {
            string ret = "";
            errflg = true;
            if (strRet == RESULT_SUCCESS)
            {
                ret = "";
                errflg = false;
            }
            else
            {
                for (int pos = 0; pos < strRet.Length; pos += 15)
                {
                    //取得結果が最初の文字列だけでいい場合
                    if (pos > 0) break;

                    var str = strRet.Substring(pos, 15);
                    if (RESULT_SUCCESS.Equals(str))
                    {
                        ret = "";
                        errflg = false;
                        break;
                    }

                    var data = str.Split('-');

                    if (data.Count() > 2)
                    {
                        DataRow[] findRow = HPFData.NaccsErrCd.Select("KBN_CODE = '" + data[0].Trim() + "'");
                        //共通項目で異常があるか？
                        if (findRow.Length > 0)
                        {
                            DataRow dr = findRow[0];
                            ret = string.Format("共通異常：{0}[{1}]({2})", dr[3].ToString(), strRet, dr[5].ToString());
                            if (ret.Length > 50) ret = ret.Substring(0, 100);
                        }
                        else
                        {
                            //二番目の項目が登録されているか？
                            DataRow[] findRow2 = HPFData.NaccsErrCdSub.Select("KBN_CODE = '" + data[1].Trim() + "'");
                            if (findRow2.Length > 0)
                            {
                                DataRow dr = findRow2[0];
                                ret = string.Format("入力項目異常:{0}({1})", dr[3].ToString(), str);
                                if (ret.Length > 50) ret = ret.Substring(0, 50);
                            }//
                            else
                            {
                                switch (data[0].Substring(0))
                                {
                                    case "U"://入力資格者エラー
                                        ret = string.Format("入力資格者エラー({0})", str);
                                        break;
                                    case "S"://単項目エラー
                                        ret = string.Format("項目異常:{0}({1})", data[1].Trim(), data[2].Trim());
                                        break;
                                    case "R"://入力項目関連エラー
                                        ret = string.Format("入力項目異常:{0}({1})", data[1].Trim(), data[2].Trim());
                                        break;
                                    case "E"://業務条件エラー
                                        ret = string.Format("業務条件異常({0})", str);
                                        break;
                                    case "W"://注意喚起メッセージ関係
                                        ret = string.Format("注意喚起メッセージ関係({0})", str);
                                        errflg = false;
                                        break;
                                    case "M"://指示メッセージ関係
                                        ret = string.Format("指示メッセージ関係({0})", str);
                                        break;
                                    case "L"://論理エラー
                                        ret = string.Format("論理エラー({0})", str);
                                        break;
                                    case "A"://システムメッセージ（共通エラー）
                                        ret = string.Format("共通異常({0})", str);
                                        break;
                                    default:
                                        ret = string.Format("異常:{0}", str);
                                        break;
                                }//switch
                            }//findRow2
                        }//findRow

                        DataRow[] findRow3 = HPFData.NaccsOmitErrCd.Select("KBN_CODE = '" + data[0].Trim() + "'");
                        if (findRow3.Length > 0)
                        {
                            DataRow dr = findRow3[0];
                            ret = string.Format("警告：{0}[{1}]", dr[3].ToString(), strRet);
                            if (ret.Length > 50) ret = ret.Substring(0, 100);
                            errflg = false;
                        }

                    }
                    else
                    {
                        ret = "受信データ解析異常";
                    }//受信データが-で区切られているか
                }//for pos
            }
            return ret;
        }  
    }

    /// <summary>
    /// 出力共通項目
    /// </summary>
    public class OutputCommonModel : NaccsModel
    {
        /// <summary>
        /// レポートタイプ
        /// </summary>
        public NACCS_REPORTTYPE REPORT_TYPE = NACCS_REPORTTYPE.NONE;
        //{
        //    get { return OutputCommonModel.GetReportType(this.OUTPUT_INFO.Data, this.GYOMU_CD.Data); }
        //}
        /// <summary>
        /// 登録SEQ番号
        /// </summary>
        public long? SEQ_NO = null;
        /// <summary>
        /// LOG識別用文字列
        /// </summary>
        protected const string LOGSTR = "RECV";
        /// <summary>
        /// 保存用ファイル名（拡張子なし）
        /// </summary>
        public string GetFileName()
        {
            if (this.RECV_TIME != null)
            {
                return string.Format("{0}_{1}_{2}", LOGSTR, this.REPORT_TYPE, ((DateTime)this.RECV_TIME).ToString("yyyyMMddhhmmss"));
            }
            else return "";
        }
        private DateTime? RECV_TIME = null;
        /// <summary>
        /// 予約コード
        /// </summary>
        private NacCollumnReserve ReserveTop = new NacCollumnReserve(3, "", false);
        /// <summary>
        /// 業務コード
        /// </summary>
        public NacCollumnAN GYOMU_CD = new NacCollumnAN(5, "", false);
        /// <summary>
        /// 出力情報コード
        /// </summary>
        public NacCollumnAN OUTPUT_INFO = new NacCollumnAN(7, "", false);
        /// <summary>
        /// 電文受信日時
        /// </summary>
        public NacCollumnDateTime DENBUN_RECV_DATE = new NacCollumnDateTime(14, "", false);
        /// <summary>
        /// 利用者コード
        /// </summary>
        public NacCollumnAN USER_CD = new NacCollumnAN(5, "", false);
        /// <summary>
        /// 予約コード６
        /// </summary>
        private NacCollumnReserve Reserve6 = new NacCollumnReserve(17, "", false);
        /// <summary>
        /// 利用者のメールアドレス
        /// </summary>
        public NacCollumnAN USER_MAIL_ADD = new NacCollumnAN(64, "", false);
        /// <summary>
        /// 業務個別データ(申告番号等）
        /// </summary>
        public NacCollumnAN SUBJECT = new NacCollumnAN(64, "", false);
        /// <summary>
        /// 予約コード9
        /// </summary>
        private NacCollumnReserve Reserve9 = new NacCollumnReserve(40, "", false);
        /// <summary>
        /// 電文引継コード
        /// </summary>
        public NacCollumnAN DENBUN_HANDOVER = new NacCollumnAN(26, "", false);
        /// <summary>
        /// 分割通番号
        /// </summary>
        public NacCollumnAN BUNKATSU_NO = new NacCollumnAN(3, "", false);
        /// <summary>
        /// 最終表示
        /// </summary>
        public NacCollumnAN LAST_FLG = new NacCollumnAN(1, "", false);
        /// <summary>
        ///  電文種別
        ///  P:出力情報電文（帳票用）
        ///  C:出力情報電文（照会結果を除く）（画面用）
        ///  M:出力情報電文（照会結果）（画面用）
        ///  T:出力情報電文（社内インターフェース用情報電文）
        ///  U:蓄積用情報電文（溜め置き電文）
        /// </summary>
        public NacCollumnAN DENBUN_TYPE = new NacCollumnAN(1, "", false);
        /// <summary>
        /// 予約コード14
        /// </summary>
        private NacCollumnReserve Reserve14 = new NacCollumnReserve(3, "", false);
        /// <summary>
        /// 入力情報特定番号
        /// </summary>
        public NacCollumnAN INPUT_INFO_NO = new NacCollumnAN(10, "", false);
        /// <summary>
        /// 索引引継情報
        /// </summary>
        public NacCollumnAN SAKUIN_HANDOVER = new NacCollumnAN(100, "", false);
        /// <summary>
        /// 宛管形式
        /// </summary>
        public NacCollumnAN ATEKAN_TYPE = new NacCollumnAN(1, "", false);
        /// <summary>
        /// 予約コード18
        /// </summary>
        private NacCollumnReserve Reserve18 = new NacCollumnReserve(28, "", false);
        /// <summary>
        /// 電文長
        /// </summary>
        public NacCollumnNLEN DENBUN_LEN = new NacCollumnNLEN(6, 6, false);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetByteLength()
        {
            int intlen = 0;

            intlen += ReserveTop.GetByteLength();
            intlen += GYOMU_CD.GetByteLength();
            intlen += OUTPUT_INFO.GetByteLength();
            intlen += DENBUN_RECV_DATE.GetByteLength();
            intlen += USER_CD.GetByteLength();
            intlen += Reserve6.GetByteLength();
            intlen += USER_MAIL_ADD.GetByteLength();
            intlen += SUBJECT.GetByteLength();
            intlen += Reserve9.GetByteLength();
            intlen += DENBUN_HANDOVER.GetByteLength();
            intlen += BUNKATSU_NO.GetByteLength();
            intlen += LAST_FLG.GetByteLength();
            intlen += DENBUN_TYPE.GetByteLength();
            intlen += Reserve14.GetByteLength();
            intlen += INPUT_INFO_NO.GetByteLength();
            intlen += SAKUIN_HANDOVER.GetByteLength();
            intlen += ATEKAN_TYPE.GetByteLength();
            intlen += Reserve18.GetByteLength();
            intlen += DENBUN_LEN.GetByteLength();

            //改行コード用
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
                        pos += GYOMU_CD.GetByteLength();
                        
                        Array.Copy(this.OUTPUT_INFO.GetByteData(), 0, btData, pos, this.OUTPUT_INFO.GetByteLength());
                        pos += OUTPUT_INFO.GetByteLength();

                        Array.Copy(this.DENBUN_RECV_DATE.GetByteData(), 0, btData, pos, this.DENBUN_RECV_DATE.GetByteLength());
                        pos += DENBUN_RECV_DATE.GetByteLength();

                        Array.Copy(this.USER_CD.GetByteData(), 0, btData, pos, this.USER_CD.GetByteLength());
                        pos += USER_CD.GetByteLength();

                        Array.Copy(this.Reserve6.GetByteData(), 0, btData, pos, this.Reserve6.GetByteLength());
                        pos += Reserve6.GetByteLength();

                        Array.Copy(this.USER_MAIL_ADD.GetByteData(), 0, btData, pos, this.USER_MAIL_ADD.GetByteLength());
                        pos += USER_MAIL_ADD.GetByteLength();

                        Array.Copy(this.SUBJECT.GetByteData(), 0, btData, pos, this.SUBJECT.GetByteLength());
                        pos += SUBJECT.GetByteLength();

                        Array.Copy(this.Reserve9.GetByteData(), 0, btData, pos, this.Reserve9.GetByteLength());
                        pos += Reserve9.GetByteLength();

                        Array.Copy(this.DENBUN_HANDOVER.GetByteData(), 0, btData, pos, this.DENBUN_HANDOVER.GetByteLength());
                        pos += DENBUN_HANDOVER.GetByteLength();

                        Array.Copy(this.BUNKATSU_NO.GetByteData(), 0, btData, pos, this.BUNKATSU_NO.GetByteLength());
                        pos += BUNKATSU_NO.GetByteLength();

                        Array.Copy(this.LAST_FLG.GetByteData(), 0, btData, pos, this.LAST_FLG.GetByteLength());
                        pos += LAST_FLG.GetByteLength();

                        Array.Copy(this.DENBUN_TYPE.GetByteData(), 0, btData, pos, this.DENBUN_TYPE.GetByteLength());
                        pos += DENBUN_TYPE.GetByteLength();

                        Array.Copy(this.Reserve14.GetByteData(), 0, btData, pos, this.Reserve14.GetByteLength());
                        pos += Reserve14.GetByteLength();

                        Array.Copy(this.INPUT_INFO_NO.GetByteData(), 0, btData, pos, this.INPUT_INFO_NO.GetByteLength());
                        pos += INPUT_INFO_NO.GetByteLength();

                        Array.Copy(this.SAKUIN_HANDOVER.GetByteData(), 0, btData, pos, this.SAKUIN_HANDOVER.GetByteLength());
                        pos += SAKUIN_HANDOVER.GetByteLength();

                        Array.Copy(this.ATEKAN_TYPE.GetByteData(), 0, btData, pos, this.ATEKAN_TYPE.GetByteLength());
                        pos += ATEKAN_TYPE.GetByteLength();

                        Array.Copy(this.Reserve18.GetByteData(), 0, btData, pos, this.Reserve18.GetByteLength());
                        pos += Reserve18.GetByteLength();

                        Array.Copy(this.DENBUN_LEN.GetByteData(), 0, btData, pos, this.DENBUN_LEN.GetByteLength());
                        pos += DENBUN_LEN.GetByteLength();

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
            catch(Exception e)
            {
                LogOut.ErrorOut(e.ToString().Substring(0,50), this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                throw e;
            }
        }
        public string GetFileData()
        {
            StringBuilder str = new StringBuilder();
            var s_euc = Encoding.GetEncoding(51932);
            try
            {
                str.Append(s_euc.GetString(ReserveTop.GetByteData()));
                str.Append(s_euc.GetString(GYOMU_CD.GetByteData()));
                str.Append(s_euc.GetString(OUTPUT_INFO.GetByteData()));
                str.Append(s_euc.GetString(DENBUN_RECV_DATE.GetByteData()));
                str.Append(s_euc.GetString(USER_CD.GetByteData()));
                str.Append(s_euc.GetString(Reserve6.GetByteData()));
                str.Append(s_euc.GetString(USER_MAIL_ADD.GetByteData()));
                str.Append(s_euc.GetString(SUBJECT.GetByteData()));
                str.Append(s_euc.GetString(Reserve9.GetByteData()));
                str.Append(s_euc.GetString(DENBUN_HANDOVER.GetByteData()));
                str.Append(s_euc.GetString(BUNKATSU_NO.GetByteData()));
                str.Append(s_euc.GetString(LAST_FLG.GetByteData()));
                str.Append(s_euc.GetString(DENBUN_TYPE.GetByteData()));
                str.Append(s_euc.GetString(Reserve14.GetByteData()));
                str.Append(s_euc.GetString(INPUT_INFO_NO.GetByteData()));
                str.Append(s_euc.GetString(SAKUIN_HANDOVER.GetByteData()));
                str.Append(s_euc.GetString(ATEKAN_TYPE.GetByteData()));
                str.Append(s_euc.GetString(Reserve18.GetByteData()));
                str.Append(s_euc.GetString(DENBUN_LEN.GetByteData()));

                return str.ToString();
            }
            catch
            {
                return "OutputCommonModel Error";
            }
        }
        /// <summary>
        /// byte配列からInsertまでこなす
        /// </summary>
        /// <param name="btData"></param>
        /// <returns></returns>
        public Boolean SetData(byte[] btData, DateTime dtmRecv, out long seq)
        {
            string fname;
            DataBaseSQL objDb = null;
            seq = 0;
            try
            {
                this.RECV_TIME = DateTime.Now;
                if (SetByteData(btData))
                {
                    objDb = new DataBaseSQL();
                    objDb.DBLogIn();

                    objDb.BeginTransaction();

                    seq = objDb.ExecuteScalar(this.GetSQL_INSERT(dtmRecv, this.REPORT_TYPE.ToString(), this.GetFileName()));

                    this.SEQ_NO = seq;

                    objDb.Commit();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                if(objDb != null )
                {                    
                    objDb.RollBack();
                }
                LogOut.ErrorOut(e.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
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
        /// 受信履歴テーブルにMEMOを追記
        /// </summary>
        /// <param name="btData"></param>
        /// <returns></returns>
        public Boolean SetSameYNOResultDB(long? seq = null)
        {
            DataBaseSQL objDb = null;
            if (seq == null)
            {
                seq = this.SEQ_NO;
            }
            try
            {
                if ((seq != null) && (!string.IsNullOrWhiteSpace(this.SUBJECT.Data)))
                {
                    objDb = new DataBaseSQL();
                    objDb.DBLogIn();

                    objDb.BeginTransaction();

                    objDb.ExecuteNonQuery(this.GetSQL_UPDATE_RESULT_SAMENO((long)seq), null);

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
                LogOut.ErrorOut(e.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
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
        /// 受信履歴テーブルにMEMOを追記
        /// </summary>
        /// <param name="btData"></param>
        /// <returns></returns>
        public Boolean SetFileNameResultDB(string strFilename, long? seq = null, string strMemo = "")
        {
            DataBaseSQL objDb = null;
            if (seq == null)
            {
                seq = this.SEQ_NO;
            }
            try
            {
                if (seq != null)
                {
                    objDb = new DataBaseSQL();
                    objDb.DBLogIn();

                    objDb.BeginTransaction();

                    objDb.ExecuteNonQuery(this.GetSQL_UPDATE_FILENAME((long)seq, strFilename, strMemo), null);

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
                LogOut.ErrorOut(e.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
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
        /// 受信履歴テーブルにMEMOを追記
        /// </summary>
        /// <param name="btData"></param>
        /// <returns></returns>
        public Boolean SetDBStatusMemo(long? seq = null, string strStatus = "", string strMemo = "")
        {
            DataBaseSQL objDb = null;
            if (seq == null)
            {
                seq = this.SEQ_NO;
            }
            try
            {
                if (seq != null)
                {
                    objDb = new DataBaseSQL();
                    objDb.DBLogIn();

                    objDb.BeginTransaction();

                    objDb.ExecuteNonQuery(this.GetSQL_UPDATE_RESULT_MEMO_STAT((long)seq, strStatus, strMemo), null);

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
                LogOut.ErrorOut(e.Message, this.GetType().Name, MethodBase.GetCurrentMethod().Name);
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
                    this.ReserveTop.SetData(EUC.GetString(btData, pos, this.ReserveTop.GetByteLength()));
                    pos += this.ReserveTop.GetByteLength();//3

                    this.GYOMU_CD.SetData(EUC.GetString(btData, pos, this.GYOMU_CD.GetByteLength()));
                    pos += GYOMU_CD.GetByteLength();//5
                    
                    this.OUTPUT_INFO.SetData(EUC.GetString(btData, pos, this.OUTPUT_INFO.GetByteLength()));
                    pos += OUTPUT_INFO.GetByteLength();//7

                    var dtstr = EUC.GetString(btData, pos, this.DENBUN_RECV_DATE.GetByteLength());                    
                    if(!string.IsNullOrWhiteSpace(dtstr))
                    {
                        this.DENBUN_RECV_DATE.SetData(dtstr);
                    }
                    else
                    {
                        this.DENBUN_RECV_DATE.SetData(Convert.ToString(DateTime.Now));
                    }
                    pos += DENBUN_RECV_DATE.GetByteLength();//14

                    this.USER_CD.SetData(EUC.GetString(btData, pos, this.USER_CD.GetByteLength()));
                    pos += USER_CD.GetByteLength();//5

                    this.Reserve6.SetData(EUC.GetString(btData, pos, this.Reserve6.GetByteLength()));
                    pos += Reserve6.GetByteLength();//11

                    this.USER_MAIL_ADD.SetData(EUC.GetString(btData, pos, this.USER_MAIL_ADD.GetByteLength()));
                    pos += USER_MAIL_ADD.GetByteLength();//6

                    this.SUBJECT.SetData(EUC.GetString(btData, pos, this.SUBJECT.GetByteLength()));
                    pos += SUBJECT.GetByteLength();//64

                    this.Reserve9.SetData(EUC.GetString(btData, pos, this.Reserve9.GetByteLength()));
                    pos += Reserve9.GetByteLength();//40

                    this.DENBUN_HANDOVER.SetData(EUC.GetString(btData, pos, this.DENBUN_HANDOVER.GetByteLength()));
                    pos += DENBUN_HANDOVER.GetByteLength();//26

                    this.BUNKATSU_NO.SetData(EUC.GetString(btData, pos, this.BUNKATSU_NO.GetByteLength()));
                    pos += BUNKATSU_NO.GetByteLength();

                    //LASTの場合のみE　それ以外はスペース
                    this.LAST_FLG.SetData(EUC.GetString(btData, pos, this.LAST_FLG.GetByteLength()));
                    pos += LAST_FLG.GetByteLength();

                    this.DENBUN_TYPE.SetData(EUC.GetString(btData, pos, this.DENBUN_TYPE.GetByteLength()));
                    pos += DENBUN_TYPE.GetByteLength();

                    this.Reserve14.SetData(EUC.GetString(btData, pos, this.Reserve14.GetByteLength()));
                    pos += Reserve14.GetByteLength();

                    this.INPUT_INFO_NO.SetData(EUC.GetString(btData, pos, this.INPUT_INFO_NO.GetByteLength()));
                    pos += INPUT_INFO_NO.GetByteLength();

                    this.SAKUIN_HANDOVER.SetData(EUC.GetString(btData, pos, this.SAKUIN_HANDOVER.GetByteLength()));
                    pos += SAKUIN_HANDOVER.GetByteLength();

                    //INQ->Q EXZ->Z EXC->C 資料→K 
                    this.ATEKAN_TYPE.SetData(EUC.GetString(btData, pos, this.ATEKAN_TYPE.GetByteLength()));
                    pos += ATEKAN_TYPE.GetByteLength();

                    this.Reserve18.SetData(EUC.GetString(btData, pos, this.Reserve18.GetByteLength()));
                    pos += Reserve18.GetByteLength();

                    this.DENBUN_LEN.SetData(EUC.GetString(btData, pos, this.DENBUN_LEN.GetByteLength()));
                    pos += DENBUN_LEN.GetByteLength();

                    this.REPORT_TYPE = GetReportType(this.OUTPUT_INFO.Data, this.GYOMU_CD.Data); 

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
        /// 自データからSQL作成
        /// </summary>
        /// <returns></returns>
        public string GetSQL_INSERT(DateTime dtmRecv, string strRecvType, string strFileName = "")
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("INSERT INTO T_EDI_RESULT (");
            strSql.AppendLine("  RECV_DATE");
            strSql.AppendLine("  ,RECV_TYPE");
            strSql.AppendLine("  ,GYOMU_CD");
            strSql.AppendLine("  ,OUTPUT_INFO");
            strSql.AppendLine("  ,DENBUN_RECV_DATE");
            strSql.AppendLine("  ,USER_CD");
            strSql.AppendLine("  ,USER_MAIL_ADD");
            strSql.AppendLine("  ,SUBJECT");
            strSql.AppendLine("  ,DENBUN_HANDOVER");
            strSql.AppendLine("  ,BUNKATSU_NO");
            strSql.AppendLine("  ,LAST_FLG");
            strSql.AppendLine("  ,DENBUN_TYPE");
            strSql.AppendLine("  ,INPUT_INFO_NO");
            strSql.AppendLine("  ,SAKUIN_HANDOVER");
            strSql.AppendLine("  ,ATEKAN_TYPE");
            strSql.AppendLine("  ,DENBUN_LEN");
            strSql.AppendLine("  ,MEMO");
            strSql.AppendLine("  ,EDI_STATUS");

            strSql.AppendLine("  ,CRT_STAFF_CD");
            strSql.AppendLine("  ,CRT_DATE");
            strSql.AppendLine("  ,CRT_TANMATU_ID");
            //strSql.AppendLine("  ,UPD_STAFF_CD");
            //strSql.AppendLine("  ,UPD_DATE");
            //strSql.AppendLine("  ,UPD_TANMATU_ID");
            strSql.AppendLine("  ,VERSION_NO");


            //strSql.AppendLine(") VALUES (");
            strSql.AppendLine(") OUTPUT INSERTED.DATA_SEQ");
            strSql.AppendLine(" VALUES (");
            strSql.AppendFormat("  '{0}'", Common.ConvertToDateTimeString(dtmRecv)).AppendLine();
            strSql.AppendFormat("  ,'{0}'", strRecvType).AppendLine();
            strSql.AppendFormat("  ,'{0}'", Common.ConvertToString(this.GYOMU_CD.Data)).AppendLine();
            strSql.AppendFormat("  ,'{0}'", Common.ConvertToString(this.OUTPUT_INFO.Data)).AppendLine();
            if (!string.IsNullOrWhiteSpace(this.DENBUN_RECV_DATE.Data))
            {
                strSql.AppendFormat("  ,'{0}'", Common.ConvertToDateTimeString(this.DENBUN_RECV_DATE.DataDatetime)).AppendLine();
            }
            else
            {
                strSql.AppendLine("  ,NULL");
            }
            strSql.AppendFormat("  ,'{0}'", Common.ConvertToString(this.USER_CD.Data)).AppendLine();
            strSql.AppendFormat("  ,'{0}'", Common.ConvertToString(this.USER_MAIL_ADD.Data)).AppendLine();
            strSql.AppendFormat("  ,'{0}'", Common.ConvertToString(this.SUBJECT.Data)).AppendLine();
            strSql.AppendFormat("  ,'{0}'", Common.ConvertToString(this.DENBUN_HANDOVER.Data)).AppendLine();
            strSql.AppendFormat("  ,'{0}'", Common.ConvertToString(this.BUNKATSU_NO.Data)).AppendLine();
            strSql.AppendFormat("  ,{0}", (string.IsNullOrEmpty(this.LAST_FLG.Data))?"0":"1").AppendLine();
            strSql.AppendFormat("  ,'{0}'", Common.ConvertToString(this.DENBUN_TYPE.Data)).AppendLine();
            strSql.AppendFormat("  ,'{0}'", Common.ConvertToString(this.INPUT_INFO_NO.Data)).AppendLine();
            strSql.AppendFormat("  ,'{0}'", Common.ConvertToString(this.SAKUIN_HANDOVER.Data)).AppendLine();
            strSql.AppendFormat("  ,'{0}'", Common.ConvertToString(this.ATEKAN_TYPE.Data)).AppendLine();
            strSql.AppendFormat("  ,{0}", Common.ConvertToInteger(this.DENBUN_LEN.Data)).AppendLine();

            strSql.AppendFormat("  ,'{0}'", (strFileName.Length>100)?strFileName.Substring(0,100):strFileName).AppendLine();

            string strSubject = "";
            var strlist = Common.ConvertToString(this.SUBJECT.Data).Split(' ');
            if (strlist.Count() > 0)
            {
                strSubject = strlist[0];
            }

            switch (this.REPORT_TYPE)
            {
                //case NACCS_REPORTTYPE.NONE:
                case NACCS_REPORTTYPE.OTA:
                case NACCS_REPORTTYPE.OTA01:
                case NACCS_REPORTTYPE.OTC:
                case NACCS_REPORTTYPE.SYG_LIST:
                case NACCS_REPORTTYPE.SYG_RESULT:
                case NACCS_REPORTTYPE.OTHER:
                case NACCS_REPORTTYPE.RESULT:
                case NACCS_REPORTTYPE.SAD4011://輸入申告入力控(沖縄特免制度)情報 OTA
                    strSql.AppendLine("  ,''");
                    break;
                default:
                    strSql.AppendFormat("  ,(").AppendLine();
                    strSql.AppendFormat("    CASE WHEN '{0}' <> '' THEN", strSubject).AppendLine();
                    strSql.AppendFormat("      ISNULL(").AppendLine();
                    strSql.AppendFormat("        (SELECT TOP (1) H.PAX_NO + H.EDA_NO FROM T_PAX_H AS H").AppendLine();
                    strSql.AppendFormat("        WHERE LTRIM(RTRIM('{0}')) = H.YUNYU_SHINKOKU_NO", strSubject).AppendLine();
                    strSql.AppendFormat("      ) , 'Error:該当なし')").AppendLine();
                    strSql.AppendFormat("    Else '' END").AppendLine();
                    strSql.AppendFormat("  )");
                    break;
            }     

            strSql.AppendFormat("  ,'{0}'", HPFData.AppUserName ).AppendLine();
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
        /// 自データからSQL作成
        /// </summary>
        /// <returns></returns>
        public string GetSQL_UPDATE_RESULT_SAMENO(long lngSeq)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("UPDATE T_EDI_RESULT SET ");

            strSql.AppendLine("  EDI_STATUS = (");
            //strSql.AppendFormat("    SELECT EDI_STATUS FROM T_EDI_RESULT WHERE DATA_SEQ = {0}", lngSeq).AppendLine();
            strSql.AppendLine("    SELECT TOP (1) H.PAX_NO + H.EDA_NO FROM T_PAX_H AS H");
            strSql.AppendLine("      WHERE 1=1");
            strSql.AppendLine("      AND SUBJECT LIKE 'H.YUNYU_SHINKOKU_NO%'");
            strSql.AppendLine("      AND H.DELETE_FLG IS NULL");
            strSql.AppendLine("  )");



            strSql.AppendFormat("  ,UPD_STAFF_CD = '{0}'", HPFData.AppUserName).AppendLine();
            strSql.AppendLine("  ,UPD_DATE = GETDATE()");
            strSql.AppendFormat("  ,UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
            strSql.AppendLine("  ,VERSION_NO = VERSION_NO + 1");

            strSql.AppendLine("  WHERE 1=1");

            strSql.AppendLine("  AND (");
            strSql.AppendFormat("    SELECT SUBJECT FROM T_EDI_RESULT AS OG WHERE OG.DATA_SEQ = {0}", lngSeq).AppendLine();
            strSql.AppendLine("  ) = SUBJECT");
            strSql.AppendLine("  AND DELETE_FLG IS NULL");

            return strSql.ToString();
        }
        /// <summary>
        /// 自データからSQL作成
        /// </summary>
        /// <returns></returns>
        public string GetSQL_UPDATE_FILENAME(long lngSeq, string strFilename, string strMemo = "")
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("UPDATE T_EDI_RESULT SET ");
            strSql.AppendFormat("  FILE_NAME = '{0}'", strFilename).AppendLine();
            if (strMemo != "")
            {
                strSql.AppendFormat("  ,MEMO = '{0}'", strMemo).AppendLine();
            }
            strSql.AppendFormat("  ,UPD_STAFF_CD = '{0}'", HPFData.AppUserName).AppendLine();
            strSql.AppendLine("  ,UPD_DATE = GETDATE()");
            strSql.AppendFormat("  ,UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
            strSql.AppendLine("  ,VERSION_NO = VERSION_NO + 1");

            strSql.AppendLine("  WHERE 1=1");
            strSql.AppendFormat("  AND DATA_SEQ = {0}", lngSeq).AppendLine();

            return strSql.ToString();
        }
        /// <summary>
        /// 自データからSQL作成
        /// </summary>
        /// <returns></returns>
        public string GetSQL_UPDATE_RESULT_MEMO_STAT(long lngSeq, string strStatus = "", string strMemo = "")
        {
            StringBuilder strSql = new StringBuilder();

            strSql.AppendLine("UPDATE T_EDI_RESULT SET ");
            strSql.AppendFormat("  UPD_STAFF_CD = '{0}'", HPFData.AppUserName).AppendLine();
            strSql.AppendLine("  ,UPD_DATE = GETDATE()");
            strSql.AppendFormat("  ,UPD_TANMATU_ID = '{0}'", Environment.MachineName).AppendLine();
            strSql.AppendLine("  ,VERSION_NO = VERSION_NO + 1");
            if (strStatus != "")
            {
                strSql.AppendFormat("  ,EDI_STATUS = '{0}'", strStatus).AppendLine();
            }
            if (strMemo != "")
            {
                strSql.AppendFormat("  ,MEMO = '{0}'", strMemo).AppendLine();
            }

            strSql.AppendLine("  WHERE 1=1");
            strSql.AppendFormat("  AND DATA_SEQ = {0}", lngSeq).AppendLine();

// UPDATE T_EDI_RESULT
//  SET
//    MEMO = '受信異常:送信データ該当なし'
//  WHERE 1=1
//  AND DENBUN_HANDOVER = '5005421350-1910064912'
//  AND NOT EXISTS(
//  SELECT [EDI_INFO_CD] FROM T_PAX_H AS H
//WHERE H.EDI_INFO_CD = '5005421350-191006491'
//  )

            return strSql.ToString();
        }
         
        /// <summary>
        /// 出力CDまたは業務CDからENUMの取得
        /// </summary>
        /// <param name="strOutputInfo"></param>
        /// <returns></returns>
        public static NACCS_REPORTTYPE GetReportType(string strOutputInfo, string strGyomuCd = "")
        {
            NACCS_REPORTTYPE report = NACCS_REPORTTYPE.NONE;

            if (strOutputInfo.Length > 0)
            {
                if (strOutputInfo.Trim()[0] == '*')
                {
                    return NACCS_REPORTTYPE.RESULT;
                }
                switch (strOutputInfo.Trim())
                {
                    //case "SYG": report = NACCS_REPORTTYPE.SYG_RESULT; break;
                    //case "*SOTA": report = NACCS_REPORTTYPE.RESULT; break;  //処理結果通知
                    case "SAD4011": report = NACCS_REPORTTYPE.SAD4011; break;  //輸入申告入力控（沖縄特免制度）情報
                    case "SAD4031": report = NACCS_REPORTTYPE.SAD4031; break;  //輸入申告控（沖縄特免制度）情報
                    case "SAD4041": report = NACCS_REPORTTYPE.SAD4041; break;  //輸入許可前貨物引取承認申請控（沖縄特免制度）情報
                    case "SAD4071": report = NACCS_REPORTTYPE.SAD4071; break;  //輸入許可通知兼申告控（沖縄特免制度）情報
                    case "SAD4081": report = NACCS_REPORTTYPE.SAD4081; break;  //輸入許可前貨物引取承認通知兼申請控（沖縄特免制度）情報
                    case "SAD4111": report = NACCS_REPORTTYPE.SAD4111; break;  //

                    case "SAF0010": report = NACCS_REPORTTYPE.SAF0010; break;  //納付書情報（直納）
                    case "SAD4131": report = NACCS_REPORTTYPE.SAD4131; break;  //許可・承認貨物（沖縄特免制度）情報 のみ
                    case "SAF0221": report = NACCS_REPORTTYPE.SAF0221; break;  //担保不足通知情報
                    case "SAF0021": report = NACCS_REPORTTYPE.SAF0021; break;  //納付番号通知情報
                    case "SAF0211": report = NACCS_REPORTTYPE.SAF0211; break;  //口座不足通知情報

                    //case "*SOTA01": report = NACCS_REPORTTYPE.RESULT; break;  //処理結果通知
                    case "SAD4021": report = NACCS_REPORTTYPE.SAD4021; break;  //輸入申告変更入力控（沖縄特免制度）情報
                    case "SAD4051": report = NACCS_REPORTTYPE.SAD4051; break;  //輸入申告変更控（沖縄特免制度）情報
                    case "SAD4061": report = NACCS_REPORTTYPE.SAD4061; break;  //輸入許可前貨物引取承認申請変更控（沖縄特免制度）情報
                    case "SAD4091": report = NACCS_REPORTTYPE.SAD4091; break;  //輸入許可通知兼申告変更控（沖縄特免制度）情報
                    case "SAD4101": report = NACCS_REPORTTYPE.SAD4101; break;  //輸入許可前貨物引取承認通知兼申請変更控（沖縄特免制度）情報

                    default:
                        report = GetReportTypeFromGyomuCd(strGyomuCd); 
                        break;
                }
            }
            else 
            {
                report = GetReportTypeFromGyomuCd(strGyomuCd);
            }
            return report;
        }
        /// <summary>
        /// 業務CDからENUMの取得
        /// </summary>
        /// <param name="strOutputInfo"></param>
        /// <returns></returns>
        public static NACCS_REPORTTYPE GetReportTypeFromGyomuCd(string strGyomuCd)
        {
            NACCS_REPORTTYPE report = NACCS_REPORTTYPE.NONE;

            if (strGyomuCd.Length > 0)
            {
                switch (strGyomuCd.Trim())
                {
                    case "OTA": report = NACCS_REPORTTYPE.OTA; break;
                    case "OTA01": report = NACCS_REPORTTYPE.OTA; break;
                    case "SYG": report = NACCS_REPORTTYPE.SYG_LIST; break;
                    default: report = NACCS_REPORTTYPE.OTHER; break;
                }
            }

            return report;
        }

    }

}
