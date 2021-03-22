using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using JoySmtp.JoyCommon;
using JoySmtp.CLogOut;
using JoySmtp.Data;

namespace JoySmtp.Nac
{

    #region "SYGモデル"

    #region "送信モデル"

    public class ProcessSYGSendModel : NaccsSendModel
    {
        /// <summary>
        /// 送信タイプ
        /// </summary>
        public enum SendType
        {
            GET_LIST
            , GET_RESULT
            ,OTHER
        }
        ///// <summary>
        ///// 入力共通項目398
        ///// </summary>
        //public InputCommonModel InputCommon;
        public SendType ProcessType;
        public String GyomuString = "SYG";
        public NacCollumnAN SendMessage = new NacCollumnAN(7);
        /// <summary>
        /// 電文引継情報
        /// </summary>
        public string DEFAULT_DENBUN_HANDOVER
        {
            get { return string.Format("{0}{1}", this.GyomuString, DateTime.Now.ToString("yyyyMMddhhmmss")); }
        }
        /// <summary>
        /// 入力情報番号
        /// </summary>
        public string DEFAULT_INPUT_INFO_NO
        {
            get { return this.GyomuString; }
        }      
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProcessSYGSendModel(SendType stype = SendType.GET_LIST, string strInfoCD = "")
        {
            NaccsCommon = new InputCommonModel();
            this.ProcessType = stype;
            this.NaccsCommon.SYSTEM_TYPE.SetData("2");//NACCS:航空:1海上:2

            if (this.ProcessType == SendType.GET_RESULT && strInfoCD != "")
            {
                this.SendMessage.SetData(strInfoCD);
                this.ReportType = NACCS_REPORTTYPE.SYG_RESULT;
            }
            else
            {
                this.SendMessage.SetData("REF");
                this.ReportType = NACCS_REPORTTYPE.SYG_LIST;
            }
        }
        /// <summary>
        /// コンストラクタ２
        /// </summary>
        public ProcessSYGSendModel(string strInfoCD = "", string strSysType = "2", string strSendMes = "")
        {
            this.NaccsCommon = new InputCommonModel();
            this.ProcessType = SendType.OTHER;
           
            this.NaccsCommon.SYSTEM_TYPE.SetData(strSysType);
            this.GyomuString = strInfoCD;

            int len = strSendMes.Length;
            this.SendMessage = new NacCollumnAN(len);
            this.SendMessage.SetData(strSendMes);

            this.ReportType = NACCS_REPORTTYPE.OTHER;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetByteLength()
        {
            int intlen = 0;
            intlen += NaccsCommon.GetByteLength();

            intlen += SendMessage.GetByteLength();
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
            int pos = 0;

            try
            {
                //this.NaccsCommon.GYOMU_CD.SetData("SYG");
                this.NaccsCommon.GYOMU_CD.SetData(this.GyomuString);
                this.NaccsCommon.USER_CD.SetData(HPFData.InputUserCode);
                this.NaccsCommon.SHIKIBETSU_NO.SetData(HPFData.NaccsShikibetsuNo);
                this.NaccsCommon.USER_PASS.SetData(HPFData.InputUserPass);

                this.NaccsCommon.DENBUN_HANDOVER.SetData(DEFAULT_DENBUN_HANDOVER);
                this.NaccsCommon.INPUT_INFO_NO.SetData(DEFAULT_INPUT_INFO_NO);
                this.NaccsCommon.SAKUIN_HANDOVER.SetData("");
                //this.NaccsCommon.SYSTEM_TYPE.SetData("2");//NACCS:航空:1海上:2

                Array.Copy(this.NaccsCommon.GetByteData(intLast), 0, btData, pos, this.NaccsCommon.GetByteLength());
                pos += this.NaccsCommon.GetByteLength();

                Array.Copy(this.SendMessage.GetByteData(), 0, btData, pos, this.SendMessage.GetByteLength());
                pos += SendMessage.GetByteLength();

                return btData;
            }
            catch (Exception e)
            {
                LogOut.ErrorOut(e.ToString(), this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                throw e;
            }
            
        }
        /// <summary>
        /// バイトデータから内容の取得　※※※
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
                    //this.GYOMU_CD.SetData(EUC.GetString(btData, pos, this.GYOMU_CD.GetByteLength()));
                    //pos += GYOMU_CD.GetByteLength();

                    //this.OUTPUT_INFO.SetData(EUC.GetString(btData, pos, this.OUTPUT_INFO.GetByteLength()));
                    //pos += OUTPUT_INFO.GetByteLength();

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
    }
    #endregion "送信モデル"

    #region "受信モデル"

    /// <summary>
    /// 処理結果モデル
    /// </summary>
    public class ProcessSYGRecvModel : NaccsRecvModel
    {
        ///// <summary>
        ///// 出力共通項目
        ///// </summary>
        //public OutputCommonModel OutputCommon = new OutputCommonModel();
        /// <summary>
        /// 処理結果コード
        /// </summary>
        public NacCollumnAN OutputResult = new NacCollumnAN(75);
        /// <summary>
        /// 情報数
        /// </summary>
        public NacCollumnNLEN InfoNum = new NacCollumnNLEN(4);

        /// <summary>
        /// 情報コード
        /// </summary>
        public List<InfoOutModel> InfoList = new List<InfoOutModel>();

        /// <summary>
        /// 
        /// </summary>
        public ProcessSYGRecvModel()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetByteLength()
        {
            int intlen = 0;

            intlen += NaccsCommon.GetByteLength();
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

            intlen += OutputResult.GetByteLength();

            intlen += InfoNum.GetByteLength();
            
            for (int i = 0; i < InfoList.Count(); i++)
            {
                intlen += InfoList[i].GetByteLength();
            }

            return intlen;
        }
        /// <summary>
        /// byte配列から各値を設定
        /// </summary>
        /// <param name="btData"></param>
        /// <returns></returns>
        public override Boolean SetByteData(byte[] btData)
        {
            int intLast = this.GetByteLength();
            int pos = 0, len = 0;

            try
            {
                if (btData.Length >= intLast)
                {
                    this.NaccsCommon.SetByteData(btData);
                    pos += NaccsCommon.GetByteLength();

                    this.OutputResult.SetData(EUC.GetString(btData, pos, this.OutputResult.GetByteLength()));
                    pos += OutputResult.GetByteLength();

                    this.InfoNum.SetData(EUC.GetString(btData, pos, this.InfoNum.GetByteLength()));
                    pos += InfoNum.GetByteLength();

                    InfoList = new List<InfoOutModel>();

                    for(int n =0; n<this.InfoNum.GetIntData; n++)
                    {
                        InfoOutModel info = new InfoOutModel();
                        len = info.GetByteLength();

                        var buffer = new byte[len];
                        Array.Copy(btData, pos, buffer, 0, len);
                        info.SetByteData(buffer);
                        this.InfoList.Add(info);
                        pos += len;
                    }

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
                intLen = GetByteLengthMain();
                if (intLen > 0)
                {
                    Array.Copy(this.NaccsCommon.GetByteData(intLen), 0, btData, pos, this.NaccsCommon.GetByteLength());
                    pos += NaccsCommon.GetByteLength();

                    Array.Copy(this.OutputResult.GetByteData(), 0, btData, pos, this.OutputResult.GetByteLength());
                    pos += OutputResult.GetByteLength();

                    Array.Copy(this.InfoNum.GetByteData(), 0, btData, pos, this.InfoNum.GetByteLength());
                    pos += InfoNum.GetByteLength();

                    for (int i = 0; i < InfoList.Count(); i++)
                    {
                        Array.Copy(this.InfoList[i].GetByteData(), 0, btData, pos, this.InfoList[i].GetByteLength());
                        pos += InfoList[i].GetByteLength();
                    }
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
    }

    #region "出力情報モデル"

    /// <summary>
    /// 出力情報モデル
    /// </summary>
    public class InfoOutModel : NaccsModel
    {
        /// <summary>
        /// 出力情報コード
        /// </summary>
        public NacCollumnAN INFO_CD = new NacCollumnAN(7);
        /// <summary>
        /// 未送信電文数
        /// </summary>
        public NacCollumnN UNSENT_NUM = new NacCollumnN(4);
        

        public int GetByteLength()
        {
            int intlen = 0;
            intlen += INFO_CD.GetByteLength();
            intlen += UNSENT_NUM.GetByteLength();

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
            int pos = 0;
            try
            {
                Array.Copy(this.INFO_CD.GetByteData(), 0, btData, pos, this.INFO_CD.GetByteLength());
                pos += INFO_CD.GetByteLength();

                Array.Copy(this.UNSENT_NUM.GetByteData(), 0, btData, pos, this.UNSENT_NUM.GetByteLength());
                pos += UNSENT_NUM.GetByteLength();

                return btData;
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
            try
            {
                if (btData.Length >= intLast)
                {
                    this.INFO_CD.SetData(EUC.GetString(btData, pos, this.INFO_CD.GetByteLength()));
                    pos += INFO_CD.GetByteLength();

                    this.UNSENT_NUM.SetData(EUC.GetString(btData, pos, this.UNSENT_NUM.GetByteLength()));
                    pos += UNSENT_NUM.GetByteLength();

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
    }

    #endregion "出力情報モデル"

    #endregion "受信モデル"


    #endregion "SYG"


    #region "TCCモデル"

    #region "送信モデル"

    public class ProcessTCCSendModel : NaccsSendModel
    {
        ///// <summary>
        ///// 入力共通項目398
        ///// </summary>
        //public InputCommonModel InputCommon;
        public String GyomuString = "TCC";
        public NacCollumnAN SendMessage1 = new NacCollumnAN(50);
        public NacCollumnAN SendMessage2 = new NacCollumnAN(50);
        public NacCollumnAN SendMessage3 = new NacCollumnAN(1);
        /// <summary>
        /// 電文引継情報
        /// </summary>
        public string DEFAULT_DENBUN_HANDOVER
        {
            get { return string.Format("{0}{1}", this.GyomuString, DateTime.Now.ToString("yyyyMMddhhmmss")); }
        }
        /// <summary>
        /// 入力情報番号
        /// </summary>
        public string DEFAULT_INPUT_INFO_NO
        {
            get { return this.GyomuString; }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProcessTCCSendModel(string strInfoCD = "",  string strSysType = "2", string strSendMes1 = "", string strSendMes2 = "", string strSendMes3 = "")
        {
            this.NaccsCommon = new InputCommonModel();

            this.NaccsCommon.SYSTEM_TYPE.SetData(strSysType);
            if (strInfoCD != "")
            {
                this.GyomuString = strInfoCD;
            }
            //int len = strSendMes1.Length;
            //this.SendMessage1 = new NacCollumnAN(len);
            this.SendMessage1.SetData(strSendMes1);

            //len = strSendMes2.Length;
            //this.SendMessage2 = new NacCollumnAN(len);
            this.SendMessage2.SetData(strSendMes2);

            //len = strSendMes3.Length;
            //this.SendMessage3 = new NacCollumnAN(len);
            this.SendMessage3.SetData(strSendMes3);

            this.ReportType = NACCS_REPORTTYPE.OTHER;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetByteLength()
        {
            int intlen = 0;
            intlen += NaccsCommon.GetByteLength();

            intlen += SendMessage1.GetByteLength();
            intlen += SendMessage2.GetByteLength();
            intlen += SendMessage3.GetByteLength();
            
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
            int pos = 0;

            try
            {
                //this.NaccsCommon.GYOMU_CD.SetData("SYG");
                this.NaccsCommon.GYOMU_CD.SetData(this.GyomuString);
                this.NaccsCommon.USER_CD.SetData(HPFData.InputUserCode);
                this.NaccsCommon.SHIKIBETSU_NO.SetData(HPFData.NaccsShikibetsuNo);
                this.NaccsCommon.USER_PASS.SetData(HPFData.InputUserPass);

                this.NaccsCommon.DENBUN_HANDOVER.SetData(DEFAULT_DENBUN_HANDOVER);
                this.NaccsCommon.INPUT_INFO_NO.SetData(DEFAULT_INPUT_INFO_NO);
                this.NaccsCommon.SAKUIN_HANDOVER.SetData("");
                //this.NaccsCommon.SYSTEM_TYPE.SetData("2");//NACCS:航空:1海上:2

                Array.Copy(this.NaccsCommon.GetByteData(intLast), 0, btData, pos, this.NaccsCommon.GetByteLength());
                pos += this.NaccsCommon.GetByteLength();

                Array.Copy(this.SendMessage1.GetByteData(), 0, btData, pos, this.SendMessage1.GetByteLength());
                pos += SendMessage1.GetByteLength();

                Array.Copy(this.SendMessage2.GetByteData(), 0, btData, pos, this.SendMessage2.GetByteLength());
                pos += SendMessage2.GetByteLength();

                Array.Copy(this.SendMessage3.GetByteData(), 0, btData, pos, this.SendMessage3.GetByteLength());
                pos += SendMessage3.GetByteLength();
                return btData;
            }
            catch (Exception e)
            {
                LogOut.ErrorOut(e.ToString(), this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                throw e;
            }

        }
        
    }
    #endregion "送信モデル"

    #region "受信モデル"

    /// <summary>
    /// 処理結果モデル
    /// </summary>
    public class ProcessTCCRecvModel : NaccsRecvModel
    {
        ///// <summary>
        ///// 出力共通項目
        ///// </summary>
        //public OutputCommonModel OutputCommon = new OutputCommonModel();
        /// <summary>
        /// 処理結果コード
        /// </summary>
        public NacCollumnAN OutputResult = new NacCollumnAN(75);

        public NacCollumnAN Message1 = new NacCollumnAN(50);

        public NacCollumnAN Message2 = new NacCollumnAN(50);

        ///// <summary>
        ///// 情報コード
        ///// </summary>
        //public List<InfoOutModel> InfoList = new List<InfoOutModel>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProcessTCCRecvModel()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetByteLength()
        {
            int intlen = 0;

            intlen += NaccsCommon.GetByteLength();
            intlen += this.Message1.GetByteLength();
            intlen += this.Message2.GetByteLength();

            return intlen;
        }

        /// <summary>
        /// byte配列から各値を設定
        /// </summary>
        /// <param name="btData"></param>
        /// <returns></returns>
        public override Boolean SetByteData(byte[] btData)
        {
            int intLast = this.GetByteLength();
            int pos = 0, len = 0;

            try
            {
                if (btData.Length >= intLast)
                {
                    this.NaccsCommon.SetByteData(btData);
                    pos += NaccsCommon.GetByteLength();

                    this.OutputResult.SetData(EUC.GetString(btData, pos, this.OutputResult.GetByteLength()));
                    pos += OutputResult.GetByteLength();

                    this.Message1.SetData(EUC.GetString(btData, pos, this.Message1.GetByteLength()));
                    pos += Message1.GetByteLength();

                    this.Message2.SetData(EUC.GetString(btData, pos, this.Message2.GetByteLength()));
                    pos += Message2.GetByteLength();

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

    }

    #endregion "受信モデル"


    #endregion "TCC"
}
