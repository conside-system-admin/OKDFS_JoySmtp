using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using JoySmtp.CLogOut;

namespace JoySmtp.Nac
{
    //case NACCS_REPORTTYPE.SAD4011://輸入申告入力控(沖縄特免制度)情報 OTA
    //case NACCS_REPORTTYPE.SAD4021://輸入申告変更入力控(沖縄特免制度)     OTA01 

    //case NACCS_REPORTTYPE.SAD4031://輸入申告控（Cの時）                  OTA　     OTC
    //case NACCS_REPORTTYPE.SAD4041://輸入許可前貨物引取承認申請控 SAD403　OTA       OTC
    //case NACCS_REPORTTYPE.SAD4051://輸入申告変更控               SAD403      OTA01     OTE
    //case NACCS_REPORTTYPE.SAD4061://輸入許可前貨物引取承認申請変更控SAD403   OTA01     OTE
    //case NACCS_REPORTTYPE.SAD4071://輸入許可通知兼申告控情報　   SAD403　OTA     　OTC
    //case NACCS_REPORTTYPE.SAD4081://許可前貨物引取承認通知書兼申請控情報 OTA　     OTC
    //case NACCS_REPORTTYPE.SAD4091://輸入許可通知兼申告変更控情報　SAD403　   OTA01     OTE
    //case NACCS_REPORTTYPE.SAD4101://輸入許可前貨物引取承認通知兼申請変更控   OTA01     OTE

    //case NACCS_REPORTTYPE.SAD4131://輸入許可･承認貨物(沖縄特免制度)情報　    OTA01 OTC
    //case NACCS_REPORTTYPE.SAD4151://輸入申告事項登録(沖縄特免制度)情報　    OTB
    //case NACCS_REPORTTYPE.SAD4161://輸入申告変更事項登録(沖縄特免制度)情報  OTD

    //case NACCS_REPORTTYPE.SAF0010://納付書情報　                         OTA OTA01 OTC
    //case NACCS_REPORTTYPE.SAF0021://納付番号通知情報                     OTA OTA01 OTC
    //case NACCS_REPORTTYPE.SAF0221://担保不足通知情報                     OTA OTA01 OTC

    #region "輸入申告入力控　SAD401"

    /// <summary>
    /// 輸入申告入力控　SAD401
    /// 5038    OTA　輸入申告事項登録
    /// 処理結果通知  ＊ＳＯＴＡ   ○   △   R
    /// 輸入申告入力控（沖縄特免制度）情報   ＳＡＤ４０１１ ○   C
    /// 輸入申告控（沖縄特免制度）情報 ＳＡＤ４０３１ ○   P
    /// 輸入許可前貨物引取承認申請控（沖縄特免制度）情報    ＳＡＤ４０４１ ○   P
    /// 輸入許可通知兼申告控（沖縄特免制度）情報    ＳＡＤ４０７１ ○   P
    /// 輸入許可前貨物引取承認通知兼申請控（沖縄特免制度）情報 ＳＡＤ４０８１ ○   P
    /// 納付書情報（直納）   ＳＡＦ００１０ ○   P
    /// 許可・承認貨物（沖縄特免制度）情報   ＳＡＤ４１３１ ○   ○   P
    /// 担保不足通知情報    ＳＡＦ０２２１ ○   P
    /// 納付番号通知情報    ＳＡＦ００２１ ○   P
    /// </summary>
    public class ProcessSad401Model : NaccsRecvModel
    {
        public override string GetFileName()
        {
            if (string.IsNullOrEmpty(YunyuShinkokuNo.Data))
            {
                return string.Format("{0}_{1}_{2}", LOGSTR, this.GetReportType.ToString(), DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            }
            else
            {
                //return string.Format("{0}_{1}_{2}", GetYunyuShinkokuNo, this.GetReportType.ToString(), this.KijiHanbai.Data);
                return string.Format("{0}_{1}_{2}_{3}", NaccsCommon.USER_CD.Data, this.GetReportType.ToString(), GetYunyuShinkokuNo, DateTime.Now.ToString("yyyyMMddhhmmss"));
            }
        }
        ///// <summary>
        ///// 入力共通項目398
        ///// </summary>
        //public OutputCommonModel OutputCommon = new OutputCommonModel();

        /// <summary>
        /// 宛先税関コード 2
        /// </summary>
        public NacCollumnAN ToZeikan = new NacCollumnAN(10);
        /// <summary>
        /// 宛先部門コード 3
        /// </summary>
        public NacCollumnAN BumonCode = new NacCollumnAN(2);
        /// <summary>
        /// 申告年月日　4
        /// </summary>
        public NacCollumnDate ShinkokuDate = new NacCollumnDate(8);
        /// <summary>
        /// 通関予定蔵置場コード 5
        /// </summary>
        public NacCollumnAN KuraokibaCode = new NacCollumnAN(5);

        ///// <summary>
        ///// 輸入申告番号 6
        ///// </summary>
        //public NacCollumnAN YunyuShinkokuNo = new NacCollumnAN(11);
        /// <summary>
        /// 旅客氏名 7
        /// </summary>
        public NacCollumnAN YunyushaName = new NacCollumnAN(70);
        /// <summary>
        /// 搭乗航空会社名 8
        /// </summary>
        public NacCollumnAN KoukuName = new NacCollumnAN(5);
        /// <summary>
        /// 旅客郵便番号 9
        /// </summary>
        public NacCollumnAN YubinBango = new NacCollumnAN(7);
        /// <summary>
        /// 旅客住所１都道府県 10
        /// </summary>
        public NacCollumnAN JushoKen = new NacCollumnAN(15);
        /// <summary>
        /// 旅客住所２市区町村 11
        /// </summary>
        public NacCollumnAN JushoShi = new NacCollumnAN(35);
        /// <summary>
        /// 搭乗便名 12
        /// </summary>
        public NacCollumnAN TojoBinName = new NacCollumnAN(5);
        /// <summary>
        /// 旅客住所３町域名・番地 13
        /// </summary>
        public NacCollumnAN JushoBanchi = new NacCollumnAN(35);
        /// <summary>
        /// 旅客住所４ビル名 14
        /// </summary>
        public NacCollumnAN JushoBill = new NacCollumnAN(70);
        /// <summary>
        /// 代理人コード　15
        /// </summary>
        public NacCollumnAN DairiCode = new NacCollumnAN(5);
        /// <summary>
        /// 代理人氏名　16
        /// </summary>
        public NacCollumnAN DairiName = new NacCollumnAN(50);
        /// <summary>
        /// 通関士コード　17
        /// </summary>
        public NacCollumnAN TukanCode = new NacCollumnAN(5);
        /// <summary>
        /// 購入価格の合計 18
        /// </summary>
        public NacCollumnN KounyuKakakuTotal = new NacCollumnN(10);
        /// <summary>
        /// 関税免除済購入額 19
        /// </summary>
        public NacCollumnN KanzeiMenjoGaku = new NacCollumnN(9) ;

        /// <summary>
        /// 税科目リスト
        /// </summary>
        public List<ZeiKamokuModel> ZeikamokuList = new List<ZeiKamokuModel>();
        /// <summary>
        /// 税科目リストの最大値
        /// </summary>
        public int ZEIKAMOKU_MAX = 7;

        /// <summary>
        /// 納税額合計 24
        /// </summary>
        public NacCollumnN NouzeiTotal = new NacCollumnN(11);
        /// <summary>
        /// 担保額 25
        /// </summary>
        public NacCollumnN TanpoGaku = new NacCollumnN(11);
        
        /// <summary>
        /// 都道府県コード　26
        /// </summary>
        public NacCollumnAN TodofukenCode = new NacCollumnAN(2);
        /// <summary>
        /// 口座識別　27
        /// </summary>
        public NacCollumnAN KozaCode = new NacCollumnAN(1);
        /// <summary>
        /// 納付方法識別　28
        /// </summary>
        public NacCollumnAN NoufuHouhouCode = new NacCollumnAN(1);
        /// <summary>
        /// BP申請事由コード　29
        /// </summary>
        public NacCollumnAN BPJiyuCode = new NacCollumnAN(2);
        /// <summary>
        /// 構成枚数　30
        /// </summary>
        public NacCollumnN KouseiMaiSu = new NacCollumnN(2);
        /// <summary>
        /// 申請欄数　31
        /// </summary>
        public NacCollumnN ShinseiRanNum = new NacCollumnN(2);
        /// <summary>
        /// 記事（税関用） 32
        /// </summary>
        public NacCollumnJ KijiZeikan = new NacCollumnJ(140);
        /// <summary>
        /// 記事（販売店用） 33
        /// </summary>
        public NacCollumnJ KijiHanbai = new NacCollumnJ(70);
        /// <summary>
        /// 記事（その他） 34
        /// </summary>
        public NacCollumnJ KijiOther = new NacCollumnJ(70);
        /// <summary>
        /// 社内整理用番号 35
        /// </summary>
        public NacCollumnAN SeiriyouNo = new NacCollumnAN(20);
        /// <summary>
        /// 利用者用整理番号 36
        /// </summary>
        public NacCollumnAN UserSeiriNo = new NacCollumnAN(5);
        /// <summary>
        /// 登録商品リスト 37～
        /// </summary>
        public List<ShohinRecvModel> Shohin = new List<ShohinRecvModel>();
        
            
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProcessSad401Model()
        {
            NaccsCommon = new OutputCommonModel();
            //OutputCommon = new OutputCommonModel();
            
            for (int i = 0; i < ZEIKAMOKU_MAX; i++)
            {
                ZeiKamokuModel zeikamoku = new ZeiKamokuModel();
                this.ZeikamokuList.Add(zeikamoku);
            }

            Shohin = new List<ShohinRecvModel>();
            ShohinRecvModel sm = new ShohinRecvModel();
            Shohin.Add(sm);
        }
        /// <summary>
        /// データバイト長取得
        /// </summary>
        /// <returns></returns>
        public int GetByteLength()
        {
            int intlen = 0;
            
            intlen += NaccsCommon.GetByteLength();
            intlen += GetByteLengthBasic();

            for (int i = 0; i < Shohin.Count();i++ )
            {
                intlen += Shohin[i].GetByteLength();
            }
            return intlen;
        }
        /// <summary>
        /// 内容の長さ
        /// </summary>
        /// <returns></returns>
        protected int GetByteLengthBasic()
        {
            int intlen = 0;

            intlen += ToZeikan.GetByteLength();
            intlen += BumonCode.GetByteLength();
            intlen += ShinkokuDate.GetByteLength();
            intlen += KuraokibaCode.GetByteLength();
            intlen += YunyuShinkokuNo.GetByteLength();
            intlen += YunyushaName.GetByteLength();
            intlen += KoukuName.GetByteLength();
            intlen += YubinBango.GetByteLength();
            intlen += JushoKen.GetByteLength();
            intlen += JushoShi.GetByteLength();
            intlen += TojoBinName.GetByteLength();
            intlen += JushoBanchi.GetByteLength();
            intlen += JushoBill.GetByteLength();
            intlen += DairiCode.GetByteLength();
            intlen += DairiName.GetByteLength();
            intlen += TukanCode.GetByteLength();
            intlen += KounyuKakakuTotal.GetByteLength();
            intlen += KanzeiMenjoGaku.GetByteLength();
            for (int i = 0; i < ZEIKAMOKU_MAX; i++)
            {
                intlen += this.ZeikamokuList[i].GetByteLength();
            }
            intlen += NouzeiTotal.GetByteLength();
            intlen += TanpoGaku.GetByteLength();
            intlen += TodofukenCode.GetByteLength();
            intlen += KozaCode.GetByteLength();
            intlen += NoufuHouhouCode.GetByteLength();
            intlen += BPJiyuCode.GetByteLength();
            intlen += KouseiMaiSu.GetByteLength();
            intlen += ShinseiRanNum.GetByteLength();
            intlen += KijiZeikan.GetByteLength();
            intlen += KijiHanbai.GetByteLength();
            intlen += KijiOther.GetByteLength();
            intlen += SeiriyouNo.GetByteLength();
            intlen += UserSeiriNo.GetByteLength();

            return intlen;
        }
        /// <summary>
        /// 添付用データの作成
        /// </summary>
        /// <returns></returns>
        public override byte[] GetByteData()
        {
            int intLen = this.GetByteLength();
            byte[] btData = new byte[intLen];
            int pos = 0;
            try
            {
                if (intLen > 0)
                {
                    Array.Copy(NaccsCommon.GetByteData(intLen), 0, btData, pos, NaccsCommon.GetByteLength());
                    pos += NaccsCommon.GetByteLength();

                    Array.Copy(this.GetByteDataBasic(), 0, btData, pos, this.GetByteLengthBasic());
                    pos += this.GetByteLengthBasic();

                    for (int i = 0; i < Shohin.Count(); i++)
                    {
                        Array.Copy(this.Shohin[i].GetByteData(), 0, btData, pos, this.Shohin[i].GetByteLength());
                        pos += Shohin[i].GetByteLength();
                    }

                }
                return btData;
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
        public byte[] GetByteDataBasic()
        {
            int intLast = this.GetByteLengthBasic();
            byte[] btData = new byte[intLast];
            int pos = 0;
            Array.Copy(this.ToZeikan.GetByteData(), 0, btData, pos, this.ToZeikan.GetByteLength());
            pos += ToZeikan.GetByteLength();
            Array.Copy(this.BumonCode.GetByteData(), 0, btData, pos, this.BumonCode.GetByteLength());
            pos += BumonCode.GetByteLength();
            Array.Copy(this.ShinkokuDate.GetByteData(), 0, btData, pos, this.ShinkokuDate.GetByteLength());
            pos += ShinkokuDate.GetByteLength();
            Array.Copy(this.KuraokibaCode.GetByteData(), 0, btData, pos, this.KuraokibaCode.GetByteLength());
            pos += KuraokibaCode.GetByteLength();
            Array.Copy(this.YunyuShinkokuNo.GetByteData(), 0, btData, pos, this.YunyuShinkokuNo.GetByteLength());
            pos += YunyuShinkokuNo.GetByteLength();
            Array.Copy(this.YunyushaName.GetByteData(), 0, btData, pos, this.YunyushaName.GetByteLength());
            pos += YunyushaName.GetByteLength();
            Array.Copy(this.KoukuName.GetByteData(), 0, btData, pos, this.KoukuName.GetByteLength());
            pos += KoukuName.GetByteLength();
            Array.Copy(this.YubinBango.GetByteData(), 0, btData, pos, this.YubinBango.GetByteLength());
            pos += YubinBango.GetByteLength();
            Array.Copy(this.JushoKen.GetByteData(), 0, btData, pos, this.JushoKen.GetByteLength());
            pos += JushoKen.GetByteLength();
            Array.Copy(this.JushoShi.GetByteData(), 0, btData, pos, this.JushoShi.GetByteLength());
            pos += JushoShi.GetByteLength();
            Array.Copy(this.TojoBinName.GetByteData(), 0, btData, pos, this.TojoBinName.GetByteLength());
            pos += TojoBinName.GetByteLength();
            Array.Copy(this.JushoBanchi.GetByteData(), 0, btData, pos, this.JushoBanchi.GetByteLength());
            pos += JushoBanchi.GetByteLength();
            Array.Copy(this.JushoBill.GetByteData(), 0, btData, pos, this.JushoBill.GetByteLength());
            pos += JushoBill.GetByteLength();
            Array.Copy(this.DairiCode.GetByteData(), 0, btData, pos, this.DairiCode.GetByteLength());
            pos += DairiCode.GetByteLength();
            Array.Copy(this.DairiName.GetByteData(), 0, btData, pos, this.DairiName.GetByteLength());
            pos += DairiName.GetByteLength();
            Array.Copy(this.TukanCode.GetByteData(), 0, btData, pos, this.TukanCode.GetByteLength());
            pos += TukanCode.GetByteLength();
            Array.Copy(this.KounyuKakakuTotal.GetByteData(), 0, btData, pos, this.KounyuKakakuTotal.GetByteLength());
            pos += KounyuKakakuTotal.GetByteLength();
            Array.Copy(this.KanzeiMenjoGaku.GetByteData(), 0, btData, pos, this.KanzeiMenjoGaku.GetByteLength());
            pos += KanzeiMenjoGaku.GetByteLength();

            for (int i = 0; i < ZEIKAMOKU_MAX; i++)
            {
                Array.Copy(this.ZeikamokuList[i].GetByteData(), 0, btData, pos, this.ZeikamokuList[i].GetByteLength());
                pos += this.ZeikamokuList[i].GetByteLength();
            }

            Array.Copy(this.NouzeiTotal.GetByteData(), 0, btData, pos, this.NouzeiTotal.GetByteLength());
            pos += NouzeiTotal.GetByteLength();
            Array.Copy(this.TanpoGaku.GetByteData(), 0, btData, pos, this.TanpoGaku.GetByteLength());
            pos += TanpoGaku.GetByteLength();
            Array.Copy(this.TodofukenCode.GetByteData(), 0, btData, pos, this.TodofukenCode.GetByteLength());
            pos += TodofukenCode.GetByteLength();
            Array.Copy(this.KozaCode.GetByteData(), 0, btData, pos, this.KozaCode.GetByteLength());
            pos += KozaCode.GetByteLength();
            Array.Copy(this.NoufuHouhouCode.GetByteData(), 0, btData, pos, this.NoufuHouhouCode.GetByteLength());
            pos += NoufuHouhouCode.GetByteLength();
            Array.Copy(this.BPJiyuCode.GetByteData(), 0, btData, pos, this.BPJiyuCode.GetByteLength());
            pos += BPJiyuCode.GetByteLength();
            Array.Copy(this.KouseiMaiSu.GetByteData(), 0, btData, pos, this.KouseiMaiSu.GetByteLength());
            pos += KouseiMaiSu.GetByteLength();
            Array.Copy(this.ShinseiRanNum.GetByteData(), 0, btData, pos, this.ShinseiRanNum.GetByteLength());
            pos += ShinseiRanNum.GetByteLength();
            Array.Copy(this.KijiZeikan.GetByteData(), 0, btData, pos, this.KijiZeikan.GetByteLength());
            pos += KijiZeikan.GetByteLength();
            Array.Copy(this.KijiHanbai.GetByteData(), 0, btData, pos, this.KijiHanbai.GetByteLength());
            pos += KijiHanbai.GetByteLength();
            Array.Copy(this.KijiOther.GetByteData(), 0, btData, pos, this.KijiOther.GetByteLength());
            pos += KijiOther.GetByteLength();
            Array.Copy(this.SeiriyouNo.GetByteData(), 0, btData, pos, this.SeiriyouNo.GetByteLength());
            pos += SeiriyouNo.GetByteLength();
            Array.Copy(this.UserSeiriNo.GetByteData(), 0, btData, pos, this.UserSeiriNo.GetByteLength());
            pos += UserSeiriNo.GetByteLength();
            return btData;
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
                    this.NaccsCommon.SetByteData(btData.Skip(pos).Take(this.NaccsCommon.GetByteLength()).ToArray());
                    pos += this.NaccsCommon.GetByteLength();

                    this.ToZeikan.SetData(EUC.GetString(btData, pos, this.ToZeikan.GetByteLength()));
                    pos += ToZeikan.GetByteLength();

                    this.BumonCode.SetData(EUC.GetString(btData, pos, this.BumonCode.GetByteLength()));
                    pos += BumonCode.GetByteLength();

                    this.ShinkokuDate.SetData(EUC.GetString(btData, pos, this.ShinkokuDate.GetByteLength()));
                    pos += ShinkokuDate.GetByteLength();

                    this.KuraokibaCode.SetData(EUC.GetString(btData, pos, this.KuraokibaCode.GetByteLength()));
                    pos += KuraokibaCode.GetByteLength();

                    this.YunyuShinkokuNo.SetData(EUC.GetString(btData, pos, this.YunyuShinkokuNo.GetByteLength()));
                    pos += YunyuShinkokuNo.GetByteLength();

                    this.YunyushaName.SetData(EUC.GetString(btData, pos, this.YunyushaName.GetByteLength()));
                    pos += YunyushaName.GetByteLength();

                    this.KoukuName.SetData(EUC.GetString(btData, pos, this.KoukuName.GetByteLength()));
                    pos += KoukuName.GetByteLength();

                    this.YubinBango.SetData(EUC.GetString(btData, pos, this.YubinBango.GetByteLength()));
                    pos += YubinBango.GetByteLength();

                    this.JushoKen.SetData(EUC.GetString(btData, pos, this.JushoKen.GetByteLength()));
                    pos += JushoKen.GetByteLength();

                    this.JushoShi.SetData(EUC.GetString(btData, pos, this.JushoShi.GetByteLength()));
                    pos += JushoShi.GetByteLength();

                    this.TojoBinName.SetData(EUC.GetString(btData, pos, this.TojoBinName.GetByteLength()));
                    pos += TojoBinName.GetByteLength();

                    this.JushoBanchi.SetData(EUC.GetString(btData, pos, this.JushoBanchi.GetByteLength()));
                    pos += JushoBanchi.GetByteLength();

                    this.JushoBill.SetData(EUC.GetString(btData, pos, this.JushoBill.GetByteLength()));
                    pos += JushoBill.GetByteLength();

                    this.DairiCode.SetData(EUC.GetString(btData, pos, this.DairiCode.GetByteLength()));
                    pos += DairiCode.GetByteLength();

                    this.DairiName.SetData(EUC.GetString(btData, pos, this.DairiName.GetByteLength()));
                    pos += DairiName.GetByteLength();

                    this.TukanCode.SetData(EUC.GetString(btData, pos, this.TukanCode.GetByteLength()));
                    pos += TukanCode.GetByteLength();

                    this.KounyuKakakuTotal.SetData(EUC.GetString(btData, pos, this.KounyuKakakuTotal.GetByteLength()));
                    pos += KounyuKakakuTotal.GetByteLength();

                    this.KanzeiMenjoGaku.SetData(EUC.GetString(btData, pos, this.KanzeiMenjoGaku.GetByteLength()));
                    pos += KanzeiMenjoGaku.GetByteLength();
                    
                    for (int i = 0; i < ZEIKAMOKU_MAX; i++)
                    {
                        this.ZeikamokuList[i].SetByteData(btData.Skip(pos).Take(this.ZeikamokuList[i].GetByteLength()).ToArray());
                        pos += ZeikamokuList[i].GetByteLength();
                    }

                    this.NouzeiTotal.SetData(EUC.GetString(btData, pos, this.NouzeiTotal.GetByteLength()));
                    pos += NouzeiTotal.GetByteLength();

                    this.TanpoGaku.SetData(EUC.GetString(btData, pos, this.TanpoGaku.GetByteLength()));
                    pos += TanpoGaku.GetByteLength();

                    this.TodofukenCode.SetData(EUC.GetString(btData, pos, this.TodofukenCode.GetByteLength()));
                    pos += TodofukenCode.GetByteLength();

                    this.KozaCode.SetData(EUC.GetString(btData, pos, this.KozaCode.GetByteLength()));
                    pos += KozaCode.GetByteLength();

                    this.NoufuHouhouCode.SetData(EUC.GetString(btData, pos, this.NoufuHouhouCode.GetByteLength()));
                    pos += NoufuHouhouCode.GetByteLength();

                    this.BPJiyuCode.SetData(EUC.GetString(btData, pos, this.BPJiyuCode.GetByteLength()));
                    pos += BPJiyuCode.GetByteLength();

                    this.KouseiMaiSu.SetData(EUC.GetString(btData, pos, this.KouseiMaiSu.GetByteLength()));
                    pos += KouseiMaiSu.GetByteLength();

                    this.ShinseiRanNum.SetData(EUC.GetString(btData, pos, this.ShinseiRanNum.GetByteLength()));
                    pos += ShinseiRanNum.GetByteLength();

                    if (this.ShinseiRanNum.GetIntData > 1)
                    {
                        for (int i = 1; i < this.ShinseiRanNum.GetIntData; i++)
                        {
                            ShohinRecvModel sm = new ShohinRecvModel();
                            this.Shohin.Add(sm);
                        }
                    }

                    this.KijiZeikan.SetData(EUC.GetString(btData, pos, this.KijiZeikan.GetByteLength()));
                    pos += KijiZeikan.GetByteLength();

                    this.KijiHanbai.SetData(EUC.GetString(btData, pos, this.KijiHanbai.GetByteLength()));
                    pos += KijiHanbai.GetByteLength();

                    this.KijiOther.SetData(EUC.GetString(btData, pos, this.KijiOther.GetByteLength()));
                    pos += KijiOther.GetByteLength();

                    this.SeiriyouNo.SetData(EUC.GetString(btData, pos, this.SeiriyouNo.GetByteLength()));
                    pos += SeiriyouNo.GetByteLength();

                    this.UserSeiriNo.SetData(EUC.GetString(btData, pos, this.UserSeiriNo.GetByteLength()));
                    pos += UserSeiriNo.GetByteLength();
                    
                    for (int i = 0; i < Shohin.Count(); i++)
                    {
                        this.Shohin[i].SetByteData(btData.Skip(pos).Take(this.Shohin[i].GetByteLength()).ToArray());
                        pos += Shohin[i].GetByteLength();
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
        /// ファイル用のデータを取得
        /// </summary>
        /// <returns></returns>
        public override string GetFileData()
        {
            try
            {
                var btdata = this.GetByteData();
                return System.Text.Encoding.GetEncoding(51932).GetString(btdata);
            }
            //StringBuilder str = new StringBuilder();

            //try
            //{
            //    str.AppendLine(NaccsCommon.GetFileData());
            //    str.AppendLine(ToZeikan.Data);
            //    str.AppendLine(BumonCode.Data);
            //    str.AppendLine(ShinkokuDate.Data);
            //    str.AppendLine(KuraokibaCode.Data);
            //    str.AppendLine(YunyuShinkokuNo.Data);
            //    str.AppendLine(YunyushaName.Data);
            //    str.AppendLine(KoukuName.Data);
            //    str.AppendLine(YubinBango.Data);
            //    str.AppendLine(JushoKen.Data);
            //    str.AppendLine(JushoShi.Data);
            //    str.AppendLine(TojoBinName.Data);
            //    str.AppendLine(JushoBanchi.Data);
            //    str.AppendLine(JushoBill.Data);
            //    str.AppendLine(DairiCode.Data);
            //    str.AppendLine(DairiName.Data);
            //    str.AppendLine(TukanCode.Data);
            //    str.AppendLine(KounyuKakakuTotal.Data);
            //    str.AppendLine(KanzeiMenjoGaku.Data);
            //    for (int i = 0; i < ZEIKAMOKU_MAX; i++)
            //    {
            //        str.AppendLine(this.ZeikamokuList[i].GetFileData());
            //    }
            //    str.AppendLine(NouzeiTotal.Data);
            //    str.AppendLine(TanpoGaku.Data);
            //    str.AppendLine(TodofukenCode.Data);
            //    str.AppendLine(KozaCode.Data);
            //    str.AppendLine(NoufuHouhouCode.Data);
            //    str.AppendLine(BPJiyuCode.Data);
            //    str.AppendLine(KouseiMaiSu.Data);
            //    str.AppendLine(ShinseiRanNum.Data);
            //    str.AppendLine(KijiZeikan.Data);
            //    str.AppendLine(KijiHanbai.Data);
            //    str.AppendLine(KijiOther.Data);
            //    str.AppendLine(SeiriyouNo.Data);
            //    str.AppendLine(UserSeiriNo.Data);

            //    for (int i = 0; i < Shohin.Count(); i++)
            //    {
            //        str.AppendLine(Shohin[i].GetFileData());
            //    }
            //    return str.ToString();
            //}
            catch (Exception ex)
            {
                throw ex;
                //return "ProcessSad401Model Error\r\n" + ex.ToString();
            }
        }
    }

    #endregion "輸入申告入力控　SAD401"

    #region "輸入申告控　SAD403 or 404 405 406 407 408 409 410"


    /// <summary>
    /// 輸入申告控え　403or404
    /// ＳＡＤ４０３１　P
    /// </summary>
    public class ProcessSad403Model : ProcessSad401Model
    {
        /// <summary>
        /// 税関官署長名　37　　　　
        /// </summary>
        public NacCollumnJJ ZeikanShochoName = new NacCollumnJJ(36);
        /// <summary>
        /// 許可・承認年月日タイトル　38
        /// </summary>
        public NacCollumnJJ KyokaShoninDateTitle = new NacCollumnJJ(10);
        /// <summary>
        /// 許可・承認年月日　39
        /// </summary>
        public NacCollumnDate KyokaShoninDate = new NacCollumnDate(8);
        /// <summary>
        /// 延滞税額合計タイトル　40
        /// </summary>
        public NacCollumnJ EntaiZeiTotalTitle = new NacCollumnJ(12);
        /// <summary>
        /// 延滞税額合計　41
        /// </summary>
        public NacCollumnN EntaiZeiTotal = new NacCollumnN(11);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProcessSad403Model():base()
        {
        }
        /// <summary>
        /// 内容の長さ
        /// </summary>
        /// <returns></returns>
        private new int GetByteLength()
        {
            int intlen = 0;

            intlen += this.NaccsCommon.GetByteLength();
            intlen += base.GetByteLengthBasic();

            intlen += ZeikanShochoName.GetByteLength();
            intlen += KyokaShoninDateTitle.GetByteLength();
            intlen += KyokaShoninDate.GetByteLength();
            intlen += EntaiZeiTotalTitle.GetByteLength();
            intlen += EntaiZeiTotal.GetByteLength();

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
            int intLen = this.GetByteLength();
            byte[] btData = new byte[intLen];
            int pos = 0;

            if (intLen > 0)
            {
                Array.Copy(this.NaccsCommon.GetByteData(intLen), 0, btData, pos, this.NaccsCommon.GetByteLength());
                pos += this.NaccsCommon.GetByteLength();

                Array.Copy(this.GetByteDataBasic(), 0, btData, pos, this.GetByteLengthBasic());
                pos += this.GetByteLengthBasic();

                Array.Copy(this.ZeikanShochoName.GetByteData(), 0, btData, pos, this.ZeikanShochoName.GetByteLength());
                pos += ZeikanShochoName.GetByteLength();
                Array.Copy(this.KyokaShoninDateTitle.GetByteData(), 0, btData, pos, this.KyokaShoninDateTitle.GetByteLength());
                pos += KyokaShoninDateTitle.GetByteLength();
                Array.Copy(this.KyokaShoninDate.GetByteData(), 0, btData, pos, this.KyokaShoninDate.GetByteLength());
                pos += KyokaShoninDate.GetByteLength();
                Array.Copy(this.EntaiZeiTotalTitle.GetByteData(), 0, btData, pos, this.EntaiZeiTotalTitle.GetByteLength());
                pos += EntaiZeiTotalTitle.GetByteLength();
                Array.Copy(this.EntaiZeiTotal.GetByteData(), 0, btData, pos, this.EntaiZeiTotal.GetByteLength());
                pos += EntaiZeiTotal.GetByteLength();

                for (int i = 0; i < Shohin.Count();i++ )
                {
                    Array.Copy(this.Shohin[i].GetByteData(), 0, btData, pos, this.Shohin[i].GetByteLength());
                    pos += Shohin[i].GetByteLength();
                }

            }
            return btData;
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
                    this.NaccsCommon.SetByteData(btData.Skip(pos).Take(this.NaccsCommon.GetByteLength()).ToArray());
                    pos += this.NaccsCommon.GetByteLength();

                    this.ToZeikan.SetData(EUC.GetString(btData, pos, this.ToZeikan.GetByteLength()));
                    pos += ToZeikan.GetByteLength();

                    this.BumonCode.SetData(EUC.GetString(btData, pos, this.BumonCode.GetByteLength()));
                    pos += BumonCode.GetByteLength();

                    this.ShinkokuDate.SetData(EUC.GetString(btData, pos, this.ShinkokuDate.GetByteLength()));
                    pos += ShinkokuDate.GetByteLength();

                    this.KuraokibaCode.SetData(EUC.GetString(btData, pos, this.KuraokibaCode.GetByteLength()));
                    pos += KuraokibaCode.GetByteLength();

                    this.YunyuShinkokuNo.SetData(EUC.GetString(btData, pos, this.YunyuShinkokuNo.GetByteLength()));
                    pos += YunyuShinkokuNo.GetByteLength();

                    this.YunyushaName.SetData(EUC.GetString(btData, pos, this.YunyushaName.GetByteLength()));
                    pos += YunyushaName.GetByteLength();

                    this.KoukuName.SetData(EUC.GetString(btData, pos, this.KoukuName.GetByteLength()));
                    pos += KoukuName.GetByteLength();

                    this.YubinBango.SetData(EUC.GetString(btData, pos, this.YubinBango.GetByteLength()));
                    pos += YubinBango.GetByteLength();

                    this.JushoKen.SetData(EUC.GetString(btData, pos, this.JushoKen.GetByteLength()));
                    pos += JushoKen.GetByteLength();

                    this.JushoShi.SetData(EUC.GetString(btData, pos, this.JushoShi.GetByteLength()));
                    pos += JushoShi.GetByteLength();

                    this.TojoBinName.SetData(EUC.GetString(btData, pos, this.TojoBinName.GetByteLength()));
                    pos += TojoBinName.GetByteLength();

                    this.JushoBanchi.SetData(EUC.GetString(btData, pos, this.JushoBanchi.GetByteLength()));
                    pos += JushoBanchi.GetByteLength();

                    this.JushoBill.SetData(EUC.GetString(btData, pos, this.JushoBill.GetByteLength()));
                    pos += JushoBill.GetByteLength();

                    this.DairiCode.SetData(EUC.GetString(btData, pos, this.DairiCode.GetByteLength()));
                    pos += DairiCode.GetByteLength();

                    this.DairiName.SetData(EUC.GetString(btData, pos, this.DairiName.GetByteLength()));
                    pos += DairiName.GetByteLength();

                    this.TukanCode.SetData(EUC.GetString(btData, pos, this.TukanCode.GetByteLength()));
                    pos += TukanCode.GetByteLength();

                    this.KounyuKakakuTotal.SetData(EUC.GetString(btData, pos, this.KounyuKakakuTotal.GetByteLength()));
                    pos += KounyuKakakuTotal.GetByteLength();

                    this.KanzeiMenjoGaku.SetData(EUC.GetString(btData, pos, this.KanzeiMenjoGaku.GetByteLength()));
                    pos += KanzeiMenjoGaku.GetByteLength();

                    for (int i = 0; i < ZEIKAMOKU_MAX; i++)
                    {
                        this.ZeikamokuList[i].SetByteData(btData.Skip(pos).Take(this.ZeikamokuList[i].GetByteLength()).ToArray());
                        pos += ZeikamokuList[i].GetByteLength();
                    }

                    this.NouzeiTotal.SetData(EUC.GetString(btData, pos, this.NouzeiTotal.GetByteLength()));
                    pos += NouzeiTotal.GetByteLength();

                    this.TanpoGaku.SetData(EUC.GetString(btData, pos, this.TanpoGaku.GetByteLength()));
                    pos += TanpoGaku.GetByteLength();

                    this.TodofukenCode.SetData(EUC.GetString(btData, pos, this.TodofukenCode.GetByteLength()));
                    pos += TodofukenCode.GetByteLength();

                    this.KozaCode.SetData(EUC.GetString(btData, pos, this.KozaCode.GetByteLength()));
                    pos += KozaCode.GetByteLength();

                    this.NoufuHouhouCode.SetData(EUC.GetString(btData, pos, this.NoufuHouhouCode.GetByteLength()));
                    pos += NoufuHouhouCode.GetByteLength();

                    this.BPJiyuCode.SetData(EUC.GetString(btData, pos, this.BPJiyuCode.GetByteLength()));
                    pos += BPJiyuCode.GetByteLength();

                    this.KouseiMaiSu.SetData(EUC.GetString(btData, pos, this.KouseiMaiSu.GetByteLength()));
                    pos += KouseiMaiSu.GetByteLength();

                    this.ShinseiRanNum.SetData(EUC.GetString(btData, pos, this.ShinseiRanNum.GetByteLength()));
                    pos += ShinseiRanNum.GetByteLength();

                    if (this.ShinseiRanNum.GetIntData > 1)
                    {
                        for (int i = 1; i < this.ShinseiRanNum.GetIntData; i++)
                        {
                            ShohinRecvModel sm = new ShohinRecvModel();
                            this.Shohin.Add(sm);
                        }
                    }

                    this.KijiZeikan.SetData(EUC.GetString(btData, pos, this.KijiZeikan.GetByteLength()));
                    pos += KijiZeikan.GetByteLength();

                    this.KijiHanbai.SetData(EUC.GetString(btData, pos, this.KijiHanbai.GetByteLength()));
                    pos += KijiHanbai.GetByteLength();

                    this.KijiOther.SetData(EUC.GetString(btData, pos, this.KijiOther.GetByteLength()));
                    pos += KijiOther.GetByteLength();

                    this.SeiriyouNo.SetData(EUC.GetString(btData, pos, this.SeiriyouNo.GetByteLength()));
                    pos += SeiriyouNo.GetByteLength();

                    this.UserSeiriNo.SetData(EUC.GetString(btData, pos, this.UserSeiriNo.GetByteLength()));
                    pos += UserSeiriNo.GetByteLength();


                    this.ZeikanShochoName.SetData(EUC.GetString(btData, pos, this.ZeikanShochoName.GetByteLength()));
                    pos += this.ZeikanShochoName.GetByteLength();

                    this.KyokaShoninDateTitle.SetData(EUC.GetString(btData, pos, this.KyokaShoninDateTitle.GetByteLength()));
                    pos += this.KyokaShoninDateTitle.GetByteLength();

                    this.KyokaShoninDate.SetData(EUC.GetString(btData, pos, this.KyokaShoninDate.GetByteLength()));
                    pos += this.KyokaShoninDate.GetByteLength();

                    this.EntaiZeiTotalTitle.SetData(EUC.GetString(btData, pos, this.EntaiZeiTotalTitle.GetByteLength()));
                    pos += this.EntaiZeiTotalTitle.GetByteLength();

                    this.EntaiZeiTotal.SetData(EUC.GetString(btData, pos, this.EntaiZeiTotal.GetByteLength()));
                    pos += this.EntaiZeiTotal.GetByteLength();


                    for (int i = 0; i < Shohin.Count(); i++)
                    {
                        this.Shohin[i].SetByteData(btData.Skip(pos).Take(this.Shohin[i].GetByteLength()).ToArray());
                        pos += Shohin[i].GetByteLength();
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
        ///// <summary>
        ///// ファイル用のデータを取得
        ///// </summary>
        ///// <returns></returns>
        //public override string GetFileData()
        //{
        //    try
        //    {
        //        var btdata = this.GetByteData();
        //        return System.Text.Encoding.GetEncoding(51932).GetString(btdata);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }

    #endregion "輸入申告控　SAD403 or 404"

    #region "納付書情報　SAF001"

    /// <summary>
    /// 納付書情報　SAF001 & SRF001 & SRF710
    /// </summary>
    public class ProcessSaf001Model : NaccsRecvModel
    {
        public override string GetFileName()
        {
            if (string.IsNullOrEmpty(YunyuShinkokuNo.Data))
            {
                return string.Format("{0}_{1}_{2}", LOGSTR, this.GetReportType.ToString(), DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            }
            else
            {
                //return string.Format("{0}_{1}_{2}", GetYunyuShinkokuNo, this.GetReportType.ToString(), DateTime.Now.ToString("yyyyMMddhhmmssfff"));
                return string.Format("{0}_{1}_{2}_{3}", NaccsCommon.USER_CD.Data, this.GetReportType.ToString(), GetYunyuShinkokuNo, DateTime.Now.ToString("yyyyMMddhhmmss"));
            }
        }
        ///// <summary>
        ///// 入力共通項目398
        ///// </summary>
        //public OutputCommonModel OutputCommon = new OutputCommonModel();
        /// <summary>
        /// 所属年度1
        /// </summary>
        public NacCollumnAN SHOZOKU_NENDO_1 = new NacCollumnAN(2);
        /// <summary>
        /// 統計用税関符号0
        /// </summary>
        public NacCollumnAN ZEIKAN_HUGO_0 = new NacCollumnAN(4);
        /// <summary>
        /// 取扱庁名コード1
        /// </summary>
        public NacCollumnAN CHO_NAME_CD_1 = new NacCollumnAN(8);
        /// <summary>
        /// 取扱庁名／税関1
        /// </summary>
        public NacCollumnJ CHO_NAME_ZEIKAN_1 = new NacCollumnJ(8);
        /// <summary>
        /// 取扱庁名／官署1
        /// </summary>
        public NacCollumnJ CHO_NAME_KANSHO_1 = new NacCollumnJ(12);
        /// <summary>
        /// 整理番号
        /// </summary>
        public NacCollumnAN SEIRI_NO = new NacCollumnAN(2);
        ///// <summary>
        ///// 輸入申告等番号0
        ///// </summary>
        //public NacCollumnAN YUNYU_SHINKOKU_NO_0 = new NacCollumnAN(11);
        /// <summary>
        /// 税関郵便番号
        /// </summary>
        public NacCollumnAN ZEIKAN_YUBIN_NO = new NacCollumnAN(8);
        /// <summary>
        /// 税関住所
        /// </summary>
        public NacCollumnJ ZEIKAN_ADDRESS = new NacCollumnJ(54);
        /// <summary>
        /// 宛先税関名
        /// </summary>
        public NacCollumnJ TO_ZEIKAN_NAME = new NacCollumnJ(8);
        /// <summary>
        /// 宛先官署名
        /// </summary>
        public NacCollumnJ TO_KANSHO_NAME = new NacCollumnJ(12);
        /// <summary>
        /// 受入科目名1
        /// </summary>
        public NacCollumnJ KAMOKU_NAME_1 = new NacCollumnJ(22);
        /// <summary>
        /// 輸入申告等番号1
        /// </summary>
        public NacCollumnAN YUNYU_SHINKOKU_NO_1 = new NacCollumnAN(11);
        /// <summary>
        /// 受入科目コード
        /// </summary>
        public NacCollumnAN KAMOKU_CD = new NacCollumnAN(1);
        /// <summary>
        /// 統計用税関符号1
        /// </summary>
        public NacCollumnAN ZEIKAN_HUGO_1 = new NacCollumnAN(4);
        /// <summary>
        /// 代理人1
        /// </summary>
        public NacCollumnAN DAIRININ_1 = new NacCollumnAN(50);
        /// <summary>
        /// 納税義務者住所
        /// </summary>
        public NacCollumnAN NOUZEI_ADDRESS = new NacCollumnAN(158);
        /// <summary>
        /// 納税義務者名1
        /// </summary>
        public NacCollumnAN NOUZEI_NAME_1 = new NacCollumnAN(70);
        /// <summary>
        /// 本税額1
        /// </summary>
        public NacCollumnN TAX_1 = new NacCollumnN(11);
        /// <summary>
        /// 合計額1
        /// </summary>
        public NacCollumnN TAX_TOTAL_1 = new NacCollumnN(11);
        /// <summary>
        /// 納付書用受入科目コード
        /// </summary>
        public NacCollumnAN REPORT_KAMOKU_CD = new NacCollumnAN(2);
        /// <summary>
        /// 当初申告表示1
        /// </summary>
        public NacCollumnFLG FIRST_DECLARATION_FLG_1 = new NacCollumnFLG(1);
        /// <summary>
        /// 納付通知表示1
        /// </summary>
        public NacCollumnFLG NOHU_INFO_FLG_1 = new NacCollumnFLG(1);
        /// <summary>
        /// 修正申告表示1
        /// </summary>
        public NacCollumnFLG CORRECT_DECLARATION_FLG_1 = new NacCollumnFLG(1);
        /// <summary>
        /// 更正通知表示1
        /// </summary>
        public NacCollumnFLG FORM_INFO_FLG_1 = new NacCollumnFLG(1);
        /// <summary>
        /// 賦課決定通知表示1
        /// </summary>
        public NacCollumnFLG LEVY_TAX_INFO_FLG_1 = new NacCollumnFLG(1);
        /// <summary>
        /// 出力年月日1
        /// </summary>
        public NacCollumnDate OUTPUT_DATE_1 = new NacCollumnDate(8);
        /// <summary>
        /// 延長後の納期限1
        /// </summary>
        public NacCollumnDate EXTEND_NOKI_DATE_1 = new NacCollumnDate(8);
        /// <summary>
        /// 納期限延長コード1
        /// </summary>
        public NacCollumnAN EXTEND_NOKI_CD_1 = new NacCollumnAN(1);
        /// <summary>
        /// 所属年度2
        /// </summary>
        public NacCollumnAN SHOZOKU_NENDO_2 = new NacCollumnAN(2);
        /// <summary>
        /// 統計用税関符号2
        /// </summary>
        public NacCollumnAN ZEIKAN_HUGO_2 = new NacCollumnAN(4);
        /// <summary>
        /// 取扱庁名コード2
        /// </summary>
        public NacCollumnAN CHO_NAME_CD_2 = new NacCollumnAN(8);
        /// <summary>
        /// 輸入申告等番号2
        /// </summary>
        public NacCollumnAN YUNYU_SHINKOKU_NO_2 = new NacCollumnAN(11);
        /// <summary>
        /// 取扱庁名／税関2
        /// </summary>
        public NacCollumnJ CHO_NAME_ZEIKAN_2 = new NacCollumnJ(8);
        /// <summary>
        /// 取扱庁名／官署2
        /// </summary>
        public NacCollumnJ CHO_NAME_KANSHO_2 = new NacCollumnJ(12);
        /// <summary>
        /// 受入科目名2
        /// </summary>
        public NacCollumnJ KAMOKU_NAME_2 = new NacCollumnJ(22);
        /// <summary>
        /// 出力年月日2
        /// </summary>
        public NacCollumnDate OUTPUT_DATE_2 = new NacCollumnDate(8);
        /// <summary>
        /// 延長後の納期限2
        /// </summary>
        public NacCollumnDate EXTEND_NOKI_DATE_2 = new NacCollumnDate(8);
        /// <summary>
        /// 納期限延長コード2
        /// </summary>
        public NacCollumnAN EXTEND_NOKI_CD_2 = new NacCollumnAN(1);
        /// <summary>
        /// 代理人2
        /// </summary>
        public NacCollumnAN DAIRININ_2 = new NacCollumnAN(50);
        /// <summary>
        /// 納税義務者住所2_1
        /// </summary>
        public NacCollumnAN NOZEI_ADDRESS_2_1 = new NacCollumnAN(15);
        /// <summary>
        /// 納税義務者住所2_2
        /// </summary>
        public NacCollumnAN NOZEI_ADDRESS_2_2 = new NacCollumnAN(35);
        /// <summary>
        /// 納税義務者住所2_3
        /// </summary>
        public NacCollumnAN NOZEI_ADDRESS_2_3 = new NacCollumnAN(35);
        /// <summary>
        /// 納税義務者住所2_4
        /// </summary>
        public NacCollumnAN NOZEI_ADDRESS_2_4 = new NacCollumnAN(35);
        /// <summary>
        /// 納税義務者名2
        /// </summary>
        public NacCollumnAN NOUZEI_NAME_2 = new NacCollumnAN(70);
        /// <summary>
        /// 本税額2
        /// </summary>
        public NacCollumnN TAX_2 = new NacCollumnN(70);
        /// <summary>
        /// 合計額2
        /// </summary>
        public NacCollumnN TAX_TOTAL_2 = new NacCollumnN(11);
        /// <summary>
        /// 当初申告表示2
        /// </summary>
        public NacCollumnFLG FIRST_DECLARATION_FLG_2 = new NacCollumnFLG(1);
        /// <summary>
        /// 納付通知表示2
        /// </summary>
        public NacCollumnFLG NOHU_INFO_FLG_2 = new NacCollumnFLG(1);
        /// <summary>
        /// 修正申告表示2
        /// </summary>
        public NacCollumnFLG CORRECT_DECLARATION_FLG_2 = new NacCollumnFLG(1);
        /// <summary>
        /// 更正通知表示2
        /// </summary>
        public NacCollumnFLG FORM_INFO_FLG_2 = new NacCollumnFLG(1);
        /// <summary>
        /// 賦課決定通知表示2
        /// </summary>
        public NacCollumnFLG LEVY_TAX_INFO_FLG_2 = new NacCollumnFLG(1);
        /// <summary>
        /// 電算機読取項目
        /// </summary>
        public NacCollumnAN DENSAN_READER = new NacCollumnAN(31);
        /// <summary>
        /// 所属年度3
        /// </summary>
        public NacCollumnAN SHOZOKU_NENDO_3 = new NacCollumnAN(2);
        /// <summary>
        /// 統計用税関符号3
        /// </summary>
        public NacCollumnAN ZEIKAN_HUGO_3 = new NacCollumnAN(4);
        /// <summary>
        /// 取扱庁名コード3
        /// </summary>
        public NacCollumnAN CHO_NAME_CD_3 = new NacCollumnAN(8);
        /// <summary>
        /// 輸入申告等番号3
        /// </summary>
        public NacCollumnAN YUNYU_SHINKOKU_NO_3 = new NacCollumnAN(11);
        /// <summary>
        /// 取扱庁名／税関3
        /// </summary>
        public NacCollumnJ CHO_NAME_ZEIKAN_3 = new NacCollumnJ(8);
        /// <summary>
        /// 取扱庁名／官署3
        /// </summary>
        public NacCollumnJ CHO_NAME_KANSHO_3 = new NacCollumnJ(12);
        /// <summary>
        /// 受入科目名3
        /// </summary>
        public NacCollumnJ KAMOKU_NAME_3 = new NacCollumnJ(22);
        /// <summary>
        /// 出力年月日3
        /// </summary>
        public NacCollumnDate OUTPUT_DATE_3 = new NacCollumnDate(8);
        /// <summary>
        /// 延長後の納期限3
        /// </summary>
        public NacCollumnDate EXTEND_NOKI_DATE_3 = new NacCollumnDate(8);
        /// <summary>
        /// 納期限延長コード3
        /// </summary>
        public NacCollumnAN EXTEND_NOKI_CD_3 = new NacCollumnAN(1);
        /// <summary>
        /// 代理人3
        /// </summary>
        public NacCollumnAN DAIRININ_3 = new NacCollumnAN(50);
        /// <summary>
        /// 納税義務者住所3_1
        /// </summary>
        public NacCollumnAN NOZEI_ADDRESS_3_1 = new NacCollumnAN(15);
        /// <summary>
        /// 納税義務者住所3_2
        /// </summary>
        public NacCollumnAN NOZEI_ADDRESS_3_2 = new NacCollumnAN(35);
        /// <summary>
        /// 納税義務者住所3_3
        /// </summary>
        public NacCollumnAN NOZEI_ADDRESS_3_3 = new NacCollumnAN(35);
        /// <summary>
        /// 納税義務者住所3_4
        /// </summary>
        public NacCollumnAN NOZEI_ADDRESS_3_4 = new NacCollumnAN(70);
        /// <summary>
        /// 納税義務者名3
        /// </summary>
        public NacCollumnAN NOUZEI_NAME_3 = new NacCollumnAN(70);
        /// <summary>
        /// 本税額3
        /// </summary>
        public NacCollumnN TAX_3 = new NacCollumnN(11);
        /// <summary>
        /// 合計額3
        /// </summary>
        public NacCollumnN TAX_TOTAL_3 = new NacCollumnN(11);
        /// <summary>
        /// 当初申告表示3
        /// </summary>
        public NacCollumnFLG FIRST_DECLARATION_FLG_3 = new NacCollumnFLG(1);
        /// <summary>
        /// 納付通知表示3
        /// </summary>
        public NacCollumnFLG NOHU_INFO_FLG_3 = new NacCollumnFLG(1);
        /// <summary>
        /// 修正申告表示3
        /// </summary>
        public NacCollumnFLG CORRECT_DECLARATION_FLG_3 = new NacCollumnFLG(1);
        /// <summary>
        /// 更正通知表示3
        /// </summary>
        public NacCollumnFLG FORM_INFO_FLG_3 = new NacCollumnFLG(1);
        /// <summary>
        /// 賦課決定通知表示3
        /// </summary>
        public NacCollumnFLG LEVY_TAX_INFO_FLG_3 = new NacCollumnFLG(1);
        /// <summary>
        /// バーコード１
        /// </summary>
        public NacCollumnN BARCODE_1 = new NacCollumnN(16);
        /// <summary>
        /// バーコード２
        /// </summary>
        public NacCollumnN BARCODE_2 = new NacCollumnN(15);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProcessSaf001Model()
        {
            //OutputCommon = new OutputCommonModel();
            NaccsCommon = new OutputCommonModel();
        }
        /// <summary>
        /// データバイト長取得
        /// </summary>
        /// <returns></returns>
        public int GetByteLength()
        {
            int intlen = 0;

            //intlen += OutputCommon.GetByteLength();
            intlen += NaccsCommon.GetByteLength();
            intlen += SHOZOKU_NENDO_1.GetByteLength();
            intlen += ZEIKAN_HUGO_0.GetByteLength();
            intlen += CHO_NAME_CD_1.GetByteLength();
            intlen += CHO_NAME_ZEIKAN_1.GetByteLength();
            intlen += CHO_NAME_KANSHO_1.GetByteLength();
            intlen += SEIRI_NO.GetByteLength();
            //intlen += YUNYU_SHINKOKU_NO_0.GetByteLength();
            intlen += YunyuShinkokuNo.GetByteLength();
            intlen += ZEIKAN_YUBIN_NO.GetByteLength();
            intlen += ZEIKAN_ADDRESS.GetByteLength();
            intlen += TO_ZEIKAN_NAME.GetByteLength();
            intlen += TO_KANSHO_NAME.GetByteLength();
            intlen += KAMOKU_NAME_1.GetByteLength();
            intlen += YUNYU_SHINKOKU_NO_1.GetByteLength();
            intlen += KAMOKU_CD.GetByteLength();
            intlen += ZEIKAN_HUGO_1.GetByteLength();
            intlen += DAIRININ_1.GetByteLength();
            intlen += NOUZEI_ADDRESS.GetByteLength();
            intlen += NOUZEI_NAME_1.GetByteLength();
            intlen += TAX_1.GetByteLength();
            intlen += TAX_TOTAL_1.GetByteLength();
            intlen += REPORT_KAMOKU_CD.GetByteLength();
            intlen += FIRST_DECLARATION_FLG_1.GetByteLength();
            intlen += NOHU_INFO_FLG_1.GetByteLength();
            intlen += CORRECT_DECLARATION_FLG_1.GetByteLength();
            intlen += FORM_INFO_FLG_1.GetByteLength();
            intlen += LEVY_TAX_INFO_FLG_1.GetByteLength();
            intlen += OUTPUT_DATE_1.GetByteLength();
            intlen += EXTEND_NOKI_DATE_1.GetByteLength();
            intlen += EXTEND_NOKI_CD_1.GetByteLength();
            intlen += SHOZOKU_NENDO_2.GetByteLength();
            intlen += ZEIKAN_HUGO_2.GetByteLength();
            intlen += CHO_NAME_CD_2.GetByteLength();
            intlen += YUNYU_SHINKOKU_NO_2.GetByteLength();
            intlen += CHO_NAME_ZEIKAN_2.GetByteLength();
            intlen += CHO_NAME_KANSHO_2.GetByteLength();
            intlen += KAMOKU_NAME_2.GetByteLength();
            intlen += OUTPUT_DATE_2.GetByteLength();
            intlen += EXTEND_NOKI_DATE_2.GetByteLength();
            intlen += EXTEND_NOKI_CD_2.GetByteLength();
            intlen += DAIRININ_2.GetByteLength();
            intlen += NOZEI_ADDRESS_2_1.GetByteLength();
            intlen += NOZEI_ADDRESS_2_2.GetByteLength();
            intlen += NOZEI_ADDRESS_2_3.GetByteLength();
            intlen += NOZEI_ADDRESS_2_4.GetByteLength();
            intlen += NOUZEI_NAME_2.GetByteLength();
            intlen += TAX_2.GetByteLength();
            intlen += TAX_TOTAL_2.GetByteLength();
            intlen += FIRST_DECLARATION_FLG_2.GetByteLength();
            intlen += NOHU_INFO_FLG_2.GetByteLength();
            intlen += CORRECT_DECLARATION_FLG_2.GetByteLength();
            intlen += FORM_INFO_FLG_2.GetByteLength();
            intlen += LEVY_TAX_INFO_FLG_2.GetByteLength();
            intlen += DENSAN_READER.GetByteLength();
            intlen += SHOZOKU_NENDO_3.GetByteLength();
            intlen += ZEIKAN_HUGO_3.GetByteLength();
            intlen += CHO_NAME_CD_3.GetByteLength();
            intlen += YUNYU_SHINKOKU_NO_3.GetByteLength();
            intlen += CHO_NAME_ZEIKAN_3.GetByteLength();
            intlen += CHO_NAME_KANSHO_3.GetByteLength();
            intlen += KAMOKU_NAME_3.GetByteLength();
            intlen += OUTPUT_DATE_3.GetByteLength();
            intlen += EXTEND_NOKI_DATE_3.GetByteLength();
            intlen += EXTEND_NOKI_CD_3.GetByteLength();
            intlen += DAIRININ_3.GetByteLength();
            intlen += NOZEI_ADDRESS_3_1.GetByteLength();
            intlen += NOZEI_ADDRESS_3_2.GetByteLength();
            intlen += NOZEI_ADDRESS_3_3.GetByteLength();
            intlen += NOZEI_ADDRESS_3_4.GetByteLength();
            intlen += NOUZEI_NAME_3.GetByteLength();
            intlen += TAX_3.GetByteLength();
            intlen += TAX_TOTAL_3.GetByteLength();
            intlen += FIRST_DECLARATION_FLG_3.GetByteLength();
            intlen += NOHU_INFO_FLG_3.GetByteLength();
            intlen += CORRECT_DECLARATION_FLG_3.GetByteLength();
            intlen += FORM_INFO_FLG_3.GetByteLength();
            intlen += LEVY_TAX_INFO_FLG_3.GetByteLength();
            intlen += BARCODE_1.GetByteLength();
            intlen += BARCODE_2.GetByteLength();

            return intlen;
        }

        /// <summary>
        /// 添付用データの作成
        /// </summary>
        /// <returns></returns>
        public override byte[] GetByteData()
        {
            int intLen = this.GetByteLength();
            byte[] btData = new byte[intLen];
            int pos = 0;

            try
            {
                if (intLen > 0)
                {
                    //Array.Copy(this.OutputCommon.GetByteData(intLen), 0, btData, pos, this.OutputCommon.GetByteLength());
                    //pos += OutputCommon.GetByteLength();
                    Array.Copy(this.NaccsCommon.GetByteData(intLen), 0, btData, pos, this.NaccsCommon.GetByteLength());
                    pos += NaccsCommon.GetByteLength();

                    Array.Copy(this.SHOZOKU_NENDO_1.GetByteData(), 0, btData, pos, this.SHOZOKU_NENDO_1.GetByteLength());
                    pos += SHOZOKU_NENDO_1.GetByteLength();

                    Array.Copy(this.ZEIKAN_HUGO_0.GetByteData(), 0, btData, pos, this.ZEIKAN_HUGO_0.GetByteLength());
                    pos += ZEIKAN_HUGO_0.GetByteLength();

                    Array.Copy(this.CHO_NAME_CD_1.GetByteData(), 0, btData, pos, this.CHO_NAME_CD_1.GetByteLength());
                    pos += CHO_NAME_CD_1.GetByteLength();

                    Array.Copy(this.CHO_NAME_ZEIKAN_1.GetByteData(), 0, btData, pos, this.CHO_NAME_ZEIKAN_1.GetByteLength());
                    pos += CHO_NAME_ZEIKAN_1.GetByteLength();

                    Array.Copy(this.CHO_NAME_KANSHO_1.GetByteData(), 0, btData, pos, this.CHO_NAME_KANSHO_1.GetByteLength());
                    pos += CHO_NAME_KANSHO_1.GetByteLength();

                    Array.Copy(this.SEIRI_NO.GetByteData(), 0, btData, pos, this.SEIRI_NO.GetByteLength());
                    pos += SEIRI_NO.GetByteLength();

                    //Array.Copy(this.YUNYU_SHINKOKU_NO_0.GetByteData(), 0, btData, pos, this.YUNYU_SHINKOKU_NO_0.GetByteLength());
                    //pos += YUNYU_SHINKOKU_NO_0.GetByteLength();
                    Array.Copy(this.YunyuShinkokuNo.GetByteData(), 0, btData, pos, this.YunyuShinkokuNo.GetByteLength());
                    pos += YunyuShinkokuNo.GetByteLength();

                    Array.Copy(this.ZEIKAN_YUBIN_NO.GetByteData(), 0, btData, pos, this.ZEIKAN_YUBIN_NO.GetByteLength());
                    pos += ZEIKAN_YUBIN_NO.GetByteLength();

                    Array.Copy(this.ZEIKAN_ADDRESS.GetByteData(), 0, btData, pos, this.ZEIKAN_ADDRESS.GetByteLength());
                    pos += ZEIKAN_ADDRESS.GetByteLength();

                    Array.Copy(this.TO_ZEIKAN_NAME.GetByteData(), 0, btData, pos, this.TO_ZEIKAN_NAME.GetByteLength());
                    pos += TO_ZEIKAN_NAME.GetByteLength();

                    Array.Copy(this.TO_KANSHO_NAME.GetByteData(), 0, btData, pos, this.TO_KANSHO_NAME.GetByteLength());
                    pos += TO_KANSHO_NAME.GetByteLength();

                    Array.Copy(this.KAMOKU_NAME_1.GetByteData(), 0, btData, pos, this.KAMOKU_NAME_1.GetByteLength());
                    pos += KAMOKU_NAME_1.GetByteLength();

                    Array.Copy(this.YUNYU_SHINKOKU_NO_1.GetByteData(), 0, btData, pos, this.YUNYU_SHINKOKU_NO_1.GetByteLength());
                    pos += YUNYU_SHINKOKU_NO_1.GetByteLength();

                    Array.Copy(this.KAMOKU_CD.GetByteData(), 0, btData, pos, this.KAMOKU_CD.GetByteLength());
                    pos += KAMOKU_CD.GetByteLength();

                    Array.Copy(this.ZEIKAN_HUGO_1.GetByteData(), 0, btData, pos, this.ZEIKAN_HUGO_1.GetByteLength());
                    pos += ZEIKAN_HUGO_1.GetByteLength();

                    Array.Copy(this.DAIRININ_1.GetByteData(), 0, btData, pos, this.DAIRININ_1.GetByteLength());
                    pos += DAIRININ_1.GetByteLength();

                    Array.Copy(this.NOUZEI_ADDRESS.GetByteData(), 0, btData, pos, this.NOUZEI_ADDRESS.GetByteLength());
                    pos += NOUZEI_ADDRESS.GetByteLength();

                    Array.Copy(this.NOUZEI_NAME_1.GetByteData(), 0, btData, pos, this.NOUZEI_NAME_1.GetByteLength());
                    pos += NOUZEI_NAME_1.GetByteLength();

                    Array.Copy(this.TAX_1.GetByteData(), 0, btData, pos, this.TAX_1.GetByteLength());
                    pos += TAX_1.GetByteLength();

                    Array.Copy(this.TAX_TOTAL_1.GetByteData(), 0, btData, pos, this.TAX_TOTAL_1.GetByteLength());
                    pos += TAX_TOTAL_1.GetByteLength();

                    Array.Copy(this.REPORT_KAMOKU_CD.GetByteData(), 0, btData, pos, this.REPORT_KAMOKU_CD.GetByteLength());
                    pos += REPORT_KAMOKU_CD.GetByteLength();

                    Array.Copy(this.FIRST_DECLARATION_FLG_1.GetByteData(), 0, btData, pos, this.FIRST_DECLARATION_FLG_1.GetByteLength());
                    pos += FIRST_DECLARATION_FLG_1.GetByteLength();

                    Array.Copy(this.NOHU_INFO_FLG_1.GetByteData(), 0, btData, pos, this.NOHU_INFO_FLG_1.GetByteLength());
                    pos += NOHU_INFO_FLG_1.GetByteLength();

                    Array.Copy(this.CORRECT_DECLARATION_FLG_1.GetByteData(), 0, btData, pos, this.CORRECT_DECLARATION_FLG_1.GetByteLength());
                    pos += CORRECT_DECLARATION_FLG_1.GetByteLength();

                    Array.Copy(this.FORM_INFO_FLG_1.GetByteData(), 0, btData, pos, this.FORM_INFO_FLG_1.GetByteLength());
                    pos += FORM_INFO_FLG_1.GetByteLength();

                    Array.Copy(this.LEVY_TAX_INFO_FLG_1.GetByteData(), 0, btData, pos, this.LEVY_TAX_INFO_FLG_1.GetByteLength());
                    pos += LEVY_TAX_INFO_FLG_1.GetByteLength();

                    Array.Copy(this.OUTPUT_DATE_1.GetByteData(), 0, btData, pos, this.OUTPUT_DATE_1.GetByteLength());
                    pos += OUTPUT_DATE_1.GetByteLength();

                    Array.Copy(this.EXTEND_NOKI_DATE_1.GetByteData(), 0, btData, pos, this.EXTEND_NOKI_DATE_1.GetByteLength());
                    pos += EXTEND_NOKI_DATE_1.GetByteLength();

                    Array.Copy(this.EXTEND_NOKI_CD_1.GetByteData(), 0, btData, pos, this.EXTEND_NOKI_CD_1.GetByteLength());
                    pos += EXTEND_NOKI_CD_1.GetByteLength();

                    Array.Copy(this.SHOZOKU_NENDO_2.GetByteData(), 0, btData, pos, this.SHOZOKU_NENDO_2.GetByteLength());
                    pos += SHOZOKU_NENDO_2.GetByteLength();

                    Array.Copy(this.ZEIKAN_HUGO_2.GetByteData(), 0, btData, pos, this.ZEIKAN_HUGO_2.GetByteLength());
                    pos += ZEIKAN_HUGO_2.GetByteLength();

                    Array.Copy(this.CHO_NAME_CD_2.GetByteData(), 0, btData, pos, this.CHO_NAME_CD_2.GetByteLength());
                    pos += CHO_NAME_CD_2.GetByteLength();

                    Array.Copy(this.YUNYU_SHINKOKU_NO_2.GetByteData(), 0, btData, pos, this.YUNYU_SHINKOKU_NO_2.GetByteLength());
                    pos += YUNYU_SHINKOKU_NO_2.GetByteLength();

                    Array.Copy(this.CHO_NAME_ZEIKAN_2.GetByteData(), 0, btData, pos, this.CHO_NAME_ZEIKAN_2.GetByteLength());
                    pos += CHO_NAME_ZEIKAN_2.GetByteLength();

                    Array.Copy(this.CHO_NAME_KANSHO_2.GetByteData(), 0, btData, pos, this.CHO_NAME_KANSHO_2.GetByteLength());
                    pos += CHO_NAME_KANSHO_2.GetByteLength();

                    Array.Copy(this.KAMOKU_NAME_2.GetByteData(), 0, btData, pos, this.KAMOKU_NAME_2.GetByteLength());
                    pos += KAMOKU_NAME_2.GetByteLength();

                    Array.Copy(this.OUTPUT_DATE_2.GetByteData(), 0, btData, pos, this.OUTPUT_DATE_2.GetByteLength());
                    pos += OUTPUT_DATE_2.GetByteLength();

                    Array.Copy(this.EXTEND_NOKI_DATE_2.GetByteData(), 0, btData, pos, this.EXTEND_NOKI_DATE_2.GetByteLength());
                    pos += EXTEND_NOKI_DATE_2.GetByteLength();

                    Array.Copy(this.EXTEND_NOKI_CD_2.GetByteData(), 0, btData, pos, this.EXTEND_NOKI_CD_2.GetByteLength());
                    pos += EXTEND_NOKI_CD_2.GetByteLength();

                    Array.Copy(this.DAIRININ_2.GetByteData(), 0, btData, pos, this.DAIRININ_2.GetByteLength());
                    pos += DAIRININ_2.GetByteLength();

                    Array.Copy(this.NOZEI_ADDRESS_2_1.GetByteData(), 0, btData, pos, this.NOZEI_ADDRESS_2_1.GetByteLength());
                    pos += NOZEI_ADDRESS_2_1.GetByteLength();

                    Array.Copy(this.NOZEI_ADDRESS_2_2.GetByteData(), 0, btData, pos, this.NOZEI_ADDRESS_2_2.GetByteLength());
                    pos += NOZEI_ADDRESS_2_2.GetByteLength();

                    Array.Copy(this.NOZEI_ADDRESS_2_3.GetByteData(), 0, btData, pos, this.NOZEI_ADDRESS_2_3.GetByteLength());
                    pos += NOZEI_ADDRESS_2_3.GetByteLength();

                    Array.Copy(this.NOZEI_ADDRESS_2_4.GetByteData(), 0, btData, pos, this.NOZEI_ADDRESS_2_4.GetByteLength());
                    pos += NOZEI_ADDRESS_2_4.GetByteLength();

                    Array.Copy(this.NOUZEI_NAME_2.GetByteData(), 0, btData, pos, this.NOUZEI_NAME_2.GetByteLength());
                    pos += NOUZEI_NAME_2.GetByteLength();

                    Array.Copy(this.TAX_2.GetByteData(), 0, btData, pos, this.TAX_2.GetByteLength());
                    pos += TAX_2.GetByteLength();

                    Array.Copy(this.TAX_TOTAL_2.GetByteData(), 0, btData, pos, this.TAX_TOTAL_2.GetByteLength());
                    pos += TAX_TOTAL_2.GetByteLength();

                    Array.Copy(this.FIRST_DECLARATION_FLG_2.GetByteData(), 0, btData, pos, this.FIRST_DECLARATION_FLG_2.GetByteLength());
                    pos += FIRST_DECLARATION_FLG_2.GetByteLength();

                    Array.Copy(this.NOHU_INFO_FLG_2.GetByteData(), 0, btData, pos, this.NOHU_INFO_FLG_2.GetByteLength());
                    pos += NOHU_INFO_FLG_2.GetByteLength();

                    Array.Copy(this.CORRECT_DECLARATION_FLG_2.GetByteData(), 0, btData, pos, this.CORRECT_DECLARATION_FLG_2.GetByteLength());
                    pos += CORRECT_DECLARATION_FLG_2.GetByteLength();

                    Array.Copy(this.FORM_INFO_FLG_2.GetByteData(), 0, btData, pos, this.FORM_INFO_FLG_2.GetByteLength());
                    pos += FORM_INFO_FLG_2.GetByteLength();

                    Array.Copy(this.LEVY_TAX_INFO_FLG_2.GetByteData(), 0, btData, pos, this.LEVY_TAX_INFO_FLG_2.GetByteLength());
                    pos += LEVY_TAX_INFO_FLG_2.GetByteLength();

                    Array.Copy(this.DENSAN_READER.GetByteData(), 0, btData, pos, this.DENSAN_READER.GetByteLength());
                    pos += DENSAN_READER.GetByteLength();

                    Array.Copy(this.SHOZOKU_NENDO_3.GetByteData(), 0, btData, pos, this.SHOZOKU_NENDO_3.GetByteLength());
                    pos += SHOZOKU_NENDO_3.GetByteLength();

                    Array.Copy(this.ZEIKAN_HUGO_3.GetByteData(), 0, btData, pos, this.ZEIKAN_HUGO_3.GetByteLength());
                    pos += ZEIKAN_HUGO_3.GetByteLength();

                    Array.Copy(this.CHO_NAME_CD_3.GetByteData(), 0, btData, pos, this.CHO_NAME_CD_3.GetByteLength());
                    pos += CHO_NAME_CD_3.GetByteLength();

                    Array.Copy(this.YUNYU_SHINKOKU_NO_3.GetByteData(), 0, btData, pos, this.YUNYU_SHINKOKU_NO_3.GetByteLength());
                    pos += YUNYU_SHINKOKU_NO_3.GetByteLength();

                    Array.Copy(this.CHO_NAME_ZEIKAN_3.GetByteData(), 0, btData, pos, this.CHO_NAME_ZEIKAN_3.GetByteLength());
                    pos += CHO_NAME_ZEIKAN_3.GetByteLength();

                    Array.Copy(this.CHO_NAME_KANSHO_3.GetByteData(), 0, btData, pos, this.CHO_NAME_KANSHO_3.GetByteLength());
                    pos += CHO_NAME_KANSHO_3.GetByteLength();

                    Array.Copy(this.KAMOKU_NAME_3.GetByteData(), 0, btData, pos, this.KAMOKU_NAME_3.GetByteLength());
                    pos += KAMOKU_NAME_3.GetByteLength();

                    Array.Copy(this.OUTPUT_DATE_3.GetByteData(), 0, btData, pos, this.OUTPUT_DATE_3.GetByteLength());
                    pos += OUTPUT_DATE_3.GetByteLength();

                    Array.Copy(this.EXTEND_NOKI_DATE_3.GetByteData(), 0, btData, pos, this.EXTEND_NOKI_DATE_3.GetByteLength());
                    pos += EXTEND_NOKI_DATE_3.GetByteLength();

                    Array.Copy(this.EXTEND_NOKI_CD_3.GetByteData(), 0, btData, pos, this.EXTEND_NOKI_CD_3.GetByteLength());
                    pos += EXTEND_NOKI_CD_3.GetByteLength();

                    Array.Copy(this.DAIRININ_3.GetByteData(), 0, btData, pos, this.DAIRININ_3.GetByteLength());
                    pos += DAIRININ_3.GetByteLength();

                    Array.Copy(this.NOZEI_ADDRESS_3_1.GetByteData(), 0, btData, pos, this.NOZEI_ADDRESS_3_1.GetByteLength());
                    pos += NOZEI_ADDRESS_3_1.GetByteLength();

                    Array.Copy(this.NOZEI_ADDRESS_3_2.GetByteData(), 0, btData, pos, this.NOZEI_ADDRESS_3_2.GetByteLength());
                    pos += NOZEI_ADDRESS_3_2.GetByteLength();

                    Array.Copy(this.NOZEI_ADDRESS_3_3.GetByteData(), 0, btData, pos, this.NOZEI_ADDRESS_3_3.GetByteLength());
                    pos += NOZEI_ADDRESS_3_3.GetByteLength();

                    Array.Copy(this.NOZEI_ADDRESS_3_4.GetByteData(), 0, btData, pos, this.NOZEI_ADDRESS_3_4.GetByteLength());
                    pos += NOZEI_ADDRESS_3_4.GetByteLength();

                    Array.Copy(this.NOUZEI_NAME_3.GetByteData(), 0, btData, pos, this.NOUZEI_NAME_3.GetByteLength());
                    pos += NOUZEI_NAME_3.GetByteLength();

                    Array.Copy(this.TAX_3.GetByteData(), 0, btData, pos, this.TAX_3.GetByteLength());
                    pos += TAX_3.GetByteLength();

                    Array.Copy(this.TAX_TOTAL_3.GetByteData(), 0, btData, pos, this.TAX_TOTAL_3.GetByteLength());
                    pos += TAX_TOTAL_3.GetByteLength();

                    Array.Copy(this.FIRST_DECLARATION_FLG_3.GetByteData(), 0, btData, pos, this.FIRST_DECLARATION_FLG_3.GetByteLength());
                    pos += FIRST_DECLARATION_FLG_3.GetByteLength();

                    Array.Copy(this.NOHU_INFO_FLG_3.GetByteData(), 0, btData, pos, this.NOHU_INFO_FLG_3.GetByteLength());
                    pos += NOHU_INFO_FLG_3.GetByteLength();

                    Array.Copy(this.CORRECT_DECLARATION_FLG_3.GetByteData(), 0, btData, pos, this.CORRECT_DECLARATION_FLG_3.GetByteLength());
                    pos += CORRECT_DECLARATION_FLG_3.GetByteLength();

                    Array.Copy(this.FORM_INFO_FLG_3.GetByteData(), 0, btData, pos, this.FORM_INFO_FLG_3.GetByteLength());
                    pos += FORM_INFO_FLG_3.GetByteLength();

                    Array.Copy(this.LEVY_TAX_INFO_FLG_3.GetByteData(), 0, btData, pos, this.LEVY_TAX_INFO_FLG_3.GetByteLength());
                    pos += LEVY_TAX_INFO_FLG_3.GetByteLength();

                    Array.Copy(this.BARCODE_1.GetByteData(), 0, btData, pos, this.BARCODE_1.GetByteLength());
                    pos += BARCODE_1.GetByteLength();

                    Array.Copy(this.BARCODE_2.GetByteData(), 0, btData, pos, this.BARCODE_2.GetByteLength());
                    pos += BARCODE_2.GetByteLength();

                }
                return btData;
            }
            catch(Exception e)
            {
                LogOut.ErrorOut(e.ToString().Substring(0, 50), this.GetType().Name, MethodBase.GetCurrentMethod().Name);
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

    #endregion　"納付書情報　SAF001 & SRF001 & SRF710"

    #region "納付番号通知情報　SAF002"

    /// <summary>
    /// 納付番号通知情報　SAF002 & SBF720
    /// </summary>
    public class ProcessSaf002Model : NaccsRecvModel
    {
        public override string GetFileName()
        {
            if (string.IsNullOrEmpty(YunyuShinkokuNo.Data))
            {
                return string.Format("{0}_{1}_{2}", LOGSTR, this.GetReportType.ToString(), DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            }
            else
            {
                //return string.Format("{0}_{1}_{2}", GetYunyuShinkokuNo, this.GetReportType.ToString(), DateTime.Now.ToString("yyyyMMddhhmmssfff"));
                return string.Format("{0}_{1}_{2}_{3}", NaccsCommon.USER_CD.Data, this.GetReportType.ToString(), GetYunyuShinkokuNo, DateTime.Now.ToString("yyyyMMddhhmmss"));
            }
        }
        ///// <summary>
        ///// 入力共通項目398
        ///// </summary>
        //public OutputCommonModel OutputCommon = new OutputCommonModel();
        /// <summary>
        /// データ有無識別
        /// </summary>
        public NacCollumnAN DATA_FLG_TYPE = new NacCollumnAN(1);
        /// <summary>
        /// 帳票識別
        /// </summary>
        public NacCollumnJ REPORT_TYPE = new NacCollumnJ(28);
        /// <summary>
        /// 収納機関コード
        /// </summary>
        public NacCollumnAN SHUNO_KIKAN_CD = new NacCollumnAN(5);
        /// <summary>
        /// 納付番号
        /// </summary>
        public NacCollumnAN NOUHU_NO = new NacCollumnAN(11);
        /// <summary>
        /// 確認番号
        /// </summary>
        public NacCollumnAN CONFIRM_NO = new NacCollumnAN(6);
        /// <summary>
        /// 納税義務者
        /// </summary>
        public NacCollumnAN NOZEI_USER_CD = new NacCollumnAN(17);
        /// <summary>
        /// 納税義務者名
        /// </summary>
        public NacCollumnAN NOZEI_USER_NAME = new NacCollumnAN(70);
        /// <summary>
        /// 納税義務者住所１
        /// </summary>
        public NacCollumnAN ADDRESS1 = new NacCollumnAN(15);
        /// <summary>
        /// 納税義務者住所２
        /// </summary>
        public NacCollumnAN ADDRESS2 = new NacCollumnAN(35);
        /// <summary>
        /// 納税義務者住所３
        /// </summary>
        public NacCollumnAN ADDRESS3 = new NacCollumnAN(35);
        /// <summary>
        /// 納税義務者住所４
        /// </summary>
        public NacCollumnAN ADDRESS4 = new NacCollumnAN(70);
        /// <summary>
        /// 利用者コード
        /// </summary>
        public NacCollumnAN GYOSHA_CD = new NacCollumnAN(5);
        /// <summary>
        /// 宛先税関名
        /// </summary>
        public NacCollumnJ ZEIKAN_NAME = new NacCollumnJ(8);
        /// <summary>
        /// 宛先官署名
        /// </summary>
        public NacCollumnJ KANSHO_NAME = new NacCollumnJ(12);
        /// <summary>
        /// 輸入申告等番号
        /// </summary>
        public NacCollumnAN YUNYU_SHINKOKU_NO = new NacCollumnAN(12);
        /// <summary>
        /// 納期限
        /// </summary>
        public NacCollumnDate NOKI_DATE = new NacCollumnDate(8);
        /// <summary>
        /// 納期限延長コード
        /// </summary>
        public NacCollumnAN EXTEND_NOKI_CD = new NacCollumnAN(1);
        /// <summary>
        /// 所属年度
        /// </summary>
        public NacCollumnN SHOZOKU_YEAR = new NacCollumnN(4);
        /// <summary>
        /// 当初申告表示
        /// </summary>
        public NacCollumnFLG FIRST_DECLARATION_FLG = new NacCollumnFLG(1);
        /// <summary>
        /// 修正申告表示
        /// </summary>
        public NacCollumnFLG CORRECT_DECLARATION_FLG = new NacCollumnFLG(1);
        /// <summary>
        /// 納付通知表示
        /// </summary>
        public NacCollumnFLG NOHU_INFO_FLG = new NacCollumnFLG(1);
        /// <summary>
        /// 構成通知表示
        /// </summary>
        public NacCollumnFLG FORM_INFO_FLG = new NacCollumnFLG(1);
        /// <summary>
        /// 決定通知表示
        /// </summary>
        public NacCollumnFLG DECIDE_INFO_FLG = new NacCollumnFLG(1);
        /// <summary>
        /// 賦課決定通知表示
        /// </summary>
        public NacCollumnFLG LEVY_TAX_INFO_FLG = new NacCollumnFLG(1);
        /// <summary>
        /// 過少申告加算税表示
        /// </summary>
        public NacCollumnFLG UNDER_REPORT_ADD_TAX_FLG = new NacCollumnFLG(1);
        /// <summary>
        /// 重加算税過少申告表示
        /// </summary>
        public NacCollumnFLG UNDER_REPORT_HEAVY_ADD_TAX_FLG = new NacCollumnFLG(1);
        /// <summary>
        /// 無申告加算税表示
        /// </summary>
        public NacCollumnFLG NO_REPORT_ADD_TAX_FLG = new NacCollumnFLG(1);
        /// <summary>
        /// 重加算税無申告表示
        /// </summary>
        public NacCollumnFLG NO_REPORT_HEAVY_ADD_TAX_FLG = new NacCollumnFLG(1);
        /// <summary>
        /// 受入科目コード1
        /// </summary>
        public NacCollumnAN KAMOKU_CD_1 = new NacCollumnAN(22);
        /// <summary>
        /// 受入科目名1
        /// </summary>
        public NacCollumnJ KAMOKU_NAME_1 = new NacCollumnJ(11);
        /// <summary>
        /// 税額1
        /// </summary>
        public NacCollumnN ZEI_1 = new NacCollumnN(11);
        /// <summary>
        /// 受入科目コード2
        /// </summary>
        public NacCollumnAN KAMOKU_CD_2 = new NacCollumnAN(22);
        /// <summary>
        /// 受入科目名2
        /// </summary>
        public NacCollumnJ KAMOKU_NAME_2 = new NacCollumnJ(11);
        /// <summary>
        /// 税額2
        /// </summary>
        public NacCollumnN ZEI_2 = new NacCollumnN(11);
        /// <summary>
        /// 受入科目コード3
        /// </summary>
        public NacCollumnAN KAMOKU_CD_3 = new NacCollumnAN(22);
        /// <summary>
        /// 受入科目名3
        /// </summary>
        public NacCollumnJ KAMOKU_NAME_3 = new NacCollumnJ(11);
        /// <summary>
        /// 税額3
        /// </summary>
        public NacCollumnN ZEI_3 = new NacCollumnN(11);
        /// <summary>
        /// 受入科目コード4
        /// </summary>
        public NacCollumnAN KAMOKU_CD_4 = new NacCollumnAN(22);
        /// <summary>
        /// 受入科目名4
        /// </summary>
        public NacCollumnJ KAMOKU_NAME_4 = new NacCollumnJ(11);
        /// <summary>
        /// 税額4
        /// </summary>
        public NacCollumnN ZEI_4 = new NacCollumnN(11);
        /// <summary>
        /// 受入科目コード5
        /// </summary>
        public NacCollumnAN KAMOKU_CD_5 = new NacCollumnAN(22);
        /// <summary>
        /// 受入科目名5
        /// </summary>
        public NacCollumnJ KAMOKU_NAME_5 = new NacCollumnJ(11);
        /// <summary>
        /// 税額5
        /// </summary>
        public NacCollumnN ZEI_5 = new NacCollumnN(11);
        /// <summary>
        /// 受入科目コード6
        /// </summary>
        public NacCollumnAN KAMOKU_CD_6 = new NacCollumnAN(22);
        /// <summary>
        /// 受入科目名6
        /// </summary>
        public NacCollumnJ KAMOKU_NAME_6 = new NacCollumnJ(11);
        /// <summary>
        /// 税額6
        /// </summary>
        public NacCollumnN ZEI_6 = new NacCollumnN(11);
        /// <summary>
        /// 受入科目コード7
        /// </summary>
        public NacCollumnAN KAMOKU_CD_7 = new NacCollumnAN(22);
        /// <summary>
        /// 受入科目名7
        /// </summary>
        public NacCollumnJ KAMOKU_NAME_7 = new NacCollumnJ(11);
        /// <summary>
        /// 税額7
        /// </summary>
        public NacCollumnN ZEI_7 = new NacCollumnN(11);
        /// <summary>
        /// 税額合計
        /// </summary>
        public NacCollumnN ZEI_TOTAL = new NacCollumnN(11);
        /// <summary>
        /// 一括納付番号1
        /// </summary>
        public NacCollumnAN PACKAGE_NOHU_NO_1 = new NacCollumnAN(11);
        /// <summary>
        /// 一括納付番号2
        /// </summary>
        public NacCollumnAN PACKAGE_NOHU_NO_2 = new NacCollumnAN(11);
        /// <summary>
        /// 一括納付番号3
        /// </summary>
        public NacCollumnAN PACKAGE_NOHU_NO_3 = new NacCollumnAN(11);
        /// <summary>
        /// 一括納付番号4
        /// </summary>
        public NacCollumnAN PACKAGE_NOHU_NO_4 = new NacCollumnAN(11);
        /// <summary>
        /// 一括納付番号5
        /// </summary>
        public NacCollumnAN PACKAGE_NOHU_NO_5 = new NacCollumnAN(11);
        /// <summary>
        /// 一括納付番号6
        /// </summary>
        public NacCollumnAN PACKAGE_NOHU_NO_6 = new NacCollumnAN(11);
        /// <summary>
        /// 一括納付番号7
        /// </summary>
        public NacCollumnAN PACKAGE_NOHU_NO_7 = new NacCollumnAN(11);
        /// <summary>
        /// 一括納付番号8
        /// </summary>
        public NacCollumnAN PACKAGE_NOHU_NO_8 = new NacCollumnAN(11);
        /// <summary>
        /// 一括納付番号9
        /// </summary>
        public NacCollumnAN PACKAGE_NOHU_NO_9 = new NacCollumnAN(11);
        /// <summary>
        /// 一括納付番号10
        /// </summary>
        public NacCollumnAN PACKAGE_NOHU_NO_10 = new NacCollumnAN(11);
        /// <summary>
        /// 通知日
        /// </summary>
        public NacCollumnDate REPORT_DATE = new NacCollumnDate(8);



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProcessSaf002Model()
        {
            //OutputCommon = new OutputCommonModel();
            NaccsCommon = new OutputCommonModel();
        }
        /// <summary>
        /// データバイト長取得
        /// </summary>
        /// <returns></returns>
        public int GetByteLength()
        {
            int intlen = 0;

            //intlen += OutputCommon.GetByteLength();
            intlen += NaccsCommon.GetByteLength();

            intlen += DATA_FLG_TYPE.GetByteLength();
            intlen += REPORT_TYPE.GetByteLength();
            intlen += SHUNO_KIKAN_CD.GetByteLength();
            intlen += NOUHU_NO.GetByteLength();
            intlen += CONFIRM_NO.GetByteLength();
            intlen += NOZEI_USER_CD.GetByteLength();
            intlen += NOZEI_USER_NAME.GetByteLength();
            intlen += ADDRESS1.GetByteLength();
            intlen += ADDRESS2.GetByteLength();
            intlen += ADDRESS3.GetByteLength();
            intlen += ADDRESS4.GetByteLength();
            intlen += GYOSHA_CD.GetByteLength();
            intlen += ZEIKAN_NAME.GetByteLength();
            intlen += KANSHO_NAME.GetByteLength();
            intlen += YUNYU_SHINKOKU_NO.GetByteLength();
            intlen += NOKI_DATE.GetByteLength();
            intlen += EXTEND_NOKI_CD.GetByteLength();
            intlen += SHOZOKU_YEAR.GetByteLength();
            intlen += FIRST_DECLARATION_FLG.GetByteLength();
            intlen += CORRECT_DECLARATION_FLG.GetByteLength();
            intlen += NOHU_INFO_FLG.GetByteLength();
            intlen += FORM_INFO_FLG.GetByteLength();
            intlen += DECIDE_INFO_FLG.GetByteLength();
            intlen += LEVY_TAX_INFO_FLG.GetByteLength();
            intlen += UNDER_REPORT_ADD_TAX_FLG.GetByteLength();
            intlen += UNDER_REPORT_HEAVY_ADD_TAX_FLG.GetByteLength();
            intlen += NO_REPORT_ADD_TAX_FLG.GetByteLength();
            intlen += NO_REPORT_HEAVY_ADD_TAX_FLG.GetByteLength();
            intlen += KAMOKU_CD_1.GetByteLength();
            intlen += KAMOKU_NAME_1.GetByteLength();
            intlen += ZEI_1.GetByteLength();
            intlen += KAMOKU_CD_2.GetByteLength();
            intlen += KAMOKU_NAME_2.GetByteLength();
            intlen += ZEI_2.GetByteLength();
            intlen += KAMOKU_CD_3.GetByteLength();
            intlen += KAMOKU_NAME_3.GetByteLength();
            intlen += ZEI_3.GetByteLength();
            intlen += KAMOKU_CD_4.GetByteLength();
            intlen += KAMOKU_NAME_4.GetByteLength();
            intlen += ZEI_4.GetByteLength();
            intlen += KAMOKU_CD_5.GetByteLength();
            intlen += KAMOKU_NAME_5.GetByteLength();
            intlen += ZEI_5.GetByteLength();
            intlen += KAMOKU_CD_6.GetByteLength();
            intlen += KAMOKU_NAME_6.GetByteLength();
            intlen += ZEI_6.GetByteLength();
            intlen += KAMOKU_CD_7.GetByteLength();
            intlen += KAMOKU_NAME_7.GetByteLength();
            intlen += ZEI_7.GetByteLength();
            intlen += ZEI_TOTAL.GetByteLength();
            intlen += PACKAGE_NOHU_NO_1.GetByteLength();
            intlen += PACKAGE_NOHU_NO_2.GetByteLength();
            intlen += PACKAGE_NOHU_NO_3.GetByteLength();
            intlen += PACKAGE_NOHU_NO_4.GetByteLength();
            intlen += PACKAGE_NOHU_NO_5.GetByteLength();
            intlen += PACKAGE_NOHU_NO_6.GetByteLength();
            intlen += PACKAGE_NOHU_NO_7.GetByteLength();
            intlen += PACKAGE_NOHU_NO_8.GetByteLength();
            intlen += PACKAGE_NOHU_NO_9.GetByteLength();
            intlen += PACKAGE_NOHU_NO_10.GetByteLength();
            intlen += REPORT_DATE.GetByteLength();

            return intlen;
        }

        /// <summary>
        /// 添付用データの作成
        /// </summary>
        /// <returns></returns>
        public override byte[] GetByteData()
        {
            int intLen = this.GetByteLength();
            byte[] btData = new byte[intLen];
            int pos = 0;

            try
            {
                if (intLen > 0)
                {
                    //Array.Copy(this.OutputCommon.GetByteData(intLen), 0, btData, pos, this.OutputCommon.GetByteLength());
                    //pos += OutputCommon.GetByteLength();
                    Array.Copy(this.NaccsCommon.GetByteData(intLen), 0, btData, pos, this.NaccsCommon.GetByteLength());
                    pos += NaccsCommon.GetByteLength();


                    Array.Copy(this.DATA_FLG_TYPE.GetByteData(), 0, btData, pos, this.DATA_FLG_TYPE.GetByteLength());
                    pos += DATA_FLG_TYPE.GetByteLength();

                    Array.Copy(this.REPORT_TYPE.GetByteData(), 0, btData, pos, this.REPORT_TYPE.GetByteLength());
                    pos += REPORT_TYPE.GetByteLength();

                    Array.Copy(this.SHUNO_KIKAN_CD.GetByteData(), 0, btData, pos, this.SHUNO_KIKAN_CD.GetByteLength());
                    pos += SHUNO_KIKAN_CD.GetByteLength();

                    Array.Copy(this.NOUHU_NO.GetByteData(), 0, btData, pos, this.NOUHU_NO.GetByteLength());
                    pos += NOUHU_NO.GetByteLength();

                    Array.Copy(this.CONFIRM_NO.GetByteData(), 0, btData, pos, this.CONFIRM_NO.GetByteLength());
                    pos += CONFIRM_NO.GetByteLength();

                    Array.Copy(this.NOZEI_USER_CD.GetByteData(), 0, btData, pos, this.NOZEI_USER_CD.GetByteLength());
                    pos += NOZEI_USER_CD.GetByteLength();

                    Array.Copy(this.NOZEI_USER_NAME.GetByteData(), 0, btData, pos, this.NOZEI_USER_NAME.GetByteLength());
                    pos += NOZEI_USER_NAME.GetByteLength();

                    Array.Copy(this.ADDRESS1.GetByteData(), 0, btData, pos, this.ADDRESS1.GetByteLength());
                    pos += ADDRESS1.GetByteLength();

                    Array.Copy(this.ADDRESS2.GetByteData(), 0, btData, pos, this.ADDRESS2.GetByteLength());
                    pos += ADDRESS2.GetByteLength();

                    Array.Copy(this.ADDRESS3.GetByteData(), 0, btData, pos, this.ADDRESS3.GetByteLength());
                    pos += ADDRESS3.GetByteLength();

                    Array.Copy(this.ADDRESS4.GetByteData(), 0, btData, pos, this.ADDRESS4.GetByteLength());
                    pos += ADDRESS4.GetByteLength();

                    Array.Copy(this.GYOSHA_CD.GetByteData(), 0, btData, pos, this.GYOSHA_CD.GetByteLength());
                    pos += GYOSHA_CD.GetByteLength();

                    Array.Copy(this.ZEIKAN_NAME.GetByteData(), 0, btData, pos, this.ZEIKAN_NAME.GetByteLength());
                    pos += ZEIKAN_NAME.GetByteLength();

                    Array.Copy(this.KANSHO_NAME.GetByteData(), 0, btData, pos, this.KANSHO_NAME.GetByteLength());
                    pos += KANSHO_NAME.GetByteLength();

                    Array.Copy(this.YUNYU_SHINKOKU_NO.GetByteData(), 0, btData, pos, this.YUNYU_SHINKOKU_NO.GetByteLength());
                    pos += YUNYU_SHINKOKU_NO.GetByteLength();

                    Array.Copy(this.NOKI_DATE.GetByteData(), 0, btData, pos, this.NOKI_DATE.GetByteLength());
                    pos += NOKI_DATE.GetByteLength();

                    Array.Copy(this.EXTEND_NOKI_CD.GetByteData(), 0, btData, pos, this.EXTEND_NOKI_CD.GetByteLength());
                    pos += EXTEND_NOKI_CD.GetByteLength();

                    Array.Copy(this.SHOZOKU_YEAR.GetByteData(), 0, btData, pos, this.SHOZOKU_YEAR.GetByteLength());
                    pos += SHOZOKU_YEAR.GetByteLength();

                    Array.Copy(this.FIRST_DECLARATION_FLG.GetByteData(), 0, btData, pos, this.FIRST_DECLARATION_FLG.GetByteLength());
                    pos += FIRST_DECLARATION_FLG.GetByteLength();

                    Array.Copy(this.CORRECT_DECLARATION_FLG.GetByteData(), 0, btData, pos, this.CORRECT_DECLARATION_FLG.GetByteLength());
                    pos += CORRECT_DECLARATION_FLG.GetByteLength();

                    Array.Copy(this.NOHU_INFO_FLG.GetByteData(), 0, btData, pos, this.NOHU_INFO_FLG.GetByteLength());
                    pos += NOHU_INFO_FLG.GetByteLength();

                    Array.Copy(this.FORM_INFO_FLG.GetByteData(), 0, btData, pos, this.FORM_INFO_FLG.GetByteLength());
                    pos += FORM_INFO_FLG.GetByteLength();

                    Array.Copy(this.DECIDE_INFO_FLG.GetByteData(), 0, btData, pos, this.DECIDE_INFO_FLG.GetByteLength());
                    pos += DECIDE_INFO_FLG.GetByteLength();

                    Array.Copy(this.LEVY_TAX_INFO_FLG.GetByteData(), 0, btData, pos, this.LEVY_TAX_INFO_FLG.GetByteLength());
                    pos += LEVY_TAX_INFO_FLG.GetByteLength();

                    Array.Copy(this.UNDER_REPORT_ADD_TAX_FLG.GetByteData(), 0, btData, pos, this.UNDER_REPORT_ADD_TAX_FLG.GetByteLength());
                    pos += UNDER_REPORT_ADD_TAX_FLG.GetByteLength();

                    Array.Copy(this.UNDER_REPORT_HEAVY_ADD_TAX_FLG.GetByteData(), 0, btData, pos, this.UNDER_REPORT_HEAVY_ADD_TAX_FLG.GetByteLength());
                    pos += UNDER_REPORT_HEAVY_ADD_TAX_FLG.GetByteLength();

                    Array.Copy(this.NO_REPORT_ADD_TAX_FLG.GetByteData(), 0, btData, pos, this.NO_REPORT_ADD_TAX_FLG.GetByteLength());
                    pos += NO_REPORT_ADD_TAX_FLG.GetByteLength();

                    Array.Copy(this.NO_REPORT_HEAVY_ADD_TAX_FLG.GetByteData(), 0, btData, pos, this.NO_REPORT_HEAVY_ADD_TAX_FLG.GetByteLength());
                    pos += NO_REPORT_HEAVY_ADD_TAX_FLG.GetByteLength();

                    Array.Copy(this.KAMOKU_CD_1.GetByteData(), 0, btData, pos, this.KAMOKU_CD_1.GetByteLength());
                    pos += KAMOKU_CD_1.GetByteLength();

                    Array.Copy(this.KAMOKU_NAME_1.GetByteData(), 0, btData, pos, this.KAMOKU_NAME_1.GetByteLength());
                    pos += KAMOKU_NAME_1.GetByteLength();

                    Array.Copy(this.ZEI_1.GetByteData(), 0, btData, pos, this.ZEI_1.GetByteLength());
                    pos += ZEI_1.GetByteLength();

                    Array.Copy(this.KAMOKU_CD_2.GetByteData(), 0, btData, pos, this.KAMOKU_CD_2.GetByteLength());
                    pos += KAMOKU_CD_2.GetByteLength();

                    Array.Copy(this.KAMOKU_NAME_2.GetByteData(), 0, btData, pos, this.KAMOKU_NAME_2.GetByteLength());
                    pos += KAMOKU_NAME_2.GetByteLength();

                    Array.Copy(this.ZEI_2.GetByteData(), 0, btData, pos, this.ZEI_2.GetByteLength());
                    pos += ZEI_2.GetByteLength();

                    Array.Copy(this.KAMOKU_CD_3.GetByteData(), 0, btData, pos, this.KAMOKU_CD_3.GetByteLength());
                    pos += KAMOKU_CD_3.GetByteLength();

                    Array.Copy(this.KAMOKU_NAME_3.GetByteData(), 0, btData, pos, this.KAMOKU_NAME_3.GetByteLength());
                    pos += KAMOKU_NAME_3.GetByteLength();

                    Array.Copy(this.ZEI_3.GetByteData(), 0, btData, pos, this.ZEI_3.GetByteLength());
                    pos += ZEI_3.GetByteLength();

                    Array.Copy(this.KAMOKU_CD_4.GetByteData(), 0, btData, pos, this.KAMOKU_CD_4.GetByteLength());
                    pos += KAMOKU_CD_4.GetByteLength();

                    Array.Copy(this.KAMOKU_NAME_4.GetByteData(), 0, btData, pos, this.KAMOKU_NAME_4.GetByteLength());
                    pos += KAMOKU_NAME_4.GetByteLength();

                    Array.Copy(this.ZEI_4.GetByteData(), 0, btData, pos, this.ZEI_4.GetByteLength());
                    pos += ZEI_4.GetByteLength();

                    Array.Copy(this.KAMOKU_CD_5.GetByteData(), 0, btData, pos, this.KAMOKU_CD_5.GetByteLength());
                    pos += KAMOKU_CD_5.GetByteLength();

                    Array.Copy(this.KAMOKU_NAME_5.GetByteData(), 0, btData, pos, this.KAMOKU_NAME_5.GetByteLength());
                    pos += KAMOKU_NAME_5.GetByteLength();

                    Array.Copy(this.ZEI_5.GetByteData(), 0, btData, pos, this.ZEI_5.GetByteLength());
                    pos += ZEI_5.GetByteLength();

                    Array.Copy(this.KAMOKU_CD_6.GetByteData(), 0, btData, pos, this.KAMOKU_CD_6.GetByteLength());
                    pos += KAMOKU_CD_6.GetByteLength();

                    Array.Copy(this.KAMOKU_NAME_6.GetByteData(), 0, btData, pos, this.KAMOKU_NAME_6.GetByteLength());
                    pos += KAMOKU_NAME_6.GetByteLength();

                    Array.Copy(this.ZEI_6.GetByteData(), 0, btData, pos, this.ZEI_6.GetByteLength());
                    pos += ZEI_6.GetByteLength();

                    Array.Copy(this.KAMOKU_CD_7.GetByteData(), 0, btData, pos, this.KAMOKU_CD_7.GetByteLength());
                    pos += KAMOKU_CD_7.GetByteLength();

                    Array.Copy(this.KAMOKU_NAME_7.GetByteData(), 0, btData, pos, this.KAMOKU_NAME_7.GetByteLength());
                    pos += KAMOKU_NAME_7.GetByteLength();

                    Array.Copy(this.ZEI_7.GetByteData(), 0, btData, pos, this.ZEI_7.GetByteLength());
                    pos += ZEI_7.GetByteLength();

                    Array.Copy(this.ZEI_TOTAL.GetByteData(), 0, btData, pos, this.ZEI_TOTAL.GetByteLength());
                    pos += ZEI_TOTAL.GetByteLength();

                    Array.Copy(this.PACKAGE_NOHU_NO_1.GetByteData(), 0, btData, pos, this.PACKAGE_NOHU_NO_1.GetByteLength());
                    pos += PACKAGE_NOHU_NO_1.GetByteLength();

                    Array.Copy(this.PACKAGE_NOHU_NO_2.GetByteData(), 0, btData, pos, this.PACKAGE_NOHU_NO_2.GetByteLength());
                    pos += PACKAGE_NOHU_NO_2.GetByteLength();

                    Array.Copy(this.PACKAGE_NOHU_NO_3.GetByteData(), 0, btData, pos, this.PACKAGE_NOHU_NO_3.GetByteLength());
                    pos += PACKAGE_NOHU_NO_3.GetByteLength();

                    Array.Copy(this.PACKAGE_NOHU_NO_4.GetByteData(), 0, btData, pos, this.PACKAGE_NOHU_NO_4.GetByteLength());
                    pos += PACKAGE_NOHU_NO_4.GetByteLength();

                    Array.Copy(this.PACKAGE_NOHU_NO_5.GetByteData(), 0, btData, pos, this.PACKAGE_NOHU_NO_5.GetByteLength());
                    pos += PACKAGE_NOHU_NO_5.GetByteLength();

                    Array.Copy(this.PACKAGE_NOHU_NO_6.GetByteData(), 0, btData, pos, this.PACKAGE_NOHU_NO_6.GetByteLength());
                    pos += PACKAGE_NOHU_NO_6.GetByteLength();

                    Array.Copy(this.PACKAGE_NOHU_NO_7.GetByteData(), 0, btData, pos, this.PACKAGE_NOHU_NO_7.GetByteLength());
                    pos += PACKAGE_NOHU_NO_7.GetByteLength();

                    Array.Copy(this.PACKAGE_NOHU_NO_8.GetByteData(), 0, btData, pos, this.PACKAGE_NOHU_NO_8.GetByteLength());
                    pos += PACKAGE_NOHU_NO_8.GetByteLength();

                    Array.Copy(this.PACKAGE_NOHU_NO_9.GetByteData(), 0, btData, pos, this.PACKAGE_NOHU_NO_9.GetByteLength());
                    pos += PACKAGE_NOHU_NO_9.GetByteLength();

                    Array.Copy(this.PACKAGE_NOHU_NO_10.GetByteData(), 0, btData, pos, this.PACKAGE_NOHU_NO_10.GetByteLength());
                    pos += PACKAGE_NOHU_NO_10.GetByteLength();

                    Array.Copy(this.REPORT_DATE.GetByteData(), 0, btData, pos, this.REPORT_DATE.GetByteLength());
                    pos += REPORT_DATE.GetByteLength();

                }
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
                    this.NaccsCommon.SetByteData(btData.Skip(pos).Take(this.NaccsCommon.GetByteLength()).ToArray());
                    pos += this.NaccsCommon.GetByteLength();


                    this.DATA_FLG_TYPE.SetData(EUC.GetString(btData, pos, this.DATA_FLG_TYPE.GetByteLength()));
                    pos += DATA_FLG_TYPE.GetByteLength();

                    this.REPORT_TYPE.SetData(EUC.GetString(btData, pos, this.REPORT_TYPE.GetByteLength()));
                    pos += this.REPORT_TYPE.GetByteLength();

                    this.SHUNO_KIKAN_CD.SetData(EUC.GetString(btData, pos, this.SHUNO_KIKAN_CD.GetByteLength()));
                    pos += this.SHUNO_KIKAN_CD.GetByteLength();

                    this.NOUHU_NO.SetData(EUC.GetString(btData, pos, this.NOUHU_NO.GetByteLength()));
                    pos += this.NOUHU_NO.GetByteLength();

                    this.CONFIRM_NO.SetData(EUC.GetString(btData, pos, this.CONFIRM_NO.GetByteLength()));
                    pos += this.CONFIRM_NO.GetByteLength();

                    this.NOZEI_USER_CD.SetData(EUC.GetString(btData, pos, this.NOZEI_USER_CD.GetByteLength()));
                    pos += this.NOZEI_USER_CD.GetByteLength();

                    this.NOZEI_USER_NAME.SetData(EUC.GetString(btData, pos, this.NOZEI_USER_NAME.GetByteLength()));
                    pos += this.NOZEI_USER_NAME.GetByteLength();

                    this.ADDRESS1.SetData(EUC.GetString(btData, pos, this.ADDRESS1.GetByteLength()));
                    pos += this.ADDRESS1.GetByteLength();

                    this.ADDRESS2.SetData(EUC.GetString(btData, pos, this.ADDRESS2.GetByteLength()));
                    pos += this.ADDRESS2.GetByteLength();

                    this.ADDRESS3.SetData(EUC.GetString(btData, pos, this.ADDRESS3.GetByteLength()));
                    pos += this.ADDRESS3.GetByteLength();

                    this.ADDRESS4.SetData(EUC.GetString(btData, pos, this.ADDRESS4.GetByteLength()));
                    pos += this.ADDRESS4.GetByteLength();

                    this.GYOSHA_CD.SetData(EUC.GetString(btData, pos, this.GYOSHA_CD.GetByteLength()));
                    pos += this.GYOSHA_CD.GetByteLength();

                    this.ZEIKAN_NAME.SetData(EUC.GetString(btData, pos, this.ZEIKAN_NAME.GetByteLength()));
                    pos += this.ZEIKAN_NAME.GetByteLength();

                    this.KANSHO_NAME.SetData(EUC.GetString(btData, pos, this.KANSHO_NAME.GetByteLength()));
                    pos += this.KANSHO_NAME.GetByteLength();

                    this.YUNYU_SHINKOKU_NO.SetData(EUC.GetString(btData, pos, this.YUNYU_SHINKOKU_NO.GetByteLength()));
                    pos += this.YUNYU_SHINKOKU_NO.GetByteLength();
                    
                    this.NOKI_DATE.SetData(EUC.GetString(btData, pos, this.NOKI_DATE.GetByteLength()));
                    pos += this.NOKI_DATE.GetByteLength();
                    
                    this.EXTEND_NOKI_CD.SetData(EUC.GetString(btData, pos, this.EXTEND_NOKI_CD.GetByteLength()));
                    pos += this.EXTEND_NOKI_CD.GetByteLength();
                    
                    this.SHOZOKU_YEAR.SetData(EUC.GetString(btData, pos, this.SHOZOKU_YEAR.GetByteLength()));
                    pos += this.SHOZOKU_YEAR.GetByteLength();
                    
                    this.FIRST_DECLARATION_FLG.SetData(EUC.GetString(btData, pos, this.FIRST_DECLARATION_FLG.GetByteLength()));
                    pos += this.FIRST_DECLARATION_FLG.GetByteLength();
                    
                    this.CORRECT_DECLARATION_FLG.SetData(EUC.GetString(btData, pos, this.CORRECT_DECLARATION_FLG.GetByteLength()));
                    pos += this.CORRECT_DECLARATION_FLG.GetByteLength();
                    
                    this.NOHU_INFO_FLG.SetData(EUC.GetString(btData, pos, this.NOHU_INFO_FLG.GetByteLength()));
                    pos += this.NOHU_INFO_FLG.GetByteLength();
                    
                    this.FORM_INFO_FLG.SetData(EUC.GetString(btData, pos, this.FORM_INFO_FLG.GetByteLength()));
                    pos += this.FORM_INFO_FLG.GetByteLength();
                    
                    this.DECIDE_INFO_FLG.SetData(EUC.GetString(btData, pos, this.DECIDE_INFO_FLG.GetByteLength()));
                    pos += this.DECIDE_INFO_FLG.GetByteLength();
                    
                    this.LEVY_TAX_INFO_FLG.SetData(EUC.GetString(btData, pos, this.LEVY_TAX_INFO_FLG.GetByteLength()));
                    pos += this.LEVY_TAX_INFO_FLG.GetByteLength();
                    
                    this.UNDER_REPORT_ADD_TAX_FLG.SetData(EUC.GetString(btData, pos, this.UNDER_REPORT_ADD_TAX_FLG.GetByteLength()));
                    pos += this.UNDER_REPORT_ADD_TAX_FLG.GetByteLength();
                    
                    this.UNDER_REPORT_HEAVY_ADD_TAX_FLG.SetData(EUC.GetString(btData, pos, this.UNDER_REPORT_HEAVY_ADD_TAX_FLG.GetByteLength()));
                    pos += this.UNDER_REPORT_HEAVY_ADD_TAX_FLG.GetByteLength();
                    
                    this.NO_REPORT_ADD_TAX_FLG.SetData(EUC.GetString(btData, pos, this.NO_REPORT_ADD_TAX_FLG.GetByteLength()));
                    pos += this.NO_REPORT_ADD_TAX_FLG.GetByteLength();
                    
                    this.NO_REPORT_HEAVY_ADD_TAX_FLG.SetData(EUC.GetString(btData, pos, this.NO_REPORT_HEAVY_ADD_TAX_FLG.GetByteLength()));
                    pos += this.NO_REPORT_HEAVY_ADD_TAX_FLG.GetByteLength();
                    
                    this.KAMOKU_CD_1.SetData(EUC.GetString(btData, pos, this.KAMOKU_CD_1.GetByteLength()));
                    pos += this.KAMOKU_CD_1.GetByteLength();
                    
                    this.KAMOKU_NAME_1.SetData(EUC.GetString(btData, pos, this.KAMOKU_NAME_1.GetByteLength()));
                    pos += this.KAMOKU_NAME_1.GetByteLength();
                    
                    this.ZEI_1.SetData(EUC.GetString(btData, pos, this.ZEI_1.GetByteLength()));
                    pos += this.ZEI_1.GetByteLength();
                    
                    this.KAMOKU_CD_2.SetData(EUC.GetString(btData, pos, this.KAMOKU_CD_2.GetByteLength()));
                    pos += this.KAMOKU_CD_2.GetByteLength();
                    
                    this.KAMOKU_NAME_2.SetData(EUC.GetString(btData, pos, this.KAMOKU_NAME_2.GetByteLength()));
                    pos += this.KAMOKU_NAME_2.GetByteLength();
                    
                    this.ZEI_2.SetData(EUC.GetString(btData, pos, this.ZEI_2.GetByteLength()));
                    pos += this.ZEI_2.GetByteLength();
                    
                    this.KAMOKU_CD_3.SetData(EUC.GetString(btData, pos, this.KAMOKU_CD_3.GetByteLength()));
                    pos += this.KAMOKU_CD_3.GetByteLength();
                    
                    this.KAMOKU_NAME_3.SetData(EUC.GetString(btData, pos, this.KAMOKU_NAME_3.GetByteLength()));
                    pos += this.KAMOKU_NAME_3.GetByteLength();
                    
                    this.ZEI_3.SetData(EUC.GetString(btData, pos, this.ZEI_3.GetByteLength()));
                    pos += this.ZEI_3.GetByteLength();

                    this.KAMOKU_CD_4.SetData(EUC.GetString(btData, pos, this.KAMOKU_CD_4.GetByteLength()));
                    pos += this.KAMOKU_CD_4.GetByteLength();

                    this.KAMOKU_NAME_4.SetData(EUC.GetString(btData, pos, this.KAMOKU_NAME_4.GetByteLength()));
                    pos += this.KAMOKU_NAME_4.GetByteLength();

                    this.ZEI_4.SetData(EUC.GetString(btData, pos, this.ZEI_4.GetByteLength()));
                    pos += this.ZEI_4.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += KAMOKU_CD_5.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += KAMOKU_NAME_5.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += ZEI_5.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += KAMOKU_CD_6.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += KAMOKU_NAME_6.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += ZEI_6.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += KAMOKU_CD_7.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += KAMOKU_NAME_7.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += ZEI_7.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += ZEI_TOTAL.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += PACKAGE_NOHU_NO_1.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += PACKAGE_NOHU_NO_2.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += PACKAGE_NOHU_NO_3.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += PACKAGE_NOHU_NO_4.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += PACKAGE_NOHU_NO_5.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += PACKAGE_NOHU_NO_6.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += PACKAGE_NOHU_NO_7.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += PACKAGE_NOHU_NO_8.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += PACKAGE_NOHU_NO_9.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += PACKAGE_NOHU_NO_10.GetByteLength();

                    //this..SetData(EUC.GetString(btData, pos, this..GetByteLength()));
                    //pos += this..GetByteLength();
                    //intlen += REPORT_DATE.GetByteLength();

                    this.YunyuShinkokuNo.SetData(YUNYU_SHINKOKU_NO.Data);                   
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

    #endregion "納付番号通知情報　SAF002 & SBF720"

    #region "口座使用不可通知情報　SAF021"

    /// <summary>
    /// 口座使用不可通知情報　SAF021
    /// </summary>
    public class ProcessSaf021Model : NaccsRecvModel
    {
        public override string GetFileName()
        {
            if (string.IsNullOrEmpty(YunyuShinkokuNo.Data))
            {
                return string.Format("{0}_{1}_{2}", LOGSTR, this.GetReportType.ToString(), DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            }
            else
            {
                //return string.Format("{0}_{1}_{2}", GetYunyuShinkokuNo, this.GetReportType.ToString(), DateTime.Now.ToString("yyyyMMddhhmmssfff"));
                return string.Format("{0}_{1}_{2} {3}_{4}",
                    NaccsCommon.USER_CD.Data,
                    this.GetReportType.ToString(),
                    GetYunyuShinkokuNo,
                    this.KozaNo.Data,
                    DateTime.Now.ToString("yyyyMMddhhmmss"));
            }
        }
        ///// <summary>
        ///// 入力共通項目398
        ///// </summary>
        //public OutputCommonModel OutputCommon = new OutputCommonModel();
        ///// <summary>
        ///// 輸入申告番号 2
        ///// </summary>
        //public NacCollumnAN YunyuShinkokuNo = new NacCollumnAN(11);

        /// <summary>
        /// 口座番号 3　an 14 M
        /// </summary>
        public NacCollumnAN KozaNo = new NacCollumnAN(14);
        /// <summary>
        /// 輸入者コード 4
        /// </summary>
        public NacCollumnAN YunyushaCode = new NacCollumnAN(17);
        /// <summary>
        /// 輸入者名 5
        /// </summary>
        public NacCollumnAN YunyushaName = new NacCollumnAN(70);
        /// <summary>
        /// 代理人コード　6
        /// </summary>
        public NacCollumnAN DairiCode = new NacCollumnAN(5);
        /// <summary>
        /// 代理人氏名　7
        /// </summary>
        public NacCollumnAN DairiName = new NacCollumnAN(50);
        /// <summary>
        /// 納税額合計 8
        /// </summary>
        public NacCollumnN NouzeiTotal = new NacCollumnN(11);
        /// <summary>
        /// 口座使用不可識別 9 an
        /// </summary>
        public NacCollumnAN KozaNotUse = new NacCollumnAN(1);

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProcessSaf021Model()
        {
            //OutputCommon = new OutputCommonModel();
            NaccsCommon = new OutputCommonModel();
        }
        /// <summary>
        /// データバイト長取得
        /// </summary>
        /// <returns></returns>
        public int GetByteLength()
        {
            int intlen = 0;

            //intlen += OutputCommon.GetByteLength();
            intlen += NaccsCommon.GetByteLength();
            intlen += YunyuShinkokuNo.GetByteLength();
            intlen += KozaNo.GetByteLength();
            intlen += YunyushaCode.GetByteLength();
            intlen += YunyushaName.GetByteLength();
            intlen += DairiCode.GetByteLength();
            intlen += DairiName.GetByteLength();
            intlen += NouzeiTotal.GetByteLength();
            intlen += KozaNotUse.GetByteLength();

            return intlen;
        }

        /// <summary>
        /// 添付用データの作成
        /// </summary>
        /// <returns></returns>
        public override byte[] GetByteData()
        {
            int intLen = this.GetByteLength();
            byte[] btData = new byte[intLen];
            int pos = 0;
            try
            {
                if (intLen > 0)
                {
                    //Array.Copy(this.OutputCommon.GetByteData(intLen), 0, btData, pos, this.OutputCommon.GetByteLength());
                    //pos += OutputCommon.GetByteLength();
                    Array.Copy(this.NaccsCommon.GetByteData(intLen), 0, btData, pos, this.NaccsCommon.GetByteLength());
                    pos += NaccsCommon.GetByteLength();

                    Array.Copy(this.YunyuShinkokuNo.GetByteData(), 0, btData, pos, this.YunyuShinkokuNo.GetByteLength());
                    pos += YunyuShinkokuNo.GetByteLength();

                    Array.Copy(this.KozaNo.GetByteData(), 0, btData, pos, this.KozaNo.GetByteLength());
                    pos += KozaNo.GetByteLength();

                    Array.Copy(this.YunyushaCode.GetByteData(), 0, btData, pos, this.YunyushaCode.GetByteLength());
                    pos += YunyushaCode.GetByteLength();
                    Array.Copy(this.YunyushaName.GetByteData(), 0, btData, pos, this.YunyushaName.GetByteLength());
                    pos += YunyushaName.GetByteLength();
                    Array.Copy(this.DairiCode.GetByteData(), 0, btData, pos, this.DairiCode.GetByteLength());
                    pos += DairiCode.GetByteLength();
                    Array.Copy(this.DairiName.GetByteData(), 0, btData, pos, this.DairiName.GetByteLength());
                    pos += DairiName.GetByteLength();
                    Array.Copy(this.NouzeiTotal.GetByteData(), 0, btData, pos, this.NouzeiTotal.GetByteLength());
                    pos += NouzeiTotal.GetByteLength();
                    Array.Copy(this.KozaNotUse.GetByteData(), 0, btData, pos, this.KozaNotUse.GetByteLength());
                    pos += KozaNotUse.GetByteLength();

                }
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
                    this.NaccsCommon.SetByteData(btData.Skip(pos).Take(this.NaccsCommon.GetByteLength()).ToArray());
                    pos += this.NaccsCommon.GetByteLength();

                    this.YunyuShinkokuNo.SetData(EUC.GetString(btData, pos, this.YunyuShinkokuNo.GetByteLength()));
                    pos += YunyuShinkokuNo.GetByteLength();


                    this.KozaNo.SetData(EUC.GetString(btData, pos, this.KozaNo.GetByteLength()));
                    pos += this.KozaNo.GetByteLength();

                    this.YunyushaCode.SetData(EUC.GetString(btData, pos, this.YunyushaCode.GetByteLength()));
                    pos += this.YunyushaCode.GetByteLength();

                    this.YunyushaName.SetData(EUC.GetString(btData, pos, this.YunyushaName.GetByteLength()));
                    pos += YunyushaName.GetByteLength();

                    this.DairiCode.SetData(EUC.GetString(btData, pos, this.DairiCode.GetByteLength()));
                    pos += this.DairiCode.GetByteLength();

                    this.DairiName.SetData(EUC.GetString(btData, pos, this.DairiName.GetByteLength()));
                    pos += this.DairiName.GetByteLength();


                    this.NouzeiTotal.SetData(EUC.GetString(btData, pos, this.NouzeiTotal.GetByteLength()));
                    pos += this.NouzeiTotal.GetByteLength();

                    this.KozaNotUse.SetData(EUC.GetString(btData, pos, this.KozaNotUse.GetByteLength()));
                    pos += this.KozaNotUse.GetByteLength();


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

    #endregion "口座使用不可通知情報　SAF021"

    #region "担保不足通知情報　SAF022"

    /// <summary>
    /// 担保不足通知情報　SAF022
    /// </summary>
    public class ProcessSaf022Model : NaccsRecvModel
    {
        public override string GetFileName()
        {
            if (string.IsNullOrEmpty(YunyuShinkokuNo.Data))
            {
                return string.Format("{0}_{1}_{2}", LOGSTR, this.GetReportType.ToString(), DateTime.Now.ToString("yyyyMMddhhmmssfff"));
            }
            else
            {
                //return string.Format("{0}_{1}_{2}", GetYunyuShinkokuNo, this.GetReportType.ToString(), DateTime.Now.ToString("yyyyMMddhhmmssfff"));
                return string.Format("{0}_{1}_{2}_{3}", NaccsCommon.USER_CD.Data, this.GetReportType.ToString(), GetYunyuShinkokuNo, DateTime.Now.ToString("yyyyMMddhhmmss"));
            }
        }
        ///// <summary>
        ///// 入力共通項目398
        ///// </summary>
        //public OutputCommonModel OutputCommon = new OutputCommonModel();
        ///// <summary>
        ///// 輸入申告番号 2
        ///// </summary>
        //public NacCollumnAN YunyuShinkokuNo = new NacCollumnAN(11);
        /// <summary>
        /// 輸入者コード 3
        /// </summary>
        public NacCollumnAN YunyushaCode = new NacCollumnAN(17);
        /// <summary>
        /// 輸入者名 4
        /// </summary>
        public NacCollumnAN YunyushaName = new NacCollumnAN(70);
        /// <summary>
        /// 代理人コード　5
        /// </summary>
        public NacCollumnAN DairiCode = new NacCollumnAN(5);
        /// <summary>
        /// 代理人氏名　6
        /// </summary>
        public NacCollumnAN DairiName = new NacCollumnAN(50);
        /// <summary>
        /// 担保登録番号 7
        /// </summary>
        public NacCollumnAN TanpoNo1 = new NacCollumnAN(9);
        /// <summary>
        /// 担保登録番号 7
        /// </summary>
        public NacCollumnAN TanpoNo2 = new NacCollumnAN(9);
        /// <summary>
        /// 担保提供原因 8
        /// </summary>
        public NacCollumnAN TanpoGenin1 = new NacCollumnAN(3);
        /// <summary>
        /// 担保提供原因 8
        /// </summary>
        public NacCollumnAN TanpoGenin2 = new NacCollumnAN(3);
        /// <summary>
        /// 担保提供原因 8
        /// </summary>
        public NacCollumnAN TanpoGenin3 = new NacCollumnAN(3);
        /// <summary>
        /// 担保提供原因 8
        /// </summary>
        public NacCollumnAN TanpoGenin4 = new NacCollumnAN(3);

        /// <summary>
        /// 担保不足額 9 List 11
        /// </summary>
        public List<NacCollumnN> TanpoHusokuList = new List<NacCollumnN>();


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProcessSaf022Model()
        {
            //OutputCommon = new OutputCommonModel();
            NaccsCommon = new OutputCommonModel();

            TanpoHusokuList = new List<NacCollumnN>();
            NacCollumnN sm = new NacCollumnN(11);
            TanpoHusokuList.Add(sm);
        }
        /// <summary>
        /// データバイト長取得
        /// </summary>
        /// <returns></returns>
        public int GetByteLength()
        {
            int intlen = 0;

            //intlen += OutputCommon.GetByteLength();
            intlen += NaccsCommon.GetByteLength();
            intlen += YunyuShinkokuNo.GetByteLength();
            intlen += YunyushaCode.GetByteLength();
            intlen += YunyushaName.GetByteLength();
            intlen += DairiCode.GetByteLength();
            intlen += DairiName.GetByteLength();
            intlen += TanpoNo1.GetByteLength();
            intlen += TanpoNo2.GetByteLength();
            intlen += TanpoGenin1.GetByteLength();
            intlen += TanpoGenin2.GetByteLength();
            intlen += TanpoGenin3.GetByteLength();
            intlen += TanpoGenin4.GetByteLength();

            for (int i = 0; i < TanpoHusokuList.Count(); i++)
            {
                intlen += TanpoHusokuList[i].GetByteLength();
            }
            return intlen;
        }

        /// <summary>
        /// 添付用データの作成
        /// </summary>
        /// <returns></returns>
        public override byte[] GetByteData()
        {
            int intLen = this.GetByteLength();
            byte[] btData = new byte[intLen];
            int pos = 0;
            try
            {
                if (intLen > 0)
                {
                    //Array.Copy(this.OutputCommon.GetByteData(intLen), 0, btData, pos, this.OutputCommon.GetByteLength());
                    //pos += OutputCommon.GetByteLength();
                    Array.Copy(this.NaccsCommon.GetByteData(intLen), 0, btData, pos, this.NaccsCommon.GetByteLength());
                    pos += NaccsCommon.GetByteLength();

                    Array.Copy(this.YunyuShinkokuNo.GetByteData(), 0, btData, pos, this.YunyuShinkokuNo.GetByteLength());
                    pos += YunyuShinkokuNo.GetByteLength();
                    Array.Copy(this.YunyushaCode.GetByteData(), 0, btData, pos, this.YunyushaCode.GetByteLength());
                    pos += YunyushaCode.GetByteLength();
                    Array.Copy(this.YunyushaName.GetByteData(), 0, btData, pos, this.YunyushaName.GetByteLength());
                    pos += YunyushaName.GetByteLength();
                    Array.Copy(this.DairiCode.GetByteData(), 0, btData, pos, this.DairiCode.GetByteLength());
                    pos += DairiCode.GetByteLength();
                    Array.Copy(this.DairiName.GetByteData(), 0, btData, pos, this.DairiName.GetByteLength());
                    pos += DairiName.GetByteLength();
                    Array.Copy(this.TanpoNo1.GetByteData(), 0, btData, pos, this.TanpoNo1.GetByteLength());
                    pos += TanpoNo1.GetByteLength();
                    Array.Copy(this.TanpoNo2.GetByteData(), 0, btData, pos, this.TanpoNo2.GetByteLength());
                    pos += TanpoNo2.GetByteLength();
                    Array.Copy(this.TanpoGenin1.GetByteData(), 0, btData, pos, this.TanpoGenin1.GetByteLength());
                    pos += TanpoGenin1.GetByteLength();
                    Array.Copy(this.TanpoGenin2.GetByteData(), 0, btData, pos, this.TanpoGenin2.GetByteLength());
                    pos += TanpoGenin2.GetByteLength();
                    Array.Copy(this.TanpoGenin3.GetByteData(), 0, btData, pos, this.TanpoGenin3.GetByteLength());
                    pos += TanpoGenin3.GetByteLength();
                    Array.Copy(this.TanpoGenin4.GetByteData(), 0, btData, pos, this.TanpoGenin4.GetByteLength());
                    pos += TanpoGenin4.GetByteLength();

                    for (int i = 0; i < TanpoHusokuList.Count(); i++)
                    {
                        Array.Copy(this.TanpoHusokuList[i].GetByteData(), 0, btData, pos, this.TanpoHusokuList[i].GetByteLength());
                        pos += TanpoHusokuList[i].GetByteLength();
                    }

                }
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
                    this.NaccsCommon.SetByteData(btData.Skip(pos).Take(this.NaccsCommon.GetByteLength()).ToArray());
                    pos += this.NaccsCommon.GetByteLength();

                    this.YunyuShinkokuNo.SetData(EUC.GetString(btData, pos, this.YunyuShinkokuNo.GetByteLength()));
                    pos += YunyuShinkokuNo.GetByteLength();

                    this.YunyushaCode.SetData(EUC.GetString(btData, pos, this.YunyushaCode.GetByteLength()));
                    pos += this.YunyushaCode.GetByteLength();

                    this.YunyushaName.SetData(EUC.GetString(btData, pos, this.YunyushaName.GetByteLength()));
                    pos += YunyushaName.GetByteLength();

                    this.DairiCode.SetData(EUC.GetString(btData, pos, this.DairiCode.GetByteLength()));
                    pos += this.DairiCode.GetByteLength();

                    this.DairiName.SetData(EUC.GetString(btData, pos, this.DairiName.GetByteLength()));
                    pos += this.DairiName.GetByteLength();
                    
                    this.TanpoNo1.SetData(EUC.GetString(btData, pos, this.TanpoNo1.GetByteLength()));
                    pos += this.TanpoNo1.GetByteLength();

                    this.TanpoNo2.SetData(EUC.GetString(btData, pos, this.TanpoNo2.GetByteLength()));
                    pos += this.TanpoNo2.GetByteLength();

                    this.TanpoGenin1.SetData(EUC.GetString(btData, pos, this.TanpoGenin1.GetByteLength()));
                    pos += this.TanpoGenin1.GetByteLength();

                    this.TanpoGenin2.SetData(EUC.GetString(btData, pos, this.TanpoGenin2.GetByteLength()));
                    pos += this.TanpoGenin2.GetByteLength();

                    this.TanpoGenin3.SetData(EUC.GetString(btData, pos, this.TanpoGenin3.GetByteLength()));
                    pos += this.TanpoGenin3.GetByteLength();

                    this.TanpoGenin4.SetData(EUC.GetString(btData, pos, this.TanpoGenin4.GetByteLength()));
                    pos += this.TanpoGenin4.GetByteLength();

                    //複数来る可能性あり　※※※
                    this.TanpoHusokuList[0].SetData(EUC.GetString(btData, pos, this.TanpoHusokuList[0].GetByteLength()));
                    pos += this.TanpoHusokuList[0].GetByteLength();

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

    #endregion "担保不足通知情報　SAF022"


    #region "税科目モデル"

    /// <summary>
    /// 税科目モデル
    /// </summary>
    public class ZeiKamokuModel : NaccsModel
    {
        /// <summary>
        /// 税科目コード　20
        /// </summary>
        public NacCollumnAN ZeiKamokuCode = new NacCollumnAN(1);
        /// <summary>
        /// 税科目名 21
        /// </summary>
        public NacCollumnJJ ZeiKamokuName = new NacCollumnJJ(22);
        /// <summary>
        /// 税額合計 22
        /// </summary>
        public NacCollumnN ZeiTotal = new NacCollumnN(11);
        /// <summary>
        /// 税額合計欄数 23
        /// </summary>
        public NacCollumnN ZeiTotalRanNum = new NacCollumnN(2);

        /// <summary>
        /// 内容のセット
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SetData(object obj)
        {
            return true;
        }
        /// <summary>
        /// バイト長
        /// </summary>
        /// <returns></returns>
        public int GetByteLength()
        {
            int intlen = 0;
            
            intlen += ZeiKamokuCode.GetByteLength();
            intlen += ZeiKamokuName.GetByteLength();
            intlen += ZeiTotal.GetByteLength();
            intlen += ZeiTotalRanNum.GetByteLength();

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
                Array.Copy(this.ZeiKamokuCode.GetByteData(), 0, btData, pos, this.ZeiKamokuCode.GetByteLength());
                pos += ZeiKamokuCode.GetByteLength();
                Array.Copy(this.ZeiKamokuName.GetByteData(), 0, btData, pos, this.ZeiKamokuName.GetByteLength());
                pos += ZeiKamokuName.GetByteLength();
                Array.Copy(this.ZeiTotal.GetByteData(), 0, btData, pos, this.ZeiTotal.GetByteLength());
                pos += ZeiTotal.GetByteLength();
                Array.Copy(this.ZeiTotalRanNum.GetByteData(), 0, btData, pos, this.ZeiTotalRanNum.GetByteLength());
                pos += ZeiTotalRanNum.GetByteLength();

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
                    this.ZeiKamokuCode.SetData(EUC.GetString(btData, pos, this.ZeiKamokuCode.GetByteLength()));
                    pos += ZeiKamokuCode.GetByteLength();

                    this.ZeiKamokuName.SetData(EUC.GetString(btData, pos, this.ZeiKamokuName.GetByteLength()));
                    pos += ZeiKamokuName.GetByteLength();
                    
                    this.ZeiTotal.SetData(EUC.GetString(btData, pos, this.ZeiTotal.GetByteLength()));
                    pos += ZeiTotal.GetByteLength();

                    this.ZeiTotalRanNum.SetData(EUC.GetString(btData, pos, this.ZeiTotalRanNum.GetByteLength()));
                    pos += ZeiTotalRanNum.GetByteLength();

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
        ///// <summary>
        ///// ファイル用の文字列取得
        ///// </summary>
        ///// <returns></returns>
        //public string GetFileData()
        //{
        //    StringBuilder str = new StringBuilder();
        //    try
        //    {
        //        str.AppendLine(ZeiKamokuCode.Data);
        //        str.AppendLine(ZeiKamokuName.Data);
        //        str.AppendLine(ZeiTotal.Data);
        //        str.AppendLine(ZeiTotalRanNum.Data);

        //        return str.ToString();
        //    }
        //    catch
        //    {
        //        return "ZeiKamokuModel Error";
        //    }
        //}
    }

    #endregion "税科目モデル"

    #region "商品モデル　NACCS"

    /// <summary>
    /// 入力控用商品モデル
    /// </summary>
    public class ShohinRecvModel : NaccsModel
    {
        /// <summary>
        /// 欄番号　37
        /// </summary>
        public NacCollumnNPD RanNo = new NacCollumnNPD(2, 2);
        /// <summary>
        /// 統合先欄番号　38
        /// </summary>
        public NacCollumnN RanNoToGoSaki = new NacCollumnN(2);
        /// <summary>
        /// 関税課税物品表示　39
        /// </summary>
        public NacCollumnFLG KanzeiKazeiHyoji = new NacCollumnFLG(1);
        /// <summary>
        /// 関税免除物品表示　40
        /// </summary>
        public NacCollumnFLG KanzeiMenjoHyoji = new NacCollumnFLG(1);
        /// <summary>
        /// 商品管理コード 41
        /// </summary>
        public NacCollumnAN ShohinKannriCD = new NacCollumnAN(6);
        /// <summary>
        /// 品目コード 42
        /// </summary>
        public NacCollumnAN HinmokuCD = new NacCollumnAN(9);
        /// <summary>
        /// NACCS用コード 43
        /// </summary>
        public NacCollumnAN NaccsCD = new NacCollumnAN(1);
        /// <summary>
        /// 品名 44
        /// </summary>
        public NacCollumnAN ShohinName = new NacCollumnAN(40);
        /// <summary>
        /// 販売価格（単価） 45
        /// </summary>
        public NacCollumnN HanbaiTanka = new NacCollumnN(9);
        /// <summary>
        /// 販売数量 46
        /// </summary>
        public NacCollumnN HanbaiSuryo = new NacCollumnN(4);
        /// <summary>
        /// 税表番号 47
        /// </summary>
        public NacCollumnAN ZeiHyoNo = new NacCollumnAN(19);
        /// <summary>
        /// 数量１ 48
        /// </summary>
        public NacCollumnN Suryo1 = new NacCollumnN(12);
        /// <summary>
        /// 数量単位コード 49
        /// </summary>
        public NacCollumnAN Suryo1TaniCD = new NacCollumnAN(4);
        /// <summary>
        /// 関税課税価格 50
        /// </summary>
        public NacCollumnN KazeiKakaku = new NacCollumnN(13);
        /// <summary>
        /// 関税課税入力有表示 51
        /// </summary>
        public NacCollumnFLG KazeiInputFlg = new NacCollumnFLG(1);
        /// <summary>
        /// 数量２ 52
        /// </summary>
        public NacCollumnN Suryo2 = new NacCollumnN(12);
        /// <summary>
        /// 数量単位コード２ 53
        /// </summary>
        public NacCollumnAN Suryo2TaniCD = new NacCollumnAN(4);
        /// <summary>
        /// 関税率区分コード 54
        /// </summary>
        public NacCollumnAN KanzeiRitsuKBNCode = new NacCollumnAN(1);
        /// <summary>
        /// 関税率 55
        /// </summary>
        public NacCollumnAN KanzeiRitsu = new NacCollumnAN(25);
        /// <summary>
        /// 関税課税標準数量 56
        /// </summary>
        public NacCollumnN KazeiHyojunNum = new NacCollumnN(12);
        /// <summary>
        /// 関税課税標準数量単位コード57
        /// </summary>
        public NacCollumnAN KazeiHyojunNumTani = new NacCollumnAN(4);        
        /// <summary>
        /// 関税額58
        /// </summary>
        public NacCollumnN KanzeiGaku = new NacCollumnN(11);        
        /// <summary>
        /// 原産地コード 59
        /// </summary>
        public NacCollumnAN GensanchiCD = new NacCollumnAN(2);
        /// <summary>
        /// 原産地名60
        /// </summary>
        public NacCollumnAN GensanchiName = new NacCollumnAN(7);
        /// <summary>
        /// 原産地証明書識別 61
        /// </summary>
        public NacCollumnAN GensanchiType = new NacCollumnAN(4);
        /// <summary>
        /// 関税免除額62
        /// </summary>
        public NacCollumnN KanzeiMenjoGaku = new NacCollumnN(11);
        /// <summary>
        /// INN等対象識別 63
        /// </summary>
        public NacCollumnAN INNTaishoType = new NacCollumnAN(1);

        //public NaikokuShohizeiModel[] Shohizei = new NaikokuShohizeiModel[3];

        public int NAIKOKU_SHOHIZEI_MAX = 3;
        public List<NaikokuShohizeiModel> ShohizeiList = new List<NaikokuShohizeiModel>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ShohinRecvModel()
        {
            for (int i = 0; i < NAIKOKU_SHOHIZEI_MAX; i++)
            {
                NaikokuShohizeiModel shohizei = new NaikokuShohizeiModel();
                this.ShohizeiList.Add(shohizei);
            }
        }

        ///// <summary>
        ///// 内容のセット
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public bool SetData(object obj)
        //{
        //    return true;
        //}
        /// <summary>
        /// バイト長
        /// </summary>
        /// <returns></returns>
        public int GetByteLength()
        {
            int intlen = 0;

            intlen += RanNo.GetByteLength();
            intlen += RanNoToGoSaki.GetByteLength();
            intlen += KanzeiKazeiHyoji.GetByteLength();
            intlen += KanzeiMenjoHyoji.GetByteLength();
            intlen += ShohinKannriCD.GetByteLength();
            intlen += HinmokuCD.GetByteLength();
            intlen += NaccsCD.GetByteLength();
            intlen += ShohinName.GetByteLength();
            intlen += HanbaiTanka.GetByteLength();
            intlen += HanbaiSuryo.GetByteLength();
            intlen += ZeiHyoNo.GetByteLength();
            intlen += Suryo1.GetByteLength();
            intlen += Suryo1TaniCD.GetByteLength();
            intlen += KazeiKakaku.GetByteLength();
            intlen += KazeiInputFlg.GetByteLength();
            intlen += Suryo2.GetByteLength();
            intlen += Suryo2TaniCD.GetByteLength();
            intlen += KanzeiRitsuKBNCode.GetByteLength();
            intlen += KanzeiRitsu.GetByteLength();
            intlen += KazeiHyojunNum.GetByteLength();
            intlen += KazeiHyojunNumTani.GetByteLength();
            intlen += KanzeiGaku.GetByteLength();
            intlen += GensanchiCD.GetByteLength();
            intlen += GensanchiName.GetByteLength();
            intlen += GensanchiType.GetByteLength();
            intlen += KanzeiMenjoGaku.GetByteLength();
            intlen += INNTaishoType.GetByteLength();

            for (int i = 0; i < NAIKOKU_SHOHIZEI_MAX; i++)
            {
                intlen += this.ShohizeiList[i].GetByteLength();
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
            int pos = 0;
            try
            {
                Array.Copy(this.RanNo.GetByteData(), 0, btData, pos, this.RanNo.GetByteLength());
                pos += RanNo.GetByteLength();
                Array.Copy(this.RanNoToGoSaki.GetByteData(), 0, btData, pos, this.RanNoToGoSaki.GetByteLength());
                pos += RanNoToGoSaki.GetByteLength();
                Array.Copy(this.KanzeiKazeiHyoji.GetByteData(), 0, btData, pos, this.KanzeiKazeiHyoji.GetByteLength());
                pos += KanzeiKazeiHyoji.GetByteLength();
                Array.Copy(this.KanzeiMenjoHyoji.GetByteData(), 0, btData, pos, this.KanzeiMenjoHyoji.GetByteLength());
                pos += KanzeiMenjoHyoji.GetByteLength();
                Array.Copy(this.ShohinKannriCD.GetByteData(), 0, btData, pos, this.ShohinKannriCD.GetByteLength());
                pos += ShohinKannriCD.GetByteLength();
                Array.Copy(this.HinmokuCD.GetByteData(), 0, btData, pos, this.HinmokuCD.GetByteLength());
                pos += HinmokuCD.GetByteLength();
                Array.Copy(this.NaccsCD.GetByteData(), 0, btData, pos, this.NaccsCD.GetByteLength());
                pos += NaccsCD.GetByteLength();
                Array.Copy(this.ShohinName.GetByteData(), 0, btData, pos, this.ShohinName.GetByteLength());
                pos += ShohinName.GetByteLength();
                Array.Copy(this.HanbaiTanka.GetByteData(), 0, btData, pos, this.HanbaiTanka.GetByteLength());
                pos += HanbaiTanka.GetByteLength();
                Array.Copy(this.HanbaiSuryo.GetByteData(), 0, btData, pos, this.HanbaiSuryo.GetByteLength());
                pos += HanbaiSuryo.GetByteLength();
                Array.Copy(this.ZeiHyoNo.GetByteData(), 0, btData, pos, this.ZeiHyoNo.GetByteLength());
                pos += ZeiHyoNo.GetByteLength();
                Array.Copy(this.Suryo1.GetByteData(), 0, btData, pos, this.Suryo1.GetByteLength());
                pos += Suryo1.GetByteLength();
                Array.Copy(this.Suryo1TaniCD.GetByteData(), 0, btData, pos, this.Suryo1TaniCD.GetByteLength());
                pos += Suryo1TaniCD.GetByteLength();
                Array.Copy(this.KazeiKakaku.GetByteData(), 0, btData, pos, this.KazeiKakaku.GetByteLength());
                pos += KazeiKakaku.GetByteLength();
                Array.Copy(this.KazeiInputFlg.GetByteData(), 0, btData, pos, this.KazeiInputFlg.GetByteLength());
                pos += KazeiInputFlg.GetByteLength();
                Array.Copy(this.Suryo2.GetByteData(), 0, btData, pos, this.Suryo2.GetByteLength());
                pos += Suryo2.GetByteLength();
                Array.Copy(this.Suryo2TaniCD.GetByteData(), 0, btData, pos, this.Suryo2TaniCD.GetByteLength());
                pos += Suryo2TaniCD.GetByteLength();
                Array.Copy(this.KanzeiRitsuKBNCode.GetByteData(), 0, btData, pos, this.KanzeiRitsuKBNCode.GetByteLength());
                pos += KanzeiRitsuKBNCode.GetByteLength();
                Array.Copy(this.KanzeiRitsu.GetByteData(), 0, btData, pos, this.KanzeiRitsu.GetByteLength());
                pos += KanzeiRitsu.GetByteLength();
                Array.Copy(this.KazeiHyojunNum.GetByteData(), 0, btData, pos, this.KazeiHyojunNum.GetByteLength());
                pos += KazeiHyojunNum.GetByteLength();
                Array.Copy(this.KazeiHyojunNumTani.GetByteData(), 0, btData, pos, this.KazeiHyojunNumTani.GetByteLength());
                pos += KazeiHyojunNumTani.GetByteLength();
                Array.Copy(this.KanzeiGaku.GetByteData(), 0, btData, pos, this.KanzeiGaku.GetByteLength());
                pos += KanzeiGaku.GetByteLength();
                Array.Copy(this.GensanchiCD.GetByteData(), 0, btData, pos, this.GensanchiCD.GetByteLength());
                pos += GensanchiCD.GetByteLength();
                Array.Copy(this.GensanchiName.GetByteData(), 0, btData, pos, this.GensanchiName.GetByteLength());
                pos += GensanchiName.GetByteLength();
                Array.Copy(this.GensanchiType.GetByteData(), 0, btData, pos, this.GensanchiType.GetByteLength());
                pos += GensanchiType.GetByteLength();
                Array.Copy(this.KanzeiMenjoGaku.GetByteData(), 0, btData, pos, this.KanzeiMenjoGaku.GetByteLength());
                pos += KanzeiMenjoGaku.GetByteLength();
                Array.Copy(this.INNTaishoType.GetByteData(), 0, btData, pos, this.INNTaishoType.GetByteLength());
                pos += INNTaishoType.GetByteLength();

                for (int i = 0; i < NAIKOKU_SHOHIZEI_MAX; i++)
                {
                    Array.Copy(this.ShohizeiList[i].GetByteData(), 0, btData, pos, this.ShohizeiList[i].GetByteLength());
                    pos += this.ShohizeiList[i].GetByteLength();
                }
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
                    this.RanNo.SetData(EUC.GetString(btData, pos, this.RanNo.GetByteLength()));
                    pos += RanNo.GetByteLength();

                    this.RanNoToGoSaki.SetData(EUC.GetString(btData, pos, this.RanNoToGoSaki.GetByteLength()));
                    pos += RanNoToGoSaki.GetByteLength();

                    this.KanzeiKazeiHyoji.SetData(EUC.GetString(btData, pos, this.KanzeiKazeiHyoji.GetByteLength()));
                    pos += KanzeiKazeiHyoji.GetByteLength();

                    this.KanzeiMenjoHyoji.SetData(EUC.GetString(btData, pos, this.KanzeiMenjoHyoji.GetByteLength()));
                    pos += KanzeiMenjoHyoji.GetByteLength();

                    this.ShohinKannriCD.SetData(EUC.GetString(btData, pos, this.ShohinKannriCD.GetByteLength()));
                    pos += ShohinKannriCD.GetByteLength();

                    this.HinmokuCD.SetData(EUC.GetString(btData, pos, this.HinmokuCD.GetByteLength()));
                    pos += HinmokuCD.GetByteLength();

                    this.NaccsCD.SetData(EUC.GetString(btData, pos, this.NaccsCD.GetByteLength()));
                    pos += NaccsCD.GetByteLength();

                    this.ShohinName.SetData(EUC.GetString(btData, pos, this.ShohinName.GetByteLength()));
                    pos += ShohinName.GetByteLength();

                    this.HanbaiTanka.SetData(EUC.GetString(btData, pos, this.HanbaiTanka.GetByteLength()));
                    pos += HanbaiTanka.GetByteLength();

                    this.HanbaiSuryo.SetData(EUC.GetString(btData, pos, this.HanbaiSuryo.GetByteLength()));
                    pos += HanbaiSuryo.GetByteLength();

                    this.ZeiHyoNo.SetData(EUC.GetString(btData, pos, this.ZeiHyoNo.GetByteLength()));
                    pos += ZeiHyoNo.GetByteLength();

                    this.Suryo1.SetData(EUC.GetString(btData, pos, this.Suryo1.GetByteLength()));
                    pos += Suryo1.GetByteLength();

                    this.Suryo1TaniCD.SetData(EUC.GetString(btData, pos, this.Suryo1TaniCD.GetByteLength()));
                    pos += Suryo1TaniCD.GetByteLength();

                    this.KazeiKakaku.SetData(EUC.GetString(btData, pos, this.KazeiKakaku.GetByteLength()));
                    pos += KazeiKakaku.GetByteLength();

                    this.KazeiInputFlg.SetData(EUC.GetString(btData, pos, this.KazeiInputFlg.GetByteLength()));
                    pos += KazeiInputFlg.GetByteLength();

                    this.Suryo2.SetData(EUC.GetString(btData, pos, this.Suryo2.GetByteLength()));
                    pos += Suryo2.GetByteLength();

                    this.Suryo2TaniCD.SetData(EUC.GetString(btData, pos, this.Suryo2TaniCD.GetByteLength()));
                    pos += Suryo2TaniCD.GetByteLength();

                    this.KanzeiRitsuKBNCode.SetData(EUC.GetString(btData, pos, this.KanzeiRitsuKBNCode.GetByteLength()));
                    pos += KanzeiRitsuKBNCode.GetByteLength();

                    this.KanzeiRitsu.SetData(EUC.GetString(btData, pos, this.KanzeiRitsu.GetByteLength()));
                    pos += KanzeiRitsu.GetByteLength();

                    this.KazeiHyojunNum.SetData(EUC.GetString(btData, pos, this.KazeiHyojunNum.GetByteLength()));
                    pos += KazeiHyojunNum.GetByteLength();

                    this.KazeiHyojunNumTani.SetData(EUC.GetString(btData, pos, this.KazeiHyojunNumTani.GetByteLength()));
                    pos += KazeiHyojunNumTani.GetByteLength();

                    this.KanzeiGaku.SetData(EUC.GetString(btData, pos, this.KanzeiGaku.GetByteLength()));
                    pos += KanzeiGaku.GetByteLength();

                    this.GensanchiCD.SetData(EUC.GetString(btData, pos, this.GensanchiCD.GetByteLength()));
                    pos += GensanchiCD.GetByteLength();

                    this.GensanchiName.SetData(EUC.GetString(btData, pos, this.GensanchiName.GetByteLength()));
                    pos += GensanchiName.GetByteLength();

                    this.GensanchiType.SetData(EUC.GetString(btData, pos, this.GensanchiType.GetByteLength()));
                    pos += GensanchiType.GetByteLength();

                    this.KanzeiMenjoGaku.SetData(EUC.GetString(btData, pos, this.KanzeiMenjoGaku.GetByteLength()));
                    pos += KanzeiMenjoGaku.GetByteLength();

                    this.INNTaishoType.SetData(EUC.GetString(btData, pos, this.INNTaishoType.GetByteLength()));
                    pos += INNTaishoType.GetByteLength();

                    for (int i = 0; i < NAIKOKU_SHOHIZEI_MAX; i++)
                    {
                        this.ShohizeiList[i].SetByteData(btData.Skip(pos).Take(this.ShohizeiList[i].GetByteLength()).ToArray());
                        pos += ShohizeiList[i].GetByteLength();
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
        //public string GetFileData()
        //{
        //    StringBuilder str = new StringBuilder();

        //    try
        //    {

        //        str.AppendLine(RanNo.Data);
        //        str.AppendLine(RanNoToGoSaki.Data);
        //        str.AppendLine(KanzeiKazeiHyoji.Data);
        //        str.AppendLine(KanzeiMenjoHyoji.Data);
        //        str.AppendLine(ShohinKannriCD.Data);
        //        str.AppendLine(HinmokuCD.Data);
        //        str.AppendLine(NaccsCD.Data);
        //        str.AppendLine(ShohinName.Data);
        //        str.AppendLine(HanbaiTanka.Data);
        //        str.AppendLine(HanbaiSuryo.Data);
        //        str.AppendLine(ZeiHyoNo.Data);
        //        str.AppendLine(Suryo1.Data);
        //        str.AppendLine(Suryo1TaniCD.Data);
        //        str.AppendLine(KazeiKakaku.Data);
        //        str.AppendLine(KazeiInputFlg.Data);
        //        str.AppendLine(Suryo2.Data);
        //        str.AppendLine(Suryo2TaniCD.Data);
        //        str.AppendLine(KanzeiRitsuKBNCode.Data);
        //        str.AppendLine(KanzeiRitsu.Data);
        //        str.AppendLine(KazeiHyojunNum.Data);
        //        str.AppendLine(KazeiHyojunNumTani.Data);
        //        str.AppendLine(KanzeiGaku.Data);
        //        str.AppendLine(GensanchiCD.Data);
        //        str.AppendLine(GensanchiName.Data);
        //        str.AppendLine(GensanchiType.Data);
        //        str.AppendLine(KanzeiMenjoGaku.Data);
        //        str.AppendLine(INNTaishoType.Data);

        //        for (int i = 0; i < NAIKOKU_SHOHIZEI_MAX; i++)
        //        {
        //            this.ShohizeiList[i].GetFileData();
        //        }

        //        return str.ToString();
        //    }
        //    catch
        //    {
        //        return "ShohinRecvModel Error";
        //    }
        //}
    }

    #endregion "商品モデル　NACCS"

    #region "内国消費税"

    /// <summary>
    /// 内国消費税モデル
    /// </summary>
    public class NaikokuShohizeiModel : NaccsModel
    {
        /// <summary>
        /// 内国消費税等科目名  64
        /// </summary>
        public NacCollumnJJ ShohizeiKamokuCD = new NacCollumnJJ(22);
        /// <summary>
        /// 内国消費税等課税標準数量 65
        /// </summary>
        public NacCollumnN ShohizeiKazeiNum = new NacCollumnN(12);
        /// <summary>
        /// 内国消費税等課税標準数量単位コード  66
        /// </summary>
        public NacCollumnAN ShohizeiKazeiNumTani = new NacCollumnAN(4);
        /// <summary>
        /// 内国消費税等種別 67
        /// </summary>
        public NacCollumnAN ShohizeiTypeCD = new NacCollumnAN(10);
        /// <summary>
        /// 内国消費税等税率 68
        /// </summary>
        public NacCollumnAN ShohizeiZeiRitsu = new NacCollumnAN(25);
        /// <summary>
        /// 内国消費税等課税価格 69
        /// </summary>
        public NacCollumnN ShohizeiKazeiKakaku = new NacCollumnN(13);
        /// <summary>
        /// 内国消費税等税額 70
        /// </summary>
        public NacCollumnN ShohizeiZeigaku = new NacCollumnN(11);

        /// <summary>
        /// バイト長
        /// </summary>
        /// <returns></returns>
        public int GetByteLength()
        {
            int intlen = 0;

            intlen += ShohizeiKamokuCD.GetByteLength();
            intlen += ShohizeiKazeiNum.GetByteLength();
            intlen += ShohizeiKazeiNumTani.GetByteLength();
            intlen += ShohizeiTypeCD.GetByteLength();
            intlen += ShohizeiZeiRitsu.GetByteLength();
            intlen += ShohizeiKazeiKakaku.GetByteLength();
            intlen += ShohizeiZeigaku.GetByteLength();

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
                Array.Copy(this.ShohizeiKamokuCD.GetByteData(), 0, btData, pos, this.ShohizeiKamokuCD.GetByteLength());
                pos += ShohizeiKamokuCD.GetByteLength();
                Array.Copy(this.ShohizeiKazeiNum.GetByteData(), 0, btData, pos, this.ShohizeiKazeiNum.GetByteLength());
                pos += ShohizeiKazeiNum.GetByteLength();
                Array.Copy(this.ShohizeiKazeiNumTani.GetByteData(), 0, btData, pos, this.ShohizeiKazeiNumTani.GetByteLength());
                pos += ShohizeiKazeiNumTani.GetByteLength();
                Array.Copy(this.ShohizeiTypeCD.GetByteData(), 0, btData, pos, this.ShohizeiTypeCD.GetByteLength());
                pos += ShohizeiTypeCD.GetByteLength();
                Array.Copy(this.ShohizeiZeiRitsu.GetByteData(), 0, btData, pos, this.ShohizeiZeiRitsu.GetByteLength());
                pos += ShohizeiZeiRitsu.GetByteLength();
                Array.Copy(this.ShohizeiKazeiKakaku.GetByteData(), 0, btData, pos, this.ShohizeiKazeiKakaku.GetByteLength());
                pos += ShohizeiKazeiKakaku.GetByteLength();
                Array.Copy(this.ShohizeiZeigaku.GetByteData(), 0, btData, pos, this.ShohizeiZeigaku.GetByteLength());
                pos += ShohizeiZeigaku.GetByteLength();

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
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding(51932);

            try
            {
                if (btData.Length >= intLast)
                {
                    this.ShohizeiKamokuCD.SetData(EUC.GetString(btData, pos, this.ShohizeiKamokuCD.GetByteLength()));
                    pos += this.ShohizeiKamokuCD.GetByteLength();

                    this.ShohizeiKazeiNum.SetData(EUC.GetString(btData, pos, this.ShohizeiKazeiNum.GetByteLength()));
                    pos += this.ShohizeiKazeiNum.GetByteLength();

                    this.ShohizeiKazeiNumTani.SetData(EUC.GetString(btData, pos, this.ShohizeiKazeiNumTani.GetByteLength()));
                    pos += this.ShohizeiKazeiNumTani.GetByteLength();

                    this.ShohizeiTypeCD.SetData(EUC.GetString(btData, pos, this.ShohizeiTypeCD.GetByteLength()));
                    pos += this.ShohizeiTypeCD.GetByteLength();

                    this.ShohizeiZeiRitsu.SetData(EUC.GetString(btData, pos, this.ShohizeiZeiRitsu.GetByteLength()));
                    pos += this.ShohizeiZeiRitsu.GetByteLength();

                    this.ShohizeiKazeiKakaku.SetData(EUC.GetString(btData, pos, this.ShohizeiKazeiKakaku.GetByteLength()));
                    pos += this.ShohizeiKazeiKakaku.GetByteLength();

                    this.ShohizeiZeigaku.SetData(EUC.GetString(btData, pos, this.ShohizeiZeigaku.GetByteLength()));
                    pos += this.ShohizeiZeigaku.GetByteLength();

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
        ///// <summary>
        ///// ファイル用のデータ
        ///// </summary>
        ///// <returns></returns>
        //public string GetFileData()
        //{
        //    StringBuilder str = new StringBuilder();
        //    try
        //    {
        //        str.AppendLine(ShohizeiKamokuCD.Data);
        //        str.AppendLine(ShohizeiKazeiNum.Data);
        //        str.AppendLine(ShohizeiKazeiNumTani.Data);
        //        str.AppendLine(ShohizeiTypeCD.Data);
        //        str.AppendLine(ShohizeiZeiRitsu.Data);
        //        str.AppendLine(ShohizeiKazeiKakaku.Data);
        //        str.AppendLine(ShohizeiZeigaku.Data);

        //        return str.ToString();
        //    }
        //    catch
        //    {
        //        return "NaikokuShohizeiModel Error";
        //    }
        //}
    }

    #endregion "内国消費税"

}
