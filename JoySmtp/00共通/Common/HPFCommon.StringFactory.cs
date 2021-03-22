using System;
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
using JoySmtp.Data;

namespace JoySmtp.JoyCommon
{
    public partial class Common
    {
        /// <summary>
        ///         ''' 文字列に変換
        ///         ''' </summary>
        ///         ''' <param name="objVal">変換対象</param>
        ///         ''' <returns>変換した文字列</returns>
        ///         ''' <remarks></remarks>
        public static string ConvertToString(object objVal)
        {
            if (Information.IsDBNull(objVal))
                return string.Empty;
            else if (Information.IsNothing(objVal))
                return string.Empty;
            else
                return objVal.ToString();
        }
        /// <summary>
        /// 文字列を大文字に変換
        /// </summary>
        /// <param name="objVal"></param>
        /// <returns></returns>
        public static string ConvertToStringUpper(object objVal)
        {
            if (Information.IsDBNull(objVal))
                return string.Empty;
            else if (Information.IsNothing(objVal))
                return string.Empty;
            else
            {
                //現在のカルチャを取得する
                System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;

                //現在のカルチャを使用して、大文字に変換する
                return objVal.ToString().ToUpper(ci);
            }
        }

        /// <summary>
        ///         ''' Integer型に変換
        ///         ''' </summary>
        ///         ''' <param name="objVal">変換対象</param>
        ///         ''' <param name="intDefault">デフォルト値</param>
        ///         ''' <returns>変換した結果</returns>
        ///         ''' <remarks></remarks>
        public static int ConvertToInteger(object objVal, int intDefault = 0)
        {
            if (Information.IsDBNull(objVal))
                return intDefault;
            else if (Information.IsNothing(objVal))
                return intDefault;
            else if (!Information.IsNumeric(objVal))
                return intDefault;
            else
                return Convert.ToInt32(objVal);
        }

        /// <summary>
        ///         ''' Integer型に変換
        ///         ''' </summary>
        ///         ''' <param name="objVal">変換対象</param>
        ///         ''' <param name="intDefault">デフォルト値</param>
        ///         ''' <returns>変換した結果</returns>
        ///         ''' <remarks></remarks>
        public static long ConvertToLong(object objVal, int intDefault = 0)
        {
            if (Information.IsDBNull(objVal))
                return intDefault;
            else if (Information.IsNothing(objVal))
                return intDefault;
            else if (!Information.IsNumeric(objVal))
                return intDefault;
            else
                return Convert.ToInt64(objVal);
        }

        /// <summary>
        ///         ''' Double型に変換
        ///         ''' </summary>
        ///         ''' <param name="objVal">変換対象</param>
        ///         ''' <param name="dblDefault">デフォルト値</param>
        ///         ''' <returns>変換した結果</returns>
        ///         ''' <remarks></remarks>
        public static double ConvertToDouble(object objVal, double dblDefault = 0.0)
        {
            if (Information.IsDBNull(objVal))
                return dblDefault;
            else if (Information.IsNothing(objVal))
                return dblDefault;
            else if (!Information.IsNumeric(objVal))
                return dblDefault;
            else
                return Convert.ToDouble(objVal);
        }

        /// <summary>
        ///         ''' Decimalに変換
        ///         ''' </summary>
        ///         ''' <param name="objVal">変換対象</param>
        ///         ''' <param name="decDefault">デフォルト値</param>
        ///         ''' <returns>変換した結果</returns>
        ///         ''' <remarks></remarks>
        public static decimal ConvertToDecimal(object objVal, decimal decDefault = 0)
        {
            if (Information.IsDBNull(objVal))
                return decDefault;
            else if (Information.IsNothing(objVal))
                return decDefault;
            else if (!Information.IsNumeric(objVal))
                return decDefault;
            else
                return Convert.ToDecimal(objVal);
        }

        /// <summary>
        ///         ''' 日付の文字列型に変換
        ///         ''' </summary>
        ///         ''' <param name="objVal">変換対象</param>
        ///         ''' <returns>変換した結果</returns>
        ///         ''' <remarks></remarks>
        public static string ConvertToDateString(object objVal)
        {
            if (Information.IsDBNull(objVal))
                return string.Empty;
            else if (Information.IsNothing(objVal))
                return string.Empty;
            else if (!Information.IsDate(objVal))
                return string.Empty;
            else
                return Convert.ToDateTime(objVal).ToString("yyyy/MM/dd");
        }

        public static string ConvertToDateTimeString(object objVal)
        {
            if (Information.IsDBNull(objVal))
                return string.Empty;
            else if (Information.IsNothing(objVal))
                return string.Empty;
            else if (!Information.IsDate(objVal))
                return string.Empty;
            else
                return Convert.ToDateTime(objVal).ToString("yyyy/MM/dd HH:mm:ss");
        }
        public static DateTime? ConvertToDateTime(object objVal)
        {
            if (Information.IsDBNull(objVal))
                return null;
            else if (Information.IsNothing(objVal))
                return null;
            else if (!Information.IsDate(objVal))
                return null;
            else
                return Convert.ToDateTime(objVal);
        }
        /// <summary>
        ///         ''' 日付の比較
        ///         ''' </summary>
        ///         ''' <param name="strDateS">日付1の文字列</param>
        ///         ''' <param name="strDateE">日付2の文字列</param>
        ///         ''' <returns>
        ///         ''' True : 日付1 > 日付2 
        ///         ''' False: 他
        ///         ''' </returns>
        ///         ''' <remarks></remarks>
        public static bool CompareDate(string strDateS, string strDateE)
        {
            DateTime dtStart = DateTime.Parse(strDateS);    // 日付1の文字列を日付に変化
            DateTime dtEnd = DateTime.Parse(strDateE);      // 日付1の文字列を日付に変化

            // 日付の比較を行います
            return DateTime.Compare(dtStart, dtEnd) > 0;
        }

        /// <summary>
        ///         ''' 日付に日数をプラスします
        ///         ''' </summary>
        ///         ''' <param name="strDate">日付</param>
        ///         ''' <param name="dblDays">日数</param>
        ///         ''' <returns>プラス後の日付の文字列</returns>
        ///         ''' <remarks></remarks>
        public static string AddToDate(string strDate, double dblDays)
        {
            DateTime dtDate = DateTime.Parse(strDate);

            dtDate = dtDate.AddDays(dblDays);

            return ConvertToDateString(dtDate);
        }

        /// <summary>
        ///         ''' 曖昧検索の文字列を変更します
        ///         ''' </summary>
        ///         ''' <param name="strValue">値</param>
        ///         ''' <returns>
        ///         ''' *を%に変更します
        ///         ''' </returns>
        ///         ''' <remarks></remarks>
        public static string ConvertToSearchString(string strValue)
        {
            // Dim strRetValue As String = strValue.Replace("'", "''").Trim()
            string strRetValue = strValue.Trim();
            if (strRetValue.StartsWith("*"))
                strRetValue = string.Format("%{0}", strRetValue.Substring(1));

            if (strRetValue.EndsWith("*"))
                strRetValue = string.Format("{0}%", strRetValue.Substring(0, strRetValue.Length - 1));

            return strRetValue;
        }

        /// <summary>
        ///         ''' MESSAGEの文字列を変更します
        ///         ''' </summary>
        ///         ''' <param name="strValue">値</param>
        ///         ''' <param name="strTemp">タイプ</param>
        ///         ''' <returns>変化後の文字列</returns>
        ///         ''' <remarks></remarks>
        public static string ConvertToMsgString(string strValue, bool AddFlg = false, string strTemp = "*")
        {
            string strReturn = string.Empty;
            if (!AddFlg)
                strReturn = strValue.Replace(strTemp, "");
            else
                // 2014/9/21 suzuki st バグが出るのでコメントアウト
                // '' 最後の文字が「*」ではない場合、「*」を付ける
                // 'If strValue.LastIndexOf(strTemp) <> strValue.Length - 1 Then
                strReturn = strValue + strTemp;
            return strReturn;
        }

        /// <summary>
        ///         ''' ゼロ埋め処理をします
        ///         ''' </summary>
        ///         ''' <param name="strValue"></param>
        ///         ''' <param name="intWidth"></param>
        ///         ''' <remarks></remarks>
        public static string ConvertFillZero(string strValue, int intWidth)
        {
            string strRet = string.Empty;
            if (strValue.Trim().Length > 0 && Information.IsNumeric(strValue))
                strRet = strValue.Trim().PadLeft(intWidth, HPFData.FILL_STRING);
            else
                strRet = strValue;
            return strRet;
        }

        /// <summary>
        ///         ''' yyyyMMddHHmmssの形式の文字列を日時に変化
        ///         ''' </summary>
        ///         ''' <param name="strDateTime">日時</param>
        ///         ''' <param name="objType">変化タイプ</param>
        ///         ''' <returns>変化後の文字列</returns>
        ///         ''' <remarks></remarks>
        public static string GetStrDateTime(string strDateTime, DateType objType = DateType.SeirekiHukoLong)
        {
            string strValue = string.Empty;
            string strDate = string.Empty;
            string strTime = string.Empty;

            if (strDate.Length != 14)
                return string.Empty;

            strDate = GetStrDate(strDateTime.Substring(0, 8), objType);
            strTime = GetStrTime(strDateTime.Substring(8), objType);

            // 日付と時刻を設定
            if (strDate.Length == 0)
            {
                // 日付がブランクであれば、
                if (strTime.Length > 0)
                    // 時刻があれば、時刻を戻します
                    strValue = strTime;
            }
            else
                // 日付があれば、
                if (strTime.Length == 0)
                    // 時刻がブランクであれば、日付を戻します
                    strValue = strDate;
                else
                    // 時刻があれば
                    if (objType == DateType.SeirekiHukoLong | objType == DateType.SeirekiHukoShort)
                        // 符号の時、「 」(半角)を使用し日付と時刻を連携します
                        strValue = string.Format("{0} {1}", strDate, strTime);
                    else
                        // 漢字の時、「　」(全角)を使用し日付と時刻を連携します
                        strValue = string.Format("{0}　{1}", strDate, strTime);

            return strValue;
        }

        /// <summary>
        ///         ''' yyyyMMddの形式の文字列を日付に変化
        ///         ''' </summary>
        ///         ''' <param name="strDate">日付</param>
        ///         ''' <param name="objType">変化タイプ</param>
        ///         ''' <returns>変化後の文字列</returns>
        ///         ''' <remarks></remarks>
        public static string GetStrDate(string strDate, DateType objType = DateType.SeirekiHukoLong)
        {
            string strValue = string.Empty;
            int intYear = 0;
            int intMonth = 0;
            int intDay = 0;

            // 日付の長度が不正確であれば、ブランクを戻します
            if (strDate.Length != 8 & strDate.Length != 14)
                return string.Empty;

            intYear = ConvertToInteger(strDate.Substring(0, 4), -1);
            intMonth = ConvertToInteger(strDate.Substring(4, 2), -1);
            intDay = ConvertToInteger(strDate.Substring(6, 2), -1);

            // 年月日の形式が不正確であれば、クランクを戻します
            if (intYear == -1 | intMonth == -1 | intDay == -1)
                return string.Empty;

            //switch (objType)
            //{
            //    case DateType.SeirekiHukoLong:
            //        {
            //            strValue = string.Format("{0}/{1}/{2}", intYear.ToString("0000"), intMonth.ToString("00"), intDay.ToString("00"));
            //            break;
            //        }

            //    case DateType.SeirekiHukoShort:
            //        {
            //            strValue = string.Format("{0}/{1}/{2}", intYear.ToString(), intMonth.ToString(), intDay.ToString());
            //            break;
            //        }

            //    case DateType.SeirekiKanji_Long:
            //        {
            //            strValue = string.Format("{0}年{1}月{2}日", intYear.ToString("0000"), intMonth.ToString("00"), intDay.ToString("00"));
            //            break;
            //        }

            //    case DateType.SeirekiKanji_Short:
            //        {
            //            strValue = string.Format("{0}年{1}月{2}日", intYear.ToString(), intMonth.ToString(), intDay.ToString());
            //            break;
            //        }
            //}
            return strValue;
        }

        /// <summary>
        ///         ''' HHmmssの形式の文字列を時刻に変化
        ///         ''' </summary>
        ///         ''' <param name="strTime">時刻</param>
        ///         ''' <param name="objType">変化タイプ</param>
        ///         ''' <returns>変化後の文字列</returns>
        ///         ''' <remarks></remarks>
        public static string GetStrTime(string strTime, DateType objtype = DateType.SeirekiHukoLong)
        {
            string strValue = string.Empty;
            int intHour = -1;
            int intMinute = -1;
            int intSecond = -1;
            if (strValue.Length != 6)
                return string.Empty;

            intHour = ConvertToInteger(strTime.Substring(0, 2), -1);
            intMinute = ConvertToInteger(strTime.Substring(2, 2), -1);
            intSecond = ConvertToInteger(strTime.Substring(4, 2), -1);

            // 時刻の形式が不正確であれば、ブランクを戻します
            if ((intHour < 0 & intHour > 23) | (intMinute < 0 & intMinute > 59) | (intSecond < 0 & intSecond > 59))
                return string.Empty;

            switch (objtype)
            {
                case DateType.SeirekiHukoLong:
                    {
                        strValue = string.Format("{0}:{1}:{2}", intHour.ToString("00"), intMinute.ToString("00"), intSecond.ToString("00"));
                        break;
                    }

                case DateType.SeirekiHukoShort:
                    {
                        strValue = string.Format("{0}:{1}:{2}", intHour.ToString(), intMinute.ToString(), intSecond.ToString());
                        break;
                    }

                case DateType.SeirekiKanji_Long:
                    {
                        strValue = string.Format("{0}時{1}分{2}秒", intHour.ToString("00"), intMinute.ToString("00"), intSecond.ToString("00"));
                        break;
                    }

                case DateType.SeirekiKanji_Short:
                    {
                        strValue = string.Format("{0}時{1}分{2}秒", intHour.ToString(), intMinute.ToString(), intSecond.ToString());
                        break;
                    }
            }

            return strValue;
        }

        /// <summary>
        ///         ''' DateのyyyyMMddHHmmssの形式の文字列に変化
        ///         ''' </summary>
        ///         ''' <param name="dtDate">日付</param>
        ///         ''' <param name="blnHasTime">タイプ</param>
        ///         ''' <returns>変化後の文字列</returns>
        ///         ''' <remarks></remarks>
        public static string GetDateTimeToString(DateTime dtDate, bool blnHasTime = true)
        {
            string strReturn = string.Empty;
            strReturn = string.Format("{0}{1}{2}", dtDate.Year.ToString("0000"), dtDate.Month.ToString("00"), dtDate.Day.ToString("00"));

            if (blnHasTime == true)
                strReturn = string.Format("{0}{1}{2}{3}", strReturn, dtDate.Hour.ToString("00"), dtDate.Minute.ToString("00"), dtDate.Second.ToString("00"));
            return strReturn;
        }
        /// <summary>
        ///         ''' 
        ///         ''' </summary>
        ///         ''' <param name="strTemp"></param>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        public static string ConvertToHanKakuUpper(string strTemp)
        {
            string strReturn = string.Empty;
            strReturn = Strings.StrConv(strTemp, VbStrConv.Narrow, 0);
            return strReturn.ToUpper();
        }
        /// <summary>
        ///         ''' 桁数を取得する
        ///         ''' </summary>
        ///         ''' <param name="strValue"></param>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        public static int GetLength(string strValue)
        {
            byte[] myByte;
            myByte = System.Text.Encoding.Default.GetBytes(strValue);
            return myByte.Length;
        }
        /// <summary>
        ///         ''' 整数桁数を取得する
        ///         ''' </summary>
        ///         ''' <param name="decValue"></param>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        public static int GetSeiLength(decimal decValue)
        {
            int i = decValue.ToString().IndexOf(".");
            if (i > 0)
                return decValue.ToString().Substring(0, decValue.ToString().IndexOf(".")).Length;
            else
                return decValue.ToString().Length;
        }

        /// <summary>
        ///         ''' 小数桁数を取得する
        ///         ''' </summary>
        ///         ''' <param name="decValue"></param>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        public static int GetSyoLength(decimal decValue)
        {
            int i = decValue.ToString().IndexOf(".");
            if (i > 0)
                return decValue.ToString().Substring(decValue.ToString().IndexOf(".") + 1).Length;
            else
                return 0;
        }

        /// <summary>
        ///         '''  
        ///         ''' </summary>
        ///         ''' <param name="decValue"></param>
        ///         ''' <param name="intNum">１：切り取る、２：切り上げる、３：四捨五入する</param>
        ///         ''' <returns></returns>
        ///         ''' <remarks></remarks>
        public static decimal GetRound(decimal decValue, int intNum)
        {
            return Math.Round(decValue, intNum);
        }

        /// <summary>
        ///         ''' 端数処理を行います。
        ///         ''' </summary>
        ///         ''' <param name="suu">処理する数値</param>
        ///         ''' <param name="hk">その他端数処理区分</param>
        ///         ''' <param name="i">整数部の桁数</param>
        ///         ''' <param name="f">指数部の桁数</param>
        ///         ''' <returns>処理後の数値</returns>
        public static decimal ProcHasuu(decimal suu, HasuuEtcKbn hk, int i, int f)
        {
            string w = "1.0e+" + f.ToString("000");
            double e = double.Parse(w);
            string fmt = "";
            string ret = "";
            for (int cnt = 0; cnt <= i - 1; cnt++)
                fmt += "#";
            if (f > 0)
            {
                fmt += ".";
                for (int cnt = 0; cnt <= f - 1; cnt++)
                    fmt += "#";
            }
            decimal d = suu;
            d *= System.Convert.ToDecimal(e);
            if (hk == HasuuEtcKbn.FourFive)
            {
                if (d > 0)
                    d = System.Convert.ToDecimal(System.Convert.ToInt64(Math.Truncate(d + 0.5M))) / System.Convert.ToDecimal(e);
                else
                    d = System.Convert.ToDecimal(System.Convert.ToInt64(Math.Truncate(d - 0.5M))) / System.Convert.ToDecimal(e);
                ret = d.ToString(fmt);
            }
            else if (hk == HasuuEtcKbn.Ommit)
            {
                d = System.Convert.ToDecimal(System.Convert.ToInt64(Math.Truncate(d))) / System.Convert.ToDecimal(e);
                ret = d.ToString(fmt);
            }
            else if (hk == HasuuEtcKbn.Raise)
            {
                if (d > 0)
                    d = System.Convert.ToDecimal(System.Convert.ToInt64(Math.Truncate(d + 0.9M))) / System.Convert.ToDecimal(e);
                else
                    d = System.Convert.ToDecimal(System.Convert.ToInt64(Math.Truncate(d - 0.9M))) / System.Convert.ToDecimal(e);
                ret = d.ToString(fmt);
            }
            else if (hk == HasuuEtcKbn.Devalued)
            {
                if (d > 0)
                    d = System.Convert.ToDecimal(System.Convert.ToInt64(Math.Truncate(d))) / System.Convert.ToDecimal(e);
                else
                    d = System.Convert.ToDecimal(System.Convert.ToInt64(Math.Truncate(d - 0.9M))) / System.Convert.ToDecimal(e);
                ret = d.ToString(fmt);
            }
            else
                ret = suu.ToString(fmt);
            if (ret == "")
                ret = "0";
            return ConvertToDecimal(ret);
        }

        ///// <summary>
        /////         ''' JANコードのチェックデジットを計算する関数	
        /////         ''' </summary>
        /////         ''' <param name="strCheck_Digit_Value">チェックしたい12文字</param>
        /////         ''' <returns>チェックデジット１桁</returns>
        //public string Calc_Check_Digit(string strCheck_Digit_Value)
        //{
        //    try
        //    {
        //        int[] intValue = new int[13];      // 値を1桁ずつ格納する配列																																																																																																																																																																																																																															
        //        int i;
        //        int intEven;      // 偶数																																																																																																																																																																																																																															
        //        int intOdds;      // 奇数																																																																																																																																																																																																																															
        //        string strValue_13;       // 13桁のJANコード																																																																																																																																																																																																																															

        //        // 12桁及び7桁の場合、左を0詰めして12桁にした上で最後に0を付加。																																																																																																																																																																																																																															
        //        if (Strings.Len(strCheck_Digit_Value) == 12 | Strings.Len(strCheck_Digit_Value) == 7)
        //            strValue_13 = ConvertFillZero(strCheck_Digit_Value, 12) + "0";
        //        else if (Strings.Len(strCheck_Digit_Value) == 8)
        //            strValue_13 = ConvertFillZero(strCheck_Digit_Value, 13);
        //        else
        //            strValue_13 = strCheck_Digit_Value;

        //        intEven = 0;
        //        intOdds = 0;

        //        // 右から2番目から13番目の値について、偶数位置、奇数位置の値をそれぞれ足していく。																																																																																																																																																																																																																															
        //        for (i = 2; i <= Strings.Len(strValue_13); i++)
        //        {
        //            if ((i % 2) != 0)
        //                intOdds = intOdds + System.Convert.ToInt32(Strings.Mid(strValue_13, 14 - i, 1));
        //            else
        //                intEven = intEven + System.Convert.ToInt32(Strings.Mid(strValue_13, 14 - i, 1));
        //        }

        //        // 偶数位置の合計を3倍する																																																																																																																																																																																																																															
        //        intEven = intEven * 3;

        //        // 偶数位置の合計の3倍の値と奇数位置の合計の値を足し、																																																																																																																																																																																																																															
        //        // 1の位の値が0の場合は0を返す。その他の場合は10から1の位の値を引いた値を返す。																																																																																																																																																																																																																															

        //        if (System.Convert.ToInt32(Strings.Right(intEven + intOdds, 1)) == 0)
        //            return "0";
        //        else
        //            return System.Convert.ToString(10 - System.Convert.ToInt32(Strings.Right(intEven + intOdds, 1)));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}




        /// -----------------------------------------------------------------------------------------
        ///         ''' <summary>
        ///         '''     文字列の左端から指定したバイト数分の文字列を返します。</summary>
        ///         ''' <param name="stTarget">
        ///         '''     取り出す元になる文字列。</param>
        ///         ''' <param name="iByteSize">
        ///         '''     取り出すバイト数。</param>
        ///         ''' <returns>
        ///         '''     左端から指定されたバイト数分の文字列。</returns>
        ///         ''' -----------------------------------------------------------------------------------------
        public static string LeftB(string stTarget, int iByteSize)
        {
            string strValue = MidB(stTarget, 1, iByteSize);
            byte[] myByte;
            myByte = System.Text.Encoding.Default.GetBytes(strValue);

            if (iByteSize < myByte.Length)
                strValue = MidB(stTarget, 1, iByteSize - 1);
            return strValue;
        }




        /// -----------------------------------------------------------------------------------------
        ///         ''' <summary>
        ///         '''     文字列の指定されたバイト位置以降のすべての文字列を返します。</summary>
        ///         ''' <param name="stTarget">
        ///         '''     取り出す元になる文字列。</param>
        ///         ''' <param name="iStart">
        ///         '''     取り出しを開始する位置。</param>
        ///         ''' <returns>
        ///         '''     指定されたバイト位置以降のすべての文字列。</returns>
        ///         ''' -----------------------------------------------------------------------------------------
        public static string MidB(string stTarget, int iStart)
        {
            System.Text.Encoding hEncoding = System.Text.Encoding.GetEncoding("Shift_JIS");
            byte[] btBytes = hEncoding.GetBytes(stTarget);

            return hEncoding.GetString(btBytes, iStart - 1, btBytes.Length - iStart + 1);
        }


        /// -----------------------------------------------------------------------------------------
        ///         ''' <summary>
        ///         '''     文字列の指定されたバイト位置から、指定されたバイト数分の文字列を返します。</summary>
        ///         ''' <param name="stTarget">
        ///         '''     取り出す元になる文字列。</param>
        ///         ''' <param name="iStart">
        ///         '''     取り出しを開始する位置。</param>
        ///         ''' <param name="iByteSize">
        ///         '''     取り出すバイト数。</param>
        ///         ''' <returns>
        ///         '''     指定されたバイト位置から指定されたバイト数分の文字列。</returns>
        ///         ''' -----------------------------------------------------------------------------------------
        public static string MidB(string stTarget, int iStart, int iByteSize)
        {
            System.Text.Encoding hEncoding = System.Text.Encoding.GetEncoding("Shift_JIS");
            byte[] btBytes = hEncoding.GetBytes(stTarget);

            return hEncoding.GetString(btBytes, iStart - 1, iByteSize);
        }




        /// -----------------------------------------------------------------------------------------
        ///         ''' <summary>
        ///         '''     文字列の右端から指定されたバイト数分の文字列を返します。</summary>
        ///         ''' <param name="stTarget">
        ///         '''     取り出す元になる文字列。</param>
        ///         ''' <param name="iByteSize">
        ///         '''     取り出すバイト数。</param>
        ///         ''' <returns>
        ///         '''     右端から指定されたバイト数分の文字列。</returns>
        ///         ''' -----------------------------------------------------------------------------------------
        public static string RightB(string stTarget, int iByteSize)
        {
            System.Text.Encoding hEncoding = System.Text.Encoding.GetEncoding("Shift_JIS");
            byte[] btBytes = hEncoding.GetBytes(stTarget);

            return hEncoding.GetString(btBytes, btBytes.Length - iByteSize, iByteSize);
        }
    }
}
