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
using System.Configuration;
//using System.Reflection.MethodBase;
//using JoySock.Common;
using JoySmtp.Data;//.HPFData;
using JoySmtp.CLogOut;//.HPFLogOut;
using JoySmtp.DataBase;

namespace JoySmtp.SeqBarCode
{
    //public class SeqBarCode
    //{
    //    protected HPFCommon i_Common = null/* TODO Change to default(_) if this is not a reference type */;

    //    private string i_MaxSeq = string.Empty;
    //    private decimal i_MaxDec = 0;
    //    private string i_Seq_Kbn = string.Empty;
    //    private string i_Seq_Pre_Char = string.Empty;
    //    private string i_Nendo_Reset_Flg = string.Empty;
    //    private string i_Nendo = string.Empty;
    //    private bool i_Insert_Flg = false;
    //    private decimal i_Count = 0;
    //    private DataTable i_dtBarCode = new DataTable();
    //    private decimal i_SEQ_NUM_MAX = 0;
    //    private decimal i_SEQ_LENGH_MAX = 0;
    //    private string i_HAITA_FLG = "0";
    //    private string i_Name = string.Empty;
    //    private bool i_LockFlg = false;


    //    /// <summary>
    //    ///         ''' 最大SEQを返却する
    //    ///         ''' </summary>
    //    ///         ''' <value></value>
    //    ///         ''' <returns></returns>
    //    ///         ''' <remarks></remarks>
    //    public string MaxSeq
    //    {
    //        get
    //        {
    //            return i_MaxSeq;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' 最大SEQを返却する
    //    ///         ''' </summary>
    //    ///         ''' <value></value>
    //    ///         ''' <returns></returns>
    //    ///         ''' <remarks></remarks>
    //    public string MaxDec
    //    {
    //        get
    //        {
    //            return (i_Count == 0?1:i_Count).ToString();
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' 親画面からパラメータ情報を取得します
    //    ///         ''' </summary>
    //    ///         ''' <value></value>
    //    ///         ''' <remarks></remarks>
    //    public decimal SetCount
    //    {
    //        set
    //        {
    //            i_Count += value;
    //        }
    //    }



    //    /// <summary>
    //    ///         ''' ニュー
    //    ///         ''' </summary>
    //    ///         ''' <param name="strSeqKbn"></param>
    //    ///         ''' <param name="strSeqPreChar"></param>
    //    ///         ''' <param name="strNendoResetFlg"></param>
    //    ///         ''' <param name="strNendo"></param>
    //    ///         ''' <remarks></remarks>
    //    public SeqBarCode(string strSeqKbn, string strSeqPreChar = "", string strNendoResetFlg = "0", string strNendo = "")
    //    {
    //        try
    //        {
    //            i_Seq_Kbn = strSeqKbn;
    //            i_Seq_Pre_Char = strSeqPreChar;
    //            i_Nendo_Reset_Flg = strNendoResetFlg;
    //            i_Nendo = strNendo;

    //            // 共通クラスの初期化
    //            i_Common = new HPFCommon();

    //            switch (i_Seq_Kbn)
    //            {
    //                case HPFData.SEQ_KBN_SHIP_BCD:
    //                case HPFData.SEQ_KBN_NAME:
    //                case HPFData.SEQ_KBN_TANA_BCD:
    //                case HPFData.SEQ_KBN_PICK:
    //                case HPFData.SEQ_KBN_NOHIN:
    //                case HPFData.SEQ_KBN_FRYO:
    //                case HPFData.SEQ_KBN_HENPIN:
    //                case HPFData.SEQ_KBN_WMS_IDO:
    //                case HPFData.SEQ_KBN_HINBAN:
    //                case HPFData.SEQ_KBN_NH_JK_PTN:
    //                case HPFData.SEQ_KBN_ZAIKOTS:
    //                case HPFData.SEQ_KBN_TANAOROSHI:
    //                case HPFData.SEQ_KBN_UKEBARAI:
    //                case HPFData.SEQ_KBN_SHIPK:
    //                case HPFData.SEQ_KBN_FRYO_SR:
    //                case HPFData.SEQ_KBN_MITUMORI:
    //                case HPFData.SEQ_KBN_TOIAWASE:
    //                case HPFData.SEQ_KBN_CARD:
    //                case HPFData.SEQ_KBN_URIAGE:
    //                case HPFData.SEQ_KBN_SHIRE:
    //                case HPFData.SEQ_KBN_SHIP:
    //                case HPFData.SEQ_KBN_IDO:
    //                case HPFData.SEQ_KBN_TOKUTEI:
    //                case HPFData.SEQ_KBN_EDI:
    //                case HPFData.SEQ_KBN_UKEIRE:
    //                case HPFData.SEQ_KBN_OSHIRASE:
    //                case HPFData.SEQ_KBN_URIKAKE:
    //                case HPFData.SEQ_KBN_KAIKAKE:
    //                case HPFData.SEQ_KBN_NYUKIN:
    //                case HPFData.SEQ_KBN_SHIHARAI:
    //                case HPFData.SEQ_KBN_SHIWAKE:
    //                case HPFData.SEQ_KBN_SEIKYU:
    //                case HPFData.SEQ_KBN_SHIHARAIS:
    //                case HPFData.SEQ_KBN_ARZANDAKA:
    //                case HPFData.SEQ_KBN_APZANDAKA:
    //                case HPFData.SEQ_KBN_MSHIWAKE:
    //                case HPFData.SEQ_KBN_CHOSEINO:
    //                case HPFData.SEQ_KBN_MDKANRENNO:
    //                case HPFData.SEQ_KBN_JUCHU:
    //                case HPFData.SEQ_KBN_TENJIKAI:
    //                case HPFData.SEQ_KBN_SHOHIZEI_URI:
    //                case HPFData.SEQ_KBN_SHOHIZEI_KAI:
    //                    {
    //                        SearchMaxBarCode();
    //                        i_MaxSeq = i_Count.ToString().PadLeft(i_SEQ_LENGH_MAX.ToString().Length, '0');
    //                        break;
    //                    }

    //                default:
    //                    {
    //                        if (Strings.Left(i_Seq_Kbn, 4) == HPFData.SEQ_KBN_SHUK)
    //                        {
    //                            if (i_Seq_Kbn.Length != 10)
    //                                return;
    //                            SearchMaxBarCodeForShipping();
    //                            i_MaxSeq = i_Seq_Kbn.Substring(4) + i_Count.ToString().PadLeft(3, '0');
    //                        }

    //                        break;
    //                    }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            // エラー処理
    //            HPFLogOut.ErrorOut(ex.Message, "HPFGetBarCode", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' 排他
    //    ///         ''' </summary>
    //    ///         ''' <returns></returns>
    //    ///         ''' <remarks></remarks>
    //    public bool CheckHaita()
    //    {
    //        try
    //        {

    //            // If Not i_HAITA_FLG.Equals("0") AndAlso Not i_HAITA_FLG.Equals(String.Empty) Then
    //            // i_Common.ShowMessageInfo(ERROR_CODE_I009, i_Name)
    //            // Return False
    //            // End If

    //            // 'データロック 
    //            // DoLock_Data()
    //            // i_LockFlg = True
    //            // Return True

    //            // Dim intLoopCount As Integer = i_Common.GetSysCtrlInfo("CheckHaitaLoopSeconds")
    //            // Dim intCnt As Integer = 0
    //            // Dim CsdCommon As New CsdCommon

    //            // Do While i_HAITA_FLG.Equals("1") And intLoopCount > intCnt
    //            // 'System.Threading.Thread.Sleep(4000)

    //            // CsdCommon.DoLoop(1)

    //            // SearchMaxBarCode()

    //            // intCnt += 1
    //            // Loop

    //            // If Not i_HAITA_FLG.Equals("0") AndAlso Not i_HAITA_FLG.Equals(String.Empty) Then
    //            // i_Common.ShowMessageInfo(ERROR_CODE_I009, i_Name)
    //            // Return False
    //            // End If

    //            // 'データロック 
    //            // DoLock_Data()
    //            // i_LockFlg = True
    //            // Return True

    //            // Dim intLoopCount As Integer = i_Common.GetSysCtrlInfo("CheckHaitaLoopSeconds")
    //            int intLoopCount = 120;
    //            CsdCommon CsdCommon = new CsdCommon();
    //            bool blnLock = false;
    //            int intCnt = 0;
    //            while (!blnLock | intLoopCount < intCnt)
    //            {

    //                // ロック処理
    //                if (DoLock_Data_New())
    //                {
    //                    blnLock = true;
    //                    break;
    //                }

    //                // ロック失敗時１秒待つ
    //                if (blnLock == false)
    //                    CsdCommon.DoLoop(1);

    //                intCnt += 1;
    //            }


    //            if (blnLock == false)
    //            {
    //                // ロック失敗時
    //                SearchMaxBarCode();
    //                if (!i_HAITA_FLG.Equals("0") && !i_HAITA_FLG.Equals(string.Empty))
    //                    i_Common.ShowMessageInfo(HPFData.ERROR_CODE_I009, i_Name);
    //                else
    //                    i_Common.ShowMessageInfo(HPFData.ERROR_CODE_I998, "再度、実行して下さい。");
    //            }
    //            else
    //            {
    //                SearchMaxBarCode();
    //                i_LockFlg = true;
    //            }


    //            return blnLock;
    //        }
    //        catch (Exception ex)
    //        {
    //            // エラー処理
    //            HPFLogOut.ErrorOut(ex.Message, "HPFGetBarCode", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' データロック 
    //    ///         ''' </summary>
    //    ///         ''' <param name="blnLockFlg"></param>
    //    ///         ''' <remarks></remarks>
    //    private void DoLock_Data(bool blnLockFlg = false)
    //    {
    //        HPFDataBaseSQL objDb = null/* TODO Change to default(_) if this is not a reference type */;
    //        try
    //        {
    //            objDb = new HPFDataBaseSQL();
    //            objDb.DBLogIn();
    //            objDb.BeginTransaction();

    //            // T見積Hデータロックを解除する
    //            objDb.ExecuteNonQuery(GetSql_Lock_C_SEQ(blnLockFlg), true);

    //            objDb.Commit();
    //        }
    //        catch (Exception ex)
    //        {
    //            objDb.Rollback();
    //            HPFLogOut.ErrorOut(ex.Message, "HPFGetBarCode", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //        finally
    //        {
    //            objDb.DBLogOut();
    //            objDb = null/* TODO Change to default(_) if this is not a reference type */;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' データロック 
    //    ///         ''' </summary>
    //    ///         ''' <param name="blnLockFlg"></param>
    //    ///         ''' <remarks></remarks>
    //    private bool DoLock_Data_New(bool blnLockFlg = false)
    //    {
    //        HPFDataBaseSQL objDb = null/* TODO Change to default(_) if this is not a reference type */;
    //        int intRes = -1;
    //        try
    //        {
    //            objDb = new HPFDataBaseSQL();
    //            objDb.DBLogIn();
    //            objDb.BeginTransaction();

    //            intRes = objDb.ExecuteNonQuery(GetSql_Lock_C_SEQ_New(blnLockFlg), true);

    //            if (intRes <= 0)
    //            {
    //                objDb.Rollback();
    //                return false;
    //            }

    //            objDb.Commit();
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            objDb.Rollback();
    //            HPFLogOut.ErrorOut(ex.Message, "HPFGetBarCode", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //        finally
    //        {
    //            objDb.DBLogOut();
    //            objDb = null/* TODO Change to default(_) if this is not a reference type */;
    //        }
    //    }


    //    /// <summary>
    //    ///         ''' SEQを取得する
    //    ///         ''' </summary>
    //    ///         ''' <returns></returns>
    //    ///         ''' <remarks></remarks>
    //    public string GetSeq(bool bnlCountFlg = false)
    //    {
    //        try
    //        {
    //            if (bnlCountFlg == true)
    //            {
    //                i_Count += 1;
    //                if (i_Count > i_SEQ_NUM_MAX)
    //                    i_Count = 1;
    //            }
    //            if (Strings.Left(i_Seq_Kbn, 4) == HPFData.SEQ_KBN_SHUK)
    //                return i_Seq_Kbn.Substring(4) + i_Count.ToString().PadLeft(i_SEQ_NUM_MAX.ToString().Length, '0');
    //            else
    //                return i_Count.ToString().PadLeft(i_SEQ_NUM_MAX.ToString().Length, '0');
    //        }
    //        catch (Exception ex)
    //        {
    //            // エラー処理
    //            HPFLogOut.ErrorOut(ex.Message, "HPFGetBarCode", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' SEQを取得する
    //    ///         ''' </summary>
    //    ///         ''' <returns></returns>
    //    ///         ''' <remarks></remarks>
    //    public string GetNo()
    //    {
    //        try
    //        {
    //            i_Count += 1;
    //            if (i_Count > i_SEQ_NUM_MAX)
    //                i_Count = 1;
    //            if (Strings.Left(i_Seq_Kbn, 4) == HPFData.SEQ_KBN_SHUK)
    //                return i_Seq_Kbn.Substring(4) + i_Count.ToString().PadLeft(i_SEQ_LENGH_MAX.ToString().Length, '0');
    //            else if (i_Seq_Kbn == HPFData.SEQ_KBN_WMS_IDO)
    //                return i_Count.ToString().PadLeft(i_SEQ_LENGH_MAX.ToString().Length, '0') + HPFData.SEQ_WMS_IDO_KBN;
    //            else if (i_Seq_Kbn == HPFData.SEQ_KBN_NOHIN)
    //                return i_Count.ToString().PadLeft(i_SEQ_LENGH_MAX.ToString().Length, '0') + HPFData.SEQ_WMS_NOHINSHO_KBN;
    //            else
    //                return i_Count.ToString().PadLeft(i_SEQ_LENGH_MAX.ToString().Length, '0');
    //        }
    //        catch (Exception ex)
    //        {
    //            // エラー処理
    //            HPFLogOut.ErrorOut(ex.Message, "HPFGetBarCode", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' バーコードを取得する
    //    ///         ''' </summary>
    //    ///         ''' <param name="strSeq">SEQ</param>
    //    ///         ''' <param name="strCenterID">出荷No(出荷カンバン用)ＢＣＤのｾﾝﾀｰID１桁（1：板橋 2：deco 3：OTS)</param>
    //    ///         ''' <returns></returns>
    //    ///         ''' <remarks></remarks>
    //    public string GetNewBarCode(string strSeq, string strCenterID = "1")
    //    {
    //        try
    //        {
    //            string retBarCode = string.Empty;
    //            switch (i_Seq_Kbn)
    //            {
    //                case HPFData.SEQ_KBN_SHIP_BCD:
    //                    {
    //                        retBarCode = HPFData.SEQ_PRE_CHAR_SHIP_BCD;
    //                        break;
    //                    }

    //                case HPFData.SEQ_KBN_NAME:
    //                    {
    //                        retBarCode = HPFData.SEQ_PRE_CHAR_NAME;
    //                        break;
    //                    }

    //                case HPFData.SEQ_KBN_TANA_BCD:
    //                    {
    //                        retBarCode = HPFData.SEQ_PRE_CHAR_TANA_BCD;
    //                        break;
    //                    }

    //                case HPFData.SEQ_KBN_PICK:
    //                    {
    //                        retBarCode = HPFData.SEQ_PRE_CHAR_PICK;
    //                        break;
    //                    }

    //                case HPFData.SEQ_KBN_NOHIN:
    //                    {
    //                        retBarCode = HPFData.SEQ_PRE_CHAR_NOHIN;
    //                        break;
    //                    }

    //                case HPFData.SEQ_KBN_FRYO:
    //                    {
    //                        retBarCode = HPFData.SEQ_PRE_CHAR_FURYO;
    //                        break;
    //                    }

    //                case HPFData.SEQ_KBN_HENPIN:
    //                    {
    //                        retBarCode = HPFData.SEQ_PRE_CHAR_HENPIN;
    //                        break;
    //                    }

    //                case HPFData.SEQ_KBN_WMS_IDO:
    //                    {
    //                        retBarCode = HPFData.SEQ_PRE_CHAR_WMS_IDO;
    //                        break;
    //                    }

    //                case HPFData.SEQ_KBN_ZAIKOTS:
    //                    {
    //                        retBarCode = HPFData.SEQ_PRE_CHAR_ZAIKOTS;
    //                        break;
    //                    }

    //                case HPFData.SEQ_KBN_UKEIRE:
    //                    {
    //                        retBarCode = HPFData.SEQ_PRE_CHAR_UKEIRE_NO;
    //                        break;
    //                    }

    //                default:
    //                    {
    //                        if (Strings.Left(i_Seq_Kbn, 4) == HPFData.SEQ_KBN_SHUK)
    //                            retBarCode = HPFData.SEQ_PRE_CHAR_SHUK + strCenterID;
    //                        break;
    //                    }
    //            }
    //            if (i_Seq_Kbn == HPFData.SEQ_KBN_WMS_IDO)
    //                retBarCode = retBarCode + strSeq + HPFData.SEQ_WMS_IDO_KBN;
    //            else if (i_Seq_Kbn == HPFData.SEQ_KBN_NOHIN)
    //                retBarCode = retBarCode + strSeq + HPFData.SEQ_WMS_NOHINSHO_KBN;
    //            else if (i_Seq_Kbn == HPFData.SEQ_KBN_UKEIRE)
    //                retBarCode = retBarCode + strSeq + HPFData.SEQ_WMS_UKEIRE_NO;
    //            else
    //                retBarCode = retBarCode + strSeq;
    //            return retBarCode;
    //        }
    //        catch (Exception ex)
    //        {
    //            // エラー処理
    //            HPFLogOut.ErrorOut(ex.Message, "HPFGetBarCode", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' 逆ﾋﾟｯｷﾝｸﾞﾘｽﾄＮｏＢＣＤを取得する
    //    ///         ''' </summary>
    //    ///         ''' <returns></returns>
    //    ///         ''' <remarks></remarks>
    //    public string GetReversePickListNo(string strSeq)
    //    {
    //        try
    //        {
    //            string retBarCode = string.Empty;
    //            if (i_Seq_Kbn != HPFData.SEQ_KBN_PICK)
    //                return retBarCode;
    //            retBarCode = (HPFData.SEQ_PRE_CHAR_R_PICK + strSeq).PadRight(15, '0');
    //            return retBarCode;
    //        }
    //        catch (Exception ex)
    //        {
    //            // エラー処理
    //            HPFLogOut.ErrorOut(ex.Message, "HPFGetBarCode", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' 最大出荷No(出荷カンバン用)ＢＣＤを取得する
    //    ///         ''' </summary>
    //    ///         ''' <remarks></remarks>
    //    private void SearchMaxBarCodeForShipping()
    //    {
    //        try
    //        {
    //            string retShipping = string.Empty;

    //            i_dtBarCode = GetRecord(GetSQL_MST_SEQ(), HPFData.MST_SEQ_TABLE_NAME);

    //            if (i_dtBarCode.Rows.Count > 0)
    //            {
    //                i_HAITA_FLG = i_dtBarCode.Rows(0).Item("HAITA_FLG").ToString;
    //                i_Insert_Flg = false;
    //                i_Count = i_Common.ConvertToDecimal(i_dtBarCode.Rows(0)(HPFData.MST_SEQ_COLUMN_SEQ_NUM_NOW).ToString);
    //            }
    //            else
    //            {
    //                i_Insert_Flg = true;
    //                i_HAITA_FLG = "0";
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            // エラー処理
    //            HPFLogOut.ErrorOut(ex.Message, "HPFGetBarCode", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' 最大バーコードを取得する
    //    ///         ''' </summary>
    //    ///         ''' <remarks></remarks>
    //    private void SearchMaxBarCode()
    //    {
    //        try
    //        {
    //            i_dtBarCode = GetRecord(GetSQL_MST_SEQ(),HPFData.MST_SEQ_TABLE_NAME);

    //            if (i_dtBarCode.Rows.Count > 0)
    //            {
    //                i_HAITA_FLG = i_dtBarCode.Rows(0).Item("HAITA_FLG").ToString;
    //                i_Name = i_dtBarCode.Rows(0).Item("SHAIN_NAME").ToString;
    //                i_Insert_Flg = false;
    //                i_Count = i_Common.ConvertToDecimal(i_dtBarCode.Rows(0)(HPFData.MST_SEQ_COLUMN_SEQ_NUM_NOW).ToString);
    //                i_SEQ_NUM_MAX = i_Common.ConvertToDecimal(i_dtBarCode.Rows(0)(HPFData.MST_SEQ_COLUMN_SEQ_NUM_MAX).ToString);
    //                i_SEQ_LENGH_MAX = i_Common.ConvertToDecimal(i_dtBarCode.Rows(0)(HPFData.MST_SEQ_COLUMN_SEQ_LENGH_MAX).ToString);
    //            }
    //            else
    //            {
    //                i_HAITA_FLG = "0";
    //                switch (i_Seq_Kbn)
    //                {
    //                    case HPFData.SEQ_KBN_PICK:
    //                        {
    //                            i_SEQ_NUM_MAX = 999999999;
    //                            i_SEQ_LENGH_MAX = 999999999;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_SHIWAKE:
    //                    case HPFData.SEQ_KBN_MSHIWAKE:
    //                        {
    //                            i_SEQ_NUM_MAX = 9999999;
    //                            i_SEQ_LENGH_MAX = 9999999;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_FRYO:
    //                    case HPFData.SEQ_KBN_HENPIN:
    //                        {
    //                            i_SEQ_NUM_MAX = 9999999999;
    //                            i_SEQ_LENGH_MAX = 999999;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_ZAIKOTS:
    //                    case HPFData.SEQ_KBN_SHIP_BCD:
    //                    case HPFData.SEQ_KBN_TANAOROSHI:
    //                    case HPFData.SEQ_KBN_UKEBARAI:
    //                    case HPFData.SEQ_KBN_SHIPK:
    //                        {
    //                            i_SEQ_NUM_MAX = 9999999999;
    //                            i_SEQ_LENGH_MAX = 99999999;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_WMS_IDO:
    //                    case HPFData.SEQ_KBN_NOHIN    // ＷＭＳ移動出庫伝票NoＢＣＤ
    //             :
    //                        {
    //                            i_SEQ_NUM_MAX = 999999999;
    //                            i_SEQ_LENGH_MAX = 999999;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_MITUMORI:
    //                    case HPFData.SEQ_KBN_TOIAWASE:
    //                    case HPFData.SEQ_KBN_HINBAN    // 見積№
    //             :
    //                        {
    //                            i_SEQ_NUM_MAX = 99999999;
    //                            i_SEQ_LENGH_MAX = 99999999;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_TENJIKAI                   // 展示会
    //             :
    //                        {
    //                            i_SEQ_NUM_MAX = 99999;
    //                            i_SEQ_LENGH_MAX = 99999;
    //                            break;
    //                        }

    //                    default:
    //                        {
    //                            i_SEQ_NUM_MAX = 9999999999;
    //                            i_SEQ_LENGH_MAX = 9999999999;
    //                            break;
    //                        }
    //                }
    //                i_Insert_Flg = true;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            // エラー処理
    //            HPFLogOut.ErrorOut(ex.Message, "HPFGetBarCode", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' クローズ
    //    ///         ''' </summary>
    //    ///         ''' <remarks></remarks>
    //    public void Close(bool blnUpdFlg = true)
    //    {
    //        try
    //        {
    //            // If i_LockFlg Then
    //            // DoLock_Data(True)
    //            // End If

    //            if (blnUpdFlg)
    //            {
    //                switch (i_Seq_Kbn)
    //                {
    //                    case HPFData.SEQ_KBN_SHIP_BCD:
    //                    case HPFData.SEQ_KBN_NAME:
    //                    case HPFData.SEQ_KBN_TANA_BCD:
    //                    case HPFData.SEQ_KBN_PICK:
    //                    case HPFData.SEQ_KBN_NOHIN:
    //                    case HPFData.SEQ_KBN_FRYO:
    //                    case HPFData.SEQ_KBN_HENPIN:
    //                    case HPFData.SEQ_KBN_WMS_IDO:
    //                    case HPFData.SEQ_KBN_NH_JK_PTN:
    //                    case HPFData.SEQ_KBN_ZAIKOTS:
    //                    case HPFData.SEQ_KBN_TANAOROSHI:
    //                    case HPFData.SEQ_KBN_UKEBARAI:
    //                    case HPFData.SEQ_KBN_SHIPK:
    //                    case HPFData.SEQ_KBN_MITUMORI:
    //                    case HPFData.SEQ_KBN_HINBAN:
    //                    case HPFData.SEQ_KBN_TOIAWASE:
    //                    case HPFData.SEQ_KBN_CARD:
    //                    case HPFData.SEQ_KBN_URIAGE:
    //                    case HPFData.SEQ_KBN_SHIRE:
    //                    case HPFData.SEQ_KBN_SHIP:
    //                    case HPFData.SEQ_KBN_IDO:
    //                    case HPFData.SEQ_KBN_TOKUTEI:
    //                    case HPFData.SEQ_KBN_EDI:
    //                    case HPFData.SEQ_KBN_UKEIRE:
    //                    case HPFData.SEQ_KBN_OSHIRASE:
    //                    case HPFData.SEQ_KBN_URIKAKE:
    //                    case HPFData.SEQ_KBN_KAIKAKE:
    //                    case HPFData.SEQ_KBN_NYUKIN:
    //                    case HPFData.SEQ_KBN_SHIHARAI:
    //                    case HPFData.SEQ_KBN_SHIWAKE:
    //                    case HPFData.SEQ_KBN_SEIKYU:
    //                    case HPFData.SEQ_KBN_SHIHARAIS:
    //                    case HPFData.SEQ_KBN_ARZANDAKA:
    //                    case HPFData.SEQ_KBN_APZANDAKA:
    //                    case HPFData.SEQ_KBN_MSHIWAKE:
    //                    case HPFData.SEQ_KBN_CHOSEINO:
    //                    case HPFData.SEQ_KBN_MDKANRENNO:
    //                    case HPFData.SEQ_KBN_JUCHU:
    //                    case HPFData.SEQ_KBN_TENJIKAI:
    //                    case HPFData.SEQ_KBN_SHOHIZEI_URI:
    //                    case HPFData.SEQ_KBN_SHOHIZEI_KAI:
    //                        {
    //                            if (i_Insert_Flg == false)
    //                                DoUpdateData(GetSQL_Upd());
    //                            else
    //                                DoUpdateData(GetSQL_Inst());
    //                            break;
    //                        }

    //                    default:
    //                        {
    //                            if (Strings.Left(i_Seq_Kbn, 4) == HPFData.SEQ_KBN_SHUK)
    //                            {
    //                                if (i_Insert_Flg == false)
    //                                    DoUpdateData(GetSQL_Upd());
    //                                else
    //                                    DoUpdateData(GetSQL_Inst_Shipping());
    //                            }

    //                            break;
    //                        }
    //                }
    //            }

    //            if (i_LockFlg)
    //                DoLock_Data(true);
    //        }
    //        catch (Exception ex)
    //        {
    //            // エラー処理
    //            HPFLogOut.ErrorOut(ex.Message, "HPFGetBarCode", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //    }



    //    /// <summary>
    //    ///         ''' レコードを取得します
    //    ///         ''' </summary>
    //    ///         ''' <param name="strSQL">SQL文</param>
    //    ///         ''' <param name="strTblName">表の名前</param>
    //    ///         ''' <returns>レコード</returns>
    //    ///         ''' <remarks></remarks>
    //    private DataTable GetRecord(string strSQL, string strTblName = "")
    //    {
    //        DataSet dsRecord = null/* TODO Change to default(_) if this is not a reference type */;
    //        HPFDataBaseSQL objDb = null/* TODO Change to default(_) if this is not a reference type */;

    //        try
    //        {
    //            objDb = new HPFDataBaseSQL();
    //            objDb.DBLogIn();

    //            dsRecord = objDb.GetDataSet(strSQL, strTblName);

    //            return dsRecord.Tables(strTblName);
    //        }
    //        catch (Exception ex)
    //        {
    //            HPFLogOut.ErrorOut(ex.Message, "HPFGetBarCode", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //        finally
    //        {
    //            objDb.DBLogOut();
    //            objDb = null/* TODO Change to default(_) if this is not a reference type */;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' 更新処理処理を行います
    //    ///         ''' </summary>
    //    ///         ''' <returns>
    //    ///         ''' True :正常終了
    //    ///         ''' False:エラーがあります
    //    ///         ''' </returns>
    //    ///         ''' <remarks></remarks>
    //    private bool DoUpdateData(string strSQL)
    //    {
    //        HPFDataBaseSQL objDb = null/* TODO Change to default(_) if this is not a reference type */;
    //        try
    //        {
    //            int intRes = -1;

    //            objDb = new HPFDataBaseSQL();
    //            objDb.DBLogIn();

    //            // 追加処理を行います
    //            intRes = objDb.ExecuteNonQuery(strSQL);

    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            objDb.Rollback();
    //            HPFLogOut.ErrorOut(ex.Message, "HPFCommon.Seq", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //        finally
    //        {
    //            objDb.DBLogOut();
    //        }
    //    }



    //    /// <summary>
    //    ///         ''' 検索用SQLを作成する
    //    ///         ''' </summary>
    //    ///         ''' <returns></returns>
    //    ///         ''' <remarks></remarks>
    //    private string GetSQL_MST_SEQ()
    //    {
    //        try
    //        {
    //            StringBuilder strSQL = new StringBuilder();
    //            strSQL.AppendLine(" SELECT ");
    //            strSQL.AppendLine(" CS.SEQ_KBN ,");
    //            strSQL.AppendLine(" CS.NENDO_RESET_FLAG ,");
    //            strSQL.AppendLine(" CS.NENDO ,");
    //            strSQL.AppendLine(" CS.SEQ_PRE_CHAR ,");
    //            strSQL.AppendLine(" CS.SEQ_NAME ,");
    //            strSQL.AppendLine(" CS.SEQ_NUM_NOW ,");
    //            strSQL.AppendLine(" CS.SEQ_NUM_MAX ,");
    //            strSQL.AppendLine(" CS.SEQ_LENGH_MAX ,");
    //            strSQL.AppendLine(" CS.VERSION_NO, ");
    //            strSQL.AppendLine(" CS.HAITA_FLG ,");
    //            strSQL.AppendLine(" MS.SHAIN_NAME ");
    //            strSQL.AppendLine(" FROM C_SEQ CS ");
    //            strSQL.AppendLine("LEFT JOIN ");
    //            strSQL.AppendLine("     M_SHAIN MS ");
    //            strSQL.AppendLine("ON ");
    //            strSQL.AppendLine("     CS.HAITA_USER = MS.LOGIN_ID ");
    //            strSQL.AppendLine("AND ");
    //            strSQL.AppendLine("     ISNULL(MS.MUKO_FLG,'') = '' ");
    //            strSQL.AppendLine("AND ");
    //            strSQL.AppendLine("     ISNULL(MS.DELETE_FLG,'') = '' ");
    //            strSQL.AppendLine(" WHERE ");
    //            strSQL.AppendFormat(" {0} = '{1}' ", "CS.SEQ_KBN", i_Seq_Kbn);
    //            strSQL.AppendFormat(" AND {0} = '{1}' ", "CS.NENDO_RESET_FLAG", i_Nendo_Reset_Flg);
    //            if (i_Seq_Pre_Char.Length > 0)
    //                strSQL.AppendFormat(" AND {0} = '{1}' ", "CS.SEQ_PRE_CHAR", i_Seq_Pre_Char.Substring(1));
    //            if (i_Nendo.Length > 0)
    //                strSQL.AppendFormat(" AND {0} = '{1}' ", "CS.NENDO", i_Nendo);
    //            strSQL.AppendLine(" ORDER BY CS.SEQ_KBN, CS.NENDO, CS.SEQ_PRE_CHAR ");
    //            return strSQL.ToString();
    //        }
    //        catch (Exception ex)
    //        {
    //            HPFLogOut.ErrorOut(ex.Message, "HPFCommon.Seq", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' シーケンスロックマスタロック/解除用のSQLFactoryを取得します
    //    ///         ''' </summary>
    //    ///         ''' <returns></returns>
    //    ///         ''' <remarks></remarks>
    //    private string GetSql_Lock_C_SEQ(bool blnUnLockFlg = false)
    //    {
    //        try
    //        {
    //            StringBuilder strSQL = new StringBuilder();

    //            strSQL.AppendLine("UPDATE C_SEQ SET ");
    //            if (!blnUnLockFlg)
    //            {
    //                strSQL.AppendLine("      HAITA_FLG = 1 , ");
    //                strSQL.AppendLine("      HAITA_NICHIJI = GETDATE() , ");
    //                strSQL.AppendFormat("    HAITA_USER = '{0}'", i_Common.GetLoginUserInfo((HPFData.LOGINUSERINFO_KEY_ID))).AppendLine();
    //            }
    //            else
    //            {
    //                strSQL.AppendLine("      HAITA_FLG = 0 , ");
    //                strSQL.AppendLine("      HAITA_NICHIJI = NULL , ");
    //                strSQL.AppendLine("      HAITA_USER = NULL").AppendLine();
    //            }
    //            strSQL.AppendLine("WHERE ");
    //            strSQL.AppendFormat(" {0} = '{1}' ", "SEQ_KBN", i_Seq_Kbn);


    //            return strSQL.ToString();
    //        }
    //        catch (Exception ex)
    //        {
    //            HPFLogOut.ErrorOut(ex.Message, "HPFCommon.Seq", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' シーケンスロックマスタロック/解除用のSQLFactoryを取得します
    //    ///         ''' </summary>
    //    ///         ''' <returns></returns>
    //    ///         ''' <remarks></remarks>
    //    private string GetSql_Lock_C_SEQ_New(bool blnUnLockFlg = false)
    //    {
    //        try
    //        {
    //            StringBuilder strSQL = new StringBuilder();

    //            strSQL.AppendLine("UPDATE C_SEQ SET ");
    //            if (!blnUnLockFlg)
    //            {
    //                // ロック処理
    //                strSQL.AppendLine("      HAITA_FLG = 1 , ");
    //                strSQL.AppendLine("      HAITA_NICHIJI = GETDATE() , ");
    //                strSQL.AppendFormat("    HAITA_USER = '{0}'", i_Common.GetLoginUserInfo((HPFData.LOGINUSERINFO_KEY_ID))).AppendLine();
    //            }
    //            else
    //            {
    //                // ロック解除
    //                strSQL.AppendLine("      HAITA_FLG = 0 , ");
    //                strSQL.AppendLine("      HAITA_NICHIJI = NULL , ");
    //                strSQL.AppendLine("      HAITA_USER = NULL").AppendLine();
    //            }
    //            strSQL.AppendLine("WHERE ");
    //            strSQL.AppendFormat(" {0} = '{1}' ", "SEQ_KBN", i_Seq_Kbn);

    //            if (!blnUnLockFlg)
    //                // ロック処理
    //                strSQL.AppendLine(" AND  ISNULL(HAITA_FLG,0) = 0  ");


    //            return strSQL.ToString();
    //        }
    //        catch (Exception ex)
    //        {
    //            HPFLogOut.ErrorOut(ex.Message, "HPFCommon.Seq", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' 更新用SQLを作成する
    //    ///         ''' </summary>
    //    ///         ''' <returns></returns>
    //    ///         ''' <remarks></remarks>
    //    private string GetSQL_Upd()
    //    {
    //        try
    //        {
    //            StringBuilder strSQL = new StringBuilder();
    //            {
    //                var withBlock = i_dtBarCode;
    //                strSQL.Append(" UPDATE C_SEQ SET ");
    //                strSQL.AppendFormat(" SEQ_NUM_NOW = {0} ,", i_Count);
    //                strSQL.AppendFormat(" UPD_SHAIN_CD = '{0}', ", i_Common.GetLoginUserInfo(HPFData.LOGINUSERINFO_KEY_ID));
    //                strSQL.Append(" UPD_DATE = GETDATE(), ");
    //                strSQL.AppendFormat(" UPD_TANMATU_ID = '{0}', ", i_Common.GetLoginUserInfo(HPFData.LOGINUSERINFO_KEY_TANMATSUID));
    //                strSQL.Append(" VERSION_NO = VERSION_NO + 1 ");
    //                strSQL.Append(" WHERE ");
    //                strSQL.AppendFormat(" {0} = '{1}' ", "SEQ_KBN", i_Seq_Kbn);
    //            }

    //            return strSQL.ToString();
    //        }
    //        catch (Exception ex)
    //        {
    //            HPFLogOut.ErrorOut(ex.Message, "HPFCommon.Seq", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' 登録用SQLを作成する
    //    ///         ''' </summary>
    //    ///         ''' <returns></returns>
    //    ///         ''' <remarks></remarks>
    //    private string GetSQL_Inst()
    //    {
    //        try
    //        {
    //            StringBuilder strSQL = new StringBuilder();
    //            string strSeqPraChar = string.Empty;
    //            string strSeqName = string.Empty;
    //            {
    //                var withBlock = i_dtBarCode;
    //                strSQL.Append(" INSERT INTO C_SEQ ( ");
    //                strSQL.Append(" SEQ_KBN ,");
    //                strSQL.Append(" NENDO_RESET_FLAG , ");
    //                strSQL.Append(" NENDO , ");
    //                strSQL.Append(" SEQ_PRE_CHAR , ");
    //                strSQL.Append(" SEQ_NAME , ");
    //                strSQL.Append(" SEQ_NUM_NOW , ");
    //                strSQL.Append(" SEQ_NUM_MAX , ");
    //                strSQL.Append(" SEQ_LENGH_MAX , ");

    //                strSQL.AppendLine(" CRT_SHAIN_CD , ");
    //                strSQL.AppendLine(" CRT_DATE , ");
    //                strSQL.AppendLine(" CRT_TANMATU_ID , ");

    //                strSQL.AppendLine(" UPD_SHAIN_CD , ");
    //                strSQL.AppendLine(" UPD_DATE , ");
    //                strSQL.AppendLine(" UPD_TANMATU_ID, ");
    //                strSQL.AppendLine(" VERSION_NO, ");

    //                strSQL.AppendLine(" HAITA_FLG, ");
    //                strSQL.AppendLine(" HAITA_NICHIJI, ");
    //                strSQL.AppendLine(" HAITA_USER ");

    //                strSQL.Append(" ) VALUES ( ");
    //                strSQL.AppendFormat(" '{0}' ,", i_Seq_Kbn);
    //                strSQL.Append(" '0' ,");
    //                strSQL.Append(" '9999' ,");

    //                switch (i_Seq_Kbn)
    //                {
    //                    case HPFData.SEQ_KBN_SHIP_BCD:
    //                        {
    //                            strSeqPraChar = HPFData.SEQ_PRE_CHAR_SHIP_BCD.Substring(1);
    //                            strSeqName = HPFData.SEQ_NAME_SHIP_BCD;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_NAME:
    //                        {
    //                            strSeqPraChar = HPFData.SEQ_PRE_CHAR_NAME.Substring(1);
    //                            strSeqName = HPFData.SEQ_NAME_NAME;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_TANA_BCD:
    //                        {
    //                            strSeqPraChar = HPFData.SEQ_PRE_CHAR_TANA_BCD.Substring(1);
    //                            strSeqName = HPFData.SEQ_NAME_TANA_BCD;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_PICK:
    //                        {
    //                            strSeqPraChar = HPFData.SEQ_PRE_CHAR_PICK.Substring(1);
    //                            strSeqName = HPFData.SEQ_NAME_PICK;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_NOHIN:
    //                        {
    //                            strSeqPraChar = HPFData.SEQ_PRE_CHAR_NOHIN.Substring(1);
    //                            strSeqName = HPFData.SEQ_NAME_NOHIN;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_FRYO:
    //                        {
    //                            strSeqPraChar = HPFData.SEQ_PRE_CHAR_FURYO.Substring(1);
    //                            strSeqName = HPFData.SEQ_NAME_FRYO;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_HENPIN:
    //                        {
    //                            strSeqPraChar = HPFData.SEQ_PRE_CHAR_HENPIN.Substring(1);
    //                            strSeqName = HPFData.SEQ_NAME_HENPIN;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_WMS_IDO:
    //                        {
    //                            strSeqPraChar = HPFData.SEQ_PRE_CHAR_WMS_IDO.Substring(1);
    //                            strSeqName = HPFData.SEQ_NAME_WMS_IDO;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_ZAIKOTS:
    //                        {
    //                            strSeqPraChar = HPFData.SEQ_PRE_CHAR_ZAIKOTS.Substring(1);
    //                            strSeqName = HPFData.SEQ_NAME_ZAIKOTS;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_NH_JK_PTN:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_NH_JK_PTN;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_TANAOROSHI:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_TANAOROSHI;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_SHIPK:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_SHIPK;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_MITUMORI:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_MITUMORI;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_TOIAWASE:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_TOIAWASE;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_HINBAN:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_HINBAN;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_CARD:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_CARD;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_UKEBARAI:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_UKEBARAI;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_URIAGE:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_URIAGE;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_SHIRE:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_SHIRE;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_SHIP:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_SHIP;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_IDO:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_IDO;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_TOKUTEI:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_TOKUTEI;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_EDI:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_EDI;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_UKEIRE:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_UKEIRE;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_OSHIRASE:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_OSHIRASE;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_URIKAKE:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_URIKAKE;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_KAIKAKE:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_KAIKAKE;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_NYUKIN:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_NYUKIN;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_SHIHARAI:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_SHIHARAI;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_SHIWAKE:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_SHIWAKE;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_SEIKYU:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_SEIKYU;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_SHIHARAIS:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_SHIHARAIS;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_ARZANDAKA:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_ARZANDAKA;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_APZANDAKA:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_APZANDAKA;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_MSHIWAKE:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_MSHIWAKE;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_CHOSEINO:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_CHOSEINO;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_MDKANRENNO:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_MDKANRENNO;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_TENJIKAI:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_TENJIKAI;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_SHOHIZEI_URI:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_SHOHIZEI_U;
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_SHOHIZEI_KAI:
    //                        {
    //                            strSeqPraChar = string.Empty;
    //                            strSeqName = HPFData.SEQ_NAME_SHOHIZEI_K;
    //                            break;
    //                        }
    //                }

    //                strSQL.AppendFormat(" '{0}',  ", strSeqPraChar);
    //                strSQL.AppendFormat(" '{0}',  ", strSeqName);
    //                strSQL.AppendFormat(" {0},  ", i_Count);
    //                switch (i_Seq_Kbn)
    //                {
    //                    case HPFData.SEQ_KBN_PICK:
    //                        {
    //                            strSQL.Append(" 999999999, ");
    //                            strSQL.Append(" 999999999, ");
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_SHIWAKE:
    //                    case HPFData.SEQ_KBN_MSHIWAKE:
    //                        {
    //                            strSQL.Append(" 9999999, ");
    //                            strSQL.Append(" 9999999, ");
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_NOHIN:
    //                    case HPFData.SEQ_KBN_FRYO:
    //                    case HPFData.SEQ_KBN_HENPIN:
    //                        {
    //                            strSQL.Append(" 9999999999, ");
    //                            strSQL.Append(" 999999, ");
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_ZAIKOTS:
    //                    case HPFData.SEQ_KBN_SHIP_BCD:
    //                    case HPFData.SEQ_KBN_TANAOROSHI:
    //                    case HPFData.SEQ_KBN_UKEBARAI:
    //                    case HPFData.SEQ_KBN_SHIPK:
    //                        {
    //                            strSQL.Append(" 9999999999, ");
    //                            strSQL.Append(" 99999999, ");
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_WMS_IDO    // ＷＭＳ移動出庫伝票NoＢＣＤ
    //             :
    //                        {
    //                            strSQL.Append(" 999999999, ");
    //                            strSQL.Append(" 999999, ");
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_MITUMORI:
    //                    case HPFData.SEQ_KBN_HINBAN    // 見積№
    //             :
    //                        {
    //                            strSQL.Append(" 99999999, ");
    //                            strSQL.Append(" 99999999, ");
    //                            break;
    //                        }

    //                    case HPFData.SEQ_KBN_TENJIKAI:
    //                        {
    //                            strSQL.Append(" 99999, ");
    //                            strSQL.Append(" 99999, ");
    //                            break;
    //                        }

    //                    default:
    //                        {
    //                            strSQL.Append(" 9999999999, ");
    //                            strSQL.Append(" 9999999999, ");
    //                            break;
    //                        }
    //                }

    //                strSQL.AppendFormat(" '{0}',  ", i_Common.GetLoginUserInfo(HPFData.LOGINUSERINFO_KEY_ID)).AppendLine();
    //                strSQL.AppendLine(" GETDATE() , ");
    //                strSQL.AppendFormat(" '{0}',  ", i_Common.GetLoginUserInfo(HPFData.LOGINUSERINFO_KEY_TANMATSUID)).AppendLine();

    //                strSQL.AppendLine(" NULL , ");
    //                strSQL.AppendLine(" NULL , ");
    //                strSQL.AppendLine(" NULL , ");
    //                strSQL.AppendLine(" 0 , ");

    //                strSQL.AppendLine(" 0 , ");
    //                strSQL.AppendLine(" NULL , ");
    //                strSQL.AppendLine(" NULL ");
    //                strSQL.AppendLine(" ) ");
    //            }

    //            return strSQL.ToString();
    //        }
    //        catch (Exception ex)
    //        {
    //            HPFLogOut.ErrorOut(ex.Message, "HPFCommon.Seq", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //    }

    //    /// <summary>
    //    ///         ''' 出荷No(出荷カンバン用)ＢＣＤ登録用SQLを作成する
    //    ///         ''' </summary>
    //    ///         ''' <returns></returns>
    //    ///         ''' <remarks></remarks>
    //    private string GetSQL_Inst_Shipping()
    //    {
    //        try
    //        {
    //            StringBuilder strSQL = new StringBuilder();
    //            {
    //                var withBlock = i_dtBarCode;
    //                strSQL.Append(" INSERT INTO C_SEQ ( ");
    //                strSQL.Append(" SEQ_KBN ,");
    //                strSQL.Append(" NENDO_RESET_FLAG , ");
    //                strSQL.Append(" NENDO , ");
    //                strSQL.Append(" SEQ_PRE_CHAR , ");
    //                strSQL.Append(" SEQ_NAME , ");
    //                strSQL.Append(" SEQ_NUM_NOW , ");
    //                strSQL.Append(" SEQ_NUM_MAX , ");
    //                strSQL.Append(" SEQ_LENGH_MAX , ");

    //                strSQL.AppendLine(" CRT_SHAIN_CD , ");
    //                strSQL.AppendLine(" CRT_DATE , ");
    //                strSQL.AppendLine(" CRT_TANMATU_ID , ");

    //                strSQL.AppendLine(" UPD_SHAIN_CD , ");
    //                strSQL.AppendLine(" UPD_DATE , ");
    //                strSQL.AppendLine(" UPD_TANMATU_ID, ");
    //                strSQL.AppendLine(" VERSION_NO, ");

    //                strSQL.AppendLine(" HAITA_FLG, ");
    //                strSQL.AppendLine(" HAITA_NICHIJI, ");
    //                strSQL.AppendLine(" HAITA_USER ");

    //                strSQL.Append(" ) VALUES ( ");
    //                strSQL.AppendFormat(" '{0}' ,", i_Seq_Kbn);
    //                strSQL.Append(" '0' ,");
    //                strSQL.Append(" '9999' ,");
    //                strSQL.AppendFormat(" '{0}' ,", HPFData.SEQ_PRE_CHAR_SHUK.Substring(1));
    //                strSQL.AppendFormat(" '{0}' ,", HPFData.SEQ_NAME_SHUK);
    //                strSQL.AppendFormat(" {0},  ", i_Count);
    //                strSQL.Append(" 999,  ");
    //                strSQL.Append(" 999999,  ");

    //                strSQL.AppendFormat(" '{0}',  ", i_Common.GetLoginUserInfo(HPFData.LOGINUSERINFO_KEY_ID)).AppendLine();
    //                strSQL.AppendLine(" GETDATE() , ");
    //                strSQL.AppendFormat(" '{0}',  ", i_Common.GetLoginUserInfo(HPFData.LOGINUSERINFO_KEY_TANMATSUID)).AppendLine();

    //                strSQL.AppendLine(" NULL , ");
    //                strSQL.AppendLine(" NULL , ");
    //                strSQL.AppendLine(" NULL , ");
    //                strSQL.AppendLine(" 0 , ");

    //                strSQL.AppendLine(" 0 , ");
    //                strSQL.AppendLine(" NULL , ");
    //                strSQL.AppendLine(" NULL ");
    //                strSQL.AppendLine(" ) ");
    //            }

    //            return strSQL.ToString();
    //        }
    //        catch (Exception ex)
    //        {
    //            HPFLogOut.ErrorOut(ex.Message, "HPFCommon.Seq", MethodBase.GetCurrentMethod().Name);
    //            throw ex;
    //        }
    //    }
    //}
}
