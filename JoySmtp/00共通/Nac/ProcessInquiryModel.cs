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

    #region "OTHERモデル"

    #region "送信モデル"

    public class ProcessInquirySendModel : ProcessSYGSendModel
    {
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
        public ProcessInquirySendModel(SendType stype = SendType.OTHER, string strInfoCD = "", string strSysType = "2", string strSendMes = "")
        {
            this.NaccsCommon = new InputCommonModel();
            this.ProcessType = stype;
           
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
                this.NaccsCommon.GYOMU_CD.SetData(this.GyomuString);
                this.NaccsCommon.USER_CD.SetData(HPFData.InputUserCode);
                this.NaccsCommon.SHIKIBETSU_NO.SetData(HPFData.NaccsShikibetsuNo);
                this.NaccsCommon.USER_PASS.SetData(HPFData.InputUserPass);

                this.NaccsCommon.DENBUN_HANDOVER.SetData(DEFAULT_DENBUN_HANDOVER);
                this.NaccsCommon.INPUT_INFO_NO.SetData(DEFAULT_INPUT_INFO_NO);
                this.NaccsCommon.SAKUIN_HANDOVER.SetData("");

                Array.Copy(this.NaccsCommon.GetByteData(intLast), 0, btData, pos, this.NaccsCommon.GetByteLength());
                pos += this.NaccsCommon.GetByteLength();

                Array.Copy(this.SendMessage.GetByteData(), 0, btData, pos, this.SendMessage.GetByteLength());
                pos += SendMessage.GetByteLength();

                return btData;
            }
            catch (Exception e)
            {
                LogOut.ErrorOut(e.ToString(), MethodBase.GetCurrentMethod().Name);
                throw e;
            }
            
        }
        ///// <summary>
        ///// バイトデータから内容の取得　※※※
        ///// </summary>
        ///// <param name="btData"></param>
        ///// <returns></returns>
        //public override Boolean SetByteData(byte[] btData)
        //{
        //    int intLast = this.GetByteLength();
        //    int pos = 0;

        //    try
        //    {
        //        if (btData.Length >= intLast)
        //        {
        //            //this.GYOMU_CD.SetData(EUC.GetString(btData, pos, this.GYOMU_CD.GetByteLength()));
        //            //pos += GYOMU_CD.GetByteLength();

        //            //this.OUTPUT_INFO.SetData(EUC.GetString(btData, pos, this.OUTPUT_INFO.GetByteLength()));
        //            //pos += OUTPUT_INFO.GetByteLength();

        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        LogOut.ErrorOut(e.ToString(), MethodBase.GetCurrentMethod().Name);
        //        throw e;
        //    }
        //}
    }
    #endregion "送信モデル"

    #region "受信モデル"

    /// <summary>
    /// 処理結果モデル
    /// </summary>
    public class ProcessInquiryRecvModel : ProcessSYGRecvModel
    {
         /// <summary>
        /// 情報コード
        /// </summary>
        public List<InfoOutModel> InfoList = new List<InfoOutModel>();
        
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
                LogOut.ErrorOut(e.ToString(), MethodBase.GetCurrentMethod().Name);
                throw e;
            }
        }
        ///// <summary>
        ///// 添付用データの作成
        ///// </summary>
        ///// <returns></returns>
        //public override byte[] GetByteData()
        //{
        //    int intLast = this.GetByteLength();
        //    byte[] btData = new byte[intLast];
        //    int pos = 0, intLen = 0;

        //    try
        //    {
        //        intLen = GetByteLengthMain();
        //        if (intLen > 0)
        //        {
        //            Array.Copy(this.NaccsCommon.GetByteData(intLen), 0, btData, pos, this.NaccsCommon.GetByteLength());
        //            pos += NaccsCommon.GetByteLength();

        //            Array.Copy(this.OutputResult.GetByteData(), 0, btData, pos, this.OutputResult.GetByteLength());
        //            pos += OutputResult.GetByteLength();

        //            Array.Copy(this.InfoNum.GetByteData(), 0, btData, pos, this.InfoNum.GetByteLength());
        //            pos += InfoNum.GetByteLength();

        //            for (int i = 0; i < InfoList.Count(); i++)
        //            {
        //                Array.Copy(this.InfoList[i].GetByteData(), 0, btData, pos, this.InfoList[i].GetByteLength());
        //                pos += InfoList[i].GetByteLength();
        //            }
        //            return btData;
        //        }
        //        else return null;
        //    }
        //    catch (Exception e)
        //    {
        //        LogOut.ErrorOut(e.ToString(), MethodBase.GetCurrentMethod().Name);
        //        throw e;
        //    }
        //}
    }



    #endregion "受信モデル"


    #endregion "SYG"

}
